//------------------------------------------------------------------------------
// <copyright file="Screen.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------
 
/*
 */

using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using Win32;

namespace WGestures.Common.OsSpecific.Windows 
{

    public struct Screen 
    {
 
        readonly IntPtr hmonitor;
      
        readonly Rectangle    bounds;

        private Rectangle workingArea;// = Rectangle.Empty;
       
        readonly bool         primary;
       
        readonly string       deviceName;
 
        readonly int          bitDepth;
 
        private static object syncLock = new object();//used to lock this class before [....]'ing to SystemEvents

        private static int desktopChangedCount = -1;//static counter of desktop size changes

        private int currentDesktopChangedCount;// = -1;//instance-based counter used to invalidate WorkingArea
 

        private const int PRIMARY_MONITOR = unchecked((int)0xBAADF00D);
 
        private const int MONITOR_DEFAULTTONULL       = 0x00000000;
        private const int MONITOR_DEFAULTTOPRIMARY    = 0x00000001;
        private const int MONITOR_DEFAULTTONEAREST    = 0x00000002;
        private const int MONITORINFOF_PRIMARY        = 0x00000001;
 
        private static bool multiMonitorSupport = (User32.GetSystemMetrics(80) != 0);
        private static Screen?[] screens;
 
        internal Screen(IntPtr monitor) : this(monitor, IntPtr.Zero) {
        }
 
