using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.Core.Impl.Windows
{

    class EdgeInteractDetector : IDisposable
    {
        public bool Paused { get; set; }

        MouseHook _hook;
        Rectangle _screenBounds;
        PointAndTime _hLastPtOutsideRedline;
        PointAndTime _vLastPtOutsideRedline;
        PointAndTime _collidePoint;
        float _currentBias;
        //bool _isCollideReset;
        ScreenEdge? _activeCollideEdge;
        ScreenEdge? _activeRubEdge;
        int _rubTimes;
        int _rubPeakPos;
        int _lastRubDir;
        DateTime _lastRubTriggerTime;
        
        float _dpiScale;
        

        //public ushort DoubleCollideThreshold { get; set; } = 250; //ms

        public event Action<ScreenEdge> Collide;
        public event Action<ScreenEdge> Rub;

        public EdgeInteractDetector(MouseHook mouseHook)
        {
            //_dpiScale = (Native.GetScreenDpi() / 96.0f);
            //_screenBounds = Native.GetScreenBounds();
            //Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

            _hook = mouseHook;
            _hook.MouseHookEvent += _hook_MouseHookEvent;

            _activeCollideEdge = null;

        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            //_dpiScale = (Native.GetScreenDpi() / 96.0f);
            //_screenBounds = Native.GetScreenBounds();
        }

        //Note: Hook runs on separate thread!
        private void _hook_MouseHookEvent(MouseHook.MouseHookEventArgs e)
        {
            if (Paused) return;

            if (_activeCollideEdge != null && e.Msg == MouseMsg.WM_LBUTTONUP || e.Msg == MouseMsg.WM_RBUTTONUP)
            {
                _activeCollideEdge = null;
                _hLastPtOutsideRedline = new PointAndTime();
                _vLastPtOutsideRedline = new PointAndTime();
            }

            if (e.Msg != MouseMsg.WM_MOUSEMOVE)
            {
                return;
            }

            if (Native.GetAsyncKeyState(System.Windows.Forms.Keys.LButton) < 0 
                || Native.GetAsyncKeyState(System.Windows.Forms.Keys.RButton) < 0) //LRButton
            {
                return;
            }

            var screenBounds = Common.OsSpecific.Windows.Screen.ScreenBoundsFromPoint(e.Pos);
            if (screenBounds == null) return;
            _screenBounds = screenBounds.Value;
            _dpiScale = (Native.GetScreenDpi() / 96.0f);

            DetectCollide(e);
            DetectRub(e);
        }


        private void DetectCollide(MouseHook.MouseHookEventArgs e)
        {
            //TODO: make configurable
            var redLineDist = 100 * _dpiScale;
            var minSpeed = 0.3 * _dpiScale;
            var maxAllowedBias = 100 * _dpiScale;
            var cornerExcludeDist = 100 * _dpiScale;
            var edgeThick = 4;

            var now = PointAndTime.RecordNow(e.Pos);

            //Debug.WriteLine("wtf " + e.Pos);
            if (_activeCollideEdge == null)
            {
                if (e.Pos.X >= redLineDist && e.Pos.X <= _screenBounds.Width - redLineDist)
                {
                    _hLastPtOutsideRedline = now;
                }
                if (e.Pos.Y >= redLineDist && e.Pos.Y <= _screenBounds.Height - redLineDist)
                {
                    _vLastPtOutsideRedline = now;
                }
                float bias;
                float speed;
                ScreenEdge side;

                if (e.Pos.X <= edgeThick && (e.Pos.Y < _screenBounds.Height - cornerExcludeDist && e.Pos.Y > cornerExcludeDist))
                {
                    bias = now.Pos.Y - _hLastPtOutsideRedline.Pos.Y;
                    speed = -PointAndTime.CalSpeedVec(_hLastPtOutsideRedline, now).X;
                    side = ScreenEdge.Left;
                }
                else if (e.Pos.X >= _screenBounds.Width - edgeThick && (e.Pos.Y < _screenBounds.Height - cornerExcludeDist && e.Pos.Y > cornerExcludeDist))
                {
                    bias = now.Pos.Y - _hLastPtOutsideRedline.Pos.Y;
                    speed = PointAndTime.CalSpeedVec(_hLastPtOutsideRedline, now).X;
                    side = ScreenEdge.Right;
                }
                else if (e.Pos.Y <= edgeThick && (e.Pos.X < _screenBounds.Width - cornerExcludeDist && e.Pos.X > cornerExcludeDist))
                {
                    bias = now.Pos.X - _vLastPtOutsideRedline.Pos.X;
                    speed = -PointAndTime.CalSpeedVec(_vLastPtOutsideRedline, now).Y;
                    side = ScreenEdge.Top;
                }
                else if (e.Pos.Y >= _screenBounds.Height - edgeThick && (e.Pos.X < _screenBounds.Width - cornerExcludeDist && e.Pos.X > cornerExcludeDist))
                {
                    bias = now.Pos.X - _vLastPtOutsideRedline.Pos.X;
                    speed = PointAndTime.CalSpeedVec(_vLastPtOutsideRedline, now).Y;
                    side = ScreenEdge.Bottom;
                }
                else
                {
                    return;
                }

                if (speed >= minSpeed && Math.Abs(bias) <= maxAllowedBias)
                {
                    //Debug.WriteLine("X  ");
                    _collidePoint = now;
                    _currentBias = Math.Abs(bias);
                    _activeCollideEdge = side;
                }
            }
            else
            {
                float speed = 0;
                float bias = 0;
                switch (_activeCollideEdge)
                {
                    case ScreenEdge.Left:
                        bias = Math.Abs(_hLastPtOutsideRedline.Pos.Y - now.Pos.Y);
                        if (bias > _currentBias) _currentBias = bias;
                        if (e.Pos.X >= redLineDist)
                        {
                            speed = PointAndTime.CalSpeedVec(_collidePoint, now).X;
                        }
                        else return;
                        break;
                    case ScreenEdge.Right:
                        bias = Math.Abs(_hLastPtOutsideRedline.Pos.Y - now.Pos.Y);
                        if (bias > _currentBias) _currentBias = bias;
                        if (e.Pos.X <= _screenBounds.Width - redLineDist)
                        {
                            speed = -PointAndTime.CalSpeedVec(_collidePoint, now).X;
                        }
                        else return;
                        break;
                    case ScreenEdge.Top:
                        bias = Math.Abs(_vLastPtOutsideRedline.Pos.X - now.Pos.X);
                        if (bias > _currentBias) _currentBias = bias;
                        if (e.Pos.Y >= redLineDist)

                        {
                            speed = PointAndTime.CalSpeedVec(_collidePoint, now).Y;
                        }
                        else return;
                        break;
                    case ScreenEdge.Bottom:
                        bias = Math.Abs(_vLastPtOutsideRedline.Pos.X - now.Pos.X);
                        if (bias > _currentBias) _currentBias = bias;
                        if (e.Pos.Y <= _screenBounds.Height - redLineDist)
                        {
                            speed = -PointAndTime.CalSpeedVec(_collidePoint, now).Y;
                        }
                        else return;
                        break;
                }


                if (speed >= minSpeed && _currentBias <= maxAllowedBias)
                {
                    Debug.WriteLine("O  " + _currentBias);
                    OnCollide(_activeCollideEdge.Value);
                }
                _activeCollideEdge = null; //Reset
            }
        }

        private void DetectRub(MouseHook.MouseHookEventArgs e)
        {
            //todo: should be cofigurable
            var MIN_MOVE = 80;
            var RED_LINE_DIST = 50;
            var TRIGGER_INTERVAL = 1500;
            var EDGE_THICK = (int)(16 * _dpiScale);
            var MOVES_REQUIERED = 4;

            if (_activeRubEdge == null)
            {
                _activeRubEdge = GetActiveEdge(e.Pos, EDGE_THICK);
                if (_activeRubEdge == null) return;

                _lastRubTriggerTime = DateTime.UtcNow;
                _rubPeakPos = GetPosOnEdge(_activeRubEdge.Value, e.Pos);
                _rubTimes = 0;
                _lastRubDir = 0;
            }
            else
            {
                if (_rubTimes >= MOVES_REQUIERED)
                {
                    if (GetDistToEdge(_activeRubEdge.Value, e.Pos) >= RED_LINE_DIST || DateTime.UtcNow - _lastRubTriggerTime > TimeSpan.FromMilliseconds(TRIGGER_INTERVAL))
                    {
                        _activeRubEdge = null;
                    }

                    return;
                }

                if (GetActiveEdge(e.Pos, EDGE_THICK) != _activeRubEdge)
                {
                    _activeRubEdge = null;
                    return;
                }

                var pos = GetPosOnEdge(_activeRubEdge.Value, e.Pos);

                var dist = pos - _rubPeakPos;
                if (Math.Sign(dist) == Math.Sign(_lastRubDir) && _lastRubDir != 0)
                {
                    _rubPeakPos = pos;
                    return;
                }

                if (Math.Abs(dist) >= MIN_MOVE)
                {
                    var now = DateTime.UtcNow;
                    if (now - _lastRubTriggerTime > TimeSpan.FromMilliseconds(600))
                    {
                        _activeRubEdge = null;
                        return;
                    }

                    _lastRubTriggerTime = now;


                    _rubTimes += 1;
                    _rubPeakPos = pos;
                    _lastRubDir = Math.Sign(dist);

                    Debug.WriteLine("Rub: " + _rubTimes);

                    if (_rubTimes >= MOVES_REQUIERED)
                    {
                        Debug.WriteLine("Rub: Trigger!");

                        OnRub(_activeRubEdge.Value);
                    }

                }
            }
        }


        private ScreenEdge? GetActiveEdge(Point p, int edgeThick = 4, int cornerExcludeDist = 100)
        {
            var x = p.X;
            var y = p.Y;
            var scr = _screenBounds;
            var cor = cornerExcludeDist;
            var et = edgeThick;


            if (x <= et && (y < scr.Height - cor && y > cor))
            {
                return ScreenEdge.Left;
            }
            else if (x >= scr.Width - et && (y < scr.Height - cor && y > cor))
            {
                return ScreenEdge.Right;
            }
            else if (y <= et && (x < scr.Width - cor && x > cor))
            {
                return ScreenEdge.Top;
            }
            else if (y >= scr.Height - et && (x < scr.Width - cor && x > cor))
            {
                return ScreenEdge.Bottom;
            }
            else
            {
                return null;
            }
        }

        private int GetPosOnEdge(ScreenEdge e, Point p)
        {
            switch (e)
            {
                case ScreenEdge.Left:
                case ScreenEdge.Right:
                    return p.Y;

                default:
                    return p.X;
            }
        }

        private int GetDistToEdge(ScreenEdge e, Point p)
        {
            switch (e)
            {
                case ScreenEdge.Left:
                    return p.X;
                case ScreenEdge.Right:
                    return _screenBounds.Width - p.X;
                case ScreenEdge.Top:
                    return p.Y;
                default:
                    return _screenBounds.Bottom - p.Y;
            }
        }


        private void OnCollide(ScreenEdge edge)
        {
            if (Collide != null) Collide(edge);
            
        }

        private void OnRub(ScreenEdge e)
        {
            if (Rub != null) Rub(e);
            
        }


        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _hook.MouseHookEvent -= _hook_MouseHookEvent;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        struct PointAndTime
        {
            private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public Point Pos;
            public long Time;

            public PointAndTime(Point p, long t)
            {
                Time = t;
                Pos = p;
            }

            public PointAndTime(Point p, DateTime t)
            {
                Pos = p;
                Time = (long)(t - Jan1st1970).TotalMilliseconds;
            }

            public static long Now
            {
                get
                {
                    return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
                }
            }

            public static PointAndTime RecordNow(Point pt)
            {
                return new PointAndTime(pt, Now);
            }

            public static PointF CalSpeedVec(PointAndTime a, PointAndTime b)
            {
                long delta;
                if (a.Time > b.Time)
                {
                    delta = a.Time - b.Time;
                }
                else
                {
                    delta = b.Time - a.Time;
                }

                if (delta > 0)
                {
                    var dx = (float)(b.Pos.X - a.Pos.X);
                    var dy = (float)(b.Pos.Y - a.Pos.Y);
                    return new PointF(dx / delta, dy / delta);
                }

                return default(PointF);
            }
        }
    }
    

}
