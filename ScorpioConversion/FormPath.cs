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
            this.richTextBox1.Text = Util.GetConfig(program, key, file).Replace(";", "\n");
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            Util.SetConfig(m_Program, m_Key, richTextBox1.Text.Replace("\n", ";"), m_File);
        }
    }
}
