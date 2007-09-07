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
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;
using EcellLib.PathwayWindow.Element;
using UMD.HCIL.Piccolo.Nodes;
using System.Drawing;
using EcellLib.PathwayWindow.Figure;

namespace EcellLib.PathwayWindow.Node
{
    /// <summary>
    /// Super class for piccolo object of variable, process, etc.
    /// </summary>
    public class PPathwayNode : PPathwayObject
    {
        #region Fields(readonly)
        /// <summary>
        /// Font size for text.
        /// </summary>
        protected static readonly int m_nodeTextFontSize = 10;
        public static readonly float Width = 60;
        public static readonly float Height = 40;
        #endregion

        #region Fields
        /// <summary>
        /// PText for showing this node's ID on it.
        /// </summary>
        protected PText m_idText;

        /// <summary>
        /// list of figure.
        /// </summary>
        protected List<FigureBase> m_figureList;

        /// <summary>
        /// Whether this node is showing ID or not.
        /// </summary>
        protected bool m_showingId;

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
        /// system have this node.
        /// </summary>
        protected PEcellSystem m_system;

        /// <summary>
        /// handler for line
        /// </summary>
        protected PInputEventHandler m_handler4Line;
        #endregion

        #region Accessors
        /// <summary>
        /// get/set the position of center.
        /// </summary>
        public PointF CenterPoint
        {
            get
            {
                PointF returnP = new PointF(base.X + base.Width / 2f,
                                      base.Y + base.Height / 2f);

                if (this.Parent != null && this.Parent is PEcellComposite)
                {
                    returnP.X += base.Parent.OffsetX;
                    returnP.Y += base.Parent.OffsetY;
                }
                else
                {
                    returnP.X += base.OffsetX;
                    returnP.Y += base.OffsetY;
                }

                return returnP;
            }
        }

        /// <summary>
        /// Accessor for handler for edge line.
        /// </summary>
        public PInputEventHandler Handler4Line
        {
            get { return this.m_handler4Line; }
            set { this.m_handler4Line = value; }
        }
        
