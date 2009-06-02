namespace Ecell.IDE.MainWindow
{
    partial class GridJobStatusDialog
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridJobStatusDialog));
            System.Windows.Forms.Label label2;
            this.jobTreeView = new System.Windows.Forms.TreeView();
            this.jobStatusImageList = new System.Windows.Forms.ImageList(this.components);
            this.parameterDataGridView = new System.Windows.Forms.DataGridView();
            this.PropNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PropValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jobIDTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.jobContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.jobRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.queueStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.jobDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobGroupContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.jobGroupRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobGroupStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.jobGroupLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobGroupSaveStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.jobGroupDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.parameterDataGridView)).BeginInit();
            this.jobContextMenuStrip.SuspendLayout();
            this.jobGroupContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // jobTreeView
            // 
            resources.ApplyResources(this.jobTreeView, "jobTreeView");
            this.jobTreeView.ImageList = this.jobStatusImageList;
            this.jobTreeView.Name = "jobTreeView";
            // 
            // jobStatusImageList
            // 
            this.jobStatusImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("jobStatusImageList.ImageStream")));
            this.jobStatusImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.jobStatusImageList.Images.SetKeyName(0, "gear.png");
            this.jobStatusImageList.Images.SetKeyName(1, "gear_pause.png");
            this.jobStatusImageList.Images.SetKeyName(2, "gear_run.png");
            this.jobStatusImageList.Images.SetKeyName(3, "gear_preferences.png");
            this.jobStatusImageList.Images.SetKeyName(4, "gear_stop.png");
            this.jobStatusImageList.Images.SetKeyName(5, "gear_error.png");
            this.jobStatusImageList.Images.SetKeyName(6, "media_pause.png");
            this.jobStatusImageList.Images.SetKeyName(7, "media_play_green.png");
            this.jobStatusImageList.Images.SetKeyName(8, "check.png");
            this.jobStatusImageList.Images.SetKeyName(9, "stop.png");
            this.jobStatusImageList.Images.SetKeyName(10, "error.png");
            // 
            // parameterDataGridView
            // 
            this.parameterDataGridView.AllowUserToAddRows = false;
            this.parameterDataGridView.AllowUserToDeleteRows = false;
            this.parameterDataGridView.AllowUserToResizeRows = false;
            this.parameterDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.parameterDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parameterDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropNameColumn,
            this.PropValueColumn});
            resources.ApplyResources(this.parameterDataGridView, "parameterDataGridView");
            this.parameterDataGridView.MultiSelect = false;
            this.parameterDataGridView.Name = "parameterDataGridView";
            this.parameterDataGridView.RowHeadersVisible = false;
            this.parameterDataGridView.RowTemplate.Height = 21;
            this.parameterDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // PropNameColumn
            // 
            resources.ApplyResources(this.PropNameColumn, "PropNameColumn");
            this.PropNameColumn.Name = "PropNameColumn";
            // 
            // PropValueColumn
            // 
            resources.ApplyResources(this.PropValueColumn, "PropValueColumn");
            this.PropValueColumn.Name = "PropValueColumn";
            // 
            // jobIDTextBox
            // 
            resources.ApplyResources(this.jobIDTextBox, "jobIDTextBox");
            this.jobIDTextBox.Name = "jobIDTextBox";
            this.jobIDTextBox.ReadOnly = true;
            // 
            // statusTextBox
            // 
            resources.ApplyResources(this.statusTextBox, "statusTextBox");
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            // 
            // jobContextMenuStrip
            // 
            this.jobContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jobRunToolStripMenuItem,
            this.jobStopToolStripMenuItem,
            this.toolStripSeparator3,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.jobDeleteToolStripMenuItem});
            this.jobContextMenuStrip.Name = "jobContextMenuStrip";
            resources.ApplyResources(this.jobContextMenuStrip, "jobContextMenuStrip");
            // 
            // jobRunToolStripMenuItem
            // 
            this.jobRunToolStripMenuItem.Name = "jobRunToolStripMenuItem";
            resources.ApplyResources(this.jobRunToolStripMenuItem, "jobRunToolStripMenuItem");
            // 
            // jobStopToolStripMenuItem
            // 
            this.jobStopToolStripMenuItem.Name = "jobStopToolStripMenuItem";
            resources.ApplyResources(this.jobStopToolStripMenuItem, "jobStopToolStripMenuItem");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queueStatusToolStripMenuItem,
            this.errorStatusToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // queueStatusToolStripMenuItem
            // 
            this.queueStatusToolStripMenuItem.Name = "queueStatusToolStripMenuItem";
            resources.ApplyResources(this.queueStatusToolStripMenuItem, "queueStatusToolStripMenuItem");
            this.queueStatusToolStripMenuItem.Tag = "Queued";
            // 
            // errorStatusToolStripMenuItem
            // 
            this.errorStatusToolStripMenuItem.Name = "errorStatusToolStripMenuItem";
            resources.ApplyResources(this.errorStatusToolStripMenuItem, "errorStatusToolStripMenuItem");
            this.errorStatusToolStripMenuItem.Tag = "Error";
            this.errorStatusToolStripMenuItem.Click += new System.EventHandler(this.JobTree_ChangeJobStatus);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // jobDeleteToolStripMenuItem
            // 
            this.jobDeleteToolStripMenuItem.Name = "jobDeleteToolStripMenuItem";
            resources.ApplyResources(this.jobDeleteToolStripMenuItem, "jobDeleteToolStripMenuItem");
            // 
            // jobGroupContextMenuStrip
            // 
            this.jobGroupContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jobGroupRunToolStripMenuItem,
            this.jobGroupStopToolStripMenuItem,
            this.toolStripSeparator4,
            this.jobGroupLoadToolStripMenuItem,
            this.jobGroupSaveStripMenuItem,
            this.toolStripSeparator2,
            this.jobGroupDeleteToolStripMenuItem});
            this.jobGroupContextMenuStrip.Name = "jobGroupContextMenuStrip";
            resources.ApplyResources(this.jobGroupContextMenuStrip, "jobGroupContextMenuStrip");
            this.jobGroupContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.JobTree_JobGroupContextOpening);
            // 
            // jobGroupRunToolStripMenuItem
            // 
            this.jobGroupRunToolStripMenuItem.Name = "jobGroupRunToolStripMenuItem";
            resources.ApplyResources(this.jobGroupRunToolStripMenuItem, "jobGroupRunToolStripMenuItem");
            // 
            // jobGroupStopToolStripMenuItem
            // 
            this.jobGroupStopToolStripMenuItem.Name = "jobGroupStopToolStripMenuItem";
            resources.ApplyResources(this.jobGroupStopToolStripMenuItem, "jobGroupStopToolStripMenuItem");
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // jobGroupLoadToolStripMenuItem
            // 
            this.jobGroupLoadToolStripMenuItem.Name = "jobGroupLoadToolStripMenuItem";
            resources.ApplyResources(this.jobGroupLoadToolStripMenuItem, "jobGroupLoadToolStripMenuItem");
            // 
            // jobGroupSaveStripMenuItem
            // 
            this.jobGroupSaveStripMenuItem.Name = "jobGroupSaveStripMenuItem";
            resources.ApplyResources(this.jobGroupSaveStripMenuItem, "jobGroupSaveStripMenuItem");
            this.jobGroupSaveStripMenuItem.Click += new System.EventHandler(this.JobTree_SaveJobGroup);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // jobGroupDeleteToolStripMenuItem
            // 
            this.jobGroupDeleteToolStripMenuItem.Name = "jobGroupDeleteToolStripMenuItem";
            resources.ApplyResources(this.jobGroupDeleteToolStripMenuItem, "jobGroupDeleteToolStripMenuItem");
            // 
            // GridJobStatusDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(label2);
            this.Controls.Add(this.jobIDTextBox);
            this.Controls.Add(this.parameterDataGridView);
            this.Controls.Add(label1);
            this.Controls.Add(this.jobTreeView);
            this.IsSavable = true;
            this.Name = "GridJobStatusDialog";
            this.TabText = "Job status";
            this.Shown += new System.EventHandler(this.WinShown);
            ((System.ComponentModel.ISupportInitialize)(this.parameterDataGridView)).EndInit();
            this.jobContextMenuStrip.ResumeLayout(false);
            this.jobGroupContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView jobTreeView;
        private System.Windows.Forms.DataGridView parameterDataGridView;
        private System.Windows.Forms.TextBox jobIDTextBox;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropValueColumn;
        private System.Windows.Forms.ContextMenuStrip jobContextMenuStrip;
        private System.Windows.Forms.ContextMenuStrip jobGroupContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem jobRunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobStopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem jobDeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem queueStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorStatusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobGroupRunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobGroupStopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem jobGroupDeleteToolStripMenuItem;
        private System.Windows.Forms.ImageList jobStatusImageList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem jobGroupSaveStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobGroupLoadToolStripMenuItem;
    }
}