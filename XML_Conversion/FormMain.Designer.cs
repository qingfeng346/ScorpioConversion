namespace XML_Conversion
{
    partial class FormMain
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelRollbackFiles = new System.Windows.Forms.Label();
            this.textRollbackFiles = new System.Windows.Forms.TextBox();
            this.selectRollbackFiles = new System.Windows.Forms.Button();
            this.buttonRollback = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.progressLabel = new System.Windows.Forms.Label();
            this.buttonTransform = new System.Windows.Forms.Button();
            this.selectTransformFiles = new System.Windows.Forms.Button();
            this.textTransformFiles = new System.Windows.Forms.TextBox();
            this.labelTransformFiles = new System.Windows.Forms.Label();
            this.getManager = new System.Windows.Forms.CheckBox();
            this.tableEnum = new System.Windows.Forms.CheckBox();
            this.Language = new System.Windows.Forms.CheckBox();
            this.buttonLanguage = new System.Windows.Forms.Button();
            this.buttonRefreshLanguage = new System.Windows.Forms.Button();
            this.getCustom = new System.Windows.Forms.CheckBox();
            this.getBase = new System.Windows.Forms.CheckBox();
            this.buttonCode = new System.Windows.Forms.Button();
            this.programBox = new System.Windows.Forms.ComboBox();
            this.buttonData = new System.Windows.Forms.Button();
            this.checkCreate = new System.Windows.Forms.CheckBox();
            this.buttonSpwan = new System.Windows.Forms.Button();
            this.buttonLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelRollbackFiles
            // 
            this.labelRollbackFiles.AutoSize = true;
            this.labelRollbackFiles.Location = new System.Drawing.Point(11, 131);
            this.labelRollbackFiles.Name = "labelRollbackFiles";
            this.labelRollbackFiles.Size = new System.Drawing.Size(101, 12);
            this.labelRollbackFiles.TabIndex = 2;
            this.labelRollbackFiles.Text = "选择要反转的文件";
            // 
            // textRollbackFiles
            // 
            this.textRollbackFiles.Location = new System.Drawing.Point(142, 128);
            this.textRollbackFiles.Name = "textRollbackFiles";
            this.textRollbackFiles.ReadOnly = true;
            this.textRollbackFiles.Size = new System.Drawing.Size(232, 21);
            this.textRollbackFiles.TabIndex = 5;
            // 
            // selectRollbackFiles
            // 
            this.selectRollbackFiles.Location = new System.Drawing.Point(379, 126);
            this.selectRollbackFiles.Name = "selectRollbackFiles";
            this.selectRollbackFiles.Size = new System.Drawing.Size(75, 22);
            this.selectRollbackFiles.TabIndex = 8;
            this.selectRollbackFiles.Text = "选择";
            this.selectRollbackFiles.UseVisualStyleBackColor = true;
            this.selectRollbackFiles.Click += new System.EventHandler(this.selectRollbackFiles_Click);
            // 
            // buttonRollback
            // 
            this.buttonRollback.Location = new System.Drawing.Point(464, 126);
            this.buttonRollback.Name = "buttonRollback";
            this.buttonRollback.Size = new System.Drawing.Size(75, 22);
            this.buttonRollback.TabIndex = 10;
            this.buttonRollback.Text = "转换";
            this.buttonRollback.UseVisualStyleBackColor = true;
            this.buttonRollback.Click += new System.EventHandler(this.buttonRollback_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(78, 156);
            this.progressBar.MarqueeAnimationSpeed = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(461, 21);
            this.progressBar.Step = 0;
            this.progressBar.TabIndex = 11;
            // 
            // timerProgress
            // 
            this.timerProgress.Enabled = true;
            this.timerProgress.Interval = 1;
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(13, 160);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(47, 12);
            this.progressLabel.TabIndex = 12;
            this.progressLabel.Text = "100/100";
            // 
            // buttonTransform
            // 
            this.buttonTransform.Location = new System.Drawing.Point(464, 97);
            this.buttonTransform.Name = "buttonTransform";
            this.buttonTransform.Size = new System.Drawing.Size(75, 22);
            this.buttonTransform.TabIndex = 9;
            this.buttonTransform.Text = "转换";
            this.buttonTransform.UseVisualStyleBackColor = true;
            this.buttonTransform.Click += new System.EventHandler(this.buttonTransform_Click);
            // 
            // selectTransformFiles
            // 
            this.selectTransformFiles.Location = new System.Drawing.Point(379, 97);
            this.selectTransformFiles.Name = "selectTransformFiles";
            this.selectTransformFiles.Size = new System.Drawing.Size(75, 22);
            this.selectTransformFiles.TabIndex = 7;
            this.selectTransformFiles.Text = "选择";
            this.selectTransformFiles.UseVisualStyleBackColor = true;
            this.selectTransformFiles.Click += new System.EventHandler(this.selectTransformFiles_Click);
            // 
            // textTransformFiles
            // 
            this.textTransformFiles.Location = new System.Drawing.Point(142, 99);
            this.textTransformFiles.Name = "textTransformFiles";
            this.textTransformFiles.ReadOnly = true;
            this.textTransformFiles.Size = new System.Drawing.Size(232, 21);
            this.textTransformFiles.TabIndex = 4;
            // 
            // labelTransformFiles
            // 
            this.labelTransformFiles.AutoSize = true;
            this.labelTransformFiles.Location = new System.Drawing.Point(12, 102);
            this.labelTransformFiles.Name = "labelTransformFiles";
            this.labelTransformFiles.Size = new System.Drawing.Size(101, 12);
            this.labelTransformFiles.TabIndex = 1;
            this.labelTransformFiles.Text = "选择要转换的文件";
            // 
            // getManager
            // 
            this.getManager.AutoSize = true;
            this.getManager.Location = new System.Drawing.Point(14, 77);
            this.getManager.Name = "getManager";
            this.getManager.Size = new System.Drawing.Size(90, 16);
            this.getManager.TabIndex = 35;
            this.getManager.Text = "替换Manager";
            this.getManager.UseVisualStyleBackColor = true;
            // 
            // tableEnum
            // 
            this.tableEnum.AutoSize = true;
            this.tableEnum.Location = new System.Drawing.Point(110, 77);
            this.tableEnum.Name = "tableEnum";
            this.tableEnum.Size = new System.Drawing.Size(102, 16);
            this.tableEnum.TabIndex = 43;
            this.tableEnum.Text = "获取TableEnum";
            this.tableEnum.UseVisualStyleBackColor = true;
            // 
            // Language
            // 
            this.Language.AutoSize = true;
            this.Language.Location = new System.Drawing.Point(218, 77);
            this.Language.Name = "Language";
            this.Language.Size = new System.Drawing.Size(96, 16);
            this.Language.TabIndex = 44;
            this.Language.Text = "生成多国语言";
            this.Language.UseVisualStyleBackColor = true;
            // 
            // buttonLanguage
            // 
            this.buttonLanguage.Location = new System.Drawing.Point(379, 42);
            this.buttonLanguage.Name = "buttonLanguage";
            this.buttonLanguage.Size = new System.Drawing.Size(75, 23);
            this.buttonLanguage.TabIndex = 45;
            this.buttonLanguage.Text = "多国语言";
            this.buttonLanguage.UseVisualStyleBackColor = true;
            this.buttonLanguage.Click += new System.EventHandler(this.buttonLanguage_Click);
            // 
            // buttonRefreshLanguage
            // 
            this.buttonRefreshLanguage.Location = new System.Drawing.Point(464, 42);
            this.buttonRefreshLanguage.Name = "buttonRefreshLanguage";
            this.buttonRefreshLanguage.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshLanguage.TabIndex = 46;
            this.buttonRefreshLanguage.Text = "刷新表";
            this.buttonRefreshLanguage.UseVisualStyleBackColor = true;
            this.buttonRefreshLanguage.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // getCustom
            // 
            this.getCustom.AutoSize = true;
            this.getCustom.Location = new System.Drawing.Point(320, 77);
            this.getCustom.Name = "getCustom";
            this.getCustom.Size = new System.Drawing.Size(96, 16);
            this.getCustom.TabIndex = 47;
            this.getCustom.Text = "生成自定义类";
            this.getCustom.UseVisualStyleBackColor = true;
            // 
            // getBase
            // 
            this.getBase.AutoSize = true;
            this.getBase.Location = new System.Drawing.Point(422, 77);
            this.getBase.Name = "getBase";
            this.getBase.Size = new System.Drawing.Size(84, 16);
            this.getBase.TabIndex = 48;
            this.getBase.Text = "生成Base类";
            this.getBase.UseVisualStyleBackColor = true;
            // 
            // buttonCode
            // 
            this.buttonCode.Location = new System.Drawing.Point(157, 10);
            this.buttonCode.Name = "buttonCode";
            this.buttonCode.Size = new System.Drawing.Size(75, 23);
            this.buttonCode.TabIndex = 49;
            this.buttonCode.Text = "代码路径";
            this.buttonCode.UseVisualStyleBackColor = true;
            this.buttonCode.Click += new System.EventHandler(this.buttonCode_Click);
            // 
            // programBox
            // 
            this.programBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.programBox.FormattingEnabled = true;
            this.programBox.Location = new System.Drawing.Point(13, 12);
            this.programBox.Name = "programBox";
            this.programBox.Size = new System.Drawing.Size(121, 20);
            this.programBox.TabIndex = 50;
            this.programBox.SelectedIndexChanged += new System.EventHandler(this.programBox_SelectedIndexChanged);
            // 
            // buttonData
            // 
            this.buttonData.Location = new System.Drawing.Point(250, 10);
            this.buttonData.Name = "buttonData";
            this.buttonData.Size = new System.Drawing.Size(75, 23);
            this.buttonData.TabIndex = 51;
            this.buttonData.Text = "Data路径";
            this.buttonData.UseVisualStyleBackColor = true;
            this.buttonData.Click += new System.EventHandler(this.buttonData_Click);
            // 
            // checkCreate
            // 
            this.checkCreate.AutoSize = true;
            this.checkCreate.Location = new System.Drawing.Point(342, 14);
            this.checkCreate.Name = "checkCreate";
            this.checkCreate.Size = new System.Drawing.Size(72, 16);
            this.checkCreate.TabIndex = 52;
            this.checkCreate.Text = "默认生成";
            this.checkCreate.UseVisualStyleBackColor = true;
            this.checkCreate.CheckedChanged += new System.EventHandler(this.checkCreate_CheckedChanged);
            // 
            // buttonSpwan
            // 
            this.buttonSpwan.Location = new System.Drawing.Point(12, 42);
            this.buttonSpwan.Name = "buttonSpwan";
            this.buttonSpwan.Size = new System.Drawing.Size(75, 23);
            this.buttonSpwan.TabIndex = 53;
            this.buttonSpwan.Text = "批量关键字";
            this.buttonSpwan.UseVisualStyleBackColor = true;
            this.buttonSpwan.Click += new System.EventHandler(this.buttonSpwan_Click);
            // 
            // buttonLog
            // 
            this.buttonLog.Location = new System.Drawing.Point(463, 9);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(75, 23);
            this.buttonLog.TabIndex = 54;
            this.buttonLog.Text = "打开日志";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 188);
            this.Controls.Add(this.buttonLog);
            this.Controls.Add(this.buttonSpwan);
            this.Controls.Add(this.checkCreate);
            this.Controls.Add(this.buttonData);
            this.Controls.Add(this.programBox);
            this.Controls.Add(this.buttonCode);
            this.Controls.Add(this.getBase);
            this.Controls.Add(this.getCustom);
            this.Controls.Add(this.buttonRefreshLanguage);
            this.Controls.Add(this.buttonLanguage);
            this.Controls.Add(this.Language);
            this.Controls.Add(this.tableEnum);
            this.Controls.Add(this.getManager);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonRollback);
            this.Controls.Add(this.buttonTransform);
            this.Controls.Add(this.selectRollbackFiles);
            this.Controls.Add(this.selectTransformFiles);
            this.Controls.Add(this.textRollbackFiles);
            this.Controls.Add(this.textTransformFiles);
            this.Controls.Add(this.labelRollbackFiles);
            this.Controls.Add(this.labelTransformFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XML_Conversion";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRollbackFiles;

        private System.Windows.Forms.TextBox textRollbackFiles;
        private System.Windows.Forms.Button selectRollbackFiles;
        private System.Windows.Forms.Button buttonRollback;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button buttonTransform;
        private System.Windows.Forms.Button selectTransformFiles;
        private System.Windows.Forms.TextBox textTransformFiles;
        private System.Windows.Forms.Label labelTransformFiles;
        private System.Windows.Forms.CheckBox getManager;
        private System.Windows.Forms.CheckBox tableEnum;
        private System.Windows.Forms.CheckBox Language;
        private System.Windows.Forms.Button buttonLanguage;
        private System.Windows.Forms.Button buttonRefreshLanguage;
        private System.Windows.Forms.CheckBox getCustom;
        private System.Windows.Forms.CheckBox getBase;
        private System.Windows.Forms.Button buttonCode;
        private System.Windows.Forms.ComboBox programBox;
        private System.Windows.Forms.Button buttonData;
        private System.Windows.Forms.CheckBox checkCreate;
        private System.Windows.Forms.Button buttonSpwan;
        private System.Windows.Forms.Button buttonLog;
    }
}

