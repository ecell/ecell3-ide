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

        public EdgeInfo Info
        {
            get { return m_edgeInfo; }
            set { this.m_edgeInfo = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="edgeInfo"></param>
        public Line(EdgeInfo edgeInfo)
        {
            m_edgeInfo = edgeInfo;
        }

        public void highlight()
        {
            float width = this.Pen.Width;
            this.Pen = new Pen(Brushes.Orange, width);
            this.Brush = Brushes.Orange;
        }

        public void unhighlight()
        {
            float width = this.Pen.Width;
            this.Pen = new Pen(Brushes.Black, width);
            this.Brush = Brushes.Black;
        }

        public override void SetOffset(float x, float y)
        {
            //base.SetOffset(x, y);
        }

    }
}
