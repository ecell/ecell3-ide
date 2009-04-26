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
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.MWstatusStrip = new System.Windows.Forms.StatusStrip();
            this.genericStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.quickInspectorText = new System.Windows.Forms.ToolStripStatusLabel();
            this.genericProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripOpenProjectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSaveProjectButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSaveAsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripUndoButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripRedoButton = new System.Windows.Forms.ToolStripButton();
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
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.MWstatusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menustrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.MWstatusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.dockPanel);
            resources.ApplyResources(this.toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
            resources.ApplyResources(this.toolStripContainer, "toolStripContainer");
            this.toolStripContainer.Name = "toolStripContainer";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // MWstatusStrip
            // 
            resources.ApplyResources(this.MWstatusStrip, "MWstatusStrip");
            this.MWstatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.genericStatusText,
            this.quickInspectorText,
            this.genericProgressBar});
            this.MWstatusStrip.Name = "MWstatusStrip";
            this.MWstatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // genericStatusText
            // 
            this.genericStatusText.Name = "genericStatusText";
            resources.ApplyResources(this.genericStatusText, "genericStatusText");
            this.genericStatusText.Spring = true;
            // 
            // quickInspectorText
            // 
            this.quickInspectorText.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.quickInspectorText.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.quickInspectorText.Name = "quickInspectorText";
            resources.ApplyResources(this.quickInspectorText, "quickInspectorText");
            // 
            // genericProgressBar
            // 
            this.genericProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.genericProgressBar.Name = "genericProgressBar";
            resources.ApplyResources(this.genericProgressBar, "genericProgressBar");
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            resources.ApplyResources(this.dockPanel, "dockPanel");
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel.Name = "dockPanel";
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel.Skin = dockPanelSkin1;
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpenProjectButton,
            this.toolStripSaveProjectButton,
            this.toolStripSaveAsButton,
            this.toolStripSeparator7,
            this.toolStripUndoButton,
            this.toolStripRedoButton});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripOpenProjectButton
            // 
            this.toolStripOpenProjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripOpenProjectButton, "toolStripOpenProjectButton");
            this.toolStripOpenProjectButton.Name = "toolStripOpenProjectButton";
            this.toolStripOpenProjectButton.Click += new System.EventHandler(this.LoadProjectMenuClick);
            // 
            // toolStripSaveProjectButton
            // 
            this.toolStripSaveProjectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripSaveProjectButton, "toolStripSaveProjectButton");
            this.toolStripSaveProjectButton.Name = "toolStripSaveProjectButton";
            this.toolStripSaveProjectButton.Click += new System.EventHandler(this.SaveProjectMenuClick);
            // 
            // toolStripSaveAsButton
            // 
            this.toolStripSaveAsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripSaveAsButton, "toolStripSaveAsButton");
            this.toolStripSaveAsButton.Name = "toolStripSaveAsButton";
            this.toolStripSaveAsButton.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // toolStripUndoButton
            // 
            this.toolStripUndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripUndoButton, "toolStripUndoButton");
            this.toolStripUndoButton.Name = "toolStripUndoButton";
            this.toolStripUndoButton.Click += new System.EventHandler(this.UndoMenuClick);
            // 
            // toolStripRedoButton
            // 
            this.toolStripRedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripRedoButton, "toolStripRedoButton");
            this.toolStripRedoButton.Name = "toolStripRedoButton";
            this.toolStripRedoButton.Click += new System.EventHandler(this.RedoMenuClick);
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(toolStripSeparator8, "toolStripSeparator8");
            toolStripSeparator8.Tag = "40";
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
            this.MenuItemTools,
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
            this.projectWizardMenuItem.Tag = "2";
            this.projectWizardMenuItem.Click += new System.EventHandler(this.LoadProjectWizardMenuClick);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.MergeIndex = 1;
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            resources.ApplyResources(this.openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
            this.openProjectToolStripMenuItem.Tag = "3";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.LoadProjectMenuClick);
            // 
            // recentProejctToolStripMenuItem
            // 
            this.recentProejctToolStripMenuItem.Name = "recentProejctToolStripMenuItem";
            resources.ApplyResources(this.recentProejctToolStripMenuItem, "recentProejctToolStripMenuItem");
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.MergeIndex = 2;
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            resources.ApplyResources(this.saveProjectToolStripMenuItem, "saveProjectToolStripMenuItem");
            this.saveProjectToolStripMenuItem.Tag = "4";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.SaveProjectMenuClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeProjectToolStripMenuItem
            // 
            this.closeProjectToolStripMenuItem.MergeIndex = 3;
            this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
            resources.ApplyResources(this.closeProjectToolStripMenuItem, "closeProjectToolStripMenuItem");
            this.closeProjectToolStripMenuItem.Tag = "5";
            this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.CloseProjectMenuClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Tag = "10";
            // 
            // importModelToolStripMenuItem
            // 
            this.importModelToolStripMenuItem.MergeIndex = 4;
            this.importModelToolStripMenuItem.Name = "importModelToolStripMenuItem";
            resources.ApplyResources(this.importModelToolStripMenuItem, "importModelToolStripMenuItem");
            this.importModelToolStripMenuItem.Tag = "11";
            this.importModelToolStripMenuItem.Click += new System.EventHandler(this.ImportModelMenuClick);
            // 
            // exportModelToolStripMenuItem
            // 
            this.exportModelToolStripMenuItem.MergeIndex = 5;
            this.exportModelToolStripMenuItem.Name = "exportModelToolStripMenuItem";
            resources.ApplyResources(this.exportModelToolStripMenuItem, "exportModelToolStripMenuItem");
            this.exportModelToolStripMenuItem.Tag = "12";
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.ExportModelMenuClick);
            // 
            // importSBMLMenuItem
            // 
            this.importSBMLMenuItem.Name = "importSBMLMenuItem";
            resources.ApplyResources(this.importSBMLMenuItem, "importSBMLMenuItem");
            this.importSBMLMenuItem.Click += new System.EventHandler(this.ImportSBMLMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Tag = "20";
            // 
            // saveScriptToolStripMenuItem
            // 
            this.saveScriptToolStripMenuItem.MergeIndex = 6;
            this.saveScriptToolStripMenuItem.Name = "saveScriptToolStripMenuItem";
            resources.ApplyResources(this.saveScriptToolStripMenuItem, "saveScriptToolStripMenuItem");
            this.saveScriptToolStripMenuItem.Tag = "22";
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.SaveScriptMenuClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Tag = "30";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Tag = "40";
            // 
            // saveWindowSettingsToolStripMenuItem
            // 
            this.saveWindowSettingsToolStripMenuItem.Name = "saveWindowSettingsToolStripMenuItem";
            resources.ApplyResources(this.saveWindowSettingsToolStripMenuItem, "saveWindowSettingsToolStripMenuItem");
            this.saveWindowSettingsToolStripMenuItem.Tag = "41";
            this.saveWindowSettingsToolStripMenuItem.Click += new System.EventHandler(this.SaveWindowSettingsClick);
            // 
            // loadWindowSettingsToolStripMenuItem
            // 
            this.loadWindowSettingsToolStripMenuItem.Name = "loadWindowSettingsToolStripMenuItem";
            resources.ApplyResources(this.loadWindowSettingsToolStripMenuItem, "loadWindowSettingsToolStripMenuItem");
            this.loadWindowSettingsToolStripMenuItem.Tag = "42";
            this.loadWindowSettingsToolStripMenuItem.Click += new System.EventHandler(this.LoadWindowSettingsClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Tag = "50";
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.MergeIndex = 7;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            resources.ApplyResources(this.printToolStripMenuItem, "printToolStripMenuItem");
            this.printToolStripMenuItem.Tag = "51";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintMenuClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Tag = "60";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.MergeIndex = 100;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Tag = "61";
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
            this.settingsToolStripMenuItem});
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
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsMenuClick);
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
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            resources.ApplyResources(this.showWindowToolStripMenuItem, "showWindowToolStripMenuItem");
            // 
            // MenuItemRun
            // 
            this.MenuItemRun.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator10,
            this.importScriptToolStripMenuItem});
            this.MenuItemRun.Name = "MenuItemRun";
            resources.ApplyResources(this.MenuItemRun, "MenuItemRun");
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            this.toolStripSeparator10.Tag = "40";
            // 
            // importScriptToolStripMenuItem
            // 
            this.importScriptToolStripMenuItem.Name = "importScriptToolStripMenuItem";
            resources.ApplyResources(this.importScriptToolStripMenuItem, "importScriptToolStripMenuItem");
            this.importScriptToolStripMenuItem.Tag = "50";
            this.importScriptToolStripMenuItem.Click += new System.EventHandler(this.ImportScriptMenuClick);
            // 
            // MenuItemTools
            // 
            this.MenuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptEditorToolStripMenuItem,
            toolStripSeparator8});
            this.MenuItemTools.Name = "MenuItemTools";
            resources.ApplyResources(this.MenuItemTools, "MenuItemTools");
            // 
            // scriptEditorToolStripMenuItem
            // 
            this.scriptEditorToolStripMenuItem.Name = "scriptEditorToolStripMenuItem";
            resources.ApplyResources(this.scriptEditorToolStripMenuItem, "scriptEditorToolStripMenuItem");
            this.scriptEditorToolStripMenuItem.Tag = "30";
            this.scriptEditorToolStripMenuItem.Click += new System.EventHandler(this.ClickScriptEditorMenu);
            // 
            // MenuItemHelp
            // 
            this.MenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowVersionMenuItem,
            this.aboutIDEToolStripMenuItem,
            this.feedbackToolStripMenuItem});
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
            // aboutIDEToolStripMenuItem
            // 
            this.aboutIDEToolStripMenuItem.Name = "aboutIDEToolStripMenuItem";
            resources.ApplyResources(this.aboutIDEToolStripMenuItem, "aboutIDEToolStripMenuItem");
            this.aboutIDEToolStripMenuItem.Click += new System.EventHandler(this.ShowAboutDialog);
            // 
            // feedbackToolStripMenuItem
            // 
            this.feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            resources.ApplyResources(this.feedbackToolStripMenuItem, "feedbackToolStripMenuItem");
            this.feedbackToolStripMenuItem.Click += new System.EventHandler(this.feedbackToolStripMenuItem_Click);
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
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ContentPanel.PerformLayout();
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.MWstatusStrip.ResumeLayout(false);
            this.MWstatusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripOpenProjectButton;
        private System.Windows.Forms.ToolStripButton toolStripSaveProjectButton;
        private System.Windows.Forms.ToolStripButton toolStripSaveAsButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripUndoButton;
        private System.Windows.Forms.ToolStripButton toolStripRedoButton;
    }
}

