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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;	// 名前空間の宣言
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using EcellLib;

namespace EcellLib.StaticDebugWindow
{
    /// <summary>
    /// Controls the static debug.
    /// </summary>
    public class StaticDebugWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// The "DataMenager"
        /// </summary>
        DataManager m_dManager;
        /// <summary>
        /// The dictionary of the multi-validation
        /// </summary>
        Dictionary<string, Dictionary<string, Dictionary<Type, MultiValidateMethod>>> m_multiValidateModelDic;
        /// <summary>
        /// The dictionary of the uni-model-validation
        /// </summary>
        Dictionary<string, Dictionary<string, Dictionary<Type, UniValidateMethod>>> m_uniValidateModelDic;
        /// <summary>
        /// The dictionary of the uni-network-validation
        /// </summary>
        Dictionary<string, Dictionary<string, Dictionary<Type, UniValidateMethod>>> m_uniValidateNetworkDic;
        /// <summary>
        /// The list of the error message
        /// </summary>
        List<ErrorMessage> m_errorMessageList;
        /// <summary>
        /// MenuItem of [Debug]->[Static Debug].
        /// </summary>
        ToolStripMenuItem m_staticDebug;
        /// <summary>
        /// The dictionary of StaticDebugPlugin.
        /// Word is the name of static debug. Data is the plugin of static debug.
        /// </summary>
        // Dictionary<string, StaticDebugPlugin> m_pluginDic;
        Dictionary<string, Type> m_pluginDic;
        /// <summary>
        /// The list of the used "Variable"
        /// </summary>
        List<string> m_usedVariableList;
        /// <summary>
        /// The list of the existed "Variable"
        /// </summary>
        List<string> m_existVariableList;
        #endregion

        #region Delegate
        /// <summary>
        /// Delegates the multi-validarion.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="ecellDataList"></param>
        delegate void MultiValidateMethod(string modelID, string type, List<EcellData> ecellDataList);

        /// <summary>
        /// Delegates the uni-validation.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="ecellData"></param>
        delegate void UniValidateMethod(string modelID, string type, EcellData ecellData);
        #endregion

        #region Property
        /// <summary>
        /// get/set the list of the "ErrorMessage"
        /// </summary>
        public List<ErrorMessage> ErrorMessageList
        {
            get { return this.m_errorMessageList; }
        }
        #endregion

        #region PluginBase
        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">the current simulation time</param>
        public void AdvancedTime(double time)
        {
            // nothing
        }

        /// <summary>
        ///  When the system status is changed, the menu is changed to enable/disable.
        /// </summary>
        /// <param name="type">the status type</param>
        public void ChangeStatus(int type)
        {
            if (type == Util.LOADED) m_staticDebug.Enabled = true;
            else m_staticDebug.Enabled = false;
        }

        /*
        /// <summary>
        /// The event sequence on closing this project.
        /// </summary>
        public void Clear()
        {
            // nothing
        }
         */

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The list of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID before values are changed</param>
        /// <param name="key">the ID before values are changed</param>
        /// <param name="type">the data type before values are changed</param>
        /// <param name="data">the data after values are changed</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID of the deleted object</param>
        /// <param name="key">the ID of the deleted object</param>
        /// <param name="type">the data type of the deleted object</param>
        public void DataDelete(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// Returns items of the menu strip used on the main menu.
        /// </summary>
        /// <returns>items of the menu strip</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_staticDebug = new ToolStripMenuItem();
            m_staticDebug.Name = "MenuItemStaticDebug";
            m_staticDebug.Size = new Size(96, 22);
            m_staticDebug.Text = "Static Debug";
            m_staticDebug.Tag = 10;
            m_staticDebug.Enabled = false;
            m_staticDebug.Click += new EventHandler(this.ShowStaticDebugSetupWindow);

            ToolStripMenuItem debug = new ToolStripMenuItem();
            debug.DropDownItems.AddRange(new ToolStripItem[] {
                m_staticDebug
            });
            debug.Name = "MenuItemDebug";
            debug.Size = new Size(36, 20);
            debug.Text = "Debug";
            tmp.Add(debug);

            return tmp;
        }

        /// <summary>
        /// Returns the name of this plugin.
        /// </summary>
        /// <returns>"StaticDebugWindow"(Fixed)</returns>        
        public string GetPluginName()
        {
            return "StaticDebugWindow";
        }

        /// <summary>
        /// Returns items of the menu strip used on the toolbar.
        /// </summary>
        /// <returns>items of the menu strip</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Returns window forms used on the main window.
        /// </summary>
        /// <returns>window forms</returns>
        public List<UserControl> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// Checks whether this plugin can print the display image.
        /// </summary>
        /// <returns>false(Fixed)</returns>
        public bool IsEnablePrint()
        {
            return false;
        }

