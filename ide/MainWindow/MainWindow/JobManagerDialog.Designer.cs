namespace Ecell.IDE.MainWindow
{
    partial class JobManagerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobManagerDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.envComboBox = new System.Windows.Forms.ComboBox();
            this.envDataGridView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.concTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.envDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // envComboBox
            // 
            resources.ApplyResources(this.envComboBox, "envComboBox");
            this.envComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.envComboBox.FormattingEnabled = true;
            this.envComboBox.Name = "envComboBox";
            this.envComboBox.SelectedIndexChanged += new System.EventHandler(this.envComboBox_SelectedIndexChanged);
            // 
            // envDataGridView
            // 
            this.envDataGridView.AllowUserToAddRows = false;
            this.envDataGridView.AllowUserToDeleteRows = false;
            this.envDataGridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.envDataGridView, "envDataGridView");
            this.envDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.envDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.envDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.ValueColumn});
            this.envDataGridView.Name = "envDataGridView";
            this.envDataGridView.RowHeadersVisible = false;
            this.envDataGridView.RowTemplate.Height = 21;
            // 
            // NameColumn
            // 
            resources.ApplyResources(this.NameColumn, "NameColumn");
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // ValueColumn
            // 
            resources.ApplyResources(this.ValueColumn, "ValueColumn");
            this.ValueColumn.Name = "ValueColumn";
            // 
            // concTextBox
            // 
            resources.ApplyResources(this.concTextBox, "concTextBox");
            this.concTextBox.Name = "concTextBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // JobManagerDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.concTextBox);
            this.Controls.Add(this.envDataGridView);
            this.Controls.Add(this.envComboBox);
            this.Controls.Add(this.label1);
            this.Name = "JobManagerDialog";
            ((System.ComponentModel.ISupportInitialize)(this.envDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox envComboBox;
        private System.Windows.Forms.DataGridView envDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueColumn;
        private System.Windows.Forms.TextBox concTextBox;
        private System.Windows.Forms.Label label2;
    }
}
