namespace WGestures.App.Gui.Windows.CommandViews
{
    partial class WindowControlCommandView
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
            this.label1 = new System.Windows.Forms.Label();
            this.combo_operation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "窗口操作";
            // 
            // combo_operation
            // 
            this.combo_operation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_operation.FormattingEnabled = true;
            this.combo_operation.Items.AddRange(new object[] {
            "最大化/复原",
            "最小化",
            "关闭"});
            this.combo_operation.Location = new System.Drawing.Point(62, 2);
            this.combo_operation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.combo_operation.Name = "combo_operation";
            this.combo_operation.Size = new System.Drawing.Size(121, 20);
            this.combo_operation.TabIndex = 1;
            this.combo_operation.SelectedIndexChanged += new System.EventHandler(this.combo_operation_SelectedIndexChanged);
            // 
            // WindowControlCommandView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.combo_operation);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "WindowControlCommandView";
            this.Size = new System.Drawing.Size(186, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combo_operation;
    }
}
