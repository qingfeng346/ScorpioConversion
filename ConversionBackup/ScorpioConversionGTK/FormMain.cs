using System;
using System.Collections.Generic;
using Gtk;
using System.IO;
using System.Text;
using System.Threading;
public partial class FormMain: Gtk.Window
{
	private Thread m_Thread;
	private PROGRAM m_Program;              //当前选择的语言
	public FormMain () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		Logger.SetLogger (new LibLogger ());
		for (int i = (int)PROGRAM.NONE + 1; i < (int)PROGRAM.COUNT; ++i) {
			programBox.AppendText(global::Mono.Unix.Catalog.GetString (((PROGRAM)i).ToString()));
		}
		programBox.Active = 0;
		ConversionUtil.Bind (packageText, ConfigKey.PackageName, ConfigFile.InitConfig);
		ConversionUtil.Bind(textTableConfig, ConfigKey.TableConfigPath, ConfigFile.InitConfig);
		ConversionUtil.Bind(textTableFolder, ConfigKey.TableFolderPath, ConfigFile.InitConfig);
		ConversionUtil.Bind(textMessage, ConfigKey.MessagePath, ConfigFile.InitConfig);
		ConversionUtil.Bind(textDatabase, ConfigKey.DatabasePath, ConfigFile.InitConfig);
		var a = this.IsRealized;
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
			});
		}
	}
	void SetEnable(bool enable)
	{
		this.programBox.Sensitive = enable;
		this.buttonSpwan.Sensitive = enable;
		this.buttonCode.Sensitive = enable;
		this.buttonData.Sensitive = enable;
		this.checkCreate.Sensitive = enable;
		this.checkCompress.Sensitive = enable;
		this.selectTransformFiles.Sensitive = enable;
		this.buttonTransform.Sensitive = enable;
		this.selectRollbackFiles.Sensitive = enable;
		this.buttonRollback.Sensitive = enable;
		this.buttonLanguage.Sensitive = enable;
		this.buttonRefreshLanguage.Sensitive = enable;
		this.buttonMessage.Sensitive = enable;
		this.buttonDatabase.Sensitive = enable;
		this.textTableConfig.Sensitive = enable;
		this.buttonTransformFolder.Sensitive = enable;
		this.clearPath.Sensitive = enable;
		this.textTransformFiles.Sensitive = enable;
		this.textRollbackFiles.Sensitive = enable;
		this.textMessage.Sensitive = enable;
		this.textDatabase.Sensitive = enable;
		this.textTableFolder.Sensitive = enable;
		this.getManager.Sensitive = enable;
		this.Language.Sensitive = enable;
		this.refreshNote.Sensitive = enable;
		this.packageText.Sensitive = enable;
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
	void ShowPathForm(PROGRAM program, string key, ConfigFile file) {
		var form = new FormPath();
		form.Initialize(program, key, file);
		form.Show();
	}
	void StartRun(ThreadStart start)
	{
//		timeEnable = true;
//		this.timerProgress.Enabled = true;
		SetEnable(false);
		FormLog.GetInstance().Show();
		new Thread(start).Start();
	}
	void EndRun()
	{
		Application.Invoke ((sender, e) => {
			SetEnable (true);
		});
	}
	void programBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		m_Program = (PROGRAM)Enum.Parse(typeof(PROGRAM), programBox.ActiveText);
		checkCreate.Active = Util.ToBoolean(ConversionUtil.GetConfig(m_Program, ConfigKey.Create, ConfigFile.PathConfig), true);
		checkCompress.Active = Util.ToBoolean(ConversionUtil.GetConfig(m_Program, ConfigKey.Compress, ConfigFile.InitConfig), false);
	}
	//设置代码路径
	void buttonCode_Click(object sender, EventArgs e)
	{
		ShowPathForm(m_Program, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
	}
	//设置Data路径
	void buttonData_Click(object sender, EventArgs e)
	{
		ShowPathForm(m_Program, ConfigKey.DataDirectory, ConfigFile.PathConfig);
	}
	void buttonTransformFolder_Click(object sender, EventArgs e)
	{
		StartRun(() => {
			try {
				var files = Directory.GetFiles(textTableFolder.Text, "*.xls", SearchOption.AllDirectories);
				if (files.Length == 0)
					throw new Exception(string.Format("路径[{0}]下的文件数量为0", textTableFolder.Text));
				new TableBuilder().Transform(string.Join(";", files),
					textTableConfig.Text,
					packageText.Text,
					ConversionUtil.GetConfig(ConfigKey.SpawnList, ConfigFile.InitConfig),
					getManager.Active,
					refreshNote.Active,
					GetProgramConfig());
			} catch (Exception ex) {
				Logger.error("TransformFolder is error : " + ex.ToString());
			}
			EndRun();
		});
	}

	protected void buttonLog_Click (object sender, EventArgs e)
	{
		//throw new NotImplementedException ();
		FormLog.GetInstance().Show();
	}

	protected void buttonSpwan_Click (object sender, EventArgs e)
	{
		ShowPathForm(PROGRAM.NONE, ConfigKey.SpawnList, ConfigFile.InitConfig);
	}

	protected void buttonLanguage_Click (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}

	protected void buttonRefreshLanguage_Click (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
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
			ConversionUtil.SetToolTip(textTransformFiles, strText.Replace(";", "\n"));
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
				GetProgramConfig());
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
				GetProgramConfig());
			EndRun();
		});
	}

	protected void buttonDatabase_Click (object sender, EventArgs e)
	{
		StartRun(() => {
			new DatabaseBuilder().Transform(textDatabase.Text,
				packageText.Text,
				ConversionUtil.GetConfig(ConfigKey.DatabaseConfigDirectory, ConfigFile.PathConfig),
				GetProgramConfig());
			EndRun();
		});
	}
}
