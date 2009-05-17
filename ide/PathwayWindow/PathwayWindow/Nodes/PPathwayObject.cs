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
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.Serialization;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using Ecell;
using System.ComponentModel;
using UMD.HCIL.Piccolo.Event;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using System.IO;
using Ecell.IDE.Plugins.PathwayWindow.Components;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayObject is a super class for all component of PCanvas.
    /// </summary>
    public class PPathwayObject : PPathwayNode
    {
        #region Enums
        /// <summary>
        /// Represents the types of picking modes for a PProcess object.
        /// </summary>
        public enum PathPickMode
        {
            /// <summary>
            /// Faster Picking.  Paths are picked in local coordinates.
            /// </summary>
            Fast,

            /// <summary>
            /// Slower and more accurate picking.  Paths are picked in canvas
            /// coordinates.
            /// </summary>
            Accurate
        }
        #endregion

        #region Constant
        /// <summary>
        /// Font size of node object.
        /// </summary>
        protected const int FONT_SIZE = 10;

        #endregion

        #region Fields
        /// <summary>
        /// On this CanvasViewComponentSet this PPathwayObject is drawn.
        /// </summary>
        protected CanvasControl m_canvas;

        /// <summary>
        /// this node belong the layer.
        /// </summary>
        protected PPathwayLayer m_layer;

        /// <summary>
        /// Parent object.
        /// </summary>
        protected PPathwaySystem m_parentSystem;

        /// <summary>
        /// PText for showing this object's ID.
        /// </summary>
        protected PText m_pText;

        /// <summary>
        /// ResizeHandler
        /// </summary>
        protected PathwayResizeHandler m_resizeHandler;

        /// <summary>
        /// From this ComponentSetting, this object was created.
        /// </summary>
        protected ComponentSetting m_setting;

        /// <summary>
        /// Figure
        /// </summary>
        protected IFigure m_figure;

        /// <summary>
        /// Object will be painted with this Brush when object is not selected.
        /// </summary>
        protected Brush m_fillBrush = Brushes.White;
        /// <summary>
        /// Object will be painted with this Brush when object is not selected.
        /// </summary>
        protected Brush m_lineBrush = Brushes.Black;
        /// <summary>
        /// Object will be painted with this Brush when object is selected.
        /// </summary>
        protected Brush m_highLightBrush = Brushes.Gold;
        /// <summary>
        /// Object will be painted with this Brush when object is in invalid state.
        /// </summary>
        protected Brush m_invalidBrush = Brushes.Red;

        /// <summary>
        /// Whether this object is highlighted or not.
        /// </summary>
        protected bool m_selected = false;

        /// <summary>
        /// Whether this node is showing ID or not.
        /// </summary>
        protected bool m_showingId = false;

        /// <summary>
        /// Is Edit Mode or not.
        /// </summary>
        protected bool m_isViewMode = false;

        /// <summary>
        /// EcellObject for this object.
        /// </summary>
        protected EcellObject m_ecellObj;

        /// <summary>
        /// For memorizing a position before the start of a dragging.
        /// When the dragging failed, this object will be returned to this position.
        /// </summary>
        protected float m_originalX = 0;

        /// <summary>
        /// For memorizing a position before the start of a dragging.
        /// When the dragging failed, this object will be returned to this position.
        /// </summary>
        protected float m_originalY = 0;

        /// <summary>
        /// For memorizing a position before the start of a dragging.
        /// When the dragging failed, this object will be returned to this position.
        /// </summary>
        protected float m_originalOffsetX = 0;

        /// <summary>
        /// For memorizing a position before the start of a dragging.
        /// When the dragging failed, this object will be returned to this position.
        /// </summary>
        protected float m_originalOffsetY = 0;

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
        /// Accessor for an instance of CanvasViewComponentSet which this instance belongs.
        /// </summary>
        public virtual CanvasControl Canvas
        {
            get { return m_canvas; }
            set
            {
                m_canvas = value;

            }
        }
        /// <summary>
        /// Accessor for m_layer.
        /// </summary>
        public virtual PPathwayLayer Layer
        {
            get { return this.m_layer; }
            set
            {
                this.m_layer = value;
                this.m_layer.AddChild(this);
                this.Visible = value.Visible;
                this.Pickable = this.Visible;
            }
        }
        /// <summary>
        /// Accessor for m_parentObject.
        /// </summary>
        public virtual PPathwaySystem ParentObject
        {
            get { return m_parentSystem; }
            set { m_parentSystem = value; }
        }
        /// <summary>
        /// Accessor for Text.
        /// </summary>
        public PText PText
        {
            get
            {
                RefreshText();
                return m_pText;
            }
        }

        /// <summary>
        /// Accessor for m_setting.
        /// </summary>
        public ComponentSetting Setting
        {
            get { return this.m_setting; }
            set 
            {
                if (m_setting != null)
                {
                    this.m_setting.PropertyChange -= Setting_PropertyChange;
                }
                if (value != null)
                {
                    this.m_setting = value;
                    this.m_setting.PropertyChange += new EventHandler(Setting_PropertyChange);
                    RefreshSettings();
                }
            }
        }
        /// <summary>
        /// Accessor for m_figure.
        /// </summary>
        public IFigure Figure
        {
            get { return this.m_figure; }
            set
            {
                this.m_figure = value;
                MemorizePosition();
                AddPath(m_figure.GraphicsPath, false);
                ResetPosition();
            }
        }
        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush FillBrush
        {
            get { return this.m_fillBrush; }
            set
            {
                this.m_fillBrush = value;
                base.Brush = m_fillBrush;
            }
        }
        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush LineBrush
        {
            get { return this.m_lineBrush; }
            set {
                this.m_lineBrush = value;
                this.Pen = new Pen(value, 0);
            }
        }
        /// <summary>
        /// Accessor for m_highLightBrush.
        /// </summary>
        public Brush HighLightBrush
        {
            get { return this.m_highLightBrush; }
            set { this.m_highLightBrush = value; }
        }

        /// <summary>
        /// Accessor for m_isHighLighted.
        /// </summary>
        public virtual bool Selected
        {
            get { return this.m_selected; }
            set
            {
                this.m_selected = value;
                if (value)
                {
                    this.Brush = m_highLightBrush;
                    this.m_pText.Brush = m_highLightBrush;
                }
                else
                {
                    this.Brush = m_fillBrush;
                    this.m_pText.Brush = Brushes.Transparent;
                    RefreshView();
                }
            }
        }

        /// <summary>
        /// Set Invalid state.
        /// </summary>
        public virtual bool Invalid
        {
            set
            {
                if (value)
                    this.Brush = m_invalidBrush;
                else if (m_selected)
                    this.Brush = m_highLightBrush;
                else
                    this.Brush = m_fillBrush;
            }
        }

        /// <summary>
        /// get/set m_isViewMode.
        /// </summary>
        public virtual bool ViewMode
        {
            get { return m_isViewMode; }
            set
            {
                m_isViewMode = value;
                RefreshView();
                SetTextVisiblity();
            }
        }

        /// <summary>
        /// get/set whether is shown ID.
        /// </summary>
        public bool ShowingID
        {
            get { return m_showingId; }
            set { m_showingId = value;
            SetTextVisiblity();
            }
        }

        /// <summary>
        /// Accessor for EcellObject.
        /// </summary>
        public virtual EcellObject EcellObject
        {
            get
            {
                return this.m_ecellObj;
            }
            set
            {
                this.m_ecellObj = value;
                if (m_ecellObj.IsPosSet)
                {
                    this.CenterPointF = m_ecellObj.CenterPointF;
                    base.OffsetX = m_ecellObj.OffsetX;
                    base.OffsetY = m_ecellObj.OffsetY;
                }
                MemorizePosition();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float Top
        {
            get { return base.Y + base.OffsetY; }
        }
        /// <summary>
        /// 
        /// </summary>
        public float Bottom
        {
            get { return base.Y + base.OffsetY + base.Height; }
        }
        /// <summary>
        /// 
        /// </summary>
        public float Left
        {
            get { return base.X + base.OffsetX; }
        }
        /// <summary>
        /// 
        /// </summary>
        public float Right
        {
            get { return base.X + base.OffsetX + base.Width; }
        }

        /// <summary>
        /// Accessor for X coordinate.
        /// </summary>
        public PointF PointF
        {
            get { return new PointF(base.X + base.OffsetX, base.Y + base.OffsetY); }
            set
            {
                base.X = value.X;
                base.Y = value.Y;
            }
        }
        /// <summary>
        /// acessor for a rectangle of this system.
        /// </summary>
        public virtual RectangleF Rect
        {
            get
            {
                return new RectangleF(base.X + this.OffsetX,
                                      base.Y + this.OffsetY,
                                      base.Width,
                                      base.Height);
            }
            set
            {
                base.X = value.X;
                base.Y = value.Y;
                base.Width = value.Width;
                base.Height = value.Height;
            }
        }

        /// <summary>
        /// Accessor for X coordinate.
        /// </summary>
        public PointF CenterPointF
        {
            get { return new PointF(base.X + base.OffsetX + base.Width / 2f, base.Y + base.OffsetY + base.Height / 2f); }
            set
            {
                base.X = value.X - base.Width / 2f;
                base.Y = value.Y - base.Height / 2f;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayObject()
        {
            m_ecellObj = null;

            m_pText = new PText();
            m_pText.Pickable = false;
            m_pText.Font = new Font("Arial", FONT_SIZE, FontStyle.Bold);
            this.AddChild(m_pText);
        }
        #endregion

        #region Methods
        #region Abstract Methods
        /// <summary>
        /// Create new instance of this object.
        /// </summary>
        /// <returns></returns>
        public virtual PPathwayObject CreateNewObject()
        {
            return new PPathwayObject();
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Refresh graphical contents of this object.
        /// ex) Edges of a process can be refreshed by using this.
        /// </summary>
        public virtual void Refresh()
        {
            RefreshText();
        }

        /// <summary>
        /// Change View Mode.
        /// </summary>
        public virtual void RefreshView()
        {
            SetFillBrush();
            Refresh();
        }

        /// <summary>
        /// Refresh ComponentSetting.
        /// </summary>
        protected virtual void RefreshSettings()
        {
            this.PText.TextBrush = m_setting.TextBrush;
            this.LineBrush = m_setting.LineBrush;
            this.Figure = m_setting.Figure;
            RefreshView();
            if (m_setting.IconExists)
            {
                MemorizePosition();
                //this.Image = Image.FromFile(m_setting.IconFileName);
                ResetPosition();
            }
        }

        /// <summary>
        /// Refresh Text contents of this object.
        /// </summary>
        protected virtual void RefreshText()
        {
            if (this.m_ecellObj != null)
                this.m_pText.Text = GetLabelFor(this.m_ecellObj);
            this.m_pText.CenterBoundsOnPoint(base.X + base.Width / 2, base.Y + base.Height / 2);
            this.m_pText.MoveToFront();
        }

        /// <summary>
        /// Get a suitable label for the specified object.
        /// </summary>
        protected static string GetLabelFor(EcellObject obj)
        {
            StringBuilder retval = new StringBuilder();
            if (obj is EcellSystem)
            {
                retval.Append(string.Format("{0} (size={1})",
                    obj.LocalID, ((EcellSystem)obj).SizeInVolume));
            }
            else
            {
                retval.Append(string.Format("{0}", obj.LocalID));
            }
            if (obj.Logged)
            {
                retval.Append("*");
            }
            return retval.ToString();
        } 

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SetTextVisiblity()
        {
            if (m_showingId)
                m_pText.Visible = true;
            else
                m_pText.Visible = false;
        }
        #endregion

        /// <summary>
        /// Set FillBrush
        /// </summary>
        private void SetFillBrush()
        {
            // Create and set FillBrush.
            // If Highlighted, set Hightlighted Brush.
            if (m_selected)
                this.Brush = m_highLightBrush;
            else
                this.FillBrush = m_setting.CreateBrush(m_path);
        }

        /// <summary>
        /// Memorize a current position for returning to this position in the future in neccessary.
        /// </summary>
        public void MemorizePosition()
        {
            this.m_originalX = base.X;
            this.m_originalY = base.Y;
            this.m_originalOffsetX = base.OffsetX;
            this.m_originalOffsetY = base.OffsetY;
            this.m_originalWidth = this.Width;
            this.m_originalHeight = this.Height;
        }
        /// <summary>
        /// Reset a current position.
        /// </summary>
        public void ResetPosition()
        {
            base.X = this.m_originalX;
            base.Y = this.m_originalY;
            base.OffsetX = this.m_originalOffsetX;
            base.OffsetY = this.m_originalOffsetY;
            base.Width = this.m_originalWidth;
            base.Height = this.m_originalHeight;
        }

        /// <summary>
        /// Set Moving delta.
        /// </summary>
        /// <param name="delta"></param>
        public void MovePosition(PointF delta)
        {
            this.OffsetX = this.OffsetX + delta.X;
            this.OffsetY = this.OffsetY + delta.Y;
        }

        #endregion

        #region EventHandlers
        /// <summary>
        /// Eventhandler on ComponentSetting.PropertyChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setting_PropertyChange(object sender, EventArgs e)
        {
            RefreshSettings();
        }

        /// <summary>
        /// Event on mouse down.
        /// Change the selected state of this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_canvas == null)
                return;

            bool isCtrl = (e.Modifiers == Keys.Control);
            bool isLeft = (e.Button == MouseButtons.Left);

            // Set IsSelect
            if(!m_selected && ! isCtrl)
                m_canvas.NotifySelectChanged(this);
            else if(!m_selected && isCtrl)
                m_canvas.NotifyAddSelect(this);
            else if (m_selected && isCtrl && isLeft)
                m_canvas.NotifyRemoveSelect(this);

            // Set Focus
            m_canvas.FocusNode = this;
        }

        #endregion

        #region EventHandler for HighLightChanged
        private PPropertyEventHandler m_onHighLightChanged;
        /// <summary>
        /// Event on layer change.
        /// </summary>
        public event PPropertyEventHandler HighLightChanged
        {
            add { m_onHighLightChanged += value; }
            remove { m_onHighLightChanged
                -= value; }
        }
        /// <summary>
        /// Event on layer change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnHightLightChanged(PPropertyEventArgs e)
        {
            if (m_onHighLightChanged != null)
                m_onHighLightChanged(this, e);
        }
        internal void RaiseHightLightChanged()
        {
            PPropertyEventArgs e = new PPropertyEventArgs(!this.m_selected, this.m_selected);
            OnHightLightChanged(e);
        }
        #endregion
    }
}
