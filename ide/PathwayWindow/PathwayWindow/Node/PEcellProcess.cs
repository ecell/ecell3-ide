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
using System.Drawing;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib.PathwayWindow.Element;
using PathwayWindow.UIComponent;

namespace EcellLib.PathwayWindow.Node
{
    /// <summary>
    /// PPathwayObject for E-Cell variable.
    /// </summary>
    public class PEcellProcess : PPathwayNode
    {
        #region Static readonly fields
        /// <summary>
        ///  Arrow design settings
        /// </summary>
        public static readonly float ARROW_RADIAN_A = 0.471f;

        /// <summary>
        ///  Arrow design settings
        /// </summary>
        public static readonly float ARROW_RADIAN_B = 5.812f;

        /// <summary>
        ///  Arrow design settings
        /// </summary>        
        public static readonly float ARROW_LENGTH = 15;

        /// <summary>
        /// Edges will be refreshed every time when this process has moved by this distance.
        /// </summary>
        protected static readonly float REFRESH_DISTANCE = 4;
        #endregion

        #region Fields
        /// <summary>
        /// dictionary of Line for variable. key is PPEcellVariable.
        /// </summary>
        protected Dictionary<PEcellVariable,List<Line>> m_relatedVariables = new Dictionary<PEcellVariable,List<Line>>();

        /// <summary>
        /// delta of moving this node.
        /// </summary>
        protected SizeF m_movingDelta = new SizeF(0,0);

        /// <summary>
        /// list of size.
        /// </summary>
        protected List<SizeF> m_sizes = new List<SizeF>();
        #endregion

        #region Accessors
        /// <summary>
        /// get/set the related element.
        /// </summary>
        public new NodeElement Element
        {
            get { return (NodeElement)base.m_element; }
            set
            {
                base.m_element = value;
                this.X = this.Element.X;
                this.Y = this.Element.Y;
                this.OffsetX = 0;
                this.OffsetY = 0;
                RefreshText();
                base.m_idText.MoveToFront();
                RefreshEdges();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// create new PEcellProcess.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PEcellProcess();
        }
        #endregion

        #region Methods
        /// <summary>
        /// clone PEcellProcess.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            PEcellProcess process = new PEcellProcess();
            process.Bounds = this.Bounds;
            process.Brush = this.Brush;
            process.m_idText = this.m_idText;

            return process;
        }
        /// <summary>
        /// Freeze this object and related lines.
        /// </summary>
        public override void Freeze()
        {
            base.Freeze();
            foreach (List<Line> list in m_relatedVariables.Values)
                foreach (Line line in list)
                    line.Pickable = false;
        }

        /// <summary>
        /// Unfreeze this object and related lines.
        /// </summary>
        public override void Unfreeze()
        {
            base.Unfreeze();
            foreach (List<Line> list in m_relatedVariables.Values)
                foreach (Line line in list)
                    line.Pickable = true;
        }

        /// <summary>
        /// get list of elements.
        /// if this node is process, this return is that process.
        /// </summary>
        /// <returns></returns>
        public override List<PathwayElement> GetElements()
        {
            List<PathwayElement> returnList = new List<PathwayElement>();

            this.Element.X = this.X + this.OffsetX;
            this.Element.Y = this.Y + this.OffsetY;
            this.Element.CsId = this.m_csId;
            returnList.Add(this.Element);
            return returnList;
        }

        /// <summary>
        /// event on mouse up on this node.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseUp(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseUp(e);
            this.RefreshEdges();

            m_movingDelta = new SizeF(0, 0);
        }

        /// <summary>
        /// event on mouse drag on this node.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDrag(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDrag(e);


