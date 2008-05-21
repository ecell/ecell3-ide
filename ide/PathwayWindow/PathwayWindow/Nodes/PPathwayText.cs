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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Event;
using System.Drawing;
using EcellLib.Objects;
using EcellLib.PathwayWindow.Handler;
using UMD.HCIL.Piccolo;

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayText
    /// </summary>
    public class PPathwayText : PPathwayObject
    {
        #region Fields
        private TextBox m_tbox = new TextBox();
        private string m_name;
        private PathwayResizeHandler m_resizeHandler;
        
        #endregion

        #region Accessors
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// EcellObject
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return m_ecellObj; }
            set
            {
                this.m_name = value.Name;
                base.m_pText.Text = ((EcellText)value).Comment;
                if (value.Width != 0 && value.Height != 0)
                {
                    base.Width = value.Width;
                    base.Height = value.Height;
                }
                else
                {
                    base.Width = m_pText.Width;
                    base.Height = m_pText.Height;
                }
                base.EcellObject = value;
                RefreshView();
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
            this.m_name = "Text";
            this.m_tbox.LostFocus += new EventHandler(m_tbox_LostFocus);
            this.m_tbox.KeyPress += new KeyPressEventHandler(m_tbox_KeyPress);
            this.m_tbox.Multiline = true;
            this.m_resizeHandler = new PathwayResizeHandler(this);
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
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDrag(PInputEventArgs e)
        {
            base.OnMouseDrag(e);
            m_resizeHandler.UpdateResizeHandle();
        }

        private void m_tbox_LostFocus(object sender, EventArgs e)
        {
            SetText();
        }

        private void m_tbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
                SetText();
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
                base.Width = m_pText.Width;
                base.Height = m_pText.Height;
                NotifyDataChanged();
            }
        }
        /// <summary>
        /// NotifyDataChanged()
        /// </summary>
        public void NotifyDataChanged()
        {
            EcellObject eo = m_ecellObj.Copy();
            eo.X = this.X + this.OffsetX;
            eo.Y = this.Y + this.OffsetY;
            eo.OffsetX = 0;
            eo.OffsetY = 0;
            m_canvas.Control.NotifyDataChanged(eo.Key, eo, true, true);
        }
    }
}
