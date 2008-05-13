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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.UIComponent;

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// CreateReactionMouseHandler
    /// </summary>
    public class CreateReactionMouseHandler : PPathwayInputEventHandler
    {
        #region Fields
        /// <summary>
        /// Used to draw line to connect.
        /// </summary>
        private static readonly Pen LINE_THICK_PEN = new Pen(new SolidBrush(Color.FromArgb(200, Color.Orange)), 3);

        /// <summary>
        /// Currently selected node.
        /// </summary>
        private PPathwayNode m_start = null;

        /// <summary>
        /// 
        /// </summary>
        public PPathwayNode StartNode
        {
            get { return m_start; }
            set { m_start = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor with PathwayView.
        /// </summary>
        /// <param name="control">The control of PathwayView.</param>
        public CreateReactionMouseHandler(PathwayControl control)
        {
            this.m_con = control;
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Called when the mouse is down on the canvas.
        /// </summary>
        /// <param name="sender">UserControl.</param>
        /// <param name="e">PInputEventArgs</param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);

            CanvasControl canvas = m_con.Canvas;
            PPathwayNode newNode = canvas.GetPickedNode(e.Position);
            // Reset node.
            if (newNode == null)
            {
                SetCurrent(canvas, null);
                return;
            }
            else if(m_start == null)
            {
                SetCurrent(canvas, newNode);
                return;
            }
            if ((m_start is PPathwayVariable && newNode is PPathwayVariable)
                || (m_start is PPathwayProcess && newNode is PPathwayProcess))
            {
                SetCurrent(canvas, newNode);
                return;
            }

            // Set object.
            int coef = 0;
            PPathwayProcess process;
            PPathwayVariable variable;
            if (newNode is PPathwayProcess)
            {
                process = (PPathwayProcess)newNode;
                variable = (PPathwayVariable)m_start;
                coef = -1;
            }
            else
            {
                process = (PPathwayProcess)m_start;
                variable = (PPathwayVariable)newNode;
                coef = 1;
            }
            Mode mode = m_con.Menu.Handle.Mode;
            // Create Edge.
            if (mode == Mode.CreateConstant)
            {
                this.CreateEdge(process, variable, 0);
            }
            else if (mode == Mode.CreateOneWayReaction)
            {
                this.CreateEdge(process, variable, coef);
            }
            else if (mode == Mode.CreateMutualReaction)
            {
                this.CreateEdge(process, variable, 2);
            }
            SetCurrent(canvas, null);
            canvas.LineHandler.SetLineVisibility(false);
        }
        /// <summary>
        /// Called when the mouse is moving on the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(object sender, PInputEventArgs e)
        {
            base.OnMouseMove(sender, e);
            if (m_start == null)
                return;

            // Get Line Type
            LineType type;
            Mode mode = m_con.Menu.Handle.Mode;
            if (mode == Mode.CreateConstant)
                type = LineType.Dashed;
            else
                type = LineType.Solid;

            // Get Line Direction
            EdgeDirection direction;
            if (mode == Mode.CreateMutualReaction)
                direction = EdgeDirection.Bidirection;
            else if (mode == Mode.CreateOneWayReaction)
                direction = EdgeDirection.Outward;
            else 
                direction = EdgeDirection.None;

            // Set Line
            CanvasControl canvas = m_start.CanvasControl;
            PPathwayLine line = canvas.LineHandler.Line4Reconnect;
            line.Info.TypeOfLine = type;
            line.Info.Direction = direction;
            canvas.LineHandler.VarPoint = e.Position;
            canvas.LineHandler.ProPoint = m_start.GetContactPoint(e.Position);
            canvas.LineHandler.ResetLinePosition();
            canvas.LineHandler.SetLineVisibility(true);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Create VariableReferenceList of process.
        /// </summary>
        /// <param name="process">For this process, VariableReferenceList will be created</param>
        /// <param name="variable">VariableReferenceList to this variable will be created</param>
        /// <param name="coefficient">coefficient of VariableReferenceList of process</param>
        private void CreateEdge(PPathwayProcess process, PPathwayVariable variable, int coefficient)
        {
            if (coefficient == 2)
            {
                m_con.NotifyVariableReferenceChanged(
                    process.EcellObject.Key,
                    variable.EcellObject.Key,
                    RefChangeType.BiDir,
                    0,
                    true);
            }
            else
            {
                m_con.NotifyVariableReferenceChanged(
                    process.EcellObject.Key,
                    variable.EcellObject.Key,
                    RefChangeType.SingleDir,
                    coefficient,
                    true);
            }
            m_con.Menu.SetDefaultEventHandler();
        }

        /// <summary>
        /// Set current node.
        /// </summary>
        /// <param name="canvas">CanvasView to which node belongs</param>
        /// <param name="node">current node</param>
        private void SetCurrent(CanvasControl canvas, PPathwayNode node)
        {
            if (node != null)
                AddNodeToBeConnected(node);
            else
                ResetNodeToBeConnected();
        }

        /// <summary>
        /// Add node, which is to be connected
        /// </summary>
        /// <param name="obj">node which is to be connected</param>
        public void AddNodeToBeConnected(PPathwayNode obj)
        {
            if (null != m_start)
                m_start.IsToBeConnected = false;
            m_start = obj;
            m_start.IsToBeConnected = true;
        }

        /// <summary>
        /// Reset node to be connected to normal state.
        /// </summary>
        public void ResetNodeToBeConnected()
        {
            if (m_start != null)
                m_start.IsToBeConnected = false;
            m_start = null;
        }

        /// <summary>
        /// Reset
        /// </summary>
        public override void Reset()
        {
            ResetNodeToBeConnected();
            base.Reset();
        }

        #endregion
    }
}
