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

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// PPathwayObject for E-Cell variable.
    /// </summary>
    public class PPathwayProcess : PPathwayNode
    {
        #region Static readonly fields

        /// <summary>
        /// Edges will be refreshed every time when this process has moved by this distance.
        /// </summary>
        protected static readonly float REFRESH_DISTANCE = 4;
        #endregion

        #region Fields
        /// <summary>
        /// dictionary of Line. Key is node.EcellObject.key.
        /// </summary>
        protected Dictionary<string, List<Line>> m_lines = new Dictionary<string, List<Line>>();
        /// <summary>
        /// dictionary of related PPathwayVariable. Key is node.EcellObject.key.
        /// </summary>
        protected Dictionary<string, PPathwayVariable> m_relatedVariables = new Dictionary<string, PPathwayVariable>();

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
        public new EcellProcess EcellObject
        {
            get { return (EcellProcess)base.m_ecellObj; }
            set
            {
                base.EcellObject = value;
                Refresh();
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
            foreach (List<Line> list in m_lines.Values)
                foreach (Line line in list)
                    line.Pickable = false;
        }

        /// <summary>
        /// Unfreeze this object and related lines.
        /// </summary>
        public override void Unfreeze()
        {
            base.Unfreeze();
            foreach (List<Line> list in m_lines.Values)
                foreach (Line line in list)
                    line.Pickable = true;
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
            this.RefreshText();
            this.RefreshEdges();
        }

        /// <summary>
        /// event on double click this node.
        /// </summary>
        /// <param name="e"></param>
        public override void OnDoubleClick(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {

            EcellObject obj = this.EcellObject;

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
        private void AddRelatedVariable(PPathwayVariable var, Line path)
        {
            string key = var.EcellObject.key;
            if (m_lines.ContainsKey(key))
            {
                m_lines[key].Add(path);
            }
            else
            {
                m_relatedVariables.Add(key, var);
                List<Line> ppaths = new List<Line>();
                ppaths.Add(path);
                m_lines.Add(key, ppaths);
            }
        }

        /// <summary>
        /// create edge by using the information of element.
        /// </summary>
        public void CreateEdges()
        {
            if (this.EcellObject == null || this.EcellObject.ReferenceList == null)
                return;

            try
            {
                foreach (EcellReference er in EcellObject.ReferenceList)
                {
                    if (!base.m_canvas.Variables.ContainsKey(er.Key))
                        continue;

                    PPathwayVariable var = base.m_canvas.Variables[er.Key];
                    EdgeInfo edge = new EdgeInfo(this.EcellObject.key, er);
                    Line path = new Line(edge);
                    
                    path.MouseDown += this.m_handler4Line;
                    path.Brush = Brushes.Black;
                    path.VarPoint = var.GetContactPoint(base.CenterPoint);
                    path.ProPoint = base.GetContactPoint(path.VarPoint);
                    path.DrawLine();
                    path.Pickable = true;
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
            if (base.m_canvas == null || this.EcellObject == null)
                return;
            DeleteEdges();
            CreateEdges();
        }

        /// <summary>
        /// delete all related process from list.
        /// </summary>
        public override void Delete()
        {
            NotifyRemoveToRelatedVariable();
            DeleteEdges();
        }

        /// <summary>
        /// delete the specified related variable from list.
        /// </summary>
        /// <param name="key">key that specifies the variable.</param>
        public void DeleteEdge(string key)
        {
            if (!m_lines.ContainsKey(key))
                return;
            List<Line> pathList = m_lines[key];

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
            foreach (List<Line> pathList in m_lines.Values)
            {
                foreach (Line path in pathList)
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
                var.RemoveRelatedProcess(this.EcellObject.key);
            }
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
        /// start to move this Node by drag.
        /// </summary>
        public override void MoveStart()
        {
            RefreshEdges();
        }

        /// <summary>
        /// end to move this Node by drag.
        /// </summary>
        public override void MoveEnd()
        {
            RefreshEdges();
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
            int l_coef = er.coefficient;
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