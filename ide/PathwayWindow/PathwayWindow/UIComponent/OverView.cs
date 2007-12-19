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
        /// Brush for m_transparentNode
        /// </summary>
        private static readonly SolidBrush m_transparentBrush
             = new SolidBrush(Color.FromArgb(0, Color.White));

        /// <summary>
        /// PPath to show main pathway area in the overview.
        /// Normally, this node is colored in red.
        /// </summary>
        private PPath m_transparentNode;
        private PCanvas m_canvas;
        private GroupBox groupBox;

        /// <summary>
        /// Display rectangles using overview.
        /// </summary>
        protected PDisplayedArea m_area;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public OverView()
        {
            base.m_isSavable = true;
            InitializeComponent();
            m_area = new PDisplayedArea();
            m_transparentNode = new PPath();
        }
        #endregion

        #region Accessor
        /// <summary>
        /// the canvas of overview.
        /// </summary>
        /// <summary>
        /// Accessor for m_overviewCanvas.
        /// </summary>
        public PCanvas Canvas
        {
            get { return m_canvas; }
        }

        /// <summary>
        /// Accessor for m_area.
        /// </summary>
        public PDisplayedArea DisplayedArea
        {
            get { return m_area; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set PathwayCanvas.
        /// </summary>
        /// <param name="canvas">The display canvas.</param>
        public void SetCanvas(PCanvas canvas)
        {
            this.groupBox.Controls.Clear();
            this.groupBox.Controls.Add(canvas);
            this.m_canvas = canvas;
        }
        /// <summary>
        /// Set PathwayCanvas.
        /// </summary>
        public void Clear()
        {
            this.m_canvas = new PCanvas();
        }

        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        void InitializeComponent()
        {
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(292, 273);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "OverView";
            // 
            // OverView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.groupBox);
            this.Name = "OverView";
            this.TabText = "OverView";
            this.Text = this.Name;
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

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
