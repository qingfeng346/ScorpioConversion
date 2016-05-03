namespace ScorpioConversion
{
    partial class FormTiny
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
            this.buttonTransform = new System.Windows.Forms.Button();
            this.TextBoxSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxTarget = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxApiKey = new System.Windows.Forms.TextBox();
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
            // TextBoxSource
            // 
            this.TextBoxSource.Location = new System.Drawing.Point(95, 51);
            this.TextBoxSource.Name = "TextBoxSource";
            this.TextBoxSource.Size = new System.Drawing.Size(214, 21);
            this.TextBoxSource.TabIndex = 1;
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
            // TextBoxTarget
            // 
            this.TextBoxTarget.Location = new System.Drawing.Point(95, 89);
            this.TextBoxTarget.Name = "TextBoxTarget";
            this.TextBoxTarget.Size = new System.Drawing.Size(214, 21);
            this.TextBoxTarget.TabIndex = 4;
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
            // TextBoxApiKey
            // 
            this.TextBoxApiKey.Location = new System.Drawing.Point(95, 12);
            this.TextBoxApiKey.Name = "TextBoxApiKey";
            this.TextBoxApiKey.Size = new System.Drawing.Size(214, 21);
            this.TextBoxApiKey.TabIndex = 7;
            // 
            // FormTiny
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 124);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextBoxApiKey);
            this.Controls.Add(this.TextBoxTarget);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxSource);
            this.Controls.Add(this.buttonTransform);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormTiny";
            this.Text = "TinyPNG";
            this.Load += new System.EventHandler(this.FormTiny_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTransform;
        private System.Windows.Forms.TextBox TextBoxSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxTarget;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxApiKey;
    }
}

