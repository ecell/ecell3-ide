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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using EcellLib.PathwayWindow.Node;
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
        protected PathwayView m_view;

        /// <summary>
        /// Currently selected node.
        /// </summary>
        protected PPathwayNode m_current = null;

        /// <summary>
        /// Start point of line
        /// </summary>
        protected PointF m_startPoint = PointF.Empty;

        /// <summary>
        /// Constructor with PathwayView.
        /// </summary>
        /// <param name="view"></param>
        public CreateReactionMouseHandler(PathwayView view)
        {
            this.m_view = view;
        }

        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.Button != MouseButtons.Right;
        }

        /// <summary>
        /// Called when the mouse is down on the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);

            m_startPoint = e.Position;
            CanvasView canvas = m_view.CanvasDictionary[e.Canvas.Name];

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
            else if(m_current is PEcellVariable)
            {
                if (newNode is PEcellProcess)
                {
                    if (m_view.SelectedHandle.Mode == Mode.CreateConstant)
                    {
                        this.CreateEdge((PEcellProcess)newNode, (PEcellVariable)m_current, 0);
                    }
                    else if (m_view.SelectedHandle.Mode == Mode.CreateOneWayReaction)
                    {
                        this.CreateEdge((PEcellProcess)newNode, (PEcellVariable)m_current, -1);
                    }
                    else if (m_view.SelectedHandle.Mode == Mode.CreateMutualReaction)
                    {
                        this.CreateEdge((PEcellProcess)newNode, (PEcellVariable)m_current, -1);
                        this.CreateEdge((PEcellProcess)newNode, (PEcellVariable)m_current, 1);
                    }

                    SetCurrent(canvas, null);
                }
                else if(newNode is PEcellVariable)
                {
                    SetCurrent(canvas, newNode);
                }
            }
            else if(m_current is PEcellProcess)
            {
                if (newNode is PEcellVariable)
                {
                    if (m_view.SelectedHandle.Mode == Mode.CreateConstant)
                    {
                        this.CreateEdge((PEcellProcess)m_current, (PEcellVariable)newNode, 0);
                    }
                    else if (m_view.SelectedHandle.Mode == Mode.CreateOneWayReaction)
                    {
                        this.CreateEdge((PEcellProcess)m_current, (PEcellVariable)newNode, 1);
                    }
                    else if (m_view.SelectedHandle.Mode == Mode.CreateMutualReaction)
                    {
                        this.CreateEdge((PEcellProcess)m_current, (PEcellVariable)newNode, 1);
                        this.CreateEdge((PEcellProcess)m_current, (PEcellVariable)newNode, -1);
                    }
                    SetCurrent(canvas, null);
                }
                else if(newNode is PEcellProcess)
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
                
                PPath line = m_view.CanvasDictionary[e.Canvas.Name].Line4Reconnect;
                line.Reset();
                if (m_view.SelectedHandle.Mode != Mode.CreateConstant)
                {
                    line.AddLine(contactP.X, contactP.Y, e.Position.X, e.Position.Y);
                    line.AddPolygon(
                        PathUtil.GetArrowPoints(
                        e.Position,
                        contactP,
                        PEcellProcess.ARROW_RADIAN_A,
                        PEcellProcess.ARROW_RADIAN_B,
                        PEcellProcess.ARROW_LENGTH));

                    if (m_view.SelectedHandle.Mode == Mode.CreateMutualReaction)
                    {
                        line.AddPolygon(
                        PathUtil.GetArrowPoints(
                        contactP,
                        e.Position,
                        PEcellProcess.ARROW_RADIAN_A,
                        PEcellProcess.ARROW_RADIAN_B,
                        PEcellProcess.ARROW_LENGTH));
                    }
                }
                else
                {
                    PEcellProcess.AddDashedLine(line, contactP.X, contactP.Y, e.Position.X, e.Position.Y);
                }
            }
        }


        /// <summary>
        /// Create VariableReferenceList of process.
        /// </summary>
        /// <param name="process">For this process, VariableReferenceList will be created</param>
        /// <param name="variable">VariableReferenceList to this variable will be created</param>
        /// <param name="coefficient">coefficient of VariableReferenceList of process</param>
        private void CreateEdge(PEcellProcess process, PEcellVariable variable, int coefficient)
        {
            EcellObject obj = m_view.GetData(process.Element.Key,
                process.Element.Type);

            foreach (EcellData d in obj.M_value)
            {
                if (!d.M_name.Equals("VariableReferenceList"))
                    continue;

                List<EcellReference> list =
                    EcellReference.ConvertString(d.M_value.ToString());

                // If this process and variable are connected in the same direction, nothing will be done.
                if (PathUtil.CheckReferenceListContainsEntity(list, variable.Element.Key, coefficient))
                {
                    MessageBox.Show("Already connected !",
                     "Notice",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Exclamation);
                    return;
                }

                String refStr = "(";
                int i = 0;
                foreach (EcellReference r in list)
                {
                    if (i == 0) refStr = refStr + r.ToString();
                    else refStr = refStr + ", " + r.ToString();
                    i++;
                }

                String n = "";
                String pre = "";
                if (coefficient == 0)
                {
                    pre = "S";
                }
                else
                {
                    pre = "P";
                }

                int k = 0;
                while (true)
                {
                    bool ishit = false;
                    n = pre + k;
                    foreach (EcellReference r in list)
                    {
                        if (r.name == n)
                        {
                            k++;
                            ishit = true;
                            continue;
                        }
                    }
                    if (ishit == false) break;
                }

                EcellReference eref = new EcellReference();
                eref.name = n;
                eref.fullID = ":" + variable.Element.Key;
                eref.coefficient = coefficient;
                eref.isAccessor = 1;

                if (i == 0) refStr = refStr + eref.ToString();
                else refStr = refStr + ", " + eref.ToString();
                refStr = refStr + ")";

                d.M_value = EcellValue.ToVariableReferenceList(refStr);

                DataManager.GetDataManager().DataChanged(
                    obj.modelID, obj.key, obj.type, obj);
                return;
            }
        }

        /// <summary>
        /// Set current node.
        /// </summary>
        /// <param name="canvas">CanvasView to which node belongs</param>
        /// <param name="node">current node</param>
        private void SetCurrent(CanvasView canvas, PPathwayNode node)
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

        /*
        /// <summary>
        /// Called when the mouse is down on the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            bool isToProcess = false;
            PEcellProcess process = null;
            PEcellVariable variable = null;

            base.OnMouseDown(sender, e);

            PEcellComposite com = e.PickedNode as PEcellComposite;
            if (com == null)
            {
                if (m_current != null)
                {
                    MessageBox.Show("No process and variable selected.",
                        "Warning", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    m_current = null;
                }
                return;
            }

            foreach (PNode p in com.AllNodes)
            {
                PPathwayNode tmp = p as PPathwayNode;
                if (tmp == null) continue;

                if (m_current == null)
                {
                    m_current = tmp;
                    return;
                }

                if (m_current is PEcellVariable)
                {
                    if (!(tmp is PEcellProcess))
                    {
                        MessageBox.Show("Process not selected.", "WARNING",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        m_current = null;
                        return;
                    }
                    isToProcess = true;
                    process = (PEcellProcess)tmp;
                    variable = (PEcellVariable)m_current;
                }
                else if (m_current is PEcellProcess)
                {
                    if (!(tmp is PEcellVariable))
                    {
                        MessageBox.Show("Variable not selected.", "WARNING",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        m_current = null;
                        return;
                    }
                    isToProcess = false;
                    process = (PEcellProcess)m_current;
                    variable = (PEcellVariable)tmp;
                }
                EcellObject obj = m_view.GetData(process.Element.Key,
                    process.Element.Type);

                foreach (EcellData d in obj.M_value)
                {
                    if (!d.M_name.Equals("VariableReferenceList"))
                        continue;

                    List<EcellReference> list = 
                        EcellReference.ConvertString(d.M_value.ToString());
                    String refStr = "(";
                    int i = 0;
                    foreach (EcellReference r in list)
                    {
                        if (i == 0) refStr = refStr + r.ToString();
                        else refStr = refStr + ", " + r.ToString();
                        i++;
                    }

                    EcellReference eref = new EcellReference();
                    eref.name = "X0";
                    eref.fullID = variable.Element.Key;
                    if (m_view.CheckedComponent == -3)
                    {
                        if (!isToProcess)
                            eref.coefficient = 1;
                        else
                            eref.coefficient = -1;
                    }
                    else if (m_view.CheckedComponent == -4)
                        eref.coefficient = 0;
                    eref.isFixed = 1;

                    if (i == 0) refStr = refStr + eref.ToString();
                    else refStr = refStr + ", " + eref.ToString();
                    refStr = refStr + ")";

                    d.M_value = EcellValue.ToVariableReferenceList(refStr);

                    DataManager.GetDataManager().DataChanged(
                        obj.modelID, obj.key, obj.type, obj);
                    m_current = null;
                    return;
                }
            }
        }*/
    }
}
