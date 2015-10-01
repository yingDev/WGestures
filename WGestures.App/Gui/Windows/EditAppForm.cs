using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Shell32;
using WGestures.Common.OsSpecific.Windows;
using WGestures.Core;
using WGestures.Core.Persistence;
using Win32;

namespace WGestures.App.Gui.Windows
{
    public partial class EditAppForm : Form
    {
        public string AppName { get; private set; }
        public string AppPath { get; private set; }

        private bool _isEditingMode;
        private IGestureIntentStore _intentStore;
        private ExeApp _editingApp;

        private bool _isSelectingWindow = false;
        private IntPtr _curWindow;
        private string _mousePicedProcessFilePath;

        //private string _selfProcessFile;

        public EditAppForm(IGestureIntentStore intentStore)
        {
            InitializeComponent();
            _intentStore = intentStore;
            //todo:暂时找不到解决uiAccess拖拽的办法
            //HackDragDrop();
        }

        public EditAppForm(ExeApp editingApp, IGestureIntentStore intentStore)
            : this(intentStore)
        {
            _isEditingMode = true;

            _editingApp = editingApp;

            Text = "修改 \"" + editingApp.Name + "\"";
            AppName = txtSelectedAppNae.Text = editingApp.Name;
            AppPath = lnkSelectedAppPath.Text = editingApp.ExecutablePath;

            groupSelectedApp.Visible = true;
            btnOk.Enabled = true;

            toolTip1.SetToolTip(lnkSelectedAppPath, editingApp.ExecutablePath);


            LoadIcon(editingApp.ExecutablePath);

            if (!editingApp.AppExists())
            {
                ShowErrorMsg("该应用程序不存在，请确定它的位置");
            }

        }


        #region Window Selector
        private void pictureWindowSelector_MouseDown(object sender, MouseEventArgs e)
        {
            //_selfProcessFile = Native.GetProcessFileFromWindow(Handle);

            pictureWindowSelector.Capture = true;
            _isSelectingWindow = true;
            pictureWindowSelector.Image = Properties.Resources.block;
            Cursor = Cursors.Cross;

            var picBounds = pictureWindowSelector.Bounds;
            var cursorPos = pictureWindowSelector.PointToScreen(new Point(picBounds.Size.Width/2, picBounds.Size.Height/2));
            User32.SetCursorPos(cursorPos.X, cursorPos.Y);
        }

        private void pictureWindowSelector_MouseMove(object sender, MouseEventArgs e)
        {



        }

        private void pictureWindowSelector_MouseUp(object sender, MouseEventArgs e)
        {
            // pictureWindowSelector.Capture = false;
            _isSelectingWindow = false;
            pictureWindowSelector.Image = Properties.Resources.cross;
            Cursor = Cursors.Arrow;

            TopMost = true;
            var win = Native.GetHoveringWindow();
            var rootWin = Native.GetAncestor(win, Native.GetAncestorFlags.GetRoot);

            var procId = Native.GetProcessIdByWindowHandle(rootWin);
            Debug.WriteLine("Selected Proc: " + procId);
            
            //var parentPid = Native.GetParentProcess(procId);
            //Debug.WriteLine("ParentPId: " + parentPid);

            if (win != _curWindow)
            {
                var file = Native.GetProcessFile(procId);

                // if (_selfProcessFile != file)
                //{
                    _mousePicedProcessFilePath = file;
                    //Text = file;
                //}
                // else
                // {
                //   _mousePicedProcessFilePath = null;
                //}

            }


            if (!string.IsNullOrEmpty(_mousePicedProcessFilePath))
            {
                HandleSelectedApp(_mousePicedProcessFilePath, Path.GetFileNameWithoutExtension(_mousePicedProcessFilePath));
            }

        }
        #endregion

