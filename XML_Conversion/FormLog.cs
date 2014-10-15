using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace XML_Conversion
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
        }
        private void FormLog_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }
        private void FormLog_SizeChanged(object sender, EventArgs e)
        {
            this.richTextBox1.Size = new Size(this.Size.Width - 25, this.Size.Height - 75);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (Logger.OutMessage)
            {
                FileStream stream = new FileStream(Util.BaseDirectory + "log.log", FileMode.Append, FileAccess.Write);
                while (Logger.OutMessage.Count > 0)
                {
                    LogValue value = Logger.OutMessage.Dequeue();
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    if (value.type == LogType.INFO)
                        richTextBox1.SelectionColor = System.Drawing.Color.Black;
                    else if (value.type == LogType.WARNING)
                        richTextBox1.SelectionColor = System.Drawing.Color.Red;
                    else
                        richTextBox1.SelectionColor = System.Drawing.Color.Red;
                    string str = DateTime.Now.ToString() + "  [" + value.type + "]" + value.message + "\r\n";
                    richTextBox1.AppendText(str);
                    richTextBox1.ScrollToCaret();
                    byte[] buffer = Encoding.UTF8.GetBytes(str);
                    stream.Write(buffer, 0, buffer.Length);
                }
                stream.Close();
            }
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "请选择要保存的位置";
            dialog.FileName = "log.log";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileUtil.CreateFile(dialog.FileName, richTextBox1.Text);
            }
        }
    }
}
