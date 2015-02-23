//
////This file contains the common Win32 API of the desktop Windows and the Windows CE/Mobile. 
//
////Created by Warren Tang on 8/8/2008
//
//
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using System.Runtime.InteropServices;
using System.Drawing;

namespace Win32
{
    public static partial class User32
    {
        
#if PocketPC
        private const string User32Dll = "coredll.dll";
#else
        private const string User32Dll = "user32.dll";
#endif
//
//        #region Win32
//
//        public const int CW_USEDEFAULT = unchecked((int)0x80000000);
//
//
        public delegate IntPtr WndProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
//
        //[DllImport(DllName, SetLastError = true)]
        //public extern static int RegisterClassEx([MarshalAs(UnmanagedType.Struct)]ref WNDCLASSEX wndClassEx);

        [DllImport(User32Dll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U2)]
        public static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);

        [DllImport(User32Dll, SetLastError = true)]
        public extern static bool UnregisterClass(string lpClassName, IntPtr hInstance);
//
        [DllImport(User32Dll)]
        public extern static IntPtr DefWindowProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
//
//        [DllImport("user32.dll")]
//        public static extern IntPtr SetMessageExtraInfo(IntPtr lParam);
//
        [DllImport(User32Dll, SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
            WS_EX dwExStyle,
            string lpClassName,
            string lpWindowName,
            WS dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport(User32Dll)]
        public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport(User32Dll)]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport(User32Dll)]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
//
//        [DllImport(User32Dll)]
//        public static extern IntPtr BeginPaint(IntPtr hwnd, out GDI32.PAINTSTRUCT lpPaint);
//
//        [DllImport(User32Dll)]
//        public static extern IntPtr EndPaint(IntPtr hwnd, out GDI32.PAINTSTRUCT lpPaint);
//
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, SPIF fWinIni); // T = any type

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        // For setting a string parameter
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, SPIF fWinIni);

        // For reading a string parameter
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, StringBuilder pvParam, SPIF fWinIni);

        [DllImport("user32.dll")]
        public static extern bool AllowSetForegroundWindow(int dwProcessId);
//
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LockSetForegroundWindow(uint uLockCode);
//
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref ANIMATIONINFO pvParam, SPIF fWinIni);

//
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            public uint cbSize;
            public GDI32.RECT rcWindow;
            public GDI32.RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filler)
                : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
            }

        }
//
        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEX
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public CS style;
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public UInt32 message;
            public IntPtr wParam;
            public IntPtr lParam;
            public UInt32 time;
            public GDI32.POINT pt;
        }
//
//        public enum Bool
//        {
//            False = 0,
//            True
//        }
//
//        [StructLayout(LayoutKind.Sequential, Pack = 1)]
//        public struct BLENDFUNCTION
//        {
//            public byte BlendOp;
//            public byte BlendFlags;
//            public byte SourceConstantAlpha;
//            public byte AlphaFormat;
//        }
//
//        #endregion
//
//        [DllImport(User32Dll)]
//        public static extern bool GetClientRect(IntPtr hWnd, out GDI32.RECT lpRect);
//
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out GDI32.RECT lpRect);
//
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

