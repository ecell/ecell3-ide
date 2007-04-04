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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib.PathwayWindow.Node;
using EcellLib.PathwayWindow.Element;

namespace EcellLib.PathwayWindow.UIComponent
{
    public class ShowBtnDownward : PPathwayObject
    {
        /// <summary>
        /// button width
        /// </summary>
        private static readonly float m_width = 15;

        private static readonly float m_margin = 2;

        private static readonly float m_shadowWidth = 2;
        /// <summary>
        /// Brush to paint a background
        /// </summary>
        private Brush m_backGroundBrush;

        /// <summary>
        /// Brush to paint an arrow
        /// </summary>
        private Brush m_arrowBrush;

        /// <summary>
        /// Brush to paint a shadow
        /// </summary>
        private Brush m_shadowBrush;

        public ShowBtnDownward(Pen pen, Brush backGroundBrush, Brush arrowBrush, Brush shadowBrush)
        {
            base.path = new GraphicsPath();
            base.Pen = pen;
            base.Brush = Brushes.Black;
            this.m_backGroundBrush = backGroundBrush;
            this.m_arrowBrush = arrowBrush;
            this.m_shadowBrush = shadowBrush;
            base.AddRectangle(0,0,m_width,m_width);
        }

        #region Inherited methods
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
            return this;
        }
        #endregion

        public override void OnMouseEnter(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseEnter(e);
            e.Canvas.Cursor = Cursors.PanSouth;
        }

        public override void OnMouseLeave(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseLeave(e);
            e.Canvas.Cursor = Cursors.Default;
        }

        protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
        {
            Graphics g = paintContext.Graphics;

            g.DrawRectangle(Pen, 0f, 0f, m_width - m_shadowWidth, m_width - m_shadowWidth);
            g.FillRectangle(m_backGroundBrush, new RectangleF(0f, 0f, m_width - m_shadowWidth, m_width - m_shadowWidth));
            g.FillRectangle(m_shadowBrush, new RectangleF(m_width - m_shadowWidth, 0f, m_shadowWidth, m_width - m_shadowWidth));
            g.FillRectangle(m_shadowBrush, new RectangleF(0f, m_width - m_shadowWidth, m_width, m_shadowWidth));
            g.FillPolygon(m_arrowBrush, new PointF[]{
                                        new PointF(m_margin,m_margin),
                                        new PointF(m_width - m_shadowWidth - m_margin,m_margin),
                                        new PointF((m_width - m_shadowWidth) / 2f,m_width - m_shadowWidth - m_margin)});
        }
    }
}