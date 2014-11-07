using System;
using WGestures.Core.Commands;

namespace WGestures.App.Gui
{
    public interface ICommandViewFactory<TBaseViewType> : IDisposable where TBaseViewType : ICommandView
    {
        TBaseViewType GetCommandView(AbstractCommand command);
        TBaseViewType GetCommandView(Type commandType);
        

        void Register<TC, TV>()
            where TC : AbstractCommand
            where TV : TBaseViewType;
        void UnRegisterFor<TC>() where TC : AbstractCommand;
        void UnRegisterAll();
    }
}
