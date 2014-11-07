using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using WGestures.Common.OsSpecific.Windows;
using Win32;

namespace WGestures.Core.Impl.Windows
{
    internal class Win32GestureContext : GestureContext
    {
        //todo: 活动窗口时shell的情况

        public override bool IsInFullScreenMode
        {
            get
            {
                var fgWindow = Native.GetAncestor(Native.GetForegroundWindow(), Native.GetAncestorFlags.GetRoot);
                var deskWindow = User32.GetDesktopWindow();
                var shellWindow = User32.GetShellWindow();


                if (fgWindow == IntPtr.Zero || // !IsTopMostWindow(fgWindow) ||
                    fgWindow == deskWindow ||
                    fgWindow == shellWindow) return false;


                GDI32.RECT fgRect, screenRect;
                User32.GetWindowRect(deskWindow, out screenRect);
                User32.GetWindowRect(fgWindow, out fgRect);

                if (fgRect == screenRect)
                {
                    var className = new StringBuilder(64);
                    User32.GetClassName(fgWindow, className, className.Capacity);

                    var classNameStr = className.ToString();
                    if (classNameStr == "WorkerW" || //桌面窗口
                        classNameStr == "CanvasWindow" ||
                        classNameStr == "ImmersiveLauncher" || //win8 开始屏幕
                        classNameStr == "Windows.UI.Core.CoreWindow") //win8 metro
                    {
                        return false;
                    }

                    Debug.WriteLine(string.Format("Window[{0:x}] IsInFullScreenMode:",fgWindow.ToInt32()));
                    return true;

                }

                return false;
            }
            
        }

        bool IsTopMostWindow(IntPtr hwnd)
        {
            User32.WINDOWINFO info = new User32.WINDOWINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            User32.GetWindowInfo(hwnd, ref info);

            const uint TOP_MOST = (uint) User32.WS_EX.WS_EX_TOPMOST;

            return (info.dwExStyle & TOP_MOST) == TOP_MOST;
        }
    }
}
