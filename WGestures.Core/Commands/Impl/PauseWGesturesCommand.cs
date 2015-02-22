using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WGestures.Common.Annotation;

namespace WGestures.Core.Commands.Impl
{
    [Named("暂停WGestures"), Serializable]
    public class PauseWGesturesCommand : AbstractCommand,IGestureParserAware
    {
        public override void Execute()
        {
            Parser.Pause();
        }

        public GestureParser Parser { set; private get; }
    }
}
