using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Common;
using Win32;
using WindowsInput;
using static Win32.User32;
using System.ComponentModel;

namespace WGestures.Core.Impl.Windows
{
    class TouchHook
    {
        enum POINTER_INPUT_TYPE
        {
            PT_POINTER = 0x00000001,
            PT_TOUCH = 0x00000002,
            PT_PEN = 0x00000003,
            PT_MOUSE = 0x00000004,
            PT_TOUCHPAD = 0x00000005
        }

        [Flags]
        private enum POINTER_FLAGS
        {
            NONE = 0x00000000,
            NEW = 0x00000001,
            INRANGE = 0x00000002,
            INCONTACT = 0x00000004,
            FIRSTBUTTON = 0x00000010,
            SECONDBUTTON = 0x00000020,
            THIRDBUTTON = 0x00000040,
            FOURTHBUTTON = 0x00000080,
            FIFTHBUTTON = 0x00000100,
            PRIMARY = 0x00002000,
            CONFIDENCE = 0x00004000,
            CANCELED = 0x00008000,
            DOWN = 0x00010000,
            UPDATE = 0x00020000,
            UP = 0x00040000,
            WHEEL = 0x00080000,
            HWHEEL = 0x00100000,
            CAPTURECHANGED = 0x00200000,
        }


        [Flags]
        private enum VIRTUAL_KEY_STATES
        {
            NONE = 0x0000,
            LBUTTON = 0x0001,
            RBUTTON = 0x0002,
            SHIFT = 0x0004,
            CTRL = 0x0008,
            MBUTTON = 0x0010,
            XBUTTON1 = 0x0020,
            XBUTTON2 = 0x0040
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct POINTER_INFO
        {
            public POINTER_INPUT_TYPE pointerType;
            public int PointerID;
            public int FrameID;
            public POINTER_FLAGS PointerFlags;
            public IntPtr SourceDevice;
            public IntPtr WindowTarget;

            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtPixelLocation;

            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtPixelLocationRaw;

            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtHimetricLocation;

            [MarshalAs(UnmanagedType.Struct)]
            public POINT PtHimetricLocationRaw;

            public uint Time;
            public uint HistoryCount;
            public uint InputData;
            public VIRTUAL_KEY_STATES KeyStates;
            public long PerformanceCount;
            public int ButtonChangeType;
        }

        [DllImport("User32")]
        static extern bool RegisterPointerInputTarget(IntPtr hwnd, POINTER_INPUT_TYPE  pointerType);

        [DllImport("User32")]
        static extern bool GetPointerInfo(UInt32 pointerId, ref POINTER_INFO pointerInfo);

        static uint GET_POINTERID_WPARAM(uint wParam) { return ((HiLoWord)wParam).Low; }

        public static POINT GetTouchPoint(uint pointerId)
        {
            POINTER_INFO pi = new POINTER_INFO();
            if (!GetPointerInfo(pointerId, ref pi))
            {
                int errCode = Marshal.GetLastWin32Error();
                if (errCode != 0)
                {
                    throw new Win32Exception(errCode);
                }
            }
            return pi.PtPixelLocation;
        }

        class InputTargetWindow : NativeWindow, IDisposable
        {
            static IntPtr HWND_MESSAGE = new IntPtr(-3);

            public InputTargetWindow()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams()
                {
                    ExStyle = (int)(User32.WS_EX.WS_EX_NOACTIVATE | User32.WS_EX.WS_EX_TRANSPARENT),
                    Parent = HWND_MESSAGE
                });
            }

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        InputTargetWindow _win;

        const int WM_NCPOINTERUPDATE = 0x0241;
        const int WM_NCPOINTERDOWN = 0x0242;
        const int WM_NCPOINTERUP = 0x0243;
        const int WM_POINTERUPDATE = 0x0245; //581
        const int WM_POINTERDOWN = 0x0246;
        const int WM_POINTERUP = 0x0247; //583
        const int WM_POINTERENTER = 0x0249; //585
        const int WM_POINTERLEAVE = 0x024A; //586
        const int WM_POINTERACTIVATE = 0x024B;
        const int WM_POINTERCAPTURECHANGED = 0x024C;
        const int WM_TOUCHHITTESTING = 0x024D;
        const int WM_POINTERWHEEL = 0x024E;
        const int WM_POINTERHWHEEL = 0x024F;
        const int DM_POINTERHITTEST = 0x0250;

        public void Install()
        {
            var thread = new Thread(() => 
            {
                _win = new InputTargetWindow();

                var ok = RegisterPointerInputTarget(_win.Handle, POINTER_INPUT_TYPE.PT_TOUCH);
                if(!ok)
                {
                    Debug.WriteLine("失败 RegisterPointerInputTarget: " + Native.GetLastError());
                    return; //todo: !!
                }else
                {
                    Debug.WriteLine("TouchHook Installed.");
                }

                Native.MSG msg;
                int ret;

                int touchCount = 0;
                var sim = new InputSimulator();
                uint lastPointerId = 0;

                while ((ret = Native.GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0)
                {              
                    switch(msg.message)
                    {
                        case WM_POINTERDOWN:
                            touchCount += 1;
                            Debug.WriteLine("Pointer Down: " + touchCount);
                            if(touchCount == 3)
                            {
                                
                                lastPointerId = GET_POINTERID_WPARAM((uint) msg.wParam.ToInt32());

                                var p = GetTouchPoint(lastPointerId);
                                sim.Mouse.MoveMouseTo(p.X * (65535.0f/2736), p.Y * (65535.0f/1824));
                                sim.Mouse.RightButtonDown();
                            }
                            
                            break;
                        case WM_POINTERUP:
                            Debug.WriteLine("Pointer Up");
                            //sim.Mouse.RightButtonUp();
                            if(touchCount == 3)
                            {
                                sim.Mouse.RightButtonUp();
                            }
                            touchCount -= 1;

                            break;
                        case WM_POINTERUPDATE:
                            Debug.WriteLine(GET_POINTERID_WPARAM((uint) msg.wParam.ToInt32()));
                            if(touchCount == 3)
                            {
                                var p = GetTouchPoint(lastPointerId);
                                Debug.WriteLine("POS = " + (System.Drawing.Point)p);
                                sim.Mouse.MoveMouseTo(p.X * (65535.0f / 2736), p.Y * (65535.0f / 1824));
                            }
                            break;

                        default:
                           // Debug.WriteLine("msg=" + msg.message);
                            break;
                    }
                    var MSG = new Message() { HWnd = msg.hwnd, LParam = msg.lParam, WParam = msg.wParam, Msg = (int)msg.message, Result = IntPtr.Zero };
                    _win.DefWndProc(ref MSG);
                }

            });

            thread.Start();


        }
    }
}
