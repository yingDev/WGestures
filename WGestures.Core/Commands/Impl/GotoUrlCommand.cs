using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WGestures.Common.Annotation;

namespace WGestures.Core.Commands.Impl
{
    [Named("打开网址")]
    public class GotoUrlCommand : AbstractCommand
    {
        private string _url;

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
            }
        }

        public GotoUrlCommand()
        {
            Url = "";
        }

        public override void Execute()
        {
            if (Url != null)
            {
                if (!Url.Contains("://"))
                {
                    Url = "http://" + Url;
                }
            }
            using(Process.Start(Url));

            GC.Collect(3, GCCollectionMode.Forced);
        }
    }
}
