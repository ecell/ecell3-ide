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

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.UIComponent;
using System.Drawing;
using UMD.HCIL.Piccolo.Nodes;

namespace EcellLib.PathwayWindow
{
    class CreateReactionMouseHandler : PBasicInputEventHandler
    {
        /// <summary>
        /// Used to draw line to connect.
        /// </summary>
        private static readonly Pen LINE_THICK_PEN = new Pen(new SolidBrush(Color.FromArgb(200, Color.Orange)), 5);

        public enum ReferenceKind {Changeable, Constant};

        /// <summary>
        /// PathwayView
        /// </summary>
        protected PathwayControl m_con;

        /// <summary>
        /// Currently selected node.
        /// </summary>
        protected PPathwayNode m_current = null;

        /// <summary>
        /// Start point of line
        /// </summary>
        protected PointF m_startPoint = PointF.Empty;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));


        /// <summary>
        /// Constructor with PathwayView.
        /// </summary>
        /// <param name="control">The control of PathwayView.</param>
        public CreateReactionMouseHandler(PathwayControl control)
        {
            this.m_con = control;
        }

        /// <summary>
        /// Get the flag whether system accept this events.
        /// </summary>
        /// <param name="e">Target events.</param>
        /// <returns>The judgement whether this event is accepted.</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.Button != MouseButtons.Right;
        }

        /// <summary>
        /// Called when the mouse is down on the canvas.
        /// </summary>
        /// <param name="sender">UserControl.</param>
        /// <param name="e">PInputEventArgs</param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);

            CanvasControl canvas = m_con.CanvasDictionary[e.Canvas.Name];

            PPathwayNode newNode = e.PickedNode as PPathwayNode;

            if (newNode == null)
            {
                SetCurrent(canvas, null);
                return;
            }

            if (m_current == null)
            {
                SetCurrent(canvas, newNode);
            }
            else if(m_current is PPathwayVariable)
            {
                if (newNode is PPathwayProcess)
                {
                    if (m_con.SelectedHandle.Mode == Mode.CreateConstant)
                    {
                        this.CreateEdge((PPathwayProcess)newNode, (PPathwayVariable)m_current, 0);
                    }
                    else if (m_con.SelectedHandle.Mode == Mode.CreateOneWayReaction)
                    {
                        this.CreateEdge((PPathwayProcess)newNode, (PPathwayVariable)m_current, -1);
                    }
                    else if (m_con.SelectedHandle.Mode == Mode.CreateMutualReaction)
                    {
                        this.CreateEdge((PPathwayProcess)newNode, (PPathwayVariable)m_current, -1);
                        this.CreateEdge((PPathwayProcess)newNode, (PPathwayVariable)m_current, 1);
                    }

                    SetCurrent(canvas, null);
                }
                else if(newNode is PPathwayVariable)
                {
                    SetCurrent(canvas, newNode);
                }
            }
            else if(m_current is PPathwayProcess)
            {
                if (newNode is PPathwayVariable)
                {
                    if (m_con.SelectedHandle.Mode == Mode.CreateConstant)
                    {
                        this.CreateEdge((PPathwayProcess)m_current, (PPathwayVariable)newNode, 0);
                    }
                    else if (m_con.SelectedHandle.Mode == Mode.CreateOneWayReaction)
                    {
                        this.CreateEdge((PPathwayProcess)m_current, (PPathwayVariable)newNode, 1);
                    }
                    else if (m_con.SelectedHandle.Mode == Mode.CreateMutualReaction)
                    {
                        this.CreateEdge((PPathwayProcess)m_current, (PPathwayVariable)newNode, 1);
                        this.CreateEdge((PPathwayProcess)m_current, (PPathwayVariable)newNode, -1);
                    }
                    SetCurrent(canvas, null);
                }
                else if(newNode is PPathwayProcess)
                {
                    SetCurrent(canvas, newNode);
                }
            }
        }

        /// <summary>
        /// Called when the mouse is moving on the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(object sender, PInputEventArgs e)
        {
            base.OnMouseMove(sender, e);

            if(null != m_current)
            {
                PointF contactP = m_current.GetContactPoint(e.Position);
                
                Line line = m_con.CanvasDictionary[e.Canvas.Name].Line4Reconnect;
                line.Reset();
                if (m_con.SelectedHandle.Mode != Mode.CreateConstant)
                {
                    line.ProPoint = contactP;
                    line.VarPoint = e.Position;
                    line.DrawLine();
                    if (m_con.SelectedHandle.Mode == Mode.CreateMutualReaction)
                    {
                        line.AddPolygon( Line.GetArrowPoints( contactP, e.Position ));
                    }
                }
                else
                {
                    Line.AddDashedLine(line, contactP.X, contactP.Y, e.Position.X, e.Position.Y);
                }
            }
        }

        /// <summary>
        /// Create VariableReferenceList of process.
        /// </summary>
        /// <param name="process">For this process, VariableReferenceList will be created</param>
        /// <param name="variable">VariableReferenceList to this variable will be created</param>
        /// <param name="coefficient">coefficient of VariableReferenceList of process</param>
        private void CreateEdge(PPathwayProcess process, PPathwayVariable variable, int coefficient)
        {
            process.CreateEdge(variable, coefficient);
        }

        /// <summary>
        /// Set current node.
        /// </summary>
        /// <param name="canvas">CanvasView to which node belongs</param>
        /// <param name="node">current node</param>
        private void SetCurrent(CanvasControl canvas, PPathwayNode node)
        {
            m_current = node;
            if (null != node)
            {
                canvas.AddNodeToBeConnected(node);
                canvas.Line4Reconnect.Pen = LINE_THICK_PEN;
                canvas.SetLineVisibility(true);
            }
            else
            {
                canvas.Line4Reconnect.Reset();
                canvas.ResetNodeToBeConnected();
                canvas.SetLineVisibility(false);
            }
        }

    }
}