        /// <summary>
        /// Checks whether this plugin is the "MessageWindow".
        /// </summary>
        /// <returns>false(Fixed)</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">the model ID of the object to which values are changed</param>
        /// <param name="key">the ID of the object to which values are changed</param>
        /// <param name="type">the data type of the object to which values are changed</param>
        /// <param name="propName">the property name of the object to which values are changed</param>
        /// <param name="data">changed values of the object</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="key">the IDt</param>
        /// <param name="type">the data type</param>
        /// <param name="path">the path of the entity</param>
        public void LoggerAdd(string modelID, string key, string type, string path)
        {
            // nothing
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">the log type</param>
        /// <param name="message">the message</param>
        public void Message(string type, string message)
        {
            // nothing
        }

        /// <summary>
        /// Returns the bitmap image that converts the display image on this plugin.
        /// </summary>
        /// <returns>the bitmap image</returns>
        public Bitmap Print()
        {
            return null;
        }

        /// <summary>
        /// Saves the model to the selected directory.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="directory">the selected directory</param>
        public void SaveModel(string modelID, string directory)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">the selected model ID</param>
        /// <param name="key">the selected ID</param>
        /// <param name="type">the selected data type</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// Sets the panel that shows this plugin in the MainWindow.
        /// </summary>
        /// <param name="panel">the set panel</param>
        public void SetPanel(Panel panel)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">the model ID generating warning data</param>
        /// <param name="key">the ID generating warning data</param>
        /// <param name="type">the data type generating warning data</param>
        /// <param name="warntype">the type of warning data</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            // nothing
        }
        #endregion

        /// <summary>
        /// Clears the stored list of the "ErrorMessage".
        /// </summary>
        public void Clear()
        {
            this.m_errorMessageList.Clear();
            this.m_usedVariableList.Clear();
        }

