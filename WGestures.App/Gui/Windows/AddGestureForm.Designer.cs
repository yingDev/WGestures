using WGestures.App.Gui.Windows.Controls;

namespace WGestures.App.Gui.Windows
{
    partial class AddGestureForm
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
                _gestureParser = null;
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
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_gestureName = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lb_capturing = new System.Windows.Forms.Label();
            this.lb_mnemonic = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.lineFlowLayout1 = new WGestures.App.Gui.Windows.Controls.LineFlowLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flowLayoutPanel4.Controls.Add(this.btnCancel);
            this.flowLayoutPanel4.Controls.Add(this.btnOk);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 243);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Padding = new System.Windows.Forms.Padding(6);
            this.flowLayoutPanel4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel4.Size = new System.Drawing.Size(365, 42);
            this.flowLayoutPanel4.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(279, 8);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 9, 2);
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
            this.btnOk.Location = new System.Drawing.Point(196, 8);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 26);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "保存";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "手势名称";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tb_gestureName
            // 
            this.tb_gestureName.Location = new System.Drawing.Point(66, 2);
            this.tb_gestureName.Margin = new System.Windows.Forms.Padding(2);
            this.tb_gestureName.Name = "tb_gestureName";
            this.tb_gestureName.Size = new System.Drawing.Size(259, 21);
            this.tb_gestureName.TabIndex = 8;
            this.tb_gestureName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_gestureName_KeyDown);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.tb_gestureName);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 109);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(346, 49);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // lb_capturing
            // 
            this.lb_capturing.BackColor = System.Drawing.Color.Transparent;
            this.lb_capturing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lb_capturing.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_capturing.Location = new System.Drawing.Point(7, 5);
            this.lb_capturing.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_capturing.Name = "lb_capturing";
            this.lb_capturing.Size = new System.Drawing.Size(340, 34);
            this.lb_capturing.TabIndex = 10;
            this.lb_capturing.Text = "请用鼠标画出手势";
            this.lb_capturing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_mnemonic
            // 
            this.lb_mnemonic.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_mnemonic.ForeColor = System.Drawing.Color.Orange;
            this.lb_mnemonic.Location = new System.Drawing.Point(7, 59);
            this.lb_mnemonic.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_mnemonic.Name = "lb_mnemonic";
            this.lb_mnemonic.Size = new System.Drawing.Size(344, 41);
            this.lb_mnemonic.TabIndex = 11;
            this.lb_mnemonic.Text = " ...";
            this.lb_mnemonic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel2.Controls.Add(this.lb_capturing);
            this.flowLayoutPanel2.Controls.Add(this.lineFlowLayout1);
            this.flowLayoutPanel2.Controls.Add(this.lb_mnemonic);
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel1);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(365, 243);
            this.flowLayoutPanel2.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(7, 158);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(344, 45);
            this.label2.TabIndex = 13;
            this.label2.Text = "* 你可以使用鼠标 右键 或 中键 绘制手势（它们并不等价）\r\n*  ↗↙↘↖方向只支持单笔手势，不能与任何方向组合。";
            // 
            // lineFlowLayout1
            // 
            this.lineFlowLayout1.ForeColor = System.Drawing.Color.SkyBlue;
            this.lineFlowLayout1.Location = new System.Drawing.Point(7, 41);
            this.lineFlowLayout1.Margin = new System.Windows.Forms.Padding(2);
            this.lineFlowLayout1.Name = "lineFlowLayout1";
            this.lineFlowLayout1.Size = new System.Drawing.Size(344, 16);
            this.lineFlowLayout1.TabIndex = 12;
            this.lineFlowLayout1.Vertical = false;
            // 
            // AddGestureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(365, 285);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddGestureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加手势";
            this.TopMost = true;
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_gestureName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lb_capturing;
        private System.Windows.Forms.Label lb_mnemonic;
        private LineFlowLayout lineFlowLayout1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label2;
    }
}