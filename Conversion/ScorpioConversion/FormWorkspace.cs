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
        const string workspacelist = "workspacelist";           //工作目录列表
        private static FormWorkspace instance;
        public static FormWorkspace GetInstance() { return instance ?? (instance = new FormWorkspace()); }
        public FormWorkspace() {
            InitializeComponent();
            FormClosing += (sender, e) => { e.Cancel = true;  this.Hide(); };
            VisibleChanged += (sender, e) => { ConversionUtil.CheckExit(); };
            Shown += (sender, e) => { this.Hide(); };
            listPaths.DoubleClick += listPaths_DoubleClick;
        }
        private List<string> paths = new List<string>();
        private void FormWorkspace_Load(object sender, EventArgs e) {
            paths.AddRange(localStroage.get(workspacelist).Split(';'));
            while (true) {
                if (!RemoveInvalidPath()) break;
            }
            listPaths.Items.AddRange(paths.ToArray());
            textWorkspace.Text = (paths.Count > 0) ? paths[0] : ConversionUtil.CurrentDirectory;
            EnterWorkspace();
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

        private void listPaths_DoubleClick(object sender, EventArgs e) {
            int index = listPaths.SelectedIndex;
            if (index < 0) { return; }
            textWorkspace.Text = listPaths.Items[index].ToString();
            EnterWorkspace();
        }
        private void buttonOK_Click(object sender, EventArgs e) {
            EnterWorkspace();
        }
        private void EnterWorkspace() {
            var path = textWorkspace.Text;
            if (string.IsNullOrEmpty(path) || !FileUtil.CreateDirectory(path)) {
                MessageBox.Show("请输入有效目录");
                return;
            }
            InsertPath(path);
            FormMain.GetInstance().Show(path + "/");
        }
        void InsertPath(string path) {
            paths.Remove(path);
            paths.Insert(0, path);
            if (!listPaths.Items.Contains(path))
                listPaths.Items.Add(path);
            localStroage.set(workspacelist, string.Join(";", paths.ToArray()));
        }
        private void buttonClose_Click(object sender, EventArgs e) {
            this.Hide();
        }
    }
}
