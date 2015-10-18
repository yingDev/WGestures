using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using WindowsInput;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;
using Win32;
using System.Windows.Forms;

namespace WGestures.Core.Commands.Impl
{
    [Named("按键序列"), Serializable]
    public class SendTextCommand : AbstractCommand
    {
        public string Text { get; set; }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                SendKeys.SendWait(Text);
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
}
