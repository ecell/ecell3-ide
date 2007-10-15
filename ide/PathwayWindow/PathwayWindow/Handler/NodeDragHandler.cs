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

    public class NodeDragHandler : PDragEventHandler
    {
        #region Fields
        /// <summary>
        /// the related CanvasViewCompomnetSet.
        /// </summary>
        private CanvasView m_set;

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
        /// <param name="set">component set.</param>
        /// <param name="dict">dictionary of system.</param>
        public NodeDragHandler(CanvasView set, Dictionary<string, PPathwaySystem> dict)
        {
            m_set = set;
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
                pnode.MemorizeCurrentPosition();
                m_set.ControlLayer.AddChild(pnode);

                m_downPosition = new PointF(e.Position.X, e.Position.Y);
                m_composite = new PComposite();
                m_set.ControlLayer.AddChild(m_composite);
                foreach (EcellObject eo in m_set.SelectedNodes)
                {
                    if (eo == pnode.EcellObject)
                        continue;

                    PPathwayNode obj = (PPathwayNode)m_set.GetSelectedObject(eo.key, eo.type);
                    obj.MemorizeCurrentPosition();
                    m_composite.AddChild(obj);
                }
            }
            else if (e.PickedNode is PPathwaySystem)
            {
                ((PPathwaySystem)e.PickedNode).MemorizeCurrentPosition();
            }

            e.Canvas.BackColor = Color.Silver;
            SetBackToDefault();
            SetShadeWithoutSystem(m_set.GetSurroundingSystemKey(e.Position));
        }

        /// <summary>
        /// Set background color of pathway view to default color;
        /// </summary>
        private void SetBackToDefault()
        {
            foreach (PPathwaySystem system in m_systems.Values)
                system.BackgroundBrush = null;

            m_set.PathwayCanvas.BackColor = Color.White;
        }

        /// <summary>
        /// Set background color of pathway view without given systemName to shading color
        /// </summary>
        /// <param name="systemName">Only this system's color remain default state.</param>
        private void SetShadeWithoutSystem(string systemName)
        {
            if (systemName != null && m_systems.ContainsKey(systemName))
            {
                m_set.PathwayCanvas.BackColor = Color.Silver;
                m_systems[systemName].BackgroundBrush = Brushes.White;

                foreach (PPathwaySystem system in m_systems.Values)
                    if (!system.EcellObject.key.Equals(systemName))
                        system.BackgroundBrush = Brushes.Silver;
            }
            else
            {
                m_set.PathwayCanvas.BackColor = Color.White;
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
            SetShadeWithoutSystem(m_set.GetSurroundingSystemKey(e.Position));
            if (e.PickedNode is PPathwaySystem)
            {
                PPathwaySystem picked = (PPathwaySystem)e.PickedNode;
                picked.Refresh();
                // Change color if the system overlaps other system
                RectangleF rectF = picked.Rect;
                if (m_set.DoesSystemOverlaps(picked.GlobalBounds, picked.EcellObject.key)
                    || !m_set.IsInsideRoot(rectF))
                    picked.IsInvalid = true;
                else
                    picked.IsInvalid = false;
                m_set.UpdateResizeHandlePositions();

                picked.MoveStart();
            }
            else if (e.PickedNode is PPathwayNode)
            {
                m_composite.OffsetX += e.Delta.Width;
                m_composite.OffsetY += e.Delta.Height;

                m_movingDelta += e.CanvasDelta;

                if ((Math.Abs(m_movingDelta.Width) + Math.Abs(m_movingDelta.Height)) > m_refreshDistance)
                {
                    m_movingDelta = new SizeF(0, 0);
                    //foreach (EcellObject node in m_set.SelectedNodes)
                    //    node.Refresh();
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
                if (m_set.ControlLayer.ChildrenReference.Contains(m_composite))
                {
                    m_set.ControlLayer.RemoveChild(m_composite);
                    m_composite = null;
                }
                
            }
            else if (e.PickedNode is PPathwaySystem)
            {
                PPathwaySystem picked = (PPathwaySystem)e.PickedNode;
                if (picked.Parent is PLayer)
                {
                    m_set.Control.NotifyDataChanged(
                                    picked.EcellObject.key,
                                    picked.EcellObject.key,
                                    picked.EcellObject,
                                    true);
                }
                else
                {
                    RectangleF rectF = picked.Rect;

                    if (m_set.DoesSystemOverlaps(picked.GlobalBounds, picked.EcellObject.key)
                        || !m_set.IsInsideRoot(rectF))
                    {
                        picked.ReturnToMemorizedPosition();
                        m_systems[picked.Name].Refresh();
                        m_set.UpdateResizeHandlePositions();
                        picked.IsInvalid = false;
                    }
                    else
                    {
                        string oldSystemName = ((PPathwaySystem)e.PickedNode).Name;
                        string surSys = m_set.GetSurroundingSystemKey(e.Position, oldSystemName);
                        string newSys = null;
                        if (surSys.Equals("/"))
                            newSys = "/" + PathUtil.RemovePath(oldSystemName);
                        //else if (surSys.Equals(oldSystemName))
                        //{
                        //    surSys = PathUtil.GetParentSystemId(oldSystemName);
                        //    newSys = oldSystemName;
                        //}
                        else
                            newSys = surSys + "/" + PathUtil.RemovePath(oldSystemName);
                        if (!oldSystemName.Equals(newSys) && m_systems.ContainsKey(newSys))
                        {
                            MessageBox.Show(newSys + m_resources.GetString("ErrAlrExist"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            picked.ReturnToMemorizedPosition();
                            m_systems[picked.Name].Refresh();
                            m_set.UpdateResizeHandlePositions();
                            picked.IsInvalid = false;
                        }
                        else
                        {
                            if (surSys == null || !surSys.Equals(PathUtil.GetParentSystemId(oldSystemName)))
                                m_set.TransferSystemTo(surSys, oldSystemName, true);
                            else
                            {
                                m_set.Control.NotifyDataChanged(
                                    oldSystemName,
                                    oldSystemName,
                                    picked.EcellObject,
                                    true);
                            }

                        }
                    }
                }
            }
            SetBackToDefault();
            m_set.UpdateOverview();
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
            string newSystem = m_set.GetSurroundingSystemKey(newPosition);

            if (newSystem != null)
            {
                node.X += newPosition.X - oldPosition.X;
                node.Y += newPosition.Y - oldPosition.Y;

                if (newSystem.Equals(oldSystem))
                {
                    m_set.TransferNodeTo(m_set.GetSurroundingSystemKey(newPosition), node.EcellObject, toBeNotified, true);
                }
                else if (node is PPathwayVariable)
                {
                    string nodeName = PathUtil.RemovePath(((PPathwayVariable)node).EcellObject.key);
                    if (m_set.Variables.ContainsKey(newSystem + ":" + nodeName))
                    {
                        node.ReturnToMemorizedPosition();
                        MessageBox.Show(nodeName + m_resources.GetString("ErrAlrExist"),
                                        "Error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                    else
                    {
                        m_set.TransferNodeTo(m_set.GetSurroundingSystemKey(newPosition), node.EcellObject, toBeNotified, true);
                    }
                }
                else if (node is PPathwayProcess)
                {
                    string nodeName = PathUtil.RemovePath(((PPathwayProcess)node).EcellObject.key);
                    if (m_set.Processes.ContainsKey(newSystem + ":" + nodeName))
                    {
                        node.ReturnToMemorizedPosition();
                        MessageBox.Show(nodeName + m_resources.GetString("ErrAlrExist"),
                                        "Error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                    else
                    {
                        m_set.TransferNodeTo(m_set.GetSurroundingSystemKey(newPosition), node.EcellObject, toBeNotified, true);
                    }
                }
            }
            else
            {
                m_set.TransferNodeTo(oldSystem, node.EcellObject, toBeNotified, true);
                ((PPathwayNode)node).ReturnToMemorizedPosition();
            }
            node.RefreshText();
        }
    }
}
