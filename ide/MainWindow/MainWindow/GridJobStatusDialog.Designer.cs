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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridJobStatusDialog));
            this.JobGridView = new System.Windows.Forms.DataGridView();
            this.JobIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MachineColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScriptColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArgumentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DEWStartButton = new System.Windows.Forms.Button();
            this.DEWStopButton = new System.Windows.Forms.Button();
            this.DEWDeleteButton = new System.Windows.Forms.Button();
            this.DEWUpdateButton = new System.Windows.Forms.Button();
            this.DialogToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.JobGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // JobGridView
            // 
            this.JobGridView.AccessibleDescription = null;
            this.JobGridView.AccessibleName = null;
            this.JobGridView.AllowUserToAddRows = false;
            resources.ApplyResources(this.JobGridView, "JobGridView");
            this.JobGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.JobGridView.BackgroundImage = null;
            this.JobGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.JobGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.JobIDColumn,
            this.StatusColumn,
            this.MachineColumn,
            this.ScriptColumn,
            this.ArgumentColumn});
            this.JobGridView.Font = null;
            this.JobGridView.Name = "JobGridView";
            this.JobGridView.RowHeadersVisible = false;
            this.JobGridView.RowTemplate.Height = 21;
            this.JobGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DialogToolTip.SetToolTip(this.JobGridView, resources.GetString("JobGridView.ToolTip"));
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
            // DEWStartButton
            // 
            this.DEWStartButton.AccessibleDescription = null;
            this.DEWStartButton.AccessibleName = null;
            resources.ApplyResources(this.DEWStartButton, "DEWStartButton");
            this.DEWStartButton.BackgroundImage = null;
            this.DEWStartButton.Font = null;
            this.DEWStartButton.Name = "DEWStartButton";
            this.DialogToolTip.SetToolTip(this.DEWStartButton, resources.GetString("DEWStartButton.ToolTip"));
            this.DEWStartButton.UseVisualStyleBackColor = true;
            this.DEWStartButton.Click += new System.EventHandler(this.DEWStartButton_Click);
            // 
            // DEWStopButton
            // 
            this.DEWStopButton.AccessibleDescription = null;
            this.DEWStopButton.AccessibleName = null;
            resources.ApplyResources(this.DEWStopButton, "DEWStopButton");
            this.DEWStopButton.BackgroundImage = null;
            this.DEWStopButton.Font = null;
            this.DEWStopButton.Name = "DEWStopButton";
            this.DialogToolTip.SetToolTip(this.DEWStopButton, resources.GetString("DEWStopButton.ToolTip"));
            this.DEWStopButton.UseVisualStyleBackColor = true;
            this.DEWStopButton.Click += new System.EventHandler(this.DEWStopButton_Click);
            // 
            // DEWDeleteButton
            // 
            this.DEWDeleteButton.AccessibleDescription = null;
            this.DEWDeleteButton.AccessibleName = null;
            resources.ApplyResources(this.DEWDeleteButton, "DEWDeleteButton");
            this.DEWDeleteButton.BackgroundImage = null;
            this.DEWDeleteButton.Font = null;
            this.DEWDeleteButton.Name = "DEWDeleteButton";
            this.DialogToolTip.SetToolTip(this.DEWDeleteButton, resources.GetString("DEWDeleteButton.ToolTip"));
            this.DEWDeleteButton.UseVisualStyleBackColor = true;
            this.DEWDeleteButton.Click += new System.EventHandler(this.DEWDeleteButtonClick);
            // 
            // DEWUpdateButton
            // 
            this.DEWUpdateButton.AccessibleDescription = null;
            this.DEWUpdateButton.AccessibleName = null;
            resources.ApplyResources(this.DEWUpdateButton, "DEWUpdateButton");
            this.DEWUpdateButton.BackgroundImage = null;
            this.DEWUpdateButton.FlatAppearance.BorderSize = 0;
            this.DEWUpdateButton.Font = null;
            this.DEWUpdateButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.DEWUpdateButton.Name = "DEWUpdateButton";
            this.DialogToolTip.SetToolTip(this.DEWUpdateButton, resources.GetString("DEWUpdateButton.ToolTip"));
            this.DEWUpdateButton.UseVisualStyleBackColor = true;
            this.DEWUpdateButton.Click += new System.EventHandler(this.DEWUpdateButton_Click);
            // 
            // GridJobStatusDialog
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.DEWStartButton);
            this.Controls.Add(this.JobGridView);
            this.Controls.Add(this.DEWStopButton);
            this.Controls.Add(this.DEWDeleteButton);
            this.Controls.Add(this.DEWUpdateButton);
            this.Font = null;
            this.IsSavable = true;
            this.Name = "GridJobStatusDialog";
            this.DialogToolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ToolTipText = null;
            this.Shown += new System.EventHandler(this.WinShown);
            ((System.ComponentModel.ISupportInitialize)(this.JobGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// DataGridView to display the list of jobs.
        /// </summary>
        public System.Windows.Forms.DataGridView JobGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn JobIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MachineColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScriptColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArgumentColumn;
        private System.Windows.Forms.Button DEWDeleteButton;
        private System.Windows.Forms.Button DEWStartButton;
        private System.Windows.Forms.Button DEWStopButton;
        private System.Windows.Forms.Button DEWUpdateButton;
        private System.Windows.Forms.ToolTip DialogToolTip;
    }
}