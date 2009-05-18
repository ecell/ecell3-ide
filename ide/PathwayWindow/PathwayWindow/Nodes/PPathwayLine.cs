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
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.Objects;
using UMD.HCIL.Piccolo.Event;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Line
    /// </summary>
    public class PPathwayLine : PPathwayNode
    {
        #region Constants
        /// <summary>
        ///  Arrow design settings
        /// </summary>
        internal const float ARROW_DEGREE = 18f / 360f;
        /// <summary>
        /// pi
        /// </summary>
        internal const float PI2 = (float)Math.PI * 2;
        /// <summary>
        ///  Arrow design settings
        /// radian = x / 360 * 2pi, x = ARROW_DEGREE, 2pi = 6.283
        /// </summary>
        internal const float ARROW_RADIAN_A = ARROW_DEGREE * PI2;

        /// <summary>
        ///  Arrow design settings
        /// </summary>
        internal const float ARROW_RADIAN_B = PI2 * (1 - ARROW_DEGREE);

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        internal const float ARROW_LENGTH = 15;

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        internal const float LINE_WIDTH = 2;

        /// <summary>
        /// 
        /// </summary>
        internal static readonly Brush DefaultEdgeBrush = Brushes.Black;

        /// <summary>
        /// 
        /// </summary>
        internal static readonly Brush SelectedBrush = Brushes.Yellow;
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
        /// <summary>
        /// 
        /// </summary>
        private PPathwayVariable m_variable;
        /// <summary>
        /// 
        /// </summary>
        private PPathwayProcess m_process;

        /// <summary>
        /// 
        /// </summary>
        private bool m_selected = false;

        /// <summary>
        /// 
        /// </summary>
        private float m_width = LINE_WIDTH;
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

        /// <summary>
        /// 
        /// </summary>
        public float EdgeWidth
        {
            get { return m_width; }
            set
            {
                this.m_width = value;
                this.Pen.Width = m_width;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush EdgeBrush
        {
            get { return this.Brush; }
            set
            {
                this.Brush = value;
                this.Pen.Brush = this.Brush;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Selected
        {
            get { return m_selected; }
            set
            {
                m_selected = value;
                if (value)
                {
                    this.EdgeBrush = LineHandler.LINE_BRUSH;
                }
                else
                {
                    this.EdgeBrush = m_canvas.Control.Animation.EdgeBrush;
                }
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayLine(CanvasControl canvas)
            : this(canvas, new EdgeInfo())
        {
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
            
            SetEdge(DefaultEdgeBrush, m_width);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="edgeInfo"></param>
        /// <param name="process"></param>
        /// <param name="variable"></param>
        public PPathwayLine(CanvasControl canvas, EdgeInfo edgeInfo, PPathwayProcess process, PPathwayVariable variable)
            : this(canvas, edgeInfo)
        {
            m_variable = variable;
            m_process = process;
            m_variable.Relations.Add(this);
            m_process.Relations.Add(this);

            SetEdge(canvas.Control.Animation.EdgeBrush, canvas.Control.Animation.EdgeWidth);
            base.Pickable = (variable.Visible && process.Visible);
            base.Visible = (variable.Visible && process.Visible);

            m_varPoint = variable.GetContactPoint(process.CenterPointF);
            m_proPoint = process.GetContactPoint(m_varPoint);
            this.DrawLine();
            
        }

        /// <summary>
        /// Refresh Line.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            if (m_variable == null || m_process == null)
                return;
            m_varPoint = m_variable.GetContactPoint(m_process.CenterPointF);
            m_proPoint = m_process.GetContactPoint(m_varPoint);
            DrawLine();
            this.Visible = m_process.Visible && m_variable.Visible;
            this.Pickable = this.Visible;
        }

        /// <summary>
        /// Visible change
        /// </summary>
        public void VisibleChange()
        {
            bool visible = m_process.Visible && m_variable.Visible;
            this.Visible = visible;
            this.Pickable = visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="width"></param>
        public void SetEdge(Brush brush, float width)
        {
            this.Brush = brush;
            this.m_width = width;
            this.Pen = new Pen(brush, width);
        }

        /// <summary>
        /// Draw Line.
        /// </summary>
        public void DrawLine()
        {
            if (m_proPoint == m_varPoint)
                return;

            //Set Pen
            this.Pen = new Pen(Brush, m_width);

            //Set line
            GraphicsPath path = new GraphicsPath();
            SetLine(path);
            //Set Arrow
            SetArrow(path);
            this.AddPath(path, false);
        }

        /// <summary>
        /// Set line
        /// </summary>
        /// <param name="path"></param>
        private void SetLine(GraphicsPath path)
        {
            try
            {
                if (m_edgeInfo.LineType == LineType.Dashed)
                    AddDashedLine(path, m_proPoint.X, m_proPoint.Y, m_varPoint.X, m_varPoint.Y);
                else
                    path.AddLine(m_proPoint.X, m_proPoint.Y, m_varPoint.X, m_varPoint.Y);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Set Arrow.
        /// </summary>
        /// <param name="path"></param>
        private void SetArrow(GraphicsPath path)
        {
            if (m_process != null && m_process.ViewMode && m_edgeInfo.Coefficient != 1 && !m_edgeInfo.IsEndNode)
                return;
            switch (this.m_edgeInfo.Direction)
            {
                case EdgeDirection.Bidirection:
                    path.AddPolygon(GetArrowPoints(m_proPoint, m_varPoint));
                    path.AddPolygon(GetArrowPoints(m_varPoint, m_proPoint));
                    break;
                case EdgeDirection.Inward:
                    path.AddPolygon(GetArrowPoints(m_proPoint, m_varPoint));
                    break;
                case EdgeDirection.Outward:
                    path.AddPolygon(GetArrowPoints(m_varPoint, m_proPoint));
                    break;
                case EdgeDirection.None:
                    break;
            }
        }

        /// <summary>
        /// add the dash line to PPath.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="startX">the position of start.</param>
        /// <param name="startY">the position of start.</param>
        /// <param name="endX">the position of end.</param>
        /// <param name="endY">the position of end.</param>
        private void AddDashedLine(GraphicsPath path, float startX, float startY, float endX, float endY)
        {
            float repeatNum = (float)Math.Sqrt(Math.Pow(endX - startX, 2) + Math.Pow(endY - startY, 2)) / 8f;

            float xMovement = (endX - startX) / repeatNum;
            float yMovement = (endY - startY) / repeatNum;
            float xFragment = xMovement * 0.6f;
            float yFragment = yMovement * 0.6f;

            float presentX = startX;
            float presentY = startY;
            for (int i = 0; i < repeatNum; i++)
            {
                path.AddLine(presentX, presentY, presentX + xFragment, presentY + yFragment);
                path.CloseFigure();

                presentX += xMovement;
                presentY += yMovement;
            }
        }

        /// <summary>
        /// Get coordinates of an arrow head.
        /// </summary>
        /// <param name="arrowApex">an apex of an arrow</param>
        /// <param name="guidePoint">an arrow line goes direction from arrowApex to guidePoint</param>
        /// <returns></returns>
        internal static PointF[] GetArrowPoints(PointF arrowApex, PointF guidePoint)
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

        #region EventHandlers
        /// <summary>
        /// Called when the mouse up.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_canvas == null)
                return;
            m_canvas.NotifyResetSelect();
            m_canvas.FocusNode = this;
            m_canvas.LineHandler.AddSelectedLine(this);
        }
        #endregion
    }

    #region Enum
    /// <summary>
    /// Enumeration for a direction of a edge
    /// </summary>
    public enum EdgeDirection
    {
        /// <summary>
        /// Outward direction
        /// </summary>
        Outward,
        /// <summary>
        /// Inward direction
        /// </summary>
        Inward,
        /// <summary>
        /// Outward and inward direction
        /// </summary>
        Bidirection,
        /// <summary>
        /// An edge has no direction
        /// </summary>
        None
    }
    /// <summary>
    /// Enumeration of a type of a line.
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        Unknown,
        /// <summary>
        /// Solid line
        /// </summary>
        Solid,
        /// <summary>
        /// Dashed line
        /// </summary>
        Dashed
    }
    #endregion

    /// <summary>
    /// EdgeInfo contains all information for one edge.
    /// </summary>
    public class EdgeInfo
    {

        #region Fields

        /// <summary>
        /// Key of a process, an owner of this edge.
        /// </summary>
        protected string m_proKey;

        /// <summary>
        /// Key of a variable with which a process has an edge.
        /// </summary>
        protected string m_varKey;

        private List<EcellReference> m_refList = null;

        /// <summary>
        /// Direction of this edge.
        /// </summary>
        protected EdgeDirection m_direction = EdgeDirection.None;

        /// <summary>
        /// Type of a line of this edge.
        /// </summary>
        protected LineType m_type = LineType.Unknown;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EdgeInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="list"></param>
        /// <param name="er"></param>
        public EdgeInfo(string processKey, List<EcellReference> list, EcellReference er)
        {
            m_refList = list;
            bool bidir = CheckBidir(list, er);

            m_proKey = processKey;
            m_varKey = er.Key;
            // Set Relation
            int l_coef = er.Coefficient;
            if (bidir)
            {
                m_direction = EdgeDirection.Bidirection;
                m_type = LineType.Solid;
            }
            else if (l_coef < 0)
            {
                m_direction = EdgeDirection.Inward;
                m_type = LineType.Solid;
            }
            else if (l_coef == 0)
            {
                m_direction = EdgeDirection.None;
                m_type = LineType.Dashed;
            }
            else
            {
                m_direction = EdgeDirection.Outward;
                m_type = LineType.Solid;
            }

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processKey">The key of process.</param>
        /// <param name="er">The reference of EcellObject.</param>
        public EdgeInfo(string processKey, EcellReference er)
        {
            m_proKey = processKey;
            // Set Relation
            int coef = er.Coefficient;
            if (coef < 0)
            {
                m_direction = EdgeDirection.Inward;
                m_type = LineType.Solid;
            }
            else if (coef == 0)
            {
                m_direction = EdgeDirection.None;
                m_type = LineType.Dashed;
            }
            else
            {
                m_direction = EdgeDirection.Outward;
                m_type = LineType.Solid;
            }
            m_varKey = er.Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processKey"></param>
        /// <param name="varKey"></param>
        /// <param name="direction"></param>
        public EdgeInfo(string processKey, string varKey, EdgeDirection direction)
        {
            m_proKey = processKey;
            // Set Relation
            if (direction == EdgeDirection.Inward)
            {
                m_direction = EdgeDirection.Inward;
                m_type = LineType.Solid;
            }
            else if (direction == EdgeDirection.None)
            {
                m_direction = EdgeDirection.None;
                m_type = LineType.Dashed;
            }
            else if (direction == EdgeDirection.Outward)
            {
                m_direction = EdgeDirection.Outward;
                m_type = LineType.Solid;
            }
            else
            {
                m_direction = EdgeDirection.Bidirection;
                m_type = LineType.Solid;
            }
            m_varKey = varKey;
        }

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string EdgeKey
        {
            get { return m_varKey + ":" + m_direction.ToString(); }
        }
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string VariableKey
        {
            get { return m_varKey; }
            set { m_varKey = value; }
        }
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string ProcessKey
        {
            get { return m_proKey; }
            set { m_proKey = value; }
        }
        /// <summary>
        /// Accessor for m_direction.
        /// </summary>
        public EdgeDirection Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }
        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        public LineType LineType
        {
            get { return m_type; }
            set { m_type = value; }
        }
        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        public int Coefficient
        {
            get
            {
                int coefficient = 0;
                switch (this.m_direction)
                {
                    case EdgeDirection.Inward:
                        coefficient = -1;
                        break;
                    case EdgeDirection.None:
                        coefficient = 0;
                        break;
                    case EdgeDirection.Outward:
                        coefficient = 1;
                        break;
                }
                return coefficient;
            }
        }

        /// <summary>
        /// IsEndNode
        /// </summary>
        public bool IsEndNode
        {
            get
            {
                return CheckEndNode(m_refList);
            }
        }
        #endregion

        private static bool CheckBidir(List<EcellReference> list, EcellReference er)
        {
            bool bidir = false;
            foreach (EcellReference er1 in list)
            {
                if (er.Key.Equals(er1.Key) &&
                    er.Coefficient != 0 &&
                    er.Coefficient == -1 * er1.Coefficient)
                {
                    bidir = true;
                    break;
                }
            }
            return bidir;
        }

        private static bool CheckEndNode(List<EcellReference> list)
        {
            bool isEndNode = true;
            foreach (EcellReference er in list)
            {
                if (er.Coefficient != 1)
                    continue;
                isEndNode = false;
                break;
            }
            return isEndNode;
        }

    }
}
