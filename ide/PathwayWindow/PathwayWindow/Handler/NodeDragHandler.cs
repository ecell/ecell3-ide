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
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Nodes;
using System.Drawing;
using EcellLib.PathwayWindow.Nodes;
using System.ComponentModel;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;
using EcellLib.PathwayWindow.Resources;
using EcellLib.Objects;

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// EcventHandler when the node object is dragged.
    /// </summary>
    public class NodeDragHandler : ObjectDragHandler
    {
        #region Fields
        /// <summary>
        /// the related CanvasControl.
        /// </summary>
        private CanvasControl m_canvas;

        /// <summary>
        /// the delta of moving.
        /// </summary>
        private SizeF m_movingDelta = new SizeF(0, 0);

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));
        #endregion

        /// <summary>
        /// constructor with initial parameters.
        /// </summary>
        /// <param name="canvas">canvas control.</param>
        public NodeDragHandler(CanvasControl canvas)
        {
            m_canvas = canvas;
        }

        /// <summary>
        /// check whether this event is mouse event.
        /// </summary>
        /// <param name="e">event.</param>
        /// <returns>mouse event is true.</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.IsMouseEvent
               && (e.Button == MouseButtons.Left || e.IsMouseEnterOrMouseLeave);
        }

        /// <summary>
        /// event on down the mouse button in canvas.
        /// </summary>
        /// <param name="sender">PathwayView.</param>
        /// <param name="e">PInputEventArgs.</param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);
            if (!(e.PickedNode is PPathwayObject))
                return;
            PPathwayObject obj = e.PickedNode as PPathwayObject;

            if (obj is PPathwayNode)
            {
                foreach (PPathwayObject node in m_canvas.SelectedNodes)
                    node.MemorizePosition();
            }
            else if (obj is PPathwaySystem)
                obj.MemorizePosition();

            //e.Canvas.BackColor = Color.Silver;
            //SetBackToDefault();
            //SetShadeWithoutSystem(m_canvas.GetSurroundingSystemKey(obj.PointF));
        }

        /// <summary>
        /// Set background color of pathway view to default color;
        /// </summary>
        private void SetBackToDefault()
        {
            m_canvas.PCanvas.BackColor = Color.White;
            foreach (PPathwaySystem system in m_canvas.Systems.Values)
                system.BackgroundBrush = null;
        }

        /// <summary>
        /// Set background color of pathway view without given systemName to shading color
        /// </summary>
        /// <param name="systemName">Only this system's color remain default state.</param>
        private void SetShadeWithoutSystem(string systemName)
        {
            if (systemName != null && m_canvas.Systems.ContainsKey(systemName))
            {
                m_canvas.PCanvas.BackColor = Color.Silver;
                m_canvas.Systems[systemName].BackgroundBrush = Brushes.White;

                foreach (PPathwaySystem system in m_canvas.Systems.Values)
                    if (!system.EcellObject.Key.Equals(systemName))
                        system.BackgroundBrush = Brushes.Silver;
            }
            else
            {
                m_canvas.PCanvas.BackColor = Color.White;
                foreach (PPathwaySystem system in m_canvas.Systems.Values)
                    system.BackgroundBrush = Brushes.Silver;
            }
        }

        /// <summary>
        /// event on drag PNode.
        /// </summary>
        /// <param name="sender">PathwayView.</param>
        /// <param name="e">PInputEventArgs.</param>
        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            base.OnDrag(sender, e);
            //e.Canvas.BackColor = Color.Silver;
            //SetBackToDefault();

            if (!(e.PickedNode is PPathwayObject))
                return;
            PPathwayObject obj = e.PickedNode as PPathwayObject;
            //SetShadeWithoutSystem(m_canvas.GetSurroundingSystemKey(obj.PointF));
            // Move Nodes.
            if (obj is PPathwaySystem)
            {
                PPathwaySystem system = (PPathwaySystem)obj;
                system.Refresh();
                // Change color if the system overlaps other system
                if (m_canvas.DoesSystemOverlaps(system) || !IsInsideRoot(system.Rect))
                    system.IsInvalid = true;
                else
                    system.IsInvalid = false;
                m_canvas.ResizeHandler.UpdateResizeHandlePositions();
                foreach (PPathwayObject child in m_canvas.GetAllObjectUnder(system.EcellObject.Key))
                {
                    child.Offset = obj.Offset;
                    child.Refresh();
                }

            }
            else if (obj is PPathwayNode)
            {
                foreach (PPathwayObject child in m_canvas.SelectedNodes)
                {
                    child.Offset = obj.Offset;
                    child.Refresh();
                }
            }
        }

        /// <summary>
        /// Whether given rectangle is inside the root system or not.
        /// </summary>
        /// <param name="rectF">a rectangle to be checked.</param>
        /// <returns>True, if the given rectangle is inside the root system.
        ///          False, if the given rectangle is outside the root system.
        /// </returns>
        private bool IsInsideRoot(RectangleF rectF)
        {
            RectangleF rootRect = m_canvas.Systems["/"].Rect;
            if (rootRect.Contains(rectF))
                return true;
            else
                return false;
        }
        /// <summary>
        /// event on end to drag PNode in PathwayView.
        /// </summary>
        /// <param name="sender">Pathwayview.</param>
        /// <param name="e">PInputEventArgs.</param>
        protected override void OnEndDrag(object sender, PInputEventArgs e)
        {
            base.OnEndDrag(sender, e);

            if (e.PickedNode.OffsetX == 0 && e.PickedNode.OffsetY == 0)
                return;
            // Move Nodes
            if (e.PickedNode is PPathwayNode)
            {
                TransferNodes(m_canvas.SelectedNodes);
            }
            // Move System.
            else if (e.PickedNode is PPathwaySystem)
            {
                PPathwaySystem system = (PPathwaySystem)e.PickedNode;
                string oldSysKey = system.EcellObject.Key;
                string parentSysKey = m_canvas.GetSurroundingSystemKey(system.PointF, oldSysKey);
                string newSysKey = null;
                if (parentSysKey == null)
                    newSysKey = "/";
                else if (parentSysKey.Equals("/"))
                    newSysKey = "/" + system.EcellObject.Name;
                else
                    newSysKey = parentSysKey + "/" + system.EcellObject.Name;

                // Reset system movement when the system is overlapping other system or out of root.
                if (m_canvas.DoesSystemOverlaps(system) || !IsInsideRoot(system.Rect))
                {
                    system.ResetPosition();
                    m_canvas.ResizeHandler.UpdateResizeHandlePositions();
                    system.IsInvalid = false;
                }
                // Reset if system is duplicated.
                else if (!oldSysKey.Equals(newSysKey) && m_canvas.Systems.ContainsKey(newSysKey))
                {
                    Util.__showErrorDialog(newSysKey + m_resources.GetString(MessageConstants.ErrAlrExist));
                    system.ResetPosition();
                    m_canvas.ResizeHandler.UpdateResizeHandlePositions();
                    system.IsInvalid = false;
                }
                // Transfer system.
                else
                {
                    TransferSystemTo(newSysKey, oldSysKey, system);
                }
                system.RefreshView();
            }
            // Move Text
            else if (e.PickedNode is PPathwayText)
            {
                PPathwayText text = (PPathwayText)e.PickedNode;
                text.NotifyDataChanged();
            }
            //SetBackToDefault();
            m_canvas.PCanvas.Refresh();
            m_canvas.UpdateOverview();
        }

        /// <summary>
        /// Transfer an system from one System/Layer to other System/Layer.
        /// </summary>
        /// <param name="nodeList">Transfered nodes</param>
        private void TransferNodes(List<PPathwayObject> nodeList)
        {
            PointF newPosition;
            string newSystem;
            string newKey;
            bool isError = false;
            List<PPathwayObject> processList = new List<PPathwayObject>();
            List<PPathwayObject> VariableList = new List<PPathwayObject>();

            foreach (PPathwayObject node in nodeList)
            {
                m_canvas.SetLayer(node);
                newPosition = new PointF(node.X + node.OffsetX, node.Y + node.OffsetY);
                newSystem = m_canvas.GetSurroundingSystemKey(newPosition);
                newKey = newSystem + ":" + node.EcellObject.Name;

                // When node is out of root.
                if (newSystem == null)
                {
                    Util.__showErrorDialog(node.EcellObject.Name + ":" + m_resources.GetString(MessageConstants.ErrOutRoot));
                    isError = true;
                    continue;
                }
                // When node is duplicated.
                else if (!newSystem.Equals(node.EcellObject.ParentSystemID)
                    && m_canvas.GetSelectedObject(newKey, node.EcellObject.Type) != null)
                {
                    Util.__showErrorDialog(node.EcellObject.Name + ":" + m_resources.GetString(MessageConstants.ErrAlrExist));
                    isError = true;
                    continue;
                }
                // No error.
                else
                {
                    node.PointF = newPosition;
                    node.Offset = PointF.Empty;
                }
                // Set NodeList.
                if (node is PPathwayProcess)
                    processList.Add(node);
                else if (node is PPathwayVariable)
                    VariableList.Add(node);
            }

            // If error, reset node position.
            if (isError)
            {
                foreach (PPathwayObject node in nodeList)
                {
                    node.ResetPosition();
                    node.Refresh();
                }
                return;
            }

            // Set Position
            processList.AddRange(VariableList);
            int i = 0;
            foreach (PPathwayObject node in processList)
            {
                i++;
                node.Refresh();
                newSystem = m_canvas.GetSurroundingSystemKey(node.PointF);
                newKey = newSystem + ":" + node.EcellObject.Name;
                m_canvas.Control.NotifyDataChanged(
                    node.EcellObject.Key,
                    newKey,
                    node,
                    true,
                    (i == nodeList.Count));
            }
            m_canvas.ResetSelectedObjects();
        }

        /// <summary>
        /// Transfer an system from one System/Layer to other System/Layer.
        /// </summary>
        /// <param name="newKey">new key of a system to be transfered</param>
        /// <param name="oldKey">old key of a system to be transfered</param>
        /// <param name="system">transfered system</param>
        private void TransferSystemTo(string newKey, string oldKey, PPathwaySystem system)
        {
            PointF offset = system.Offset;
            if (offset.X <= 5 && offset.Y <= 5)
            {
                m_canvas.Refresh();
                return;
            }

            // Move objects under this system.
            // TODO: This process should be implemented in EcellLib.DataChanged().
            foreach (PPathwayObject obj in m_canvas.GetAllObjectUnder(oldKey))
            {
                obj.X = obj.X + offset.X;
                obj.Y = obj.Y + offset.Y;
                obj.Offset = PointF.Empty;
                m_canvas.Control.NotifyDataChanged(
                    obj.EcellObject.Key,
                    obj.EcellObject.Key,
                    obj,
                    true,
                    false);
            }

            // Move system path.
            m_canvas.Control.NotifyDataChanged(
                oldKey,
                newKey,
                system,
                true,
                false);

            // Import Systems and Nodes
            RectangleF rect = system.Rect;
            rect.X = rect.X + offset.X;
            rect.Y = rect.Y + offset.Y;
            string parentSystemName = system.EcellObject.ParentSystemID;
            foreach (PPathwayObject obj in m_canvas.GetAllObjects())
            {
                if (obj == system)
                    continue;
                if (obj.EcellObject.ParentSystemID.StartsWith(newKey))
                    continue;
                if (obj is PPathwaySystem && !rect.Contains(obj.Rect))
                    continue;
                if (obj is PPathwayNode && !rect.Contains(obj.CenterPointF))
                    continue;

                string newNodeKey = PathUtil.GetMovedKey(obj.EcellObject.Key, parentSystemName, newKey);
                m_canvas.Control.NotifyDataChanged(
                    obj.EcellObject.Key,
                    newNodeKey,
                    obj,
                    true,
                    false);
            }

            // Move system.
            system.X = system.X + offset.X;
            system.Y = system.Y + offset.Y;
            system.Offset = PointF.Empty;
            m_canvas.Control.NotifyDataChanged(
                newKey,
                newKey,
                system,
                true,
                true);
            m_canvas.ResetSelectedObjects();
        }
    }
}
