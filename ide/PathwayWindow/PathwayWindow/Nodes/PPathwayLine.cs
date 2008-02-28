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
using System.Text;
using UMD.HCIL.Piccolo.Nodes;
using System.Windows.Forms;
using System.Drawing;
using EcellLib.PathwayWindow;
using EcellLib.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo.Event;
using EcellLib.PathwayWindow.Graphic;

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// Line
    /// </summary>
    public class PPathwayLine : PPath
    {
        /// <summary>
        ///  Arrow design settings
        /// </summary>
        public const float ARROW_RADIAN_A = 0.471f;

        /// <summary>
        ///  Arrow design settings
        /// </summary>
        public const float ARROW_RADIAN_B = 5.812f;

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        public const float ARROW_LENGTH = 15;

        /// <summary>
        /// On this CanvasViewComponentSet this PPathwayObject is drawn.
        /// </summary>
        protected CanvasControl m_canvas;

        /// <summary>
        /// this line stands for this EdgeInfo.
        /// </summary>
        private EdgeInfo m_edgeInfo;

        /// <summary>
        /// Coordinate of the variable side end point in global coordinate system.
        /// </summary>
        private PointF m_varPoint;

        /// <summary>
        /// Coordinate of the process side end point in global coordinate system.
        /// </summary>
        private PointF m_proPoint;

        /// <summary>
        /// Accessor for m_edgeInfo.
        /// </summary>
        public EdgeInfo Info
        {
            get { return m_edgeInfo; }
            set { this.m_edgeInfo = value; }
        }

        /// <summary>
        /// Accessor for m_varPoint.
        /// </summary>
        public PointF VarPoint
        {
            get { return m_varPoint; }
            set { this.m_varPoint = value; }
        }

        /// <summary>
        /// Accessor for m_proPoint.
        /// </summary>
        public PointF ProPoint
        {
            get { return m_proPoint; }
            set { this.m_proPoint = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayLine(CanvasControl canvas)
        {
            m_canvas = canvas;
            m_edgeInfo = new EdgeInfo();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="edgeInfo"></param>
        public PPathwayLine(CanvasControl canvas, EdgeInfo edgeInfo)
        {
            m_canvas = canvas;
            m_edgeInfo = edgeInfo;
        }

        /// <summary>
        /// highlight this line.
        /// </summary>
        public void highlight()
        {
            this.Pen = new Pen(Brushes.Orange, Pen.Width);
            this.Brush = Brushes.Orange;
        }

        /// <summary>
        /// Turn off highlight status.
        /// </summary>
        public void unhighlight()
        {
            this.Pen = new Pen(Brushes.Black, Pen.Width);
            this.Brush = Brushes.Black;
        }

        /// <summary>
        /// Inherited method.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void SetOffset(float x, float y)
        {
            //base.SetOffset(x, y);
        }

        /// <summary>
        /// Draw Line.
        /// </summary>
        public void DrawLine()
        {
            SetLine();
            SetDirection();
        }

        /// <summary>
        /// Draw Line
        /// </summary>
        public void SetLine()
        {
            SetLine(Pen.Width);
        }

        /// <summary>
        /// Draw Line
        /// </summary>
        public void SetLine(float width)
        {
            switch (this.m_edgeInfo.TypeOfLine)
            {
                case LineType.Solid:
                    this.Pen = new Pen(this.Brush, width);
                    this.AddLine(this.ProPoint.X, this.ProPoint.Y, this.VarPoint.X, this.VarPoint.Y);
                    break;
                case LineType.Dashed:
                    this.Pen = new Pen(this.Brush, width);
                    AddDashedLine(this, this.ProPoint.X, this.ProPoint.Y, this.VarPoint.X, this.VarPoint.Y);
                    break;
                case LineType.Unknown:
                    this.Pen = new Pen(this.Brush, width);
                    this.AddLine(this.ProPoint.X, this.ProPoint.Y, this.VarPoint.X, this.VarPoint.Y);
                    break;
            }
        }

        /// <summary>
        /// Draw Line.
        /// </summary>
        public void SetDirection()
        {
            switch (this.m_edgeInfo.Direction)
            {
                case EdgeDirection.Bidirection:
                    this.AddPolygon( GetArrowPoints(this.ProPoint, this.VarPoint));
                    this.AddPolygon( GetArrowPoints(this.VarPoint, this.ProPoint));
                    break;
                case EdgeDirection.Inward:
                    this.AddPolygon( GetArrowPoints(this.ProPoint, this.VarPoint));
                    break;
                case EdgeDirection.Outward:
                    this.AddPolygon( GetArrowPoints(this.VarPoint, this.ProPoint));
                    break;
                case EdgeDirection.None:
                    break;
            }
        }

        /// <summary>
        /// add the dash line to PPath.
        /// </summary>
        /// <param name="path">PPath added the dash line.</param>
        /// <param name="startX">the position of start.</param>
        /// <param name="startY">the position of start.</param>
        /// <param name="endX">the position of end.</param>
        /// <param name="endY">the position of end.</param>
        private void AddDashedLine(PPath path, float startX, float startY, float endX, float endY)
        {
            if (path == null)
                return;

            path.FillMode = System.Drawing.Drawing2D.FillMode.Winding;
            float repeatNum = (float)Math.Sqrt((endX - startX) * (endX - startX) + (endY - startY) * (endY - startY)) / 6f;
            float xFragment = (endX - startX) / repeatNum;
            float yFragment = (endY - startY) / repeatNum;

            float presentX = startX;
            float presentY = startY;
            for (int i = 0; i + 2 < repeatNum; i++)
            {
                presentX += xFragment;
                presentY += yFragment;

                if (i % 2 == 1)
                {
                    continue;
                }
                path.AddLine(presentX, presentY, presentX + xFragment, presentY + yFragment);
                path.CloseFigure();
            }
        }

        /// <summary>
        /// Get coordinates of an arrow head.
        /// </summary>
        /// <param name="arrowApex">an apex of an arrow</param>
        /// <param name="guidePoint">an arrow line goes direction from arrowApex to guidePoint</param>
        /// <returns></returns>
        private PointF[] GetArrowPoints(PointF arrowApex, PointF guidePoint)
        {
            guidePoint.X = guidePoint.X - arrowApex.X;
            guidePoint.Y = guidePoint.Y - arrowApex.Y;

            float factor = PathUtil.GetDistance(guidePoint, new Point(0, 0));
            if (factor == 0)
                return new PointF[] { arrowApex, arrowApex, arrowApex };
            guidePoint.X = guidePoint.X / factor;
            float guideRadian = (float)Math.Acos(guidePoint.X);
            if (guidePoint.Y < 0)
                guideRadian = 6.283f - guideRadian;

            PointF arrowPointA = new PointF((float)Math.Cos(ARROW_RADIAN_A + guideRadian), (float)Math.Sin(ARROW_RADIAN_A + guideRadian));
            PointF arrowPointB = new PointF((float)Math.Cos(ARROW_RADIAN_B + guideRadian), (float)Math.Sin(ARROW_RADIAN_B + guideRadian));

            arrowPointA.X = arrowPointA.X * ARROW_LENGTH + arrowApex.X;
            arrowPointA.Y = arrowPointA.Y * ARROW_LENGTH + arrowApex.Y;
            arrowPointB.X = arrowPointB.X * ARROW_LENGTH + arrowApex.X;
            arrowPointB.Y = arrowPointB.Y * ARROW_LENGTH + arrowApex.Y;

            return new PointF[] { arrowApex, arrowPointA, arrowPointB };
        }

        /// <summary>
        /// Create SVG object.
        /// </summary>
        /// <returns></returns>
        public string CreateSVGObject()
        {
            string obj = "";
            string brush = BrushManager.ParseBrushToString(this.Brush);
            switch (this.m_edgeInfo.TypeOfLine)
            {
                case LineType.Solid:
                case LineType.Unknown:
                    obj += SVGUtil.Line(m_proPoint, m_varPoint, brush);
                    break;
                case LineType.Dashed:
                    obj += SVGUtil.DashedLine(m_proPoint, m_varPoint, brush);
                    break;
            }
            switch (this.m_edgeInfo.Direction)
            {
                case EdgeDirection.Bidirection:
                    obj += SVGUtil.Polygon(GetArrowPoints(this.ProPoint, this.VarPoint), brush);
                    obj += SVGUtil.Polygon(GetArrowPoints(this.VarPoint, this.ProPoint), brush);
                    break;
                case EdgeDirection.Inward:
                    obj += SVGUtil.Polygon(GetArrowPoints(this.ProPoint, this.VarPoint), brush);
                    break;
                case EdgeDirection.Outward:
                    obj += SVGUtil.Polygon(GetArrowPoints(this.VarPoint, this.ProPoint), brush);
                    break;
                case EdgeDirection.None:
                    break;
            }
            return obj;
        }

        #region EventHandlers
        /// <summary>
        /// Called when the mouse up.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseUp(PInputEventArgs e)
        {
            base.OnMouseUp(e);
            if (m_canvas == null)
                return;
            m_canvas.FocusNode = this;
            m_canvas.ResetSelectedObjects();
            m_canvas.LineHandler.AddSelectedLine(this);
        }
        #endregion

    }
}
