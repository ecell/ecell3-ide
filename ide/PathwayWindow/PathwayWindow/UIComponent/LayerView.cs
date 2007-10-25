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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// UserControl class to display the list of layer.
    /// </summary>
    public class LayerView: DockContent
    {
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

        private DataGridView m_dgv;
        private DataGridViewCheckBoxColumn showDc;
        private DataGridViewTextBoxColumn nameDc;
        #region Fields
        /// <summary>
        /// PPath to show main pathway area in the overview.
        /// Normally, this node is colored in red.
        /// </summary>
        private GroupBox groupBox;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LayerView(PathwayControl control)
        {
            this.m_con = control;
            InitializeComponent();
        }
        #endregion

        #region Accessor
        /// <summary>
        ///  get DataGridView of layer table.
        /// </summary>
        public DataGridView DataGridView
        {
            get { return this.m_dgv; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Clear Layer Table.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Set layer.
        /// </summary>
        /// <param name="layer"></param>
        public void AddLayer(PLayer layer)
        {
        }

        /// <summary>
        /// Stop to observe a layer.
        /// </summary>
        /// <param name="layer">a layer</param>
        public void RemoveLayer(PLayer layer)
        {
        }

        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.m_dgv = new System.Windows.Forms.DataGridView();
            this.showDc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameDc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.m_dgv);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(275, 196);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "LayerView";
            // 
            // m_dgv
            // 
            this.m_dgv.AllowUserToAddRows = false;
            this.m_dgv.AllowUserToDeleteRows = false;
            this.m_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.showDc,
            this.nameDc});
            this.m_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgv.Location = new System.Drawing.Point(3, 15);
            this.m_dgv.MultiSelect = false;
            this.m_dgv.Name = "m_dgv";
            this.m_dgv.RowTemplate.Height = 21;
            this.m_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_dgv.Size = new System.Drawing.Size(269, 178);
            this.m_dgv.TabIndex = 0;
            this.m_dgv.CurrentCellDirtyStateChanged += new EventHandler(m_dgv_CurrentCellDirtyStateChanged);
            this.m_dgv.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv_DataBindingComplete);
            this.m_dgv.VisibleChanged += new EventHandler(m_dgv_VisibleChanged);
            // 
            // showDc
            // 
            this.showDc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.showDc.HeaderText = "Show";
            this.showDc.Name = "showDc";
            this.showDc.Width = 40;
            // 
            // nameDc
            // 
            this.nameDc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDc.HeaderText = "Name";
            this.nameDc.Name = "nameDc";
            this.nameDc.ReadOnly = true;
            // 
            // LayerView
            // 
            this.ClientSize = new System.Drawing.Size(275, 196);
            this.Controls.Add(this.groupBox);
            this.Name = "LayerView";
            this.TabText = this.Name;
            this.Text = this.Name;
            this.groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgv)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        /// <summary>
        /// event sequence of changing the check of layer showing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!m_dirtyEventProcessed)
            {
                string modelID = ((DataGridView)sender).DataMember;
                bool show = !(bool)((DataGridView)sender).CurrentRow.Cells["Show"].Value;
                string layerName = (string)((DataGridView)sender).CurrentRow.Cells["Name"].Value;
                m_con.CanvasDictionary[modelID].Layers[layerName].Visible = show;
                m_con.CanvasDictionary[modelID].PathwayCanvas.Refresh();
                m_con.CanvasDictionary[modelID].RefreshVisibility();
                m_con.OverView.Canvas.Refresh();
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
        void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (((DataGridView)sender).Columns.Contains("Show") && ((DataGridView)sender).Visible)
            {
                ((DataGridView)sender).Columns["Show"].Width = LAYER_SHOWCOLUMN_WIDTH;
                ((DataGridView)sender).Columns["Show"].Resizable = DataGridViewTriState.False;
                ((DataGridView)sender).Columns["Show"].Frozen = true;
                ((DataGridView)sender).Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                ((DataGridView)sender).Columns["Name"].ReadOnly = true;
            }
        }

        /// <summary>
        /// When Layer visiblity is changed,
        /// ...
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewBindingComplete.</param>
        void m_dgv_VisibleChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).Columns.Contains("Show") && ((DataGridView)sender).Visible)
            {
                ((DataGridView)sender).Columns["Show"].Width = LAYER_SHOWCOLUMN_WIDTH;
                ((DataGridView)sender).Columns["Show"].Resizable = DataGridViewTriState.False;
                ((DataGridView)sender).Columns["Show"].Frozen = true;
                ((DataGridView)sender).Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                ((DataGridView)sender).Columns["Name"].ReadOnly = true;
            }
        }

    }
}
