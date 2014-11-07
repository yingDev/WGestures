namespace WGestures.App.Gui.Windows
{
    partial class UpdateInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateInfoForm));
            this.tb_whatsNew = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lnk_gotoUrl = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_currentVersion = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lb_newVersion = new System.Windows.Forms.Label();
            this.btn_ok = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_whatsNew
            // 
            this.tb_whatsNew.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_whatsNew.Location = new System.Drawing.Point(3, 18);
            this.tb_whatsNew.Multiline = true;
            this.tb_whatsNew.Name = "tb_whatsNew";
            this.tb_whatsNew.ReadOnly = true;
            this.tb_whatsNew.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_whatsNew.Size = new System.Drawing.Size(341, 143);
            this.tb_whatsNew.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "WGestures发布了新版本！";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "主要更新内容:";
            // 
            // lnk_gotoUrl
            // 
            this.lnk_gotoUrl.AutoSize = true;
            this.lnk_gotoUrl.Location = new System.Drawing.Point(241, 0);
            this.lnk_gotoUrl.Name = "lnk_gotoUrl";
            this.lnk_gotoUrl.Size = new System.Drawing.Size(85, 19);
            this.lnk_gotoUrl.TabIndex = 3;
            this.lnk_gotoUrl.TabStop = true;
            this.lnk_gotoUrl.Text = "前往下载";
            this.lnk_gotoUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_gotoUrl_LinkClicked);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.lnk_gotoUrl);
            this.flowLayoutPanel1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(11, 10);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(329, 19);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.lb_currentVersion);
            this.flowLayoutPanel2.Controls.Add(this.label5);
            this.flowLayoutPanel2.Controls.Add(this.lb_newVersion);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(11, 38);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(224, 12);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "当前版本:";
            // 
            // lb_currentVersion
            // 
            this.lb_currentVersion.AutoSize = true;
            this.lb_currentVersion.ForeColor = System.Drawing.Color.Gray;
            this.lb_currentVersion.Location = new System.Drawing.Point(68, 0);
            this.lb_currentVersion.Name = "lb_currentVersion";
            this.lb_currentVersion.Size = new System.Drawing.Size(47, 12);
            this.lb_currentVersion.TabIndex = 1;
            this.lb_currentVersion.Text = "x.x.x.x";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(121, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "新版本:";
            // 
            // lb_newVersion
            // 
            this.lb_newVersion.AutoSize = true;
            this.lb_newVersion.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lb_newVersion.Location = new System.Drawing.Point(174, 0);
            this.lb_newVersion.Name = "lb_newVersion";
            this.lb_newVersion.Size = new System.Drawing.Size(47, 12);
            this.lb_newVersion.TabIndex = 1;
            this.lb_newVersion.Text = "y.y.y.y";
            // 
            // btn_ok
            // 
            this.btn_ok.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_ok.Location = new System.Drawing.Point(114, 244);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(3, 2, 3, 8);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(132, 23);
            this.btn_ok.TabIndex = 6;
            this.btn_ok.Text = "关闭";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel3.Controls.Add(this.label2);
            this.flowLayoutPanel3.Controls.Add(this.tb_whatsNew);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(11, 66);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(348, 171);
            this.flowLayoutPanel3.TabIndex = 7;
            // 
            // UpdateInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(366, 289);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateInfoForm";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新版本可用";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_whatsNew;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnk_gotoUrl;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_currentVersion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lb_newVersion;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    }
}