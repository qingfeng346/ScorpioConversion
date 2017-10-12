using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace ScorpioConversion {
    public partial class FormMain : Form {
        private static FormMain instance;
        public static FormMain GetInstance() { return instance ?? (instance = new FormMain()); }
        public FormMain() {
            InitializeComponent();
            FormClosing += (sender, e) => { this.Visible = false; e.Cancel = true; };
            VisibleChanged += (sender, e) => { ConversionUtil.CheckExit(); };
        }
        private void FormMain_Load(object sender, EventArgs e) {
            Init();
        }
        public void Show(string path) {
            this.Show();
            ConversionUtil.WorkspaceDirectory = path;
            Util.WorkspaceDirectory = path;
            this.Text = "转表工具 : " + path;
            Bind();
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
            FormTiny.GetInstance().Show();
        }

        private void ChangeWorkspace_Click(object sender, EventArgs e) {
            FormWorkspace.GetInstance().Show();
        }
        private void OpenWorkspace_Click(object sender, EventArgs e) {
            Process.Start(ConversionUtil.WorkspaceDirectory);
        }
        private void MenuAbout_Click(object sender, EventArgs e) {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Process.Start("ScorpioUpdater.exe", version + " http://www.fengyuezhu.com/app.php?app=ScorpioConversion http://www.fengyuezhu.com/project/ScorpioConversion/");
        }

        private void mD5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormMD5().Show();
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormTime().Show();
        }
    }
}
