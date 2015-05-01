using System;
using System.Drawing;

namespace WGestures.Core
{
    public abstract class GestureContext : MarshalByRefObject
    {
        public Point StartPoint;
        public Point EndPoint;

        public uint ProcId;
        public IntPtr WinId;

        public GestureButtons GestureButton;

    }
}