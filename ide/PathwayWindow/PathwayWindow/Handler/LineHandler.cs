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
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.ComponentModel;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// LineHandler
    /// </summary>
    public class LineHandler
    {
        #region Static readonly fields
        /// <summary>
        /// radius of a line handle
        /// </summary>
        private const float LINE_HANDLE_RADIUS = 5;

        /// <summary>
        /// Used to draw line to reconnect.
        /// </summary>
        private static readonly Brush LINE_BRUSH = new SolidBrush(Color.FromArgb(200, Color.Orange));

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
        PPathwayLine m_selectedLine = null;

        /// <summary>
        /// Line handle on the end for a variable
        /// </summary>
        LineHandle m_lineHandle4V = null;

        /// <summary>
        /// Line handle on the end for a process
        /// </summary>
        LineHandle m_lineHandle4P = null;

        /// <summary>
        /// Line for reconnecting.
        /// </summary>
        PPathwayLine m_line4reconnect = null;

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
        public PPathwayLine SelectedLine
        {
            get { return m_selectedLine; }
        }

        /// <summary>
        /// Accessor for m_line4reconnect.
        /// </summary>
        public PPathwayLine Line4Reconnect
        {
            get { return m_line4reconnect; }
        }
        /// <summary>
        /// Set ProPoint of reconnectLine.
        /// </summary>
        public PointF ProPoint
        {
            get { return m_line4reconnect.ProPoint; }
            set
            {
                m_lineHandle4P.Offset = value;
                m_line4reconnect.ProPoint = value;
            }
        }
        /// <summary>
        /// Set VarPoint of reconnectLine.
        /// </summary>
        public PointF VarPoint
        {
            get { return m_line4reconnect.VarPoint; }
            set
            {
                m_lineHandle4V.Offset = value;
                m_line4reconnect.VarPoint = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas"></param>
        public LineHandler(CanvasControl canvas)
        {
            this.m_canvas = canvas;
            this.m_con = canvas.Control;

            // Prepare line handles
            m_lineHandle4V = new LineHandle();
            m_lineHandle4V.ComponentType = ComponentType.Variable;
            m_lineHandle4V.MouseDrag += new PInputEventHandler(m_lineHandle_MouseDrag);
            m_lineHandle4V.MouseUp += new PInputEventHandler(LineHandle_MouseUp);

            m_lineHandle4P = new LineHandle();
            m_lineHandle4P.ComponentType = ComponentType.Process;
            m_lineHandle4P.MouseDrag += new PInputEventHandler(m_lineHandle_MouseDrag);
            m_lineHandle4P.MouseUp += new PInputEventHandler(LineHandle_MouseUp);

            m_line4reconnect = new PPathwayLine(m_canvas);
            m_line4reconnect.Brush = new SolidBrush(Color.FromArgb(200, Color.Orange));
            m_line4reconnect.Pen = new Pen(LINE_BRUSH, 2);
            m_line4reconnect.Pickable = false;
        }
        #endregion

        #region Method

        /// <summary>
        /// Select this line on this canvas.
        /// </summary>
        /// <param name="line"></param>
        public void AddSelectedLine(PPathwayLine line)
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
            m_line4reconnect.Reset();
            m_line4reconnect.Pen = new Pen(LINE_BRUSH, line.Pen.Width);
            m_line4reconnect.Info.Direction = m_selectedLine.Info.Direction;
            m_line4reconnect.Info.LineType = m_selectedLine.Info.LineType;
            m_line4reconnect.VarPoint = m_selectedLine.VarPoint;
            m_line4reconnect.ProPoint = m_selectedLine.ProPoint;
            m_line4reconnect.DrawLine();

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
            m_line4reconnect.Reset();
            m_line4reconnect.VarPoint = m_lineHandle4V.Offset;
            m_line4reconnect.ProPoint = m_lineHandle4P.Offset;
            m_line4reconnect.DrawLine();
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
                m_canvas.ControlLayer.AddChild(m_line4reconnect);
                m_canvas.ControlLayer.AddChild(m_lineHandle4V);
                m_canvas.ControlLayer.AddChild(m_lineHandle4P);
            }
            else if(m_line4reconnect.Parent != null)
            {
                m_line4reconnect.Parent.RemoveChild(m_line4reconnect);
                m_lineHandle4V.Parent.RemoveChild(m_lineHandle4V);
                m_lineHandle4P.Parent.RemoveChild(m_lineHandle4P);
            }

            if (!visible)
                m_line4reconnect.Reset();
        }

        #endregion

        #region EventHandlers
        /// <summary>
        /// Called when m_lineHandle is being dragged.
        /// reconnecting line is redrawn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseDrag(object sender, PInputEventArgs e)
        {
            if (m_line4reconnect == null)
                return;

            m_line4reconnect.Reset();
            m_line4reconnect.ProPoint = m_lineHandle4P.Offset;
            m_line4reconnect.VarPoint = m_lineHandle4V.Offset;
            m_line4reconnect.DrawLine();
        }

        /// <summary>
        /// Called when the mouse is up on m_lineHandle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LineHandle_MouseUp(object sender, PInputEventArgs e)
        {
            // Check exception.
            PPathwayNode obj = m_canvas.GetPickedNode(e.Position);
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
                // Remove  old edge.
                m_con.NotifyVariableReferenceChanged(processKey, variableKey, RefChangeType.Delete, 0, false);

                // Get new EdgeInfo.
                LineHandle handle = (LineHandle)sender;
                if (obj is PPathwayProcess && handle.ComponentType == ComponentType.Process)
                {
                    processKey = obj.EcellObject.Key;
                }
                else if (obj is PPathwayVariable && handle.ComponentType == ComponentType.Variable)
                {
                    variableKey = obj.EcellObject.Key;
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

                m_con.NotifyVariableReferenceChanged(processKey, variableKey, type, coefficient, true);
            }
            catch (Exception ex)
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
        class LineHandle : PPath
        {
            /// <summary>
            /// ComponentType
            /// </summary>
            private ComponentType m_cType;
            /// <summary>
            /// Accessor for m_cType
            /// </summary>
            public ComponentType ComponentType
            {
                get { return m_cType; }
                set { m_cType = value; }
            }
            /// <summary>
            /// Constructor
            /// </summary>
            public LineHandle()
            {
                base.AddInputEventListener(new PDragEventHandler());
                base.Brush = new SolidBrush(Color.FromArgb(125, Color.Orange));
                base.Pen = new Pen(Brushes.DarkCyan, 1);
                base.AddEllipse(
                    -LINE_HANDLE_RADIUS,
                    -LINE_HANDLE_RADIUS,
                    2 * LINE_HANDLE_RADIUS,
                    2 * LINE_HANDLE_RADIUS);
            }
        }
        #endregion
    }
}
