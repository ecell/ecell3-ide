﻿using System.Security;
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
        /// <summary>
        /// 
        /// </summary>
        [SecurityPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.menustrip = new System.Windows.Forms.MenuStrip();
            this.MenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectWizardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.modelEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setIDEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.distributedEnvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRun = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowVersionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openScriptDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.menustrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.dockPanel);
            resources.ApplyResources(this.toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
            resources.ApplyResources(this.toolStripContainer, "toolStripContainer");
            this.toolStripContainer.Name = "toolStripContainer";
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            resources.ApplyResources(this.dockPanel, "dockPanel");
            this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel.Name = "dockPanel";
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
            this.projectWizardMenuItem,
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
            this.newProjectToolStripMenuItem.Tag = "1";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.NewProjectMenuClick);
            // 
            // projectWizardMenuItem
            // 
            this.projectWizardMenuItem.Name = "projectWizardMenuItem";
            resources.ApplyResources(this.projectWizardMenuItem, "projectWizardMenuItem");
            this.projectWizardMenuItem.Click += new System.EventHandler(this.ProjectWizardMenuClick);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.MergeIndex = 1;
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            resources.ApplyResources(this.openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
            this.openProjectToolStripMenuItem.Tag = "2";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.OpenProjectMenuClick);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.MergeIndex = 2;
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            resources.ApplyResources(this.saveProjectToolStripMenuItem, "saveProjectToolStripMenuItem");
            this.saveProjectToolStripMenuItem.Tag = "3";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectMenuClick);
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.MergeIndex = 3;
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            resources.ApplyResources(this.closeProjectToolStripMenuItem, "closeProjectToolStripMenuItem");
            this.closeProjectToolStripMenuItem.Tag = "4";
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.CloseProjectMenuClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Tag = "6";
            // 
            // importModelToolStripMenuItem
            // 
            this.importModelToolStripMenuItem.MergeIndex = 4;
            this.importModelToolStripMenuItem.Name = "importModelToolStripMenuItem";
            resources.ApplyResources(this.importModelToolStripMenuItem, "importModelToolStripMenuItem");
            this.importModelToolStripMenuItem.Tag = "6";
            this.importModelToolStripMenuItem.Click += new System.EventHandler(this.ImportModelMenuClick);
            // 
            // exportModelToolStripMenuItem
            // 
            this.exportModelToolStripMenuItem.MergeIndex = 5;
            this.exportModelToolStripMenuItem.Name = "exportModelToolStripMenuItem";
            resources.ApplyResources(this.exportModelToolStripMenuItem, "exportModelToolStripMenuItem");
            this.exportModelToolStripMenuItem.Tag = "7";
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.ExportModelMenuClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Tag = "8";
            // 
            // importScriptToolStripMenuItem
            // 
            this.importScriptToolStripMenuItem.Name = "importScriptToolStripMenuItem";
            resources.ApplyResources(this.importScriptToolStripMenuItem, "importScriptToolStripMenuItem");
            this.importScriptToolStripMenuItem.Tag = "9";
            this.importScriptToolStripMenuItem.Click += new System.EventHandler(this.ImportScriptMenuClick);
            // 
            // saveScriptToolStripMenuItem
            // 
            this.saveScriptToolStripMenuItem.MergeIndex = 6;
            this.saveScriptToolStripMenuItem.Name = "saveScriptToolStripMenuItem";
            resources.ApplyResources(this.saveScriptToolStripMenuItem, "saveScriptToolStripMenuItem");
            this.saveScriptToolStripMenuItem.Tag = "10";
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.SaveScriptMenuClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Tag = "11";
            // 
            // importActionMenuItem
            // 
            this.importActionMenuItem.Name = "importActionMenuItem";
            resources.ApplyResources(this.importActionMenuItem, "importActionMenuItem");
            this.importActionMenuItem.Tag = "12";
            this.importActionMenuItem.Click += new System.EventHandler(this.ImportActionMenuClick);
            // 
            // saveActionMenuItem
            // 
            this.saveActionMenuItem.Name = "saveActionMenuItem";
            resources.ApplyResources(this.saveActionMenuItem, "saveActionMenuItem");
            this.saveActionMenuItem.Tag = "13";
            this.saveActionMenuItem.Click += new System.EventHandler(this.SaveActionMenuClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Tag = "14";
            // 
            // saveWindowSettingsToolStripMenuItem
            // 
            this.saveWindowSettingsToolStripMenuItem.Name = "saveWindowSettingsToolStripMenuItem";
            resources.ApplyResources(this.saveWindowSettingsToolStripMenuItem, "saveWindowSettingsToolStripMenuItem");
            this.saveWindowSettingsToolStripMenuItem.Tag = "15";
            this.saveWindowSettingsToolStripMenuItem.Click += new System.EventHandler(this.SaveWindowSettingsClick);
            // 
            // loadWindowSettingsToolStripMenuItem
            // 
            this.loadWindowSettingsToolStripMenuItem.Name = "loadWindowSettingsToolStripMenuItem";
            resources.ApplyResources(this.loadWindowSettingsToolStripMenuItem, "loadWindowSettingsToolStripMenuItem");
            this.loadWindowSettingsToolStripMenuItem.Tag = "16";
            this.loadWindowSettingsToolStripMenuItem.Click += new System.EventHandler(this.LoadWindowSettingsClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Tag = "17";
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.MergeIndex = 7;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            resources.ApplyResources(this.printToolStripMenuItem, "printToolStripMenuItem");
            this.printToolStripMenuItem.Tag = "18";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintMenuClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Tag = "19";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.MergeIndex = 100;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Tag = "20";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClick);
            // 
            // MenuItemEdit
            // 
            this.MenuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.MenuItemEdit.Name = "MenuItemEdit";
            resources.ApplyResources(this.MenuItemEdit, "MenuItemEdit");
            // 
            // undoToolStripMenuItem
            // 
            resources.ApplyResources(this.undoToolStripMenuItem, "undoToolStripMenuItem");
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoMenuClick);
            // 
            // redoToolStripMenuItem
            // 
            resources.ApplyResources(this.redoToolStripMenuItem, "redoToolStripMenuItem");
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoMenuClick);
            // 
            // MenuItemSetup
            // 
            this.MenuItemSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelEditorToolStripMenuItem,
            this.setIDEToolStripMenuItem,
            this.toolStripSeparator7,
            this.distributedEnvToolStripMenuItem});
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
            // setIDEToolStripMenuItem
            // 
            this.setIDEToolStripMenuItem.Name = "setIDEToolStripMenuItem";
            resources.ApplyResources(this.setIDEToolStripMenuItem, "setIDEToolStripMenuItem");
            this.setIDEToolStripMenuItem.Click += new System.EventHandler(this.SetupIDEMenuClick);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // distributedEnvToolStripMenuItem
            // 
            this.distributedEnvToolStripMenuItem.Name = "distributedEnvToolStripMenuItem";
            resources.ApplyResources(this.distributedEnvToolStripMenuItem, "distributedEnvToolStripMenuItem");
            this.distributedEnvToolStripMenuItem.Click += new System.EventHandler(this.ClickDistributedEnvMenu);
            // 
            // MenuItemLayout
            // 
            this.MenuItemLayout.Name = "MenuItemLayout";
            resources.ApplyResources(this.MenuItemLayout, "MenuItemLayout");
            // 
            // MenuItemView
            // 
            this.MenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem,
            this.jobStatusToolStripMenuItem});
            this.MenuItemView.Name = "MenuItemView";
            resources.ApplyResources(this.MenuItemView, "MenuItemView");
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            resources.ApplyResources(this.showWindowToolStripMenuItem, "showWindowToolStripMenuItem");
            // 
            // jobStatusToolStripMenuItem
            // 
            this.jobStatusToolStripMenuItem.Name = "jobStatusToolStripMenuItem";
            resources.ApplyResources(this.jobStatusToolStripMenuItem, "jobStatusToolStripMenuItem");
            this.jobStatusToolStripMenuItem.Click += new System.EventHandler(this.ClickJobStatusMenu);
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
            // openScriptDialog
            // 
            resources.ApplyResources(this.openScriptDialog, "openScriptDialog");
            this.openScriptDialog.RestoreDirectory = true;
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menustrip);
            this.MainMenuStrip = this.menustrip;
            this.Name = "MainWindow";
            this.Disposed += new System.EventHandler(this.MainWindowDisposed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindowFormClosing);
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ContentPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.menustrip.ResumeLayout(false);
            this.menustrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.MenuStrip menustrip;
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
        System.Windows.Forms.OpenFileDialog m_openFileDialog;
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
        private System.Windows.Forms.ToolStripMenuItem saveWindowSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem loadWindowSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setIDEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        /// <summary>
        /// MenuItem to display the setup dialog to set distributed environment.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem distributedEnvToolStripMenuItem;
        /// <summary>
        /// MenuItem to display the list of jobs.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem jobStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectWizardMenuItem;
    }
}

