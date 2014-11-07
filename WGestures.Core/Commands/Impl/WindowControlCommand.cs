using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsInput;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;
using Win32;

namespace WGestures.Core.Commands.Impl
{
    [Named("窗口控制")]
    public class WindowControlCommand : AbstractCommand, IGestureContextAware
    {
        public enum WindowOperation
        {
            MAXIMIZE_RESTORE = 0, MINIMIZE, CLOSE, DOCK_LEFT, DOCK_RIGHT
        }

        public WindowOperation ChangeWindowStateTo { get; set; }

        public override void Execute()
        {
            var cursorWin = Native.WindowFromPoint(new Native.POINT() { x = Context.StartPoint.X, y = Context.StartPoint.Y });
           
            DoOperation(cursorWin);
        }

        private void DoOperation(IntPtr win)
        {
            while (true)
            {
                //topLevelWin是本进程（？）内的顶层窗口
                //rootWindow可能会跨进程
                var topLevelWin = Native.GetTopLevelWindow(win);
                var rootWin = Native.GetAncestor(topLevelWin, Native.GetAncestorFlags.GetRoot);

                if (rootWin == IntPtr.Zero) return;

                Debug.WriteLine(string.Format("win     : {0:X}", win.ToInt32()));
                Debug.WriteLine(string.Format("root    : {0:X}",rootWin.ToInt32()));
                Debug.WriteLine(string.Format("topLevel: {0:X}", topLevelWin.ToInt32()));

                var rootWinStyle = User32.GetWindowLong(rootWin, User32.GWL.GWL_STYLE);
                var topLevelWinstyle = User32.GetWindowLong(topLevelWin, User32.GWL.GWL_STYLE);

                switch (ChangeWindowStateTo)
                {
                    case WindowOperation.MAXIMIZE_RESTORE:
                        IntPtr winToControl;
                        if ((long) User32.WS.WS_MAXIMIZEBOX == (topLevelWinstyle & (long) User32.WS.WS_MAXIMIZEBOX))
                        {
                            winToControl = topLevelWin;
                        }
                        else if (topLevelWin != rootWin && (long) User32.WS.WS_MAXIMIZEBOX == (rootWinStyle & (long) User32.WS.WS_MAXIMIZEBOX))
                        {
                            winToControl = rootWin;
                        }
                        else //如果窗口都不响应， 考虑回滚为处理活动窗口
                        {
                            var fgWin = Native.GetForegroundWindow();
                            if (fgWin == win) return;

                            win = fgWin;
                            continue;
                        }

                        var wp = new User32.WINDOWPLACEMENT();
                        wp.length = Marshal.SizeOf(typeof (User32.WINDOWPLACEMENT));

                        if (!User32.GetWindowPlacement(rootWin, ref wp)) return;

                        if (wp.showCmd == (int) ShowWindowCommands.MAXIMIZED)
                        {
                            User32.ShowWindowAsync(winToControl, (int) ShowWindowCommands.NORMAL);
                        }
                        else
                        {
                            User32.ShowWindowAsync(winToControl, (int) ShowWindowCommands.MAXIMIZED);
                        }
                        return;

                    case WindowOperation.MINIMIZE:
                        if ((long) User32.WS.WS_MINIMIZEBOX == (rootWinStyle & (long) User32.WS.WS_MINIMIZEBOX))
                        {
                            User32.PostMessage(rootWin, User32.WM.WM_SYSCOMMAND, (int) User32.SysCommands.SC_MINIMIZE, 0);
                        }
                        else if (topLevelWin != rootWin && (long) User32.WS.WS_MINIMIZEBOX == (topLevelWinstyle & (long) User32.WS.WS_MINIMIZEBOX))
                        {
                            User32.PostMessage(topLevelWin, User32.WM.WM_SYSCOMMAND, (int) User32.SysCommands.SC_MINIMIZE, 0);
                        }
                        return;

                    case WindowOperation.CLOSE:
                        User32.PostMessage(rootWin, User32.WM.WM_SYSCOMMAND, (int) User32.SysCommands.SC_CLOSE, 0);
                        return;

                    default:
                        return;
                }
                break;
            }
        }

        public GestureContext Context { set; private get; }

        public override string Description()
        {
            switch (ChangeWindowStateTo)
            {
                case WindowOperation.MAXIMIZE_RESTORE:
                    return "最大化/恢复";
                case WindowOperation.MINIMIZE:
                    return "最小化";
                case WindowOperation.DOCK_LEFT:
                    return "左停靠";
                case WindowOperation.DOCK_RIGHT:
                    return "右停靠";
                default:
                    return "关闭窗口";
            }
        }

        internal enum ShowWindowCommands : int
        {
            HIDE = 0,
            NORMAL = 1,
            MINIMIZED = 2,
            MAXIMIZED = 3,
        }





    }
}
