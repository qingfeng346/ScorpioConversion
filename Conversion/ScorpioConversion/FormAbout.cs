using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScorpioConversion
{
    public partial class FormAbout : Form
    {
        
        public FormAbout()
        {
            InitializeComponent();
        }
        private void FormAbout_Load(object sender, EventArgs e)
        {
            label1.Text = "当前版本 : " + UpdateUtil.Version;
;       }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UpdateUtil.OpenUrl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateUtil.CheckVersion(true);
        }
    }
}
