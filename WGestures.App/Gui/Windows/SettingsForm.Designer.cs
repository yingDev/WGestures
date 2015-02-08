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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_general = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lineLabel2 = new WGestures.App.Gui.Windows.Controls.LineLabel();
            this.check_disableOnFullscreen = new System.Windows.Forms.CheckBox();
            this.settingsFormControllerBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
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
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Controls.Add(this.tabPage_general);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage1);
            this.errorProvider.SetError(this.tabControl, resources.GetString("tabControl.Error"));
            this.errorProvider.SetIconAlignment(this.tabControl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabControl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabControl, ((int)(resources.GetObject("tabControl.IconPadding"))));
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tip.SetToolTip(this.tabControl, resources.GetString("tabControl.ToolTip"));
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPage_general
            // 
            resources.ApplyResources(this.tabPage_general, "tabPage_general");
            this.tabPage_general.BackColor = System.Drawing.Color.White;
            this.tabPage_general.Controls.Add(this.groupBox2);
            this.tabPage_general.Controls.Add(this.groupBox1);
            this.errorProvider.SetError(this.tabPage_general, resources.GetString("tabPage_general.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage_general, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage_general.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage_general, ((int)(resources.GetObject("tabPage_general.IconPadding"))));
            this.tabPage_general.Name = "tabPage_general";
            this.tabPage_general.Tag = "general";
            this.tip.SetToolTip(this.tabPage_general, resources.GetString("tabPage_general.ToolTip"));
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
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
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.tip.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            this.tip.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // lineLabel2
            // 
            resources.ApplyResources(this.lineLabel2, "lineLabel2");
            this.errorProvider.SetError(this.lineLabel2, resources.GetString("lineLabel2.Error"));
            this.lineLabel2.ForeColor = System.Drawing.Color.Gainsboro;
            this.errorProvider.SetIconAlignment(this.lineLabel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lineLabel2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.lineLabel2, ((int)(resources.GetObject("lineLabel2.IconPadding"))));
            this.lineLabel2.IsVertical = true;
            this.lineLabel2.Name = "lineLabel2";
            this.tip.SetToolTip(this.lineLabel2, resources.GetString("lineLabel2.ToolTip"));
            // 
            // check_disableOnFullscreen
            // 
            resources.ApplyResources(this.check_disableOnFullscreen, "check_disableOnFullscreen");
            this.check_disableOnFullscreen.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureParserDisableInFullScreenMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.check_disableOnFullscreen, resources.GetString("check_disableOnFullscreen.Error"));
            this.errorProvider.SetIconAlignment(this.check_disableOnFullscreen, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("check_disableOnFullscreen.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.check_disableOnFullscreen, ((int)(resources.GetObject("check_disableOnFullscreen.IconPadding"))));
            this.check_disableOnFullscreen.Name = "check_disableOnFullscreen";
            this.tip.SetToolTip(this.check_disableOnFullscreen, resources.GetString("check_disableOnFullscreen.ToolTip"));
            this.check_disableOnFullscreen.UseVisualStyleBackColor = true;
            // 
            // settingsFormControllerBindingSource
            // 
            this.settingsFormControllerBindingSource.DataSource = typeof(WGestures.App.Gui.Windows.SettingsFormController);
            // 
            // num_pathTrackerInitialStayTimeoutMillis
            // 
            resources.ApplyResources(this.num_pathTrackerInitialStayTimeoutMillis, "num_pathTrackerInitialStayTimeoutMillis");
            this.num_pathTrackerInitialStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerInitalStayTimeoutMillis", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.num_pathTrackerInitialStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.settingsFormControllerBindingSource, "PathTrackerInitialStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.num_pathTrackerInitialStayTimeoutMillis, resources.GetString("num_pathTrackerInitialStayTimeoutMillis.Error"));
            this.errorProvider.SetIconAlignment(this.num_pathTrackerInitialStayTimeoutMillis, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("num_pathTrackerInitialStayTimeoutMillis.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.num_pathTrackerInitialStayTimeoutMillis, ((int)(resources.GetObject("num_pathTrackerInitialStayTimeoutMillis.IconPadding"))));
            this.num_pathTrackerInitialStayTimeoutMillis.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
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
            this.tip.SetToolTip(this.num_pathTrackerInitialStayTimeoutMillis, resources.GetString("num_pathTrackerInitialStayTimeoutMillis.ToolTip"));
            this.num_pathTrackerInitialStayTimeoutMillis.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // check_pathTrackerInitialStayTimeout
            // 
            resources.ApplyResources(this.check_pathTrackerInitialStayTimeout, "check_pathTrackerInitialStayTimeout");
            this.check_pathTrackerInitialStayTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerInitialStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.check_pathTrackerInitialStayTimeout, resources.GetString("check_pathTrackerInitialStayTimeout.Error"));
            this.errorProvider.SetIconAlignment(this.check_pathTrackerInitialStayTimeout, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("check_pathTrackerInitialStayTimeout.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.check_pathTrackerInitialStayTimeout, ((int)(resources.GetObject("check_pathTrackerInitialStayTimeout.IconPadding"))));
            this.check_pathTrackerInitialStayTimeout.Name = "check_pathTrackerInitialStayTimeout";
            this.tip.SetToolTip(this.check_pathTrackerInitialStayTimeout, resources.GetString("check_pathTrackerInitialStayTimeout.ToolTip"));
            this.check_pathTrackerInitialStayTimeout.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.errorProvider.SetError(this.label9, resources.GetString("label9.Error"));
            this.errorProvider.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            this.tip.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
            // 
            // colorMiddle
            // 
            resources.ApplyResources(this.colorMiddle, "colorMiddle");
            this.colorMiddle.BackColor = System.Drawing.Color.White;
            this.colorMiddle.Color = System.Drawing.Color.YellowGreen;
            this.colorMiddle.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewMiddleBtnMainColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.colorMiddle, resources.GetString("colorMiddle.Error"));
            this.errorProvider.SetIconAlignment(this.colorMiddle, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("colorMiddle.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.colorMiddle, ((int)(resources.GetObject("colorMiddle.IconPadding"))));
            this.colorMiddle.Name = "colorMiddle";
            this.tip.SetToolTip(this.colorMiddle, resources.GetString("colorMiddle.ToolTip"));
            this.colorMiddle.UseVisualStyleBackColor = false;
            // 
            // colorBtn_recogonized
            // 
            resources.ApplyResources(this.colorBtn_recogonized, "colorBtn_recogonized");
            this.colorBtn_recogonized.BackColor = System.Drawing.Color.White;
            this.colorBtn_recogonized.Color = System.Drawing.Color.MediumTurquoise;
            this.colorBtn_recogonized.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewMainPathColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.colorBtn_recogonized, resources.GetString("colorBtn_recogonized.Error"));
            this.errorProvider.SetIconAlignment(this.colorBtn_recogonized, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("colorBtn_recogonized.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.colorBtn_recogonized, ((int)(resources.GetObject("colorBtn_recogonized.IconPadding"))));
            this.colorBtn_recogonized.Name = "colorBtn_recogonized";
            this.tip.SetToolTip(this.colorBtn_recogonized, resources.GetString("colorBtn_recogonized.ToolTip"));
            this.colorBtn_recogonized.UseVisualStyleBackColor = false;
            // 
            // colorBtn_unrecogonized
            // 
            resources.ApplyResources(this.colorBtn_unrecogonized, "colorBtn_unrecogonized");
            this.colorBtn_unrecogonized.BackColor = System.Drawing.Color.White;
            this.colorBtn_unrecogonized.Color = System.Drawing.Color.DeepPink;
            this.colorBtn_unrecogonized.DataBindings.Add(new System.Windows.Forms.Binding("Color", this.settingsFormControllerBindingSource, "GestureViewAlternativePathColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.colorBtn_unrecogonized, resources.GetString("colorBtn_unrecogonized.Error"));
            this.errorProvider.SetIconAlignment(this.colorBtn_unrecogonized, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("colorBtn_unrecogonized.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.colorBtn_unrecogonized, ((int)(resources.GetObject("colorBtn_unrecogonized.IconPadding"))));
            this.colorBtn_unrecogonized.Name = "colorBtn_unrecogonized";
            this.tip.SetToolTip(this.colorBtn_unrecogonized, resources.GetString("colorBtn_unrecogonized.ToolTip"));
            this.colorBtn_unrecogonized.UseVisualStyleBackColor = false;
            // 
            // numPathTrackerStayTimeoutMillis
            // 
            resources.ApplyResources(this.numPathTrackerStayTimeoutMillis, "numPathTrackerStayTimeoutMillis");
            this.numPathTrackerStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerStayTimeoutMillis", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numPathTrackerStayTimeoutMillis.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.settingsFormControllerBindingSource, "PathTrackerStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.numPathTrackerStayTimeoutMillis, resources.GetString("numPathTrackerStayTimeoutMillis.Error"));
            this.errorProvider.SetIconAlignment(this.numPathTrackerStayTimeoutMillis, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("numPathTrackerStayTimeoutMillis.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.numPathTrackerStayTimeoutMillis, ((int)(resources.GetObject("numPathTrackerStayTimeoutMillis.IconPadding"))));
            this.numPathTrackerStayTimeoutMillis.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
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
            this.tip.SetToolTip(this.numPathTrackerStayTimeoutMillis, resources.GetString("numPathTrackerStayTimeoutMillis.ToolTip"));
            this.numPathTrackerStayTimeoutMillis.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // numPathTrackerInitialValidMove
            // 
            resources.ApplyResources(this.numPathTrackerInitialValidMove, "numPathTrackerInitialValidMove");
            this.numPathTrackerInitialValidMove.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsFormControllerBindingSource, "PathTrackerInitialValidMove", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.numPathTrackerInitialValidMove, resources.GetString("numPathTrackerInitialValidMove.Error"));
            this.errorProvider.SetIconAlignment(this.numPathTrackerInitialValidMove, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("numPathTrackerInitialValidMove.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.numPathTrackerInitialValidMove, ((int)(resources.GetObject("numPathTrackerInitialValidMove.IconPadding"))));
            this.numPathTrackerInitialValidMove.Name = "numPathTrackerInitialValidMove";
            this.tip.SetToolTip(this.numPathTrackerInitialValidMove, resources.GetString("numPathTrackerInitialValidMove.ToolTip"));
            this.numPathTrackerInitialValidMove.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // checkGestureView_fadeOut
            // 
            resources.ApplyResources(this.checkGestureView_fadeOut, "checkGestureView_fadeOut");
            this.checkGestureView_fadeOut.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewFadeOut", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.checkGestureView_fadeOut, resources.GetString("checkGestureView_fadeOut.Error"));
            this.errorProvider.SetIconAlignment(this.checkGestureView_fadeOut, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkGestureView_fadeOut.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.checkGestureView_fadeOut, ((int)(resources.GetObject("checkGestureView_fadeOut.IconPadding"))));
            this.checkGestureView_fadeOut.Name = "checkGestureView_fadeOut";
            this.tip.SetToolTip(this.checkGestureView_fadeOut, resources.GetString("checkGestureView_fadeOut.ToolTip"));
            this.checkGestureView_fadeOut.UseVisualStyleBackColor = true;
            // 
            // checkGestureViewShowCommandName
            // 
            resources.ApplyResources(this.checkGestureViewShowCommandName, "checkGestureViewShowCommandName");
            this.checkGestureViewShowCommandName.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewShowCommandName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.checkGestureViewShowCommandName, resources.GetString("checkGestureViewShowCommandName.Error"));
            this.errorProvider.SetIconAlignment(this.checkGestureViewShowCommandName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkGestureViewShowCommandName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.checkGestureViewShowCommandName, ((int)(resources.GetObject("checkGestureViewShowCommandName.IconPadding"))));
            this.checkGestureViewShowCommandName.Name = "checkGestureViewShowCommandName";
            this.tip.SetToolTip(this.checkGestureViewShowCommandName, resources.GetString("checkGestureViewShowCommandName.ToolTip"));
            this.checkGestureViewShowCommandName.UseVisualStyleBackColor = true;
            // 
            // checkPathTrackerStayTimeout
            // 
            resources.ApplyResources(this.checkPathTrackerStayTimeout, "checkPathTrackerStayTimeout");
            this.checkPathTrackerStayTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "PathTrackerStayTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.checkPathTrackerStayTimeout, resources.GetString("checkPathTrackerStayTimeout.Error"));
            this.errorProvider.SetIconAlignment(this.checkPathTrackerStayTimeout, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkPathTrackerStayTimeout.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.checkPathTrackerStayTimeout, ((int)(resources.GetObject("checkPathTrackerStayTimeout.IconPadding"))));
            this.checkPathTrackerStayTimeout.Name = "checkPathTrackerStayTimeout";
            this.tip.SetToolTip(this.checkPathTrackerStayTimeout, resources.GetString("checkPathTrackerStayTimeout.ToolTip"));
            this.checkPathTrackerStayTimeout.UseVisualStyleBackColor = true;
            // 
            // checkGestureViewShowPath
            // 
            resources.ApplyResources(this.checkGestureViewShowPath, "checkGestureViewShowPath");
            this.checkGestureViewShowPath.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "GestureViewShowPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.checkGestureViewShowPath, resources.GetString("checkGestureViewShowPath.Error"));
            this.errorProvider.SetIconAlignment(this.checkGestureViewShowPath, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkGestureViewShowPath.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.checkGestureViewShowPath, ((int)(resources.GetObject("checkGestureViewShowPath.IconPadding"))));
            this.checkGestureViewShowPath.Name = "checkGestureViewShowPath";
            this.tip.SetToolTip(this.checkGestureViewShowPath, resources.GetString("checkGestureViewShowPath.ToolTip"));
            this.checkGestureViewShowPath.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            this.tip.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            this.tip.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            this.tip.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.flowLayoutPanel3);
            this.groupBox1.Controls.Add(this.check_autoStart);
            this.groupBox1.Controls.Add(this.btn_checkUpdateNow);
            this.groupBox1.Controls.Add(this.check_autoCheckUpdate);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.tip.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // flowLayoutPanel3
            // 
            resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Controls.Add(this.lb_Version);
            this.errorProvider.SetError(this.flowLayoutPanel3, resources.GetString("flowLayoutPanel3.Error"));
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel3, ((int)(resources.GetObject("flowLayoutPanel3.IconPadding"))));
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.tip.SetToolTip(this.flowLayoutPanel3, resources.GetString("flowLayoutPanel3.ToolTip"));
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.label4.ForeColor = System.Drawing.Color.DarkGray;
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            this.tip.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // lb_Version
            // 
            resources.ApplyResources(this.lb_Version, "lb_Version");
            this.errorProvider.SetError(this.lb_Version, resources.GetString("lb_Version.Error"));
            this.lb_Version.ForeColor = System.Drawing.Color.DarkGray;
            this.errorProvider.SetIconAlignment(this.lb_Version, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lb_Version.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.lb_Version, ((int)(resources.GetObject("lb_Version.IconPadding"))));
            this.lb_Version.Name = "lb_Version";
            this.tip.SetToolTip(this.lb_Version, resources.GetString("lb_Version.ToolTip"));
            // 
            // check_autoStart
            // 
            resources.ApplyResources(this.check_autoStart, "check_autoStart");
            this.check_autoStart.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "AutoStart", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.check_autoStart, resources.GetString("check_autoStart.Error"));
            this.errorProvider.SetIconAlignment(this.check_autoStart, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("check_autoStart.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.check_autoStart, ((int)(resources.GetObject("check_autoStart.IconPadding"))));
            this.check_autoStart.Name = "check_autoStart";
            this.tip.SetToolTip(this.check_autoStart, resources.GetString("check_autoStart.ToolTip"));
            this.check_autoStart.UseVisualStyleBackColor = true;
            // 
            // btn_checkUpdateNow
            // 
            resources.ApplyResources(this.btn_checkUpdateNow, "btn_checkUpdateNow");
            this.errorProvider.SetError(this.btn_checkUpdateNow, resources.GetString("btn_checkUpdateNow.Error"));
            this.errorProvider.SetIconAlignment(this.btn_checkUpdateNow, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btn_checkUpdateNow.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.btn_checkUpdateNow, ((int)(resources.GetObject("btn_checkUpdateNow.IconPadding"))));
            this.btn_checkUpdateNow.Name = "btn_checkUpdateNow";
            this.tip.SetToolTip(this.btn_checkUpdateNow, resources.GetString("btn_checkUpdateNow.ToolTip"));
            this.btn_checkUpdateNow.UseVisualStyleBackColor = true;
            this.btn_checkUpdateNow.Click += new System.EventHandler(this.btn_checkUpdateNow_Click);
            // 
            // check_autoCheckUpdate
            // 
            resources.ApplyResources(this.check_autoCheckUpdate, "check_autoCheckUpdate");
            this.check_autoCheckUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsFormControllerBindingSource, "AutoCheckForUpdate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProvider.SetError(this.check_autoCheckUpdate, resources.GetString("check_autoCheckUpdate.Error"));
            this.errorProvider.SetIconAlignment(this.check_autoCheckUpdate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("check_autoCheckUpdate.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.check_autoCheckUpdate, ((int)(resources.GetObject("check_autoCheckUpdate.IconPadding"))));
            this.check_autoCheckUpdate.Name = "check_autoCheckUpdate";
            this.tip.SetToolTip(this.check_autoCheckUpdate, resources.GetString("check_autoCheckUpdate.ToolTip"));
            this.check_autoCheckUpdate.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.flowLayoutPanel2);
            this.tabPage2.Controls.Add(this.btnEditApp);
            this.tabPage2.Controls.Add(this.btnAppRemove);
            this.tabPage2.Controls.Add(this.btnAddApp);
            this.tabPage2.Controls.Add(this.listApps);
            this.errorProvider.SetError(this.tabPage2, resources.GetString("tabPage2.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage2, ((int)(resources.GetObject("tabPage2.IconPadding"))));
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Tag = "gestures";
            this.tip.SetToolTip(this.tabPage2, resources.GetString("tabPage2.ToolTip"));
            // 
            // flowLayoutPanel2
            // 
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.Controls.Add(this.panel1);
            this.flowLayoutPanel2.Controls.Add(this.listGestureIntents);
            this.flowLayoutPanel2.Controls.Add(this.panel_intentListOperations);
            this.flowLayoutPanel2.Controls.Add(this.group_Command);
            this.errorProvider.SetError(this.flowLayoutPanel2, resources.GetString("flowLayoutPanel2.Error"));
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel2, ((int)(resources.GetObject("flowLayoutPanel2.IconPadding"))));
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.tip.SetToolTip(this.flowLayoutPanel2, resources.GetString("flowLayoutPanel2.ToolTip"));
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.checkInheritGlobal);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.errorProvider.SetError(this.panel1, resources.GetString("panel1.Error"));
            this.errorProvider.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.panel1, ((int)(resources.GetObject("panel1.IconPadding"))));
            this.panel1.Name = "panel1";
            this.tip.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // checkInheritGlobal
            // 
            resources.ApplyResources(this.checkInheritGlobal, "checkInheritGlobal");
            this.errorProvider.SetError(this.checkInheritGlobal, resources.GetString("checkInheritGlobal.Error"));
            this.errorProvider.SetIconAlignment(this.checkInheritGlobal, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("checkInheritGlobal.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.checkInheritGlobal, ((int)(resources.GetObject("checkInheritGlobal.IconPadding"))));
            this.checkInheritGlobal.Name = "checkInheritGlobal";
            this.tip.SetToolTip(this.checkInheritGlobal, resources.GetString("checkInheritGlobal.ToolTip"));
            this.checkInheritGlobal.UseVisualStyleBackColor = true;
            this.checkInheritGlobal.CheckedChanged += new System.EventHandler(this.checkInheritGlobal_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.check_gesturingDisabled);
            this.flowLayoutPanel1.Controls.Add(this.pictureSelectedApp);
            this.flowLayoutPanel1.Controls.Add(this.labelAppName);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.errorProvider.SetError(this.flowLayoutPanel1, resources.GetString("flowLayoutPanel1.Error"));
            this.flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel1, ((int)(resources.GetObject("flowLayoutPanel1.IconPadding"))));
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.tip.SetToolTip(this.flowLayoutPanel1, resources.GetString("flowLayoutPanel1.ToolTip"));
            // 
            // check_gesturingDisabled
            // 
            resources.ApplyResources(this.check_gesturingDisabled, "check_gesturingDisabled");
            this.errorProvider.SetError(this.check_gesturingDisabled, resources.GetString("check_gesturingDisabled.Error"));
            this.check_gesturingDisabled.ForeColor = System.Drawing.Color.Black;
            this.errorProvider.SetIconAlignment(this.check_gesturingDisabled, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("check_gesturingDisabled.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.check_gesturingDisabled, ((int)(resources.GetObject("check_gesturingDisabled.IconPadding"))));
            this.check_gesturingDisabled.Name = "check_gesturingDisabled";
            this.tip.SetToolTip(this.check_gesturingDisabled, resources.GetString("check_gesturingDisabled.ToolTip"));
            this.check_gesturingDisabled.UseVisualStyleBackColor = true;
            this.check_gesturingDisabled.CheckedChanged += new System.EventHandler(this.check_gesturingEnabled_CheckedChanged);
            // 
            // pictureSelectedApp
            // 
            resources.ApplyResources(this.pictureSelectedApp, "pictureSelectedApp");
            this.errorProvider.SetError(this.pictureSelectedApp, resources.GetString("pictureSelectedApp.Error"));
            this.errorProvider.SetIconAlignment(this.pictureSelectedApp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureSelectedApp.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.pictureSelectedApp, ((int)(resources.GetObject("pictureSelectedApp.IconPadding"))));
            this.pictureSelectedApp.Name = "pictureSelectedApp";
            this.pictureSelectedApp.TabStop = false;
            this.tip.SetToolTip(this.pictureSelectedApp, resources.GetString("pictureSelectedApp.ToolTip"));
            // 
            // labelAppName
            // 
            resources.ApplyResources(this.labelAppName, "labelAppName");
            this.labelAppName.AutoEllipsis = true;
            this.labelAppName.BackColor = System.Drawing.Color.White;
            this.errorProvider.SetError(this.labelAppName, resources.GetString("labelAppName.Error"));
            this.labelAppName.ForeColor = System.Drawing.Color.Black;
            this.errorProvider.SetIconAlignment(this.labelAppName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("labelAppName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.labelAppName, ((int)(resources.GetObject("labelAppName.IconPadding"))));
            this.labelAppName.Name = "labelAppName";
            this.tip.SetToolTip(this.labelAppName, resources.GetString("labelAppName.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.White;
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            this.tip.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // listGestureIntents
            // 
            resources.ApplyResources(this.listGestureIntents, "listGestureIntents");
            this.listGestureIntents.AllowDrop = true;
            this.listGestureIntents.AllowItemDrag = true;
            this.listGestureIntents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listGestureIntents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGestureName,
            this.colGestureDirs,
            this.operation});
            this.errorProvider.SetError(this.listGestureIntents, resources.GetString("listGestureIntents.Error"));
            this.listGestureIntents.FullRowSelect = true;
            this.listGestureIntents.GridLines = true;
            this.listGestureIntents.HideSelection = false;
            this.errorProvider.SetIconAlignment(this.listGestureIntents, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("listGestureIntents.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.listGestureIntents, ((int)(resources.GetObject("listGestureIntents.IconPadding"))));
            this.listGestureIntents.InsertionLineColor = System.Drawing.Color.DeepSkyBlue;
            this.listGestureIntents.LabelEdit = true;
            this.listGestureIntents.MultiSelect = false;
            this.listGestureIntents.Name = "listGestureIntents";
            this.listGestureIntents.SmallImageList = this.dummyImgLstForLstViewHeightFix;
            this.listGestureIntents.TileSize = new System.Drawing.Size(255, 84);
            this.tip.SetToolTip(this.listGestureIntents, resources.GetString("listGestureIntents.ToolTip"));
            this.listGestureIntents.UseCompatibleStateImageBehavior = false;
            this.listGestureIntents.View = System.Windows.Forms.View.Details;
            this.listGestureIntents.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listGestureIntents_AfterLabelEdit);
            this.listGestureIntents.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listGestureIntents_ItemSelectionChanged);
            // 
            // colGestureName
            // 
            resources.ApplyResources(this.colGestureName, "colGestureName");
            // 
            // colGestureDirs
            // 
            resources.ApplyResources(this.colGestureDirs, "colGestureDirs");
            // 
            // operation
            // 
            resources.ApplyResources(this.operation, "operation");
            // 
            // dummyImgLstForLstViewHeightFix
            // 
            this.dummyImgLstForLstViewHeightFix.ColorDepth = System.Windows.Forms.ColorDepth.Depth4Bit;
            resources.ApplyResources(this.dummyImgLstForLstViewHeightFix, "dummyImgLstForLstViewHeightFix");
            this.dummyImgLstForLstViewHeightFix.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel_intentListOperations
            // 
            resources.ApplyResources(this.panel_intentListOperations, "panel_intentListOperations");
            this.panel_intentListOperations.Controls.Add(this.btn_RemoveGesture);
            this.panel_intentListOperations.Controls.Add(this.btn_modifyGesture);
            this.panel_intentListOperations.Controls.Add(this.btnAddGesture);
            this.errorProvider.SetError(this.panel_intentListOperations, resources.GetString("panel_intentListOperations.Error"));
            this.errorProvider.SetIconAlignment(this.panel_intentListOperations, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel_intentListOperations.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.panel_intentListOperations, ((int)(resources.GetObject("panel_intentListOperations.IconPadding"))));
            this.panel_intentListOperations.Name = "panel_intentListOperations";
            this.tip.SetToolTip(this.panel_intentListOperations, resources.GetString("panel_intentListOperations.ToolTip"));
            // 
            // btn_RemoveGesture
            // 
            resources.ApplyResources(this.btn_RemoveGesture, "btn_RemoveGesture");
            this.errorProvider.SetError(this.btn_RemoveGesture, resources.GetString("btn_RemoveGesture.Error"));
            this.btn_RemoveGesture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.errorProvider.SetIconAlignment(this.btn_RemoveGesture, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btn_RemoveGesture.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.btn_RemoveGesture, ((int)(resources.GetObject("btn_RemoveGesture.IconPadding"))));
            this.btn_RemoveGesture.Image = global::WGestures.App.Properties.Resources.remove;
            this.btn_RemoveGesture.Name = "btn_RemoveGesture";
            this.tip.SetToolTip(this.btn_RemoveGesture, resources.GetString("btn_RemoveGesture.ToolTip"));
            this.btn_RemoveGesture.UseVisualStyleBackColor = true;
            this.btn_RemoveGesture.Click += new System.EventHandler(this.btnRemoveGesture_Click);
            // 
            // btn_modifyGesture
            // 
            resources.ApplyResources(this.btn_modifyGesture, "btn_modifyGesture");
            this.errorProvider.SetError(this.btn_modifyGesture, resources.GetString("btn_modifyGesture.Error"));
            this.btn_modifyGesture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.errorProvider.SetIconAlignment(this.btn_modifyGesture, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btn_modifyGesture.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.btn_modifyGesture, ((int)(resources.GetObject("btn_modifyGesture.IconPadding"))));
            this.btn_modifyGesture.Image = global::WGestures.App.Properties.Resources.Edit;
            this.btn_modifyGesture.Name = "btn_modifyGesture";
            this.tip.SetToolTip(this.btn_modifyGesture, resources.GetString("btn_modifyGesture.ToolTip"));
            this.btn_modifyGesture.UseVisualStyleBackColor = true;
            this.btn_modifyGesture.Click += new System.EventHandler(this.btn_modifyGesture_Click);
            // 
            // btnAddGesture
            // 
            resources.ApplyResources(this.btnAddGesture, "btnAddGesture");
            this.errorProvider.SetError(this.btnAddGesture, resources.GetString("btnAddGesture.Error"));
            this.btnAddGesture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.errorProvider.SetIconAlignment(this.btnAddGesture, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnAddGesture.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.btnAddGesture, ((int)(resources.GetObject("btnAddGesture.IconPadding"))));
            this.btnAddGesture.Image = global::WGestures.App.Properties.Resources.add;
            this.btnAddGesture.Name = "btnAddGesture";
            this.tip.SetToolTip(this.btnAddGesture, resources.GetString("btnAddGesture.ToolTip"));
            this.btnAddGesture.UseVisualStyleBackColor = true;
            this.btnAddGesture.Click += new System.EventHandler(this.btnAddGesture_Click);
            // 
            // group_Command
            // 
            resources.ApplyResources(this.group_Command, "group_Command");
            this.group_Command.Controls.Add(this.flowLayoutPanel6);
            this.errorProvider.SetError(this.group_Command, resources.GetString("group_Command.Error"));
            this.errorProvider.SetIconAlignment(this.group_Command, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("group_Command.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.group_Command, ((int)(resources.GetObject("group_Command.IconPadding"))));
            this.group_Command.Name = "group_Command";
            this.group_Command.TabStop = false;
            this.tip.SetToolTip(this.group_Command, resources.GetString("group_Command.ToolTip"));
            // 
            // flowLayoutPanel6
            // 
            resources.ApplyResources(this.flowLayoutPanel6, "flowLayoutPanel6");
            this.flowLayoutPanel6.Controls.Add(this.panel3);
            this.flowLayoutPanel6.Controls.Add(this.check_executeOnMouseWheeling);
            this.flowLayoutPanel6.Controls.Add(this.lineLabel1);
            this.flowLayoutPanel6.Controls.Add(this.panel_commandView);
            this.errorProvider.SetError(this.flowLayoutPanel6, resources.GetString("flowLayoutPanel6.Error"));
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel6, ((int)(resources.GetObject("flowLayoutPanel6.IconPadding"))));
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.tip.SetToolTip(this.flowLayoutPanel6, resources.GetString("flowLayoutPanel6.ToolTip"));
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.combo_CommandTypes);
            this.errorProvider.SetError(this.panel3, resources.GetString("panel3.Error"));
            this.errorProvider.SetIconAlignment(this.panel3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.panel3, ((int)(resources.GetObject("panel3.IconPadding"))));
            this.panel3.Name = "panel3";
            this.tip.SetToolTip(this.panel3, resources.GetString("panel3.ToolTip"));
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            this.tip.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
            // 
            // combo_CommandTypes
            // 
            resources.ApplyResources(this.combo_CommandTypes, "combo_CommandTypes");
            this.combo_CommandTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.combo_CommandTypes, resources.GetString("combo_CommandTypes.Error"));
            this.combo_CommandTypes.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.combo_CommandTypes, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("combo_CommandTypes.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.combo_CommandTypes, ((int)(resources.GetObject("combo_CommandTypes.IconPadding"))));
            this.combo_CommandTypes.Name = "combo_CommandTypes";
            this.tip.SetToolTip(this.combo_CommandTypes, resources.GetString("combo_CommandTypes.ToolTip"));
            this.combo_CommandTypes.SelectedIndexChanged += new System.EventHandler(this.combo_CommandTypes_SelectedIndexChanged);
            // 
            // check_executeOnMouseWheeling
            // 
            resources.ApplyResources(this.check_executeOnMouseWheeling, "check_executeOnMouseWheeling");
            this.errorProvider.SetError(this.check_executeOnMouseWheeling, resources.GetString("check_executeOnMouseWheeling.Error"));
            this.errorProvider.SetIconAlignment(this.check_executeOnMouseWheeling, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("check_executeOnMouseWheeling.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.check_executeOnMouseWheeling, ((int)(resources.GetObject("check_executeOnMouseWheeling.IconPadding"))));
            this.check_executeOnMouseWheeling.Name = "check_executeOnMouseWheeling";
            this.tip.SetToolTip(this.check_executeOnMouseWheeling, resources.GetString("check_executeOnMouseWheeling.ToolTip"));
            this.check_executeOnMouseWheeling.UseVisualStyleBackColor = true;
            this.check_executeOnMouseWheeling.CheckedChanged += new System.EventHandler(this.check_executeOnMouseWheeling_CheckedChanged);
            // 
            // lineLabel1
            // 
            resources.ApplyResources(this.lineLabel1, "lineLabel1");
            this.errorProvider.SetError(this.lineLabel1, resources.GetString("lineLabel1.Error"));
            this.lineLabel1.ForeColor = System.Drawing.Color.Gainsboro;
            this.errorProvider.SetIconAlignment(this.lineLabel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lineLabel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.lineLabel1, ((int)(resources.GetObject("lineLabel1.IconPadding"))));
            this.lineLabel1.IsVertical = false;
            this.lineLabel1.Name = "lineLabel1";
            this.tip.SetToolTip(this.lineLabel1, resources.GetString("lineLabel1.ToolTip"));
            // 
            // panel_commandView
            // 
            resources.ApplyResources(this.panel_commandView, "panel_commandView");
            this.panel_commandView.BackColor = System.Drawing.Color.Transparent;
            this.errorProvider.SetError(this.panel_commandView, resources.GetString("panel_commandView.Error"));
            this.errorProvider.SetIconAlignment(this.panel_commandView, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel_commandView.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.panel_commandView, ((int)(resources.GetObject("panel_commandView.IconPadding"))));
            this.panel_commandView.Name = "panel_commandView";
            this.tip.SetToolTip(this.panel_commandView, resources.GetString("panel_commandView.ToolTip"));
            // 
            // btnEditApp
            // 
            resources.ApplyResources(this.btnEditApp, "btnEditApp");
            this.errorProvider.SetError(this.btnEditApp, resources.GetString("btnEditApp.Error"));
            this.btnEditApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.errorProvider.SetIconAlignment(this.btnEditApp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnEditApp.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.btnEditApp, ((int)(resources.GetObject("btnEditApp.IconPadding"))));
            this.btnEditApp.Image = global::WGestures.App.Properties.Resources.Edit;
            this.btnEditApp.Name = "btnEditApp";
            this.tip.SetToolTip(this.btnEditApp, resources.GetString("btnEditApp.ToolTip"));
            this.btnEditApp.UseVisualStyleBackColor = true;
            this.btnEditApp.Click += new System.EventHandler(this.btnEditApp_Click);
            // 
            // btnAppRemove
            // 
            resources.ApplyResources(this.btnAppRemove, "btnAppRemove");
            this.errorProvider.SetError(this.btnAppRemove, resources.GetString("btnAppRemove.Error"));
            this.btnAppRemove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.errorProvider.SetIconAlignment(this.btnAppRemove, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnAppRemove.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.btnAppRemove, ((int)(resources.GetObject("btnAppRemove.IconPadding"))));
            this.btnAppRemove.Image = global::WGestures.App.Properties.Resources.remove;
            this.btnAppRemove.Name = "btnAppRemove";
            this.tip.SetToolTip(this.btnAppRemove, resources.GetString("btnAppRemove.ToolTip"));
            this.btnAppRemove.UseVisualStyleBackColor = true;
            this.btnAppRemove.Click += new System.EventHandler(this.btnAppRemove_Click);
            // 
            // btnAddApp
            // 
            resources.ApplyResources(this.btnAddApp, "btnAddApp");
            this.errorProvider.SetError(this.btnAddApp, resources.GetString("btnAddApp.Error"));
            this.btnAddApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.errorProvider.SetIconAlignment(this.btnAddApp, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnAddApp.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.btnAddApp, ((int)(resources.GetObject("btnAddApp.IconPadding"))));
            this.btnAddApp.Image = global::WGestures.App.Properties.Resources.add;
            this.btnAddApp.Name = "btnAddApp";
            this.tip.SetToolTip(this.btnAddApp, resources.GetString("btnAddApp.ToolTip"));
            this.btnAddApp.UseVisualStyleBackColor = true;
            this.btnAddApp.Click += new System.EventHandler(this.btnAddApp_Click);
            // 
            // listApps
            // 
            resources.ApplyResources(this.listApps, "listApps");
            this.listApps.AllowDrop = true;
            this.listApps.AllowItemDrag = true;
            this.listApps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listApps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colListAppDummy});
            this.errorProvider.SetError(this.listApps, resources.GetString("listApps.Error"));
            this.listApps.FullRowSelect = true;
            this.listApps.GridLines = true;
            this.listApps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listApps.HideSelection = false;
            this.errorProvider.SetIconAlignment(this.listApps, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("listApps.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.listApps, ((int)(resources.GetObject("listApps.IconPadding"))));
            this.listApps.InsertionLineColor = System.Drawing.Color.DeepSkyBlue;
            this.listApps.MultiSelect = false;
            this.listApps.Name = "listApps";
            this.listApps.SmallImageList = this.imglistAppIcons;
            this.listApps.TileSize = new System.Drawing.Size(160, 42);
            this.tip.SetToolTip(this.listApps, resources.GetString("listApps.ToolTip"));
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
            resources.ApplyResources(this.colListAppDummy, "colListAppDummy");
            // 
            // imglistAppIcons
            // 
            this.imglistAppIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imglistAppIcons, "imglistAppIcons");
            this.imglistAppIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.picture_alipayCode);
            this.tabPage1.Controls.Add(this.tb_updateLog);
            this.tabPage1.Controls.Add(this.flowLayoutPanel5);
            this.tabPage1.Controls.Add(this.picture_logo);
            this.errorProvider.SetError(this.tabPage1, resources.GetString("tabPage1.Error"));
            this.errorProvider.SetIconAlignment(this.tabPage1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tabPage1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tabPage1, ((int)(resources.GetObject("tabPage1.IconPadding"))));
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Tag = "about";
            this.tip.SetToolTip(this.tabPage1, resources.GetString("tabPage1.ToolTip"));
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.errorProvider.SetError(this.label10, resources.GetString("label10.Error"));
            this.label10.ForeColor = System.Drawing.Color.OrangeRed;
            this.errorProvider.SetIconAlignment(this.label10, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label10.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label10, ((int)(resources.GetObject("label10.IconPadding"))));
            this.label10.Name = "label10";
            this.tip.SetToolTip(this.label10, resources.GetString("label10.ToolTip"));
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.errorProvider.SetError(this.label12, resources.GetString("label12.Error"));
            this.errorProvider.SetIconAlignment(this.label12, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label12.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label12, ((int)(resources.GetObject("label12.IconPadding"))));
            this.label12.Name = "label12";
            this.tip.SetToolTip(this.label12, resources.GetString("label12.ToolTip"));
            // 
            // picture_alipayCode
            // 
            resources.ApplyResources(this.picture_alipayCode, "picture_alipayCode");
            this.errorProvider.SetError(this.picture_alipayCode, resources.GetString("picture_alipayCode.Error"));
            this.errorProvider.SetIconAlignment(this.picture_alipayCode, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("picture_alipayCode.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.picture_alipayCode, ((int)(resources.GetObject("picture_alipayCode.IconPadding"))));
            this.picture_alipayCode.Image = global::WGestures.App.Properties.Resources.alipay;
            this.picture_alipayCode.Name = "picture_alipayCode";
            this.picture_alipayCode.TabStop = false;
            this.tip.SetToolTip(this.picture_alipayCode, resources.GetString("picture_alipayCode.ToolTip"));
            // 
            // tb_updateLog
            // 
            resources.ApplyResources(this.tb_updateLog, "tb_updateLog");
            this.errorProvider.SetError(this.tb_updateLog, resources.GetString("tb_updateLog.Error"));
            this.errorProvider.SetIconAlignment(this.tb_updateLog, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tb_updateLog.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tb_updateLog, ((int)(resources.GetObject("tb_updateLog.IconPadding"))));
            this.tb_updateLog.Name = "tb_updateLog";
            this.tb_updateLog.ReadOnly = true;
            this.tip.SetToolTip(this.tb_updateLog, resources.GetString("tb_updateLog.ToolTip"));
            // 
            // flowLayoutPanel5
            // 
            resources.ApplyResources(this.flowLayoutPanel5, "flowLayoutPanel5");
            this.flowLayoutPanel5.Controls.Add(this.linkLabel1);
            this.flowLayoutPanel5.Controls.Add(this.linkLabel2);
            this.errorProvider.SetError(this.flowLayoutPanel5, resources.GetString("flowLayoutPanel5.Error"));
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel5, ((int)(resources.GetObject("flowLayoutPanel5.IconPadding"))));
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.tip.SetToolTip(this.flowLayoutPanel5, resources.GetString("flowLayoutPanel5.ToolTip"));
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.errorProvider.SetError(this.linkLabel1, resources.GetString("linkLabel1.Error"));
            this.errorProvider.SetIconAlignment(this.linkLabel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("linkLabel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.linkLabel1, ((int)(resources.GetObject("linkLabel1.IconPadding"))));
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.tip.SetToolTip(this.linkLabel1, resources.GetString("linkLabel1.ToolTip"));
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            resources.ApplyResources(this.linkLabel2, "linkLabel2");
            this.errorProvider.SetError(this.linkLabel2, resources.GetString("linkLabel2.Error"));
            this.errorProvider.SetIconAlignment(this.linkLabel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("linkLabel2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.linkLabel2, ((int)(resources.GetObject("linkLabel2.IconPadding"))));
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.TabStop = true;
            this.tip.SetToolTip(this.linkLabel2, resources.GetString("linkLabel2.ToolTip"));
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // picture_logo
            // 
            resources.ApplyResources(this.picture_logo, "picture_logo");
            this.errorProvider.SetError(this.picture_logo, resources.GetString("picture_logo.Error"));
            this.errorProvider.SetIconAlignment(this.picture_logo, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("picture_logo.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.picture_logo, ((int)(resources.GetObject("picture_logo.IconPadding"))));
            this.picture_logo.Image = global::WGestures.App.Properties.Resources._128;
            this.picture_logo.Name = "picture_logo";
            this.picture_logo.TabStop = false;
            this.tip.SetToolTip(this.picture_logo, resources.GetString("picture_logo.ToolTip"));
            // 
            // flowLayoutPanel4
            // 
            resources.ApplyResources(this.flowLayoutPanel4, "flowLayoutPanel4");
            this.flowLayoutPanel4.Controls.Add(this.lb_info);
            this.errorProvider.SetError(this.flowLayoutPanel4, resources.GetString("flowLayoutPanel4.Error"));
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel4, ((int)(resources.GetObject("flowLayoutPanel4.IconPadding"))));
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.tip.SetToolTip(this.flowLayoutPanel4, resources.GetString("flowLayoutPanel4.ToolTip"));
            // 
            // lb_info
            // 
            resources.ApplyResources(this.lb_info, "lb_info");
            this.errorProvider.SetError(this.lb_info, resources.GetString("lb_info.Error"));
            this.lb_info.ForeColor = System.Drawing.Color.DimGray;
            this.errorProvider.SetIconAlignment(this.lb_info, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lb_info.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.lb_info, ((int)(resources.GetObject("lb_info.IconPadding"))));
            this.lb_info.Name = "lb_info";
            this.tip.SetToolTip(this.lb_info, resources.GetString("lb_info.ToolTip"));
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
            resources.ApplyResources(this.pic_menuBtn, "pic_menuBtn");
            this.errorProvider.SetError(this.pic_menuBtn, resources.GetString("pic_menuBtn.Error"));
            this.errorProvider.SetIconAlignment(this.pic_menuBtn, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pic_menuBtn.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.pic_menuBtn, ((int)(resources.GetObject("pic_menuBtn.IconPadding"))));
            this.pic_menuBtn.Image = global::WGestures.App.Properties.Resources.menuBtn;
            this.pic_menuBtn.Name = "pic_menuBtn";
            this.pic_menuBtn.TabStop = false;
            this.tip.SetToolTip(this.pic_menuBtn, resources.GetString("pic_menuBtn.ToolTip"));
            this.pic_menuBtn.Click += new System.EventHandler(this.pic_menuBtn_Click);
            this.pic_menuBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic_menuBtn_MouseDown);
            // 
            // ctx_gesturesMenu
            // 
            resources.ApplyResources(this.ctx_gesturesMenu, "ctx_gesturesMenu");
            this.errorProvider.SetError(this.ctx_gesturesMenu, resources.GetString("ctx_gesturesMenu.Error"));
            this.errorProvider.SetIconAlignment(this.ctx_gesturesMenu, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ctx_gesturesMenu.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.ctx_gesturesMenu, ((int)(resources.GetObject("ctx_gesturesMenu.IconPadding"))));
            this.ctx_gesturesMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_import,
            this.menuItem_export});
            this.ctx_gesturesMenu.Name = "contextMenuStrip1";
            this.tip.SetToolTip(this.ctx_gesturesMenu, resources.GetString("ctx_gesturesMenu.ToolTip"));
            this.ctx_gesturesMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.ctx_gesturesMenu_Closed);
            // 
            // menuItem_import
            // 
            resources.ApplyResources(this.menuItem_import, "menuItem_import");
            this.menuItem_import.Name = "menuItem_import";
            this.menuItem_import.Click += new System.EventHandler(this.menuItem_imxport_Click);
            // 
            // menuItem_export
            // 
            resources.ApplyResources(this.menuItem_export, "menuItem_export");
            this.menuItem_export.Name = "menuItem_export";
            this.menuItem_export.Click += new System.EventHandler(this.menuItem_export_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            this.tip.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.comboBox1, resources.GetString("comboBox1.Error"));
            this.comboBox1.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.comboBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comboBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.comboBox1, ((int)(resources.GetObject("comboBox1.IconPadding"))));
            this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items"),
            resources.GetString("comboBox1.Items1")});
            this.comboBox1.Name = "comboBox1";
            this.tip.SetToolTip(this.comboBox1, resources.GetString("comboBox1.ToolTip"));
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkRate = 300;
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pic_menuBtn);
            this.Controls.Add(this.flowLayoutPanel4);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.tip.SetToolTip(this, resources.GetString("$this.ToolTip"));
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
            this.PerformLayout();

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
        private ComboBox comboBox1;
    }
}