        /// <summary>
        /// Initializes validated patterns.
        /// </summary>
        void Initialize()
        {
            //
            // 4 System
            //
            this.m_uniValidateModelDic[Util.s_xpathSystem]
                = new Dictionary<string, Dictionary<Type, UniValidateMethod>>();
            //
            this.m_uniValidateModelDic[Util.s_xpathSystem][Util.s_xpathID] = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathSystem][Util.s_xpathID][typeof(string)]
                = new UniValidateMethod(this.ValidateInvalidCharacters);
            //
            this.m_uniValidateNetworkDic[Util.s_xpathSystem]
                = new Dictionary<string, Dictionary<Type, UniValidateMethod>>();
            //
            this.m_uniValidateNetworkDic[Util.s_xpathSystem][Util.s_xpathStepper + Util.s_xpathID]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateNetworkDic[Util.s_xpathSystem][Util.s_xpathStepper + Util.s_xpathID][typeof(string)]
                = new UniValidateMethod(this.ValidateExistStepperID);
            //
            // 4 Variable
            //
            this.m_uniValidateModelDic[Util.s_xpathVariable]
                = new Dictionary<string, Dictionary<Type, UniValidateMethod>>();
            //
            this.m_uniValidateModelDic[Util.s_xpathVariable][Util.s_xpathID]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathVariable][Util.s_xpathID][typeof(string)]
                = new UniValidateMethod(this.ValidateInvalidCharacters);
            //
            this.m_uniValidateModelDic[Util.s_xpathVariable][Util.s_xpathFixed]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathVariable][Util.s_xpathFixed][typeof(int)]
                = new UniValidateMethod(this.ValidateIsBool);
            //
            this.m_uniValidateModelDic[Util.s_xpathVariable][Util.s_xpathMolarConc]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathVariable][Util.s_xpathMolarConc][typeof(double)]
                = new UniValidateMethod(this.ValidateIsPositiveNumberWithZero);
            //
            this.m_uniValidateModelDic[Util.s_xpathVariable][Util.s_xpathNumberConc]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathVariable][Util.s_xpathNumberConc][typeof(double)]
                = new UniValidateMethod(this.ValidateIsPositiveNumberWithZero);
            //
            /*
            this.m_uniValidateNetworkDic[Util.s_xpathVariable][Util.s_xpathStepper + Util.s_xpathID]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateNetworkDic[Util.s_xpathVariable][Util.s_xpathStepper + Util.s_xpathID][typeof(string)]
                = new UniValidateMethod(this.ValidateExistStepperID);
             */
            //
            // 4 Process
            //
            this.m_uniValidateModelDic[Util.s_xpathProcess]
                = new Dictionary<string, Dictionary<Type, UniValidateMethod>>();
            //
            this.m_uniValidateModelDic[Util.s_xpathProcess][Util.s_xpathID] = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathProcess][Util.s_xpathID][typeof(string)]
                = new UniValidateMethod(this.ValidateInvalidCharacters);
            //
            this.m_uniValidateModelDic[Util.s_xpathProcess][Util.s_xpathExpression]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathProcess][Util.s_xpathExpression][typeof(string)]
                = new UniValidateMethod(this.ValidateParenthesisInExpression);
            //
            this.m_uniValidateModelDic[Util.s_xpathProcess][Util.s_xpathFireMethod]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathProcess][Util.s_xpathFireMethod][typeof(string)]
                = new UniValidateMethod(this.ValidateParenthesisInExpression);
            //
            this.m_multiValidateModelDic[Util.s_xpathProcess]
                = new Dictionary<string, Dictionary<Type, MultiValidateMethod>>();
            //
            this.m_multiValidateModelDic[Util.s_xpathProcess][Util.s_xpathExpression]
                = new Dictionary<Type, MultiValidateMethod>();
            this.m_multiValidateModelDic[Util.s_xpathProcess][Util.s_xpathExpression][typeof(string)]
                = new MultiValidateMethod(this.ValidateExistVariableInExpression);
            //
            this.m_multiValidateModelDic[Util.s_xpathProcess][Util.s_xpathFireMethod]
                = new Dictionary<Type, MultiValidateMethod>();
            this.m_multiValidateModelDic[Util.s_xpathProcess][Util.s_xpathFireMethod][typeof(string)]
                = new MultiValidateMethod(this.ValidateExistVariableInExpression);
            //
            this.m_uniValidateNetworkDic[Util.s_xpathProcess]
                = new Dictionary<string, Dictionary<Type, UniValidateMethod>>();
            //
            this.m_uniValidateNetworkDic[Util.s_xpathProcess][Util.s_xpathStepper + Util.s_xpathID]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateNetworkDic[Util.s_xpathProcess][Util.s_xpathStepper + Util.s_xpathID][typeof(string)]
                = new UniValidateMethod(this.ValidateExistStepperID);
            //
            this.m_uniValidateNetworkDic[Util.s_xpathProcess][Util.s_xpathVRL]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateNetworkDic[Util.s_xpathProcess][Util.s_xpathVRL][typeof(List<EcellValue>)]
                = new UniValidateMethod(this.ValidateVariableList);
            //
            // 4 Stepper
            //
            this.m_uniValidateModelDic[Util.s_xpathStepper] 
                = new Dictionary<string, Dictionary<Type, UniValidateMethod>>();
            //
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_headerMaximum + Util.s_xpathStepInterval]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_headerMaximum + Util.s_xpathStepInterval]
                [typeof(double)] = new UniValidateMethod(this.ValidateIsPositiveNumber);
            //
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_headerMinimum + Util.s_xpathStepInterval]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_headerMinimum + Util.s_xpathStepInterval]
                [typeof(double)] = new UniValidateMethod(this.ValidateIsPositiveNumberWithZero);
            //
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_xpathStepInterval]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_xpathStepInterval]
                [typeof(double)] = new UniValidateMethod(this.ValidateIsPositiveNumber);
            //
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_headerTolerable + Util.s_xpathStepInterval]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_headerTolerable + Util.s_xpathStepInterval]
                [typeof(double)] = new UniValidateMethod(this.ValidateIsPositiveNumber);
            //
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_xpathIsEpsilonChecked]
                = new Dictionary<Type, UniValidateMethod>();
            this.m_uniValidateModelDic[Util.s_xpathStepper][Util.s_xpathIsEpsilonChecked][typeof(int)]
                = new UniValidateMethod(this.ValidateIsBool);
            //
            this.m_multiValidateModelDic[Util.s_xpathStepper]
                = new Dictionary<string, Dictionary<Type, MultiValidateMethod>>();
            //
            this.m_multiValidateModelDic[Util.s_xpathStepper][Util.s_headerMaximum + Util.s_xpathStepInterval]
                = new Dictionary<Type, MultiValidateMethod>();
            this.m_multiValidateModelDic[Util.s_xpathStepper][Util.s_headerMaximum + Util.s_xpathStepInterval]
                [typeof(double)] = new MultiValidateMethod(this.ValidateCompareStepInterval);
            //
            // 4 Network
            //
            this.m_usedVariableList = new List<string>();
            this.m_existVariableList = new List<string>();
            //
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser;
                Microsoft.Win32.RegistryKey subkey = key.OpenSubKey("software\\KeioUniv\\E-Cell_IDE");
                string m_pluginDir = (string)subkey.GetValue("ECellIDE_StaticDebug_Plugin");
                if (m_pluginDir != null)
                {
                    if (Directory.Exists(m_pluginDir))
                    {
                        foreach (string fileName in Directory.GetFiles(m_pluginDir, "*.dll"))
                        {
                            // StaticDebugPlugin p = LoadPlugin(fileName);
                            // if (p != null)
                            //     m_pluginDic.Add(p.GetDebugName(), p);
                            Type aType = LoadPlugin(fileName);
                            if (aType != null)
                            {
                                object p
                                    = aType.InvokeMember(
                                        null,
                                        BindingFlags.CreateInstance,
                                        null,
                                        null,
                                        null);
                                string debugName
                                    = (string)aType.InvokeMember(
                                        "GetDebugName",
                                        BindingFlags.InvokeMethod,
                                        null,
                                        p,
                                        null);
                                m_pluginDic.Add(debugName, aType);
                            }
                        }
                    }
                }
                subkey.Close();
                key.Close();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return;
            }
        }

        /// <summary>
        /// Loads the plugin of the static debug.
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The "StaticDebugObject"</returns>
        private Type LoadPlugin(String path)
        {
            try
            {
                string pName = Path.GetFileNameWithoutExtension(path);
                string className = "EcellLib." + pName + "." + pName;
                Assembly m_theHandle = Assembly.LoadFile(path);
                Type aType = m_theHandle.GetType(className);
                /*
                anAllocator = aType.InvokeMember(
                    null,
                    BindingFlags.CreateInstance,
                    null,
                    null,
                    null
                );

                // StaticDebugPlugin p = (StaticDebugPlugin)anAllocator;

                // return p;
                 */
                return aType;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// Creates the new "StaticDebugWindow".
        /// </summary>
        public StaticDebugWindow()
        {
            this.m_dManager = DataManager.GetDataManager();
            this.m_uniValidateModelDic
                = new Dictionary<string, Dictionary<string, Dictionary<Type, UniValidateMethod>>>();
            this.m_uniValidateNetworkDic
                = new Dictionary<string, Dictionary<string, Dictionary<Type, UniValidateMethod>>>();
            this.m_multiValidateModelDic
                    = new Dictionary<string, Dictionary<string, Dictionary<Type, MultiValidateMethod>>>();
            this.m_errorMessageList = new List<ErrorMessage>();
            // this.m_pluginDic = new Dictionary<string, StaticDebugPlugin>();
            this.m_pluginDic = new Dictionary<string, Type>();
            this.Initialize();
        }

        /// <summary>
        /// Validates data.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        public void ValidateAll(string modelID)
        {
            this.ValidateModel(modelID);
            this.ValidateNetwork(modelID);
            this.ValidateMassConservation(modelID);
        }

        /// <summary>
        /// Validates whether "StepInterval"s are correct.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellDataList">The list of the validated "EcellData"</param>
        void ValidateCompareStepInterval(string modelID, string type, List<EcellData> ecellDataList)
        {
            if (modelID == null || modelID.Length <= 0 || ecellDataList == null || ecellDataList.Count <= 0)
            {
                return;
            }
            double maxStepInterval = Double.MinValue;
            double minStepInterval = Double.MaxValue;
            string maxEntityPath = null;
            string minEntityPath = null;
            foreach (EcellData ecellData in ecellDataList)
            {
                if (ecellData.M_name.Equals(Util.s_headerMaximum + Util.s_xpathStepInterval))
                {
                    maxStepInterval = ecellData.M_value.CastToDouble();
                    maxEntityPath = ecellData.M_entityPath;
                }
                else if (ecellData.M_name.Equals(Util.s_headerMinimum + Util.s_xpathStepInterval))
                {
                    minStepInterval = ecellData.M_value.CastToDouble();
                    minEntityPath = ecellData.M_entityPath;
                }
            }
            if (maxStepInterval < minStepInterval)
            {
                string message
                        = "The MaxStepInterval[" + maxStepInterval
                        + "] is smaller than MinStepInterval[" + minStepInterval + "].";
                this.m_errorMessageList.Add(
                        new ErrorMessage(modelID, type, maxEntityPath + ", " + minEntityPath, message));
            }
        }

        /// <summary>
        /// Validates whether the stepper ID exists.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateExistStepperID(string modelID, string type, EcellData ecellData)
        {
            if (modelID == null || modelID.Length <= 0 || ecellData == null)
            {
                return;
            }
            bool existedFlag = false;
            foreach (EcellObject stepper in this.m_dManager.GetStepper(null, modelID))
            {
                if (stepper.key.Equals(ecellData.M_value.CastToString()))
                {
                    existedFlag = true;
                    break;
                }
            }
            if (!existedFlag)
            {
                string message= "Can't find the stepper with the key[" + ecellData.M_value.CastToString() + "].";
                this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
        }

        /// <summary>
        /// Validates whether the "VariableReferenceList" has available "Valiable"s.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateExistVariable(string modelID, string type, EcellData ecellData)
        {
            Dictionary<int, List<string>> directionDic = new Dictionary<int, List<string>>();
            try
            {
                if (modelID == null || modelID.Length <= 0 || ecellData == null)
                {
                    return;
                }
                foreach (EcellValue vr in ecellData.M_value.CastToList())
                {
                    List<EcellValue> vrInfoList = vr.CastToList();
                    //
                    // Searchs the "Variable"
                    //
                    string[] variableInfos = vrInfoList[1].CastToString().Split(Util.s_delimiterColon.ToCharArray());
                    string systemPath = variableInfos[1];
                    if (systemPath.Equals(Util.s_delimiterPeriod))
                    {
                        systemPath = ecellData.M_entityPath.Split(Util.s_delimiterColon.ToCharArray())[1];
                    }
                    string variableKey = systemPath + Util.s_delimiterColon + variableInfos[2];
                    bool existFlag = false;
                    foreach (EcellObject ecellObject in this.m_dManager.GetData(modelID, systemPath))
                    {
                        if (ecellObject.type.Equals(Util.s_xpathVariable) && ecellObject.key.Equals(variableKey))
                        {
                            existFlag = true;
                            break;
                        }
                        if (ecellObject.M_instances != null && ecellObject.M_instances.Count > 0)
                        {
                            foreach (EcellObject childEcellObject in ecellObject.M_instances)
                            {
                                if (childEcellObject.type.Equals(Util.s_xpathVariable)
                                        && childEcellObject.key.Equals(variableKey))
                                {
                                    existFlag = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!existFlag)
                    {
                        string message
                            = "The \"VariableReference[" + vr.ToString() + "]\" has no available \"Variable\".";
                        this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
                    }
                    //
                    // Validates the direction.
                    //
                    if (directionDic.ContainsKey(vrInfoList[2].CastToInt()))
                    {
                        if (directionDic[vrInfoList[2].CastToInt()].Contains(vrInfoList[1].CastToString()))
                        {
                            string message
                                = "The \"VariableReference["
                                + vr.ToString() + "]\" has the duplicated \"Coefficient\" and \"Variable\""
                                + "in this \"VariableReferenceList[" + ecellData.M_value.ToString() + "]\".";
                            this.m_errorMessageList.Add(
                                new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
                        }
                        else
                        {
                            directionDic[vrInfoList[2].CastToInt()].Add(vrInfoList[1].CastToString());
                        }
                    }
                    else
                    {
                        directionDic[vrInfoList[2].CastToInt()] = new List<string>();
                        directionDic[vrInfoList[2].CastToInt()].Add(vrInfoList[1].CastToString());
                    }
                }
                //
                // Validates the "0" "Coefficient".
                //
                if (directionDic.ContainsKey(0) && directionDic.Count == 1)
                {
                    string message
                        = "The \"VariableReferenceList["
                        + ecellData.M_value.ToString() + "]\" has only the 0 \"Coefficient\".";
                    this.m_errorMessageList.Add(
                        new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                MessageBox.Show("The load object is out of order. Please reload the model.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // string message
                //     = "The [" + ecellData.M_value.CastToString() + "] is out of order. [" + ex.ToString() + "]";
                // this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
            finally
            {
                directionDic.Clear();
                directionDic = null;
            }
        }

        /// <summary>
        /// Validates whether the expression has proper terms.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateExistVariableInExpression(string modelID, string type, List<EcellData> ecellDataList)
        {
            if (modelID == null || modelID.Length <= 0 || ecellDataList == null || ecellDataList.Count <= 0)
            {
                return;
            }
            EcellData expression = null;
            EcellData variableReferenceList = null;
            List<string> variableList = new List<string>();
            List<string> nameList = new List<string>();
            foreach (EcellData ecellData in ecellDataList)
            {
                if (ecellData.M_name.Equals(Util.s_xpathExpression))
                {
                    expression = ecellData.Copy();
                }
                else if (ecellData.M_name.Equals(Util.s_xpathVRL))
                {
                    variableReferenceList = ecellData.Copy();
                    foreach (EcellValue parent in variableReferenceList.M_value.CastToList())
                    {
                        variableList.Add((parent.CastToList())[0].CastToString());
                    }
                }
                else
                {
                    nameList.Add(ecellData.M_name);
                }
            }
            if (expression == null || variableReferenceList == null)
            {
                return;
            }
            Regex regVariable
                = new Regex("[\\(\\)+\\-\\*/ ]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string message = " in this expression   \"";
            string[] elements = regVariable.Split(expression.M_value.CastToString());
            foreach (string element in elements)
            {
                if (element.Length <= 0)
                {
                    continue;
                }
                else if (element.IndexOf("self") == 0)
                {
                    continue;
                }
                else if (element.IndexOf(Util.s_delimiterPeriod) < 0)
                {
                    if (!nameList.Contains(element))
                    {
                        try
                        {
                            Convert.ToDouble(element);
                        }
                        catch (Exception)
                        {
                            message
                                = "The [" + element + "] in this expression [" + expression.M_value.CastToString() + "]"
                                + " isn't a property of the process.";
                            this.m_errorMessageList.Add(
                                new ErrorMessage(modelID, type, expression.M_entityPath, message));
                        }
                    }
                }
                else
                {
                    string alias = element.Split(Util.s_delimiterPeriod.ToCharArray())[0];
                    if (alias != null && alias.Length > 0 && !variableList.Contains(alias))
                    {
//                        message = "The [" + alias + "] in this expression [" + expression.M_value.CastToString() + "]"
//                            + " isn't a element of this \"VariableReferenceList\" [" 
//                            + variableReferenceList.M_value.ToString() + "].";
                        message = "Can't not find the variable[" + alias + "] in this expression from VariableReferemce+List";  
                        this.m_errorMessageList.Add(new ErrorMessage(modelID, type, expression.M_entityPath, message));
                    }
                }
            }
        }
        
        /// <summary>
        /// Validates whether the value has invalid characters.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateInvalidCharacters(string modelID, string type, EcellData ecellData)
        {
            if (modelID == null || modelID.Length <= 0 || ecellData == null)
            {
                return;
            }
            string message = " has the invalid character \"";
            string value = ecellData.M_value.CastToString();
            if (value.IndexOf(Util.s_delimiterPath) >= 0)
            {
                message = "The \"" + ecellData.M_name + "[" + value + "]\"" + message + Util.s_delimiterPath + "\".";
                this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
            else if (value.IndexOf(Util.s_delimiterColon) >= 0)
            {
                message = "The \"" + ecellData.M_name + "[" + value + "]\"" + message + Util.s_delimiterColon + "\".";
                this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
        }

        /// <summary>
        /// Validates whether the value is the boolean value. 
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateIsBool(string modelID, string type, EcellData ecellData)
        {
            if (modelID == null || modelID.Length <= 0 || ecellData == null)
            {
                return;
            }
            if (ecellData.M_value.CastToInt() != 0 && ecellData.M_value.CastToInt() != 1)
            {
                string message = "The \"" + ecellData.M_name + "[" + ecellData.M_value.CastToInt() 
                    + "]\" cannot be converted into \"Bool\". ";
                this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
        }

        /// <summary>
        /// Validates whether the value is a positive number.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateIsPositiveNumber(string modelID, string type, EcellData ecellData)
        {
            if (modelID == null || modelID.Length <= 0 || ecellData == null)
            {
                return;
            }
            if (ecellData.M_value.CastToDouble() <= 0.0)
            {
                string message = "The \"" + ecellData.M_name + "[" + ecellData.M_value.CastToDouble()
                    + "]\" isn't a positive number. ";
                this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
        }

        /// <summary>
        /// Validates whether the value is a positive number or 0.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "ECellData"</param>
        void ValidateIsPositiveNumberWithZero(string modelID, string type, EcellData ecellData)
        {
            if (modelID == null || modelID.Length <= 0 || ecellData == null)
            {
                return;
            }
            if (ecellData.M_value.CastToDouble() < 0.0)
            {
                string message = "The \"" + ecellData.M_name + "[" + ecellData.M_value.CastToDouble() 
                    + "] isn't 0 or a positive number. ";
                this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
        }

        /// <summary>
        /// Validates the list of the "EcellObject" 4 the network.
        /// </summary>
        /// <param name="ecellObjectList"></param>
        private void ValidateNetwork(List<EcellObject> ecellObjectList)
        {
            if (ecellObjectList == null || ecellObjectList.Count <= 0)
            {
                return;
            }
            foreach (EcellObject ecellObject in ecellObjectList)
            {
                if (ecellObject.type.Equals(Util.s_xpathModel))
                {
                    //
                    // 4 Stepper
                    //
                    this.ValidateNetwork(this.m_dManager.GetStepper(null, ecellObject.modelID));
                }
                if (this.m_uniValidateNetworkDic.ContainsKey(ecellObject.type))
                {
                    //
                    // 4 System, Variable and Process
                    //
                    foreach (EcellData ecellData in ecellObject.M_value)
                    {
                        if (this.m_uniValidateNetworkDic[ecellObject.type].ContainsKey(ecellData.M_name))
                        {
                            if (this.m_uniValidateNetworkDic[ecellObject.type][ecellData.M_name].ContainsKey(
                                ecellData.M_value.M_type))
                            {
                                this.m_uniValidateNetworkDic[ecellObject.type][ecellData.M_name]
                                    [ecellData.M_value.M_type](ecellObject.modelID, ecellObject.type, ecellData);
                            }
                        }
                    }
                }
                /*
                if (this.m_multiValidateNetworkDic.ContainsKey(ecellObject.type))
                {
                    //
                    // 4 System, Variable and Process
                    //
                    foreach (EcellData ecellData in ecellObject.M_value)
                    {
                        if (this.m_multiValidateNetworkDic[ecellObject.type].ContainsKey(ecellData.M_name))
                        {
                            if (this.m_multiValidateNetworkDic[ecellObject.type][ecellData.M_name].ContainsKey(
                                ecellData.M_value.M_type))
                            {
                                this.m_multiValidateNetworkDic[ecellObject.type][ecellData.M_name]
                                    [ecellData.M_value.M_type](
                                        ecellObject.modelID, ecellObject.type, ecellObject.M_value);
                            }
                        }
                    }
                }
                 */
                if (ecellObject.type.Equals(Util.s_xpathVariable))
                {
                    this.m_existVariableList.Add(ecellObject.key);
                }
                if (ecellObject.M_instances != null && ecellObject.M_instances.Count > 0)
                {
                    this.ValidateNetwork(ecellObject.M_instances);
                }
            }
            if (ecellObjectList.Count > 0 && !ecellObjectList[0].type.Equals(Util.s_xpathStepper))
            {
                this.ValidateReferToVariable(ecellObjectList[0].modelID);
            }
        }

        /// <summary>
        /// Validates the network.
        /// </summary>
        /// <param name="modelID"></param>
        public void ValidateNetwork(string modelID)
        {
            try
            {
                this.ValidateNetwork(this.m_dManager.GetData(modelID, null));
            }
            catch (Exception ex)
            {
                throw new Exception("The static debug of the network failed. [" + ex.ToString() + "]");
            }
        }

        /// <summary>
        /// Validates the list of the "EcellObject" 4 the mass conservation.
        /// </summary>
        /// <param name="ecellObjectList"></param>
        private void ValidateMassConservation(List<EcellObject> ecellObjectList)
        {
            // MEN WORKING
        }

        /// <summary>
        /// Validates the mass conservation.
        /// </summary>
        /// <param name="modelID"></param>
        public void ValidateMassConservation(string modelID)
        {
            try
            {
                this.ValidateMassConservation(this.m_dManager.GetData(modelID, null));
            }
            catch (Exception ex)
            {
                throw new Exception("The static debug of the mass conservation failed. [" + ex.ToString() + "]");
            }
        }

        /// <summary>
        /// Validates the list of the "EcellObject" 4 the model consistency.
        /// </summary>
        /// <param name="ecellObjectList">the list of the validated "EcellObject"</param>
        private void ValidateModel(List<EcellObject> ecellObjectList)
        {
            if (ecellObjectList == null || ecellObjectList.Count <= 0)
            {
                return;
            }
            foreach (EcellObject ecellObject in ecellObjectList)
            {
                if (ecellObject.type.Equals(Util.s_xpathModel))
                {
                    //
                    // 4 Stepper
                    //
                    this.ValidateModel(this.m_dManager.GetStepper(null, ecellObject.modelID));
                }
                if (this.m_uniValidateModelDic.ContainsKey(ecellObject.type))
                {
                    //
                    // 4 System, Variable and Process
                    //
                    foreach (EcellData ecellData in ecellObject.M_value)
                    {
                        if (this.m_uniValidateModelDic[ecellObject.type].ContainsKey(ecellData.M_name))
                        {
                            if (this.m_uniValidateModelDic[ecellObject.type][ecellData.M_name].ContainsKey(
                                ecellData.M_value.M_type))
                            {
                                this.m_uniValidateModelDic[ecellObject.type][ecellData.M_name]
                                    [ecellData.M_value.M_type](ecellObject.modelID, ecellObject.type, ecellData);
                            }
                        }
                    }
                }
                if (this.m_multiValidateModelDic.ContainsKey(ecellObject.type))
                {
                    //
                    // 4 System, Variable and Process
                    //
                    foreach (EcellData ecellData in ecellObject.M_value)
                    {
                        if (this.m_multiValidateModelDic[ecellObject.type].ContainsKey(ecellData.M_name))
                        {
                            if (this.m_multiValidateModelDic[ecellObject.type][ecellData.M_name].ContainsKey(
                                ecellData.M_value.M_type))
                            {
                                this.m_multiValidateModelDic[ecellObject.type][ecellData.M_name]
                                    [ecellData.M_value.M_type](
                                        ecellObject.modelID, ecellObject.type, ecellObject.M_value);
                            }
                        }
                    }
                }
                if (ecellObject.M_instances != null && ecellObject.M_instances.Count > 0)
                {
                    this.ValidateModel(ecellObject.M_instances);
                }
            }
        }

        /// <summary>
        /// Validates the model consistency.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        public void ValidateModel(string modelID)
        {
            try
            {
                this.ValidateModel(this.m_dManager.GetData(modelID, null));
            }
            catch (Exception ex)
            {
                throw new Exception("The static debug of the model failed. [" + ex.ToString() + "]");
            }
        }

        /// <summary>
        /// Validates whether the expression has parentheses of the same number.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateParenthesisInExpression(string modelID, string type, EcellData ecellData)
        {
            if (modelID == null || modelID.Length <= 0 || ecellData == null)
            {
                return;
            }
            Regex regParenthesis = new Regex("[\\(\\)]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matchParenthesis = null;
            int leftParenthesisCount = 0;
            int rightParenthesisCount = 0;
            for (matchParenthesis = regParenthesis.Match(ecellData.M_value.CastToString());
                matchParenthesis.Success; matchParenthesis = matchParenthesis.NextMatch())
            {
                if (ecellData.M_value.CastToString()[matchParenthesis.Groups[0].Index] == '(')
                {
                    leftParenthesisCount++;
                }
                else
                {
                    rightParenthesisCount++;
                }
            }
            if (leftParenthesisCount != rightParenthesisCount)
            {
                string message = "The number of left parenthesis isn't equal to the number of right ones"
                    + " in this expression.";
                //                    + " in this expression [" + ecellData.M_value.CastToString() + "].";
                this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
        }

        /// <summary>
        /// Validates whether the "VariableReferenceList" is null.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateReferToProcess(string modelID, string type, EcellData ecellData)
        {
            if (modelID == null || modelID.Length <= 0 || ecellData == null)
            {
                return;
            }
            if (ecellData.M_value.CastToList().Count <= 0)
            {
                string message = "The \"VariableReferenceList\" is \"null\".";
                this.m_errorMessageList.Add(new ErrorMessage(modelID, type, ecellData.M_entityPath, message));
            }
        }

        /// <summary>
        /// Validates whether the "Variable" is refered to some "Process".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        void ValidateReferToVariable(string modelID)
        {
            foreach (string variableKey in this.m_existVariableList)
            {
                if (!this.m_usedVariableList.Contains(Util.s_delimiterColon + variableKey)
                    && !variableKey.EndsWith(Util.s_xpathSize.ToUpper()))
                {
                    string message = "The \"Variable\" is referred to nowhere.";
                    this.m_errorMessageList.Add(new ErrorMessage(modelID, Util.s_xpathVariable, variableKey, message));
                }
            }
            this.m_existVariableList.Clear();
            this.m_usedVariableList.Clear();
        }

        /// <summary>
        /// Validates whether the "Variable" is refered to some "Process".
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="type">The type</param>
        /// <param name="ecellData">The validated "EcellData"</param>
        void ValidateReferToVariable4Process(string modelID, string type, EcellData ecellData)
        {
            if (modelID == null || modelID.Length <= 0 || ecellData == null)
            {
                return;
            }
            foreach (EcellValue vr in ecellData.M_value.CastToList())
            {
                this.m_usedVariableList.Add(vr.CastToList()[1].ToString());
            }
        }

        /// <summary>
        /// Validates the "VariableReferenceList".
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="type"></param>
        /// <param name="ecellData"></param>
        void ValidateVariableList(string modelID, string type, EcellData ecellData)
        {
            this.ValidateExistVariable(modelID, type, ecellData);
            this.ValidateReferToProcess(modelID, type, ecellData);
            this.ValidateReferToVariable4Process(modelID, type, ecellData);
        }

        /// <summary>
        /// The action of selecting the menu [Debug]->[Static Debug].
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        public void ShowStaticDebugSetupWindow(object sender, EventArgs e)
        {
            StaticDebugSetupWindow win = new StaticDebugSetupWindow();
            win.SetPlugin(this);
            List<String> list = new List<string>();

            list.Add("Model");
            list.Add("Network");
            list.Add("MassConservation");

            foreach (string key in m_pluginDic.Keys)
            {
                list.Add(key);
            }

            win.LayoutCheckList(list);
            win.debugButton.Select();

            win.Show();
        }

        /// <summary>
        /// execute the static debug in existing the list.
        /// </summary>
        /// <param name="list">the list of static debug.</param>
        public void Debug(List<string> list)
        {
            m_errorMessageList.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals("Model"))
                {
                    List<string> mList = m_dManager.GetModelList();
                    for (int j = 0; j < mList.Count; j++)
                        ValidateModel(mList[j]);
                }
                else if (list[i].Equals("Network"))
                {
                    List<string> mList = m_dManager.GetModelList();
                    for (int j = 0; j < mList.Count; j++)
                        ValidateNetwork(mList[j]);
                }
                else if (list[i].Equals("MassConservation"))
                {
                    List<string> mList = m_dManager.GetModelList();
                    for (int j = 0; j < mList.Count; j++)
                        ValidateMassConservation(mList[j]);
                }
                else
                {
                    if (m_pluginDic.ContainsKey(list[i]))
                    {
                        /*
                        StaticDebugPlugin p = m_pluginDic[list[i]];
                        List<ErrorMessage> tmplist = p.Debug();
                        for (int k = 0; k < tmplist.Count; k++)
                            m_errorMessageList.Add(tmplist[k]);
                         */
                        Type aType = m_pluginDic[list[i]];
                        Object p
                            = aType.InvokeMember(
                                null,
                                BindingFlags.CreateInstance,
                                null,
                                null,
                                null);
                        Object r
                            = aType.InvokeMember(
                                "Debug",
                                BindingFlags.InvokeMethod,
                                null,
                                p,
                                null);
                        Object enu
                            = r.GetType().InvokeMember(
                                "GetEnumerator",
                                BindingFlags.InvokeMethod,
                                null,
                                r,
                                null);
                        while (true)
                        {
                            bool nextFlag
                                = (bool)enu.GetType().InvokeMember(
                                    "MoveNext",
                                    BindingFlags.InvokeMethod,
                                    null,
                                    enu,
                                    null);
                            if (!nextFlag)
                            {
                                break;
                            }
                            Object errorMessage
                                = enu.GetType().InvokeMember(
                                    "Current",
                                    BindingFlags.GetProperty,
                                    null,
                                    enu,
                                    null);
                            string modelID
                                = (string)errorMessage.GetType().InvokeMember(
                                    "ModelID",
                                    BindingFlags.GetProperty,
                                    null,
                                    errorMessage,
                                    null);
                            string type
                                = (string)errorMessage.GetType().InvokeMember(
                                    "Type",
                                    BindingFlags.GetProperty,
                                    null,
                                    errorMessage,
                                    null);
                            string entityPath
                                = (string)errorMessage.GetType().InvokeMember(
                                    "EntityPath",
                                    BindingFlags.GetProperty,
                                    null,
                                    errorMessage,
                                    null);
                            string message
                                = (string)errorMessage.GetType().InvokeMember(
                                    "Message",
                                    BindingFlags.GetProperty,
                                    null,
                                    errorMessage,
                                    null);
                            m_errorMessageList.Add(new ErrorMessage(modelID, type, entityPath, message));
                        }
                    }
                }

                // execute other static debug!
            }
        }
    }
}

/// <summary>
/// Controls the error message.
/// </summary>
public class ErrorMessage
{
    #region Fields
    /// <summary>
    /// The model ID
    /// </summary>
    string m_modelID = null;
    /// <summary>
    /// The type
    /// </summary>
    string m_type = null;
    /// <summary>
    /// The entity path
    /// </summary>
    string m_entityPath = null;
    /// <summary>
    /// The message
    /// </summary>
    string m_message = null;
    #endregion

    #region Property
    /// <summary>
    /// get/set the model ID 
    /// </summary>
    public string ModelID
    {
        get { return this.m_modelID; }
    }
    /// <summary>
    /// get/set the type
    /// </summary>
    public string Type
    {
        get { return this.m_type; }
    }
    /// <summary>
    /// get/set the entity path
    /// </summary>
    public string EntityPath
    {
        get { return this.m_entityPath; }
    }
    /// <summary>
    /// get/set the message
    /// </summary>
    public string Message
    {
        get { return this.m_message; }
    }
    #endregion

    /// <summary>
    /// Creates the new "ErrorMessage".
    /// </summary>
    private ErrorMessage()
    {
    }

    /// <summary>
    /// Creates the new "ErrorMessage" with some parameters.
    /// </summary>
    /// <param name="modelID">the model ID</param>
    /// <param name="entityPath">the entity path</param>
    /// <param name="message">the error message</param>
    public ErrorMessage(string modelID, string type, string entityPath, string message)
    {
        this.m_modelID = modelID;
        this.m_type = type;
        this.m_entityPath = entityPath;
        this.m_message = message;
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
