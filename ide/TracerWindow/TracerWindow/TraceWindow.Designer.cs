namespace Ecell.IDE.Plugins.TracerWindow
{
    partial class TraceWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TraceWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.view = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.color = new System.Windows.Forms.DataGridViewImageColumn();
            this.LineStyle = new System.Windows.Forms.DataGridViewImageColumn();
            this.full = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.m_colorDialog = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.dgv, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // dgv
            // 
            this.dgv.AllowDrop = true;
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.view,
            this.color,
            this.LineStyle,
            this.full});
            resources.ApplyResources(this.dgv, "dgv");
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 21;
            // 
            // view
            // 
            this.view.FillWeight = 41.50502F;
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            // 
            // color
            // 
            this.color.FillWeight = 40.81528F;
            resources.ApplyResources(this.color, "color");
            this.color.Name = "color";
            // 
            // LineStyle
            // 
            this.LineStyle.FillWeight = 40F;
            resources.ApplyResources(this.LineStyle, "LineStyle");
            this.LineStyle.Name = "LineStyle";
            // 
            // full
            // 
            this.full.FillWeight = 185.7F;
            resources.ApplyResources(this.full, "full");
            this.full.Name = "full";
            // 
            // TraceWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TraceWindow";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        /// <summary>
        /// DataGrid to show the list of trace.
        /// </summary>
        public System.Windows.Forms.DataGridView dgv;
        /// <summary>
        /// FolderBrowseDialog to set the output directory.
        /// </summary>
        public System.Windows.Forms.FolderBrowserDialog m_folderDialog;
        /// <summary>
        /// ColorDialog to set line color.
        /// </summary>
        public System.Windows.Forms.ColorDialog m_colorDialog;
        private System.Windows.Forms.DataGridViewCheckBoxColumn view;
        private System.Windows.Forms.DataGridViewImageColumn color;
        private System.Windows.Forms.DataGridViewImageColumn LineStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn full;
    }
}