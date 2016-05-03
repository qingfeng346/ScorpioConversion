using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Text;
using Gtk;
using ScorpioConversion;
public partial class MainWindow: Gtk.Window
{
	private Thread m_Thread;
	private StringBuilder builder = new StringBuilder();
	public MainWindow () : base (Gtk.WindowType.Toplevel) {
		Build ();
		Logger.SetLogger (new LibLogger ());
		Init ();
		ShowAll ();
		CheckBox ();
	}
	private void Init() {
		for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
			var languageWidget = new LanguageWidget ();
			languageWidget.SetProgram (((PROGRAM)i));
			CodeBox.Add (languageWidget);
		}
		ConversionUtil.Bind (packageText, ConfigKey.PackageName, ConfigFile.InitConfig);
		ConversionUtil.Bind (textTableConfig, ConfigKey.TableConfigPath, ConfigFile.InitConfig);
		ConversionUtil.Bind (textTableFolder, ConfigKey.TableFolderPath, ConfigFile.InitConfig);
		ConversionUtil.Bind (textMessage, ConfigKey.MessagePath, ConfigFile.InitConfig);
		ConversionUtil.Bind (textDatabaseConfig, ConfigKey.DatabaseConfigDirectory, ConfigFile.InitConfig);
		ConversionUtil.Bind (textSpawns, ConfigKey.SpawnList, ConfigFile.InitConfig);
		ConversionUtil.Bind (textDatabase, ConfigKey.DatabasePath, ConfigFile.InitConfig);

		ConversionUtil.Bind (TextBoxAllLanguages, ConfigKey.AllLanguage, ConfigFile.LanguageConfig);
		ConversionUtil.Bind (TextBoxLanguage, ConfigKey.LanguageDirectory, ConfigFile.LanguageConfig);
		ConversionUtil.Bind (TextBoxTranslation, ConfigKey.TranslationDirectory, ConfigFile.LanguageConfig);
		m_Thread = new Thread (ThreadFunc);
		m_Thread.Start ();
	}
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		m_Thread.Abort ();
		Application.Quit ();
		a.RetVal = true;
	}
	void ThreadFunc() {
		while (true) {
			Thread.Sleep (10);
			Application.Invoke ((sender, e) => {
				if (Progress.Count > 0) {
					this.progressBar.Text = string.Format ("{0}/{1}", Progress.Current, Progress.Count);
					this.progressBar.Fraction = Convert.ToDouble (Progress.Current) / Convert.ToDouble (Progress.Count);
				}
				lock (ConversionLogger.OutMessage) {
					FileStream stream = new FileStream (ConversionUtil.CurrentDirectory + "log.log", FileMode.Append, FileAccess.Write);
					while (ConversionLogger.OutMessage.Count > 0) {
						LogValue value = ConversionLogger.OutMessage.Dequeue ();
						string str = DateTime.Now.ToString () + "  [" + value.type + "]" + value.message + "\r\n";
						TextIter iter = TextIter.Zero;
						builder.Append(str);
						TextViewLog.Buffer.Text = builder.ToString();
						TextViewLog.ScrollToIter(TextViewLog.Buffer.EndIter, 0, true, 0, 0);
						byte[] buffer = Encoding.UTF8.GetBytes (str);
						stream.Write (buffer, 0, buffer.Length);
					}
					stream.Close ();
				}
			});
		}
	}
	void StartRun(ThreadStart start)
	{
		builder.Clear ();
		new Thread(start).Start();
	}
	void EndRun()
	{
		Application.Invoke ((sender, e) => {});
	}
	protected void OnToggleCode (object sender, EventArgs e) {
		CheckBox ();
	}
	protected void OnToggleLanguage (object sender, EventArgs e) {
		CheckBox ();
	}
	protected void OnToggleSpawn (object sender, EventArgs e) {
		CheckBox ();
	}
	private void CheckBox() {
		this.CodeBox.Visible = this.ToggleCode.Active;
		this.LanguageBox.Visible = this.ToggleLanguage.Active;
		this.SpawnBox.Visible = this.ToggleSpawn.Active;
	}
	protected void buttonTransformFolder_Click (object sender, EventArgs e)
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
					getManager.Active,
					refreshNote.Active,
					ConversionUtil.GetProgramConfig());
				if (Language.Active) {
					BuildLanguage(mTables, true);
				}
			} catch (Exception ex) {
				Logger.error("TransformFolder is error : " + ex.ToString());
			}
			EndRun();
		});
	}

	protected void selectTransformFiles_Click (object sender, EventArgs e)
	{
		Gtk.FileChooserDialog dialog = new Gtk.FileChooserDialog ("请选择要转换的文件", this, FileChooserAction.Open,
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
	}

	protected void buttonTransform_Click (object sender, EventArgs e)
	{
		StartRun(() => {
			new TableBuilder().Transform(this.textTransformFiles.Text, 
				textTableConfig.Text, 
				packageText.Text, 
				ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
				getManager.Active,
				refreshNote.Active,
				ConversionUtil.GetProgramConfig());
			EndRun();
		});
	}

	protected void selectRollbackFiles_Click (object sender, EventArgs e)
	{
		Gtk.FileChooserDialog dialog = new Gtk.FileChooserDialog ("请选择要转换的文件", this, FileChooserAction.Open,
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
	}

	protected void buttonRollback_Click (object sender, EventArgs e)
	{
		StartRun(() => {
			new TableBuilder().Rollback(this.textRollbackFiles.Text);
			EndRun();
		});
	}

	protected void buttonMessage_Click (object sender, EventArgs e)
	{
		StartRun(() => {
			new MessageBuilder().Transform(textMessage.Text,
				packageText.Text, 
				ConversionUtil.GetProgramConfig());
			EndRun();
		});
	}

	protected void buttonDatabase_Click (object sender, EventArgs e)
	{
		StartRun(() => {
			new DatabaseBuilder().Transform(textDatabaseConfig.Text,
				packageText.Text,
				ConversionUtil.GetConfig(ConfigKey.DatabaseConfigDirectory, ConfigFile.PathConfig),
				ConversionUtil.GetProgramConfig());
			EndRun();
		});
	}
	protected void buttonRefreshLanguage_Click (object sender, EventArgs e) {
		StartRun (() => {
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
			refreshNote.Active,
			ConversionUtil.GetProgramConfig());
	}
	protected void OnClickTinyPNG (object sender, EventArgs e)
	{
		TinyWindow window = new TinyWindow ();
		window.Show ();
	}
}