        /// <summary>
        /// get the position of center at canvas.
        /// </summary>
        public PointF CenterPointToCanvas
        {
            get
            {
                PNode dummyParent = null;
                PointF canPos = new PointF(base.X + base.Width / 2f + base.OffsetX,
                                        base.Y + base.Height / 2f + base.OffsetY);
                
                do
                {
                    if (dummyParent == null)
                        dummyParent = this.Parent;
                    else
                        dummyParent = dummyParent.Parent;
                    if (dummyParent != null)
                    {
                        canPos.X += dummyParent.OffsetX;
                        canPos.Y += dummyParent.OffsetY;
                    }
                } while (dummyParent != null && dummyParent.Parent != this.Root);
                return canPos;
            }
        }

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
                    this.Brush = m_normalBrush;
            }
        }

        /// <summary>
        /// Accessor for status whether the mouse is on this node or not.
        /// </summary>
        public virtual bool IsMouseOn
        {
            get { return this.m_isMouseOn; }
            set
            {
                this.m_isMouseOn = value;
                if (value)
                    this.Brush = m_toBeConnectedBrush;
                else if (!m_isToBeConnected)
                    this.Brush = m_normalBrush;
            }
        }

        /// <summary>
        /// get/set whether is shown ID.
        /// </summary>
        public bool ShowingID
        {
            get { return m_showingId; }
            set
            {
                m_showingId = value;
                if (m_showingId)
                    m_idText.Visible = true;
                else
                    m_idText.Visible = false;
            }
        }

        /// <summary>
        /// get/set related element.
        /// </summary>
        public new virtual NodeElement Element
        {
            get { return (NodeElement)base.m_element; }
            set
            {
                base.m_element = value;
                base.X = this.Element.X; //-this.Bounds.Width / 2;
                base.Y = this.Element.Y;// -this.Bounds.Height / 2;
                base.OffsetX = 0;
                base.OffsetY = 0;
//                Refresh();
                RefreshText();
                this.m_idText.MoveToFront();           
            }
        }

        /// <summary>
        /// get/set the list of figure.
        /// </summary>
        public virtual List<FigureBase> FigureList
        {
            get { return m_figureList; }
            set { m_figureList = value; }
        }

        /// <summary>
        /// get/set the parent system.
        /// </summary>
        public virtual PEcellSystem ParentSystem
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
            m_idText = new PText();
            m_idText.Pickable = false;
            m_idText.Font = new Font("Gothics", m_nodeTextFontSize, System.Drawing.FontStyle.Bold);
            this.AddChild(m_idText);
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void Delete()
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        /// <param name="highlight"></param>
        /// <returns></returns>
        public override bool HighLighted(bool highlight)
        {
            return true;
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void Initialize()
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        /// <param name="ecellObj"></param>
        public override void DataChanged(EcellObject ecellObj)
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void DataDeleted()
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void SelectChanged()
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void Start()
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void Change()
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void Stop()
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void End()
        {
        }
        /// <summary>
        /// Get a list of PathwayElement which are related to this node.
        /// </summary>
        /// <returns></returns>
        public override List<PathwayElement> GetElements()
        {
            return new List<PathwayElement>();
        }
        /// <summary>
        /// Create new instance of this class.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayNode();
        }
        /// <summary>
        /// Add related node.
        /// </summary>
        /// <param name="node"></param>
        public virtual void AddRelatedNode(PPathwayNode node)
        {
        }
        /// <summary>
        /// Memorize current position of this node.
        /// </summary>
        public override void MemorizeCurrentPosition()
        {
            base.m_originalX = base.X;
            base.m_originalY = base.Y;
            base.m_originalOffsetX = base.OffsetX;
            base.m_originalOffsetY = base.OffsetY;
        }
        /// <summary>
        /// Return this node to a memorized position.
        /// </summary>
        public override void ReturnToMemorizedPosition()
        {
            base.X = base.m_originalX;
            base.Y = base.m_originalY;
            base.OffsetX = base.m_originalOffsetX;
            base.OffsetY = base.m_originalOffsetY;
            this.Refresh();
        }
        /// <summary>
        /// Called when the mouse enters this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseEnter(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseEnter(e);
            if(null != base.m_set)
            {
                base.m_set.NotifyMouseEnter(this.Element);
            }
        }
        /// <summary>
        /// Called when the mouse leaves this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseLeave(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseLeave(e);
            if (null != base.m_set)
            {
                base.m_set.NotifyMouseLeave();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// set the offset of this node.
        /// </summary>
        /// <param name="dx">x of offset.</param>
        /// <param name="dy">y of offset.</param>
        public override void OffsetBy(float dx, float dy)
        {
            base.OffsetBy(dx, dy);
        }

        /// <summary>
        /// clear the offset in all parent objects.
        /// </summary>
        public override void CancelAllParentOffsets()
        {
            PNode dummyParent = null;
            do
            {
                if (dummyParent == null)
                    dummyParent = this.Parent;
                else
                    dummyParent = dummyParent.Parent;
                this.X += dummyParent.OffsetX;
                this.Y += dummyParent.OffsetY;
                m_idText.X += dummyParent.OffsetX;
                m_idText.Y += dummyParent.OffsetY;

            } while (dummyParent != this.Root);
        }

        /// <summary>
        /// calculate the point of contact this Node.
        /// </summary>
        /// <param name="refPoint">reference point.</param>
        /// <returns></returns>
        public PointF GetContactPoint(PointF refPoint)
        {
            if (base.m_setting == null)
                return PointF.Empty;

            List<FigureBase> figureList = base.m_setting.FigureList;
            if (figureList == null)
                return PointF.Empty;

            PointF originalPoint = new PointF(0f,0f);
            PointF centerPoint = this.CenterPointToCanvas;
            
            refPoint.X -= centerPoint.X;
            refPoint.Y -= centerPoint.Y;

            float minDistance = 0;
            PointF minContactPoint = PointF.Empty;
            foreach(FigureBase figureBase in figureList)
            {
                PointF candPoint = figureBase.GetContactPoint(refPoint, originalPoint);
                float distance = PathUtil.GetDistance(refPoint, candPoint)
                                + PathUtil.GetDistance(originalPoint, candPoint);
                if(minContactPoint == PointF.Empty || distance < minDistance)
                {
                    minContactPoint = candPoint;
                    minDistance = distance;
                }
            }

            minContactPoint.X += centerPoint.X;
            minContactPoint.Y += centerPoint.Y;

            return minContactPoint;
        }

        /// <summary>
        /// reconstruct the information of edge.
        /// </summary>
        public virtual void ValidateEdges()
        {            
        }

        /// <summary>
        /// redraw the text of this Node.
        /// </summary>
        public virtual void RefreshText()
        {
            m_idText.Text = this.Element.Text;
            RectangleF rect = base.bounds;
            this.Element.X = rect.X + rect.Width / 2f;
            this.Element.Y = rect.Y + rect.Height / 2f;
            m_idText.CenterBoundsOnPoint(this.Element.X,
                                         this.Element.Y);
        }

        /// <summary>
        /// String expression of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + ", m_idText.X:" + m_idText.X + ", m_idText.Y:" + m_idText.Y
                + ", m_idText.OffsetX:" + m_idText.OffsetX + ", m_idText.OffsetY:" + m_idText.OffsetY;
        }
        #endregion
    }
}