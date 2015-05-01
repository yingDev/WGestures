using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput.Native;
using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;


namespace WGestures.Core
{
    //todo:重构事件发布
    public class GestureParser : IDisposable
    {

        #region delegate types
        public delegate void GestureIntentEventHandler(GestureIntent intent);
        public delegate void IntentExecutedEventHandler(GestureIntent intent, GestureIntent.ExecutionResult result);
        #endregion

        private bool _isInCaptureMode;
        private SynchronizationContext _syncContext;
        private object @lock = new object();

        #region Properties

        /// <summary>
        /// 决定是否处于捕获模式，此属性线程安全
        /// </summary>
        public virtual bool IsInCaptureMode
        {
            get { return _isInCaptureMode; }
            set
            {
                lock (@lock)
                {
                    if (_isInCaptureMode && value) throw new InvalidOperationException();
                    if (value)
                    {
                        _syncContext = SynchronizationContext.Current;
                        if (_isPaused) Resume();
                    }
                    _isInCaptureMode = value;
                }
            }
        }

        public bool EnableHotCorners { get; set; }
        public bool Enable8DirGesture { get; set; }

        //public bool DisableInFullScreenMode { get; set; }

        public int MaxGestureSteps { get; set; }
        public bool IsPaused { get { return _isPaused; } }

        public IPathTracker PathTracker { get; private set; }
        public IGestureIntentFinder IntentFinder { get; private set; }
        #endregion


        private Dictionary<Action<Gesture>, SynchronizationContext> _gestureCapturedEventHandlerContexts = new Dictionary<Action<Gesture>, SynchronizationContext>();

        #region Events
        public event GestureIntentEventHandler IntentRecognized;
        public event IntentExecutedEventHandler IntentExecuted;
        public event GestureIntentEventHandler IntentReadyToExecute;
        public event Action IntentInvalid;
        public event Action IntentOrPathCanceled;
        public event Action<GestureModifier> IntentReadyToExecuteOnModifier;
        public event Action<string,GestureIntent> CommandReportStatus;

        public event Action<Gesture> GestureCaptured
        {
            add
            {
                lock (@lock)
                {
                    if (_gestureCapturedEventHandlerContexts.ContainsKey(value)) return;
                    _gestureCapturedEventHandlerContexts[value] = SynchronizationContext.Current;
                }
            }
            remove
            {
                lock (@lock)
                {
                    _gestureCapturedEventHandlerContexts.Remove(value);
                }

            }
        }
        public event Action<State> StateChanged;
        #endregion

        private Gesture _gesture;
        private Point _lastPoint;
        private int _pointCount = 0;
        private GestureIntent _effectiveIntent;
        private ExeApp _currentApp;

        private bool _isPaused;

        public GestureParser(IPathTracker pathTracker, IGestureIntentFinder intentFinder)
        {
            PathTracker = pathTracker;
            IntentFinder = intentFinder;

            MaxGestureSteps = 12;


            PathTracker.BeforePathStart += PathTrackerOnBeforePathStart;
            PathTracker.PathStart += PathTrackerOnPathStart;
            PathTracker.PathEnd += PathTrackerOnPathEnd;
            PathTracker.EffectivePathGrow += PathTrackerOnEffectivePathGrow;
            PathTracker.PathModifier += PathTrackerOnPathModifier;
            PathTracker.HotCornerTriggered += PathTracker_HotCornerTriggered;
        }



        public virtual void Start()
        {
            PathTracker.Start();

        }

        public virtual void Pause()
        {
            PathTracker.Paused = true;
            _isPaused = true;

            OnStateChanged(State.PAUSED);
        }

        public virtual void Resume()
        {
            PathTracker.Paused = false;
            _isPaused = false;

            OnStateChanged(State.RUNNING);
        }

        public virtual void Stop()
        {
            PathTracker.Stop();
            OnStateChanged(State.STOPPED);

        }

        #region internal

        private Point _lastVector;
        private Point _firstStrokeEndPoint;

