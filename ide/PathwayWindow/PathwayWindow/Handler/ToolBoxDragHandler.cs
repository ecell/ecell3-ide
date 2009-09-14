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

using System.Drawing;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.Objects;
using UMD.HCIL.Piccolo.Event;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// ToolBoxDragHandler
    /// </summary>
    public class ToolBoxDragHandler : PDragSequenceEventHandler
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        Stencils m_stencils;
        /// <summary>
        /// PathwayControl
        /// </summary>
        PathwayControl m_con;
        /// <summary>
        /// Current canvas.
        /// </summary>
        CanvasControl m_canvas;
        /// <summary>
        /// Template object.
        /// </summary>
        PPathwayObject m_object = null;
        /// <summary>
        /// 
        /// </summary>
        bool m_eventFlag = true;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stencils"></param>
        public ToolBoxDragHandler(Stencils stencils)
        {
            m_stencils = stencils;
            m_con = stencils.PathwayControl;
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Event on mouse move.
        /// Move template object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            base.OnDrag(sender, e);

            if (m_canvas == null || m_object == null)
            {
                SetEventHandler((PToolBoxCanvas)e.Canvas, e);
                SetCurrentStencil((PToolBoxCanvas)e.Canvas);
                return;
            }
            // Check Position
            Rectangle recta = m_canvas.PCanvas.Bounds;
            recta.Location = m_con.PathwayView.GetDesktopLocation();
            Point systemPos = GetSystemPos(e);

            m_object.CenterPointF = m_canvas.SystemPosToCanvasPos(systemPos);
            m_object.RefreshView();
            if (recta.Contains(systemPos))
            {
                SetCursor(m_stencils.Stencil, Cursors.Arrow);
                m_canvas.ControlLayer.AddChild(m_object);
            }
            else
            {
                SetCursor(m_stencils.Stencil);
                m_object.RemoveFromParent();
            }

            // operation for System.
            if (!(m_object is PPathwaySystem))
                return;
            PPathwaySystem system = (PPathwaySystem)m_object;
            RectangleF rect = system.Rect;
            system.Invalid = m_canvas.DoesSystemOverlaps(rect) || !m_canvas.IsInsideRoot(rect);

        }

        /// <summary>
        /// Get system position.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Point GetSystemPos(PInputEventArgs e)
        {
            Point canvasPos = GetDesktopLocation(e.Canvas);
            float scale = e.Canvas.Camera.ViewScale;
            Point systemPos = new Point(canvasPos.X + (int)(e.Position.X * scale), canvasPos.Y + (int)(e.Position.Y * scale));
            return systemPos;
        }

        /// <summary>
        /// Get desktop location.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private Point GetDesktopLocation(Control control)
        {
            Point pos = control.Location;
            if (control.Parent != null)
            {
                Point temp = GetDesktopLocation(control.Parent);
                pos = new Point(temp.X + pos.X, temp.Y + pos.Y);
            }
            return pos;
        }

        /// <summary>
        /// Event on MouseStartDrag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartDrag(object sender, PInputEventArgs e)
        {
            base.OnStartDrag(sender, e);
            if (!(e.Canvas is PToolBoxCanvas))
                return;
            SetCurrentStencil((PToolBoxCanvas)e.Canvas);

            // Set EventHandler for current canvas.
            if (m_con.Canvas == null)
                return;
            if (!(e.Canvas is PToolBoxCanvas))
                return;
            SetEventHandler((PToolBoxCanvas)e.Canvas, e);
        }

        /// <summary>
        /// Event on mouse up.
        /// Check EventHandler status and Set/Reset EventHandler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(object sender, PInputEventArgs e)
        {
            base.OnMouseUp(sender, e);
            ResetCurrentStencil();

            if (m_canvas == null)
            {
                ResetEventHandler();
                return;
            }

            Point systemPos = GetSystemPos(e);

            // If ToolBox window contains the systemPos, return.
            Rectangle rect = m_con.Stencil.Bounds;
            rect.Location = m_con.Stencil.GetDesktopLocation();
            if (rect.Contains(systemPos))
            {
                ResetEventHandler();
                return;
            }
            // If PathwayView window contains the systemPos, add new object.
            rect = m_canvas.PCanvas.Bounds;
            rect.Location = m_con.PathwayView.GetDesktopLocation();
            // Add new object and reset EventHandler.
            if (rect.Contains(systemPos))
                AddObject();

            ResetEventHandler();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return m_eventFlag;
        }

        #endregion

        #region private Methods
        /// <summary>
        /// Add new Object.
        /// </summary>
        private void AddObject()
        {
            m_eventFlag = false;
            string systemKey = m_canvas.GetSurroundingSystemKey(m_object.CenterPointF);
            if (!(m_object is PPathwayText) && !(m_object is PPathwayStepper) && string.IsNullOrEmpty(systemKey))
            {
                Util.ShowErrorDialog(MessageResources.ErrOutRoot);
                return;
            }
            if (m_object is PPathwaySystem && m_canvas.DoesSystemOverlaps(m_object.Rect))
            {
                Util.ShowErrorDialog(MessageResources.ErrOverSystem);
                return;
            }
            string type = m_object.Setting.Type;
            bool isSystem = m_object is PPathwaySystem;
            // check
            if(isSystem)
            {
                RectangleF rect = ((PPathwaySystem)m_object).Rect;
                foreach (PPathwayVariable variable in m_canvas.Variables.Values)
                {
                    if (variable.Aliases.Count <= 0)
                        continue;
                    bool contein = rect.Contains(variable.CenterPointF);
                    foreach (PPathwayAlias alias in variable.Aliases)
                    {
                        if (rect.Contains(alias.CenterPointF) == contein)
                            continue;

                        Util.ShowErrorDialog(MessageResources.ErrOutSystemAlias);
                        return;
                    }
                }
            }

            // Add
            EcellObject eo = m_con.CreateDefaultObject(m_canvas.ModelID, systemKey, type);
            eo.X = m_object.X;
            eo.Y = m_object.Y;
            eo.Width = m_object.Width;
            eo.Height = m_object.Height;
            eo.Layout.Figure = m_object.Setting.Name;
            eo.isFixed = true;
            m_canvas.Control.NotifyDataAdd(eo, !isSystem);
            PPathwayObject temp = m_canvas.GetObject(eo.Key, eo.Type);
            if (temp != null)
                m_con.Canvas.NotifySelectChanged(temp);

            // Update System 
            if (!isSystem)
                return;
            PPathwayObject system = m_canvas.GetObject(eo.Key, eo.Type);
            if (system == null)
                return;
            // Move Object under new system.
            foreach (PPathwayObject obj in m_canvas.GetNodeList())
            {
                if (obj.EcellObject.ParentSystemID.StartsWith(eo.Key))
                    continue;
                if (!m_canvas.DoesSystemContains(eo.Key, obj.CenterPointF))
                    continue;

                string newKey = PathUtil.GetMovedKey(obj.EcellObject.Key, eo.ParentSystemID, eo.Key);
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    newKey,
                    obj,
                    true,
                    false);
            }
            m_canvas.Control.NotifyDataChanged(system, true);
        }

        /// <summary>
        /// Initialise EventHandler.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="e"></param>
        private void SetEventHandler(PToolBoxCanvas canvas, PInputEventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            if (m_object != null)
            {
                ResetEventHandler();
                ResetCurrentStencil();
            }
            m_con.Menu.ResetEventHandler();

            // Set Cursor
            SetCursor(canvas);

            // Set Template.
            m_object = canvas.Setting.CreateTemplate();
            m_object.Pickable = false;
            m_canvas = m_con.Canvas;

            Point systemPos = GetSystemPos(e);
            m_object.CenterPointF = m_canvas.SystemPosToCanvasPos(systemPos);
            m_object.Pickable = false;
        }

        private void SetCursor(PToolBoxCanvas canvas)
        {
            Bitmap image = (Bitmap)canvas.Setting.Icon;
            Cursor cursor = new Cursor(image.GetHicon());
            SetCursor(canvas, cursor);
        }

        private void SetCursor(PToolBoxCanvas canvas, Cursor cursor)
        {
            canvas.Cursor = cursor;
            m_con.Canvas.PCanvas.Cursor = cursor;
        }

        /// <summary>
        /// Reset EventHandler
        /// </summary>
        private void ResetEventHandler()
        {
            // Reset object
            if (m_object != null && m_object.Parent != null)
                m_object.RemoveFromParent();
            // Reset Cursor
            if (m_stencils.Stencil != null)
                m_stencils.Stencil.Cursor = Cursors.Arrow;
            if(m_canvas != null)
                m_canvas.PCanvas.Cursor = Cursors.Arrow;

            // Reset pointer.
            m_canvas = null;
            m_object = null;
            m_eventFlag = true;
        }

        private void SetCurrentStencil(PToolBoxCanvas canvas)
        {
            m_stencils.Stencil = canvas;
            m_stencils.Stencil.BackColor = Color.Yellow;
        }

        private void ResetCurrentStencil()
        {
            if (m_stencils.Stencil != null)
                m_stencils.Stencil.BackColor = Color.White;
        }
        #endregion
    }
}
