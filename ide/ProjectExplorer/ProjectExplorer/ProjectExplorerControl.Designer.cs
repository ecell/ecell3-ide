namespace Ecell.IDE.Plugins.ProjectExplorer
{
    /// <summary>
    /// ProjectExplorerControl UserControl.
    /// </summary>
    partial class ProjectExplorerControl
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripMenuItem createNewDMToolStripMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorerControl));
            System.Windows.Forms.ToolStrip toolStrip1;
            this.toolStripButtonSortByType = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSortByName = new System.Windows.Forms.ToolStripButton();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new Ecell.IDE.Plugins.ProjectExplorer.MultiSelectTreeView();
            this.contextMenuStripDM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDMCollection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importDMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripModel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.plotGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSimulationSetCollection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSimulationSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSimulationSet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copySimulationSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSimulationSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.configureSimulationSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStripProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.projectSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewRevisionOnProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripRevisions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createNewRevisionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripRevision = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadRevisionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportRevisionEMLMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportRevisionZipMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripStepper = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripJobGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteJobGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            createNewDMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStrip1.SuspendLayout();
            this.contextMenuStripDM.SuspendLayout();
            this.contextMenuStripDMCollection.SuspendLayout();
            this.contextMenuStripModel.SuspendLayout();
            this.contextMenuStripLog.SuspendLayout();
            this.contextMenuSimulationSetCollection.SuspendLayout();
            this.contextMenuStripSimulationSet.SuspendLayout();
            this.contextMenuStripProject.SuspendLayout();
            this.contextMenuStripRevisions.SuspendLayout();
            this.contextMenuStripRevision.SuspendLayout();
            this.contextMenuStripStepper.SuspendLayout();
            this.contextMenuStripJobGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // createNewDMToolStripMenuItem
            // 
            createNewDMToolStripMenuItem.AccessibleDescription = null;
            createNewDMToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(createNewDMToolStripMenuItem, "createNewDMToolStripMenuItem");
            createNewDMToolStripMenuItem.BackgroundImage = null;
            createNewDMToolStripMenuItem.Name = "createNewDMToolStripMenuItem";
            createNewDMToolStripMenuItem.ShortcutKeyDisplayString = null;
            createNewDMToolStripMenuItem.Click += new System.EventHandler(this.TreeViewNewDm);
            // 
            // toolStrip1
            // 
            toolStrip1.AccessibleDescription = null;
            toolStrip1.AccessibleName = null;
            resources.ApplyResources(toolStrip1, "toolStrip1");
            toolStrip1.BackgroundImage = null;
            toolStrip1.Font = null;
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSortByType,
            this.toolStripButtonSortByName});
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Stretch = true;
            // 
            // toolStripButtonSortByType
            // 
            this.toolStripButtonSortByType.AccessibleDescription = null;
            this.toolStripButtonSortByType.AccessibleName = null;
            resources.ApplyResources(this.toolStripButtonSortByType, "toolStripButtonSortByType");
            this.toolStripButtonSortByType.BackgroundImage = null;
            this.toolStripButtonSortByType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortByType.Image = global::Ecell.IDE.Plugins.ProjectExplorer.Resources.SortByType;
            this.toolStripButtonSortByType.Name = "toolStripButtonSortByType";
            this.toolStripButtonSortByType.Click += new System.EventHandler(this.TreeViewSortByType);
            // 
            // toolStripButtonSortByName
            // 
            this.toolStripButtonSortByName.AccessibleDescription = null;
            this.toolStripButtonSortByName.AccessibleName = null;
            resources.ApplyResources(this.toolStripButtonSortByName, "toolStripButtonSortByName");
            this.toolStripButtonSortByName.BackgroundImage = null;
            this.toolStripButtonSortByName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortByName.Image = global::Ecell.IDE.Plugins.ProjectExplorer.Resources.SortByName;
            this.toolStripButtonSortByName.Name = "toolStripButtonSortByName";
            this.toolStripButtonSortByName.Click += new System.EventHandler(this.TreeViewSortByName);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.AccessibleDescription = null;
            this.editToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            this.editToolStripMenuItem.BackgroundImage = null;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.editToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDMDisplay);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.AccessibleDescription = null;
            this.compileToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.compileToolStripMenuItem, "compileToolStripMenuItem");
            this.compileToolStripMenuItem.BackgroundImage = null;
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCompile);
            // 
            // treeView1
            // 
            this.treeView1.AccessibleDescription = null;
            this.treeView1.AccessibleName = null;
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.BackgroundImage = null;
            this.treeView1.Environment = null;
            this.treeView1.Font = null;
            this.treeView1.HideSelection = false;
            this.treeView1.IsDrag = false;
            this.treeView1.Name = "treeView1";
            this.treeView1.TabStop = false;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeDoubleClick);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeExpand);
            this.treeView1.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeCollapse);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeMouseClick);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeViewItemDrag);
            // 
            // contextMenuStripDM
            // 
            this.contextMenuStripDM.AccessibleDescription = null;
            this.contextMenuStripDM.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripDM, "contextMenuStripDM");
            this.contextMenuStripDM.BackgroundImage = null;
            this.contextMenuStripDM.Font = null;
            this.contextMenuStripDM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripSeparator5,
            this.propertyToolStripMenuItem});
            this.contextMenuStripDM.Name = "contextMenuStripDM";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.AccessibleDescription = null;
            this.toolStripSeparator5.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.AccessibleDescription = null;
            this.propertyToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.propertyToolStripMenuItem, "propertyToolStripMenuItem");
            this.propertyToolStripMenuItem.BackgroundImage = null;
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            this.propertyToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDMProperty);
            // 
            // contextMenuStripDMCollection
            // 
            this.contextMenuStripDMCollection.AccessibleDescription = null;
            this.contextMenuStripDMCollection.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripDMCollection, "contextMenuStripDMCollection");
            this.contextMenuStripDMCollection.BackgroundImage = null;
            this.contextMenuStripDMCollection.Font = null;
            this.contextMenuStripDMCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            createNewDMToolStripMenuItem,
            this.importDMToolStripMenuItem});
            this.contextMenuStripDMCollection.Name = "contextMenuStripDMCollection";
            // 
            // importDMToolStripMenuItem
            // 
            this.importDMToolStripMenuItem.AccessibleDescription = null;
            this.importDMToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.importDMToolStripMenuItem, "importDMToolStripMenuItem");
            this.importDMToolStripMenuItem.BackgroundImage = null;
            this.importDMToolStripMenuItem.Name = "importDMToolStripMenuItem";
            this.importDMToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.importDMToolStripMenuItem.Click += new System.EventHandler(this.TreeViewImportDM);
            // 
            // contextMenuStripModel
            // 
            this.contextMenuStripModel.AccessibleDescription = null;
            this.contextMenuStripModel.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripModel, "contextMenuStripModel");
            this.contextMenuStripModel.BackgroundImage = null;
            this.contextMenuStripModel.Font = null;
            this.contextMenuStripModel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportModelToolStripMenuItem,
            this.toolStripSeparator4,
            this.addToolStripMenuItem});
            this.contextMenuStripModel.Name = "contextMenuStripModel";
            // 
            // exportModelToolStripMenuItem
            // 
            this.exportModelToolStripMenuItem.AccessibleDescription = null;
            this.exportModelToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.exportModelToolStripMenuItem, "exportModelToolStripMenuItem");
            this.exportModelToolStripMenuItem.BackgroundImage = null;
            this.exportModelToolStripMenuItem.Name = "exportModelToolStripMenuItem";
            this.exportModelToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.TreeViewExportModel);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.AccessibleDescription = null;
            this.toolStripSeparator4.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.AccessibleDescription = null;
            this.addToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.addToolStripMenuItem, "addToolStripMenuItem");
            this.addToolStripMenuItem.BackgroundImage = null;
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stepperToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // stepperToolStripMenuItem
            // 
            this.stepperToolStripMenuItem.AccessibleDescription = null;
            this.stepperToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.stepperToolStripMenuItem, "stepperToolStripMenuItem");
            this.stepperToolStripMenuItem.BackgroundImage = null;
            this.stepperToolStripMenuItem.Name = "stepperToolStripMenuItem";
            this.stepperToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.stepperToolStripMenuItem.Click += new System.EventHandler(this.TreeViewAddStepper);
            // 
            // contextMenuStripLog
            // 
            this.contextMenuStripLog.AccessibleDescription = null;
            this.contextMenuStripLog.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripLog, "contextMenuStripLog");
            this.contextMenuStripLog.BackgroundImage = null;
            this.contextMenuStripLog.Font = null;
            this.contextMenuStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plotGraphToolStripMenuItem,
            this.editLogToolStripMenuItem,
            this.toolStripSeparator2,
            this.exportToolStripMenuItem});
            this.contextMenuStripLog.Name = "contextMenuStripLog";
            // 
            // plotGraphToolStripMenuItem
            // 
            this.plotGraphToolStripMenuItem.AccessibleDescription = null;
            this.plotGraphToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.plotGraphToolStripMenuItem, "plotGraphToolStripMenuItem");
            this.plotGraphToolStripMenuItem.BackgroundImage = null;
            this.plotGraphToolStripMenuItem.Name = "plotGraphToolStripMenuItem";
            this.plotGraphToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.plotGraphToolStripMenuItem.Click += new System.EventHandler(this.TreeViewShowLogOnGraph);
            // 
            // editLogToolStripMenuItem
            // 
            this.editLogToolStripMenuItem.AccessibleDescription = null;
            this.editLogToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.editLogToolStripMenuItem, "editLogToolStripMenuItem");
            this.editLogToolStripMenuItem.BackgroundImage = null;
            this.editLogToolStripMenuItem.Name = "editLogToolStripMenuItem";
            this.editLogToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.editLogToolStripMenuItem.Click += new System.EventHandler(this.TreeViewLogDisplayWithApp);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.AccessibleDescription = null;
            this.exportToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.exportToolStripMenuItem, "exportToolStripMenuItem");
            this.exportToolStripMenuItem.BackgroundImage = null;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.TreeViewExportLog);
            // 
            // contextMenuSimulationSetCollection
            // 
            this.contextMenuSimulationSetCollection.AccessibleDescription = null;
            this.contextMenuSimulationSetCollection.AccessibleName = null;
            resources.ApplyResources(this.contextMenuSimulationSetCollection, "contextMenuSimulationSetCollection");
            this.contextMenuSimulationSetCollection.BackgroundImage = null;
            this.contextMenuSimulationSetCollection.Font = null;
            this.contextMenuSimulationSetCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSimulationSetToolStripMenuItem});
            this.contextMenuSimulationSetCollection.Name = "contextMenuSimulationSetCollection";
            // 
            // addSimulationSetToolStripMenuItem
            // 
            this.addSimulationSetToolStripMenuItem.AccessibleDescription = null;
            this.addSimulationSetToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.addSimulationSetToolStripMenuItem, "addSimulationSetToolStripMenuItem");
            this.addSimulationSetToolStripMenuItem.BackgroundImage = null;
            this.addSimulationSetToolStripMenuItem.Name = "addSimulationSetToolStripMenuItem";
            this.addSimulationSetToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.addSimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewAddSimulationSet);
            // 
            // contextMenuStripSimulationSet
            // 
            this.contextMenuStripSimulationSet.AccessibleDescription = null;
            this.contextMenuStripSimulationSet.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripSimulationSet, "contextMenuStripSimulationSet");
            this.contextMenuStripSimulationSet.BackgroundImage = null;
            this.contextMenuStripSimulationSet.Font = null;
            this.contextMenuStripSimulationSet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySimulationSetToolStripMenuItem,
            this.deleteSimulationSetToolStripMenuItem,
            this.toolStripSeparator3,
            this.configureSimulationSetToolStripMenuItem});
            this.contextMenuStripSimulationSet.Name = "contextMenuStripSimulationSet";
            // 
            // copySimulationSetToolStripMenuItem
            // 
            this.copySimulationSetToolStripMenuItem.AccessibleDescription = null;
            this.copySimulationSetToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.copySimulationSetToolStripMenuItem, "copySimulationSetToolStripMenuItem");
            this.copySimulationSetToolStripMenuItem.BackgroundImage = null;
            this.copySimulationSetToolStripMenuItem.Name = "copySimulationSetToolStripMenuItem";
            this.copySimulationSetToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.copySimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCopySimulationSet);
            // 
            // deleteSimulationSetToolStripMenuItem
            // 
            this.deleteSimulationSetToolStripMenuItem.AccessibleDescription = null;
            this.deleteSimulationSetToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.deleteSimulationSetToolStripMenuItem, "deleteSimulationSetToolStripMenuItem");
            this.deleteSimulationSetToolStripMenuItem.BackgroundImage = null;
            this.deleteSimulationSetToolStripMenuItem.Name = "deleteSimulationSetToolStripMenuItem";
            this.deleteSimulationSetToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.deleteSimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDeleteSimulationSet);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AccessibleDescription = null;
            this.toolStripSeparator3.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // configureSimulationSetToolStripMenuItem
            // 
            this.configureSimulationSetToolStripMenuItem.AccessibleDescription = null;
            this.configureSimulationSetToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.configureSimulationSetToolStripMenuItem, "configureSimulationSetToolStripMenuItem");
            this.configureSimulationSetToolStripMenuItem.BackgroundImage = null;
            this.configureSimulationSetToolStripMenuItem.Name = "configureSimulationSetToolStripMenuItem";
            this.configureSimulationSetToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.configureSimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewConfigureSimulationSet);
            // 
            // m_saveFileDialog
            // 
            resources.ApplyResources(this.m_saveFileDialog, "m_saveFileDialog");
            // 
            // contextMenuStripProject
            // 
            this.contextMenuStripProject.AccessibleDescription = null;
            this.contextMenuStripProject.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripProject, "contextMenuStripProject");
            this.contextMenuStripProject.BackgroundImage = null;
            this.contextMenuStripProject.Font = null;
            this.contextMenuStripProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectSettingsToolStripMenuItem,
            this.createNewRevisionOnProjectToolStripMenuItem,
            this.zipToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.contextMenuStripProject.Name = "contextMenuStripProject";
            // 
            // projectSettingsToolStripMenuItem
            // 
            this.projectSettingsToolStripMenuItem.AccessibleDescription = null;
            this.projectSettingsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.projectSettingsToolStripMenuItem, "projectSettingsToolStripMenuItem");
            this.projectSettingsToolStripMenuItem.BackgroundImage = null;
            this.projectSettingsToolStripMenuItem.Name = "projectSettingsToolStripMenuItem";
            this.projectSettingsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.projectSettingsToolStripMenuItem.Click += new System.EventHandler(this.TreeViewSetProject);
            // 
            // createNewRevisionOnProjectToolStripMenuItem
            // 
            this.createNewRevisionOnProjectToolStripMenuItem.AccessibleDescription = null;
            this.createNewRevisionOnProjectToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.createNewRevisionOnProjectToolStripMenuItem, "createNewRevisionOnProjectToolStripMenuItem");
            this.createNewRevisionOnProjectToolStripMenuItem.BackgroundImage = null;
            this.createNewRevisionOnProjectToolStripMenuItem.Name = "createNewRevisionOnProjectToolStripMenuItem";
            this.createNewRevisionOnProjectToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.createNewRevisionOnProjectToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCreateNewRevision);
            // 
            // zipToolStripMenuItem
            // 
            this.zipToolStripMenuItem.AccessibleDescription = null;
            this.zipToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.zipToolStripMenuItem, "zipToolStripMenuItem");
            this.zipToolStripMenuItem.BackgroundImage = null;
            this.zipToolStripMenuItem.Name = "zipToolStripMenuItem";
            this.zipToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.zipToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCompressZip);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.AccessibleDescription = null;
            this.closeToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
            this.closeToolStripMenuItem.BackgroundImage = null;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCloseProject);
            // 
            // contextMenuStripRevisions
            // 
            this.contextMenuStripRevisions.AccessibleDescription = null;
            this.contextMenuStripRevisions.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripRevisions, "contextMenuStripRevisions");
            this.contextMenuStripRevisions.BackgroundImage = null;
            this.contextMenuStripRevisions.Font = null;
            this.contextMenuStripRevisions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewRevisionMenuItem});
            this.contextMenuStripRevisions.Name = "contextMenuStripModel";
            // 
            // createNewRevisionMenuItem
            // 
            this.createNewRevisionMenuItem.AccessibleDescription = null;
            this.createNewRevisionMenuItem.AccessibleName = null;
            resources.ApplyResources(this.createNewRevisionMenuItem, "createNewRevisionMenuItem");
            this.createNewRevisionMenuItem.BackgroundImage = null;
            this.createNewRevisionMenuItem.Name = "createNewRevisionMenuItem";
            this.createNewRevisionMenuItem.ShortcutKeyDisplayString = null;
            this.createNewRevisionMenuItem.Click += new System.EventHandler(this.TreeViewCreateNewRevision);
            // 
            // contextMenuStripRevision
            // 
            this.contextMenuStripRevision.AccessibleDescription = null;
            this.contextMenuStripRevision.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripRevision, "contextMenuStripRevision");
            this.contextMenuStripRevision.BackgroundImage = null;
            this.contextMenuStripRevision.Font = null;
            this.contextMenuStripRevision.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadRevisionMenuItem,
            this.exportRevisionEMLMenuItem,
            this.exportRevisionZipMenuItem});
            this.contextMenuStripRevision.Name = "contextMenuStripModel";
            // 
            // loadRevisionMenuItem
            // 
            this.loadRevisionMenuItem.AccessibleDescription = null;
            this.loadRevisionMenuItem.AccessibleName = null;
            resources.ApplyResources(this.loadRevisionMenuItem, "loadRevisionMenuItem");
            this.loadRevisionMenuItem.BackgroundImage = null;
            this.loadRevisionMenuItem.Name = "loadRevisionMenuItem";
            this.loadRevisionMenuItem.ShortcutKeyDisplayString = null;
            this.loadRevisionMenuItem.Click += new System.EventHandler(this.TreeViewLoadRevision);
            // 
            // exportRevisionEMLMenuItem
            // 
            this.exportRevisionEMLMenuItem.AccessibleDescription = null;
            this.exportRevisionEMLMenuItem.AccessibleName = null;
            resources.ApplyResources(this.exportRevisionEMLMenuItem, "exportRevisionEMLMenuItem");
            this.exportRevisionEMLMenuItem.BackgroundImage = null;
            this.exportRevisionEMLMenuItem.Name = "exportRevisionEMLMenuItem";
            this.exportRevisionEMLMenuItem.ShortcutKeyDisplayString = null;
            this.exportRevisionEMLMenuItem.Click += new System.EventHandler(this.TreeViewExportRevision);
            // 
            // exportRevisionZipMenuItem
            // 
            this.exportRevisionZipMenuItem.AccessibleDescription = null;
            this.exportRevisionZipMenuItem.AccessibleName = null;
            resources.ApplyResources(this.exportRevisionZipMenuItem, "exportRevisionZipMenuItem");
            this.exportRevisionZipMenuItem.BackgroundImage = null;
            this.exportRevisionZipMenuItem.Name = "exportRevisionZipMenuItem";
            this.exportRevisionZipMenuItem.ShortcutKeyDisplayString = null;
            this.exportRevisionZipMenuItem.Click += new System.EventHandler(this.TreeViewExportRevisionToZip);
            // 
            // contextMenuStripStepper
            // 
            this.contextMenuStripStepper.AccessibleDescription = null;
            this.contextMenuStripStepper.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripStepper, "contextMenuStripStepper");
            this.contextMenuStripStepper.BackgroundImage = null;
            this.contextMenuStripStepper.Font = null;
            this.contextMenuStripStepper.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStripStepper.Name = "contextMenuStripStepper";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.AccessibleDescription = null;
            this.deleteToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.BackgroundImage = null;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDeleteStepper);
            // 
            // contextMenuStripJobGroup
            // 
            this.contextMenuStripJobGroup.AccessibleDescription = null;
            this.contextMenuStripJobGroup.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStripJobGroup, "contextMenuStripJobGroup");
            this.contextMenuStripJobGroup.BackgroundImage = null;
            this.contextMenuStripJobGroup.Font = null;
            this.contextMenuStripJobGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewResultToolStripMenuItem,
            this.toolStripSeparator6,
            this.deleteJobGroupToolStripMenuItem});
            this.contextMenuStripJobGroup.Name = "contextMenuStripJobGroup";
            // 
            // viewResultToolStripMenuItem
            // 
            this.viewResultToolStripMenuItem.AccessibleDescription = null;
            this.viewResultToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.viewResultToolStripMenuItem, "viewResultToolStripMenuItem");
            this.viewResultToolStripMenuItem.BackgroundImage = null;
            this.viewResultToolStripMenuItem.Name = "viewResultToolStripMenuItem";
            this.viewResultToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.viewResultToolStripMenuItem.Click += new System.EventHandler(this.TreeView_ViewResult);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.AccessibleDescription = null;
            this.toolStripSeparator6.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // deleteJobGroupToolStripMenuItem
            // 
            this.deleteJobGroupToolStripMenuItem.AccessibleDescription = null;
            this.deleteJobGroupToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.deleteJobGroupToolStripMenuItem, "deleteJobGroupToolStripMenuItem");
            this.deleteJobGroupToolStripMenuItem.BackgroundImage = null;
            this.deleteJobGroupToolStripMenuItem.Name = "deleteJobGroupToolStripMenuItem";
            this.deleteJobGroupToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.deleteJobGroupToolStripMenuItem.Click += new System.EventHandler(this.TreeView_DeleteJobGroup);
            // 
            // ProjectExplorerControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(toolStrip1);
            this.Controls.Add(this.treeView1);
            this.Name = "ProjectExplorerControl";
            this.TabText = "ProjectExplorerControl";
            this.ToolTipText = null;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            this.contextMenuStripDM.ResumeLayout(false);
            this.contextMenuStripDMCollection.ResumeLayout(false);
            this.contextMenuStripModel.ResumeLayout(false);
            this.contextMenuStripLog.ResumeLayout(false);
            this.contextMenuSimulationSetCollection.ResumeLayout(false);
            this.contextMenuStripSimulationSet.ResumeLayout(false);
            this.contextMenuStripProject.ResumeLayout(false);
            this.contextMenuStripRevisions.ResumeLayout(false);
            this.contextMenuStripRevision.ResumeLayout(false);
            this.contextMenuStripStepper.ResumeLayout(false);
            this.contextMenuStripJobGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// treeView1
        /// </summary>
        public Ecell.IDE.Plugins.ProjectExplorer.MultiSelectTreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDM;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDMCollection;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripModel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuSimulationSetCollection;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSimulationSet;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripProject;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRevisions;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRevision;
        private System.Windows.Forms.ToolStripButton toolStripButtonSortByType;
        private System.Windows.Forms.ToolStripButton toolStripButtonSortByName;
        private System.Windows.Forms.ToolStripMenuItem createNewRevisionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadRevisionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plotGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem addSimulationSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySimulationSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSimulationSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem configureSimulationSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewRevisionOnProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zipToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportRevisionEMLMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportRevisionZipMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepperToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripStepper;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripJobGroup;
        private System.Windows.Forms.ToolStripMenuItem viewResultToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem deleteJobGroupToolStripMenuItem;

    }
}
