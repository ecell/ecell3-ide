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
using EcellLib.PathwayWindow.Resources;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// UserControl class to display the list of layer.
    /// </summary>
    public class LayerView: EcellDockContent
    {
        #region Static Fields
        private static string MenuCreate = "LayerMenuCreate";
        private static string MenuDelete = "LayerMenuDelete";
        private static string MenuMerge = "LayerMenuMerge";
        private static string MenuRename = "LayerMenuRename";
        private static string MenuSelectNode = "LayerMenuSelectNodes";
        private static string MenuMoveFront = "LayerMenuMoveFront";
        private static string MenuMoveBack = "LayerMenuMoveBack";
        private static string MenuSepalator = "Sepalator";
        private static string DialogTitle = "LayerDialogTitle";
        private static string DialogMessage = "LayerDialogMessage";
        #endregion

        #region Fields
        /// <summary>
        /// Width of "Show" column of layer DataGridView.
        /// </summary>
        private readonly int LAYER_SHOWCOLUMN_WIDTH = 50;

        /// <summary>
        /// The PathwayControl, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        protected PathwayControl m_con;

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
        /// <summary>
        /// PPath to show main pathway area in the overview.
        /// Normally, this node is colored in red.
        /// </summary>
        private GroupBox groupBox;
        /// <summary>
        /// List of ToolStripMenuItems for ContextMenu
        /// </summary>
        private Dictionary<string, ToolStripItem> m_cMenuDict = new Dictionary<string, ToolStripItem>();
        /// <summary>
        /// Selected DataRow.
        /// </summary>
        private DataGridViewRow m_selectedRow = null;
        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LayerView(PathwayControl control)
        {
            base.m_isSavable = true;
            this.m_con = control;
            this.m_resources = control.Resources;
            this.m_con.CanvasChange += new EventHandler(m_con_CanvasChange);
            InitializeComponent();
            // Preparing context menus.
            m_dgv.ContextMenuStrip = CreatePopUpMenus();

        }

        void m_con_CanvasChange(object sender, EventArgs e)
        {
            m_dgv.DataSource = null;
            if (m_con.CanvasControl == null)
                return;
            m_dgv.DataSource = m_con.CanvasControl.LayerTable;
        }
        #endregion

        #region Accessor
        #endregion

        #region Methods
        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            this.groupBox = new GroupBox();
            this.m_dgv = new DataGridView();
            this.groupBox.SuspendLayout();
            ((ISupportInitialize)this.m_dgv).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.m_dgv);
            this.groupBox.Dock = DockStyle.Fill;
            this.groupBox.Location = new Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new Size(275, 196);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "LayerView";
            // 
            // m_dgv
            // 
            this.m_dgv.AllowUserToAddRows = false;
            this.m_dgv.AllowUserToDeleteRows = false;
            this.m_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.m_dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgv.Dock = DockStyle.Fill;
            this.m_dgv.Location = new Point(3, 15);
            this.m_dgv.MultiSelect = false;
            this.m_dgv.Name = "m_dgv";
            this.m_dgv.RowHeadersVisible = false;
            this.m_dgv.RowTemplate.Height = 21;
            this.m_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.m_dgv.Size = new Size(269, 178);
            this.m_dgv.TabIndex = 0;
            this.m_dgv.MouseDown += new MouseEventHandler(this.m_dgv_MouseDown);
            this.m_dgv.CellMouseDown += new DataGridViewCellMouseEventHandler(this.m_dgv_CellMouseDown);
            this.m_dgv.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            this.m_dgv.CurrentCellDirtyStateChanged += new EventHandler(this.m_dgv_CurrentCellDirtyStateChanged);
            this.m_dgv.VisibleChanged += new EventHandler(this.m_dgv_VisibleChanged);
            // 
            // LayerView
            // 
            this.ClientSize = new Size(275, 196);
            this.Controls.Add(this.groupBox);
            this.Name = "LayerView";
            this.TabText = this.Name;
            this.Text = this.Name;
            this.groupBox.ResumeLayout(false);
            ((ISupportInitialize)this.m_dgv).EndInit();
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
            menuSelectNodes.Text = m_resources.GetString(MenuSelectNode);
            menuSelectNodes.Click += new EventHandler(SelectNodesClick);
            nodeMenu.Items.Add(menuSelectNodes);
            m_cMenuDict.Add(MenuSelectNode, menuSelectNodes);

            ToolStripItem menuMoveFront = new ToolStripMenuItem(MenuMoveFront);
            menuMoveFront.Text = m_resources.GetString(MenuMoveFront);
            menuMoveFront.Click += new EventHandler(MoveFrontClick);
            nodeMenu.Items.Add(menuMoveFront);
            m_cMenuDict.Add(MenuMoveFront, menuMoveFront);

            ToolStripItem menuMoveBack = new ToolStripMenuItem(MenuMoveBack);
            menuMoveBack.Text = m_resources.GetString(MenuMoveBack);
            menuMoveBack.Click += new EventHandler(MoveBackClick);
            nodeMenu.Items.Add(menuMoveBack);
            m_cMenuDict.Add(MenuMoveBack, menuMoveBack);

            ToolStripSeparator separator = new ToolStripSeparator();
            m_cMenuDict.Add(MenuSepalator, separator);
            nodeMenu.Items.Add(separator);

            ToolStripItem menuCreateLayer = new ToolStripMenuItem(MenuCreate);
            menuCreateLayer.Text = m_resources.GetString(MenuCreate);
            menuCreateLayer.Click += new EventHandler(CreateLayerClick);
            nodeMenu.Items.Add(menuCreateLayer);
            m_cMenuDict.Add(MenuCreate, menuCreateLayer);

            ToolStripItem menuRenameLayer = new ToolStripMenuItem(MenuRename);
            menuRenameLayer.Text = m_resources.GetString(MenuRename);
            menuRenameLayer.Click += new EventHandler(RenameLayerClick);
            nodeMenu.Items.Add(menuRenameLayer);
            m_cMenuDict.Add(MenuRename, menuRenameLayer);

            ToolStripItem menuMergeLayer = new ToolStripMenuItem(MenuMerge);
            menuMergeLayer.Text = m_resources.GetString(MenuMerge);
            menuMergeLayer.Click += new EventHandler(MergeLayerClick);
            nodeMenu.Items.Add(menuMergeLayer);
            m_cMenuDict.Add(MenuMerge, menuMergeLayer);

            ToolStripItem menuDeleteLayer = new ToolStripMenuItem(MenuDelete);
            menuDeleteLayer.Text = m_resources.GetString(MenuDelete);
            menuDeleteLayer.Click += new EventHandler(DeleteLayerClick);
            nodeMenu.Items.Add(menuDeleteLayer);
            m_cMenuDict.Add(MenuDelete, menuDeleteLayer);

            return nodeMenu;
        }
        #endregion

        #region Event sequences
        /// <summary>
        /// event sequence of changing the check of layer showing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!m_dirtyEventProcessed)
            {
                CanvasControl canvas = m_con.CanvasControl;
                bool show = !(bool)((DataGridView)sender).CurrentRow.Cells["Show"].Value;
                string layerName = (string)((DataGridView)sender).CurrentRow.Cells["Name"].Value;

                canvas.ChangeLayerVisibility(layerName, show);
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
            if (((DataGridView)sender).Columns.Contains("Show") && ((DataGridView)sender).Visible)
            {
                ((DataGridView)sender).Columns["Show"].Width = LAYER_SHOWCOLUMN_WIDTH;
                ((DataGridView)sender).Columns["Show"].Resizable = DataGridViewTriState.False;
                ((DataGridView)sender).Columns["Show"].Frozen = true;
                ((DataGridView)sender).Columns["Name"].SortMode = DataGridViewColumnSortMode.Automatic;
                ((DataGridView)sender).Columns["Name"].ReadOnly = true;
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
            if (((DataGridView)sender).Columns.Contains("Show") && ((DataGridView)sender).Visible)
            {
                ((DataGridView)sender).Columns["Show"].Width = LAYER_SHOWCOLUMN_WIDTH;
                ((DataGridView)sender).Columns["Show"].Resizable = DataGridViewTriState.False;
                ((DataGridView)sender).Columns["Show"].Frozen = true;
                ((DataGridView)sender).Columns["Name"].SortMode = DataGridViewColumnSortMode.Automatic;
                ((DataGridView)sender).Columns["Name"].ReadOnly = true;
            }
        }
        /// <summary>
        /// when click menu "Create Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectNodesClick(object sender, EventArgs e)
        {
            CanvasControl canvas = m_con.CanvasControl;
            string name = (string)m_selectedRow.Cells[1].FormattedValue;
            canvas.SelectNodesUnderLayer(name);
        }
        /// <summary>
        /// when click menu "Move Front"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveFrontClick(object sender, EventArgs e)
        {
            CanvasControl canvas = m_con.CanvasControl;
            string name = (string)m_selectedRow.Cells[1].FormattedValue;
            PLayer layer = canvas.Layers[name];
            canvas.LayerMoveToFront(layer);
        }
        /// <summary>
        /// when click menu "Move Back"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveBackClick(object sender, EventArgs e)
        {
            CanvasControl canvas = m_con.CanvasControl;
            string name = (string)m_selectedRow.Cells[1].FormattedValue;
            PLayer layer = canvas.Layers[name];
            canvas.LayerMoveToBack(layer);
        }
        /// <summary>
        /// when click menu "Create Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateLayerClick(object sender, EventArgs e)
        {
            CanvasControl canvas = m_con.CanvasControl;
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
            canvas.AddLayer(name);
        }

        /// <summary>
        /// when click menu "Delete Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteLayerClick(object sender, EventArgs e)
        {
            string name = (string)m_selectedRow.Cells[1].FormattedValue;
            CanvasControl canvas = m_con.CanvasControl;
            canvas.DeleteLayer(name);
        }
        /// <summary>
        /// when click menu "Rename Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameLayerClick(object sender, EventArgs e)
        {
            CanvasControl canvas = m_con.CanvasControl;
            string oldName = (string)m_selectedRow.Cells[1].FormattedValue;
            string newName = InputBoxDialog.Show(m_resources.GetString(DialogMessage), m_resources.GetString(DialogTitle), oldName);
            if (newName == null || newName.Equals(""))
                return;
            if (canvas.Layers.ContainsKey(newName))
            {
                MessageBox.Show(newName + m_resources.GetString("ErrAlrExist"));
                return;
            }

            canvas.RenameLayer(oldName, newName);
        }
        /// <summary>
        /// when click menu "Merge Layer"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MergeLayerClick(object sender, EventArgs e)
        {
            string oldName = (string)m_selectedRow.Cells[1].FormattedValue;
            CanvasControl canvas = m_con.CanvasControl;
            List<string> list = canvas.GetLayerNameList();
            string newName = SelectBoxDialog.Show(m_resources.GetString(DialogMessage), m_resources.GetString(DialogTitle), list);
            if (newName == null || newName.Equals(""))
                return;
            if (!canvas.Layers.ContainsKey(newName))
            {
                MessageBox.Show(m_resources.GetString("ErrLayerNot"));
                return;
            }

            canvas.MergeLayer(oldName, newName);
        }
        /// <summary>
        /// when click DataGridRows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_cMenuDict[MenuSelectNode].Visible = true;
            m_cMenuDict[MenuMoveFront].Visible = true;
            m_cMenuDict[MenuMoveBack].Visible = true;
            m_cMenuDict[MenuSepalator].Visible = true;
            m_cMenuDict[MenuCreate].Visible = true;
            m_cMenuDict[MenuRename].Visible = true;
            m_cMenuDict[MenuMerge].Visible = true;
            m_cMenuDict[MenuDelete].Visible = true;
            if (e.RowIndex >= 0)
            {
                m_selectedRow = m_dgv.Rows[e.RowIndex];
                m_selectedRow.Selected = true;
            }
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
            m_cMenuDict[MenuCreate].Visible = (m_con.CanvasControl != null);
            m_cMenuDict[MenuRename].Visible = false;
            m_cMenuDict[MenuMerge].Visible = false;
            m_cMenuDict[MenuDelete].Visible = false;
            m_selectedRow = null;
        }
        #endregion
    }
}
