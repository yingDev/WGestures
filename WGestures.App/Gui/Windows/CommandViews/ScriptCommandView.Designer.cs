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
            this.check_handleModifiers = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_gestureEnded = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_modifierRecognized = new System.Windows.Forms.TextBox();
            this.txt_modifierTriggered = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_script
            // 
            this.txt_script.Location = new System.Drawing.Point(3, 92);
            this.txt_script.Margin = new System.Windows.Forms.Padding(2);
            this.txt_script.Multiline = true;
            this.txt_script.Name = "txt_script";
            this.txt_script.Size = new System.Drawing.Size(254, 52);
            this.txt_script.TabIndex = 0;
            this.txt_script.TextChanged += new System.EventHandler(this.txt_script_TextChanged);
            // 
            // txt_initScript
            // 
            this.txt_initScript.Location = new System.Drawing.Point(4, 18);
            this.txt_initScript.Margin = new System.Windows.Forms.Padding(2);
            this.txt_initScript.Multiline = true;
            this.txt_initScript.Name = "txt_initScript";
            this.txt_initScript.Size = new System.Drawing.Size(254, 52);
            this.txt_initScript.TabIndex = 0;
            this.txt_initScript.TextChanged += new System.EventHandler(this.txt_initScript_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "初始化";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "执行";
            // 
            // check_handleModifiers
            // 
            this.check_handleModifiers.AutoSize = true;
            this.check_handleModifiers.Location = new System.Drawing.Point(3, 149);
            this.check_handleModifiers.Name = "check_handleModifiers";
            this.check_handleModifiers.Size = new System.Drawing.Size(84, 16);
            this.check_handleModifiers.TabIndex = 2;
            this.check_handleModifiers.Text = "处理修饰键";
            this.check_handleModifiers.UseVisualStyleBackColor = true;
            this.check_handleModifiers.CheckedChanged += new System.EventHandler(this.check_handleModifiers_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txt_gestureEnded);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txt_modifierRecognized);
            this.groupBox1.Controls.Add(this.txt_modifierTriggered);
            this.groupBox1.Location = new System.Drawing.Point(3, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 253);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "修饰键脚本";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 168);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "手势结束";
            // 
            // txt_gestureEnded
            // 
            this.txt_gestureEnded.Location = new System.Drawing.Point(8, 185);
            this.txt_gestureEnded.Margin = new System.Windows.Forms.Padding(2);
            this.txt_gestureEnded.Multiline = true;
            this.txt_gestureEnded.Name = "txt_gestureEnded";
            this.txt_gestureEnded.Size = new System.Drawing.Size(238, 52);
            this.txt_gestureEnded.TabIndex = 6;
            this.txt_gestureEnded.TextChanged += new System.EventHandler(this.txt_gestureEnded_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 94);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "处理修饰键";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "手势识别时";
            // 
            // txt_modifierRecognized
            // 
            this.txt_modifierRecognized.Location = new System.Drawing.Point(8, 37);
            this.txt_modifierRecognized.Margin = new System.Windows.Forms.Padding(2);
            this.txt_modifierRecognized.Multiline = true;
            this.txt_modifierRecognized.Name = "txt_modifierRecognized";
            this.txt_modifierRecognized.Size = new System.Drawing.Size(238, 52);
            this.txt_modifierRecognized.TabIndex = 2;
            this.txt_modifierRecognized.TextChanged += new System.EventHandler(this.txt_modifierRecognized_TextChanged);
            // 
            // txt_modifierTriggered
            // 
            this.txt_modifierTriggered.Location = new System.Drawing.Point(8, 111);
            this.txt_modifierTriggered.Margin = new System.Windows.Forms.Padding(2);
            this.txt_modifierTriggered.Multiline = true;
            this.txt_modifierTriggered.Name = "txt_modifierTriggered";
            this.txt_modifierTriggered.Size = new System.Drawing.Size(238, 52);
            this.txt_modifierTriggered.TabIndex = 3;
            this.txt_modifierTriggered.TextChanged += new System.EventHandler(this.txt_modifierTriggered_TextChanged);
            // 
            // ScriptCommandView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.check_handleModifiers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_initScript);
            this.Controls.Add(this.txt_script);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "ScriptCommandView";
            this.Size = new System.Drawing.Size(260, 427);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_script;
        private System.Windows.Forms.TextBox txt_initScript;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox check_handleModifiers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_gestureEnded;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_modifierRecognized;
        private System.Windows.Forms.TextBox txt_modifierTriggered;
    }
}
