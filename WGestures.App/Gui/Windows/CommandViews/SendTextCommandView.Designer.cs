namespace WGestures.App.Gui.Windows.CommandViews
{
    partial class SendTextCommandView
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
            this.txt_text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_text
            // 
            this.txt_text.Location = new System.Drawing.Point(3, 5);
            this.txt_text.Multiline = true;
            this.txt_text.Name = "txt_text";
            this.txt_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_text.Size = new System.Drawing.Size(254, 50);
            this.txt_text.TabIndex = 0;
            this.txt_text.TextChanged += new System.EventHandler(this.txt_text_TextChanged);
            // 
            // SendTextCommandView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_text);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "SendTextCommandView";
            this.Size = new System.Drawing.Size(260, 58);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_text;
    }
}
