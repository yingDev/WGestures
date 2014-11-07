using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace WGestures.Common
{
    public class DetailedConsoleListener : ConsoleTraceListener
    {
        public override void WriteLine(string message)
        {            
            var mth = new StackTrace().GetFrame(2).GetMethod();

            this.Writer.WriteLine(">>" + mth.ReflectedType.Name +"[" + Thread.CurrentThread.ManagedThreadId +
                "] " + Thread.CurrentThread.Name);
            
            base.WriteLine(message);
        }
    }
}
