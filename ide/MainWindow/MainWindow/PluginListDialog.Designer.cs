namespace Ecell.IDE.MainWindow
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
            this.okButton = new System.Windows.Forms.Button();
            this.versionListView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.versionListView)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // versionListView
            // 
            this.versionListView.AllowUserToAddRows = false;
            this.versionListView.AllowUserToDeleteRows = false;
            this.versionListView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.versionListView, "versionListView");
            this.versionListView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.versionListView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.versionListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.versionListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.VersionColumn});
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
            this.NameColumn.FillWeight = 200F;
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
            // PluginListDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.versionListView);
            this.Name = "PluginListDialog";
            ((System.ComponentModel.ISupportInitialize)(this.versionListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        /// <summary>
        /// DataGridView to display the version list of plugin.
        /// </summary>
        public System.Windows.Forms.DataGridView versionListView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionColumn;
    }
}