        #region Dag and Drop
        private void panelDropFile_ElevatedDragDropHandler(object sender, ElevatedDragDropArgs e)
        {
            if (e.Files == null || e.Files.Count < 1) return;

            var path = e.Files[0];

            if (!File.Exists(path))
            {
                Console.WriteLine("not exist");
                return;
            }

            if (IsDirectory(path))
            {
                Console.WriteLine("Is Directory");
                return;
            }

            string resultPath = null;

            if (Path.GetExtension(path).Equals(".exe", StringComparison.CurrentCultureIgnoreCase)
                || Path.GetExtension(path).Equals(".msc", StringComparison.CurrentCultureIgnoreCase))
            {
                resultPath = path;
            }
            else if (Path.GetExtension(path).Equals(".lnk", StringComparison.CurrentCultureIgnoreCase))
            {
                string target;

                //Console.WriteLine("first Try");
                target = GetLnkTargetSimple(path);
                /*Console.WriteLine(target);
                if (target.Equals(""))
                {
                    try
                    {
                        Console.WriteLine("Second Try");
                        target = GetLnkTarget(path);
                    }
                    catch (Exception)
                    {
                        target = "";
                    }

                }*/

                if (!File.Exists(target))
                {
                    Console.WriteLine("target not exists");
                    return;
                }

                if (IsDirectory(target))
                {
                    Console.WriteLine("target is a directory");
                    return;
                }

                if (!Path.GetExtension(target).Equals(".exe", StringComparison.CurrentCultureIgnoreCase)
                    && !Path.GetExtension(target).Equals(".msc", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("wrong format");
                    return;
                }

                resultPath = target;
            }
            else
            {
                Console.WriteLine("wrong format");
                return;
            }

            HandleSelectedApp(resultPath, Path.GetFileNameWithoutExtension(path));
        }

        private bool IsDirectory(string path)
        {
            var fileAttrs = File.GetAttributes(path);
            if ((fileAttrs & FileAttributes.Directory) != 0)
            {
                Console.WriteLine("Is Directory");
                return true;
            }

            return false;
        }

        #endregion

        #region util
        private static string GetLnkTarget(string lnkPath)
        {
            Shell shl = null;
            Folder dir = null;
            FolderItem itm = null;
            ShellLinkObject lnk = null;

            try
            {
                shl = new Shell32.Shell(); // Move this to class scope
                lnkPath = System.IO.Path.GetFullPath(lnkPath);
                dir = shl.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));
                itm = dir.Items().Item(System.IO.Path.GetFileName(lnkPath));


                lnk = (Shell32.ShellLinkObject)itm.GetLink;

                Console.WriteLine(lnk.Target.Type);
                if (lnk.Target.Type.Equals("System Folder"))
                {
                    Console.WriteLine("explore.exe");
                    return Environment.GetEnvironmentVariable("windir") + @"\explorer.exe";

                }

                try
                {
                    var p = lnk.Target.Path;
                    Console.WriteLine(p);

                    return p;
                }
                catch (COMException)
                {
                    return null;
                }

            }
            finally
            {
                if (shl != null) Marshal.ReleaseComObject(shl);
                if (dir != null) Marshal.ReleaseComObject(dir);
                if (itm != null) Marshal.ReleaseComObject(itm);
                if (lnk != null) Marshal.ReleaseComObject(lnk);
            }


        }

        private static string GetShortcutTarget(string file)
        {
            try
            {
                if (System.IO.Path.GetExtension(file).ToLower() != ".lnk")
                {
                    throw new Exception("Supplied file must be a .LNK file");
                }

                FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read);
                //Console.WriteLine("file length: "+fileStream.Length);
                using (System.IO.BinaryReader fileReader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(0x14, SeekOrigin.Begin);     // Seek to flags
                    uint flags = fileReader.ReadUInt32();        // Read flags
                    if ((flags & 1) == 1)
                    {                      // Bit 1 set means we have to
                        // skip the shell item ID list
                        fileStream.Seek(0x4c, SeekOrigin.Begin); // Seek to the end of the header
                        uint offset = fileReader.ReadUInt16();   // Read the length of the Shell item ID list
                        fileStream.Seek(offset, SeekOrigin.Current); // Seek past it (to the file locator info)
                    }

                    long fileInfoStartsAt = fileStream.Position; // Store the offset where the file info
                    // structure begins
                    uint totalStructLength = fileReader.ReadUInt32(); // read the length of the whole struct
                    fileStream.Seek(0xc, SeekOrigin.Current); // seek to offset to base pathname
                    uint fileOffset = fileReader.ReadUInt32(); // read offset to base pathname
                    // the offset is from the beginning of the file info struct (fileInfoStartsAt)
                    fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin); // Seek to beginning of
                    // base pathname (target)
                    long pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2; // read
                    // the base pathname. I don't need the 2 terminating nulls.
                    char[] linkTarget = fileReader.ReadChars((int)pathLength); // should be unicode safe
                    var link = new string(linkTarget);

                    int begin = link.IndexOf("\0\0");
                    if (begin > -1)
                    {
                        int end = link.IndexOf("\\\\", begin + 2) + 2;
                        end = link.IndexOf('\0', end) + 1;

                        string firstPart = link.Substring(0, begin);
                        string secondPart = link.Substring(end);

                        return firstPart + secondPart;
                    }
                    else
                    {
                        return link;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
                return "";
            }
        }

        private static string GetLnkTargetSimple(string lnkPath)
        {
            IWshRuntimeLibrary.IWshShell wsh = null;
            IWshRuntimeLibrary.IWshShortcut sc = null;

            try
            {
                wsh = new IWshRuntimeLibrary.WshShell();
                sc = (IWshRuntimeLibrary.IWshShortcut)wsh.CreateShortcut(lnkPath);
                return sc.TargetPath;
            }
            finally
            {
                if (wsh != null) Marshal.ReleaseComObject(wsh);
                if (sc != null) Marshal.ReleaseComObject(sc);
            }
        }

