using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;
using Win32;
using ThreadState = System.Diagnostics.ThreadState;

namespace WGestures.Core.Commands.Impl
{
    [Named("执行快捷键")]
    public class HotKeyCommand : AbstractCommand
    {
        public HotKeyCommand()
        {
            Modifiers = new List<VirtualKeyCode>();
            Keys = new List<VirtualKeyCode>();
        }

        public List<VirtualKeyCode> Modifiers { get; set; }

        public List<VirtualKeyCode> Keys { get; set; }
        private readonly InputSimulator _sim = new InputSimulator();


        public override void Execute()
        {
            if (Keys.Count + Modifiers.Count == 0) return;

            if (Keys.Count == 1 && (Keys[0] == VirtualKeyCode.VK_L) &&
                Modifiers.Count == 1 && (Modifiers[0] == VirtualKeyCode.LWIN || Modifiers[0] == VirtualKeyCode.RWIN))
            {
                User32.LockWorkStation();
                return;
            }


            //活动进程 未必 是活动root窗口进程, 就像clover
            var fgWindow = Native.GetForegroundWindow();
            var rootWindow = IntPtr.Zero;

            Debug.WriteLine(string.Format("FGWindow: {0:X}", fgWindow.ToInt32()));

            //如果没有前台窗口，或者前台窗口是任务栏，则使用鼠标指针下方的窗口？
            var useCursorWindow = false;
            if (fgWindow != IntPtr.Zero)
            {
                var className = new StringBuilder(32);
                Native.GetClassName(fgWindow, className, className.Capacity);

                //如果是任务栏 或者 窗口处于最小化状态
                if (className.ToString() == "Shell_TrayWnd")
                {
                    useCursorWindow = true;
                } //如果活动窗口与鼠标指针不在同一个屏幕
                else if (!IsCursorAndWindowSameScreen(fgWindow))
                {
                    useCursorWindow = true;
                }
                else
                {
                    rootWindow = Native.GetAncestor(fgWindow, Native.GetAncestorFlags.GetRoot);
                    if (IsWindowMinimized(rootWindow))
                    {
                        Debug.WriteLine("Use Cursor Window Cuz rootWindow is Minimized.");
                        useCursorWindow = true;
                    }
                }
            }
            else
            {
                useCursorWindow = true;
            }


            if (useCursorWindow)
            {
                Debug.WriteLine("* * Why Is fgWindow NULL?");

                Native.POINT pt;
                Native.GetCursorPos(out pt);
                fgWindow = Native.WindowFromPoint(pt);

                if (fgWindow == IntPtr.Zero) return;
            }

            if (rootWindow == IntPtr.Zero) rootWindow = Native.GetAncestor(fgWindow, Native.GetAncestorFlags.GetRoot);

            User32.SetForegroundWindow(fgWindow);

            uint pid;
            var fgThread = Native.GetWindowThreadProcessId(rootWindow, out pid);


            //失败可能原因之一：被杀毒软件或系统拦截

            try
            {
                foreach (var k in Modifiers)
                {
                    Debug.Write(k);
                    PerformKey(pid, fgThread, k);
                }

                foreach (var k in Keys)
                {
                    Debug.Write(k);
                    PerformKey(pid, fgThread, k);
                }


                foreach (var k in Keys)
                {
                    Debug.Write(k + " Up:");

                    PerformKey(pid, fgThread, k, true);
                }

                foreach (var k in Modifiers)
                {
                    Debug.Write(k + " Up:");

                    PerformKey(pid, fgThread, k, true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("发送按键的时候发生异常： " + ex);
                Native.TryResetKeys(Keys, Modifiers);
#if TEST
                throw;
#endif
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);



        }

        private static bool IsWindowMinimized(IntPtr hwnd)
        {
            int style = User32.GetWindowLong(hwnd, User32.GWL.GWL_STYLE);

            return (int)User32.WS.WS_MINIMIZE == (style & (int)User32.WS.WS_MINIMIZE);
        }

        private void PerformKey(uint pid, uint tid, VirtualKeyCode key, bool isUp = false)
        {

            Native.WaitForInputIdle(pid, tid, 100);

            if (!isUp)
            {
                _sim.Keyboard.KeyDown(key);

            }
            else
            {
                _sim.Keyboard.KeyUp(key);
            }

            Native.WaitForInputIdle(pid, tid, 20);

        }


        private static bool IsCursorAndWindowSameScreen(IntPtr win)
        {
            Native.POINT pt;
            Native.GetCursorPos(out pt);

            var fgWinScreen = Screen.FromHandle(win);
            var cursorScreen = Screen.FromPoint(pt.ToPoint());

            return fgWinScreen.Equals(cursorScreen);

        }

        public override string Description()
        {
            return HotKeyToString(Modifiers, Keys);
        }

        public static void ForceWindowIntoForeground(IntPtr window)
        {
            const uint LSFW_LOCK = 1;
            const uint LSFW_UNLOCK = 2;
            const int ASFW_ANY = -1; // by MSDN

            uint currentThread = Native.GetCurrentThreadId();

            IntPtr activeWindow = User32.GetForegroundWindow();
            //uint activeProcess;
            uint activeThread = User32.GetWindowThreadProcessId(activeWindow, IntPtr.Zero);

            uint windowProcess;
            uint windowThread = User32.GetWindowThreadProcessId(window, IntPtr.Zero);

            if (currentThread != activeThread)
                User32.AttachThreadInput(currentThread, activeThread, true);
            if (windowThread != currentThread)
                User32.AttachThreadInput(windowThread, currentThread, true);

            uint oldTimeout = 0, newTimeout = 0;
            User32.SystemParametersInfo(User32.SPI_GETFOREGROUNDLOCKTIMEOUT, 0, ref oldTimeout, 0);
            User32.SystemParametersInfo(User32.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, ref newTimeout, 0);
            User32.LockSetForegroundWindow(LSFW_UNLOCK);
            User32.AllowSetForegroundWindow(ASFW_ANY);

            User32.SetForegroundWindow(window);
            User32.ShowWindow(window, User32.SW.SW_RESTORE);

            User32.SystemParametersInfo(User32.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, ref oldTimeout, 0);

            if (currentThread != activeThread)
                User32.AttachThreadInput(currentThread, activeThread, false);
            if (windowThread != currentThread)
                User32.AttachThreadInput(windowThread, currentThread, false);
        }

        public static string HotKeyToString(ICollection<VirtualKeyCode> modifiers, ICollection<VirtualKeyCode> keys)
        {
            if (keys.Count != 0 || modifiers.Count != 0)
            {
                var sb = new StringBuilder(32);
                foreach (var k in modifiers)
                {
                    string str = "";
                    switch (k)
                    {
                        case VirtualKeyCode.MENU:
                        case VirtualKeyCode.RMENU:
                        case VirtualKeyCode.LMENU:
                            str = "Alt";
                            break;
                        case VirtualKeyCode.LCONTROL:
                        case VirtualKeyCode.RCONTROL:
                        case VirtualKeyCode.CONTROL:
                            str = "Ctrl";
                            break;
                        case VirtualKeyCode.RWIN:
                        case VirtualKeyCode.LWIN:
                            str = "Win";
                            break;
                        case VirtualKeyCode.SHIFT:
                        case VirtualKeyCode.LSHIFT:
                        case VirtualKeyCode.RSHIFT:
                            str = "Shift";
                            break;
                        default:
                            str = k.ToString();
                            break;
                    }

                    sb.Append(str);
                    sb.Append(" + ");
                }

                foreach (var k in keys)
                {
                    string str = k.ToString();
                    if (str.StartsWith("VK_")) str = str.Substring(3);

                    sb.Append(str);
                    sb.Append(" + ");
                }


                sb.Remove(sb.Length - 3, 3);
                return sb.ToString();
            }

            return "";
        }


    }
}