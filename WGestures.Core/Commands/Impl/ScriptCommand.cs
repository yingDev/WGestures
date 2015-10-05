using System;
using NLua;
using WGestures.Common.Annotation;
using NLua.Exceptions;
using NLua.Event;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WGestures.Core.Commands.Impl
{
    [Named("执行脚本"), Serializable, JsonObject(MemberSerialization.OptIn)]
    public class ScriptCommand : AbstractCommand/*, IGestureModifiersAware*/, INeedInit
    {
        Lua _state;

        public event Action<string> ReportStatus;

        [JsonProperty]
        public string InitScript { get; set; }
        [JsonProperty]
        public string Script { get; set; }

        //for modified gestures
        [JsonProperty]
        public string GestureRecognizedScript { get; set; }
        [JsonProperty]
        public string ModifierTriggeredScript { get; set; }
        [JsonProperty]
        public string GestureEndedScript { get; set; }


        public ScriptCommand()
        {
        }

        //INeedInit
        public bool IsInitialized { get; private set; }

        public void Init()
        {
            if (IsInitialized)
                throw new InvalidOperationException("Already Initialized!");

            _state = new Lua();
            _state.HookException += _hookLuaException;
            _state.LoadCLRPackage();

            if(InitScript != null)
            {
                _state.DoString(InitScript);
            }

           

            IsInitialized = true;
        }

        public override void Execute()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("I Need Init!");

            if(Script != null)
            {
                _state.DoString(Script);
            }
        }

        public void GestureRecognized(out GestureModifier observeModifiers)
        {
            throw new NotImplementedException();
        }

        public void ModifierTriggered(GestureModifier modifier)
        {
            throw new NotImplementedException();
        }

        public void GestureEnded()
        {
            throw new NotImplementedException();
        }

        private void _hookLuaException(object sender, HookExceptionEventArgs args)
        {
            //todo: impl
            Debug.WriteLine(args.Exception);
        }
    }

    //helper methods
    static class WG
    {

    }
}
