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

namespace WGestures.Core.Commands.Impl
{
    [Named("输出文本"), Serializable]
    public class SendTextCommand : AbstractCommand
    {
        public string Text { get; set; }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                //var fgWin = User32.GetForegroundWindow();
                //uint procId;
                //var fgThread = Native.GetWindowThreadProcessId(fgWin, out procId);

                var txt = Text.Replace("\r\n", "\r");
                Sim.TextEntry(txt);

                /*foreach (var c in txt)
                {
                    Thread.Sleep(20);
                    try
                    {
                        Sim.TextEntry(c);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("发送按键失败: " +ex);
                    }
                }*/


            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
}
