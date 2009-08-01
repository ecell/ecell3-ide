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
//

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Plugin;
using System.Reflection;
using Ecell.Objects;
using System.Drawing;

namespace CBGridLayout
{
    /// <summary>
    /// 
    /// </summary>
    public class CBGridLayout : LayoutBase
    {
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public CBGridLayout()
        {
            m_panel = new CBGridLayoutPanel(this);
        }
        #endregion

        #region Inherited from ILayout
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetPluginName()
        {
            return "CBGridLayout";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subCommandNum"></param>
        /// <param name="layoutSystem"></param>
        /// <param name="systemList"></param>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        public override bool DoLayout(int subCommandNum, bool layoutSystem, List<Ecell.Objects.EcellObject> systemList, List<Ecell.Objects.EcellObject> nodeList)
        {

            float Kr = 10;
            float Ka = 0.001f;
            int iterations = 10;
            // Set references
            List<LayoutReference> references = new List<LayoutReference>();
            foreach (EcellObject obj in nodeList)
            {
                if (!(obj is EcellProcess))
                    continue;
                EcellProcess process = (EcellProcess)obj;
                foreach (EcellReference er in process.ReferenceList)
                {

                }
            }

            for (int i = 0; i < iterations; i++)
            {
                // calculate repulsive forces
                foreach (EcellObject node1 in nodeList)
                {
                    // each vertex has two vectors: pos and disp
                    node1.OffsetX = 0.0f;
                    node1.OffsetY = 0.0f;
                    foreach (EcellObject node2 in nodeList)
                    {
                        if (node1 == node2)
                            continue;
                        PointF delta = new PointF(node1.X - node2.X, node1.Y - node2.Y);
                        double r = Math.Sqrt(Math.Pow((double)delta.X, 2) + Math.Pow((double)delta.Y, 2));
                        node1.OffsetX += delta.X * Kr / (float)Math.Pow(r, 2);
                        node1.OffsetY += delta.Y * Kr / (float)Math.Pow(r, 2);
                    }

                    // calculate attractive force
                    foreach (LayoutReference lr in references)
                    {
                        PointF delta = lr.Delta;
                        double r = Math.Sqrt(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2));
                        lr.Process.OffsetX -= delta.X * Ka;
                        lr.Process.OffsetY -= delta.Y * Ka;
                        lr.Variable.OffsetX += delta.X * Ka;
                        lr.Variable.OffsetY += delta.Y * Ka;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override LayoutType GetLayoutType()
        {
            return LayoutType.Whole;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetLayoutName()
        {
            return "CBGrid";
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void Initialization()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetEdgeVertex()
        {
        }

        private void SetEdgeEdge()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetVertexEdge()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDistance()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public class LayoutReference
        {
            private EcellProcess _process;
            private EcellVariable _variable;
            /// <summary>
            /// 
            /// </summary>
            public EcellProcess Process
            {
                get { return _process; }
            }
            /// <summary>
            /// 
            /// </summary>
            public EcellVariable Variable
            {
                get { return _variable; }
            }
            /// <summary>
            /// 
            /// </summary>
            public PointF Delta
            {
                get
                {
                    PointF point1 = new PointF(_process.X + _process.OffsetX, _process.Y + _process.OffsetY);
                    PointF point2 = new PointF(_variable.X + _variable.OffsetX, _variable.Y + _variable.OffsetY);
                    return new PointF(point1.X - point2.X, point1.Y - point2.Y);
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public float Length
            {
                get
                {
                    PointF delta = this.Delta;
                    float length = (float)Math.Sqrt(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2));
                    return length;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="process"></param>
            /// <param name="variable"></param>
            public LayoutReference(EcellProcess process, EcellVariable variable)
            {
                _process = process;
                _variable = variable;
            }
        }
    }
}
