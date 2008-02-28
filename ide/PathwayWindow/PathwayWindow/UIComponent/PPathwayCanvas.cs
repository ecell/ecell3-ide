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
            this.ContextMenuStrip = m_con.Menu.PopupMenu;
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

            // Set popup menu visibility flags.
            bool isPPathwayObject = (m_canvas.FocusNode is PPathwayObject);
            bool isPPathwayNode = (m_canvas.FocusNode is PPathwayNode);
            bool isPPathwaySystem = (m_canvas.FocusNode is PPathwaySystem);
            bool isRoot = false;
            bool isLine = (m_canvas.FocusNode is PPathwayLine);
            bool isCopiedObject = (m_con.CopiedNodes.Count > 0);
            bool isLayoutMenu = (m_con.Menu.LayoutMenus.Count > 0);

            // Set popup menu text.
            if (isPPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)m_canvas.FocusNode;
                m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuID].Text = obj.EcellObject.Key;
                SetLoggerMenu(obj);
                SetLayerManu(obj);
                if (isPPathwaySystem)
                {
                    m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuMerge].Text =
                        m_resources.GetString(MenuConstants.CanvasMenuMerge) + "(" + obj.EcellObject.ParentSystemID + ")";
                }
                if (obj.EcellObject.Key.Equals("/"))
                    isRoot = true;
            }

            // Show ObjectID(key).
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuID].Visible = isPPathwayObject;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuSeparator1].Visible = isPPathwayObject;
            // Show Layout menus.
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuLayout].Visible = isLayoutMenu;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuSeparator2].Visible = isLayoutMenu && (isPPathwayObject || isLine || isCopiedObject);
            // Show Line menus.
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuRightArrow].Visible = isLine;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuLeftArrow].Visible = isLine;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuBidirArrow].Visible = isLine;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuConstantLine].Visible = isLine;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuSeparator3].Visible = isLine;
            // Show Node / System edit menus.
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuCut].Visible = isPPathwayObject && !isRoot;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuCopy].Visible = isPPathwayObject && !isRoot;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuPaste].Visible = isCopiedObject;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuDelete].Visible = isPPathwayObject && !isRoot;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuMerge].Visible = isPPathwaySystem && !isRoot;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuSeparator4].Visible = isCopiedObject || (isPPathwayObject && !isRoot);
            // Show Layer menu.
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuChangeLayer].Visible = isPPathwayObject && !isRoot;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuMoveFront].Visible = isPPathwayObject && !isRoot;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuMoveBack].Visible = isPPathwayObject && !isRoot;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuSeparator5].Visible = isPPathwayObject && !isRoot;
            // Show Logger menu.
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuCreateLogger].Visible = isPPathwayObject;
            m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuDeleteLogger].Visible = isPPathwayObject;
        }

        /// <summary>
        /// Set logger menu items.
        /// </summary>
        /// <param name="obj">Selected PPathwayObject.</param>
        private void SetLoggerMenu(PPathwayObject obj)
        {

            ToolStripMenuItem createLogger = (ToolStripMenuItem)m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuCreateLogger];
            ToolStripMenuItem deleteLogger = (ToolStripMenuItem)m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuDeleteLogger];
            createLogger.DropDown.Items.Clear();
            deleteLogger.DropDown.Items.Clear();

            if (obj.EcellObject == null || obj.EcellObject.ModelID == null)
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
                createItem.Click += new EventHandler(m_con.Menu.CreateLoggerClick);
                createLogger.DropDown.Items.Add(createItem);

                // create "Delete Logger" menu.
                if (!data.Logged)
                    continue;
                ToolStripItem deleteItem = new ToolStripMenuItem(data.Name);
                deleteItem.Text = data.Name;
                deleteItem.Click += new EventHandler(m_con.Menu.DeleteLoggerClick);
                deleteLogger.DropDown.Items.Add(deleteItem);
            }
            createLogger.Enabled = (createLogger.DropDown.Items.Count != 0);
            deleteLogger.Enabled = (deleteLogger.DropDown.Items.Count != 0);
        }

        /// <summary>
        /// Set layer menu items.
        /// </summary>
        /// <param name="obj"></param>
        private void SetLayerManu(PPathwayObject obj)
        {
            ToolStripMenuItem layerMenu = (ToolStripMenuItem)m_con.Menu.PopupMenuDict[MenuConstants.CanvasMenuChangeLayer];
            layerMenu.DropDown.Items.Clear();
            foreach (string layerName in m_canvas.GetLayerNameList())
            {
                ToolStripMenuItem layerItem = new ToolStripMenuItem(layerName);
                layerItem.Checked = layerName.Equals(obj.Layer.Name);
                layerItem.Click += new EventHandler(m_con.Menu.ChangeLeyerClick);
                layerMenu.DropDown.Items.Add(layerItem);
            }
            ToolStripSeparator separator = new ToolStripSeparator();
            layerMenu.DropDown.Items.Add(separator);

            ToolStripMenuItem createNewLayer = new ToolStripMenuItem(m_resources.GetString(MenuConstants.CanvasMenuCreateLayer));
            createNewLayer.Click += new EventHandler(m_con.Menu.ChangeLeyerClick);
            layerMenu.DropDown.Items.Add(createNewLayer);
        }
    }
}