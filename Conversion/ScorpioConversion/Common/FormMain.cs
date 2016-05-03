using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
#if MONO_GTK

#else
using System.Windows.Forms;
#endif

namespace ScorpioConversion {
    public partial class FormMain {
        private void Init() {
            Logger.SetLogger(new LibLogger());
            CodeFoldPanel.AutoSize = true;
            for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
                var codeControl = new CodeControl();
                codeControl.Dock = DockStyle.Bottom;
                codeControl.SetProgram(((PROGRAM)i));
                CodeFoldPanel.Controls.Add(codeControl);
            }
            ConversionUtil.Bind(textBoxPackage, ConfigKey.PackageName, ConfigFile.InitConfig);
            ConversionUtil.Bind(textBoxDatabase, ConfigKey.DatabasePath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textBoxSpawns, ConfigKey.SpawnList, ConfigFile.InitConfig);

            ConversionUtil.Bind(textTableConfig, ConfigKey.TableConfigPath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textTableFolder, ConfigKey.TableFolderPath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textMessage, ConfigKey.MessagePath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textDatabaseConfig, ConfigKey.DatabaseConfigDirectory, ConfigFile.InitConfig);

            ConversionUtil.Bind(textBoxAllLanguages, ConfigKey.AllLanguage, ConfigFile.LanguageConfig);
            ConversionUtil.Bind(textBoxTranslation, ConfigKey.TranslationDirectory, ConfigFile.LanguageConfig);
            ConversionUtil.Bind(textBoxLanguage, ConfigKey.LanguageDirectory, ConfigFile.LanguageConfig);
        }
        private void Tick() {
            if (Progress.Count > 0) {
                this.progressLabel.Text = string.Format("{0}/{1}", Progress.Current, Progress.Count);
                this.progressBar.Value = Math.Min(Convert.ToInt32(Progress.Current * 100 / Progress.Count), 100);
            }
            lock (ConversionLogger.OutMessage) {
                FileStream stream = new FileStream(ConversionUtil.CurrentDirectory + "log.log", FileMode.Append, FileAccess.Write);
                while (ConversionLogger.OutMessage.Count > 0) {
                    LogValue value = ConversionLogger.OutMessage.Dequeue();
                    richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                    if (value.type == LogType.Info)
                        richTextBoxLog.SelectionColor = System.Drawing.Color.Black;
                    else if (value.type == LogType.Warn)
                        richTextBoxLog.SelectionColor = System.Drawing.Color.Red;
                    else
                        richTextBoxLog.SelectionColor = System.Drawing.Color.Red;
                    string str = DateTime.Now.ToString() + "  [" + value.type + "]" + value.message + "\r\n";
                    richTextBoxLog.AppendText(str);
                    richTextBoxLog.ScrollToCaret();
                    byte[] buffer = Encoding.UTF8.GetBytes(str);
                    stream.Write(buffer, 0, buffer.Length);
                }
                stream.Close();
            }
        }
        void StartRun(ThreadStart start) {
            new Thread(start).Start();
        }
        void EndRun() {
        }
        private void buttonTransformFolder_Click(object sender, EventArgs e) {
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
                        textBoxPackage.Text,
                        ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
						getManager.GetChecked(),
						refreshNote.GetChecked(),
                        ConversionUtil.GetProgramConfig());
					if (Language.GetChecked()) {
                        BuildLanguage(mTables, true);
                    }
                } catch (Exception ex) {
                    Logger.error("TransformFolder is error : " + ex.ToString());
                }
                EndRun();
            });
        }

        private void selectTransformFiles_Click(object sender, EventArgs e) {
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

        private void buttonTransform_Click(object sender, EventArgs e) {
            StartRun(() => {
                new TableBuilder().Transform(this.textTransformFiles.Text,
                    textTableConfig.Text,
                    textBoxPackage.Text,
                    ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
					getManager.GetChecked(),
					refreshNote.GetChecked(),
                    ConversionUtil.GetProgramConfig());
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
                textBoxPackage.Text,
                ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
                false,
				refreshNote.GetChecked(),
                ConversionUtil.GetProgramConfig());
        }

        private void selectRollbackFiles_Click(object sender, EventArgs e) {
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

        private void buttonRollback_Click(object sender, EventArgs e) {
            StartRun(() => {
                new TableBuilder().Rollback(this.textRollbackFiles.Text);
                EndRun();
            });
        }

        private void buttonMessage_Click(object sender, EventArgs e) {
            StartRun(() => {
                new MessageBuilder().Transform(textMessage.Text,
                    textBoxPackage.Text,
                    ConversionUtil.GetProgramConfig());
                EndRun();
            });
        }

        private void buttonDatabase_Click(object sender, EventArgs e) {
            StartRun(() => {
                new DatabaseBuilder().Transform(textBoxDatabase.Text,
                    textBoxPackage.Text,
                    ConversionUtil.GetConfig(ConfigKey.DatabaseConfigDirectory, ConfigFile.PathConfig),
                    ConversionUtil.GetProgramConfig());
                EndRun();
            });
        }

        private void buttonRefreshLanguage_Click(object sender, EventArgs e) {
            StartRun(() => {
                BuildLanguage(null, false);
                EndRun();
            });
        }
    }
}
