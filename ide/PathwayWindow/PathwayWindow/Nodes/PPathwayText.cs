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

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// 
    /// </summary>
    public class PPathwayText : PText
    {
        private CanvasControl m_canvas;
        private EcellObject m_ecellObj;
        private TextBox m_tbox = new TextBox();
        private string m_name;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Accessor for an instance of CanvasViewComponentSet which this instance belongs.
        /// </summary>
        public virtual CanvasControl CanvasControl
        {
            get { return m_canvas; }
            set
            {
                m_canvas = value;
            }
        }

        /// <summary>
        /// EcellObject
        /// </summary>
        public EcellObject EcellObject
        {
            get { return m_ecellObj; }
            set
            {
                m_ecellObj = value;
                this.Text = ((EcellText)value).Comment;
                this.Name = value.Name;
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public PointF PointF
        {
            get { return new PointF(this.X,this.Y);}
            set
            { 
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PPathwayText(CanvasControl canvas)
        {
            this.m_name = "Text";
            base.Text = "Text";
            this.m_canvas = canvas;
            base.Brush = Brushes.Beige;
            base.AddInputEventListener(new ObjectDragHandler());
            this.m_tbox.LostFocus += new EventHandler(m_tbox_LostFocus);
            this.m_tbox.KeyPress += new KeyPressEventHandler(m_tbox_KeyPress);
            this.m_tbox.Multiline = true;
        }

        /// <summary>
        /// Called when the mouse leaves this object.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(PInputEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_canvas == null)
                return;
            m_canvas.FocusNode = this;
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
            m_tbox.Text = base.Text;
            m_tbox.Location = m_canvas.CanvasPosToSystemPos(pos);
            m_tbox.Width = (int)(base.Width * viewScale + 5);
            m_tbox.Height = (int)(base.Height * viewScale + 5);
            m_tbox.Focus();
        }

        void m_tbox_LostFocus(object sender, EventArgs e)
        {
            SetText();
        }

        void m_tbox_KeyPress(object sender, KeyPressEventArgs e)
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
                m_canvas.Control.NotifyDataChanged(m_ecellObj.Key, m_ecellObj, true, true);
            }
        }

    }
}
