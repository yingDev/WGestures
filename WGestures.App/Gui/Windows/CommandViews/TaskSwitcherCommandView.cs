using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;

namespace WGestures.App.Gui.Windows.CommandViews
{
    class TaskSwitcherCommandView : CommandViewUserControl
    {
        public TaskSwitcherCommandView()
        {
            InitializeComponent();
        }
        private TaskSwitcherCommand _command;

        public override AbstractCommand Command 
        {
            get { return _command; }
            set
            {
                _command = (TaskSwitcherCommand) value;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TaskSwitcherCommandView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Name = "TaskSwitcherCommandView";
            this.ResumeLayout(false);

        }

        private void check_prevTask_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
