using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScorpioConversion {
    public partial class FormTime : Form {
        public FormTime() {
            InitializeComponent();
            dateTimePicker.Value = DateTime.Now;
        }
        //DateTime转换为时间戳
        public long GetTimeSpan(DateTime time) {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long)(time - startTime).TotalMilliseconds;
        }
        //timeSpan转换为DateTime
        public DateTime TimeSpanToDateTime(long span) {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return startTime.AddMilliseconds(span);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) {
            var str = GetTimeSpan(dateTimePicker.Value).ToString();
            if (textBoxTime.Text != str)
                textBoxTime.Text = str;
        }

        private void textBoxTime_TextChanged(object sender, EventArgs e) {
            long time = 0;
            if (long.TryParse(textBoxTime.Text, out time)) {
                var date = TimeSpanToDateTime(time);
                if (dateTimePicker.Value != date)
                    dateTimePicker.Value = date;
            } else {
                MessageBox.Show("请输入数字");
            }
        }

        private void buttonCurTime_Click(object sender, EventArgs e) {
            dateTimePicker.Value = DateTime.Now;
            Console.WriteLine(dateTimePicker.Value.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
    }
}
