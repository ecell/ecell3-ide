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
using System.Drawing;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib.PathwayWindow.Nodes;
using System.ComponentModel;

namespace EcellLib.PathwayWindow.Handler
{
    public class LineHandler
    {
        #region Static readonly fields
        /// <summary>
        /// Used to draw line to reconnect.
        /// </summary>
        private static readonly Pen LINE_THIN_PEN = new Pen(new SolidBrush(Color.FromArgb(200, Color.Orange)), 2);

        /// <summary>
        /// radius of a line handle
        /// </summary>
        private static readonly float LINE_HANDLE_RADIUS = 5;
        #endregion

        #region Fields
        /// <summary>
        /// CanvasControl.
        /// </summary>
        protected CanvasControl m_canvas = null;

        /// <summary>
        /// selected line
        /// </summary>
        PPathwayLine m_selectedLine = null;

        /// <summary>
        /// To handle an edge to reconnect
        /// </summary>
        bool m_isReconnectMode = false;

        /// <summary>
        /// The key of variable at the end of an edge
        /// </summary>
        string m_vOnLinesEnd = null;

        /// <summary>
        /// Line handle on the end for a variable
        /// </summary>
        PPath m_lineHandle4V = null;

        /// <summary>
        /// The key of process at the end of an edge
        /// </summary>
        string m_pOnLinesEnd = null;

        /// <summary>
        /// Line handle on the end for a process
        /// </summary>
        PPath m_lineHandle4P = null;

        /// <summary>
        /// Line for reconnecting.
        /// When a line owned by PEcellProcess is selected, this line will be hidden.
        /// Then m_line4reconnect will appear.
        /// </summary>
        PPathwayLine m_line4reconnect = null;

        /// <summary>
        /// Variable or Process.
        /// </summary>
        ComponentType m_cType;

