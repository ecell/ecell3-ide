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
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// CreateReactionMouseHandler
    /// </summary>
    public class CreateReactionMouseHandler : PPathwayInputEventHandler
    {
        #region Fields
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
            : base(control)
        {
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
            // Set start node.
            if (newNode == null)
            {
                ResetStartNode();
                m_con.Menu.SetDefaultEventHandler();
                return;
            }
            else if(m_start == null)
            {
                SetStartNode(newNode);
                return;
            }
            if ((m_start is PPathwayVariable && newNode is PPathwayVariable)
                || (m_start is PPathwayProcess && newNode is PPathwayProcess))
            {
                SetStartNode(newNode);
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
                this.CreateEdge(process, variable, RefChangeType.SingleDir, 0);
            }
            else if (mode == Mode.CreateOneWayReaction)
            {
                this.CreateEdge(process, variable, RefChangeType.SingleDir, coef);
            }
            else if (mode == Mode.CreateMutualReaction)
            {
                this.CreateEdge(process, variable, RefChangeType.BiDir, 0);
            }
            ResetStartNode();
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
            CanvasControl canvas = m_con.Canvas;
            PPathwayLine line = canvas.LineHandler.Line4Reconnect;
            line.Info.LineType = type;
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
        /// <param name="process">VariableReferenceList for this process will be created</param>
        /// <param name="variable">VariableReferenceList to this variable will be created</param>
        /// <param name="type">RefChangeType of this connection.</param>
        /// <param name="coefficient">coefficient of connection.</param>
        private void CreateEdge(PPathwayProcess process, PPathwayVariable variable, RefChangeType type, int coefficient)
        {
            try
            {
                m_con.NotifyVariableReferenceChanged(
                    process.EcellObject.Key,
                    variable.EcellObject.Key,
                    type,
                    coefficient,
                    true);
            }
            catch (EcellException)
            {
                Util.ShowErrorDialog(MessageResources.ErrCreateEdge);
            }
            m_con.Menu.SetDefaultEventHandler();
        }

        /// <summary>
        /// Add node, which is to be connected
        /// </summary>
        /// <param name="obj">node which is to be connected</param>
        public void SetStartNode(PPathwayNode obj)
        {
            m_start = obj;
        }

        /// <summary>
        /// Reset node to be connected to normal state.
        /// </summary>
        public void ResetStartNode()
        {
            if (m_start != null)
                m_start.RefreshView();
            m_start = null;
        }

        /// <summary>
        /// Reset
        /// </summary>
        public override void Reset()
        {
            ResetStartNode();
            base.Reset();
        }

        #endregion
    }
}
