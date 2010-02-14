//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.Objects;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using System.Drawing.Drawing2D;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// LineHandler
    /// </summary>
    public class EdgeHandler
    {
        #region Static readonly fields
        /// <summary>
        /// radius of a line handle
        /// </summary>
        internal const float LINE_HANDLE_RADIUS = 5;

        /// <summary>
        /// Used to draw line to reconnect.
        /// </summary>
        internal static readonly Brush LINE_BRUSH = Brushes.Gold;

        #endregion

        #region Fields
        /// <summary>
        /// CanvasControl.
        /// </summary>
        protected CanvasControl m_canvas = null;
        
        /// <summary>
        /// PathwayControl.
        /// </summary>
        protected PathwayControl m_con = null;

        /// <summary>
        /// selected line
        /// </summary>
        PPathwayEdge m_selectedLine = null;

        /// <summary>
        /// Line handle on the end for a variable
        /// </summary>
        EdgeHandle m_lineHandle4V = null;

        /// <summary>
        /// Line handle on the end for a process
        /// </summary>
        EdgeHandle m_lineHandle4P = null;

        /// <summary>
        /// Line for reconnecting.
        /// </summary>
        PPathwayEdge m_edge4reconnect = null;

        /// <summary>
        /// Stack for nodes under the mouse.
        /// this will be used to reconnect edge.
        /// </summary>
        Stack<PPathwayObject> m_nodesUnderMouse = new Stack<PPathwayObject>();
        #endregion

        #region Accessor
        /// <summary>
        /// Accessor for m_selectedNodes.
        /// </summary>
        public PPathwayEdge SelectedLine
        {
            get { return m_selectedLine; }
        }

        /// <summary>
        /// Accessor for m_line4reconnect.
        /// </summary>
        public PPathwayEdge Line4Reconnect
        {
            get { return m_edge4reconnect; }
        }
        /// <summary>
        /// Set ProPoint of reconnectLine.
        /// </summary>
        public PointF ProPoint
        {
            get { return m_edge4reconnect.ProPoint; }
            set
            {
                m_lineHandle4P.Offset = value;
                m_edge4reconnect.ProPoint = value;
            }
        }
        /// <summary>
        /// Set VarPoint of reconnectLine.
        /// </summary>
        public PointF VarPoint
        {
            get { return m_edge4reconnect.VarPoint; }
            set
            {
                m_lineHandle4V.Offset = value;
                m_edge4reconnect.VarPoint = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas"></param>
        public EdgeHandler(CanvasControl canvas)
        {
            this.m_canvas = canvas;
            this.m_con = canvas.Control;

            // Prepare line handles
            m_lineHandle4V = new EdgeHandle();
            m_lineHandle4V.ComponentType = EcellObject.VARIABLE;
            m_lineHandle4V.MouseDrag += new PInputEventHandler(LineHandle_MouseDrag);
            m_lineHandle4V.MouseUp += new PInputEventHandler(LineHandle_MouseUp);

            m_lineHandle4P = new EdgeHandle();
            m_lineHandle4P.ComponentType = EcellObject.PROCESS;
            m_lineHandle4P.MouseDrag += new PInputEventHandler(LineHandle_MouseDrag);
            m_lineHandle4P.MouseUp += new PInputEventHandler(LineHandle_MouseUp);

            m_edge4reconnect = new PPathwayEdge(m_canvas);
            m_edge4reconnect.SetEdge(LINE_BRUSH, 2);
            m_edge4reconnect.Pickable = false;
        }
        #endregion

        #region Method

        /// <summary>
        /// Select this line on this canvas.
        /// </summary>
        /// <param name="line"></param>
        public void AddSelectedLine(PPathwayEdge line)
        {
            if (line == null)
                return;
            m_selectedLine = line;

            // Prepare line handles
            m_lineHandle4V.Offset = PointF.Empty;
            m_lineHandle4P.Offset = PointF.Empty;

            m_lineHandle4V.OffsetX = m_selectedLine.VarPoint.X;
            m_lineHandle4V.OffsetY = m_selectedLine.VarPoint.Y;

            m_lineHandle4P.OffsetX = m_selectedLine.ProPoint.X;
            m_lineHandle4P.OffsetY = m_selectedLine.ProPoint.Y;

            // Create Reconnect line
            m_edge4reconnect.SetEdge(LINE_BRUSH, m_edge4reconnect.EdgeWidth);
            m_edge4reconnect.Info.Direction = m_selectedLine.Info.Direction;
            m_edge4reconnect.Info.LineType = m_selectedLine.Info.LineType;
            m_edge4reconnect.VarPoint = m_selectedLine.VarPoint;
            m_edge4reconnect.ProPoint = m_selectedLine.ProPoint;
            m_edge4reconnect.PIndex = m_selectedLine.PIndex;
            m_edge4reconnect.VIndex = m_selectedLine.VIndex;
            m_edge4reconnect.DrawLine();

            SetLineVisibility(true);
        }

        /// <summary>
        /// Reset selected line
        /// </summary>
        public void ResetSelectedLine()
        {
            m_selectedLine = null;
            SetLineVisibility(false);
        }

        /// <summary>
        /// Reset a reconnecting line.
        /// </summary>
        public void ResetLinePosition()
        {
            // Create line
            m_edge4reconnect.VarPoint = m_lineHandle4V.Offset;
            m_edge4reconnect.ProPoint = m_lineHandle4P.Offset;
            m_edge4reconnect.DrawLine();
            //m_canvas.Processes[m_selectedLine.Info.ProcessKey].Refresh();
        }

        /// <summary>
        /// Show/Hide line4reconnect.
        /// </summary>
        /// <param name="visible">visibility</param>
        public void SetLineVisibility(bool visible)
        {
            if (visible)
            {
                m_canvas.ControlLayer.AddChild(m_edge4reconnect);
                m_canvas.ControlLayer.AddChild(m_lineHandle4V);
                m_canvas.ControlLayer.AddChild(m_lineHandle4P);
            }
            else
            {
                if (m_edge4reconnect.Parent != null)
                {
                    m_edge4reconnect.RemoveFromParent();
                    m_lineHandle4V.RemoveFromParent();
                    m_lineHandle4P.RemoveFromParent();
                }
                foreach (EdgeHandle connector in m_connectors)
                {
                    connector.RemoveFromParent();
                }
                m_connectors.Clear();
            }

            if (!visible)
                m_edge4reconnect.Reset();
        }

        #endregion

        #region EventHandlers
        List<EdgeHandle> m_connectors = new List<EdgeHandle>();
        /// <summary>
        /// Called when m_lineHandle is being dragged.
        /// reconnecting line is redrawn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LineHandle_MouseDrag(object sender, PInputEventArgs e)
        {
            if (m_edge4reconnect == null)
            {
                return;
            }
            if (m_selectedLine == null)
            {
                m_con.Menu.ResetEventHandler();
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ResetSelectedLine();
                return;
            }

            // Reset edge pointers
            foreach (EdgeHandle connector in m_connectors)
            {
                connector.RemoveFromParent();
            }
            m_connectors.Clear();
            // Show edge pointers.
            EdgeHandle handle = (EdgeHandle)sender; 
            PPathwayEntity entity = m_canvas.GetPickedEntity(e.Position);
            if (entity != null && !entity.ViewMode &&
                ((entity is PPathwayProcess && handle.ComponentType == EcellObject.PROCESS) || (entity is PPathwayVariable && handle.ComponentType == EcellObject.VARIABLE) || (entity is PPathwayAlias && handle.ComponentType == EcellObject.VARIABLE)))
            {
                foreach (PointF point in entity.Figure.ContactPoints)
                {
                    EdgeHandler.EdgeHandle connector = new EdgeHandler.EdgeHandle();
                    connector.Center = new PointF(point.X + entity.X, point.Y + entity.Y);
                    m_canvas.ControlLayer.AddChild(connector);
                    m_connectors.Add(connector);
                }
            }
            // Reset edge
            m_edge4reconnect.Reset();
            m_edge4reconnect.ProPoint = m_lineHandle4P.Offset;
            m_edge4reconnect.VarPoint = m_lineHandle4V.Offset;
            m_edge4reconnect.DrawLine();
        }

        /// <summary>
        /// Called when the mouse is up on m_lineHandle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LineHandle_MouseUp(object sender, PInputEventArgs e)
        {
            // Check exception.
            PPathwayEntity obj = m_canvas.GetPickedEntity(e.Position);
            if (m_selectedLine == null || obj == null)
            {
                ResetLinePosition();
                SetLineVisibility(false);
                return;
            }
            // Get EdgeInfo
            EdgeInfo info = m_selectedLine.Info;
            string processKey = info.ProcessKey;
            string variableKey = info.VariableKey;
            int coefficient = info.Coefficient;
            RefChangeType type = RefChangeType.SingleDir;

            try
            {

                // Get new EdgeInfo.
                EdgeHandle handle = (EdgeHandle)sender;
                if (obj is PPathwayProcess && handle.ComponentType == EcellObject.PROCESS)
                {
                    processKey = obj.EcellObject.Key;
                }
                else if (obj is PPathwayVariable && handle.ComponentType == EcellObject.VARIABLE)
                {
                    variableKey = obj.EcellObject.Key;
                }
                else if (obj is PPathwayAlias && handle.ComponentType == EcellObject.VARIABLE)
                {
                    variableKey = ((PPathwayAlias)obj).Variable.EcellObject.Key;
                }
                else
                {
                    m_canvas.ResetSelectedLine();
                    ResetLinePosition();
                    return;
                }
                if (info.Direction == EdgeDirection.Bidirection)
                {
                    type = RefChangeType.BiDir;
                }

                // Remove old edge.
                m_con.NotifyVariableReferenceChanged(info.ProcessKey, info.VariableKey, RefChangeType.Delete, 0, false);
                // Add new edge.
                m_con.NotifyVariableReferenceChanged(processKey, variableKey, type, coefficient, true);

                // Set Edge Position
                PPathwayProcess process = m_canvas.Processes[processKey];
                PPathwayEdge edge = process.GetRelation(variableKey, coefficient);
                if (edge != null)
                {
                    // get pointer.
                    EdgeHandle edgePointer = null;
                    foreach (EdgeHandle connector in m_connectors)
                    {
                        if (connector.Rect.Contains(e.Position))
                            edgePointer = connector;
                    }
                    // set pointer.
                    int vIndex = m_edge4reconnect.VIndex;
                    int pIndex = m_edge4reconnect.PIndex;
                    if (obj is PPathwayProcess && handle.ComponentType == EcellObject.PROCESS && edgePointer == null)
                    {
                        pIndex = -1;
                    }
                    else if (obj is PPathwayVariable && handle.ComponentType == EcellObject.VARIABLE && edgePointer == null)
                    {
                        vIndex = -1;
                    }
                    else if (obj is PPathwayAlias && handle.ComponentType == EcellObject.VARIABLE && edgePointer == null)
                    {
                        vIndex = -1;
                    }
                    //
                    else if (obj is PPathwayProcess && handle.ComponentType == EcellObject.PROCESS && edgePointer != null)
                    {
                        edge.ProPoint = edgePointer.Center;
                        pIndex = process.GetConnectorIndex(edgePointer.Center);
                    }
                    else if (obj is PPathwayVariable && handle.ComponentType == EcellObject.VARIABLE && edgePointer != null)
                    {
                        PPathwayVariable variable = m_canvas.Variables[variableKey];
                        vIndex = variable.GetConnectorIndex(edgePointer.Center);
                    }
                    else if (obj is PPathwayAlias && handle.ComponentType == EcellObject.VARIABLE && edgePointer != null)
                    {
                        PPathwayAlias alias = (PPathwayAlias)obj;
                        vIndex = alias.GetConnectorIndex(edgePointer.Center);
                    }
                    edge.PIndex = pIndex;
                    edge.VIndex = vIndex;
                    edge.Refresh();
                }
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrCreateEdge);
            }
            m_canvas.ResetSelectedLine();
            ResetLinePosition();
        }
        #endregion

        #region private class
        /// <summary>
        /// LineHandle to control reconnect line.
        /// private class for Linehandler
        /// </summary>
        public class EdgeHandle : PPathwayNode
        {
            /// <summary>
            /// ComponentType
            /// </summary>
            private string m_type;
            /// <summary>
            /// Accessor for m_cType
            /// </summary>
            public string ComponentType
            {
                get { return m_type; }
                set { m_type = value; }
            }
            /// <summary>
            /// Constructor
            /// </summary>
            public EdgeHandle()
            {
                base.AddInputEventListener(new EdgeHandleDragHandler());
                base.Brush = new SolidBrush(Color.FromArgb(125, Color.Orange));
                base.Pen = new Pen(Brushes.DarkCyan, 1);
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(
                    -LINE_HANDLE_RADIUS,
                    -LINE_HANDLE_RADIUS,
                    2 * LINE_HANDLE_RADIUS,
                    2 * LINE_HANDLE_RADIUS);
                base.AddPath(path, false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal class EdgeHandleDragHandler : PDragEventHandler
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="e"></param>
            /// <returns></returns>
            public override bool DoesAcceptEvent(PInputEventArgs e)
            {
                return base.DoesAcceptEvent(e) && e.Button != System.Windows.Forms.MouseButtons.Right;
            }
        }
        #endregion
    }
}
