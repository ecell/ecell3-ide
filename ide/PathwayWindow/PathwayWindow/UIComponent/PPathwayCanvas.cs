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

using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// PathwayCanvas which have piccolo objects.
    /// </summary>
    public class PPathwayCanvas : PCanvas
    {
        /// <summary>
        /// CanvasView to which this PathwayCanvas belongs
        /// </summary>
        protected CanvasControl m_canvas = null;
        /// <summary>
        /// PathwayControl to control the PathwayView.
        /// </summary>
        protected PathwayControl m_con = null;
        /// <summary>
        /// 
        /// </summary>
        protected bool m_highQuality = true;
        /// <summary>
        /// get / set Quality of Canvas Image.
        /// </summary>
        public bool HighQuality
        {
            get { return m_highQuality; }
            set
            {
                m_highQuality = value;

                // Set RenderQuality
                RenderQuality quality;
                if (m_highQuality)
                    quality = RenderQuality.HighQuality;
                else
                    quality = RenderQuality.LowQuality;

                this.AnimatingRenderQuality = quality;
                this.DefaultRenderQuality = quality;
                this.InteractingRenderQuality = quality;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas">CanvasControl instance</param>
        public PPathwayCanvas(CanvasControl canvas)
        {
            m_canvas = canvas;
            m_con = canvas.Control;
            // Preparing context menus.
            this.ContextMenuStrip = m_con.Menu.PopupMenu;
            this.KeyDown += new KeyEventHandler(m_con.Menu.Canvas_KeyDown);
            // 
            this.HighQuality = m_con.HighQuality;
            //
            this.RemoveInputEventListener(PanEventHandler);
            this.RemoveInputEventListener(ZoomEventHandler);
            this.Dock = DockStyle.Fill;
            this.Name = canvas.ModelID;
            this.Camera.ScaleViewBy(0.7f);
        }

        /// <summary>
        /// </summary>
        /// <param name="e">MouseEventArgs.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                m_canvas.PanCanvas(Direction.Horizontal, e.Delta);
            }
            else if (Control.ModifierKeys == Keys.Control || e.Button == MouseButtons.Right)
            {
                float zoomRate = (float)1.00 + (float)e.Delta / 1200;
                m_con.Menu.Zoom(zoomRate);
            }
            else
            {
                m_canvas.PanCanvas(Direction.Vertical, e.Delta);
            }
            base.OnMouseWheel(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e">MouseEventArgs.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus(); 
            m_canvas.FocusNode = null;
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_canvas.FocusNode = null;
            base.OnMouseUp(e);
        }

    }
}