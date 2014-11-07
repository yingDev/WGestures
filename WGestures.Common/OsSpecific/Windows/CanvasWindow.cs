using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Win32;

namespace WGestures.Common.OsSpecific.Windows
{
    public class CanvasWindow : IDisposable
    {
        private const int WS_EX_NOACTIVATE = 0x8000000;
        private const int WS_EX_TRANSPARENT = 0x00000020;


        public bool IsDisposed { get; private set; }

        public bool NoActivate
        {
            get
            {
                int exStyle = User32.GetWindowLong(Handle, User32.GWL.GWL_EXSTYLE);
                return (exStyle & WS_EX_NOACTIVATE) != 0;
            }
            set
            {
                int exStyle = User32.GetWindowLong(Handle, User32.GWL.GWL_EXSTYLE);
                if (value)
                {
                    exStyle |= WS_EX_NOACTIVATE;
                }
                else
                {
                    exStyle &= ~WS_EX_NOACTIVATE;
                }
                User32.SetWindowLong(Handle, User32.GWL.GWL_EXSTYLE, exStyle);

                ApplySetWindowLong();
            }
        }

        public bool Visible
        {
            get
            {
                var style = User32.GetWindowLong(Handle, User32.GWL.GWL_STYLE);
                return (style & (int)User32.WS.WS_VISIBLE) != 0;
            }
            set
            {
                int style = User32.GetWindowLong(Handle, User32.GWL.GWL_STYLE);
                if (value)
                {
                    style |= (int)User32.WS.WS_VISIBLE;
                }
                else
                {
                    style &= ~(int)User32.WS.WS_VISIBLE;
                }
                User32.SetWindowLong(Handle, User32.GWL.GWL_STYLE, style);
                ApplySetWindowLong();
                
                //User32.ShowWindow(this.Handle, value ? User32.SW.SW_SHOWNOACTIVATE : User32.SW.SW_HIDE);

            }
        }
        public bool IgnoreInput
        {
            get
            {
                int exStyle = User32.GetWindowLong(Handle, User32.GWL.GWL_EXSTYLE);
                return (exStyle & WS_EX_TRANSPARENT) != 0;
            }

            set
            {
                int exStyle = User32.GetWindowLong(Handle, User32.GWL.GWL_EXSTYLE);

                if (value)
                {
                    exStyle |= WS_EX_TRANSPARENT;
                }
                else
                {
                    exStyle &= ~WS_EX_TRANSPARENT;
                }
                User32.SetWindowLong(Handle, User32.GWL.GWL_EXSTYLE, exStyle);
                ApplySetWindowLong();
            }

        }
        public Rectangle Bounds
        {
            get
            {
                GDI32.RECT bounds;
                User32.GetWindowRect(Handle, out bounds);
                _bounds = new Rectangle(bounds.Left, bounds.Top, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
                return _bounds;
            }
            set
            {
                User32.SetWindowPos(Handle, IntPtr.Zero, value.X, value.Y, value.Width, value.Height, User32.SWP.SWP_NOZORDER);
                _bounds = value;
            }
        }
        public bool TopMost
        {
            set
            {
                if (value)
                {
                    User32.SetWindowPos(Handle, new IntPtr(-1), 0, 0, 0, 0, User32.SWP.SWP_NOMOVE | User32.SWP.SWP_NOSIZE);
                }
                else
                {
                    User32.SetWindowPos(Handle, new IntPtr(-2), 0, 0, 0, 0, User32.SWP.SWP_NOMOVE | User32.SWP.SWP_NOSIZE);
                }
            }
        }
        public IntPtr Handle
        {
            get
            {
                if (_handle == IntPtr.Zero) throw new InvalidOperationException("窗口已经销毁或未创建");
                return _handle;
            }
        }

        private User32.WndProc _wndProc;

        private Rectangle _bounds;
        private IntPtr _handle;


        public CanvasWindow()
        {
            _bounds = new Rectangle(0, 0, 400, 300);
            _wndProc = wndProc;
            CreateWindow();
            //NoActivate = true;
            //IgnoreInput = true;
            
        }


        public virtual void Show()
        {
            if (Handle == IntPtr.Zero) throw new Exception("窗口尚未创建");

            Visible = true;
            //Win32.User32.ShowWindow(Handle, User32.SW.SW_SHOWNOACTIVATE);
            Win32.User32.UpdateWindow(Handle);
        }

        public void ShowDialog()
        {
            Show();

            var msg = new User32.MSG();
            while (Win32.User32.GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                Win32.User32.TranslateMessage(ref msg);
                Win32.User32.DispatchMessage(ref msg);
            }
        }

        public virtual void Close()
        {
            User32.SendMessage(Handle, User32.WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _handle = IntPtr.Zero;
        }

        public void SetDiBitmap(DiBitmap bmp, Rectangle dirtyRect)
        {
            SetDiBitmap(bmp,dirtyRect,255);
        }
        public void SetDiBitmap(DiBitmap bmp)
        {
            SetHBitmap(bmp.HBitmap, _bounds, new Point(), new Rectangle(new Point(), bmp.Size), 255);
        }
        public void SetDiBitmap(DiBitmap bmp, Rectangle dirtyRect, byte opacity)
        {
            SetHBitmap(bmp.HBitmap, _bounds, new Point(), dirtyRect, opacity);
        }
        public void SetDiBitmap(DiBitmap bmp, Point drawAt)
        {
            SetHBitmap(bmp.HBitmap, _bounds, drawAt, new Rectangle(0, 0, bmp.Size.Width, bmp.Size.Height), 255);
        }

        public void SetDiBitmap(DiBitmap bmp, Rectangle newWindowBounds, Point drawAt, Rectangle dirtyRect, byte opacity)
        {
            SetHBitmap(bmp.HBitmap, newWindowBounds, drawAt, new Rectangle(0, 0, _bounds.Width, _bounds.Height), opacity);

        }

        //dirtyRect是绝对坐标（在多屏的情况下）
        public virtual void SetHBitmap(IntPtr hBitmap, Rectangle newWindowBounds, Point drawAt, Rectangle dirtyRect,byte opacity)
        {
           // IntPtr screenDc = Win32.GDI32.GetDC(IntPtr.Zero);

            IntPtr memDc = Win32.GDI32.CreateCompatibleDC(IntPtr.Zero);
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                oldBitmap = Win32.GDI32.SelectObject(memDc, hBitmap);

                var winSize = new Native.Size(newWindowBounds.Width, newWindowBounds.Height);
                var winPos = new Native.Point(newWindowBounds.X, newWindowBounds.Y);

                var drawBmpAt = new Native.Point(drawAt.X, drawAt.Y);
                var blend = new Native.BLENDFUNCTION { BlendOp = GDI32.AC_SRC_OVER, BlendFlags = 0, SourceConstantAlpha = opacity, AlphaFormat = GDI32.AC_SRC_ALPHA };

                var updateInfo = new Native.UPDATELAYEREDWINDOWINFO();
                updateInfo.cbSize = (uint) Marshal.SizeOf(typeof (Native.UPDATELAYEREDWINDOWINFO));
                updateInfo.dwFlags = GDI32.ULW_ALPHA;
                updateInfo.hdcDst = IntPtr.Zero;//Native.GetDC(IntPtr.Zero);//IntPtr.Zero; //ScreenDC
                updateInfo.hdcSrc = memDc;

               // dirtyRect.X -= _bounds.X;
               // dirtyRect.Y -= _bounds.Y;
                    
                //dirtyRect.Offset(-_bounds.X, -_bounds.Y);
                var dirRect = new GDI32.RECT(dirtyRect.X,dirtyRect.Y,dirtyRect.Right,dirtyRect.Bottom);
                
                unsafe
                {
                    updateInfo.pblend = &blend;
                    updateInfo.pptDst = &winPos;
                    updateInfo.psize = &winSize;
                    updateInfo.pptSrc = &drawBmpAt;
                    updateInfo.prcDirty = &dirRect;
                }

                Native.UpdateLayeredWindowIndirect(Handle, ref updateInfo);
               // Debug.Assert(Native.GetLastError() == 0);

                //Native.UpdateLayeredWindow(Handle, IntPtr.Zero, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, GDI32.ULW_ALPHA);

            }
            finally
            {

                //GDI32.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    GDI32.SelectObject(memDc, oldBitmap);
                    //Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
                    //Win32.DeleteObject(hBitmap);
                }
                GDI32.DeleteDC(memDc);
            }
        }

