using System;
using System.Collections.Generic;
using WGestures.Core.Commands;

namespace WGestures.App.Gui
{
    internal class CommandViewFactory<TBaseViewType> : ICommandViewFactory<TBaseViewType> 
        where TBaseViewType : ICommandView
    {

        private readonly Dictionary<Type, Type> _commandViewRegistry = new Dictionary<Type, Type>();

        private bool _enableCaching = true;
        public bool EnableCaching
        {
            get { return _enableCaching; }
            set { _enableCaching = value; }
        }

        public bool IsDisposed { get; private set; }

        private readonly Dictionary<Type, TBaseViewType> _viewInstanceCache = new Dictionary<Type, TBaseViewType>();


        public TBaseViewType GetCommandView(AbstractCommand command)
        {
            var cmdType = command.GetType();

            var closestParentKv = new KeyValuePair<Type, Type>(null,null);
            foreach (var kv in _commandViewRegistry)
            {
                //如果精确匹配，则直接返回
                if (kv.Key == cmdType) return ConstructCommandView(kv.Value, command);

                //否则，找出一个最具体的父类型
                if (cmdType.IsSubclassOf(kv.Key) && 
                    (closestParentKv.Key == null || kv.Key.IsSubclassOf(closestParentKv.Key))) 
                    closestParentKv = kv;
            }

            return closestParentKv.Key == null ? default(TBaseViewType) : ConstructCommandView(closestParentKv.Value, command);
        }

        public TBaseViewType GetCommandView(Type commandType)
        {
            var cmd = Activator.CreateInstance(commandType) as AbstractCommand;
            return GetCommandView(cmd);
        }


        public void Register<TC, TV>()
            where TC : AbstractCommand
            where TV : TBaseViewType
        {
            var cmdType = typeof(TC);
            var viewType = typeof(TV);

            Type registeredType;
            _commandViewRegistry.TryGetValue(cmdType, out registeredType);

            if (registeredType != null) throw new InvalidOperationException(string.Format("命令类型 {0} 已经注册到{1}", cmdType, registeredType));

            _commandViewRegistry[cmdType] = viewType;

           // if(EnableCaching) _viewInstanceCache.Add(cmdType,GetCommandView(cmdType));
        }

        public void UnRegisterFor<TC>() where TC : AbstractCommand
        {
            _commandViewRegistry.Remove(typeof(TC));
        }

        public void UnRegisterAll()
        {
            _commandViewRegistry.Clear(); 
            _viewInstanceCache.Clear();
        }

        private TBaseViewType ConstructCommandView(Type viewType, AbstractCommand command)
        {
            var view = default(TBaseViewType);

            if (EnableCaching)
            {
                TBaseViewType found;
                _viewInstanceCache.TryGetValue(viewType, out found);
                if (found != null)
                {
                    found.Command = command;
                    return found;
                }

                view = (TBaseViewType)Activator.CreateInstance(viewType);
                view.Command = command;

                _viewInstanceCache[viewType] = view;
                return view;
            }


            view = (TBaseViewType)Activator.CreateInstance(viewType);
            view.Command = command;

            return view;
        }

        public void Dispose()
        {
            foreach (var kv in _viewInstanceCache)
            {
                var view = kv.Value as IDisposable;
                if(view != null) view.Dispose();
            }

            UnRegisterAll();

            IsDisposed = true;
        }
    }
}
