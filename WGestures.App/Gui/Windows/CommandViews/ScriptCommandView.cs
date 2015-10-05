using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;

namespace WGestures.App.Gui.Windows.CommandViews
{
    public partial class ScriptCommandView : CommandViewUserControl
    {
        ScriptCommand _cmd;

        public ScriptCommandView()
        {
            InitializeComponent();
        }

        public override AbstractCommand Command
        {
            get { return _cmd; }
            set
            {
                _cmd = (ScriptCommand)value;
                txt_initScript.Text = _cmd.InitScript;
                txt_script.Text = _cmd.Script;
            }
        }

        private void txt_script_TextChanged(object sender, System.EventArgs e)
        {
            _cmd.Script = txt_script.Text;
        }

        private void txt_initScript_TextChanged(object sender, System.EventArgs e)
        {
            _cmd.InitScript = txt_initScript.Text;
        }
    }
}
