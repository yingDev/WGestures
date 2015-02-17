
//This file contains the common Win32 API of the desktop Windows and the Windows CE/Mobile. 

//Created by Warren Tang on 8/8/2008

using System;

using System.Runtime.InteropServices;

namespace Win32
{
    public static partial class GDI32
    {
#if PocketPC
        private const string Gdi32Dll = "coredll.dll";
#else
        private const string Gdi32Dll = "gdi32.dll";
#endif

        #region StretchBlt
        [DllImport(Gdi32Dll)]
        public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest,
             IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
             TernaryRasterOperations dwRop);
        #endregion

        #region BitBlt
        [DllImport(Gdi32Dll)]
        public static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
                IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        //public static void DrawControlToBitMap(Control srcControl, Bitmap destBitmap, Rectangle destBounds)
        //{
        //    using (Graphics srcGraph = srcControl.CreateGraphics())
        //    {
        //        IntPtr srcHdc = srcGraph.GetHdc();
        //        User32.SendMessage(srcControl.Handle, User32.WM.WM_PRINT, (int)srcHdc, 30);

        //        using (Graphics destGraph = Graphics.FromImage(destBitmap))
        //        {
        //            IntPtr destHdc = destGraph.GetHdc();

        //            //BitBlt(destHdc, destBounds.X, destBounds.Y, destBounds.Width, destBounds.Height,
        //            //    srcHdc, 0, 0, TernaryRasterOperations.SRCCOPY);

        //            User32.SendMessage(srcControl.Handle, User32.WM.WM_PRINT, (int)destHdc, 30);

        //            destGraph.ReleaseHdc(destHdc);
        //        }

        //        srcGraph.ReleaseHdc(srcHdc);
        //    }
        //}

        /// <summary>
        /// Only print the current active form?! WM_PRINT and PrintWindow is not supported on CE.
        /// </summary>
        /// <param name="srcControl"></param>
        /// <param name="destBitmap"></param>
        /// <param name="destBounds"></param>
        /*public static void DrawControlToBitMap(Control srcControl, Bitmap destBitmap, Rectangle destBounds)
        {
            using (Graphics srcGraph = srcControl.CreateGraphics())
            {
                IntPtr srcHdc = srcGraph.GetHdc();
                User32.SendMessage(srcControl.Handle, User32.WM.WM_PRINT, (int)srcHdc, 30);

                using (Graphics destGraph = Graphics.FromImage(destBitmap))
                {
                    IntPtr destHdc = destGraph.GetHdc();
                    BitBlt(destHdc, destBounds.X, destBounds.Y, destBounds.Width, destBounds.Height,
                        srcHdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                    destGraph.ReleaseHdc(destHdc);
                }

                srcGraph.ReleaseHdc(srcHdc);
            }
        }
        */
        public enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020, /* dest = source*/
            SRCPAINT = 0x00EE0086, /* dest = source OR dest*/
            SRCAND = 0x008800C6, /* dest = source AND dest*/
            SRCINVERT = 0x00660046, /* dest = source XOR dest*/
            SRCERASE = 0x00440328, /* dest = source AND (NOT dest )*/
            NOTSRCCOPY = 0x00330008, /* dest = (NOT source)*/
            NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
            MERGECOPY = 0x00C000CA, /* dest = (source AND pattern)*/
            MERGEPAINT = 0x00BB0226, /* dest = (NOT source) OR dest*/
            PATCOPY = 0x00F00021, /* dest = pattern*/
            PATPAINT = 0x00FB0A09, /* dest = DPSnoo*/
            PATINVERT = 0x005A0049, /* dest = pattern XOR dest*/
            DSTINVERT = 0x00550009, /* dest = (NOT dest)*/
            BLACKNESS = 0x00000042, /* dest = BLACK*/
            WHITENESS = 0x00FF0062, /* dest = WHITE*/
        }

        public enum PRF
        {
            PRF_CHECKVISIBLE = 1,
            PRF_NONCLIENT = 2,
            PRF_CLIENT = 4,
            PRF_ERASEBKGND = 8,
            PRF_CHILDREN = 16,
            PRF_OWNED = 32
        }

        #endregion

        #region Device Context
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(Gdi32Dll)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        public enum DeviceCap
        {
            /// <summary>
            /// Logical pixels inch in X
            /// </summary>
            LOGPIXELSX = 88,
            /// <summary>
            /// Logical pixels inch in Y
            /// </summary>
            LOGPIXELSY = 90

            // Other constants may be founded on pinvoke.net
        }      

        #endregion


        #region structs

        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            int x;
            int y;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RECT:IEquatable<RECT>
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public void Shift(int x, int y)
            {
                Left += x;
                Right += x;
                Top += y;
                Bottom += y;
            }

            public bool Equals(RECT other)
            {
                return Left == other.Left &&
                       Top == other.Top &&
                       Right == other.Right &&
                       Bottom == other.Bottom;
            }

            public static bool operator==(RECT thiz, RECT other)
            {
                return thiz.Equals(other);
            }

            public static bool operator !=(RECT thiz, RECT other)
            {
                return !(thiz == other);
            }

            public override string ToString()
            {
                return string.Format("({0}, {1}, {2}, {3})", Left, Top, Right, Bottom);
            }
        }
        #endregion

        #region Color
        public enum COLOR
        {
            COLOR_WINDOW = 5,
            COLOR_WINDOWFRAME = 6,
            COLOR_WINDOWTEXT = 8
        }
        #endregion

        #region Imports
        [DllImport(Gdi32Dll)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, RasterOperation dwRop);

        [DllImport(Gdi32Dll,  SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport(Gdi32Dll,  SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport(Gdi32Dll,   SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport(Gdi32Dll,  SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport(Gdi32Dll)]
        static extern bool TextOut(IntPtr hdc, int nXStart, int nYStart,
           string lpString, int cbString);

        #endregion

        #region RasterOperation
        public enum RasterOperation : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062
        }
        #endregion

        [DllImport(Gdi32Dll, SetLastError = true, EntryPoint = "CreateDC", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateDC(string lpszDriver, string lpszDeviceName, string lpszOutput, HandleRef devMode);
        

        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
    }
}
