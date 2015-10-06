using System;
using NLua;
using WGestures.Common.Annotation;
using NLua.Exceptions;
using NLua.Event;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace WGestures.Core.Commands.Impl
{
    [Named("执行脚本"), Serializable, JsonObject(MemberSerialization.OptIn)]
    public class ScriptCommand : AbstractCommand, IGestureModifiersAware, INeedInit
    {
        Lua _state;
        string _initScript;

        public event Action<string> ReportStatus;

        [JsonProperty]
        public string InitScript
        {
            get{ return _initScript; }
            set
            {
                _initScript = value;
                IsInitialized = false;//需要重新初始化
            }
        }
        [JsonProperty]
        public string Script { get; set; }

        [JsonProperty]
        public bool HandleModifiers { get; set; }

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

            if (_state != null) _state.Dispose();

            _state = new Lua();
            _state.LoadCLRPackage();

            if(InitScript != null)
            {
                DoString(InitScript);
            }
            
            IsInitialized = true;
        }

        public override void Execute()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("I Need Init!");

            if(Script != null)
            {
                DoString(Script);
            }
        }

        public void GestureRecognized(out GestureModifier observeModifiers)
        {
            if (HandleModifiers && GestureRecognizedScript != null)
            {
                var retVals = DoString(GestureRecognizedScript);
                if(retVals.Length > 0)
                {
                    var gm = retVals[0] as GestureModifier?;
                    if(gm != null)
                    {
                        observeModifiers = gm.Value;
                        return;
                    }
                }
            }

            observeModifiers = GestureModifier.None;
        }

        public void ModifierTriggered(GestureModifier modifier)
        {
            if(HandleModifiers && ModifierTriggeredScript != null)
            {
                _state["modifier"] = modifier;
                DoString(ModifierTriggeredScript);
                _state["modifier"] = null;
            }
        }

        public void GestureEnded()
        {
            if(HandleModifiers && GestureEndedScript != null)
            {
                DoString(GestureEndedScript);
            }
        }

        private object[] DoString(string script)
        {
            try
            {
                return _state.DoString(script);
            }catch(LuaScriptException e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.ToString(), "Lua脚本错误");
                return new object[0];
            }
            
        }
    }

    //helper methods
    static class WG
    {

    }
}
