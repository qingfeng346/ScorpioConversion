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
        private PROGRAM m_Program;                                              //当前选择的语言
        private FormLog m_FormLog;                                              //日志显示
        private FormLanguage m_FormLanguage;                                    //多国语言配置
        public FormMain()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            programBox.Items.Clear();
            for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
                programBox.Items.Add(((PROGRAM)i).ToString());
            }
            programBox.SelectedIndex = 0;
            Util.Bind(packageText, "PackageName", ConfigFile.InitConfig);
            m_FormLog = new FormLog();
            m_FormLanguage = new FormLanguage();
        }
        void OnQuit()
        {
            Application.Exit();
        }
        void SetEnable(bool enable)
        {
            this.programBox.Enabled = enable;
            this.buttonSpwan.Enabled = enable;
            this.buttonCode.Enabled = enable;
            this.buttonData.Enabled = enable;
            this.checkCreate.Enabled = enable;
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
                Util.SetToolTip(textTransformFiles, strText.Replace(";", "\n"));
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
            TableManager.GetInstance().Transform(transformFileNames);
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
            //TableManager.GetInstance().Rollback(rollbackFileNames.ToArray());
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
            //TableManager.GetInstance().RefreshLanguage();
            EndRun();
        }
        //打开多国语言配置界面
        private void buttonLanguage_Click(object sender, EventArgs e)
        {
            m_FormLanguage.Show();
        }
        //打开日志界面
        private void buttonLog_Click(object sender, EventArgs e)
        {
            m_FormLog.Show();
        }
        private void buttonCode_Click(object sender, EventArgs e)
        {
            var form = new FormPath();
            form.Initialize(m_Program, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
            form.Show();
        }
        private void buttonData_Click(object sender, EventArgs e)
        {
            var form = new FormPath();
            form.Initialize(m_Program, ConfigKey.DataDirectory, ConfigFile.PathConfig);
            form.Show();
        }
        private void buttonSpwan_Click(object sender, EventArgs e)
        {
            var form = new FormPath();
            form.Initialize(PROGRAM.NONE, ConfigKey.SpawnList, ConfigFile.InitConfig);
            form.Show();
        }
        private void programBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Program = (PROGRAM)Enum.Parse(typeof(PROGRAM), programBox.Text);
            checkCreate.Checked = Util.ToBoolean(Util.GetConfig(m_Program, ConfigKey.Create, ConfigFile.PathConfig), true);
            checkCompress.Checked = Util.ToBoolean(Util.GetConfig(m_Program, ConfigKey.Compress, ConfigFile.InitConfig), false);
        }
        private void checkCreate_CheckedChanged(object sender, EventArgs e)
        {
            Util.SetConfig(m_Program, ConfigKey.Create, checkCreate.Checked ? "true" : "false", ConfigFile.PathConfig);
        }
        private void checkCompress_CheckedChanged(object sender, EventArgs e)
        {
            Util.SetConfig(m_Program, ConfigKey.Compress, checkCreate.Checked ? "true" : "false", ConfigFile.InitConfig);
        }
    }
}
