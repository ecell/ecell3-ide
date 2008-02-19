using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EcellLib.PathwayWindow.Figure
{
    class RoundedRectangle : FigureBase
    {
        /// <summary>
        /// COnstructor without params.
        /// </summary>
        public RoundedRectangle()
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
        public RoundedRectangle(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height);
        }

        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public RoundedRectangle(float[] vars)
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
            m_type = "RoundedRectangle";
            float marginX = width * 0.05f;
            float marginY = height * 0.05f;
            m_gp.AddArc(x, y, marginX * 2, marginY * 2, 180, 90);
            m_gp.AddLine(x + marginX, y, x + width - marginX, y);
            m_gp.AddArc(x + width - marginX * 2, y, marginX * 2, marginY * 2, 270, 90);
            m_gp.AddLine(x + width, y + marginY, x + width, y + height -marginY);
            m_gp.AddArc(x + width - marginX * 2, y + height - marginY * 2, marginX * 2, marginY * 2, 0, 90);
            m_gp.AddLine(x + marginX, y + height, x + width - marginX, y + height);
            m_gp.AddArc(x, y + height - marginY * 2, marginX * 2, marginY * 2, 90, 90);
            m_gp.AddLine(x, y + marginY, x, y + height -marginY);
        }
    }
}
