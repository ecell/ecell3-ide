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
using EcellLib.PathwayWindow.Element;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;

namespace EcellLib.PathwayWindow.Node
{
    /// <summary>
    /// PSystem is a subclass of PPathwayObject for a system.
    /// </summary>
    public class PSystem : PPathwayObject
    {
        #region Fields
        /// <summary>
        /// Indicate whether this PSystem is valid or not.
        /// </summary>
        protected bool m_valid = true;

        /// <summary>
        /// Whether this system is showing ID or not.
        /// </summary>
        protected bool m_showingId;

        /// <summary>
        /// Brush for drawing back ground.
        /// </summary>
        protected Brush m_backBrush = Brushes.White;

        /// <summary>
        /// For memorizing a width and a height before the start of a dragging.
        /// When the dragging failed, this object will be set to this width
        /// </summary>
        protected float m_originalWidth = 0;

        /// <summary>
        /// For memorizing a width and a height before the start of a dragging.
        /// When the dragging failed, this object will be set to this width
        /// </summary>
        protected float m_originalHeight = 0;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_systemElement.
        /// </summary>
        public new SystemElement Element
        {
            get { return (SystemElement)base.m_element; }
            set { base.m_element = value; }
        }

        /// <summary>
        /// Accessor for x coordinates of memorized position.
        /// </summary>
        public virtual float OriginalX
        {
            get { return m_originalX; }
        }

        /// <summary>
        /// Accessor for y coordinates of memorized position.
        /// </summary>
        public virtual float OriginalY
        {
            get { return m_originalY; }
        }

        /// <summary>
        /// Accessor for m_valid.
        /// </summary>
        public virtual bool Valid
        {
            get { return m_valid; }
            set { m_valid = value; }
        }

        /// <summary>
        /// Accessor for m_backBrush.
        /// </summary>
        public virtual Brush BackgroundBrush
        {
            get { return m_backBrush; }
            set { m_backBrush = value; }
        }

        /// <summary>
        /// Accessor for offset of this system to a layer.
        /// </summary>
        public PointF OffsetToLayer
        {
            get
            {
                if (base.Parent is PLayer)
                    return Offset;
                PointF OffsetToLayer = base.Offset;
                PNode dummyParent = null;
                do
                {
                    if (dummyParent == null)
                        dummyParent = base.Parent;
                    else
                        dummyParent = dummyParent.Parent;
                    if (dummyParent != null)
                    {
                        OffsetToLayer.X = OffsetToLayer.X + dummyParent.OffsetX;
                        OffsetToLayer.Y = OffsetToLayer.Y + dummyParent.OffsetY;
                    }
                } while (!(dummyParent.Parent is PLayer));
                return OffsetToLayer;
            }
        }

        /// <summary>
        /// acessor for a rectangle of this system.
        /// </summary>
        public virtual RectangleF Rect
        {
            get
            {
                return new RectangleF(base.X + this.OffsetToLayer.X, 
                                      base.Y + this.OffsetToLayer.Y,
                                      base.Width,
                                      base.Height);
            }
        }
        #endregion

        #region nouse
        /// <summary>
        /// Update position of resize handles.
        /// </summary>
        public virtual void UpdateResizeHandlePositions()
        {
        }
        /// <summary>
        /// Inherited method.
        /// </summary>
        public override void Delete()
        {
        }
        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="highlight"></param>
        /// <returns></returns>
        public override bool HighLighted(bool highlight)
        {
            return true;
        }
        /// <summary>
        /// Inherited method
        /// </summary>
        public override void Initialize()
        {
        }
        /// <summary>
        /// Inherited method
        /// </summary>
        /// <param name="ecellObj"></param>
        public override void DataChanged(EcellObject ecellObj)
        {
        }
        /// <summary>
        /// Inherited method
        /// </summary>
        public override void DataDeleted()
        {
        }
        /// <summary>
        /// Inherited method
        /// </summary>
        public override void SelectChanged()
        {
        }
        /// <summary>
        /// Inherited method
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
        /// Get a list of PathwayElement.
        /// Actually, this method returns a empty list of PathwayElement
        /// </summary>
        /// <returns></returns>
        public override List<PathwayElement> GetElements()
        {
            return new List<PathwayElement>();
        }
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <returns>new PSystem instance</returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PSystem();
        }
        #endregion

