using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
namespace XML_Conversion
{
    public partial class FormMain : Form
    {
        private delegate void OnFinished();
        
        
        private List<string> transformFileNames = new List<string>();           //转换文件列表
        private List<string> rollbackFileNames = new List<string>();            //反转文件列表
        private bool timeEnable = false;                                        //计时器是否能使用
        private OnFinished m_Finished = null;                                   //结束回调
        public FormMain()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.listLog.Columns.Add("行数", 50);
                this.listLog.Columns.Add("层级", 50);
                this.listLog.Columns.Add("内容", 400);
                this.listLog.GridLines = true;
                this.listLog.FullRowSelect = true;
                this.listLog.Scrollable = true;
                this.listLog.View = View.Details;
                this.listLog.AutoArrange = true;
                Util.Bind(this.textCodeCS, PROGRAM.CS, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
                Util.Bind(this.textDataCS, PROGRAM.CS, ConfigKey.DataDirectory, ConfigFile.PathConfig);
                Util.Bind(this.textCodeJava, PROGRAM.JAVA, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
                Util.Bind(this.textDataJava, PROGRAM.JAVA, ConfigKey.DataDirectory, ConfigFile.PathConfig);
                Util.Bind(this.textCodePHP, PROGRAM.PHP, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
                Util.Bind(this.textDataPHP, PROGRAM.PHP, ConfigKey.DataDirectory, ConfigFile.PathConfig);
                Util.Bind(this.textSpawn, ConfigKey.SpawnList, ConfigFile.InitConfig);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        void OnQuit()
        {
            Application.Exit();
        }
        void SetEnable(bool enable)
        {
            this.textCodeCS.Enabled = enable;
            this.textDataCS.Enabled = enable;
            this.textCodeJava.Enabled = enable;
            this.textDataJava.Enabled = enable;
            this.textCodePHP.Enabled = enable;
            this.textDataPHP.Enabled = enable;
            this.changeCSCode.Enabled = enable;
            this.changeCSData.Enabled = enable;
            this.changeJavaCode.Enabled = enable;
            this.changeJavaData.Enabled = enable;
            this.changePHPCode.Enabled = enable;
            this.changePHPData.Enabled = enable;
            this.selectTransformFiles.Enabled = enable;
            this.buttonTransform.Enabled = enable;
            this.selectRollbackFiles.Enabled = enable;
            this.buttonRollback.Enabled = enable;
            this.buttonLanguage.Enabled = enable;
            this.buttonRefreshLanguage.Enabled = enable;
        }
        void StartRun(OnFinished callBack)
        {
            m_Finished = callBack;
            timeEnable = true;
            this.timerProgress.Enabled = true;
            SetEnable(false);
        }
        void EndRun()
        {
            timeEnable = false;
            if (m_Finished != null) m_Finished();
        }
        //选择cs代码路径
        private void changeCode_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            dialog.SelectedPath = this.textCodeCS.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.textCodeCS.Text = dialog.SelectedPath;
        }
        //选择cs文件路径
        private void changeData_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            dialog.SelectedPath = this.textDataCS.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.textDataCS.Text = dialog.SelectedPath;
        }
        //选择java代码路径
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            dialog.SelectedPath = this.textCodeJava.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.textCodeJava.Text = dialog.SelectedPath;
        }
        //选择java文件路径
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            dialog.SelectedPath = this.textDataJava.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.textDataJava.Text = dialog.SelectedPath;
        }
        //选择php代码路径
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            dialog.SelectedPath = this.textCodePHP.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.textCodePHP.Text = dialog.SelectedPath;
        }
        //选择php文件路径
        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            dialog.SelectedPath = this.textDataPHP.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.textDataPHP.Text = dialog.SelectedPath;
        }
        //选择要转换的文件
        private void selectTransformFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.InitialDirectory = Util.GetConfig(ConfigKey.TransformDirectory, ConfigFile.PathConfig);
            dialog.Filter = "Excel文件|*.xls;|所有文件|*.*";
            dialog.Title = "请选择要转换的文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Util.SetConfig(ConfigKey.TransformDirectory, Path.GetDirectoryName(dialog.FileName), ConfigFile.PathConfig);
                transformFileNames.Clear();
                transformFileNames.AddRange(dialog.FileNames);
                string strText = "";
                for (int i = 0; i < transformFileNames.Count; ++i)
                    strText += (transformFileNames[i] + ";");
                this.textTransformFiles.Text = strText;
                this.progressLabel.Text = string.Format("{0}/{1}", 1, transformFileNames.Count);
            }
        }
        //选择要反转的文件
        private void selectRollbackFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.InitialDirectory = Util.GetConfig(ConfigKey.RollbackDirectory, ConfigFile.PathConfig);
            dialog.Filter = "data文件|*.data|所有文件|*.*";
            dialog.Title = "请选择要反转的文件";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Util.SetConfig(ConfigKey.RollbackDirectory, Path.GetDirectoryName(dialog.FileName), ConfigFile.PathConfig);
                rollbackFileNames.Clear();
                rollbackFileNames.AddRange(dialog.FileNames);
                string strText = "";
                for (int i = 0; i < rollbackFileNames.Count; ++i)
                    strText += (rollbackFileNames[i] + ";");
                this.textRollbackFiles.Text = strText;
                this.progressLabel.Text = string.Format("{0}/{1}", 1, rollbackFileNames.Count);
            }
        }
        //点击转换按钮
        void buttonTransform_Click(object sender, EventArgs e)
        {
            Transform(null);
        }
        void Transform(OnFinished callBack)
        {
            StartRun(callBack);
            Thread nonParameterThread = new Thread(new ThreadStart(ThreadTransform));
            nonParameterThread.Start();
        }
        void ThreadTransform()
        {
            TableManager.GetInstance().Transform(transformFileNames, this.getManager.Checked, this.tableEnum.Checked, 
                this.Language.Checked, this.getCustom.Checked, this.getBase.Checked);
            EndRun();
        }
        //点击反转
        void buttonRollback_Click(object sender, EventArgs e)
        {
            Rollback(null);
        }
        void Rollback(OnFinished callBack)
        {
            StartRun(callBack);
            Thread nonParameterThread = new Thread(new ThreadStart(ThreadRollback));
            nonParameterThread.Start();
        }
        void ThreadRollback()
        {
            TableManager.GetInstance().Rollback(rollbackFileNames.ToArray());
            EndRun();
        }
        //进度计时器
        private void timerProgress_Tick(object sender, EventArgs e)
        {
            this.progressLabel.Text = string.Format("{0}/{1}", Progress.Current, Progress.Count);
            this.progressBar.Value = Convert.ToInt32(Progress.Value * 100);
            if (timeEnable == false)
            {
                this.timerProgress.Enabled = false;
                SetEnable(true);
            }
        }
        //导出日志
        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择要保存的位置";
            dialog.FileName = "log.log";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = "";
                for (int i = 0; i < this.listLog.Items.Count; ++i) {
                    for (int j = 0; j < this.listLog.Columns.Count; ++j) {
                        str += this.listLog.Items[i].SubItems[j].Text + "\t";
                    }
                    str += Util.ReturnString;
                }
                FileUtil.CreateFile(dialog.FileName, str);
            }
        }
        //清空日志
        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.listLog.Items.Clear();
        }
        //输出日志计时器
        void timerLog_Tick(object sender, EventArgs e)
        {
            lock (Logger.OutMessage)
            {
                FileStream stream = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "log.log", FileMode.Append, FileAccess.Write);
                while (Logger.OutMessage.Count > 0)
                {
                    LogValue value = Logger.OutMessage.Dequeue();
                    ListViewItem item = new ListViewItem((this.listLog.Items.Count + 1).ToString());
                    item.SubItems.Add(value.type.ToString());
                    item.SubItems.Add(value.message);
                    if (value.type == LogType.INFO)
                        item.BackColor = System.Drawing.Color.White;
                    else if (value.type == LogType.WARNING)
                        item.BackColor = System.Drawing.Color.Yellow;
                    else
                        item.BackColor = System.Drawing.Color.Red;
                    this.listLog.Items.Insert(this.listLog.Items.Count, item);
                    item.EnsureVisible();
                    byte[] buffer = Encoding.UTF8.GetBytes(DateTime.Now.ToString() + "  [" + value.type + "]" + value.message + "\n");
                    stream.Write(buffer, 0, buffer.Length);
                }
                stream.Close();
            }
        }
        //打开多国语言配置界面
        private void buttonLanguage_Click(object sender, EventArgs e)
        {
            new FormLanguage().ShowDialog();
        }
        //刷新多国语言
        private void button1_Click_1(object sender, EventArgs e)
        {
            RefreshLanguage(null);
        }
        void RefreshLanguage(OnFinished callBack)
        {
            StartRun(callBack);
            Thread nonParameterThread = new Thread(new ThreadStart(ThreadRefreshLanguage));
            nonParameterThread.Start();
        }
        void ThreadRefreshLanguage()
        {
            TableManager.GetInstance().RefreshLanguage();
            EndRun();
        }
    }
}
