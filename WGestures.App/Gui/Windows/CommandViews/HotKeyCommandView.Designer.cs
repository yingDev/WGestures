namespace WGestures.App.Gui.Windows.CommandViews
{
    partial class HotKeyCommandView
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
            if (disposing)
            {
                hook.unhook();
                hook = null;
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
            this.btn_recordHotkey = new System.Windows.Forms.Button();
            this.lb_shortcut = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_recordHotkey
            // 
            this.btn_recordHotkey.Location = new System.Drawing.Point(3, 5);
            this.btn_recordHotkey.Name = "btn_recordHotkey";
            this.btn_recordHotkey.Size = new System.Drawing.Size(75, 23);
            this.btn_recordHotkey.TabIndex = 1;
            this.btn_recordHotkey.Text = "录入快捷键";
            this.btn_recordHotkey.UseVisualStyleBackColor = true;
            this.btn_recordHotkey.Click += new System.EventHandler(this.btn_recordHotkey_Click);
            // 
            // lb_shortcut
            // 
            this.lb_shortcut.AutoSize = true;
            this.lb_shortcut.Location = new System.Drawing.Point(84, 10);
            this.lb_shortcut.Margin = new System.Windows.Forms.Padding(3);
            this.lb_shortcut.Name = "lb_shortcut";
            this.lb_shortcut.Size = new System.Drawing.Size(77, 12);
            this.lb_shortcut.TabIndex = 2;
            this.lb_shortcut.Text = "未指定快捷键";
            this.lb_shortcut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HotKeyCommandView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_recordHotkey);
            this.Controls.Add(this.lb_shortcut);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "HotKeyCommandView";
            this.Size = new System.Drawing.Size(164, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private MouseKeyboardActivityMonitor.Controls.MouseKeyEventProvider mouseKeyEventProvider;
        private System.Windows.Forms.Button btn_recordHotkey;
        private System.Windows.Forms.Label lb_shortcut;
    }
}
