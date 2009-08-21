namespace Ecell.IDE.Plugins.Spreadsheet
{
    partial class Spreadsheet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Spreadsheet));
            this.m_gridView = new System.Windows.Forms.DataGridView();
            this.spreadSheetContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridView)).BeginInit();
            this.spreadSheetContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_gridView
            // 
            this.m_gridView.AllowUserToAddRows = false;
            this.m_gridView.AllowUserToDeleteRows = false;
            this.m_gridView.AllowUserToResizeRows = false;
            this.m_gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_gridView.ContextMenuStrip = this.spreadSheetContextMenuStrip;
            resources.ApplyResources(this.m_gridView, "m_gridView");
            this.m_gridView.Name = "m_gridView";
            this.m_gridView.RowTemplate.Height = 21;
            this.m_gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_gridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridViewMouseDown);
            this.m_gridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridViewMouseMove);
            this.m_gridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridViewMouseUp);
            this.m_gridView.MouseLeave += new System.EventHandler(this.GridViewMouseLeave);
            this.m_gridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewClickObjectCell);
            this.m_gridView.CurrentCellChanged += new System.EventHandler(this.GridViewCurrentCellChanged);
            this.m_gridView.SelectionChanged += new System.EventHandler(this.GridViewSelectionChanged);
            // 
            // spreadSheetContextMenuStrip
            // 
            this.spreadSheetContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.spreadSheetContextMenuStrip.Name = "spreadSheetContextMenuStrip";
            resources.ApplyResources(this.spreadSheetContextMenuStrip, "spreadSheetContextMenuStrip");
            this.spreadSheetContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStripOpening);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.ClickCopyMenu);
            // 
            // Spreadsheet
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_gridView);
            this.Name = "Spreadsheet";
            ((System.ComponentModel.ISupportInitialize)(this.m_gridView)).EndInit();
            this.spreadSheetContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView m_gridView;
        private System.Windows.Forms.ContextMenuStrip spreadSheetContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}