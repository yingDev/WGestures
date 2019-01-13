namespace WGestures.Core.Commands
{
    interface INeedInit
    {
        void Init();
        bool IsInitialized { get; }
    }
}
