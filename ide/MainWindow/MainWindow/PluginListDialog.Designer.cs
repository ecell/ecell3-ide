namespace EcellLib.MainWindow
{
    partial class PluginListDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginListDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PVOKButton = new System.Windows.Forms.Button();
            this.versionListView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.versionListView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.PVOKButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.versionListView, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // PVOKButton
            // 
            resources.ApplyResources(this.PVOKButton, "PVOKButton");
            this.PVOKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.PVOKButton.Name = "PVOKButton";
            this.PVOKButton.UseVisualStyleBackColor = true;
            this.PVOKButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // versionListView
            // 
            this.versionListView.AllowUserToAddRows = false;
            this.versionListView.AllowUserToDeleteRows = false;
            this.versionListView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.versionListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.versionListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.VersionColumn});
            resources.ApplyResources(this.versionListView, "versionListView");
            this.versionListView.MultiSelect = false;
            this.versionListView.Name = "versionListView";
            this.versionListView.ReadOnly = true;
            this.versionListView.RowHeadersVisible = false;
            this.versionListView.RowTemplate.Height = 21;
            this.versionListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // NameColumn
            // 
            this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.NameColumn, "NameColumn");
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // VersionColumn
            // 
            resources.ApplyResources(this.VersionColumn, "VersionColumn");
            this.VersionColumn.Name = "VersionColumn";
            this.VersionColumn.ReadOnly = true;
            // 
            // PluginVersionListWindow
            // 
            this.AcceptButton = this.PVOKButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.PVOKButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PluginVersionListWindow";
            this.Shown += new System.EventHandler(this.WindowShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.versionListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button PVOKButton;
        /// <summary>
        /// DataGridView to display the version list of plugin.
        /// </summary>
        public System.Windows.Forms.DataGridView versionListView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionColumn;
    }
}