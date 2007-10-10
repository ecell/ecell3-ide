using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Nodes;
using System.Windows.Forms;
using System.Drawing;
using EcellLib.PathwayWindow;
using EcellLib.PathwayWindow.Nodes;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// Line
    /// </summary>
    public class Line : PPath
    {
        /// <summary>
        ///  Arrow design settings
        /// </summary>
        public static readonly float ARROW_RADIAN_A = 0.471f;

        /// <summary>
        ///  Arrow design settings
        /// </summary>
        public static readonly float ARROW_RADIAN_B = 5.812f;

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        public static readonly float ARROW_LENGTH = 15;

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
        /// <param name="edgeInfo"></param>
        public Line(EdgeInfo edgeInfo)
        {
            m_edgeInfo = edgeInfo;
        }

        /// <summary>
        /// highlight this line.
        /// </summary>
        public void highlight()
        {
            float width = this.Pen.Width;
            this.Pen = new Pen(Brushes.Orange, width);
            this.Brush = Brushes.Orange;
        }

        /// <summary>
        /// Turn off highlight status.
        /// </summary>
        public void unhighlight()
        {
            float width = this.Pen.Width;
            this.Pen = new Pen(Brushes.Black, width);
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
        /// <param name="type">LineType</param>
        public void DrawLine()
        {
            SetDirection();
            switch (this.Info.TypeOfLine)
            {
                case LineType.Solid:
                    this.Pen = new Pen(Brushes.Black, 2);
                    this.AddLine(this.ProPoint.X, this.ProPoint.Y, this.VarPoint.X, this.VarPoint.Y);
                    break;
                case LineType.Dashed:
                    this.Pen = new Pen(Brushes.Black, 3);
                    AddDashedLine(this, this.ProPoint.X, this.ProPoint.Y, this.VarPoint.X, this.VarPoint.Y);
                    break;
                case LineType.Unknown:
                    this.AddLine(this.ProPoint.X, this.ProPoint.Y, this.VarPoint.X, this.VarPoint.Y);
                    break;
            }
        }

        /// <summary>
        /// Draw Line.
        /// </summary>
        /// <param name="type">LineType</param>
        public void SetDirection()
        {
            switch (this.Info.Direction)
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
        public static void AddDashedLine(PPath path, float startX, float startY, float endX, float endY)
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
        /// <param name="arrowRadianA">angle of skirt of an arrow head</param>
        /// <param name="arrowRadianB">angle of skirt of an arrow head</param>
        /// <param name="arrowLength">length of an arrow head</param>
        /// <returns></returns>
        public static PointF[] GetArrowPoints(PointF arrowApex,
                                              PointF guidePoint)
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
    }
}
