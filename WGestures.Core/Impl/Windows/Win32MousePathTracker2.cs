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
using System.Text;
using Microsoft.Win32;
using WindowsInput.Native;

namespace WGestures.Core.Impl.Windows
{
    public class Win32MousePathTracker2 : IPathTracker
    {
        private const int SIMULATED_EVENT_TAG = 19900620;
        static readonly Version OSVersion = Environment.OSVersion.Version;

        #region properties
        /// <summary>
        /// 获取和设置哪个鼠标键引发手势
        /// </summary>
        public GestureTriggerButton TriggerButton { get; set; }

        /// <summary>
        ///获取和设置鼠标键按下后至少移动多少距离才开始跟踪手势，单位为像素 
        /// </summary>
        public int InitialValidMove { get; set; }

        /// <summary>
        /// 获取和设置是否允许起始移动超时（如果启用， 则手势键按下后若超过InitalStayTimeoutMillis时间没有有效移动，将执行正常拖拽操作）
        /// </summary>
        public bool InitialStayTimeout
        {
            get { return _initalStayTimeout; }
            set
            {
                _initalStayTimeout = value;
                /*if (value && _initialStayTimer == null)
                {
                    //_initialStayTimer = new Timer();
                    //_initialStayTimer.Elapsed += InitialStayTimeoutProc;
                }
                else if (!value && _initialStayTimer != null)
                {
                    //_initialStayTimer.Dispose();
                    //_initialStayTimer = null;
                }*/
            }
        }

        /// <summary>
        /// 是否优先使用鼠标指针下方窗口作为目标
        /// </summary>
        public bool PreferWindowUnderCursorAsTarget
        {
            get;
            set;
        }

        public bool DisableInFullscreen
        {
            get;
            set;
        }

        /// <summary>
        /// 获取和设置起始移动超时的时间值
        /// </summary>
        public int InitialStayTimeoutMillis
        {
            get { return _intialStayTimeoutMillis; }
            set
            {
                _intialStayTimeoutMillis = value;
                //if (_initialStayTimer != null) _initialStayTimer.Interval = value;
            }
        }

        private int _effectiveMove;

        /// <summary>
        /// 获取和设置噪音过滤的阈值，即至少移动了多少像素才被认为'有效‘移动了
        /// </summary>
        public int EffectiveMove
        {
            get { return _effectiveMove; }
            set
            {
                if(value < _stepSize) throw new ArgumentException("EffectiveMove 不能小于 StepSize");
                _effectiveMove = value;
            }
        }

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

        private int _stepSize;
        public int StepSize 
        {
            get { return _stepSize; }
            set
            {
                if (value > EffectiveMove) throw new ArgumentException("SetpSize 不能大于 EffectiveMove");
                _stepSize = value;
            } 
        }

        private bool _enableWinKeyGesturing;
        public bool EnableWindowsKeyGesturing
        {
            get { return _enableWinKeyGesturing; }
            set
            {
                if(value)
                {
                     if(!_enableWinKeyGesturing)
                        _mouseKbdHook.KeyboardHookEvent += KeyboardHookProc;
                }else
                {
                    _mouseKbdHook.KeyboardHookEvent -= KeyboardHookProc;
                }

                _enableWinKeyGesturing = value;
            }
        }
        #endregion

        
        #region fields
        private readonly MouseKeyboardHook _mouseKbdHook;
        //private TouchHook _touchHook;

        private Queue<MSG> _msgQueue = new Queue<MSG>(16);
        
        //表明是否是“performNormal”的情况下自己模拟的鼠标事件。
        private GestureModifier _filteredModifiers;

        private Point _startPoint;
        private Point _lastPoint;
        private Point _lastEffectivePos;
        private Point _curPos;
        
        private int _moveCount;
        private readonly GestureContext _currentContext = new Win32GestureContext();
        private readonly PathEventArgs _currentEventArgs = new PathEventArgs();
        
        //for timers
        private Timer _stayTimer;//, _initialStayTimer;
        private volatile bool _isTimeout;
        private bool _isInitialTimeout;
        
        private bool _stayTimeout;
        private int _stayTimeoutMillis = 500;
        private bool _initalStayTimeout;
        private int _intialStayTimeoutMillis = 150;
        
        private bool _initialMoveValid;

        private bool _isPaused;
        private bool _isStopped;

