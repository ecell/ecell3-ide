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
        private static readonly Pen LINE_THICK_PEN = new Pen(new SolidBrush(Color.FromArgb(200, Color.Orange)), 5);

        /// <summary>
        /// Currently selected node.
        /// </summary>
        protected PPathwayNode m_current = null;

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

            CanvasControl canvas = m_con.CanvasControl;
            PPathwayNode newNode = e.PickedNode as PPathwayNode;
            // Reset node.
            if (newNode == null)
            {
                SetCurrent(canvas, null);
                return;
            }
            else if(m_current == null)
            {
                SetCurrent(canvas, newNode);
                return;
            }
            if ((m_current is PPathwayVariable && newNode is PPathwayVariable)
                || (m_current is PPathwayProcess && newNode is PPathwayProcess))
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
                variable = (PPathwayVariable)m_current;
                coef = -1;
            }
            else
            {
                process = (PPathwayProcess)m_current;
                variable = (PPathwayVariable)newNode;
                coef = 1;
            }

            // Create Edge.
            if (m_con.SelectedHandle.Mode == Mode.CreateConstant)
            {
                this.CreateEdge(process, variable, 0);
            }
            else if (m_con.SelectedHandle.Mode == Mode.CreateOneWayReaction)
            {
                this.CreateEdge(process, variable, coef);
            }
            else if (m_con.SelectedHandle.Mode == Mode.CreateMutualReaction)
            {
                this.CreateEdge(process, variable, 2);
            }
            SetCurrent(canvas, null);
        }

        /// <summary>
        /// Called when the mouse is moving on the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(object sender, PInputEventArgs e)
        {
            if (m_current == null)
                return;

            base.OnMouseMove(sender, e);
            PointF contactP = m_current.GetContactPoint(e.Position);

            CanvasControl canvas = m_current.CanvasControl;
            PPathwayLine line = canvas.LineHandler.Line4Reconnect;
            line.Reset();

            // Set Line Type
            if (m_con.SelectedHandle.Mode == Mode.CreateConstant)
                line.Info.TypeOfLine = LineType.Dashed;
            else
                line.Info.TypeOfLine = LineType.Solid;

            // Set Line Direction
            if (m_con.SelectedHandle.Mode == Mode.CreateMutualReaction)
                line.Info.Direction = EdgeDirection.Bidirection;
            else if (m_con.SelectedHandle.Mode == Mode.CreateOneWayReaction)
                line.Info.Direction = EdgeDirection.Outward;
            else 
                line.Info.Direction = EdgeDirection.None;

            line.ProPoint = contactP;
            line.VarPoint = e.Position;
            line.DrawLine();
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
        }

        /// <summary>
        /// Set current node.
        /// </summary>
        /// <param name="canvas">CanvasView to which node belongs</param>
        /// <param name="node">current node</param>
        private void SetCurrent(CanvasControl canvas, PPathwayNode node)
        {
            m_current = node;
            if (node != null)
            {
                canvas.AddNodeToBeConnected(node);
                canvas.LineHandler.Line4Reconnect.Pen = LINE_THICK_PEN;
                canvas.LineHandler.SetLineVisibility(true);
            }
            else
            {
                canvas.LineHandler.Line4Reconnect.Reset();
                canvas.ResetNodeToBeConnected();
                canvas.LineHandler.SetLineVisibility(false);
            }
        }
        
        #endregion
    }
}
