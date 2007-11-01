namespace EcellLib.MainWindow
{
    partial class DistributedEnvWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DistributedEnvWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.JobGridView = new System.Windows.Forms.DataGridView();
            this.JobIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MachineColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScriptColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArgumentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.DEWCloseButton = new System.Windows.Forms.Button();
            this.DEWClearButton = new System.Windows.Forms.Button();
            this.DEWDeleteButton = new System.Windows.Forms.Button();
            this.DEWUpdateButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JobGridView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.JobGridView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // JobGridView
            // 
            this.JobGridView.AllowUserToAddRows = false;
            this.JobGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.JobGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.JobGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JobIDColumn,
            this.StatusColumn,
            this.MachineColumn,
            this.ScriptColumn,
            this.ArgumentColumn});
            resources.ApplyResources(this.JobGridView, "JobGridView");
            this.JobGridView.Name = "JobGridView";
            this.JobGridView.RowHeadersVisible = false;
            this.JobGridView.RowTemplate.Height = 21;
            this.JobGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // JobIDColumn
            // 
            this.JobIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.JobIDColumn, "JobIDColumn");
            this.JobIDColumn.Name = "JobIDColumn";
            this.JobIDColumn.ReadOnly = true;
            // 
            // StatusColumn
            // 
            this.StatusColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.StatusColumn, "StatusColumn");
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            // 
            // MachineColumn
            // 
            this.MachineColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.MachineColumn, "MachineColumn");
            this.MachineColumn.Name = "MachineColumn";
            this.MachineColumn.ReadOnly = true;
            // 
            // ScriptColumn
            // 
            this.ScriptColumn.FillWeight = 50F;
            resources.ApplyResources(this.ScriptColumn, "ScriptColumn");
            this.ScriptColumn.Name = "ScriptColumn";
            this.ScriptColumn.ReadOnly = true;
            // 
            // ArgumentColumn
            // 
            resources.ApplyResources(this.ArgumentColumn, "ArgumentColumn");
            this.ArgumentColumn.Name = "ArgumentColumn";
            this.ArgumentColumn.ReadOnly = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.DEWCloseButton, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.DEWClearButton, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.DEWDeleteButton, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.DEWUpdateButton, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // DEWCloseButton
            // 
            resources.ApplyResources(this.DEWCloseButton, "DEWCloseButton");
            this.DEWCloseButton.Name = "DEWCloseButton";
            this.DEWCloseButton.UseVisualStyleBackColor = true;
            this.DEWCloseButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // DEWClearButton
            // 
            resources.ApplyResources(this.DEWClearButton, "DEWClearButton");
            this.DEWClearButton.Name = "DEWClearButton";
            this.DEWClearButton.UseVisualStyleBackColor = true;
            this.DEWClearButton.Click += new System.EventHandler(this.DEWClearButtonClick);
            // 
            // DEWDeleteButton
            // 
            resources.ApplyResources(this.DEWDeleteButton, "DEWDeleteButton");
            this.DEWDeleteButton.Name = "DEWDeleteButton";
            this.DEWDeleteButton.UseVisualStyleBackColor = true;
            this.DEWDeleteButton.Click += new System.EventHandler(this.DEWDeleteButtonClick);
            // 
            // DEWUpdateButton
            // 
            resources.ApplyResources(this.DEWUpdateButton, "DEWUpdateButton");
            this.DEWUpdateButton.Name = "DEWUpdateButton";
            this.DEWUpdateButton.UseVisualStyleBackColor = true;
            this.DEWUpdateButton.Click += new System.EventHandler(this.DEWUpdateButton_Click);
            // 
            // DistributedEnvWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DistributedEnvWindow";
            this.Shown += new System.EventHandler(this.WinShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.JobGridView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button DEWCloseButton;
        /// <summary>
        /// DataGridView to display the list of jobs.
        /// </summary>
        public System.Windows.Forms.DataGridView JobGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn JobIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MachineColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScriptColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArgumentColumn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button DEWClearButton;
        private System.Windows.Forms.Button DEWDeleteButton;
        private System.Windows.Forms.Button DEWUpdateButton;
    }
}