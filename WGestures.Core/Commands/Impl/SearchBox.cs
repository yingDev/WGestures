using System.Windows.Forms;

namespace WGestures.Core.Commands.Impl
{
    public partial class SearchBox : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public SearchBox()
        {
            InitializeComponent();
        }

        private void SearchBox_Shown(object sender, System.EventArgs e)
        {
            Activate();
        }

        public void SetSearchText(string txt)
        {
            txt_content.Text = txt;
            txt_content.SelectAll();
        }

        private void SearchBox_Deactivate(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
