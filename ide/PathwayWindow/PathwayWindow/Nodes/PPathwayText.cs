﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Event;
using System.Drawing;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using UMD.HCIL.Piccolo;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayText
    /// </summary>
    public class PPathwayText : PPathwayObject
    {
        #region Constants
        /// <summary>
        /// 
        /// </summary>
        public const float MIN_WIDTH = 80;
        /// <summary>
        /// 
        /// </summary>
        public const float MIN_HEIGHT = 40;
        /// <summary>
        /// 
        /// </summary>
        public const float TEXT_MARGIN = 10;
        #endregion

        #region Fields
        /// <summary>
        /// TextBox for EcellText.Comment.
        /// </summary>
        private TextBox m_tbox = new TextBox();
        
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayText()
        {
            base.m_pText.Text = "Text";
            base.m_pText.ConstrainWidthToTextWidth = false;
            base.LineBrush = Brushes.Black;
            base.FillBrush = Brushes.White;
            this.m_tbox.LostFocus += new EventHandler(m_tbox_LostFocus);
            this.m_tbox.KeyPress += new KeyPressEventHandler(m_tbox_KeyPress);
            this.m_tbox.Multiline = true;
            this.m_resizeHandler = new PathwayResizeHandler(this);
            this.m_resizeHandler.MinHeight = 40;
            this.m_resizeHandler.MinWidth = 80;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// EcellObject
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return m_ecellObj; }
            set
            {
                EcellText text = (EcellText)value;
                m_pText.Text = text.Comment;
                base.X = text.X;
                base.Y = text.Y;
                base.Width = Math.Max(text.Width, MIN_WIDTH);
                base.Height = m_pText.Height + TEXT_MARGIN;
                this.m_resizeHandler.MinWidth = Math.Min(base.Width, MIN_WIDTH);
                this.m_resizeHandler.MinHeight = Math.Min(base.Height, MIN_HEIGHT);

                m_pText.TextAlignment = text.Alignment;
                base.EcellObject = text;
                RefreshView();
            }
        }
        /// <summary>
        /// Offset
        /// </summary>
        public override PointF Offset
        {
            get
            {
                return base.Offset;
            }
            set
            {
                base.Offset = value;
                m_resizeHandler.UpdateResizeHandle();
            }
        }

        /// <summary>
        /// CanvasControl
        /// </summary>
        public override CanvasControl Canvas
        {
            get
            {
                return base.Canvas;
            }
            set
            {
                base.Canvas = value;
                m_resizeHandler.Canvas = value;
            }
        }
        /// <summary>
        /// get/set the flag whether display this system with highlight.
        /// </summary>
        public override bool IsHighLighted
        {
            get { return this.m_isSelected; }
            set
            {
                this.m_isSelected = value;
                if (value)
                {
                    this.Brush = m_highLightBrush;
                    this.m_resizeHandler.ShowResizeHandles();
                }
                else
                {
                    this.Brush = m_fillBrush;
                    this.m_resizeHandler.HideResizeHandles();
                }
            }
        }
        #endregion

        /// <summary>
        /// Refresh Text contents of this object.
        /// </summary>
        protected override void RefreshText()
        {
            if (this.m_ecellObj != null)
                this.m_pText.Text = ((EcellText)m_ecellObj).Comment;
            this.m_pText.X = base.X;
            this.m_pText.Y = base.Y;
            this.m_pText.Width = base.Width;
            this.m_pText.Height = base.Height;
            this.m_pText.MoveToFront();
        }

        /// <summary>
        /// Create new instance of this object.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnDoubleClick(PInputEventArgs e)
        {
            base.OnDoubleClick(e);
            m_canvas.NotifyRemoveSelect(this);
            PointF pos = new PointF(base.X + base.OffsetX, base.Y + base.OffsetY);
            m_canvas.PCanvas.Controls.Add(m_tbox);
            float viewScale = m_canvas.PCanvas.Camera.ViewScale;
            m_tbox.Text = base.m_pText.Text;
            m_tbox.Location = m_canvas.CanvasPosToSystemPos(pos);
            m_tbox.Width = (int)(base.Width * viewScale + 5);
            m_tbox.Height = (int)(base.Height * viewScale + 5);
            m_tbox.Focus();
        }

        /// <summary>
        /// RefreshView
        /// </summary>
        public override void RefreshView()
        {
            base.RefreshView();
            m_resizeHandler.UpdateResizeHandle();
        }

        private void m_tbox_LostFocus(object sender, EventArgs e)
        {
            SetText();
        }

        private void m_tbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
                m_canvas.PCanvas.Controls.Remove(m_tbox);
        }

        private void SetText()
        {
            m_canvas.PCanvas.Controls.Remove(m_tbox);
            if (string.IsNullOrEmpty(m_tbox.Text))
            {
                m_canvas.Control.NotifyDataDelete(m_ecellObj, true);
            }
            else if (!m_tbox.Text.Equals(((EcellText)m_ecellObj).Comment))
            {
                ((EcellText)m_ecellObj).Comment = m_tbox.Text;
                m_pText.Text = m_tbox.Text;
                base.Width = m_pText.Width;
                base.Height = m_pText.Height + TEXT_MARGIN;

                m_ecellObj.Layer = this.Layer.Name;
                m_ecellObj.X = this.X + this.OffsetX;
                m_ecellObj.Y = this.Y + this.OffsetY;
                m_ecellObj.Width = this.Width;
                m_ecellObj.Height = this.Height;
                m_ecellObj.OffsetX = 0f;
                m_ecellObj.OffsetY = 0f;

                m_canvas.Control.NotifyDataChanged(m_ecellObj.Key, m_ecellObj, true, true);
            }
        }
    }
}
