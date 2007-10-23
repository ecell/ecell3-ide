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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace EcellLib.PathwayWindow.Nodes
{
    /// <summary>
    /// Subclass of PPathwayNode for variable of E-Cell.
    /// </summary>
    public class PPathwayVariable : PPathwayNode
    {
        #region Static readonly
        /// <summary>
        /// Edges will be refreshed every time when this process has moved by this distance.
        /// </summary>
        protected static readonly float m_refreshDistance = 4;
        #endregion

        /// <summary>
        /// the delta of moving.
        /// </summary>
        protected SizeF m_movingDelta = new SizeF(0, 0);

        /// <summary>
        /// list of related processes.
        /// </summary>
        protected List<PPathwayProcess> m_relatedProcesses = new List<PPathwayProcess>();

        #region Accessors
        /// <summary>
        /// get/set the related element.
        /// </summary>
        public new EcellVariable EcellObject
        {
            get { return (EcellVariable)base.m_ecellObj; }
            set
            {
                base.EcellObject = value;
                this.Name = value.name;
                Refresh();
            }
        }
        #endregion

        /// <summary>
        /// create new PEcellVariable.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayVariable();
        }

        /// <summary>
        /// event on mouse drag on this node.
        /// </summary>
        /// <param name="e"></param>
        public override void OnMouseDrag(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDrag(e);
            this.Refresh();
        }

        /// <summary>
        /// event on mouse up on this variable.
        /// </summary>
        /// <param name="e">PInputEventArgs.</param>
        public override void OnMouseUp(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseUp(e);
            foreach (PPathwayProcess process in m_relatedProcesses)
            {
                process.RefreshEdges();
            }
            m_movingDelta = new SizeF(0, 0);
        }

        /// <summary>
        /// event on double click on this variable.
        /// </summary>
        /// <param name="e">PInputEventArgs.</param>
        public override void OnDoubleClick(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            PropertyEditor editor = new PropertyEditor();
            editor.layoutPanel.SuspendLayout();
            editor.SetCurrentObject(EcellObject);
            editor.SetDataType(EcellObject.type);
            editor.PEApplyButton.Click += new EventHandler(editor.UpdateProperty);
            editor.LayoutPropertyEditor();
            editor.layoutPanel.ResumeLayout(false);
            editor.ShowDialog();
        }

        /// <summary>
        /// reconstruct the list of related process by using the information of elements.
        /// </summary>
        public override void ValidateEdges()
        {
            List<PPathwayProcess> newProcesses = new List<PPathwayProcess>();
            foreach (PPathwayProcess process in m_relatedProcesses)
            {
                if (base.m_set.Processes.ContainsKey(process.EcellObject.key))
                    newProcesses.Add(process);
            }
            m_relatedProcesses = newProcesses;
        }

        /// <summary>
        /// notify to move this node in canvas.
        /// </summary>
        public override void NotifyMovement()
        {
            foreach (PPathwayProcess p in m_relatedProcesses)
            {
                p.RefreshEdges();
            }
        }

        /// <summary>
        /// notify to add the related process to list.
        /// </summary>
        /// <param name="pro">the related process.</param>
        public void NotifyAddRelatedProcess(PPathwayProcess pro)
        {
            if (!m_relatedProcesses.Contains(pro))
                m_relatedProcesses.Add(pro);
        }

        /// <summary>
        /// before it delete this variable,
        /// notify to remove the related variable from list.
        /// </summary>
        public void NotifyRemoveRelatedProcess()
        {
            List<PPathwayProcess> list = new List<PPathwayProcess>();
            foreach (PPathwayProcess p in m_relatedProcesses)
                list.Add(p);
            m_relatedProcesses.Clear();

            foreach (PPathwayProcess p in list)
            {
                p.NotifyRemoveRelatedVariable(this.EcellObject.key);
                p.Refresh();
            }
        }

        /// <summary>
        /// notify to remove the related process from list.
        /// </summary>
        /// <param name="key"></param>
        public void NotifyRemoveRelatedProcess(string key)
        {
            List<PPathwayProcess> rList = new List<PPathwayProcess>();
            foreach (PPathwayProcess p in m_relatedProcesses)
            {
                if (p.EcellObject.key.Equals(key))
                {
                    rList.Add(p);
                }
            }

            foreach (PPathwayProcess p in rList)
            {
                m_relatedProcesses.Remove(p);
            }
            rList.Clear();
        }

        /// <summary>
        /// reconstruct the information of edge.
        /// </summary>
        public override void Refresh()
        {
            ValidateEdges();
            foreach(PPathwayProcess p in m_relatedProcesses)
            {
                p.RefreshEdges();
            }
            RefreshText();
        }

        /// <summary>
        /// start to move this Node by drag.
        /// </summary>
        public override void MoveStart()
        {
            foreach (PPathwayProcess p in m_relatedProcesses)
            {
                p.DeleteEdge(this);
                p.RefreshEdges();
            }
        }

        /// <summary>
        /// end to move this Node by drag.
        /// </summary>
        public override void MoveEnd()
        {
            foreach (PPathwayProcess p in m_relatedProcesses)
            {
//                p.DeleteEdges();
                p.MoveEnd();
            }
        }
    }
}