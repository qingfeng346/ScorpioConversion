using System;

public partial class FormPath : Gtk.Window
{
	private PROGRAM m_Program;
	private string m_Key;
	private ConfigFile m_File;
	public FormPath () : base (Gtk.WindowType.Toplevel)
	{
		this.Build ();
		this.richTextBox1.Buffer.Changed += new System.EventHandler(richTextBox1_TextChanged);
	}
	public void Initialize(PROGRAM program, string key, ConfigFile file)
	{
		m_Program = program;
		m_Key = key;
		m_File = file;
		this.Title = m_Key + "[" + m_Program + "]" + "  分隔符为回车键!!!";
		this.richTextBox1.Buffer.Text = ConversionUtil.GetConfig(program, key, file).Replace(";", "\n");
	}
	private void richTextBox1_TextChanged(object sender, EventArgs e)
	{
		ConversionUtil.SetConfig(m_Program, m_Key, richTextBox1.Buffer.Text.Replace("\n", ";"), m_File);
	}
	private void button1_Click(object sender, EventArgs e)
	{
		string[] paths = richTextBox1.Buffer.Text.Split('\n');
		foreach (var path in paths) {
			System.Diagnostics.Process.Start("Explorer.exe", path);
		}
	}
}


