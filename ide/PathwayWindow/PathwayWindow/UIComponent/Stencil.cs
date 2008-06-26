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
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.Handler;
using UMD.HCIL.Piccolo;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// UI class for PathwayWindow
    /// </summary>
    public partial class Stencil : EcellDockContent
    {
        private PathwayControl m_con;
        private FlowLayoutPanel flowLayoutPanel1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public Stencil(PathwayControl control)
        {
            m_con = control;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BackColor = SystemColors.Window;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.TabIndex = 1;
            SetToolBoxItems();
            // 
            // Stencil
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = false;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.flowLayoutPanel1);
            this.IsSavable = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1, 0);
            this.Name = "Stencils";
            this.Text = MessageResPathway.WindowStencil;
            this.TabText = this.Text;

            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void SetToolBoxItems()
        {
            ToolBoxDragHandler eventHandler = new ToolBoxDragHandler(m_con);

            foreach (ComponentSetting c in m_con.ComponentManager.ComponentSettings)
            {
                // XXX: FIXME
                if (c.ComponentType == ComponentType.Text)
                    continue;

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