        //避免滚轮事件发布过于频繁
        //用于记录鼠标滚轮事件上一次发生的时间，如果时间间隔小于一定值，则不发布新的事件。
        private DateTime _modifierEventHappendPrevTime;

        //note: 由于360等可能导致dwExtraInfo丢失，因此使用这个变量作为备份方案
        private bool _simulatingInput;
        private bool _captured;
        private GestureTriggerButton _gestureBtn;
        private DateTime _mouseDownTime = DateTime.UtcNow;
        
        private bool _isHotCornerReset = true;
        private ScreenCorner _lastTriggeredCorner;

        private EdgeInteractDetector _edgeDetector;

        //Virtual Gesturing
        private bool _isVirtualGesturing;
        
        #endregion

        public Win32MousePathTracker2()
        {
            var dpiFactor = Native.GetScreenDpi()/96.0f;
            //properties defaults
            TriggerButton = /*GestureButtons.RightButton |*/ GestureTriggerButton.Middle;
            InitialValidMove = (int)(5 * dpiFactor);
            InitialStayTimeout = true;
            InitialStayTimeoutMillis = 150;
            
            EffectiveMove = (int)(10 * dpiFactor) * 2;//todo: 增加灵敏度调整
            StepSize = 3;// EffectiveMove/4;// (int) (EffectiveMove * 0.8 * dpiFactor);// EffectiveMove/8;
            StayTimeout = false;
            PerformNormalWhenTimeout = false;

            _mouseKbdHook = new MouseKeyboardHook();
            _mouseKbdHook.MouseHookEvent += MouseHookProc;

            //_mouseKbdHook.KeyboardHookEvent += KeyboardHookProc;

            //Touch Only Support Win8+
            if (OSVersion.Major >= 6 && OSVersion.Minor > 1)
            {
                //太难弄，暂时屏蔽
                //_touchHook = new TouchHook();
            }
            
            _edgeDetector = new EdgeInteractDetector(_mouseKbdHook);
            _edgeDetector.Rub += EdgeDetector_Rub;

            //virtual gesturing

        }


        public event Action<bool> RequestPauseResume;
        public event Action RequestShowHideTray;

        #region IPathTracker Members
        public event BeforePathStartEventHandler BeforePathStart;
        public event PathTrackEventHandler PathStart;
        public event PathTrackEventHandler PathGrow;
        public event PathTrackEventHandler EffectivePathGrow;
        public event PathTrackEventHandler PathEnd;
        public event PathTrackEventHandler PathTimeout;
        public event PathTrackEventHandler PathModifier;
        public event Action<ScreenCorner> HotCornerTriggered;
        public event Action<ScreenEdge> EdgeRubbed;
        
