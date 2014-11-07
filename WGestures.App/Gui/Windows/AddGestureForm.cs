using System;
using System.Windows.Forms;
using WGestures.Core;

namespace WGestures.App.Gui.Windows
{
    public partial class AddGestureForm : Form
    {
        private GestureParser _gestureParser;
       // private AbstractApp _app;

        public Gesture CapturedGesture { get; set; }

        public string GestureName
        {
            get
            {
                if (!string.IsNullOrEmpty(tb_gestureName.Text))
                {
                    return tb_gestureName.Text;
                }

                return CapturedGesture.ToString();
            }
        }

        public AddGestureForm(GestureParser gestureParser)
        {
            _gestureParser = gestureParser;

            InitializeComponent();

            BeginCapture();

        }

        private void GestureParser_GestureCaptured(Gesture gesture)
        {
#if DEBUG
            Console.WriteLine("{0} - GestureCaptured: {1}", typeof(AddGestureForm), gesture);
#endif
            var formerMnemonic = lb_mnemonic.Text;

            CapturedGesture = gesture;
            lb_mnemonic.Text = gesture.ToString();

            if (formerMnemonic == tb_gestureName.Text|| string.IsNullOrEmpty(tb_gestureName.Text))
            {
                tb_gestureName.Text = lb_mnemonic.Text;
                tb_gestureName.SelectAll();
                tb_gestureName.Focus();
            }
            else
            {
                btnOk.Focus();
            }

            btnOk.Enabled = true;
            
        }

        private void BeginCapture()
        {
#if DEBUG
            Console.WriteLine("{0} - {1}", typeof(AddGestureForm), "开始捕获手势");
#endif
            _gestureParser.GestureCaptured += GestureParser_GestureCaptured;
            _gestureParser.IsInCaptureMode = true;

        }

        private void CancelCapture()
        {
            _gestureParser.GestureCaptured -= GestureParser_GestureCaptured;
            _gestureParser.IsInCaptureMode = false;

        }



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CancelCapture();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tb_gestureName_KeyDown(object sender, KeyEventArgs e)
        {
            if(btnOk.Enabled && e.KeyCode == Keys.Return) btnOk.PerformClick();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.W))
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
