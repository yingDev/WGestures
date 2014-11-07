using System;
using System.Windows.Forms;
using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;

namespace WGestures.App.Gui.Windows.CommandViews
{
    public partial class OpenFileCommandView : CommandViewUserControl
    {
        private OpenFileCommand _command;

        public OpenFileCommandView()
        {
            InitializeComponent();
        }


        public override AbstractCommand Command
        {
            get { return _command; }
            set
            {
                _command = (OpenFileCommand)value;
                tb_path.Text = _command.FilePath ?? "";
            }
        }

        private void btn_selectFile_Click(object sender, EventArgs e)
        {
            using (var fileDlg = new OpenFileDialog())
            {
                fileDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
               

                var result = fileDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var path = fileDlg.FileName;
                    tb_path.Text = path;
                    _command.FilePath = path;

                    OnCommandValueChanged();
                }
            }
        }
    }
}