        public void Start()
        {
            _mouseKbdHook.Install();
            //Touch Only Support Win8+
            //if (OSVersion.Major >= 6 && OSVersion.Minor > 1) _touchHook.Install();
            
            
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
                        OnMouseDown();break;
                    case WM.GESTBTN_MOVE:
                        OnMouseMove();break;
                    case WM.HOT_CORNER:
                        OnHotCorner((ScreenCorner)msg.param);break;
                    case WM.RUB_EDGE:
                        OnRubEdge((ScreenEdge)msg.param); break;
                    case WM.GESTBTN_MODIFIER:
                        OnModifier((GestureModifier)msg.param);break;
                    case WM.GESTBTN_UP:
                        OnMouseUp(msg.param != 0);break;
                    case WM.STAY_TIMEOUT:
                        OnTimeout();break;
                    case WM.PAUSE_RESUME:
                        var pause = (msg.param == 1);
                        OnPauseResume(pause);break;
                    case WM.GUI_REQUEST:
                        if (msg.param == (int) GUI_RequestType.PauseResume)
                        {
                            if(RequestPauseResume != null) RequestPauseResume(_isPaused);//todo: 必要这个参数吗
                        }else if (msg.param == (int) GUI_RequestType.ShowHideTray)
                        {
                            if (RequestShowHideTray != null) RequestShowHideTray();
                        }
                        break;
                    //case WM.SIMULATE_MOUSE:

                    case WM.STOP:
                        OnStop();return;
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
        //NOTE: hook procs run in a separate thread.
        private void MouseHookProc(MouseKeyboardHook.MouseHookEventArgs e)
        {
            //处理 左键 + 中键 用于 暂停继续的情形
            //if( HandleSpecialButtonCombination(e) ) return;
            if (_isPaused) return;

            var mouseData = (Native.MSLLHOOKSTRUCT)Marshal.PtrToStructure(e.lParam, typeof(Native.MSLLHOOKSTRUCT));
            //fixme: 判断是否在模拟事件， 为什么不一定可靠？
            if (_simulatingInput || mouseData.dwExtraInfo.ToInt64() == SIMULATED_EVENT_TAG)
            {
                Debug.WriteLine("Simulated:" + e.Msg);
                if (InitialStayTimeout && _isInitialTimeout)
                {
                    Debug.WriteLine("_captured=false");
                    _captured = false;
                }
                return;
            }

            var prevPos = _curPos;
            _curPos = new Point(e.X, e.Y);

            var m = e.Msg;
            switch (m)
            {
                //必须在这里立即决定是否应该捕获
                case MouseMsg.WM_RBUTTONDOWN:
                case MouseMsg.WM_MBUTTONDOWN:
                case MouseMsg.WM_XBUTTONDOWN:
                    if (!_captured)
                    {
                        if (m == MouseMsg.WM_MBUTTONDOWN && (TriggerButton & GestureTriggerButton.Middle) == 0
                            || m == MouseMsg.WM_RBUTTONDOWN && (TriggerButton & GestureTriggerButton.Right) == 0
                            || m == MouseMsg.WM_XBUTTONDOWN && (TriggerButton & GestureTriggerButton.X) == 0)
                        {
                            return;
                        }
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
                            //_gestureBtn = (m == MouseMsg.WM_RBUTTONDOWN ? GestureButtons.RightButton : GestureButtons.MiddleButton);
                            switch(m) //TODO: extract function
                            {
                                case MouseMsg.WM_RBUTTONDOWN:
                                    _gestureBtn = GestureTriggerButton.Right;
                                    break;
                                case MouseMsg.WM_MBUTTONDOWN:
                                    _gestureBtn = GestureTriggerButton.Middle;
                                    break;
                                case MouseMsg.WM_XBUTTONDOWN:
                                    var x = (XButtonNumber)(mouseData.mouseData >> 16); //which X Button
                                    _gestureBtn = x == XButtonNumber.One ? GestureTriggerButton.X1 : GestureTriggerButton.X2;
                                    break;
                                default:
                                    Debug.Assert(false, "WTF! shouldn't happen");
                                    break;
                            }
                            
                            _modifierEventHappendPrevTime = new DateTime(0);
                            e.Handled = true;
                            Post(WM.GESTBTN_DOWN);
                        }
                    }
                    else //另一个键作为手势键的时候，作为修饰键
                    {
                        GestureModifier gestMod;// = m == MouseMsg.WM_RBUTTONDOWN ? GestureModifier.RightButtonDown : GestureModifier.MiddleButtonDown;

                        switch(m) //TODO: extract function
                        {
                            case MouseMsg.WM_RBUTTONDOWN:
                                gestMod = GestureModifier.RightButtonDown;
                                break;
                            case MouseMsg.WM_MBUTTONDOWN:
                                gestMod = GestureModifier.MiddleButtonDown;
                                break;
                            case MouseMsg.WM_XBUTTONDOWN:
                                var x = (XButtonNumber)(mouseData.mouseData >> 16); //which X Button
                                gestMod = x == XButtonNumber.One ? GestureModifier.X1 : GestureModifier.X2;
                                break;
                            default:
                                gestMod = GestureModifier.LeftButtonDown;
                                break;
                        }

                        e.Handled = HandleModifier(gestMod);
                    }
                    break;

                case MouseMsg.WM_MOUSEMOVE:
                    if (_captured)
                    {
                        //永远不拦截move消息，所以不设置e.Handled = true
                        Post(WM.GESTBTN_MOVE);
                    }
                    else 
                    {
                       if(_isVirtualGesturing)
                        {
                            //忽略禁用列表
                            OnBeforePathStart();
                            _captured = true;
                            _gestureBtn = GestureTriggerButton.Right;
                            Post(WM.GESTBTN_DOWN, 1);

                        }
                        //未捕获的情况下才允许hotcorner
                        HotCornerHitTest();
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
                case MouseMsg.WM_XBUTTONUP:
                    if (_captured)
                    {
                        var gestBtn_as_MouseMsg = (MouseMsg)(-1);
                        switch(_gestureBtn)
                        {
                            case GestureTriggerButton.Middle:
                                gestBtn_as_MouseMsg = MouseMsg.WM_MBUTTONUP;
                                break;
                            case GestureTriggerButton.Right:
                                gestBtn_as_MouseMsg = MouseMsg.WM_RBUTTONUP;
                                break;
                            case GestureTriggerButton.X1:
                            case GestureTriggerButton.X2:
                                gestBtn_as_MouseMsg = MouseMsg.WM_XBUTTONUP;
                                break;
  
                        }
                        
                        //是手势键up
                        if (m == gestBtn_as_MouseMsg)
                        {
                              _captured = false;       
                               Post(WM.GESTBTN_UP);
                        }

                        e.Handled = true;
                    }
                    break;
                default:
                    //其他消息不处理
                    break;
            }
        }
        
        private void KeyboardHookProc(MouseKeyboardHook.KeyboardHookEventArgs e)
        {
            if (_isPaused || _simulatingInput || e.lParam.dwExtraInfo == SIMULATED_EVENT_TAG)
            {
                Debug.WriteLine("paused or simulating");
                return;
            }
            //Debug.WriteLine("WTF " + e.key + " " + e.Type);
            
            if(e.key == Keys.LWin)
            {
                e.Handled = true;

               if (e.Type == KeyboardEventType.KeyDown)
               {
                    if (!_captured && !_isVirtualGesturing)
                    {
                        Debug.WriteLine("Begin Virtual Gesturing");
                        _isVirtualGesturing = true;

                        return;
                    }
               }else 
               {
                    if (_isVirtualGesturing) // && _isVirtualGesturing)
                    {
                        Debug.WriteLine("End Virtual Gesturing!");
                        _isVirtualGesturing = false;

                        if(_captured)
                        {
                            _captured = false;
                            Post(WM.GESTBTN_UP, 1); //isVirtual
                        }else
                        {
                            new Thread(() =>
                            {
                                _simulatingInput = true;
                                var sim = new InputSimulator() { ExtraInfo = new IntPtr(SIMULATED_EVENT_TAG) };
                                sim.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN }, new[] { (VirtualKeyCode)e.key });
                                _simulatingInput = false;
                            }).Start();
                        }
                    }else
                    {
                        e.Handled = false;
                    }

                }
            }else if(_isVirtualGesturing && e.Type == KeyboardEventType.KeyDown)
            {
                _isVirtualGesturing = false;
                new Thread(() =>
                {
                    _simulatingInput = true;
                    var sim = new InputSimulator() { ExtraInfo = new IntPtr(SIMULATED_EVENT_TAG) };
                    sim.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN }, new[] {(VirtualKeyCode) e.key });
                    _simulatingInput = false;
                }).Start();

