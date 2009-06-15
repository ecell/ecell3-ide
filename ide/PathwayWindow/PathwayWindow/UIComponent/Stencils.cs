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
        private ToolStripMenuItem propertyToolStripMenuItem;
        private ToolStripMenuItem 追加ToolStripMenuItem;
        private ToolStripMenuItem 削除ToolStripMenuItem;
        private FlowLayoutPanel flowLayoutPanel1;

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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.追加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.flowLayoutPanel1.ContextMenuStrip = this.contextMenuStrip;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(322, 273);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertyToolStripMenuItem,
            this.追加ToolStripMenuItem,
            this.削除ToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 92);
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            this.propertyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.propertyToolStripMenuItem.Text = "Property";
            // 
            // 追加ToolStripMenuItem
            // 
            this.追加ToolStripMenuItem.Name = "追加ToolStripMenuItem";
            this.追加ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.追加ToolStripMenuItem.Text = "追加";
            // 
            // 削除ToolStripMenuItem
            // 
            this.削除ToolStripMenuItem.Name = "削除ToolStripMenuItem";
            this.削除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.削除ToolStripMenuItem.Text = "削除";
            // 
            // Stencils
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(322, 273);
            this.Controls.Add(this.flowLayoutPanel1);
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
            ToolBoxDragHandler eventHandler = new ToolBoxDragHandler(m_con);

            foreach (ComponentSetting c in m_con.ComponentManager.DefaultComponentSettings)
            {
                PToolBoxCanvas pCanvas = new PToolBoxCanvas();
                pCanvas.AllowDrop = true;
                pCanvas.Anchor = (System.Windows.Forms.AnchorStyles)
                            System.Windows.Forms.AnchorStyles.Top
                            | System.Windows.Forms.AnchorStyles.Bottom
                            | System.Windows.Forms.AnchorStyles.Left
                            | System.Windows.Forms.AnchorStyles.Right;
                pCanvas.BackColor = System.Drawing.Color.White;
                pCanvas.Margin = new Padding(4);
                pCanvas.GridFitText = false;
                pCanvas.MinimumSize = pCanvas.Size = new System.Drawing.Size(64, 64);
                pCanvas.PPathwayObject = null;
                pCanvas.RegionManagement = true;
                pCanvas.Name = c.Name;
                pCanvas.Text = c.Name;
                pCanvas.Setting = c;
                pCanvas.AddInputEventListener(eventHandler);
                flowLayoutPanel1.Controls.Add(pCanvas);
            }
        }
    }
}