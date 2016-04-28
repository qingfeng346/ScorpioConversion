namespace ScorpioConversion {
    partial class CodeControl {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.button = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CodePath = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DataPath = new System.Windows.Forms.RichTextBox();
            this.CheckCreate = new System.Windows.Forms.CheckBox();
            this.CheckCompress = new System.Windows.Forms.CheckBox();
            this.panel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Dock = System.Windows.Forms.DockStyle.Top;
            this.button.Location = new System.Drawing.Point(10, 0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(300, 23);
            this.button.TabIndex = 0;
            this.button.Text = "button";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.CheckCompress);
            this.panel.Controls.Add(this.CheckCreate);
            this.panel.Controls.Add(this.groupBox2);
            this.panel.Controls.Add(this.groupBox1);
            this.panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel.Location = new System.Drawing.Point(10, 23);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(300, 338);
            this.panel.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.CodePath);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "代码路径";
            // 
            // CodePath
            // 
            this.CodePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodePath.Location = new System.Drawing.Point(3, 17);
            this.CodePath.Name = "CodePath";
            this.CodePath.Size = new System.Drawing.Size(292, 130);
            this.CodePath.TabIndex = 0;
            this.CodePath.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DataPath);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 150);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data路径";
            // 
            // DataPath
            // 
            this.DataPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataPath.Location = new System.Drawing.Point(3, 17);
            this.DataPath.Name = "DataPath";
            this.DataPath.Size = new System.Drawing.Size(292, 130);
            this.DataPath.TabIndex = 0;
            this.DataPath.Text = "";
            // 
            // CheckCreate
            // 
            this.CheckCreate.AutoSize = true;
            this.CheckCreate.Dock = System.Windows.Forms.DockStyle.Left;
            this.CheckCreate.Location = new System.Drawing.Point(0, 300);
            this.CheckCreate.Name = "CheckCreate";
            this.CheckCreate.Size = new System.Drawing.Size(96, 36);
            this.CheckCreate.TabIndex = 2;
            this.CheckCreate.Text = "默认是否生成";
            this.CheckCreate.UseVisualStyleBackColor = true;
            // 
            // CheckCompress
            // 
            this.CheckCompress.AutoSize = true;
            this.CheckCompress.Dock = System.Windows.Forms.DockStyle.Right;
            this.CheckCompress.Location = new System.Drawing.Point(178, 300);
            this.CheckCompress.Name = "CheckCompress";
            this.CheckCompress.Size = new System.Drawing.Size(120, 36);
            this.CheckCompress.TabIndex = 3;
            this.CheckCompress.Text = "是否使用gzip压缩";
            this.CheckCompress.UseVisualStyleBackColor = true;
            // 
            // CodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.button);
            this.Name = "CodeControl";
            this.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Size = new System.Drawing.Size(320, 458);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox CodePath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox DataPath;
        private System.Windows.Forms.CheckBox CheckCreate;
        private System.Windows.Forms.CheckBox CheckCompress;
    }
}
