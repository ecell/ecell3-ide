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
using WeifenLuo.WinFormsUI.Docking;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;

namespace EcellLib.PathwayWindow.UIComponent
{
    public class OverView: DockContent
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

        /// <summary>
        /// The canvas of overview.
        /// </summary>
        private PCanvas m_canvas;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reductionScale"></param>
        /// <param name="observedLayer"></param>
        /// <param name="mainCamera"></param>
        /// <param name="area"></param>
        public OverView(float reductionScale,
                              PLayer observedLayer,
                              PCamera mainCamera,
                              PDisplayedArea area)
        {
            m_transparentNode = PPath.CreateRectangle(-500,-500,1300,1300);
            m_transparentNode.Brush = m_transparentBrush;
            m_transparentNode.Pickable = true;

            InitializeComponent();
            m_canvas.Camera.AddLayer(observedLayer);
            m_canvas.RemoveInputEventListener(m_canvas.PanEventHandler);
            m_canvas.RemoveInputEventListener(m_canvas.ZoomEventHandler);
            m_canvas.Camera.AddInputEventListener(new AreaDragHandler(mainCamera));
            m_canvas.Camera.ScaleViewBy(reductionScale);
            m_canvas.Camera.TranslateViewBy(500, 500);
            m_canvas.Layer.AddChild(m_transparentNode);
            m_canvas.Layer.AddChild(area);
            m_canvas.Camera.AddLayer(m_canvas.Layer);
            m_canvas.Camera.ChildrenPickable = false;
            m_canvas.Camera.BoundsChanged += new PPropertyEventHandler(Camera_BoundsChanged);
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

        #endregion

        #region Methods
        /// <summary>
        /// Set layer which will be overviewed on this overview canvas.
        /// </summary>
        /// <param name="layer"></param>
        public void AddObservedLayer(PLayer layer)
        {
            this.m_canvas.Camera.AddLayer(0, layer);
        }

        /// <summary>
        /// Stop to observe a layer.
        /// </summary>
        /// <param name="layer">a layer</param>
        public void RemoveObservedLayer(PLayer layer)
        {
            this.m_canvas.Camera.RemoveLayer(layer);
        }

        /// <summary>
        /// Update position of transparent node
        /// </summary>
        public void UpdateTransparent()
        {
            RectangleF rect = this.m_canvas.Camera.ViewBounds;
            m_transparentNode.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Initializer for PCanvas
        /// </summary>
        private void InitializeComponent()
        {
            this.m_canvas = new PCanvas();
            this.SuspendLayout();
            // 
            // m_canvas
            // 
            this.m_canvas.AllowDrop = true;
            this.m_canvas.BackColor = System.Drawing.Color.White;
            this.m_canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_canvas.GridFitText = false;
            this.m_canvas.Location = new System.Drawing.Point(0, 0);
            this.m_canvas.Name = "m_canvas";
            this.m_canvas.RegionManagement = true;
            this.m_canvas.Size = new System.Drawing.Size(292, 273);
            this.m_canvas.TabIndex = 0;
            this.m_canvas.Text = "pCanvas1";
            // 
            // OverView
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.m_canvas);
            this.Name = "OverView";
            this.ResumeLayout(false);

        }

        void Camera_BoundsChanged(object sender, PPropertyEventArgs e)
        {
            UpdateTransparent();
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
