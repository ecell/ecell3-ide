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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
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
            this.openScriptDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.menustrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.AccessibleDescription = null;
            this.splitContainer1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackgroundImage = null;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Font = null;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AccessibleDescription = null;
            this.splitContainer1.Panel1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackgroundImage = null;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Font = null;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AccessibleDescription = null;
            this.splitContainer1.Panel2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackgroundImage = null;
            this.splitContainer1.Panel2.Font = null;
            // 
            // splitContainer2
            // 
            this.splitContainer2.AccessibleDescription = null;
            this.splitContainer2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.BackgroundImage = null;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Font = null;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.AccessibleDescription = null;
            this.splitContainer2.Panel1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            this.splitContainer2.Panel1.BackgroundImage = null;
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            this.splitContainer2.Panel1.Font = null;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.AccessibleDescription = null;
            this.splitContainer2.Panel2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.BackgroundImage = null;
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel2.Font = null;
            // 
            // splitContainer4
            // 
            this.splitContainer4.AccessibleDescription = null;
            this.splitContainer4.AccessibleName = null;
            resources.ApplyResources(this.splitContainer4, "splitContainer4");
            this.splitContainer4.BackgroundImage = null;
            this.splitContainer4.Font = null;
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.AccessibleDescription = null;
            this.splitContainer4.Panel1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer4.Panel1, "splitContainer4.Panel1");
            this.splitContainer4.Panel1.BackgroundImage = null;
            this.splitContainer4.Panel1.Font = null;
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.AccessibleDescription = null;
            this.splitContainer4.Panel2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer4.Panel2, "splitContainer4.Panel2");
            this.splitContainer4.Panel2.BackgroundImage = null;
            this.splitContainer4.Panel2.Font = null;
            // 
            // splitContainer3
            // 
            this.splitContainer3.AccessibleDescription = null;
            this.splitContainer3.AccessibleName = null;
            resources.ApplyResources(this.splitContainer3, "splitContainer3");
            this.splitContainer3.BackgroundImage = null;
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer3.Font = null;
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.AccessibleDescription = null;
            this.splitContainer3.Panel1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer3.Panel1, "splitContainer3.Panel1");
            this.splitContainer3.Panel1.BackgroundImage = null;
            this.splitContainer3.Panel1.Font = null;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.AccessibleDescription = null;
            this.splitContainer3.Panel2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer3.Panel2, "splitContainer3.Panel2");
            this.splitContainer3.Panel2.BackgroundImage = null;
            this.splitContainer3.Panel2.Font = null;
            // 
            // menustrip
            // 
            this.menustrip.AccessibleDescription = null;
            this.menustrip.AccessibleName = null;
            resources.ApplyResources(this.menustrip, "menustrip");
            this.menustrip.BackgroundImage = null;
            this.menustrip.Font = null;
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
            this.menustrip.Name = "menustrip";
            // 
            // MenuItemFile
            // 
            this.MenuItemFile.AccessibleDescription = null;
            this.MenuItemFile.AccessibleName = null;
            resources.ApplyResources(this.MenuItemFile, "MenuItemFile");
            this.MenuItemFile.BackgroundImage = null;
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
            this.MenuItemFile.ShortcutKeyDisplayString = null;
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.AccessibleDescription = null;
            this.newProjectToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.newProjectToolStripMenuItem, "newProjectToolStripMenuItem");
            this.newProjectToolStripMenuItem.BackgroundImage = null;
            this.newProjectToolStripMenuItem.MergeIndex = 0;
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.NewProjectMenuClick);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.AccessibleDescription = null;
            this.openProjectToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
            this.openProjectToolStripMenuItem.BackgroundImage = null;
            this.openProjectToolStripMenuItem.MergeIndex = 1;
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.OpenProjectMenuClick);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.AccessibleDescription = null;
            this.saveProjectToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveProjectToolStripMenuItem, "saveProjectToolStripMenuItem");
            this.saveProjectToolStripMenuItem.BackgroundImage = null;
            this.saveProjectToolStripMenuItem.MergeIndex = 2;
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectMenuClick);
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.AccessibleDescription = null;
            this.closeProjectToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.closeProjectToolStripMenuItem, "closeProjectToolStripMenuItem");
            this.closeProjectToolStripMenuItem.BackgroundImage = null;
            this.closeProjectToolStripMenuItem.MergeIndex = 3;
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            this.closeProjectToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.CloseProjectMenuClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // importModelToolStripMenuItem
            // 
            this.importModelToolStripMenuItem.AccessibleDescription = null;
            this.importModelToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.importModelToolStripMenuItem, "importModelToolStripMenuItem");
            this.importModelToolStripMenuItem.BackgroundImage = null;
            this.importModelToolStripMenuItem.MergeIndex = 4;
            this.importModelToolStripMenuItem.Name = "importModelToolStripMenuItem";
            this.importModelToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.importModelToolStripMenuItem.Click += new System.EventHandler(this.ImportModelMenuClick);
            // 
            // exportModelToolStripMenuItem
            // 
            this.exportModelToolStripMenuItem.AccessibleDescription = null;
            this.exportModelToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.exportModelToolStripMenuItem, "exportModelToolStripMenuItem");
            this.exportModelToolStripMenuItem.BackgroundImage = null;
            this.exportModelToolStripMenuItem.MergeIndex = 5;
            this.exportModelToolStripMenuItem.Name = "exportModelToolStripMenuItem";
            this.exportModelToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.ExportModelMenuClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // importScriptToolStripMenuItem
            // 
            this.importScriptToolStripMenuItem.AccessibleDescription = null;
            this.importScriptToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.importScriptToolStripMenuItem, "importScriptToolStripMenuItem");
            this.importScriptToolStripMenuItem.BackgroundImage = null;
            this.importScriptToolStripMenuItem.Name = "importScriptToolStripMenuItem";
            this.importScriptToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.importScriptToolStripMenuItem.Click += new System.EventHandler(this.ImportScriptMenuClick);
            // 
            // saveScriptToolStripMenuItem
            // 
            this.saveScriptToolStripMenuItem.AccessibleDescription = null;
            this.saveScriptToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveScriptToolStripMenuItem, "saveScriptToolStripMenuItem");
            this.saveScriptToolStripMenuItem.BackgroundImage = null;
            this.saveScriptToolStripMenuItem.MergeIndex = 6;
            this.saveScriptToolStripMenuItem.Name = "saveScriptToolStripMenuItem";
            this.saveScriptToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.SaveScriptMenuClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.AccessibleDescription = null;
            this.toolStripSeparator5.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // importActionMenuItem
            // 
            this.importActionMenuItem.AccessibleDescription = null;
            this.importActionMenuItem.AccessibleName = null;
            resources.ApplyResources(this.importActionMenuItem, "importActionMenuItem");
            this.importActionMenuItem.BackgroundImage = null;
            this.importActionMenuItem.Name = "importActionMenuItem";
            this.importActionMenuItem.ShortcutKeyDisplayString = null;
            this.importActionMenuItem.Click += new System.EventHandler(this.ImportActionMenuClick);
            // 
            // saveActionMenuItem
            // 
            this.saveActionMenuItem.AccessibleDescription = null;
            this.saveActionMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveActionMenuItem, "saveActionMenuItem");
            this.saveActionMenuItem.BackgroundImage = null;
            this.saveActionMenuItem.Name = "saveActionMenuItem";
            this.saveActionMenuItem.ShortcutKeyDisplayString = null;
            this.saveActionMenuItem.Click += new System.EventHandler(this.SaveActionMenuClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AccessibleDescription = null;
            this.toolStripSeparator3.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.AccessibleDescription = null;
            this.printToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.printToolStripMenuItem, "printToolStripMenuItem");
            this.printToolStripMenuItem.BackgroundImage = null;
            this.printToolStripMenuItem.MergeIndex = 7;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintMenuClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AccessibleDescription = null;
            this.toolStripSeparator4.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.AccessibleDescription = null;
            this.exitToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.BackgroundImage = null;
            this.exitToolStripMenuItem.MergeIndex = 100;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClick);
            // 
            // MenuItemEdit
            // 
            this.MenuItemEdit.AccessibleDescription = null;
            this.MenuItemEdit.AccessibleName = null;
            resources.ApplyResources(this.MenuItemEdit, "MenuItemEdit");
            this.MenuItemEdit.BackgroundImage = null;
            this.MenuItemEdit.Name = "MenuItemEdit";
            this.MenuItemEdit.ShortcutKeyDisplayString = null;
            // 
            // MenuItemSetup
            // 
            this.MenuItemSetup.AccessibleDescription = null;
            this.MenuItemSetup.AccessibleName = null;
            resources.ApplyResources(this.MenuItemSetup, "MenuItemSetup");
            this.MenuItemSetup.BackgroundImage = null;
            this.MenuItemSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelEditorToolStripMenuItem});
            this.MenuItemSetup.Name = "MenuItemSetup";
            this.MenuItemSetup.ShortcutKeyDisplayString = null;
            // 
            // modelEditorToolStripMenuItem
            // 
            this.modelEditorToolStripMenuItem.AccessibleDescription = null;
            this.modelEditorToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.modelEditorToolStripMenuItem, "modelEditorToolStripMenuItem");
            this.modelEditorToolStripMenuItem.BackgroundImage = null;
            this.modelEditorToolStripMenuItem.Name = "modelEditorToolStripMenuItem";
            this.modelEditorToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.modelEditorToolStripMenuItem.Tag = "100";
            this.modelEditorToolStripMenuItem.Click += new System.EventHandler(this.ModelEditorMenuClick);
            // 
            // MenuItemLayout
            // 
            this.MenuItemLayout.AccessibleDescription = null;
            this.MenuItemLayout.AccessibleName = null;
            resources.ApplyResources(this.MenuItemLayout, "MenuItemLayout");
            this.MenuItemLayout.BackgroundImage = null;
            this.MenuItemLayout.Name = "MenuItemLayout";
            this.MenuItemLayout.ShortcutKeyDisplayString = null;
            // 
            // MenuItemView
            // 
            this.MenuItemView.AccessibleDescription = null;
            this.MenuItemView.AccessibleName = null;
            resources.ApplyResources(this.MenuItemView, "MenuItemView");
            this.MenuItemView.BackgroundImage = null;
            this.MenuItemView.Name = "MenuItemView";
            this.MenuItemView.ShortcutKeyDisplayString = null;
            // 
            // MenuItemRun
            // 
            this.MenuItemRun.AccessibleDescription = null;
            this.MenuItemRun.AccessibleName = null;
            resources.ApplyResources(this.MenuItemRun, "MenuItemRun");
            this.MenuItemRun.BackgroundImage = null;
            this.MenuItemRun.Name = "MenuItemRun";
            this.MenuItemRun.ShortcutKeyDisplayString = null;
            // 
            // MenuItemAnalysis
            // 
            this.MenuItemAnalysis.AccessibleDescription = null;
            this.MenuItemAnalysis.AccessibleName = null;
            resources.ApplyResources(this.MenuItemAnalysis, "MenuItemAnalysis");
            this.MenuItemAnalysis.BackgroundImage = null;
            this.MenuItemAnalysis.Name = "MenuItemAnalysis";
            this.MenuItemAnalysis.ShortcutKeyDisplayString = null;
            // 
            // MenuItemDebug
            // 
            this.MenuItemDebug.AccessibleDescription = null;
            this.MenuItemDebug.AccessibleName = null;
            resources.ApplyResources(this.MenuItemDebug, "MenuItemDebug");
            this.MenuItemDebug.BackgroundImage = null;
            this.MenuItemDebug.Name = "MenuItemDebug";
            this.MenuItemDebug.ShortcutKeyDisplayString = null;
            // 
            // MenuItemHelp
            // 
            this.MenuItemHelp.AccessibleDescription = null;
            this.MenuItemHelp.AccessibleName = null;
            resources.ApplyResources(this.MenuItemHelp, "MenuItemHelp");
            this.MenuItemHelp.BackgroundImage = null;
            this.MenuItemHelp.Name = "MenuItemHelp";
            this.MenuItemHelp.ShortcutKeyDisplayString = null;
            // 
            // toolstrip
            // 
            this.toolstrip.AccessibleDescription = null;
            this.toolstrip.AccessibleName = null;
            resources.ApplyResources(this.toolstrip, "toolstrip");
            this.toolstrip.BackgroundImage = null;
            this.toolstrip.Font = null;
            this.toolstrip.Name = "toolstrip";
            // 
            // openFileDialog
            // 
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            // 
            // saveFileDialog
            // 
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            // 
            // openScriptDialog
            // 
            resources.ApplyResources(this.openScriptDialog, "openScriptDialog");
            this.openScriptDialog.RestoreDirectory = true;
            // 
            // MainWindow
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolstrip);
            this.Controls.Add(this.menustrip);
            this.Font = null;
            this.MainMenuStrip = this.menustrip;
            this.Name = "MainWindow";
            this.Disposed += new System.EventHandler(this.MainWindowDisposed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.menustrip.ResumeLayout(false);
            this.menustrip.PerformLayout();
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