        protected virtual IntPtr WndProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case (uint)User32.WM.WM_CLOSE:
                    User32.DestroyWindow(hwnd);
                    break;

                case (uint)User32.WM.WM_DESTROY:
                    User32.PostQuitMessage(0);
                    break;

                default:
                    return User32.DefWindowProc(hwnd, msg, wParam, lParam);
            }

            return IntPtr.Zero;
        }

        private void CreateWindow()
        {
            var wc = new User32.WNDCLASSEX();
            IntPtr hwnd;


            //1 register window class
            wc.cbSize = Marshal.SizeOf(wc);
            wc.style = User32.CS.CS_HREDRAW | User32.CS.CS_VREDRAW;
            wc.lpfnWndProc = _wndProc;
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;

            wc.hInstance = Kernel32.GetModuleHandle(null);//Kernel32.GetModuleHandle(null);
            wc.lpszClassName = "CanvasWindow";

            wc.hIcon = User32.LoadIcon(wc.hInstance, new IntPtr((uint)User32.IDI.IDI_APPLICATION));
            wc.hCursor = User32.LoadCursor(IntPtr.Zero, User32.IDC.IDC_ARROW);
            wc.hbrBackground = new IntPtr((uint)Win32.GDI32.COLOR.COLOR_WINDOW + 1);
            wc.lpszMenuName = "";
            wc.hIconSm = IntPtr.Zero;


            if (User32.RegisterClassEx(ref wc) == 0)
            {
               // throw new Exception("注册窗口类失败: " + Marshal.GetLastWin32Error());
            }

            hwnd = User32.CreateWindowEx(User32.WS_EX.WS_EX_LAYERED | User32.WS_EX.WS_EX_TOPMOST
                | User32.WS_EX.WS_EX_TOOLWINDOW, wc.lpszClassName, null,
                0, _bounds.Left, _bounds.Top,
                _bounds.Width, _bounds.Height, IntPtr.Zero, IntPtr.Zero, wc.hInstance, IntPtr.Zero);

            if (hwnd == IntPtr.Zero)
            {
                throw new Exception("创建窗口失败: " + Marshal.GetLastWin32Error());
            }

            _handle = hwnd;
        }

        private void ApplySetWindowLong()
        {
            User32.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, User32.SWP.SWP_NOMOVE | User32.SWP.SWP_NOSIZE
                | User32.SWP.SWP_NOZORDER | User32.SWP.SWP_FRAMECHANGED);
        }

        private IntPtr wndProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            //Console.WriteLine("event " + msg);
            return WndProc(hwnd, msg, wParam, lParam);
        }


        public void Dispose()
        {
            if(IsDisposed )return;
            
            Close();

            IsDisposed = true;
        }
    }
}
