//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Ecell;
using Ecell.Objects;
using Ecell.Reporting;

namespace Ecell.IDE.Plugins.StaticDebugWindow
{
    /// <summary>
    /// Static debug for network compliance.
    /// </summary>
    public class StaticDebugForNetwork : IStaticDebugPlugin
    {
        #region Fields
        /// <summary>
        /// Owner of this object
        /// </summary>
        private StaticDebugWindow m_owner;
        /// <summary>
        /// The list of exist variable.
        /// </summary>
        private Dictionary<string, EcellObject> m_existVariableList;
        /// <summary>
        /// The list of exist process.
        /// </summary>
        private Dictionary<string, EcellObject> m_existProcessList;
        /// <summary>
        /// List of error message.
        /// </summary>
        private List<IReport> m_errorList = new List<IReport>();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public StaticDebugForNetwork(StaticDebugWindow owner)
        {
            m_owner = owner;
            m_existVariableList = new Dictionary<string, EcellObject>();
            m_existProcessList = new Dictionary<string, EcellObject>();
        }
        #endregion

        /// <summary>
        /// Debugger Name.
        /// </summary>
        /// <returns>"Network Compliance."</returns>
        public string Name
        {
            get { return MessageResources.NetworkComplianceName; }
        }

        /// <summary>
        /// Execute static debug.
        /// </summary>
        /// <param name="l_data">The list of object to be checked.</param>
        /// <returns>The list of error messages.</returns>
        public IEnumerable<IReport> Debug(List<EcellObject> l_data)
        {
            m_errorList.Clear();
            m_existProcessList.Clear();
            m_existVariableList.Clear();

            foreach (EcellObject obj in l_data)
            {
                if (obj.Type == Constants.xpathProcess && !obj.Classname.Equals("MassCalculationProcess"))
                    m_existProcessList.Add(Constants.xpathProcess + 
                        Constants.delimiterColon + obj.Key, obj);
                if (obj.Type == Constants.xpathVariable)
                    m_existVariableList.Add(Constants.xpathVariable + 
                        Constants.delimiterColon + obj.Key, obj);

                if (obj.Children == null) continue;
                foreach (EcellObject cobj in obj.Children)
                {
                    if (cobj.Type == Constants.xpathProcess && !cobj.Classname.Equals("MassCalculationProcess"))
                        m_existProcessList.Add(Constants.xpathProcess +
                            Constants.delimiterColon + cobj.Key, cobj);
                    if (cobj.Type == Constants.xpathVariable)
                        m_existVariableList.Add(Constants.xpathVariable +
                            Constants.delimiterColon + cobj.Key, cobj);
                }
            }
            CheckNoConnect();
            CheckExistVariable();
            CheckSameVariable();

            return m_errorList;
        }

        /// <summary>
        /// Check whether process have the reference to the same variable.
        /// </summary>
        private void CheckSameVariable()
        {
            foreach (EcellObject obj in m_existProcessList.Values)
            {
                List<string> refstrList = new List<string>();
                EcellProcess process = (EcellProcess)obj;
                List<EcellReference> refList = process.ReferenceList;
                foreach (EcellReference er in refList)
                {
                    if (refstrList.Contains(er.FullID))
                    {
                        m_errorList.Add(new ObjectPropertyReport(
                            MessageType.Warning,
                            MessageResources.ErrSameVariable,
                            Constants.groupDebug,
                            obj,
                            EcellProcess.VARIABLEREFERENCELIST));
                        break;
                    }
                    refstrList.Add(er.FullID);
                }
            }
        }

        /// <summary>
        /// Check whether Variable in VariableReferenceList exist.
        /// </summary>
        private void CheckExistVariable()
        {
            foreach (EcellObject obj in m_existProcessList.Values)
            {
                EcellProcess process = (EcellProcess)obj;
                List<EcellReference> refList = process.ReferenceList;
                foreach (EcellReference er in refList)
                {
                    if (!m_existVariableList.ContainsKey(er.FullID))
                    {
                        m_errorList.Add(new ObjectPropertyReport(
                            MessageType.Error,
                            MessageResources.ErrNoVariable,
                            Constants.groupDebug,
                            obj,
                            EcellProcess.VARIABLEREFERENCELIST
                        ));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Check whether process and variable have the connection to other object.
        /// </summary>
        private void CheckNoConnect()
        {
            Dictionary<string, EcellObject> valDic = new Dictionary<string, EcellObject>();
            foreach (string id in m_existVariableList.Keys)
            {
                if (id.EndsWith(":SIZE"))
                    continue;
                valDic.Add(id, m_existVariableList[id]);
            }

            foreach (EcellObject obj in m_existProcessList.Values)
            {
                EcellProcess process = (EcellProcess)obj;
                if (process.Classname.Equals("MassCalculationProcess"))
                    continue;
                List<EcellReference> refList = process.ReferenceList;
                if (refList.Count <= 0)
                {
                    m_errorList.Add(new ObjectReport(
                        MessageType.Warning,
                        MessageResources.ErrNoConnect,
                        Constants.groupDebug,
                        obj
                    ));
                    continue;
                }
                foreach (EcellReference er in refList)
                {
                    if (valDic.ContainsKey(er.FullID))
                        valDic.Remove(er.FullID);

                }
            }
            foreach (string key in valDic.Keys)
            {
                EcellObject obj = valDic[key];
                m_errorList.Add(new ObjectReport(
                    MessageType.Warning,
                    MessageResources.ErrNoConnect,
                    Constants.groupDebug,
                    obj
                ));
            }
        }
    }
}
