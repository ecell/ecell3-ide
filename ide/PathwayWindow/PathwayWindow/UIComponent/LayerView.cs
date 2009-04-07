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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using System.Diagnostics;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.Objects;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// UserControl class to display the list of layer.
    /// </summary>
    public class LayerView: EcellDockContent
    {
        #region Fields
        /// <summary>
        /// Width of "Show" column of layer DataGridView.
        /// </summary>
        private readonly int LAYER_SHOWCOLUMN_WIDTH = 50;

        private const string LAYER_HEDDER = "Layer";
        /// <summary>
        /// The PathwayControl, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        protected PathwayControl m_con = null;
        /// <summary>
        /// The current CanvasControl.
        /// </summary>
        protected CanvasControl m_canvas = null;
        /// <summary>
        /// A name of selected layer.
        /// </summary>
        private string m_selectedLayer = null;
        /// <summary>
        /// Every time when m_dgv.CurrentCellDirtyStateChanged event occurs, 
        /// m_dgv_CurrentCellDirtyStateChanged delegate will be called twice.
        /// This flag is used for neglecting one of two delagate calling.
        /// </summary>
        private bool m_dirtyEventProcessed = false;

        /// <summary>
        /// DetaGridView
        /// </summary>
        private DataGridView dataGridView;
        private Panel panel;
        private DataGridViewCheckBoxColumn checkBoxColumn;
        private DataGridViewTextBoxColumn layerNameColumn;
        private IContainer components;

        #region Menu Items
        private ContextMenuStrip popupMenu;
        private ToolStripMenuItem menuCreateLayer;
        private ToolStripMenuItem menuRenameLayer;
        private ToolStripMenuItem menuMergeLayer;
        private ToolStripMenuItem menuRemoveLayer;
        private ToolStripSeparator separator1;
        private ToolStripMenuItem menuSelectNodes;
        private ToolStripSeparator separator2;
        private ToolStripMenuItem menuMoveFront;
        private ToolStripMenuItem menuMoveBack;
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LayerView(PathwayControl control)
        {
            base.m_isSavable = true;
            this.m_con = control;
            control.CanvasChange += new EventHandler(OnCanvasChange);
            control.ProjectStatusChange += new EventHandler(OnProjectStatusChange);
            InitializeComponent();

            // Preparing context menus.
        }

        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerView));
            this.panel = new System.Windows.Forms.Panel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.checkBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.layerNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.popupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCreateLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRenameLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMergeLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRemoveLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSelectNodes = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMoveFront = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMoveBack = new System.Windows.Forms.ToolStripMenuItem();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.popupMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.dataGridView);
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // m_dgv
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkBoxColumn,
            this.layerNameColumn});
            this.dataGridView.ContextMenuStrip = this.popupMenu;
            resources.ApplyResources(this.dataGridView, "m_dgv");
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "m_dgv";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 21;
            this.dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_dgv_MouseDown);
            this.dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_dgv_CellEndEdit);
            this.dataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_dgv_CellMouseDown);
            this.dataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.m_dgv_CurrentCellDirtyStateChanged);
            this.dataGridView.VisibleChanged += new System.EventHandler(this.m_dgv_VisibleChanged);
            // 
            // CheckBoxColumn
            // 
            this.checkBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.checkBoxColumn.HeaderText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerColumnShow;
            this.checkBoxColumn.Name = "CheckBoxColumn";
            resources.ApplyResources(this.checkBoxColumn, "CheckBoxColumn");
            // 
            // LayerNameColumn
            // 
            this.layerNameColumn.HeaderText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerColumnName;
            this.layerNameColumn.Name = "LayerNameColumn";
            // 
            // popupMenu
            // 
            this.popupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCreateLayer,
            this.menuRenameLayer,
            this.menuMergeLayer,
            this.menuRemoveLayer,
            this.separator1,
            this.menuSelectNodes,
            this.separator2,
            this.menuMoveFront,
            this.menuMoveBack});
            this.popupMenu.Name = "popupMenu";
            resources.ApplyResources(this.popupMenu, "popupMenu");
            // 
            // menuCreateLayer
            // 
            this.menuCreateLayer.Name = "menuCreateLayer";
            resources.ApplyResources(this.menuCreateLayer, "menuCreateLayer");
            this.menuCreateLayer.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuCreate;
            this.menuCreateLayer.Click += new System.EventHandler(this.CreateLayerClick);
            // 
            // menuRenameLayer
            // 
            this.menuRenameLayer.Name = "menuRenameLayer";
            resources.ApplyResources(this.menuRenameLayer, "menuRenameLayer");
            this.menuRenameLayer.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuRename;
            this.menuRenameLayer.Click += new System.EventHandler(this.RenameLayerClick);
            // 
            // menuMergeLayer
            // 
            this.menuMergeLayer.Name = "menuMergeLayer";
            resources.ApplyResources(this.menuMergeLayer, "menuMergeLayer");
            this.menuMergeLayer.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuMerge;
            // 
            // menuRemoveLayer
            // 
            this.menuRemoveLayer.Name = "menuRemoveLayer";
            resources.ApplyResources(this.menuRemoveLayer, "menuRemoveLayer");
            this.menuRemoveLayer.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuDelete;
            this.menuRemoveLayer.Click += new System.EventHandler(this.RemoveLayerClick);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            resources.ApplyResources(this.separator1, "separator1");
            // 
            // menuSelectNodes
            // 
            this.menuSelectNodes.Name = "menuSelectNodes";
            resources.ApplyResources(this.menuSelectNodes, "menuSelectNodes");
            this.menuSelectNodes.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuSelectNodes;
            this.menuSelectNodes.Click += new System.EventHandler(this.SelectNodesClick);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            resources.ApplyResources(this.separator2, "separator2");
            // 
            // menuMoveFront
            // 
            this.menuMoveFront.Name = "menuMoveFront";
            resources.ApplyResources(this.menuMoveFront, "menuMoveFront");
            this.menuMoveFront.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuMoveFront;
            this.menuMoveFront.Click += new System.EventHandler(this.MoveFrontClick);
            // 
            // menuMoveBack
            // 
            this.menuMoveBack.Name = "menuMoveBack";
            resources.ApplyResources(this.menuMoveBack, "menuMoveBack");
            this.menuMoveBack.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuMoveBack;
            this.menuMoveBack.Click += new System.EventHandler(this.MoveBackClick);
            // 
            // LayerView
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel);
            this.Icon = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.Icon_LayerView;
            this.Name = "LayerView";
            this.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.WindowLayer;
            this.TabText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.WindowLayer;
            this.panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.popupMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void OnProjectStatusChange(object sender, EventArgs e)
        {
            if (m_con.ProjectStatus == ProjectStatus.Uninitialized)
            {
                dataGridView.Rows.Clear();
                m_canvas = null;
            }
        }

        void OnCanvasChange(object sender, EventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            m_canvas = m_con.Canvas;

        }

        #endregion

        #region Methods
        /// <summary>
        /// ResetLayers
        /// </summary>
        /// <param name="list"></param>
        internal void ResetLayers(List<EcellLayer> list)
        {
            dataGridView.Rows.Clear();

            List<PPathwayLayer> layers = new List<PPathwayLayer>();
            foreach (EcellLayer el in list)
            {
                if (!m_canvas.Layers.ContainsKey(el.Name))
                    m_canvas.AddLayer(el.Name, el.Visible);
                PPathwayLayer layer = m_canvas.Layers[el.Name];
                layer.Visible = el.Visible;
                LayerGridRow row = new LayerGridRow(layer);
                dataGridView.Rows.Add(row);
                layers.Add(layer);
            }

            List<PPathwayLayer> tempList = new List<PPathwayLayer>();
            tempList.AddRange(m_canvas.Layers.Values);
            foreach (PPathwayLayer layer in tempList)
            {
                if (!layers.Contains(layer))
                    m_canvas.RemoveLayer(layer.Name);
            }
        }

        #endregion

        #region Event sequences

        void m_dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex <= 0)
                return;
            LayerGridRow row = (LayerGridRow)dataGridView.Rows[e.RowIndex];
            string oldName = row.Layer.Name;
            string newName = row.Name;
            if (oldName.Equals(newName))
                return;
            // Check Errors.
            if (string.IsNullOrEmpty(newName))
            {
                Util.ShowNoticeDialog(string.Format(MessageResources.ErrInvalidValue, newName));
                row.Name = row.Layer.Name;
                return;
            }
            if (m_canvas.Layers.ContainsKey(newName))
            {
                Util.ShowNoticeDialog(string.Format(MessageResources.ErrAlrExist, newName));
                row.Name = row.Layer.Name;
                return;
            }
            m_canvas.NotifyRenameLayer(oldName, newName);
        }

        /// <summary>
        /// event sequence of changing the check of layer showing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell is DataGridViewTextBoxCell)
                return;
            if (!m_dirtyEventProcessed)
            {
                LayerGridRow row = (LayerGridRow)((DataGridView)sender).CurrentRow;
                // row.Layer.Visible = row.Checked;
                m_dirtyEventProcessed = true;
                m_canvas.ChangeLayerVisibility(row.Layer.Name, row.Checked);
            }
            else
            {
                m_dirtyEventProcessed = false;
            }
            ((DataGridView)sender).CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        /// <summary>
        /// When Layer visiblity is changed,
        /// ...
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewBindingComplete.</param>
        private void m_dgv_VisibleChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).Columns.Contains(MessageResources.LayerColumnShow) && ((DataGridView)sender).Visible)
            {
                ((DataGridView)sender).Columns[MessageResources.LayerColumnShow].Width = LAYER_SHOWCOLUMN_WIDTH;
                ((DataGridView)sender).Columns[MessageResources.LayerColumnShow].Resizable = DataGridViewTriState.False;
                ((DataGridView)sender).Columns[MessageResources.LayerColumnShow].Frozen = true;
                ((DataGridView)sender).Columns[MessageResources.LayerColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        /// <summary>
        /// when click menu "Create Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectNodesClick(object sender, EventArgs e)
        {
            PPathwayLayer layer = m_canvas.Layers[m_selectedLayer];
            if (layer == null)
                return;
            foreach (PPathwayObject obj in layer.GetNodes())
            {
                m_canvas.NotifyAddSelect(obj);
            }
        }

        /// <summary>
        /// when click menu "Move Front"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveFrontClick(object sender, EventArgs e)
        {
            m_canvas.LayerMoveToFront(m_selectedLayer);
        }

        /// <summary>
        /// when click menu "Move Back"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveBackClick(object sender, EventArgs e)
        {
            m_canvas.LayerMoveToBack(m_selectedLayer);
        }

        /// <summary>
        /// when click menu "Create Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateLayerClick(object sender, EventArgs e)
        {
            string name = GetNewLayerID(m_canvas);
            m_canvas.NotifyAddLayer(name, true);
        }

        /// <summary>
        /// Get new LayerID.
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        private static string GetNewLayerID(CanvasControl canvas)
        {
            int i = 0;
            string name = null;
            do
            {
                name = LAYER_HEDDER + i.ToString();
                foreach (string layerName in canvas.Layers.Keys)
                {
                    if (layerName.Equals(name))
                    {
                        name = null;
                        break;
                    }
                }
                i++;
            }
            while (name == null);
            return name;
        }

        /// <summary>
        /// when click menu "Delete Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLayerClick(object sender, EventArgs e)
        {
            m_canvas.NotifyRemoveLayer(m_selectedLayer, true);
        }

        /// <summary>
        /// when click menu "Rename Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameLayerClick(object sender, EventArgs e)
        {
            dataGridView.BeginEdit(false);
        }

        /// <summary>
        /// when click menu "Merge Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MergeLayerClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            m_canvas.NotifyMergeLayer(m_selectedLayer, item.Text);
        }

        /// <summary>
        /// when click DataGridRows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0)
                return;
            LayerGridRow row = (LayerGridRow)dataGridView.Rows[index];
            m_selectedLayer = row.Name;
            SetMergeLayerMenus(row.Name);
            dataGridView.ClearSelection();
            row.Selected = true;

            menuCreateLayer.Visible = true;
            menuRenameLayer.Visible = true;
            menuMergeLayer.Visible = true;
            menuRemoveLayer.Visible = true;
            separator1.Visible = true;
            menuSelectNodes.Visible = true;
            separator2.Visible = true;
            menuMoveFront.Visible = true;
            menuMoveBack.Visible = true;
        }

        private void SetMergeLayerMenus(string layer)
        {
            menuMergeLayer.DropDownItems.Clear();
            foreach (LayerGridRow row in dataGridView.Rows)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(row.Name);
                item.Click += new EventHandler(MergeLayerClick);

                if (row.Name == layer)
                {
                    item.Checked = true;
                    item.Enabled = false;
                }
            }
        }

        /// <summary>
        /// when click DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dgv_MouseDown(object sender, MouseEventArgs e)
        {
            menuCreateLayer.Visible = (m_canvas != null);
            menuRenameLayer.Visible = false;
            menuMergeLayer.Visible = false;
            menuRemoveLayer.Visible = false;
            separator1.Visible = false;
            menuSelectNodes.Visible = false;
            separator2.Visible = false;
            menuMoveFront.Visible = false;
            menuMoveBack.Visible = false;
        }
        #endregion
    }

    internal class LayerGridRow : DataGridViewRow
    {
        /// <summary>
        /// 
        /// </summary>
        private PPathwayLayer m_layer;
        /// <summary>
        /// 
        /// </summary>
        private DataGridViewCheckBoxCell checkBox;
        /// <summary>
        /// 
        /// </summary>
        private DataGridViewTextBoxCell textBox;
        /// <summary>
        /// 
        /// </summary>
        public PPathwayLayer Layer
        {
            get { return m_layer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Checked
        {
            get { return !(bool)checkBox.Value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return (string)textBox.Value; }
            set { textBox.Value = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        public LayerGridRow(PPathwayLayer layer)
        {
            m_layer = layer;
            checkBox = new DataGridViewCheckBoxCell();
            checkBox.Value = layer.Visible;
            this.Cells.Add(checkBox);

            textBox = new DataGridViewTextBoxCell();
            textBox.Value = layer.Name;
            this.Cells.Add(textBox);
        }
    }
}
