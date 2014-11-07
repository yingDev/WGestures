using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WGestures.App.Migrate
{
    internal class MigrateException : Exception
    {
        public MigrateException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public MigrateException(string message): base(message)
        {

        }
    }
}
