using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
#if MONO_GTK
using Gtk;
#else
using System.Windows.Forms;
#endif

namespace ScorpioConversion {
    public partial class FormMain {
		private StringBuilder builder = new StringBuilder();
        private Dictionary<PROGRAM, CodeControl> controls = new Dictionary<PROGRAM, CodeControl>();
        private void Init() {
            Logger.SetLogger(new LibLogger());
			#if MONO_GTK
			for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
				var codeControl = new CodeControl ();
				codeControl.SetProgram (((PROGRAM)i));
				CodeBox.Add (codeControl);
			}
			#else
            CodeFoldPanel.AutoSize = true;
            for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
                var codeControl = new CodeControl();
                codeControl.Dock = DockStyle.Bottom;
                CodeFoldPanel.Controls.Add(codeControl);
                controls[(PROGRAM)i] = codeControl;
            }
			#endif
        }
        private void Bind() {
            ConversionUtil.Cleanup();
            ConversionUtil.Bind(textBoxPackage, ConfigKey.PackageName, ConfigFile.InitConfig);
            ConversionUtil.Bind(textBoxSpawns, ConfigKey.SpawnList, ConfigFile.InitConfig);

            ConversionUtil.Bind(textTableConfig, ConfigKey.TableConfigPath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textTableFolder, ConfigKey.TableFolderPath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textMessage, ConfigKey.MessagePath, ConfigFile.InitConfig);
            ConversionUtil.Bind(textDatabaseConfig, ConfigKey.DatabaseConfigDirectory, ConfigFile.InitConfig);

            ConversionUtil.Bind(textBoxAllLanguages, ConfigKey.AllLanguage, ConfigFile.LanguageConfig);
            ConversionUtil.Bind(textBoxTranslation, ConfigKey.TranslationDirectory, ConfigFile.LanguageConfig);
            ConversionUtil.Bind(textBoxLanguage, ConfigKey.LanguageDirectory, ConfigFile.LanguageConfig);
            foreach (var pair in controls) {
                pair.Value.SetProgram(pair.Key);
            } 

        }
        private void Tick() {
            if (Progress.Count > 0) {
				#if MONO_GTK
				this.progressBar.Text = string.Format ("{0}/{1}", Progress.Current, Progress.Count);
				this.progressBar.Fraction = Convert.ToDouble (Progress.Current) / Convert.ToDouble (Progress.Count);
				#else
                this.progressLabel.Text = string.Format("{0}/{1}", Progress.Current, Progress.Count);
                this.progressBar.Value = Math.Min(Convert.ToInt32(Progress.Current * 100 / Progress.Count), 100);
				#endif
			}
            lock (ConversionLogger.OutMessage) {
                using (FileStream stream = new FileStream(ConversionUtil.GetPath("log.log"), FileMode.Append, FileAccess.Write)) {
                    while (ConversionLogger.OutMessage.Count > 0) {
                        LogValue value = ConversionLogger.OutMessage.Dequeue();
                        string str = DateTime.Now.ToString() + "  [" + value.type + "]" + value.message + "\r\n";
#if MONO_GTK
					    TextIter iter = TextIter.Zero;
					    builder.Append(str);
					    richTextBoxLog.Buffer.Text = builder.ToString();
					    richTextBoxLog.ScrollToIter(richTextBoxLog.Buffer.EndIter, 0, true, 0, 0);
#else
                        richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                        if (value.type == LogType.Info)
                            richTextBoxLog.SelectionColor = System.Drawing.Color.Black;
                        else if (value.type == LogType.Warn)
                            richTextBoxLog.SelectionColor = System.Drawing.Color.Red;
                        else
                            richTextBoxLog.SelectionColor = System.Drawing.Color.Red;
                        richTextBoxLog.AppendText(str);
                        richTextBoxLog.ScrollToCaret();
#endif
                        byte[] buffer = Encoding.UTF8.GetBytes(str);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        void StartRun(ThreadStart start) {
            builder = new StringBuilder();
            new Thread(() => {
                lock(this) {
                    try {
                        if (start != null) start();
                    } catch (System.Exception ex) {
                        Logger.error(string.Format("Run is Error method : {0}  error : {1}", start.ToString(), ex.ToString()));
                    }
                }
                EndRun();
            }).Start();
        }
        void EndRun() {
        }
        private void buttonTransformFolder_Click(object sender, EventArgs e) {
            StartRun(() => {
                if (string.IsNullOrEmpty(textTableFolder.Text)) return;
                var files = Directory.GetFiles(ConversionUtil.GetPath(textTableFolder.Text), "*.xls", SearchOption.AllDirectories);
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
                    ConversionUtil.GetPath(textTableConfig.Text),
                    textBoxPackage.Text,
                    ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
                    Extends.GetChecked(getManager),
                    Extends.GetChecked(refreshNote),
                    ConversionUtil.GetProgramConfig());
                if (Extends.GetChecked(Language)) {
                    BuildLanguage(mTables, true);
                }
            });
        }

        private void selectTransformFiles_Click(object sender, EventArgs e) {
			#if MONO_GTK
			FileChooserDialog dialog = new FileChooserDialog ("请选择要转换的文件", this, FileChooserAction.Open,
				"关闭", ResponseType.Cancel, "打开", ResponseType.Accept);
			dialog.SelectMultiple = true;
			dialog.SetCurrentFolder(ConversionUtil.GetConfig(ConfigKey.TransformDirectory, ConfigFile.PathConfig));
			FileFilter filter = new FileFilter ();
			filter.Name = "Excel文件";
			filter.AddPattern ("*.xls");
			dialog.Filter = filter;
			if (dialog.Run() == (int)ResponseType.Accept) {
				ConversionUtil.SetConfig(ConfigKey.TransformDirectory, System.IO.Path.GetDirectoryName(dialog.Filename), ConfigFile.PathConfig);
				string strText = string.Join(";", dialog.Filenames);
				this.textTransformFiles.Text = strText;
				ConversionUtil.SetToolTip(textTransformFiles, "一共 " + dialog.Filenames.Length + " 个文件\n" + strText.Replace(";", "\n"));
				this.progressBar.Text = string.Format("{0}/{1}", 1, dialog.Filenames.Length);
			}
			dialog.Destroy ();
			#else
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.InitialDirectory = ConversionUtil.GetPath(ConversionUtil.GetConfig(ConfigKey.TransformDirectory, ConfigFile.PathConfig));
            dialog.Filter = "Excel文件|*.xls;|所有文件|*.*";
            dialog.Title = "请选择要转换的文件";
            if (dialog.ShowDialog() == DialogResult.OK) {
                ConversionUtil.SetConfig(ConfigKey.TransformDirectory, Path.GetDirectoryName(dialog.FileName), ConfigFile.PathConfig);
                string strText = string.Join(";", dialog.FileNames);
                this.textTransformFiles.Text = strText;
                ConversionUtil.SetToolTip(textTransformFiles, strText.Replace(";", "\n"));
                this.progressLabel.Text = string.Format("{0}/{1}", 1, dialog.FileNames.Length);
            }
			#endif
        }

        private void buttonTransform_Click(object sender, EventArgs e) {
            StartRun(() => {
                new TableBuilder().Transform(this.textTransformFiles.Text,
                    ConversionUtil.GetPath(textTableConfig.Text),
                    textBoxPackage.Text,
                    ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
                    Extends.GetChecked(getManager),
                    Extends.GetChecked(refreshNote),
                    ConversionUtil.GetProgramConfig());
            });
        }
        private void BuildLanguage(Dictionary<string, LanguageTable> tables, bool build) {
            string allLanguages = ConversionUtil.GetConfig(ConfigKey.AllLanguage, ConfigFile.LanguageConfig);
            string languageDirectory = ConversionUtil.GetPath(ConversionUtil.GetConfig(ConfigKey.LanguageDirectory, ConfigFile.LanguageConfig));
            LanguageBuilder builder = new LanguageBuilder(
                allLanguages,
                languageDirectory,
                ConversionUtil.GetPath(ConversionUtil.GetConfig(ConfigKey.TranslationDirectory, ConfigFile.LanguageConfig)),
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
                Extends.GetChecked(refreshNote),
                ConversionUtil.GetProgramConfig());
        }

        private void selectRollbackFiles_Click(object sender, EventArgs e) {
			#if MONO_GTK
			FileChooserDialog dialog = new FileChooserDialog ("请选择要转换的文件", this, FileChooserAction.Open,
				"关闭", ResponseType.Cancel, "打开", ResponseType.Accept);
			dialog.SelectMultiple = true;
			dialog.SetCurrentFolder(ConversionUtil.GetConfig(ConfigKey.TransformDirectory, ConfigFile.PathConfig));
			FileFilter filter = new FileFilter ();
			filter.Name = "Data文件";
			filter.AddPattern ("*.data");
			dialog.Filter = filter;
			if (dialog.Run() == (int)ResponseType.Accept) {
				ConversionUtil.SetConfig(ConfigKey.RollbackDirectory, System.IO.Path.GetDirectoryName(dialog.Filename), ConfigFile.PathConfig);
				string strText = "";
				for (int i = 0; i < dialog.Filenames.Length; ++i)
					strText += (dialog.Filenames[i] + ";");
				this.textRollbackFiles.Text = strText;
				ConversionUtil.SetToolTip(textTransformFiles, "一共 " + dialog.Filenames.Length + " 个文件\n" + strText.Replace(";", "\n"));
				this.progressBar.Text = string.Format("{0}/{1}", 1, dialog.Filenames.Length);
			}
			dialog.Destroy ();
			#else
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Multiselect = true;
			dialog.InitialDirectory = ConversionUtil.GetPath(ConversionUtil.GetConfig(ConfigKey.RollbackDirectory, ConfigFile.PathConfig));
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
			#endif 
        }

        private void buttonRollback_Click(object sender, EventArgs e) {
            StartRun(() => {
                new TableBuilder().Rollback(this.textRollbackFiles.Text);
            });
        }

        private void buttonMessage_Click(object sender, EventArgs e) {
            StartRun(() => {
                if (string.IsNullOrEmpty(textMessage.Text)) { return; }
                new MessageBuilder().Transform(ConversionUtil.GetPath(textMessage.Text),
                    textBoxPackage.Text,
                    ConversionUtil.GetProgramConfig());
            });
        }

        private void buttonDatabase_Click(object sender, EventArgs e) {
            StartRun(() => {
                if (string.IsNullOrEmpty(textDatabaseConfig.Text)) { return; }
                new DatabaseBuilder().Transform(ConversionUtil.GetPath(textDatabaseConfig.Text),
                    textBoxPackage.Text,
                    ConversionUtil.GetProgramConfig());
            });
        }

        private void buttonRefreshLanguage_Click(object sender, EventArgs e) {
            StartRun(() => {
                BuildLanguage(null, false);
            });
        }
    }
}
