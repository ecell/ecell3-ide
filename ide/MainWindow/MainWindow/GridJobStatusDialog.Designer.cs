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
            this.DialogToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.jobTreeView = new System.Windows.Forms.TreeView();
            this.parameterDataGridView = new System.Windows.Forms.DataGridView();
            this.jobIDTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.PropNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PropValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.parameterDataGridView)).BeginInit();
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
            // DialogToolTip
            // 
            this.DialogToolTip.ShowAlways = true;
            // 
            // jobTreeView
            // 
            resources.ApplyResources(this.jobTreeView, "jobTreeView");
            this.jobTreeView.Name = "jobTreeView";
            this.jobTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeNodeMouseClick);
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
            this.DockStateChanged += new System.EventHandler(this.GridJobStatusDialog_DockStateChanged);
            ((System.ComponentModel.ISupportInitialize)(this.parameterDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolTip DialogToolTip;
        private System.Windows.Forms.TreeView jobTreeView;
        private System.Windows.Forms.DataGridView parameterDataGridView;
        private System.Windows.Forms.TextBox jobIDTextBox;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropValueColumn;
    }
}