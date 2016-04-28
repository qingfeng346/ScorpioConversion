using System;
using System.Windows.Forms;

namespace ScorpioConversion {
    public partial class CodeControl : UserControl {
        private PROGRAM m_Program;
        public CodeControl() {
            InitializeComponent();
        }
        public void SetProgram(PROGRAM program) {
            m_Program = program;
            button.Text = program.ToString() + " 语言导出设置";
            ConversionUtil.Bind(CodePath, m_Program, ConfigKey.CodeDirectory, ConfigFile.PathConfig);
            ConversionUtil.Bind(DataPath, m_Program, ConfigKey.DataDirectory, ConfigFile.PathConfig);
            ConversionUtil.Bind(CheckCreate, m_Program, ConfigKey.Create, ConfigFile.PathConfig);
            ConversionUtil.Bind(CheckCompress, m_Program, ConfigKey.Compress, ConfigFile.PathConfig);
        }
        private void button_Click(object sender, EventArgs e) {
            this.panel.Visible = !this.panel.Visible;
        }
    }
}
