using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ScorpioConversion {
    public partial class FormWorkspace : Form {
        private static FormWorkspace instance;
        public static FormWorkspace GetInstance() { return instance ?? (instance = new FormWorkspace()); }
        public FormWorkspace() {
            InitializeComponent();
            FormClosing += (sender, e) => { e.Cancel = true;  this.Hide(); };
            VisibleChanged += (sender, e) => { ConversionUtil.CheckExit(); };
            Shown += (sender, e) => { if (checkDefault.Checked) Enter(); };
        }
        private string workspaceConfig = "";
        private string workspaceDefault = "";
        private List<string> paths = new List<string>();
        private void FormWorkspace_Load(object sender, EventArgs e) {
            workspaceConfig = ConversionUtil.CurrentDirectory + "workspace.ini";
            workspaceDefault = ConversionUtil.CurrentDirectory + "workspace.default";
            paths.AddRange(FileUtil.GetFileString(workspaceConfig).Split(';'));
            while (true) {
                if (!RemoveInvalidPath()) break;
            }
            comboWorkspace.Items.AddRange(paths.ToArray());
            if (paths.Count > 0)
                comboWorkspace.Text = paths[0];
            else
                comboWorkspace.Text = ConversionUtil.CurrentDirectory;
            if (FileUtil.FileExist(workspaceDefault)) {
                checkDefault.Checked = FileUtil.GetFileString(workspaceDefault) == "1";
            } else {
                checkDefault.Checked = true;
            }
        }
        bool RemoveInvalidPath() {
            for (int i = 0;i < paths.Count;++i) {
                if (!FileUtil.PathExist(paths[i])) {
                    paths.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        void InsertPath(string path) {
            paths.Remove(path);
            paths.Insert(0, path);
            comboWorkspace.Items.Remove(path);
            comboWorkspace.Items.Insert(0, path);
            comboWorkspace.Text = path;
            FileUtil.CreateFile(workspaceConfig, string.Join(";", paths.ToArray()));
        }
        private void buttonOK_Click(object sender, EventArgs e) {
            Enter();
        }
        private void Enter() {
            var path = comboWorkspace.Text;
            if (string.IsNullOrEmpty(path) || !FileUtil.CreateDirectory(path)) {
                MessageBox.Show("清输入有效目录");
                return;
            }
            InsertPath(path);
            FormMain.GetInstance().Show(path + "/");
            this.Hide();
        }
        private void buttonClose_Click(object sender, EventArgs e) {
            this.Hide();
        }
        private void checkDefault_CheckedChanged(object sender, EventArgs e) {
            if (this.checkDefault.Checked)
                FileUtil.CreateFile(workspaceDefault, "1");
            else
                FileUtil.CreateFile(workspaceDefault, "0");
        }
    }
}
