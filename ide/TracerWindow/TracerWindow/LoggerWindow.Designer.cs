﻿namespace Ecell.IDE.Plugins.TracerWindow
{
    partial class LoggerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoggerWindow));
            this.loggerDataGrid = new System.Windows.Forms.DataGridView();
            this.IsShownColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FullPNColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColorColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.LineColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.m_colorDialog = new System.Windows.Forms.ColorDialog();
            this.gridContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.loggerDataGrid)).BeginInit();
            this.gridContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // loggerDataGrid
            // 
            this.loggerDataGrid.AllowDrop = true;
            this.loggerDataGrid.AllowUserToAddRows = false;
            this.loggerDataGrid.AllowUserToDeleteRows = false;
            this.loggerDataGrid.AllowUserToResizeRows = false;
            resources.ApplyResources(this.loggerDataGrid, "loggerDataGrid");
            this.loggerDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.loggerDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.loggerDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsShownColumn,
            this.FullPNColumn,
            this.ColorColumn,
            this.LineColumn});
            this.loggerDataGrid.Name = "loggerDataGrid";
            this.loggerDataGrid.RowHeadersVisible = false;
            this.loggerDataGrid.RowTemplate.Height = 21;
            this.loggerDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.loggerDataGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.loggerDataGrid_CellValueChanged);
            this.loggerDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.loggerDataGrid_CellDoubleClick);
            this.loggerDataGrid.DragEnter += new System.Windows.Forms.DragEventHandler(this.loggerDataGrid_DragEnter);
            this.loggerDataGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.loggerDataGrid_CurrentCellDirtyStateChanged);
            this.loggerDataGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.loggerDataGrid_DragDrop);
            // 
            // IsShownColumn
            // 
            this.IsShownColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IsShownColumn.FillWeight = 101.5228F;
            this.IsShownColumn.Frozen = true;
            resources.ApplyResources(this.IsShownColumn, "IsShownColumn");
            this.IsShownColumn.Name = "IsShownColumn";
            this.IsShownColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // FullPNColumn
            // 
            this.FullPNColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FullPNColumn.FillWeight = 99.61929F;
            resources.ApplyResources(this.FullPNColumn, "FullPNColumn");
            this.FullPNColumn.Name = "FullPNColumn";
            // 
            // ColorColumn
            // 
            this.ColorColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColorColumn.FillWeight = 99.61929F;
            resources.ApplyResources(this.ColorColumn, "ColorColumn");
            this.ColorColumn.Name = "ColorColumn";
            this.ColorColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // LineColumn
            // 
            this.LineColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LineColumn.FillWeight = 99.61929F;
            resources.ApplyResources(this.LineColumn, "LineColumn");
            this.LineColumn.Name = "LineColumn";
            this.LineColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // gridContextMenuStrip
            // 
            this.gridContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowToolStripMenuItem,
            this.toolStripSeparator1,
            this.colorToolStripMenuItem,
            this.lineStyleToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator2,
            this.importLogToolStripMenuItem});
            this.gridContextMenuStrip.Name = "gridContextMenuStrip";
            resources.ApplyResources(this.gridContextMenuStrip, "gridContextMenuStrip");
            this.gridContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.gridContextMenuStrip_Opening);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            resources.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // importLogToolStripMenuItem
            // 
            this.importLogToolStripMenuItem.Name = "importLogToolStripMenuItem";
            resources.ApplyResources(this.importLogToolStripMenuItem, "importLogToolStripMenuItem");
            this.importLogToolStripMenuItem.Click += new System.EventHandler(this.importLogToolStripMenuItem_Click);
            // 
            // m_openFileDialog
            // 
            this.m_openFileDialog.FileName = "m_openFileDialog";
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            resources.ApplyResources(this.colorToolStripMenuItem, "colorToolStripMenuItem");
            this.colorToolStripMenuItem.Click += new System.EventHandler(this.colorToolStripMenuItem_Click);
            // 
            // lineStyleToolStripMenuItem
            // 
            this.lineStyleToolStripMenuItem.Name = "lineStyleToolStripMenuItem";
            resources.ApplyResources(this.lineStyleToolStripMenuItem, "lineStyleToolStripMenuItem");
            this.lineStyleToolStripMenuItem.Click += new System.EventHandler(this.lineStyleToolStripMenuItem_Click);
            // 
            // LoggerWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loggerDataGrid);
            this.Name = "LoggerWindow";
            ((System.ComponentModel.ISupportInitialize)(this.loggerDataGrid)).EndInit();
            this.gridContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView loggerDataGrid;
        private System.Windows.Forms.ColorDialog m_colorDialog;
        private System.Windows.Forms.ContextMenuStrip gridContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsShownColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullPNColumn;
        private System.Windows.Forms.DataGridViewImageColumn ColorColumn;
        private System.Windows.Forms.DataGridViewImageColumn LineColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem importLogToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineStyleToolStripMenuItem;
    }
}