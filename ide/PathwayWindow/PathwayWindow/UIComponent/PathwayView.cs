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
    /// Control class to display pathway.
    /// </summary>
    public class PathwayView: DockContent
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
            this.m_tabControl = new TabControl();
            this.SuspendLayout();
            // 
            // m_tabControl
            // 
            this.m_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabControl.Location = new System.Drawing.Point(0, 0);
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Size = new System.Drawing.Size(292, 273);
            this.m_tabControl.TabIndex = 0;
            this.m_tabControl.SelectedIndexChanged += new EventHandler(m_tabControl_SelectedIndexChanged);
            this.m_tabControl.MouseEnter += new EventHandler(m_tabControl_OnMouseEnter);
            this.m_tabControl.MouseWheel += new MouseEventHandler(m_tabControl_OnMouseWheel);

            // 
            // PathwayView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
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
            if (((TabControl)sender).TabCount != 0)
            {
                string modelID = ((TabControl)sender).TabPages[((TabControl)sender).SelectedIndex].Text;
                m_con.OverView.SetCanvas(m_con.CanvasDictionary[modelID]);
                // TODO LayerView Setting
                //m_dgv.DataMember = modelID;

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">TabControl.</param>
        /// <param name="e">MouseEventArgs.</param>
        void m_tabControl_OnMouseWheel(object sender, MouseEventArgs e)
        {

            if (Control.ModifierKeys == Keys.Shift)
            {
                this.m_con.PanCanvas(Direction.Horizontal, e.Delta);
            }
            else if (Control.ModifierKeys == Keys.Control || e.Button == MouseButtons.Right)
            {
                float zoom = (float)1.00 + (float)e.Delta / 1200;
                this.m_con.ActiveCanvas.Zoom(zoom);
            }
            else
            {
                this.m_con.PanCanvas(Direction.Vertical, e.Delta);
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="sender">TabControl.</param>
        /// <param name="e">EventArgs.</param>
        void m_tabControl_OnMouseEnter(object sender, EventArgs e)
        {
            this.m_tabControl.Focus();
        }
        #endregion

        #region Inner Class
        /// <summary>
        /// Handler for an overview.
        /// </summary>
        public class AreaDragHandler : PDragEventHandler
        {
            /// <summary>
            /// PCamera of a main pathway canvas. When a displayed area in an overview window
            /// is moved with a mouse drag, a main window's image will be changed for displaying
            /// the area indicated in an overview window. This operation is done by moving this 
            /// camera by this object (PDisplayedArea instance).
            /// </summary>
            private PCamera m_mainCamera;

            private PointF startPoint;
            private PointF prevPoint;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="mainCamera"></param>
            public AreaDragHandler(PCamera mainCamera)
            {
                this.m_mainCamera = mainCamera;
            }

            /// <summary>
            /// Called when the mouse is dragged.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected override void OnDrag(object sender, PInputEventArgs e)
            {
                if (e.PickedNode is PPath)
                {
                    return;
                }
                base.OnDrag(sender, e);
                PointF newPoint = e.PickedNode.Offset;
                m_mainCamera.TranslateViewBy(prevPoint.X - newPoint.X, prevPoint.Y - newPoint.Y);
                m_mainCamera.Canvas.Refresh();
                prevPoint = e.PickedNode.Offset;
            }

            /// <summary>
            /// Called when the mouse is down.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public override void OnMouseDown(object sender, PInputEventArgs e)
            {
                base.OnMouseDown(sender, e);
                startPoint = prevPoint = e.PickedNode.Offset;
            }
        }
        #endregion
    }
}