        #endregion

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            //dlgSelectFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // dlgSelectFile.FileName = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Camera.lnk";


            var result = dlgSelectFile.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                HandleSelectedApp(dlgSelectFile.FileName, Path.GetFileNameWithoutExtension(dlgSelectFile.FileName));
            }
            else
            {
                Console.WriteLine("Aborted");
            }
        }

        private void HandleSelectedApp(string path, string appName)
        {
            groupSelectedApp.Visible = true;

            lnkSelectedAppPath.Text = path;
            toolTip1.SetToolTip(lnkSelectedAppPath, path);
            if (!_isEditingMode) txtSelectedAppNae.Text = appName;

            LoadIcon(path);
            AppPath = path;

            ExeApp found;
            bool exist = _intentStore.TryGetExeApp(path, out found);

            if (_isEditingMode)
            {
                if (path.Equals(_editingApp.ExecutablePath))
                {
                    flowAlert.Visible = false;
                    btnOk.Enabled = true;
                    return;
                }
            }
            else
            {
                AppName = appName;
            }


            if (!exist)
            {
                ClearErrorMsg();
            }
            else
            {
                ShowErrorMsg("应用程序已经在列表中，请重新选择");
            }
            btnOk.Enabled = !exist;

            txtSelectedAppNae.SelectAll();
            txtSelectedAppNae.Focus();
        }

        private void LoadIcon(string path)
        {
            pictureSelectedAppIcon.Image = IconHelper.ExtractIconForPath(path, new Size(48, 48),true, 0);

        }

        private void lnkSelectedAppPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!File.Exists(lnkSelectedAppPath.Text))
            {
                return;
            }

            using (Process.Start("explorer", "/select, " + lnkSelectedAppPath.Text)) ;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void HackDragDrop()
        {
            //由于uipi限制，需要这样启用拖放支持。AllowDrop和相关事件不再有效。
            ElevatedDragDropManager.Instance.EnableDragDrop(panelFileDrop.Handle);
            ElevatedDragDropManager.Instance.ElevatedDragDrop += panelDropFile_ElevatedDragDropHandler;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            AppName = txtSelectedAppNae.Text;
            DialogResult = DialogResult.OK;

            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.W))
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txtSelectedAppNae_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = (txtSelectedAppNae.Text.Trim().Length != 0);

        }

        private void panelFileDrop_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length < 1) return;

            var path = files[0];

            if (!File.Exists(path))
            {
                Console.WriteLine("not exist");
                return;
            }

            if (IsDirectory(path))
            {
                Console.WriteLine("Is Directory");
                return;
            }

            string resultPath = null;

            if (Path.GetExtension(path).Equals(".exe", StringComparison.CurrentCultureIgnoreCase)
                || Path.GetExtension(path).Equals(".msc", StringComparison.CurrentCultureIgnoreCase))
            {
                resultPath = path;
            }
            else if (Path.GetExtension(path).Equals(".lnk", StringComparison.CurrentCultureIgnoreCase))
            {
                string target;

                //Console.WriteLine("first Try");
                target = GetLnkTargetSimple(path);
                /*Console.WriteLine(target);
                if (target.Equals(""))
                {
                    try
                    {
                        Console.WriteLine("Second Try");
                        target = GetLnkTarget(path);
                    }
                    catch (Exception)
                    {
                        target = "";
                    }

                }*/

                if (!File.Exists(target))
                {
                    Console.WriteLine("target not exists");
                    return;
                }

                if (IsDirectory(target))
                {
                    Console.WriteLine("target is a directory");
                    return;
                }

                if (!Path.GetExtension(target).Equals(".exe", StringComparison.CurrentCultureIgnoreCase)
                    && !Path.GetExtension(target).Equals(".msc", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("wrong format");
                    return;
                }

                resultPath = target;
            }
            else
            {
                Console.WriteLine("wrong format");
                return;
            }

            HandleSelectedApp(resultPath, Path.GetFileNameWithoutExtension(path));
        }

        private void panelFileDrop_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("WTF");
            e.Effect = DragDropEffects.Link;
        }

        private void ShowErrorMsg(string msg)
        {
            flowAlert.Visible = true;
            lb_errMsg.Text = msg;
        }

        private void ClearErrorMsg()
        {
            flowAlert.Visible = false;
        }

        private void EditAppForm_Load(object sender, EventArgs e)
        {
            txtSelectedAppNae.SelectAll();
            txtSelectedAppNae.Focus();
        }

    }

}
