namespace ScorpioConversion {
    partial class FormWorkspace {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWorkspace));
            this.label1 = new System.Windows.Forms.Label();
            this.comboWorkspace = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkDefault = new System.Windows.Forms.CheckBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "工作目录";
            // 
            // comboWorkspace
            // 
            this.comboWorkspace.FormattingEnabled = true;
            this.comboWorkspace.Location = new System.Drawing.Point(81, 26);
            this.comboWorkspace.Name = "comboWorkspace";
            this.comboWorkspace.Size = new System.Drawing.Size(346, 20);
            this.comboWorkspace.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(352, 87);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkDefault
            // 
            this.checkDefault.AutoSize = true;
            this.checkDefault.Location = new System.Drawing.Point(12, 91);
            this.checkDefault.Name = "checkDefault";
            this.checkDefault.Size = new System.Drawing.Size(108, 16);
            this.checkDefault.TabIndex = 3;
            this.checkDefault.Text = "默认使用此目录";
            this.checkDefault.UseVisualStyleBackColor = true;
            this.checkDefault.CheckedChanged += new System.EventHandler(this.checkDefault_CheckedChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(271, 87);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormWorkspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 120);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.checkDefault);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboWorkspace);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWorkspace";
            this.Text = "选择工作目录";
            this.Load += new System.EventHandler(this.FormWorkspace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboWorkspace;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkDefault;
        private System.Windows.Forms.Button buttonClose;
    }
}