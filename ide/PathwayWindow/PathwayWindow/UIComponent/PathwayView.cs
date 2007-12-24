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
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// Control class to display pathway.
    /// </summary>
    public class PathwayView : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// The PathwayControl, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        protected PathwayControl m_con;

        /// <summary>
        /// Tshi tabcontrol contains tab pages which represent different type of networks, such as 
        /// metabolic networks, genetic networks.
        /// </summary>
        TabControl m_tabControl;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control">PathwayControl</param>
        public PathwayView(PathwayControl control)
        {
            base.m_isSavable = true;
            this.m_con = control;
            InitializeComponent();
        }
        #endregion

        #region Accessor
        /// <summary>
        ///  get TabControl related this object.
        /// </summary>
        public TabControl TabControl
        {
            get { return this.m_tabControl; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Clear the information managed by this object.
        /// </summary>
        public void Clear()
        {
            m_tabControl.TabPages.Clear();
        }
        #endregion

        #region Inner Methods
        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            this.m_tabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // m_tabControl
            // 
            this.m_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabControl.Location = new System.Drawing.Point(0, 0);
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Size = new System.Drawing.Size(622, 491);
            this.m_tabControl.TabIndex = 0;
            this.m_tabControl.SelectedIndexChanged += new System.EventHandler(this.m_tabControl_SelectedIndexChanged);
            // 
            // PathwayView
            // 
            this.ClientSize = new System.Drawing.Size(622, 491);
            this.Controls.Add(this.m_tabControl);
            this.Name = "PathwayView";
            this.TabText = this.Name;
            this.Text = this.Name;
            this.ResumeLayout(false);

        }
        /// <summary>
        /// When change the focused tab control,
        /// system show the selected model in overview.
        /// </summary>
        /// <param name="sender">TabControl.</param>
        /// <param name="e">EventArgs.</param>
        void m_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((TabControl)sender).TabCount == 0)
                return;

            string modelID = ((TabControl)sender).TabPages[((TabControl)sender).SelectedIndex].Text;
            CanvasControl canvas = m_con.CanvasDictionary[modelID];
            m_con.OverView.SetCanvas(canvas.OverviewCanvas);
            // TODO LayerView Setting
            //m_dgv.DataMember = modelID;
        }
        #endregion
    }
}
