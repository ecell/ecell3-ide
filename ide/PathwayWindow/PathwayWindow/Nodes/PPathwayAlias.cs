﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using UMD.HCIL.Piccolo.Event;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using System.Windows.Forms;
using System.Drawing;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Alias object for Variable
    /// </summary>
    public class PPathwayAlias : PPathwayEntity
    {
        /// <summary>
        /// 
        /// </summary>
        PPathwayVariable m_variable = null;

        /// <summary>
        /// 
        /// </summary>
        public PPathwayVariable Variable
        {
            get { return m_variable; }
        }

        /// <summary>
        /// 
        /// </summary>
        public PPathwayAlias(PPathwayVariable variable)
        {
            this.m_variable = variable;
            this.m_canvas = variable.Canvas;
            this.AddPath(variable.Figure.GraphicsPath, false);
            this.Brush = variable.Setting.CreateBrush(m_path);
            this.Text = string.Format("[{0}]", variable.EcellObject.LocalID);
            this.Layer = variable.Layer;
            this.Setting = variable.Setting;

            this.AddInputEventListener(new NodeDragHandler(variable.Canvas));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDown(e);

            // Set Focus
            m_variable.Selected = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int GetConnectorIndex(PointF point)
        {
            int index = base.GetConnectorIndex(point);
            index = index + (1 + m_variable.Aliases.IndexOf(this)) * 10;
            return index;
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            if (m_variable.Aliases.Count <= 0 || m_layer == null)
                return;
            foreach (PPathwayEdge line in m_variable.Edges)
                line.Refresh();
        }

    }
}
