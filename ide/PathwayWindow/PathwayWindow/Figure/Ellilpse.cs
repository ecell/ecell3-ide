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
using System.Drawing;

namespace EcellLib.PathwayWindow.Figure
{
    /// <summary>
    /// FigureBase for an ellipse.
    /// </summary>
    public class EllipseFigure : FigureBase
    {
        #region Constructors
        /// <summary>
        /// Constructor with no params.
        /// </summary>
        public EllipseFigure()
        {
            Initialize(0, 0, 1, 1);
        }
        /// <summary>
        /// Constructor with params
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public EllipseFigure(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height);
        }
        /// <summary>
        /// Constructor with float array.
        /// </summary>
        /// <param name="vars"></param>
        public EllipseFigure(float[] vars)
        {
            if (vars.Length >= 4)
                Initialize(vars[0], vars[1], vars[2], vars[3]);
            else
                Initialize(0, 0, 1, 1);
        }

        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void Initialize(float x, float y, float width, float height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
            m_type = "Ellipse";
            RectangleF rect = new RectangleF(x, y, width, height);
            m_gp.AddEllipse(rect);
        }
        #endregion

        #region Inherited
        /// <summary>
        /// Get contact point for this figure.
        /// </summary>
        /// <param name="outerPoint"></param>
        /// <param name="innerPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF outerPoint, PointF innerPoint)
        {
            // Transform the coordinate system as the center of this ellipse is the original point
            // and this ellipse's radius is 1.
            PointF centerPoint = new PointF(m_x + m_width / 2f, m_y + m_height / 2f);
            float xFactor = m_width / 2f;
            float yFactor = m_height / 2f;

            if (xFactor == 0 || yFactor == 0)
                return PointF.Empty;

            PointF transOuterPoint = new PointF((outerPoint.X - centerPoint.X) / xFactor,
                                                (outerPoint.Y - centerPoint.Y) / yFactor);
            PointF transInnerPoint = new PointF((innerPoint.X - centerPoint.X) / xFactor,
                                                (innerPoint.Y - centerPoint.Y) / yFactor);

            // Add all candidates for a contact point excepts those in the same quadrant as innerPoint
            List<PointF> candidates = new List<PointF>();
            
            candidates.Add(new PointF(1, 0) );
            candidates.Add(new PointF(0.92f, -0.38f));
            candidates.Add(new PointF(0.70f, -0.70f));
            candidates.Add(new PointF(0.38f, -0.92f));
            candidates.Add(new PointF(0, -1));
            candidates.Add(new PointF(-0.38f, -0.92f));
            candidates.Add(new PointF(-0.70f, -0.70f));
            candidates.Add(new PointF(-0.92f, -0.38f));
            candidates.Add(new PointF(-1, 0));
            candidates.Add(new PointF(-0.92f, 0.38f));
            candidates.Add(new PointF(-0.70f, 0.70f));
            candidates.Add(new PointF(-0.38f, 0.92f));
            candidates.Add(new PointF(0, 1));
            candidates.Add(new PointF(0.38f, 0.92f));
            candidates.Add(new PointF(0.70f, 0.70f));
            candidates.Add(new PointF(0.92f, 0.38f));

            // Calculate distances between a candidate point and transOuterPoint.
            // Then pick up a point which has minimum distance and a point which has
            // second minimum distance.
            float minDistance = 0;
            PointF minPoint = PointF.Empty;
            float secondMinDistance = 0;
            PointF secondMinPoint = PointF.Empty;
                        
            foreach(PointF candP in candidates)
            {
                float distance = PathUtil.GetDistance(transOuterPoint, candP);

                if(minPoint == PointF.Empty || distance < minDistance)
                {
                    secondMinDistance = minDistance;
                    secondMinPoint = minPoint;
                    minDistance = distance;
                    minPoint = candP;
                }
                else if(secondMinPoint == PointF.Empty || distance < secondMinDistance)
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
        #endregion

    }
}