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
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
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

            if (m_canvas == null)
                return;
            if (m_object == null)
                return;

            Point systemPos = GetSystemPos(e);
            m_object.CenterPointF = m_canvas.SystemPosToCanvasPos(systemPos);
            m_object.RefreshView();
            m_canvas.ControlLayer.AddChild(m_object);

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
            
            // Set EventHandler for current canvas.
            if (m_con.Canvas == null)
                return;
            if (!(e.Canvas is PToolBoxCanvas))
                return;
            SetEventHandler((PToolBoxCanvas)e.Canvas, e);
            // m_con.Canvas.PCanvas.Cursor = Cursors.Hand;
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
            if (m_canvas == null)
                return;

            Point systemPos = GetSystemPos(e);

            // If ToolBox window contains the systemPos, return.
            Rectangle rect = m_con.ToolBox.Bounds;
            rect.Location = m_con.ToolBox.GetDesktopLocation();
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
            return true;
        }

        #endregion

        #region private Methods
        /// <summary>
        /// Add new Object.
        /// </summary>
        private void AddObject()
        {
            string systemKey = m_canvas.GetSurroundingSystemKey(m_object.CenterPointF);
            if (!(m_object is PPathwayText) && !(m_object is PPathwayStepper) && string.IsNullOrEmpty(systemKey))
                return;
            if (m_object is PPathwaySystem && m_canvas.DoesSystemOverlaps(m_object.Rect))
            {
                Util.ShowErrorDialog(MessageResources.ErrOverSystem);
                return;
            }
            string type = GetType(m_object);
            bool isSystem = m_object is PPathwaySystem;
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
        /// get object type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetType(PPathwayObject obj)
        {
            if (obj is PPathwaySystem)
                return EcellObject.SYSTEM;
            else if (obj is PPathwayProcess)
                return EcellObject.PROCESS;
            else if (obj is PPathwayVariable)
                return EcellObject.VARIABLE;
            else if (obj is PPathwayText)
                return EcellObject.TEXT;
            else if (obj is PPathwayStepper)
                return EcellObject.STEPPER;
            else
                return null;
        }
        /// <summary>
        /// Initialise EventHandler.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="e"></param>
        private void SetEventHandler(PToolBoxCanvas canvas, PInputEventArgs e)
        {
            //
            m_stencils.Stencil = canvas;
            canvas.BackColor = Color.Yellow;
            m_object = canvas.Setting.CreateTemplate();
            m_canvas = m_con.Canvas;

            Point systemPos = GetSystemPos(e);
            m_object.CenterPointF = m_canvas.SystemPosToCanvasPos(systemPos);
            m_object.Pickable = false;
            m_canvas.ControlLayer.AddChild(m_object);
        }

        /// <summary>
        /// Reset EventHandler
        /// </summary>
        private void ResetEventHandler()
        {
            if(m_canvas.ControlLayer.ChildrenReference.Contains(m_object))
                m_canvas.ControlLayer.RemoveChild(m_object);
            m_canvas = null;
            m_object = null;
            if(m_stencils.Stencil != null)
                m_stencils.Stencil.BackColor = Color.White;
        }
        #endregion
    }
}
