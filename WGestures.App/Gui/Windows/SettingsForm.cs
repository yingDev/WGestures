using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WGestures.App.Gui.Model;
using WGestures.App.Gui.Windows.CommandViews;
using WGestures.App.Properties;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Common.Product;
using WGestures.Core;
using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;
using WGestures.Core.Persistence;

namespace WGestures.App.Gui.Windows
{
    internal partial class SettingsForm : Form
    {
        private readonly float _dpiF = Native.GetScreenDpi() / 96f;

        private VersionChecker _versionChecker;

        public SettingsFormController Controller { get; set; }

        public SettingsForm(SettingsFormController controller)
        {
            Controller = controller;

            InitializeComponent();
            Icon = Resources.icon;

            SuspendDrawingControl.SuspendDrawing(this);

            settingsFormControllerBindingSource.Add(Controller);

            DpiFix();
            ControlFixes();

            InitControlValues();
            SuspendDrawingControl.SuspendDrawing(this);
        }

        private void DpiFix()
        {
            tabControl.ItemSize = new Size((int)(tabControl.ItemSize.Width * _dpiF), (int)(tabControl.ItemSize.Height * _dpiF));
            //tabControl.Height -= tabControl.ItemSize.Height;

            //listApps.TileSize = new Size((int)(listApps.TileSize.Width * _dpiF), (int)(listApps.TileSize.Height * _dpiF));
            //listApps.Width = (int) (listApps.Width*_dpiF);

            //listApps.Padding = new Padding(0,0,SystemInformation.VerticalScrollBarWidth+100,0);

            imglistAppIcons.ImageSize = new Size((int)(imglistAppIcons.ImageSize.Width * _dpiF), (int)(imglistAppIcons.ImageSize.Height * _dpiF));
            dummyImgLstForLstViewHeightFix.ImageSize = new Size(1, (int)(dummyImgLstForLstViewHeightFix.ImageSize.Height * _dpiF));

            //listGestureIntents.TileSize = new Size((int)((listGestureIntents.TileSize.Width - 2) * _dpiF), (int)(listGestureIntents.TileSize.Height * _dpiF));


            //Height += tabControl.ItemSize.Height;
        }

        private void ControlFixes()
        {
            //disable combo_commandtypes scrolling
            combo_CommandTypes.MouseWheel += (sender, args) =>
            {
                ((HandledMouseEventArgs)args).Handled = true;
            };
        }

        private void InitControlValues()
        {
            #region tab1
            lb_Version.Text = Application.ProductVersion;

            var rightButtonText = Native.IsMouseButtonSwapped() ? "左键" : "右键";
            var middleButtonText = "中键";
            combo_GestureTriggerButton.Items.AddRange(new object[]
            {
                rightButtonText,middleButtonText, rightButtonText + "和" + middleButtonText
            
            });
            #endregion

            #region tab2
            imglistAppIcons.Images.Add("icon", Resources.icon);
            imglistAppIcons.Images.Add("icon_bw", Resources.icon_bw);
            imglistAppIcons.Images.Add("unknown", Resources.unknown);

            #endregion

            #region tab about
            tb_updateLog.Text = Application.ProductName + " " + Application.ProductVersion + Environment.NewLine + Environment.NewLine;
            tb_updateLog.Text += File.ReadAllText(Path.GetDirectoryName(Application.ExecutablePath) + @"\UpdateLog.txt");
            #endregion
        }

