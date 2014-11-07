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
    public partial class SendTextCommandView : CommandViewUserControl
    {
        private SendTextCommand _command;

        public override AbstractCommand Command 
        {
            get { return _command; }
            set 
            { 
                _command = (SendTextCommand) value;
                txt_text.Text = _command.Text;
            } 
        }

        public SendTextCommandView()
        {
            InitializeComponent();
        }

        private void txt_text_TextChanged(object sender, EventArgs e)
        {
            _command.Text = txt_text.Text;
        }
    }
}
