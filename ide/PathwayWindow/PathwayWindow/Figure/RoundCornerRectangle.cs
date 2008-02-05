using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EcellLib.PathwayWindow.Figure
{
    class RoundCornerRectangle : FigureBase
    {
        /// <summary>
        /// COnstructor without params.
        /// </summary>
        public RoundCornerRectangle()
        {
            Initialize(0, 0, 1, 1);
        }

        /// <summary>
        /// Constructor with params.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RoundCornerRectangle(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height);
        }

        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public RoundCornerRectangle(float[] vars)
        {
            if (vars.Length >= 4)
                Initialize(vars[0], vars[1], vars[2], vars[3]);
            else
                Initialize(0, 0, 1, 1);
        }

        private void Initialize(float x, float y, float width, float height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
            m_type = "RoundCornerRectangle";
            RectangleF rect = new RectangleF(x, y, width, height);
            m_gp.AddRectangle(rect);
        }

        /// <summary>
        /// GetContactPoint
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF outerPoint, PointF innerPoint)
        {
            return innerPoint;
        }
    }
}
