using System.Security;
using System.Security.Permissions;
namespace EcellLib.MainWindow
{
    partial class MainWindow
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
[SecurityPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menustrip = new System.Windows.Forms.MenuStrip();
            this.MenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.importActionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveActionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.modelEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRun = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstrip = new System.Windows.Forms.ToolStrip();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.openScriptDialog = new System.Windows.Forms.OpenFileDialog();
            this.menustrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menustrip
            // 
            this.menustrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemFile,
            this.MenuItemEdit,
            this.MenuItemSetup,
            this.MenuItemLayout,
            this.MenuItemView,
            this.MenuItemRun,
            this.MenuItemAnalysis,
            this.MenuItemDebug,
            this.MenuItemHelp});
            this.menustrip.Location = new System.Drawing.Point(0, 0);
            this.menustrip.Name = "menustrip";
            this.menustrip.Size = new System.Drawing.Size(994, 24);
            this.menustrip.TabIndex = 0;
            this.menustrip.Text = "menuStrip1";
            // 
            // MenuItemFile
            // 
            this.MenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.closeProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.importModelToolStripMenuItem,
            this.exportModelToolStripMenuItem,
            this.toolStripSeparator2,
            this.importScriptToolStripMenuItem,
            this.saveScriptToolStripMenuItem,
            this.toolStripSeparator5,
            this.importActionMenuItem,
            this.saveActionMenuItem,
            this.toolStripSeparator3,
            this.printToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.MenuItemFile.Name = "MenuItemFile";
            this.MenuItemFile.Size = new System.Drawing.Size(36, 20);
            this.MenuItemFile.Text = "File";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.MergeIndex = 0;
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.NewProjectMenuClick);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.MergeIndex = 1;
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.OpenProjectMenuClick);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.MergeIndex = 2;
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectMenuClick);
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.MergeIndex = 3;
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.closeProjectToolStripMenuItem.Text = "Close Project";
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.CloseProjectMenuClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // importModelToolStripMenuItem
            // 
            this.importModelToolStripMenuItem.MergeIndex = 4;
            this.importModelToolStripMenuItem.Name = "importModelToolStripMenuItem";
            this.importModelToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.importModelToolStripMenuItem.Text = "Import Model";
            this.importModelToolStripMenuItem.Click += new System.EventHandler(this.ImportModelMenuClick);
            // 
            // exportModelToolStripMenuItem
            // 
            this.exportModelToolStripMenuItem.MergeIndex = 5;
            this.exportModelToolStripMenuItem.Name = "exportModelToolStripMenuItem";
            this.exportModelToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.exportModelToolStripMenuItem.Text = "Export Model";
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.ExportModelMenuClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(136, 6);
            // 
            // importScriptToolStripMenuItem
            // 
            this.importScriptToolStripMenuItem.Name = "importScriptToolStripMenuItem";
            this.importScriptToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.importScriptToolStripMenuItem.Text = "Import Script";
            this.importScriptToolStripMenuItem.Click += new System.EventHandler(this.ImportScriptMenuClick);
            // 
            // saveScriptToolStripMenuItem
            // 
            this.saveScriptToolStripMenuItem.MergeIndex = 6;
            this.saveScriptToolStripMenuItem.Name = "saveScriptToolStripMenuItem";
            this.saveScriptToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveScriptToolStripMenuItem.Text = "Save Script";
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.SaveScriptMenuClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(136, 6);
            // 
            // importActionMenuItem
            // 
            this.importActionMenuItem.Name = "importActionMenuItem";
            this.importActionMenuItem.Size = new System.Drawing.Size(139, 22);
            this.importActionMenuItem.Text = "Import Action";
            this.importActionMenuItem.Click += new System.EventHandler(this.ImportActionMenuClick);
            // 
            // saveActionMenuItem
            // 
            this.saveActionMenuItem.Name = "saveActionMenuItem";
            this.saveActionMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveActionMenuItem.Text = "Save Action";
            this.saveActionMenuItem.Click += new System.EventHandler(this.SaveActionMenuClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(136, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.MergeIndex = 7;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintMenuClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(136, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.MergeIndex = 100;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClick);
            // 
            // MenuItemEdit
            // 
            this.MenuItemEdit.Name = "MenuItemEdit";
            this.MenuItemEdit.Size = new System.Drawing.Size(37, 20);
            this.MenuItemEdit.Text = "Edit";
            // 
            // MenuItemSetup
            // 
            this.MenuItemSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelEditorToolStripMenuItem});
            this.MenuItemSetup.Name = "MenuItemSetup";
            this.MenuItemSetup.Size = new System.Drawing.Size(46, 20);
            this.MenuItemSetup.Text = "Setup";
            // 
            // modelEditorToolStripMenuItem
            // 
            this.modelEditorToolStripMenuItem.Name = "modelEditorToolStripMenuItem";
            this.modelEditorToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.modelEditorToolStripMenuItem.Tag = "100";
            this.modelEditorToolStripMenuItem.Text = "ModelEditor";
            this.modelEditorToolStripMenuItem.Click += new System.EventHandler(this.ModelEditorMenuClick);
            // 
            // MenuItemLayout
            // 
            this.MenuItemLayout.Name = "MenuItemLayout";
            this.MenuItemLayout.Size = new System.Drawing.Size(51, 20);
            this.MenuItemLayout.Text = "Layout";
            // 
            // MenuItemView
            // 
            this.MenuItemView.Name = "MenuItemView";
            this.MenuItemView.Size = new System.Drawing.Size(42, 20);
            this.MenuItemView.Text = "View";
            // 
            // MenuItemRun
            // 
            this.MenuItemRun.Name = "MenuItemRun";
            this.MenuItemRun.Size = new System.Drawing.Size(37, 20);
            this.MenuItemRun.Text = "Run";
            // 
            // MenuItemAnalysis
            // 
            this.MenuItemAnalysis.Name = "MenuItemAnalysis";
            this.MenuItemAnalysis.Size = new System.Drawing.Size(61, 20);
            this.MenuItemAnalysis.Text = "Analysis";
            // 
            // MenuItemDebug
            // 
            this.MenuItemDebug.Name = "MenuItemDebug";
            this.MenuItemDebug.Size = new System.Drawing.Size(49, 20);
            this.MenuItemDebug.Text = "Debug";
            // 
            // MenuItemHelp
            // 
            this.MenuItemHelp.Name = "MenuItemHelp";
            this.MenuItemHelp.Size = new System.Drawing.Size(40, 20);
            this.MenuItemHelp.Text = "Help";
            // 
            // toolstrip
            // 
            this.toolstrip.Location = new System.Drawing.Point(0, 24);
            this.toolstrip.Name = "toolstrip";
            this.toolstrip.Size = new System.Drawing.Size(994, 25);
            this.toolstrip.TabIndex = 1;
            this.toolstrip.Text = "toolStrip1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "model file(*.sbml)|*.sbml";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(994, 644);
            this.splitContainer1.SplitterDistance = 459;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(984, 449);
            this.splitContainer2.SplitterDistance = 693;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Size = new System.Drawing.Size(689, 445);
            this.splitContainer4.SplitterDistance = 224;
            this.splitContainer4.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer3.Size = new System.Drawing.Size(277, 439);
            this.splitContainer3.SplitterDistance = 218;
            this.splitContainer3.TabIndex = 0;
            // 
            // openScriptDialog
            // 
            this.openScriptDialog.Filter = "script file(*.ess)|*.ess|all(*.*)|*.*";
            this.openScriptDialog.RestoreDirectory = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 693);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolstrip);
            this.Controls.Add(this.menustrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menustrip;
            this.Name = "MainWindow";
            this.Text = "E-CELL Integration Environment";
            this.Disposed += new System.EventHandler(this.MainWindowDisposed);
            this.menustrip.ResumeLayout(false);
            this.menustrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menustrip;
        private System.Windows.Forms.ToolStrip toolstrip;
        private System.Windows.Forms.ToolStripMenuItem MenuItemFile;
        public System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem closeProjectToolStripMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripMenuItem importModelToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem exportModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripMenuItem saveScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        public System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.SplitContainer splitContainer2;
        public System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSetup;
        public System.Windows.Forms.ToolStripMenuItem modelEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRun;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDebug;
        private System.Windows.Forms.ToolStripMenuItem MenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuItemView;
        private System.Windows.Forms.OpenFileDialog openScriptDialog;
        private System.Windows.Forms.ToolStripMenuItem MenuItemEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        public System.Windows.Forms.ToolStripMenuItem importActionMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveActionMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer4;
        public System.Windows.Forms.ToolStripMenuItem importScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAnalysis;
        private System.Windows.Forms.ToolStripMenuItem MenuItemLayout;
    }
}

