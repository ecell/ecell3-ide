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
using EcellLib.PathwayWindow.Resources;

namespace EcellLib.PathwayWindow.UIComponent
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
        /// ResourceManager
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas">CanvasControl instance</param>
        public PPathwayCanvas(CanvasControl canvas)
        {
            m_canvas = canvas;
            m_con = canvas.PathwayControl;
            // Preparing context menus.
            this.ContextMenuStrip = m_con.MenuControl.PopupMenu;
            //
            this.RemoveInputEventListener(PanEventHandler);
            this.RemoveInputEventListener(ZoomEventHandler);
            this.Dock = DockStyle.Fill;
            this.Name = canvas.ModelID;
            this.Camera.ScaleViewBy(0.7f);

        }

        /// <summary>
        /// Called when the canvas has been resized.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (m_canvas == null)
                return;
            m_canvas.UpdateOverview();
        }
        /// <summary>
        /// </summary>
        /// <param name="e">MouseEventArgs.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //base.OnMouseWheel(e);
            if (Control.ModifierKeys == Keys.Shift)
            {
                m_canvas.PanCanvas(Direction.Horizontal, e.Delta);
            }
            else if (Control.ModifierKeys == Keys.Control || e.Button == MouseButtons.Right)
            {
                float zoom = (float)1.00 + (float)e.Delta / 1200;
                m_canvas.Zoom(zoom);
            }
            else
            {
                m_canvas.PanCanvas(Direction.Vertical, e.Delta);
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
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (m_con == null)
                return;
            if (e.Button != MouseButtons.Right)
                return;

            // Set popup menu visibility.
            bool isPPathwayObject = (m_canvas.FocusNode is PPathwayObject);
            bool isPPathwayNode = (m_canvas.FocusNode is PPathwayNode);
            bool isPPathwaySystem = (m_canvas.FocusNode is PPathwaySystem);
            bool isRoot = false;
            bool isLine = (m_canvas.FocusNode is PPathwayLine);
            bool isCopiedObject = (m_con.CopiedNodes.Count > 0);
            bool isLayoutMenu = (m_con.MenuControl.LayoutMenus.Count > 0);

            // Set popup menu text.
            if (isPPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)m_canvas.FocusNode;
                m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuID].Text = obj.EcellObject.key;
                SetLoggerMenu(obj);
                if (isPPathwaySystem)
                {
                    m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuMerge].Text =
                        m_resources.GetString(MenuControl.CanvasMenuMerge) + "(" + obj.EcellObject.parentSystemID + ")";
                }
                if (obj.EcellObject.key.Equals("/"))
                    isRoot = true;
            }

            // Show ObjectID(key).
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuID].Visible = isPPathwayObject;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuSeparator1].Visible = isPPathwayObject;
            // Show Layout menus.
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuLayout].Visible = isLayoutMenu;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuSeparator2].Visible = isLayoutMenu && (isPPathwayObject || isLine || isCopiedObject);
            // Show Line menus.
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuRightArrow].Visible = isLine;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuLeftArrow].Visible = isLine;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuBidirArrow].Visible = isLine;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuConstantLine].Visible = isLine;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuSeparator3].Visible = isLine;
            // Show Node / System edit menus.
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuCut].Visible = isPPathwayObject && !isRoot;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuCopy].Visible = isPPathwayObject && !isRoot;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuPaste].Visible = isCopiedObject;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuDelete].Visible = isPPathwayObject && !isRoot;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuMerge].Visible = isPPathwaySystem && !isRoot;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuSeparator4].Visible = isCopiedObject || (isPPathwayObject && !isRoot);
            // Show Layer menu.
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuChangeLayer].Visible = isPPathwayNode;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuSeparator5].Visible = isPPathwayNode;
            // Show Logger menu.
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuCreateLogger].Visible = isPPathwayObject;
            m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuDeleteLogger].Visible = isPPathwayObject;
        }

        /// <summary>
        /// Set logger menu items.
        /// </summary>
        /// <param name="obj">Selected PPathwayObject.</param>
        private void SetLoggerMenu(PPathwayObject obj)
        {

            ToolStripMenuItem createLogger = (ToolStripMenuItem)m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuCreateLogger];
            ToolStripMenuItem deleteLogger = (ToolStripMenuItem)m_con.MenuControl.PopupMenuDict[MenuControl.CanvasMenuDeleteLogger];
            createLogger.DropDown.Items.Clear();
            deleteLogger.DropDown.Items.Clear();

            if (obj.EcellObject == null || obj.EcellObject.modelID == null)
                return;
            // set logger menu
            EcellObject eo = obj.EcellObject;
            foreach (EcellData data in eo.Value)
            {
                // Create "Create Logger" menu.
                if (!data.Logable)
                    continue;
                ToolStripItem createItem = new ToolStripMenuItem(data.Name);
                createItem.Text = data.Name;
                createItem.Click += new EventHandler(m_con.CreateLoggerClick);
                createLogger.DropDown.Items.Add(createItem);

                // create "Delete Logger" menu.
                if (!data.Logged)
                    continue;
                ToolStripItem deleteItem = new ToolStripMenuItem(data.Name);
                deleteItem.Text = data.Name;
                deleteItem.Click += new EventHandler(m_con.DeleteLoggerClick);
                deleteLogger.DropDown.Items.Add(deleteItem);
            }
            createLogger.Enabled = (createLogger.DropDown.Items.Count != 0);
            deleteLogger.Enabled = (deleteLogger.DropDown.Items.Count != 0);
        }
    }
}