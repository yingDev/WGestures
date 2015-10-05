using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGestures.Core.Commands
{
    interface INeedInit
    {
        void Init();
        bool IsInitialized { get; }
    }
}