            this.RefreshEdges();
        }

        /// <summary>
        /// event on double click this node.
        /// </summary>
        /// <param name="e"></param>
        public override void OnDoubleClick(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {

            EcellObject obj = m_set.PathwayView.GetEcellObject(Element.Key, "Process");

            PropertyEditor editor = new PropertyEditor();
            editor.layoutPanel.SuspendLayout();
            editor.SetCurrentObject(obj);
            editor.SetDataType(obj.type);
            editor.PEApplyButton.Click += new EventHandler(editor.UpdateProperty);
            editor.LayoutPropertyEditor();
            editor.layoutPanel.ResumeLayout(false);
            editor.ShowDialog();
        }

        /// <summary>
        /// notify to add the related variable to list.
        /// </summary>
        /// <param name="var">the related variable.</param>
        /// <param name="path">PPath of the related variable.</param>
        private void NotifyAddRelatedVariable(PEcellVariable var, Line path)
        {
            if (m_relatedVariables.ContainsKey(var))
            {
                m_relatedVariables[var].Add(path);
            }
            else
            {
                List<Line> ppaths = new List<Line>();
                ppaths.Add(path);
                m_relatedVariables.Add(var, ppaths);
            }
        }

        /// <summary>
        /// create edge by using the information of element.
        /// </summary>
        public void CreateEdges()
        {
            DeleteEdges();
            if (this.Element is ProcessElement && base.m_set != null)
            {
                try
                {
                    ProcessElement proEle = (ProcessElement)this.Element;
                    foreach (EdgeInfo edge in proEle.Edges.Values)
                    {
                        if (base.m_set.Variables.ContainsKey(edge.VariableKey))
                        {
                            PEcellVariable var = base.m_set.Variables[edge.VariableKey];
                            Line path = new Line(edge);
                            
                            path.MouseDown += this.m_handler4Line;
                            path.Brush = Brushes.Black;
                            PointF endPos = var.GetContactPoint(base.CenterPointToCanvas);
                            PointF startPos = base.GetContactPoint(endPos);

                            path.VarPoint = endPos;
                            path.ProPoint = startPos;

                            if (base.ParentObject is PEcellSystem)
                            {
                                startPos = ((PEcellSystem)base.ParentObject).CanvasPos2SystemPos(startPos);
                                endPos = ((PEcellSystem)base.ParentObject).CanvasPos2SystemPos(endPos);
                            }

                            switch (edge.TypeOfLine)
                            {
                                case LineType.Solid:
                                    path.Pen = new Pen(Brushes.Black, 2);
                                    path.AddLine(startPos.X, startPos.Y, endPos.X, endPos.Y);
                                    break;
                                case LineType.Dashed:
                                    path.Pen = new Pen(Brushes.Black, 3);
                                    PEcellProcess.AddDashedLine(path, startPos.X, startPos.Y, endPos.X, endPos.Y);
                                    break;
                                case LineType.Unknown:
                                    path.AddLine(startPos.X, startPos.Y, endPos.X, endPos.Y);
                                    break;
                            }

                            switch (edge.Direction)
                            {
                                case EdgeDirection.Bidirection:
                                    path.AddPolygon(PathUtil.GetArrowPoints(startPos, endPos, ARROW_RADIAN_A, ARROW_RADIAN_B, ARROW_LENGTH));
                                    path.AddPolygon(PathUtil.GetArrowPoints(endPos, startPos, ARROW_RADIAN_A, ARROW_RADIAN_B, ARROW_LENGTH));
                                    break;
                                case EdgeDirection.Inward:
                                    path.AddPolygon(PathUtil.GetArrowPoints(startPos, endPos, ARROW_RADIAN_A, ARROW_RADIAN_B, ARROW_LENGTH));
                                    break;
                                case EdgeDirection.Outward:
                                    path.AddPolygon(PathUtil.GetArrowPoints(endPos, startPos, ARROW_RADIAN_A, ARROW_RADIAN_B, ARROW_LENGTH));
                                    break;
                                case EdgeDirection.None:
                                    break;
                            }

                            path.Pickable = true;
                            
                            this.ParentObject.AddChild(path);
                            this.NotifyAddRelatedVariable(var, path);
                            var.NotifyAddRelatedProcess(this);
                        }
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(" target is " + e.TargetSite);
                }
            }
        }

        /// <summary>
        /// check whethet exist invalid process in list.
        /// </summary>
        public void RefreshEdges()
        {
            if (base.m_set != null && m_relatedVariables != null)
            {
                ProcessElement proEle = (ProcessElement)this.Element;
                foreach (EdgeInfo edge in proEle.Edges.Values)
                {
                    if (base.m_set.Variables.ContainsKey(edge.VariableKey))
                    {
                        PEcellVariable var = base.m_set.Variables[edge.VariableKey];
                        PointF baseCenter = base.CenterPointToCanvas;
                        PointF endPos = var.GetContactPoint(baseCenter);
                        PointF startPos = base.GetContactPoint(endPos);

                        PointF globalEndPos = endPos;
                        PointF globalStartPos = startPos;

                        if (base.ParentObject is PEcellSystem)
                        {
                            startPos = ((PEcellSystem)base.ParentObject).CanvasPos2SystemPos(startPos);
                            endPos = ((PEcellSystem)base.ParentObject).CanvasPos2SystemPos(endPos);
                        }

                        if (m_relatedVariables.ContainsKey(var))
                        {
                            foreach (Line path in m_relatedVariables[var])
                            {
                                path.Parent.RemoveChild(path);
                                base.ParentObject.AddChild(path);
                                path.Reset();

                                path.VarPoint = globalEndPos;
                                path.ProPoint = globalStartPos;

                                switch (edge.TypeOfLine)
                                {
                                    case LineType.Solid:
                                        path.Pen = new Pen(Brushes.Black, 2);
                                        path.AddLine(startPos.X, startPos.Y, endPos.X, endPos.Y);
                                        break;
                                    case LineType.Dashed:
                                        path.Pen = new Pen(Brushes.Black, 3);
                                        PEcellProcess.AddDashedLine(path, startPos.X, startPos.Y, endPos.X, endPos.Y);
                                        break;
                                    case LineType.Unknown:
                                        path.AddLine(startPos.X, startPos.Y, endPos.X, endPos.Y);
                                        break;
                                }

                                switch (edge.Direction)
                                {
                                    case EdgeDirection.Bidirection:
                                        path.AddPolygon(PathUtil.GetArrowPoints(startPos, endPos, ARROW_RADIAN_A, ARROW_RADIAN_B, ARROW_LENGTH));
                                        path.AddPolygon(PathUtil.GetArrowPoints(endPos, startPos, ARROW_RADIAN_A, ARROW_RADIAN_B, ARROW_LENGTH));
                                        break;
                                    case EdgeDirection.Inward:
                                        path.AddPolygon(PathUtil.GetArrowPoints(startPos, endPos, ARROW_RADIAN_A, ARROW_RADIAN_B, ARROW_LENGTH));
                                        break;
                                    case EdgeDirection.Outward:
                                        path.AddPolygon(PathUtil.GetArrowPoints(endPos, startPos, ARROW_RADIAN_A, ARROW_RADIAN_B, ARROW_LENGTH));
                                        break;
                                    case EdgeDirection.None:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// delete all related process from list.
        /// </summary>
        public void DeleteEdges()
        {
            foreach(List<Line> pathList in m_relatedVariables.Values)
            {
                foreach(Line path in pathList)
                {
                    if(path.Parent != null)
                    {
                        path.Parent.RemoveChild(path);
                    }
                }
            }
            m_relatedVariables = new Dictionary<PEcellVariable, List<Line>>();
        }

        /// <summary>
        /// delete the specified related variable from list.
        /// </summary>
        /// <param name="p">the specified variable.</param>
        public void DeleteEdge(PEcellVariable p)
        {
            if (!m_relatedVariables.ContainsKey(p)) return;
            List<Line> pathList = m_relatedVariables[p];
            {
                foreach (PPath path in pathList)
                {
                    if (path.Parent != null)
                    {
                        path.Parent.RemoveChild(path);
                    }
                }
            }

            m_relatedVariables.Remove(p);
        }

        /// <summary>
        /// reconstruct the list of related variable 
        /// by using the information of elements.
        /// </summary>
        public override void ValidateEdges()
        {
            // Whether all variables of VariableReferenceList exists or not
            Boolean isAllExist = true;

            ProcessElement proEle = (ProcessElement)this.Element;
            foreach (EdgeInfo edge in proEle.Edges.Values)
            {
                if (!base.m_set.Variables.ContainsKey(edge.VariableKey))
                    isAllExist = false;
            }

            if(!isAllExist)
            {
                this.DeleteEdges();
                this.CreateEdges();
            }
        }

        /// <summary>
        /// add the dash line to PPath.
        /// </summary>
        /// <param name="path">PPath added the dash line.</param>
        /// <param name="startX">the position of start.</param>
        /// <param name="startY">the position of start.</param>
        /// <param name="endX">the position of end.</param>
        /// <param name="endY">the position of end.</param>
        public static void AddDashedLine(PPath path, float startX, float startY, float endX, float endY)
        {
            if (path == null)
                return;

            path.FillMode = System.Drawing.Drawing2D.FillMode.Winding;
            float repeatNum = (float)Math.Sqrt((endX - startX) * (endX - startX) + (endY - startY) * (endY - startY)) / 6f;
            float xFragment = (endX - startX) / repeatNum;
            float yFragment = (endY - startY) / repeatNum;

            float presentX = startX;
            float presentY = startY;
            for(int i = 0; i + 2 < repeatNum;i++ )
            {
                presentX += xFragment;
                presentY += yFragment;

                if(i % 2 == 1)
                {
                    continue;
                }
                path.AddLine(presentX, presentY, presentX + xFragment, presentY + yFragment);
                path.CloseFigure();                
            }
        }

        /// <summary>
        /// Notify this object is moved.
        /// Will be called when parent system is moving
        /// </summary>
        public override void NotifyMovement()
        {
            RefreshEdges();
        }

        /// <summary>
        /// notify to remove the related variable from list.
        /// </summary>
        /// <param name="key"></param>
        public void NotifyRemoveRelatedVariable(string key)
        {
            List<PEcellVariable> rList = new List<PEcellVariable>();
            foreach (PEcellVariable p in m_relatedVariables.Keys)
            {
                if (p.Element.Key.Equals(key))
                {
                    rList.Add(p);
                }
            }

            foreach (PEcellVariable p in rList)
            {
                m_relatedVariables.Remove(p);
            }
            rList.Clear();
            ((ProcessElement)this.Element).Edges.Remove(key);            
        }

        /// <summary>
        /// notify to remove all related process from list.
        /// </summary>
        public void NotifyRemoveRelatedProcess()
        {
            foreach (PEcellVariable p in m_relatedVariables.Keys)
            {
                p.NotifyRemoveRelatedProcess(this.Element.Key);
            }
            DeleteEdges();
            RefreshEdges();
        }

        /// <summary>
        /// refresh the information of edge.
        /// </summary>
        public override void Refresh()
        {
            ValidateEdges();
            RefreshEdges();
            RefreshText();
        }

        /// <summary>
        /// start to move this Node by drag.
        /// </summary>
        public override void MoveStart()
        {
            DeleteEdges();
        }

        /// <summary>
        /// end to move this Node by drag.
        /// </summary>
        public override void MoveEnd()
        {
            CreateEdges();
        }
        #endregion
    }
}