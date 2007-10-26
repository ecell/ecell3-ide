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
        /// PComposite to move selected nodes together.
        /// </summary>
        private PComposite m_composite;

        /// <summary>
        /// Point, where the mouse is down.
        /// </summary>
        private PointF m_downPosition;

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
                PPathwayNode pnode = (PPathwayNode)e.PickedNode;
                pnode.MemorizePosition();
                m_canvas.ControlLayer.AddChild(pnode);

                m_downPosition = new PointF(e.Position.X, e.Position.Y);
                m_composite = new PComposite();
                m_canvas.ControlLayer.AddChild(m_composite);
                foreach (PPathwayObject node in m_canvas.SelectedNodes)
                {
                    if (node == pnode)
                        continue;

                    node.MemorizePosition();
                    m_composite.AddChild(node);
                }
            }
            else if (e.PickedNode is PPathwaySystem)
                ((PPathwaySystem)e.PickedNode).MemorizePosition();
            else
                m_canvas.ClickedNode = null;

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
                    || !m_canvas.IsInsideRoot(system.Rect))
                    system.IsInvalid = true;
                else
                    system.IsInvalid = false;
                m_canvas.UpdateResizeHandlePositions();

                system.MoveStart();
            }
            else if (e.PickedNode is PPathwayNode)
            {
                m_composite.OffsetX += e.Delta.Width;
                m_composite.OffsetY += e.Delta.Height;

                m_movingDelta += e.CanvasDelta;

                if ((Math.Abs(m_movingDelta.Width) + Math.Abs(m_movingDelta.Height)) > m_refreshDistance)
                {
                    m_movingDelta = new SizeF(0, 0);
                    foreach (PPathwayObject node in m_canvas.SelectedNodes)
                        node.Refresh();
                }
            }
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
                PPathwayNode pnode = (PPathwayNode)e.PickedNode;
                pnode.Offset = PointF.Empty;
                ReturnToSystem(pnode, m_downPosition, e.Position, m_isMoved);

                PNodeList togetherList = new PNodeList();
                foreach (PNode together in m_composite.ChildrenReference)
                    togetherList.Add(together);

                foreach (PNode together in togetherList)
                {
                    PPathwayNode ptogether = (PPathwayNode)together;
                    ReturnToSystem(ptogether,
                                   new PointF(ptogether.X, ptogether.Y),
                                   new PointF(ptogether.X + m_composite.OffsetX, ptogether.Y + m_composite.OffsetY),
                                   m_isMoved);
                }
                if (m_canvas.ControlLayer.ChildrenReference.Contains(m_composite))
                {
                    m_canvas.ControlLayer.RemoveChild(m_composite);
                    m_composite = null;
                }
                
            }
            else if (e.PickedNode is PPathwaySystem)
            {
                PPathwaySystem system = (PPathwaySystem)e.PickedNode;
                string oldSystemName = system.EcellObject.key;
                if (system.Parent is PLayer)
                {
                    m_canvas.PathwayControl.NotifyDataChanged(
                                    oldSystemName,
                                    oldSystemName,
                                    system,
                                    true,
                                    true);
                }
                else
                {
                    RectangleF rectF = system.Rect;

                    if (m_canvas.DoesSystemOverlaps(system.GlobalBounds, oldSystemName)
                        || !m_canvas.IsInsideRoot(rectF))
                    {
                        system.ResetPosition();
                        system.Refresh();
                        m_canvas.UpdateResizeHandlePositions();
                        system.IsInvalid = false;
                    }
                    else
                    {
                        string surSys = m_canvas.GetSurroundingSystemKey(e.Position, oldSystemName);
                        string newSys = null;
                        if (surSys.Equals("/"))
                            newSys = "/" + system.EcellObject.name;
                        else
                            newSys = surSys + "/" + system.EcellObject.name;
                        if (!oldSystemName.Equals(newSys) && m_systems.ContainsKey(newSys))
                        {
                            MessageBox.Show(newSys + m_resources.GetString("ErrAlrExist"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            system.Refresh();
                            m_systems[system.EcellObject.key].Refresh();
                            m_canvas.UpdateResizeHandlePositions();
                            system.IsInvalid = false;
                        }
                        else
                        {
                            if (surSys == null || !surSys.Equals(system.EcellObject.parentSystemID))
                                m_canvas.TransferSystemTo(surSys, oldSystemName, true);
                            else
                            {
                                m_canvas.PathwayControl.NotifyDataChanged(
                                    oldSystemName,
                                    oldSystemName,
                                    system,
                                    true,
                                    true);
                            }

                        }
                    }
                }
            }
            SetBackToDefault();
            m_canvas.UpdateOverview();
            if (e.PickedNode is PPathwaySystem)
            {
                PPathwaySystem picked = (PPathwaySystem)e.PickedNode;
                picked.MoveEnd();
            }
        }

        private void ReturnToSystem(PPathwayNode node, PointF oldPosition, PointF newPosition, bool toBeNotified)
        {
            node.ParentObject.AddChild(node);
            string oldSystem = node.EcellObject.parentSystemID;
            string newSystem = m_canvas.GetSurroundingSystemKey(newPosition);

            if (newSystem == null)
                return;

            node.X += newPosition.X - oldPosition.X;
            node.Y += newPosition.Y - oldPosition.Y;

            if (newSystem.Equals(oldSystem))
            {
                m_canvas.TransferNodeTo(m_canvas.GetSurroundingSystemKey(newPosition), node, toBeNotified, true);
            }
            else if (node is PPathwayVariable)
            {
                string nodeName = node.EcellObject.name;
                if (m_canvas.Variables.ContainsKey(newSystem + ":" + nodeName))
                {
                    node.ResetPosition();
                    MessageBox.Show(nodeName + m_resources.GetString("ErrAlrExist"),
                                    "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                else
                {
                    m_canvas.TransferNodeTo(m_canvas.GetSurroundingSystemKey(newPosition), node, toBeNotified, true);
                }
            }
            else if (node is PPathwayProcess)
            {
                string nodeName = node.EcellObject.name;
                if (m_canvas.Processes.ContainsKey(newSystem + ":" + nodeName))
                {
                    node.ResetPosition();
                    MessageBox.Show(nodeName + m_resources.GetString("ErrAlrExist"),
                                    "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                else
                {
                    m_canvas.TransferNodeTo(m_canvas.GetSurroundingSystemKey(newPosition), node, toBeNotified, true);
                }
            }
            else
            {
                m_canvas.TransferNodeTo(oldSystem, node, toBeNotified, true);
            }
        }
    }
}
