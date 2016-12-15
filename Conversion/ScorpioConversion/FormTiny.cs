using System;
using System.Windows.Forms;
namespace ScorpioConversion {
    public partial class FormTiny : Form {
        private static FormTiny instance;
        public static FormTiny GetInstance() { return instance ?? (instance = new FormTiny()); }
        public FormTiny()
        {
            InitializeComponent();
            FormClosing += (sender, e) => { this.Hide(); e.Cancel = true; };
            VisibleChanged += (sender, e) => { ConversionUtil.CheckExit(); };
        }
        private void FormTiny_Load(object sender, EventArgs e)
        {
            Init();
        }
    }
}
