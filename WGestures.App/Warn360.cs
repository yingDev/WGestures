using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WGestures.App.Properties;

namespace WGestures.App
{
    public partial class Warn360 : Form
    {
        public Warn360()
        {
            InitializeComponent();
            Icon = Resources.icon;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://tieba.baidu.com/p/3275239932");
        }

        private void Warn360_Load(object sender, EventArgs e)
        {
            tb_wgPath.Text = Application.ExecutablePath;
            Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
