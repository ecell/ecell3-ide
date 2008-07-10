namespace Ecell.IDE.MainWindow
{
    partial class GridConfigurationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridConfigurationDialog));
            this.DEApplyButton = new System.Windows.Forms.Button();
            this.DECloseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DEEnvComboBox = new System.Windows.Forms.ComboBox();
            this.DEConcTextBox = new System.Windows.Forms.TextBox();
            this.DESearchDir = new System.Windows.Forms.Button();
            this.DEWorkDirTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DEOptionGridView = new System.Windows.Forms.DataGridView();
            this.m_folderSelectDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.DistributeParamNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DistributeValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DEOptionGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // DEApplyButton
            // 
            resources.ApplyResources(this.DEApplyButton, "DEApplyButton");
            this.DEApplyButton.Name = "DEApplyButton";
            this.DEApplyButton.UseVisualStyleBackColor = true;
            this.DEApplyButton.Click += new System.EventHandler(this.DEApplyButton_Click);
            // 
            // DECloseButton
            // 
            resources.ApplyResources(this.DECloseButton, "DECloseButton");
            this.DECloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DECloseButton.Name = "DECloseButton";
            this.DECloseButton.UseVisualStyleBackColor = true;
            this.DECloseButton.Click += new System.EventHandler(this.DECloseButton_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // DEEnvComboBox
            // 
            resources.ApplyResources(this.DEEnvComboBox, "DEEnvComboBox");
            this.DEEnvComboBox.FormattingEnabled = true;
            this.DEEnvComboBox.Name = "DEEnvComboBox";
            // 
            // DEConcTextBox
            // 
            resources.ApplyResources(this.DEConcTextBox, "DEConcTextBox");
            this.DEConcTextBox.Name = "DEConcTextBox";
            // 
            // DESearchDir
            // 
            resources.ApplyResources(this.DESearchDir, "DESearchDir");
            this.DESearchDir.Name = "DESearchDir";
            this.DESearchDir.UseVisualStyleBackColor = true;
            this.DESearchDir.Click += new System.EventHandler(this.DESearchDir_Click);
            // 
            // DEWorkDirTextBox
            // 
            resources.ApplyResources(this.DEWorkDirTextBox, "DEWorkDirTextBox");
            this.DEWorkDirTextBox.Name = "DEWorkDirTextBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // DEOptionGridView
            // 
            this.DEOptionGridView.AllowUserToAddRows = false;
            this.DEOptionGridView.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.DEOptionGridView, "DEOptionGridView");
            this.DEOptionGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DEOptionGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DEOptionGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DistributeParamNameColumn,
            this.DistributeValueColumn});
            this.DEOptionGridView.Name = "DEOptionGridView";
            this.DEOptionGridView.RowHeadersVisible = false;
            this.DEOptionGridView.RowTemplate.Height = 21;
            // 
            // DistributeParamNameColumn
            // 
            this.DistributeParamNameColumn.FillWeight = 50F;
            resources.ApplyResources(this.DistributeParamNameColumn, "DistributeParamNameColumn");
            this.DistributeParamNameColumn.Name = "DistributeParamNameColumn";
            // 
            // DistributeValueColumn
            // 
            resources.ApplyResources(this.DistributeValueColumn, "DistributeValueColumn");
            this.DistributeValueColumn.Name = "DistributeValueColumn";
            // 
            // GridConfigurationDialog
            // 
            this.AcceptButton = this.DEApplyButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DECloseButton;
            this.Controls.Add(this.DECloseButton);
            this.Controls.Add(this.DEApplyButton);
            this.Controls.Add(this.DEOptionGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DESearchDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DEWorkDirTextBox);
            this.Controls.Add(this.DEConcTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DEEnvComboBox);
            this.Name = "GridConfigurationDialog";
            ((System.ComponentModel.ISupportInitialize)(this.DEOptionGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// Button to apply the editing informations.
        /// </summary>
        public System.Windows.Forms.Button DEApplyButton;
        /// <summary>
        /// Button to close this windows.
        /// </summary>
        public System.Windows.Forms.Button DECloseButton;
        /// <summary>
        /// ComboBox to set the environment.
        /// </summary>
        public System.Windows.Forms.ComboBox DEEnvComboBox;
        /// <summary>
        /// TextBox  to set the concurrency of this type.
        /// </summary>
        public System.Windows.Forms.TextBox DEConcTextBox;
        /// <summary>
        /// TextBox to set the working directory.
        /// </summary>
        public System.Windows.Forms.TextBox DEWorkDirTextBox;
        /// <summary>
        /// DataGridView to display the property list of object.
        /// </summary>
        public System.Windows.Forms.DataGridView DEOptionGridView;
        /// <summary>
        /// Button to display the select folder dialog.
        /// </summary>
        public System.Windows.Forms.Button DESearchDir;
        /// <summary>
        /// Dialog to set the working directory.
        /// </summary>
        public System.Windows.Forms.FolderBrowserDialog m_folderSelectDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn DistributeParamNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DistributeValueColumn;
    }
}