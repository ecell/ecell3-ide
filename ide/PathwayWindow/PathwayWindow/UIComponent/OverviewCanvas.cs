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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
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
    /// PCanvas for overview.
    /// </summary>
    public class OverviewCanvas : PCanvas
    {
        /// <summary>
        /// Graphical content of m_canvas is scaled by m_reductionScale in overview canvas (m_overCanvas)
        /// </summary>
        private static readonly float REDUCTION_SCALE = 0.05f;

        /// <summary>
        /// Display rectangles using overview.
        /// </summary>
        protected PDisplayedArea m_area;

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
        /// Constructor
        /// </summary>
        /// <param name="observedLayer"></param>
        /// <param name="mainCamera"></param>
        public OverviewCanvas(PLayer observedLayer,
                              PCamera mainCamera)
        {
            this.Dock = DockStyle.Fill;
            m_area = new PDisplayedArea();

            m_transparentNode = PPath.CreateRectangle(-500, -500, 1300, 1300);
            m_transparentNode.Brush = m_transparentBrush;
            m_transparentNode.Pickable = true;

            this.Camera.AddLayer(observedLayer);
            this.RemoveInputEventListener(this.PanEventHandler);
            this.RemoveInputEventListener(this.ZoomEventHandler);
            this.Camera.AddInputEventListener(new AreaDragHandler(mainCamera));
            this.Camera.ScaleViewBy(REDUCTION_SCALE);
            this.Camera.TranslateViewBy(500, 500);
            this.Layer.AddChild(m_transparentNode);
            this.Layer.AddChild(m_area);
            this.Camera.AddLayer(this.Layer);
            this.Camera.ChildrenPickable = false;
            this.Camera.BoundsChanged += new PPropertyEventHandler(Camera_BoundsChanged);
        }

        void Camera_BoundsChanged(object sender, PPropertyEventArgs e)
        {
            UpdateTransparent();
        }

        /// <summary>
        /// Set layer which will be overviewed on this overview canvas.
        /// </summary>
        /// <param name="layer"></param>
        public void AddObservedLayer(PLayer layer)
        {
            this.Camera.AddLayer(0, layer);
        }

        /// <summary>
        /// Stop to observe a layer.
        /// </summary>
        /// <param name="layer">a layer</param>
        public void RemoveObservedLayer(PLayer layer)
        {
            this.Camera.RemoveLayer(layer);
        }

        /// <summary>
        /// Update position of transparent node
        /// </summary>
        private void UpdateTransparent()
        {
            RectangleF rect = this.Camera.ViewBounds;
            m_transparentNode.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Update position of transparent node
        /// </summary>
        /// <param name="rect">The overviewing area of pathway view.</param>
        public void UpdateOverview(RectangleF rect)
        {
            this.m_area.Reset();
            this.m_area.Offset = PointF.Empty;
            this.m_area.AddRectangle(rect.X, rect.Y, rect.Width, rect.Height);
            this.UpdateTransparent();
            this.Refresh();
        }

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
    }
}