namespace WGestures.App.Gui.Windows.CommandViews
{
    partial class ScriptCommandView
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
            this.txt_script = new System.Windows.Forms.TextBox();
            this.txt_initScript = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_script
            // 
            this.txt_script.Location = new System.Drawing.Point(3, 195);
            this.txt_script.Multiline = true;
            this.txt_script.Name = "txt_script";
            this.txt_script.Size = new System.Drawing.Size(502, 155);
            this.txt_script.TabIndex = 0;
            this.txt_script.TextChanged += new System.EventHandler(this.txt_script_TextChanged);
            // 
            // txt_initScript
            // 
            this.txt_initScript.Location = new System.Drawing.Point(3, 40);
            this.txt_initScript.Multiline = true;
            this.txt_initScript.Name = "txt_initScript";
            this.txt_initScript.Size = new System.Drawing.Size(502, 101);
            this.txt_initScript.TabIndex = 0;
            this.txt_initScript.TextChanged += new System.EventHandler(this.txt_initScript_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "初始化";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "执行";
            // 
            // ScriptCommandView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_initScript);
            this.Controls.Add(this.txt_script);
            this.Name = "ScriptCommandView";
            this.Size = new System.Drawing.Size(508, 353);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_script;
        private System.Windows.Forms.TextBox txt_initScript;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
