using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScorpioConversion {
    public partial class FormMD5 : Form {
        public FormMD5() {
            InitializeComponent();
        }
        private void richTextBoxSource_TextChanged(object sender, EventArgs e) {
            GetMD5();
        }
        private void checkBoxLower_CheckedChanged(object sender, EventArgs e) {
            GetMD5();
        }
        void GetMD5() {
            var result = FileUtil.GetMD5FromString(richTextBoxSource.Text);
            textBoxResult.Text = checkBoxLower.Checked ? result.ToLower() : result.ToUpper();
        }
        private void buttonCMD5_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("http://www.cmd5.com/");
        }
    }
}
