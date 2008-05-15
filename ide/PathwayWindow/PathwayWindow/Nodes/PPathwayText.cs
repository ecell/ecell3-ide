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
using EcellLib.Objects;
using EcellLib.PathwayWindow.Handler;

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
                this.m_pText.Text = ((EcellText)value).Comment;
                this.m_name = value.Name;
                this.Width = value.Width;
                this.Height = value.Height;
                base.EcellObject = value;
                RefreshView();
            }
        }        
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayText(CanvasControl canvas)
        {
            base.m_pText.Text = "Text000\n";
            base.m_pText.ConstrainWidthToTextWidth = false;
            base.LineBrush = Brushes.Black;
            base.FillBrush = Brushes.White;
            base.AddInputEventListener(new NodeDragHandler(canvas));
            this.m_name = "Text";
            this.m_canvas = canvas;
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
            this.m_pText.MoveToFront();
        }

        /// <summary>
        /// Create new instance of this object.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayText(m_canvas);
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
