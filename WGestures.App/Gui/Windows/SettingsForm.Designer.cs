﻿using System;
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
            this.check_enable8DirGesture = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.check_disableOnFullscreen = new System.Windows.Forms.CheckBox();
            this.check_pathTrackerInitialStayTimeout = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.check_gestBtn_X = new System.Windows.Forms.CheckBox();
            this.checkGestureView_fadeOut = new System.Windows.Forms.CheckBox();
            this.check_gestBtn_Middle = new System.Windows.Forms.CheckBox();
            this.checkGestureViewShowCommandName = new System.Windows.Forms.CheckBox();
            this.checkPathTrackerStayTimeout = new System.Windows.Forms.CheckBox();
            this.check_gestBtn_Right = new System.Windows.Forms.CheckBox();
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
            this.lb_pause_shortcut = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkInheritGlobal = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.check_gesturingDisabled = new System.Windows.Forms.CheckBox();
            this.pictureSelectedApp = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dummyImgLstForLstViewHeightFix = new System.Windows.Forms.ImageList(this.components);
            this.panel_intentListOperations = new System.Windows.Forms.Panel();
            this.group_Command = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.combo_CommandTypes = new System.Windows.Forms.ComboBox();
            this.check_executeOnMouseWheeling = new System.Windows.Forms.CheckBox();
            this.panel_commandView = new System.Windows.Forms.Panel();
            this.imglistAppIcons = new System.Windows.Forms.ImageList(this.components);
            this.tab_hotCorners = new System.Windows.Forms.TabPage();
            this.panel_hotcornerSettings = new System.Windows.Forms.Panel();
            this.radio_edge_0 = new System.Windows.Forms.RadioButton();
            this.radio_edge_2 = new System.Windows.Forms.RadioButton();
            this.radio_edge_1 = new System.Windows.Forms.RadioButton();
            this.radio_edge_3 = new System.Windows.Forms.RadioButton();
            this.radio_corner_1 = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.combo_hotcornerCmdTypes = new System.Windows.Forms.ComboBox();
            this.panel_cornorCmdView = new System.Windows.Forms.Panel();
            this.radio_corner_0 = new System.Windows.Forms.RadioButton();
            this.radio_corner_2 = new System.Windows.Forms.RadioButton();
            this.radio_corner_3 = new System.Windows.Forms.RadioButton();
            this.check_enableRubEdge = new System.Windows.Forms.CheckBox();
            this.check_enableHotCorners = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
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
            this.menuItem_resetGestures = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.settingsFormControllerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lineLabel2 = new WGestures.App.Gui.Windows.Controls.LineLabel();
            this.num_pathTrackerInitialStayTimeoutMillis = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            this.colorMiddle = new WGestures.App.Gui.Windows.Controls.ColorButton();
            this.colorBtn_recogonized = new WGestures.App.Gui.Windows.Controls.ColorButton();
            this.colorBtn_x = new WGestures.App.Gui.Windows.Controls.ColorButton();
            this.colorBtn_unrecogonized = new WGestures.App.Gui.Windows.Controls.ColorButton();
            this.numPathTrackerStayTimeoutMillis = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            this.numPathTrackerInitialValidMove = new WGestures.App.Gui.Windows.Controls.InstantNumericUpDown();
            this.shortcutRec_pause = new WGestures.App.Gui.Windows.Controls.ShortcutRecordButton();
            this.listGestureIntents = new WGestures.App.Gui.Windows.Controls.AlwaysSelectedListView();
            this.colGestureName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGestureDirs = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.operation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_RemoveGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.btn_modifyGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.btnAddGesture = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.lineLabel1 = new WGestures.App.Gui.Windows.Controls.LineLabel();
            this.btnEditApp = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.btnAppRemove = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.btnAddApp = new WGestures.App.Gui.Windows.Controls.MetroButton();
            this.listApps = new WGestures.App.Gui.Windows.Controls.AlwaysSelectedListView();
            this.colListAppDummy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl.SuspendLayout();
            this.tabPage_general.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.panel_hotcornerSettings.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.flowLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_alipayCode)).BeginInit();
            this.flowLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_logo)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_menuBtn)).BeginInit();
            this.ctx_gesturesMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsFormControllerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_pathTrackerInitialStayTimeoutMillis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPathTrackerStayTimeoutMillis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPathTrackerInitialValidMove)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_general);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tab_hotCorners);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.HotTrack = true;
            this.tabControl.ItemSize = new System.Drawing.Size(250, 28);
            this.tabControl.Location = new System.Drawing.Point(20, 20);
            this.tabControl.Margin = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(20, 3);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1116, 1008);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPage_general
            // 
            this.tabPage_general.BackColor = System.Drawing.Color.White;
            this.tabPage_general.Controls.Add(this.groupBox2);
            this.tabPage_general.Controls.Add(this.groupBox1);
            this.tabPage_general.Location = new System.Drawing.Point(8, 36);
            this.tabPage_general.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage_general.Name = "tabPage_general";
            this.tabPage_general.Size = new System.Drawing.Size(1100, 964);
            this.tabPage_general.TabIndex = 0;
            this.tabPage_general.Tag = "general";
            this.tabPage_general.Text = "选 项";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.check_preferCursorWindow);
            this.groupBox2.Controls.Add(this.check_enable8DirGesture);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lineLabel2);
            this.groupBox2.Controls.Add(this.check_disableOnFullscreen);
            this.groupBox2.Controls.Add(this.num_pathTrackerInitialStayTimeoutMillis);
            this.groupBox2.Controls.Add(this.check_pathTrackerInitialStayTimeout);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.colorMiddle);
            this.groupBox2.Controls.Add(this.colorBtn_recogonized);
            this.groupBox2.Controls.Add(this.colorBtn_x);
            this.groupBox2.Controls.Add(this.colorBtn_unrecogonized);
            this.groupBox2.Controls.Add(this.numPathTrackerStayTimeoutMillis);
            this.groupBox2.Controls.Add(this.numPathTrackerInitialValidMove);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.check_gestBtn_X);
            this.groupBox2.Controls.Add(this.checkGestureView_fadeOut);
            this.groupBox2.Controls.Add(this.check_gestBtn_Middle);
            this.groupBox2.Controls.Add(this.checkGestureViewShowCommandName);
            this.groupBox2.Controls.Add(this.checkPathTrackerStayTimeout);
            this.groupBox2.Controls.Add(this.check_gestBtn_Right);
            this.groupBox2.Controls.Add(this.checkGestureViewShowPath);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(28, 342);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(1052, 544);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "参数";
            // 
            // check_preferCursorWindow
            // 
            this.check_preferCursorWindow.AutoSize = true;
            this.check_preferCursorWindow.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerPreferCursorWindow", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_preferCursorWindow.Location = new System.Drawing.Point(600, 228);
            this.check_preferCursorWindow.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_preferCursorWindow.Name = "check_preferCursorWindow";
            this.check_preferCursorWindow.Size = new System.Drawing.Size(334, 35);
            this.check_preferCursorWindow.TabIndex = 18;
            this.check_preferCursorWindow.Text = "总是作用于指针下方的窗口";
            this.tip.SetToolTip(this.check_preferCursorWindow, "使手势总是作用于鼠标指针下方窗口，而不是当前活动程序");
            this.check_preferCursorWindow.UseVisualStyleBackColor = true;
            // 
            // check_enable8DirGesture
            // 
            this.check_enable8DirGesture.AutoSize = true;
            this.check_enable8DirGesture.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureParserEnable8DirGesture", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_enable8DirGesture.Location = new System.Drawing.Point(600, 168);
            this.check_enable8DirGesture.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_enable8DirGesture.Name = "check_enable8DirGesture";
            this.check_enable8DirGesture.Size = new System.Drawing.Size(238, 35);
            this.check_enable8DirGesture.TabIndex = 17;
            this.check_enable8DirGesture.Text = "允许使用斜线手势";
            this.tip.SetToolTip(this.check_enable8DirGesture, "是否允许使用”↖↙↗↘“手势");
            this.check_enable8DirGesture.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(586, 64);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 31);
            this.label3.TabIndex = 16;
            this.label3.Text = "手势键:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tip.SetToolTip(this.label3, "允许哪个鼠标按钮触发手势？");
            // 
            // check_disableOnFullscreen
            // 
            this.check_disableOnFullscreen.AutoSize = true;
            this.check_disableOnFullscreen.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerDisableInFullScreen", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_disableOnFullscreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_disableOnFullscreen.Location = new System.Drawing.Point(168, 234);
            this.check_disableOnFullscreen.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_disableOnFullscreen.Name = "check_disableOnFullscreen";
            this.check_disableOnFullscreen.Size = new System.Drawing.Size(262, 35);
            this.check_disableOnFullscreen.TabIndex = 12;
            this.check_disableOnFullscreen.Text = "全屏时自动禁用手势";
            this.check_disableOnFullscreen.UseVisualStyleBackColor = true;
            // 
            // check_pathTrackerInitialStayTimeout
            // 
            this.check_pathTrackerInitialStayTimeout.AutoSize = true;
            this.check_pathTrackerInitialStayTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerInitialStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_pathTrackerInitialStayTimeout.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_pathTrackerInitialStayTimeout.Location = new System.Drawing.Point(168, 114);
            this.check_pathTrackerInitialStayTimeout.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_pathTrackerInitialStayTimeout.Name = "check_pathTrackerInitialStayTimeout";
            this.check_pathTrackerInitialStayTimeout.Size = new System.Drawing.Size(213, 35);
            this.check_pathTrackerInitialStayTimeout.TabIndex = 10;
            this.check_pathTrackerInitialStayTimeout.Text = "起始超时 (毫秒)";
            this.tip.SetToolTip(this.check_pathTrackerInitialStayTimeout, "若按下右键后超过此时间未移动，则执行正常右键拖拽操作");
            this.check_pathTrackerInitialStayTimeout.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(162, 384);
            this.label9.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 31);
            this.label9.TabIndex = 9;
            this.label9.Text = "轨迹风格:";
            // 
            // check_gestBtn_X
            // 
            this.check_gestBtn_X.AutoSize = true;
            this.check_gestBtn_X.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_gestBtn_X.Location = new System.Drawing.Point(932, 62);
            this.check_gestBtn_X.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_gestBtn_X.Name = "check_gestBtn_X";
            this.check_gestBtn_X.Size = new System.Drawing.Size(85, 35);
            this.check_gestBtn_X.TabIndex = 1;
            this.check_gestBtn_X.Tag = "12";
            this.check_gestBtn_X.Text = "X键";
            this.check_gestBtn_X.UseVisualStyleBackColor = true;
            this.check_gestBtn_X.CheckedChanged += new System.EventHandler(this.check_gestBtns_checkedChanged);
            // 
            // checkGestureView_fadeOut
            // 
            this.checkGestureView_fadeOut.AutoSize = true;
            this.checkGestureView_fadeOut.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewFadeOut", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkGestureView_fadeOut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkGestureView_fadeOut.Location = new System.Drawing.Point(540, 324);
            this.checkGestureView_fadeOut.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.checkGestureView_fadeOut.Name = "checkGestureView_fadeOut";
            this.checkGestureView_fadeOut.Size = new System.Drawing.Size(166, 35);
            this.checkGestureView_fadeOut.TabIndex = 1;
            this.checkGestureView_fadeOut.Text = "执行后淡出";
            this.tip.SetToolTip(this.checkGestureView_fadeOut, "手势执行后图形逐渐消失(而非突然消失)");
            this.checkGestureView_fadeOut.UseVisualStyleBackColor = true;
            // 
            // check_gestBtn_Middle
            // 
            this.check_gestBtn_Middle.AutoSize = true;
            this.check_gestBtn_Middle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_gestBtn_Middle.Location = new System.Drawing.Point(814, 62);
            this.check_gestBtn_Middle.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_gestBtn_Middle.Name = "check_gestBtn_Middle";
            this.check_gestBtn_Middle.Size = new System.Drawing.Size(94, 35);
            this.check_gestBtn_Middle.TabIndex = 1;
            this.check_gestBtn_Middle.Tag = "2";
            this.check_gestBtn_Middle.Text = "中键";
            this.check_gestBtn_Middle.UseVisualStyleBackColor = true;
            this.check_gestBtn_Middle.CheckedChanged += new System.EventHandler(this.check_gestBtns_checkedChanged);
            // 
            // checkGestureViewShowCommandName
            // 
            this.checkGestureViewShowCommandName.AutoSize = true;
            this.checkGestureViewShowCommandName.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewShowCommandName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkGestureViewShowCommandName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkGestureViewShowCommandName.Location = new System.Drawing.Point(328, 324);
            this.checkGestureViewShowCommandName.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.checkGestureViewShowCommandName.Name = "checkGestureViewShowCommandName";
            this.checkGestureViewShowCommandName.Size = new System.Drawing.Size(142, 35);
            this.checkGestureViewShowCommandName.TabIndex = 1;
            this.checkGestureViewShowCommandName.Text = "手势名称";
            this.checkGestureViewShowCommandName.UseVisualStyleBackColor = true;
            // 
            // checkPathTrackerStayTimeout
            // 
            this.checkPathTrackerStayTimeout.AutoSize = true;
            this.checkPathTrackerStayTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkPathTrackerStayTimeout.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkPathTrackerStayTimeout.Location = new System.Drawing.Point(168, 174);
            this.checkPathTrackerStayTimeout.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.checkPathTrackerStayTimeout.Name = "checkPathTrackerStayTimeout";
            this.checkPathTrackerStayTimeout.Size = new System.Drawing.Size(213, 35);
            this.checkPathTrackerStayTimeout.TabIndex = 0;
            this.checkPathTrackerStayTimeout.Text = "停留超时 (毫秒)";
            this.tip.SetToolTip(this.checkPathTrackerStayTimeout, "若鼠标停止移动超过此时间，已画出的手势将被取消");
            this.checkPathTrackerStayTimeout.UseVisualStyleBackColor = true;
            // 
            // check_gestBtn_Right
            // 
            this.check_gestBtn_Right.AutoSize = true;
            this.check_gestBtn_Right.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_gestBtn_Right.Location = new System.Drawing.Point(696, 62);
            this.check_gestBtn_Right.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_gestBtn_Right.Name = "check_gestBtn_Right";
            this.check_gestBtn_Right.Size = new System.Drawing.Size(94, 35);
            this.check_gestBtn_Right.TabIndex = 0;
            this.check_gestBtn_Right.Tag = "1";
            this.check_gestBtn_Right.Text = "右键";
            this.check_gestBtn_Right.UseVisualStyleBackColor = true;
            this.check_gestBtn_Right.CheckedChanged += new System.EventHandler(this.check_gestBtns_checkedChanged);
            // 
            // checkGestureViewShowPath
            // 
            this.checkGestureViewShowPath.AutoSize = true;
            this.checkGestureViewShowPath.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewShowPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkGestureViewShowPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkGestureViewShowPath.Location = new System.Drawing.Point(168, 324);
            this.checkGestureViewShowPath.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.checkGestureViewShowPath.Name = "checkGestureViewShowPath";
            this.checkGestureViewShowPath.Size = new System.Drawing.Size(94, 35);
            this.checkGestureViewShowPath.TabIndex = 0;
            this.checkGestureViewShowPath.Text = "轨迹";
            this.checkGestureViewShowPath.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(162, 64);
            this.label6.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(222, 31);
            this.label6.TabIndex = 2;
            this.label6.Text = "起始移动距离(像素)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tip.SetToolTip(this.label6, "只有移动超过此距离，才开始识别手势");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(52, 64);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 31);
            this.label5.TabIndex = 2;
            this.label5.Text = "有效性:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(52, 328);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "显   示:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.shortcutRec_pause);
            this.groupBox1.Controls.Add(this.flowLayoutPanel3);
            this.groupBox1.Controls.Add(this.check_autoStart);
            this.groupBox1.Controls.Add(this.btn_checkUpdateNow);
            this.groupBox1.Controls.Add(this.check_autoCheckUpdate);
            this.groupBox1.Controls.Add(this.lb_pause_shortcut);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(28, 40);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1052, 254);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通用";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Controls.Add(this.lb_Version);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(464, 108);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(556, 46);
            this.flowLayoutPanel3.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(4, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 12, 4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 31);
            this.label4.TabIndex = 2;
            this.label4.Text = "当前版本:";
            // 
            // lb_Version
            // 
            this.lb_Version.AutoSize = true;
            this.lb_Version.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lb_Version.ForeColor = System.Drawing.Color.Gray;
            this.lb_Version.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_Version.Location = new System.Drawing.Point(128, 12);
            this.lb_Version.Margin = new System.Windows.Forms.Padding(4, 12, 4, 4);
            this.lb_Version.Name = "lb_Version";
            this.lb_Version.Size = new System.Drawing.Size(97, 31);
            this.lb_Version.TabIndex = 3;
            this.lb_Version.Text = "version";
            // 
            // check_autoStart
            // 
            this.check_autoStart.AutoSize = true;
            this.check_autoStart.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "AutoStart", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_autoStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_autoStart.Location = new System.Drawing.Point(60, 60);
            this.check_autoStart.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_autoStart.Name = "check_autoStart";
            this.check_autoStart.Size = new System.Drawing.Size(190, 35);
            this.check_autoStart.TabIndex = 0;
            this.check_autoStart.Text = "开机自动运行";
            this.check_autoStart.UseVisualStyleBackColor = true;
            // 
            // btn_checkUpdateNow
            // 
            this.btn_checkUpdateNow.BackColor = System.Drawing.SystemColors.Control;
            this.btn_checkUpdateNow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_checkUpdateNow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_checkUpdateNow.Location = new System.Drawing.Point(286, 114);
            this.btn_checkUpdateNow.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.btn_checkUpdateNow.Name = "btn_checkUpdateNow";
            this.btn_checkUpdateNow.Size = new System.Drawing.Size(148, 46);
            this.btn_checkUpdateNow.TabIndex = 2;
            this.btn_checkUpdateNow.Text = "立即检查";
            this.btn_checkUpdateNow.UseVisualStyleBackColor = false;
            this.btn_checkUpdateNow.Click += new System.EventHandler(this.btn_checkUpdateNow_Click);
            // 
            // check_autoCheckUpdate
            // 
            this.check_autoCheckUpdate.AutoSize = true;
            this.check_autoCheckUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "AutoCheckForUpdate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_autoCheckUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_autoCheckUpdate.Location = new System.Drawing.Point(60, 120);
            this.check_autoCheckUpdate.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.check_autoCheckUpdate.Name = "check_autoCheckUpdate";
            this.check_autoCheckUpdate.Size = new System.Drawing.Size(190, 35);
            this.check_autoCheckUpdate.TabIndex = 1;
            this.check_autoCheckUpdate.Text = "自动检查更新";
            this.check_autoCheckUpdate.UseVisualStyleBackColor = true;
            // 
            // lb_pause_shortcut
            // 
            this.lb_pause_shortcut.AutoSize = true;
            this.lb_pause_shortcut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_pause_shortcut.Location = new System.Drawing.Point(478, 180);
            this.lb_pause_shortcut.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.lb_pause_shortcut.Name = "lb_pause_shortcut";
            this.lb_pause_shortcut.Size = new System.Drawing.Size(38, 31);
            this.lb_pause_shortcut.TabIndex = 2;
            this.lb_pause_shortcut.Text = "无";
            this.lb_pause_shortcut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(54, 180);
            this.label14.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(198, 31);
            this.label14.TabIndex = 2;
            this.label14.Text = "暂停/继续快捷键:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.flowLayoutPanel2);
            this.tabPage2.Controls.Add(this.btnEditApp);
            this.tabPage2.Controls.Add(this.btnAppRemove);
            this.tabPage2.Controls.Add(this.btnAddApp);
            this.tabPage2.Controls.Add(this.listApps);
            this.tabPage2.Location = new System.Drawing.Point(8, 36);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1100, 964);
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
            this.flowLayoutPanel2.Location = new System.Drawing.Point(380, 36);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(704, 885);
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
            this.panel1.Size = new System.Drawing.Size(698, 43);
            this.panel1.TabIndex = 12;
            // 
            // checkInheritGlobal
            // 
            this.checkInheritGlobal.AutoSize = true;
            this.checkInheritGlobal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkInheritGlobal.Location = new System.Drawing.Point(508, 4);
            this.checkInheritGlobal.Margin = new System.Windows.Forms.Padding(0);
            this.checkInheritGlobal.Name = "checkInheritGlobal";
            this.checkInheritGlobal.Size = new System.Drawing.Size(190, 35);
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
            this.flowLayoutPanel1.Controls.Add(this.label15);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(458, 43);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // check_gesturingDisabled
            // 
            this.check_gesturingDisabled.AutoSize = true;
            this.check_gesturingDisabled.ForeColor = System.Drawing.Color.Black;
            this.check_gesturingDisabled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_gesturingDisabled.Location = new System.Drawing.Point(20, 4);
            this.check_gesturingDisabled.Margin = new System.Windows.Forms.Padding(20, 4, 0, 4);
            this.check_gesturingDisabled.Name = "check_gesturingDisabled";
            this.check_gesturingDisabled.Size = new System.Drawing.Size(118, 35);
            this.check_gesturingDisabled.TabIndex = 5;
            this.check_gesturingDisabled.Text = "不要在";
            this.tip.SetToolTip(this.check_gesturingDisabled, "在该程序上禁用手势（等同于双击应用程序条目）");
            this.check_gesturingDisabled.UseVisualStyleBackColor = true;
            this.check_gesturingDisabled.CheckedChanged += new System.EventHandler(this.check_gesturingEnabled_CheckedChanged);
            // 
            // pictureSelectedApp
            // 
            this.pictureSelectedApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureSelectedApp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureSelectedApp.Location = new System.Drawing.Point(138, 1);
            this.pictureSelectedApp.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pictureSelectedApp.Name = "pictureSelectedApp";
            this.pictureSelectedApp.Size = new System.Drawing.Size(36, 36);
            this.pictureSelectedApp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSelectedApp.TabIndex = 2;
            this.pictureSelectedApp.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.White;
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(174, 4);
            this.label15.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(182, 31);
            this.label15.TabIndex = 3;
            this.label15.Text = "上使用任何手势";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(356, 4);
            this.label7.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 31);
            this.label7.TabIndex = 3;
            this.label7.Text = "(黑名单)";
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
            this.panel_intentListOperations.Location = new System.Drawing.Point(20, 381);
            this.panel_intentListOperations.Margin = new System.Windows.Forms.Padding(20, 4, 0, 0);
            this.panel_intentListOperations.Name = "panel_intentListOperations";
            this.panel_intentListOperations.Size = new System.Drawing.Size(680, 44);
            this.panel_intentListOperations.TabIndex = 9;
            // 
            // group_Command
            // 
            this.group_Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.group_Command.Controls.Add(this.flowLayoutPanel6);
            this.group_Command.Enabled = false;
            this.group_Command.Location = new System.Drawing.Point(20, 445);
            this.group_Command.Margin = new System.Windows.Forms.Padding(20, 20, 4, 4);
            this.group_Command.Name = "group_Command";
            this.group_Command.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.group_Command.Size = new System.Drawing.Size(680, 436);
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
            this.flowLayoutPanel6.Location = new System.Drawing.Point(8, 36);
            this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(664, 388);
            this.flowLayoutPanel6.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.combo_CommandTypes);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(664, 68);
            this.panel3.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(4, 14);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 31);
            this.label8.TabIndex = 1;
            this.label8.Text = "执行操作";
            // 
            // combo_CommandTypes
            // 
            this.combo_CommandTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_CommandTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.combo_CommandTypes.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.combo_CommandTypes.FormattingEnabled = true;
            this.combo_CommandTypes.ItemHeight = 31;
            this.combo_CommandTypes.Location = new System.Drawing.Point(132, 8);
            this.combo_CommandTypes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.combo_CommandTypes.Name = "combo_CommandTypes";
            this.combo_CommandTypes.Size = new System.Drawing.Size(516, 39);
            this.combo_CommandTypes.TabIndex = 0;
            this.tip.SetToolTip(this.combo_CommandTypes, "手势触发后要执行的操作");
            this.combo_CommandTypes.SelectedIndexChanged += new System.EventHandler(this.combo_CommandTypes_SelectedIndexChanged);
            // 
            // check_executeOnMouseWheeling
            // 
            this.check_executeOnMouseWheeling.AutoSize = true;
            this.check_executeOnMouseWheeling.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.check_executeOnMouseWheeling.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.check_executeOnMouseWheeling.Location = new System.Drawing.Point(4, 72);
            this.check_executeOnMouseWheeling.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.check_executeOnMouseWheeling.Name = "check_executeOnMouseWheeling";
            this.check_executeOnMouseWheeling.Size = new System.Drawing.Size(279, 36);
            this.check_executeOnMouseWheeling.TabIndex = 3;
            this.check_executeOnMouseWheeling.Text = "修饰键触发时立即执行";
            this.check_executeOnMouseWheeling.UseVisualStyleBackColor = true;
            this.check_executeOnMouseWheeling.Visible = false;
            this.check_executeOnMouseWheeling.CheckedChanged += new System.EventHandler(this.check_executeOnMouseWheeling_CheckedChanged);
            // 
            // panel_commandView
            // 
            this.panel_commandView.AutoScroll = true;
            this.panel_commandView.BackColor = System.Drawing.Color.Transparent;
            this.panel_commandView.Location = new System.Drawing.Point(0, 120);
            this.panel_commandView.Margin = new System.Windows.Forms.Padding(0);
            this.panel_commandView.Name = "panel_commandView";
            this.panel_commandView.Size = new System.Drawing.Size(664, 256);
            this.panel_commandView.TabIndex = 2;
            this.tip.SetToolTip(this.panel_commandView, "操作的额外参数");
            // 
            // imglistAppIcons
            // 
            this.imglistAppIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imglistAppIcons.ImageSize = new System.Drawing.Size(32, 32);
            this.imglistAppIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tab_hotCorners
            // 
            this.tab_hotCorners.Controls.Add(this.panel_hotcornerSettings);
            this.tab_hotCorners.Controls.Add(this.check_enableRubEdge);
            this.tab_hotCorners.Controls.Add(this.check_enableHotCorners);
            this.tab_hotCorners.Location = new System.Drawing.Point(8, 36);
            this.tab_hotCorners.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tab_hotCorners.Name = "tab_hotCorners";
            this.tab_hotCorners.Size = new System.Drawing.Size(1100, 964);
            this.tab_hotCorners.TabIndex = 3;
            this.tab_hotCorners.Tag = "corners";
            this.tab_hotCorners.Text = "触发角 & 摩擦边";
            this.tab_hotCorners.UseVisualStyleBackColor = true;
            // 
            // panel_hotcornerSettings
            // 
            this.panel_hotcornerSettings.Controls.Add(this.radio_edge_0);
            this.panel_hotcornerSettings.Controls.Add(this.radio_edge_2);
            this.panel_hotcornerSettings.Controls.Add(this.radio_edge_1);
            this.panel_hotcornerSettings.Controls.Add(this.radio_edge_3);
            this.panel_hotcornerSettings.Controls.Add(this.radio_corner_1);
            this.panel_hotcornerSettings.Controls.Add(this.label11);
            this.panel_hotcornerSettings.Controls.Add(this.panel2);
            this.panel_hotcornerSettings.Controls.Add(this.combo_hotcornerCmdTypes);
            this.panel_hotcornerSettings.Controls.Add(this.panel_cornorCmdView);
            this.panel_hotcornerSettings.Controls.Add(this.radio_corner_0);
            this.panel_hotcornerSettings.Controls.Add(this.radio_corner_2);
            this.panel_hotcornerSettings.Controls.Add(this.radio_corner_3);
            this.panel_hotcornerSettings.Location = new System.Drawing.Point(28, 92);
            this.panel_hotcornerSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_hotcornerSettings.Name = "panel_hotcornerSettings";
            this.panel_hotcornerSettings.Size = new System.Drawing.Size(1044, 836);
            this.panel_hotcornerSettings.TabIndex = 10;
            // 
            // radio_edge_0
            // 
            this.radio_edge_0.Appearance = System.Windows.Forms.Appearance.Button;
            this.radio_edge_0.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radio_edge_0.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radio_edge_0.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.radio_edge_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radio_edge_0.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_edge_0.Location = new System.Drawing.Point(170, 192);
            this.radio_edge_0.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radio_edge_0.Name = "radio_edge_0";
            this.radio_edge_0.Size = new System.Drawing.Size(208, 52);
            this.radio_edge_0.TabIndex = 7;
            this.radio_edge_0.TabStop = true;
            this.radio_edge_0.Tag = "4";
            this.radio_edge_0.Text = "?";
            this.radio_edge_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tip.SetToolTip(this.radio_edge_0, "屏幕左边缘");
            this.radio_edge_0.UseVisualStyleBackColor = false;
            this.radio_edge_0.CheckedChanged += new System.EventHandler(this.radio_corner_1_CheckedChanged);
            // 
            // radio_edge_2
            // 
            this.radio_edge_2.Appearance = System.Windows.Forms.Appearance.Button;
            this.radio_edge_2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radio_edge_2.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radio_edge_2.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.radio_edge_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radio_edge_2.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_edge_2.Location = new System.Drawing.Point(668, 192);
            this.radio_edge_2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radio_edge_2.Name = "radio_edge_2";
            this.radio_edge_2.Size = new System.Drawing.Size(208, 52);
            this.radio_edge_2.TabIndex = 7;
            this.radio_edge_2.TabStop = true;
            this.radio_edge_2.Tag = "6";
            this.radio_edge_2.Text = "?";
            this.radio_edge_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tip.SetToolTip(this.radio_edge_2, "屏幕右边缘");
            this.radio_edge_2.UseVisualStyleBackColor = false;
            this.radio_edge_2.CheckedChanged += new System.EventHandler(this.radio_corner_1_CheckedChanged);
            // 
            // radio_edge_1
            // 
            this.radio_edge_1.Appearance = System.Windows.Forms.Appearance.Button;
            this.radio_edge_1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radio_edge_1.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radio_edge_1.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.radio_edge_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radio_edge_1.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_edge_1.Location = new System.Drawing.Point(424, 22);
            this.radio_edge_1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radio_edge_1.Name = "radio_edge_1";
            this.radio_edge_1.Size = new System.Drawing.Size(208, 52);
            this.radio_edge_1.TabIndex = 7;
            this.radio_edge_1.TabStop = true;
            this.radio_edge_1.Tag = "5";
            this.radio_edge_1.Text = "?";
            this.radio_edge_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tip.SetToolTip(this.radio_edge_1, "屏幕上边缘");
            this.radio_edge_1.UseVisualStyleBackColor = false;
            this.radio_edge_1.CheckedChanged += new System.EventHandler(this.radio_corner_1_CheckedChanged);
            // 
            // radio_edge_3
            // 
            this.radio_edge_3.Appearance = System.Windows.Forms.Appearance.Button;
            this.radio_edge_3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radio_edge_3.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radio_edge_3.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.radio_edge_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radio_edge_3.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_edge_3.Location = new System.Drawing.Point(424, 360);
            this.radio_edge_3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radio_edge_3.Name = "radio_edge_3";
            this.radio_edge_3.Size = new System.Drawing.Size(208, 52);
            this.radio_edge_3.TabIndex = 7;
            this.radio_edge_3.TabStop = true;
            this.radio_edge_3.Tag = "7";
            this.radio_edge_3.Text = "?";
            this.radio_edge_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tip.SetToolTip(this.radio_edge_3, "屏幕下边缘");
            this.radio_edge_3.UseVisualStyleBackColor = false;
            this.radio_edge_3.CheckedChanged += new System.EventHandler(this.radio_corner_1_CheckedChanged);
            // 
            // radio_corner_1
            // 
            this.radio_corner_1.Appearance = System.Windows.Forms.Appearance.Button;
            this.radio_corner_1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radio_corner_1.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radio_corner_1.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.radio_corner_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radio_corner_1.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_corner_1.Location = new System.Drawing.Point(58, 48);
            this.radio_corner_1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radio_corner_1.Name = "radio_corner_1";
            this.radio_corner_1.Size = new System.Drawing.Size(208, 52);
            this.radio_corner_1.TabIndex = 7;
            this.radio_corner_1.TabStop = true;
            this.radio_corner_1.Tag = "1";
            this.radio_corner_1.Text = "?";
            this.radio_corner_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tip.SetToolTip(this.radio_corner_1, "屏幕左上角");
            this.radio_corner_1.UseVisualStyleBackColor = false;
            this.radio_corner_1.CheckedChanged += new System.EventHandler(this.radio_corner_1_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label11.Location = new System.Drawing.Point(134, 478);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 31);
            this.label11.TabIndex = 9;
            this.label11.Text = "执行操作";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label13);
            this.panel2.ForeColor = System.Drawing.Color.AliceBlue;
            this.panel2.Location = new System.Drawing.Point(274, 48);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(498, 336);
            this.panel2.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.Silver;
            this.label13.Location = new System.Drawing.Point(200, 142);
            this.label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(109, 50);
            this.label13.TabIndex = 0;
            this.label13.Text = "屏 幕";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // combo_hotcornerCmdTypes
            // 
            this.combo_hotcornerCmdTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_hotcornerCmdTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.combo_hotcornerCmdTypes.FormattingEnabled = true;
            this.combo_hotcornerCmdTypes.Location = new System.Drawing.Point(254, 472);
            this.combo_hotcornerCmdTypes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.combo_hotcornerCmdTypes.Name = "combo_hotcornerCmdTypes";
            this.combo_hotcornerCmdTypes.Size = new System.Drawing.Size(240, 39);
            this.combo_hotcornerCmdTypes.TabIndex = 8;
            this.combo_hotcornerCmdTypes.SelectedIndexChanged += new System.EventHandler(this.combo_hotcornerCmdTypes_SelectedIndexChanged);
            this.combo_hotcornerCmdTypes.SelectedValueChanged += new System.EventHandler(this.combo_hotcornerCmdTypes_SelectedValueChanged);
            // 
            // panel_cornorCmdView
            // 
            this.panel_cornorCmdView.BackColor = System.Drawing.Color.Transparent;
            this.panel_cornorCmdView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_cornorCmdView.Location = new System.Drawing.Point(140, 534);
            this.panel_cornorCmdView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_cornorCmdView.Name = "panel_cornorCmdView";
            this.panel_cornorCmdView.Size = new System.Drawing.Size(782, 280);
            this.panel_cornorCmdView.TabIndex = 6;
            // 
            // radio_corner_0
            // 
            this.radio_corner_0.Appearance = System.Windows.Forms.Appearance.Button;
            this.radio_corner_0.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radio_corner_0.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radio_corner_0.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.radio_corner_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radio_corner_0.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_corner_0.Location = new System.Drawing.Point(58, 334);
            this.radio_corner_0.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radio_corner_0.Name = "radio_corner_0";
            this.radio_corner_0.Size = new System.Drawing.Size(208, 52);
            this.radio_corner_0.TabIndex = 7;
            this.radio_corner_0.TabStop = true;
            this.radio_corner_0.Tag = "0";
            this.radio_corner_0.Text = "?";
            this.radio_corner_0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tip.SetToolTip(this.radio_corner_0, "屏幕左下角");
            this.radio_corner_0.UseVisualStyleBackColor = false;
            this.radio_corner_0.CheckedChanged += new System.EventHandler(this.radio_corner_1_CheckedChanged);
            // 
            // radio_corner_2
            // 
            this.radio_corner_2.Appearance = System.Windows.Forms.Appearance.Button;
            this.radio_corner_2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radio_corner_2.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radio_corner_2.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.radio_corner_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radio_corner_2.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_corner_2.Location = new System.Drawing.Point(782, 48);
            this.radio_corner_2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radio_corner_2.Name = "radio_corner_2";
            this.radio_corner_2.Size = new System.Drawing.Size(208, 52);
            this.radio_corner_2.TabIndex = 7;
            this.radio_corner_2.TabStop = true;
            this.radio_corner_2.Tag = "2";
            this.radio_corner_2.Text = "?";
            this.radio_corner_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tip.SetToolTip(this.radio_corner_2, "屏幕右上角");
            this.radio_corner_2.UseVisualStyleBackColor = false;
            this.radio_corner_2.CheckedChanged += new System.EventHandler(this.radio_corner_1_CheckedChanged);
            // 
            // radio_corner_3
            // 
            this.radio_corner_3.Appearance = System.Windows.Forms.Appearance.Button;
            this.radio_corner_3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radio_corner_3.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radio_corner_3.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.radio_corner_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radio_corner_3.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radio_corner_3.Location = new System.Drawing.Point(782, 334);
            this.radio_corner_3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radio_corner_3.Name = "radio_corner_3";
            this.radio_corner_3.Size = new System.Drawing.Size(208, 52);
            this.radio_corner_3.TabIndex = 7;
            this.radio_corner_3.TabStop = true;
            this.radio_corner_3.Tag = "3";
            this.radio_corner_3.Text = "?";
            this.radio_corner_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tip.SetToolTip(this.radio_corner_3, "屏幕右下角");
            this.radio_corner_3.UseVisualStyleBackColor = false;
            this.radio_corner_3.CheckedChanged += new System.EventHandler(this.radio_corner_1_CheckedChanged);
            // 
            // check_enableRubEdge
            // 
            this.check_enableRubEdge.AutoSize = true;
            this.check_enableRubEdge.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureParserEnableRubEdges", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_enableRubEdge.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.check_enableRubEdge.Location = new System.Drawing.Point(222, 40);
            this.check_enableRubEdge.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.check_enableRubEdge.Name = "check_enableRubEdge";
            this.check_enableRubEdge.Size = new System.Drawing.Size(159, 36);
            this.check_enableRubEdge.TabIndex = 0;
            this.check_enableRubEdge.Text = "启用摩擦边";
            this.check_enableRubEdge.UseVisualStyleBackColor = true;
            // 
            // check_enableHotCorners
            // 
            this.check_enableHotCorners.AutoSize = true;
            this.check_enableHotCorners.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureParserEnableHotCorners", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.check_enableHotCorners.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.check_enableHotCorners.Location = new System.Drawing.Point(28, 40);
            this.check_enableHotCorners.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.check_enableHotCorners.Name = "check_enableHotCorners";
            this.check_enableHotCorners.Size = new System.Drawing.Size(159, 36);
            this.check_enableHotCorners.TabIndex = 0;
            this.check_enableHotCorners.Text = "启用触发角";
            this.check_enableHotCorners.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.flowLayoutPanel7);
            this.tabPage1.Controls.Add(this.tb_updateLog);
            this.tabPage1.Controls.Add(this.flowLayoutPanel5);
            this.tabPage1.Controls.Add(this.picture_logo);
            this.tabPage1.Location = new System.Drawing.Point(8, 36);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1100, 964);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Tag = "about";
            this.tabPage1.Text = "关 于";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel7
            // 
            this.flowLayoutPanel7.Controls.Add(this.label12);
            this.flowLayoutPanel7.Controls.Add(this.label10);
            this.flowLayoutPanel7.Controls.Add(this.picture_alipayCode);
            this.flowLayoutPanel7.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel7.Location = new System.Drawing.Point(254, 372);
            this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel7.Name = "flowLayoutPanel7";
            this.flowLayoutPanel7.Size = new System.Drawing.Size(820, 588);
            this.flowLayoutPanel7.TabIndex = 7;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(4, 4);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(763, 31);
            this.label12.TabIndex = 6;
            this.label12.Text = "若WGestures对您有用，可以考虑捐助支持该项目，以帮助我做得更好";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.ForeColor = System.Drawing.Color.OrangeRed;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(4, 43);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(182, 31);
            this.label10.TabIndex = 6;
            this.label10.Text = "支付宝钱包扫码";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picture_alipayCode
            // 
            this.picture_alipayCode.Image = global::WGestures.App.Properties.Resources.alipay;
            this.picture_alipayCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picture_alipayCode.Location = new System.Drawing.Point(4, 82);
            this.picture_alipayCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picture_alipayCode.Name = "picture_alipayCode";
            this.picture_alipayCode.Size = new System.Drawing.Size(446, 436);
            this.picture_alipayCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_alipayCode.TabIndex = 4;
            this.picture_alipayCode.TabStop = false;
            // 
            // tb_updateLog
            // 
            this.tb_updateLog.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tb_updateLog.Location = new System.Drawing.Point(254, 44);
            this.tb_updateLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tb_updateLog.Multiline = true;
            this.tb_updateLog.Name = "tb_updateLog";
            this.tb_updateLog.ReadOnly = true;
            this.tb_updateLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_updateLog.Size = new System.Drawing.Size(820, 304);
            this.tb_updateLog.TabIndex = 3;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.AutoSize = true;
            this.flowLayoutPanel5.Controls.Add(this.linkLabel1);
            this.flowLayoutPanel5.Controls.Add(this.linkLabel2);
            this.flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(20, 252);
            this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(220, 164);
            this.flowLayoutPanel5.TabIndex = 2;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.linkLabel1.LinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabel1.Location = new System.Drawing.Point(4, 0);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 16);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(110, 31);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "项目主页";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.linkLabel2.LinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabel2.Location = new System.Drawing.Point(4, 47);
            this.linkLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 16);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(110, 31);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "作者邮箱";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // picture_logo
            // 
            this.picture_logo.Image = global::WGestures.App.Properties.Resources._128;
            this.picture_logo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picture_logo.Location = new System.Drawing.Point(20, 44);
            this.picture_logo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picture_logo.Name = "picture_logo";
            this.picture_logo.Size = new System.Drawing.Size(212, 196);
            this.picture_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_logo.TabIndex = 0;
            this.picture_logo.TabStop = false;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.lb_info);
            this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(20, 1044);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 24, 0, 20);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(1116, 32);
            this.flowLayoutPanel4.TabIndex = 10;
            // 
            // lb_info
            // 
            this.lb_info.AutoSize = true;
            this.lb_info.ForeColor = System.Drawing.Color.Gray;
            this.lb_info.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lb_info.Location = new System.Drawing.Point(799, 0);
            this.lb_info.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_info.Name = "lb_info";
            this.lb_info.Size = new System.Drawing.Size(313, 31);
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
            this.pic_menuBtn.BackColor = System.Drawing.Color.Transparent;
            this.pic_menuBtn.Image = global::WGestures.App.Properties.Resources.menuBtn;
            this.pic_menuBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pic_menuBtn.Location = new System.Drawing.Point(1084, 20);
            this.pic_menuBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pic_menuBtn.Name = "pic_menuBtn";
            this.pic_menuBtn.Size = new System.Drawing.Size(48, 48);
            this.pic_menuBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic_menuBtn.TabIndex = 11;
            this.pic_menuBtn.TabStop = false;
            this.tip.SetToolTip(this.pic_menuBtn, "菜单");
            this.pic_menuBtn.Click += new System.EventHandler(this.pic_menuBtn_Click);
            this.pic_menuBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_menuBtn_MouseDown);
            // 
            // ctx_gesturesMenu
            // 
            this.ctx_gesturesMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ctx_gesturesMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_import,
            this.menuItem_export,
            this.menuItem_resetGestures});
            this.ctx_gesturesMenu.Name = "contextMenuStrip1";
            this.ctx_gesturesMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ctx_gesturesMenu.Size = new System.Drawing.Size(228, 118);
            this.ctx_gesturesMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.ctx_gesturesMenu_Closed);
            // 
            // menuItem_import
            // 
            this.menuItem_import.Name = "menuItem_import";
            this.menuItem_import.Size = new System.Drawing.Size(227, 38);
            this.menuItem_import.Text = "导入...";
            this.menuItem_import.Click += new System.EventHandler(this.menuItem_imxport_Click);
            // 
            // menuItem_export
            // 
            this.menuItem_export.Name = "menuItem_export";
            this.menuItem_export.Size = new System.Drawing.Size(227, 38);
            this.menuItem_export.Text = "导出...";
            this.menuItem_export.Click += new System.EventHandler(this.menuItem_export_Click);
            // 
            // menuItem_resetGestures
            // 
            this.menuItem_resetGestures.Name = "menuItem_resetGestures";
            this.menuItem_resetGestures.Size = new System.Drawing.Size(227, 38);
            this.menuItem_resetGestures.Text = "恢复默认...";
            this.menuItem_resetGestures.Click += new System.EventHandler(this.menuItem_resetGestures_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkRate = 300;
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerEnableWinKeyGesturing", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox1.Location = new System.Drawing.Point(600, 111);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(414, 35);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Tag = "12";
            this.checkBox1.Text = "启用Windows键触发 (等价于右键)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // settingsFormControllerBindingSource
            // 
            this.settingsFormControllerBindingSource.DataSource = typeof(WGestures.App.Gui.Windows.SettingsFormController);
            // 
            // lineLabel2
            // 
            this.lineLabel2.BackColor = System.Drawing.Color.Transparent;
            this.lineLabel2.CausesValidation = false;
            this.lineLabel2.ForeColor = System.Drawing.Color.Gainsboro;
            this.lineLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lineLabel2.IsVertical = true;
            this.lineLabel2.Location = new System.Drawing.Point(534, 60);
            this.lineLabel2.Margin = new System.Windows.Forms.Padding(8);
            this.lineLabel2.Name = "lineLabel2";
            this.lineLabel2.Size = new System.Drawing.Size(36, 218);
            this.lineLabel2.TabIndex = 13;
            // 
            // num_pathTrackerInitialStayTimeoutMillis
            // 
            this.num_pathTrackerInitialStayTimeoutMillis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.num_pathTrackerInitialStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerInitalStayTimeoutMillis", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.num_pathTrackerInitialStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.settingsFormControllerBindingSource, "PathTrackerInitialStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.num_pathTrackerInitialStayTimeoutMillis.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_pathTrackerInitialStayTimeoutMillis.Location = new System.Drawing.Point(418, 114);
            this.num_pathTrackerInitialStayTimeoutMillis.Margin = new System.Windows.Forms.Padding(8);
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
            this.num_pathTrackerInitialStayTimeoutMillis.Size = new System.Drawing.Size(100, 39);
            this.num_pathTrackerInitialStayTimeoutMillis.TabIndex = 11;
            this.tip.SetToolTip(this.num_pathTrackerInitialStayTimeoutMillis, "若按下右键后超过此时间未移动，则执行正常右键拖拽操作");
            this.num_pathTrackerInitialStayTimeoutMillis.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // colorMiddle
            // 
            this.colorMiddle.BackColor = System.Drawing.Color.White;
            this.colorMiddle.Color = System.Drawing.Color.YellowGreen;
            this.colorMiddle.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewMiddleBtnMainColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.colorMiddle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colorMiddle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorMiddle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colorMiddle.Location = new System.Drawing.Point(304, 436);
            this.colorMiddle.Margin = new System.Windows.Forms.Padding(8);
            this.colorMiddle.Name = "colorMiddle";
            this.colorMiddle.Size = new System.Drawing.Size(120, 70);
            this.colorMiddle.TabIndex = 8;
            this.colorMiddle.Text = "中键";
            this.colorMiddle.UseVisualStyleBackColor = false;
            // 
            // colorBtn_recogonized
            // 
            this.colorBtn_recogonized.BackColor = System.Drawing.Color.White;
            this.colorBtn_recogonized.Color = System.Drawing.Color.MediumTurquoise;
            this.colorBtn_recogonized.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewMainPathColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.colorBtn_recogonized.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colorBtn_recogonized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorBtn_recogonized.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colorBtn_recogonized.Location = new System.Drawing.Point(168, 436);
            this.colorBtn_recogonized.Margin = new System.Windows.Forms.Padding(8);
            this.colorBtn_recogonized.Name = "colorBtn_recogonized";
            this.colorBtn_recogonized.Size = new System.Drawing.Size(120, 70);
            this.colorBtn_recogonized.TabIndex = 8;
            this.colorBtn_recogonized.Text = "右键";
            this.tip.SetToolTip(this.colorBtn_recogonized, "手势被识别时，轨迹的颜色");
            this.colorBtn_recogonized.UseVisualStyleBackColor = false;
            // 
            // colorBtn_x
            // 
            this.colorBtn_x.BackColor = System.Drawing.Color.White;
            this.colorBtn_x.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.colorBtn_x.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureVieXBtnMainColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.colorBtn_x.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colorBtn_x.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorBtn_x.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colorBtn_x.Location = new System.Drawing.Point(440, 436);
            this.colorBtn_x.Margin = new System.Windows.Forms.Padding(8);
            this.colorBtn_x.Name = "colorBtn_x";
            this.colorBtn_x.Size = new System.Drawing.Size(120, 70);
            this.colorBtn_x.TabIndex = 8;
            this.colorBtn_x.Text = "X键";
            this.colorBtn_x.UseVisualStyleBackColor = false;
            // 
            // colorBtn_unrecogonized
            // 
            this.colorBtn_unrecogonized.BackColor = System.Drawing.Color.White;
            this.colorBtn_unrecogonized.Color = System.Drawing.Color.Gray;
            this.colorBtn_unrecogonized.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewAlternativePathColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.colorBtn_unrecogonized.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colorBtn_unrecogonized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorBtn_unrecogonized.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colorBtn_unrecogonized.Location = new System.Drawing.Point(588, 436);
            this.colorBtn_unrecogonized.Margin = new System.Windows.Forms.Padding(8);
            this.colorBtn_unrecogonized.Name = "colorBtn_unrecogonized";
            this.colorBtn_unrecogonized.Size = new System.Drawing.Size(120, 70);
            this.colorBtn_unrecogonized.TabIndex = 8;
            this.colorBtn_unrecogonized.Text = "未识别";
            this.tip.SetToolTip(this.colorBtn_unrecogonized, "手势未被识别时，轨迹的颜色");
            this.colorBtn_unrecogonized.UseVisualStyleBackColor = false;
            // 
            // numPathTrackerStayTimeoutMillis
            // 
            this.numPathTrackerStayTimeoutMillis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPathTrackerStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerStayTimeoutMillis", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numPathTrackerStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.settingsFormControllerBindingSource, "PathTrackerStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numPathTrackerStayTimeoutMillis.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPathTrackerStayTimeoutMillis.Location = new System.Drawing.Point(418, 174);
            this.numPathTrackerStayTimeoutMillis.Margin = new System.Windows.Forms.Padding(8);
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
            this.numPathTrackerStayTimeoutMillis.Size = new System.Drawing.Size(100, 39);
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
            this.numPathTrackerInitialValidMove.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPathTrackerInitialValidMove.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerInitialValidMove", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numPathTrackerInitialValidMove.Location = new System.Drawing.Point(418, 60);
            this.numPathTrackerInitialValidMove.Margin = new System.Windows.Forms.Padding(8);
            this.numPathTrackerInitialValidMove.Name = "numPathTrackerInitialValidMove";
            this.numPathTrackerInitialValidMove.Size = new System.Drawing.Size(100, 39);
            this.numPathTrackerInitialValidMove.TabIndex = 7;
            this.tip.SetToolTip(this.numPathTrackerInitialValidMove, "只有移动超过此距离，才开始识别手势");
            this.numPathTrackerInitialValidMove.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // shortcutRec_pause
            // 
            this.shortcutRec_pause.Location = new System.Drawing.Point(286, 174);
            this.shortcutRec_pause.Margin = new System.Windows.Forms.Padding(8);
            this.shortcutRec_pause.Name = "shortcutRec_pause";
            this.shortcutRec_pause.Size = new System.Drawing.Size(176, 46);
            this.shortcutRec_pause.TabIndex = 6;
            this.shortcutRec_pause.Text = "录入快捷键";
            this.shortcutRec_pause.UseVisualStyleBackColor = true;
            this.shortcutRec_pause.EndRecord += new System.EventHandler<WGestures.App.Gui.Windows.Controls.ShortcutRecordButton.ShortcutRecordEventArgs>(this.shortcutRec_pause_EndRecord);
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
            this.listGestureIntents.Font = new System.Drawing.Font("微软雅黑", 8.25F);
            this.listGestureIntents.FullRowSelect = true;
            this.listGestureIntents.GridLines = true;
            this.listGestureIntents.HideSelection = false;
            this.listGestureIntents.InsertionLineColor = System.Drawing.Color.DeepSkyBlue;
            this.listGestureIntents.LabelEdit = true;
            this.listGestureIntents.Location = new System.Drawing.Point(20, 47);
            this.listGestureIntents.Margin = new System.Windows.Forms.Padding(20, 4, 4, 4);
            this.listGestureIntents.MultiSelect = false;
            this.listGestureIntents.Name = "listGestureIntents";
            this.listGestureIntents.Size = new System.Drawing.Size(678, 326);
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
            // btn_RemoveGesture
            // 
            this.btn_RemoveGesture.Enabled = false;
            this.btn_RemoveGesture.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            this.btn_RemoveGesture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_RemoveGesture.Image = global::WGestures.App.Properties.Resources.remove;
            this.btn_RemoveGesture.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_RemoveGesture.Location = new System.Drawing.Point(56, 0);
            this.btn_RemoveGesture.Margin = new System.Windows.Forms.Padding(0);
            this.btn_RemoveGesture.Name = "btn_RemoveGesture";
            this.btn_RemoveGesture.Size = new System.Drawing.Size(60, 40);
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
            this.btn_modifyGesture.Location = new System.Drawing.Point(618, 0);
            this.btn_modifyGesture.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btn_modifyGesture.Name = "btn_modifyGesture";
            this.btn_modifyGesture.Size = new System.Drawing.Size(60, 40);
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
            this.btnAddGesture.Size = new System.Drawing.Size(60, 40);
            this.btnAddGesture.TabIndex = 8;
            this.tip.SetToolTip(this.btnAddGesture, "添加手势");
            this.btnAddGesture.UseVisualStyleBackColor = true;
            this.btnAddGesture.Click += new System.EventHandler(this.btnAddGesture_Click);
            // 
            // lineLabel1
            // 
            this.lineLabel1.ForeColor = System.Drawing.Color.Gainsboro;
            this.lineLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lineLabel1.IsVertical = false;
            this.lineLabel1.Location = new System.Drawing.Point(4, 108);
            this.lineLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lineLabel1.Name = "lineLabel1";
            this.lineLabel1.Size = new System.Drawing.Size(660, 12);
            this.lineLabel1.TabIndex = 3;
            // 
            // btnEditApp
            // 
            this.btnEditApp.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold);
            this.btnEditApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEditApp.Image = global::WGestures.App.Properties.Resources.Edit;
            this.btnEditApp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEditApp.Location = new System.Drawing.Point(304, 886);
            this.btnEditApp.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnEditApp.Name = "btnEditApp";
            this.btnEditApp.Size = new System.Drawing.Size(60, 40);
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
            this.btnAppRemove.Location = new System.Drawing.Point(84, 886);
            this.btnAppRemove.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnAppRemove.Name = "btnAppRemove";
            this.btnAppRemove.Size = new System.Drawing.Size(60, 40);
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
            this.btnAddApp.Location = new System.Drawing.Point(28, 886);
            this.btnAddApp.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnAddApp.Name = "btnAddApp";
            this.btnAddApp.Size = new System.Drawing.Size(60, 40);
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
            this.listApps.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.listApps.FullRowSelect = true;
            this.listApps.GridLines = true;
            this.listApps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listApps.HideSelection = false;
            this.listApps.InsertionLineColor = System.Drawing.Color.DeepSkyBlue;
            this.listApps.LabelWrap = false;
            this.listApps.Location = new System.Drawing.Point(28, 40);
            this.listApps.Margin = new System.Windows.Forms.Padding(4);
            this.listApps.MultiSelect = false;
            this.listApps.Name = "listApps";
            this.listApps.Size = new System.Drawing.Size(334, 838);
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
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1156, 1106);
            this.Controls.Add(this.pic_menuBtn);
            this.Controls.Add(this.flowLayoutPanel4);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.panel_hotcornerSettings.ResumeLayout(false);
            this.panel_hotcornerSettings.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.flowLayoutPanel7.ResumeLayout(false);
            this.flowLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_alipayCode)).EndInit();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_logo)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_menuBtn)).EndInit();
            this.ctx_gesturesMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsFormControllerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_pathTrackerInitialStayTimeoutMillis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPathTrackerStayTimeoutMillis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPathTrackerInitialValidMove)).EndInit();
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
        private Label label3;
        private TabPage tab_hotCorners;
        private CheckBox check_enableHotCorners;
        private Panel panel2;
        private CheckBox check_enable8DirGesture;
        private CheckBox check_preferCursorWindow;
        private ToolStripMenuItem menuItem_resetGestures;
        private Panel panel_cornorCmdView;
        private RadioButton radio_corner_2;
        private RadioButton radio_corner_3;
        private RadioButton radio_corner_0;
        private RadioButton radio_corner_1;
        private ComboBox combo_hotcornerCmdTypes;
        private Label label11;
        private Panel panel_hotcornerSettings;
        private FlowLayoutPanel flowLayoutPanel7;
        private RadioButton radio_edge_3;
        private RadioButton radio_edge_0;
        private RadioButton radio_edge_1;
        private RadioButton radio_edge_2;
        private CheckBox check_enableRubEdge;
        private Label label13;
        private Label label14;
        private ShortcutRecordButton shortcutRec_pause;
        private Label lb_pause_shortcut;
        private Label label15;
        private CheckBox check_gestBtn_X;
        private CheckBox check_gestBtn_Middle;
        private CheckBox check_gestBtn_Right;
        private ColorButton colorBtn_x;
        private CheckBox checkBox1;
    }
}