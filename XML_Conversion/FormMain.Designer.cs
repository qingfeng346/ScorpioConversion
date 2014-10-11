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
            this.labelCode = new System.Windows.Forms.Label();
            this.labelRollbackFiles = new System.Windows.Forms.Label();
            this.textCodeCS = new System.Windows.Forms.TextBox();
            this.textRollbackFiles = new System.Windows.Forms.TextBox();
            this.changeCSCode = new System.Windows.Forms.Button();
            this.selectRollbackFiles = new System.Windows.Forms.Button();
            this.buttonRollback = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.progressLabel = new System.Windows.Forms.Label();
            this.buttonTransform = new System.Windows.Forms.Button();
            this.selectTransformFiles = new System.Windows.Forms.Button();
            this.textTransformFiles = new System.Windows.Forms.TextBox();
            this.labelTransformFiles = new System.Windows.Forms.Label();
            this.changeCSData = new System.Windows.Forms.Button();
            this.textDataCS = new System.Windows.Forms.TextBox();
            this.labelData = new System.Windows.Forms.Label();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.timerLog = new System.Windows.Forms.Timer(this.components);
            this.listLog = new System.Windows.Forms.ListView();
            this.textSpawn = new System.Windows.Forms.TextBox();
            this.labelSpawn = new System.Windows.Forms.Label();
            this.label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.changeJavaData = new System.Windows.Forms.Button();
            this.textDataJava = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.changeJavaCode = new System.Windows.Forms.Button();
            this.textCodeJava = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.getManager = new System.Windows.Forms.CheckBox();
            this.changePHPData = new System.Windows.Forms.Button();
            this.textDataPHP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.changePHPCode = new System.Windows.Forms.Button();
            this.textCodePHP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableEnum = new System.Windows.Forms.CheckBox();
            this.Language = new System.Windows.Forms.CheckBox();
            this.buttonLanguage = new System.Windows.Forms.Button();
            this.buttonRefreshLanguage = new System.Windows.Forms.Button();
            this.getCustom = new System.Windows.Forms.CheckBox();
            this.getBase = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(60, 10);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(53, 12);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "代码路径";
            // 
            // labelRollbackFiles
            // 
            this.labelRollbackFiles.AutoSize = true;
            this.labelRollbackFiles.Location = new System.Drawing.Point(12, 251);
            this.labelRollbackFiles.Name = "labelRollbackFiles";
            this.labelRollbackFiles.Size = new System.Drawing.Size(101, 12);
            this.labelRollbackFiles.TabIndex = 2;
            this.labelRollbackFiles.Text = "选择要反转的文件";
            // 
            // textCodeCS
            // 
            this.textCodeCS.Location = new System.Drawing.Point(143, 7);
            this.textCodeCS.Name = "textCodeCS";
            this.textCodeCS.Size = new System.Drawing.Size(232, 21);
            this.textCodeCS.TabIndex = 3;
            // 
            // textRollbackFiles
            // 
            this.textRollbackFiles.Location = new System.Drawing.Point(143, 248);
            this.textRollbackFiles.Name = "textRollbackFiles";
            this.textRollbackFiles.ReadOnly = true;
            this.textRollbackFiles.Size = new System.Drawing.Size(232, 21);
            this.textRollbackFiles.TabIndex = 5;
            // 
            // changeCSCode
            // 
            this.changeCSCode.Location = new System.Drawing.Point(380, 5);
            this.changeCSCode.Name = "changeCSCode";
            this.changeCSCode.Size = new System.Drawing.Size(75, 23);
            this.changeCSCode.TabIndex = 6;
            this.changeCSCode.Text = "浏览";
            this.changeCSCode.UseVisualStyleBackColor = true;
            this.changeCSCode.Click += new System.EventHandler(this.changeCode_Click);
            // 
            // selectRollbackFiles
            // 
            this.selectRollbackFiles.Location = new System.Drawing.Point(380, 246);
            this.selectRollbackFiles.Name = "selectRollbackFiles";
            this.selectRollbackFiles.Size = new System.Drawing.Size(75, 22);
            this.selectRollbackFiles.TabIndex = 8;
            this.selectRollbackFiles.Text = "选择";
            this.selectRollbackFiles.UseVisualStyleBackColor = true;
            this.selectRollbackFiles.Click += new System.EventHandler(this.selectRollbackFiles_Click);
            // 
            // buttonRollback
            // 
            this.buttonRollback.Location = new System.Drawing.Point(465, 246);
            this.buttonRollback.Name = "buttonRollback";
            this.buttonRollback.Size = new System.Drawing.Size(75, 22);
            this.buttonRollback.TabIndex = 10;
            this.buttonRollback.Text = "转换";
            this.buttonRollback.UseVisualStyleBackColor = true;
            this.buttonRollback.Click += new System.EventHandler(this.buttonRollback_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(79, 278);
            this.progressBar.MarqueeAnimationSpeed = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(461, 21);
            this.progressBar.Step = 0;
            this.progressBar.TabIndex = 11;
            this.progressBar.Value = 50;
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
            this.progressLabel.Location = new System.Drawing.Point(12, 282);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(47, 12);
            this.progressLabel.TabIndex = 12;
            this.progressLabel.Text = "100/100";
            // 
            // buttonTransform
            // 
            this.buttonTransform.Location = new System.Drawing.Point(465, 217);
            this.buttonTransform.Name = "buttonTransform";
            this.buttonTransform.Size = new System.Drawing.Size(75, 22);
            this.buttonTransform.TabIndex = 9;
            this.buttonTransform.Text = "转换";
            this.buttonTransform.UseVisualStyleBackColor = true;
            this.buttonTransform.Click += new System.EventHandler(this.buttonTransform_Click);
            // 
            // selectTransformFiles
            // 
            this.selectTransformFiles.Location = new System.Drawing.Point(380, 217);
            this.selectTransformFiles.Name = "selectTransformFiles";
            this.selectTransformFiles.Size = new System.Drawing.Size(75, 22);
            this.selectTransformFiles.TabIndex = 7;
            this.selectTransformFiles.Text = "选择";
            this.selectTransformFiles.UseVisualStyleBackColor = true;
            this.selectTransformFiles.Click += new System.EventHandler(this.selectTransformFiles_Click);
            // 
            // textTransformFiles
            // 
            this.textTransformFiles.Location = new System.Drawing.Point(143, 219);
            this.textTransformFiles.Name = "textTransformFiles";
            this.textTransformFiles.ReadOnly = true;
            this.textTransformFiles.Size = new System.Drawing.Size(232, 21);
            this.textTransformFiles.TabIndex = 4;
            // 
            // labelTransformFiles
            // 
            this.labelTransformFiles.AutoSize = true;
            this.labelTransformFiles.Location = new System.Drawing.Point(13, 222);
            this.labelTransformFiles.Name = "labelTransformFiles";
            this.labelTransformFiles.Size = new System.Drawing.Size(101, 12);
            this.labelTransformFiles.TabIndex = 1;
            this.labelTransformFiles.Text = "选择要转换的文件";
            // 
            // changeCSData
            // 
            this.changeCSData.Location = new System.Drawing.Point(380, 36);
            this.changeCSData.Name = "changeCSData";
            this.changeCSData.Size = new System.Drawing.Size(75, 23);
            this.changeCSData.TabIndex = 15;
            this.changeCSData.Text = "浏览";
            this.changeCSData.UseVisualStyleBackColor = true;
            this.changeCSData.Click += new System.EventHandler(this.changeData_Click);
            // 
            // textDataCS
            // 
            this.textDataCS.Location = new System.Drawing.Point(143, 38);
            this.textDataCS.Name = "textDataCS";
            this.textDataCS.Size = new System.Drawing.Size(232, 21);
            this.textDataCS.TabIndex = 14;
            // 
            // labelData
            // 
            this.labelData.AutoSize = true;
            this.labelData.Location = new System.Drawing.Point(60, 39);
            this.labelData.Name = "labelData";
            this.labelData.Size = new System.Drawing.Size(53, 12);
            this.labelData.TabIndex = 13;
            this.labelData.Text = "数据路径";
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(15, 306);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 22;
            this.buttonExport.Text = "导出";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(113, 306);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 23;
            this.buttonClear.Text = "清空";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // timerLog
            // 
            this.timerLog.Enabled = true;
            this.timerLog.Tick += new System.EventHandler(this.timerLog_Tick);
            // 
            // listLog
            // 
            this.listLog.Location = new System.Drawing.Point(14, 341);
            this.listLog.Name = "listLog";
            this.listLog.Size = new System.Drawing.Size(535, 131);
            this.listLog.TabIndex = 24;
            this.listLog.UseCompatibleStateImageBehavior = false;
            // 
            // textSpawn
            // 
            this.textSpawn.Location = new System.Drawing.Point(143, 187);
            this.textSpawn.Name = "textSpawn";
            this.textSpawn.Size = new System.Drawing.Size(232, 21);
            this.textSpawn.TabIndex = 25;
            // 
            // labelSpawn
            // 
            this.labelSpawn.AutoSize = true;
            this.labelSpawn.Location = new System.Drawing.Point(13, 192);
            this.labelSpawn.Name = "labelSpawn";
            this.labelSpawn.Size = new System.Drawing.Size(119, 12);
            this.labelSpawn.TabIndex = 26;
            this.labelSpawn.Text = "批量关键字【;】隔开";
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(7, 11);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(23, 12);
            this.label.TabIndex = 27;
            this.label.Text = "CS:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 28;
            this.label1.Text = "JAVA:";
            // 
            // changeJavaData
            // 
            this.changeJavaData.Location = new System.Drawing.Point(380, 96);
            this.changeJavaData.Name = "changeJavaData";
            this.changeJavaData.Size = new System.Drawing.Size(75, 23);
            this.changeJavaData.TabIndex = 34;
            this.changeJavaData.Text = "浏览";
            this.changeJavaData.UseVisualStyleBackColor = true;
            this.changeJavaData.Click += new System.EventHandler(this.button1_Click);
            // 
            // textDataJava
            // 
            this.textDataJava.Location = new System.Drawing.Point(143, 98);
            this.textDataJava.Name = "textDataJava";
            this.textDataJava.Size = new System.Drawing.Size(232, 21);
            this.textDataJava.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "数据路径";
            // 
            // changeJavaCode
            // 
            this.changeJavaCode.Location = new System.Drawing.Point(380, 65);
            this.changeJavaCode.Name = "changeJavaCode";
            this.changeJavaCode.Size = new System.Drawing.Size(75, 23);
            this.changeJavaCode.TabIndex = 31;
            this.changeJavaCode.Text = "浏览";
            this.changeJavaCode.UseVisualStyleBackColor = true;
            this.changeJavaCode.Click += new System.EventHandler(this.button2_Click);
            // 
            // textCodeJava
            // 
            this.textCodeJava.Location = new System.Drawing.Point(143, 67);
            this.textCodeJava.Name = "textCodeJava";
            this.textCodeJava.Size = new System.Drawing.Size(232, 21);
            this.textCodeJava.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 29;
            this.label3.Text = "代码路径";
            // 
            // getManager
            // 
            this.getManager.AutoSize = true;
            this.getManager.Location = new System.Drawing.Point(465, 10);
            this.getManager.Name = "getManager";
            this.getManager.Size = new System.Drawing.Size(90, 16);
            this.getManager.TabIndex = 35;
            this.getManager.Text = "替换Manager";
            this.getManager.UseVisualStyleBackColor = true;
            // 
            // changePHPData
            // 
            this.changePHPData.Location = new System.Drawing.Point(380, 154);
            this.changePHPData.Name = "changePHPData";
            this.changePHPData.Size = new System.Drawing.Size(75, 23);
            this.changePHPData.TabIndex = 42;
            this.changePHPData.Text = "浏览";
            this.changePHPData.UseVisualStyleBackColor = true;
            this.changePHPData.Click += new System.EventHandler(this.button3_Click);
            // 
            // textDataPHP
            // 
            this.textDataPHP.Location = new System.Drawing.Point(143, 156);
            this.textDataPHP.Name = "textDataPHP";
            this.textDataPHP.Size = new System.Drawing.Size(232, 21);
            this.textDataPHP.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 40;
            this.label4.Text = "数据路径";
            // 
            // changePHPCode
            // 
            this.changePHPCode.Location = new System.Drawing.Point(380, 123);
            this.changePHPCode.Name = "changePHPCode";
            this.changePHPCode.Size = new System.Drawing.Size(75, 23);
            this.changePHPCode.TabIndex = 39;
            this.changePHPCode.Text = "浏览";
            this.changePHPCode.UseVisualStyleBackColor = true;
            this.changePHPCode.Click += new System.EventHandler(this.button4_Click);
            // 
            // textCodePHP
            // 
            this.textCodePHP.Location = new System.Drawing.Point(143, 125);
            this.textCodePHP.Name = "textCodePHP";
            this.textCodePHP.Size = new System.Drawing.Size(232, 21);
            this.textCodePHP.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 37;
            this.label5.Text = "代码路径";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 36;
            this.label6.Text = "PHP:";
            // 
            // tableEnum
            // 
            this.tableEnum.AutoSize = true;
            this.tableEnum.Location = new System.Drawing.Point(465, 36);
            this.tableEnum.Name = "tableEnum";
            this.tableEnum.Size = new System.Drawing.Size(102, 16);
            this.tableEnum.TabIndex = 43;
            this.tableEnum.Text = "获取TableEnum";
            this.tableEnum.UseVisualStyleBackColor = true;
            // 
            // Language
            // 
            this.Language.AutoSize = true;
            this.Language.Location = new System.Drawing.Point(465, 61);
            this.Language.Name = "Language";
            this.Language.Size = new System.Drawing.Size(96, 16);
            this.Language.TabIndex = 44;
            this.Language.Text = "生成多国语言";
            this.Language.UseVisualStyleBackColor = true;
            // 
            // buttonLanguage
            // 
            this.buttonLanguage.Location = new System.Drawing.Point(380, 187);
            this.buttonLanguage.Name = "buttonLanguage";
            this.buttonLanguage.Size = new System.Drawing.Size(75, 23);
            this.buttonLanguage.TabIndex = 45;
            this.buttonLanguage.Text = "多国语言";
            this.buttonLanguage.UseVisualStyleBackColor = true;
            this.buttonLanguage.Click += new System.EventHandler(this.buttonLanguage_Click);
            // 
            // buttonRefreshLanguage
            // 
            this.buttonRefreshLanguage.Location = new System.Drawing.Point(465, 188);
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
            this.getCustom.Location = new System.Drawing.Point(465, 86);
            this.getCustom.Name = "getCustom";
            this.getCustom.Size = new System.Drawing.Size(96, 16);
            this.getCustom.TabIndex = 47;
            this.getCustom.Text = "生成自定义类";
            this.getCustom.UseVisualStyleBackColor = true;
            // 
            // getBase
            // 
            this.getBase.AutoSize = true;
            this.getBase.Location = new System.Drawing.Point(465, 111);
            this.getBase.Name = "getBase";
            this.getBase.Size = new System.Drawing.Size(84, 16);
            this.getBase.TabIndex = 48;
            this.getBase.Text = "生成Base类";
            this.getBase.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 488);
            this.Controls.Add(this.getBase);
            this.Controls.Add(this.getCustom);
            this.Controls.Add(this.buttonRefreshLanguage);
            this.Controls.Add(this.buttonLanguage);
            this.Controls.Add(this.Language);
            this.Controls.Add(this.tableEnum);
            this.Controls.Add(this.changePHPData);
            this.Controls.Add(this.textDataPHP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.changePHPCode);
            this.Controls.Add(this.textCodePHP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.getManager);
            this.Controls.Add(this.changeJavaData);
            this.Controls.Add(this.textDataJava);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.changeJavaCode);
            this.Controls.Add(this.textCodeJava);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label);
            this.Controls.Add(this.labelSpawn);
            this.Controls.Add(this.textSpawn);
            this.Controls.Add(this.listLog);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.changeCSData);
            this.Controls.Add(this.textDataCS);
            this.Controls.Add(this.labelData);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonRollback);
            this.Controls.Add(this.buttonTransform);
            this.Controls.Add(this.selectRollbackFiles);
            this.Controls.Add(this.selectTransformFiles);
            this.Controls.Add(this.changeCSCode);
            this.Controls.Add(this.textRollbackFiles);
            this.Controls.Add(this.textTransformFiles);
            this.Controls.Add(this.textCodeCS);
            this.Controls.Add(this.labelRollbackFiles);
            this.Controls.Add(this.labelTransformFiles);
            this.Controls.Add(this.labelCode);
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

        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label labelRollbackFiles;
        
        private System.Windows.Forms.TextBox textRollbackFiles;
        private System.Windows.Forms.Button changeCSCode;
        private System.Windows.Forms.Button selectRollbackFiles;
        private System.Windows.Forms.Button buttonRollback;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button buttonTransform;
        private System.Windows.Forms.Button selectTransformFiles;
        private System.Windows.Forms.TextBox textTransformFiles;
        private System.Windows.Forms.Label labelTransformFiles;
        private System.Windows.Forms.Button changeCSData;
        private System.Windows.Forms.TextBox textDataCS;
        private System.Windows.Forms.Label labelData;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Timer timerLog;


        private System.Windows.Forms.ListView listLog;
        private System.Windows.Forms.TextBox textCodeCS;
        private System.Windows.Forms.TextBox textSpawn;
        private System.Windows.Forms.Label labelSpawn;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button changeJavaData;
        private System.Windows.Forms.TextBox textDataJava;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button changeJavaCode;
        private System.Windows.Forms.TextBox textCodeJava;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox getManager;
        private System.Windows.Forms.Button changePHPData;
        private System.Windows.Forms.TextBox textDataPHP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button changePHPCode;
        private System.Windows.Forms.TextBox textCodePHP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox tableEnum;
        private System.Windows.Forms.CheckBox Language;
        private System.Windows.Forms.Button buttonLanguage;
        private System.Windows.Forms.Button buttonRefreshLanguage;
        private System.Windows.Forms.CheckBox getCustom;
        private System.Windows.Forms.CheckBox getBase;
    }
}

