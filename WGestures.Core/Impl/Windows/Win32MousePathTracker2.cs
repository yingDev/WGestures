using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;
using WindowsInput;
using WGestures.Common.OsSpecific.Windows;
using Win32;
using Timer = System.Timers.Timer;

namespace WGestures.Core.Impl.Windows
{
    public class Win32MousePathTracker2 : IPathTracker
    {
        private const int MOUSE_EVENT_EXTRA_SIMULATED = 19900620;

        #region properties
        /// <summary>
        /// 获取和设置哪个鼠标键引发手势
        /// </summary>
        [Obsolete("不在使用单个按键作为手势键")]
        public GestureButtons GestureButton { get; set; }

        /// <summary>
        ///获取和设置鼠标键按下后至少移动多少距离才开始跟踪手势，单位为像素 
        /// </summary>
        public int InitialValidMove { get; set; }

        /// <summary>
        /// 获取和设置是否允许起始移动超时（如果启用， 则手势键按下后若超过InitalStayTimeoutMillis时间没有有效移动，将执行正常拖拽操作）
        /// </summary>
        public bool InitialStayTimeout { get; set; }

        /// <summary>
        /// 获取和设置起始移动超时的时间值
        /// </summary>
        public int InitialStayTimeoutMillis { get; set; }

        /// <summary>
        /// 获取和设置噪音过滤的阈值，即至少移动了多少像素才被认为移动了
        /// </summary>
        public int EffectiveMove { get; set; }

        /// <summary>
        /// 获取和设置是否允许停留超时
        /// </summary>
        public bool StayTimeout
        {
            get { return _stayTimeout; }
            set
            {
                _stayTimeout = value;
                if (value && _stayTimer == null)
                {
                    _stayTimer = new Timer(_stayTimeoutMillis);
                    _stayTimer.Elapsed += StayTimeoutProc;
                }
                else if (!value && _stayTimer != null)
                {
                    _stayTimer.Dispose();
                    _stayTimer = null;
                }
            }
        }

        /// <summary>
        /// 获取和设置停留超时的毫秒数
        /// </summary>
        public int StayTimeoutMillis
        {
            get { return _stayTimeoutMillis; }
            set
            {
                _stayTimeoutMillis = value;
                if (_stayTimer != null) _stayTimer.Interval = value;
            }
        }

        /// <summary>
        /// 获取和设置超时后是否执行正常点击
        /// </summary>
        [Obsolete]
        public bool PerformNormalWhenTimeout { get; set; }


        public bool IsDisposed { get; private set; }

        #endregion


        #region fields
        private readonly MouseHook _mouseHook;
        //表明是否是“performNormal”的情况下自己模拟的鼠标事件。
        private GestureModifier _filteredModifiers;

        private Point _startPoint;
        private Point _lastPoint;
        private Point _lastEffectivePos;
        private Point _curPos;
        private Queue<MSG> _msgQueue = new Queue<MSG>(8);
        private int _moveCount;
        private readonly GestureContext _currentContext = new Win32GestureContext();
        private readonly PathEventArgs _currentEventArgs = new PathEventArgs();

        //for timers
        private Timer _stayTimer;
        private volatile bool _isTimeout;
        private bool _isInitialTimeout;

        private bool _stayTimeout;
        private int _stayTimeoutMillis = 500;


        private bool _initialMoveValid;

        private bool _isPaused;
        private bool _isStopped;

        //避免滚轮事件发布过于频繁
        //用于记录鼠标滚轮事件上一次发生的时间，如果时间间隔小于一定值，则不发布新的事件。
        private DateTime _modifierEventHappendPrevTime;

        #endregion
        public Win32MousePathTracker2()
        {
            
            //properties defaults
            //GestureButton = GestureButtons.RightButton;
            InitialValidMove = 10;
            InitialStayTimeout = true;
            InitialStayTimeoutMillis = 150;

            EffectiveMove = (int)(20 * Native.GetScreenDpi() / 96.0f) * 2;//todo: 增加灵敏度调整
            StayTimeout = false;
            PerformNormalWhenTimeout = false;

            _mouseHook = new MouseHook();
            _mouseHook.MouseHookEvent += HookProc;

        }

        public event Action<bool> RequestPauseResume;

