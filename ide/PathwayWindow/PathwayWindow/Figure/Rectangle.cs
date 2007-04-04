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

using System;
using System.Collections.Generic;
using System.Text;
using EcellLib.PathwayWindow.Figure;
using System.Drawing;
using EcellLib.PathwayWindow;

namespace EcellLib.PathwayWindow.Figure
{
    /// <summary>
    /// FigureBase for a rectangle
    /// </summary>
    public class RectangleFigure : FigureBase
    {
        /// <summary>
        /// X coordinate of this figure.
        /// </summary>
        protected float m_x;

        /// <summary>
        /// Y coordinate of this figure.
        /// </summary>
        protected float m_y;

        /// <summary>
        /// Width of this figure.
        /// </summary>
        protected float m_width;

        /// <summary>
        /// Height of this figure.
        /// </summary>
        protected float m_height;

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RectangleFigure(float x, float y, float width, float height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
        }

        /// <summary>
        /// Get contact point for this figure.
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF outerPoint, PointF innerPoint)
        {
            // Transform the coordinate system as the center of this rectangle is the original point
            // and this recntangle's width is 2.
            PointF centerPoint = new PointF(m_x + m_width / 2f, m_y + m_height / 2f);
            float xFactor = m_width / 2f;
            float yFactor = m_height / 2f;

            if (xFactor == 0 || yFactor == 0)
                return PointF.Empty;

            PointF transOuterPoint = new PointF((outerPoint.X - centerPoint.X) / xFactor,
                                                (outerPoint.Y - centerPoint.Y) / yFactor);
            PointF transInnerPoint = new PointF((innerPoint.X - centerPoint.X) / xFactor,
                                                (innerPoint.Y - centerPoint.Y) / yFactor);
            
            // Add all candidates for a contact point
            List<PointF> candidates = new List<PointF>();

            candidates.Add(new PointF(1f, 1f));
            candidates.Add(new PointF(1f, 0.5f));
            candidates.Add(new PointF(1f, 0f));
            candidates.Add(new PointF(1f, -0.5f));
            candidates.Add(new PointF(1f, -1f));
            candidates.Add(new PointF(0.5f, -1f));
            candidates.Add(new PointF(0f, -1f));
            candidates.Add(new PointF(-0.5f, -1f));
            candidates.Add(new PointF(-1f, -1f));
            candidates.Add(new PointF(-1f, -0.5f));
            candidates.Add(new PointF(-1f, 0f));
            candidates.Add(new PointF(-1f, 0.5f));
            candidates.Add(new PointF(-1f, 1f));
            candidates.Add(new PointF(-0.5f, 1f));
            candidates.Add(new PointF(0f, 1f));
            candidates.Add(new PointF(0.5f, 1f));

            // Calculate distances between a candidate point and transOuterPoint.
            // Then pick up a point which has minimum distance and a point which has
            // second minimum distance.
            float minDistance = 0;
            PointF minPoint = PointF.Empty;
            float secondMinDistance = 0;
            PointF secondMinPoint = PointF.Empty;

            foreach (PointF candP in candidates)
            {
                float distance = PathUtil.GetDistance(transOuterPoint, candP);

                if (minPoint == PointF.Empty || distance < minDistance)
                {
                    secondMinDistance = minDistance;
                    secondMinPoint = minPoint;
                    minDistance = distance;
                    minPoint = candP;
                }
                else if (secondMinPoint == PointF.Empty || distance < secondMinDistance)
                {
                    secondMinDistance = distance;
                    secondMinPoint = candP;
                }
            }

            // Calculate the sum of the following two distances about above selected two points
            //  the distance between the point and transOuterPoint
            //  the distance between the point and transInnerPoint
            // Then the point which has smaller sum will be contact point
            float minPointSum = PathUtil.GetDistance(transOuterPoint, minPoint)
                                + PathUtil.GetDistance(transInnerPoint, minPoint);
            float secondPointSum = PathUtil.GetDistance(transOuterPoint, secondMinPoint)
                                + PathUtil.GetDistance(transInnerPoint, secondMinPoint);

            PointF contactPoint;

            if (minPointSum <= secondPointSum)
                contactPoint = minPoint;
            else
                contactPoint = secondMinPoint;

            // Transform the coordinate system to the default state.
            contactPoint.X = contactPoint.X * xFactor + centerPoint.X;
            contactPoint.Y = contactPoint.Y * yFactor + centerPoint.Y;
            
            return contactPoint;
        }
    }
}
