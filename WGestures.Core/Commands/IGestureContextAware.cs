namespace WGestures.Core.Commands
{
    internal interface IGestureContextAware
    {
        GestureContext Context { set; }
    }
}