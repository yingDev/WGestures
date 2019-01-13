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
using System.Reflection;
using TCD.System.TouchInjection;
using System.Drawing;

namespace WGestures.Core.Impl.Windows
{
    class TouchHook : IDisposable
    {
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


        [DllImport("User32")]
        static extern bool RegisterPointerInputTarget(IntPtr hwnd, PointerInputType pointerType);
        [DllImport("User32")]
        static extern bool GetPointerFrameTouchInfo(UInt32 pointerId, ref UInt32 pointerCount, [Out] PointerTouchInfo[] touchInfo);
        static uint GET_POINTERID_WPARAM(uint wParam) { return ((HiLoWord)wParam).Low; }

        public void Install()
        {
            var thread = new Thread(() => 
            {
                _win = new InputTargetWindow();

                var ok = RegisterPointerInputTarget(_win.Handle, PointerInputType.TOUCH);
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
                
                var sim = new InputSimulator();

                TouchInjector.InitializeTouchInjection();
                var screenBounds = Rectangle.Empty;

                var contacts = new PointerTouchInfo[10];
                uint contactCount = 10;
                uint startingPointId = 0;
                
                while ((ret = Native.GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0)
                {
                    if(ret == -1)
                    {
                        Debug.WriteLine("Error!");
                        continue;
                    }
                    var pointerId = GET_POINTERID_WPARAM((uint)msg.wParam.ToInt32());

                    if (! GetPointerFrameTouchInfo(pointerId, ref contactCount, contacts))
                    {
                        Debug.WriteLine("GetPointerFrameTouchInfo Error: " + Native.GetLastError());
                        continue;
                    }

                    //TODO: refactor... just hacking...
                    switch (msg.message)
                    {
                        case WM_POINTERDOWN:
                            Debug.WriteLine("Touch Down: " + contactCount);
                            
                            if(contactCount == 3)
                            {
                                startingPointId = contacts[0].PointerInfo.PointerId;
                                if (screenBounds.Width > 0 && screenBounds.Height > 0)
                                {
                                    var pos = contacts[0].PointerInfo.PtPixelLocation;
                                    User32.SetCursorPos(pos.X, pos.Y);
                                }
                                screenBounds = Native.GetScreenBounds();
                                sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LWIN);
                                continue;
                            }
                            ConvertToNewTouchInfo(contacts, PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT);
                            if (!TouchInjector.InjectTouchInput(1, contacts))
                            {
                                Debug.WriteLine("Error InjectTouchInput: " + Native.GetLastError());
                            }
                            break;

                        case WM_POINTERUPDATE:
                            Debug.Write('.');

                            if(contactCount == 3)
                            {
                                TouchPoint? pos = null;
                                foreach(var contact in contacts)
                                {
                                    if(contact.PointerInfo.PointerId == startingPointId)
                                    {
                                        pos = contact.PointerInfo.PtPixelLocation;
                                    }
                                }

                                if(pos != null && screenBounds.Width > 0 && screenBounds.Height > 0)
                                {
                                    var absX = pos.Value.X * (65535.0 / screenBounds.Width);
                                    var absY = pos.Value.Y * (65535.0 / screenBounds.Height);
                                    sim.Mouse.MoveMouseTo(absX, absY);
                                }
                                continue;
                            }

                            ConvertToNewTouchInfo(contacts, PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT);
                            if (!TouchInjector.InjectTouchInput((int)contactCount, contacts))
                            {
                                Debug.WriteLine("Error InjectTouchInput: " + Native.GetLastError());
                            }

                            break;


                        case WM_POINTERUP:
                            Debug.WriteLine("Touch Up");
                            if (contactCount == 3)
                            {
                                sim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LWIN);
                                continue;
                            }
                            ConvertToNewTouchInfo(contacts, PointerFlags.UP);
                            if (!TouchInjector.InjectTouchInput(1, contacts))
                            {
                                Debug.WriteLine("Error InjectTouchInput: " + Native.GetLastError());
                            }
                            break;

                        case WM_POINTERENTER:
                            Debug.WriteLine("Touch Enter");
                            //ConvertToNewTouchInfo(contacts, PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT);
                            continue;

                        case WM_POINTERLEAVE:
                            Debug.WriteLine("Touch Leave");
                            //ConvertToNewTouchInfo(contacts, PointerFlags.UP | PointerFlags.INRANGE | PointerFlags.INCONTACT);
                            continue;

                        default:
                            Debug.WriteLine("Unhandled Msg: " + msg.message);
                            continue;
                    }



                    var MSG = new Message() { HWnd = msg.hwnd, LParam = msg.lParam, WParam = msg.wParam, Msg = (int)msg.message, Result = IntPtr.Zero };
                    _win.DefWndProc(ref MSG);
                }

            });

