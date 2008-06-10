namespace EcellLib.MessageListWindow
{
    partial class MessageListWindowControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageListWindowControl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.MLWMessageDridView = new System.Windows.Forms.DataGridView();
            this.MLWDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MLWTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MLWLocColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MLWMesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MLWMessageDridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.MLWMessageDridView, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(401, 227);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // MLWMessageDridView
            // 
            this.MLWMessageDridView.AllowUserToAddRows = false;
            this.MLWMessageDridView.AllowUserToDeleteRows = false;
            this.MLWMessageDridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MLWMessageDridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MLWMessageDridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MLWDateColumn,
            this.MLWTypeColumn,
            this.MLWLocColumn,
            this.MLWMesColumn});
            this.MLWMessageDridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MLWMessageDridView.Location = new System.Drawing.Point(3, 3);
            this.MLWMessageDridView.Name = "MLWMessageDridView";
            this.MLWMessageDridView.RowHeadersVisible = false;
            this.MLWMessageDridView.RowTemplate.Height = 21;
            this.MLWMessageDridView.Size = new System.Drawing.Size(395, 221);
            this.MLWMessageDridView.TabIndex = 0;
            // 
            // MLWDateColumn
            // 
            this.MLWDateColumn.FillWeight = 30F;
            this.MLWDateColumn.HeaderText = "Date";
            this.MLWDateColumn.Name = "MLWDateColumn";
            // 
            // MLWTypeColumn
            // 
            this.MLWTypeColumn.FillWeight = 15F;
            this.MLWTypeColumn.HeaderText = "Type";
            this.MLWTypeColumn.Name = "MLWTypeColumn";
            // 
            // MLWLocColumn
            // 
            this.MLWLocColumn.FillWeight = 35F;
            this.MLWLocColumn.HeaderText = "Location";
            this.MLWLocColumn.Name = "MLWLocColumn";
            // 
            // MLWMesColumn
            // 
            this.MLWMesColumn.FillWeight = 50F;
            this.MLWMesColumn.HeaderText = "Message";
            this.MLWMesColumn.Name = "MLWMesColumn";
            // 
            // MessageListWindowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 227);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MessageListWindowControl";
            this.TabText = "MessageListWindowControl";
            this.Text = "MessageListWindowControl";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MLWMessageDridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView MLWMessageDridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn MLWDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MLWTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MLWLocColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MLWMesColumn;
    }
}