//
//        #region DrawText
//
//        [DllImport(User32Dll)]
//        public static extern int DrawText(IntPtr hDC, string lpString, int nCount, ref GDI32.RECT lpRect, DT uFormat);
//
//        [Flags]
//        public enum DT : uint
//        {
//            DT_TOP = 0x00000000,
//            DT_LEFT = 0x00000000,
//            DT_CENTER = 0x00000001,
//            DT_RIGHT = 0x00000002,
//            DT_VCENTER = 0x00000004,
//            DT_BOTTOM = 0x00000008,
//            DT_WORDBREAK = 0x00000010,
//            DT_SINGLELINE = 0x00000020,
//            DT_EXPANDTABS = 0x00000040,
//            DT_TABSTOP = 0x00000080,
//            DT_NOCLIP = 0x00000100,
//            DT_EXTERNALLEADING = 0x00000200,
//            DT_CALCRECT = 0x00000400,
//            DT_NOPREFIX = 0x00000800,
//            DT_INTERNAL = 0x00001000
//        }
//        #endregion
//
        #region Cursor & Icon
        [DllImport(User32Dll, SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IDC lpCursorName);

        [DllImport(User32Dll, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        public enum IDI
        {
            IDI_APPLICATION = 32512,
            IDI_HAND = 32513,
            IDI_QUESTION = 32514,
            IDI_EXCLAMATION = 32515,
            IDI_ASTERISK = 32516,
            IDI_WINLOGO = 32517,
            IDI_WARNING = IDI_EXCLAMATION,
            IDI_ERROR = IDI_HAND,
            IDI_INFORMATION = IDI_ASTERISK,
        }

        public enum IDC
        {
            IDC_ARROW = 32512,
            IDC_IBEAM = 32513,
            IDC_WAIT = 32514,
            IDC_CROSS = 32515,
            IDC_UPARROW = 32516,
            IDC_SIZE = 32640,
            IDC_ICON = 32641,
            IDC_SIZENWSE = 32642,
            IDC_SIZENESW = 32643,
            IDC_SIZEWE = 32644,
            IDC_SIZENS = 32645,
            IDC_SIZEALL = 32646,
            IDC_NO = 32648,
            IDC_APPSTARTING = 32650,
            IDC_HELP = 32651
        }

        #endregion
//
//        #region keyboard
//        [DllImport(User32Dll)]
//        public static extern int GetKeyboardType(GKT nTypeFlag);
//
//        public enum GKT
//        {
//            KeyBoardType = 0,
//            KeyBoardSubType = 1,
//            NumOfFuncKeys = 2
//        }
//        #endregion
//
//        #region Scroll bar
//        [DllImport(User32Dll)]
//        public static extern int GetScrollInfo(IntPtr hwnd, SB fnBar, ref SCROLLINFO lpsi);
//
//        [DllImport(User32Dll)]
//        public static extern int SetScrollInfo(IntPtr hwnd, SB fnBar, ref SCROLLINFO lpsi, bool fRedraw);
//
//        public enum SB : int
//        {
//            SB_HORZ = 0,
//            SB_VERT = 1,
//            SB_CTL = 2
//        }
//
//
//        [StructLayout(LayoutKind.Sequential)]
//        public struct SCROLLINFO
//        {
//            public int cbSize;
//            public int fMask;
//            public int nMin;
//            public int nMax;
//            public int nPage;
//            public int nPos;
//            public int nTrackPos;
//        }
//        #endregion
//
//        #region ExitWindow
//        [DllImport("aygshell.dll")]
//        public static extern bool ExitWindowsEx(EWX uFlags, uint dwReserved);
//
//        public enum EWX : uint
//        {
//            //EWX_LOGOFF = 0,
//            //EWX_SHUTDOWN = 1, 
//            EWX_REBOOT = 2,
//            //EWX_FORCE = 4,
//            EWX_POWEROFF = 8 //Not supported on PPC
//            //EWX_FORCEIFHUNG = 16
//        }
//
//        #endregion
//
       #region SetWindowLong
//
        [DllImport(User32Dll, SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, WS nIndex);

        [DllImport(User32Dll)]
        public extern static void SetWindowLong(IntPtr hwnd, GWL nIndex, int dwNewLong);

        [DllImport(User32Dll)]
        public extern static IntPtr SetWindowLong(IntPtr hwnd, GWL nIndex, IntPtr dwNewLong);
        [DllImport(User32Dll)]
        public extern static int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, uint wParam, int lParam);

        #endregion
//
        #region GWL
        public enum GWL : int
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }
        #endregion
//
        #region WindowStyles
        [Flags]
        public enum WS : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,
        }

        [DllImport(User32Dll, SetLastError = true)]
        public static extern void PostQuitMessage(int nExitCode);

        #endregion
//
        #region Entented Window Styles

        [Flags]
        public enum WS_EX : uint
        {
            WS_EX_NONE = 0,
            /// <summary>
            /// Specifies that a window created with this style accepts drag-drop files.
            /// </summary>
            WS_EX_ACCEPTFILES = 0x00000010,
            /// <summary>
            /// Forces a top-level window onto the taskbar when the window is visible.
            /// </summary>
            WS_EX_APPWINDOW = 0x00040000,
            /// <summary>
            /// Specifies that a window has a border with a sunken edge.
            /// </summary>
            WS_EX_CLIENTEDGE = 0x00000200,
            /// <summary>
            /// Windows XP: Paints all descendants of a window in bottom-to-top painting order using double-buffering. For more information, see Remarks. This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
            /// </summary>
            WS_EX_COMPOSITED = 0x02000000,
            /// <summary>
            /// Includes a question mark in the title bar of the window. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window.
            /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
            /// </summary>
            WS_EX_CONTEXTHELP = 0x00000400,
            /// <summary>
            /// The window itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.
            /// </summary>
            WS_EX_CONTROLPARENT = 0x00010000,
            /// <summary>
            /// Creates a window that has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
            /// </summary>
            WS_EX_DLGMODALFRAME = 0x00000001,
            /// <summary>
            /// Windows 2000/XP: Creates a layered window. Note that this cannot be used for child windows. Also, this cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
            /// </summary>
            WS_EX_LAYERED = 0x00080000,
            /// <summary>
            /// Arabic and Hebrew versions of Windows 98/Me, Windows 2000/XP: Creates a window whose horizontal origin is on the right edge. Increasing horizontal values advance to the left.
            /// </summary>
            WS_EX_LAYOUTRTL = 0x00400000,
            /// <summary>
            /// Creates a window that has generic left-aligned properties. This is the default.
            /// </summary>
            WS_EX_LEFT = 0x00000000,
            /// <summary>
            /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.
            /// </summary>
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            /// <summary>
            /// The window text is displayed using left-to-right reading-order properties. This is the default.
            /// </summary>
            WS_EX_LTRREADING = 0x00000000,
            /// <summary>
            /// Creates a multiple-document interface (MDI) child window.
            /// </summary>
            WS_EX_MDICHILD = 0x00000040,
            /// <summary>
            /// Windows 2000/XP: A top-level window created with this style does not become the foreground window when the user clicks it. The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
            /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
            /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
            /// </summary>
            WS_EX_NOACTIVATE = 0x08000000,
            /// <summary>
            /// Windows 2000/XP: A window created with this style does not pass its window layout to its child windows.
            /// </summary>
            WS_EX_NOINHERITLAYOUT = 0x00100000,
            /// <summary>
            /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
            /// </summary>
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            /// <summary>
            /// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles.
            /// </summary>
            WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
            /// <summary>
            /// Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles.
            /// </summary>
            WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
            /// <summary>
            /// The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored.
            /// Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
            /// </summary>
            WS_EX_RIGHT = 0x00001000,
            /// <summary>
            /// Vertical scroll bar (if present) is to the right of the client area. This is the default.
            /// </summary>
            WS_EX_RIGHTSCROLLBAR = 0x00000000,
            /// <summary>
            /// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored.
            /// </summary>
            WS_EX_RTLREADING = 0x00002000,
            /// <summary>
            /// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input.
            /// </summary>
            WS_EX_STATICEDGE = 0x00020000,
            /// <summary>
            /// Creates a tool window; that is, a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
            /// </summary>
            WS_EX_TOOLWINDOW = 0x00000080,
            /// <summary>
            /// Specifies that a window created with this style should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function.
            /// </summary>
            WS_EX_TOPMOST = 0x00000008,
            /// <summary>
            /// Specifies that a window created with this style should not be painted until siblings beneath the window (that were created by the same thread) have been painted. The window appears transparent because the bits of underlying sibling windows have already been painted.
            /// To achieve transparency without these restrictions, use the SetWindowRgn function.
            /// </summary>
            WS_EX_TRANSPARENT = 0x00000020,
            /// <summary>
            /// Specifies that a window has a border with a raised edge.
            /// </summary>
            WS_EX_WINDOWEDGE = 0x00000100
        }

        #endregion
//
//        /// <summary>
//        /// <para>The DestroyWindow function destroys the specified window. The function sends WM_DESTROY and WM_NCDESTROY messages to the window to deactivate it and remove the keyboard focus from it. The function also destroys the window's menu, flushes the thread message queue, destroys timers, removes clipboard ownership, and breaks the clipboard viewer chain (if the window is at the top of the viewer chain).</para>
//        /// <para>If the specified window is a parent or owner window, DestroyWindow automatically destroys the associated child or owned windows when it destroys the parent or owner window. The function first destroys child or owned windows, and then it destroys the parent or owner window.</para>
//        /// <para>DestroyWindow also destroys modeless dialog boxes created by the CreateDialog function.</para>
//        /// </summary>
//        /// <param name="hwnd">Handle to the window to be destroyed.</param>
//        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);
//
        [DllImport(User32Dll)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);
//
//        [DllImport(User32Dll)]
//        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
//
//        [DllImport("coredll")]
//        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
//
        [DllImport(User32Dll)]
        public static extern bool ShowWindow(IntPtr hwnd, SW nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
//
//        [DllImport(User32Dll)]
//        public static extern bool EnableWindow(IntPtr hwnd, bool enabled);
//
//        [DllImport(User32Dll)]
//        public static extern bool InvalidateRect(IntPtr hWnd, int pRect, bool bErase);
//
        [DllImport(User32Dll)]
        public static extern bool UpdateWindow(IntPtr hwnd);
//
//        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
//        public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);
//
//        [DllImport(User32Dll)]
//        public static extern bool SetWindowText(IntPtr hWnd, string lpString);
//
        [DllImport(User32Dll)]
        public static extern int SendMessage(IntPtr hWnd, WM Msg, uint wParam, uint lParam);

        [DllImport(User32Dll)]
        public static extern int SendMessage(IntPtr hWnd, WM Msg, int wParam, int lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, WM Msg, int wParam, int lParam);

//        [DllImport(User32Dll)]
//        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RDW flags);
//
        [DllImport(User32Dll)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
//
        [DllImport(User32Dll)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport(User32Dll)]
        public static extern IntPtr GetActiveWindow();
//
//        [DllImport(User32Dll, SetLastError = true)]
//        public static extern int GetWindowTextLength(IntPtr hWnd);
//
//        [DllImport(User32Dll, SetLastError = true)]
//        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
//
//        [DllImport("user32.dll")]
//        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);
//
//
        [DllImport(User32Dll)]
        public static extern bool LockWorkStation();
//
//        #region GetLastInputInfo
//        [DllImport(User32Dll)]
//        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
//
//
//        [StructLayout(LayoutKind.Sequential)]
//        public struct LASTINPUTINFO
//        {
//            public int cbSize;
//            public int dwTime;
//        }
//
//        /// <summary>
//        /// Get the tick count since the last user input. (Wrapper)
//        /// </summary>
//        /// <returns>The tick count since the last user input.</returns>
//        public static int GetLastInputTime()
//        {
//            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
//            lastInPut.cbSize = (int)Marshal.SizeOf(lastInPut);
//            lastInPut.dwTime = 0;
//
//            if (!GetLastInputInfo(ref lastInPut))
//            {
//                throw new Exception(Marshal.GetLastWin32Error().ToString());
//            }
//
//            return lastInPut.dwTime; // (int)TimeSpan.TicksPerMillisecond;
//        }
//
//        #endregion
//
        #region User input
        [DllImport(User32Dll)]
        public static extern bool EnableHardwareKeyboard(bool bEnable);

        [DllImport(User32Dll)]
        public static extern void mouse_event(MOUSEEVENTF dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, KEYEVENTF dwFlags, uint dwExtraInfo);

        [Flags]
        public enum KEYEVENTF : uint
        {
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
            //KEYEVENTF_SILENT = ?
            //KEYEVENTF_UNICODE = 0x0004, 
            //KEYEVENTF_SCANCODE = 0x0008
        }
        #endregion

        [DllImport(User32Dll)]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport(User32Dll)]
        public static extern IntPtr SetCursor(IntPtr hCursor);

        [DllImport(User32Dll)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport(User32Dll, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();
//
//        [DllImport("user32.dll")]
//        public static extern IntPtr GetWindow(IntPtr hWnd, GW uCmd);
//
//        #region RedrawWindow
//        [Flags]
//        public enum RDW : uint
//        {
//            RDW_INVALIDATE = 0x0001,
//            RDW_INTERNALPAINT = 0x0002,
//            RDW_ERASE = 0x0004,
//
//            RDW_VALIDATE = 0x0008,
//            RDW_NOINTERNALPAINT = 0x0010,
//            RDW_NOERASE = 0x0020,
//
//            RDW_NOCHILDREN = 0x0040,
//            RDW_ALLCHILDREN = 0x0080,
//
//            RDW_UPDATENOW = 0x0100,
//            RDW_ERASENOW = 0x0200,
//
//            RDW_FRAME = 0x0400,
//            RDW_NOFRAME = 0x0800
//        }
//        #endregion
//
//        #region Get Window(GW)
//        public enum GW : uint
//        {
//            GW_HWNDFIRST = 0,
//            GW_HWNDLAST = 1,
//            GW_HWNDNEXT = 2,
//            GW_HWNDPREV = 3,
//            GW_OWNER = 4,
//            GW_CHILD = 5,
//            GW_ENABLEDPOPUP = 6
//        }
//        #endregion
//
//        #region Hotkey
//
//        [DllImport("coredll")]
//        public static extern int RegisterHotKey(IntPtr hwnd, int id, KeyModifiers fsModifiers, VK vk);
//
//        [DllImport(User32Dll)]
//        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
//
//        [Flags]
//        public enum KeyModifiers
//        {
//            MOD_ALT = 0x0001,
//            MOD_CONTROL = 0x0002,
//            MOD_SHIFT = 0x0004,
//            MOD_WIN = 0x0008,
//            MOD_KEYUP = 0x1000,
//            NONE = 0x0000
//        }
//
//        public enum HardwareKeys : int
//        {
//            APP1 = 0xC1,
//            APP2 = 0xC2,
//            APP3 = 0xC3,
//            APP4 = 0xC4,
//            APP5 = 0xC5,
//            APP6 = 0xC6,
//            APP7 = 0xC7,
//            APP8 = 0xC8,
//            APP9 = 0xC9,
//            APP10 = 0xCA,
//            APP11 = 0xCB,
//            APP12 = 0xCC,
//            APP13 = 0xCD,
//            APP14 = 0xCE,
//            APP15 = 0xCF,
//            APP16
//        }
//
//        #endregion
//
        #region MOUSEEVENTF
        [Flags]
        public enum MOUSEEVENTF
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }
        #endregion
//
        #region SetWindowPos(SWP)
        [Flags]
        public enum SWP
        {
            SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_NOACTIVATE = 0x0010,
            SWP_FRAMECHANGED = 0x0020,  /* The frame changed: send WM_NCCALCSIZE */
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOOWNERZORDER = 0x0200, /* Don't do owner Z ordering */
            SWP_DRAWFRAME = SWP_FRAMECHANGED,
            SWP_NOREPOSITION = SWP_NOOWNERZORDER,
            SWP_NOSTARTUP = 0x04000000,
            SWP_STARTUP = 0x08000000
        }
        #endregion

        #region Show Window(SW)
        [Flags]
        public enum SW
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWNA = 8,
            SW_SHOWMAXIMIZED = 11,
            SW_MAXIMIZE = 12,
            SW_RESTORE = 13
        }
        #endregion
//
//        #region HWND
//        public static class HWND
//        {
//            public static IntPtr HWND_BROADCAST = new IntPtr(0xffff);
//
//            public static IntPtr HWND_TOP = (IntPtr)0;
//
//            public static IntPtr HWND_TOPMOST = (IntPtr)(-1);
//
//            public static IntPtr HWND_NOTOPMOST = (IntPtr)(-2);
//
//            public static IntPtr HWND_BOTTOM = (IntPtr)1;
//        }
//        #endregion
//
        #region Windows Message(WM)
        public enum WM
        {
            WM_ACTIVATE = 0x0006,
            WM_ACTIVATEAPP = 0x001C,
            WM_AFXFIRST = 0x0360,
            WM_AFXLAST = 0x037F,
            WM_APP = 0x8000,
            WM_ASKCBFORMATNAME = 0x030C,
            WM_CANCELJOURNAL = 0x004B,
            WM_CANCELMODE = 0x001F,
            WM_CAPTURECHANGED = 0x0215,
            WM_CHANGECBCHAIN = 0x030D,
            WM_CHANGEUISTATE = 0x0127,
            WM_CHAR = 0x0102,
            WM_CHARTOITEM = 0x002F,
            WM_CHILDACTIVATE = 0x0022,
            WM_CLEAR = 0x0303,
            WM_CLOSE = 0x0010,
            WM_COMMAND = 0x0111,
            WM_COMPACTING = 0x0041,
            WM_COMPAREITEM = 0x0039,
            WM_CONTEXTMENU = 0x007B,
            WM_COPY = 0x0301,
            WM_COPYDATA = 0x004A,
            WM_CREATE = 0x0001,
            WM_CTLCOLORBTN = 0x0135,
            WM_CTLCOLORDLG = 0x0136,
            WM_CTLCOLOREDIT = 0x0133,
            WM_CTLCOLORLISTBOX = 0x0134,
            WM_CTLCOLORMSGBOX = 0x0132,
            WM_CTLCOLORSCROLLBAR = 0x0137,
            WM_CTLCOLORSTATIC = 0x0138,
            WM_CUT = 0x0300,
            WM_DEADCHAR = 0x0103,
            WM_DELETEITEM = 0x002D,
            WM_DESTROY = 0x0002,
            WM_DESTROYCLIPBOARD = 0x0307,
            WM_DEVICECHANGE = 0x0219,
            WM_DEVMODECHANGE = 0x001B,
            WM_DISPLAYCHANGE = 0x007E,
            WM_DRAWCLIPBOARD = 0x0308,
            WM_DRAWITEM = 0x002B,
            WM_DROPFILES = 0x0233,
            WM_ENABLE = 0x000A,
            WM_ENDSESSION = 0x0016,
            WM_ENTERIDLE = 0x0121,
            WM_ENTERMENULOOP = 0x0211,
            WM_ENTERSIZEMOVE = 0x0231,
            WM_ERASEBKGND = 0x0014,
            WM_EXITMENULOOP = 0x0212,
            WM_EXITSIZEMOVE = 0x0232,
            WM_FONTCHANGE = 0x001D,
            WM_GETDLGCODE = 0x0087,
            WM_GETFONT = 0x0031,
            WM_GETHOTKEY = 0x0033,
            WM_GETICON = 0x007F,
            WM_GETMINMAXINFO = 0x0024,
            WM_GETOBJECT = 0x003D,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_HANDHELDFIRST = 0x0358,
            WM_HANDHELDLAST = 0x035F,
            WM_HELP = 0x0053,
            WM_HOTKEY = 0x0312,
            WM_HSCROLL = 0x0114,
            WM_HSCROLLCLIPBOARD = 0x030E,
            WM_ICONERASEBKGND = 0x0027,
            WM_IME_CHAR = 0x0286,
            WM_IME_COMPOSITION = 0x010F,
            WM_IME_COMPOSITIONFULL = 0x0284,
            WM_IME_CONTROL = 0x0283,
            WM_IME_ENDCOMPOSITION = 0x010E,
            WM_IME_KEYDOWN = 0x0290,
            WM_IME_KEYLAST = 0x010F,
            WM_IME_KEYUP = 0x0291,
            WM_IME_NOTIFY = 0x0282,
            WM_IME_REQUEST = 0x0288,
            WM_IME_SELECT = 0x0285,
            WM_IME_SETCONTEXT = 0x0281,
            WM_IME_STARTCOMPOSITION = 0x010D,
            WM_INITDIALOG = 0x0110,
            WM_INITMENU = 0x0116,
            WM_INITMENUPOPUP = 0x0117,
            WM_INPUTLANGCHANGE = 0x0051,
            WM_INPUTLANGCHANGEREQUEST = 0x0050,
            WM_KEYDOWN = 0x0100,
            WM_KEYFIRST = 0x0100,
            WM_KEYLAST = 0x0108,
            WM_KEYUP = 0x0101,
            WM_KILLFOCUS = 0x0008,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MBUTTONDBLCLK = 0x0209,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MDIACTIVATE = 0x0222,
            WM_MDICASCADE = 0x0227,
            WM_MDICREATE = 0x0220,
            WM_MDIDESTROY = 0x0221,
            WM_MDIGETACTIVE = 0x0229,
            WM_MDIICONARRANGE = 0x0228,
            WM_MDIMAXIMIZE = 0x0225,
            WM_MDINEXT = 0x0224,
            WM_MDIREFRESHMENU = 0x0234,
            WM_MDIRESTORE = 0x0223,
            WM_MDISETMENU = 0x0230,
            WM_MDITILE = 0x0226,
            WM_MEASUREITEM = 0x002C,
            WM_MENUCHAR = 0x0120,
            WM_MENUCOMMAND = 0x0126,
            WM_MENUDRAG = 0x0123,
            WM_MENUGETOBJECT = 0x0124,
            WM_MENURBUTTONUP = 0x0122,
            WM_MENUSELECT = 0x011F,
            WM_MOUSEACTIVATE = 0x0021,
            WM_MOUSEFIRST = 0x0200,
            WM_MOUSEHOVER = 0x02A1,
            WM_MOUSELAST = 0x020A,
            WM_MOUSELEAVE = 0x02A3,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_MOVE = 0x0003,
            WM_MOVING = 0x0216,
            WM_NCACTIVATE = 0x0086,
            WM_NCCALCSIZE = 0x0083,
            WM_NCCREATE = 0x0081,
            WM_NCDESTROY = 0x0082,
            WM_NCHITTEST = 0x0084,
            WM_NCLBUTTONDBLCLK = 0x00A3,
            WM_NCLBUTTONDOWN = 0x00A1,
            WM_NCLBUTTONUP = 0x00A2,
            WM_NCMBUTTONDBLCLK = 0x00A9,
            WM_NCMBUTTONDOWN = 0x00A7,
            WM_NCMBUTTONUP = 0x00A8,
            WM_NCMOUSEMOVE = 0x00A0,
            WM_NCPAINT = 0x0085,
            WM_NCRBUTTONDBLCLK = 0x00A6,
            WM_NCRBUTTONDOWN = 0x00A4,
            WM_NCRBUTTONUP = 0x00A5,
            WM_NEXTDLGCTL = 0x0028,
            WM_NEXTMENU = 0x0213,
            WM_NOTIFY = 0x004E,
            WM_NOTIFYFORMAT = 0x0055,
            WM_NULL = 0x0000,
            WM_PAINT = 0x000F,
            WM_PAINTCLIPBOARD = 0x0309,
            WM_PAINTICON = 0x0026,
            WM_PALETTECHANGED = 0x0311,
            WM_PALETTEISCHANGING = 0x0310,
            WM_PARENTNOTIFY = 0x0210,
            WM_PASTE = 0x0302,
            WM_PENWINFIRST = 0x0380,
            WM_PENWINLAST = 0x038F,
            WM_POWER = 0x0048,
            WM_POWERBROADCAST = 0x0218,
            WM_PRINT = 0x0317,
            WM_PRINTCLIENT = 0x0318,
            WM_QUERYDRAGICON = 0x0037,
            WM_QUERYENDSESSION = 0x0011,
            WM_QUERYNEWPALETTE = 0x030F,
            WM_QUERYOPEN = 0x0013,
            WM_QUEUESYNC = 0x0023,
            WM_QUIT = 0x0012,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_RENDERALLFORMATS = 0x0306,
            WM_RENDERFORMAT = 0x0305,
            WM_SETCURSOR = 0x0020,
            WM_SETFOCUS = 0x0007,
            WM_SETFONT = 0x0030,
            WM_SETHOTKEY = 0x0032,
            WM_SETICON = 0x0080,
            WM_SETREDRAW = 0x000B,
            WM_SETTEXT = 0x000C,
            WM_SETTINGCHANGE = 0x001A,
            WM_SHOWWINDOW = 0x0018,
            WM_SIZE = 0x0005,
            WM_SIZECLIPBOARD = 0x030B,
            WM_SIZING = 0x0214,
            WM_SPOOLERSTATUS = 0x002A,
            WM_STYLECHANGED = 0x007D,
            WM_STYLECHANGING = 0x007C,
            WM_SYNCPAINT = 0x0088,
            WM_SYSCHAR = 0x0106,
            WM_SYSCOLORCHANGE = 0x0015,
            WM_SYSCOMMAND = 0x0112,
            WM_SYSDEADCHAR = 0x0107,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
            WM_TCARD = 0x0052,
            WM_TIMECHANGE = 0x001E,
            WM_TIMER = 0x0113,
            WM_UNDO = 0x0304,
            WM_UNINITMENUPOPUP = 0x0125,
            WM_USER = 0x0400,
            WM_USERCHANGED = 0x0054,
            WM_VKEYTOITEM = 0x002E,
            WM_VSCROLL = 0x0115,
            WM_VSCROLLCLIPBOARD = 0x030A,
            WM_WINDOWPOSCHANGED = 0x0047,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WININICHANGE = 0x001A,
            WM_XBUTTONDBLCLK = 0x020D,
            WM_XBUTTONDOWN = 0x020B,
            WM_XBUTTONUP = 0x020C
        }
        #endregion
//
        #region Virtual Key(VK)
        public enum VK : int
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,
            //
            VK_XBUTTON1 = 0x05,
            VK_XBUTTON2 = 0x06,
            //
            VK_BACK = 0x08,
            VK_TAB = 0x09,
            //
            VK_CLEAR = 0x0C,
            VK_RETURN = 0x0D,
            //
            VK_SHIFT = 0x10,
            VK_CONTROL = 0x11,
            VK_MENU = 0x12,
            VK_PAUSE = 0x13,
            VK_CAPITAL = 0x14,
            //
            VK_KANA = 0x15,
            VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
            VK_HANGUL = 0x15,
            VK_JUNJA = 0x17,
            VK_FINAL = 0x18,
            VK_HANJA = 0x19,
            VK_KANJI = 0x19,
            //
            VK_ESCAPE = 0x1B,
            //
            VK_CONVERT = 0x1C,
            VK_NONCONVERT = 0x1D,
            VK_ACCEPT = 0x1E,
            VK_MODECHANGE = 0x1F,
            //
            VK_SPACE = 0x20,
            VK_PRIOR = 0x21,
            VK_NEXT = 0x22,
            VK_END = 0x23,
            VK_HOME = 0x24,
            VK_LEFT = 0x25,
            VK_UP = 0x26,
            VK_RIGHT = 0x27,
            VK_DOWN = 0x28,
            VK_SELECT = 0x29,
            VK_PRINT = 0x2A,
            VK_EXECUTE = 0x2B,
            VK_SNAPSHOT = 0x2C,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_HELP = 0x2F,
            //
            VK_LWIN = 0x5B,
            VK_RWIN = 0x5C,
            VK_APPS = 0x5D,
            //
            VK_SLEEP = 0x5F,
            //
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 0x64,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_MULTIPLY = 0x6A,
            VK_ADD = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT = 0x6D,
            VK_DECIMAL = 0x6E,
            VK_DIVIDE = 0x6F,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,
            //
            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91,
            //
            VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad
            //
            VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
            VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
            VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
            VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
            VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key
            //
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3,
            VK_LMENU = 0xA4,
            VK_RMENU = 0xA5,
            //
            VK_BROWSER_BACK = 0xA6,
            VK_BROWSER_FORWARD = 0xA7,
            VK_BROWSER_REFRESH = 0xA8,
            VK_BROWSER_STOP = 0xA9,
            VK_BROWSER_SEARCH = 0xAA,
            VK_BROWSER_FAVORITES = 0xAB,
            VK_BROWSER_HOME = 0xAC,
            //
            VK_VOLUME_MUTE = 0xAD,
            VK_VOLUME_DOWN = 0xAE,
            VK_VOLUME_UP = 0xAF,
            VK_MEDIA_NEXT_TRACK = 0xB0,
            VK_MEDIA_PREV_TRACK = 0xB1,
            VK_MEDIA_STOP = 0xB2,
            VK_MEDIA_PLAY_PAUSE = 0xB3,
            VK_LAUNCH_MAIL = 0xB4,
            VK_LAUNCH_MEDIA_SELECT = 0xB5,
            VK_LAUNCH_APP1 = 0xB6,
            VK_LAUNCH_APP2 = 0xB7,
            //
            VK_OEM_1 = 0xBA,   // ';:' for US
            VK_OEM_PLUS = 0xBB,   // '+' any country
            VK_OEM_COMMA = 0xBC,   // ',' any country
            VK_OEM_MINUS = 0xBD,   // '-' any country
            VK_OEM_PERIOD = 0xBE,   // '.' any country
            VK_OEM_2 = 0xBF,   // '/?' for US
            VK_OEM_3 = 0xC0,   // '`~' for US
            //
            VK_OEM_4 = 0xDB,  //  '[{' for US
            VK_OEM_5 = 0xDC,  //  '\|' for US
            VK_OEM_6 = 0xDD,  //  ']}' for US
            VK_OEM_7 = 0xDE,  //  ''"' for US
            VK_OEM_8 = 0xDF,
            //
            VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
            VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
            VK_ICO_HELP = 0xE3,  //  Help key on ICO
            VK_ICO_00 = 0xE4,  //  00 key on ICO
            //
            VK_PROCESSKEY = 0xE5,
            //
            VK_ICO_CLEAR = 0xE6,
            //
            VK_PACKET = 0xE7,
            //
            VK_OEM_RESET = 0xE9,
            VK_OEM_JUMP = 0xEA,
            VK_OEM_PA1 = 0xEB,
            VK_OEM_PA2 = 0xEC,
            VK_OEM_PA3 = 0xED,
            VK_OEM_WSCTRL = 0xEE,
            VK_OEM_CUSEL = 0xEF,
            VK_OEM_ATTN = 0xF0,
            VK_OEM_FINISH = 0xF1,
            VK_OEM_COPY = 0xF2,
            VK_OEM_AUTO = 0xF3,
            VK_OEM_ENLW = 0xF4,
            VK_OEM_BACKTAB = 0xF5,
            //
            VK_ATTN = 0xF6,
            VK_CRSEL = 0xF7,
            VK_EXSEL = 0xF8,
            VK_EREOF = 0xF9,
            VK_PLAY = 0xFA,
            VK_ZOOM = 0xFB,
            VK_NONAME = 0xFC,
            VK_PA1 = 0xFD,
            VK_OEM_CLEAR = 0xFE
        }
        #endregion
//
        #region Class Styles
        [Flags]
        public enum CS : uint
        {
            CS_VREDRAW = 0x0001,
            CS_HREDRAW = 0x0002,
            CS_DBLCLKS = 0x0008,
            CS_OWNDC = 0x0020,
            CS_CLASSDC = 0x0040,
            CS_PARENTDC = 0x0080,
            CS_NOCLOSE = 0x0200,
            CS_SAVEBITS = 0x0800,
            CS_BYTEALIGNCLIENT = 0x1000,
            CS_BYTEALIGNWINDOW = 0x2000,
            CS_GLOBALCLASS = 0x4000,
            CS_IME = 0x00010000,
            CS_DROPSHADOW = 0x00020000
        }

        #endregion
//
//        [DllImport(User32Dll)]
//        public static extern bool BlockInput(bool fBlockIt);
//
//        [DllImport(User32Dll)]
//        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
//
//        [DllImport(User32Dll, SetLastError = true)]
//        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport(User32Dll, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);
//
//        [DllImport(User32Dll)]
//        public static extern void mouse_event(MOUSEEVENTF dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
//
//        [DllImport(User32Dll)]
//        public static extern bool GetCursorPos(out Point lpPoint);
//
//        [DllImport(User32Dll, SetLastError = true)]
//        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
//
//        [DllImport(User32Dll)]
//        public static extern IntPtr GetMessageExtraInfo();
//
//        #region Hook
//        [DllImport(User32Dll, SetLastError = true)]
//        static extern IntPtr SetWindowsHookEx(HookType hook, HookProc callback,
//           IntPtr hMod, uint dwThreadId);
//
//        [DllImport(User32Dll, SetLastError = true)]
//        static extern IntPtr SetWindowsHookEx(HookType hook, LowLevelKeyboardProc callback,
//           IntPtr hMod, uint dwThreadId);
//
//        [DllImport(User32Dll, SetLastError = true)]
//        static extern IntPtr SetWindowsHookEx(HookType code, LowLevelMouseProc func,
//            IntPtr hInstance, int threadID);
//
//        delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
//
//        delegate int LowLevelKeyboardProc(int nCode, WM wParam, [In]KBDLLHOOKSTRUCT lParam);
//
//        delegate int LowLevelMouseProc(int code, WM wParam, [In]MSLLHOOKSTRUCT lParam);
//
//        public enum HookType : int
//        {
//            WH_JOURNALRECORD = 0,
//            WH_JOURNALPLAYBACK = 1,
//            WH_KEYBOARD = 2,
//            WH_GETMESSAGE = 3,
//            WH_CALLWNDPROC = 4,
//            WH_CBT = 5,
//            WH_SYSMSGFILTER = 6,
//            WH_MOUSE = 7,
//            WH_HARDWARE = 8,
//            WH_DEBUG = 9,
//            WH_SHELL = 10,
//            WH_FOREGROUNDIDLE = 11,
//            WH_CALLWNDPROCRET = 12,
//            WH_KEYBOARD_LL = 13,
//            WH_MOUSE_LL = 14
//        }
//
//        [StructLayout(LayoutKind.Sequential)]
//        public class MSLLHOOKSTRUCT
//        {
//            public POINT pt;
//            public int mouseData;
//            public int flags;
//            public int time;
//            public IntPtr dwExtraInfo;
//        }
//
//        [StructLayout(LayoutKind.Sequential)]
//        public class KBDLLHOOKSTRUCT
//        {
//            public int vkCode;
//            public int scanCode;
//            public int flags;
//            public int time;
//            public IntPtr dwExtraInfo;
//        }
//        #endregion
//
        #region POINT
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;

            public Point(Int32 x, Int32 y) { this.x = x; this.y = y; }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;

            public Size(Int32 cx, Int32 cy) { this.cx = cx; this.cy = cy; }
        }

        #endregion
