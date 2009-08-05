//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Motokazu Ishikawa<m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

namespace Ecell.IDE.Plugins.EntityList
{
    /// <summary>
    /// EntityListControl
    /// </summary>
    partial class EntityListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityListControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.clearButton = new System.Windows.Forms.Button();
            this.objectListDataGrid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.titleContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.typeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Type = new System.Windows.Forms.DataGridViewImageColumn();
            this.ClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ObjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.objectListDataGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.titleContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // clearButton
            // 
            resources.ApplyResources(this.clearButton, "clearButton");
            this.clearButton.Name = "clearButton";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // objectListDataGrid
            // 
            this.objectListDataGrid.AllowUserToAddRows = false;
            this.objectListDataGrid.AllowUserToDeleteRows = false;
            this.objectListDataGrid.AllowUserToOrderColumns = true;
            this.objectListDataGrid.AllowUserToResizeRows = false;
            resources.ApplyResources(this.objectListDataGrid, "objectListDataGrid");
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.objectListDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.objectListDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.objectListDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.ClassName,
            this.ID,
            this.ObjectName});
            this.objectListDataGrid.Name = "objectListDataGrid";
            this.objectListDataGrid.ReadOnly = true;
            this.objectListDataGrid.RowHeadersVisible = false;
            this.objectListDataGrid.RowTemplate.Height = 21;
            this.objectListDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.objectListDataGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridViewMouseDown);
            this.objectListDataGrid.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.TypeSortCompare);
            this.objectListDataGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridViewMouseMove);
            this.objectListDataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DataGridViewMouseUp);
            this.objectListDataGrid.MouseLeave += new System.EventHandler(this.DataGridViewMouseLeave);
            this.objectListDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ClickObjectCell);
            this.objectListDataGrid.SelectionChanged += new System.EventHandler(this.EntSelectionChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.clearButton);
            this.panel1.Controls.Add(this.searchTextBox);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // searchTextBox
            // 
            resources.ApplyResources(this.searchTextBox, "searchTextBox");
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            this.searchTextBox.Leave += new System.EventHandler(this.searchTextBox_Leave);
            this.searchTextBox.Enter += new System.EventHandler(this.searchTextBox_Enter);
            // 
            // titleContextMenuStrip
            // 
            this.titleContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.typeToolStripMenuItem,
            this.classToolStripMenuItem,
            this.pathIDToolStripMenuItem,
            this.nameToolStripMenuItem});
            this.titleContextMenuStrip.Name = "titleContextMenuStrip";
            resources.ApplyResources(this.titleContextMenuStrip, "titleContextMenuStrip");
            // 
            // typeToolStripMenuItem
            // 
            this.typeToolStripMenuItem.Name = "typeToolStripMenuItem";
            resources.ApplyResources(this.typeToolStripMenuItem, "typeToolStripMenuItem");
            this.typeToolStripMenuItem.Tag = "Type";
            this.typeToolStripMenuItem.Click += new System.EventHandler(this.ClickShowColumnMenu);
            // 
            // classToolStripMenuItem
            // 
            this.classToolStripMenuItem.Name = "classToolStripMenuItem";
            resources.ApplyResources(this.classToolStripMenuItem, "classToolStripMenuItem");
            this.classToolStripMenuItem.Tag = "ClassName";
            this.classToolStripMenuItem.Click += new System.EventHandler(this.ClickShowColumnMenu);
            // 
            // pathIDToolStripMenuItem
            // 
            this.pathIDToolStripMenuItem.Name = "pathIDToolStripMenuItem";
            resources.ApplyResources(this.pathIDToolStripMenuItem, "pathIDToolStripMenuItem");
            this.pathIDToolStripMenuItem.Tag = "ID";
            this.pathIDToolStripMenuItem.Click += new System.EventHandler(this.ClickShowColumnMenu);
            // 
            // nameToolStripMenuItem
            // 
            this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            resources.ApplyResources(this.nameToolStripMenuItem, "nameToolStripMenuItem");
            this.nameToolStripMenuItem.Tag = "ObjectName";
            this.nameToolStripMenuItem.Click += new System.EventHandler(this.ClickShowColumnMenu);
            // 
            // Type
            // 
            this.Type.FillWeight = 10F;
            this.Type.Frozen = true;
            resources.ApplyResources(this.Type, "Type");
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ClassName
            // 
            this.ClassName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ClassName.FillWeight = 30F;
            resources.ApplyResources(this.ClassName, "ClassName");
            this.ClassName.Name = "ClassName";
            this.ClassName.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ID.FillWeight = 30F;
            resources.ApplyResources(this.ID, "ID");
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // ObjectName
            // 
            this.ObjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ObjectName.FillWeight = 30F;
            resources.ApplyResources(this.ObjectName, "ObjectName");
            this.ObjectName.Name = "ObjectName";
            this.ObjectName.ReadOnly = true;
            // 
            // EntityListControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.objectListDataGrid);
            this.Name = "EntityListControl";
            ((System.ComponentModel.ISupportInitialize)(this.objectListDataGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.titleContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.DataGridView objectListDataGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.ContextMenuStrip titleContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem typeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
        private System.Windows.Forms.DataGridViewImageColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObjectName;
    }
}
