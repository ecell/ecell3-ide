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
using System.Drawing;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayObject for E-Cell variable.
    /// </summary>
    public class PPathwayProcess : PPathwayEntity
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private PPathwayObject _stepper = null;
        /// <summary>
        /// 
        /// </summary>
        private bool _showEdge = true;
        #endregion

        #region Accessors
        /// <summary>
        /// get/set the related element.
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return base.m_ecellObj; }
            set
            {
                // Show edge
                if (m_ecellObj == null && value.Classname == EcellProcess.MASSCALCULATIONPROCESS)
                    _showEdge = false;
                else if (m_ecellObj != null && m_ecellObj.Classname != value.Classname)
                    _showEdge = (value.Classname != EcellProcess.MASSCALCULATIONPROCESS);

                base.EcellObject = value;


                SetEdges();
                RefreshStepperIcon();
            }
        }

        /// <summary>
        /// get/set m_isViewMode.
        /// </summary>
        public override bool ViewMode
        {
            get { return base.ViewMode; }
            set
            {
                ChangePath(value);
                Stepper.Visible = !value;
                if(!value)
                    RefreshStepperIcon();
                base.ViewMode = value;
            }
        }

        /// <summary>
        /// Stepper Icon.
        /// </summary>
        public PPathwayObject Stepper
        {
            get { return _stepper; }
            set { _stepper = value; }
        }

        /// <summary>
        /// ShowEdge
        /// </summary>
        public bool ShowEdge
        {
            get { return _showEdge; }
            set
            {
                _showEdge = value;
                SetEdges();
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayProcess()
            : base()
        {
        }
        /// <summary>
        /// create new PPathwayProcess.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayProcess();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        protected override void RefreshSettings()
        {
            base.RefreshSettings();
            ChangePath(m_isViewMode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void ChangePath(bool value)
        {
            PointF centerPos = base.CenterPointF;
            if (value)
            {
                base.AddPath(m_tempFigure.GraphicsPath, false);
            }
            else
            {
                base.AddPath(m_figure.GraphicsPath, false);
            }
            base.CenterPointF = centerPos;
            MemorizePosition();
            //RefreshStepperIcon();
        }

        /// <summary>
        /// create edge by using the information of element.
        /// </summary>
        public void SetEdges()
        {
            // Error Check
            if (base.m_canvas == null || m_ecellObj == null || m_layer == null)
                return;
            EcellProcess process = (EcellProcess)m_ecellObj;
            if (process.ReferenceList == null)
                return;

            try
            {
                List<EcellReference> list = process.ReferenceList;
                float width = m_canvas.Control.Animation.EdgeWidth;
                Brush brush = m_canvas.Control.Animation.EdgeBrush;
                // Create EdgeInfo
                List<EdgeInfo> infos = CreateEdgeInfos(process.Key, list);

                // Delete edges.
                List<PPathwayEdge> edges = new List<PPathwayEdge>();
                foreach (PPathwayEdge edge in m_edges)
                {
                    bool exist = false;
                    EdgeInfo temp = null;
                    foreach (EdgeInfo info in infos)
                    {
                        if (edge.Info.VariableKey != info.VariableKey)
                            continue;
                        // Check existing edge.
                        exist = true;
                        temp = info;
                        edge.Info.Direction = info.Direction;
                        edge.Info.LineType = info.LineType;
                        //
                        edge.Refresh();
                        edges.Add(edge);
                        break;
                    }
                    // Delete
                    if (!exist)
                    {
                        edge.RemoveFromParent();
                        edge.Dispose();
                        continue;
                    }
                    //
                    infos.Remove(temp);
                    int index = m_layer.IndexOfChild(edge);
                    if (index < 0)
                        m_layer.AddChild(edge);
                }
                m_edges.Clear();
                m_edges = edges;

                // Create edge.
                foreach (EdgeInfo info in infos)
                {
                    PPathwayVariable var = base.m_canvas.Variables[info.VariableKey];
                    PPathwayEdge edge = new PPathwayEdge(m_canvas, info, this, var);
                    edge.EdgeWidth = width;
                    edge.EdgeBrush = brush;
                    edge.Selected = this.Selected || var.Selected;
                    edge.Refresh();
                    m_layer.AddChild(edge);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(" target is " + e.TargetSite);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<EdgeInfo> CreateEdgeInfos(string process, List<EcellReference> list)
        {
            Dictionary<string, EdgeInfo> dic = new Dictionary<string, EdgeInfo>();
            List<EdgeInfo> infos = new List<EdgeInfo>();
            foreach (EcellReference er in list)
            {
                if (!base.m_canvas.Variables.ContainsKey(er.Key))
                    continue;
                // register
                if (!dic.ContainsKey(er.Key))
                {
                    EdgeInfo info = new EdgeInfo(process, list, er);
                    dic.Add(er.Key, info);
                    continue;
                }

                // set edge type.
                if (dic[er.Key].Direction == EdgeDirection.None)
                {
                    EdgeInfo tmp = new EdgeInfo(process, list, er);
                    dic[er.Key].Direction = tmp.Direction;
                    dic[er.Key].LineType = tmp.LineType;
                }
            }

            infos.AddRange(dic.Values);
            return infos;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void RefreshStepperIcon()
        {
            if (m_canvas == null)
                return;

            // Create Stepper Icon
            if (_stepper == null)
            {
                _stepper = m_canvas.Control.ComponentManager.StepperSetting.CreateTemplate();
                _stepper.AddPath(_stepper.Figure.CreatePath(new RectangleF(0,0,10,10)), false);
                _stepper.Pickable = false;
            }
            EcellValue value = m_ecellObj.GetEcellValue(EcellProcess.STEPPERID);
            if(value == null)
                return;
            PPathwayStepper stepper = null;
            if(!m_canvas.Steppers.TryGetValue((string)value, out stepper))
                return;

            this.AddChild(_stepper);
            _stepper.Width = 10;
            _stepper.Height = 10;
            _stepper.X = this.Right - 10;
            _stepper.Y = this.Bottom - 10;
            _stepper.Setting = stepper.Setting;
            _stepper.MoveToFront();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPoint"></param>
        /// <returns></returns>
        public override PointF GetContactPoint(PointF refPoint)
        {
            if (m_isViewMode)
                return m_tempFigure.GetContactPoint(refPoint, CenterPointF);
            else
                return base.GetContactPoint(refPoint);
        }

        /// <summary>
        /// SetTextVisiblity
        /// </summary>
        protected override void SetTextVisiblity()
        {
            if (m_showingId && !m_isViewMode)
                m_pText.Visible = true;
            else
                m_pText.Visible = false;
        }

        private static bool IsEndNode(List<EcellReference> list)
        {
            bool isEndNode = true;
            foreach (EcellReference er in list)
            {
                if (er.Coefficient != 1)
                    continue;
                isEndNode = false;
                break;
            }
            return isEndNode;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableKey"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        internal PPathwayEdge GetRelation(string variableKey, int coefficient)
        {
            PPathwayEdge value = null;
            foreach (PPathwayEdge edge in m_edges)
            {
                if (edge.Info.VariableKey == variableKey && edge.Info.Coefficient == coefficient)
                    value = edge;
            }
            return value;
        }
    }
}