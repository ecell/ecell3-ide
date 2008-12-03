namespace Ecell.IDE.Plugins.PropertyWindow
{
    partial class PropertyWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyWindow));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.m_dgv = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.defineANewPropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteThisPropertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.observedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parameterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgv)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.UseMnemonic = false;
            // 
            // m_dgv
            // 
            this.m_dgv.AllowUserToAddRows = false;
            this.m_dgv.AllowUserToDeleteRows = false;
            this.m_dgv.AllowUserToResizeRows = false;
            this.m_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.valueColumn});
            this.m_dgv.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.m_dgv, "m_dgv");
            this.m_dgv.MultiSelect = false;
            this.m_dgv.Name = "m_dgv";
            this.m_dgv.RowHeadersVisible = false;
            this.m_dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.m_dgv.RowTemplate.Height = 21;
            this.m_dgv.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.m_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgv.ShowEditingIcon = false;
            this.m_dgv.ShowRowErrors = false;
            this.m_dgv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownOnDataGrid);
            this.m_dgv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveOnDataGridView);
            this.m_dgv.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.ChangeProperty);
            this.m_dgv.MouseLeave += new System.EventHandler(this.LeaveMouse);
            this.m_dgv.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.m_dgv_EditingControlShowing);
            this.m_dgv.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_dgv_CellEndEdit);
            this.m_dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defineANewPropertyToolStripMenuItem,
            this.deleteThisPropertyToolStripMenuItem,
            this.toolStripSeparator1,
            this.loggingToolStripMenuItem,
            this.observedToolStripMenuItem,
            this.parameterToolStripMenuItem,
            this.toolStripSeparator2,
            this.propertyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // defineANewPropertyToolStripMenuItem
            // 
            this.defineANewPropertyToolStripMenuItem.Name = "defineANewPropertyToolStripMenuItem";
            resources.ApplyResources(this.defineANewPropertyToolStripMenuItem, "defineANewPropertyToolStripMenuItem");
            this.defineANewPropertyToolStripMenuItem.Click += new System.EventHandler(this.defineANewPropertyToolStripMenuItem_Click);
            // 
            // deleteThisPropertyToolStripMenuItem
            // 
            this.deleteThisPropertyToolStripMenuItem.Name = "deleteThisPropertyToolStripMenuItem";
            resources.ApplyResources(this.deleteThisPropertyToolStripMenuItem, "deleteThisPropertyToolStripMenuItem");
            this.deleteThisPropertyToolStripMenuItem.Click += new System.EventHandler(this.deleteThisPropertyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            resources.ApplyResources(this.loggingToolStripMenuItem, "loggingToolStripMenuItem");
            this.loggingToolStripMenuItem.Click += new System.EventHandler(this.ClickLoggingMenu);
            // 
            // observedToolStripMenuItem
            // 
            this.observedToolStripMenuItem.Name = "observedToolStripMenuItem";
            resources.ApplyResources(this.observedToolStripMenuItem, "observedToolStripMenuItem");
            this.observedToolStripMenuItem.Click += new System.EventHandler(this.ClickObservedDataMenu);
            // 
            // parameterToolStripMenuItem
            // 
            this.parameterToolStripMenuItem.Name = "parameterToolStripMenuItem";
            resources.ApplyResources(this.parameterToolStripMenuItem, "parameterToolStripMenuItem");
            this.parameterToolStripMenuItem.Click += new System.EventHandler(this.ClickUnknownParameterMenu);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            resources.ApplyResources(this.propertyToolStripMenuItem, "propertyToolStripMenuItem");
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.ClickShowPropertyMenu);
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightYellow;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightYellow;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.FillWeight = 50F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // nameColumn
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightYellow;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.nameColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.nameColumn.FillWeight = 50F;
            resources.ApplyResources(this.nameColumn, "nameColumn");
            this.nameColumn.Name = "nameColumn";
            // 
            // valueColumn
            // 
            this.valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.valueColumn, "valueColumn");
            this.valueColumn.Name = "valueColumn";
            // 
            // PropertyWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_dgv);
            this.Controls.Add(this.label1);
            this.IsSavable = true;
            this.Name = "PropertyWindow";
            ((System.ComponentModel.ISupportInitialize)(this.m_dgv)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView m_dgv;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteThisPropertyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defineANewPropertyToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem observedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parameterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;
    }
}
