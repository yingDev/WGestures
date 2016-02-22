using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Timers;
using Microsoft.Win32;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Core;
using Timer = System.Timers.Timer;

namespace WGestures.View.Impl.Windows
{
    public class CanvasWindowGestureView : IDisposable
    {
        #region properties
        public Color PathMainColor
        {
            get { return _mainPen.Color; }
            set { _mainPen.Color = value; }
        }
        public Color PathBorderColor
        {
            get { return _borderPen.Color; }
            set { _borderPen.Color = value; }
        }
        public Color PathAlternativeColor
        {
            get { return _alternativePen.Color; }
            set { _alternativePen.Color = value; }
        }
        public Color PathMiddleBtnMainColor
        {
            get { return _middleBtnPen.Color; }
            set { _middleBtnPen.Color = value; }
        }

        public Color PathXBtnMainColor
        {
            get { return _xBtnPen.Color; }
            set { _xBtnPen.Color = value; }
        }

        public float PathWidth
        {
            get
            {
                return _mainPen.Width;
            }
            set
            {
                _mainPen.Width = _alternativePen.Width = _middleBtnPen.Width =value;
            }
        }
        public float PathBorderWidth
        {
            get { return _borderPen.Width; }
            set { _borderPen.Width = value; }
        }

        //Pen.Width 内部使用了new float[] !, 反复调用Pen.Width导致alloc大量heap内存
        private float _shadowPenWidth;
        public float PathShadowWidth
        {
            get { return _shadowPenWidth; }
            set { /*_shadowPenWidth = _shadowPen.Width = value;*/ } //TODO: ignore for temporarily
        }
        public int PathMaxPointCount
        {
            get { return _pathMaxPointCount; }
            set { _pathMaxPointCount = value; }
        }
        public bool ShowPath { get; set; }
        public bool ShowCommandName { get; set; }
        public bool ViewFadeOut { get; set; }
        #endregion

        public bool IsDisposed { get; private set; }

        #region fields
        Pen _tempMainPen;
        Pen _middleBtnPen;
        Pen _xBtnPen;
        Pen _mainPen;
        Pen _alternativePen;
        Pen _borderPen;
        Pen _shadowPen;
        Pen _dirtyMarkerPen;
        Point _prevPoint;
        private GraphicsPath _gPath = new GraphicsPath();
        private GraphicsPath _gPathDirty = new GraphicsPath();
        Pen _pathPen;
        bool _pathVisible;
        
        readonly GestureParser _gestureParser;

        Rectangle _screenBounds = Native.GetScreenBounds();
        float _dpiFactor = Native.GetScreenDpi() / 96.0f;
        int _pathMaxPointCount;

        CanvasWindow _canvasWindow;
        DiBitmap _canvasBuf;

        RectangleF _labelRect;
        RectangleF _lastLabelRect;
        bool _labelVisible;
        string _labelText;
        Color _labelColor;
        Color _systemColor;
        Color _labelBgColor;
        bool _labelChanged;
        GraphicsPath _labelPath = new GraphicsPath();
        Font _labelFont = new Font("微软雅黑", 32);

        bool _isCurrentRecognized;
        bool _recognizeStateChanged;
        short _pointCount;

        Timer _fadeOuTimer = new Timer(55);
        const byte FadeOutDelta = 60;
        byte _canvasOpacity;
        #endregion

        public CanvasWindowGestureView(GestureParser gestureParser)
        {
            _gestureParser = gestureParser;
            RegisterEventHandlers();
            var waitCanvasWindow = new AutoResetEvent(false);
            new Thread(() =>
            {
                _canvasWindow = new CanvasWindow()
                {
                    //最初的时候放在屏幕以外
                    Visible = false,
                    IgnoreInput = true,
                    NoActivate = true,
                    TopMost = true
                };
                waitCanvasWindow.Set();
                _canvasWindow.ShowDialog();
            }, maxStackSize: 1) { Name = "CanvasWindow" }.Start();

            waitCanvasWindow.WaitOne();

            _canvasBuf = new DiBitmap(_screenBounds.Size);

            InitDefaultProperties();
            _fadeOuTimer.Elapsed += OnFadeOutTimerElapsed;
            SystemEvents.DisplaySettingsChanged += SystemDisplaySettingsChanged;
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            _systemColor = Native.GetWindowColorization();
        }

