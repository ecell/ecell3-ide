using System.Security;
using System.Security.Permissions;
namespace EcellLib.MainWindow
{
    /// <summary>
    /// Application class for E-Cell IDE.
    /// </summary>
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.sDockBay1 = new Yukichika.Controls.SDockBay();
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
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.saveWindowSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadWindowSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.modelEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entityListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathwayWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.messageWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRun = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowVersionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolstrip = new System.Windows.Forms.ToolStrip();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openScriptDialog = new System.Windows.Forms.OpenFileDialog();
            this.sDockBay1.SuspendLayout();
            this.menustrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // sDockBay1
            // 
            this.sDockBay1.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.sDockBay1, "sDockBay1");
            // 
            // sDockBay1.MainPane
            // 
            this.sDockBay1.MainPane.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.sDockBay1.MainPane, "sDockBay1.MainPane");
            this.sDockBay1.MainPane.Name = "MainPane";
            this.sDockBay1.MainPane.TabFirstVisibleIndex = 0;
            this.sDockBay1.MainPane.BeforeCloseTabPage += new Yukichika.Controls.BeforeCloseTabPageEventHandler(this.sDockBay1_BeforeCloseTabPage);
            this.sDockBay1.Name = "sDockBay1";
            this.sDockBay1.BeforeCloseTabPage += new Yukichika.Controls.BeforeCloseTabPageEventHandler(this.sDockBay1_BeforeCloseTabPage);
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
            resources.ApplyResources(this.menustrip, "menustrip");
            this.menustrip.Name = "menustrip";
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
            this.toolStripSeparator6,
            this.saveWindowSettingsToolStripMenuItem,
            this.loadWindowSettingsToolStripMenuItem,
            this.toolStripSeparator3,
            this.printToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.MenuItemFile.Name = "MenuItemFile";
            resources.ApplyResources(this.MenuItemFile, "MenuItemFile");
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.MergeIndex = 0;
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            resources.ApplyResources(this.newProjectToolStripMenuItem, "newProjectToolStripMenuItem");
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.NewProjectMenuClick);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.MergeIndex = 1;
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            resources.ApplyResources(this.openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.OpenProjectMenuClick);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.MergeIndex = 2;
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            resources.ApplyResources(this.saveProjectToolStripMenuItem, "saveProjectToolStripMenuItem");
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectMenuClick);
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.MergeIndex = 3;
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            resources.ApplyResources(this.closeProjectToolStripMenuItem, "closeProjectToolStripMenuItem");
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.CloseProjectMenuClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // importModelToolStripMenuItem
            // 
            this.importModelToolStripMenuItem.MergeIndex = 4;
            this.importModelToolStripMenuItem.Name = "importModelToolStripMenuItem";
            resources.ApplyResources(this.importModelToolStripMenuItem, "importModelToolStripMenuItem");
            this.importModelToolStripMenuItem.Click += new System.EventHandler(this.ImportModelMenuClick);
            // 
            // exportModelToolStripMenuItem
            // 
            this.exportModelToolStripMenuItem.MergeIndex = 5;
            this.exportModelToolStripMenuItem.Name = "exportModelToolStripMenuItem";
            resources.ApplyResources(this.exportModelToolStripMenuItem, "exportModelToolStripMenuItem");
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.ExportModelMenuClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // importScriptToolStripMenuItem
            // 
            this.importScriptToolStripMenuItem.Name = "importScriptToolStripMenuItem";
            resources.ApplyResources(this.importScriptToolStripMenuItem, "importScriptToolStripMenuItem");
            this.importScriptToolStripMenuItem.Click += new System.EventHandler(this.ImportScriptMenuClick);
            // 
            // saveScriptToolStripMenuItem
            // 
            this.saveScriptToolStripMenuItem.MergeIndex = 6;
            this.saveScriptToolStripMenuItem.Name = "saveScriptToolStripMenuItem";
            resources.ApplyResources(this.saveScriptToolStripMenuItem, "saveScriptToolStripMenuItem");
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.SaveScriptMenuClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // importActionMenuItem
            // 
            this.importActionMenuItem.Name = "importActionMenuItem";
            resources.ApplyResources(this.importActionMenuItem, "importActionMenuItem");
            this.importActionMenuItem.Click += new System.EventHandler(this.ImportActionMenuClick);
            // 
            // saveActionMenuItem
            // 
            this.saveActionMenuItem.Name = "saveActionMenuItem";
            resources.ApplyResources(this.saveActionMenuItem, "saveActionMenuItem");
            this.saveActionMenuItem.Click += new System.EventHandler(this.SaveActionMenuClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // saveWindowSettingsToolStripMenuItem
            // 
            this.saveWindowSettingsToolStripMenuItem.Name = "saveWindowSettingsToolStripMenuItem";
            resources.ApplyResources(this.saveWindowSettingsToolStripMenuItem, "saveWindowSettingsToolStripMenuItem");
            this.saveWindowSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveWindowSettingsToolStripMenuItem_Click);
            // 
            // loadWindowSettingsToolStripMenuItem
            // 
            this.loadWindowSettingsToolStripMenuItem.Name = "loadWindowSettingsToolStripMenuItem";
            resources.ApplyResources(this.loadWindowSettingsToolStripMenuItem, "loadWindowSettingsToolStripMenuItem");
            this.loadWindowSettingsToolStripMenuItem.Click += new System.EventHandler(this.loadWindowSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.MergeIndex = 7;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            resources.ApplyResources(this.printToolStripMenuItem, "printToolStripMenuItem");
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintMenuClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.MergeIndex = 100;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClick);
            // 
            // MenuItemEdit
            // 
            this.MenuItemEdit.Name = "MenuItemEdit";
            resources.ApplyResources(this.MenuItemEdit, "MenuItemEdit");
            // 
            // MenuItemSetup
            // 
            this.MenuItemSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelEditorToolStripMenuItem});
            this.MenuItemSetup.Name = "MenuItemSetup";
            resources.ApplyResources(this.MenuItemSetup, "MenuItemSetup");
            // 
            // modelEditorToolStripMenuItem
            // 
            this.modelEditorToolStripMenuItem.Name = "modelEditorToolStripMenuItem";
            resources.ApplyResources(this.modelEditorToolStripMenuItem, "modelEditorToolStripMenuItem");
            this.modelEditorToolStripMenuItem.Tag = "100";
            this.modelEditorToolStripMenuItem.Click += new System.EventHandler(this.ModelEditorMenuClick);
            // 
            // MenuItemLayout
            // 
            this.MenuItemLayout.Name = "MenuItemLayout";
            resources.ApplyResources(this.MenuItemLayout, "MenuItemLayout");
            // 
            // MenuItemView
            // 
            this.MenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem});
            this.MenuItemView.Name = "MenuItemView";
            resources.ApplyResources(this.MenuItemView, "MenuItemView");
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entityListToolStripMenuItem,
            this.pathwayWindowToolStripMenuItem,
            this.messageWindowToolStripMenuItem,
            this.objectListToolStripMenuItem,
            this.propertyWindowToolStripMenuItem});
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            resources.ApplyResources(this.showWindowToolStripMenuItem, "showWindowToolStripMenuItem");
            // 
            // entityListToolStripMenuItem
            // 
            this.entityListToolStripMenuItem.Checked = true;
            this.entityListToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.entityListToolStripMenuItem.Name = "entityListToolStripMenuItem";
            resources.ApplyResources(this.entityListToolStripMenuItem, "entityListToolStripMenuItem");
            this.entityListToolStripMenuItem.Click += new System.EventHandler(this.entityListToolStripMenuItem_Click);
            // 
            // pathwayWindowToolStripMenuItem
            // 
            this.pathwayWindowToolStripMenuItem.Checked = true;
            this.pathwayWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pathwayWindowToolStripMenuItem.Name = "pathwayWindowToolStripMenuItem";
            resources.ApplyResources(this.pathwayWindowToolStripMenuItem, "pathwayWindowToolStripMenuItem");
            this.pathwayWindowToolStripMenuItem.Click += new System.EventHandler(this.pathwayWindowToolStripMenuItem_Click);
            // 
            // messageWindowToolStripMenuItem
            // 
            this.messageWindowToolStripMenuItem.Checked = true;
            this.messageWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.messageWindowToolStripMenuItem.Name = "messageWindowToolStripMenuItem";
            resources.ApplyResources(this.messageWindowToolStripMenuItem, "messageWindowToolStripMenuItem");
            this.messageWindowToolStripMenuItem.Click += new System.EventHandler(this.messageWindowToolStripMenuItem_Click);
            // 
            // objectListToolStripMenuItem
            // 
            this.objectListToolStripMenuItem.Checked = true;
            this.objectListToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.objectListToolStripMenuItem.Name = "objectListToolStripMenuItem";
            resources.ApplyResources(this.objectListToolStripMenuItem, "objectListToolStripMenuItem");
            this.objectListToolStripMenuItem.Click += new System.EventHandler(this.objectListToolStripMenuItem_Click);
            // 
            // propertyWindowToolStripMenuItem
            // 
            this.propertyWindowToolStripMenuItem.Checked = true;
            this.propertyWindowToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.propertyWindowToolStripMenuItem.Name = "propertyWindowToolStripMenuItem";
            resources.ApplyResources(this.propertyWindowToolStripMenuItem, "propertyWindowToolStripMenuItem");
            this.propertyWindowToolStripMenuItem.Click += new System.EventHandler(this.propertyWindowToolStripMenuItem_Click);
            // 
            // MenuItemRun
            // 
            this.MenuItemRun.Name = "MenuItemRun";
            resources.ApplyResources(this.MenuItemRun, "MenuItemRun");
            // 
            // MenuItemAnalysis
            // 
            this.MenuItemAnalysis.Name = "MenuItemAnalysis";
            resources.ApplyResources(this.MenuItemAnalysis, "MenuItemAnalysis");
            // 
            // MenuItemDebug
            // 
            this.MenuItemDebug.Name = "MenuItemDebug";
            resources.ApplyResources(this.MenuItemDebug, "MenuItemDebug");
            // 
            // MenuItemHelp
            // 
            this.MenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowVersionMenuItem});
            this.MenuItemHelp.Name = "MenuItemHelp";
            resources.ApplyResources(this.MenuItemHelp, "MenuItemHelp");
            // 
            // ShowVersionMenuItem
            // 
            this.ShowVersionMenuItem.Name = "ShowVersionMenuItem";
            resources.ApplyResources(this.ShowVersionMenuItem, "ShowVersionMenuItem");
            this.ShowVersionMenuItem.Tag = "100";
            this.ShowVersionMenuItem.Click += new System.EventHandler(this.ShowPluginVersionClick);
            // 
            // toolstrip
            // 
            resources.ApplyResources(this.toolstrip, "toolstrip");
            this.toolstrip.Name = "toolstrip";
            // 
            // openFileDialog
            // 
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            // 
            // openScriptDialog
            // 
            resources.ApplyResources(this.openScriptDialog, "openScriptDialog");
            this.openScriptDialog.RestoreDirectory = true;
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sDockBay1);
            this.Controls.Add(this.menustrip);
            this.Controls.Add(this.toolstrip);
            this.MainMenuStrip = this.menustrip;
            this.Name = "MainWindow";
            this.Disposed += new System.EventHandler(this.MainWindowDisposed);
            this.sDockBay1.ResumeLayout(false);
            this.menustrip.ResumeLayout(false);
            this.menustrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menustrip;
        private System.Windows.Forms.ToolStrip toolstrip;
        private System.Windows.Forms.ToolStripMenuItem MenuItemFile;
        /// <summary>
        /// MenuItem to create project.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        /// <summary>
        /// MenuItem to open project.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        /// <summary>
        /// MenuItem to save project.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        /// <summary>
        /// MenuItem to close project.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem closeProjectToolStripMenuItem;
        /// <summary>
        /// MenuItem to separate menu by function.
        /// </summary>
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        /// <summary>
        /// MenuItem to import model.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem importModelToolStripMenuItem;
        /// <summary>
        /// MenuItem to export model.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem exportModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        /// <summary>
        /// MenuItem to save script.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem saveScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        /// <summary>
        /// MenuItem to print each window of plugin.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        /// <summary>
        /// MenuItem to exit this program.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSetup;
        /// <summary>
        /// MenuItem to set the working directory.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem modelEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRun;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDebug;
        private System.Windows.Forms.ToolStripMenuItem MenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuItemView;
        private System.Windows.Forms.OpenFileDialog openScriptDialog;
        private System.Windows.Forms.ToolStripMenuItem MenuItemEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        /// <summary>
        /// MenuItem to import action.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem importActionMenuItem;
        /// <summary>
        /// MenuItem to save action.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem saveActionMenuItem;
        /// <summary>
        /// MenuItem to import selected script.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem importScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAnalysis;
        private System.Windows.Forms.ToolStripMenuItem MenuItemLayout;
        /// <summary>
        /// MenuItem to display version dialog.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem ShowVersionMenuItem;
        private Yukichika.Controls.SDockBay sDockBay1;
        private System.Windows.Forms.ToolStripMenuItem saveWindowSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem loadWindowSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entityListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathwayWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem messageWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertyWindowToolStripMenuItem;
    }
}

