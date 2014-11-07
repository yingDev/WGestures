using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WGestures.Common.OsSpecific.Windows
{
    public class ClipboardMonitor : NativeWindow
    {
        public ClipboardMonitor()
        {
            var cp = new CreateParams();

            CreateHandle(cp);
        }

        private bool _listenerAdded;

        public class ClipbardUpdatedEventArgs : EventArgs
        {
            public bool Handled { get; set; }
        }

        public event Action<ClipbardUpdatedEventArgs> ClipboardUpdated;
        public event Action MonitorRegistered;

        protected virtual void OnMonitorRegistered()
        {
            var handler = MonitorRegistered;
            if (handler != null) handler();
        }

        protected virtual void OnClipboardUpdated(ClipbardUpdatedEventArgs args)
        {
            var handler = ClipboardUpdated;
            if (handler != null) handler(args);
        }

        public void StartMonitor()
        {
            var ok = AddClipboardFormatListener(this.Handle);
            if (!_listenerAdded && !ok) throw new Exception("未能注册剪贴板监听器");

            _listenerAdded = true;
            OnMonitorRegistered();
        }

        public void StopMonitor()
        {
            var ok = RemoveClipboardFormatListener(this.Handle);
            if (_listenerAdded && !ok) throw new Exception("未能移除剪贴板监听器");

            _listenerAdded = false;

        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_CLIPBOARDUPDATE:
                    var args = new ClipbardUpdatedEventArgs();
                    OnClipboardUpdated(args);
                    if (args.Handled) m.Result = IntPtr.Zero;
                    return;
                case WM_DESTROY:
#if DEBUG
                    Console.WriteLine("ClipboardMonitor: WM_DESTROY");
#endif
                    StopMonitor();
                    break;
                case WM_CLOSE:
#if DEBUG
                    Console.WriteLine("ClipboardMonitor: WM_CLOSE");
#endif
                    StopMonitor();
                    break;
            }

            base.WndProc(ref m);
        }


        private const int WM_CLIPBOARDUPDATE = 0x031D;
        private const int WM_DESTROY = 0x0002;
        private const int WM_CLOSE = 0x0010;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

    }

}