        #region IPathTracker Members
        public event BeforePathStartEventHandler BeforePathStart;
        public event PathTrackEventHandler PathStart;
        public event PathTrackEventHandler PathGrow;
        public event PathTrackEventHandler EffectivePathGrow;
        public event PathTrackEventHandler PathEnd;
        public event PathTrackEventHandler PathTimeout;
        public event PathTrackEventHandler PathModifier;


        public void Start()
        {
            _mouseHook.Install();

            while (true)
            {            
                MSG msg;
                lock(_msgQueue)
                {
                    if(_msgQueue.Count == 0) Monitor.Wait(_msgQueue);
                    msg = _msgQueue.Dequeue();
                    UpdateContextAndEventArgs();
                }

                switch (msg.message)
                {
                    case WM.GESTBTN_DOWN:
                        OnMouseDown();
                        break;

                    case WM.GESTBTN_MOVE:
                        OnMouseMove();
                        break;

                    case WM.GESTBTN_MODIFIER:
                        OnModifier((GestureModifier)msg.param);
                        break;

                    case WM.GESTBTN_UP:
                        OnMouseUp();
                        break;

                    case WM.STAY_TIMEOUT:
                        OnTimeout();
                        break;

                    case WM.PAUSE_RESUME:
                        var pause = (msg.param == 1);
                        OnPauseResume(pause);
                        break;

                    case WM.STOP:
                        OnStop();
                        return;
                }

            }

        }

        /// <summary>
        /// 停止鼠标钩子并退出runloop
        /// </summary>
        public void Stop()
        {
            if (_isStopped) return;
            Post(WM.STOP);
        }

        /// <summary>
        ///停止鼠标钩子，但不退出runloop 
        /// </summary>
        public bool Paused
        {
            get { return _isPaused; }

            set
            {
                if (_isStopped) throw new InvalidOperationException("已处于停止状态");
                _isPaused = value;
                Post(WM.PAUSE_RESUME, value ? 1 : 0);
            }
        }

        /// <summary>
        /// 暂停发布Move事件，并设置直到路径结束之前不应该被hook的修饰符
        /// </summary>
        /// <param name="filteredModifiers"></param>
        public void SuspendTemprarily(GestureModifier filteredModifiers)
        {
            if (_stayTimeout) _stayTimer.Stop();

            _filteredModifiers = filteredModifiers;
            IsSuspended = true;
        }

        public bool IsSuspended { get; private set; }

        #endregion


        #region internal
        //note: 由于360等可能导致dwExtraInfo丢失，因此使用这个变量作为备份方案
        private bool _simulatingMouse;

