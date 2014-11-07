using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;

namespace WGestures.App.Gui.Windows.CommandViews
{
    public partial class WindowControlCommandView : CommandViewUserControl
    {
        private WindowControlCommand _command;


        public WindowControlCommandView()
        {
            InitializeComponent();
        }


        public override AbstractCommand Command
        {
            get { return _command; }
            set
            {
                _command = (WindowControlCommand) value;
                combo_operation.SelectedIndex = (int) _command.ChangeWindowStateTo;
            }
        }

        private void combo_operation_SelectedIndexChanged(object sender, EventArgs e)
        {
            _command.ChangeWindowStateTo = (WindowControlCommand.WindowOperation) combo_operation.SelectedIndex;
            OnCommandValueChanged();
        }
    }
}
