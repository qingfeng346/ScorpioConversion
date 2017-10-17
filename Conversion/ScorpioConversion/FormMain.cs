using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ScorpioConversion {
    public partial class FormMain : Form {

        //如果函数执行成功，返回值不为0。
        //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd,                //要定义热键的窗口的句柄
            int id,                     //定义热键ID（不能与其它ID重复）
            KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
            Keys vk                     //定义热键的内容
            );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,                //要取消热键的窗口的句柄
            int id                      //要取消热键的ID
            );
        //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
        [Flags()]
        public enum KeyModifiers {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }

        private static FormMain instance;
        public static FormMain GetInstance() { return instance ?? (instance = new FormMain()); }
        public FormMain() {
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e) {
            Init();
            Show(ConversionUtil.CurrentDirectory);
            //注册热键Shift+S，Id号为100。HotKey.KeyModifiers.Shift也可以直接使用数字4来表示。
            RegisterHotKey(Handle, 100, KeyModifiers.Alt, Keys.W);
        }
        private void FormMain_Closed(object sender, EventArgs e) {
            //this.notifyIcon1.Visible = false;
        }
        private void FormMain_SizeChanged(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.Hide();
            }
        }
        public void Show(string path) {
            ConversionUtil.WorkspaceDirectory = path;
            Util.WorkspaceDirectory = path;
            this.Text = "转表工具 : " + path;
            this.Show();
            Bind();
        }
        protected override void WndProc(ref Message m) {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg) {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32()) {
                        case 100:
                            Switch();
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        private void Switch() {
            if (this.Visible) {
                this.Hide();
            } else {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
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
        private void md5ToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormMD5().Show();
        }
        private void timeToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormTime().Show();
        }
        private void imageToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormTiny().Show();
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            Switch();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
