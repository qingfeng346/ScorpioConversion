namespace ScorpioConversion
{
    partial class FormLanguage
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
            this.textAll = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textLanguage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textTranslation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textAll
            // 
            this.textAll.Location = new System.Drawing.Point(110, 12);
            this.textAll.Name = "textAll";
            this.textAll.Size = new System.Drawing.Size(291, 21);
            this.textAll.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "所有多国语言";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Language目录";
            // 
            // textLanguage
            // 
            this.textLanguage.Location = new System.Drawing.Point(110, 66);
            this.textLanguage.Name = "textLanguage";
            this.textLanguage.Size = new System.Drawing.Size(291, 21);
            this.textLanguage.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Translation目录";
            // 
            // textTranslation
            // 
            this.textTranslation.Location = new System.Drawing.Point(110, 39);
            this.textTranslation.Name = "textTranslation";
            this.textTranslation.Size = new System.Drawing.Size(291, 21);
            this.textTranslation.TabIndex = 4;
            // 
            // FormLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 100);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textTranslation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textLanguage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormLanguage";
            this.Text = "多国语言设置";
            this.Load += new System.EventHandler(this.FormLanguage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textLanguage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textTranslation;
    }
}