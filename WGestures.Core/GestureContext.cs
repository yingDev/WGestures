using System;
using System.Drawing;

namespace WGestures.Core
{
    public abstract class GestureContext : MarshalByRefObject
    {
        public Point StartPoint;
        public Point EndPoint;

        public GestureButtons GestureButton;

        public virtual bool IsInFullScreenMode { get{return false;} }

    }
}