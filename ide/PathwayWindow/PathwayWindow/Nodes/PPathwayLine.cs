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
using System.Windows.Forms;
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Event;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Line
    /// </summary>
    public class PPathwayLine : PPath
    {
        #region Constants
        /// <summary>
        ///  Arrow design settings
        /// </summary>
        private const float ARROW_DEGREE = 18f / 360f;
        /// <summary>
        /// pi
        /// </summary>
        private const float PI2 = (float)Math.PI * 2;
        /// <summary>
        ///  Arrow design settings
        /// radian = x / 360 * 2pi, x = ARROW_DEGREE, 2pi = 6.283
        /// </summary>
        private const float ARROW_RADIAN_A = ARROW_DEGREE * PI2;

        /// <summary>
        ///  Arrow design settings
        /// </summary>
        private const float ARROW_RADIAN_B = PI2 * (1 - ARROW_DEGREE);

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        private const float ARROW_LENGTH = 15;

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        private const float LINE_WIDTH = 2;
        #endregion

        #region Fields
        /// <summary>
        /// On this CanvasViewComponentSet this PPathwayObject is drawn.
        /// </summary>
        private CanvasControl m_canvas;

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
        #endregion

        #region Accessors
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
        #endregion

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
            SetLine(LINE_WIDTH);
        }

        /// <summary>
        /// Draw Line
        /// </summary>
        public void SetLine(float width)
        {
            base.Pen = new Pen(Brush, width);
            switch (this.m_edgeInfo.TypeOfLine)
            {
                case LineType.Solid:
                case LineType.Unknown:
                    AddLine(m_proPoint.X, m_proPoint.Y, m_varPoint.X, m_varPoint.Y);
                    break;
                case LineType.Dashed:
                    AddDashedLine(m_proPoint.X, m_proPoint.Y, m_varPoint.X, m_varPoint.Y);
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
                    this.AddPolygon(GetArrowPoints(m_proPoint, m_varPoint));
                    this.AddPolygon(GetArrowPoints(m_varPoint, m_proPoint));
                    break;
                case EdgeDirection.Inward:
                    this.AddPolygon(GetArrowPoints(m_proPoint, m_varPoint));
                    break;
                case EdgeDirection.Outward:
                    this.AddPolygon(GetArrowPoints(m_varPoint, m_proPoint));
                    break;
                case EdgeDirection.None:
                    break;
            }
        }

        /// <summary>
        /// add the dash line to PPath.
        /// </summary>
        /// <param name="startX">the position of start.</param>
        /// <param name="startY">the position of start.</param>
        /// <param name="endX">the position of end.</param>
        /// <param name="endY">the position of end.</param>
        private void AddDashedLine(float startX, float startY, float endX, float endY)
        {
            this.FillMode = System.Drawing.Drawing2D.FillMode.Winding;
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
                this.AddLine(presentX, presentY, presentX + xFragment, presentY + yFragment);
                this.CloseFigure();
            }
        }

        /// <summary>
        /// Get coordinates of an arrow head.
        /// </summary>
        /// <param name="arrowApex">an apex of an arrow</param>
        /// <param name="guidePoint">an arrow line goes direction from arrowApex to guidePoint</param>
        /// <returns></returns>
        private static PointF[] GetArrowPoints(PointF arrowApex, PointF guidePoint)
        {
            guidePoint.X = guidePoint.X - arrowApex.X;
            guidePoint.Y = guidePoint.Y - arrowApex.Y;

            float factor = PathUtil.GetDistance(guidePoint, new Point(0, 0));
            if (factor == 0)
                return new PointF[] { arrowApex, arrowApex, arrowApex };
            guidePoint.X = guidePoint.X / factor;
            float guideRadian = (float)Math.Acos(guidePoint.X);
            if (guidePoint.Y < 0)
                guideRadian = PI2 - guideRadian;

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
            string width = this.Pen.Width.ToString();
            switch (this.m_edgeInfo.TypeOfLine)
            {
                case LineType.Solid:
                case LineType.Unknown:
                    obj += SVGUtil.Line(m_proPoint, m_varPoint, brush, width);
                    break;
                case LineType.Dashed:
                    obj += SVGUtil.DashedLine(m_proPoint, m_varPoint, brush, width);
                    break;
            }
            switch (this.m_edgeInfo.Direction)
            {
                case EdgeDirection.Bidirection:
                    obj += SVGUtil.Polygon(GetArrowPoints(m_proPoint, m_varPoint), brush, width);
                    obj += SVGUtil.Polygon(GetArrowPoints(m_varPoint, m_proPoint), brush, width);
                    break;
                case EdgeDirection.Inward:
                    obj += SVGUtil.Polygon(GetArrowPoints(m_proPoint, m_varPoint), brush, width);
                    break;
                case EdgeDirection.Outward:
                    obj += SVGUtil.Polygon(GetArrowPoints(m_proPoint, m_varPoint), brush, width);
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
            m_canvas.NotifyResetSelect();
            m_canvas.FocusNode = this;
            m_canvas.LineHandler.AddSelectedLine(this);
        }
        #endregion

    }
}
