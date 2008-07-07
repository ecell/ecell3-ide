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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Drawing.Drawing2D;
using Ecell.Objects;
using UMD.HCIL.Piccolo;
using Ecell.IDE.Plugins.PathwayWindow;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
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
        protected const float m_refreshDistance = 4;

        private List<PPathwayAlias> m_aliases = new List<PPathwayAlias>();
        #endregion

        /// <summary>
        /// list of related processes.
        /// </summary>
        protected Dictionary<string, PPathwayProcess> m_relatedProcesses = new Dictionary<string, PPathwayProcess>();

        #region Accessors
        /// <summary>
        /// get/set the related element.
        /// </summary>
        public override EcellObject EcellObject
        {
            get { return base.m_ecellObj; }
            set
            {
                base.EcellObject = value;
                ResetAlias();
            }
        }
        #endregion

        private void ResetAlias()
        {
            if (m_ecellObj == null)
                return;
            // Remove current alias
            foreach (PPathwayAlias alias in m_aliases)
                this.RemoveChild(alias);

            // Set alias
            foreach (EcellObject eo in m_ecellObj.Children)
            {
                PPathwayAlias alias = new PPathwayAlias(this);
                alias.Setting = m_setting;
                alias.EcellObject = eo;
                m_canvas.SetLayer(alias);
                m_aliases.Add(alias);
                alias.RefreshView();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PPathwayVariable()
        {
        }
        /// <summary>
        /// create new PPathwayVariable.
        /// </summary>
        /// <returns></returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayVariable();
        }

        /// <summary>
        /// reconstruct the list of related process by using the information of elements.
        /// </summary>
        public override void ValidateEdges()
        {
            Dictionary<string, PPathwayProcess> newProcesses = new Dictionary<string, PPathwayProcess>();
            foreach (KeyValuePair<string, PPathwayProcess> process in m_relatedProcesses)
            {
                if (base.m_canvas.Processes.ContainsKey(process.Key))
                    newProcesses.Add(process.Key, process.Value);
            }
            m_relatedProcesses = newProcesses;
        }

        /// <summary>
        /// Change View Mode.
        /// </summary>
        public override void RefreshView()
        {
            if (m_isViewMode)
                m_pPropertyText.Visible = true;
            else
                m_pPropertyText.Visible = false;
            base.RefreshView();
        }
        /// <summary>
        /// notify to add the related process to list.
        /// </summary>
        /// <param name="pro">the related process.</param>
        public void NotifyAddRelatedProcess(PPathwayProcess pro)
        {
            if (!m_relatedProcesses.ContainsKey(pro.EcellObject.Key))
                m_relatedProcesses.Add(pro.EcellObject.Key, pro);
        }

        /// <summary>
        /// before it delete this variable,
        /// notify to remove the related variable from list.
        /// </summary>
        public void NotifyRemoveToRelatedProcess()
        {
            foreach (PPathwayProcess process in m_relatedProcesses.Values)
                process.DeleteEdge(this.EcellObject.Key);
            m_relatedProcesses.Clear();
        }

        /// <summary>
        /// notify to remove the related process from list.
        /// </summary>
        /// <param name="key"></param>
        public void RemoveRelatedProcess(string key)
        {
            if (!m_relatedProcesses.ContainsKey(key))
                return;

            m_relatedProcesses.Remove(key);
        }

        /// <summary>
        /// reconstruct the information of edge.
        /// </summary>
        public override void Refresh()
        {
            ValidateEdges();
            foreach(PPathwayProcess process in m_relatedProcesses.Values)
            {
                process.RefreshEdges();
            }
            RefreshText();
        }
    }
}