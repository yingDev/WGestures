using System.ComponentModel;
using WGestures.Common.Annotation;
using WGestures.Core.Annotations;

namespace WGestures.Core.Commands
{
    public abstract class AbstractCommand
    {
        public abstract void Execute();

        public virtual string Description()
        {
            return NamedAttribute.GetNameOf(this.GetType());
        }

    }
}
