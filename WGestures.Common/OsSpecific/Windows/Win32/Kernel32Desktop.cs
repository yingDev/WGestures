
//This file contains the Win32 API that are specific to the desktop Windows.

//Created by Warren Tang on 8/28/2008

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace Win32
{
    public partial class Kernel32
    {
       

        #region Time

        /// <summary>
        /// Equal to Environment.TickCount which is in milliseconds though. 
        /// </summary>
        /// <returns></returns>
        [DllImport(Kernel32Dll)]
        public static extern uint GetTickCount();

        [DllImport(Kernel32Dll, SetLastError = true)]
        public static extern bool GetSystemTimes(
                    out System.Runtime.InteropServices.ComTypes.FILETIME lpIdleTime,
                    out System.Runtime.InteropServices.ComTypes.FILETIME lpKernelTime,
                    out System.Runtime.InteropServices.ComTypes.FILETIME lpUserTime
                    );


        /// <summary>
        /// Get system idle time in milliseconds.(Wrapper)
        /// </summary>
        /// <returns>System idle time in milliseconds</returns>
        public static int GetIdleTime()
        {

            System.Runtime.InteropServices.ComTypes.FILETIME idleTime, kernelTime, userTime;
            GetSystemTimes(out idleTime, out kernelTime, out userTime);
            ulong idleTimeLong = ((ulong)idleTime.dwHighDateTime << 32) + (uint)idleTime.dwLowDateTime;
            return (int)(idleTimeLong / TimeSpan.TicksPerMillisecond);
        }

        /// <summary>
        /// Get system kernel time in milliseconds.(Wrapper)
        /// </summary>
        /// <returns>System kernel time in milliseconds.</returns>
        public static int GetKernelTime()
        {

            System.Runtime.InteropServices.ComTypes.FILETIME idleTime, kernelTime, userTime;
            GetSystemTimes(out idleTime, out kernelTime, out userTime);
            ulong kernelTimeLong = ((ulong)kernelTime.dwHighDateTime << 32) + (uint)kernelTime.dwLowDateTime;
            return (int)(kernelTimeLong / TimeSpan.TicksPerMillisecond);
        }

        /// <summary>
        /// Get system user time in milliseconds.(Wrapper)
        /// </summary>
        /// <returns>System user time in milliseconds.</returns>
        public static int GetUserTime()
        {
            System.Runtime.InteropServices.ComTypes.FILETIME idleTime, kernelTime, userTime;
            GetSystemTimes(out idleTime, out kernelTime, out userTime);
            ulong userTimeLong = ((ulong)userTime.dwHighDateTime << 32) + (uint)userTime.dwLowDateTime;
            return (int)(userTimeLong / TimeSpan.TicksPerMillisecond);
        }
        #endregion

    }
}
