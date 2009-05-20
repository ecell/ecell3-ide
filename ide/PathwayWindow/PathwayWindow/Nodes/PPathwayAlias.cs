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

using UMD.HCIL.Piccolo.Event;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using System.Windows.Forms;
using System.Drawing;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Alias object for Variable
    /// </summary>
    public class PPathwayAlias : PPathwayNode
    {
        /// <summary>
        /// 
        /// </summary>
        PPathwayVariable m_variable = null;

        /// <summary>
        /// Whether this object is highlighted or not.
        /// </summary>
        protected bool m_selected = false;

        /// <summary>
        /// 
        /// </summary>
        public PPathwayVariable Variable
        {
            get { return m_variable; }
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
                //if (value)
                //{
                //    this.Brush = m_highLightBrush;
                //    this.m_pText.Brush = m_highLightBrush;
                //}
                //else
                //{
                //    this.Brush = m_fillBrush;
                //    this.m_pText.Brush = Brushes.Transparent;
                //    RefreshView();
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PPathwayAlias(PPathwayVariable variable)
        {
            this.m_variable = variable;
            this.AddPath(variable.Figure.GraphicsPath, false);
            this.Brush = variable.Setting.CreateBrush(m_path);
            this.Text = string.Format("[{0}]", variable.Text);

            this.AddInputEventListener(new NodeDragHandler(variable.Canvas));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDown(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDown(e);

            bool isCtrl = (e.Modifiers == Keys.Control);
            bool isLeft = (e.Button == MouseButtons.Left);

            // Set IsSelect
            CanvasControl canvas = m_variable.Canvas;
            bool selected = m_variable.Selected;
            if (!selected && !isCtrl)
                canvas.NotifySelectChanged(m_variable);
            else if (!selected && isCtrl)
                canvas.NotifyAddSelect(m_variable);
            else if (selected && isCtrl && isLeft)
                canvas.NotifyRemoveSelect(m_variable);


            // Set Focus
            canvas.FocusNode = this;

        }
    }
}
