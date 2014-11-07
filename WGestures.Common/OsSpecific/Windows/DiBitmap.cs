using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WGestures.Common.OsSpecific.Windows
{
    /// <summary>
    /// 对GDI hBitmap的封装。主要提供DrawWith方法以便画图。通过HBitmap属性方位原始hBitmap
    /// </summary>
    public class DiBitmap : IDisposable
    {
        public delegate void DrawingHandler(Graphics g);

        public IntPtr HBitmap { get; private set; }
        public IntPtr PointerToBits { get; private set; }

        public Size Size { get; private set; }
        public PixelFormat PixelFormat { get; private set; }

        private uint _errCode = 0;
        private IntPtr _memDc;
        private IntPtr _oldObject;
        private Graphics _graphics;

        public DiBitmap(Size size, PixelFormat pixelFormat = PixelFormat.Format32bppArgb)
        {
            Size = size;
            PixelFormat = pixelFormat;

            var binfo = new Native.BITMAPINFO();
            binfo.bmiHeader.biSize = Marshal.SizeOf(typeof(Native.BITMAPINFOHEADER));
            binfo.bmiHeader.biWidth = Size.Width;
            binfo.bmiHeader.biHeight = Size.Height;
            binfo.bmiHeader.biBitCount = (short)Image.GetPixelFormatSize(PixelFormat);
            binfo.bmiHeader.biPlanes = 1;
            binfo.bmiHeader.biCompression = 0;
            //var screenDc = Native.GetDC(IntPtr.Zero);
            //var memDc = Native.CreateCompatibleDC(IntPtr.Zero);
            //if (memDc == IntPtr.Zero) throw new ApplicationException("初始化失败：创建MemDc失败(" + Native.GetLastError() + ")");

            try
            {
                _memDc = Native.CreateCompatibleDC(IntPtr.Zero);
                if (_memDc == IntPtr.Zero)
                    throw new ApplicationException("CreateCompatibleDC(IntPtr.Zero)失败(" + Native.GetLastError() + ")");

                IntPtr ptrBits = IntPtr.Zero;
                HBitmap = Native.CreateDIBSection(_memDc, ref binfo, 0, out ptrBits, IntPtr.Zero, 0);

                //HBitmap = Native.CreateCompatibleBitmap(MemDc, size.Width, size.Height);
                PointerToBits = ptrBits;
                if (HBitmap == IntPtr.Zero)
                    throw new ApplicationException("初始化失败：CreateDIBSection(...)失败(" + Native.GetLastError() + ")");
            }
            finally
            {
                //Native.DeleteDC(memDc);
                // Native.ReleaseDC(IntPtr.Zero, screenDc);
            }

        }

        public void DrawWith(DrawingHandler drawingHandler)
        {

            try
            {                    
                
                _oldObject = Native.SelectObject(_memDc, HBitmap);

                if (_graphics == null)
                {
                    _graphics = Graphics.FromHdc(_memDc);

                    _graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    _graphics.SmoothingMode = SmoothingMode.AntiAlias;
                }
                
                //using (_graphics)
                //{
                drawingHandler(_graphics);
                //}
            }
            finally
            {
                Native.SelectObject(_memDc, _oldObject);
                //Native.ReleaseDC(IntPtr.Zero, screenDc);

            }
        }

        public void Dispose()
        {
            if (_graphics != null)
            {
                _graphics.Dispose();
                _graphics = null;
            }

            Native.SelectObject(_memDc, _oldObject);
            Native.DeleteDC(_memDc);

            if (HBitmap != IntPtr.Zero)
            {
                //Debug.WriteLine("Disposing ");
                Native.DeleteObject(HBitmap);
                HBitmap = IntPtr.Zero;
                //GC.Collect();
            }

        }
    }
}
