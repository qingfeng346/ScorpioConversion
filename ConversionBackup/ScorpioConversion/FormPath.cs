using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScorpioConversion
{
    public partial class FormPath : Form
    {
        private PROGRAM m_Program;
        private string m_Key;
        private ConfigFile m_File;
        public FormPath()
        {
            InitializeComponent();
        }
        public void Initialize(PROGRAM program, string key, ConfigFile file)
        {
            m_Program = program;
            m_Key = key;
            m_File = file;
            this.Text = m_Key + "[" + m_Program + "]" + "  分隔符为回车键!!!";
            this.richTextBox1.Text = ConversionUtil.GetConfig(program, key, file).Replace(";", "\n");
        }
        private void FormLog_SizeChanged(object sender, EventArgs e)
        {
            this.richTextBox1.Size = new Size(this.Size.Width - 25, this.Size.Height - 75);
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            ConversionUtil.SetConfig(m_Program, m_Key, richTextBox1.Text.Replace("\n", ";"), m_File);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string[] paths = richTextBox1.Text.Split('\n');
            foreach (var path in paths) {
                System.Diagnostics.Process.Start("Explorer.exe", path);
            }
        }
    }
}
