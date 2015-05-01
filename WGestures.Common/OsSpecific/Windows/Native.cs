using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using Win32;
using ThreadState = System.Diagnostics.ThreadState;
using System.IO;

namespace WGestures.Common.OsSpecific.Windows
{
    public static class Native
    {
        public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        public const int WH_MOUSE_LL = 14;
        public const int WH_KEYBOARD_LL = 13;

        /// <summary>
        /// 设置鼠标钩子
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        public static IntPtr SetMouseHook(LowLevelMouseProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, 
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public static int GetScreenDpi()
        {
            var scrDc = GetDC(IntPtr.Zero);

            int dpi = GDI32.GetDeviceCaps(scrDc, (int)GDI32.DeviceCap.LOGPIXELSX);

            ReleaseDC(IntPtr.Zero, scrDc);

            return dpi;
        }

        public static Rectangle GetScreenBounds()
        {
            IntPtr hwnd = User32.GetDesktopWindow();

            GDI32.RECT rect;
            User32.GetWindowRect(hwnd, out rect);

            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }


        //Todo: ForeGround VS Active ??
        public static uint GetActiveProcessId()
        {
            IntPtr hwnd = GetForegroundWindow();
            uint procId;
            GetWindowThreadProcessId(hwnd, out procId);

            return procId;
        }

        public static bool IsMouseButtonSwapped()
        {
            return GetSystemMetrics(SystemMetric.SM_SWAPBUTTON) != 0;
            
        }

        /*public static int GetParentProcessId(uint pid)
        {
            PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();

            //Get a handle to our own process
            IntPtr hProc = OpenProcess((ProcessAccessFlags)0x001F0FFF, false, pid);

            try
            {
                int sizeInfoReturned;
                int queryStatus = NtQueryInformationProcess(hProc, 0, ref pbi, pbi.Size, out sizeInfoReturned);
            }
            finally
            {
                if (!hProc.Equals(IntPtr.Zero))
                {
                    //Close handle and free allocated memory
                    CloseHandle(hProc);
                    hProc = IntPtr.Zero;
                }
            }

            return (int)pbi.InheritedFromUniqueProcessId;
        }*/


        // get the parent process given a pid
        public static uint GetParentProcess(uint pid)
        {
            uint parentProc = 0;
            IntPtr handleToSnapshot = IntPtr.Zero;
            try
            {
                PROCESSENTRY32 procEntry = new PROCESSENTRY32();
                procEntry.dwSize = (UInt32)Marshal.SizeOf(typeof(PROCESSENTRY32));
                handleToSnapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Process, 0);
                if (Process32First(handleToSnapshot, ref procEntry))
                {
                    do
                    {
                        if (pid == procEntry.th32ProcessID)
                        {
                            parentProc = procEntry.th32ParentProcessID;
                            break;
                        }
                    } while (Process32Next(handleToSnapshot, ref procEntry));
                }
                else
                {
                    throw new ApplicationException(string.Format("Failed with win32 error code {0}", Marshal.GetLastWin32Error()));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Can't get the process.", ex);
            }
            finally
            {
                // Must clean up the snapshot object!
                CloseHandle(handleToSnapshot);
            }
            return parentProc;
        }

        [DllImport("NTDLL.DLL", SetLastError = true)]
        static extern int NtQueryInformationProcess(IntPtr hProcess, PROCESSINFOCLASS pic,
        ref PROCESS_BASIC_INFORMATION pbi, int cb, out int pSize);

        //inner enum used only internally
        [Flags]
        private enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            Inherit = 0x80000000,
            All = 0x0000001F,
            NoHeaps = 0x40000000
        }
        //inner struct used only internally
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct PROCESSENTRY32
        {
            const int MAX_PATH = 260;
            internal UInt32 dwSize;
            internal UInt32 cntUsage;
            internal UInt32 th32ProcessID;
            internal IntPtr th32DefaultHeapID;
            internal UInt32 th32ModuleID;
            internal UInt32 cntThreads;
            internal UInt32 th32ParentProcessID;
            internal Int32 pcPriClassBase;
            internal UInt32 dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            internal string szExeFile;
        }

