using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Nodes;
using System.Windows.Forms;
using EcellLib.PathwayWindow.Element;
using System.Drawing;

namespace PathwayWindow.UIComponent
{
    /// <summary>
    /// Line
    /// </summary>
    public class Line : PPath
    {
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

    }
}
