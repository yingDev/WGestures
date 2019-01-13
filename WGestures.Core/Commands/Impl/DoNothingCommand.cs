using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WGestures.Common.Annotation;

namespace WGestures.Core.Commands.Impl
{
    [Named("(什么也不做)"), Serializable]
    public class DoNothingCommand : AbstractCommand
    {
        public override void Execute()
        {
            //啥也不做~
        }

        public override string Description()
        {
            return "";
        }
    }
}
