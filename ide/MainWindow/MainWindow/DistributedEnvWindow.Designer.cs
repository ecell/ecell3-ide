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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.JobGridView = new System.Windows.Forms.DataGridView();
            this.JobIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScriptColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ArgumentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JobGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.JobGridView, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(392, 333);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.button1.Location = new System.Drawing.Point(157, 301);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
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
            this.ScriptColumn,
            this.ArgumentColumn});
            this.JobGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JobGridView.Location = new System.Drawing.Point(3, 3);
            this.JobGridView.Name = "JobGridView";
            this.JobGridView.RowHeadersVisible = false;
            this.JobGridView.RowTemplate.Height = 21;
            this.JobGridView.Size = new System.Drawing.Size(386, 292);
            this.JobGridView.TabIndex = 1;
            // 
            // JobIDColumn
            // 
            this.JobIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.JobIDColumn.HeaderText = "JobID";
            this.JobIDColumn.Name = "JobIDColumn";
            this.JobIDColumn.Width = 50;
            // 
            // StatusColumn
            // 
            this.StatusColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.Width = 80;
            // 
            // ScriptColumn
            // 
            this.ScriptColumn.FillWeight = 50F;
            this.ScriptColumn.HeaderText = "Script";
            this.ScriptColumn.Name = "ScriptColumn";
            // 
            // ArgumentColumn
            // 
            this.ArgumentColumn.HeaderText = "Arg";
            this.ArgumentColumn.Name = "ArgumentColumn";
            // 
            // DistributedEnvWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 333);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DistributedEnvWindow";
            this.Text = "DistributedEnvWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.JobGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.DataGridView JobGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn JobIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScriptColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArgumentColumn;
    }
}