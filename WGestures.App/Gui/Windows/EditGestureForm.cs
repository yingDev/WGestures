﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using WGestures.Core;

namespace WGestures.App.Gui.Windows
{
    public partial class EditGestureForm : Form
    {
        private GestureParser _gestureParser;
        private AbstractApp _app;
        private GestureIntent _intent;

        //如果inApp不为null, 则为编辑模式
        public EditGestureForm(GestureParser gestureParser, AbstractApp inApp, GestureIntent intent = null)
        {
            _gestureParser = gestureParser;
            _app = inApp;
            _intent = intent;

            InitializeComponent();

            if (_intent != null)
            {
                if (inApp.Find(intent.Gesture) != intent)
                {
                    throw new InvalidOperationException("intent必须是inApp中的");
                }
                Text = "编辑手势";
                lb_mnemonic.Text = _intent.Gesture.ToString();
                tb_gestureName.Text = _intent.Name;
            }
            BeginCapture();

        }

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


        private void GestureParser_GestureCaptured(Gesture gesture)
        {
            if (gesture.Count() == 0 && gesture.Modifier == GestureModifier.None) return;
            Debug.WriteLine("GestureCaptured: " + gesture);
            var formerMnemonic = lb_mnemonic.Text;

            CapturedGesture = gesture;
            //if (_intent != null)
            {
                var found = _app.Find(gesture);

                if (found != null && (found != _intent))
                {
                    lb_errMsg.Text = "相同手势(" + found.Name + ")已存在，点击‘保存’会将其替代";
                    flowAlert.Visible = true;
                }
                else
                {
                    flowAlert.Visible = false;
                }
            }
                            
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
            Debug.WriteLine("开始捕获手势");
            
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

        private void tb_gestureName_TextChanged(object sender, EventArgs e)
        {
            var isNameValid = (tb_gestureName.Text.Length > 0);

            if(isNameValid)
            {
                if(_intent == null) //new
                {
                    btnOk.Enabled = (CapturedGesture != null);
                }
                else //edit
                {
                    if(CapturedGesture == null)
                    {
                        CapturedGesture = _intent.Gesture;
                        btnOk.Enabled = true;
                    }else
                    {
                        btnOk.Enabled = true;
                    }
                }
            }else
            {
                btnOk.Enabled = false;
            }
        }
    }
}
