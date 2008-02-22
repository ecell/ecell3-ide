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
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Figure;
using System.Drawing.Drawing2D;

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayObject for E-Cell variable.
    /// </summary>
    public class PPathwayProcess : PPathwayNode
    {
        #region Static readonly fields
        #endregion

        #region Fields
        /// <summary>
        /// dictionary of Line. Key is node.EcellObject.key.
        /// </summary>
        protected Dictionary<string, List<PPathwayLine>> m_lines = new Dictionary<string, List<PPathwayLine>>();
        /// <summary>
        /// dictionary of related PPathwayVariable. Key is node.EcellObject.key.
        /// </summary>
        protected Dictionary<string, PPathwayVariable> m_relatedVariables = new Dictionary<string, PPathwayVariable>();

        /// <summary>
        /// edge brush.
        /// </summary>
        private Brush m_edgeBrush = Brushes.Black;
        #endregion

        #region Accessors
        /// <summary>
        /// get/set the related element.
        /// </summary>
        public new EcellProcess EcellObject
        {
            get { return (EcellProcess)base.m_ecellObj; }
            set
            {
                base.EcellObject = value;
                Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush EdgeBrush
        {
            get { return m_edgeBrush; }
            set 
            {
                m_edgeBrush = value;
                foreach (List<PPathwayLine> list in m_lines.Values)
                {
                    foreach (PPathwayLine line in list)
                    {
                        line.Pen.Brush = m_edgeBrush;
                        line.Brush = m_edgeBrush;
                    }
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayProcess()
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
        /// Freeze this object and related lines.
        /// </summary>
        public override void Freeze()
        {
            base.Freeze();
            foreach (List<PPathwayLine> list in m_lines.Values)
                foreach (PPathwayLine line in list)
                    line.Pickable = false;
        }

        /// <summary>
        /// Unfreeze this object and related lines.
        /// </summary>
        public override void Unfreeze()
        {
            base.Unfreeze();
            foreach (List<PPathwayLine> list in m_lines.Values)
                foreach (PPathwayLine line in list)
                    line.Pickable = true;
        }

        /// <summary>
        /// notify to add the related variable to list.
        /// </summary>
        /// <param name="var">the related variable.</param>
        /// <param name="path">PPath of the related variable.</param>
        private void AddRelatedVariable(PPathwayVariable var, PPathwayLine path)
        {
            string key = var.EcellObject.Key;
            if (m_lines.ContainsKey(key))
            {
                m_lines[key].Add(path);
            }
            else
            {
                m_relatedVariables.Add(key, var);
                List<PPathwayLine> ppaths = new List<PPathwayLine>();
                ppaths.Add(path);
                m_lines.Add(key, ppaths);
            }
        }

        /// <summary>
        /// create edge by using the information of element.
        /// </summary>
        public void CreateEdges()
        {
            // Error Check
            if (this.EcellObject == null || this.EcellObject.ReferenceList == null)
                return;
            List<EcellReference> list = EcellObject.ReferenceList;
            // Check if this node is tarminal node or not.
            bool isEndNode = true;
            foreach (EcellReference er in list)
            {
                if (er.Coefficient != 1)
                    continue;
                isEndNode = false;
                break;
            }

            try
            {
                foreach (EcellReference er in list)
                {
                    if (!base.m_canvas.Variables.ContainsKey(er.Key))
                        continue;

                    PPathwayVariable var = base.m_canvas.Variables[er.Key];
                    EdgeInfo edge = new EdgeInfo(this.EcellObject.Key, er);
                    PPathwayLine path = new PPathwayLine(m_canvas, edge);
                    
                    path.Brush = m_edgeBrush;
                    path.VarPoint = var.GetContactPoint(base.CenterPointF);
                    path.ProPoint = base.GetContactPoint(path.VarPoint);
                    path.SetLine();
                    if (!m_isViewMode || isEndNode || er.Coefficient == 1)
                        path.SetDirection();
                    path.Pickable = (var.Visible && this.Visible);
                    path.Visible = (var.Visible && this.Visible);
                    
                    m_layer.AddChild(path);
                    this.AddRelatedVariable(var, path);
                    var.NotifyAddRelatedProcess(this);
                }
            }catch(Exception e)
            {
                Console.WriteLine(" target is " + e.TargetSite);
            }
        }

        /// <summary>
        /// check whethet exist invalid process in list.
        /// </summary>
        public void RefreshEdges()
        {
            if (base.m_canvas == null || m_ecellObj == null  || m_layer == null)
                return;
            DeleteEdges();
            CreateEdges();
        }
        /// <summary>
        /// Set Line Width.
        /// </summary>
        /// <param name="width"></param>
        public void SetLineWidth(float width)
        {
            foreach (List<PPathwayLine> list in m_lines.Values)
                foreach (PPathwayLine line in list)
                    line.Pen.Width = width;
        }

        /// <summary>
        /// delete the specified related variable from list.
        /// </summary>
        /// <param name="key">key that specifies the variable.</param>
        public void DeleteEdge(string key)
        {
            if (!m_lines.ContainsKey(key))
                return;
            List<PPathwayLine> pathList = m_lines[key];

            foreach (PPath path in pathList)
            {
                if (path.Parent != null)
                {
                    path.Parent.RemoveChild(path);
                }
            }
            m_lines.Remove(key);
            m_relatedVariables.Remove(key);
        }

        /// <summary>
        /// delete all related process from list.
        /// </summary>
        public void DeleteEdges()
        {
            foreach (List<PPathwayLine> pathList in m_lines.Values)
            {
                foreach (PPathwayLine path in pathList)
                {
                    if (path.Parent != null)
                        path.Parent.RemoveChild(path);
                    else
                        path.CloseAllFigures();
                }
            }
            m_lines.Clear();
            m_relatedVariables.Clear();
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
        /// notify to remove all related process from list.
        /// </summary>
        public void NotifyRemoveToRelatedVariable()
        {
            foreach (PPathwayVariable var in m_relatedVariables.Values)
            {
                var.RemoveRelatedProcess(this.EcellObject.Key);
            }
            DeleteEdges();
        }

        /// <summary>
        /// refresh the information of edge.
        /// </summary>
        public override void Refresh()
        {
            RefreshEdges();
            RefreshText();
        }

        /// <summary>
        /// Change View Mode.
        /// </summary>
        public override void RefreshView()
        {
            m_path.Reset();
            PointF centerPos = this.CenterPointF;
            if (m_isViewMode)
                base.AddPath(m_tempFigure.GraphicsPath, false);
            else
                base.AddPath(m_setting.EditModeFigure.GraphicsPath, false);
            base.CenterPointF = centerPos;
            base.RefreshView();
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

        #endregion
    }

    #region Enum
    /// <summary>
    /// Enumeration for a direction of a edge
    /// </summary>
    public enum EdgeDirection
    {
        /// <summary>
        /// Outward direction
        /// </summary>
        Outward,
        /// <summary>
        /// Inward direction
        /// </summary>
        Inward,
        /// <summary>
        /// Outward and inward direction
        /// </summary>
        Bidirection,
        /// <summary>
        /// An edge has no direction
        /// </summary>
        None
    }
    /// <summary>
    /// Enumeration of a type of a line.
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        Unknown,
        /// <summary>
        /// Solid line
        /// </summary>
        Solid,
        /// <summary>
        /// Dashed line
        /// </summary>
        Dashed
    }
    #endregion

    /// <summary>
    /// EdgeInfo contains all information for one edge.
    /// </summary>
    public class EdgeInfo
    {

        #region Fields

        /// <summary>
        /// Key of a process, an owner of this edge.
        /// </summary>
        protected string m_proKey;

        /// <summary>
        /// Key of a variable with which a process has an edge.
        /// </summary>
        protected string m_varKey;

        /// <summary>
        /// Direction of this edge.
        /// </summary>
        protected EdgeDirection m_direction = EdgeDirection.None;

        /// <summary>
        /// Type of a line of this edge.
        /// </summary>
        protected LineType m_type = LineType.Unknown;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EdgeInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proKey">key of process</param>
        /// <param name="varKey">key of variable</param>
        public EdgeInfo(string proKey, string varKey)
        {
            m_proKey = proKey;
            m_varKey = varKey;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processKey">The key of process.</param>
        /// <param name="er">The reference of EcellObject.</param>
        public EdgeInfo(string processKey, EcellReference er)
        {
            m_proKey = processKey;
            // Set Relation
            int l_coef = er.Coefficient;
            if (l_coef < 0)
            {
                m_direction = EdgeDirection.Inward;
                m_type = LineType.Solid;
            }
            else if (l_coef == 0)
            {
                m_direction = EdgeDirection.None;
                m_type = LineType.Dashed;
            }
            else
            {
                m_direction = EdgeDirection.Outward;
                m_type = LineType.Solid;
            }
            m_varKey = er.Key;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string EdgeKey
        {
            get { return m_varKey + ":" + m_direction.ToString(); }
        }
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string VariableKey
        {
            get { return m_varKey; }
            set { m_varKey = value; }
        }
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string ProcessKey
        {
            get { return m_proKey; }
            set { m_proKey = value; }
        }
        /// <summary>
        /// Accessor for m_direction.
        /// </summary>
        public EdgeDirection Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }
        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        public LineType TypeOfLine
        {
            get { return m_type; }
            set { m_type = value; }
        }
        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        public int Coefficient
        {
            get
            {
                int coefficient = 0;
                switch (this.m_direction)
                {
                    case EdgeDirection.Inward:
                        coefficient = -1;
                        break;
                    case EdgeDirection.None:
                        coefficient = 0;
                        break;
                    case EdgeDirection.Outward:
                        coefficient = 1;
                        break;
                }
                return coefficient;
            }
        }

        #endregion
    }
}