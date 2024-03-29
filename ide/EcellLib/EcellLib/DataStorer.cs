﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using EcellCoreLib;
using Ecell.Objects;
using System.Diagnostics;
using System.Collections;

namespace Ecell
{
    /// <summary>
    /// DataStorer
    /// </summary>
    public class DataStorer
    {
        #region DataStored
        /// <summary>
        /// Stores the "EcellObject"
        /// </summary>
        /// <param name="simulator">The "simulator"</param>
        /// <param name="dmm">The "DMDescriptorKeeper"</param>
        /// <param name="ecellObject">The stored "EcellObject"</param>
        /// <param name="initialCondition">The initial condition.</param>
        internal static void DataStored(
                WrappedSimulator simulator,
                DMDescriptorKeeper dmm,
                EcellObject ecellObject,
                Dictionary<string, double> initialCondition)
        {
            if (ecellObject.Type.Equals(Constants.xpathStepper))
            {
                DataStored4Stepper(simulator, dmm, ecellObject);
            }
            else if (ecellObject.Type.Equals(Constants.xpathSystem))
            {
                DataStored4System(
                        simulator,
                        ecellObject,
                        initialCondition);
            }
            else if (ecellObject.Type.Equals(Constants.xpathProcess))
            {
                DataStored4Process(
                        simulator,
                        dmm,
                        ecellObject,
                        initialCondition);
            }
            else if (ecellObject.Type.Equals(Constants.xpathVariable))
            {
                DataStored4Variable(
                        simulator,
                        ecellObject,
                        initialCondition);
            }
            //
            // 4 children
            //
            if (ecellObject.Children != null)
            {
                foreach (EcellObject childEcellObject in ecellObject.Children)
                    DataStored(simulator, dmm, childEcellObject, initialCondition);
            }
        }
        
