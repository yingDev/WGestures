﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WGestures.App.Properties;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.App.Gui.Windows
{
    public partial class QuickStartGuideForm : Form
    {
        public QuickStartGuideForm()
        {
            try
            {
                InitializeComponent();
            }catch(Exception e)
            {
                //IGNORE
                Debug.WriteLine(e);
            }
            
            var dpiScale = Native.GetScreenDpi() / 96;
            ClientSize = new System.Drawing.Size(936 * dpiScale, 525 * dpiScale);
            Icon = Resources.icon;

            //TopMost = true;

        }

        private void QuickStartGuidForm_Load(object sender, EventArgs e)
        {
            web_container.Navigate(new Uri(
                string.Format(@"{0}\QuickStartGuide\index.html",
                Path.GetDirectoryName(Application.ExecutablePath))));



            Activate();

        }


        private void web_container_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           if(web_container.Document == null) return;

           if (e.KeyData == (Keys.Alt | Keys.Right) || e.KeyData == (Keys.Control | Keys.Tab))
            {
                web_container.Document.InvokeScript("performNext");
            }else if (e.KeyData == (Keys.Alt | Keys.Left) || e.KeyData == (Keys.Control | Keys.Tab | Keys.Shift))
            {
                web_container.Document.InvokeScript("performPrev");
            }else if (e.KeyData == (Keys.Control | Keys.W))
            {
                Close();
            }

        }

        protected override void OnClosed(EventArgs e)
        {            
            web_container.PreviewKeyDown -= web_container_PreviewKeyDown;

            //web_container.Navigate("about:blank");
            while (web_container.IsBusy)
            {
                Thread.Sleep(100);
                //Console.WriteLine("buuuusy");
            }

            base.OnClosed(e);
        }

        private void QuickStartGuideForm_MouseEnter(object sender, EventArgs e)
        {
        }
    }
}