        /// <summary>
        /// Check if this PSystem's region overlaps given rectangle
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if each rectangle overlaps other rectangle
        /// (doesn't contain whole rectangle)</returns>
        public virtual bool Overlaps(RectangleF rect)
        {
            RectangleF ownRect = base.GlobalBounds;
            bool isOverlapping = false;
            bool cornerInsideExists = false;
            bool cornerOutsideExists = false;

            if (ownRect.Contains(rect.X,rect.Y))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if (ownRect.Contains(rect.X + rect.Width, rect.Y))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if (ownRect.Contains(rect.X + rect.Width, rect.Y + rect.Height))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if (ownRect.Contains(rect.X, rect.Y + rect.Height))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            isOverlapping = cornerInsideExists && cornerOutsideExists;
            cornerInsideExists = false;
            cornerOutsideExists = false;

            if (rect.Contains(ownRect.X, ownRect.Y))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if(rect.Contains(ownRect.X + ownRect.Width, ownRect.Y))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if(rect.Contains(ownRect.X + ownRect.Width, ownRect.Y + ownRect.Height))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if (rect.Contains(ownRect.X, ownRect.Y + ownRect.Height))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            return isOverlapping || (cornerInsideExists && cornerOutsideExists);
        }

        /// <summary>
        /// Memorize current position of the canvas.
        /// </summary>
        public override void MemorizeCurrentPosition()
        {
            base.m_originalX = base.X;
            base.m_originalY = base.Y;
            base.m_originalOffsetX = base.OffsetX;
            base.m_originalOffsetY = base.OffsetY;
            this.m_originalWidth = this.Width;//this.SystemWidth;
            this.m_originalHeight = this.Height;// this.SystemHeight;
        }

        /// <summary>
        /// Return to memorized position.
        /// </summary>
        public override void ReturnToMemorizedPosition()
        {
            base.X = base.m_originalX;
            base.Y = base.m_originalY;
            base.OffsetX = base.m_originalOffsetX;
            base.OffsetY = base.m_originalOffsetY;
            base.Width = this.m_originalWidth;
            base.Height = this.m_originalHeight;
            //this.SystemWidth = this.m_originalWidth;
            //this.SystemHeight = this.m_originalHeight;
            this.Element.X = base.m_originalX;
            this.Element.Y = base.m_originalY;
            foreach(PPathwayObject obj in this.ChildObjectList)
            {
                if (obj is PPathwayVariable)
                    ((PPathwayVariable)obj).Refresh();
            }
        }

        /// <summary>
        /// Refresh graphical contents of this object.
        /// ex) Edges of a process can be refreshed by using this.
        /// </summary>
        public override void Refresh()
        {
            base.X = base.m_originalX;
            base.Y = base.m_originalY;
            base.OffsetX = base.m_originalOffsetX;
            base.OffsetY = base.m_originalOffsetY;
            //this.SystemWidth = this.m_originalWidth;
            //this.SystemHeight = this.m_originalHeight;
            base.Width = this.m_originalWidth;
            base.Height = this.m_originalHeight;
        }
        
        /// <summary>
        /// start to move this Node by drag.
        /// </summary>
        public override void MoveStart()
        {
            PNodeList tmplist = new PNodeList();
            foreach (PNode p in this.ChildrenReference)
            {
                if (p is PPathwayProcess)
                    tmplist.Add(p);
            }
            foreach (PNode p in tmplist) {
                if (p is PPathwayObject)
                {
                    ((PPathwayObject)p).MoveStart();
                }
                else if (p is PPath)
                {
                    int ind = this.IndexOfChild(p);
                    this.RemoveChild(ind);
                }
            }
        }
        
        /// <summary>
        /// end to move this Node by drag.
        /// </summary>
        public override void MoveEnd()
        {
            PNodeList tmplist = new PNodeList();
            foreach (PNode p in this.ChildrenReference)
            {
                if (p is PPathwayProcess)
                    tmplist.Add(p);
            }
            foreach (PNode p in tmplist)
            {
                if (p is PPathwayObject)
                {
                    ((PPathwayObject)p).MoveEnd();
                }
            }
        }
    }
}