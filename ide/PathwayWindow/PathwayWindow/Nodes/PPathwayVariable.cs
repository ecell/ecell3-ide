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
//
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.

using System;
using System.Collections.Generic;
using System.Drawing;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Subclass of PPathwayNode for variable of E-Cell.
    /// </summary>
    public class PPathwayVariable : PPathwayEntity
    {
        #region Field.
        /// <summary>
        /// Aliase
        /// </summary>
        private List<PPathwayAlias> m_aliases = new List<PPathwayAlias>();
        #endregion

        #region Accessors
        /// <summary>
        /// get/set the related element.
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return base.m_ecellObj; }
            set
            {
                base.EcellObject = value;
                ResetAlias();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                foreach (PPathwayAlias alias in m_aliases)
                {
                    alias.Selected = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<PPathwayAlias> Aliases
        {
            get { return m_aliases; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public PPathwayVariable()
        {
        }

        /// <summary>
        /// create new PPathwayVariable.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayVariable();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            // Clear Aliases
            foreach (PPathwayAlias alias in m_aliases)
            {
                alias.RemoveFromParent();
                alias.Dispose();
            }
            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="outer"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(int index, PointF outer)
        {
            PointF point;
            int aliasIndex = (int)(index / 10) - 1;
            if (index < 10 || aliasIndex < 0)
            {
                point = base.GetContactPoint(index, outer);
            }
            else if (m_aliases.Count < aliasIndex + 1)
            {
                point = base.GetContactPoint(outer);
            }
            else
            {
                PPathwayAlias alias = m_aliases[aliasIndex];
                point = alias.GetContactPoint(index % 10, outer);
            }
            return point;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF refPoint)
        {
            // Get base pointer.
            PointF contactPoint = base.GetContactPoint(refPoint);
            if (m_aliases.Count <= 0)
                return contactPoint;

            // Get Alias pointer.
            double length = GetDistance(refPoint, contactPoint);
            double tempLength;
            PointF tempPoint;
            foreach (PPathwayAlias alias in m_aliases)
            {
                if (!alias.Visible)
                    continue;
                tempPoint = m_figure.GetContactPoint(refPoint, alias.CenterPointF);
                tempLength = GetDistance(refPoint, tempPoint);
                if (tempLength < length)
                {
                    length = tempLength;
                    contactPoint = tempPoint;
                }
            }
            return contactPoint;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetAlias()
        {
            if (m_ecellObj == null)
                return;
            // Remove current alias
            foreach (PPathwayAlias alias in m_aliases)
                alias.RemoveFromParent();
            m_aliases.Clear();

            // Set alias
            EcellVariable variable = (EcellVariable)m_ecellObj;
            foreach (EcellLayout layout in variable.Aliases)
            {
                PPathwayAlias alias = new PPathwayAlias(this);
                alias.X = layout.X;
                alias.Y = layout.Y;
                alias.Brush = m_setting.CreateBrush(alias.Path);
                alias.Refresh();
                m_canvas.SetLayer(alias, layout.Layer);
                m_aliases.Add(alias);
            }
            // Set Edge.
            foreach (PPathwayEdge edge in m_edges)
            {
                edge.Refresh();
            }
        }
    }
}