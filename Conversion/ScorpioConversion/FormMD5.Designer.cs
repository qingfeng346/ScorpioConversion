namespace ScorpioConversion
{
    partial class FormMD5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBoxSource = new System.Windows.Forms.RichTextBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.checkBoxLower = new System.Windows.Forms.CheckBox();
            this.buttonCMD5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxSource
            // 
            this.richTextBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxSource.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxSource.Name = "richTextBoxSource";
            this.richTextBoxSource.Size = new System.Drawing.Size(479, 239);
            this.richTextBoxSource.TabIndex = 0;
            this.richTextBoxSource.Text = "";
            this.richTextBoxSource.TextChanged += new System.EventHandler(this.richTextBoxSource_TextChanged);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxResult.Location = new System.Drawing.Point(12, 257);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(412, 21);
            this.textBoxResult.TabIndex = 1;
            // 
            // checkBoxLower
            // 
            this.checkBoxLower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxLower.AutoSize = true;
            this.checkBoxLower.Location = new System.Drawing.Point(443, 260);
            this.checkBoxLower.Name = "checkBoxLower";
            this.checkBoxLower.Size = new System.Drawing.Size(48, 16);
            this.checkBoxLower.TabIndex = 2;
            this.checkBoxLower.Text = "小写";
            this.checkBoxLower.UseVisualStyleBackColor = true;
            this.checkBoxLower.CheckedChanged += new System.EventHandler(this.checkBoxLower_CheckedChanged);
            // 
            // buttonCMD5
            // 
            this.buttonCMD5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCMD5.Location = new System.Drawing.Point(12, 284);
            this.buttonCMD5.Name = "buttonCMD5";
            this.buttonCMD5.Size = new System.Drawing.Size(479, 23);
            this.buttonCMD5.TabIndex = 3;
            this.buttonCMD5.Text = "MD5解密网站";
            this.buttonCMD5.UseVisualStyleBackColor = true;
            this.buttonCMD5.Click += new System.EventHandler(this.buttonCMD5_Click);
            // 
            // FormMD5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 315);
            this.Controls.Add(this.buttonCMD5);
            this.Controls.Add(this.checkBoxLower);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.richTextBoxSource);
            this.Name = "FormMD5";
            this.Text = "FormMD5";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxSource;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.CheckBox checkBoxLower;
        private System.Windows.Forms.Button buttonCMD5;
    }
}