        //返回告知手势是否发生了变化
        private bool Parse(PathEventArgs args)
        {
            var gestureChanged = false;

            if (_pointCount != 0 && args.Location != _lastPoint)
            {
                var vector = new Point(args.Location.X - _lastPoint.X, -args.Location.Y + _lastPoint.Y);
                Gesture.GestureDir dir;

                if (Enable8DirGesture)
                {
                    var count = _gesture.Count();
                    switch (count)
                    {
                        case 0:
                            dir = Get8DirectionDir(vector);
                            _lastVector = vector;
                            _firstStrokeEndPoint = args.Location;
                            break;
                        case 1:
                            var last = _gesture.Last().Value;
                            if ((int)last % 2 == 0) //如果不是斜线
                            {
                                dir = Get4DirectionDir(vector);
                                break;
                            }

                            dir = Get8DirectionDir(vector);
                            if (dir != last)
                            {
                                if (GetAngle(new Point(_firstStrokeEndPoint.X - args.Context.StartPoint.X, _firstStrokeEndPoint.Y - args.Context.StartPoint.X),
                                    new Point(args.Location.X - _firstStrokeEndPoint.X, args.Location.Y - _firstStrokeEndPoint.Y)) < 36f)
                                {
                                    dir = last;
                                    break;
                                }

                                dir = Get4DirectionDir(vector);

                                var lastDirShouldBe = Get4DirectionDir(_lastVector);

                                _gesture.Dirs[0] = lastDirShouldBe;

                                gestureChanged = true;
                            }
                            else
                            {
                                //如果依然延续斜线，则记录下最后一个点
                                _firstStrokeEndPoint = args.Location;
                            }
                            break;
                        default:
                            dir = Get4DirectionDir(vector);
                            break;
                    }
                }
                else
                {
                    dir = Get4DirectionDir(vector);
                }
                

                if (dir != _gesture.Last())
                {
                    _gesture.Add(dir);
                    gestureChanged = true;
                }

                if (_gesture.Modifier != args.Modifier)
                {
                    _gesture.Modifier = args.Modifier;
                    gestureChanged = true;
                }
            }

            _lastPoint = args.Location;
            _pointCount++;

            return gestureChanged;
        }

        //决定是否应该开始路径
        private void PathTrackerOnBeforePathStart(BeforePathStartEventArgs args)
        {
            if (IsInCaptureMode) return;

            //全屏下禁止手势的情况
            /*if (DisableInFullScreenMode && args.Context.IsInFullScreenMode)
            {
                Debug.WriteLine("全屏禁用");
                args.ShouldPathStart = false;
                return;
            }*/

            var shouldStart = IntentFinder.IsGesturingEnabledForContext(args.PathEventArgs.Context, out _currentApp);
            args.ShouldPathStart = shouldStart;
        }

        //Stopwatch sw = new Stopwatch();
        private void PathTrackerOnEffectivePathGrow(PathEventArgs args)
        {
            if (_gesture.Count() >= MaxGestureSteps) return;

            var gestureChanged = Parse(args);

            //如果手势同之前则不需要判断
            if (!gestureChanged)
            {
                return;
            }

            Debug.WriteLine("GestureChanged");

            if (IsInCaptureMode)
            {
                return;
            }

            var lastEffectiveIntent = _effectiveIntent;
            _effectiveIntent = IntentFinder.Find(_gesture, _currentApp);

            if (_effectiveIntent != null)
            {
                Debug.WriteLine("Call IntentRecognized");
                if (IntentRecognized != null) IntentRecognized(_effectiveIntent);
            }
            else if (lastEffectiveIntent != null)
            {
                if (IntentInvalid != null) IntentInvalid();
            }

            Debug.WriteLine("Gesture:" + _gesture);
        }

        private void PathTrackerOnPathEnd(PathEventArgs args)
        {
            if (IsInCaptureMode)
            {
                OnGestureCaptured(_gesture);
                return;
            }

            if (_effectiveIntent == null)
            {
                if (IntentOrPathCanceled != null) IntentOrPathCanceled();
                return;
            }

            //如果是一个允许修饰符执行的手势，显然，它必然已经被执行过了（在被识别为滚轮手势随后）
            //所以不执行而直接结束
            if (_effectiveIntent.CanExecuteOnModifier())
            {
                var modifierStateAwareCmd = _effectiveIntent.Command as IGestureModifiersAware;
                if (modifierStateAwareCmd != null)
                {
                    modifierStateAwareCmd.GestureEnded();
                    modifierStateAwareCmd.ReportStatus -= OnCommandReportStatus;
                }

                //发布事件
                OnIntentReadyToExecute(_effectiveIntent);
                if (IntentExecuted != null) IntentExecuted(_effectiveIntent, null);
                return;
            }


            OnIntentReadyToExecute(_effectiveIntent);
            var result = _effectiveIntent.Execute(args.Context, this);

            //发布事件
            if (IntentExecuted != null) IntentExecuted(_effectiveIntent, result);
        }

        private void PathTrackerOnPathStart(PathEventArgs args)
        {
            //初始化
            _effectiveIntent = null;
            _pointCount = 1;
            _gesture = new Gesture(args.Context.GestureButton);
            _lastPoint = args.Location;
        }