        internal Screen(IntPtr monitor, IntPtr hdc)
        {
            currentDesktopChangedCount = -1;
            workingArea = Rectangle.Empty;

            IntPtr screenDC = hdc;
 
            if (!multiMonitorSupport || monitor == (IntPtr)PRIMARY_MONITOR) {
                // Single monitor system
                //

                if (User32.GetSystemMetrics(80) != 0)
                {
                    bounds = new Rectangle(User32.GetSystemMetrics(76),
                                         User32.GetSystemMetrics(77),
                                         User32.GetSystemMetrics(78),
                                         User32.GetSystemMetrics(79));
                }
                else
                {
                    Size size = new Size(User32.GetSystemMetrics(0),User32.GetSystemMetrics(1));
                    bounds =  new Rectangle(0, 0, size.Width, size.Height);
                }
                //bounds = SystemInformation.VirtualScreen;
                primary = true;
                deviceName = "DISPLAY";
            }
            else {
                // MultiMonitor System
                // We call the 'A' version of GetMonitorInfoA() because
                // the 'W' version just never fills out the struct properly on Win2K.
                //
                var info = new User32.MonitorInfoEx();
                info.Init();

                User32.GetMonitorInfo(monitor, ref info);
                bounds = Rectangle.FromLTRB(info.Monitor.Left, info.Monitor.Top, info.Monitor.Right, info.Monitor.Bottom);
                primary = ((info.Flags & MONITORINFOF_PRIMARY) != 0);
                /*int count = info.DeviceName.Length;
                while (count > 0 && info.DeviceName[count - 1] == (char)0) {
                    count--;
                }
 
                deviceName = new string(info.szDevice);
                deviceName = deviceName.TrimEnd((char)0);*/

                deviceName = info.DeviceName;
                if (hdc == IntPtr.Zero) {
                    
                    screenDC = GDI32.CreateDC(deviceName, null, null, new HandleRef());
                }
 
            }
            hmonitor = monitor;
 
            this.bitDepth = GDI32.GetDeviceCaps(new HandleRef(null, screenDC).Handle, 12);
            this.bitDepth *= GDI32.GetDeviceCaps(new HandleRef(null, screenDC).Handle, 14);
 
            if (hdc != screenDC) {
                GDI32.DeleteDC(new HandleRef(null, screenDC).Handle);
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.AllScreens"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets an array of all of the displays on the system.
        ///    </para>
        /// </devdoc>
        public static Screen?[] AllScreens {
            get {
                if (screens == null) {
                    if (multiMonitorSupport) {
                        MonitorEnumCallback closure = new MonitorEnumCallback();
                        User32.MonitorEnumProc proc = new User32.MonitorEnumProc(closure.Callback);
                        User32.EnumDisplayMonitors(new HandleRef(), null, proc, IntPtr.Zero);
 
                        if (closure.screens.Count > 0) {
                            Screen?[] temp = new Screen?[closure.screens.Count];
                            closure.screens.CopyTo(temp, 0);
                            screens = temp;
                        }
                        else {
                            screens = new Screen?[] {new Screen((IntPtr)PRIMARY_MONITOR)};
                        }
                    }
                    else {
                        screens = new Screen?[] {PrimaryScreen};
                    }
 
                    // Now that we have our screens, attach a display setting changed
                    // event so that we know when to invalidate them.
                    //
                    SystemEvents.DisplaySettingsChanging += new EventHandler(OnDisplaySettingsChanging);
                }
 
                return screens;
            }
        }
        
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.BitsPerPixel"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets Bits per Pixel value.
        ///    </para>
        /// </devdoc>
        public int BitsPerPixel {
            get {
                return bitDepth;
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.Bounds"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the bounds of the display.
        ///    </para>
        /// </devdoc>
        public Rectangle Bounds {
            get {
                return bounds;
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.DeviceName"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the device name associated with a display.
        ///    </para>
        /// </devdoc>
        public string DeviceName {
            get {
                return deviceName;
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.Primary"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets a value indicating whether a particular display is
        ///       the primary device.
        ///    </para>
        /// </devdoc>
        public bool Primary {
            get {
                return primary;
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.PrimaryScreen"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the
        ///       primary display.
        ///    </para>
        /// </devdoc>
        public static Screen? PrimaryScreen {
            get {
                if (multiMonitorSupport) {
                    Screen?[] screens = AllScreens;
                    for (int i=0; i<screens.Length; i++) {
                        if (screens[i].Value.primary) {
                            return screens[i];
                        }
                    }
                    return null;
                }
                else {
                    return new Screen((IntPtr)PRIMARY_MONITOR, IntPtr.Zero);
                }
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.WorkingArea"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the working area of the screen.
        ///    </para>
        /// </devdoc>
        public Rectangle WorkingArea {
            get {
 
                //if the static Screen class has a different desktop change count 
                //than this instance then update the count and recalculate our working area
                if (currentDesktopChangedCount != Screen.DesktopChangedCount) {
 
                    Interlocked.Exchange(ref currentDesktopChangedCount, Screen.DesktopChangedCount);
 
                    if (!multiMonitorSupport ||hmonitor == (IntPtr)PRIMARY_MONITOR) {
                        // Single monitor system
                        //

                        GDI32.RECT rc = new GDI32.RECT();
                        User32.SystemParametersInfo((int)User32.SPI_GETWORKAREA, 0, ref rc, 0);
                         workingArea = Rectangle.FromLTRB(rc.Left, rc.Top, rc.Right, rc.Bottom);
                        //workingArea = SystemInformation.WorkingArea;
                    }
                    else {
                        // MultiMonitor System
                        // We call the 'A' version of GetMonitorInfoA() because
                        // the 'W' version just never fills out the struct properly on Win2K.
                        //
                        var info = new User32.MonitorInfoEx();
                        info.Init();
                        User32.GetMonitorInfo(hmonitor, ref info);
                        workingArea = new Rectangle(info.WorkArea.Left, info.WorkArea.Top, info.WorkArea.Right, info.WorkArea.Bottom);
                    }
                }
                
                return workingArea;
            }
        }
 
        /// <devdoc>
        ///     Screen instances call this property to determine
        ///     if their WorkingArea cache needs to be invalidated.
        /// </devdoc>
        private static int DesktopChangedCount {
            get {
                if (desktopChangedCount == -1) {
 
                    lock (syncLock) {
 
                        //now that we have a lock, verify (again) our changecount...
                        if (desktopChangedCount == -1) {
                            //[....] the UserPreference.Desktop change event.  We'll keep count 
                            //of desktop changes so that the WorkingArea property on Screen 
                            //instances know when to invalidate their cache.
                            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(OnUserPreferenceChanged);
 
                            desktopChangedCount = 0;
                        }
                    }
                }
                return desktopChangedCount;
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.Equals"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Specifies a value that indicates whether the specified object is equal to
        ///       this one.
        ///    </para>
        /// </devdoc>
        public override bool Equals(object obj) {
            if (obj is Screen) {
                Screen comp = (Screen)obj;
                if (hmonitor == comp.hmonitor) {
                    return true;
                }
            }
            return false;
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.FromPoint"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Retrieves a <see cref='Screen'/>
        ///       for the monitor that contains the specified point.
        ///       
        ///    </para>
        /// </devdoc>
        public static Screen FromPoint(Point point) {
            if (multiMonitorSupport)
            {
                var pt = new User32.POINT() {X = point.X, Y = point.Y};
                return new Screen(User32.MonitorFromPoint(pt, MONITOR_DEFAULTTONEAREST));
            }
            else {
                return new Screen((IntPtr)PRIMARY_MONITOR);
            }
        }


        public static Rectangle? ScreenBoundsFromPoint(Point point)
        {
            var screens = AllScreens;
            var len = screens.Length;

            for(var i=0; i< len; i++)
            {
                if (screens[i].HasValue && screens[i].Value.Bounds.Contains(point) )
                {
                    return screens[i].Value.Bounds;
                }
            }

            return null;
            /*var pt = new User32.POINT() { X = point.X, Y = point.Y };

            var info = new User32.MonitorInfoEx();//new User32.MONITORINFOEX();
            info.Init();
            User32.GetMonitorInfo(User32.MonitorFromPoint(pt, MONITOR_DEFAULTTONEAREST),ref info);
            return Rectangle.FromLTRB(info.Monitor.Left, info.Monitor.Top, info.Monitor.Right, info.Monitor.Bottom);*/
        }

 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.FromRectangle"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Retrieves a <see cref='Screen'/>
        ///       for the monitor that contains the
        ///       largest region of the rectangle.
        ///       
        ///    </para>
        /// </devdoc>
        public static Screen FromRectangle(Rectangle rect) {
            if (multiMonitorSupport) {
                var rc = new GDI32.RECT(rect.X, rect.Y, rect.Width, rect.Height);
                return new Screen(User32.MonitorFromRect(ref rc, MONITOR_DEFAULTTONEAREST));
            }
            else {
                return new Screen((IntPtr)PRIMARY_MONITOR, IntPtr.Zero);
            }
        }
 
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.FromHandle"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Retrieves a <see cref='Screen'/>
        ///       for the monitor that
        ///       contains the largest region of the window.
        ///    </para>
        /// </devdoc>
        public static Screen FromHandle(IntPtr hwnd) {
            return FromHandleInternal(hwnd);
        }
 
        internal static Screen FromHandleInternal(IntPtr hwnd) {
            if (multiMonitorSupport) {
                return new Screen(User32.MonitorFromWindow(new HandleRef(null, hwnd), MONITOR_DEFAULTTONEAREST));
            }
            else {
                return new Screen((IntPtr)PRIMARY_MONITOR, IntPtr.Zero);
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.GetWorkingArea"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Retrieves the working area for the monitor that is closest to the
        ///       specified point.
        ///       
        ///    </para>
        /// </devdoc>
        public static Rectangle GetWorkingArea(Point pt) {
            return Screen.FromPoint(pt).WorkingArea;
        }
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.GetWorkingArea1"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Retrieves the working area for the monitor that contains the largest region
        ///       of the specified rectangle.
        ///       
        ///    </para>
        /// </devdoc>
        public static Rectangle GetWorkingArea(Rectangle rect) {
            return Screen.FromRectangle(rect).WorkingArea;
        }

 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.GetBounds"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Retrieves the bounds of the monitor that is closest to the specified
        ///       point.
        ///    </para>
        /// </devdoc>
        public static Rectangle GetBounds(Point pt) {
            return Screen.FromPoint(pt).Bounds;
        }
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.GetBounds1"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Retrieves the bounds of the monitor that contains the largest region of the
        ///       specified rectangle.
        ///    </para>
        /// </devdoc>
        public static Rectangle GetBounds(Rectangle rect) {
            return Screen.FromRectangle(rect).Bounds;
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.GetHashCode"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Computes and retrieves a hash code for an object.
        ///    </para>
        /// </devdoc>
        public override int GetHashCode() {
            return(int)hmonitor;
        }
 
        /// <devdoc>
        ///     Called by the SystemEvents class when our display settings are
        ///     changing.  We cache screen information and at this point we must
        ///     invalidate our cache.
        /// </devdoc>
        private static void OnDisplaySettingsChanging(object sender, EventArgs e) {
 
            // Now that we've responded to this event, we don't need it again until
            // someone re-queries. We will re-add the event at that time.
            //
            SystemEvents.DisplaySettingsChanging -= new EventHandler(OnDisplaySettingsChanging);
 
            // Display settings changed, so the set of screens we have is invalid.
            //
            screens = null;
        }
 
        /// <devdoc>
        ///     Called by the SystemEvents class when our display settings have
        ///     changed.  Here, we increment a static counter that Screen instances
        ///     can check against to invalidate their cache.
        /// </devdoc>
        private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) {
 
            if (e.Category == UserPreferenceCategory.Desktop) {
                Interlocked.Increment(ref desktopChangedCount);
            }
        }
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.ToString"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Retrieves a string representing this object.
        ///    </para>
        /// </devdoc>
        public override string ToString() {
            return GetType().Name + "[Bounds=" + bounds.ToString() + " WorkingArea=" + WorkingArea.ToString() + " Primary=" + primary.ToString() + " DeviceName=" + deviceName;
        }
 
 
        /// <include file='doc\Screen.uex' path='docs/doc[@for="Screen.MonitorEnumCallback"]/*' />
        /// <devdoc>         
        /// </devdoc>         
        private class MonitorEnumCallback {
            public ArrayList screens = new ArrayList();
 
            public virtual bool Callback(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lparam) {
                screens.Add(new Screen(monitor, hdc));
                return true;
            }
        }

    }

}