        private bool _captured;
        private GestureButtons _gestureBtn;
        private void HookProc(MouseHook.MouseHookEventArgs e)
        {
            //处理 左键 + 中键 用于 暂停继续的情形
            if (e.Msg == MouseMsg.WM_MBUTTONDOWN)
            {
                var mouseSwapped = Native.GetSystemMetrics(Native.SystemMetric.SM_SWAPBUTTON) != 0;
                if (Native.GetAsyncKeyState(mouseSwapped ? Keys.RButton : Keys.LButton) < 0)
                {
                    e.Handled = true;
                    if (RequestPauseResume != null) RequestPauseResume(_isPaused);
                    return;
                }
            }

            if (_isPaused) return;

            var mouseData = (Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(e.lParam, typeof(Native.MSLLHOOKSTRUCT));
            if (_simulatingMouse || mouseData.dwExtraInfo.ToInt32() == MOUSE_EVENT_EXTRA_SIMULATED)
            {
                Debug.WriteLine("Got Simulated Event!");
                if (InitialStayTimeout && _isInitialTimeout)
                {
                    Debug.WriteLine("_captured=false");
                    _captured = false;
                }
                return;
            }

            _curPos = new Point(e.X, e.Y);

            var m = e.Msg;
            switch (m)
            {
                //必须在这里立即决定是否应该捕获
                case MouseMsg.WM_RBUTTONDOWN:
                case MouseMsg.WM_MBUTTONDOWN:
                    if (!_captured)
                    {                        
                        try
                        {
                            //notice: 这个方法在钩子线程中运行，因此必须足够快，而且不能失败
                            _captured = OnBeforePathStart();
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            throw;
#endif
                            //如果出错，则不捕获手势
                            _captured = false;
                        }


                        if (_captured)
                        {
                            _gestureBtn = (m == MouseMsg.WM_RBUTTONDOWN ? GestureButtons.RightButton : GestureButtons.MiddleButton);

                            _modifierEventHappendPrevTime = new DateTime(0);
                            e.Handled = true;

                            Post(WM.GESTBTN_DOWN);
                        }
                    }
                    else //另一个键作为手势键的时候，作为修饰键
                    {
                        var gestMod = m == MouseMsg.WM_RBUTTONDOWN ? GestureModifier.RightButtonDown : GestureModifier.MiddleButtonDown;
                        e.Handled = HandleModifier(gestMod);
                    }
                    break;

                case MouseMsg.WM_MOUSEMOVE:
                    if (_captured)
                    {
                        //永远不拦截move消息，所以不设置e.Handled = true
                        Post(WM.GESTBTN_MOVE);
                    }
                    break;

                case MouseMsg.WM_MOUSEWHEEL:
                    if (_captured)
                    {
                        //获得滚动方向
                        int delta = (short)(mouseData.mouseData >> 16);
                        var gestMod = delta > 0 ? GestureModifier.WheelForward : GestureModifier.WheelBackward;

                        e.Handled = HandleModifier(gestMod);
                    }
                    else if (DateTime.UtcNow - _modifierEventHappendPrevTime < TimeSpan.FromMilliseconds(300))//延迟一下，因为 中键手势 + 滚动，可能导致快捷键还没结束，而滚轮事件发送到了目标窗口，可鞥解释成其他功能（比如ctrl + 滚轮 = 缩放）
                    {
                        e.Handled = true;
                    }
                    break;

                case MouseMsg.WM_LBUTTONDOWN:
                    if (_captured)
                    {
                        e.Handled = HandleModifier(GestureModifier.LeftButtonDown);
                    }
                    break;

                case MouseMsg.WM_RBUTTONUP:
                case MouseMsg.WM_MBUTTONUP:
                    if (_captured)
                    {
                        //是手势键up
                        if (m == (_gestureBtn == GestureButtons.RightButton ? MouseMsg.WM_RBUTTONUP : MouseMsg.WM_MBUTTONUP))
                        {
                            _captured = false;

                            //起始没有移动足够距离
                            if (!_initialMoveValid)
                            {
                                SimulateGestureBtnEvent(GestureBtnEventType.DOWN, _curPos.X, _curPos.Y);
                            }
                            else
                            {
                                e.Handled = true;
                                Post(WM.GESTBTN_UP);
                            }
                        }
                    }
                    
                    break;

                default:
                    //其他消息不处理
                    //e.Handled = false;
                    break;
            }


        }

        private void SimulateGestureBtnEvent(GestureBtnEventType eventType, int x, int y)
        {
            Debug.WriteLine("SimulateMouseEvent: " + _gestureBtn + " " + eventType);
            _simulatingMouse = true;

            var mouseSwapped = Native.GetSystemMetrics(Native.SystemMetric.SM_SWAPBUTTON) != 0;

            Native.POINT curPos;
            Native.GetCursorPos(out curPos);

            User32.MOUSEEVENTF events = 0;

            switch (_gestureBtn)
            {
                case GestureButtons.RightButton:
                    if (mouseSwapped)
                    {
                        switch (eventType)
                        {
                            case GestureBtnEventType.UP:
                                events = User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP;
                                break;
                            case GestureBtnEventType.DOWN:
                                events = User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN;
                                break;
                            case GestureBtnEventType.CLICK:
                                events = User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN | User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP;
                                break;
                        }
                    }
                    else
                    {
                        switch (eventType)
                        {
                            case GestureBtnEventType.UP:
                                events = User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTUP;
                                break;
                            case GestureBtnEventType.DOWN:
                                events = User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTDOWN;
                                break;
                            case GestureBtnEventType.CLICK:
                                events = User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTDOWN | User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTUP;
                                break;
                        }
                    }
                    break;
                case GestureButtons.MiddleButton:
                    switch (eventType)
                    {
                        case GestureBtnEventType.UP:
                            events = User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEUP;
                            break;
                        case GestureBtnEventType.DOWN:
                            events = User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEDOWN;
                            break;
                        case GestureBtnEventType.CLICK:
                            events = User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEDOWN | User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEUP;
                            break;
                    }
                    break;
            }

            User32.SetCursorPos(x, y);
            //User32.SetMessageExtraInfo(new IntPtr(MOUSE_EVENT_EXTRA_SIMULATED));
            User32.mouse_event(events, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
            //var sim = new InputSimulator();
            //sim.Mouse.RightButtonDown();
            //sim.Mouse.Sleep(10);
            //sim.Mouse.RightButtonUp();

            _simulatingMouse = false;
        }

        private void Post(WM msg, int param = 0)
        {
            lock (_msgQueue)
            {
                _msgQueue.Enqueue(new MSG(){message = msg, param = param});
                Monitor.Pulse(_msgQueue);
            }
        }

        private bool HandleModifier(GestureModifier modifier)
        {
            if (IsModifierFiltered(modifier))
            {
                return false;
            }

            if (modifier == (modifier & GestureModifier.Scroll))
            {
                //如果事件发生的间隔不到x毫秒，则不发布新事件
                var now = DateTime.UtcNow;
                if (now - _modifierEventHappendPrevTime > TimeSpan.FromMilliseconds(100))
                {
                    Post(WM.GESTBTN_MODIFIER, (int)modifier);
                    _modifierEventHappendPrevTime = now;
                }
            }
            else
            {
                Post(WM.GESTBTN_MODIFIER, (int)modifier);
            }


            return true;
        }

        private bool IsModifierFiltered(GestureModifier modifier)
        {
            return modifier == (modifier & _filteredModifiers);
        }

        private void StayTimeoutProc(object o, ElapsedEventArgs e)
        {
            _stayTimer.Stop();
            _moveCount = 0;
            _isTimeout = true;

            Post(WM.STAY_TIMEOUT);
        }

        private void UpdateContextAndEventArgs()
        {
            if (_moveCount == 0)
            {
                _currentContext.StartPoint = _curPos; //ToLeftDownCoord(ref _curPos);
            }

            _currentContext.GestureButton = _gestureBtn;
            _currentContext.EndPoint = _curPos;//ToLeftDownCoord(ref _curPos);

            _currentEventArgs.Button = _gestureBtn;

            _currentEventArgs.Context = _currentContext;
            _currentEventArgs.Location = _currentContext.EndPoint;
            _currentEventArgs.Modifier = GestureModifier.None;

        }

        #endregion


        #region event publisher

        private bool OnBeforePathStart()
        {
            UpdateContextAndEventArgs();

            var args = new BeforePathStartEventArgs(_currentEventArgs);
            if (BeforePathStart != null) BeforePathStart(args);
            return args.ShouldPathStart;
        }

        private DateTime _mouseDownTime = DateTime.UtcNow;
        private void OnMouseDown()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            Debug.WriteLine("OnMouseDown");

            _lastPoint = _curPos;
            _lastEffectivePos = _curPos;
            _startPoint = _lastEffectivePos;
            _isTimeout = false;
            _isInitialTimeout = false;
            _initialMoveValid = false;

            if (InitialStayTimeout) _mouseDownTime = DateTime.UtcNow;
        }

        private void OnMouseMove()
        {
            if (StayTimeout && _isTimeout) return;

            //如果冻结了移动跟踪，则不通知移动事件
            if (IsSuspended) return;

            if (!_initialMoveValid)
            {
                //起始停留超时
                if (InitialStayTimeout && !_isInitialTimeout)
                {
                    var now = DateTime.UtcNow;

                    if (now - _mouseDownTime > TimeSpan.FromMilliseconds(InitialStayTimeoutMillis))
                    {
                        _isInitialTimeout = true;
                        return;
                    }

                    _mouseDownTime = now;
                }

                var initialMoveDist = GetPointDistance(ref _curPos, ref _startPoint);
                if (initialMoveDist > InitialValidMove)
                {
                    _initialMoveValid = true;

                    if (InitialStayTimeout && _isInitialTimeout)
                    {
                        Debug.WriteLine("Begin Drag");
                        SimulateGestureBtnEvent(GestureBtnEventType.DOWN, _startPoint.X, _startPoint.Y);
                        return;
                    }

                    if (PathStart != null)
                    {
                        _currentEventArgs.Location = _startPoint;
                        PathStart(_currentEventArgs);
                    }
                }
                else goto end;
            }

            if (InitialStayTimeout && _isInitialTimeout) return;


            _moveCount++;

            var dist = GetPointDistance(ref _curPos, ref _lastPoint);

            if (dist > 4 && PathGrow != null)
            {
                PathGrow(_currentEventArgs);
                _lastPoint = _curPos;
            }
            if (GetPointDistance(ref _curPos, ref _lastEffectivePos) > EffectiveMove && EffectivePathGrow != null)
            {
                EffectivePathGrow(_currentEventArgs);
                _lastEffectivePos = _curPos;
            }

        end:
            if (_stayTimeout && _initialMoveValid && !_isInitialTimeout)
            {
                _stayTimer.Stop();
                _stayTimer.Start();
            }
        }

        private void OnModifier(GestureModifier modifier)
        {
            Debug.WriteLine("OnModifier");
            if (_isTimeout) return;


            //lParam代表滚动的角度,0表click

            _currentEventArgs.Modifier = modifier;
            if (!_initialMoveValid && PathStart != null)
            {
                _initialMoveValid = true;

                _currentEventArgs.Location = _startPoint;
                PathStart(_currentEventArgs);
            }
            if (PathModifier != null) PathModifier(_currentEventArgs);

            //如果已经被冻结，则不应继续计时
            if (!IsSuspended && _stayTimeout)
            {
                _stayTimer.Stop();
                _stayTimer.Start();
            }

            _modifierEventHappendPrevTime = DateTime.UtcNow;

        }

        private void OnMouseUp()
        {
            Debug.WriteLine("OnMouseUp");
            //如果手势初始时没有移动足够的距离，则模拟发送相应事件
            //            if (!_initialMoveValid)
            //            {
            //                Console.WriteLine("SimulateCick");
            //                SimulateMouseEvent(MouseEventType.DOWN,_curPos.X, _curPos.Y);
            //                //SimulateGestureKeyButtonEvent(GestureKeyButtonEventType.DOWN);
            //                //两次事件稍微间隔一点时间，否则任务栏上右击有时会无效！
            //                //Thread.Sleep(10);
            //                SimulateMouseEvent(MouseEventType.UP,_curPos.X, _curPos.Y);
            //            }


            if (_stayTimeout) _stayTimer.Stop();
            if (PathEnd != null && _initialMoveValid && !_isTimeout) PathEnd(_currentEventArgs);


            _filteredModifiers = GestureModifier.None;
            IsSuspended = false;
            _moveCount = 0;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GCSettings.LatencyMode = GCLatencyMode.Interactive;

            //Low Memory Usage Illusion...
            /*using (var proc = Process.GetCurrentProcess())
            {
                Native.SetProcessWorkingSetSize(proc.Handle, -1, -1);
            }*/
        }

        private void OnTimeout()
        {
            Debug.WriteLine("OnTimeout");

            if (PerformNormalWhenTimeout) SimulateGestureBtnEvent(GestureBtnEventType.UP, _curPos.X, _curPos.Y);

            if (PathTimeout != null) PathTimeout(_currentEventArgs);
        }

        private void OnPauseResume(bool pause)
        {
            if (pause)
            {
                Debug.WriteLine("Pausing");

                //_mouseHook.Uninstall();
                _isPaused = true;
            }
            else
            {
                Debug.WriteLine("Resuming");

                //_mouseHook.Install();
                _isPaused = false;
            }
        }

        private void OnStop()
        {
            Debug.WriteLine("Stopping");
            _mouseHook.Uninstall();
            _isPaused = false;
            _isStopped = true;
        }

        #endregion


        #region util

        private static float GetPointDistance(ref Point a, ref Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return (int)Math.Sqrt(dx * dx + dy * dy);
        }
        #endregion


        #region Dispose

        protected void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                _mouseHook.Dispose();

                if (!_isStopped) Stop();
                if (_stayTimer != null) _stayTimer.Dispose();

            }
            else
            {
                Debug.WriteLine("Win32MousePathTracker2.Dispose(false) called by finalizer, which is probably dangerous.");
            }


            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Win32MousePathTracker2()
        {
            Dispose(false);
        }

        #endregion


        #region types

        struct MSG
        {
            public WM message;
            public int param;
        }

        public enum GestureBtnEventType
        {
            UP, DOWN, CLICK
        }

        private enum WM : uint
        {
            WM_NOTHING = 0,
            WM_USER = 0x0400,
            STOP = WM_USER + 1,
            MOUSE = WM_USER + 2,
            STAY_TIMEOUT = WM_USER + 3,

            GESTBTN_DOWN = WM_USER + 4,
            GESTBTN_UP = WM_USER + 5,
            GESTBTN_MOVE = WM_USER + 6,
            GESTBTN_MODIFIER = WM_USER + 7,//代表左键和滚轮事件

            PAUSE_RESUME = WM_USER + 8
        }


        #endregion
    }
}
