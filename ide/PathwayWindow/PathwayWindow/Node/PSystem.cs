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
        /// An element of this class.
        /// </summary>
        protected SystemElement m_systemElement;

        /// <summary>
        /// Width of this system.
        /// </summary>
        protected float m_systemWidth = 0;

        /// <summary>
        /// Height of this system.
        /// </summary>
        protected float m_systemHeight = 0;

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
        public SystemElement Element
        {
            get { return m_systemElement; }
            set { m_systemElement = value; }
        }

        /// <summary>
        /// Accessor for m_systemWidth.
        /// </summary>
        public virtual float SystemWidth
        {
            get { return m_systemWidth; }
            set { m_systemWidth = value; }
        }

        /// <summary>
        /// Accessor for m_systemHeight.
        /// </summary>
        public virtual float SystemHeight
        {
            get { return m_systemHeight; }
            set { m_systemHeight = value; }
        }

        public virtual float OriginalX
        {
            get { return m_originalX; }
        }

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
                    OffsetToLayer.X = OffsetToLayer.X + dummyParent.OffsetX;
                    OffsetToLayer.Y = OffsetToLayer.Y + dummyParent.OffsetY;
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
                                      SystemWidth,
                                      SystemHeight);
            }
        }
        #endregion

        /// <summary>
        /// set the rectangles displayed this system in canvas.
        /// </summary>
        /// <param name="x">the position of this system.</param>
        /// <param name="y">the position of this system.</param>
        /// <param name="width">the width of this system.</param>
        /// <param name="height">the height of this system.</param>
        public virtual void SetRect(float x, float y, float width, float height)
        {
            base.X = x;
            base.Y = y;
            m_systemWidth = width;
            m_systemHeight = height;
        }

        #region nouse
        public virtual void UpdateResizeHandlePositions()
        {
        }
        public override void Delete()
        {
        }
        public override bool HighLighted(bool highlight)
        {
            return true;
        }
        public override void Initialize()
        {
        }
        public override void DataChanged(EcellObject ecellObj)
        {
        }
        public override void DataDeleted()
        {
        }
        public override void SelectChanged()
        {
        }
        public override void Start()
        {
        }
        public override void Change()
        {
        }
        public override void Stop()
        {
        }
        public override void End()
        {
        }
        public override List<PathwayElement> GetElements()
        {
            return new List<PathwayElement>();
        }
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
            this.m_originalWidth = this.SystemWidth;
            this.m_originalHeight = this.SystemHeight;
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
            this.SystemWidth = this.m_originalWidth;
            this.SystemHeight = this.m_originalHeight;
            this.Element.X = base.m_originalX;
            this.Element.Y = base.m_originalY;
            foreach(PPathwayObject obj in this.ChildObjectList)
            {
                if (obj is PEcellVariable)
                    ((PEcellVariable)obj).Refresh();
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
            this.SystemWidth = this.m_originalWidth;
            this.SystemHeight = this.m_originalHeight;
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
                if (p is PEcellProcess)
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
                if (p is PEcellProcess)
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