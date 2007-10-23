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
            this.button1 = new System.Windows.Forms.Button();
            this.JobGridView = new System.Windows.Forms.DataGridView();
            this.JobIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MachineColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScriptColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArgumentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JobGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.JobGridView, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // DistributedEnvWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DistributedEnvWindow";
            this.Shown += new System.EventHandler(this.WinShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.JobGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        /// <summary>
        /// DataGridView to display the list of jobs.
        /// </summary>
        public System.Windows.Forms.DataGridView JobGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn JobIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MachineColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScriptColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArgumentColumn;
    }
}