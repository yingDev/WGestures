using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;
using WGestures.Core.Persistence;

namespace WGestures.App.Gui.Windows.CommandViews
{
    public partial class CmdCommandView : CommandViewUserControl
    {
        private readonly static string explorerPath = Environment.GetEnvironmentVariable("windir") + Path.DirectorySeparatorChar + "explorer.exe";

        private CmdCommand _command;

        public override AbstractCommand Command
        {
            get { return _command; }
            set
            {
                _command = (CmdCommand) value;
                check_ShowWindow.Checked = _command.ShowWindow;
                check_setWorkingDir.Checked = _command.AutoSetWorkingDir;
                txt_CmdLine.Text = _command.Code ?? "echo Hello";
            }
        }


        public CmdCommandView()
        {
            InitializeComponent();
        }

        private void check_ShowWindow_CheckedChanged(object sender, System.EventArgs e)
        {
            _command.ShowWindow = check_ShowWindow.Checked;

        }

        private void txt_CmdLine_TextChanged(object sender, System.EventArgs e)
        {
            _command.Code = txt_CmdLine.Text;

            OnCommandValueChanged();
        }



        private void check_setWorkingDir_CheckedChanged(object sender, EventArgs e)
        {
            _command.AutoSetWorkingDir = check_setWorkingDir.Checked;
        }
    }
}