        [DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr CreateToolhelp32Snapshot([In]UInt32 dwFlags, [In]UInt32 th32ProcessID);

        [DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern bool Process32First([In]IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern bool Process32Next([In]IntPtr hSnapshot, ref PROCESSENTRY32 lppe);



        public static bool WaitForInputIdle(uint pid, uint tid, int timeout = 0)
        {
            //int pid;
            //int tid = GetWindowThreadProcessId(hWnd, out pid);
            //if (tid == 0 || pid == 0) return false;
            int tick = Environment.TickCount;
            do
            {
                //Thread.Sleep(10);

                if (IsThreadIdle(pid, tid))
                {
                    //Console.Write(" Idle ");
                    return true;
                }

                Thread.Sleep(10);
            } while (timeout > 0 && Environment.TickCount - tick < timeout);

            //Console.Write(" Timeout ");
            return false;
        }


        private static bool IsThreadIdle(uint pid, uint tid)
        {
            try
            {
                using (var prc = Process.GetProcessById((int)pid))
                {
                    var threads = prc.Threads.Cast<ProcessThread>().Where(t => tid == t.Id);
                    var processThreads = threads as ProcessThread[] ?? threads.ToArray();

                    if (!processThreads.Any()) return true;

                    using (var thr = processThreads.First())
                    {
                        return thr.ThreadState == ThreadState.Wait &&
                            thr.WaitReason == ThreadWaitReason.UserRequest;
                    }

                }
            }
            //进程可能已经退出
            catch (ArgumentException)
            {

                return true;
            }

        }

        public static string GetProcessFileFromWindow(IntPtr hwnd)
        {
            uint proc = GetProcessIdByWindowHandle(hwnd);
            return GetProcessFile(proc);
        }

        public static string GetProcessFile(uint proc)
        {
            /*using (var p = Process.GetProcessById((int) proc))
            {
                return p.MainModule.FileName;
            }*/

            int pathLength = 256;
            var procFullName = new StringBuilder(pathLength);

            var hProc = OpenProcess(Native.ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VMRead, false, proc);
           // QueryFullProcessImageNameW(hProc, 0, procFullName, ref pathLength);
            if (GetModuleFileNameEx(hProc, IntPtr.Zero, procFullName, (uint) pathLength) == 0)
            {
                throw new Exception("GetModuleFileNameEx Failed:"+GetLastError());
            }
            CloseHandle(hProc);
            
            
            //toLower
//            for(int i=0; i<procFullName.Length; i++)
//            {
//                if (Char.IsUpper(procFullName[i]))
//                {
//                    procFullName[i] = Char.ToLower(procFullName[i]);
//                }
//            }
            
            return procFullName.ToString();
        }

        //[DllImport("psapi.dll")]
        [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

        public static uint GetProcessIdByWindowHandle(IntPtr hwnd)
        {
            uint procId;
            GetWindowThreadProcessId(hwnd, out procId);

            return procId;
        }

        public static IntPtr GetHoveringWindow()
        {
            POINT cursorPos;
            GetCursorPos(out cursorPos);
            return WindowFromPoint(cursorPos);
        }

        public static uint GetHoveringProcessId()
        {
            return GetProcessIdByWindowHandle(GetHoveringWindow());
        }

        public static uint GetProcessIdFromPoint(POINT p)
        {
            return GetProcessIdByWindowHandle(WindowFromPoint(p));

        }

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsGUIThread([MarshalAs(UnmanagedType.Bool)] bool bConvert);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey); 

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetExitCodeProcess(uint hProcess, out uint lpExitCode);

        public static IntPtr GetTopLevelWindow(IntPtr window)
        {
            var temp = IntPtr.Zero;
            while ((temp = Native.GetParent(window)) != IntPtr.Zero) window = temp;

            return window;
        }

        public static IntPtr TopLevelWindowFromPoint(POINT p)
        {
            var window = Native.WindowFromPoint(p);
            var temp = IntPtr.Zero;
            while ((temp = Native.GetParent(window)) != IntPtr.Zero) window = temp;

            return window;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool ChangeWindowMessageFilter(uint msg, ChangeWindowMessageFilterFlags flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, ChangeWindowMessageFilterExAction action, ref CHANGEFILTERSTRUCT changeInfo);

        //==============================================================

        /// <summary>
        /// 通过向指定的按键发送KeyUp来恢复状态
        /// </summary>
        /// <param name="c"></param>
        /// <param name="allKeys"></param>
        public static void TryResetKeys(params IEnumerable<VirtualKeyCode>[] allKeys)
        {
            var dummyWnd = new NativeWindow();

            try
            {
                //如果被系统或者杀毒软件阻止， 则发送给自己的一个窗口，这样避免被在此拦截。
                dummyWnd.CreateHandle(new CreateParams(){ExStyle = (int) (User32.WS_EX.WS_EX_LAYERED | User32.WS_EX.WS_EX_TOOLWINDOW)});
                User32.ShowWindow(dummyWnd.Handle, User32.SW.SW_SHOWNORMAL);

                var sim = new InputSimulator();
                sim.Keyboard.Sleep(10);

                foreach (var keys in allKeys)
                {
                     foreach (var k in keys)
                    {
                        User32.SetForegroundWindow(dummyWnd.Handle);
                        sim.Keyboard.KeyUp(k);
                    }
                }

            }
            catch (Exception)
            {
                Debug.WriteLine("恢复键盘状态失败");
#if TEST
                throw;
#endif
            }
            finally
            {
                dummyWnd.DestroyHandle();
            }
        }

        public enum MessageFilterInfo : uint
        {
            None = 0, AlreadyAllowed = 1, AlreadyDisAllowed = 2, AllowedHigher = 3
        };

        public enum ChangeWindowMessageFilterExAction : uint
        {
            Reset = 0, Allow = 1, DisAllow = 2
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct CHANGEFILTERSTRUCT
        {
            public uint size;
            public MessageFilterInfo info;
        }

        public enum ChangeWindowMessageFilterFlags : uint
        {
            Add = 1, Remove = 2
        };

        // offset of window style value
        public const int GWL_STYLE = -16;

        // window style constants for scrollbars
        public const int WS_VSCROLL = 0x00200000;
        public const int WS_HSCROLL = 0x00100000;

        
        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,

            WM_MOUSEWHEEL = 0x020A,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0X0208,

            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public System.Drawing.Point ToPoint()
            {
                return new System.Drawing.Point(x, y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public UInt32 message;
            public IntPtr wParam;
            public IntPtr lParam;
            public UInt32 time;
            public POINT pt;
        }

        public enum Bool
        {
            False = 0,
            True
        };


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


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public Int32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public Int16 biPlanes;
            public Int16 biBitCount;
            public Int32 biCompression;
            public Int32 biSizeImage;
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public Int32 biClrUsed;
            public Int32 biClrImportant;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            /// <summary>
            /// A BITMAPINFOHEADER structure that contains information about the dimensions of color format.
            /// </summary>
            public BITMAPINFOHEADER bmiHeader;

            /// <summary>
            /// An array of RGBQUAD. The elements of the array that make up the color table.
            /// </summary>
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
            public RGBQUAD[] bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        enum VirtualKeyStates : int
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

        #region Window
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, 
            IntPtr hMod,
            uint dwThreadId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnOn);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        [DllImport("user32.dll")]
        public static extern bool WaitMessage();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax, uint wRemoveMsg);
        #endregion

        #region Hooking
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        #endregion

        #region GDI
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BITMAPINFO pbmi,
           uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        public static extern int GetObject(IntPtr hgdiobj, int cbBuffer, IntPtr lpvObject);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);
        #endregion


        [DllImport("psapi.dll")]
        public static extern bool EmptyWorkingSet(IntPtr hProcess);


        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);


        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();

        [DllImport("kernel32.dll")]
        public static extern void OutputDebugString(string lpOutputString);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool QueryFullProcessImageNameW(IntPtr hProcess, uint dwFlags,
                                                      [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpExeName,
                                                      ref int lpdwSize);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
                                         uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(uint threadId, uint msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool UpdateLayeredWindowIndirect(IntPtr hwnd, ref UPDATELAYEREDWINDOWINFO pULWInfo);

        [DllImport("USER32.dll")]
        static extern short GetKeyState(VirtualKeyStates nVirtKey);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);


        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        public enum GetAncestorFlags
        {
            /// <summary>
            /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function. 
            /// </summary>
            GetParent = 1,
            /// <summary>
            /// Retrieves the root window by walking the chain of parent windows.
            /// </summary>
            GetRoot = 2,
            /// <summary>
            /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent. 
            /// </summary>
            GetRootOwner = 3
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct UPDATELAYEREDWINDOWINFO
        {
            public uint cbSize;
            public IntPtr hdcDst;
            public unsafe Point* pptDst;
            public unsafe Size* psize;
            public IntPtr hdcSrc;
            public unsafe Point* pptSrc;
            public uint crKey;
            public unsafe BLENDFUNCTION* pblend;
            public uint dwFlags;
            public unsafe GDI32.RECT* prcDirty;
        }


    /// <summary>
    /// Flags used with the Windows API (User32.dll):GetSystemMetrics(SystemMetric smIndex)
    ///   
    /// This Enum and declaration signature was written by Gabriel T. Sharp
    /// ai_productions@verizon.net or osirisgothra@hotmail.com
    /// Obtained on pinvoke.net, please contribute your code to support the wiki!
    /// </summary>
    public enum SystemMetric:int
    {
        /// <summary>
        /// The flags that specify how the system arranged minimized windows. For more information, see the Remarks section in this topic.
        /// </summary>
        SM_ARRANGE = 56,

        /// <summary>
        /// The value that specifies how the system is started: 
        /// 0 Normal boot
        /// 1 Fail-safe boot
        /// 2 Fail-safe with network boot
        /// A fail-safe boot (also called SafeBoot, Safe Mode, or Clean Boot) bypasses the user startup files.
        /// </summary>
        SM_CLEANBOOT = 67,

        /// <summary>
        /// The number of display monitors on a desktop. For more information, see the Remarks section in this topic.
        /// </summary>
        SM_CMONITORS = 80,

        /// <summary>
        /// The number of buttons on a mouse, or zero if no mouse is installed.
        /// </summary>
        SM_CMOUSEBUTTONS = 43,

        /// <summary>
        /// The width of a window border, in pixels. This is equivalent to the SM_CXEDGE value for windows with the 3-D look.
        /// </summary>
        SM_CXBORDER = 5,

        /// <summary>
        /// The width of a cursor, in pixels. The system cannot create cursors of other sizes.
        /// </summary>
        SM_CXCURSOR = 13,

        /// <summary>
        /// This value is the same as SM_CXFIXEDFRAME.
        /// </summary>
        SM_CXDLGFRAME = 7,

        /// <summary>
        /// The width of the rectangle around the location of a first click in a double-click sequence, in pixels. ,
        /// The second click must occur within the rectangle that is defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system
        /// to consider the two clicks a double-click. The two clicks must also occur within a specified time.
        /// To set the width of the double-click rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKWIDTH.
        /// </summary>
        SM_CXDOUBLECLK = 36,

        /// <summary>
        /// The number of pixels on either side of a mouse-down point that the mouse pointer can move before a drag operation begins. 
        /// This allows the user to click and release the mouse button easily without unintentionally starting a drag operation. 
        /// If this value is negative, it is subtracted from the left of the mouse-down point and added to the right of it.
        /// </summary>
        SM_CXDRAG = 68,

        /// <summary>
        /// The width of a 3-D border, in pixels. This metric is the 3-D counterpart of SM_CXBORDER.
        /// </summary>
        SM_CXEDGE = 45,

        /// <summary>
        /// The thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels.
        /// SM_CXFIXEDFRAME is the height of the horizontal border, and SM_CYFIXEDFRAME is the width of the vertical border.
        /// This value is the same as SM_CXDLGFRAME.
        /// </summary>
        SM_CXFIXEDFRAME = 7,

        /// <summary>
        /// The width of the left and right edges of the focus rectangle that the DrawFocusRectdraws. 
        /// This value is in pixels. 
        /// Windows 2000:  This value is not supported.
        /// </summary>
        SM_CXFOCUSBORDER = 83,

        /// <summary>
        /// This value is the same as SM_CXSIZEFRAME.
        /// </summary>
        SM_CXFRAME = 32,

        /// <summary>
        /// The width of the client area for a full-screen window on the primary display monitor, in pixels. 
        /// To get the coordinates of the portion of the screen that is not obscured by the system taskbar or by application desktop toolbars, 
        /// call the SystemParametersInfofunction with the SPI_GETWORKAREA value.
        /// </summary>
        SM_CXFULLSCREEN = 16,

        /// <summary>
        /// The width of the arrow bitmap on a horizontal scroll bar, in pixels.
        /// </summary>
        SM_CXHSCROLL = 21,

        /// <summary>
        /// The width of the thumb box in a horizontal scroll bar, in pixels.
        /// </summary>
        SM_CXHTHUMB = 10,

        /// <summary>
        /// The default width of an icon, in pixels. The LoadIcon function can load only icons with the dimensions 
        /// that SM_CXICON and SM_CYICON specifies.
        /// </summary>
        SM_CXICON = 11,

        /// <summary>
        /// The width of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size 
        /// SM_CXICONSPACING by SM_CYICONSPACING when arranged. This value is always greater than or equal to SM_CXICON.
        /// </summary>
        SM_CXICONSPACING = 38,

        /// <summary>
        /// The default width, in pixels, of a maximized top-level window on the primary display monitor.
        /// </summary>
        SM_CXMAXIMIZED = 61,

        /// <summary>
        /// The default maximum width of a window that has a caption and sizing borders, in pixels. 
        /// This metric refers to the entire desktop. The user cannot drag the window frame to a size larger than these dimensions.
        /// A window can override this value by processing the WM_GETMINMAXINFO message.
        /// </summary>
        SM_CXMAXTRACK = 59,

        /// <summary>
        /// The width of the default menu check-mark bitmap, in pixels.
        /// </summary>
        SM_CXMENUCHECK = 71,

        /// <summary>
        /// The width of menu bar buttons, such as the child window close button that is used in the multiple document interface, in pixels.
        /// </summary>
        SM_CXMENUSIZE = 54,

        /// <summary>
        /// The minimum width of a window, in pixels.
        /// </summary>
        SM_CXMIN = 28,

        /// <summary>
        /// The width of a minimized window, in pixels.
        /// </summary>
        SM_CXMINIMIZED = 57,

        /// <summary>
        /// The width of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged. 
        /// This value is always greater than or equal to SM_CXMINIMIZED.
        /// </summary>
        SM_CXMINSPACING = 47,

        /// <summary>
        /// The minimum tracking width of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions. 
        /// A window can override this value by processing the WM_GETMINMAXINFO message.
        /// </summary>
        SM_CXMINTRACK = 34,

        /// <summary>
        /// The amount of border padding for captioned windows, in pixels. Windows XP/2000:  This value is not supported.
        /// </summary>
        SM_CXPADDEDBORDER = 92,

        /// <summary>
        /// The width of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
        /// GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
        /// </summary>
        SM_CXSCREEN = 0,

        /// <summary>
        /// The width of a button in a window caption or title bar, in pixels.
        /// </summary>
        SM_CXSIZE = 30,

        /// <summary>
        /// The thickness of the sizing border around the perimeter of a window that can be resized, in pixels. 
        /// SM_CXSIZEFRAME is the width of the horizontal border, and SM_CYSIZEFRAME is the height of the vertical border. 
        /// This value is the same as SM_CXFRAME.
        /// </summary>
        SM_CXSIZEFRAME = 32,

        /// <summary>
        /// The recommended width of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
        /// </summary>
        SM_CXSMICON = 49,

        /// <summary>
        /// The width of small caption buttons, in pixels.
        /// </summary>
        SM_CXSMSIZE = 52,

        /// <summary>
        /// The width of the virtual screen, in pixels. The virtual screen is the bounding rectangle of all display monitors. 
        /// The SM_XVIRTUALSCREEN metric is the coordinates for the left side of the virtual screen.
        /// </summary>
        SM_CXVIRTUALSCREEN = 78,

        /// <summary>
        /// The width of a vertical scroll bar, in pixels.
        /// </summary>
        SM_CXVSCROLL = 2,

        /// <summary>
        /// The height of a window border, in pixels. This is equivalent to the SM_CYEDGE value for windows with the 3-D look.
        /// </summary>
        SM_CYBORDER = 6,

        /// <summary>
        /// The height of a caption area, in pixels.
        /// </summary>
        SM_CYCAPTION = 4,

        /// <summary>
        /// The height of a cursor, in pixels. The system cannot create cursors of other sizes.
        /// </summary>
        SM_CYCURSOR = 14,

        /// <summary>
        /// This value is the same as SM_CYFIXEDFRAME.
        /// </summary>
        SM_CYDLGFRAME = 8,

        /// <summary>
        /// The height of the rectangle around the location of a first click in a double-click sequence, in pixels. 
        /// The second click must occur within the rectangle defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider 
        /// the two clicks a double-click. The two clicks must also occur within a specified time. To set the height of the double-click 
        /// rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKHEIGHT.
        /// </summary>
        SM_CYDOUBLECLK = 37,

        /// <summary>
        /// The number of pixels above and below a mouse-down point that the mouse pointer can move before a drag operation begins.
        /// This allows the user to click and release the mouse button easily without unintentionally starting a drag operation. 
        /// If this value is negative, it is subtracted from above the mouse-down point and added below it.
        /// </summary>
        SM_CYDRAG = 69,

        /// <summary>
        /// The height of a 3-D border, in pixels. This is the 3-D counterpart of SM_CYBORDER.
        /// </summary>
        SM_CYEDGE = 46,

        /// <summary>
        /// The thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels. 
        /// SM_CXFIXEDFRAME is the height of the horizontal border, and SM_CYFIXEDFRAME is the width of the vertical border. 
        /// This value is the same as SM_CYDLGFRAME.
        /// </summary>
        SM_CYFIXEDFRAME = 8,

        /// <summary>
        /// The height of the top and bottom edges of the focus rectangle drawn byDrawFocusRect. 
        /// This value is in pixels. 
        /// Windows 2000:  This value is not supported.
        /// </summary>
        SM_CYFOCUSBORDER = 84,

        /// <summary>
        /// This value is the same as SM_CYSIZEFRAME.
        /// </summary>
        SM_CYFRAME = 33,

        /// <summary>
        /// The height of the client area for a full-screen window on the primary display monitor, in pixels. 
        /// To get the coordinates of the portion of the screen not obscured by the system taskbar or by application desktop toolbars,
        /// call the SystemParametersInfo function with the SPI_GETWORKAREA value.
        /// </summary>
        SM_CYFULLSCREEN = 17,

        /// <summary>
        /// The height of a horizontal scroll bar, in pixels.
        /// </summary>
        SM_CYHSCROLL = 3,

        /// <summary>
        /// The default height of an icon, in pixels. The LoadIcon function can load only icons with the dimensions SM_CXICON and SM_CYICON.
        /// </summary>
        SM_CYICON = 12,

        /// <summary>
        /// The height of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size 
        /// SM_CXICONSPACING by SM_CYICONSPACING when arranged. This value is always greater than or equal to SM_CYICON.
        /// </summary>
        SM_CYICONSPACING = 39,

        /// <summary>
        /// For double byte character set versions of the system, this is the height of the Kanji window at the bottom of the screen, in pixels.
        /// </summary>
        SM_CYKANJIWINDOW = 18,

        /// <summary>
        /// The default height, in pixels, of a maximized top-level window on the primary display monitor.
        /// </summary>
        SM_CYMAXIMIZED = 62,

        /// <summary>
        /// The default maximum height of a window that has a caption and sizing borders, in pixels. This metric refers to the entire desktop. 
        /// The user cannot drag the window frame to a size larger than these dimensions. A window can override this value by processing 
        /// the WM_GETMINMAXINFO message.
        /// </summary>
        SM_CYMAXTRACK = 60,

        /// <summary>
        /// The height of a single-line menu bar, in pixels.
        /// </summary>
        SM_CYMENU = 15,

        /// <summary>
        /// The height of the default menu check-mark bitmap, in pixels.
        /// </summary>
        SM_CYMENUCHECK = 72,

        /// <summary>
        /// The height of menu bar buttons, such as the child window close button that is used in the multiple document interface, in pixels.
        /// </summary>
        SM_CYMENUSIZE = 55,

        /// <summary>
        /// The minimum height of a window, in pixels.
        /// </summary>
        SM_CYMIN = 29,

        /// <summary>
        /// The height of a minimized window, in pixels.
        /// </summary>
        SM_CYMINIMIZED = 58,

        /// <summary>
        /// The height of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged. 
        /// This value is always greater than or equal to SM_CYMINIMIZED.
        /// </summary>
        SM_CYMINSPACING = 48,

        /// <summary>
        /// The minimum tracking height of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions. 
        /// A window can override this value by processing the WM_GETMINMAXINFO message.
        /// </summary>
        SM_CYMINTRACK = 35,

        /// <summary>
        /// The height of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
        /// GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, VERTRES).
        /// </summary>
        SM_CYSCREEN = 1,

        /// <summary>
        /// The height of a button in a window caption or title bar, in pixels.
        /// </summary>
        SM_CYSIZE = 31,

        /// <summary>
        /// The thickness of the sizing border around the perimeter of a window that can be resized, in pixels. 
        /// SM_CXSIZEFRAME is the width of the horizontal border, and SM_CYSIZEFRAME is the height of the vertical border. 
        /// This value is the same as SM_CYFRAME.
        /// </summary>
        SM_CYSIZEFRAME = 33,

        /// <summary>
        /// The height of a small caption, in pixels.
        /// </summary>
        SM_CYSMCAPTION = 51,

        /// <summary>
        /// The recommended height of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
        /// </summary>
        SM_CYSMICON = 50,

        /// <summary>
        /// The height of small caption buttons, in pixels.
        /// </summary>
        SM_CYSMSIZE = 53,

        /// <summary>
        /// The height of the virtual screen, in pixels. The virtual screen is the bounding rectangle of all display monitors. 
        /// The SM_YVIRTUALSCREEN metric is the coordinates for the top of the virtual screen.
        /// </summary>
        SM_CYVIRTUALSCREEN = 79,

        /// <summary>
        /// The height of the arrow bitmap on a vertical scroll bar, in pixels.
        /// </summary>
        SM_CYVSCROLL = 20,

        /// <summary>
        /// The height of the thumb box in a vertical scroll bar, in pixels.
        /// </summary>
        SM_CYVTHUMB = 9,

        /// <summary>
        /// Nonzero if User32.dll supports DBCS; otherwise, 0.
        /// </summary>
        SM_DBCSENABLED = 42,

        /// <summary>
        /// Nonzero if the debug version of User.exe is installed; otherwise, 0.
        /// </summary>
        SM_DEBUG = 22,

        /// <summary>
        /// Nonzero if the current operating system is Windows 7 or Windows Server 2008 R2 and the Tablet PC Input 
        /// service is started; otherwise, 0. The return value is a bitmask that specifies the type of digitizer input supported by the device. 
        /// For more information, see Remarks. 
        /// Windows Server 2008, Windows Vista, and Windows XP/2000:  This value is not supported.
        /// </summary>
        SM_DIGITIZER = 94,

        /// <summary>
        /// Nonzero if Input Method Manager/Input Method Editor features are enabled; otherwise, 0. 
        /// SM_IMMENABLED indicates whether the system is ready to use a Unicode-based IME on a Unicode application. 
        /// To ensure that a language-dependent IME works, check SM_DBCSENABLED and the system ANSI code page.
        /// Otherwise the ANSI-to-Unicode conversion may not be performed correctly, or some components like fonts
        /// or registry settings may not be present.
        /// </summary>
        SM_IMMENABLED = 82,

        /// <summary>
        /// Nonzero if there are digitizers in the system; otherwise, 0. SM_MAXIMUMTOUCHES returns the aggregate maximum of the 
        /// maximum number of contacts supported by every digitizer in the system. If the system has only single-touch digitizers, 
        /// the return value is 1. If the system has multi-touch digitizers, the return value is the number of simultaneous contacts 
        /// the hardware can provide. Windows Server 2008, Windows Vista, and Windows XP/2000:  This value is not supported.
        /// </summary>
        SM_MAXIMUMTOUCHES = 95,

        /// <summary>
        /// Nonzero if the current operating system is the Windows XP, Media Center Edition, 0 if not.
        /// </summary>
        SM_MEDIACENTER = 87,

        /// <summary>
        /// Nonzero if drop-down menus are right-aligned with the corresponding menu-bar item; 0 if the menus are left-aligned.
        /// </summary>
        SM_MENUDROPALIGNMENT = 40,

        /// <summary>
        /// Nonzero if the system is enabled for Hebrew and Arabic languages, 0 if not.
        /// </summary>
        SM_MIDEASTENABLED = 74,

        /// <summary>
        /// Nonzero if a mouse is installed; otherwise, 0. This value is rarely zero, because of support for virtual mice and because 
        /// some systems detect the presence of the port instead of the presence of a mouse.
        /// </summary>
        SM_MOUSEPRESENT = 19,

        /// <summary>
        /// Nonzero if a mouse with a horizontal scroll wheel is installed; otherwise 0.
        /// </summary>
        SM_MOUSEHORIZONTALWHEELPRESENT = 91,

        /// <summary>
        /// Nonzero if a mouse with a vertical scroll wheel is installed; otherwise 0.
        /// </summary>
        SM_MOUSEWHEELPRESENT = 75,

        /// <summary>
        /// The least significant bit is set if a network is present; otherwise, it is cleared. The other bits are reserved for future use.
        /// </summary>
        SM_NETWORK = 63,

        /// <summary>
        /// Nonzero if the Microsoft Windows for Pen computing extensions are installed; zero otherwise.
        /// </summary>
        SM_PENWINDOWS = 41,

        /// <summary>
        /// This system metric is used in a Terminal Services environment to determine if the current Terminal Server session is 
        /// being remotely controlled. Its value is nonzero if the current session is remotely controlled; otherwise, 0. 
        /// You can use terminal services management tools such as Terminal Services Manager (tsadmin.msc) and shadow.exe to 
        /// control a remote session. When a session is being remotely controlled, another user can view the contents of that session 
        /// and potentially interact with it.
        /// </summary>
        SM_REMOTECONTROL = 0x2001,

        /// <summary>
        /// This system metric is used in a Terminal Services environment. If the calling process is associated with a Terminal Services 
        /// client session, the return value is nonzero. If the calling process is associated with the Terminal Services console session, 
        /// the return value is 0. 
        /// Windows Server 2003 and Windows XP:  The console session is not necessarily the physical console. 
        /// For more information, seeWTSGetActiveConsoleSessionId.
        /// </summary>
        SM_REMOTESESSION = 0x1000,

        /// <summary>
        /// Nonzero if all the display monitors have the same color format, otherwise, 0. Two displays can have the same bit depth,
        /// but different color formats. For example, the red, green, and blue pixels can be encoded with different numbers of bits, 
        /// or those bits can be located in different places in a pixel color value.
        /// </summary>
        SM_SAMEDISPLAYFORMAT = 81,

        /// <summary>
        /// This system metric should be ignored; it always returns 0.
        /// </summary>
        SM_SECURE = 44,

        /// <summary>
        /// The build number if the system is Windows Server 2003 R2; otherwise, 0.
        /// </summary>
        SM_SERVERR2 = 89,

        /// <summary>
        /// Nonzero if the user requires an application to present information visually in situations where it would otherwise present 
        /// the information only in audible form; otherwise, 0.
        /// </summary>
        SM_SHOWSOUNDS = 70,

        /// <summary>
        /// Nonzero if the current session is shutting down; otherwise, 0. Windows 2000:  This value is not supported.
        /// </summary>
        SM_SHUTTINGDOWN = 0x2000,

        /// <summary>
        /// Nonzero if the computer has a low-end (slow) processor; otherwise, 0.
        /// </summary>
        SM_SLOWMACHINE = 73,

        /// <summary>
        /// Nonzero if the current operating system is Windows 7 Starter Edition, Windows Vista Starter, or Windows XP Starter Edition; otherwise, 0.
        /// </summary>
        SM_STARTER = 88,

        /// <summary>
        /// Nonzero if the meanings of the left and right mouse buttons are swapped; otherwise, 0.
        /// </summary>
        SM_SWAPBUTTON = 23,

        /// <summary>
        /// Nonzero if the current operating system is the Windows XP Tablet PC edition or if the current operating system is Windows Vista
        /// or Windows 7 and the Tablet PC Input service is started; otherwise, 0. The SM_DIGITIZER setting indicates the type of digitizer 
        /// input supported by a device running Windows 7 or Windows Server 2008 R2. For more information, see Remarks.
        /// </summary>
        SM_TABLETPC = 86,

        /// <summary>
        /// The coordinates for the left side of the virtual screen. The virtual screen is the bounding rectangle of all display monitors. 
        /// The SM_CXVIRTUALSCREEN metric is the width of the virtual screen.
        /// </summary>
        SM_XVIRTUALSCREEN = 76,

        /// <summary>
        /// The coordinates for the top of the virtual screen. The virtual screen is the bounding rectangle of all display monitors. 
        /// The SM_CYVIRTUALSCREEN metric is the height of the virtual screen.
        /// </summary>
        SM_YVIRTUALSCREEN = 77,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct PROCESS_BASIC_INFORMATION
    {
        public IntPtr ExitStatus;
        public IntPtr PebBaseAddress;
        public IntPtr AffinityMask;
        public IntPtr BasePriority;
        public UIntPtr UniqueProcessId;
        public IntPtr InheritedFromUniqueProcessId;

        public int Size
        {
            get { return (int)Marshal.SizeOf(typeof(PROCESS_BASIC_INFORMATION)); }
        }
    }

    private enum PROCESSINFOCLASS : int
    {
        ProcessBasicInformation = 0, // 0, q: PROCESS_BASIC_INFORMATION, PROCESS_EXTENDED_BASIC_INFORMATION
        ProcessQuotaLimits, // qs: QUOTA_LIMITS, QUOTA_LIMITS_EX
        ProcessIoCounters, // q: IO_COUNTERS
        ProcessVmCounters, // q: VM_COUNTERS, VM_COUNTERS_EX
        ProcessTimes, // q: KERNEL_USER_TIMES
        ProcessBasePriority, // s: KPRIORITY
        ProcessRaisePriority, // s: ULONG
        ProcessDebugPort, // q: HANDLE
        ProcessExceptionPort, // s: HANDLE
        ProcessAccessToken, // s: PROCESS_ACCESS_TOKEN
        ProcessLdtInformation, // 10
        ProcessLdtSize,
        ProcessDefaultHardErrorMode, // qs: ULONG
        ProcessIoPortHandlers, // (kernel-mode only)
        ProcessPooledUsageAndLimits, // q: POOLED_USAGE_AND_LIMITS
        ProcessWorkingSetWatch, // q: PROCESS_WS_WATCH_INFORMATION[]; s: void
        ProcessUserModeIOPL,
        ProcessEnableAlignmentFaultFixup, // s: BOOLEAN
        ProcessPriorityClass, // qs: PROCESS_PRIORITY_CLASS
        ProcessWx86Information,
        ProcessHandleCount, // 20, q: ULONG, PROCESS_HANDLE_INFORMATION
        ProcessAffinityMask, // s: KAFFINITY
        ProcessPriorityBoost, // qs: ULONG
        ProcessDeviceMap, // qs: PROCESS_DEVICEMAP_INFORMATION, PROCESS_DEVICEMAP_INFORMATION_EX
        ProcessSessionInformation, // q: PROCESS_SESSION_INFORMATION
        ProcessForegroundInformation, // s: PROCESS_FOREGROUND_BACKGROUND
        ProcessWow64Information, // q: ULONG_PTR
        ProcessImageFileName, // q: UNICODE_STRING
        ProcessLUIDDeviceMapsEnabled, // q: ULONG
        ProcessBreakOnTermination, // qs: ULONG
        ProcessDebugObjectHandle, // 30, q: HANDLE
        ProcessDebugFlags, // qs: ULONG
        ProcessHandleTracing, // q: PROCESS_HANDLE_TRACING_QUERY; s: size 0 disables, otherwise enables
        ProcessIoPriority, // qs: ULONG
        ProcessExecuteFlags, // qs: ULONG
        ProcessResourceManagement,
        ProcessCookie, // q: ULONG
        ProcessImageInformation, // q: SECTION_IMAGE_INFORMATION
        ProcessCycleTime, // q: PROCESS_CYCLE_TIME_INFORMATION
        ProcessPagePriority, // q: ULONG
        ProcessInstrumentationCallback, // 40
        ProcessThreadStackAllocation, // s: PROCESS_STACK_ALLOCATION_INFORMATION, PROCESS_STACK_ALLOCATION_INFORMATION_EX
        ProcessWorkingSetWatchEx, // q: PROCESS_WS_WATCH_INFORMATION_EX[]
        ProcessImageFileNameWin32, // q: UNICODE_STRING
        ProcessImageFileMapping, // q: HANDLE (input)
        ProcessAffinityUpdateMode, // qs: PROCESS_AFFINITY_UPDATE_MODE
        ProcessMemoryAllocationMode, // qs: PROCESS_MEMORY_ALLOCATION_MODE
        ProcessGroupInformation, // q: USHORT[]
        ProcessTokenVirtualizationEnabled, // s: ULONG
        ProcessConsoleHostProcess, // q: ULONG_PTR
        ProcessWindowInformation, // 50, q: PROCESS_WINDOW_INFORMATION
        ProcessHandleInformation, // q: PROCESS_HANDLE_SNAPSHOT_INFORMATION // since WIN8
        ProcessMitigationPolicy, // s: PROCESS_MITIGATION_POLICY_INFORMATION
        ProcessDynamicFunctionTableInformation,
        ProcessHandleCheckingMode,
        ProcessKeepAliveCount, // q: PROCESS_KEEPALIVE_COUNT_INFORMATION
        ProcessRevokeFileHandles, // s: PROCESS_REVOKE_FILE_HANDLES_INFORMATION
        MaxProcessInfoClass
    };


    }
}
