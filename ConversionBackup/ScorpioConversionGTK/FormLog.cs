using System;
using Gtk;
using System.IO;
using System.Text;
using System.Threading;
public partial class FormLog : Gtk.Window
{
	private static StringBuilder builder = new StringBuilder();
	private static FormLog instance = null;
	public static FormLog GetInstance()
	{
		if (instance == null)
			instance = new FormLog();
		return instance;
	}
	public FormLog () : base (Gtk.WindowType.Toplevel)
	{
		this.Build ();
		this.richTextBox1.Buffer.Text = builder.ToString();
	}
	public void CheckOutput() {
		Application.Invoke ((sender, e) => {
			lock (ConversionLogger.OutMessage) {
				FileStream stream = new FileStream (ConversionUtil.CurrentDirectory + "log.log", FileMode.Append, FileAccess.Write);
				while (ConversionLogger.OutMessage.Count > 0) {
					LogValue value = ConversionLogger.OutMessage.Dequeue ();
					string str = DateTime.Now.ToString () + "  [" + value.type + "]" + value.message + "\r\n";
					builder.Append (str);
					richTextBox1.Buffer.Text = builder.ToString ();
					byte[] buffer = Encoding.UTF8.GetBytes (str);
					stream.Write (buffer, 0, buffer.Length);
				}
				stream.Close ();
			}
		});
	}
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		instance = null;
	}
	protected void buttonClear_Click (object sender, EventArgs e)
	{
		Console.WriteLine ("22222222222222222222");
		builder.Clear ();
		this.richTextBox1.Buffer.Text = "";
	}
	protected void buttonExport_Click (object sender, EventArgs e)
	{
		Gtk.FileChooserDialog dialog = new Gtk.FileChooserDialog ("请选择要保存的位置", this, FileChooserAction.Save,
			"关闭", ResponseType.Cancel, "打开", ResponseType.Accept);
		dialog.SetFilename ("log.log");
		dialog.SetCurrentFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
		if (dialog.Run() == (int)ResponseType.Accept) {
			FileUtil.CreateFile(dialog.Filename, richTextBox1.Buffer.Text);
		}
		dialog.Destroy ();
	}
}

