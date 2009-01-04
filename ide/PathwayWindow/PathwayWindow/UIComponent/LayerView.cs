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
using Ecell.IDE.Plugins.PathwayWindow.Dialog;
using System.Diagnostics;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// UserControl class to display the list of layer.
    /// </summary>
    public class LayerView: EcellDockContent
    {
        #region Static Fields
        /// <summary>
        /// PopupMenu to create layer.
        /// </summary>
        private const string MenuCreate = "LayerMenuCreate";
        /// <summary>
        /// PopupMenu to delete layer.
        /// </summary>
        private const string MenuDelete = "LayerMenuDelete";
        /// <summary>
        /// PopupMenu to merge layer.
        /// </summary>
        private const string MenuMerge = "LayerMenuMerge";
        /// <summary>
        /// PopupMenu to rename layer.
        /// </summary>
        private const string MenuRename = "LayerMenuRename";
        /// <summary>
        /// PopupMenu to select nodes under the layer.
        /// </summary>
        private const string MenuSelectNode = "LayerMenuSelectNodes";
        /// <summary>
        /// PopupMenu to move layer.
        /// </summary>
        private const string MenuMoveFront = "LayerMenuMoveFront";
        /// <summary>
        /// PopupMenu to move layer.
        /// </summary>
        private const string MenuMoveBack = "LayerMenuMoveBack";
        /// <summary>
        /// PopupMenu Sepalator.
        /// </summary>
        private const string MenuSepalator = "Sepalator";
        /// <summary>
        /// Dialog title.
        /// </summary>
        private const string DialogTitle = "LayerDialogTitle";
        /// <summary>
        /// Dialog message.
        /// </summary>
        private const string DialogMessage = "LayerDialogMessage";
        #endregion

        #region Fields
        /// <summary>
        /// Width of "Show" column of layer DataGridView.
        /// </summary>
        private readonly int LAYER_SHOWCOLUMN_WIDTH = 50;

        private readonly int LAYER_CHECK_COLUMN = 1;
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
        /// Every time when m_dgv.CurrentCellDirtyStateChanged event occurs, 
        /// m_dgv_CurrentCellDirtyStateChanged delegate will be called twice.
        /// This flag is used for neglecting one of two delagate calling.
        /// </summary>
        private bool m_dirtyEventProcessed = false;

        /// <summary>
        /// DetaGridView
        /// </summary>
        private DataGridView m_dgv;
        private DataGridViewCheckBoxColumn CheckBoxColumn;
        private DataGridViewTextBoxColumn LayerNameColumn;
        /// <summary>
        /// PPath to show main pathway area in the overview.
        /// Normally, this node is colored in red.
        /// </summary>
        private Panel panel;
        /// <summary>
        /// List of ToolStripMenuItems for ContextMenu
        /// </summary>
        private Dictionary<string, ToolStripItem> m_cMenuDict = new Dictionary<string, ToolStripItem>();
        /// <summary>
        /// A name of selected layer.
        /// </summary>
        private string m_selectedLayer = null;
        #endregion

        #region Accessor
        /// <summary>
        /// A name of selected layer.
        /// </summary>
        public string CurrentLayer
        {
            get { return GetCurrentLayerID(); }
        }

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
            m_dgv.ContextMenuStrip = CreatePopUpMenus();
            this.Text = MessageResources.WindowLayer;
            this.TabText = this.Text;
        }

        void OnProjectStatusChange(object sender, EventArgs e)
        {
            if (m_con.ProjectStatus == ProjectStatus.Uninitialized)
                m_dgv.Rows.Clear();
        }

        void OnCanvasChange(object sender, EventArgs e)
        {
            if (m_canvas != null)
                m_canvas.LayerChange -= this.OnLayerChange;
            if (m_con.Canvas == null)
                return;
            m_canvas = m_con.Canvas;
            m_canvas.LayerChange += new EventHandler(OnLayerChange);
        }

        void OnLayerChange(object sender, EventArgs e)
        {
            RefreshLayerTable();
        }
        /// <summary>
        /// Refresh 
        /// </summary>
        private void RefreshLayerTable()
        {
            m_dgv.Rows.Clear();
            foreach (PNode obj in m_canvas.PCanvas.Root.ChildrenReference)
            {
                if (!(obj is PPathwayLayer))
                    continue;
                PPathwayLayer layer = (PPathwayLayer)obj;
                if (string.IsNullOrEmpty(layer.Name))
                    continue;
                LayerGridRow row = new LayerGridRow(layer);
                m_dgv.Rows.Add(row);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerView));
            this.panel = new System.Windows.Forms.Panel();
            this.m_dgv = new System.Windows.Forms.DataGridView();
            this.CheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LayerNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.m_dgv);
            resources.ApplyResources(this.panel, "panel");
            this.panel.Name = "panel";
            // 
            // m_dgv
            // 
            this.m_dgv.AllowUserToAddRows = false;
            this.m_dgv.AllowUserToDeleteRows = false;
            this.m_dgv.AllowUserToResizeRows = false;
            this.m_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheckBoxColumn,
            this.LayerNameColumn});
            resources.ApplyResources(this.m_dgv, "m_dgv");
            this.m_dgv.MultiSelect = false;
            this.m_dgv.Name = "m_dgv";
            this.m_dgv.RowHeadersVisible = false;
            this.m_dgv.RowTemplate.Height = 21;
            this.m_dgv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_dgv_MouseDown);
            this.m_dgv.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_dgv_CellValidated);
            this.m_dgv.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_dgv_CellMouseDown);
            this.m_dgv.CurrentCellDirtyStateChanged += new System.EventHandler(this.m_dgv_CurrentCellDirtyStateChanged);
            this.m_dgv.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            this.m_dgv.VisibleChanged += new System.EventHandler(this.m_dgv_VisibleChanged);
            // 
            // CheckBoxColumn
            // 
            this.CheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CheckBoxColumn.HeaderText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerColumnShow;
            this.CheckBoxColumn.Name = "CheckBoxColumn";
            resources.ApplyResources(this.CheckBoxColumn, "CheckBoxColumn");
            // 
            // LayerNameColumn
            // 
            this.LayerNameColumn.HeaderText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerColumnName;
            this.LayerNameColumn.Name = "LayerNameColumn";
            // 
            // LayerView
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel);
            this.Icon = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.Icon_LayerView;
            this.Name = "LayerView";
            this.TabText = this.Name;
            this.Text = this.Name;
            this.panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgv)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Get Popup Menus.
        /// </summary>
        ///<returns>ContextMenu.</returns>
        private ContextMenuStrip CreatePopUpMenus()
        {
            // Preparing a context menu.
            ContextMenuStrip nodeMenu = new ContextMenuStrip();

            ToolStripItem menuSelectNodes = new ToolStripMenuItem(MenuSelectNode);
            menuSelectNodes.Text = MessageResources.LayerMenuSelectNodes;
            menuSelectNodes.Click += new EventHandler(SelectNodesClick);
            nodeMenu.Items.Add(menuSelectNodes);
            m_cMenuDict.Add(MenuSelectNode, menuSelectNodes);

            ToolStripItem menuMoveFront = new ToolStripMenuItem(MenuMoveFront);
            menuMoveFront.Text = MessageResources.LayerMenuMoveFront;
            menuMoveFront.Click += new EventHandler(MoveFrontClick);
            nodeMenu.Items.Add(menuMoveFront);
            m_cMenuDict.Add(MenuMoveFront, menuMoveFront);

            ToolStripItem menuMoveBack = new ToolStripMenuItem(MenuMoveBack);
            menuMoveBack.Text = MessageResources.LayerMenuMoveBack;
            menuMoveBack.Click += new EventHandler(MoveBackClick);
            nodeMenu.Items.Add(menuMoveBack);
            m_cMenuDict.Add(MenuMoveBack, menuMoveBack);

            ToolStripSeparator separator = new ToolStripSeparator();
            m_cMenuDict.Add(MenuSepalator, separator);
            nodeMenu.Items.Add(separator);

            ToolStripItem menuCreateLayer = new ToolStripMenuItem(MenuCreate);
            menuCreateLayer.Text = MessageResources.LayerMenuCreate;
            menuCreateLayer.Click += new EventHandler(CreateLayerClick);
            nodeMenu.Items.Add(menuCreateLayer);
            m_cMenuDict.Add(MenuCreate, menuCreateLayer);

            ToolStripItem menuRenameLayer = new ToolStripMenuItem(MenuRename);
            menuRenameLayer.Text = MessageResources.LayerMenuRename;
            menuRenameLayer.Click += new EventHandler(RenameLayerClick);
            nodeMenu.Items.Add(menuRenameLayer);
            m_cMenuDict.Add(MenuRename, menuRenameLayer);

            ToolStripItem menuMergeLayer = new ToolStripMenuItem(MenuMerge);
            menuMergeLayer.Text = MessageResources.LayerMenuMerge;
            menuMergeLayer.Click += new EventHandler(MergeLayerClick);
            nodeMenu.Items.Add(menuMergeLayer);
            m_cMenuDict.Add(MenuMerge, menuMergeLayer);

            ToolStripItem menuRemoveLayer = new ToolStripMenuItem(MenuDelete);
            menuRemoveLayer.Text = MessageResources.LayerMenuDelete;
            menuRemoveLayer.Click += new EventHandler(RemoveLayerClick);
            nodeMenu.Items.Add(menuRemoveLayer);
            m_cMenuDict.Add(MenuDelete, menuRemoveLayer);

            return nodeMenu;
        }

        #endregion

        #region Event sequences
        void m_dgv_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex <= 0)
                return;
            LayerGridRow row = (LayerGridRow)m_dgv.Rows[e.RowIndex];
            string oldName = row.Layer.Name;
            string newName = row.Name;
            if (oldName.Equals(newName))
                return;

            if (m_canvas.Layers.ContainsKey(newName))
            {
                Util.ShowNoticeDialog(string.Format(MessageResources.ErrAlrExist, newName));
                row.Name = row.Layer.Name;
                return;
            }
            row.Layer.Name = row.Name;
            m_canvas.RenameLayer(oldName, newName);
        }

        /// <summary>
        /// event sequence of changing the check of layer showing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (m_dgv.CurrentCell is DataGridViewTextBoxCell)
                return;
            if (!m_dirtyEventProcessed)
            {
                LayerGridRow row = (LayerGridRow)((DataGridView)sender).CurrentRow;
                row.Layer.Visible = row.Checked;
                m_dirtyEventProcessed = true;
            }
            else
            {
                m_dirtyEventProcessed = false;
            }
            ((DataGridView)sender).CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        /// <summary>
        /// When binding data in DataGridView,
        /// ...
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewBindingComplete.</param>
        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (((DataGridView)sender).Columns.Contains(MessageResources.LayerColumnShow) && ((DataGridView)sender).Visible)
            {
                ((DataGridView)sender).Columns[MessageResources.LayerColumnShow].Width = LAYER_SHOWCOLUMN_WIDTH;
                ((DataGridView)sender).Columns[MessageResources.LayerColumnShow].Resizable = DataGridViewTriState.False;
                ((DataGridView)sender).Columns[MessageResources.LayerColumnShow].Frozen = true;
                ((DataGridView)sender).Columns[MessageResources.LayerColumnName].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
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
            m_canvas.AddLayer(name);
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
                name = "Layer" + i.ToString();
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
        /// Get new LayerID.
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        private string GetCurrentLayerID()
        {
            string name = null;
            foreach (DataGridViewRow row in m_dgv.Rows)
            {
                if ((bool)row.Cells[0].FormattedValue)
                {
                    name = (string)row.Cells[1].FormattedValue;
                    break;
                }
            }

            if (name == null)
                name = GetNewLayerID(m_canvas);
            return name;
        }

        /// <summary>
        /// when click menu "Delete Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLayerClick(object sender, EventArgs e)
        {
            m_canvas.RemoveLayer(m_selectedLayer);
        }

        /// <summary>
        /// when click menu "Rename Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameLayerClick(object sender, EventArgs e)
        {
            m_dgv.BeginEdit(false);
        }

        /// <summary>
        /// when click menu "Merge Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MergeLayerClick(object sender, EventArgs e)
        {
            List<string> list = m_canvas.GetLayerNameList();
            string newName = SelectBoxDialog.Show(MessageResources.MergeLayerDialogMessage, MessageResources.MergeLayerDialogText, m_selectedLayer, list); 
            if (string.IsNullOrEmpty(newName))
                return;
            Debug.Assert(!m_canvas.Layers.ContainsKey(newName));

            m_canvas.MergeLayer(m_selectedLayer, newName);
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
            LayerGridRow row = (LayerGridRow)m_dgv.Rows[index];
            m_selectedLayer = row.Name;
            m_dgv.ClearSelection();
            row.Selected = true;

            m_cMenuDict[MenuSelectNode].Visible = true;
            m_cMenuDict[MenuMoveFront].Visible = true;
            m_cMenuDict[MenuMoveBack].Visible = true;
            m_cMenuDict[MenuSepalator].Visible = true;
            m_cMenuDict[MenuCreate].Visible = true;
            m_cMenuDict[MenuRename].Visible = true;
            m_cMenuDict[MenuMerge].Visible = true;
            m_cMenuDict[MenuDelete].Visible = true;
        }

        /// <summary>
        /// when click DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dgv_MouseDown(object sender, MouseEventArgs e)
        {
            m_cMenuDict[MenuSelectNode].Visible = false;
            m_cMenuDict[MenuMoveFront].Visible = false;
            m_cMenuDict[MenuMoveBack].Visible = false;
            m_cMenuDict[MenuSepalator].Visible = false;
            m_cMenuDict[MenuCreate].Visible = (m_canvas != null);
            m_cMenuDict[MenuRename].Visible = false;
            m_cMenuDict[MenuMerge].Visible = false;
            m_cMenuDict[MenuDelete].Visible = false;
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