            thread.Start();
        }

        private void ConvertToNewTouchInfo(PointerTouchInfo[] contacts, PointerFlags flags)
        {
            for(var i=0; i< contacts.Length; i++)
            {
                var oldPointerInfo = contacts[i].PointerInfo;
                contacts[i].PointerInfo = new PointerInfo()
                {
                    pointerType = PointerInputType.TOUCH,
                    PointerFlags = flags,
                    PointerId = (uint)i + 1,// oldPointerInfo.PointerId,
                    PtPixelLocation = oldPointerInfo.PtPixelLocation
                };
                contacts[i].TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE;
                contacts[i].ContactAreaRaw = new ContactArea();
            }
        }

        private List<String> DumpPointerTouchInfo(PointerTouchInfo contact)
        {
            var lst = new List<String>();

            lst.Add("PointerInfo.pointerType = " + contact.PointerInfo.pointerType);
            lst.Add("TouchFlags = " + contact.TouchFlags);
            lst.Add("Orientation = " + contact.Orientation);
            lst.Add("Pressure = " + contact.Pressure);
            lst.Add("PointerInfo.PointerFlags = " + contact.PointerInfo.PointerFlags);
            lst.Add("TouchMasks = " + contact.TouchMasks);
            lst.Add("PointerInfo.PtPixelLocation.X = " + contact.PointerInfo.PtPixelLocation.X);
            lst.Add("PointerInfo.PtPixelLocation.Y = " + contact.PointerInfo.PtPixelLocation.Y);
            lst.Add("PointerInfo.PointerId = " + contact.PointerInfo.PointerId);
            lst.Add("ContactArea.left = " + contact.ContactArea.left);
            lst.Add("ContactArea.right = " + contact.ContactArea.right);
            lst.Add("ContactArea.top = " + contact.ContactArea.top);
            lst.Add("ContactArea.bottom = " + contact.ContactArea.bottom);


            lst.Add("pointerType = " + contact.PointerInfo.pointerType);
            lst.Add("PointerId = " + contact.PointerInfo.PointerId);
            lst.Add("FrameId = " + contact.PointerInfo.FrameId);
            lst.Add("PointerFlags = " + contact.PointerInfo.PointerFlags);
            lst.Add("SourceDevice = " + contact.PointerInfo.SourceDevice);
            lst.Add("WindowTarget = " + contact.PointerInfo.WindowTarget);
            lst.Add("PtPixelLocation = " + contact.PointerInfo.PtPixelLocation);
            lst.Add("PtPixelLocationRaw = " + contact.PointerInfo.PtPixelLocationRaw);
            lst.Add("PtHimetricLocation = " + contact.PointerInfo.PtHimetricLocation);
            lst.Add("PtHimetricLocationRaw = " + contact.PointerInfo.PtHimetricLocationRaw);
            lst.Add("Time = " + contact.PointerInfo.Time);
            lst.Add("HistoryCount = " + contact.PointerInfo.HistoryCount);
            lst.Add("InputData = " + contact.PointerInfo.InputData);
            lst.Add("KeyStates = " + contact.PointerInfo.KeyStates);
            lst.Add("PerformanceCount = " + contact.PointerInfo.PerformanceCount);
            lst.Add("ButtonChangeType = " + contact.PointerInfo.ButtonChangeType);

            return lst;
        }

        private PointerTouchInfo MakePointerTouchInfo(int x, int y, int radius, uint id, uint orientation = 90, uint pressure = 32000)
        {
            PointerTouchInfo contact = new PointerTouchInfo();
            contact.PointerInfo.pointerType = PointerInputType.TOUCH;
            contact.TouchFlags = TouchFlags.NONE;
            contact.Orientation = orientation;
            contact.Pressure = pressure;
            contact.PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
            contact.TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE;
            contact.PointerInfo.PtPixelLocation.X = x;
            contact.PointerInfo.PtPixelLocation.Y = y;
            contact.PointerInfo.PointerId = id;
            contact.ContactArea.left = x - radius;
            contact.ContactArea.right = x + radius;
            contact.ContactArea.top = y - radius;
            contact.ContactArea.bottom = y + radius;
            return contact;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
