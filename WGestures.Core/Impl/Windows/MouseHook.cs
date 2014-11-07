using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading;
using WGestures.Common.OsSpecific.Windows;
using Win32;

namespace WGestures.Core.Impl.Windows
{
    internal class MouseHook : IDisposable
    {
        public bool IsDisposed { get; private set; }
        private IntPtr _hookId;
        private uint _hookThreadNativeId;
        private Thread _hookThread;

        private Native.LowLevelMouseProc _hookProc;

        public class MouseHookEventArgs : EventArgs
        {
            public MouseMsg Msg { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }

            public IntPtr wParam { get; private set; }
            public IntPtr lParam { get; private set; }

            public bool Handled { get; set; }

            public MouseHookEventArgs(MouseMsg msg, int x, int y,IntPtr wParam, IntPtr lParam)
            {
                Msg = msg;
                X = x;
                Y = y;

                this.wParam = wParam;
                this.lParam = lParam;
            }
        }

        public delegate void MouseHookEventHandler(MouseHookEventArgs e);

        public event MouseHookEventHandler MouseHookEvent;
        public event Func<Native.MSG,bool> GotMessage;


        public MouseHook()
        {
            _hookProc = MouseHookProc;
        }

        public void Install()
        {
            if (_hookThread != null) throw new InvalidOperationException("钩子已经安装了");

            _hookThread = new Thread(() =>
            {
                _hookThreadNativeId = Native.GetCurrentThreadId();

                try
                {
                    _hookId = Native.SetMouseHook(_hookProc);

                    if (_hookId == IntPtr.Zero)
                    {
                        Debug.WriteLine("安装钩子失败");
                        _hookThreadNativeId = 0;
                        return;
                    }

                    Debug.WriteLine("钩子安装成功");

                    var @continue = true;
                    do
                    {
                        Native.MSG msg;
                        Native.GetMessage(out msg, IntPtr.Zero, 0, 0);

                        if (msg.message == (uint)User32.WM.WM_CLOSE)
                        {
                            @continue = false;
                        }
                        else if (GotMessage != null)
                        {
                            @continue = GotMessage(msg);
                        }
                        else
                        {
                            @continue = true;
                        }

                    } while (@continue);

                }
                finally
                {
                    if (_hookId != IntPtr.Zero) Native.UnhookWindowsHookEx(_hookId);
                }

                Debug.WriteLine("钩子线程结束");

                //GC.KeepAlive(hookProc);

            }) { IsBackground = true, Priority = ThreadPriority.AboveNormal, Name = "MouseHook钩子线程" };

            _hookThread.Start();
        }

        public void Uninstall()
        {
            if (_hookId == IntPtr.Zero || _hookThreadNativeId == 0) return;
            try
            {
                if (Native.UnhookWindowsHookEx(_hookId))
                {
                    Debug.WriteLine("钩子已卸载");
                }
                else
                {
                    Debug.WriteLine("卸载钩子失败");
                }


                //发送一个消息给钩子线程,使其GetMessage退出
                if (_hookThread != null && _hookThread.IsAlive)
                {
                    Native.PostThreadMessage(_hookThreadNativeId, (uint)User32.WM.WM_CLOSE, UIntPtr.Zero, IntPtr.Zero);

                    if (!_hookThread.Join(1000 * 3))
                    {
                        throw new TimeoutException("等待钩子线程结束超时");
                    }
                }

            }
            finally
            {

                _hookThreadNativeId = 0;
                _hookId = IntPtr.Zero;
                _hookThread = null;
            }

        }

        protected virtual IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                Debug.WriteLine("nCode < 0 ??");
                return Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
            }

            //注意：用这个API来过的鼠标位置，不会出现在迅雷上坐标值变为一半的问题。
            Native.POINT curPos;
            Native.GetCursorPos(out curPos);

            var args = new MouseHookEventArgs((MouseMsg)wParam, curPos.x, curPos.y,wParam,lParam);

            try
            {
                if (MouseHookEvent != null)
                {
                   
                    MouseHookEvent(args);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("MouseHookEvent中发生了未处理的异常，并且冒泡到了MouseHookProc。这是不应该出现的。"+e);
            }

            return args.Handled ?  new IntPtr(-1) : Native.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        #region dispose
        //If the method is invoked from the finalizer (disposing is false), 
        //other objects should not be accessed. 
        //The reason is that objects are finalized in an unpredictable order and so they,
        //or any of their dependencies, might already have been finalized.
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            if (disposing)
            {
                Uninstall();
            }
            else
            {
                Uninstall();
            }

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MouseHook()
        {
            Dispose(false);
        }
        #endregion
    }

    public enum MouseMsg
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

}