        private void PathTrackerOnPathModifier(PathEventArgs args)
        {
            Debug.WriteLineIf(_gesture.Modifier != args.Modifier,"Gesture:" + _gesture);

            _gesture.Modifier = args.Modifier;

            if (IsInCaptureMode)
            {
                return;
            }

            //如果当前被“捕获”了，则把修饰符事件发送给命令。
            if (_effectiveIntent != null)
            {
                var modifierStateAwareCommand = _effectiveIntent.Command as IGestureModifiersAware;
                if (PathTracker.IsSuspended && modifierStateAwareCommand != null)
                {
                    modifierStateAwareCommand.ModifierTriggered(args.Modifier);
                    return;
                }

            }

            var lastEffectiveIntent = _effectiveIntent;
            _effectiveIntent = IntentFinder.Find(_gesture, args.Context);

            if (_effectiveIntent != null)
            {
                if (IntentRecognized != null && _effectiveIntent != lastEffectiveIntent) IntentRecognized(_effectiveIntent);

                //如果设置了允许滚动时执行 且 确实可以执行（手势包含滚轮），则执行
                //这样执行之后，在释放手势的时候应该 不再执行！
                if (_effectiveIntent.CanExecuteOnModifier())
                {
                    OnIntentReadyToExecuteOnModifier(args.Modifier);


                    var modifierStateAwareCommand = _effectiveIntent.Command as IGestureModifiersAware;

                    //todo: 这个逻辑似乎应该放在GestureIntent中
                    if (modifierStateAwareCommand != null)
                    {
                        modifierStateAwareCommand.ReportStatus += OnCommandReportStatus;
                        GestureModifier observedModifiers;
                        modifierStateAwareCommand.GestureRecognized(out observedModifiers);

                        //要观察的modifier事件与PathTracker需要排除的恰好相反
                        PathTracker.SuspendTemprarily(filteredModifiers: GestureModifier.All &~ observedModifiers);
                    }
                    else
                    {
                        //对于非组合手势，同样unhook除了触发修饰符之外的所有修饰符，这样可以仍然反复执行，实现类似“多出粘贴”的功能！
                        PathTracker.SuspendTemprarily(filteredModifiers: GestureModifier.None);//GestureModifier.All &~ args.Modifier);

                        //todo：在这里发布一个事件应该是合理的
                        _effectiveIntent.Execute(args.Context, this);
                    }

                }
            }
            else if (lastEffectiveIntent != null)
            {
                if (IntentInvalid != null) IntentInvalid();
            }

        }


        //FIXME: MUST REFACTOR
        void PathTracker_HotCornerTriggered(ScreenCorner corner)
        {
            if (!EnableHotCorners) return;
            Debug.WriteLine("HotCorner: " + corner);
            switch (corner)
            {
                case ScreenCorner.RightBottom:
                    Sim.KeyDown(VirtualKeyCode.LWIN);
                    Sim.KeyDown(VirtualKeyCode.VK_D);
                    Sim.KeyUp(VirtualKeyCode.VK_D);
                    Sim.KeyUp(VirtualKeyCode.LWIN);
                    break;
                case ScreenCorner.RightTop:
                    Sim.KeyDown(VirtualKeyCode.LMENU);
                    Sim.KeyDown(VirtualKeyCode.TAB);
                    Thread.Sleep(100);
                    Sim.KeyUp(VirtualKeyCode.TAB);
                    Sim.KeyUp(VirtualKeyCode.LMENU);
                    break;
                case ScreenCorner.LeftTop:
                    Sim.KeyPress(VirtualKeyCode.CANCEL);
                    break;
                case ScreenCorner.LeftBottom:
                    Sim.KeyPress(VirtualKeyCode.LWIN);
                    break;
            }
        }
        #endregion

        #region Event Publishing
        /// <summary>
        /// 当处于捕获模式，用户手势结束的时候发生。如果注册GestureCaptured Handler的时候捕获的SynchronizationContext不为null（比如Gui线程），
        /// 则在其SynchronizationContext文中执行。
        /// </summary>
        /// <param name="gesture"></param>
        protected void OnGestureCaptured(Gesture gesture)
        {
            lock (@lock)
            {
                if (_gestureCapturedEventHandlerContexts.Count == 0) return;

                foreach (var kv in _gestureCapturedEventHandlerContexts)
                {
                    var syncContext = kv.Value;
                    var handler = kv.Key;
                    if (syncContext == null)
                    {
                        handler(gesture);
                    }
                    else
                    {
                        syncContext.Post(s => handler(gesture), null);
                    }
                }

            }

        }

        protected void OnStateChanged(State state)
        {
            if (StateChanged != null) StateChanged(state);
        }

