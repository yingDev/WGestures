using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WGestures.App.Gui.Windows.Controls
{

    internal class AlwaysSelectedListView : ReorderableListView
    {
        public AlwaysSelectedListView()
        {
            //SetStyle(ControlStyles.UserPaint,true);

        }


        protected override void WndProc(ref Message m)
        {
            // Swallow mouse messages that are not in the client area
                        if (m.Msg >= 0x201 && m.Msg <= 0x209)
                        {
                            Point pos = new Point((int) (m.LParam.ToInt64() & 0xffff), (int) (m.LParam.ToInt64() >> 16));
                            var hit = this.HitTest(pos);
                            switch (hit.Location)
                            {
                                case ListViewHitTestLocations.AboveClientArea:
                                case ListViewHitTestLocations.BelowClientArea:
                                case ListViewHitTestLocations.LeftOfClientArea:
                                case ListViewHitTestLocations.RightOfClientArea:
                                case ListViewHitTestLocations.None:
                                    return;
                            }
                        }

            //hide scroll
            switch (m.Msg)
            {
                case 0x83: // WM_NCCALCSIZE
                    int style = (int)GetWindowLong(this.Handle, GWL_STYLE);
                    if ((style & WS_HSCROLL) == WS_HSCROLL)
                        SetWindowLong(this.Handle, GWL_STYLE, style & ~WS_HSCROLL);
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }


        }

        private const int GWL_STYLE = -16;
        private const int WS_VSCROLL = 0x00200000;
        private const int WS_HSCROLL = 0x00100000;

        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return (int)GetWindowLong32(hWnd, nIndex);
            else
                return (int)(long)GetWindowLongPtr64(hWnd, nIndex);
        }

        public static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 4)
                return (int)SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            else
                return (int)(long)SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, int dwNewLong);


        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent)
        {
            var pos = Parent.PointToClient(MousePosition);

            if (!Bounds.Contains(pos))
            {
                qcdevent.Action = DragAction.Cancel;
                return;
            }

            base.OnQueryContinueDrag(qcdevent);
        }

    }
}
