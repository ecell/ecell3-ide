namespace Ecell.IDE.Plugins.MessageListWindow
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
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.MLWMessageDridView, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
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
            resources.ApplyResources(this.MLWMessageDridView, "MLWMessageDridView");
            this.MLWMessageDridView.Name = "MLWMessageDridView";
            this.MLWMessageDridView.RowHeadersVisible = false;
            this.MLWMessageDridView.RowTemplate.Height = 21;
            // 
            // MLWDateColumn
            // 
            this.MLWDateColumn.FillWeight = 30F;
            resources.ApplyResources(this.MLWDateColumn, "MLWDateColumn");
            this.MLWDateColumn.Name = "MLWDateColumn";
            // 
            // MLWTypeColumn
            // 
            this.MLWTypeColumn.FillWeight = 15F;
            resources.ApplyResources(this.MLWTypeColumn, "MLWTypeColumn");
            this.MLWTypeColumn.Name = "MLWTypeColumn";
            // 
            // MLWLocColumn
            // 
            this.MLWLocColumn.FillWeight = 35F;
            resources.ApplyResources(this.MLWLocColumn, "MLWLocColumn");
            this.MLWLocColumn.Name = "MLWLocColumn";
            // 
            // MLWMesColumn
            // 
            this.MLWMesColumn.FillWeight = 50F;
            resources.ApplyResources(this.MLWMesColumn, "MLWMesColumn");
            this.MLWMesColumn.Name = "MLWMesColumn";
            // 
            // MessageListWindowControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MessageListWindowControl";
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