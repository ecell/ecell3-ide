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
using EcellLib.PathwayWindow.Node;
using PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Element;

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
        protected CanvasView m_cview = null;
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cview"></param>
        public PathwayCanvas(CanvasView cview)
        {
            m_cview = cview;
        }

        /// <summary>
        /// Called when the mouse is on this canvas.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            m_cview.PathwayView.TabControl.Focus();
        }
        /// <summary>
        /// Called when the mouse is on this canvas.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (m_cview == null)
                return;
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip = m_cview.NodeMenu;
                // Case null
                if (m_cview.ClickedNode == null)
                {
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_ID].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR1].Visible = false;

                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_SEPARATOR2 ].Visible = false;

                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_RIGHT_ARROW ].Visible = false;
                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_LEFT_ARROW ].Visible = false;
                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_BIDIR_ARROW ].Visible = false;
                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_CONSTANT_LINE ].Visible = false;
                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_SEPARATOR3 ].Visible = false;
                    
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CUT].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_COPY].Visible = false;
                    if (this.m_cview.PathwayView.CopiedNodes.Count > 0)
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_PASTE].Visible = true;
                    else
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_PASTE].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_WITH].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR4].Visible = false;
                    
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CREATE_LOGGER].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_LOGGER].Visible = false;
                }
                // Case PPathwayNode
                else if (m_cview.ClickedNode is PPathwayNode)
                {
                    PPathwayNode n = m_cview.ClickedNode as PPathwayNode;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_ID].Text = n.Element.Key;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_ID].Visible = true;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR1].Visible = true;
                    
                    if ((int)m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Tag > 0)
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Visible = true;
                    else
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Visible = false;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_LEFT_ARROW ].Visible = false;
                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_BIDIR_ARROW ].Visible = false;
                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_CONSTANT_LINE ].Visible = false;
                    m_cview.ContextMenuDict[ CanvasView.CANVAS_MENU_SEPARATOR3 ].Visible = false;
                    
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CUT].Visible = true;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_COPY].Visible = true;
                    if (this.m_cview.PathwayView.CopiedNodes.Count > 0)
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_PASTE].Visible = true;
                    else
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_PASTE].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE].Visible = true;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_WITH].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR4].Visible = true;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CREATE_LOGGER].Visible = true;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_LOGGER].Visible = true;

                    setMenuLogger(n);

                }
                else if (m_cview.ClickedNode is Line)
                {
                    EdgeInfo info = ((Line)m_cview.ClickedNode).Info;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_ID].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR1].Visible = false;

                    if ((int)m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Tag > 0)
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Visible = true;
                    else
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Visible = false;

                    switch (info.Direction)
                    {
                        case EdgeDirection.Inward:
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_RIGHT_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_LEFT_ARROW].Visible = false;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_BIDIR_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CONSTANT_LINE].Visible = true;
                            break;
                        case EdgeDirection.Outward:
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_LEFT_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_BIDIR_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CONSTANT_LINE].Visible = true;
                            break;
                        case EdgeDirection.Bidirection:
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_RIGHT_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_LEFT_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_BIDIR_ARROW].Visible = false;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CONSTANT_LINE].Visible = true;
                            break;
                        case EdgeDirection.None:
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_RIGHT_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_LEFT_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_BIDIR_ARROW].Visible = true;
                            m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CONSTANT_LINE].Visible = false;
                            break;
                    }
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_RIGHT_ARROW].Tag = m_cview.ClickedNode;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_LEFT_ARROW].Tag = m_cview.ClickedNode;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_BIDIR_ARROW].Tag = m_cview.ClickedNode;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CONSTANT_LINE].Tag = m_cview.ClickedNode;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR3].Visible = true;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CUT].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_COPY].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_PASTE].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE].Visible = true;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_WITH].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE].Tag = m_cview.ClickedNode;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR4].Visible = false;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CREATE_LOGGER].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_LOGGER].Visible = false;

                }
                else if (m_cview.ClickedNode is PEcellSystem)
                {
                    PEcellSystem n = m_cview.ClickedNode as PEcellSystem;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_ID].Text = n.Element.Key;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_ID].Visible = true;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR1].Visible = true;

                    if ((int)m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Tag > 0)
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Visible = true;
                    else
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Visible = false;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_LEFT_ARROW].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_BIDIR_ARROW].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CONSTANT_LINE].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR3].Visible = false;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CUT].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_COPY].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_PASTE].Visible = false;
                    if (n.Element.Key != "/")
                    {
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE].Visible = true;
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_WITH].Visible = true;
                        String superSys = n.Element.Key.Substring(0, n.Element.Key.LastIndexOf("/"));
                        if (superSys == "") superSys = "/";
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_WITH].Text =
                            m_resources.GetString("MergeMenuText") + "(" + superSys + ")";
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR4].Visible = true;
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE].Tag = m_cview.ClickedNode;
                    }
                    else
                    {
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE].Visible = false;
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_WITH].Visible = false;
                        m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR4].Visible = false;
                    }
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CREATE_LOGGER].Visible = true;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_LOGGER].Visible = true;

                    setMenuLogger(n);
                }
                else
                {
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_ID].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR1].Visible = false;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR2].Visible = false;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_RIGHT_ARROW].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_LEFT_ARROW].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_BIDIR_ARROW].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CONSTANT_LINE].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR3].Visible = false;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CUT].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_COPY].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_PASTE].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_WITH].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_SEPARATOR4].Visible = false;

                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CREATE_LOGGER].Visible = false;
                    m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_LOGGER].Visible = false;

                }
            }
            m_cview.ClickedNode = null;
        }

        private void setMenuLogger(PPathwayObject obj)
        {

            ToolStripMenuItem createLogger = (ToolStripMenuItem)m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_CREATE_LOGGER];
            ToolStripMenuItem deleteLogger = (ToolStripMenuItem)m_cview.ContextMenuDict[CanvasView.CANVAS_MENU_DELETE_LOGGER];
            createLogger.DropDown.Items.Clear();
            deleteLogger.DropDown.Items.Clear();

            if (obj.Element == null || obj.Element.ModelID == null)
                return;

            EcellObject ecellobj = DataManager.GetDataManager().GetEcellObject(obj.Element.ModelID, obj.Element.Key, obj.Element.Type);
            if (ecellobj == null)
                return;
            // set logger menu
            foreach (EcellData d in ecellobj.M_value)
            {
                if (d.M_isLogable)
                {
                    ToolStripItem sysLogger = new ToolStripMenuItem(d.M_name);
                    sysLogger.Text = d.M_name;
                    if (d.M_isLogger)
                    {
                        sysLogger.Click += new EventHandler(m_cview.DeleteLoggerClick);
                        deleteLogger.DropDown.Items.Add(sysLogger);
                    }
                    else
                    {
                        sysLogger.Click += new EventHandler(m_cview.CreateLoggerClick);
                        createLogger.DropDown.Items.Add(sysLogger);
                    }
                }
            }
            // 
            createLogger.Visible = !(createLogger.DropDown.Items.Count == 0);
            deleteLogger.Visible = !(deleteLogger.DropDown.Items.Count == 0);

        }
    }
}