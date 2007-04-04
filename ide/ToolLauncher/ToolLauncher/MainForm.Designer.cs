namespace ToolLauncher
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageConvert = new System.Windows.Forms.TabPage();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.buttonConvertFolder = new System.Windows.Forms.Button();
            this.labelConvert = new System.Windows.Forms.Label();
            this.buttonConvertFile = new System.Windows.Forms.Button();
            this.textBoxConvert = new System.Windows.Forms.TextBox();
            this.fbdConvert = new System.Windows.Forms.FolderBrowserDialog();
            this.messageWindow = new System.Windows.Forms.UserControl();
            this.openFileDialogConvert = new System.Windows.Forms.OpenFileDialog();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.panelTools = new System.Windows.Forms.Panel();
            this.labelTools = new System.Windows.Forms.Label();
            this.labelMessages = new System.Windows.Forms.Label();
            this.panelMessages = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageConvert.SuspendLayout();
            this.panelTools.SuspendLayout();
            this.panelMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(492, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertyToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.fileToolStripMenuItem.Text = "File (&F)";
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            this.propertyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.propertyToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.propertyToolStripMenuItem.Text = "Property (&P)";
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItemProperty_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "Exit (&X)";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageConvert);
            this.tabControlMain.Location = new System.Drawing.Point(2, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(475, 120);
            this.tabControlMain.TabIndex = 2;
            // 
            // tabPageConvert
            // 
            this.tabPageConvert.Controls.Add(this.buttonConvert);
            this.tabPageConvert.Controls.Add(this.buttonConvertFolder);
            this.tabPageConvert.Controls.Add(this.labelConvert);
            this.tabPageConvert.Controls.Add(this.buttonConvertFile);
            this.tabPageConvert.Controls.Add(this.textBoxConvert);
            this.tabPageConvert.Location = new System.Drawing.Point(4, 21);
            this.tabPageConvert.Name = "tabPageConvert";
            this.tabPageConvert.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConvert.Size = new System.Drawing.Size(467, 95);
            this.tabPageConvert.TabIndex = 1;
            this.tabPageConvert.Text = "Convert to DLL";
            this.tabPageConvert.UseVisualStyleBackColor = true;
            // 
            // buttonConvert
            // 
            this.buttonConvert.Location = new System.Drawing.Point(193, 61);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonConvert.TabIndex = 4;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // buttonConvertFolder
            // 
            this.buttonConvertFolder.Location = new System.Drawing.Point(402, 6);
            this.buttonConvertFolder.Name = "buttonConvertFolder";
            this.buttonConvertFolder.Size = new System.Drawing.Size(52, 23);
            this.buttonConvertFolder.TabIndex = 2;
            this.buttonConvertFolder.Text = "Folder";
            this.buttonConvertFolder.UseVisualStyleBackColor = true;
            this.buttonConvertFolder.Click += new System.EventHandler(this.buttonConvertFolder_Click);
            // 
            // labelConvert
            // 
            this.labelConvert.AutoSize = true;
            this.labelConvert.Location = new System.Drawing.Point(421, 32);
            this.labelConvert.Name = "labelConvert";
            this.labelConvert.Size = new System.Drawing.Size(15, 12);
            this.labelConvert.TabIndex = 2;
            this.labelConvert.Text = "or";
            // 
            // buttonConvertFile
            // 
            this.buttonConvertFile.Location = new System.Drawing.Point(402, 46);
            this.buttonConvertFile.Name = "buttonConvertFile";
            this.buttonConvertFile.Size = new System.Drawing.Size(52, 23);
            this.buttonConvertFile.TabIndex = 3;
            this.buttonConvertFile.Text = "File";
            this.buttonConvertFile.UseVisualStyleBackColor = true;
            this.buttonConvertFile.Click += new System.EventHandler(this.buttonConvertFile_Click);
            // 
            // textBoxConvert
            // 
            this.textBoxConvert.Location = new System.Drawing.Point(6, 30);
            this.textBoxConvert.Name = "textBoxConvert";
            this.textBoxConvert.Size = new System.Drawing.Size(390, 19);
            this.textBoxConvert.TabIndex = 1;
            // 
            // messageWindow
            // 
            this.messageWindow.Location = new System.Drawing.Point(0, 0);
            this.messageWindow.Name = "messageWindow";
            this.messageWindow.Size = new System.Drawing.Size(150, 150);
            this.messageWindow.TabIndex = 0;
            // 
            // openFileDialogConvert
            // 
            this.openFileDialogConvert.Filter = "C++ Files (*.cpp)|*.cpp|All Files (*.*)|*.*";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.AcceptsReturn = true;
            this.textBoxMessage.AllowDrop = true;
            this.textBoxMessage.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessage.Location = new System.Drawing.Point(3, 3);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMessage.Size = new System.Drawing.Size(470, 250);
            this.textBoxMessage.TabIndex = 0;
            this.textBoxMessage.WordWrap = false;
            // 
            // panelTools
            // 
            this.panelTools.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelTools.Controls.Add(this.tabControlMain);
            this.panelTools.Location = new System.Drawing.Point(5, 45);
            this.panelTools.Name = "panelTools";
            this.panelTools.Size = new System.Drawing.Size(480, 128);
            this.panelTools.TabIndex = 4;
            // 
            // labelTools
            // 
            this.labelTools.AutoSize = true;
            this.labelTools.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelTools.Location = new System.Drawing.Point(6, 30);
            this.labelTools.Name = "labelTools";
            this.labelTools.Size = new System.Drawing.Size(43, 13);
            this.labelTools.TabIndex = 5;
            this.labelTools.Text = "Tools";
            // 
            // labelMessages
            // 
            this.labelMessages.AutoSize = true;
            this.labelMessages.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMessages.Location = new System.Drawing.Point(6, 187);
            this.labelMessages.Name = "labelMessages";
            this.labelMessages.Size = new System.Drawing.Size(69, 13);
            this.labelMessages.TabIndex = 6;
            this.labelMessages.Text = "Messages";
            // 
            // panelMessages
            // 
            this.panelMessages.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelMessages.Controls.Add(this.textBoxMessage);
            this.panelMessages.Location = new System.Drawing.Point(5, 203);
            this.panelMessages.Name = "panelMessages";
            this.panelMessages.Size = new System.Drawing.Size(480, 260);
            this.panelMessages.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 466);
            this.Controls.Add(this.panelMessages);
            this.Controls.Add(this.labelMessages);
            this.Controls.Add(this.labelTools);
            this.Controls.Add(this.panelTools);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "ToolLauncher";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageConvert.ResumeLayout(false);
            this.tabPageConvert.PerformLayout();
            this.panelTools.ResumeLayout(false);
            this.panelMessages.ResumeLayout(false);
            this.panelMessages.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageConvert;
        private System.Windows.Forms.TextBox textBoxConvert;
        private System.Windows.Forms.Button buttonConvertFile;
        private System.Windows.Forms.Button buttonConvertFolder;
        private System.Windows.Forms.Label labelConvert;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.FolderBrowserDialog fbdConvert;
        private System.Windows.Forms.UserControl messageWindow;
        private System.Windows.Forms.OpenFileDialog openFileDialogConvert;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Panel panelTools;
        private System.Windows.Forms.Label labelTools;
        private System.Windows.Forms.Label labelMessages;
        private System.Windows.Forms.Panel panelMessages;
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
