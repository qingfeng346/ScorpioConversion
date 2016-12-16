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
            Shown += (sender, e) => {
                if (checkDefault.Checked) {
                    Enter();
                    this.Hide();
                }
            };
            listPaths.DoubleClick += listPaths_DoubleClick;
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
            listPaths.Items.AddRange(paths.ToArray());
            textWorkspace.Text = (paths.Count > 0) ? paths[0] : ConversionUtil.CurrentDirectory;
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
            if (!listPaths.Items.Contains(path))
                listPaths.Items.Add(path);
            FileUtil.CreateFile(workspaceConfig, string.Join(";", paths.ToArray()));
        }
        private void listPaths_DoubleClick(object sender, EventArgs e) {
            int index = listPaths.SelectedIndex;
            if (index < 0) { return; }
            textWorkspace.Text = listPaths.Items[index].ToString();
            Enter();
        }
        private void buttonOK_Click(object sender, EventArgs e) {
            Enter();
        }
        private void Enter() {
            var path = textWorkspace.Text;
            if (string.IsNullOrEmpty(path) || !FileUtil.CreateDirectory(path)) {
                MessageBox.Show("清输入有效目录");
                return;
            }
            InsertPath(path);
            FormMain.GetInstance().Show(path + "/");
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
