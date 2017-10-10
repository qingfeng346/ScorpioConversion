namespace ScorpioConversion {
    partial class FormMain {
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.OtherFoldPanel = new System.Windows.Forms.Panel();
            this.textBoxPackage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSpawns = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OtherFoldButton = new System.Windows.Forms.Button();
            this.LanguageFoldPanel = new System.Windows.Forms.Panel();
            this.textBoxTranslation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAllLanguages = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LanguageFoldButton = new System.Windows.Forms.Button();
            this.CodeFoldPanel = new System.Windows.Forms.Panel();
            this.CodeFoldButton = new System.Windows.Forms.Button();
            this.splitContainerFunc = new System.Windows.Forms.SplitContainer();
            this.buttonRefreshLanguage = new System.Windows.Forms.Button();
            this.refreshNote = new System.Windows.Forms.CheckBox();
            this.buttonTransformFolder = new System.Windows.Forms.Button();
            this.textTableFolder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textTableConfig = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonDatabase = new System.Windows.Forms.Button();
            this.textDatabaseConfig = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonMessage = new System.Windows.Forms.Button();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Language = new System.Windows.Forms.CheckBox();
            this.getManager = new System.Windows.Forms.CheckBox();
            this.progressLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonRollback = new System.Windows.Forms.Button();
            this.buttonTransform = new System.Windows.Forms.Button();
            this.selectRollbackFiles = new System.Windows.Forms.Button();
            this.selectTransformFiles = new System.Windows.Forms.Button();
            this.textRollbackFiles = new System.Windows.Forms.TextBox();
            this.textTransformFiles = new System.Windows.Forms.TextBox();
            this.labelRollbackFiles = new System.Windows.Forms.Label();
            this.labelTransformFiles = new System.Windows.Forms.Label();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonTiny = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeWorkspace = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenWorkspace = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.小工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mD5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.OtherFoldPanel.SuspendLayout();
            this.LanguageFoldPanel.SuspendLayout();
            this.splitContainerFunc.Panel1.SuspendLayout();
            this.splitContainerFunc.Panel2.SuspendLayout();
            this.splitContainerFunc.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.AutoScroll = true;
            this.splitContainerMain.Panel1.Controls.Add(this.OtherFoldPanel);
            this.splitContainerMain.Panel1.Controls.Add(this.OtherFoldButton);
            this.splitContainerMain.Panel1.Controls.Add(this.LanguageFoldPanel);
            this.splitContainerMain.Panel1.Controls.Add(this.LanguageFoldButton);
            this.splitContainerMain.Panel1.Controls.Add(this.CodeFoldPanel);
            this.splitContainerMain.Panel1.Controls.Add(this.CodeFoldButton);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.AutoScroll = true;
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerFunc);
            this.splitContainerMain.Size = new System.Drawing.Size(882, 520);
            this.splitContainerMain.SplitterDistance = 309;
            this.splitContainerMain.SplitterIncrement = 10;
            this.splitContainerMain.SplitterWidth = 10;
            this.splitContainerMain.TabIndex = 0;
            // 
            // OtherFoldPanel
            // 
            this.OtherFoldPanel.Controls.Add(this.textBoxPackage);
            this.OtherFoldPanel.Controls.Add(this.label5);
            this.OtherFoldPanel.Controls.Add(this.textBoxSpawns);
            this.OtherFoldPanel.Controls.Add(this.label4);
            this.OtherFoldPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.OtherFoldPanel.Location = new System.Drawing.Point(0, 239);
            this.OtherFoldPanel.Name = "OtherFoldPanel";
            this.OtherFoldPanel.Size = new System.Drawing.Size(307, 66);
            this.OtherFoldPanel.TabIndex = 5;
            // 
            // textBoxPackage
            // 
            this.textBoxPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPackage.Location = new System.Drawing.Point(106, 33);
            this.textBoxPackage.Name = "textBoxPackage";
            this.textBoxPackage.Size = new System.Drawing.Size(209, 21);
            this.textBoxPackage.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "生成的文件包名";
            // 
            // textBoxSpawns
            // 
            this.textBoxSpawns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSpawns.Location = new System.Drawing.Point(106, 6);
            this.textBoxSpawns.Name = "textBoxSpawns";
            this.textBoxSpawns.Size = new System.Drawing.Size(209, 21);
            this.textBoxSpawns.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "关键字列表";
            // 
            // OtherFoldButton
            // 
            this.OtherFoldButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.OtherFoldButton.Location = new System.Drawing.Point(0, 216);
            this.OtherFoldButton.Name = "OtherFoldButton";
            this.OtherFoldButton.Size = new System.Drawing.Size(307, 23);
            this.OtherFoldButton.TabIndex = 4;
            this.OtherFoldButton.Text = "其他设置";
            this.OtherFoldButton.UseVisualStyleBackColor = true;
            this.OtherFoldButton.Click += new System.EventHandler(this.OtherFoldButton_Click);
            // 
            // LanguageFoldPanel
            // 
            this.LanguageFoldPanel.Controls.Add(this.textBoxTranslation);
            this.LanguageFoldPanel.Controls.Add(this.label2);
            this.LanguageFoldPanel.Controls.Add(this.textBoxAllLanguages);
            this.LanguageFoldPanel.Controls.Add(this.label1);
            this.LanguageFoldPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.LanguageFoldPanel.Location = new System.Drawing.Point(0, 146);
            this.LanguageFoldPanel.Name = "LanguageFoldPanel";
            this.LanguageFoldPanel.Size = new System.Drawing.Size(307, 70);
            this.LanguageFoldPanel.TabIndex = 3;
            // 
            // textBoxTranslation
            // 
            this.textBoxTranslation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTranslation.Location = new System.Drawing.Point(106, 36);
            this.textBoxTranslation.Name = "textBoxTranslation";
            this.textBoxTranslation.Size = new System.Drawing.Size(209, 21);
            this.textBoxTranslation.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "翻译表生成目录";
            // 
            // textBoxAllLanguages
            // 
            this.textBoxAllLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAllLanguages.Location = new System.Drawing.Point(106, 9);
            this.textBoxAllLanguages.Name = "textBoxAllLanguages";
            this.textBoxAllLanguages.Size = new System.Drawing.Size(209, 21);
            this.textBoxAllLanguages.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "多国语言列表";
            // 
            // LanguageFoldButton
            // 
            this.LanguageFoldButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.LanguageFoldButton.Location = new System.Drawing.Point(0, 123);
            this.LanguageFoldButton.Name = "LanguageFoldButton";
            this.LanguageFoldButton.Size = new System.Drawing.Size(307, 23);
            this.LanguageFoldButton.TabIndex = 2;
            this.LanguageFoldButton.Text = "多国语言设置";
            this.LanguageFoldButton.UseVisualStyleBackColor = true;
            this.LanguageFoldButton.Click += new System.EventHandler(this.LanguageFoldButton_Click);
            // 
            // CodeFoldPanel
            // 
            this.CodeFoldPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CodeFoldPanel.Location = new System.Drawing.Point(0, 23);
            this.CodeFoldPanel.Name = "CodeFoldPanel";
            this.CodeFoldPanel.Size = new System.Drawing.Size(307, 100);
            this.CodeFoldPanel.TabIndex = 1;
            // 
            // CodeFoldButton
            // 
            this.CodeFoldButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.CodeFoldButton.Location = new System.Drawing.Point(0, 0);
            this.CodeFoldButton.Name = "CodeFoldButton";
            this.CodeFoldButton.Size = new System.Drawing.Size(307, 23);
            this.CodeFoldButton.TabIndex = 0;
            this.CodeFoldButton.Text = "脚本导出设置";
            this.CodeFoldButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CodeFoldButton.UseVisualStyleBackColor = true;
            this.CodeFoldButton.Click += new System.EventHandler(this.CodeFoldButton_Click);
            // 
            // splitContainerFunc
            // 
            this.splitContainerFunc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerFunc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerFunc.Location = new System.Drawing.Point(0, 0);
            this.splitContainerFunc.Name = "splitContainerFunc";
            this.splitContainerFunc.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerFunc.Panel1
            // 
            this.splitContainerFunc.Panel1.Controls.Add(this.buttonRefreshLanguage);
            this.splitContainerFunc.Panel1.Controls.Add(this.refreshNote);
            this.splitContainerFunc.Panel1.Controls.Add(this.buttonTransformFolder);
            this.splitContainerFunc.Panel1.Controls.Add(this.textTableFolder);
            this.splitContainerFunc.Panel1.Controls.Add(this.label7);
            this.splitContainerFunc.Panel1.Controls.Add(this.textTableConfig);
            this.splitContainerFunc.Panel1.Controls.Add(this.label8);
            this.splitContainerFunc.Panel1.Controls.Add(this.buttonDatabase);
            this.splitContainerFunc.Panel1.Controls.Add(this.textDatabaseConfig);
            this.splitContainerFunc.Panel1.Controls.Add(this.label9);
            this.splitContainerFunc.Panel1.Controls.Add(this.buttonMessage);
            this.splitContainerFunc.Panel1.Controls.Add(this.textMessage);
            this.splitContainerFunc.Panel1.Controls.Add(this.label10);
            this.splitContainerFunc.Panel1.Controls.Add(this.Language);
            this.splitContainerFunc.Panel1.Controls.Add(this.getManager);
            this.splitContainerFunc.Panel1.Controls.Add(this.progressLabel);
            this.splitContainerFunc.Panel1.Controls.Add(this.progressBar);
            this.splitContainerFunc.Panel1.Controls.Add(this.buttonRollback);
            this.splitContainerFunc.Panel1.Controls.Add(this.buttonTransform);
            this.splitContainerFunc.Panel1.Controls.Add(this.selectRollbackFiles);
            this.splitContainerFunc.Panel1.Controls.Add(this.selectTransformFiles);
            this.splitContainerFunc.Panel1.Controls.Add(this.textRollbackFiles);
            this.splitContainerFunc.Panel1.Controls.Add(this.textTransformFiles);
            this.splitContainerFunc.Panel1.Controls.Add(this.labelRollbackFiles);
            this.splitContainerFunc.Panel1.Controls.Add(this.labelTransformFiles);
            // 
            // splitContainerFunc.Panel2
            // 
            this.splitContainerFunc.Panel2.Controls.Add(this.richTextBoxLog);
            this.splitContainerFunc.Size = new System.Drawing.Size(563, 520);
            this.splitContainerFunc.SplitterDistance = 256;
            this.splitContainerFunc.SplitterIncrement = 10;
            this.splitContainerFunc.SplitterWidth = 10;
            this.splitContainerFunc.TabIndex = 0;
            // 
            // buttonRefreshLanguage
            // 
            this.buttonRefreshLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefreshLanguage.Location = new System.Drawing.Point(467, 24);
            this.buttonRefreshLanguage.Name = "buttonRefreshLanguage";
            this.buttonRefreshLanguage.Size = new System.Drawing.Size(75, 23);
            this.buttonRefreshLanguage.TabIndex = 96;
            this.buttonRefreshLanguage.Text = "刷新表";
            this.buttonRefreshLanguage.UseVisualStyleBackColor = true;
            this.buttonRefreshLanguage.Click += new System.EventHandler(this.buttonRefreshLanguage_Click);
            // 
            // refreshNote
            // 
            this.refreshNote.AutoSize = true;
            this.refreshNote.Location = new System.Drawing.Point(240, 27);
            this.refreshNote.Name = "refreshNote";
            this.refreshNote.Size = new System.Drawing.Size(174, 16);
            this.refreshNote.TabIndex = 95;
            this.refreshNote.Text = "刷新表注释(必须关闭Excel)";
            this.refreshNote.UseVisualStyleBackColor = true;
            // 
            // buttonTransformFolder
            // 
            this.buttonTransformFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTransformFolder.Location = new System.Drawing.Point(382, 79);
            this.buttonTransformFolder.Name = "buttonTransformFolder";
            this.buttonTransformFolder.Size = new System.Drawing.Size(75, 22);
            this.buttonTransformFolder.TabIndex = 94;
            this.buttonTransformFolder.Text = "转换";
            this.buttonTransformFolder.UseVisualStyleBackColor = true;
            this.buttonTransformFolder.Click += new System.EventHandler(this.buttonTransformFolder_Click);
            // 
            // textTableFolder
            // 
            this.textTableFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textTableFolder.Location = new System.Drawing.Point(144, 79);
            this.textTableFolder.Name = "textTableFolder";
            this.textTableFolder.Size = new System.Drawing.Size(232, 21);
            this.textTableFolder.TabIndex = 93;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 12);
            this.label7.TabIndex = 92;
            this.label7.Text = "选择要转换的文件夹";
            // 
            // textTableConfig
            // 
            this.textTableConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textTableConfig.Location = new System.Drawing.Point(145, 52);
            this.textTableConfig.Name = "textTableConfig";
            this.textTableConfig.Size = new System.Drawing.Size(232, 21);
            this.textTableConfig.TabIndex = 91;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 90;
            this.label8.Text = "选择配置目录";
            // 
            // buttonDatabase
            // 
            this.buttonDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDatabase.Location = new System.Drawing.Point(382, 192);
            this.buttonDatabase.Name = "buttonDatabase";
            this.buttonDatabase.Size = new System.Drawing.Size(75, 22);
            this.buttonDatabase.TabIndex = 89;
            this.buttonDatabase.Text = "转换";
            this.buttonDatabase.UseVisualStyleBackColor = true;
            this.buttonDatabase.Click += new System.EventHandler(this.buttonDatabase_Click);
            // 
            // textDatabaseConfig
            // 
            this.textDatabaseConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDatabaseConfig.Location = new System.Drawing.Point(144, 193);
            this.textDatabaseConfig.Name = "textDatabaseConfig";
            this.textDatabaseConfig.Size = new System.Drawing.Size(232, 21);
            this.textDatabaseConfig.TabIndex = 88;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 196);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 12);
            this.label9.TabIndex = 87;
            this.label9.Text = "选择数据库配置路径";
            // 
            // buttonMessage
            // 
            this.buttonMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMessage.Location = new System.Drawing.Point(382, 164);
            this.buttonMessage.Name = "buttonMessage";
            this.buttonMessage.Size = new System.Drawing.Size(75, 22);
            this.buttonMessage.TabIndex = 86;
            this.buttonMessage.Text = "转换";
            this.buttonMessage.UseVisualStyleBackColor = true;
            this.buttonMessage.Click += new System.EventHandler(this.buttonMessage_Click);
            // 
            // textMessage
            // 
            this.textMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessage.Location = new System.Drawing.Point(144, 165);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(232, 21);
            this.textMessage.TabIndex = 85;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 168);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 84;
            this.label10.Text = "选择协议路径";
            // 
            // Language
            // 
            this.Language.AutoSize = true;
            this.Language.Location = new System.Drawing.Point(126, 27);
            this.Language.Name = "Language";
            this.Language.Size = new System.Drawing.Size(96, 16);
            this.Language.TabIndex = 83;
            this.Language.Text = "生成多国语言";
            this.Language.UseVisualStyleBackColor = true;
            // 
            // getManager
            // 
            this.getManager.AutoSize = true;
            this.getManager.Location = new System.Drawing.Point(18, 27);
            this.getManager.Name = "getManager";
            this.getManager.Size = new System.Drawing.Size(90, 16);
            this.getManager.TabIndex = 82;
            this.getManager.Text = "生成Manager";
            this.getManager.UseVisualStyleBackColor = true;
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(16, 227);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(47, 12);
            this.progressLabel.TabIndex = 81;
            this.progressLabel.Text = "100/100";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(81, 223);
            this.progressBar.MarqueeAnimationSpeed = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(461, 21);
            this.progressBar.Step = 0;
            this.progressBar.TabIndex = 80;
            // 
            // buttonRollback
            // 
            this.buttonRollback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRollback.Location = new System.Drawing.Point(467, 136);
            this.buttonRollback.Name = "buttonRollback";
            this.buttonRollback.Size = new System.Drawing.Size(75, 22);
            this.buttonRollback.TabIndex = 79;
            this.buttonRollback.Text = "转换";
            this.buttonRollback.UseVisualStyleBackColor = true;
            this.buttonRollback.Click += new System.EventHandler(this.buttonRollback_Click);
            // 
            // buttonTransform
            // 
            this.buttonTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTransform.Location = new System.Drawing.Point(467, 107);
            this.buttonTransform.Name = "buttonTransform";
            this.buttonTransform.Size = new System.Drawing.Size(75, 22);
            this.buttonTransform.TabIndex = 78;
            this.buttonTransform.Text = "转换";
            this.buttonTransform.UseVisualStyleBackColor = true;
            this.buttonTransform.Click += new System.EventHandler(this.buttonTransform_Click);
            // 
            // selectRollbackFiles
            // 
            this.selectRollbackFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectRollbackFiles.Location = new System.Drawing.Point(382, 136);
            this.selectRollbackFiles.Name = "selectRollbackFiles";
            this.selectRollbackFiles.Size = new System.Drawing.Size(75, 22);
            this.selectRollbackFiles.TabIndex = 77;
            this.selectRollbackFiles.Text = "选择";
            this.selectRollbackFiles.UseVisualStyleBackColor = true;
            this.selectRollbackFiles.Click += new System.EventHandler(this.selectRollbackFiles_Click);
            // 
            // selectTransformFiles
            // 
            this.selectTransformFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectTransformFiles.Location = new System.Drawing.Point(382, 107);
            this.selectTransformFiles.Name = "selectTransformFiles";
            this.selectTransformFiles.Size = new System.Drawing.Size(75, 22);
            this.selectTransformFiles.TabIndex = 76;
            this.selectTransformFiles.Text = "选择";
            this.selectTransformFiles.UseVisualStyleBackColor = true;
            this.selectTransformFiles.Click += new System.EventHandler(this.selectTransformFiles_Click);
            // 
            // textRollbackFiles
            // 
            this.textRollbackFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textRollbackFiles.Location = new System.Drawing.Point(145, 138);
            this.textRollbackFiles.Name = "textRollbackFiles";
            this.textRollbackFiles.ReadOnly = true;
            this.textRollbackFiles.Size = new System.Drawing.Size(232, 21);
            this.textRollbackFiles.TabIndex = 75;
            // 
            // textTransformFiles
            // 
            this.textTransformFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textTransformFiles.Location = new System.Drawing.Point(145, 109);
            this.textTransformFiles.Name = "textTransformFiles";
            this.textTransformFiles.ReadOnly = true;
            this.textTransformFiles.Size = new System.Drawing.Size(232, 21);
            this.textTransformFiles.TabIndex = 74;
            // 
            // labelRollbackFiles
            // 
            this.labelRollbackFiles.AutoSize = true;
            this.labelRollbackFiles.Location = new System.Drawing.Point(15, 141);
            this.labelRollbackFiles.Name = "labelRollbackFiles";
            this.labelRollbackFiles.Size = new System.Drawing.Size(101, 12);
            this.labelRollbackFiles.TabIndex = 73;
            this.labelRollbackFiles.Text = "选择要反转的文件";
            // 
            // labelTransformFiles
            // 
            this.labelTransformFiles.AutoSize = true;
            this.labelTransformFiles.Location = new System.Drawing.Point(15, 112);
            this.labelTransformFiles.Name = "labelTransformFiles";
            this.labelTransformFiles.Size = new System.Drawing.Size(101, 12);
            this.labelTransformFiles.TabIndex = 72;
            this.labelTransformFiles.Text = "选择要转换的文件";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(561, 252);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // timerMain
            // 
            this.timerMain.Enabled = true;
            this.timerMain.Interval = 10;
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(882, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainerMain);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(882, 520);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(882, 542);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonTiny});
            this.toolStrip1.Location = new System.Drawing.Point(3, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(35, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonTiny
            // 
            this.toolStripButtonTiny.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTiny.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTiny.Image")));
            this.toolStripButtonTiny.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTiny.Name = "toolStripButtonTiny";
            this.toolStripButtonTiny.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTiny.Text = "toolStripButton1";
            this.toolStripButtonTiny.Click += new System.EventHandler(this.toolStripButtonTiny_Click);
            // 
            // toolStripContainer2
            // 
            this.toolStripContainer2.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.AutoScroll = true;
            this.toolStripContainer2.ContentPanel.Controls.Add(this.toolStripContainer1);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(882, 542);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.LeftToolStripPanelVisible = false;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.RightToolStripPanelVisible = false;
            this.toolStripContainer2.Size = new System.Drawing.Size(882, 592);
            this.toolStripContainer2.TabIndex = 4;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.小工具ToolStripMenuItem,
            this.MenuAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(882, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeWorkspace,
            this.OpenWorkspace});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // ChangeWorkspace
            // 
            this.ChangeWorkspace.Name = "ChangeWorkspace";
            this.ChangeWorkspace.Size = new System.Drawing.Size(172, 22);
            this.ChangeWorkspace.Text = "切换工作目录";
            this.ChangeWorkspace.Click += new System.EventHandler(this.ChangeWorkspace_Click);
            // 
            // OpenWorkspace
            // 
            this.OpenWorkspace.Name = "OpenWorkspace";
            this.OpenWorkspace.Size = new System.Drawing.Size(172, 22);
            this.OpenWorkspace.Text = "打开当前工作目录";
            this.OpenWorkspace.Click += new System.EventHandler(this.OpenWorkspace_Click);
            // 
            // MenuAbout
            // 
            this.MenuAbout.Name = "MenuAbout";
            this.MenuAbout.Size = new System.Drawing.Size(44, 21);
            this.MenuAbout.Text = "关于";
            this.MenuAbout.Click += new System.EventHandler(this.MenuAbout_Click);
            // 
            // 小工具ToolStripMenuItem
            // 
            this.小工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mD5ToolStripMenuItem});
            this.小工具ToolStripMenuItem.Name = "小工具ToolStripMenuItem";
            this.小工具ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.小工具ToolStripMenuItem.Text = "小工具";
            // 
            // mD5ToolStripMenuItem
            // 
            this.mD5ToolStripMenuItem.Name = "mD5ToolStripMenuItem";
            this.mD5ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.mD5ToolStripMenuItem.Text = "MD5";
            this.mD5ToolStripMenuItem.Click += new System.EventHandler(this.mD5ToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 592);
            this.Controls.Add(this.toolStripContainer2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "转表工具";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.OtherFoldPanel.ResumeLayout(false);
            this.OtherFoldPanel.PerformLayout();
            this.LanguageFoldPanel.ResumeLayout(false);
            this.LanguageFoldPanel.PerformLayout();
            this.splitContainerFunc.Panel1.ResumeLayout(false);
            this.splitContainerFunc.Panel1.PerformLayout();
            this.splitContainerFunc.Panel2.ResumeLayout(false);
            this.splitContainerFunc.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Button CodeFoldButton;
        private System.Windows.Forms.Panel CodeFoldPanel;
        private System.Windows.Forms.Button LanguageFoldButton;
        private System.Windows.Forms.Panel LanguageFoldPanel;
        private System.Windows.Forms.Button OtherFoldButton;
        private System.Windows.Forms.Panel OtherFoldPanel;
        private System.Windows.Forms.SplitContainer splitContainerFunc;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxAllLanguages;
        private System.Windows.Forms.TextBox textBoxTranslation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSpawns;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPackage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox refreshNote;
        private System.Windows.Forms.Button buttonTransformFolder;
        private System.Windows.Forms.TextBox textTableFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textTableConfig;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonDatabase;
        private System.Windows.Forms.TextBox textDatabaseConfig;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonMessage;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox Language;
        private System.Windows.Forms.CheckBox getManager;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonRollback;
        private System.Windows.Forms.Button buttonTransform;
        private System.Windows.Forms.Button selectRollbackFiles;
        private System.Windows.Forms.Button selectTransformFiles;
        private System.Windows.Forms.TextBox textRollbackFiles;
        private System.Windows.Forms.TextBox textTransformFiles;
        private System.Windows.Forms.Label labelRollbackFiles;
        private System.Windows.Forms.Label labelTransformFiles;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.Button buttonRefreshLanguage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStripButton toolStripButtonTiny;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChangeWorkspace;
        private System.Windows.Forms.ToolStripMenuItem MenuAbout;
        private System.Windows.Forms.ToolStripMenuItem OpenWorkspace;
        private System.Windows.Forms.ToolStripMenuItem 小工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mD5ToolStripMenuItem;
    }
}

