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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Components;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.Objects;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;

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
        protected bool m_showingId = true;

        /// <summary>
        /// Is Edit Mode or not.
        /// </summary>
        protected bool m_isViewMode = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_changed = false;

        /// <summary>
        /// EcellObject for this object.
        /// </summary>
        protected EcellObject m_ecellObj = null;

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
            set { m_canvas = value; }
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
                if (value == null)
                    return;
                // Set Layer
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
        /// 
        /// </summary>
        public PathwayResizeHandler ResizeHandler
        {
            get { return m_resizeHandler; }
            set { m_resizeHandler = value; }
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
                PointF point = this.Center;
                AddPath(m_figure.GraphicsPath, false);
                this.Center = point;
            }
        }

        /// <summary>
        /// Accessor for m_normalBrush.
        /// </summary>
        public Brush LineBrush
        {
            get { return this.Pen.Brush; }
            set { this.Pen = new Pen(value, 0); }
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
                RefreshView();
                RaiseHightLightChanged();
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
                    this.Brush = m_setting.CreateBrush(m_path);
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
        /// 
        /// </summary>
        public bool Changed
        {
            get { return m_changed; }
            set { m_changed = value; }
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
                    base.OffsetX = m_ecellObj.OffsetX;
                    base.OffsetY = m_ecellObj.OffsetY;
                    this.Center = m_ecellObj.CenterPointF;
                }
                MemorizePosition();
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayObject()
        {
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
        /// Change View Mode.
        /// </summary>
        public virtual void RefreshView()
        {
            // Create and set FillBrush.
            // If Highlighted, set Hightlighted Brush.
            if (m_selected)
            {
                this.Brush = m_highLightBrush;
                this.m_pText.Brush = m_highLightBrush;
            }
            else
            {
                this.Brush = m_setting.CreateBrush(m_path);
                this.m_pText.Brush = Brushes.Transparent;
            }
            Refresh();
        }

        /// <summary>
        /// Refresh ComponentSetting.
        /// </summary>
        protected virtual void RefreshSettings()
        {
            MemorizePosition();
            this.Figure = m_setting.Figure;
            // Set Image
            this.Image = m_setting.Image;
            ResetPosition();
            this.PText.TextBrush = m_setting.TextBrush;
            this.LineBrush = m_setting.LineBrush;
            this.Brush = m_setting.CreateBrush(m_path);
            RefreshView();
        }

        /// <summary>
        /// Refresh Text contents of this object.
        /// </summary>
        protected override void RefreshText()
        {
            if (this.m_ecellObj != null)
                this.m_pText.Text = GetLabelFor(this.m_ecellObj);
            base.RefreshText();
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
            if (!this.Path.IsVisible(e.Position))
            {
                m_canvas.NotifyResetSelect();
                return;
            }

            bool isShift = (e.Modifiers == Keys.Shift);
            bool isCtrl = (e.Modifiers == Keys.Control);
            bool isRight = (e.Button == MouseButtons.Right);
            // Set Focus
            if (!m_selected && isCtrl)
            {
                m_canvas.NotifyAddSelect(this);
                m_canvas.FocusNode = this;
            }
            else if (m_selected && isCtrl)
            {
                m_canvas.NotifyRemoveSelect(this);
                m_canvas.FocusNode = null;
            }
            else if (isShift || (!isCtrl && (!isRight || !m_selected)) )
            {
                m_canvas.NotifySelectChanged(this);
                m_canvas.FocusNode = this;
            }
            else 
            {
                m_canvas.NotifyAddSelect(this);
                m_canvas.FocusNode = this;
            }

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


        #region Inherited from PImage
        /// <summary>
        /// The key that identifies a change in this node's <see cref="Image">Image</see>.
        /// </summary>
        /// <remarks>
        /// In a property change event both the old and new value will be set correctly
        /// to Image objects.
        /// </remarks>
        protected static readonly object PROPERTY_KEY_IMAGE = new object();

        /// <summary>
        /// A bit field that identifies a <see cref="ImageChanged">ImageChanged</see> event.
        /// </summary>
        /// <remarks>
        /// This field is used to indicate whether ImageChanged events should be forwarded to
        /// a node's parent.
        /// <seealso cref="UMD.HCIL.Piccolo.Event.PPropertyEventArgs">PPropertyEventArgs</seealso>.
        /// <seealso cref="UMD.HCIL.Piccolo.PNode.PropertyChangeParentMask">PropertyChangeParentMask</seealso>.
        /// </remarks>
        public const int PROPERTY_CODE_IMAGE = 1 << 14;

        /// <summary>
        /// The underlying image object.
        /// </summary>
        protected Image image;

        /// <summary>
        /// Gets or sets the image shown by this node.
        /// </summary>
        /// <value>The image shown by this node.</value>
        public virtual Image Image
        {
            get { return image; }
            set
            {
                InvalidatePaint();
                FirePropertyChangedEvent(PROPERTY_KEY_IMAGE, PROPERTY_CODE_IMAGE, image, value);
                image = value;
            }
        }

        /// <summary>
        /// Occurs when there is a change in this node's
        /// <see cref="Image">Image</see>.
        /// </summary>
        /// <remarks>
        /// When a user attaches an event handler to the ImageChanged Event as in
        /// ImageChanged += new PPropertyEventHandler(aHandler),
        /// the add method adds the handler to the delegate for the event
        /// (keyed by PROPERTY_KEY_IMAGE in the Events list).
        /// When a user removes an event handler from the ImageChanged event as in 
        /// ImageChanged -= new PPropertyEventHandler(aHandler),
        /// the remove method removes the handler from the delegate for the event
        /// (keyed by PROPERTY_KEY_IMAGE in the Events list).
        /// </remarks>
        public virtual event PPropertyEventHandler ImageChanged
        {
            add { HandlerList.AddHandler(PROPERTY_KEY_IMAGE, value); }
            remove { HandlerList.RemoveHandler(PROPERTY_KEY_IMAGE, value); }
        }

        /// <summary>
        /// Overridden.  See <see cref="PNode.Paint">PNode.Paint</see>.
        /// </summary>
        protected override void Paint(PPaintContext paintContext)
        {
            if (image != null)
            {
                RectangleF b = Bounds;
                System.Drawing.Graphics g = paintContext.Graphics;

                g.DrawImage((Image)image.Clone(), b);
                if (this is PPathwaySystem)
                    base.Paint(paintContext);
            }
            else
            {
                base.Paint(paintContext);
            }
        }
        #endregion


        #region IDisposable ÉÅÉìÉo
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            if (m_setting != null)
            {
                this.m_setting.PropertyChange -= Setting_PropertyChange;
            }
        }
        #endregion
    }
}
