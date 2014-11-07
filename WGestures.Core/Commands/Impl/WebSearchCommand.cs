using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;
using Timer = System.Windows.Forms.Timer;

namespace WGestures.Core.Commands.Impl
{
    [Named("Web搜索")]
    public class WebSearchCommand : AbstractCommand, IGestureContextAware
    {
        public string SearchEngineUrl { get; set; }
        public string SearchEngingName { get; set; }

        private InputSimulator _sim = new InputSimulator();

        public WebSearchCommand()
        {
            SearchEngingName = "Google";
            SearchEngineUrl = "https://www.google.com/search?q={0}";
        }

        public override void Execute()
        {
            if(SearchEngineUrl ==null || !Uri.IsWellFormedUriString(PopulateSearchEngingUrl("Nothing"),UriKind.Absolute)) return;

            var t = new Thread(() =>
            {

                var clipboardMonitor = new ClipboardMonitor();

                clipboardMonitor.MonitorRegistered += () =>
                {
                    var timer = new Timer() { Interval = 2000 };
                    timer.Tick += (sender, args) =>
                    {
                        Debug.WriteLine("WebSearchCommand Timeout!");
                        timer.Enabled = false;

                        clipboardMonitor.StopMonitor();
                        clipboardMonitor.DestroyHandle();
                        Application.ExitThread();
                        
                        Debug.WriteLine("超时结束 WebSearchCommand Runloop");

                    };

                    timer.Enabled = true;

                    try
                    {
                        _sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, new[] { VirtualKeyCode.VK_C });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("发送按键失败:" + ex);
                        Native.TryResetKeys(new []{VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C});
#if DEBUG
                        //throw;
#endif
                    }
                };

                clipboardMonitor.ClipboardUpdated += args =>
                {
                    Debug.WriteLine("ClipboardChanged");
                    args.Handled = true;

                    var text = "";
                    if (Clipboard.ContainsText() && (text = Clipboard.GetText().Trim()).Length > 0)
                    {
                        //如果是URL则打开，否则搜索
                        if (Uri.IsWellFormedUriString(text, UriKind.Absolute))
                        {
                            using(Process.Start(text));
                        }
                        else
                        {
                            if (text.Length > 100) text = text.Substring(0, 100);
                            using (Process.Start(PopulateSearchEngingUrl(text))) ;
                        }
                    }

                    clipboardMonitor.StopMonitor();
                    clipboardMonitor.DestroyHandle();

                    Application.ExitThread();
                };

                clipboardMonitor.StartMonitor();

                Application.Run();
                Debug.WriteLine("Thread End?");

            }) {Name = "WebSearchCommand Runloop(Bug！我不应该存在！)"};

            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

        public GestureContext Context { set; private get; }

        private string PopulateSearchEngingUrl(string param)
        {

            return HttpUtility.UrlPathEncode(string.Format(SearchEngineUrl, param));
        }

        public override string Description()
        {
            return ((SearchEngingName ?? "Web") + "搜索");
        }
    }
}
