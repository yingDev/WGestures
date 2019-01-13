using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WGestures.App.Gui.Windows
{
    public class ElevatedDragDropManager : IMessageFilter
    {
        #region "P/Invoke"
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, ChangeWindowMessageFilterExAction action, ref CHANGEFILTERSTRUCT changeInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeWindowMessageFilter(uint msg, ChangeWindowMessageFilterFlags flags);

        [DllImport("shell32.dll")]
        private static extern void DragAcceptFiles(IntPtr hwnd, bool fAccept);

        [DllImport("shell32.dll")]
        private static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out()]
StringBuilder lpszFile, uint cch);

        [DllImport("shell32.dll")]
        private static extern bool DragQueryPoint(IntPtr hDrop, ref POINT lppt);

        [DllImport("shell32.dll")]
        private static extern void DragFinish(IntPtr hDrop);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;

            public int Y;
            public POINT(int newX, int newY)
            {
                X = newX;
                Y = newY;
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

        private enum MessageFilterInfo : uint
        {
            None,
            AlreadyAllowed,
            AlreadyDisAllowed,
            AllowedHigher
        }

        private enum ChangeWindowMessageFilterExAction : uint
        {
            Reset,
            Allow,
            Disallow
        }

        private enum ChangeWindowMessageFilterFlags : uint
        {
            Add = 1,
            Remove = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CHANGEFILTERSTRUCT
        {
            public uint cbSize;
            public MessageFilterInfo ExtStatus;
        }
        #endregion

        public static ElevatedDragDropManager Instance = new ElevatedDragDropManager();
        public event EventHandler<ElevatedDragDropArgs> ElevatedDragDrop;

        private const uint WM_DROPFILES = 0x233;
        private const uint WM_COPYDATA = 0x4a;

        private const uint WM_COPYGLOBALDATA = 0x49;
        private readonly bool IsVistaOrHigher = Environment.OSVersion.Version.Major >= 6;

        private readonly bool Is7OrHigher = (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1) || Environment.OSVersion.Version.Major > 6;
        protected ElevatedDragDropManager()
        {
            Application.AddMessageFilter(this);
        }

        public void EnableDragDrop(IntPtr hWnd)
        {
            if (Is7OrHigher)
            {
                CHANGEFILTERSTRUCT changeStruct = new CHANGEFILTERSTRUCT();
                changeStruct.cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(CHANGEFILTERSTRUCT)));
                ChangeWindowMessageFilterEx(hWnd, WM_DROPFILES, ChangeWindowMessageFilterExAction.Allow, ref changeStruct);
                ChangeWindowMessageFilterEx(hWnd, WM_COPYDATA, ChangeWindowMessageFilterExAction.Allow, ref changeStruct);
                ChangeWindowMessageFilterEx(hWnd, WM_COPYGLOBALDATA, ChangeWindowMessageFilterExAction.Allow, ref changeStruct);
            }
            else if (IsVistaOrHigher)
            {
                ChangeWindowMessageFilter(WM_DROPFILES, ChangeWindowMessageFilterFlags.Add);
                ChangeWindowMessageFilter(WM_COPYDATA, ChangeWindowMessageFilterFlags.Add);
                ChangeWindowMessageFilter(WM_COPYGLOBALDATA, ChangeWindowMessageFilterFlags.Add);
            }

            DragAcceptFiles(hWnd, true);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_DROPFILES)
            {
                HandleDragDropMessage(m);
                return true;
            }

            return false;
        }

        private void HandleDragDropMessage(Message m)
        {
            var sb = new StringBuilder(260);
            uint numFiles = DragQueryFile(m.WParam, 0xffffffffu, sb, 0);
            var list = new List<string>();

            for (uint i = 0; i <= numFiles - 1; i++)
            {
                if (DragQueryFile(m.WParam, i, sb, Convert.ToUInt32(sb.Capacity) * 2) > 0)
                {
                    list.Add(sb.ToString());
                }
            }

            POINT p = default(POINT);
            DragQueryPoint(m.WParam, ref p);
            DragFinish(m.WParam);

            var args = new ElevatedDragDropArgs();
            args.HWnd = m.HWnd;
            args.Files = list;
            args.X = p.X;
            args.Y = p.Y;

            if (ElevatedDragDrop != null)
            {
                ElevatedDragDrop(this, args);
            }
        }
    }

    public class ElevatedDragDropArgs : EventArgs
    {
        public IntPtr HWnd
        {
            get { return m_HWnd; }
            set { m_HWnd = value; }
        }
        private IntPtr m_HWnd;
        public List<string> Files
        {
            get { return m_Files; }
            set { m_Files = value; }
        }
        private List<string> m_Files;
        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }
        private int m_X;
        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        private int m_Y;
        public ElevatedDragDropArgs()
        {
            Files = new List<string>();
        }
    }
}
