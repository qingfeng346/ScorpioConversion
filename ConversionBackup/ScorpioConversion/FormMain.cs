using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
namespace ScorpioConversion
{
    public partial class FormMain : Form
    {
        private bool timeEnable = false;        //计时器是否能使用
        private PROGRAM m_Program;              //当前选择的语言
        private FormLanguage m_FormLanguage;    //多国语言配置
        private FormTinyPNG m_FormTinyPNG;      //TinyPNG界面    
        public FormMain()
        {
            Logger.SetLogger(new LibLogger());
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            programBox.Items.Clear();
            for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
                programBox.Items.Add(((PROGRAM)i).ToString());
            }
            programBox.SelectedIndex = 0;
            ConversionUtil.Bind(packageText, ConfigKey.PackageName, ConfigFile.InitConfig);
            ConversionUtil.Bind(textTableConfig, ConfigKey.TableConfigPath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textTableFolder, ConfigKey.TableFolderPath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textMessage, ConfigKey.MessagePath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textDatabase, ConfigKey.DatabasePath, ConfigFile.InitConfig);
            m_FormLanguage = new FormLanguage();
            m_FormTinyPNG = new FormTinyPNG();
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
            this.checkCompress.Enabled = enable;
            this.selectTransformFiles.Enabled = enable;
            this.buttonTransform.Enabled = enable;
            this.selectRollbackFiles.Enabled = enable;
            this.buttonRollback.Enabled = enable;
            this.buttonLanguage.Enabled = enable;
            this.buttonRefreshLanguage.Enabled = enable;
            this.buttonMessage.Enabled = enable;
            this.buttonDatabase.Enabled = enable;
            this.textTableConfig.Enabled = enable;
            this.buttonTransformFolder.Enabled = enable;
            this.buttonDatabaseConfig.Enabled = enable;
            this.clearPath.Enabled = enable;
            this.textTransformFiles.Enabled = enable;
            this.textRollbackFiles.Enabled = enable;
            this.textMessage.Enabled = enable;
            this.textDatabase.Enabled = enable;
            this.textTableFolder.Enabled = enable;
            this.getManager.Enabled = enable;
            this.Language.Enabled = enable;
            this.refreshNote.Enabled = enable;
            this.packageText.Enabled = enable;
        }
        void StartRun(ThreadStart start)
        {
            timeEnable = true;
            this.timerProgress.Enabled = true;
            SetEnable(false);
            FormLog.GetInstance().Show();
            new Thread(start).Start();
        }
        void EndRun()
        {
            timeEnable = false;
        }
        private Dictionary<PROGRAM, ProgramConfig> GetProgramConfig()
        {
            Dictionary<PROGRAM, ProgramConfig> configs = new Dictionary<PROGRAM, ProgramConfig>();
            for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
                PROGRAM program = (PROGRAM)i;
                configs.Add(program, new ProgramConfig() {
                    CodeDirectory = ConversionUtil.GetConfig(program, ConfigKey.CodeDirectory, ConfigFile.PathConfig),
                    DataDirectory = ConversionUtil.GetConfig(program, ConfigKey.DataDirectory, ConfigFile.PathConfig),
                    Create = ConversionUtil.GetConfig(program, ConfigKey.Create, ConfigFile.PathConfig),
                    Compress = ConversionUtil.GetConfig(program, ConfigKey.Compress, ConfigFile.InitConfig),
                });
            }
            return configs;
        }
        //选择要转换的文件
        private void selectTransformFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.InitialDirectory = ConversionUtil.GetConfig(ConfigKey.TransformDirectory, ConfigFile.PathConfig);
            dialog.Filter = "Excel文件|*.xls;|所有文件|*.*";
            dialog.Title = "请选择要转换的文件";
            if (dialog.ShowDialog() == DialogResult.OK) {
                ConversionUtil.SetConfig(ConfigKey.TransformDirectory, Path.GetDirectoryName(dialog.FileName), ConfigFile.PathConfig);
                string strText = string.Join(";", dialog.FileNames);
                this.textTransformFiles.Text = strText;
                ConversionUtil.SetToolTip(textTransformFiles, strText.Replace(";", "\n"));
                this.progressLabel.Text = string.Format("{0}/{1}", 1, dialog.FileNames.Length);
            }
        }
        //选择要反转的文件
        private void selectRollbackFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.InitialDirectory = ConversionUtil.GetConfig(ConfigKey.RollbackDirectory, ConfigFile.PathConfig);
            dialog.Filter = "data文件|*.data|所有文件|*.*";
            dialog.Title = "请选择要反转的文件";
            if (dialog.ShowDialog() == DialogResult.OK) {
                ConversionUtil.SetConfig(ConfigKey.RollbackDirectory, Path.GetDirectoryName(dialog.FileName), ConfigFile.PathConfig);
                string strText = "";
                for (int i = 0; i < dialog.FileNames.Length; ++i)
                    strText += (dialog.FileNames[i] + ";");
                this.textRollbackFiles.Text = strText;
                this.progressLabel.Text = string.Format("{0}/{1}", 1, dialog.FileNames.Length);
            }
        }
        //点击转换文件夹
        void buttonTransformFolder_Click(object sender, EventArgs e)
        {
            StartRun(() => {
                try {
                    var files = Directory.GetFiles(textTableFolder.Text, "*.xls", SearchOption.AllDirectories);
                    if (files.Length == 0)
                        throw new Exception(string.Format("路径[{0}]下的文件数量为0", textTableFolder.Text));
                    TableBuilder tableBuilder = new TableBuilder();
                    Dictionary<string, LanguageTable> mTables = new Dictionary<string, LanguageTable>();
                    tableBuilder.ExecuteField = (PackageField field, string id, ref string value) => {
                        if (field.Attribute.GetValue("Language").LogicOperation()) {
                            if (!Util.IsEmptyString(value)) {
                                if (!mTables.ContainsKey(tableBuilder.Filer))
                                    mTables[tableBuilder.Filer] = new LanguageTable();
                                string key = string.Format("{0}_{1}_{2}", tableBuilder.Filer, field.Name, id);
                                mTables[tableBuilder.Filer].Languages[key] = new Language(key, value);
                            }
                        }
                    };
                    tableBuilder.Transform(string.Join(";", files),
                        textTableConfig.Text,
                        packageText.Text,
                        ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
                        getManager.Checked,
                        refreshNote.Checked,
                        GetProgramConfig());
                    if (Language.Checked) {
                        BuildLanguage(mTables, true);
                    }
                } catch (Exception ex) {
                    Logger.error("TransformFolder is error : " + ex.ToString());
                }
                EndRun();
            });
        }
        //点击转换按钮
        void buttonTransform_Click(object sender, EventArgs e)
        {
            StartRun(() => {
                new TableBuilder().Transform(this.textTransformFiles.Text, 
                    textTableConfig.Text, 
                    packageText.Text, 
                    ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
                    getManager.Checked,
                    refreshNote.Checked,
                    GetProgramConfig());
                EndRun();
            });
        }
        //点击反转
        void buttonRollback_Click(object sender, EventArgs e)
        {
            StartRun(() => {
                new TableBuilder().Rollback(this.textRollbackFiles.Text);
                EndRun();
            });
        }
        //进度计时器
        private void timerProgress_Tick(object sender, EventArgs e)
        {
            if (Progress.Count > 0) {
                this.progressLabel.Text = string.Format("{0}/{1}", Progress.Current, Progress.Count);
                this.progressBar.Value = Math.Min(Convert.ToInt32(Progress.Current * 100 / Progress.Count), 100);
            }
            if (timeEnable == false) {
                this.timerProgress.Enabled = false;
                SetEnable(true);
            }
        }
        //刷新多国语言
        private void buttonRefreshLanguage_Click(object sender, EventArgs e) {
            StartRun(() => {
                BuildLanguage(null, false);
                EndRun();
            });
        }
        private void BuildLanguage(Dictionary<string, LanguageTable> tables, bool build) {
            string allLanguages = ConversionUtil.GetConfig(ConfigKey.AllLanguage, ConfigFile.LanguageConfig);
            string languageDirectory = ConversionUtil.GetConfig(ConfigKey.LanguageDirectory, ConfigFile.LanguageConfig);
            LanguageBuilder builder = new LanguageBuilder(
                allLanguages,
                languageDirectory,
                ConversionUtil.GetConfig(ConfigKey.TranslationDirectory, ConfigFile.LanguageConfig),
                tables);
            if (build) {
                builder.Build();
            } else {
                builder.RefreshLanguage();
            }
            string[] languages = allLanguages.Split(';');
            List<string> languageFiles = new List<string>();
            foreach (var language in languages) {
                languageFiles.Add(languageDirectory + "/Language_" + language + ".xls");
            }
            new TableBuilder().Transform(string.Join(";", languageFiles.ToArray()),
                textTableConfig.Text,
                packageText.Text,
                ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
                false,
                refreshNote.Checked,
                GetProgramConfig());
        }
        //打开多国语言配置界面
        private void buttonLanguage_Click(object sender, EventArgs e)
        {
            m_FormLanguage.Show();
        }
        //打开日志界面
        private void buttonLog_Click(object sender, EventArgs e)
        {
            FormLog.GetInstance().Show();
        }
        //设置代码路径
        private void buttonCode_Click(object sender, EventArgs e)
        {
            ShowPathForm(m_Program, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
        }
        //设置Data路径
        private void buttonData_Click(object sender, EventArgs e)
        {
            ShowPathForm(m_Program, ConfigKey.DataDirectory, ConfigFile.PathConfig);
        }
        //设置批量关键字
        private void buttonSpwan_Click(object sender, EventArgs e)
        {
            ShowPathForm(PROGRAM.NONE, ConfigKey.SpawnList, ConfigFile.InitConfig);
        }
        //显示设置界面
        private void ShowPathForm(PROGRAM program, string key, ConfigFile file)
        {
            var form = new FormPath();
            form.Initialize(program, key, file);
            form.Show();
        }
        private void programBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Program = (PROGRAM)Enum.Parse(typeof(PROGRAM), programBox.Text);
            checkCreate.Checked = Util.ToBoolean(ConversionUtil.GetConfig(m_Program, ConfigKey.Create, ConfigFile.PathConfig), true);
            checkCompress.Checked = Util.ToBoolean(ConversionUtil.GetConfig(m_Program, ConfigKey.Compress, ConfigFile.InitConfig), false);
        }
        private void checkCreate_CheckedChanged(object sender, EventArgs e)
        {
            ConversionUtil.SetConfig(m_Program, ConfigKey.Create, checkCreate.Checked ? "true" : "false", ConfigFile.PathConfig);
        }
        private void checkCompress_CheckedChanged(object sender, EventArgs e)
        {
            ConversionUtil.SetConfig(m_Program, ConfigKey.Compress, checkCompress.Checked ? "true" : "false", ConfigFile.InitConfig);
        }
        private void buttonMessage_Click(object sender, EventArgs e)
        {
            StartRun(() => {
                new MessageBuilder().Transform(textMessage.Text,
                    packageText.Text, 
                    GetProgramConfig());
                EndRun();
            });
        }
        private void buttonDatabase_Click(object sender, EventArgs e)
        {
            StartRun(() => {
                new DatabaseBuilder().Transform(textDatabase.Text,
                    packageText.Text,
                    ConversionUtil.GetConfig(ConfigKey.DatabaseConfigDirectory, ConfigFile.PathConfig),
                    GetProgramConfig());
                EndRun();
            });
        }
        private void buttonDatabaseConfig_Click(object sender, EventArgs e)
        {
            ShowPathForm(PROGRAM.NONE, ConfigKey.DatabaseConfigDirectory, ConfigFile.PathConfig);
        }
        private void clearPath_Click(object sender, EventArgs e)
        {
            StartRun(() => {
                var configs = GetProgramConfig();
                foreach (var config in configs) {
                    foreach (var path in config.Value.CodeDirectory.Split(';')) {
                        if (!string.IsNullOrEmpty(path)) {
                            FileUtil.DeleteFiles(path, "*", true);
                            Logger.info("清空目录 [{0}] [{1}]", config.Key, path);
                        }
                    }
                    foreach (var path in config.Value.DataDirectory.Split(';')) {
                        if (!string.IsNullOrEmpty(path)) {
                            FileUtil.DeleteFiles(path, "*", true);
                            Logger.info("清空目录 [{0}] [{1}]", config.Key, path);
                        }
                    }
                }
                EndRun();
            });
        }

        private void buttonTinyPNG_Click(object sender, EventArgs e)
        {
            m_FormTinyPNG.Show();
        }
    }
}