        protected void OnIntentReadyToExecute(GestureIntent intent)
        {
            if (IntentReadyToExecute != null) IntentReadyToExecute(intent);
        }

        protected void OnIntentReadyToExecuteOnModifier(GestureModifier modifier)
        {
            if (IntentReadyToExecuteOnModifier != null) IntentReadyToExecuteOnModifier(modifier);
        }

        protected void OnCommandReportStatus(string status)
        {
            if (CommandReportStatus != null) CommandReportStatus(status, _effectiveIntent);
        }

        #endregion

        #region util
        private static Gesture.GestureDir Get4DirectionDir(Point vector)
        {
            var dir = Gesture.GestureDir.Up;
            //第一象限
            if (vector.X >= 0 && vector.Y >= 0)
            {
                dir = vector.X > vector.Y ? Gesture.GestureDir.Right : Gesture.GestureDir.Up;
            }
            //第二象限
            else if (vector.X <= 0 && vector.Y >= 0)
            {
                dir = -vector.X > vector.Y ? Gesture.GestureDir.Left : Gesture.GestureDir.Up;
            }
            //第三
            else if (vector.X <= 0 && vector.Y <= 0)
            {
                dir = -vector.X > -vector.Y ? Gesture.GestureDir.Left : Gesture.GestureDir.Down;
            }
            //4
            else if (vector.X >= 0 && vector.Y <= 0)
            {
                dir = vector.X > -vector.Y ? Gesture.GestureDir.Right : Gesture.GestureDir.Down;
            }

            return dir;
        }

        private static Gesture.GestureDir Get8DirectionDir(Point vector)
        {
            var angle = GetAngle(new Point(0, 1), vector);
            if (vector.X < 0) angle = 360 - angle;

            var mod = angle % 90;

            // Console.WriteLine("angle:" + angle);
            //Console.WriteLine("mod:"+mod);

            //8 parts
            //Console.WriteLine("floor:"+Math.Floor(angle / 45));
            var n = (int)Math.Floor(angle / 45);
            var nIsEven = ((n & 1) == 0);

            const float slashRange = 50;

            if (nIsEven && (mod > 45 - slashRange / 2) || !nIsEven && (mod > 45 + slashRange / 2))
            {
                n++;
                if (n == 8) n = 0;
            }
            //Console.WriteLine("n="+n);
            //Console.WriteLine((Gesture.GestureDir)n);

            /*
            var N = (int)Math.Floor(angle / 22.5f);
            N = (N & 1) == 1 ? (N + 1) >> 1 : N >> 1;
            if (N == 8) N = 0;*/

            return (Gesture.GestureDir)n;
        }

        private static float GetAngle(Point vectorA, Point vectorB)
        {
            float angle = 0.0f; // 夹角

            // 向量Vector a的(x, y)坐标
            float va_x = vectorA.X;
            float va_y = vectorA.Y;

            // 向量b的(x, y)坐标
            float vb_x = vectorB.X;
            float vb_y = vectorB.Y;

            float productValue = (va_x * vb_x) + (va_y * vb_y);  // 向量的乘积
            float va_val = (float)Math.Sqrt(va_x * va_x + va_y * va_y);  // 向量a的模
            float vb_val = (float)Math.Sqrt(vb_x * vb_x + vb_y * vb_y);  // 向量b的模
            float cosValue = productValue / (va_val * vb_val);      // 余弦公式

            // acos的输入参数范围必须在[-1, 1]之间，否则会"domain error"
            // 对输入参数作校验和处理
            if (cosValue < -1 && cosValue > -2)
                cosValue = -1;
            else if (cosValue > 1 && cosValue < 2)
                cosValue = 1;

            // acos返回的是弧度值，转换为角度值
            angle = (float)(Math.Acos(cosValue) * 180 / Math.PI);

            return angle;
        }

        private static float GetDistance(Point a, Point b)
        {
            var deltaX = a.X - b.X;
            var deltaY = a.Y - b.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
        #endregion

        public enum State
        {
            RUNNING, PAUSED, STOPPED
        }

        public void Dispose()
        {
            if (PathTracker != null)
            {
                PathTracker.BeforePathStart -= PathTrackerOnBeforePathStart;
                PathTracker.PathStart -= PathTrackerOnPathStart;
                PathTracker.PathEnd -= PathTrackerOnPathEnd;
                PathTracker.EffectivePathGrow -= PathTrackerOnEffectivePathGrow;
                PathTracker.PathModifier -= PathTrackerOnPathModifier;
                PathTracker.HotCornerTriggered -= PathTracker_HotCornerTriggered;
            }
        }
    }


}