        private void SystemDisplaySettingsChanged(object sender, EventArgs e)
        {
            _screenBounds = Native.GetScreenBounds();
            _canvasBuf = new DiBitmap(_screenBounds.Size);
            _dpiFactor = Native.GetScreenDpi() / 96.0f;

            
        }

        private void InitDefaultProperties()
        {
            //defaults
            ShowCommandName = true;
            ShowPath = true;
            ViewFadeOut = true;

            _pathMaxPointCount = (int)(512 * _dpiFactor);

            var widthBase = 2 * _dpiFactor;

            #region init pens
            _mainPen = new Pen(Color.FromArgb(255, 50, 200, 100), widthBase) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            _middleBtnPen = new Pen(Color.FromArgb(255, 20, 150, 200), widthBase) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            _xBtnPen = new Pen(Color.FromArgb(255, 20, 100, 200), widthBase) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            _borderPen = new Pen(Color.FromArgb(230, 255, 255, 255), widthBase * 2.5f) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            _alternativePen = new Pen(Color.FromArgb(255, 255, 120, 20), widthBase) { EndCap = LineCap.Round, StartCap = LineCap.Round };


            _shadowPen = new Pen(Color.FromArgb(25, Color.Black), widthBase * 3) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            
            
            _shadowPenWidth = _shadowPen.Width;
            _dirtyMarkerPen = (Pen)_shadowPen.Clone();
            _dirtyMarkerPen.Width *= 1.5f;

            _systemColor = Native.GetWindowColorization();

            #endregion
        }

        #region event handlers
        private void HandlePathStart(PathEventArgs args)
        {
            if (!ShowPath && !ShowCommandName) return;
            
            Debug.WriteLine("WhenPathStart");

            _screenBounds = Screen.ScreenBoundsFromPoint(args.Location).Value;//Screen.FromPoint(args.Location);
           
            _prevPoint = args.Location;//ToUpLeftCoord(args.Location);
            _pointCount = 1;

            //_tempMainPen = args.Button == GestureTriggerButton.Right ? _mainPen : _middleBtnPen;
            if ((args.Button & GestureTriggerButton.Right) != 0) _tempMainPen = _mainPen;
            else if ((args.Button & GestureTriggerButton.X) != 0) _tempMainPen = _xBtnPen;
            else _tempMainPen = _middleBtnPen;
            
            _isCurrentRecognized = false;
            _recognizeStateChanged = false;

            BeginView();
        }

        private void HandlePathGrow(PathEventArgs args)
        {
            if (!ShowPath && !ShowCommandName) return;
            if (_pointCount > _pathMaxPointCount) return;

            _pointCount++;

            if (_pointCount == _pathMaxPointCount)
            {
                ShowLabel(Color.White, "您是有多无聊啊 :)", Color.FromArgb(150, 255, 0, 0));

                DrawAndUpdate();
                _labelChanged = false;
                _recognizeStateChanged = false;

                return;
            }

            var curPos = args.Location;//ToUpLeftCoord(args.Location);

            if (ShowPath)
            {
                //需要将点换算为基于窗口的坐标
                var pA = new Point(_prevPoint.X - _screenBounds.X, _prevPoint.Y - _screenBounds.Y);
                var pB = new Point(curPos.X - _screenBounds.X, curPos.Y - _screenBounds.Y);

                if (pA != pB)
                {
                    _gPath.AddLine(pA, pB);

                    _gPathDirty.Reset();
                    _gPathDirty.AddLine(pA, pB);
                    _gPathDirty.Widen(_dirtyMarkerPen);
                }

                curPos = new Point(pB.X + _screenBounds.X, pB.Y + _screenBounds.Y);
            }

            DrawAndUpdate();

            _recognizeStateChanged = false;

            _prevPoint = curPos;//args.Location;//ToUpLeftCoord(args.Location);
        }

