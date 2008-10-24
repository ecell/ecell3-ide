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
            System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectExplorerControl));
            System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem variableToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
            System.Windows.Forms.ToolStripMenuItem addADMToolStripMenuItem;
            System.Windows.Forms.ToolStrip toolStrip1;
            this.toolStripButtonSortByType = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSortByName = new System.Windows.Forms.ToolStripButton();
            this.toolStripMenuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLogging = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemObservation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemParameter = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripStdEntity = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemMerge = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new Ecell.IDE.Plugins.ProjectExplorer.MultiSelectTreeView();
            this.contextMenuStripDM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripDMCollection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStripModel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewRevisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.configureSimulationSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            variableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            addADMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStrip1.SuspendLayout();
            this.contextMenuStripStdEntity.SuspendLayout();
            this.contextMenuStripDM.SuspendLayout();
            this.contextMenuStripDMCollection.SuspendLayout();
            this.contextMenuStripModel.SuspendLayout();
            this.contextMenuStripLog.SuspendLayout();
            this.contextMenuSimulationSetCollection.SuspendLayout();
            this.contextMenuStripSimulationSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // compileToolStripMenuItem
            // 
            compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            resources.ApplyResources(compileToolStripMenuItem, "compileToolStripMenuItem");
            compileToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCompile);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(editToolStripMenuItem, "editToolStripMenuItem");
            editToolStripMenuItem.Click += new System.EventHandler(this.TreeViewDMDisplay);
            // 
            // systemToolStripMenuItem
            // 
            systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            resources.ApplyResources(systemToolStripMenuItem, "systemToolStripMenuItem");
            systemToolStripMenuItem.Click += new System.EventHandler(this.TreeviewAddSystem);
            // 
            // variableToolStripMenuItem
            // 
            variableToolStripMenuItem.Name = "variableToolStripMenuItem";
            resources.ApplyResources(variableToolStripMenuItem, "variableToolStripMenuItem");
            variableToolStripMenuItem.Click += new System.EventHandler(this.TreeviewAddVariable);
            // 
            // processToolStripMenuItem
            // 
            processToolStripMenuItem.Name = "processToolStripMenuItem";
            resources.ApplyResources(processToolStripMenuItem, "processToolStripMenuItem");
            processToolStripMenuItem.Click += new System.EventHandler(this.TreeviewAddProcess);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(toolStripSeparator4, "toolStripSeparator4");
            // 
            // addADMToolStripMenuItem
            // 
            addADMToolStripMenuItem.Name = "addADMToolStripMenuItem";
            resources.ApplyResources(addADMToolStripMenuItem, "addADMToolStripMenuItem");
            addADMToolStripMenuItem.Click += new System.EventHandler(this.TreeViewNewDm);
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
            // toolStripMenuItemAdd
            // 
            this.toolStripMenuItemAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            systemToolStripMenuItem,
            variableToolStripMenuItem,
            processToolStripMenuItem});
            this.toolStripMenuItemAdd.Name = "toolStripMenuItemAdd";
            resources.ApplyResources(this.toolStripMenuItemAdd, "toolStripMenuItemAdd");
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            resources.ApplyResources(this.toolStripMenuItemDelete, "toolStripMenuItemDelete");
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.TreeviewDelete);
            // 
            // toolStripMenuItemLogging
            // 
            this.toolStripMenuItemLogging.Name = "toolStripMenuItemLogging";
            resources.ApplyResources(this.toolStripMenuItemLogging, "toolStripMenuItemLogging");
            // 
            // toolStripMenuItemObservation
            // 
            this.toolStripMenuItemObservation.Name = "toolStripMenuItemObservation";
            resources.ApplyResources(this.toolStripMenuItemObservation, "toolStripMenuItemObservation");
            // 
            // toolStripMenuItemParameter
            // 
            this.toolStripMenuItemParameter.Name = "toolStripMenuItemParameter";
            resources.ApplyResources(this.toolStripMenuItemParameter, "toolStripMenuItemParameter");
            // 
            // contextMenuStripStdEntity
            // 
            this.contextMenuStripStdEntity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAdd,
            this.toolStripMenuItemDelete,
            this.toolStripMenuItemMerge,
            toolStripSeparator4,
            this.toolStripMenuItemLogging,
            this.toolStripMenuItemObservation,
            this.toolStripMenuItemParameter,
            this.toolStripSeparator1,
            this.toolStripMenuItemProperties});
            this.contextMenuStripStdEntity.Name = "contextMenuVariable";
            resources.ApplyResources(this.contextMenuStripStdEntity, "contextMenuStripStdEntity");
            // 
            // toolStripMenuItemMerge
            // 
            this.toolStripMenuItemMerge.Name = "toolStripMenuItemMerge";
            resources.ApplyResources(this.toolStripMenuItemMerge, "toolStripMenuItemMerge");
            this.toolStripMenuItemMerge.Click += new System.EventHandler(this.TreeviewMerge);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripMenuItemProperties
            // 
            this.toolStripMenuItemProperties.Name = "toolStripMenuItemProperties";
            resources.ApplyResources(this.toolStripMenuItemProperties, "toolStripMenuItemProperties");
            this.toolStripMenuItemProperties.Click += new System.EventHandler(this.TreeViewElementProperties);
            // 
            // treeView1
            // 
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.Environment = null;
            this.treeView1.HideSelection = false;
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
            this.contextMenuStripDM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            compileToolStripMenuItem,
            editToolStripMenuItem});
            this.contextMenuStripDM.Name = "contextMenuStripDM";
            resources.ApplyResources(this.contextMenuStripDM, "contextMenuStripDM");
            // 
            // contextMenuStripDMCollection
            // 
            this.contextMenuStripDMCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            addADMToolStripMenuItem});
            this.contextMenuStripDMCollection.Name = "contextMenuStripDMCollection";
            resources.ApplyResources(this.contextMenuStripDMCollection, "contextMenuStripDMCollection");
            // 
            // contextMenuStripModel
            // 
            this.contextMenuStripModel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportModelToolStripMenuItem,
            this.createNewRevisionToolStripMenuItem});
            this.contextMenuStripModel.Name = "contextMenuStripModel";
            resources.ApplyResources(this.contextMenuStripModel, "contextMenuStripModel");
            // 
            // exportModelToolStripMenuItem
            // 
            this.exportModelToolStripMenuItem.Name = "exportModelToolStripMenuItem";
            resources.ApplyResources(this.exportModelToolStripMenuItem, "exportModelToolStripMenuItem");
            this.exportModelToolStripMenuItem.Click += new System.EventHandler(this.TreeViewExportModel);
            // 
            // createNewRevisionToolStripMenuItem
            // 
            this.createNewRevisionToolStripMenuItem.Name = "createNewRevisionToolStripMenuItem";
            resources.ApplyResources(this.createNewRevisionToolStripMenuItem, "createNewRevisionToolStripMenuItem");
            this.createNewRevisionToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCreateNewRevision);
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
            this.addSimulationSetToolStripMenuItem});
            this.contextMenuSimulationSetCollection.Name = "contextMenuSimulationSetCollection";
            resources.ApplyResources(this.contextMenuSimulationSetCollection, "contextMenuSimulationSetCollection");
            // 
            // addSimulationSetToolStripMenuItem
            // 
            this.addSimulationSetToolStripMenuItem.Name = "addSimulationSetToolStripMenuItem";
            resources.ApplyResources(this.addSimulationSetToolStripMenuItem, "addSimulationSetToolStripMenuItem");
            this.addSimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewAddSimulationSet);
            // 
            // contextMenuStripSimulationSet
            // 
            this.contextMenuStripSimulationSet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySimulationSetToolStripMenuItem,
            this.deleteSimulationSetToolStripMenuItem,
            this.toolStripSeparator3,
            this.configureSimulationSetToolStripMenuItem});
            this.contextMenuStripSimulationSet.Name = "contextMenuStripSimulationSet";
            resources.ApplyResources(this.contextMenuStripSimulationSet, "contextMenuStripSimulationSet");
            // 
            // copySimulationSetToolStripMenuItem
            // 
            this.copySimulationSetToolStripMenuItem.Name = "copySimulationSetToolStripMenuItem";
            resources.ApplyResources(this.copySimulationSetToolStripMenuItem, "copySimulationSetToolStripMenuItem");
            this.copySimulationSetToolStripMenuItem.Click += new System.EventHandler(this.TreeViewCopySimulationSet);
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
            // ProjectExplorerControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(toolStrip1);
            this.Controls.Add(this.treeView1);
            this.Name = "ProjectExplorerControl";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            this.contextMenuStripStdEntity.ResumeLayout(false);
            this.contextMenuStripDM.ResumeLayout(false);
            this.contextMenuStripDMCollection.ResumeLayout(false);
            this.contextMenuStripModel.ResumeLayout(false);
            this.contextMenuStripLog.ResumeLayout(false);
            this.contextMenuSimulationSetCollection.ResumeLayout(false);
            this.contextMenuStripSimulationSet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// treeView1
        /// </summary>
        public Ecell.IDE.Plugins.ProjectExplorer.MultiSelectTreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDM;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripStdEntity;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMerge;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLogging;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemObservation;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemParameter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAdd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDMCollection;
        private System.Windows.Forms.ToolStripButton toolStripButtonSortByType;
        private System.Windows.Forms.ToolStripButton toolStripButtonSortByName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemProperties;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripModel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLog;
        private System.Windows.Forms.ToolStripMenuItem plotGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuSimulationSetCollection;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSimulationSet;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem addSimulationSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySimulationSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSimulationSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewRevisionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem configureSimulationSetToolStripMenuItem;

    }
}
