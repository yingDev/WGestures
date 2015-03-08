using System;
using System.Drawing;
using System.IO;

namespace WGestures.Core
{
    public class PathEventArgs : EventArgs
    {
        public Point Location{get; set;}
        public GestureButtons Button;
        public GestureModifier Modifier { get; set; }
        public GestureContext Context { get; set; }

        public PathEventArgs(Point location, GestureContext context)
        {
            Location = location;
            Context = context;
        }

        public PathEventArgs()
        {
            
        }
    }

    public class BeforePathStartEventArgs
    {
        public PathEventArgs PathEventArgs { get; private set; }
        public bool ShouldPathStart { get; set; }
        public GestureContext Context { get; private set; }

        public BeforePathStartEventArgs(PathEventArgs pathEventArgs)
        {
            PathEventArgs = pathEventArgs;
            Context = pathEventArgs.Context;
            ShouldPathStart = true;
        }
    }

    public enum ScreenCorner
    {
        LeftBottom, LeftTop, RightTop, RightBottom
    }

    public delegate void PathTrackEventHandler(PathEventArgs args);

    public delegate void BeforePathStartEventHandler(BeforePathStartEventArgs args);

    public interface IPathTracker : IDisposable
    {
        bool Paused { get; set; }
        void Start();
        void Stop();

        void SuspendTemprarily(GestureModifier filteredModifiers);
        bool IsSuspended { get; }

        event BeforePathStartEventHandler BeforePathStart; 

        event PathTrackEventHandler PathStart;
        event PathTrackEventHandler PathGrow;
        event PathTrackEventHandler EffectivePathGrow;
        event PathTrackEventHandler PathEnd;
        event PathTrackEventHandler PathTimeout;
        event PathTrackEventHandler PathModifier;

        event Action<ScreenCorner> HotCornerTriggered;
    }
}
