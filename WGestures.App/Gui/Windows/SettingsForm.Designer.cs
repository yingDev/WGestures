using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using WGestures.App.Gui.Windows.Controls;
using WGestures.App.Properties;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.App.Gui.Windows
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
                Icon.Dispose();
                Icon = null;

                //循环引用会造成内存泄漏
                Controller = null;
                imglistAppIcons = null;
                dummyImgLstForLstViewHeightFix = null;


                if (_versionChecker != null)
                {
                    _versionChecker.Dispose();
                    _versionChecker = null;
                }
                //Resources.ResourceManager.ReleaseAllResources();

                //GC.Collect();

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
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_general = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.check_preferCursorWindow = new System.Windows.Forms.CheckBox();
            this.settingsFormControllerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.check_enable8DirGesture = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.combo_GestureTriggerButton = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lineLabel2 = new WGestures.App.Gui.Windows.Controls.LineLabel();
            this.check_disableOnFullscreen = new System.Windows.Forms.CheckBox();
            this.num_pathTrackerInitialStayTimeoutMillis = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            this.check_pathTrackerInitialStayTimeout = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.colorMiddle = new WGestures.App.Gui.Windows.Controls.ColorButton();
            this.colorBtn_recogonized = new WGestures.App.Gui.Windows.Controls.ColorButton();
            this.colorBtn_unrecogonized = new WGestures.App.Gui.Windows.Controls.ColorButton();
            this.numPathTrackerStayTimeoutMillis = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            this.numPathTrackerInitialValidMove = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            this.checkGestureView_fadeOut = new System.Windows.Forms.CheckBox();
            this.checkGestureViewShowCommandName = new System.Windows.Forms.CheckBox();
            this.checkPathTrackerStayTimeout = new System.Windows.Forms.CheckBox();
            this.checkGestureViewShowPath = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_Version = new System.Windows.Forms.Label();
            this.check_autoStart = new System.Windows.Forms.CheckBox();
            this.btn_checkUpdateNow = new System.Windows.Forms.Button();
            this.check_autoCheckUpdate = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkInheritGlobal = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.check_gesturingDisabled = new System.Windows.Forms.CheckBox();
            this.pictureSelectedApp = new System.Windows.Forms.PictureBox();
            this.labelAppName = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.listGestureIntents = new WGestures.App.Gui.Windows.Controls.AlwaysSelectedListView();
            this.colGestureName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGestureDirs = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.operation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dummyImgLstForLstViewHeightFix = new System.Windows.Forms.ImageList(this.components);
            this.panel_intentListOperations = new System.Windows.Forms.Panel();
            this.btn_RemoveGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.btn_modifyGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.btnAddGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.group_Command = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.combo_CommandTypes = new System.Windows.Forms.ComboBox();
            this.check_executeOnMouseWheeling = new System.Windows.Forms.CheckBox();
            this.lineLabel1 = new WGestures.App.Gui.Windows.Controls.LineLabel();
            this.panel_commandView = new System.Windows.Forms.Panel();
            this.btnEditApp = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.btnAppRemove = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.btnAddApp = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.listApps = new WGestures.App.Gui.Windows.Controls.AlwaysSelectedListView();
            this.colListAppDummy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imglistAppIcons = new System.Windows.Forms.ImageList(this.components);
            this.tab_hotCorners = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.check_enableHotCorners = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.picture_alipayCode = new System.Windows.Forms.PictureBox();
            this.tb_updateLog = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.picture_logo = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.lb_info = new System.Windows.Forms.Label();
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.pic_menuBtn = new System.Windows.Forms.PictureBox();
            this.ctx_gesturesMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItem_import = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_export = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl.SuspendLayout();
            this.tabPage_general.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsFormControllerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_pathTrackerInitialStayTimeoutMillis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPathTrackerStayTimeoutMillis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPathTrackerInitialValidMove)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSelectedApp)).BeginInit();
            this.panel_intentListOperations.SuspendLayout();
            this.group_Command.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tab_hotCorners.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_alipayCode)).BeginInit();
            this.flowLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_logo)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_menuBtn)).BeginInit();
            this.ctx_gesturesMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_general);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tab_hotCorners);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.ItemSize = new System.Drawing.Size(250, 28);
            this.tabControl.Location = new System.Drawing.Point(9, 9);
            this.tabControl.Margin = new System.Windows.Forms.Padding(8);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(20, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(559, 471);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPage_general
            // 
            this.tabPage_general.BackColor = System.Drawing.Color.White;
            this.tabPage_general.Controls.Add(this.groupBox2);
            this.tabPage_general.Controls.Add(this.groupBox1);
            this.tabPage_general.Location = new System.Drawing.Point(4, 32);
            this.tabPage_general.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage_general.Name = "tabPage_general";
            this.tabPage_general.Size = new System.Drawing.Size(551, 435);
            this.tabPage_general.TabIndex = 0;
            this.tabPage_general.Tag = "general";
            this.tabPage_general.Text = "选 项";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.check_preferCursorWindow);
            this.groupBox2.Controls.Add(this.check_enable8DirGesture);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.combo_GestureTriggerButton);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lineLabel2);
            this.groupBox2.Controls.Add(this.check_disableOnFullscreen);
            this.groupBox2.Controls.Add(this.num_pathTrackerInitialStayTimeoutMillis);
            this.groupBox2.Controls.Add(this.check_pathTrackerInitialStayTimeout);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.colorMiddle);
            this.groupBox2.Controls.Add(this.colorBtn_recogonized);
            this.groupBox2.Controls.Add(this.colorBtn_unrecogonized);
            this.groupBox2.Controls.Add(this.numPathTrackerStayTimeoutMillis);
            this.groupBox2.Controls.Add(this.numPathTrackerInitialValidMove);
            this.groupBox2.Controls.Add(this.checkGestureView_fadeOut);
            this.groupBox2.Controls.Add(this.checkGestureViewShowCommandName);
            this.groupBox2.Controls.Add(this.checkPathTrackerStayTimeout);
            this.groupBox2.Controls.Add(this.checkGestureViewShowPath);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(15, 132);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(525, 291);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "手势";
            // 
            // check_preferCursorWindow
            // 
            this.check_preferCursorWindow.AutoSize = true;
            this.check_preferCursorWindow.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerPreferCursorWindow", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_preferCursorWindow.Location = new System.Drawing.Point(299, 119);
            this.check_preferCursorWindow.Name = "check_preferCursorWindow";
            this.check_preferCursorWindow.Size = new System.Drawing.Size(171, 21);
            this.check_preferCursorWindow.TabIndex = 18;
            this.check_preferCursorWindow.Text = "总是作用于指针下方的窗口";
            this.tip.SetToolTip(this.check_preferCursorWindow, "使手势总是作用于鼠标指针下方窗口，而不是当前活动程序");
            this.check_preferCursorWindow.UseVisualStyleBackColor = true;
            // 
            // settingsFormControllerBindingSource
            // 
            this.settingsFormControllerBindingSource.DataSource = typeof(WGestures.App.Gui.Windows.SettingsFormController);
            // 
            // check_enable8DirGesture
            // 
            this.check_enable8DirGesture.AutoSize = true;
            this.check_enable8DirGesture.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureParserEnable8DirGesture", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_enable8DirGesture.Location = new System.Drawing.Point(299, 90);
            this.check_enable8DirGesture.Margin = new System.Windows.Forms.Padding(2);
            this.check_enable8DirGesture.Name = "check_enable8DirGesture";
            this.check_enable8DirGesture.Size = new System.Drawing.Size(123, 21);
            this.check_enable8DirGesture.TabIndex = 17;
            this.check_enable8DirGesture.Text = "允许使用斜线手势";
            this.tip.SetToolTip(this.check_enable8DirGesture, "是否允许使用”↖↙↗↘“手势");
            this.check_enable8DirGesture.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(296, 60);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "手势触发键";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tip.SetToolTip(this.label3, "允许哪个鼠标按钮触发手势？");
            // 
            // combo_GestureTriggerButton
            // 
            this.combo_GestureTriggerButton.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.settingsFormControllerBindingSource, "PathTrackerTriggerButton", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.combo_GestureTriggerButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_GestureTriggerButton.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.combo_GestureTriggerButton.FormattingEnabled = true;
            this.combo_GestureTriggerButton.ItemHeight = 17;
            this.combo_GestureTriggerButton.Location = new System.Drawing.Point(372, 57);
            this.combo_GestureTriggerButton.Margin = new System.Windows.Forms.Padding(1);
            this.combo_GestureTriggerButton.Name = "combo_GestureTriggerButton";
            this.combo_GestureTriggerButton.Size = new System.Drawing.Size(116, 25);
            this.combo_GestureTriggerButton.TabIndex = 15;
            this.tip.SetToolTip(this.combo_GestureTriggerButton, "允许哪个鼠标按钮触发手势？");
            this.combo_GestureTriggerButton.SelectedIndexChanged += new System.EventHandler(this.combo_GestureTriggerButton_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(296, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "使用习惯:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lineLabel2
            // 
            this.lineLabel2.ForeColor = System.Drawing.Color.Gainsboro;
            this.lineLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lineLabel2.IsVertical = true;
            this.lineLabel2.Location = new System.Drawing.Point(268, 30);
            this.lineLabel2.Name = "lineLabel2";
            this.lineLabel2.Size = new System.Drawing.Size(18, 112);
            this.lineLabel2.TabIndex = 13;
            // 
            // check_disableOnFullscreen
            // 
            this.check_disableOnFullscreen.AutoSize = true;
            this.check_disableOnFullscreen.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerDisableInFullScreen", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_disableOnFullscreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_disableOnFullscreen.Location = new System.Drawing.Point(91, 119);
            this.check_disableOnFullscreen.Margin = new System.Windows.Forms.Padding(1);
            this.check_disableOnFullscreen.Name = "check_disableOnFullscreen";
            this.check_disableOnFullscreen.Size = new System.Drawing.Size(135, 21);
            this.check_disableOnFullscreen.TabIndex = 12;
            this.check_disableOnFullscreen.Text = "全屏时自动禁用手势";
            this.check_disableOnFullscreen.UseVisualStyleBackColor = true;
            // 
            // num_pathTrackerInitialStayTimeoutMillis
            // 
            this.num_pathTrackerInitialStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerInitalStayTimeoutMillis", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.num_pathTrackerInitialStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.settingsFormControllerBindingSource, "PathTrackerInitialStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.num_pathTrackerInitialStayTimeoutMillis.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_pathTrackerInitialStayTimeoutMillis.Location = new System.Drawing.Point(203, 61);
            this.num_pathTrackerInitialStayTimeoutMillis.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.num_pathTrackerInitialStayTimeoutMillis.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.num_pathTrackerInitialStayTimeoutMillis.Name = "num_pathTrackerInitialStayTimeoutMillis";
            this.num_pathTrackerInitialStayTimeoutMillis.Size = new System.Drawing.Size(51, 23);
            this.num_pathTrackerInitialStayTimeoutMillis.TabIndex = 11;
            this.tip.SetToolTip(this.num_pathTrackerInitialStayTimeoutMillis, "若按下右键后超过此时间未移动，则执行正常右键拖拽操作");
            this.num_pathTrackerInitialStayTimeoutMillis.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // check_pathTrackerInitialStayTimeout
            // 
            this.check_pathTrackerInitialStayTimeout.AutoSize = true;
            this.check_pathTrackerInitialStayTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerInitialStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_pathTrackerInitialStayTimeout.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_pathTrackerInitialStayTimeout.Location = new System.Drawing.Point(91, 59);
            this.check_pathTrackerInitialStayTimeout.Margin = new System.Windows.Forms.Padding(1);
            this.check_pathTrackerInitialStayTimeout.Name = "check_pathTrackerInitialStayTimeout";
            this.check_pathTrackerInitialStayTimeout.Size = new System.Drawing.Size(111, 21);
            this.check_pathTrackerInitialStayTimeout.TabIndex = 10;
            this.check_pathTrackerInitialStayTimeout.Text = "起始超时 (毫秒)";
            this.tip.SetToolTip(this.check_pathTrackerInitialStayTimeout, "若按下右键后超过此时间未移动，则执行正常右键拖拽操作");
            this.check_pathTrackerInitialStayTimeout.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(87, 190);
            this.label9.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "轨迹风格:";
            // 
            // colorMiddle
            // 
            this.colorMiddle.BackColor = System.Drawing.Color.White;
            this.colorMiddle.Color = System.Drawing.Color.YellowGreen;
            this.colorMiddle.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewMiddleBtnMainColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.colorMiddle.Font = new System.Drawing.Font("SimSun", 8.25F);
            this.colorMiddle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colorMiddle.Location = new System.Drawing.Point(162, 214);
            this.colorMiddle.Margin = new System.Windows.Forms.Padding(1);
            this.colorMiddle.Name = "colorMiddle";
            this.colorMiddle.Size = new System.Drawing.Size(60, 42);
            this.colorMiddle.TabIndex = 8;
            this.colorMiddle.Text = "已识别\r\n(中键)";
            this.colorMiddle.UseVisualStyleBackColor = false;
            // 
            // colorBtn_recogonized
            // 
            this.colorBtn_recogonized.BackColor = System.Drawing.Color.White;
            this.colorBtn_recogonized.Color = System.Drawing.Color.MediumTurquoise;
            this.colorBtn_recogonized.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewMainPathColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.colorBtn_recogonized.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colorBtn_recogonized.Location = new System.Drawing.Point(89, 214);
            this.colorBtn_recogonized.Margin = new System.Windows.Forms.Padding(1);
            this.colorBtn_recogonized.Name = "colorBtn_recogonized";
            this.colorBtn_recogonized.Size = new System.Drawing.Size(60, 42);
            this.colorBtn_recogonized.TabIndex = 8;
            this.colorBtn_recogonized.Text = "已识别";
            this.tip.SetToolTip(this.colorBtn_recogonized, "手势被识别时，轨迹的颜色");
            this.colorBtn_recogonized.UseVisualStyleBackColor = false;
            // 
            // colorBtn_unrecogonized
            // 
            this.colorBtn_unrecogonized.BackColor = System.Drawing.Color.White;
            this.colorBtn_unrecogonized.Color = System.Drawing.Color.DeepPink;
            this.colorBtn_unrecogonized.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewAlternativePathColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.colorBtn_unrecogonized.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colorBtn_unrecogonized.Location = new System.Drawing.Point(232, 214);
            this.colorBtn_unrecogonized.Margin = new System.Windows.Forms.Padding(1);
            this.colorBtn_unrecogonized.Name = "colorBtn_unrecogonized";
            this.colorBtn_unrecogonized.Size = new System.Drawing.Size(60, 42);
            this.colorBtn_unrecogonized.TabIndex = 8;
            this.colorBtn_unrecogonized.Text = "未识别";
            this.tip.SetToolTip(this.colorBtn_unrecogonized, "手势未被识别时，轨迹的颜色");
            this.colorBtn_unrecogonized.UseVisualStyleBackColor = false;
            // 
            // numPathTrackerStayTimeoutMillis
            // 
            this.numPathTrackerStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerStayTimeoutMillis", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numPathTrackerStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.settingsFormControllerBindingSource, "PathTrackerStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numPathTrackerStayTimeoutMillis.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPathTrackerStayTimeoutMillis.Location = new System.Drawing.Point(203, 92);
            this.numPathTrackerStayTimeoutMillis.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numPathTrackerStayTimeoutMillis.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numPathTrackerStayTimeoutMillis.Name = "numPathTrackerStayTimeoutMillis";
            this.numPathTrackerStayTimeoutMillis.Size = new System.Drawing.Size(51, 23);
            this.numPathTrackerStayTimeoutMillis.TabIndex = 7;
            this.tip.SetToolTip(this.numPathTrackerStayTimeoutMillis, "若鼠标停止移动超过此时间，已画出的手势将被取消");
            this.numPathTrackerStayTimeoutMillis.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // numPathTrackerInitialValidMove
            // 
            this.numPathTrackerInitialValidMove.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerInitialValidMove", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numPathTrackerInitialValidMove.Location = new System.Drawing.Point(203, 30);
            this.numPathTrackerInitialValidMove.Name = "numPathTrackerInitialValidMove";
            this.numPathTrackerInitialValidMove.Size = new System.Drawing.Size(51, 23);
            this.numPathTrackerInitialValidMove.TabIndex = 7;
            this.tip.SetToolTip(this.numPathTrackerInitialValidMove, "只有移动超过此距离，才开始识别手势");
            this.numPathTrackerInitialValidMove.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // checkGestureView_fadeOut
            // 
            this.checkGestureView_fadeOut.AutoSize = true;
            this.checkGestureView_fadeOut.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewFadeOut", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkGestureView_fadeOut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkGestureView_fadeOut.Location = new System.Drawing.Point(271, 161);
            this.checkGestureView_fadeOut.Margin = new System.Windows.Forms.Padding(1);
            this.checkGestureView_fadeOut.Name = "checkGestureView_fadeOut";
            this.checkGestureView_fadeOut.Size = new System.Drawing.Size(87, 21);
            this.checkGestureView_fadeOut.TabIndex = 1;
            this.checkGestureView_fadeOut.Text = "执行后淡出";
            this.tip.SetToolTip(this.checkGestureView_fadeOut, "手势执行后图形逐渐消失(而非突然消失)");
            this.checkGestureView_fadeOut.UseVisualStyleBackColor = true;
            // 
            // checkGestureViewShowCommandName
            // 
            this.checkGestureViewShowCommandName.AutoSize = true;
            this.checkGestureViewShowCommandName.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewShowCommandName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkGestureViewShowCommandName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkGestureViewShowCommandName.Location = new System.Drawing.Point(169, 161);
            this.checkGestureViewShowCommandName.Margin = new System.Windows.Forms.Padding(1);
            this.checkGestureViewShowCommandName.Name = "checkGestureViewShowCommandName";
            this.checkGestureViewShowCommandName.Size = new System.Drawing.Size(75, 21);
            this.checkGestureViewShowCommandName.TabIndex = 1;
            this.checkGestureViewShowCommandName.Text = "手势名称";
            this.checkGestureViewShowCommandName.UseVisualStyleBackColor = true;
            // 
            // checkPathTrackerStayTimeout
            // 
            this.checkPathTrackerStayTimeout.AutoSize = true;
            this.checkPathTrackerStayTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkPathTrackerStayTimeout.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkPathTrackerStayTimeout.Location = new System.Drawing.Point(89, 90);
            this.checkPathTrackerStayTimeout.Margin = new System.Windows.Forms.Padding(1);
            this.checkPathTrackerStayTimeout.Name = "checkPathTrackerStayTimeout";
            this.checkPathTrackerStayTimeout.Size = new System.Drawing.Size(111, 21);
            this.checkPathTrackerStayTimeout.TabIndex = 0;
            this.checkPathTrackerStayTimeout.Text = "停留超时 (毫秒)";
            this.tip.SetToolTip(this.checkPathTrackerStayTimeout, "若鼠标停止移动超过此时间，已画出的手势将被取消");
            this.checkPathTrackerStayTimeout.UseVisualStyleBackColor = true;
            // 
            // checkGestureViewShowPath
            // 
            this.checkGestureViewShowPath.AutoSize = true;
            this.checkGestureViewShowPath.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewShowPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkGestureViewShowPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkGestureViewShowPath.Location = new System.Drawing.Point(91, 161);
            this.checkGestureViewShowPath.Margin = new System.Windows.Forms.Padding(1);
            this.checkGestureViewShowPath.Name = "checkGestureViewShowPath";
            this.checkGestureViewShowPath.Size = new System.Drawing.Size(51, 21);
            this.checkGestureViewShowPath.TabIndex = 0;
            this.checkGestureViewShowPath.Text = "轨迹";
            this.checkGestureViewShowPath.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(86, 32);
            this.label6.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "起始移动距离(像素)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tip.SetToolTip(this.label6, "只有移动超过此距离，才开始识别手势");
            // 
            // label5
            // 
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(27, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "有效性:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(26, 162);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "显   示:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel3);
            this.groupBox1.Controls.Add(this.check_autoStart);
            this.groupBox1.Controls.Add(this.btn_checkUpdateNow);
            this.groupBox1.Controls.Add(this.check_autoCheckUpdate);
            this.groupBox1.Location = new System.Drawing.Point(15, 20);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox1.Size = new System.Drawing.Size(525, 95);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通用";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Controls.Add(this.lb_Version);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(232, 53);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(277, 25);
            this.flowLayoutPanel3.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkGray;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "当前版本:";
            // 
            // lb_Version
            // 
            this.lb_Version.AutoSize = true;
            this.lb_Version.ForeColor = System.Drawing.Color.DarkGray;
            this.lb_Version.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_Version.Location = new System.Drawing.Point(68, 6);
            this.lb_Version.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.lb_Version.Name = "lb_Version";
            this.lb_Version.Size = new System.Drawing.Size(50, 17);
            this.lb_Version.TabIndex = 3;
            this.lb_Version.Text = "version";
            // 
            // check_autoStart
            // 
            this.check_autoStart.AutoSize = true;
            this.check_autoStart.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "AutoStart", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_autoStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_autoStart.Location = new System.Drawing.Point(29, 29);
            this.check_autoStart.Margin = new System.Windows.Forms.Padding(1);
            this.check_autoStart.Name = "check_autoStart";
            this.check_autoStart.Size = new System.Drawing.Size(99, 21);
            this.check_autoStart.TabIndex = 0;
            this.check_autoStart.Text = "开机自动运行";
            this.check_autoStart.UseVisualStyleBackColor = true;
            // 
            // btn_checkUpdateNow
            // 
            this.btn_checkUpdateNow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_checkUpdateNow.Location = new System.Drawing.Point(130, 53);
            this.btn_checkUpdateNow.Margin = new System.Windows.Forms.Padding(1);
            this.btn_checkUpdateNow.Name = "btn_checkUpdateNow";
            this.btn_checkUpdateNow.Size = new System.Drawing.Size(75, 25);
            this.btn_checkUpdateNow.TabIndex = 2;
            this.btn_checkUpdateNow.Text = "立即检查";
            this.btn_checkUpdateNow.UseVisualStyleBackColor = true;
            this.btn_checkUpdateNow.Click += new System.EventHandler(this.btn_checkUpdateNow_Click);
            // 
            // check_autoCheckUpdate
            // 
            this.check_autoCheckUpdate.AutoSize = true;
            this.check_autoCheckUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "AutoCheckForUpdate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_autoCheckUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_autoCheckUpdate.Location = new System.Drawing.Point(29, 57);
            this.check_autoCheckUpdate.Margin = new System.Windows.Forms.Padding(1);
            this.check_autoCheckUpdate.Name = "check_autoCheckUpdate";
            this.check_autoCheckUpdate.Size = new System.Drawing.Size(99, 21);
            this.check_autoCheckUpdate.TabIndex = 1;
            this.check_autoCheckUpdate.Text = "自动检查更新";
            this.check_autoCheckUpdate.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.flowLayoutPanel2);
            this.tabPage2.Controls.Add(this.btnEditApp);
            this.tabPage2.Controls.Add(this.btnAppRemove);
            this.tabPage2.Controls.Add(this.btnAddApp);
            this.tabPage2.Controls.Add(this.listApps);
            this.tabPage2.Location = new System.Drawing.Point(4, 32);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(551, 435);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Tag = "gestures";
            this.tabPage2.Text = "手 势";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.panel1);
            this.flowLayoutPanel2.Controls.Add(this.listGestureIntents);
            this.flowLayoutPanel2.Controls.Add(this.panel_intentListOperations);
            this.flowLayoutPanel2.Controls.Add(this.group_Command);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(189, 17);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(352, 400);
            this.flowLayoutPanel2.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.checkInheritGlobal);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(352, 25);
            this.panel1.TabIndex = 12;
            // 
            // checkInheritGlobal
            // 
            this.checkInheritGlobal.AutoSize = true;
            this.checkInheritGlobal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkInheritGlobal.Location = new System.Drawing.Point(253, 2);
            this.checkInheritGlobal.Margin = new System.Windows.Forms.Padding(0);
            this.checkInheritGlobal.Name = "checkInheritGlobal";
            this.checkInheritGlobal.Size = new System.Drawing.Size(99, 21);
            this.checkInheritGlobal.TabIndex = 3;
            this.checkInheritGlobal.Text = "继承全局手势";
            this.checkInheritGlobal.UseVisualStyleBackColor = true;
            this.checkInheritGlobal.CheckedChanged += new System.EventHandler(this.checkInheritGlobal_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.check_gesturingDisabled);
            this.flowLayoutPanel1.Controls.Add(this.pictureSelectedApp);
            this.flowLayoutPanel1.Controls.Add(this.labelAppName);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(212, 25);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // check_gesturingDisabled
            // 
            this.check_gesturingDisabled.AutoSize = true;
            this.check_gesturingDisabled.ForeColor = System.Drawing.Color.Black;
            this.check_gesturingDisabled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_gesturingDisabled.Location = new System.Drawing.Point(9, 2);
            this.check_gesturingDisabled.Margin = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.check_gesturingDisabled.Name = "check_gesturingDisabled";
            this.check_gesturingDisabled.Size = new System.Drawing.Size(51, 21);
            this.check_gesturingDisabled.TabIndex = 5;
            this.check_gesturingDisabled.Text = "禁止";
            this.tip.SetToolTip(this.check_gesturingDisabled, "在该程序上禁用手势（等同于双击应用程序条目）");
            this.check_gesturingDisabled.UseVisualStyleBackColor = true;
            this.check_gesturingDisabled.CheckedChanged += new System.EventHandler(this.check_gesturingEnabled_CheckedChanged);
            // 
            // pictureSelectedApp
            // 
            this.pictureSelectedApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureSelectedApp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureSelectedApp.Location = new System.Drawing.Point(60, 3);
            this.pictureSelectedApp.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.pictureSelectedApp.Name = "pictureSelectedApp";
            this.pictureSelectedApp.Size = new System.Drawing.Size(16, 16);
            this.pictureSelectedApp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSelectedApp.TabIndex = 2;
            this.pictureSelectedApp.TabStop = false;
            // 
            // labelAppName
            // 
            this.labelAppName.AutoEllipsis = true;
            this.labelAppName.AutoSize = true;
            this.labelAppName.BackColor = System.Drawing.Color.White;
            this.labelAppName.ForeColor = System.Drawing.Color.Black;
            this.labelAppName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelAppName.Location = new System.Drawing.Point(76, 2);
            this.labelAppName.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.labelAppName.MaximumSize = new System.Drawing.Size(90, 17);
            this.labelAppName.Name = "labelAppName";
            this.labelAppName.Size = new System.Drawing.Size(80, 17);
            this.labelAppName.TabIndex = 1;
            this.labelAppName.Text = "文件管理器阿不都第三方斯蒂芬李双江的方式";
            this.labelAppName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(156, 2);
            this.label7.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 17);
            this.label7.TabIndex = 3;
            this.label7.Text = "使用手势";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listGestureIntents
            // 
            this.listGestureIntents.AllowDrop = true;
            this.listGestureIntents.AllowItemDrag = true;
            this.listGestureIntents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listGestureIntents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGestureName,
            this.colGestureDirs,
            this.operation});
            this.listGestureIntents.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F);
            this.listGestureIntents.FullRowSelect = true;
            this.listGestureIntents.GridLines = true;
            this.listGestureIntents.HideSelection = false;
            this.listGestureIntents.InsertionLineColor = System.Drawing.Color.DeepSkyBlue;
            this.listGestureIntents.LabelEdit = true;
            this.listGestureIntents.Location = new System.Drawing.Point(9, 28);
            this.listGestureIntents.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.listGestureIntents.MultiSelect = false;
            this.listGestureIntents.Name = "listGestureIntents";
            this.listGestureIntents.Size = new System.Drawing.Size(339, 165);
            this.listGestureIntents.SmallImageList = this.dummyImgLstForLstViewHeightFix;
            this.listGestureIntents.TabIndex = 1;
            this.listGestureIntents.TileSize = new System.Drawing.Size(255, 84);
            this.listGestureIntents.UseCompatibleStateImageBehavior = false;
            this.listGestureIntents.View = System.Windows.Forms.View.Details;
            this.listGestureIntents.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listGestureIntents_AfterLabelEdit);
            this.listGestureIntents.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listGestureIntents_ItemSelectionChanged);
            this.listGestureIntents.DoubleClick += new System.EventHandler(this.listGestureIntents_DoubleClick);
            this.listGestureIntents.MouseEnter += new System.EventHandler(this.listGestureIntents_MouseEnter);
            this.listGestureIntents.MouseHover += new System.EventHandler(this.listGestureIntents_MouseHover);
            // 
            // colGestureName
            // 
            this.colGestureName.Text = "名称";
            this.colGestureName.Width = 10;
            // 
            // colGestureDirs
            // 
            this.colGestureDirs.Text = "助记符";
            this.colGestureDirs.Width = 10;
            // 
            // operation
            // 
            this.operation.Text = "操作";
            // 
            // dummyImgLstForLstViewHeightFix
            // 
            this.dummyImgLstForLstViewHeightFix.ColorDepth = System.Windows.Forms.ColorDepth.Depth4Bit;
            this.dummyImgLstForLstViewHeightFix.ImageSize = new System.Drawing.Size(1, 24);
            this.dummyImgLstForLstViewHeightFix.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel_intentListOperations
            // 
            this.panel_intentListOperations.Controls.Add(this.btn_RemoveGesture);
            this.panel_intentListOperations.Controls.Add(this.btn_modifyGesture);
            this.panel_intentListOperations.Controls.Add(this.btnAddGesture);
            this.panel_intentListOperations.Location = new System.Drawing.Point(9, 196);
            this.panel_intentListOperations.Margin = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.panel_intentListOperations.Name = "panel_intentListOperations";
            this.panel_intentListOperations.Size = new System.Drawing.Size(340, 23);
            this.panel_intentListOperations.TabIndex = 9;
            // 
            // btn_RemoveGesture
            // 
            this.btn_RemoveGesture.Enabled = false;
            this.btn_RemoveGesture.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            this.btn_RemoveGesture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_RemoveGesture.Image = global::WGestures.App.Properties.Resources.remove;
            this.btn_RemoveGesture.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_RemoveGesture.Location = new System.Drawing.Point(28, 0);
            this.btn_RemoveGesture.Margin = new System.Windows.Forms.Padding(0);
            this.btn_RemoveGesture.Name = "btn_RemoveGesture";
            this.btn_RemoveGesture.Size = new System.Drawing.Size(29, 20);
            this.btn_RemoveGesture.TabIndex = 8;
            this.tip.SetToolTip(this.btn_RemoveGesture, "删除选中的项目");
            this.btn_RemoveGesture.UseVisualStyleBackColor = true;
            this.btn_RemoveGesture.Click += new System.EventHandler(this.btnRemoveGesture_Click);
            // 
            // btn_modifyGesture
            // 
            this.btn_modifyGesture.Enabled = false;
            this.btn_modifyGesture.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            this.btn_modifyGesture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_modifyGesture.Image = global::WGestures.App.Properties.Resources.Edit;
            this.btn_modifyGesture.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_modifyGesture.Location = new System.Drawing.Point(309, 0);
            this.btn_modifyGesture.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.btn_modifyGesture.Name = "btn_modifyGesture";
            this.btn_modifyGesture.Size = new System.Drawing.Size(29, 20);
            this.btn_modifyGesture.TabIndex = 8;
            this.btn_modifyGesture.UseVisualStyleBackColor = true;
            this.btn_modifyGesture.Click += new System.EventHandler(this.btn_modifyGesture_Click);
            // 
            // btnAddGesture
            // 
            this.btnAddGesture.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            this.btnAddGesture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAddGesture.Image = global::WGestures.App.Properties.Resources.add;
            this.btnAddGesture.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddGesture.Location = new System.Drawing.Point(0, 0);
            this.btnAddGesture.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddGesture.Name = "btnAddGesture";
            this.btnAddGesture.Size = new System.Drawing.Size(29, 20);
            this.btnAddGesture.TabIndex = 8;
            this.tip.SetToolTip(this.btnAddGesture, "添加手势");
            this.btnAddGesture.UseVisualStyleBackColor = true;
            this.btnAddGesture.Click += new System.EventHandler(this.btnAddGesture_Click);
            // 
            // group_Command
            // 
            this.group_Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.group_Command.Controls.Add(this.flowLayoutPanel6);
            this.group_Command.Enabled = false;
            this.group_Command.Location = new System.Drawing.Point(9, 228);
            this.group_Command.Margin = new System.Windows.Forms.Padding(9, 9, 1, 1);
            this.group_Command.Name = "group_Command";
            this.group_Command.Padding = new System.Windows.Forms.Padding(1);
            this.group_Command.Size = new System.Drawing.Size(340, 171);
            this.group_Command.TabIndex = 10;
            this.group_Command.TabStop = false;
            this.group_Command.Text = "手势参数";
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.Controls.Add(this.panel3);
            this.flowLayoutPanel6.Controls.Add(this.check_executeOnMouseWheeling);
            this.flowLayoutPanel6.Controls.Add(this.lineLabel1);
            this.flowLayoutPanel6.Controls.Add(this.panel_commandView);
            this.flowLayoutPanel6.Location = new System.Drawing.Point(4, 18);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(332, 149);
            this.flowLayoutPanel6.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.combo_CommandTypes);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(332, 35);
            this.panel3.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(3, 10);
            this.label8.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "执行操作";
            // 
            // combo_CommandTypes
            // 
            this.combo_CommandTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_CommandTypes.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.combo_CommandTypes.FormattingEnabled = true;
            this.combo_CommandTypes.ItemHeight = 17;
            this.combo_CommandTypes.Location = new System.Drawing.Point(66, 4);
            this.combo_CommandTypes.Margin = new System.Windows.Forms.Padding(1);
            this.combo_CommandTypes.Name = "combo_CommandTypes";
            this.combo_CommandTypes.Size = new System.Drawing.Size(260, 25);
            this.combo_CommandTypes.TabIndex = 0;
            this.tip.SetToolTip(this.combo_CommandTypes, "手势触发后要执行的操作");
            this.combo_CommandTypes.SelectedIndexChanged += new System.EventHandler(this.combo_CommandTypes_SelectedIndexChanged);
            // 
            // check_executeOnMouseWheeling
            // 
            this.check_executeOnMouseWheeling.AutoSize = true;
            this.check_executeOnMouseWheeling.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_executeOnMouseWheeling.Location = new System.Drawing.Point(3, 38);
            this.check_executeOnMouseWheeling.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.check_executeOnMouseWheeling.Name = "check_executeOnMouseWheeling";
            this.check_executeOnMouseWheeling.Size = new System.Drawing.Size(147, 21);
            this.check_executeOnMouseWheeling.TabIndex = 3;
            this.check_executeOnMouseWheeling.Text = "修饰键触发时立即执行";
            this.check_executeOnMouseWheeling.UseVisualStyleBackColor = true;
            this.check_executeOnMouseWheeling.Visible = false;
            this.check_executeOnMouseWheeling.CheckedChanged += new System.EventHandler(this.check_executeOnMouseWheeling_CheckedChanged);
            // 
            // lineLabel1
            // 
            this.lineLabel1.ForeColor = System.Drawing.Color.Gainsboro;
            this.lineLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lineLabel1.IsVertical = false;
            this.lineLabel1.Location = new System.Drawing.Point(3, 59);
            this.lineLabel1.Name = "lineLabel1";
            this.lineLabel1.Size = new System.Drawing.Size(329, 6);
            this.lineLabel1.TabIndex = 3;
            // 
            // panel_commandView
            // 
            this.panel_commandView.AutoScroll = true;
            this.panel_commandView.BackColor = System.Drawing.Color.Transparent;
            this.panel_commandView.Location = new System.Drawing.Point(0, 65);
            this.panel_commandView.Margin = new System.Windows.Forms.Padding(0);
            this.panel_commandView.Name = "panel_commandView";
            this.panel_commandView.Size = new System.Drawing.Size(332, 86);
            this.panel_commandView.TabIndex = 2;
            this.tip.SetToolTip(this.panel_commandView, "操作的额外参数");
            // 
            // btnEditApp
            // 
            this.btnEditApp.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            this.btnEditApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEditApp.Image = global::WGestures.App.Properties.Resources.Edit;
            this.btnEditApp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEditApp.Location = new System.Drawing.Point(152, 393);
            this.btnEditApp.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.btnEditApp.Name = "btnEditApp";
            this.btnEditApp.Size = new System.Drawing.Size(29, 20);
            this.btnEditApp.TabIndex = 8;
            this.tip.SetToolTip(this.btnEditApp, "修改选中项目的名称或路径");
            this.btnEditApp.UseVisualStyleBackColor = true;
            this.btnEditApp.Click += new System.EventHandler(this.btnEditApp_Click);
            // 
            // btnAppRemove
            // 
            this.btnAppRemove.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            this.btnAppRemove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAppRemove.Image = global::WGestures.App.Properties.Resources.remove;
            this.btnAppRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAppRemove.Location = new System.Drawing.Point(41, 393);
            this.btnAppRemove.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.btnAppRemove.Name = "btnAppRemove";
            this.btnAppRemove.Size = new System.Drawing.Size(29, 20);
            this.btnAppRemove.TabIndex = 8;
            this.tip.SetToolTip(this.btnAppRemove, "删除选中的项目");
            this.btnAppRemove.UseVisualStyleBackColor = true;
            this.btnAppRemove.Click += new System.EventHandler(this.btnAppRemove_Click);
            // 
            // btnAddApp
            // 
            this.btnAddApp.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            this.btnAddApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAddApp.Image = global::WGestures.App.Properties.Resources.add;
            this.btnAddApp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddApp.Location = new System.Drawing.Point(13, 393);
            this.btnAddApp.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.btnAddApp.Name = "btnAddApp";
            this.btnAddApp.Size = new System.Drawing.Size(29, 20);
            this.btnAddApp.TabIndex = 8;
            this.tip.SetToolTip(this.btnAddApp, "添加应用程序");
            this.btnAddApp.UseVisualStyleBackColor = true;
            this.btnAddApp.Click += new System.EventHandler(this.btnAddApp_Click);
            // 
            // listApps
            // 
            this.listApps.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listApps.AllowDrop = true;
            this.listApps.AllowItemDrag = true;
            this.listApps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listApps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colListAppDummy});
            this.listApps.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.listApps.FullRowSelect = true;
            this.listApps.GridLines = true;
            this.listApps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listApps.HideSelection = false;
            this.listApps.InsertionLineColor = System.Drawing.Color.DeepSkyBlue;
            this.listApps.LabelWrap = false;
            this.listApps.Location = new System.Drawing.Point(13, 20);
            this.listApps.Margin = new System.Windows.Forms.Padding(1);
            this.listApps.MultiSelect = false;
            this.listApps.Name = "listApps";
            this.listApps.Size = new System.Drawing.Size(168, 370);
            this.listApps.SmallImageList = this.imglistAppIcons;
            this.listApps.TabIndex = 0;
            this.listApps.TileSize = new System.Drawing.Size(160, 42);
            this.listApps.UseCompatibleStateImageBehavior = false;
            this.listApps.View = System.Windows.Forms.View.Details;
            this.listApps.ItemDragDrop += new System.EventHandler<WGestures.App.Gui.Windows.Controls.ListViewItemDragEventArgs>(this.listApps_ItemDragDrop);
            this.listApps.ItemDragging += new System.EventHandler<WGestures.App.Gui.Windows.Controls.CancelListViewItemDragEventArgs>(this.listApps_ItemDragging);
            this.listApps.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listApps_ItemSelectionChanged);
            this.listApps.DragOver += new System.Windows.Forms.DragEventHandler(this.listApps_DragOver);
            this.listApps.DoubleClick += new System.EventHandler(this.listApps_DoubleClick);
            // 
            // colListAppDummy
            // 
            this.colListAppDummy.Width = 64;
            // 
            // imglistAppIcons
            // 
            this.imglistAppIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imglistAppIcons.ImageSize = new System.Drawing.Size(32, 32);
            this.imglistAppIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tab_hotCorners
            // 
            this.tab_hotCorners.Controls.Add(this.label11);
            this.tab_hotCorners.Controls.Add(this.comboBox4);
            this.tab_hotCorners.Controls.Add(this.comboBox3);
            this.tab_hotCorners.Controls.Add(this.comboBox2);
            this.tab_hotCorners.Controls.Add(this.comboBox1);
            this.tab_hotCorners.Controls.Add(this.panel2);
            this.tab_hotCorners.Controls.Add(this.check_enableHotCorners);
            this.tab_hotCorners.Location = new System.Drawing.Point(4, 32);
            this.tab_hotCorners.Margin = new System.Windows.Forms.Padding(2);
            this.tab_hotCorners.Name = "tab_hotCorners";
            this.tab_hotCorners.Size = new System.Drawing.Size(551, 435);
            this.tab_hotCorners.TabIndex = 3;
            this.tab_hotCorners.Text = "触发角";
            this.tab_hotCorners.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(163, 61);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(200, 17);
            this.label11.TabIndex = 6;
            this.label11.Text = "(即将支持自定义触发角， 敬请期待)";
            // 
            // comboBox4
            // 
            this.comboBox4.Enabled = false;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(64, 245);
            this.comboBox4.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(98, 25);
            this.comboBox4.TabIndex = 5;
            this.comboBox4.Text = "开始";
            // 
            // comboBox3
            // 
            this.comboBox3.Enabled = false;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(396, 245);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(98, 25);
            this.comboBox3.TabIndex = 4;
            this.comboBox3.Text = "切换到桌面";
            // 
            // comboBox2
            // 
            this.comboBox2.Enabled = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(396, 102);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(98, 25);
            this.comboBox2.TabIndex = 3;
            this.comboBox2.Text = "上一个任务";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(64, 102);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(98, 25);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "Esc键";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(166, 102);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(225, 168);
            this.panel2.TabIndex = 1;
            // 
            // check_enableHotCorners
            // 
            this.check_enableHotCorners.AutoSize = true;
            this.check_enableHotCorners.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureParserEnableHotCorners", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_enableHotCorners.Location = new System.Drawing.Point(13, 20);
            this.check_enableHotCorners.Margin = new System.Windows.Forms.Padding(2);
            this.check_enableHotCorners.Name = "check_enableHotCorners";
            this.check_enableHotCorners.Size = new System.Drawing.Size(87, 21);
            this.check_enableHotCorners.TabIndex = 0;
            this.check_enableHotCorners.Text = "启用触发角";
            this.check_enableHotCorners.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.picture_alipayCode);
            this.tabPage1.Controls.Add(this.tb_updateLog);
            this.tabPage1.Controls.Add(this.flowLayoutPanel5);
            this.tabPage1.Controls.Add(this.picture_logo);
            this.tabPage1.Location = new System.Drawing.Point(4, 32);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(1);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(551, 435);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Tag = "about";
            this.tabPage1.Text = "关 于";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.ForeColor = System.Drawing.Color.OrangeRed;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(233, 215);
            this.label10.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(185, 23);
            this.label10.TabIndex = 6;
            this.label10.Text = "支付宝钱包扫码";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(130, 181);
            this.label12.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(412, 34);
            this.label12.TabIndex = 6;
            this.label12.Text = "若WGestures对您有用，请考虑捐助支持该项目，以帮助我做得更好";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picture_alipayCode
            // 
            this.picture_alipayCode.Image = global::WGestures.App.Properties.Resources.alipay;
            this.picture_alipayCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picture_alipayCode.Location = new System.Drawing.Point(235, 239);
            this.picture_alipayCode.Margin = new System.Windows.Forms.Padding(1);
            this.picture_alipayCode.Name = "picture_alipayCode";
            this.picture_alipayCode.Size = new System.Drawing.Size(183, 184);
            this.picture_alipayCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_alipayCode.TabIndex = 4;
            this.picture_alipayCode.TabStop = false;
            // 
            // tb_updateLog
            // 
            this.tb_updateLog.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.tb_updateLog.Location = new System.Drawing.Point(130, 21);
            this.tb_updateLog.Margin = new System.Windows.Forms.Padding(1);
            this.tb_updateLog.Multiline = true;
            this.tb_updateLog.Name = "tb_updateLog";
            this.tb_updateLog.ReadOnly = true;
            this.tb_updateLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_updateLog.Size = new System.Drawing.Size(412, 153);
            this.tb_updateLog.TabIndex = 3;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Controls.Add(this.linkLabel1);
            this.flowLayoutPanel5.Controls.Add(this.linkLabel2);
            this.flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(9, 127);
            this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(105, 81);
            this.flowLayoutPanel5.TabIndex = 2;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.linkLabel1.Location = new System.Drawing.Point(1, 0);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 8);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(56, 17);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "项目主页";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.linkLabel2.Location = new System.Drawing.Point(1, 25);
            this.linkLabel2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 8);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(56, 17);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "作者邮箱";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // picture_logo
            // 
            this.picture_logo.Image = global::WGestures.App.Properties.Resources._128;
            this.picture_logo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picture_logo.Location = new System.Drawing.Point(9, 21);
            this.picture_logo.Margin = new System.Windows.Forms.Padding(1);
            this.picture_logo.Name = "picture_logo";
            this.picture_logo.Size = new System.Drawing.Size(105, 97);
            this.picture_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_logo.TabIndex = 0;
            this.picture_logo.TabStop = false;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.lb_info);
            this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(9, 488);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 12, 0, 10);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(557, 16);
            this.flowLayoutPanel4.TabIndex = 10;
            // 
            // lb_info
            // 
            this.lb_info.AutoSize = true;
            this.lb_info.ForeColor = System.Drawing.Color.DimGray;
            this.lb_info.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_info.Location = new System.Drawing.Point(397, 0);
            this.lb_info.Name = "lb_info";
            this.lb_info.Size = new System.Drawing.Size(157, 17);
            this.lb_info.TabIndex = 6;
            this.lb_info.Text = "*改动将自动保存并立即生效";
            // 
            // tip
            // 
            this.tip.AutomaticDelay = 80000;
            this.tip.AutoPopDelay = 144640;
            this.tip.InitialDelay = 120;
            this.tip.ReshowDelay = 2892;
            // 
            // pic_menuBtn
            // 
            this.pic_menuBtn.Image = global::WGestures.App.Properties.Resources.menuBtn;
            this.pic_menuBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pic_menuBtn.Location = new System.Drawing.Point(541, 10);
            this.pic_menuBtn.Name = "pic_menuBtn";
            this.pic_menuBtn.Size = new System.Drawing.Size(24, 24);
            this.pic_menuBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pic_menuBtn.TabIndex = 11;
            this.pic_menuBtn.TabStop = false;
            this.tip.SetToolTip(this.pic_menuBtn, "菜单");
            this.pic_menuBtn.Click += new System.EventHandler(this.pic_menuBtn_Click);
            this.pic_menuBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_menuBtn_MouseDown);
            // 
            // ctx_gesturesMenu
            // 
            this.ctx_gesturesMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_import,
            this.menuItem_export});
            this.ctx_gesturesMenu.Name = "contextMenuStrip1";
            this.ctx_gesturesMenu.Size = new System.Drawing.Size(110, 48);
            this.ctx_gesturesMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.ctx_gesturesMenu_Closed);
            // 
            // menuItem_import
            // 
            this.menuItem_import.Name = "menuItem_import";
            this.menuItem_import.Size = new System.Drawing.Size(109, 22);
            this.menuItem_import.Text = "导入...";
            this.menuItem_import.Click += new System.EventHandler(this.menuItem_imxport_Click);
            // 
            // menuItem_export
            // 
            this.menuItem_export.Name = "menuItem_export";
            this.menuItem_export.Size = new System.Drawing.Size(109, 22);
            this.menuItem_export.Text = "导出...";
            this.menuItem_export.Click += new System.EventHandler(this.menuItem_export_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkRate = 300;
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(577, 517);
            this.Controls.Add(this.pic_menuBtn);
            this.Controls.Add(this.flowLayoutPanel4);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WGestures设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.tabControl.ResumeLayout(false);
            this.tabPage_general.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsFormControllerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_pathTrackerInitialStayTimeoutMillis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPathTrackerStayTimeoutMillis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPathTrackerInitialValidMove)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSelectedApp)).EndInit();
            this.panel_intentListOperations.ResumeLayout(false);
            this.group_Command.ResumeLayout(false);
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tab_hotCorners.ResumeLayout(false);
            this.tab_hotCorners.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_alipayCode)).EndInit();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_logo)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_menuBtn)).EndInit();
            this.ctx_gesturesMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPage_general;
        private TabPage tabPage2;
        private CheckBox check_autoStart;
        private TabPage tabPage1;
        private CheckBox check_autoCheckUpdate;
        private Button btn_checkUpdateNow;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private CheckBox checkGestureViewShowPath;
        private Label label1;
        private CheckBox checkGestureViewShowCommandName;
        private Label label4;
        private Label label5;
        private Label label6;
        private InstantNumericUpDown numPathTrackerInitialValidMove;
        private CheckBox checkPathTrackerStayTimeout;
        private InstantNumericUpDown numPathTrackerStayTimeoutMillis;
        private AlwaysSelectedListView listApps;
        private Label lb_Version;
        private ImageList imglistAppIcons;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label labelAppName;
        private AlwaysSelectedListView listGestureIntents;
        private ColumnHeader colGestureName;
        private ColumnHeader colGestureDirs;
        private CheckBox check_gesturingDisabled;
        private ColumnHeader colListAppDummy;
        private PictureBox pictureSelectedApp;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label7;
        private MetroButton btnAddApp;
        private MetroButton btnAppRemove;
        private MetroButton btnEditApp;
        private Panel panel_intentListOperations;
        private MetroButton btn_RemoveGesture;
        private MetroButton btnAddGesture;
        private GroupBox group_Command;
        private Label label8;
        private ComboBox combo_CommandTypes;
        private Panel panel_commandView;
        private Windows.Controls.ColorButton colorBtn_unrecogonized;
        private Windows.Controls.ColorButton colorBtn_recogonized;
        private Label label9;
        private ColumnHeader operation;
        private ImageList dummyImgLstForLstViewHeightFix;
        private ToolTip tip;
        private FlowLayoutPanel flowLayoutPanel4;
        private Label lb_info;
        private PictureBox picture_logo;
        private CheckBox checkGestureView_fadeOut;
        private MetroButton btn_modifyGesture;
        private FlowLayoutPanel flowLayoutPanel3;
        private ErrorProvider errorProvider;
        private TextBox tb_updateLog;
        private FlowLayoutPanel flowLayoutPanel5;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel2;
        private PictureBox picture_alipayCode;
        private Label label12;
        private Label label10;
        private LineLabel lineLabel1;
        private FlowLayoutPanel flowLayoutPanel6;
        private Panel panel3;
        private CheckBox check_executeOnMouseWheeling;
        private InstantNumericUpDown num_pathTrackerInitialStayTimeoutMillis;
        private CheckBox check_pathTrackerInitialStayTimeout;
        private PictureBox pic_menuBtn;
        private ContextMenuStrip ctx_gesturesMenu;
        private ToolStripMenuItem menuItem_import;
        private ToolStripMenuItem menuItem_export;
        private CheckBox check_disableOnFullscreen;
        private BindingSource settingsFormControllerBindingSource;
        private ColorButton colorMiddle;
        private Panel panel1;
        private CheckBox checkInheritGlobal;
        private LineLabel lineLabel2;
        private Label label2;
        private Label label3;
        private ComboBox combo_GestureTriggerButton;
        private TabPage tab_hotCorners;
        private CheckBox check_enableHotCorners;
        private ComboBox comboBox4;
        private ComboBox comboBox3;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private Panel panel2;
        private Label label11;
        private CheckBox check_enable8DirGesture;
        private CheckBox check_preferCursorWindow;
    }
}