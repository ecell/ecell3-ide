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

namespace Ecell.IDE.Plugins.StaticDebugWindow
{
    /// <summary>
    /// Static debug for network compliance.
    /// </summary>
    class StaticDebugForNetwork : StaticDebugPlugin
    {
        /// <summary>
        /// Owner of this object
        /// </summary>
        private StaticDebugWindow m_owner;

        private Dictionary<string, EcellObject> m_existVariableList;

        private Dictionary<string, EcellObject> m_existProcessList;
        /// <summary>
        /// List of error message.
        /// </summary>
        private List<ErrorMessage> m_errorList = new List<ErrorMessage>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public StaticDebugForNetwork(StaticDebugWindow owner)
        {
            m_owner = owner;
            m_existVariableList = new Dictionary<string, EcellObject>();
            m_existProcessList = new Dictionary<string, EcellObject>();
        }

        /// <summary>
        /// Debugger Name.
        /// </summary>
        /// <returns>"Network Compliance."</returns>
        public string GetDebugName()
        {
            return MessageResStDebug.NetworkComplianceName;
        }

        /// <summary>
        /// Execute static debug.
        /// </summary>
        /// <param name="l_data">The list of object to be checked.</param>
        /// <returns>The list of error messages.</returns>
        public List<ErrorMessage> Debug(List<EcellObject> l_data)
        {
            m_errorList.Clear();
            m_existProcessList.Clear();
            m_existVariableList.Clear();

            foreach (EcellObject obj in l_data)
            {
                if (obj.Type == Constants.xpathProcess)
                    m_existProcessList.Add(Constants.xpathProcess + 
                        Constants.delimiterColon + obj.Key, obj);
                if (obj.Type == Constants.xpathVariable)
                    m_existVariableList.Add(Constants.xpathVariable + 
                        Constants.delimiterColon + obj.Key, obj);

                if (obj.Children == null) continue;
                foreach (EcellObject cobj in obj.Children)
                {
                    if (cobj.Type == Constants.xpathProcess)
                        m_existProcessList.Add(Constants.xpathProcess +
                            Constants.delimiterColon + cobj.Key, cobj);
                    if (cobj.Type == Constants.xpathVariable)
                        m_existVariableList.Add(Constants.xpathVariable +
                            Constants.delimiterColon + cobj.Key, cobj);
                }
            }
            CheckNoConnect();
            CheckExistVariable();

            return m_errorList;
        }

        /// <summary>
        /// Check whether Variable in VariableReferenceList exist.
        /// </summary>
        private void CheckExistVariable()
        {
            foreach (EcellObject obj in m_existProcessList.Values)
            {
                if (obj.Value == null) continue;
                foreach (EcellData d in obj.Value)
                {
                    if (d.Name != Constants.xpathVRL) continue;
                    List<EcellValue> rList = d.Value.CastToList();
                    if (rList == null) continue;
                    foreach (EcellValue v in rList)
                    {
                        string systemPath = "";
                        List<EcellValue> vList = v.CastToList();
                        EcellValue vData = vList[1];
                        string[] data = vData.ToString().Split(new char[] { ':' });
                        if (data[1].Equals("."))
                        {
                            Util.GetNameFromPath(obj.Key, ref systemPath);
                        }
                        else
                        {
                            systemPath = data[1];
                        }
                        systemPath = Constants.xpathVariable + Constants.delimiterColon +
                            systemPath + Constants.delimiterColon + data[2];
                        if (!m_existVariableList.ContainsKey(systemPath))
                        {
                            ErrorMessage mes = new ErrorMessage(obj.ModelID, obj.Type,
                                d.EntityPath,
                                MessageResStDebug.ErrNoVariable);
                            m_errorList.Add(mes);
                            break;
                        }

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
                if (id.EndsWith(":SIZE")) continue;
                valDic.Add(id, m_existVariableList[id]);
            }

            foreach (EcellObject obj in m_existProcessList.Values)
            {
                if (obj.Value == null)
                {
                    ErrorMessage mes = new ErrorMessage(obj.ModelID, obj.Type,
                        Constants.xpathProcess + Constants.delimiterColon + 
                        obj.Key + Constants.delimiterColon + Constants.xpathVRL,
                        MessageResStDebug.ErrNoConnect);
                    m_errorList.Add(mes);
                    continue;
                }
                foreach (EcellData d in obj.Value)
                {
                    if (d.Name != Constants.xpathVRL) continue;

                    List<EcellValue> rList = d.Value.CastToList();
                    if (rList == null || rList.Count <= 0)
                    {
                        ErrorMessage mes = new ErrorMessage(obj.ModelID, obj.Type,
                            d.EntityPath,
                            MessageResStDebug.ErrNoConnect);
                        m_errorList.Add(mes);
                        break;
                    }
                    foreach (EcellValue v in rList)
                    {
                        string systemPath = "";
                        List<EcellValue> vList = v.CastToList();
                        EcellValue vData = vList[1];
                        string[] data = vData.ToString().Split(new char[] { ':' });
                        if (data[1].Equals("."))
                        {
                            Util.GetNameFromPath(obj.Key, ref systemPath);
                        }
                        else
                        {
                            systemPath = data[1];
                        }
                        systemPath = Constants.xpathVariable + Constants.delimiterColon +
                            systemPath + Constants.delimiterColon + data[2];

                        if (valDic.ContainsKey(systemPath))
                            valDic.Remove(systemPath);
                    }
                }
            }
            foreach (string key in valDic.Keys)
            {
                EcellObject obj = valDic[key];
                ErrorMessage mes = new ErrorMessage(obj.ModelID, obj.Type,
                    Constants.xpathVariable + Constants.delimiterColon + obj.Key + 
                    Constants.delimiterColon + Constants.xpathID,
                    MessageResStDebug.ErrNoConnect);
                m_errorList.Add(mes);
            }
        }
    }
}
