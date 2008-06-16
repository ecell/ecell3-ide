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
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using System.Windows.Forms;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// Control class to display the overview of pathway.
    /// </summary>
    public class OverView: EcellDockContent
    {
        #region Fields
        /// <summary>
        /// PathwayControl
        /// </summary>
        private PathwayControl m_con;
        /// <summary>
        /// GroupBox
        /// </summary>
        private Panel panel;

        /// <summary>
        /// Display rectangles using overview.
        /// </summary>
        protected PDisplayedArea m_area;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public OverView(PathwayControl control)
        {
            base.m_isSavable = true;
            InitializeComponent();
            m_con = control;
            m_con.CanvasChange += new EventHandler(m_con_CanvasChange);
            m_area = new PDisplayedArea();
            base.Text = MessageResPathway.WindowOverview;
            base.TabText = base.Text;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(292, 273);
            this.panel.TabIndex = 1;
            // 
            // OverView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.panel);
            this.Icon = global::EcellLib.PathwayWindow.PathwayResource.Icon_OverView;
            this.Name = "OverView";
            this.TabText = "OverView";
            this.Text = this.Name;
            this.ResumeLayout(false);

        }

        /// <summary>
        /// EventHandler to set canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_con_CanvasChange(object sender, EventArgs e)
        {
            this.panel.Controls.Clear();
            if (m_con.Canvas == null)
                return;
            PCanvas canvas = m_con.Canvas.OverviewCanvas;
            this.panel.Controls.Add(canvas);
        }
        #endregion
    }
}
