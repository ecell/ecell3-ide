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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib.PathwayWindow.Figure;
using EcellLib.Objects;
using EcellLib.PathwayWindow.Graphic;

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// Super class for piccolo object of variable, process, etc.
    /// </summary>
    public class PPathwayNode : PPathwayObject
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
        /// Object will be painted with this Brush when object is to be connected.
        /// </summary>
        protected Brush m_toBeConnectedBrush = Brushes.Orange;

        /// <summary>
        /// Whether this object is to be connected or not.
        /// </summary>
        protected bool m_isToBeConnected = false;

        /// <summary>
        /// Whether the mouse in on this node or not.
        /// </summary>
        protected bool m_isMouseOn = false;

        /// <summary>
        /// PText for showing this object's ID.
        /// </summary>
        protected PText m_pPropertyText;

        /// <summary>
        /// system have this node.
        /// </summary>
        protected PPathwaySystem m_system;

        /// <summary>
        /// Figure List
        /// </summary>
        protected IFigure m_tempFigure = null;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for status whether this object is ready to be connected.
        /// </summary>
        public virtual bool IsToBeConnected
        {
            get { return this.m_isToBeConnected; }
            set
            {
                this.m_isToBeConnected = value;
                if (value)
                    this.Brush = m_toBeConnectedBrush;
                else
                    this.Brush = m_fillBrush;
            }
        }

        /// <summary>
        /// get/set related element.
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return base.m_ecellObj; }
            set
            {   
                base.EcellObject = value;
                Refresh();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual PText PPropertyText
        {
            get { return m_pPropertyText; }
            set { m_pPropertyText = value; }
        }

        /// <summary>
        /// get/set the parent system.
        /// </summary>
        public virtual PPathwaySystem ParentSystem
        {
            get { return m_system; }
            set { m_system = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// constructor for PPathwayNode.
        /// </summary>
        public PPathwayNode()
        {
            this.Width = DEFAULT_WIDTH;
            this.Height = DEFAULT_HEIGHT;
            this.VisibleChanged += new PPropertyEventHandler(PPathwayNode_VisibleChanged);
            m_tempFigure = new EllipseFigure(-5, -5, 10, 10);
            // PropertyText
            m_pPropertyText = new PText();
            m_pPropertyText.Pickable = false;
            this.AddChild(m_pPropertyText);
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
        /// the event sequence of selecting the PNode of process or variable in PathwayEditor.
        /// </summary>
        /// <param name="e">PInputEventArgs.</param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            if (e.Modifiers == Keys.Shift || m_isSelected)
                m_canvas.NotifyAddSelect(this, true);
            else
                m_canvas.NotifySelectChanged(this);
            base.OnMouseDown(e);
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
            return new PPathwayNode();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void RefreshText()
        {
            base.RefreshText();
            m_pPropertyText.X = base.X + 5;
            m_pPropertyText.Y = base.Y - 15;
        }

        /// <summary>
        /// calculate the point of contact this Node.
        /// </summary>
        /// <param name="refPoint">reference point.</param>
        /// <returns></returns>
        public PointF GetContactPoint(PointF refPoint)
        {
            // Set Figure List
            if (base.m_setting == null)
                return base.CenterPointF;
            IFigure figure;
            if (m_isViewMode && this is PPathwayProcess)
                figure = m_tempFigure;
            else
                figure = base.m_figure;
            if (figure == null)
                return base.CenterPointF;

            return figure.GetContactPoint(refPoint, CenterPointF);
        }

        /// <summary>
        /// reconstruct the information of edge.
        /// </summary>
        public virtual void ValidateEdges()
        {            
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
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CreateSVGObject()
        {
            string obj = base.CreateSVGObject();
            PText text = m_pPropertyText;
            if (!string.IsNullOrEmpty(text.Text) && text.Visible)
            {
                PointF pos = new PointF(text.X, text.Y + text.Height);
                obj += SVGUtil.Text(pos, text.Text, BrushManager.ParseBrushToString(text.TextBrush), "", text.Font.Size);
            }
            return obj;
        }

        #endregion
    }
}