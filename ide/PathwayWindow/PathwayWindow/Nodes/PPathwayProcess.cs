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
        /// dictionary of Line for variable. key is PPEcellVariable.
        /// </summary>
        protected Dictionary<PPathwayVariable,List<Line>> m_relatedVariables = new Dictionary<PPathwayVariable,List<Line>>();

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
                this.Name = value.name;
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
        /// Create edge from PEcellVariable.
        /// </summary>
        /// <param name="variable">PEcellVariable</param>
        /// <param name="coefficient">coefficient</param>
        public void CreateEdge(PPathwayVariable variable, int coefficient)
        {
            EcellProcess obj = this.EcellObject;
            List<EcellReference> list = obj.ReferenceList;
            string variableKey = variable.EcellObject.key;

            // If this process and variable are connected in the same direction, nothing will be done.
            if (CheckReferenceListContainsEntity(list, variableKey, coefficient))
            {
                MessageBox.Show(m_resources.GetString("ErrAlrConnect"),
                 "Notice",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Exclamation);
                return;
            }
            string name;
            string pre;
            int k = 0;

            if (coefficient == 0)
                pre = "C";
            else if (coefficient == -1)
                pre = "S";
            else
                pre = "P";

            while (true)
            {
                bool ishit = false;
                name = pre + k;
                foreach (EcellReference r in list)
                {
                    if (r.name == name)
                    {
                        k++;
                        ishit = true;
                        continue;
                    }
                }
                if (ishit == false) break;
            }


            EcellReference er = new EcellReference();
            er.name = name;
            er.Key = variableKey;
            er.coefficient = coefficient;
            er.isAccessor = 1;

            list.Add(er);
            obj.ReferenceList = list;
            RefreshEdges();

            variable.NotifyAddRelatedProcess(this);
        }
        /// <summary>
        /// This method checks whether an EcellReference list contain a key with same coefficient or not.
        /// </summary>
        /// <param name="list">list of EcellReference to be checked</param>
        /// <param name="key">key of Entity</param>
        /// <param name="coefficient">coefficient of reference</param>
        /// <returns>true if a list contains a key. false if a list doesn't contain a key</returns>
        private static bool CheckReferenceListContainsEntity(List<EcellReference> list, string key, int coefficient)
        {
            // null check
            if (null == list || null == key)
                return false;

            bool contains = false;

            foreach (EcellReference reference in list)
            {
                if (reference.fullID.EndsWith(key))
                {
                    if (reference.coefficient * coefficient >= 0)
                        contains = true;
                }
            }

            return contains;
        }

        /// <summary>
        /// create edge by using the information of element.
        /// </summary>
        public void CreateEdges()
        {
            DeleteEdges();
            if (this.EcellObject == null || this.EcellObject.ReferenceList == null)
                return;

            try
            {
                foreach (EcellReference er in EcellObject.ReferenceList)
                {
                    if (!base.m_set.Variables.ContainsKey(er.Key))
                        continue;

                    PPathwayVariable var = base.m_set.Variables[er.Key];
                    EdgeInfo edge = new EdgeInfo(this.EcellObject.key, er);
                    Line path = new Line(edge);
                    
                    path.MouseDown += this.m_handler4Line;
                    path.Brush = Brushes.Black;
                    path.VarPoint = var.GetContactPoint(base.CenterPointToCanvas);
                    path.ProPoint = base.GetContactPoint(path.VarPoint);
                    path.DrawLine();
                    path.Pickable = true;
                    
                    this.ParentObject.AddChild(path);
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
            if (base.m_set == null || this.EcellObject == null)
                return;
            DeleteEdges();
            CreateEdges();
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
            m_relatedVariables = new Dictionary<PPathwayVariable, List<Line>>();
        }

        /// <summary>
        /// delete the specified related variable from list.
        /// </summary>
        /// <param name="p">the specified variable.</param>
        public void DeleteEdge(PPathwayVariable p)
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
            List<PPathwayVariable> rList = new List<PPathwayVariable>();
            foreach (PPathwayVariable p in m_relatedVariables.Keys)
            {
                if (p.EcellObject.key.StartsWith(key))
                {
                    rList.Add(p);
                }
            }

            foreach (PPathwayVariable p in rList)
            {
                m_relatedVariables.Remove(p);
            }
            rList.Clear();
        }

        /// <summary>
        /// notify to remove all related process from list.
        /// </summary>
        public void NotifyRemoveRelatedVariable()
        {
            foreach (PPathwayVariable p in m_relatedVariables.Keys)
            {
                p.NotifyRemoveRelatedProcess(this.EcellObject.key);
            }
            DeleteEdges();
            RefreshEdges();
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

        /// <summary>
        /// Type of a line of this edge.
        /// </summary>
        protected int m_isFixed = 0;
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
        /// <param name="value">EcellValue</param>
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
            m_isFixed = er.isAccessor;
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
        public int IsFixed
        {
            get { return m_isFixed; }
            set { m_isFixed = value; }
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