                e.Handled = true;
            }
        }

        private void HotCornerHitTest()
        {
            const int TRIGGER_DIST = 2;
            const int REST_DIST = 40;

            var scr = Common.OsSpecific.Windows.Screen.ScreenBoundsFromPoint(_curPos);
            if (scr == null) return;
            
            var corner = 0;
            for(corner=0; corner<4; corner++)
            {
                var p = GetBoundsCornerPoint( scr.Value, (ScreenCorner)corner );
                var dist = GetPointDistance(ref p, ref _curPos);

                if (!_isHotCornerReset && _lastTriggeredCorner == (ScreenCorner) corner && dist > REST_DIST)
                {
                    _isHotCornerReset = true;
                }
                else if (dist <= TRIGGER_DIST && _isHotCornerReset)
                {
                    _isHotCornerReset = false;
                    _lastTriggeredCorner = (ScreenCorner) corner;
                    Post(WM.HOT_CORNER, (int)corner);
                }
            }
        }

        private void EdgeDetector_Rub(ScreenEdge edge)
        {
            Post(WM.RUB_EDGE, (int)edge);
        }

        /*private void SimulateGestureBtnEvent(GestureBtnEventType eventType, int x, int y)
        {
            const int CLICK_PRESS_RELEASE_INTERVAL = 10;

            Debug.WriteLine("SimulateMouseEvent: " + _gestureBtn + " " + eventType);
            _simulatingMouse = true;

            User32.SetCursorPos(x, y);

            var mouseSwapped = Native.IsMouseButtonSwapped();

            switch (_gestureBtn)
            {
                case GestureTriggerButton.Right:
                    if (mouseSwapped)
                    {
                        switch (eventType)
                        {
                            case GestureBtnEventType.UP:
                                User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                                break;
                            case GestureBtnEventType.DOWN:
                                User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                                break;
                            case GestureBtnEventType.CLICK:
                                User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                                Thread.Sleep(CLICK_PRESS_RELEASE_INTERVAL);
                                User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                                break;
                        }
                    }
                    else
                    {
                        switch (eventType)
                        {
                            case GestureBtnEventType.UP:
                                User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTUP, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                                break;
                            case GestureBtnEventType.DOWN:
                                User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTDOWN, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                                break;
                            case GestureBtnEventType.CLICK:
                                User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTDOWN, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                                Thread.Sleep(CLICK_PRESS_RELEASE_INTERVAL);
                                User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTUP, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                                break;
                        }
                    }
                    break;
                case GestureTriggerButton.Middle:
                    switch (eventType)
                    {
                        case GestureBtnEventType.UP:
                            User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEUP, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);

                            break;
                        case GestureBtnEventType.DOWN:
                            User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEDOWN, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);

                            break;
                        case GestureBtnEventType.CLICK:
                            User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEDOWN, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                            Thread.Sleep(CLICK_PRESS_RELEASE_INTERVAL);
                            User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEUP, x, y, 0, MOUSE_EVENT_EXTRA_SIMULATED);
                            break;
                    }
                    break;

                case GestureTriggerButton.X1:
                case GestureTriggerButton.X2:
                    DoMouseEvent(eventType, User32.MOUSEEVENTF.MOUSEEVENTF_XUP)
                    break;
            }
            _simulatingMouse = false;
        }*/

        private void SimulateMouseEvent(User32.MOUSEEVENTF e, int x, int y, uint data=0)
        {
            Debug.WriteLine("SimulateMouseEvent: " + e);
            _simulatingInput = true;

            User32.SetCursorPos(x, y);

            User32.mouse_event(e, x, y, data, SIMULATED_EVENT_TAG);

            _simulatingInput = false;
        }

        private User32.MOUSEEVENTF MakeGestureBtnEvent(GestureTriggerButton btn, bool isUp, out uint data)
        {
            data = 0;
            switch(btn)
            {
                case GestureTriggerButton.Right:
                    var isMouseSwapped = Native.IsMouseButtonSwapped();
                    if(!isMouseSwapped)
                    {
                        return isUp ? User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTUP : User32.MOUSEEVENTF.MOUSEEVENTF_RIGHTDOWN;
                    }else
                    {
                        return isUp ? User32.MOUSEEVENTF.MOUSEEVENTF_LEFTUP : User32.MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN;
                    }
                    
                case GestureTriggerButton.Middle:
                    return isUp ? User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEUP : User32.MOUSEEVENTF.MOUSEEVENTF_MIDDLEDOWN;
                case GestureTriggerButton.X1:
                    data = 1;
                    return isUp ? User32.MOUSEEVENTF.MOUSEEVENTF_XUP : User32.MOUSEEVENTF.MOUSEEVENTF_XDOWN;
                case GestureTriggerButton.X2:
                    data = 2;
                    return isUp ? User32.MOUSEEVENTF.MOUSEEVENTF_XUP : User32.MOUSEEVENTF.MOUSEEVENTF_XDOWN;

                default:
                    Debug.Assert(false, "WTF");
                    return User32.MOUSEEVENTF.MOUSEEVENTF_ABSOLUTE;
            }
            
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

        private bool HandleSpecialButtonCombination(MouseKeyboardHook.MouseHookEventArgs e)
        {
           if(_captured) return false;
           
            var mouseSwapped = Native.GetSystemMetrics(Native.SystemMetric.SM_SWAPBUTTON) != 0;
            var lButtonPressed = Native.GetAsyncKeyState(mouseSwapped ? Keys.RButton : Keys.LButton) < 0;
            var shiftPressed = Native.GetAsyncKeyState(Keys.ShiftKey) < 0;

            if (e.Msg == MouseMsg.WM_MBUTTONDOWN && lButtonPressed)
            {
                if (shiftPressed)
                {
                    Post(WM.GUI_REQUEST, (int)GUI_RequestType.ShowHideTray);
                }
                else
                {
                    Post(WM.GUI_REQUEST, (int)GUI_RequestType.PauseResume);
                }
                return true;
            }
            return false;
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

                if(PreferWindowUnderCursorAsTarget)
                {
                    var fgWin = Native.WindowFromPoint(new Native.POINT() { x = _curPos.X, y = _curPos.Y });
                    _currentContext.WinId = fgWin;
                    _currentContext.ProcId = Native.GetProcessIdByWindowHandle(fgWin);
                }else
                {
                    _currentContext.ProcId = Native.GetActiveProcessId();
                }

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
            if (DisableInFullscreen && IsInFullScreenMode()) return false;

            UpdateContextAndEventArgs();

            var args = new BeforePathStartEventArgs(_currentEventArgs);
            if (BeforePathStart != null) BeforePathStart(args);
            return args.ShouldPathStart;
        }
        
        private void OnMouseDown()
        {
            //hack: dunno how
            var dpiFactor = Native.GetScreenDpi() / 96.0f;
            EffectiveMove = (int)(Common.OsSpecific.Windows.Screen.ScreenBoundsFromPoint(_curPos).Value.Width * 0.025f);
            //StepSize = 1;// (int)(EffectiveMove / 4.0f);

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

                    if (InitialStayTimeout)
                    {
                        //Reset Cursor
                        //ResetCursor();
                        if (_isInitialTimeout)
                        {
                            Debug.WriteLine("Begin Drag");
                            //SimulateGestureBtnEvent(GestureBtnEventType.DOWN, _startPoint.X, _startPoint.Y);
                            uint data;
                            SimulateMouseEvent(MakeGestureBtnEvent(_gestureBtn, false, out data), _startPoint.X, _startPoint.Y, data);
                            return; 
                        }
                    }


                    if (PathStart != null)
                    {
                        _currentEventArgs.Location = _startPoint;
                        //Console.WriteLine('>');
                        PathStart(_currentEventArgs);
                    }
                }
                else goto end;
            }

            if (InitialStayTimeout && _isInitialTimeout) return;


            _moveCount++;

            var dist = GetPointDistance(ref _curPos, ref _lastPoint);
            if (dist >= StepSize && PathGrow != null)
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

        private void OnHotCorner(ScreenCorner corner)
        {
            if (DisableInFullscreen && IsInFullScreenMode()) return;

            var mousePressed = Native.GetAsyncKeyState(Keys.LButton) < 0 ||
                               Native.GetAsyncKeyState(Keys.RButton) < 0 ||
                               Native.GetAsyncKeyState(Keys.MButton) < 0;
            if (!mousePressed && HotCornerTriggered != null) HotCornerTriggered(corner);
        }

        private void OnRubEdge(ScreenEdge edge)
        {
            if (DisableInFullscreen && IsInFullScreenMode()) return;

            var mousePressed = Native.GetAsyncKeyState(Keys.LButton) < 0 ||
                               Native.GetAsyncKeyState(Keys.RButton) < 0 ||
                               Native.GetAsyncKeyState(Keys.MButton) < 0;
            if (!mousePressed && EdgeRubbed != null) EdgeRubbed(edge);
        }

        private void OnModifier(GestureModifier modifier)
        {
            Debug.WriteLine("OnModifier: " + modifier);
            if (_isTimeout) return;
            

            _currentEventArgs.Modifier = modifier;
            if (!_initialMoveValid && PathStart != null)
            {
                _initialMoveValid = true;
                _currentEventArgs.Location = _startPoint;
                PathStart(_currentEventArgs);
            }
            if (PathModifier != null)
            {
                PathModifier(_currentEventArgs);
            }

            //如果已经被冻结，则不应继续计时
            if (!IsSuspended && _stayTimeout)
            {
                _stayTimer.Stop();
                _stayTimer.Start();
            }

            _modifierEventHappendPrevTime = DateTime.UtcNow;
        }

        private void OnMouseUp(bool isVirtual=false)
        {
            Debug.WriteLine("OnMouseUp");
            //如果手势初始时没有移动足够的距离，则模拟发送相应事件
            if (!_initialMoveValid)
            {
                Debug.WriteLine("Shit");
                if(isVirtual)
                {
                    _simulatingInput = true;
                    Debug.WriteLine("Simulating...Win Press");
                    var sim = new InputSimulator() { ExtraInfo = new IntPtr(SIMULATED_EVENT_TAG) };
                    sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.LWIN);
                    Debug.WriteLine("End Simulating...Win Press");
                    _simulatingInput = false;
                    
                }else
                {
                    uint data;
                    SimulateMouseEvent(MakeGestureBtnEvent(_gestureBtn, false, out data), _curPos.X, _curPos.Y, data);
                    SimulateMouseEvent(MakeGestureBtnEvent(_gestureBtn, true, out data), _curPos.X, _curPos.Y, data);
                }

                return;
            }

            if (_stayTimeout) _stayTimer.Stop();
            if (PathEnd != null && _initialMoveValid && !_isTimeout)
            {
                PathEnd(_currentEventArgs);
            }

            _filteredModifiers = GestureModifier.None;
            IsSuspended = false;
            _moveCount = 0;
        }

        private void OnTimeout()
        {
            Debug.WriteLine("OnTimeout");

            if (PerformNormalWhenTimeout)
            {
                //SimulateGestureBtnEvent(GestureBtnEventType.UP, _curPos.X, _curPos.Y);
                uint data;
                SimulateMouseEvent(MakeGestureBtnEvent(_gestureBtn, true, out data), _curPos.X, _curPos.Y, data);
            }

            if (PathTimeout != null) PathTimeout(_currentEventArgs);
        }

        private void OnPauseResume(bool pause)
        {
            if (pause)
            {
                Debug.WriteLine("Pausing");
                _isPaused = true;
            }
            else
            {
                Debug.WriteLine("Resuming");
                _isPaused = false;
            }


            if (_edgeDetector != null)
            {
                _edgeDetector.Paused = _isPaused;
            }
        }

        private void OnStop()
        {
            Debug.WriteLine("Stopping");
            _mouseKbdHook.Uninstall();
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

        public bool IsInFullScreenMode()
        {
            var fgWindow = Native.GetAncestor(Native.GetForegroundWindow(), Native.GetAncestorFlags.GetRoot);
            var deskWindow = User32.GetDesktopWindow();
            var shellWindow = User32.GetShellWindow();


            if (fgWindow == IntPtr.Zero || // !IsTopMostWindow(fgWindow) ||
                fgWindow == deskWindow ||
                fgWindow == shellWindow) return false;


            GDI32.RECT fgRect, screenRect;
            User32.GetWindowRect(deskWindow, out screenRect);
            User32.GetWindowRect(fgWindow, out fgRect);

            if (fgRect == screenRect)
            {
                var className = new StringBuilder(64);
                User32.GetClassName(fgWindow, className, className.Capacity);

                var classNameStr = className.ToString();
                if (classNameStr == "WorkerW" || //桌面窗口
                    classNameStr == "CanvasWindow" ||
                    classNameStr == "ImmersiveLauncher" || //win8 开始屏幕
                    classNameStr == "Windows.UI.Core.CoreWindow") //win8 metro
                {
                    return false;
                }

                Debug.WriteLine(string.Format("Window[{0:x}] IsInFullScreenMode:", fgWindow.ToInt64()));
                return true;

            }

            return false;

        }
        
        private Point GetBoundsCornerPoint(Rectangle bounds, ScreenCorner corner)
        {
            switch (corner)
            {
                case ScreenCorner.LeftBottom:
                    return new Point(bounds.Left, bounds.Bottom);
                case ScreenCorner.LeftTop:
                    return new Point(bounds.Left, bounds.Top);
                case ScreenCorner.RightBottom:
                    return new Point(bounds.Right, bounds.Bottom);
                case ScreenCorner.RightTop:
                    return new Point(bounds.Right, bounds.Top);
                default:
                    throw new NotSupportedException(corner.ToString());
            }
        }
        #endregion


        #region Dispose
        protected void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                if(_edgeDetector != null)
                {
                    _edgeDetector.Dispose();
                    _edgeDetector = null;
                }

                /*if(_touchHook != null)
                {
                    _touchHook.Dispose();
                    _touchHook = null;
                }*/

                _mouseKbdHook.Dispose();

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
            
            HOT_CORNER = WM_USER + 8,

            PAUSE_RESUME = WM_USER + 9,

            SIMULATE_MOUSE = WM_USER + 10,
            GUI_REQUEST = WM_USER + 11,
            RUB_EDGE = WM_USER + 12
        }


        private enum GUI_RequestType
        {
            PauseResume, ShowHideTray
        }





        #endregion
    }
}