        #region tab "General" event handlers
        private void btn_checkUpdateNow_Click(object sender, EventArgs e)
        {
            if (_versionChecker != null && _versionChecker.IsBusy) return;
            //btn_checkUpdateNow.Enabled = false;
            btn_checkUpdateNow.Text = "";
            btn_checkUpdateNow.Image = Resources.checking;
            //picture_checking.Visible = true;
            lb_Version.Text = Application.ProductVersion;

            errorProvider.Clear();


            if (_versionChecker == null)
            {

                _versionChecker = new VersionChecker(AppSettings.CheckForUpdateUrl);

                _versionChecker.Finished += version =>
                {
                    if (btn_checkUpdateNow.IsDisposed) return;

                    btn_checkUpdateNow.Invoke(new Action(() =>
                    {
                        //btn_checkUpdateNow.Enabled = true;
                        btn_checkUpdateNow.Text = "立即检查";
                        btn_checkUpdateNow.Image = null;
                        //picture_checking.Visible = false;
                        lb_Version.Text = Application.ProductVersion;
                        lb_Version.ForeColor = Color.DarkGray;

                        if (version == null)
                        {
                            errorProvider.SetIconPadding(btn_checkUpdateNow, 4);
                            errorProvider.SetError(btn_checkUpdateNow, "可能由于服务器错误，检查更新失败");
                            return;
                        }


                        if (version.Version != Application.ProductVersion)
                        {
                            using (var frm = new UpdateInfoForm(AppSettings.ProductHomePage, version))
                            {
                                frm.ShowDialog();
                            }
                        }
                        else
                        {
                            lb_Version.Text = Application.ProductVersion + " (已为最新)";
                            lb_Version.ForeColor = Color.CornflowerBlue;

                        }

                    }));
                };

                _versionChecker.ErrorHappened += exception =>
                {
#if DEBUG
                    Console.WriteLine(exception);
#endif

                    btn_checkUpdateNow.Invoke(new Action(() =>
                    {
                        //btn_checkUpdateNow.Enabled = true;
                        btn_checkUpdateNow.Text = "立即检查";
                        btn_checkUpdateNow.Image = null;
                        //picture_checking.Visible = false;


                        errorProvider.SetIconPadding(btn_checkUpdateNow, 4);
                        errorProvider.SetError(btn_checkUpdateNow, string.Format("可能由于网络错误，检查更新失败: \n{0}", exception.Message));
                    }));
                };

            }


            _versionChecker.CheckAsync();

        }

        #endregion

        #region tab "Gestures" event handlers
        private void listApps_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) return;
            
            Debug.WriteLine("listApps_ItemSelectionChanged");

            labelAppName.Text = e.Item.Text.Trim();
            var app = e.Item.Tag as AbstractApp;

            //if (app is GlobalApp) check_gesturingEnabled.Text = "启用全局手势";
            //else check_gesturingEnabled.Text = "在此程序上启用手势";

            
            var exeApp = app as ExeApp;
            if (exeApp != null)
            {
                checkInheritGlobal.Checked = exeApp.InheritGlobalGestures;
                checkInheritGlobal.Visible = true;

            }
            else
            {
                checkInheritGlobal.Visible = false;
            }

            //阻止引发事件，只有用户点击“启用”的时候才发布事件
            check_gesturingDisabled.CheckedChanged -= check_gesturingEnabled_CheckedChanged;
            check_gesturingDisabled.Checked = !app.IsGesturingEnabled;
            check_gesturingDisabled.CheckedChanged += check_gesturingEnabled_CheckedChanged;

            pictureSelectedApp.Image = imglistAppIcons.Images[e.Item.ImageKey];

            //listGestureIntents.Enabled = app.IsGesturingEnabled;
            //panel_intentListOperations.Enabled = app.IsGesturingEnabled;

            btnAppRemove.Enabled = listApps.SelectedItems[0].Index != 0;
            btnEditApp.Enabled = btnAppRemove.Enabled;

