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
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Components;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.Plugin;
using System.Collections.Generic;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// UI class for PathwayWindow
    /// </summary>
    public partial class Stencils : EcellDockContent
    {
        private PathwayControl m_con;
        private ContextMenuStrip contextMenuStrip;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem AddStencilMenuItem;
        private ToolStripMenuItem DeleteStencilMenuItem;
        private FlowLayoutPanel flowLayoutPanel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public Stencils(PathwayControl control)
        {
            m_con = control;
            InitializeComponent();
            base.Text = MessageResources.WindowStencil;
            base.TabText = base.Text;

            SetDefaultItems();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddStencilMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteStencilMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.BackColor = System.Drawing.SystemColors.Window;
            this.flowLayoutPanel.ContextMenuStrip = this.contextMenuStrip;
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel.Name = "flowLayoutPanel1";
            this.flowLayoutPanel.Size = new System.Drawing.Size(322, 273);
            this.flowLayoutPanel.TabIndex = 1;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddStencilMenuItem,
            this.DeleteStencilMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 70);
            // 
            // AddStencilMenuItem
            // 
            this.AddStencilMenuItem.Name = "AddStencilMenuItem";
            this.AddStencilMenuItem.Size = new System.Drawing.Size(152, 22);
            this.AddStencilMenuItem.Text = "Add Stencil";
            this.AddStencilMenuItem.Click += new System.EventHandler(this.AddStencilMenuItem_Click);
            // 
            // DeleteStencilMenuItem
            // 
            this.DeleteStencilMenuItem.Name = "DeleteStencilMenuItem";
            this.DeleteStencilMenuItem.Size = new System.Drawing.Size(152, 22);
            this.DeleteStencilMenuItem.Text = "Delete Stencil";
            this.DeleteStencilMenuItem.Click += new System.EventHandler(this.DeleteStencilMenuItem_Click);
            // 
            // Stencils
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(322, 273);
            this.Controls.Add(this.flowLayoutPanel);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.Icon_Stencil;
            this.IsSavable = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1, 0);
            this.Name = "Stencils";
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDefaultItems()
        {
            foreach (ComponentSetting cs in m_con.ComponentManager.DefaultComponentSettings)
            {
                SetNewItem(cs);
            }
        }

        private void SetNewItem(ComponentSetting cs)
        {
            PToolBoxCanvas pCanvas = new PToolBoxCanvas(cs);
            ToolBoxDragHandler eventHandler = new ToolBoxDragHandler(m_con);
            pCanvas.AddInputEventListener(eventHandler);
            flowLayoutPanel.Controls.Add(pCanvas);
        }

        private void AddStencilMenuItem_Click(object sender, System.EventArgs e)
        {
            List<PPathwayObject> objects = m_con.Canvas.SelectedNodes;
            if (objects.Count != 1)
                Util.ShowErrorDialog("Select one object to add stencil.");

            PPathwayObject obj = objects[0];
            ComponentSetting cs = obj.Setting.Clone();
            SetNewItem(cs);
        }

        private void DeleteStencilMenuItem_Click(object sender, System.EventArgs e)
        {

        }
    }
}