using System;
using System.Windows.Forms;

namespace ScorpioConversion {
    public partial class CodeControl : UserControl {
        public CodeControl() {
            InitializeComponent();
        }
        private void SetProgram_impl() {

        }
        private void button_Click(object sender, EventArgs e) {
            this.panel.Visible = !this.panel.Visible;
        }
    }
}