        private void HandleIntentRecognized(GestureIntent intent)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("IntentRecognized");

            if (ShowCommandName)
            {
                var modifierText = intent.Gesture.Modifier.ToMnemonic();
                var newLabelText = intent.Name + (modifierText == String.Empty ? String.Empty : (" " + modifierText));
                ShowLabel(Color.White, newLabelText, Color.FromArgb(120, 0, 0, 0));
            }
            
            if (!_isCurrentRecognized && ShowPath)
            {
                _isCurrentRecognized = true;
                _recognizeStateChanged = true;
                _pathPen = _tempMainPen;
                //ResetPathDirtyRect();
            }

            DrawAndUpdate();

            if (ShowCommandName) _labelChanged = false;
            _recognizeStateChanged = false;

        }

        //todo: 合并为IntentRecogChanged?
        private void HandleIntentInvalid(Gesture gesture)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("IntentInvalid");
            //pre
            if (ShowCommandName)
            {
                HideLabel();
            }

            if (_isCurrentRecognized && ShowPath)
            {
                _pathPen = _alternativePen;
                _isCurrentRecognized = false;
                _recognizeStateChanged = true;

                //ResetPathDirtyRect();
            }

            //draw
            DrawAndUpdate();

            //clear
            if (ShowCommandName)
            {
                _labelRect = default(Rectangle);
                _labelChanged = false;
            }
        }

        private void HandlePathTimeout(PathEventArgs args)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("PathTimeout");

            //if (ShowPath) ResetPathDirtyRect();
            EndView();
        }

        private void HandleIntentReadyToExecute(GestureIntent intent)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("WhenIntentReadyToExecute");
            if (ShowCommandName)
            {
                ShowLabel(Color.White, _labelText, _systemColor);////Color.FromArgb(120, 0, 80, 0));
            }

            //draw
            DrawAndUpdate();

            //clear
            if (ShowPath) _pathVisible = false;
            if (ShowCommandName) _labelChanged = false;

            if (ViewFadeOut) FadeOut();
            else EndView();
        }

        private void HandleIntentReadyToExecuteOnModifier(GestureModifier modifier)
        {
        }

        private void HandleIntentOrPathCanceled()
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("IntentOrPathCancled");
            EndView();
        }

        private void HandleGestureRecorded(Gesture g)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("WhenGestureCaptured");
            EndView();
        }

        private void HandleCommandReportStatus(string status, GestureIntent intent)
        {
            if (ShowCommandName)
            {
                var modifierText = intent.Gesture.Modifier.ToMnemonic();
                var newLabelText = (modifierText == String.Empty ? String.Empty : (modifierText + " ")) + intent.Name + status;
                if (newLabelText.Equals(_labelText)) return;

                _labelText = newLabelText;

                ShowLabel(Color.White, newLabelText, Color.FromArgb(120, 0, 0, 0));
                
                DrawAndUpdate();

                if (ShowCommandName) _labelChanged = false;
            }
        }
        #endregion

        private void BeginView()
        {
            Debug.WriteLine("BeginView");
            StopFadeout();

            if (_canvasBuf.Size != _screenBounds.Size)
            {
                _canvasBuf = new DiBitmap(_screenBounds.Size);
            }
            _canvasOpacity = 255;
            _canvasWindow.Bounds = _screenBounds;

            _canvasWindow.TopMost = true;
            _canvasWindow.Visible = true;

            if (ShowPath)
            {            
                _pathVisible = true;
                _pathPen = _alternativePen;
            }

            if (ShowCommandName)
            {
                _labelVisible = false;
            }
        }

        private void DrawAndUpdate()
        {
            Draw();

            #region 更新到窗口上
            var pathDirty = Rectangle.Ceiling(_gPathDirty.GetBounds());
            pathDirty.Offset(_screenBounds.X, _screenBounds.Y);
            pathDirty.Intersect(_screenBounds);
            pathDirty.Offset(-_screenBounds.X, -_screenBounds.Y); //挪回来变为基于窗口的坐标

            if (ShowPath) _canvasWindow.SetDiBitmap(_canvasBuf, /*_pathDirtyRect*/pathDirty);

            if (_labelChanged) //ShowCommandName)
            {
                var labelDirtyRect = _labelRect.Width > _lastLabelRect.Width ? _labelRect : _lastLabelRect;
                labelDirtyRect.Height = _labelRect.Height > _lastLabelRect.Height ? _labelRect.Height : _lastLabelRect.Height;
                _canvasWindow.SetDiBitmap(_canvasBuf, Rectangle.Ceiling(labelDirtyRect));
            }
            else if(_labelVisible)
            {
                var labelDirty = Rectangle.Ceiling(_labelRect);
                var intercected = pathDirty.IntersectsWith(labelDirty);

                if (intercected && !pathDirty.Contains(labelDirty))
                {
                    labelDirty.Intersect(pathDirty);
                    _canvasWindow.SetDiBitmap(_canvasBuf, labelDirty);
                }
            }
            #endregion
        }

        private void Draw()
        {
            var g = _canvasBuf.BeginDraw();

            //如果是识别与未识别之间转换，则使用region而非dirtyRect来重绘
            if (_recognizeStateChanged)
            {
                if (_gPath.PointCount > 0)
                {
                    _gPathDirty.Reset();
                    _gPathDirty.AddPath(_gPath, false);
                    _gPathDirty.Widen(_dirtyMarkerPen);
                }

            }
            g.SetClip(_gPathDirty);

            var labelAffected = false;
            //如果Label的内容改变，则整个重绘
            //不然，就判断哪些区域收到了path的影响，然只更新受影响的一小部分。
            if (_labelChanged)
            {
                var labelDirtyRect = _labelRect.Width > _lastLabelRect.Width ? _labelRect : _lastLabelRect;
                labelDirtyRect.Height = _labelRect.Height > _lastLabelRect.Height ? _labelRect.Height : _lastLabelRect.Height;
                
                g.SetClip(labelDirtyRect, CombineMode.Union);
                labelAffected = _labelVisible;
            }
            else if(_labelVisible)
            {
                var labelDirty = Rectangle.Ceiling(_labelRect);
                var pathDirty = Rectangle.Ceiling(_gPathDirty.GetBounds());
                labelAffected = pathDirty.IntersectsWith(labelDirty);

                if (labelAffected && !pathDirty.Contains(labelDirty))
                {
                    labelDirty.Intersect(pathDirty);
                    g.SetClip(labelDirty, CombineMode.Union);

                    Debug.WriteLine("LabelDirty="+labelDirty);
                }
            }
                     
            g.Clear(Color.Transparent);

            #region 1) 绘制路径
            if (ShowPath && _pathVisible)
            {
                g.DrawPath(_shadowPen, _gPath);
                g.DrawPath(_borderPen, _gPath);
                g.DrawPath(_pathPen, _gPath);
            }
            #endregion

            #region 2) 绘制标签
            if (labelAffected) //ShowCommandName && _labelVisible)
            {
                Debug.WriteLine("Label Redraw");
                using (var pen = new Pen(Color.White, 1.5f * _dpiFactor))
                //using (var shadow = new Pen(Color.FromArgb(40, 0, 0, 0), 3f * _dpiFactor))
                {
                    
                    /*DrawRoundedRectangle(g, RectangleF.Inflate(_labelRect,
                        -1f * _dpiFactor, -1f * _dpiFactor),
                        (int)(12 * _dpiFactor), shadow, Color.Transparent);*/
                    DrawRoundedRectangle(g, RectangleF.Inflate(_labelRect,
                        -2.6f * _dpiFactor, -2.6f * _dpiFactor),
                        0, pen, _labelBgColor);

                    //if (_labelColor != Color.White)
                        //using (var stroke = new Pen(Color.Black, 1.5f * _dpiFactor))
                        //    g.DrawPath(stroke, _labelPath);

                    using (Brush brush = new SolidBrush(_labelColor)) g.FillPath(brush, _labelPath);
                }
            }
            #endregion

            _canvasBuf.EndDraw();
        }
        
        private void EndView()
        {
            Debug.WriteLine("EndView");
            if (ShowPath)
            {
                var g = _canvasBuf.BeginDraw();

                _gPath.Widen(_dirtyMarkerPen);
                g.SetClip(_gPath);
                g.Clear(Color.Transparent);
                _canvasBuf.EndDraw();

                var pathDirty = Rectangle.Ceiling(_gPath.GetBounds());
                pathDirty.Offset(_screenBounds.X, _screenBounds.Y);
                pathDirty.Intersect(_screenBounds);
                pathDirty.Offset(-_screenBounds.X, -_screenBounds.Y); //挪回来变为基于窗口的坐标

                _canvasWindow.SetDiBitmap(_canvasBuf, pathDirty);
                    
                _gPath.Reset();
                _gPathDirty.Reset();
            }

            if (_labelVisible)
            {
                var g = _canvasBuf.BeginDraw();
                g.SetClip(_labelRect);
                g.Clear(Color.Transparent);
                _canvasBuf.EndDraw();
                _canvasWindow.SetDiBitmap(_canvasBuf, Rectangle.Ceiling(_labelRect));
                _labelPath.Reset();
                    
                HideLabel();
                _labelChanged = false;
            }                   

            _canvasWindow.Visible = false;
            _labelRect = default(Rectangle);//todo: dirtyRect也是多余
        }

        #region timer handlers
        private void FadeOut()
        {
            _fadeOuTimer.Enabled = true;
        }

        private void StopFadeout()
        {
            //终止fadeout
            if (_fadeOuTimer.Enabled)
            {
                lock (_fadeOuTimer)
                {
                    if (_fadeOuTimer.Enabled)
                    {
                        _fadeOuTimer.Enabled = false;
                        EndView();
                    }
                }
            }
        }

        private void OnFadeOutTimerElapsed(object o, ElapsedEventArgs e)
        {
            lock (_fadeOuTimer)
            {
                if (!_fadeOuTimer.Enabled) return;

                Debug.Write("*");

                //利用溢出来判断是否小于_fadeOutTo了
                var before = _canvasOpacity;
                _canvasOpacity -= FadeOutDelta;

                if (before < _canvasOpacity)
                {                    
                    EndView();
                    _fadeOuTimer.Enabled = false;
                }
                else
                {
                    _canvasWindow.SetDiBitmap(_canvasBuf, Rectangle.Ceiling(_labelRect), _canvasOpacity);
                    _canvasWindow.SetDiBitmap(_canvasBuf, Rectangle.Ceiling(_gPathDirty.GetBounds()), _canvasOpacity);
                }
            }
        }
        #endregion
        
        private void HideLabel()
        {
            _labelText = null;
            _labelVisible = false;
            _labelChanged = true;
        }

        private void ShowLabel(Color color, string text, Color bgColor)
        {
            _labelVisible = true;
            _labelColor = color;
            _labelText = text;
            _labelBgColor = bgColor;
            _labelChanged = true;

            _lastLabelRect = _labelRect;

            _labelPath.Reset();
            var msgPos = new PointF(_screenBounds.Width / 2, (_screenBounds.Height / 2) + _screenBounds.Width / 8);
            
            _labelPath.AddString(_labelText, _labelFont.FontFamily, 0, _labelFont.Size * _dpiFactor, msgPos, StringFormat.GenericDefault);
            _labelRect = _labelPath.GetBounds();
            msgPos.X -= _labelRect.Width / 2;

            _labelPath.Reset();
            _labelPath.AddString(_labelText, _labelFont.FontFamily, 0, _labelFont.Size * _dpiFactor, msgPos, StringFormat.GenericDefault);

            _labelRect = RectangleF.Inflate(_labelPath.GetBounds(), 25 * _dpiFactor, 15 * _dpiFactor);
        }

        private void RegisterEventHandlers()
        {
            _gestureParser.IntentRecognized += HandleIntentRecognized;
            _gestureParser.IntentInvalid += HandleIntentInvalid;
            _gestureParser.IntentOrPathCanceled += HandleIntentOrPathCanceled;
            _gestureParser.IntentReadyToExecute += HandleIntentReadyToExecute;
            _gestureParser.IntentReadyToExecuteOnModifier += HandleIntentReadyToExecuteOnModifier;

            _gestureParser.PathTracker.PathStart += HandlePathStart;
            _gestureParser.PathTracker.PathGrow += HandlePathGrow;
            _gestureParser.PathTracker.PathTimeout += HandlePathTimeout;

            _gestureParser.GestureCaptured += HandleGestureRecorded;
            _gestureParser.CommandReportStatus += HandleCommandReportStatus;
        }


        #region Util
       /* private static void AddRoundedRectangle(GraphicsPath path, RectangleF bounds, int cornerRadius)
        {
            path.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
        }*/

        private void DrawRoundedRectangle(Graphics gfx, RectangleF Bounds, int CornerRadius, Pen DrawPen, Color FillColor)
        {
            int strokeOffset = Convert.ToInt32(Math.Ceiling(DrawPen.Width));
            
            var rect = Rectangle.Truncate(Bounds);

            rect.Inflate(-strokeOffset, -strokeOffset);

            using (var brush = new SolidBrush(FillColor))
            {
                //gfx.DrawRectangle(_shadowPen, Rectangle.Truncate(Bounds));

                gfx.FillRectangle(brush, rect);
                gfx.DrawRectangle(DrawPen, rect);
            }
                

            /*using (var gfxPath = new GraphicsPath())
            {
                gfxPath.AddArc(Bounds.X, Bounds.Y, CornerRadius, CornerRadius, 180, 90);
                gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y, CornerRadius, CornerRadius, 270, 90);
                gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                gfxPath.AddArc(Bounds.X, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                gfxPath.CloseAllFigures();

                using (var sb = new SolidBrush(FillColor)) gfx.FillPath(sb, gfxPath);
                gfx.DrawPath(DrawPen, gfxPath);
            }*/
        }
        #endregion

        public void Dispose()
        {

            if (IsDisposed) return;

            #region unregistor events
            _gestureParser.IntentRecognized -= HandleIntentRecognized;
            _gestureParser.IntentInvalid -= HandleIntentInvalid;
            _gestureParser.IntentOrPathCanceled -= HandleIntentOrPathCanceled;

            _gestureParser.PathTracker.PathStart -= HandlePathStart;
            _gestureParser.PathTracker.PathGrow -= HandlePathGrow;
            _gestureParser.PathTracker.PathTimeout -= HandlePathTimeout;
            _gestureParser.IntentReadyToExecute -= HandleIntentReadyToExecute;
            _gestureParser.IntentReadyToExecuteOnModifier -= HandleIntentReadyToExecuteOnModifier;

            _gestureParser.GestureCaptured -= HandleGestureRecorded;
            _gestureParser.CommandReportStatus -= HandleCommandReportStatus;

            #endregion

            #region dispose pens
            _mainPen.Dispose();
            _middleBtnPen.Dispose();
            _borderPen.Dispose();
            _alternativePen.Dispose();

            _shadowPen.Dispose();
            
            _shadowPen = null;
           

            _dirtyMarkerPen.Dispose();
            #endregion

            if (_canvasBuf != null)
            {
                _canvasBuf.Dispose();
                _canvasBuf = null;
            }
            if (_canvasWindow != null)
            {
                _canvasWindow.Dispose();
                _canvasWindow = null;
            }
            if (_gPath != null)
            {
                _gPath.Dispose();
                _gPath = null;
            }
            if (_gPathDirty != null)
            {
                _gPathDirty.Dispose();
                _gPathDirty = null;
            }

            if (_labelPath != null)
            {
                _labelPath.Dispose();
                _labelPath = null;
            }
            if (_labelFont != null)
            {
                _labelFont.Dispose();
                _labelFont = null;
            }

            if (_fadeOuTimer != null)
            {
                _fadeOuTimer.Elapsed -= OnFadeOutTimerElapsed;
                _fadeOuTimer.Dispose();
                _fadeOuTimer = null;
            }

            SystemEvents.DisplaySettingsChanged -= SystemDisplaySettingsChanged;


            Debug.WriteLine("Dispose");
            IsDisposed = true;

        }


    }
}
