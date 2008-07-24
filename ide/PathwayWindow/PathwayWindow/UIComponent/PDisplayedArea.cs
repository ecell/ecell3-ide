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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.IO;
using Ecell.IDE.Plugins.PathwayWindow.Figure;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// PDisplayedArea is used in overview window for indicating the area which 
    /// is displayed main canvas. PDisplayedArea has a rectangle shape and painted
    /// with alpha blended color.
    /// </summary>
    public class PDisplayedArea : PPathwayObject
    {
        /// <summary>
        /// used for painting within rectangle. For viewing objects under rectangle area,
        /// this color has alpha value.
        /// </summary>
        SolidBrush m_brush = new SolidBrush(Color.FromArgb(64, Color.Red));

        /// <summary>
        /// Cursor to move this object.
        /// </summary>
        Cursor m_hand = new Cursor( new MemoryStream( PathwayResource.move ));
                
        /// <summary>
        /// constructor. almost all settings for this object is done here.
        /// </summary>
        public PDisplayedArea()
        {
            base.Brush = m_brush;
            base.Pen = new Pen(Brushes.DarkRed, 4);
            RectangleFigure fig = new RectangleFigure();
            base.AddPath(fig.GraphicsPath, false);
        }

        #region inherited from PPathwayObject
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return null;
        }
        #endregion

        #region Event
        /// <summary>
        /// Called when the mouse enter a displayed area.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseEnter(PInputEventArgs e)
        {
            base.OnMouseEnter(e);
            e.Canvas.Cursor = m_hand;
        }

        /// <summary>
        /// Called when the mouse leave a displayed area.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseLeave(PInputEventArgs e)
        {
            base.OnMouseLeave(e);
            e.Canvas.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public override void Refresh()
        {
            Brush = m_brush;
            base.Refresh();
        }
        #endregion
    }
}
