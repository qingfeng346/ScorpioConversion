namespace ScorpioConversion
{
    partial class FormTinyPNG
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonTransform = new System.Windows.Forms.Button();
            this.textSourcePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textTargetPath = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusWorking = new System.Windows.Forms.ToolStripStatusLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.textApiKey = new System.Windows.Forms.TextBox();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTransform
            // 
            this.buttonTransform.Location = new System.Drawing.Point(330, 12);
            this.buttonTransform.Name = "buttonTransform";
            this.buttonTransform.Size = new System.Drawing.Size(99, 98);
            this.buttonTransform.TabIndex = 0;
            this.buttonTransform.Text = "开始转换";
            this.buttonTransform.UseVisualStyleBackColor = true;
            this.buttonTransform.Click += new System.EventHandler(this.buttonTransform_Click);
            // 
            // textSourcePath
            // 
            this.textSourcePath.Location = new System.Drawing.Point(95, 51);
            this.textSourcePath.Name = "textSourcePath";
            this.textSourcePath.Size = new System.Drawing.Size(214, 21);
            this.textSourcePath.TabIndex = 1;
            this.textSourcePath.TextChanged += new System.EventHandler(this.textSourcePath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "图片所在目录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "导出目录";
            // 
            // textTargetPath
            // 
            this.textTargetPath.Location = new System.Drawing.Point(95, 89);
            this.textTargetPath.Name = "textTargetPath";
            this.textTargetPath.Size = new System.Drawing.Size(214, 21);
            this.textTargetPath.TabIndex = 4;
            this.textTargetPath.TextChanged += new System.EventHandler(this.textTargetPath_TextChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusProgress,
            this.toolStripStatusWorking});
            this.statusStrip1.Location = new System.Drawing.Point(0, 120);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(441, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusProgress
            // 
            this.toolStripStatusProgress.Name = "toolStripStatusProgress";
            this.toolStripStatusProgress.Size = new System.Drawing.Size(91, 17);
            this.toolStripStatusProgress.Text = "进度：100/100";
            // 
            // toolStripStatusWorking
            // 
            this.toolStripStatusWorking.Name = "toolStripStatusWorking";
            this.toolStripStatusWorking.Size = new System.Drawing.Size(115, 17);
            this.toolStripStatusWorking.Text = "正在处理：aaa.png";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "API Key";
            // 
            // textApiKey
            // 
            this.textApiKey.Location = new System.Drawing.Point(95, 12);
            this.textApiKey.Name = "textApiKey";
            this.textApiKey.Size = new System.Drawing.Size(214, 21);
            this.textApiKey.TabIndex = 7;
            this.textApiKey.TextChanged += new System.EventHandler(this.textApiKey_TextChanged);
            // 
            // timerProgress
            // 
            this.timerProgress.Interval = 10;
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // FormTinyPNG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 142);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textApiKey);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textTargetPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textSourcePath);
            this.Controls.Add(this.buttonTransform);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormTinyPNG";
            this.Text = "TinyPNG";
            this.Load += new System.EventHandler(this.FormTinyPNG_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTransform;
        private System.Windows.Forms.TextBox textSourcePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textTargetPath;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusProgress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusWorking;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textApiKey;
        private System.Windows.Forms.Timer timerProgress;
    }
}

