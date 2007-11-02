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

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// EcventHandler when the node object is dragged.
    /// </summary>
    public class NodeDragHandler : PDragEventHandler
    {
        #region Fields
        /// <summary>
        /// the related CanvasViewCompomnetSet.
        /// </summary>
        private CanvasControl m_canvas;

        /// <summary>
        /// dictionary of system container that key is name.
        /// </summary>
        private Dictionary<string, PPathwaySystem> m_systems;

        /// <summary>
        /// Edges will be refreshed every time when this process has moved by this distance.
        /// </summary>
        private static readonly float m_refreshDistance = 4;

        /// <summary>
        /// Whether a node is moved or not.
        /// </summary>
        private bool m_isMoved = false;

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
        /// <param name="dict">dictionary of system.</param>
        public NodeDragHandler(CanvasControl canvas, Dictionary<string, PPathwaySystem> dict)
        {
            m_canvas = canvas;
            m_systems = dict;
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

            if (e.PickedNode is PPathwayProcess)
                ((PPathwayProcess)e.PickedNode).RefreshEdges();

            if (e.PickedNode is PPathwayNode)
            {
                foreach (PPathwayObject node in m_canvas.SelectedNodes)
                {
                    node.MemorizePosition();
                    m_canvas.ControlLayer.AddChild(node);
                }
            }
            else if (e.PickedNode is PPathwaySystem)
                ((PPathwaySystem)e.PickedNode).MemorizePosition();

            e.Canvas.BackColor = Color.Silver;
            SetBackToDefault();
            SetShadeWithoutSystem(m_canvas.GetSurroundingSystemKey(e.Position));
        }

        /// <summary>
        /// Set background color of pathway view to default color;
        /// </summary>
        private void SetBackToDefault()
        {
            foreach (PPathwaySystem system in m_systems.Values)
                system.BackgroundBrush = null;

            m_canvas.PathwayCanvas.BackColor = Color.White;
        }

        /// <summary>
        /// Set background color of pathway view without given systemName to shading color
        /// </summary>
        /// <param name="systemName">Only this system's color remain default state.</param>
        private void SetShadeWithoutSystem(string systemName)
        {
            if (systemName != null && m_systems.ContainsKey(systemName))
            {
                m_canvas.PathwayCanvas.BackColor = Color.Silver;
                m_systems[systemName].BackgroundBrush = Brushes.White;

                foreach (PPathwaySystem system in m_systems.Values)
                    if (!system.EcellObject.key.Equals(systemName))
                        system.BackgroundBrush = Brushes.Silver;
            }
            else
            {
                m_canvas.PathwayCanvas.BackColor = Color.White;
                foreach (PPathwaySystem system in m_systems.Values)
                    system.BackgroundBrush = Brushes.Silver;
            }
        }
        /// <summary>
        /// event on start to drag PNode.
        /// </summary>
        /// <param name="sender">PathwayView.</param>
        /// <param name="e">PInputEventArgs.</param>
        protected override void OnStartDrag(object sender, PInputEventArgs e)
        {
            base.OnStartDrag(sender, e);
            m_isMoved = false;
            if (e.PickedNode is PPathwaySystem)
            {
                PPathwaySystem system = (PPathwaySystem)e.PickedNode;
            }
            e.Handled = true;
            if (e.PickedNode.ChildrenCount != 1 || !(e.PickedNode.ChildrenReference[0] is PPathwaySystem))
            {
                e.PickedNode.MoveToFront();
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
            m_isMoved = true;
            e.Canvas.BackColor = Color.Silver;
            SetBackToDefault();
            SetShadeWithoutSystem(m_canvas.GetSurroundingSystemKey(e.Position));
            if (e.PickedNode is PPathwaySystem)
            {
                PPathwaySystem system = (PPathwaySystem)e.PickedNode;
                system.Refresh();
                // Change color if the system overlaps other system
                if (m_canvas.DoesSystemOverlaps(system.GlobalBounds, system.EcellObject.key)
                    || !IsInsideRoot(system.Rect))
                    system.IsInvalid = true;
                else
                    system.IsInvalid = false;
                m_canvas.UpdateResizeHandlePositions();
                system.MoveStart();
            }
            else if (e.PickedNode is PPathwayNode)
            {
                PointF offset = e.PickedNode.Offset;
                m_movingDelta += e.CanvasDelta;

                if ((Math.Abs(m_movingDelta.Width) + Math.Abs(m_movingDelta.Height)) > m_refreshDistance)
                {
                    m_movingDelta = SizeF.Empty;
                    foreach (PPathwayObject node in m_canvas.SelectedNodes)
                    {
                        node.Offset = offset;
                        node.Refresh();
                    }
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
            RectangleF rootRect = m_systems["/"].Rect;
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
            if (e.PickedNode is PPathwayNode)
            {
                foreach (PPathwayObject node in m_canvas.SelectedNodes)
                {
                    if (m_canvas.ControlLayer.ChildrenReference.Contains(node))
                        m_canvas.ControlLayer.RemoveChild(node);
                    TransferNode((PPathwayNode)node, m_isMoved);
                }
            }
            else if (e.PickedNode is PPathwaySystem)
            {
                PPathwaySystem system = (PPathwaySystem)e.PickedNode;
                string oldSysKey = system.EcellObject.key;
                string parentSysKey = m_canvas.GetSurroundingSystemKey(e.Position, oldSysKey);
                string newSysKey = null;
                if (parentSysKey == null)
                    newSysKey = "/";
                else if (parentSysKey.Equals("/"))
                    newSysKey = "/" + system.EcellObject.name;
                else
                    newSysKey = parentSysKey + "/" + system.EcellObject.name;

                // Reset system movement when the system is overlapping other system or out of root.
                if (m_canvas.DoesSystemOverlaps(system.GlobalBounds, oldSysKey) || !IsInsideRoot(system.Rect))
                {
                    system.ResetPosition();
                    m_canvas.UpdateResizeHandlePositions();
                    system.IsInvalid = false;
                }
                // Reset if system is duplicated.
                else if (!oldSysKey.Equals(newSysKey) && m_systems.ContainsKey(newSysKey))
                {
                    MessageBox.Show(newSysKey + m_resources.GetString("ErrAlrExist"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    system.ResetPosition();
                    m_canvas.UpdateResizeHandlePositions();
                    system.IsInvalid = false;
                }
                // Transfer system.
                else
                {
                    TransferSystemTo(newSysKey, oldSysKey, system);
                }
                system.MoveEnd();
                system.Refresh();
            }
            SetBackToDefault();
            m_canvas.UpdateOverview();
        }

        /// <summary>
        /// Transfer an system from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="node">transfered node</param>
        /// <param name="oldPosition">new key of a system to be transfered</param>
        /// <param name="newPosition">old key of a system to be transfered</param>
        /// <param name="toBeNotified">old key of a system to be transfered</param>
        private void TransferNode(PPathwayObject node, bool toBeNotified)
        {
            node.ParentObject.AddChild(node);
            PointF newPosition = new PointF(node.X + node.OffsetX, node.Y + node.OffsetY);
            string oldSystem = node.EcellObject.parentSystemID;
            string newSystem = m_canvas.GetSurroundingSystemKey(newPosition);
            string newKey = newSystem + ":" + node.EcellObject.name;

            // When node is out of root.
            if (newSystem == null)
            {
                node.ResetPosition();
                node.Refresh();
                return;
            }
            // When node is duplicated.
            else if (!newSystem.Equals(oldSystem)
                && m_canvas.GetSelectedObject(newKey, node.EcellObject.type) != null)
            {
                node.ResetPosition();
                MessageBox.Show(node.EcellObject.name + m_resources.GetString("ErrAlrExist"),
                                "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                node.PointF = newPosition;
                node.Offset = PointF.Empty;

                if (!toBeNotified)
                    return;
                // Update Node.
                m_canvas.PathwayControl.NotifyDataChanged(
                    node.EcellObject.key,
                    newKey,
                    node,
                    true,
                    true);

            }
        }

        /// <summary>
        /// Transfer an system from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="newKey">new key of a system to be transfered</param>
        /// <param name="oldKey">old key of a system to be transfered</param>
        /// <param name="system">transfered system</param>
        private void TransferSystemTo(string newKey, string oldKey, PPathwaySystem system)
        {
            PointF offset = system.Offset;
            if (offset.X == 0 && offset.Y == 0)
                return;

            // Move objects under this system.
            // TODO: This process should be implemented in EcellLib.DataChanged().
            foreach (PPathwayObject obj in m_canvas.GetAllObjectUnder(oldKey))
            {
                obj.X = obj.X + offset.X;
                obj.Y = obj.Y + offset.Y;
                obj.Offset = PointF.Empty;
                m_canvas.PathwayControl.NotifyDataChanged(
                    obj.EcellObject.key,
                    obj.EcellObject.key,
                    obj,
                    true,
                    false);
            }

            // Move system.
            system.X = system.X + offset.X;
            system.Y = system.Y + offset.Y;
            system.Offset = PointF.Empty;
            m_canvas.PathwayControl.NotifyDataChanged(
                oldKey,
                newKey,
                system,
                true,
                true);

            m_canvas.ResetSelectedObjects();
        }
    }
}
