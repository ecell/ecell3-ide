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

using System.Collections.Generic;
using System.Drawing;
using UMD.HCIL.Piccolo;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayLayer for CanvasControl
    /// </summary>
    public class PPathwayLayer : PLayer
    {
        #region Fields
        /// <summary>
        ///  Name of layer.
        /// </summary>
        private string m_name;
        #endregion
        #region Accessor
        /// <summary>
        ///  get/set Name text.
        /// </summary>
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                this.Pickable = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayLayer()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public PPathwayLayer(string name)
        {
            this.m_name = name;
            this.Visible = true;
            this.VisibleChanged += new PPropertyEventHandler(PPathwayLayer_VisibleChanged);
        }
        /// <summary>
        /// Change nodevisibility.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwayLayer_VisibleChanged(object sender, UMD.HCIL.Piccolo.Event.PPropertyEventArgs e)
        {
            foreach (PPathwayObject obj in this.GetNodes())
            {
                obj.Visible = this.Visible;
                obj.Pickable = this.Visible;
                if (!(obj is PPathwayEntity))
                    continue;

                PPathwayEntity node = (PPathwayEntity)obj;
                foreach (PPathwayEdge line in node.Edges)
                {
                    line.VisibleChange();
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the list of PPathwayObject under this layer.
        /// </summary>
        /// <returns></returns>
        public List<PPathwayObject> GetNodes()
        {
            List<PPathwayObject> list = new List<PPathwayObject>();
            foreach (PNode node in this.ChildrenReference)
            {
                if (!(node is PPathwayObject))
                    continue;
                list.Add((PPathwayObject)node);
            }
            return list;
        }

        /// <summary>
        /// Get the list of PPathwayObject under this layer.
        /// Especially
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public List<PPathwayObject> GetNodes(RectangleF rect)
        {
            List<PPathwayObject> list = new List<PPathwayObject>();
            foreach (PPathwayObject obj in GetNodes())
            {
                if (obj is PPathwayEntity && rect.Contains(obj.Center))
                    list.Add(obj);
                else if (rect.Contains(obj.Rect))
                    list.Add(obj);
            }
            return list;
        }
        #endregion
    }
}
