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
using System.Text.RegularExpressions;

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
                var lines = new Regex(@"({(?i)sleep(?-i) *[0-9]*})").Split(Text);
                var timeExtract = new Regex("{(?i)sleep(?-i) *([0-9]*)}");

                foreach(var l in lines)
                {
                    var match = timeExtract.Match(l);
                    if (match.Success)
                    {
                        int delayMs;
                        if(int.TryParse(match.Groups[1].Value, out delayMs))
                        {
                            Thread.Sleep(delayMs);
                            continue;
                        }
                    }

                    SendKeys.SendWait(l);
                }
                
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
}
