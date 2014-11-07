namespace WGestures.App.Gui.Windows
{
    partial class ImportForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_filePath = new System.Windows.Forms.TextBox();
            this.btn_selectWgb = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.group_importOptions = new System.Windows.Forms.GroupBox();
            this.check_importConfig = new System.Windows.Forms.CheckBox();
            this.check_importGestures = new System.Windows.Forms.CheckBox();
            this.combo_importGesturesOption = new System.Windows.Forms.ComboBox();
            this.flowAlert = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lb_errMsg = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.openFile_wgb = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.group_importOptions.SuspendLayout();
            this.flowAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_filePath
            // 
            this.txt_filePath.Location = new System.Drawing.Point(0, 3);
            this.txt_filePath.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.txt_filePath.Name = "txt_filePath";
            this.txt_filePath.ReadOnly = true;
            this.txt_filePath.Size = new System.Drawing.Size(193, 21);
            this.txt_filePath.TabIndex = 0;
            this.txt_filePath.TabStop = false;
            this.txt_filePath.Text = "请选择要导入的文件";
            this.txt_filePath.WordWrap = false;
            // 
            // btn_selectWgb
            // 
            this.btn_selectWgb.Location = new System.Drawing.Point(199, 3);
            this.btn_selectWgb.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btn_selectWgb.Name = "btn_selectWgb";
            this.btn_selectWgb.Size = new System.Drawing.Size(75, 21);
            this.btn_selectWgb.TabIndex = 1;
            this.btn_selectWgb.Text = "选择...";
            this.btn_selectWgb.UseVisualStyleBackColor = true;
            this.btn_selectWgb.Click += new System.EventHandler(this.btn_selectWgb_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.txt_filePath);
            this.flowLayoutPanel1.Controls.Add(this.btn_selectWgb);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(11, 11);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(274, 27);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel1);
            this.flowLayoutPanel2.Controls.Add(this.group_importOptions);
            this.flowLayoutPanel2.Controls.Add(this.flowAlert);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(8);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(302, 197);
            this.flowLayoutPanel2.TabIndex = 4;
            // 
            // group_importOptions
            // 
            this.group_importOptions.Controls.Add(this.check_importConfig);
            this.group_importOptions.Controls.Add(this.check_importGestures);
            this.group_importOptions.Controls.Add(this.combo_importGesturesOption);
            this.group_importOptions.Location = new System.Drawing.Point(11, 57);
            this.group_importOptions.Margin = new System.Windows.Forms.Padding(3, 16, 3, 3);
            this.group_importOptions.Name = "group_importOptions";
            this.group_importOptions.Size = new System.Drawing.Size(274, 90);
            this.group_importOptions.TabIndex = 4;
            this.group_importOptions.TabStop = false;
            this.group_importOptions.Text = "导入方式";
            this.group_importOptions.Visible = false;
            // 
            // check_importConfig
            // 
            this.check_importConfig.AutoSize = true;
            this.check_importConfig.Location = new System.Drawing.Point(17, 58);
            this.check_importConfig.Name = "check_importConfig";
            this.check_importConfig.Size = new System.Drawing.Size(126, 16);
            this.check_importConfig.TabIndex = 2;
            this.check_importConfig.Text = "导入WGestures选项";
            this.check_importConfig.UseVisualStyleBackColor = true;
            this.check_importConfig.CheckedChanged += new System.EventHandler(this.check_importConfig_CheckedChanged);
            // 
            // check_importGestures
            // 
            this.check_importGestures.AutoSize = true;
            this.check_importGestures.Location = new System.Drawing.Point(17, 27);
            this.check_importGestures.Name = "check_importGestures";
            this.check_importGestures.Size = new System.Drawing.Size(72, 16);
            this.check_importGestures.TabIndex = 2;
            this.check_importGestures.Text = "导入手势";
            this.check_importGestures.UseVisualStyleBackColor = true;
            this.check_importGestures.CheckedChanged += new System.EventHandler(this.check_importGestures_CheckedChanged);
            // 
            // combo_importGesturesOption
            // 
            this.combo_importGesturesOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_importGesturesOption.Enabled = false;
            this.combo_importGesturesOption.FormattingEnabled = true;
            this.combo_importGesturesOption.Items.AddRange(new object[] {
            "合并现有手势",
            "替换现有手势"});
            this.combo_importGesturesOption.Location = new System.Drawing.Point(95, 25);
            this.combo_importGesturesOption.Name = "combo_importGesturesOption";
            this.combo_importGesturesOption.Size = new System.Drawing.Size(152, 20);
            this.combo_importGesturesOption.TabIndex = 1;
            this.combo_importGesturesOption.SelectedIndexChanged += new System.EventHandler(this.combo_importGesturesOption_SelectedIndexChanged);
            // 
            // flowAlert
            // 
            this.flowAlert.Controls.Add(this.pictureBox1);
            this.flowAlert.Controls.Add(this.lb_errMsg);
            this.flowAlert.Location = new System.Drawing.Point(11, 158);
            this.flowAlert.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.flowAlert.Name = "flowAlert";
            this.flowAlert.Size = new System.Drawing.Size(274, 22);
            this.flowAlert.TabIndex = 8;
            this.flowAlert.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WGestures.App.Properties.Resources.Alert;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 15);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // lb_errMsg
            // 
            this.lb_errMsg.AutoSize = true;
            this.lb_errMsg.ForeColor = System.Drawing.Color.Red;
            this.lb_errMsg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lb_errMsg.Location = new System.Drawing.Point(25, 3);
            this.lb_errMsg.Margin = new System.Windows.Forms.Padding(3);
            this.lb_errMsg.Name = "lb_errMsg";
            this.lb_errMsg.Size = new System.Drawing.Size(29, 12);
            this.lb_errMsg.TabIndex = 4;
            this.lb_errMsg.Text = "错误";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flowLayoutPanel4.Controls.Add(this.btnCancel);
            this.flowLayoutPanel4.Controls.Add(this.btnOk);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 197);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(8, 18, 8, 8);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Padding = new System.Windows.Forms.Padding(6);
            this.flowLayoutPanel4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel4.Size = new System.Drawing.Size(302, 42);
            this.flowLayoutPanel4.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(216, 8);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 10, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(132, 8);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 26);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "导入";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // openFile_wgb
            // 
            this.openFile_wgb.DefaultExt = "wgb";
            this.openFile_wgb.Filter = "WGestures备份文件 (*.wgb)|*.wgb|WGestures 1.2手势文件|*.json";
            this.openFile_wgb.Title = "选择要导入的文件";
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(302, 239);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "导入到WGestures";
            this.Load += new System.EventHandler(this.ImportForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.group_importOptions.ResumeLayout(false);
            this.group_importOptions.PerformLayout();
            this.flowAlert.ResumeLayout(false);
            this.flowAlert.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_filePath;
        private System.Windows.Forms.Button btn_selectWgb;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox group_importOptions;
        private System.Windows.Forms.ComboBox combo_importGesturesOption;
        private System.Windows.Forms.CheckBox check_importConfig;
        private System.Windows.Forms.CheckBox check_importGestures;
        private System.Windows.Forms.OpenFileDialog openFile_wgb;
        private System.Windows.Forms.FlowLayoutPanel flowAlert;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lb_errMsg;
    }
}