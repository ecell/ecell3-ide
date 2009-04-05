using System.Security;
using System.Security.Permissions;
namespace Ecell.IDE.MainWindow
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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.MWstatusStrip = new System.Windows.Forms.StatusStrip();
            this.genericStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.quickInspectorText = new System.Windows.Forms.ToolStripStatusLabel();
            this.genericProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.menustrip = new System.Windows.Forms.MenuStrip();
            this.MenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectWizardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentProejctToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSBMLMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
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
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRun = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.importScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowVersionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutIDEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openScriptDialog = new System.Windows.Forms.OpenFileDialog();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.MWstatusStrip.SuspendLayout();
            this.menustrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            this.toolStripContainer.AccessibleDescription = null;
            this.toolStripContainer.AccessibleName = null;
            resources.ApplyResources(this.toolStripContainer, "toolStripContainer");
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer.BottomToolStripPanel.AccessibleName = null;
            this.toolStripContainer.BottomToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer.BottomToolStripPanel, "toolStripContainer.BottomToolStripPanel");
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.MWstatusStrip);
            this.toolStripContainer.BottomToolStripPanel.Font = null;
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.AccessibleDescription = null;
            this.toolStripContainer.ContentPanel.AccessibleName = null;
            resources.ApplyResources(this.toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
            this.toolStripContainer.ContentPanel.BackgroundImage = null;
            this.toolStripContainer.ContentPanel.Controls.Add(this.dockPanel);
            this.toolStripContainer.ContentPanel.Font = null;
            this.toolStripContainer.Font = null;
            // 
            // toolStripContainer.LeftToolStripPanel
            // 
            this.toolStripContainer.LeftToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer.LeftToolStripPanel.AccessibleName = null;
            this.toolStripContainer.LeftToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer.LeftToolStripPanel, "toolStripContainer.LeftToolStripPanel");
            this.toolStripContainer.LeftToolStripPanel.Font = null;
            this.toolStripContainer.Name = "toolStripContainer";
            // 
            // toolStripContainer.RightToolStripPanel
            // 
            this.toolStripContainer.RightToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer.RightToolStripPanel.AccessibleName = null;
            this.toolStripContainer.RightToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer.RightToolStripPanel, "toolStripContainer.RightToolStripPanel");
            this.toolStripContainer.RightToolStripPanel.Font = null;
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.AccessibleDescription = null;
            this.toolStripContainer.TopToolStripPanel.AccessibleName = null;
            this.toolStripContainer.TopToolStripPanel.BackgroundImage = null;
            resources.ApplyResources(this.toolStripContainer.TopToolStripPanel, "toolStripContainer.TopToolStripPanel");
            this.toolStripContainer.TopToolStripPanel.Font = null;
            // 
            // MWstatusStrip
            // 
            this.MWstatusStrip.AccessibleDescription = null;
            this.MWstatusStrip.AccessibleName = null;
            resources.ApplyResources(this.MWstatusStrip, "MWstatusStrip");
            this.MWstatusStrip.BackgroundImage = null;
            this.MWstatusStrip.Font = null;
            this.MWstatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.genericStatusText,
            this.quickInspectorText,
            this.genericProgressBar});
            this.MWstatusStrip.Name = "MWstatusStrip";
            this.MWstatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // genericStatusText
            // 
            this.genericStatusText.AccessibleDescription = null;
            this.genericStatusText.AccessibleName = null;
            resources.ApplyResources(this.genericStatusText, "genericStatusText");
            this.genericStatusText.BackgroundImage = null;
            this.genericStatusText.Name = "genericStatusText";
            this.genericStatusText.Spring = true;
            // 
            // quickInspectorText
            // 
            this.quickInspectorText.AccessibleDescription = null;
            this.quickInspectorText.AccessibleName = null;
            resources.ApplyResources(this.quickInspectorText, "quickInspectorText");
            this.quickInspectorText.BackgroundImage = null;
            this.quickInspectorText.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.quickInspectorText.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.quickInspectorText.Name = "quickInspectorText";
            // 
            // genericProgressBar
            // 
            this.genericProgressBar.AccessibleDescription = null;
            this.genericProgressBar.AccessibleName = null;
            this.genericProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.genericProgressBar, "genericProgressBar");
            this.genericProgressBar.Name = "genericProgressBar";
            // 
            // dockPanel
            // 
            this.dockPanel.AccessibleDescription = null;
            this.dockPanel.AccessibleName = null;
            this.dockPanel.ActiveAutoHideContent = null;
            resources.ApplyResources(this.dockPanel, "dockPanel");
            this.dockPanel.BackgroundImage = null;
            this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel.Name = "dockPanel";
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.AccessibleDescription = null;
            toolStripSeparator8.AccessibleName = null;
            resources.ApplyResources(toolStripSeparator8, "toolStripSeparator8");
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Tag = "1";
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
            this.MenuItemTools,
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
            this.projectWizardMenuItem,
            this.openProjectToolStripMenuItem,
            this.recentProejctToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.importModelToolStripMenuItem,
            this.exportModelToolStripMenuItem,
            this.importSBMLMenuItem,
            this.toolStripSeparator2,
            this.saveScriptToolStripMenuItem,
            this.toolStripSeparator5,
            this.toolStripSeparator6,
            this.saveWindowSettingsToolStripMenuItem,
            this.loadWindowSettingsToolStripMenuItem,
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
            this.newProjectToolStripMenuItem.Tag = "1";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.NewProjectMenuClick);
            // 
            // projectWizardMenuItem
            // 
            this.projectWizardMenuItem.AccessibleDescription = null;
            this.projectWizardMenuItem.AccessibleName = null;
            resources.ApplyResources(this.projectWizardMenuItem, "projectWizardMenuItem");
            this.projectWizardMenuItem.BackgroundImage = null;
            this.projectWizardMenuItem.Name = "projectWizardMenuItem";
            this.projectWizardMenuItem.ShortcutKeyDisplayString = null;
            this.projectWizardMenuItem.Tag = "2";
            this.projectWizardMenuItem.Click += new System.EventHandler(this.LoadProjectWizardMenuClick);
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
            this.openProjectToolStripMenuItem.Tag = "3";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.LoadProjectMenuClick);
            // 
            // recentProejctToolStripMenuItem
            // 
            this.recentProejctToolStripMenuItem.AccessibleDescription = null;
            this.recentProejctToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.recentProejctToolStripMenuItem, "recentProejctToolStripMenuItem");
            this.recentProejctToolStripMenuItem.BackgroundImage = null;
            this.recentProejctToolStripMenuItem.Name = "recentProejctToolStripMenuItem";
            this.recentProejctToolStripMenuItem.ShortcutKeyDisplayString = null;
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
            this.saveProjectToolStripMenuItem.Tag = "4";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectMenuClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.AccessibleDescription = null;
            this.saveAsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.BackgroundImage = null;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
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
            this.closeProjectToolStripMenuItem.Tag = "5";
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.CloseProjectMenuClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Tag = "10";
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
            this.importModelToolStripMenuItem.Tag = "11";
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
            this.exportModelToolStripMenuItem.Tag = "12";
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.ExportModelMenuClick);
            // 
            // importSBMLMenuItem
            // 
            this.importSBMLMenuItem.AccessibleDescription = null;
            this.importSBMLMenuItem.AccessibleName = null;
            resources.ApplyResources(this.importSBMLMenuItem, "importSBMLMenuItem");
            this.importSBMLMenuItem.BackgroundImage = null;
            this.importSBMLMenuItem.Name = "importSBMLMenuItem";
            this.importSBMLMenuItem.ShortcutKeyDisplayString = null;
            this.importSBMLMenuItem.Click += new System.EventHandler(this.ImportSBMLMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Tag = "20";
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
            this.saveScriptToolStripMenuItem.Tag = "22";
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.SaveScriptMenuClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.AccessibleDescription = null;
            this.toolStripSeparator5.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Tag = "30";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.AccessibleDescription = null;
            this.toolStripSeparator6.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Tag = "40";
            // 
            // saveWindowSettingsToolStripMenuItem
            // 
            this.saveWindowSettingsToolStripMenuItem.AccessibleDescription = null;
            this.saveWindowSettingsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.saveWindowSettingsToolStripMenuItem, "saveWindowSettingsToolStripMenuItem");
            this.saveWindowSettingsToolStripMenuItem.BackgroundImage = null;
            this.saveWindowSettingsToolStripMenuItem.Name = "saveWindowSettingsToolStripMenuItem";
            this.saveWindowSettingsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.saveWindowSettingsToolStripMenuItem.Tag = "41";
            this.saveWindowSettingsToolStripMenuItem.Click += new System.EventHandler(this.SaveWindowSettingsClick);
            // 
            // loadWindowSettingsToolStripMenuItem
            // 
            this.loadWindowSettingsToolStripMenuItem.AccessibleDescription = null;
            this.loadWindowSettingsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.loadWindowSettingsToolStripMenuItem, "loadWindowSettingsToolStripMenuItem");
            this.loadWindowSettingsToolStripMenuItem.BackgroundImage = null;
            this.loadWindowSettingsToolStripMenuItem.Name = "loadWindowSettingsToolStripMenuItem";
            this.loadWindowSettingsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.loadWindowSettingsToolStripMenuItem.Tag = "42";
            this.loadWindowSettingsToolStripMenuItem.Click += new System.EventHandler(this.LoadWindowSettingsClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AccessibleDescription = null;
            this.toolStripSeparator3.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Tag = "50";
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
            this.printToolStripMenuItem.Tag = "51";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintMenuClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AccessibleDescription = null;
            this.toolStripSeparator4.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Tag = "60";
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
            this.exitToolStripMenuItem.Tag = "61";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClick);
            // 
            // MenuItemEdit
            // 
            this.MenuItemEdit.AccessibleDescription = null;
            this.MenuItemEdit.AccessibleName = null;
            resources.ApplyResources(this.MenuItemEdit, "MenuItemEdit");
            this.MenuItemEdit.BackgroundImage = null;
            this.MenuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.MenuItemEdit.Name = "MenuItemEdit";
            this.MenuItemEdit.ShortcutKeyDisplayString = null;
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.AccessibleDescription = null;
            this.undoToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.undoToolStripMenuItem, "undoToolStripMenuItem");
            this.undoToolStripMenuItem.BackgroundImage = null;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoMenuClick);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.AccessibleDescription = null;
            this.redoToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.redoToolStripMenuItem, "redoToolStripMenuItem");
            this.redoToolStripMenuItem.BackgroundImage = null;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoMenuClick);
            // 
            // MenuItemSetup
            // 
            this.MenuItemSetup.AccessibleDescription = null;
            this.MenuItemSetup.AccessibleName = null;
            resources.ApplyResources(this.MenuItemSetup, "MenuItemSetup");
            this.MenuItemSetup.BackgroundImage = null;
            this.MenuItemSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelEditorToolStripMenuItem,
            this.setIDEToolStripMenuItem,
            this.settingsToolStripMenuItem});
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
            // setIDEToolStripMenuItem
            // 
            this.setIDEToolStripMenuItem.AccessibleDescription = null;
            this.setIDEToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.setIDEToolStripMenuItem, "setIDEToolStripMenuItem");
            this.setIDEToolStripMenuItem.BackgroundImage = null;
            this.setIDEToolStripMenuItem.Name = "setIDEToolStripMenuItem";
            this.setIDEToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.setIDEToolStripMenuItem.Click += new System.EventHandler(this.SetupIDEMenuClick);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.AccessibleDescription = null;
            this.settingsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.BackgroundImage = null;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsMenuClick);
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
            this.MenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem});
            this.MenuItemView.Name = "MenuItemView";
            this.MenuItemView.ShortcutKeyDisplayString = null;
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.AccessibleDescription = null;
            this.showWindowToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.showWindowToolStripMenuItem, "showWindowToolStripMenuItem");
            this.showWindowToolStripMenuItem.BackgroundImage = null;
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            this.showWindowToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // MenuItemRun
            // 
            this.MenuItemRun.AccessibleDescription = null;
            this.MenuItemRun.AccessibleName = null;
            resources.ApplyResources(this.MenuItemRun, "MenuItemRun");
            this.MenuItemRun.BackgroundImage = null;
            this.MenuItemRun.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator10,
            this.importScriptToolStripMenuItem});
            this.MenuItemRun.Name = "MenuItemRun";
            this.MenuItemRun.ShortcutKeyDisplayString = null;
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.AccessibleDescription = null;
            this.toolStripSeparator10.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Tag = "40";
            // 
            // importScriptToolStripMenuItem
            // 
            this.importScriptToolStripMenuItem.AccessibleDescription = null;
            this.importScriptToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.importScriptToolStripMenuItem, "importScriptToolStripMenuItem");
            this.importScriptToolStripMenuItem.BackgroundImage = null;
            this.importScriptToolStripMenuItem.Name = "importScriptToolStripMenuItem";
            this.importScriptToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.importScriptToolStripMenuItem.Tag = "50";
            this.importScriptToolStripMenuItem.Click += new System.EventHandler(this.ImportScriptMenuClick);
            // 
            // MenuItemTools
            // 
            this.MenuItemTools.AccessibleDescription = null;
            this.MenuItemTools.AccessibleName = null;
            resources.ApplyResources(this.MenuItemTools, "MenuItemTools");
            this.MenuItemTools.BackgroundImage = null;
            this.MenuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptEditorToolStripMenuItem,
            toolStripSeparator8});
            this.MenuItemTools.Name = "MenuItemTools";
            this.MenuItemTools.ShortcutKeyDisplayString = null;
            // 
            // scriptEditorToolStripMenuItem
            // 
            this.scriptEditorToolStripMenuItem.AccessibleDescription = null;
            this.scriptEditorToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.scriptEditorToolStripMenuItem, "scriptEditorToolStripMenuItem");
            this.scriptEditorToolStripMenuItem.BackgroundImage = null;
            this.scriptEditorToolStripMenuItem.Name = "scriptEditorToolStripMenuItem";
            this.scriptEditorToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.scriptEditorToolStripMenuItem.Tag = "0";
            this.scriptEditorToolStripMenuItem.Click += new System.EventHandler(this.ClickScriptEditorMenu);
            // 
            // MenuItemHelp
            // 
            this.MenuItemHelp.AccessibleDescription = null;
            this.MenuItemHelp.AccessibleName = null;
            resources.ApplyResources(this.MenuItemHelp, "MenuItemHelp");
            this.MenuItemHelp.BackgroundImage = null;
            this.MenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowVersionMenuItem,
            this.aboutIDEToolStripMenuItem,
            this.feedbackToolStripMenuItem});
            this.MenuItemHelp.Name = "MenuItemHelp";
            this.MenuItemHelp.ShortcutKeyDisplayString = null;
            // 
            // ShowVersionMenuItem
            // 
            this.ShowVersionMenuItem.AccessibleDescription = null;
            this.ShowVersionMenuItem.AccessibleName = null;
            resources.ApplyResources(this.ShowVersionMenuItem, "ShowVersionMenuItem");
            this.ShowVersionMenuItem.BackgroundImage = null;
            this.ShowVersionMenuItem.Name = "ShowVersionMenuItem";
            this.ShowVersionMenuItem.ShortcutKeyDisplayString = null;
            this.ShowVersionMenuItem.Tag = "100";
            this.ShowVersionMenuItem.Click += new System.EventHandler(this.ShowPluginVersionClick);
            // 
            // aboutIDEToolStripMenuItem
            // 
            this.aboutIDEToolStripMenuItem.AccessibleDescription = null;
            this.aboutIDEToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.aboutIDEToolStripMenuItem, "aboutIDEToolStripMenuItem");
            this.aboutIDEToolStripMenuItem.BackgroundImage = null;
            this.aboutIDEToolStripMenuItem.Name = "aboutIDEToolStripMenuItem";
            this.aboutIDEToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.aboutIDEToolStripMenuItem.Click += new System.EventHandler(this.ShowAboutDialog);
            // 
            // feedbackToolStripMenuItem
            // 
            this.feedbackToolStripMenuItem.AccessibleDescription = null;
            this.feedbackToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.feedbackToolStripMenuItem, "feedbackToolStripMenuItem");
            this.feedbackToolStripMenuItem.BackgroundImage = null;
            this.feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            this.feedbackToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.feedbackToolStripMenuItem.Click += new System.EventHandler(this.feedbackToolStripMenuItem_Click);
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
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menustrip);
            this.Font = null;
            this.MainMenuStrip = this.menustrip;
            this.Name = "MainWindow";
            this.Disposed += new System.EventHandler(this.MainWindowDisposed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindowFormClosing);
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ContentPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.MWstatusStrip.ResumeLayout(false);
            this.MWstatusStrip.PerformLayout();
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
        System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSetup;
        /// <summary>
        /// MenuItem to set the working directory.
        /// </summary>
        public System.Windows.Forms.ToolStripMenuItem modelEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRun;
        private System.Windows.Forms.ToolStripMenuItem MenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuItemView;
        private System.Windows.Forms.OpenFileDialog openScriptDialog;
        private System.Windows.Forms.ToolStripMenuItem MenuItemEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem MenuItemTools;
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
        private System.Windows.Forms.ToolStripMenuItem projectWizardMenuItem;
        private System.Windows.Forms.StatusStrip MWstatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel genericStatusText;
        private System.Windows.Forms.ToolStripProgressBar genericProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel quickInspectorText;
        private System.Windows.Forms.ToolStripMenuItem scriptEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutIDEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSBMLMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentProejctToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem importScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem feedbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    }
}

