namespace WGestures.App.Gui.Windows.CommandViews
{
    partial class CmdCommandView
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.check_setWorkingDir = new System.Windows.Forms.CheckBox();
            this.txt_CmdLine = new System.Windows.Forms.TextBox();
            this.check_ShowWindow = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // check_setWorkingDir
            // 
            this.check_setWorkingDir.AutoSize = true;
            this.check_setWorkingDir.Location = new System.Drawing.Point(131, 62);
            this.check_setWorkingDir.Name = "check_setWorkingDir";
            this.check_setWorkingDir.Size = new System.Drawing.Size(156, 16);
            this.check_setWorkingDir.TabIndex = 2;
            this.check_setWorkingDir.Text = "根据上下文设置工作目录";
            this.check_setWorkingDir.UseVisualStyleBackColor = true;
            this.check_setWorkingDir.CheckedChanged += new System.EventHandler(this.check_setWorkingDir_CheckedChanged);
            // 
            // txt_CmdLine
            // 
            this.txt_CmdLine.Location = new System.Drawing.Point(3, 5);
            this.txt_CmdLine.Multiline = true;
            this.txt_CmdLine.Name = "txt_CmdLine";
            this.txt_CmdLine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_CmdLine.Size = new System.Drawing.Size(294, 51);
            this.txt_CmdLine.TabIndex = 1;
            this.txt_CmdLine.TextChanged += new System.EventHandler(this.txt_CmdLine_TextChanged);
            // 
            // check_ShowWindow
            // 
            this.check_ShowWindow.AutoSize = true;
            this.check_ShowWindow.Location = new System.Drawing.Point(3, 62);
            this.check_ShowWindow.Name = "check_ShowWindow";
            this.check_ShowWindow.Size = new System.Drawing.Size(108, 16);
            this.check_ShowWindow.TabIndex = 0;
            this.check_ShowWindow.Text = "显示命令行窗口";
            this.check_ShowWindow.UseVisualStyleBackColor = true;
            this.check_ShowWindow.CheckedChanged += new System.EventHandler(this.check_ShowWindow_CheckedChanged);
            // 
            // CmdCommandView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.check_setWorkingDir);
            this.Controls.Add(this.txt_CmdLine);
            this.Controls.Add(this.check_ShowWindow);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "CmdCommandView";
            this.Size = new System.Drawing.Size(300, 81);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox check_ShowWindow;
        private System.Windows.Forms.TextBox txt_CmdLine;
        private System.Windows.Forms.CheckBox check_setWorkingDir;
    }
}
