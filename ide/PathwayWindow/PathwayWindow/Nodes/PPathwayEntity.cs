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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System.Collections.Generic;
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using System;
using Ecell.IDE.Plugins.PathwayWindow.Handler;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Super class for piccolo object of variable, process, etc.
    /// </summary>
    public class PPathwayEntity : PPathwayObject
    {
        #region Fields(readonly)
        /// <summary>
        /// The default width of object.
        /// </summary>
        public const float DEFAULT_WIDTH = 60;
        /// <summary>
        /// The default height of object.
        /// </summary>
        public const float DEFAULT_HEIGHT = 40;
        #endregion

        #region Fields
        /// <summary>
        /// Properties panel for showing this object's ID.
        /// </summary>
        protected PPathwayProperties m_pProperty = null;
        /// <summary>
        /// 
        /// </summary>
        protected PPathwayGraph m_graph = null;
        /// <summary>
        /// list of relations.
        /// </summary>
        protected List<PPathwayEdge> m_edges = new List<PPathwayEdge>();

        /// <summary>
        /// Figure List
        /// </summary>
        protected IFigure m_tempFigure = null;
        #endregion

        #region Accessors
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
                foreach (PPathwayEdge edge in m_edges)
                {
                    edge.Selected = value;
                }
                base.Selected = value;
            }
        }
        /// <summary>
        /// RelatedProcesses
        /// </summary>
        public List<PPathwayEdge> Edges
        {
            get { return m_edges; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual PPathwayProperties Property
        {
            get { return m_pProperty; }
            set { m_pProperty = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual PPathwayGraph Graph
        {
            get { return m_graph; }
            set { m_graph = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// constructor for PPathwayNode.
        /// </summary>
        public PPathwayEntity()
        {
            this.Width = DEFAULT_WIDTH;
            this.Height = DEFAULT_HEIGHT;
            this.VisibleChanged += new PPropertyEventHandler(PPathwayNode_VisibleChanged);
            m_tempFigure = new EllipseFigure(0, 0, 10, 10);
            // PropertyText
            m_pProperty = new PPathwayProperties(this);
            m_pProperty.Pickable = false;
            this.AddChild(m_pProperty);
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// event on visibility change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PPathwayNode_VisibleChanged(object sender, PPropertyEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// event on double click this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnDoubleClick(PInputEventArgs e)
        {
            if (m_canvas == null)
                return;
            m_canvas.Control.Menu.SetCreateLineHandler(this);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Create new instance of this class.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayEntity();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void RefreshText()
        {
            base.RefreshText();
            //m_pProperty.X = this.Right;
            //m_pProperty.Y = this.Top;
        }

        /// <summary>
        /// calculate the point of contact this Node.
        /// </summary>
        /// <param name="refPoint">reference point.</param>
        /// <returns></returns>
        public virtual PointF GetContactPoint(PointF refPoint)
        {
            return m_figure.GetContactPoint(refPoint, CenterPointF);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="outer"></param>
        /// <returns></returns>
        public virtual PointF GetContactPoint(int index, PointF outer)
        {
            PointF point;
            PointF[] points = m_figure.ContactPoints;
            if (index >= points.Length)
            {
                point = GetContactPoint(outer);
            }
            else
            {
                point = points[index];
                point = new PointF(point.X + this.X + this.OffsetX, point.Y + this.Y + this.OffsetY);
            }
            return point;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual int GetConnectorIndex(PointF point)
        {
            int index = -1;
            point = new PointF(point.X - this.X - this.OffsetX, point.Y - this.Y - this.OffsetY);
            PointF[] points = m_figure.ContactPoints;
            int i = 0;
            double length = GetDistance(point, points[0]);
            index = 0;
            foreach (PointF pos in points)
            {
                double temp = GetDistance(pos, point);
                if (temp < length)
                {
                    length = temp;
                    index = i;
                }
                i++;
            }
            return index;
        }

        /// <summary>
        /// String expression of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + ", Text.X:" + PText.X + ", Text.Y:" + PText.Y
                + ", Text.OffsetX:" + PText.OffsetX + ", Text.OffsetY:" + PText.OffsetY;
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public override void Refresh()
        {
            foreach (PPathwayEdge edge in m_edges)
                edge.Refresh();
            base.Refresh();
            m_pProperty.X = this.X + this.Width;
            m_pProperty.Y = this.Y;
            if(m_pProperty.Visible)
                m_pProperty.Refresh();
        }

        /// <summary>
        /// Event on Dispose
        /// </summary>
        public override void Dispose()
        {
            // Clear Lines
            foreach (PPathwayEdge edge in m_edges.ToArray())
            {
                edge.RemoveFromParent();
                edge.Dispose();
            }
            m_edges.Clear();
            base.Dispose();
        }
        #endregion
    }
}