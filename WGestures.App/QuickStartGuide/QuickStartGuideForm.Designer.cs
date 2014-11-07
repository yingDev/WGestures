using System;

namespace WGestures.App.Gui.Windows
{
    partial class QuickStartGuideForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (web_container != null)
            {
                web_container.Dispose();
                web_container = null;
            }


            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.web_container = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // web_container
            // 
            this.web_container.AllowNavigation = false;
            this.web_container.AllowWebBrowserDrop = false;
            this.web_container.IsWebBrowserContextMenuEnabled = false;
            this.web_container.Location = new System.Drawing.Point(0, 0);
            this.web_container.Margin = new System.Windows.Forms.Padding(0);
            this.web_container.MinimumSize = new System.Drawing.Size(13, 13);
            this.web_container.Name = "web_container";
            this.web_container.ScriptErrorsSuppressed = true;
            this.web_container.ScrollBarsEnabled = false;
            this.web_container.Size = new System.Drawing.Size(936, 560);
            this.web_container.TabIndex = 0;
            this.web_container.WebBrowserShortcutsEnabled = false;
            this.web_container.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.web_container_PreviewKeyDown);
            // 
            // QuickStartGuideForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(944, 534);
            this.Controls.Add(this.web_container);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickStartGuideForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WGestures快速入门";
            this.Load += new System.EventHandler(this.QuickStartGuidForm_Load);
            this.MouseEnter += new System.EventHandler(this.QuickStartGuideForm_MouseEnter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser web_container;

    }
}