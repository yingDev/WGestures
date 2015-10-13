namespace WGestures.App.Gui.Windows.CommandViews
{
    partial class WebSearchCommandView
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
            this.combo_searchEngines = new System.Windows.Forms.ComboBox();
            this.panel_customSearchEngine = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_url = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.combo_browsers = new System.Windows.Forms.ComboBox();
            this.panel_customSearchEngine.SuspendLayout();
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
            this.label1.Text = "搜索引擎";
            // 
            // combo_searchEngines
            // 
            this.combo_searchEngines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_searchEngines.FormattingEnabled = true;
            this.combo_searchEngines.Location = new System.Drawing.Point(76, 2);
            this.combo_searchEngines.Name = "combo_searchEngines";
            this.combo_searchEngines.Size = new System.Drawing.Size(178, 20);
            this.combo_searchEngines.TabIndex = 1;
            this.combo_searchEngines.SelectedIndexChanged += new System.EventHandler(this.combo_searchEngines_SelectedIndexChanged);
            // 
            // panel_customSearchEngine
            // 
            this.panel_customSearchEngine.AutoSize = true;
            this.panel_customSearchEngine.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_customSearchEngine.Controls.Add(this.label3);
            this.panel_customSearchEngine.Controls.Add(this.label2);
            this.panel_customSearchEngine.Controls.Add(this.tb_url);
            this.panel_customSearchEngine.Location = new System.Drawing.Point(5, 76);
            this.panel_customSearchEngine.Margin = new System.Windows.Forms.Padding(0);
            this.panel_customSearchEngine.Name = "panel_customSearchEngine";
            this.panel_customSearchEngine.Size = new System.Drawing.Size(254, 44);
            this.panel_customSearchEngine.TabIndex = 4;
            this.panel_customSearchEngine.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 8F);
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(0, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 11);
            this.label3.TabIndex = 4;
            this.label3.Text = "*用 \"{0}\" 代替url中的查询参数的内容";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "URL格式";
            // 
            // tb_url
            // 
            this.tb_url.Location = new System.Drawing.Point(59, 3);
            this.tb_url.Name = "tb_url";
            this.tb_url.Size = new System.Drawing.Size(192, 21);
            this.tb_url.TabIndex = 2;
            this.tb_url.TextChanged += new System.EventHandler(this.tb_url_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 8F);
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(3, 56);
            this.label4.Margin = new System.Windows.Forms.Padding(2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(201, 11);
            this.label4.TabIndex = 5;
            this.label4.Text = "*如果您选中的文本是URL，则会打开URL";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 28);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 31);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "使用浏览器";
            // 
            // combo_browsers
            // 
            this.combo_browsers.DisplayMember = "Name";
            this.combo_browsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_browsers.FormattingEnabled = true;
            this.combo_browsers.Location = new System.Drawing.Point(76, 28);
            this.combo_browsers.Name = "combo_browsers";
            this.combo_browsers.Size = new System.Drawing.Size(178, 20);
            this.combo_browsers.TabIndex = 8;
            this.combo_browsers.SelectedIndexChanged += new System.EventHandler(this.combo_browsers_SelectedIndexChanged);
            // 
            // WebSearchCommandView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel_customSearchEngine);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.combo_browsers);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.combo_searchEngines);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WebSearchCommandView";
            this.Size = new System.Drawing.Size(259, 120);
            this.panel_customSearchEngine.ResumeLayout(false);
            this.panel_customSearchEngine.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combo_searchEngines;
        private System.Windows.Forms.TextBox tb_url;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel_customSearchEngine;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox combo_browsers;
    }
}
