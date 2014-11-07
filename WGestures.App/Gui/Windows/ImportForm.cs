using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WGestures.App.Migrate;
using WGestures.App.Properties;

namespace WGestures.App.Gui.Windows
{
    public partial class ImportForm : Form
    {
        internal class ImportEventArgs : EventArgs
        {
            public ConfigAndGestures ConfigAndGestures { get; private set; }
            public ImportOption GesturesImportOption { get; private set; }
            public ImportOption ConfigImportOption { get; private set; }

            public bool Success { get; set; }
            public string ErrorMessage { get; set; }

            public ImportEventArgs(ConfigAndGestures confAndGest, ImportOption gestImpOpt, ImportOption confImpOpt)
            {
                Success = true;

                ConfigAndGestures = confAndGest;
                GesturesImportOption = gestImpOpt;
                ConfigImportOption = confImpOpt;
            }
        }

        private ImportOption _gesturesImportOption = ImportOption.None;
        private ImportOption _configImportOption = ImportOption.None;

        private ConfigAndGestures _configAndGestures;

        private string _filePath;

        internal event EventHandler<ImportEventArgs> Import;

        public ImportForm()
        {
            InitializeComponent();
            Icon = Resources.icon;

            combo_importGesturesOption.SelectedIndex = 0;
        }

        private void btn_selectWgb_Click(object sender, EventArgs e)
        {
            var result = openFile_wgb.ShowDialog();
            if (result == DialogResult.OK)
            {
                _filePath = openFile_wgb.FileName;
                HideError();
                

                try
                {
                    _configAndGestures = MigrateService.Import(_filePath);
                }
                catch (MigrateException ex)
                {
                    ShowError(ex.Message);
                    return;
                }

                var containsGestures = _configAndGestures.GestureIntentStore != null;
                var containsConfig = _configAndGestures.Config != null;

                check_importGestures.Checked = containsGestures;
                check_importGestures.Enabled = containsGestures;

                check_importConfig.Checked = containsConfig;
                check_importConfig.Enabled = containsConfig;

                txt_filePath.Text = _filePath;
                group_importOptions.Visible = true;
            }
        }

        internal enum ImportOption
        {
            None, Replace, Merge
        }

        private void ImportForm_Load(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void check_importGestures_CheckedChanged(object sender, EventArgs e)
        {
            combo_importGesturesOption.Enabled = check_importGestures.Checked;
            if (!check_importGestures.Checked)
            {
                _gesturesImportOption = ImportOption.None;
            }
            else
            {
                _gesturesImportOption = combo_importGesturesOption.SelectedIndex == 0 ? ImportOption.Merge : ImportOption.Replace;
            }

            Validate();
        }

        private void check_importConfig_CheckedChanged(object sender, EventArgs e)
        {
            if (!check_importConfig.Checked)
            {
                _configImportOption = ImportOption.None;
            }
            else
            {
                _configImportOption = ImportOption.Merge;
            }

            Validate();
        }



        private void combo_importGesturesOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            _gesturesImportOption = combo_importGesturesOption.SelectedIndex == 0 ? ImportOption.Merge : ImportOption.Replace;

        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            HideError();
            OnImport();
        }

        private void OnImport()
        {
            if (Import != null)
            {
                var args = new ImportEventArgs(_configAndGestures, _gesturesImportOption, _configImportOption);
                Import(this, args);

                if (args.Success)
                {
                    MessageBox.Show("导入成功！", "导入完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    if(args.ErrorMessage != null) ShowError(args.ErrorMessage);
                }

            }
        }


        private void ShowError(string msg)
        {
            lb_errMsg.Text = msg;
            flowAlert.Visible = true;
        }

        private void HideError()
        {
            flowAlert.Visible = false;

        }

        new private void Validate()
        {
            if (!(check_importConfig.Checked || check_importGestures.Checked))
            {
                btnOk.Enabled = false;
            }
            else
            {
                btnOk.Enabled = true;
            }
        }


    }
}
