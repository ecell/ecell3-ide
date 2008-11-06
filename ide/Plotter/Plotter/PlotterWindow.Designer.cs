namespace Ecell.IDE.Plugins.Plotter
{
    partial class PlotterWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlotterWindow));
            this.plotTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.displaySettingDataGrid = new System.Windows.Forms.DataGridView();
            this.ColorColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.XColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_colorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.displaySettingDataGrid)).BeginInit();
            this.gridContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // plotTableLayoutPanel
            // 
            resources.ApplyResources(this.plotTableLayoutPanel, "plotTableLayoutPanel");
            this.plotTableLayoutPanel.Name = "plotTableLayoutPanel";
            // 
            // displaySettingDataGrid
            // 
            this.displaySettingDataGrid.AllowUserToAddRows = false;
            this.displaySettingDataGrid.AllowUserToResizeRows = false;
            resources.ApplyResources(this.displaySettingDataGrid, "displaySettingDataGrid");
            this.displaySettingDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.displaySettingDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.displaySettingDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColorColumn,
            this.XColumn,
            this.YColumn});
            this.displaySettingDataGrid.Name = "displaySettingDataGrid";
            this.displaySettingDataGrid.RowHeadersVisible = false;
            this.displaySettingDataGrid.RowTemplate.Height = 21;
            this.displaySettingDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellDoubleClicked);
            this.displaySettingDataGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.CellMouseClicked);
            // 
            // ColorColumn
            // 
            this.ColorColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColorColumn.FillWeight = 45.68528F;
            this.ColorColumn.Frozen = true;
            resources.ApplyResources(this.ColorColumn, "ColorColumn");
            this.ColorColumn.Name = "ColorColumn";
            // 
            // XColumn
            // 
            this.XColumn.FillWeight = 127.1574F;
            resources.ApplyResources(this.XColumn, "XColumn");
            this.XColumn.Name = "XColumn";
            // 
            // YColumn
            // 
            this.YColumn.FillWeight = 127.1574F;
            resources.ApplyResources(this.YColumn, "YColumn");
            this.YColumn.Name = "YColumn";
            // 
            // gridContextMenuStrip
            // 
            this.gridContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.gridContextMenuStrip.Name = "gridContextMenuStrip";
            resources.ApplyResources(this.gridContextMenuStrip, "gridContextMenuStrip");
            this.gridContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OpeningContextMenu);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            resources.ApplyResources(this.addToolStripMenuItem, "addToolStripMenuItem");
            this.addToolStripMenuItem.Click += new System.EventHandler(this.ClickAddEntry);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.ClickDeleteEntry);
            // 
            // PlotterWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.displaySettingDataGrid);
            this.Controls.Add(this.plotTableLayoutPanel);
            this.Name = "PlotterWindow";
            ((System.ComponentModel.ISupportInitialize)(this.displaySettingDataGrid)).EndInit();
            this.gridContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel plotTableLayoutPanel;
        private System.Windows.Forms.DataGridView displaySettingDataGrid;
        private System.Windows.Forms.DataGridViewImageColumn ColorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn XColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn YColumn;
        private System.Windows.Forms.ContextMenuStrip gridContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ColorDialog m_colorDialog;
    }
}