        /// <summary>
        /// Stack for nodes under the mouse.
        /// this will be used to reconnect edge.
        /// </summary>
        Stack<PPathwayObject> m_nodesUnderMouse = new Stack<PPathwayObject>();

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));
        #endregion

        #region Accessor
        /// <summary>
        /// Accessor for m_selectedNodes.
        /// </summary>
        public PPathwayLine SelectedLine
        {
            get { return m_selectedLine; }
        }
        #endregion

        #region Constructor
        public LineHandler(CanvasControl canvas)
        {
            this.m_canvas = canvas;

            // Prepare line handles
            m_lineHandle4V = new PPath();
            m_lineHandle4V.Brush = new SolidBrush(Color.FromArgb(125, Color.Orange));
            m_lineHandle4V.Pen = new Pen(Brushes.DarkCyan, 1);
            m_lineHandle4V.AddEllipse(
                0,
                0,
                2 * LINE_HANDLE_RADIUS,
                2 * LINE_HANDLE_RADIUS);
            m_lineHandle4V.Tag = ComponentType.Variable;
            m_lineHandle4V.MouseDown += new PInputEventHandler(m_lineHandle_MouseDown);
            m_lineHandle4V.MouseDrag += new PInputEventHandler(m_lineHandle_MouseDrag);
            m_lineHandle4V.MouseUp += new PInputEventHandler(m_lineHandle_MouseUp);

            m_lineHandle4P = new PPath();
            m_lineHandle4P.Brush = new SolidBrush(Color.FromArgb(125, Color.Orange));
            m_lineHandle4P.Pen = new Pen(Brushes.DarkCyan, 1);
            m_lineHandle4P.AddEllipse(
                0,
                0,
                2 * LINE_HANDLE_RADIUS,
                2 * LINE_HANDLE_RADIUS);
            m_lineHandle4P.Tag = ComponentType.Process;
            m_lineHandle4P.MouseDown += new PInputEventHandler(m_lineHandle_MouseDown);
            m_lineHandle4P.MouseDrag += new PInputEventHandler(m_lineHandle_MouseDrag);
            m_lineHandle4P.MouseUp += new PInputEventHandler(m_lineHandle_MouseUp);

            m_line4reconnect = new PPathwayLine(m_canvas);
            m_line4reconnect.Brush = new SolidBrush(Color.FromArgb(200, Color.Orange));
            m_line4reconnect.Pen = LINE_THIN_PEN;
            m_line4reconnect.Pickable = false;
        }
        #endregion

        #region Method
        /// <summary>
        /// Reset a reconnecting line.
        /// </summary>
        public void ResetLinePosition()
        {
            if (null == m_selectedLine)
                return;
            PointF varPoint = new PointF(m_selectedLine.VarPoint.X, m_selectedLine.VarPoint.Y);
            PointF proPoint = new PointF(m_selectedLine.ProPoint.X, m_selectedLine.ProPoint.Y);

            m_canvas.ControlLayer.AddChild(m_lineHandle4V);
            m_canvas.ControlLayer.AddChild(m_lineHandle4P);

            m_lineHandle4V.Offset = PointF.Empty;
            m_lineHandle4V.X = varPoint.X - LINE_HANDLE_RADIUS;
            m_lineHandle4V.Y = varPoint.Y - LINE_HANDLE_RADIUS;

            m_lineHandle4P.Offset = PointF.Empty;
            m_lineHandle4P.X = proPoint.X - LINE_HANDLE_RADIUS;
            m_lineHandle4P.Y = proPoint.Y - LINE_HANDLE_RADIUS;

            // Create line
            m_line4reconnect.Reset();
            m_canvas.Processes[m_selectedLine.Info.ProcessKey].Refresh();
        }
        #endregion

        #region EventHandler
        /// <summary>
        /// Called when the mouse is down on m_lineHandle.
        /// Start to reconnecting edge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseDown(object sender, PInputEventArgs e)
        {
            m_cType = (ComponentType)e.PickedNode.Tag;
            if (m_cType == ComponentType.Process && m_canvas.ControlLayer != null)
            {
                m_canvas.ControlLayer.RemoveChild(m_lineHandle4P);
            }
            else if (m_cType == ComponentType.Variable && m_canvas.ControlLayer != null)
            {
                m_canvas.ControlLayer.RemoveChild(m_lineHandle4V);
            }
        }

        /// <summary>
        /// Called when m_lineHandle is being dragged.
        /// reconnecting line is redrawn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseDrag(object sender, PInputEventArgs e)
        {
            if (null == m_line4reconnect || null == m_selectedLine)
            {
                return;
            }
            m_line4reconnect.Reset();
            PointF ppoint = new PointF(
                LINE_HANDLE_RADIUS + m_lineHandle4P.X + m_lineHandle4P.OffsetX,
                LINE_HANDLE_RADIUS + m_lineHandle4P.Y + m_lineHandle4P.OffsetY);
            PointF vpoint = new PointF(
                LINE_HANDLE_RADIUS + m_lineHandle4V.X + m_lineHandle4V.OffsetX,
                LINE_HANDLE_RADIUS + m_lineHandle4V.Y + m_lineHandle4V.OffsetY);
            m_line4reconnect.ProPoint = ppoint;
            m_line4reconnect.VarPoint = vpoint;
            m_line4reconnect.DrawLine();
        }

        /// <summary>
        /// Called when the mouse is up on m_lineHandle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseUp(object sender, PInputEventArgs e)
        {
            if (m_nodesUnderMouse.Count != 0 && null != m_selectedLine)
            {
                PPathwayObject obj = m_nodesUnderMouse.Pop();
                if (obj is PPathwayProcess && m_cType == ComponentType.Process)
                {
                    m_canvas.PathwayControl.NotifyVariableReferenceChanged(m_pOnLinesEnd, m_vOnLinesEnd, RefChangeType.Delete, 0);
                    if (m_selectedLine.Info.Direction == EdgeDirection.Bidirection)
                    {
                        m_canvas.PathwayControl.NotifyVariableReferenceChanged(obj.EcellObject.key, m_vOnLinesEnd, RefChangeType.BiDir, 0);
                    }
                    else
                    {
                        int coefficient = m_selectedLine.Info.Coefficient;
                        m_canvas.PathwayControl.NotifyVariableReferenceChanged(
                            obj.EcellObject.key,
                            m_vOnLinesEnd,
                            RefChangeType.SingleDir,
                            coefficient);
                    }
                    m_canvas.ResetSelectedLine();
                }
                else if (obj is PPathwayVariable && m_cType == ComponentType.Variable)
                {
                    m_canvas.PathwayControl.NotifyVariableReferenceChanged(m_pOnLinesEnd, m_vOnLinesEnd, RefChangeType.Delete, 0);
                    if (m_selectedLine.Info.Direction == EdgeDirection.Bidirection)
                    {
                        m_canvas.PathwayControl.NotifyVariableReferenceChanged(m_pOnLinesEnd, obj.EcellObject.key, RefChangeType.BiDir, 0);
                    }
                    else
                    {
                        int coefficient = m_selectedLine.Info.Coefficient;
                        m_canvas.PathwayControl.NotifyVariableReferenceChanged(
                            m_pOnLinesEnd,
                            obj.EcellObject.key,
                            RefChangeType.SingleDir,
                            coefficient);
                    }
                    m_canvas.ResetSelectedLine();
                }
            }
            ResetLinePosition();
        }
        #endregion
    }
}
