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
    [Named("输出文本")]
    public class SendTextCommand : AbstractCommand
    {
        private InputSimulator _sim = new InputSimulator();

        public string Text { get; set; }

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                //var fgWin = User32.GetForegroundWindow();
                //uint procId;
                //var fgThread = Native.GetWindowThreadProcessId(fgWin, out procId);

                var txt = Text.Replace("\r\n", "\r");


                foreach (var c in txt)
                {
                    Thread.Sleep(20);
                    try
                    {
                        _sim.Keyboard.TextEntry(c);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("发送按键失败: " +ex);
                    }
                }


            }
        }
    }
}
