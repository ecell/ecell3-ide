//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.CBGridLayout
{
    /// <summary>
    /// 
    /// </summary>
    public class CBGridLayout : LayoutBase
    {
        #region Fields
        private float m_kr = 200f;
        private float m_ka = 0.01f;
        private float m_margin = 60f;
        private int m_iteration = 100;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public CBGridLayout()
        {
            m_panel = new CBGridLayoutPanel(this);
        }
        #endregion

        #region Accessors
        public float Kr
        {
            get { return m_kr; }
            set { this.m_kr = value; }
        }

        public float Ka
        {
            get { return m_ka; }
            set { this.m_ka = value; }
        }

        public int Iteration
        {
            get { return m_iteration; }
            set { this.m_iteration = value; }
        }

        public float Margin
        {
            get { return m_margin; }
            set { this.m_margin = value; }
        }
        #endregion

        #region Inherited from ILayout
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            ToolStripMenuItem layoutMenu = new ToolStripMenuItem();
            layoutMenu.Name = MenuConstants.MenuItemLayout;

            ToolStripMenuItem algoMenu = new ToolStripMenuItem(
                MessageResGridLayout.MenuItemGrid,
                null,
                new EventHandler(delegate(object o, EventArgs e)
                {
                    m_env.PluginManager.DiagramEditor.InitiateLayout(this, 0);
                })
            );

            algoMenu.ToolTipText = MessageResGridLayout.ToolTip;
            layoutMenu.DropDownItems.Add(algoMenu);
            return new ToolStripMenuItem[] { layoutMenu };
        }
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
            Dictionary<string, EcellVariable> varDic = new Dictionary<string, EcellVariable>();
            foreach (EcellObject obj in nodeList)
            {
                if (!(obj is EcellVariable))
                    continue;
                EcellVariable variable = (EcellVariable)obj;
                varDic.Add(variable.Key, variable);
            }

            // Set references
            List<LayoutReference> references = new List<LayoutReference>();
            foreach (EcellObject obj in nodeList)
            {
                if (!(obj is EcellProcess))
                    continue;
                EcellProcess process = (EcellProcess)obj;
                foreach (EcellReference er in process.ReferenceList)
                {
                    EcellVariable variable = null;
                    varDic.TryGetValue(er.Key, out variable);
                    if (variable == null)
                        continue;

                    LayoutReference lr = new LayoutReference(process, variable, er.Coefficient);
                    references.Add(lr);
                }
            }

            for (int i = 0; i < m_iteration; i++)
            {
                // calculate repulsive & attractive forces
                foreach (EcellObject node1 in nodeList)
                {
                    // each vertex has two vectors: pos and disp
                    node1.OffsetX = 0.0f;
                    node1.OffsetY = 0.0f;
                    // calculate repulsive forces
                    foreach (EcellObject node2 in nodeList)
                    {
                        if (node1 == node2)
                            continue;
                        PointF delta = new PointF(node1.X - node2.X, node1.Y - node2.Y);
                        double r = Math.Sqrt(Math.Pow((double)delta.X, 2) + Math.Pow((double)delta.Y, 2));
                        node1.OffsetX += delta.X * m_kr / (float)Math.Pow(r, 2);
                        node1.OffsetY += delta.Y * m_kr / (float)Math.Pow(r, 2);
                    }

                    // calculate attractive force
                    foreach (LayoutReference lr in references)
                    {
                        //double f = ()
                        PointF delta = lr.Delta;
                        double r = Math.Sqrt(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2));
                        lr.Process.OffsetX -= delta.X * m_ka;
                        lr.Process.OffsetY -= delta.Y * m_ka;
                        lr.Variable.OffsetX += delta.X * m_ka;
                        lr.Variable.OffsetY += delta.Y * m_ka;
                    }
                }
            }
            // Set Layout
            foreach (EcellObject node in nodeList)
            {
                node.X = node.X + node.OffsetX;
                node.Y = node.Y + node.OffsetY;
                node.OffsetX = 0;
                node.OffsetY = 0;
            }
            // Set Grid
            SetGrid(nodeList, systemList, m_margin);
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
        /// a function that calculates the costs 
        /// from the sum of the vertex-edge crossings between ev ∈ E'v ⊂ Ev and
        /// v' ∈ V' for all the mappings of v to a point.
        /// </summary>
        private void SetEdgeEdge()
        {
        }

        /// <summary>
        /// a function that calculates the costs
        /// from the sum of the vertex-edge crossings between ev ∈ E'v ⊂ Ev and 
        /// v' ∈ V' for all the mappings of v to a point.
        /// </summary>
        private void SetEdgeVertex()
        {
        }

        /// <summary>
        /// a function that calculates the costs from
        /// the sum of the vertex-edge crossings between v and e ∈ E for all the
        /// mappings of v to a point.
        /// </summary>
        private void SetVertexEdge()
        {
        }

        /// <summary>
        ///  a function that calculates the sum of the distance costs between
        ///  v and v ∈ V  for all the mappings of v to a point.
        /// </summary>
        private float SetDistance(EcellObject obj, List<EcellObject> list)
        {
            float distance = 0;
            foreach(EcellObject eo in list)
            {

            }
            return distance;
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

            private int _coefficient;
            /// <summary>
            /// 
            /// </summary>
            public int Coefficient
            {
                get { return _coefficient; }
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
            /// <param name="coefficient"></param>
            public LayoutReference(EcellProcess process, EcellVariable variable, int coefficient)
            {
                _process = process;
                _variable = variable;
                _coefficient = coefficient;
            }
        }
    }
}
