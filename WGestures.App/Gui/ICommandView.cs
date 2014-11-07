using WGestures.Core.Commands;

namespace WGestures.App.Gui
{
    public interface ICommandView
    {
        AbstractCommand Command { get; set; }

        bool Validate();
    }
}