//
        #region SendInput structs
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MOUSEEVENTF dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public INPUT_TYPE type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        public enum INPUT_TYPE
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2
        }

        public enum SysCommands : int
        {
            SC_SIZE = 0xF000,
            SC_MOVE = 0xF010,
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_NEXTWINDOW = 0xF040,
            SC_PREVWINDOW = 0xF050,
            SC_CLOSE = 0xF060,
            SC_VSCROLL = 0xF070,
            SC_HSCROLL = 0xF080,
            SC_MOUSEMENU = 0xF090,
            SC_KEYMENU = 0xF100,
            SC_ARRANGE = 0xF110,
            SC_RESTORE = 0xF120,
            SC_TASKLIST = 0xF130,
            SC_SCREENSAVE = 0xF140,
            SC_HOTKEY = 0xF150,
            //#if(WINVER >= 0x0400) //Win95
            SC_DEFAULT = 0xF160,
            SC_MONITORPOWER = 0xF170,
            SC_CONTEXTHELP = 0xF180,
            SC_SEPARATOR = 0xF00F,
            //#endif /* WINVER >= 0x0400 */

            //#if(WINVER >= 0x0600) //Vista
            SCF_ISSECURE = 0x00000001,
            //#endif /* WINVER >= 0x0600 */

            /*
              * Obsolete names
              */
            SC_ICON = SC_MINIMIZE,
            SC_ZOOM = SC_MAXIMIZE,
        }


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }
        #endregion
