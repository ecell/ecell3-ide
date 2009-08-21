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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.MLWMessageDridView = new System.Windows.Forms.DataGridView();
            this.MLWTypeColumn = new System.Windows.Forms.DataGridViewImageColumn();
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
            this.MLWMessageDridView.AllowUserToResizeRows = false;
            this.MLWMessageDridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MLWMessageDridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MLWMessageDridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.MLWMessageDridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.MLWMessageDridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MLWTypeColumn,
            this.MLWLocColumn,
            this.MLWMesColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MLWMessageDridView.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.MLWMessageDridView, "MLWMessageDridView");
            this.MLWMessageDridView.Name = "MLWMessageDridView";
            this.MLWMessageDridView.RowHeadersVisible = false;
            this.MLWMessageDridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.MLWMessageDridView.RowTemplate.Height = 21;
            this.MLWMessageDridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MessageCellDoubleClick);
            this.MLWMessageDridView.SelectionChanged += new System.EventHandler(this.MLWMessageDridView_SelectionChanged);
            // 
            // MLWTypeColumn
            // 
            this.MLWTypeColumn.FillWeight = 9.847716F;
            resources.ApplyResources(this.MLWTypeColumn, "MLWTypeColumn");
            this.MLWTypeColumn.Name = "MLWTypeColumn";
            this.MLWTypeColumn.ReadOnly = true;
            this.MLWTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MLWTypeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MLWLocColumn
            // 
            this.MLWLocColumn.FillWeight = 35.88624F;
            resources.ApplyResources(this.MLWLocColumn, "MLWLocColumn");
            this.MLWLocColumn.Name = "MLWLocColumn";
            this.MLWLocColumn.ReadOnly = true;
            // 
            // MLWMesColumn
            // 
            this.MLWMesColumn.FillWeight = 51.26605F;
            resources.ApplyResources(this.MLWMesColumn, "MLWMesColumn");
            this.MLWMesColumn.Name = "MLWMesColumn";
            this.MLWMesColumn.ReadOnly = true;
            // 
            // MessageListWindowControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MessageListWindowControl";
            this.TabText = "MessageListWindowControl";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MLWMessageDridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView MLWMessageDridView;
        private System.Windows.Forms.DataGridViewImageColumn MLWTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MLWLocColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MLWMesColumn;
    }
}