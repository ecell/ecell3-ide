using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EcellLib.PathwayWindow.Figure
{
    class SystemRectangle : FigureBase
    {
        /// <summary>
        /// COnstructor without params.
        /// </summary>
        public SystemRectangle()
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
        public SystemRectangle(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height);
        }

        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public SystemRectangle(float[] vars)
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
            m_type = "SystemRectangle";
            float marginX1 = width * 0.05f;
            float marginY1 = height * 0.05f;
            m_gp.AddArc(x, y, marginX1 * 2, marginY1 * 2, 180, 90);
            m_gp.AddLine(x + marginX1, y, x + width - marginX1, y);
            m_gp.AddArc(x + width - marginX1 * 2, y, marginX1 * 2, marginY1 * 2, 270, 90);
            m_gp.AddLine(x + width, y + marginY1, x + width, y + height - marginY1);
            m_gp.AddArc(x + width - marginX1 * 2, y + height - marginY1 * 2, marginX1 * 2, marginY1 * 2, 0, 90);
            m_gp.AddLine(x + marginX1, y + height, x + width - marginX1, y + height);
            m_gp.AddArc(x, y + height - marginY1 * 2, marginX1 * 2, marginY1 * 2, 90, 90);
            m_gp.AddLine(x, y + marginY1, x, y + height - marginY1);
            m_gp.CloseFigure();

            float marginX2 = width * 0.025f;
            float marginY2 = height * 0.025f;
            m_gp.AddArc(x + marginX2, y + marginY2, marginX2 * 2, marginY2 * 2, 180, 90);
            m_gp.AddLine(x + marginX1, y + marginY2, x + width - marginX1, y + marginY2);
            m_gp.AddArc(x + width - marginX1 - marginX2, y + marginY2, marginX2 * 2, marginY2 * 2, 270, 90);
            m_gp.AddLine(x + width - marginX2, y + marginY1, x + width - marginX2, y + height - marginY1);
            m_gp.AddArc(x + width - marginX1 - marginX2, y + height - marginY1 - marginY2, marginX2 * 2, marginY2 * 2, 0, 90);
            m_gp.AddLine(x + width - marginX1, y + height - marginY2, x + marginX1, y + height - marginY2);
            m_gp.AddArc(x + marginX2, y + height - marginY1 - marginY2, marginX2 * 2, marginY2 * 2, 90, 90);
            m_gp.AddLine(x + marginX2, y + height - marginY1, x + marginX2, y + marginY1);

        }

        /// <summary>
        /// GetContactPoint
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF outerPoint, PointF innerPoint)
        {
            // Transform the coordinate system as the center of this rectangle is the original point
            // and this recntangle's width is 2.
            float a = m_width / 2;
            float b = m_height / 2;

            float x1 = innerPoint.X - a;
            float x2 = innerPoint.X + a;
            float y1 = innerPoint.Y - b;
            float y2 = innerPoint.Y + b;

            float x = 0;
            float y = 0;

            if (outerPoint.X <= x1)
                x = x1;
            else if (outerPoint.X >= x2)
                x = x2;
            else
                x = outerPoint.X;

            if (outerPoint.Y <= y1)
                y = y1;
            else if (outerPoint.Y >= y2)
                y = y2;
            else
                y = outerPoint.Y;

            return new PointF(x, y);
        }
    }
}