//
        [Flags]
        public enum SPIF
        {
            None = 0x00,
            /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
            SPIF_UPDATEINIFILE = 0x01,
            /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
            SPIF_SENDCHANGE = 0x02,
            /// <summary>Same as SPIF_SENDCHANGE.</summary>
            SPIF_SENDWININICHANGE = 0x02
        }

//
        [DllImport(User32Dll, ExactSpelling = true)]
        public static extern IntPtr MonitorFromPoint(POINT pt, int flags);
        [DllImport(User32Dll, ExactSpelling = true)]
        public static extern IntPtr MonitorFromRect(ref GDI32.RECT rect, int flags);
        [DllImport(User32Dll, ExactSpelling = true)]
        public static extern IntPtr MonitorFromWindow(HandleRef handle, int flags);
        [DllImport(User32Dll, ExactSpelling = true)]
        public static extern bool EnumDisplayMonitors(HandleRef hdc, COMRECT rcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        [DllImport(User32Dll, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport(User32Dll, CharSet = CharSet.Auto)]
        public static extern bool SystemParametersInfo(int nAction, int nParam, ref GDI32.RECT rc, int nUpdate);
        
        public delegate bool MonitorEnumProc(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lParam);
//
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        private const int CCHDEVICENAME = 32;
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MonitorInfoEx
        {
            public int Size;
            public GDI32.RECT Monitor;
            public GDI32.RECT WorkArea;
            public uint Flags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string DeviceName;

            public void Init()
            {
                this.Size = 40 + 2 * CCHDEVICENAME;
                this.DeviceName = string.Empty;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class MONITORINFO
        {
            internal int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            internal GDI32.RECT rcMonitor = new GDI32.RECT();
            internal GDI32.RECT rcWork = new GDI32.RECT();
            internal int dwFlags = 0;
        }


        [StructLayout(LayoutKind.Sequential)]
        public class COMRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public COMRECT()
            {
            }

            public COMRECT(System.Drawing.Rectangle r)
            {
                this.left = r.X;
                this.top = r.Y;
                this.right = r.Right;
                this.bottom = r.Bottom;
            }


            public COMRECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /* Unused
            public RECT ToRECT() {
                return new RECT(left, top, right, bottom);
            }
            */

            public static COMRECT FromXYWH(int x, int y, int width, int height)
            {
                return new COMRECT(x, y, x + width, y + height);
            }

            public override string ToString()
            {
                return "Left = " + left + " Top " + top + " Right = " + right + " Bottom = " + bottom;
            }
        }
//
//        /// <summary>
//        /// ANIMATIONINFO specifies animation effects associated with user actions. 
//        /// Used with SystemParametersInfo when SPI_GETANIMATION or SPI_SETANIMATION action is specified.
//        /// </summary>
//        /// <remark>
//        /// The uiParam value must be set to (System.UInt32)Marshal.SizeOf(typeof(ANIMATIONINFO)) when using this structure.
//        /// </remark>
        [StructLayout(LayoutKind.Sequential)]
        public struct ANIMATIONINFO
        {
            /// <summary>
            /// Creates an AMINMATIONINFO structure.
            /// </summary>
            /// <param name="iMinAnimate">If non-zero and SPI_SETANIMATION is specified, enables minimize/restore animation.</param>
            public ANIMATIONINFO(System.Int32 iMinAnimate)
            {
                this.cbSize = (System.UInt32)Marshal.SizeOf(typeof(ANIMATIONINFO));
                this.iMinAnimate = iMinAnimate;
            }

            /// <summary>
            /// Always must be set to (System.UInt32)Marshal.SizeOf(typeof(ANIMATIONINFO)).
            /// </summary>
            public System.UInt32 cbSize;

            /// <summary>
            /// If non-zero, minimize/restore animation is enabled, otherwise disabled.
            /// </summary>
            public System.Int32 iMinAnimate;
        }
//
//        // ALTERNATIVE K.I.S.S. VERSION
//        public const uint SPI_GETBEEP = 0x0001;
//        public const uint SPI_SETBEEP = 0x0002;
//        public const uint SPI_GETMOUSE = 0x0003;
//        public const uint SPI_SETMOUSE = 0x0004;
//        public const uint SPI_GETBORDER = 0x0005;
//        public const uint SPI_SETBORDER = 0x0006;
//        public const uint SPI_GETKEYBOARDSPEED = 0x000A;
//        public const uint SPI_SETKEYBOARDSPEED = 0x000B;
//        public const uint SPI_LANGDRIVER = 0x000C;
//        public const uint SPI_ICONHORIZONTALSPACING = 0x000D;
//        public const uint SPI_GETSCREENSAVETIMEOUT = 0x000E;
//        public const uint SPI_SETSCREENSAVETIMEOUT = 0x000F;
//        public const uint SPI_GETSCREENSAVEACTIVE = 0x0010;
//        public const uint SPI_SETSCREENSAVEACTIVE = 0x0011;
//        public const uint SPI_GETGRIDGRANULARITY = 0x0012;
//        public const uint SPI_SETGRIDGRANULARITY = 0x0013;
//        public const uint SPI_SETDESKWALLPAPER = 0x0014;
//        public const uint SPI_SETDESKPATTERN = 0x0015;
//        public const uint SPI_GETKEYBOARDDELAY = 0x0016;
//        public const uint SPI_SETKEYBOARDDELAY = 0x0017;
//        public const uint SPI_ICONVERTICALSPACING = 0x0018;
//        public const uint SPI_GETICONTITLEWRAP = 0x0019;
//        public const uint SPI_SETICONTITLEWRAP = 0x001A;
//        public const uint SPI_GETMENUDROPALIGNMENT = 0x001B;
//        public const uint SPI_SETMENUDROPALIGNMENT = 0x001C;
//        public const uint SPI_SETDOUBLECLKWIDTH = 0x001D;
//        public const uint SPI_SETDOUBLECLKHEIGHT = 0x001E;
//        public const uint SPI_GETICONTITLELOGFONT = 0x001F;
//        public const uint SPI_SETDOUBLECLICKTIME = 0x0020;
//        public const uint SPI_SETMOUSEBUTTONSWAP = 0x0021;
//        public const uint SPI_SETICONTITLELOGFONT = 0x0022;
//        public const uint SPI_GETFASTTASKSWITCH = 0x0023;
//        public const uint SPI_SETFASTTASKSWITCH = 0x0024;
//        public const uint SPI_SETDRAGFULLWINDOWS = 0x0025;
//        public const uint SPI_GETDRAGFULLWINDOWS = 0x0026;
//        public const uint SPI_GETNONCLIENTMETRICS = 0x0029;
//        public const uint SPI_SETNONCLIENTMETRICS = 0x002A;
//        public const uint SPI_GETMINIMIZEDMETRICS = 0x002B;
//        public const uint SPI_SETMINIMIZEDMETRICS = 0x002C;
//        public const uint SPI_GETICONMETRICS = 0x002D;
//        public const uint SPI_SETICONMETRICS = 0x002E;
//        public const uint SPI_SETWORKAREA = 0x002F;
        public const uint SPI_GETWORKAREA = 0x0030;
//        public const uint SPI_SETPENWINDOWS = 0x0031;
//        public const uint SPI_GETHIGHCONTRAST = 0x0042;
//        public const uint SPI_SETHIGHCONTRAST = 0x0043;
//        public const uint SPI_GETKEYBOARDPREF = 0x0044;
//        public const uint SPI_SETKEYBOARDPREF = 0x0045;
//        public const uint SPI_GETSCREENREADER = 0x0046;
//        public const uint SPI_SETSCREENREADER = 0x0047;
//        public const uint SPI_GETANIMATION = 0x0048;
//        public const uint SPI_SETANIMATION = 0x0049;
//        public const uint SPI_GETFONTSMOOTHING = 0x004A;
//        public const uint SPI_SETFONTSMOOTHING = 0x004B;
//        public const uint SPI_SETDRAGWIDTH = 0x004C;
//        public const uint SPI_SETDRAGHEIGHT = 0x004D;
//        public const uint SPI_SETHANDHELD = 0x004E;
//        public const uint SPI_GETLOWPOWERTIMEOUT = 0x004F;
//        public const uint SPI_GETPOWEROFFTIMEOUT = 0x0050;
//        public const uint SPI_SETLOWPOWERTIMEOUT = 0x0051;
//        public const uint SPI_SETPOWEROFFTIMEOUT = 0x0052;
//        public const uint SPI_GETLOWPOWERACTIVE = 0x0053;
//        public const uint SPI_GETPOWEROFFACTIVE = 0x0054;
//        public const uint SPI_SETLOWPOWERACTIVE = 0x0055;
//        public const uint SPI_SETPOWEROFFACTIVE = 0x0056;
//        public const uint SPI_SETICONS = 0x0058;
//        public const uint SPI_GETDEFAULTINPUTLANG = 0x0059;
//        public const uint SPI_SETDEFAULTINPUTLANG = 0x005A;
//        public const uint SPI_SETLANGTOGGLE = 0x005B;
//        public const uint SPI_GETWINDOWSEXTENSION = 0x005C;
//        public const uint SPI_SETMOUSETRAILS = 0x005D;
//        public const uint SPI_GETMOUSETRAILS = 0x005E;
//        public const uint SPI_SCREENSAVERRUNNING = 0x0061;
//        public const uint SPI_GETFILTERKEYS = 0x0032;
//        public const uint SPI_SETFILTERKEYS = 0x0033;
//        public const uint SPI_GETTOGGLEKEYS = 0x0034;
//        public const uint SPI_SETTOGGLEKEYS = 0x0035;
//        public const uint SPI_GETMOUSEKEYS = 0x0036;
//        public const uint SPI_SETMOUSEKEYS = 0x0037;
//        public const uint SPI_GETSHOWSOUNDS = 0x0038;
//        public const uint SPI_SETSHOWSOUNDS = 0x0039;
//        public const uint SPI_GETSTICKYKEYS = 0x003A;
//        public const uint SPI_SETSTICKYKEYS = 0x003B;
//        public const uint SPI_GETACCESSTIMEOUT = 0x003C;
//        public const uint SPI_SETACCESSTIMEOUT = 0x003D;
//        public const uint SPI_GETSERIALKEYS = 0x003E;
//        public const uint SPI_SETSERIALKEYS = 0x003F;
//        public const uint SPI_GETSOUNDSENTRY = 0x0040;
//        public const uint SPI_SETSOUNDSENTRY = 0x0041;
//        public const uint SPI_GETSNAPTODEFBUTTON = 0x005F;
//        public const uint SPI_SETSNAPTODEFBUTTON = 0x0060;
//        public const uint SPI_GETMOUSEHOVERWIDTH = 0x0062;
//        public const uint SPI_SETMOUSEHOVERWIDTH = 0x0063;
//        public const uint SPI_GETMOUSEHOVERHEIGHT = 0x0064;
//        public const uint SPI_SETMOUSEHOVERHEIGHT = 0x0065;
//        public const uint SPI_GETMOUSEHOVERTIME = 0x0066;
//        public const uint SPI_SETMOUSEHOVERTIME = 0x0067;
//        public const uint SPI_GETWHEELSCROLLLINES = 0x0068;
//        public const uint SPI_SETWHEELSCROLLLINES = 0x0069;
//        public const uint SPI_GETMENUSHOWDELAY = 0x006A;
//        public const uint SPI_SETMENUSHOWDELAY = 0x006B;
//        public const uint SPI_GETSHOWIMEUI = 0x006E;
//        public const uint SPI_SETSHOWIMEUI = 0x006F;
//        public const uint SPI_GETMOUSESPEED = 0x0070;
//        public const uint SPI_SETMOUSESPEED = 0x0071;
//        public const uint SPI_GETSCREENSAVERRUNNING = 0x0072;
//        public const uint SPI_GETDESKWALLPAPER = 0x0073;
//        public const uint SPI_GETACTIVEWINDOWTRACKING = 0x1000;
//        public const uint SPI_SETACTIVEWINDOWTRACKING = 0x1001;
//        public const uint SPI_GETMENUANIMATION = 0x1002;
//        public const uint SPI_SETMENUANIMATION = 0x1003;
//        public const uint SPI_GETCOMBOBOXANIMATION = 0x1004;
//        public const uint SPI_SETCOMBOBOXANIMATION = 0x1005;
//        public const uint SPI_GETLISTBOXSMOOTHSCROLLING = 0x1006;
//        public const uint SPI_SETLISTBOXSMOOTHSCROLLING = 0x1007;
//        public const uint SPI_GETGRADIENTCAPTIONS = 0x1008;
//        public const uint SPI_SETGRADIENTCAPTIONS = 0x1009;
//        public const uint SPI_GETKEYBOARDCUES = 0x100A;
//        public const uint SPI_SETKEYBOARDCUES = 0x100B;
//        public const uint SPI_GETMENUUNDERLINES = SPI_GETKEYBOARDCUES;
//        public const uint SPI_SETMENUUNDERLINES = SPI_SETKEYBOARDCUES;
//        public const uint SPI_GETACTIVEWNDTRKZORDER = 0x100C;
//        public const uint SPI_SETACTIVEWNDTRKZORDER = 0x100D;
//        public const uint SPI_GETHOTTRACKING = 0x100E;
//        public const uint SPI_SETHOTTRACKING = 0x100F;
//        public const uint SPI_GETMENUFADE = 0x1012;
//        public const uint SPI_SETMENUFADE = 0x1013;
//        public const uint SPI_GETSELECTIONFADE = 0x1014;
//        public const uint SPI_SETSELECTIONFADE = 0x1015;
//        public const uint SPI_GETTOOLTIPANIMATION = 0x1016;
//        public const uint SPI_SETTOOLTIPANIMATION = 0x1017;
//        public const uint SPI_GETTOOLTIPFADE = 0x1018;
//        public const uint SPI_SETTOOLTIPFADE = 0x1019;
//        public const uint SPI_GETCURSORSHADOW = 0x101A;
//        public const uint SPI_SETCURSORSHADOW = 0x101B;
//        public const uint SPI_GETMOUSESONAR = 0x101C;
//        public const uint SPI_SETMOUSESONAR = 0x101D;
//        public const uint SPI_GETMOUSECLICKLOCK = 0x101E;
//        public const uint SPI_SETMOUSECLICKLOCK = 0x101F;
//        public const uint SPI_GETMOUSEVANISH = 0x1020;
//        public const uint SPI_SETMOUSEVANISH = 0x1021;
//        public const uint SPI_GETFLATMENU = 0x1022;
//        public const uint SPI_SETFLATMENU = 0x1023;
//        public const uint SPI_GETDROPSHADOW = 0x1024;
//        public const uint SPI_SETDROPSHADOW = 0x1025;
//        public const uint SPI_GETBLOCKSENDINPUTRESETS = 0x1026;
//        public const uint SPI_SETBLOCKSENDINPUTRESETS = 0x1027;
//        public const uint SPI_GETUIEFFECTS = 0x103E;
//        public const uint SPI_SETUIEFFECTS = 0x103F;
        public const uint SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000;
        public const uint SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001;
//        public const uint SPI_GETACTIVEWNDTRKTIMEOUT = 0x2002;
//        public const uint SPI_SETACTIVEWNDTRKTIMEOUT = 0x2003;
//        public const uint SPI_GETFOREGROUNDFLASHCOUNT = 0x2004;
//        public const uint SPI_SETFOREGROUNDFLASHCOUNT = 0x2005;
//        public const uint SPI_GETCARETWIDTH = 0x2006;
//        public const uint SPI_SETCARETWIDTH = 0x2007;
//        public const uint SPI_GETMOUSECLICKLOCKTIME = 0x2008;
//        public const uint SPI_SETMOUSECLICKLOCKTIME = 0x2009;
//        public const uint SPI_GETFONTSMOOTHINGTYPE = 0x200A;
//        public const uint SPI_SETFONTSMOOTHINGTYPE = 0x200B;
//        public const uint SPI_GETFONTSMOOTHINGCONTRAST = 0x200C;
//        public const uint SPI_SETFONTSMOOTHINGCONTRAST = 0x200D;
//        public const uint SPI_GETFOCUSBORDERWIDTH = 0x200E;
//        public const uint SPI_SETFOCUSBORDERWIDTH = 0x200F;
//        public const uint SPI_GETFOCUSBORDERHEIGHT = 0x2010;
//        public const uint SPI_SETFOCUSBORDERHEIGHT = 0x2011;
//        public const uint SPI_GETFONTSMOOTHINGORIENTATION = 0x2012;
//        public const uint SPI_SETFONTSMOOTHINGORIENTATION = 0x2013;
    }
}
