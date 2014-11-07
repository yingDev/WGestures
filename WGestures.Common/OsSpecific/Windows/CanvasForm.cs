/*using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WGestures.Common.OsSpecific.Windows
{
    public class CanvasForm : Form
    {
        private bool _ignoreInput = true;
        private bool _noActivate = true;
        public bool IgnoreInput { get { return _ignoreInput; } }
        public bool NoActivate { get { return _noActivate; } }
       // Stopwatch w = new Stopwatch();

        public CanvasForm(bool ignoreInput = false, bool noActivate = false)
        {
            _ignoreInput = ignoreInput;
            _noActivate = noActivate;
            // This form should not have a border or else Windows will clip it.
            FormBorderStyle = FormBorderStyle.None;
            //StartPosition = FormStartPosition.Manual;
        }


        /// <para>Changes the current bitmap.</para>
        public void SetBitmap(Bitmap bitmap)
        {
            SetBitmap(bitmap,new Point(0,0),new Rectangle(0,0,bitmap.Width,bitmap.Height),  255);
        }

        public void SetBitmap(Bitmap bitmap, Point destPos, Rectangle srcBounds)
        {
            SetBitmap(bitmap,destPos,srcBounds,255);
        }

        /// <para>Changes the current bitmap with a custom opacity level.  Here is where all happens!</para>
        public void SetBitmap(Bitmap bitmap, Point destPos,Rectangle srcBounds,byte opacity)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

            //w.Start();
            IntPtr screenDc = Native.GetDC(IntPtr.Zero);
            
            IntPtr memDc = Native.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;
            //w.Stop();
           // Console.WriteLine("SetBitmap a:"+w.ElapsedMilliseconds);

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  // grab a GDI handle from this GDI+ bitmap

                oldBitmap = Native.SelectObject(memDc, hBitmap);

                var size = new Native.Size(srcBounds.Width, srcBounds.Height);
                var topPos = new Native.Point(destPos.X, destPos.Y);
                var pointSource = new Native.Point(srcBounds.X, srcBounds.Y);
                var blend = new Native.BLENDFUNCTION {BlendOp = Native.AC_SRC_OVER, BlendFlags = 0, SourceConstantAlpha = opacity, AlphaFormat = Native.AC_SRC_ALPHA};

                Native.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size,
                    memDc, ref pointSource, 0, ref blend, Native.ULW_ALPHA);
            }
            finally
            {
                Native.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    Native.SelectObject(memDc, oldBitmap);
                    //Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Native GDI and it's working fine without any resource leak.
                    Native.DeleteObject(hBitmap);
                }
                Native.DeleteDC(memDc);
            }
        }

        public void SetHBitmap(IntPtr hBitmap)
        {
            SetHBitmap(hBitmap,new Point(0,0),new Rectangle(0,0,Width,Height),  255);
        }

        public void SetHBitmap(IntPtr hBitmap, Point destPos,Rectangle srcBounds,byte opacity)
        {
            IntPtr screenDc = Native.GetDC(IntPtr.Zero);

            IntPtr memDc = Native.CreateCompatibleDC(IntPtr.Zero);
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                oldBitmap = Native.SelectObject(memDc, hBitmap);

                var size = new Native.Size(srcBounds.Width, srcBounds.Height);
                var topPos = new Native.Point(destPos.X, destPos.Y);
                var pointSource = new Native.Point(srcBounds.X, srcBounds.Y);
                var blend = new Native.BLENDFUNCTION { BlendOp = Native.AC_SRC_OVER, BlendFlags = 0, SourceConstantAlpha = opacity, AlphaFormat = Native.AC_SRC_ALPHA };
                
                Native.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size,memDc, ref pointSource, 0, ref blend, Native.ULW_ALPHA);
                
            }
            finally
            {
                
                Native.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    Native.SelectObject(memDc, oldBitmap);
                    //Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Native GDI and it's working fine without any resource leak.
                    //Native.DeleteObject(hBitmap);
                }
                Native.DeleteDC(memDc);
            }
        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // This form has to have the WS_EX_LAYERED extended style

                if (_noActivate) cp.ExStyle |= 0x8000000; // WS_EX_NOACTIVATE - requires Win 2000 or higher :)
                //关键， 防止被WindowFromPoint找到！！
                if (_ignoreInput) cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT, ignore mouse events, hide from WindowFromPoint
                return cp;
            }
        }
    }
}*/
