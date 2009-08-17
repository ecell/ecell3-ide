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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Drawing;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.Objects;
using UMD.HCIL.Piccolo.Event;

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
            base.LineBrush = Brushes.Black;
            base.Brush = Brushes.White;
            this.m_tbox.LostFocus += new EventHandler(m_tbox_LostFocus);
            this.m_tbox.KeyPress += new KeyPressEventHandler(m_tbox_KeyPress);
            this.m_tbox.Multiline = true;
            this.m_resizeHandler = new PathwayResizeHandler(this);
            this.m_resizeHandler.MinHeight = 40;
            this.m_resizeHandler.MinWidth = 80;

            base.m_pText.Text = "Text";
            base.m_pText.ConstrainWidthToTextWidth = false;
            base.Height = m_pText.Height + TEXT_MARGIN;
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
                m_pText.TextAlignment = text.Alignment;

                base.X = text.X;
                base.Y = text.Y;
                base.Width = Math.Max(text.Width, MIN_WIDTH);
                base.Height = m_pText.Height + TEXT_MARGIN;
                this.m_resizeHandler.MinWidth = Math.Min(base.Width, MIN_WIDTH);
                this.m_resizeHandler.MinHeight = Math.Min(base.Height, MIN_HEIGHT);

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
                m_resizeHandler.Update();
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
        public override bool Selected
        {
            get { return this.m_selected; }
            set
            {
                base.Selected = value;
                if (value)
                {
                    this.Brush = m_highLightBrush;
                }
                else
                {
                    this.Brush = m_setting.CreateBrush(m_path);
                }
                RaiseHightLightChanged();
            }
        }
        #endregion

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            m_resizeHandler.Hide();
            base.Dispose();
        }

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
            m_resizeHandler.Update();
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
                m_pText.Text = m_tbox.Text;
                NotifyDataChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void NotifyDataChanged()
        {
            EcellText text = (EcellText)m_ecellObj.Clone();

            text.Comment = m_pText.Text;
            text.Alignment = m_pText.TextAlignment;
            base.Width = m_pText.Width;
            base.Height = m_pText.Height + TEXT_MARGIN;

            text.Layer = this.Layer.Name;
            text.X = this.X + this.OffsetX;
            text.Y = this.Y + this.OffsetY;
            text.Width = this.Width;
            text.Height = this.Height;
            text.OffsetX = 0f;
            text.OffsetY = 0f;

            m_canvas.Control.NotifyDataChanged(text.Key, text, true, true);
        }
    }
}
