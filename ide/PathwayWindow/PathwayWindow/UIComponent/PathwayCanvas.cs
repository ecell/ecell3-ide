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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Event;
using EcellLib.PathwayWindow.Nodes;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// PathwayCanvas which have piccolo objects.
    /// </summary>
    public class PathwayCanvas : PCanvas
    {
        /// <summary>
        /// CanvasView to which this PathwayCanvas belongs
        /// </summary>
        protected CanvasControl m_canvas = null;
        /// <summary>
        /// PathwayControl to control the PathwayView.
        /// </summary>
        protected PathwayControl m_con = null;
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cview"></param>
        public PathwayCanvas(CanvasControl cview)
        {
            m_canvas = cview;
            m_con = cview.PathwayControl;
        }
        /// <summary>
        /// </summary>
        /// <param name="e">MouseEventArgs.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (Control.ModifierKeys == Keys.Shift)
            {
                this.m_con.PanCanvas(Direction.Horizontal, e.Delta);
            }
            else if (Control.ModifierKeys == Keys.Control || e.Button == MouseButtons.Right)
            {
                float zoom = (float)1.00 + (float)e.Delta / 1200;
                this.m_con.ActiveCanvas.Zoom(zoom);
            }
            else
            {
                this.m_con.PanCanvas(Direction.Vertical, e.Delta);
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="e">EventArgs.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Focus();
        }
        /// <summary>
        /// Called when the mouse is on this canvas.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (m_con == null)
                return;
            if (e.Button != MouseButtons.Right)
                return;

            // Case null
            if (m_canvas.ClickedNode == null)
            {
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_ID].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR1].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR3].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CUT].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_COPY].Visible = false;
                if (this.m_con.CopiedNodes.Count > 0)
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_PASTE].Visible = true;
                else
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_PASTE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_WITH].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR4].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CREATE_LOGGER].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_LOGGER].Visible = false;
            }
            // Case PPathwayNode
            else if (m_canvas.ClickedNode is PPathwayNode)
            {
                PPathwayNode node = m_canvas.ClickedNode as PPathwayNode;
                m_con.SetMenuLogger(node);

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_ID].Visible = true;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR1].Visible = true;

                if ((int)m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Tag > 0)
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Visible = true;
                else
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR3].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CUT].Visible = true;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_COPY].Visible = true;
                if (this.m_con.CopiedNodes.Count > 0)
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_PASTE].Visible = true;
                else
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_PASTE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE].Visible = true;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_WITH].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR4].Visible = true;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CREATE_LOGGER].Visible = true;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_LOGGER].Visible = true;
            }
            else if (m_canvas.ClickedNode is Line)
            {
                EdgeInfo info = ((Line)m_canvas.ClickedNode).Info;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_ID].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR1].Visible = false;

                if ((int)m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Tag > 0)
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Visible = true;
                else
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Visible = false;

                switch (info.Direction)
                {
                    case EdgeDirection.Inward:
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Visible = false;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Visible = true;
                        break;
                    case EdgeDirection.Outward:
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Visible = true;
                        break;
                    case EdgeDirection.Bidirection:
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Visible = false;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Visible = true;
                        break;
                    case EdgeDirection.None:
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Visible = true;
                        m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Visible = false;
                        break;
                }
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Tag = m_canvas.ClickedNode;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Tag = m_canvas.ClickedNode;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Tag = m_canvas.ClickedNode;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Tag = m_canvas.ClickedNode;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR3].Visible = true;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CUT].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_COPY].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_PASTE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE].Visible = true;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_WITH].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE].Tag = m_canvas.ClickedNode;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR4].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CREATE_LOGGER].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_LOGGER].Visible = false;

            }
            else if (m_canvas.ClickedNode is PPathwaySystem)
            {
                PPathwaySystem sys = m_canvas.ClickedNode as PPathwaySystem;
                m_con.SetMenuLogger(sys);

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_ID].Visible = true;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR1].Visible = true;

                if ((int)m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Tag > 0)
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Visible = true;
                else
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR3].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CUT].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_COPY].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_PASTE].Visible = false;
                if (sys.EcellObject.key != "/")
                {
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE].Visible = true;
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_WITH].Visible = true;
                    String superSys = sys.EcellObject.key.Substring(0, sys.EcellObject.key.LastIndexOf("/"));
                    if (superSys == "") superSys = "/";
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_WITH].Text =
                        m_resources.GetString("MergeMenuText") + "(" + superSys + ")";
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR4].Visible = true;
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE].Tag = m_canvas.ClickedNode;
                }
                else
                {
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE].Visible = false;
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_WITH].Visible = false;
                    m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR4].Visible = false;
                }
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CREATE_LOGGER].Visible = true;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_LOGGER].Visible = true;
            }
            else
            {
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_ID].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR1].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR2].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_LEFT_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_BIDIR_ARROW].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CONSTANT_LINE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR3].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CUT].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_COPY].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_PASTE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_WITH].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_SEPARATOR4].Visible = false;

                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_CREATE_LOGGER].Visible = false;
                m_con.ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_LOGGER].Visible = false;

            }
        }
    }
}