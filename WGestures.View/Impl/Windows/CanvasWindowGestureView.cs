using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
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
        public float PathShadowWidth
        {
            get { return _shadowPen.Width; }
            set { _shadowPen.Width = value; }
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
        Pen _mainPen;
        Pen _alternativePen;
        Pen _borderPen;
        Pen _shadowPen;

        readonly GestureParser _gestureParser;

        Rectangle _screenBounds = Native.GetScreenBounds();
        float _dpiFactor = Native.GetScreenDpi() / 96.0f;
        int _pathMaxPointCount;

        CanvasWindow _canvasWindow;
        DiBitmap _bitmap;

        Point _prevPoint;
        Rectangle _pathDirtyRect;
        Region _pathDirtyRegion = new Region();
        GraphicsPath _gPath = new GraphicsPath();
        Pen _pathPen;
        bool _pathVisible;

        RectangleF _labelDirtyRect;
        RectangleF _labelLastDirtyRect;
        bool _labelVisible;
        string _labelText;
        Color _labelColor;
        Color _labelBgColor;
        bool _labelChanged;
        GraphicsPath _labelPath = new GraphicsPath();
        Font _labelFont = new Font("微软雅黑", 32);

        bool _isCurrentRecognized;
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
            }) { Name = "CanvasWindow" }.Start();

            waitCanvasWindow.WaitOne();

            InitDefaultProperties();
            _fadeOuTimer.Elapsed += OnFadeOutTimerElapsed;
            SystemEvents.DisplaySettingsChanged += SystemDisplaySettingsChanged;
        }

        private void SystemDisplaySettingsChanged(object sender, EventArgs e)
        {
            _screenBounds = Native.GetScreenBounds();
            _dpiFactor = Native.GetScreenDpi() / 96.0f;
        }

        private void InitDefaultProperties()
        {
            //defaults
            ShowCommandName = true;
            ShowPath = true;
            ViewFadeOut = true;

            _pathMaxPointCount = (int)(256 * _dpiFactor);

            const float widthBase = 2f;

            #region init pens
            _mainPen = new Pen(Color.FromArgb(255, 50, 200, 100), widthBase * _dpiFactor) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            _middleBtnPen = new Pen(Color.FromArgb(255, 20, 150, 200), widthBase * _dpiFactor) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            _borderPen = new Pen(Color.FromArgb(220, 255, 255, 255), (widthBase + 2.8f) * _dpiFactor) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            _alternativePen = new Pen(Color.FromArgb(255, 255, 120, 20), widthBase * _dpiFactor) { EndCap = LineCap.Round, StartCap = LineCap.Round };
            _shadowPen = new Pen(Color.FromArgb(30, 0, 0, 0), (widthBase + 5f) * _dpiFactor) { EndCap = LineCap.Round, StartCap = LineCap.Round };

            #endregion
        }

        #region event handlers
        private void WhenPathStart(PathEventArgs args)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("WhenPathStart");

            var activeScreen = Screen.FromPoint(args.Location);
            _screenBounds = activeScreen.Bounds;
           
            _prevPoint = args.Location;//ToUpLeftCoord(args.Location);
            _pointCount = 1;

            _tempMainPen = args.Button == GestureButtons.RightButton ? _mainPen : _middleBtnPen;

            BeginView();
        }

        private void WhenPathGrow(PathEventArgs args)
        {
            if (!ShowPath && !ShowCommandName) return;
            if (_pointCount > _pathMaxPointCount) return;

            _pointCount++;

            if (_pointCount == _pathMaxPointCount)
            {
                ShowLabel(Color.White, "您是有多无聊啊 :)", Color.FromArgb(128, 255, 0, 0));

                DrawAndUpdate();
                _labelChanged = false;
                return;
            }

            if (ShowPath)
            {
                var curPos = args.Location;//ToUpLeftCoord(args.Location);
                
                //需要将点换算为基于窗口的坐标
                _gPath.AddLine(new Point(_prevPoint.X - _screenBounds.X, _prevPoint.Y - _screenBounds.Y), new Point(curPos.X - _screenBounds.X, curPos.Y - _screenBounds.Y));

                //_gPath.PathPoints.

                _pathDirtyRect = GetDirtyRect(_prevPoint, curPos);
                _pathDirtyRegion.Union(_pathDirtyRect);
            }

            DrawAndUpdate();

            _prevPoint = args.Location;//ToUpLeftCoord(args.Location);
        }

        private void WhenIntentRecognized(GestureIntent intent)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("IntentRecognized");

            if (ShowCommandName)
            {
                var modifierText = intent.Gesture.Modifier.ToMnemonic();
                var newLabelText = (modifierText == String.Empty ? String.Empty : (modifierText + " ")) + intent.Name;
                ShowLabel(Color.White, newLabelText, Color.FromArgb(70, 0, 0, 0));
            }

            if (!_isCurrentRecognized && ShowPath)
            {
                _isCurrentRecognized = true;
                _pathPen = _tempMainPen;
                ResetPathDirtyRect();
            }

            DrawAndUpdate(true);

            if (ShowCommandName) _labelChanged = false;
        }

        private void WhenIntentInvalid()
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
                ResetPathDirtyRect();
            }

            //draw
            DrawAndUpdate(true);

            //clear
            if (ShowCommandName)
            {
                _labelDirtyRect = default(Rectangle);
                _labelChanged = false;
            }
        }

        private void WhenPathTimeout(PathEventArgs args)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("PathTimeout");

            if (ShowPath) ResetPathDirtyRect();
            EndView();
        }

        private void WhenIntentReadyToExecute(GestureIntent intent)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("WhenIntentReadyToExecute");

            //update
            if (ShowPath) ResetPathDirtyRect();
            if (ShowCommandName)// && !_gestureParser.PathTracker.IsSuspended
            {
                var modifierText = intent.Gesture.Modifier.ToMnemonic();
                var newLabelText = (modifierText == String.Empty ? String.Empty : (modifierText + " ")) + intent.Name;
                ShowLabel(Color.White, newLabelText, Color.FromArgb(120, 0, 0, 0));
            }

            //draw
            DrawAndUpdate();

            //clear
            if (ShowPath) _pathVisible = false;
            if (ShowCommandName) _labelChanged = false;

            if (ViewFadeOut) FadeOut();
            else EndView();
        }

        private void WhenIntentReadyToExecuteOnModifier(GestureModifier modifier)
        {
//            if (ViewFadeOut)
//            {
//                _canvasOpacity = 255;
//                FadeOutTo(120);
//
//                DrawAndUpdate();
//            }

        }

        private void WhenIntentOrPathCanceled()
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("IntentOrPathCancled");

            EndView();
        }

        private void WhenGestureCaptured(Gesture g)
        {
            if (!ShowPath && !ShowCommandName) return;

            Debug.WriteLine("WhenGestureCaptured");

            EndView();
        }

        #endregion

        private void DrawAndUpdate(bool altPath = false)
        {
            #region
            var labelDirtyRect = _labelDirtyRect.Width > _labelLastDirtyRect.Width ? _labelDirtyRect : _labelLastDirtyRect;
            labelDirtyRect.Height = _labelDirtyRect.Height > _labelLastDirtyRect.Height ? _labelDirtyRect.Height : _labelLastDirtyRect.Height;
            #endregion

            _bitmap.DrawWith(g =>
            {
                //如果是识别与未识别之间转换，则使用region而非dirtyRect来重绘
                if (altPath) g.Clip = _pathDirtyRegion;
                else g.SetClip(_pathDirtyRect);


                if (_labelChanged) g.SetClip(labelDirtyRect, CombineMode.Union);

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
                if (ShowCommandName && _labelVisible)
                {
                    var labelDirty = Rectangle.Ceiling(_labelDirtyRect);
                    var intercected = _pathDirtyRect.IntersectsWith(labelDirty);

                    //如果Label的内容改变，则整个重绘
                    //不然，就判断哪些区域收到了path的影响，然只更新受影响的一小部分。
                    if (_labelChanged) g.SetClip(labelDirty);
                    else if (!intercected) return;
                    else if (!_pathDirtyRect.Contains(labelDirty))
                    {
                        labelDirty.Intersect(_pathDirtyRect);
                        g.SetClip(labelDirty);
                    }

                    using (var pen = new Pen(Color.White, 1.5f * _dpiFactor))
                    using (var shadow = new Pen(Color.FromArgb(40, 0, 0, 0), 3f * _dpiFactor))
                    {
                        DrawRoundedRectangle(g, RectangleF.Inflate(_labelDirtyRect,
                            -1f * _dpiFactor, -1f * _dpiFactor),
                            (int)(12 * _dpiFactor), shadow, Color.Transparent);
                        DrawRoundedRectangle(g, RectangleF.Inflate(_labelDirtyRect,
                            -2.6f * _dpiFactor, -2.6f * _dpiFactor),
                            (int)(12 * _dpiFactor), pen, _labelBgColor);

                        if (_labelColor != Color.White)
                            using (var stroke = new Pen(Color.White, 1.5f * _dpiFactor))
                                g.DrawPath(stroke, _labelPath);

                        using (Brush brush = new SolidBrush(_labelColor)) g.FillPath(brush, _labelPath);
                    }
                }
                #endregion

            });

            #region 3) 更新到窗口上
            if (ShowPath) _canvasWindow.SetDiBitmap(_bitmap, _pathDirtyRect);

            if (ShowCommandName)
            {
                var labelDirty = Rectangle.Ceiling(labelDirtyRect);
                var intercected = _pathDirtyRect.IntersectsWith(labelDirty);

                if (_labelChanged)
                {
                    _canvasWindow.SetDiBitmap(_bitmap, labelDirty);
                }
                else if (intercected && !_pathDirtyRect.Contains(labelDirty))
                {
                    labelDirty.Intersect(_pathDirtyRect);
                    _canvasWindow.SetDiBitmap(_bitmap, labelDirty);
                }

            }
            #endregion
        }

        private void BeginView()
        {
            Debug.WriteLine("BeginView");
            StopFadeout();

            _bitmap = new DiBitmap(_screenBounds.Size);
            _isCurrentRecognized = false;
            _canvasOpacity = 255;
            _canvasWindow.Bounds = _screenBounds;

            _canvasWindow.TopMost = true;
            _canvasWindow.Visible = true;


            _labelVisible = false;
            _pathVisible = true;
            _pathPen = _alternativePen;
        }

        private void EndView()
        {
            Debug.WriteLine("EndView");
            using (_bitmap)
            {
                if (ShowPath)
                {
                    ResetPathDirtyRect();
                    _bitmap.DrawWith(g =>
                    {
                        //这里暂时仍用dirtyRect，因为Region存在转角残留的问题。
                        g.SetClip(_pathDirtyRect);
                        g.Clear(Color.Transparent);
                    });

                    _canvasWindow.SetDiBitmap(_bitmap, _pathDirtyRect);
                }

                if (ShowCommandName)
                {
                    _bitmap.DrawWith(g =>
                    {
                        g.SetClip(_labelDirtyRect);
                        g.Clear(Color.Transparent);
                    });

                    _canvasWindow.SetDiBitmap(_bitmap, Rectangle.Ceiling(_labelDirtyRect));
                }
            }
            _bitmap = null;

            _canvasWindow.Visible = false;
            _gPath.Reset();
            _pathDirtyRegion.MakeEmpty();
            _labelPath.Reset();
            _labelText = null;

            _pathDirtyRect = default(Rectangle);
            _labelDirtyRect = default(Rectangle);
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
                    _canvasWindow.SetDiBitmap(_bitmap, Rectangle.Ceiling(_labelDirtyRect), _canvasOpacity);
                    _canvasWindow.SetDiBitmap(_bitmap, _pathDirtyRect, _canvasOpacity);
                }
            }
        }
        #endregion

        private void ResetPathDirtyRect()
        {
            var inflate = _shadowPen.Width * _dpiFactor;
            _pathDirtyRect = Rectangle.Ceiling(RectangleF.Inflate(_gPath.GetBounds(), inflate, inflate));
            //inflate操作可能使其坐标为负，从而导致updateLayeredWindow失败，结果路径无法擦出。
            _pathDirtyRect.Intersect(new Rectangle(0,0,_screenBounds.Width, _screenBounds.Height));
        }

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

            _labelLastDirtyRect = _labelDirtyRect;

            _labelPath.Reset();
            var msgPos = new PointF(_screenBounds.Width / 2, (_screenBounds.Height / 2) + _screenBounds.Width / 8);

            _labelPath.AddString(_labelText, _labelFont.FontFamily, 0, _labelFont.Size * _dpiFactor, msgPos, StringFormat.GenericDefault);
            _labelDirtyRect = _labelPath.GetBounds();
            msgPos.X -= _labelDirtyRect.Width / 2;

            _labelPath.Reset();
            _labelPath.AddString(_labelText, _labelFont.FontFamily, 0, _labelFont.Size * _dpiFactor, msgPos, StringFormat.GenericDefault);

            _labelDirtyRect = RectangleF.Inflate(_labelPath.GetBounds(), 25 * _dpiFactor, 15 * _dpiFactor);

        }

        private void RegisterEventHandlers()
        {
            _gestureParser.IntentRecognized += WhenIntentRecognized;
            _gestureParser.IntentInvalid += WhenIntentInvalid;
            _gestureParser.IntentOrPathCanceled += WhenIntentOrPathCanceled;
            _gestureParser.IntentReadyToExecute += WhenIntentReadyToExecute;
            _gestureParser.IntentReadyToExecuteOnModifier += WhenIntentReadyToExecuteOnModifier;

            _gestureParser.PathTracker.PathStart += WhenPathStart;
            _gestureParser.PathTracker.PathGrow += WhenPathGrow;
            _gestureParser.PathTracker.PathTimeout += WhenPathTimeout;

            _gestureParser.GestureCaptured += WhenGestureCaptured;
        }


        #region Util
        //获得以基于窗口坐标的dirtyRect
        private Rectangle GetDirtyRect(Point a, Point b)
        {
            var origin = a;
            if (origin.X > b.X) origin.X = b.X;
            if (origin.Y > b.Y) origin.Y = b.Y;

            var ret = new Rectangle(origin.X - (int)(4 * _dpiFactor) - _screenBounds.X, 
                (origin.Y - 4) - _screenBounds.Y,
                Math.Abs(b.X - a.X) + (int)(8 * _dpiFactor), 
                Math.Abs(b.Y - a.Y) + (int)(8 * _dpiFactor));

            ret.Intersect(new Rectangle(0, 0, _screenBounds.Width, _screenBounds.Height));

            return ret;
        }

        private void DrawRoundedRectangle(Graphics gfx, RectangleF Bounds, int CornerRadius, Pen DrawPen, Color FillColor)
        {
            int strokeOffset = Convert.ToInt32(Math.Ceiling(DrawPen.Width));
            Bounds = RectangleF.Inflate(Bounds, -strokeOffset, -strokeOffset);

            using (var gfxPath = new GraphicsPath())
            {
                gfxPath.AddArc(Bounds.X, Bounds.Y, CornerRadius, CornerRadius, 180, 90);
                gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y, CornerRadius, CornerRadius, 270, 90);
                gfxPath.AddArc(Bounds.X + Bounds.Width - CornerRadius, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                gfxPath.AddArc(Bounds.X, Bounds.Y + Bounds.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                gfxPath.CloseAllFigures();

                using (var sb = new SolidBrush(FillColor)) gfx.FillPath(sb, gfxPath);
                gfx.DrawPath(DrawPen, gfxPath);
            }
        }
        #endregion

        public void Dispose()
        {

            if (IsDisposed) return;

            #region unregistor events
            _gestureParser.IntentRecognized -= WhenIntentRecognized;
            _gestureParser.IntentInvalid -= WhenIntentInvalid;
            _gestureParser.IntentOrPathCanceled -= WhenIntentOrPathCanceled;

            _gestureParser.PathTracker.PathStart -= WhenPathStart;
            _gestureParser.PathTracker.PathGrow -= WhenPathGrow;
            _gestureParser.PathTracker.PathTimeout -= WhenPathTimeout;
            _gestureParser.IntentReadyToExecute -= WhenIntentReadyToExecute;
            _gestureParser.IntentReadyToExecuteOnModifier -= WhenIntentReadyToExecuteOnModifier;

            _gestureParser.GestureCaptured -= WhenGestureCaptured;
            #endregion

            #region dispose pens
            _mainPen.Dispose();
            _middleBtnPen.Dispose();
            _borderPen.Dispose();
            _alternativePen.Dispose();
            _shadowPen.Dispose();
            #endregion

            if (_bitmap != null)
            {
                _bitmap.Dispose();
                _bitmap = null;
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
            if (_pathDirtyRegion != null)
            {
                _pathDirtyRegion.Dispose();
                _pathDirtyRegion = null;
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
