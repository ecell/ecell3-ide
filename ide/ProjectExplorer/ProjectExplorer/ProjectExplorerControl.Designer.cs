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
            this.deleteDMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDMCollection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importDMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripModel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportModelSBMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.importSimParamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSimulationSet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copySimulationSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSimParamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.exportRevisionZipMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripStepper = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripJobGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteJobGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
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
            createNewDMToolStripMenuItem.Name = "createNewDMToolStripMenuItem";
            resources.ApplyResources(createNewDMToolStripMenuItem, "createNewDMToolStripMenuItem");
            createNewDMToolStripMenuItem.Click += new System.EventHandler(this.TreeViewNewDm);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(toolStrip1, "toolStrip1");
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSortByType,
            this.toolStripButtonSortByName});
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Stretch = true;
            // 
            // toolStripButtonSortByType
            // 
            this.toolStripButtonSortByType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortByType.Image = global::Ecell.IDE.Plugins.ProjectExplorer.Resources.SortByType;
            resources.ApplyResources(this.toolStripButtonSortByType, "toolStripButtonSortByType");
            this.toolStripButtonSortByType.Name = "toolStripButtonSortByType";
            this.toolStripButtonSortByType.Click += new System.EventHandler(this.TreeViewSortByType);
            // 
            // toolStripButtonSortByName
            // 
            this.toolStripButtonSortByName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortByName.Image = global::Ecell.IDE.Plugins.ProjectExplorer.Resources.SortByName;
            resources.ApplyResources(this.toolStripButtonSortByName, "toolStripButtonSortByName");
            this.toolStripButtonSortByName.Name = "toolStripButtonSortByName";
            this.toolStripButtonSortByName.Click += new System.EventHandler(this.TreeViewSortByName);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            this.editToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDMDisplay);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            resources.ApplyResources(this.compileToolStripMenuItem, "compileToolStripMenuItem");
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCompile);
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.Environment = null;
            this.treeView1.HideSelection = false;
            this.treeView1.Name = "treeView1";
            this.treeView1.TabStop = false;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeDoubleClick);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeExpand);
            this.treeView1.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewBeforeCollapse);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeViewDragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeViewDragEnter);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.NodeMouseClick);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeViewItemDrag);
            // 
            // contextMenuStripDM
            // 
            this.contextMenuStripDM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripSeparator5,
            this.propertyToolStripMenuItem,
            this.deleteDMToolStripMenuItem});
            this.contextMenuStripDM.Name = "contextMenuStripDM";
            resources.ApplyResources(this.contextMenuStripDM, "contextMenuStripDM");
            this.contextMenuStripDM.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripDM_Opening);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            resources.ApplyResources(this.propertyToolStripMenuItem, "propertyToolStripMenuItem");
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDMProperty);
            // 
            // deleteDMToolStripMenuItem
            // 
            this.deleteDMToolStripMenuItem.Name = "deleteDMToolStripMenuItem";
            resources.ApplyResources(this.deleteDMToolStripMenuItem, "deleteDMToolStripMenuItem");
            this.deleteDMToolStripMenuItem.Click += new System.EventHandler(this.TreeView_DeleteDM);
            // 
            // contextMenuStripDMCollection
            // 
            this.contextMenuStripDMCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            createNewDMToolStripMenuItem,
            this.importDMToolStripMenuItem});
            this.contextMenuStripDMCollection.Name = "contextMenuStripDMCollection";
            resources.ApplyResources(this.contextMenuStripDMCollection, "contextMenuStripDMCollection");
            this.contextMenuStripDMCollection.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripDMCollection_Opening);
            // 
            // importDMToolStripMenuItem
            // 
            this.importDMToolStripMenuItem.Name = "importDMToolStripMenuItem";
            resources.ApplyResources(this.importDMToolStripMenuItem, "importDMToolStripMenuItem");
            this.importDMToolStripMenuItem.Click += new System.EventHandler(this.TreeViewImportDM);
            // 
            // contextMenuStripModel
            // 
            this.contextMenuStripModel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportModelToolStripMenuItem,
            this.exportModelSBMLToolStripMenuItem,
            this.toolStripSeparator4,
            this.addToolStripMenuItem});
            this.contextMenuStripModel.Name = "contextMenuStripModel";
            resources.ApplyResources(this.contextMenuStripModel, "contextMenuStripModel");
            this.contextMenuStripModel.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripModel_Opening);
            // 
            // exportModelToolStripMenuItem
            // 
            this.exportModelToolStripMenuItem.Name = "exportModelToolStripMenuItem";
            resources.ApplyResources(this.exportModelToolStripMenuItem, "exportModelToolStripMenuItem");
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.TreeViewExportModel);
            // 
            // exportModelSBMLToolStripMenuItem
            // 
            this.exportModelSBMLToolStripMenuItem.Name = "exportModelSBMLToolStripMenuItem";
            resources.ApplyResources(this.exportModelSBMLToolStripMenuItem, "exportModelSBMLToolStripMenuItem");
            this.exportModelSBMLToolStripMenuItem.Click += new System.EventHandler(this.TreeViewExportModel2SBML);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stepperToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            resources.ApplyResources(this.addToolStripMenuItem, "addToolStripMenuItem");
            // 
            // stepperToolStripMenuItem
            // 
            this.stepperToolStripMenuItem.Name = "stepperToolStripMenuItem";
            resources.ApplyResources(this.stepperToolStripMenuItem, "stepperToolStripMenuItem");
            this.stepperToolStripMenuItem.Click += new System.EventHandler(this.TreeViewAddStepper);
            // 
            // contextMenuStripLog
            // 
            this.contextMenuStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plotGraphToolStripMenuItem,
            this.editLogToolStripMenuItem,
            this.toolStripSeparator2,
            this.exportToolStripMenuItem});
            this.contextMenuStripLog.Name = "contextMenuStripLog";
            resources.ApplyResources(this.contextMenuStripLog, "contextMenuStripLog");
            // 
            // plotGraphToolStripMenuItem
            // 
            this.plotGraphToolStripMenuItem.Name = "plotGraphToolStripMenuItem";
            resources.ApplyResources(this.plotGraphToolStripMenuItem, "plotGraphToolStripMenuItem");
            this.plotGraphToolStripMenuItem.Click += new System.EventHandler(this.TreeViewShowLogOnGraph);
            // 
            // editLogToolStripMenuItem
            // 
            this.editLogToolStripMenuItem.Name = "editLogToolStripMenuItem";
            resources.ApplyResources(this.editLogToolStripMenuItem, "editLogToolStripMenuItem");
            this.editLogToolStripMenuItem.Click += new System.EventHandler(this.TreeViewLogDisplayWithApp);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            resources.ApplyResources(this.exportToolStripMenuItem, "exportToolStripMenuItem");
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.TreeViewExportLog);
            // 
            // contextMenuSimulationSetCollection
            // 
            this.contextMenuSimulationSetCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSimulationSetToolStripMenuItem,
            this.importSimParamToolStripMenuItem});
            this.contextMenuSimulationSetCollection.Name = "contextMenuSimulationSetCollection";
            resources.ApplyResources(this.contextMenuSimulationSetCollection, "contextMenuSimulationSetCollection");
            this.contextMenuSimulationSetCollection.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuSimulationSetCollection_Opening);
            // 
            // addSimulationSetToolStripMenuItem
            // 
            this.addSimulationSetToolStripMenuItem.Name = "addSimulationSetToolStripMenuItem";
            resources.ApplyResources(this.addSimulationSetToolStripMenuItem, "addSimulationSetToolStripMenuItem");
            this.addSimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewAddSimulationSet);
            // 
            // importSimParamToolStripMenuItem
            // 
            this.importSimParamToolStripMenuItem.Name = "importSimParamToolStripMenuItem";
            resources.ApplyResources(this.importSimParamToolStripMenuItem, "importSimParamToolStripMenuItem");
            this.importSimParamToolStripMenuItem.Click += new System.EventHandler(this.TreeView_ImportSimulationParameter);
            // 
            // contextMenuStripSimulationSet
            // 
            this.contextMenuStripSimulationSet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySimulationSetToolStripMenuItem,
            this.exportSimParamToolStripMenuItem,
            this.deleteSimulationSetToolStripMenuItem,
            this.toolStripSeparator3,
            this.configureSimulationSetToolStripMenuItem});
            this.contextMenuStripSimulationSet.Name = "contextMenuStripSimulationSet";
            resources.ApplyResources(this.contextMenuStripSimulationSet, "contextMenuStripSimulationSet");
            this.contextMenuStripSimulationSet.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripSimulationSet_Opening);
            // 
            // copySimulationSetToolStripMenuItem
            // 
            this.copySimulationSetToolStripMenuItem.Name = "copySimulationSetToolStripMenuItem";
            resources.ApplyResources(this.copySimulationSetToolStripMenuItem, "copySimulationSetToolStripMenuItem");
            this.copySimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCopySimulationSet);
            // 
            // exportSimParamToolStripMenuItem
            // 
            this.exportSimParamToolStripMenuItem.Name = "exportSimParamToolStripMenuItem";
            resources.ApplyResources(this.exportSimParamToolStripMenuItem, "exportSimParamToolStripMenuItem");
            this.exportSimParamToolStripMenuItem.Click += new System.EventHandler(this.TreeView_ExportSimulationParameter);
            // 
            // deleteSimulationSetToolStripMenuItem
            // 
            this.deleteSimulationSetToolStripMenuItem.Name = "deleteSimulationSetToolStripMenuItem";
            resources.ApplyResources(this.deleteSimulationSetToolStripMenuItem, "deleteSimulationSetToolStripMenuItem");
            this.deleteSimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDeleteSimulationSet);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // configureSimulationSetToolStripMenuItem
            // 
            this.configureSimulationSetToolStripMenuItem.Name = "configureSimulationSetToolStripMenuItem";
            resources.ApplyResources(this.configureSimulationSetToolStripMenuItem, "configureSimulationSetToolStripMenuItem");
            this.configureSimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewConfigureSimulationSet);
            // 
            // contextMenuStripProject
            // 
            this.contextMenuStripProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectSettingsToolStripMenuItem,
            this.createNewRevisionOnProjectToolStripMenuItem,
            this.zipToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.contextMenuStripProject.Name = "contextMenuStripProject";
            resources.ApplyResources(this.contextMenuStripProject, "contextMenuStripProject");
            // 
            // projectSettingsToolStripMenuItem
            // 
            this.projectSettingsToolStripMenuItem.Name = "projectSettingsToolStripMenuItem";
            resources.ApplyResources(this.projectSettingsToolStripMenuItem, "projectSettingsToolStripMenuItem");
            this.projectSettingsToolStripMenuItem.Click += new System.EventHandler(this.TreeViewSetProject);
            // 
            // createNewRevisionOnProjectToolStripMenuItem
            // 
            this.createNewRevisionOnProjectToolStripMenuItem.Name = "createNewRevisionOnProjectToolStripMenuItem";
            resources.ApplyResources(this.createNewRevisionOnProjectToolStripMenuItem, "createNewRevisionOnProjectToolStripMenuItem");
            this.createNewRevisionOnProjectToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCreateNewRevision);
            // 
            // zipToolStripMenuItem
            // 
            this.zipToolStripMenuItem.Name = "zipToolStripMenuItem";
            resources.ApplyResources(this.zipToolStripMenuItem, "zipToolStripMenuItem");
            this.zipToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCompressZip);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCloseProject);
            // 
            // contextMenuStripRevisions
            // 
            this.contextMenuStripRevisions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewRevisionMenuItem});
            this.contextMenuStripRevisions.Name = "contextMenuStripModel";
            resources.ApplyResources(this.contextMenuStripRevisions, "contextMenuStripRevisions");
            // 
            // createNewRevisionMenuItem
            // 
            this.createNewRevisionMenuItem.Name = "createNewRevisionMenuItem";
            resources.ApplyResources(this.createNewRevisionMenuItem, "createNewRevisionMenuItem");
            this.createNewRevisionMenuItem.Click += new System.EventHandler(this.TreeViewCreateNewRevision);
            // 
            // contextMenuStripRevision
            // 
            this.contextMenuStripRevision.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadRevisionMenuItem,
            this.exportRevisionZipMenuItem});
            this.contextMenuStripRevision.Name = "contextMenuStripModel";
            resources.ApplyResources(this.contextMenuStripRevision, "contextMenuStripRevision");
            this.contextMenuStripRevision.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripRevision_Opening);
            // 
            // loadRevisionMenuItem
            // 
            this.loadRevisionMenuItem.Name = "loadRevisionMenuItem";
            resources.ApplyResources(this.loadRevisionMenuItem, "loadRevisionMenuItem");
            this.loadRevisionMenuItem.Click += new System.EventHandler(this.TreeViewLoadRevision);
            // 
            // exportRevisionZipMenuItem
            // 
            this.exportRevisionZipMenuItem.Name = "exportRevisionZipMenuItem";
            resources.ApplyResources(this.exportRevisionZipMenuItem, "exportRevisionZipMenuItem");
            this.exportRevisionZipMenuItem.Click += new System.EventHandler(this.TreeViewExportRevisionToZip);
            // 
            // contextMenuStripStepper
            // 
            this.contextMenuStripStepper.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStripStepper.Name = "contextMenuStripStepper";
            resources.ApplyResources(this.contextMenuStripStepper, "contextMenuStripStepper");
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDeleteStepper);
            // 
            // contextMenuStripJobGroup
            // 
            this.contextMenuStripJobGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewResultToolStripMenuItem,
            this.toolStripSeparator6,
            this.deleteJobGroupToolStripMenuItem});
            this.contextMenuStripJobGroup.Name = "contextMenuStripJobGroup";
            resources.ApplyResources(this.contextMenuStripJobGroup, "contextMenuStripJobGroup");
            // 
            // viewResultToolStripMenuItem
            // 
            this.viewResultToolStripMenuItem.Name = "viewResultToolStripMenuItem";
            resources.ApplyResources(this.viewResultToolStripMenuItem, "viewResultToolStripMenuItem");
            this.viewResultToolStripMenuItem.Click += new System.EventHandler(this.TreeView_ViewResult);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // deleteJobGroupToolStripMenuItem
            // 
            this.deleteJobGroupToolStripMenuItem.Name = "deleteJobGroupToolStripMenuItem";
            resources.ApplyResources(this.deleteJobGroupToolStripMenuItem, "deleteJobGroupToolStripMenuItem");
            this.deleteJobGroupToolStripMenuItem.Click += new System.EventHandler(this.TreeView_DeleteJobGroup);
            // 
            // m_openFileDialog
            // 
            this.m_openFileDialog.FileName = "m_openFileDialog";
            // 
            // ProjectExplorerControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(toolStrip1);
            this.Controls.Add(this.treeView1);
            this.Name = "ProjectExplorerControl";
            this.TabText = "ProjectExplorerControl";
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
        private System.Windows.Forms.ToolStripMenuItem exportModelSBMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSimParamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSimParamToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;

    }
}
