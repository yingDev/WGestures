
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.Core.Persistence.Impl.Windows
{
    public class Win32GestrueIntentFinder : AbstractGestureIntentFinder
    {
        public Win32GestrueIntentFinder(IGestureIntentStore intentStore)
            : base(intentStore)
        {
        }

        private Dictionary<uint, string> _procFileNameDict = new Dictionary<uint, string>(64);

        /// <summary>
        /// Native下以进程文件名为key查找。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExeApp GetExeAppByContext(GestureContext context)
        {
            //全屏模式下禁用

            string str;
            ExeApp found = null;

            var procId = Native.GetActiveProcessId();


            if (!_procFileNameDict.TryGetValue(procId, out str))
            {
                str = Native.GetProcessFile(procId);
                _procFileNameDict[procId] = str;
            }
            
            IntentStore.TryGetExeApp(str, out found);

            return found;
        }
    }
}