            UnloadCommand();
            LoadGestureIntents(app);
        }


        private void listApps_DoubleClick(object sender, EventArgs e)
        {
            ToggleSelectedAppGesturingEnabled();
        }


        private void check_gesturingEnabled_CheckedChanged(object sender, EventArgs e)
        {
           ToggleSelectedAppGesturingEnabled();
        }

        private void checkInheritGlobal_CheckedChanged(object sender, EventArgs e)
        {
            var app = GetSelectedAppOrGlobal() as ExeApp;
            if (app != null)
            {
                app.InheritGlobalGestures = checkInheritGlobal.Checked;
            }

        }

        private void btnAddApp_Click(object sender, EventArgs e)
        {
            using (var frm = new EditAppForm(Controller.IntentStore))
            {
                var result = frm.ShowDialog();

                if (result != DialogResult.OK) return;

                var appPath = frm.AppPath;
                ExeApp found;

                if (Controller.IntentStore.TryGetExeApp(appPath, out found))
                {
                    HighlightAppInList(found);
                }
                else
                {
                    var app = new OrderableExeApp() { ExecutablePath = appPath, Name = frm.AppName, Order = 1}; 
                    
                    AddAppToList(app);
                    HighlightAppInList(app);
                    Controller.IntentStore.Add(app);
                }
            }
        }

        private void btnAppRemove_Click(object sender, EventArgs e)
        {
            var sels = listApps.SelectedItems;
            if (sels.Count != 1) return;


            sels[0].EnsureVisible();
            var item = (sels[0].Tag as ExeApp);


            var ret = MessageBox.Show(string.Format("您确定删除项目 \"{0}\" 及其所有手势吗?", item.Name), "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (ret == DialogResult.Yes)
            {
                Controller.IntentStore.Remove(item.ExecutablePath);
                RemoveSelectedAppFromList();
            }

        }

        private void btnEditApp_Click(object sender, EventArgs e)
        {
            EditSelectedApp();
        }

        //after in-place edit
        private void listGestureIntents_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
#if DEBUG
            Console.WriteLine("{0} - Edit Gesture Intent[{1}]:{2}", typeof(SettingsForm), e.Item, e.Label);
#endif
            if (e.Label == null) return;

            var value = e.Label.Trim();
            if (value.Length == 0)
            {
                e.CancelEdit = true;
                return;
            }

            if (value.Length > 16)
            {
                value = value.Substring(0, 16) + "...";
                e.CancelEdit = true;
            }

            var item = listGestureIntents.Items[e.Item];

            var intent = item.Tag as GestureIntent;
            item.Text = value;
            intent.Name = value;

            LoadCommand(intent);

            AdjustListGestureIntentsColumnSize();
        }

        //Add Gesture
        private void btnAddGesture_Click(object sender, EventArgs e)
        {
            var app = GetSelectedAppOrGlobal();
            if (app == null) return;

            using (var addGestureForm = new EditGestureForm(Controller.GestureParser))
            {

                var ok = addGestureForm.ShowDialog();
                if (ok == DialogResult.OK)
                {
                    var gesture = addGestureForm.CapturedGesture;
                    var name = addGestureForm.GestureName;

                    var gestureIntent = new GestureIntent() { Command = new HotKeyCommand(), Gesture = gesture, Name = name };
                    AddOrReplaceGestureIntent(gestureIntent);

                    AdjustListGestureIntentsColumnSize();

                    //add to model
                    app.GestureIntents.AddOrReplace(gestureIntent);
                }
            }
        }

        private void btnRemoveGesture_Click(object sender, EventArgs e)
        {
            var sels = listGestureIntents.SelectedItems;
            if (sels.Count != 1) return;

            var item = sels[0];
            item.EnsureVisible();

            var ret = MessageBox.Show(string.Format("您确定删除手势 \"{0}\" 吗?", item.Text), "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (ret == DialogResult.Yes)
            {
                var selIndex = item.Index;
                listGestureIntents.Items.Remove(item);

                var app = GetSelectedAppOrGlobal();
                app.GestureIntents.Remove(item.Tag as GestureIntent);

                if (listGestureIntents.Items.Count >= 1)
                {
                    listGestureIntents.Items[selIndex == 0 ? selIndex : selIndex - 1].Selected = true;
                }
                else
                {
                    UnloadCommand();
                }
            }
        }

        private void listGestureIntents_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            var sel = listGestureIntents.SelectedItems;
            if (sel.Count == 0)
            {
                btn_RemoveGesture.Enabled = false;
                btn_modifyGesture.Enabled = false;
                return;
            }

            Debug.WriteLine("listGestureIntents_ItemSelectionChanged");

            var intent = sel[0].Tag as GestureIntent;

            btn_RemoveGesture.Enabled = true;
            btn_modifyGesture.Enabled = true;

            LoadCommand(intent);
        }

        private void combo_CommandTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel_commandView.Controls.Clear();

            if (combo_CommandTypes.SelectedIndex == -1) return;
            var intent = GetSelectedGestureIntent();
            if (intent == null) return;

            var selected = combo_CommandTypes.SelectedItem as CommandTypesComboBoxItem;
            if (selected == null) return;


            CommandViewUserControl cmdView;
            //如果这个成立，说明是用户主动选择了不同的命令类型。
            //否则，选择的改变应该是受listGestureIntents的影响。
            //故而不是构造新的命令，而是把既有的传过去
            if (intent.Command.GetType() != selected.CommandType)
            {
                var cmd = Activator.CreateInstance(selected.CommandType) as AbstractCommand;
                //更新选中的intent的command
                intent.Command = cmd;

                //更新描述
                var itemInGestureIntentList = listGestureIntents.SelectedItems[0];
                itemInGestureIntentList.SubItems[2].Text = intent.Command.Description();
            }

            LoadCommandView(intent);

        }


        private void btn_modifyGesture_Click(object sender, EventArgs e)
        {
            var app = GetSelectedAppOrGlobal();
            var intent = GetSelectedGestureIntent();

            using (var editFrm = new EditGestureForm(Controller.GestureParser, app, intent))
            {
                var result = editFrm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var found = app.Find(editFrm.CapturedGesture);
                    if (found != null && found != intent)
                    {
                        foreach (ListViewItem item in listGestureIntents.Items)
                        {
                            if (item.Tag == found)
                            {
                                item.Remove();
                            }
                        }
                        app.Remove(found);
                    }

                    app.Remove(intent); //因为app内部是按gesture为key存储的， 无法单独修改key

                    intent.Gesture = editFrm.CapturedGesture;
                    intent.Name = editFrm.GestureName;
                    app.Add(intent);

                    var editingItem = listGestureIntents.SelectedItem;
                    editingItem.Text = intent.Name;
                    editingItem.SubItems[1].Text = intent.Gesture.ToString();
                    editingItem.EnsureVisible();
                    listGestureIntents.Focus();
                }
            }

            //listGestureIntents.SelectedItems[0].BeginEdit();
        }

        private void check_executeOnMouseWheeling_CheckedChanged(object sender, EventArgs e)
        {
            var intent = GetSelectedGestureIntent();
            intent.ExecuteOnModifier = check_executeOnMouseWheeling.Checked;
        }

        #endregion


        private void CommandValueChangedHandler(AbstractCommand command)
        {
#if DEBUG
            Console.WriteLine("CommandValueChanged");
#endif
            var item = listGestureIntents.SelectedItems[0];
            item.SubItems[2].Text = command.Description();
        }

        #region internal
        private void LoadApps()
        {
            //如果有不是OrderableExeApp的，则转换并替换
            var appsArray = Controller.IntentStore.ToArray();
            for(var i=0; i<appsArray.Length; i++)
            {
                var app = appsArray[i];
                if (!(app is OrderableExeApp))
                {
                    appsArray[i] = new OrderableExeApp(app);
                    Controller.IntentStore.Remove(app);
                    Controller.IntentStore.Add(appsArray[i]);
                }
            }

            Array.Sort(appsArray, (a, b) =>  ((OrderableExeApp) a).Order.CompareTo(((OrderableExeApp) b).Order));


            listApps.BeginUpdate();

            ResetListApps();

            foreach (var app in appsArray)
            {
                AddAppToList(app);
            }

            listApps.Items[0].Selected = true;

            listApps.EndUpdate();

            AdjustListAppsColumnWidth();
        }

        private void AddAppToList(ExeApp app)
        {
            var item = new ListViewItem(app.Name);
            var appPath = app.ExecutablePath;

            item.Tag = app;

            //get icon
            using (var thumb = IconHelper.ExtractIconForPath(appPath, new Size((int) (32*_dpiF), (int) (32*_dpiF)), app.IsGesturingEnabled))
            {
                
                var imgIndex = imglistAppIcons.Images.IndexOfKey(appPath);
                if (imgIndex < 0)
                {
                    imglistAppIcons.Images.Add(appPath, thumb);
                }
                else
                {
                    imglistAppIcons.Images[imgIndex] = IconHelper.ExtractIconForPath(appPath, new Size((int)(32 * _dpiF), (int)(32 * _dpiF)), app.IsGesturingEnabled);

                }

            }

            item.ImageKey = appPath;
            if (!app.IsGesturingEnabled) item.ForeColor = Color.Firebrick;

            if (!File.Exists(appPath))
            {
                item.Text += "(不存在)";
                item.ForeColor = Color.Gray;
            }

            listApps.Items.Add(item);
        }

        private void EditSelectedApp()
        {
            if (listApps.SelectedIndices.Count != 1 || listApps.SelectedIndices[0] == 0)
            {
                return;
            }

            var sel = listApps.SelectedItems[0];
            var found = (OrderableExeApp) Controller.IntentStore.GetExeApp((listApps.SelectedItems[0].Tag as ExeApp).ExecutablePath);

            using (var frm = new EditAppForm(found, Controller.IntentStore))
            {
                var ret = frm.ShowDialog();

                if (ret == DialogResult.OK)
                {
                    if (frm.AppPath.Equals(found.ExecutablePath))
                    {
                        found.Name = frm.AppName;

                        sel.Text = found.Name;

                        if (!found.AppExists()) sel.Text += "(不存在)";

                        labelAppName.Text = sel.Text;
                    }
                    else
                    {
                        ExeApp existed;
                        if (Controller.IntentStore.TryGetExeApp(frm.AppPath, out existed))
                        {
                            HighlightAppInList(existed);
                        }
                        else
                        {
                            RemoveSelectedAppFromList();

                            Controller.IntentStore.Remove(found.ExecutablePath);

                            found.ExecutablePath = frm.AppPath;
                            found.Name = frm.AppName;

                            Controller.IntentStore.Add(found);

                            AddAppToList(found);
                        }


                    }
                }


            }
        }
        
        private void RemoveSelectedAppFromList()
        {
            if (listApps.SelectedIndices.Count != 1) return;
            if (listApps.SelectedItems[0].Index == 0) return;

            var selIndex = listApps.SelectedItems[0].Index;

            var sel = listApps.SelectedItems[0];
            //imglistAppIcons.Images.RemoveByKey(sel.ImageKey);

            listApps.BeginUpdate();

            listApps.Items.Remove(sel);
            listApps.Items[selIndex - 1].Selected = true;

            listApps.EndUpdate();

        }

        private void AdjustListAppsColumnWidth()
        {
            if (GetVisibleScrollbars(listApps) == ScrollBars.Vertical)
            {
                colListAppDummy.Width = listApps.Width - SystemInformation.VerticalScrollBarWidth - 5;
            }
            else
            {
                colListAppDummy.Width = listApps.Width - 5;
            }

        }

        private void ToggleSelectedAppGesturingEnabled()
        {
            var app = GetSelectedAppOrGlobal();
            if (app == null) return;

            app.IsGesturingEnabled = !app.IsGesturingEnabled;
            var item = listApps.SelectedItems[0];

            var exeApp = app as ExeApp;
            if (exeApp == null) //global
            {
                item.ImageKey = app.IsGesturingEnabled ? "icon" : "icon_bw";
                item.ForeColor = app.IsGesturingEnabled ? Color.DodgerBlue : Color.Firebrick;
            }
            else //exeApp
            {
                var imgIndex = imglistAppIcons.Images.IndexOfKey(exeApp.ExecutablePath);
                imglistAppIcons.Images[imgIndex] = IconHelper.ExtractIconForPath(exeApp.ExecutablePath, new Size((int)(32 * _dpiF), (int)(32 * _dpiF)), exeApp.IsGesturingEnabled);

                item.ForeColor = exeApp.IsGesturingEnabled ? Color.Black : Color.Firebrick;
                //item.Group = listApps.Groups[exeApp.IsGesturingEnabled ? "enabled" : "disabled"];
            }

            //更新“启用作用于xxx”中的小图片
            pictureSelectedApp.Image = imglistAppIcons.Images[item.ImageKey];


            //阻止引发事件，只有用户点击“启用”的时候才发布事件
            check_gesturingDisabled.CheckedChanged -= check_gesturingEnabled_CheckedChanged;
            check_gesturingDisabled.Checked = !app.IsGesturingEnabled;
            check_gesturingDisabled.CheckedChanged += check_gesturingEnabled_CheckedChanged;


//            if (!listGestureIntents.Enabled && listGestureIntents.SelectedItems.Count == 1)
//            {
//                listGestureIntents.SelectedItems[0].Selected = false;
//                UnloadCommand();
//            }
        }

        private void ResetListApps()
        {
            listApps.BeginUpdate();
            
            listApps.Items.Clear();

            //第0个项目，必须是(全局)
            var globalAppItem = new ListViewItem("(全局)");//listApps.Items[0];
            globalAppItem.ImageKey = Controller.IntentStore.GlobalApp.IsGesturingEnabled ? "icon" : "icon_bw";
            globalAppItem.ForeColor = Controller.IntentStore.GlobalApp.IsGesturingEnabled ? Color.DodgerBlue : Color.Firebrick;
            globalAppItem.Tag = Controller.IntentStore.GlobalApp;
            listApps.Items.Add(globalAppItem);

            listApps.Items[0].Selected = true;
            listApps.EndUpdate();
        }


        private void LoadGestureIntents(AbstractApp app)
        {
            //禁用删除和修改按钮
            btn_modifyGesture.Enabled = false;
            btn_RemoveGesture.Enabled = false;

            listGestureIntents.BeginUpdate();

            listGestureIntents.Items.Clear();

            if (app.GestureIntents.Count == 0)
            {

                AdjustListGestureIntentsColumnSize();

                listGestureIntents.EndUpdate();
                return;
            }

            foreach (var gest in app.GestureIntents)
            {
                AddGestureIntent(gest.Value);
            }
            Debug.WriteLine("LoadGestureIntents");
            //listGestureIntents.Items[0].Selected = true;
            AdjustListGestureIntentsColumnSize();

            listGestureIntents.EndUpdate();

        }

        private ListViewItem AddGestureIntent(GestureIntent intent)
        {
            var item = new ListViewItem(intent.Name) { Tag = intent };

            var mnemonic = intent.Gesture.ToString();

            item.SubItems.Add(mnemonic);
            item.SubItems.Add(intent.Command.Description());

            listGestureIntents.Items.Add(item);

            return item;
        }

        private void HighlightAppInList(ExeApp app)
        {
            ListViewItem ensureVisiblItem = null;

            foreach (ListViewItem item in listApps.Items)
            {
                if (item.Index != 0 && app.ExecutablePath.Equals((item.Tag as ExeApp).ExecutablePath))
                {
                    item.Selected = true;
                    ensureVisiblItem = item;
                    break;
                }
            }

            listApps.Focus();
            if (ensureVisiblItem != null) listApps.EnsureVisible(ensureVisiblItem.Index);
        }

        private AbstractApp GetSelectedAppOrGlobal()
        {
            return listApps.SelectedItems[0].Tag as AbstractApp;
        }

        private GestureIntent GetSelectedGestureIntent()
        {
            var sels = listGestureIntents.SelectedItems;
            if (sels.Count != 1) return null;

            return sels[0].Tag as GestureIntent;
        }

        private void AddOrReplaceGestureIntent(GestureIntent intent)
        {
            listGestureIntents.BeginUpdate();

            foreach (ListViewItem item in listGestureIntents.Items)
            {
                var current = item.Tag as GestureIntent;
                if (current.Gesture.Equals(intent.Gesture))
                {
                    listGestureIntents.Items.Remove(item);
                    break;
                }
            }

            var i = AddGestureIntent(intent);
            i.Selected = true;
            listGestureIntents.Focus();
            listGestureIntents.EnsureVisible(i.Index);

            listGestureIntents.EndUpdate();
        }

        private void AdjustListGestureIntentsColumnSize()
        {
            SuspendDrawingControl.SuspendDrawing(listGestureIntents);
            listGestureIntents.SuspendLayout();

            listGestureIntents.Columns[0].Width = -2;
            listGestureIntents.Columns[1].Width = -2;
            listGestureIntents.Columns[2].Width = -2;

            listGestureIntents.ResumeLayout();
            SuspendDrawingControl.ResumeDrawing(listGestureIntents);
        }



        private void LoadCommand(GestureIntent intent)
        {
            //SuspendDrawingControl.SuspendDrawing(group_Command);

            if (!group_Command.Enabled) group_Command.Enabled = true;
            if (group_Command.Text != intent.Name) group_Command.Text = string.Format("手势 \"{0}\" 的参数", intent.Name);

            //根据intent的Gesture是否包WheelDirection来决定是显示“滚动滚轮时立即执行”选框
            //check_executeOnMouseWheeling.Text = "识别手势时立即执行";

            check_executeOnMouseWheeling.Visible = (intent.Gesture.Modifier != GestureModifier.None);
            if (check_executeOnMouseWheeling.Visible) //设定其值
            {
                check_executeOnMouseWheeling.Checked = intent.ExecuteOnModifier;
            }

            //修改下拉框
            bool itemMatched = false;
            foreach (CommandTypesComboBoxItem item in combo_CommandTypes.Items)
            {
                if (item.CommandType == intent.Command.GetType())
                {
                    //如果与前一个command相同，则不会引发changed事件，需要手动修改。
                    if (combo_CommandTypes.SelectedItem == item)
                    {
                        LoadCommandView(intent);
                    }

                    combo_CommandTypes.SelectedItem = item;
                    itemMatched = true;

                    break;
                }
            }
            if (!itemMatched) combo_CommandTypes.SelectedIndex = -1;

            //SuspendDrawingControl.ResumeDrawing(group_Command);

        }

        private void UnloadCommand()
        {
            //SuspendDrawingControl.SuspendDrawing(group_Command);
            if (!group_Command.Enabled) return;
            group_Command.Enabled = false;
            group_Command.Text = "手势的参数";
            combo_CommandTypes.SelectedIndex = -1;

            check_executeOnMouseWheeling.Visible = false;
            //SuspendDrawingControl.ResumeDrawing(group_Command);
        }

        private void LoadCommandTypes()
        {
            combo_CommandTypes.BeginUpdate();
            foreach (var kv in Controller.SupportedCommands)
            {
                combo_CommandTypes.Items.Add(new CommandTypesComboBoxItem() { Name = kv.Key, CommandType = kv.Value });
            }
            combo_CommandTypes.EndUpdate();
        }

        private void LoadCommandView(GestureIntent intent)
        {
            var cmdView = Controller.CommandViewFactory.GetCommandView(intent.Command);


            if (cmdView != null)
            {
                //如果目标视图实现了该接口，则注入选中的app
                var view = cmdView as ITargetAppAware;
                if (view != null)
                {
                    view.TargetApp = GetSelectedAppOrGlobal();
                }

                cmdView.CommandValueChanged -= CommandValueChangedHandler;
                cmdView.CommandValueChanged += CommandValueChangedHandler;

                //如果就是先前那个，则不用清除再加载了
                if (!panel_commandView.Controls.Contains(cmdView))
                {
                    panel_commandView.Controls.Clear();
                    panel_commandView.Controls.Add(cmdView);
                }
            }
        }

        #endregion

        #region utils


        private static ScrollBars GetVisibleScrollbars(Control ctl)
        {
            var wndStyle = Native.GetWindowLong(ctl.Handle, Native.GWL_STYLE);
            var hsVisible = (wndStyle & Native.WS_HSCROLL) != 0;
            var vsVisible = (wndStyle & Native.WS_VSCROLL) != 0;

            if (hsVisible)
                return vsVisible ? ScrollBars.Both : ScrollBars.Horizontal;
            else
                return vsVisible ? ScrollBars.Vertical : ScrollBars.None;
        }

       
        #endregion


        //处理快捷键
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.W))
            {
                Close();
                return true;
            }
            else if (keyData == (Keys.Delete) || keyData == Keys.Back)
            {
                if (listApps.Focused && btnAppRemove.Enabled) btnAppRemove.PerformClick();
                else if (listGestureIntents.Focused && btn_RemoveGesture.Enabled) btn_RemoveGesture.PerformClick();
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            lb_info.Text = Equals(tabControl.SelectedTab.Tag, "about") ? "Copyright (c) " + DateTime.Now.Year+" 应元东" : "*改动将自动保存并立即生效";

            //初次选中“手势”tab的时候，选中app
            var isGesturesTab = object.Equals(tabControl.SelectedTab.Tag, "gestures");

            if (isGesturesTab && listApps.SelectedItems.Count == 0)
            {
                //第0个项目，必须是(全局)
                var globalAppItem = new ListViewItem("(全局)");//listApps.Items[0];
                globalAppItem.ImageKey = Controller.IntentStore.GlobalApp.IsGesturingEnabled ? "icon" : "icon_bw";
                globalAppItem.ForeColor = Controller.IntentStore.GlobalApp.IsGesturingEnabled ? Color.DodgerBlue : Color.Firebrick;
                globalAppItem.Tag = Controller.IntentStore.GlobalApp;
                listApps.Items.Add(globalAppItem);

                LoadApps();

                LoadCommandTypes();
            }

        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            BringToFront();
            Activate();
        }


        #region Tab3
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(AppSettings.ProductHomePage);
        }

        #endregion

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://mail.qq.com/cgi-bin/qm_share?t=qm_mailme&email=HiYnKS0sKCguJ15vbzB9cXM");
        }

        #region Gesture tab Menu button
        private void pic_menuBtn_Click(object sender, EventArgs e)
        {
            ctx_gesturesMenu.Show(pic_menuBtn,0, pic_menuBtn.Height);
        }

        private void pic_menuBtn_MouseDown(object sender, MouseEventArgs e)
        {
            pic_menuBtn.Image = Properties.Resources.menuBtn_down;
        }

        private void ctx_gesturesMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            pic_menuBtn.Image = Properties.Resources.menuBtn;
        }

        private void menuItem_imxport_Click(object sender, EventArgs e)
        {
            var importForm = new ImportForm();
            importForm.Import += (o, args) =>
            {
                var importConfig = args.ConfigImportOption != ImportForm.ImportOption.None;
                var importGestures = args.GesturesImportOption != ImportForm.ImportOption.None;
                var mergeGestures = args.GesturesImportOption == ImportForm.ImportOption.Merge;

                //冻结绘图，以提升批量修改的性能
                SuspendDrawingControl.SuspendDrawing(this);
                try
                {
                    Controller.Import(args.ConfigAndGestures, importConfig, importGestures, mergeGestures);
                    //如果还没有切换到“手势”tab，则listApps没有app加载。
                    if(listApps.Items.Count > 0) LoadApps();
                }
                finally
                {
                    SuspendDrawingControl.ResumeDrawing(this);
                }

                //settingsFormControllerBindingSource.ResetBindings(true);
            };

            importForm.ShowDialog();
        }

        private void menuItem_export_Click(object sender, EventArgs e)
        {
            var selectSaveToPath = new SaveFileDialog();
            selectSaveToPath.FileOk += (o, args) =>
            {
                if (!selectSaveToPath.FileName.EndsWith(".wgb")) args.Cancel = true;
            };
            selectSaveToPath.DefaultExt = ".wgb";
            selectSaveToPath.Filter = "WGestures备份文件 (*.wgb)|*.wgb";


            selectSaveToPath.FileName = "WGestures " + Application.ProductVersion + ".wgb";

            var result = selectSaveToPath.ShowDialog();
            if (result != DialogResult.OK) return;

            var saveTo = selectSaveToPath.FileName;

            //先应用列表中的顺序，然后保存，然后备份
            ApplyListAppsOrder();
            Controller.IntentStore.Save();
            Controller.Config.Save();

            try
            {
                Controller.ExportTo(saveTo);
                MessageBox.Show("导出成功。", "WGestures导出", MessageBoxButtons.OK, MessageBoxIcon.Information);

            } catch (Exception ex)
            {
                MessageBox.Show("导出失败！原因：" + ex.Message,"WGestures导出失败",MessageBoxButtons.OK, MessageBoxIcon.Error);
#if DEBUG
                throw;
#endif
            }
        }

        #endregion


        private void listApps_ItemDragging(object sender, Controls.CancelListViewItemDragEventArgs e)
        {
            //禁止拖动“（全局）”
            if (e.Item.Index == 0) e.Cancel = true;
        }

        private void listApps_ItemDragDrop(object sender, Controls.ListViewItemDragEventArgs e)
        {
            if (e.InsertionIndex == 0) e.Cancel = true;
        }

        private void listApps_DragOver(object sender, DragEventArgs e)
        {
            var mousePos = listApps.PointToClient(MousePosition);
            var hoverItem = listApps.GetItemAt(mousePos.X,mousePos.Y);

            if (hoverItem != null && hoverItem.Index == 0)
            {
                e.Effect = DragDropEffects.None;
                listApps.InsertionLineColor = Color.Transparent;
            }
            else
            {
                e.Effect = DragDropEffects.Move;
                listApps.InsertionLineColor = Color.DeepSkyBlue;
            }
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ApplyListAppsOrder();
        }

        private void ApplyListAppsOrder()
        {
            foreach (ListViewItem item in listApps.Items)
            {
                if (item.Index == 0) continue;

                var app = (OrderableExeApp)item.Tag;
                app.Order = item.Index;
            }
        }

        #region inner types

        private class CommandTypesComboBoxItem
        {
            public string Name { get; set; }
            public Type CommandType { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
        #endregion

        private void combo_GestureTriggerButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_GestureTriggerButton.DataBindings[0].WriteValue();
        }

        private void listGestureIntents_DoubleClick(object sender, EventArgs e)
        {
            btn_modifyGesture.PerformClick();
        }

    }

}
