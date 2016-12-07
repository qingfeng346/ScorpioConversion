using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Threading;

namespace ScorpioConversion {
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e) {
            this.Text = "转表工具 : " + Util.CurrentDirectory;
            Init();
        }
        private void CodeFoldButton_Click(object sender, EventArgs e) {
            this.CodeFoldPanel.Visible = !this.CodeFoldPanel.Visible;
        }
        private void LanguageFoldButton_Click(object sender, EventArgs e) {
            this.LanguageFoldPanel.Visible = !this.LanguageFoldPanel.Visible;
        }
        private void OtherFoldButton_Click(object sender, EventArgs e) {
            this.OtherFoldPanel.Visible = !this.OtherFoldPanel.Visible;
        }
        private void timerMain_Tick(object sender, EventArgs e) {
            Tick();
        }

        private void toolStripButtonTiny_Click(object sender, EventArgs e) {
            new FormTiny().Show();
        }
    }
}