        /// <summary>
        /// Stores the "EcellObject" 4 the "Process".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="dmm">The "DMDescriptorKeeper"</param>
        /// <param name="ecellObject">The stored "Process"</param>
        /// <param name="initialCondition">The initial condition.</param>
        internal static void DataStored4Process(
                WrappedSimulator simulator,
                DMDescriptorKeeper dmm,
                EcellObject ecellObject,
                Dictionary<string, double> initialCondition)
        {
            string key = ecellObject.FullID;
            IList<string> wrappedPolymorph = null;
            try
            {
                wrappedPolymorph = simulator.GetEntityPropertyList(key);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return;
            }
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> processEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> storedEcellDataDic = new Dictionary<string, EcellData>();

            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData storedEcellData in ecellObject.Value)
                {
                    storedEcellDataDic[storedEcellData.Name] = storedEcellData;
                    processEcellDataList.Add(storedEcellData);

                }
            }
            //
            // Stores the "EcellData"
            //
            foreach (string name in wrappedPolymorph)
            {
                string entityPath = Util.BuildFullPN(key, name);

                PropertyAttributes flag = simulator.GetEntityPropertyAttributes(entityPath);
                if (!flag.Gettable)
                {
                    continue;
                }
                EcellValue value = null;

                if (name == Constants.xpathVRL)
                {
                    // Won't restore the variable reference list from the simulator's corresponding
                    // object.
                    if (storedEcellDataDic.ContainsKey(name))
                        value = storedEcellDataDic[name].Value;
                    else
                        value = new EcellValue(new List<object>());
                }
                else if (name == Constants.xpathActivity || name == Constants.xpathMolarActivity)
                {
                    value = new EcellValue(0.0);
                }
                else
                {
                    try
                    {
                        value = new EcellValue(simulator.GetEntityProperty(entityPath));
                        if (dmm.ContainsDescriptor(ecellObject.Type, ecellObject.Classname))
                        {
                            DMDescriptor desc = dmm.GetDMDescriptor(ecellObject.Type, ecellObject.Classname);
                            if (desc.ContainsProperty(name))
                            {
                                PropertyDescriptor prop = desc[name];
                                if (prop.DefaultValue.Type == EcellValueType.List && !value.IsList)
                                    value = new EcellValue(new List<EcellValue>());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                        value = GetValueFromDMM(dmm, ecellObject.Type, ecellObject.Classname, name);
                    }
                }
                EcellData ecellData = CreateEcellData(name, value, entityPath, flag);
                if (ecellData.Value != null)
                {
                    ecellData.Logable = ecellData.Value.IsDouble &&
                        (ecellData.Settable == false || ecellData.Saveable == false);

                }

                if (storedEcellDataDic.ContainsKey(name))
                {
                    ecellData.Logged = storedEcellDataDic[name].Logged;
                    processEcellDataList.Remove(storedEcellDataDic[name]);
                }
                processEcellDataList.Add(ecellData);

            }
            ecellObject.SetEcellDatas(processEcellDataList);
        }

        /// <summary>
        /// GetValueFromDMM
        /// </summary>
        /// <param name="dmm">DMDescriptorKeeper</param>
        /// <param name="type">DM type.</param>
        /// <param name="className">the class name,</param>
        /// <param name="name">the property name.</param>
        /// <returns></returns>
        private static EcellValue GetValueFromDMM(DMDescriptorKeeper dmm, string type, string className, string name)
        {
            EcellValue value = null;
            try
            {
                if (dmm.ContainsDescriptor(type, className))
                {
                    DMDescriptor desc = dmm.GetDMDescriptor(type, className);
                    if (desc.ContainsProperty(name))
                    {
                        PropertyDescriptor prop = desc[name];
                        value = new EcellValue(prop.DefaultValue);
                    }
                    else
                    {
                        value = new EcellValue(0.0);
                    }
                }
                else
                {
                    value = new EcellValue("");
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            return value;
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Stepper".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="dmm">The "DynamicModuleManager"</param>
        /// <param name="ecellObject">The stored "Stepper"</param>
        internal static void DataStored4Stepper(
            WrappedSimulator simulator,
            DMDescriptorKeeper dmm,
            EcellObject ecellObject)
        {
            string key = Constants.xpathStepper + Constants.delimiterColon + Constants.delimiterColon + ecellObject.Key;
            List<EcellData> stepperEcellDataList = new List<EcellData>();
            IList<string> wrappedPolymorph = null;
            //
            // Property List
            //
            try
            {
                wrappedPolymorph = simulator.GetStepperPropertyList(ecellObject.Key);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.StackTrace);
                return;
            }
            //
            // Sets the class name.
            //
            if (string.IsNullOrEmpty(ecellObject.Classname))
            {
                ecellObject.Classname = simulator.GetStepperClassName(ecellObject.Key);
            }
            //
            // Checks the stored "EcellData"
            //
            Dictionary<string, EcellData> storedEcellDataDic = new Dictionary<string, EcellData>();
            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData storedEcellData in ecellObject.Value)
                {
                    storedEcellDataDic[storedEcellData.Name] = storedEcellData;
                    stepperEcellDataList.Add(storedEcellData);
                }
            }
            //
            // Stores the "EcellData"
            //
            foreach (string name in wrappedPolymorph)
            {
                string entityPath = key + Constants.delimiterColon + name;
                PropertyAttributes flag = simulator.GetStepperPropertyAttributes(ecellObject.Key, name);
                if (!flag.Gettable)
                {
                    continue;
                }
                EcellValue value = null;
                try
                {
                    object property = simulator.GetStepperProperty(ecellObject.Key, name);
                    value = new EcellValue(property);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    value = GetValueFromDMM(dmm, ecellObject.Type, ecellObject.Classname, name);
                }
                EcellData ecellData = CreateEcellData(name, value, entityPath, flag);
                if (storedEcellDataDic.ContainsKey(name))
                {
                    if (value.IsString && value.ToString().Equals(""))
                    {
                        continue;
                    }
                    else
                    {
                        stepperEcellDataList.Remove(storedEcellDataDic[name]);
                    }
                }
                stepperEcellDataList.Add(ecellData);
            }
            ecellObject.SetEcellDatas(stepperEcellDataList);
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "System".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="ecellObject">The stored "System"</param>
        /// <param name="initialCondition">The initial condition.</param>
        internal static void DataStored4System(
                WrappedSimulator simulator,
                EcellObject ecellObject,
                Dictionary<string, double> initialCondition)
        {
            // Creates an entityPath.
            string parentPath = ecellObject.ParentSystemID;
            string childPath = ecellObject.LocalID;
            string key = Constants.xpathSystem + Constants.delimiterColon +
                parentPath + Constants.delimiterColon +
                childPath;
            // Property List
            IList<string> wrappedPolymorph = simulator.GetEntityPropertyList(key);
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> systemEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> storedEcellDataDic
                    = new Dictionary<string, EcellData>();
            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData storedEcellData in ecellObject.Value)
                {
                    storedEcellDataDic[storedEcellData.Name] = storedEcellData;
                    systemEcellDataList.Add(storedEcellData);

                }
            }
            foreach (string name in wrappedPolymorph)
            {
                string entityPath = key + Constants.delimiterColon + name;
                PropertyAttributes flag = simulator.GetEntityPropertyAttributes(entityPath);

                if (!flag.Gettable)
                {
                    continue;
                }

                object value = null;
                if (name.Equals(Constants.xpathSize))
                {
                    value = new EcellValue(EcellSystem.DefaultSize);
                }
                else
                {
                    try
                    {
                        value = new EcellValue(simulator.GetEntityProperty(entityPath));
                    }
                    catch (WrappedException ex)
                    {
                        Trace.WriteLine(ex);
                        if (storedEcellDataDic.ContainsKey(name))
                        {
                            IEnumerable val = storedEcellDataDic[name].Value as IEnumerable;
                            object firstItem = null;
                            {
                                IEnumerator i = val.GetEnumerator();
                                if (i.MoveNext())
                                    firstItem = i.Current;
                            }
                            if (firstItem is IEnumerable)
                            {
                                value = val;
                            }
                            else
                            {
                                value = firstItem;
                            }
                        }
                        else
                        {
                            value = "";
                        }
                    }
                }

                EcellData ecellData = CreateEcellData(name, new EcellValue(value), entityPath, flag);
                if (ecellData.Value != null)
                {
                    ecellData.Logable = ecellData.Value.IsDouble;
                }
                if (storedEcellDataDic.ContainsKey(name))
                {
                    ecellData.Logged = storedEcellDataDic[name].Logged;
                    systemEcellDataList.Remove(storedEcellDataDic[name]);
                }
                systemEcellDataList.Add(ecellData);
            }

            ecellObject.SetEcellDatas(systemEcellDataList);
        }

