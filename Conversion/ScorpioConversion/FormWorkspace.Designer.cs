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
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkDefault = new System.Windows.Forms.CheckBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.listPaths = new System.Windows.Forms.ListBox();
            this.textWorkspace = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(520, 333);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkDefault
            // 
            this.checkDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkDefault.AutoSize = true;
            this.checkDefault.Location = new System.Drawing.Point(12, 337);
            this.checkDefault.Name = "checkDefault";
            this.checkDefault.Size = new System.Drawing.Size(108, 16);
            this.checkDefault.TabIndex = 3;
            this.checkDefault.Text = "默认使用此目录";
            this.checkDefault.UseVisualStyleBackColor = true;
            this.checkDefault.CheckedChanged += new System.EventHandler(this.checkDefault_CheckedChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(439, 333);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // listPaths
            // 
            this.listPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPaths.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.listPaths.FormattingEnabled = true;
            this.listPaths.ItemHeight = 12;
            this.listPaths.Location = new System.Drawing.Point(12, 17);
            this.listPaths.Name = "listPaths";
            this.listPaths.Size = new System.Drawing.Size(583, 280);
            this.listPaths.TabIndex = 5;
            // 
            // textWorkspace
            // 
            this.textWorkspace.Location = new System.Drawing.Point(12, 306);
            this.textWorkspace.Name = "textWorkspace";
            this.textWorkspace.Size = new System.Drawing.Size(583, 21);
            this.textWorkspace.TabIndex = 6;
            // 
            // FormWorkspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 368);
            this.Controls.Add(this.textWorkspace);
            this.Controls.Add(this.listPaths);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.checkDefault);
            this.Controls.Add(this.buttonOK);
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
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkDefault;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ListBox listPaths;
        private System.Windows.Forms.TextBox textWorkspace;
    }
}