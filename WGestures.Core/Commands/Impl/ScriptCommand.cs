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
    [Named("Lua脚本"), Serializable, JsonObject(MemberSerialization.OptIn)]
    public class ScriptCommand : AbstractCommand, IGestureModifiersAware, INeedInit, IGestureContextAware
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

        public GestureContext Context{set; get;}

        public void Init()
        {
            if (IsInitialized)
                throw new InvalidOperationException("Already Initialized!");

            if (_state != null) _state.Dispose();
            
            _state = new Lua();
            _state.LoadCLRPackage();

            _state.DoString(@"luanet.load_assembly('WGestures.Core');
                              luanet.load_assembly('WindowsInput');
                              luanet.load_assembly('WGestures.Common');

                              GestureModifier=luanet.import_type('WGestures.Core.GestureModifier');  
                              VK=luanet.import_type('WindowsInput.Native.VirtualKeyCode');
                              Native=luanet.import_type('WGestures.Common.OsSpecific.Windows.Native');
                            ", "_init");

            _state["Input"] = Sim.Simulator;
            _state.RegisterFunction("ReportStatus", this, typeof(ScriptCommand).GetMethod("OnReportStatus"));

            if(InitScript != null)
            {
                DoString(InitScript, "Init");
            }
            
            IsInitialized = true;
        }

        public override void Execute()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("I Need Init!");

            if(Script != null)
            {
                DoString(Script, "Execute");
            }
        }

        public void GestureRecognized(out GestureModifier observeModifiers)
        {
            if (HandleModifiers && GestureRecognizedScript != null)
            {
                var retVals = DoString(GestureRecognizedScript, "GestureRecognized");
                if(retVals.Length > 0)
                {
                    var number = (int)(GestureModifier)retVals[0];
                    
                    var gm = (GestureModifier)number;
                    Debug.WriteLine("observeModifier=" + gm);
                    observeModifiers = gm;
                    return;
                    
                }
            }

            observeModifiers = GestureModifier.None;
        }

        public void ModifierTriggered(GestureModifier modifier)
        {
            if(HandleModifiers && ModifierTriggeredScript != null)
            {
                _state["modifier"] = modifier;
                DoString(ModifierTriggeredScript, "ModifierTriggered");
                _state["modifier"] = null;
            }
        }

        public void GestureEnded()
        {
            if(HandleModifiers && GestureEndedScript != null)
            {
                DoString(GestureEndedScript, "GestureEnded");
            }
        }

        private object[] DoString(string script, string name)
        {
            _state["Context"] = this.Context;
            try
            {
                return _state.DoString(script, name);
            }catch(LuaScriptException e)
            {
                Console.WriteLine(e.InnerException);
                MessageBox.Show(e.ToString(), "Lua脚本错误");
                return new object[0];
            }
            
        }

        public void OnReportStatus(string s)
        {
            if(ReportStatus != null)
                ReportStatus(s);
        }
    }

    //helper methods
    namespace _ExportToLua
    {
        public static class WG
        {
            public static void HelloWorld()
            {
                MessageBox.Show("Hello World!");
            }

        }


    }

}