        /// <summary>
        /// Stores the "EcellObject" 4 the "Variable".
        /// </summary>
        /// <param name="simulator">The simulator</param>
        /// <param name="ecellObject">The stored "Variable"</param>
        /// <param name="initialCondition">The initial condition.</param>
        internal static void DataStored4Variable(
                WrappedSimulator simulator,
                EcellObject ecellObject,
                Dictionary<string, double> initialCondition)
        {
            string fullID = ecellObject.FullID;
            IList<string> wrappedPolymorph = simulator.GetEntityPropertyList(fullID);
            //
            // Checks the stored "EcellData"
            //
            List<EcellData> variableEcellDataList = new List<EcellData>();
            Dictionary<string, EcellData> storedEcellDataDic
                    = new Dictionary<string, EcellData>();
            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData storedEcellData in ecellObject.Value)
                {
                    storedEcellDataDic[storedEcellData.Name] = storedEcellData;
                    variableEcellDataList.Add(storedEcellData);

                }
            }
            foreach (string name in wrappedPolymorph)
            {
                string entityPath = fullID + Constants.delimiterColon + name;
                PropertyAttributes flag = simulator.GetEntityPropertyAttributes(entityPath);
                if (!flag.Gettable)
                {
                    continue;
                }
                EcellValue value = GetVariableValue(simulator, name, entityPath);
                EcellData ecellData = CreateEcellData(name, value, entityPath, flag);
                if (ecellData.Value != null)
                {
                    ecellData.Logable = ecellData.Value.IsDouble;
                }
                if (storedEcellDataDic.ContainsKey(name))
                {
                    ecellData.Logged = storedEcellDataDic[name].Logged;
                    variableEcellDataList.Remove(storedEcellDataDic[name]);
                }
                variableEcellDataList.Add(ecellData);
            }
            ecellObject.SetEcellDatas(variableEcellDataList);
        }

        /// <summary>
        /// GetVariableValue
        /// </summary>
        /// <param name="simulator">the current simulator.</param>
        /// <param name="name">the variable name</param>
        /// <param name="entityPath">the entity name.</param>
        /// <returns></returns>
        private static EcellValue GetVariableValue(WrappedSimulator simulator, string name, string entityPath)
        {
            EcellValue value = null;
            try
            {
                object property = simulator.GetEntityProperty(entityPath);
                value = new EcellValue(property);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                if (name.Equals(Constants.xpathFixed))
                {
                    value = new EcellValue(0);
                }
                else
                {
                    value = new EcellValue(0.0);
                }
            }
            return value;
        }

        /// <summary>
        /// Create new EcellData.
        /// </summary>
        /// <param name="name">the entity name.</param>
        /// <param name="value">the entity value.</param>
        /// <param name="entityPath">the entity path.</param>
        /// <param name="flags">property attribute object.</param>
        /// <returns></returns>
        private static EcellData CreateEcellData(string name, EcellValue value, string entityPath, PropertyAttributes flags)
        {
            EcellData data = new EcellData(name, value, entityPath);
            data.Settable = flags.Settable;
            data.Gettable = flags.Gettable;
            data.Loadable = flags.Loadable;
            data.Saveable = flags.Savable;
            return data;
        }

        #endregion

    }
}
