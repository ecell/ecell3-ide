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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// A manager for ComponentSettings.
    /// </summary>
    public class ComponentManager
    {
        #region Constants
        /// <summary>
        /// The default name of system.
        /// </summary>
        public const string DEFAULT_SYSTEM_NAME = "DefaultSystem";
        /// <summary>
        /// The default name of process.
        /// </summary>
        public const string DEFAULT_PROCESS_NAME = "DefaultProcess";
        /// <summary>
        /// The default name of variable.
        /// </summary>
        public const string DEFAULT_VARIABLE_NAME = "DefaultVariable";
        #endregion

        #region Fields
        /// <summary>
        /// Dictionary of ComponentSettings for creating PEcellSystems.
        /// </summary>
        protected Dictionary<string, ComponentSetting> m_systemSettings;
        
        /// <summary>
        /// Dictionary of ComponentSetting for creating PEcellProcess.
        /// </summary>
        protected Dictionary<string, ComponentSetting> m_processSettings;

        /// <summary>
        /// Dictionary of ComponentSetting for creating PEcellVariable.
        /// </summary>
        protected Dictionary<string, ComponentSetting> m_variableSettings;

        /// <summary>
        /// The name of default ComponentSetting for System.
        /// </summary>
        protected string m_defaultSystemName;

        /// <summary>
        /// The name of default ComponentSetting for Process.
        /// </summary>
        protected string m_defaultProcessName;

        /// <summary>
        /// The name of default ComponentSetting for Variable.
        /// </summary>
        protected string m_defaultVariableName;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        protected static ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_systemSettings.
        /// </summary>
        public Dictionary<string, ComponentSetting> SystemSettings
        {
            get { return this.m_systemSettings; }
        }

        /// <summary>
        /// Accessor for m_processSetting.
        /// </summary>
        public Dictionary<string, ComponentSetting> ProcessSettings
        {
            get { return this.m_processSettings; }
        }

        /// <summary>
        /// Accessor for m_variableSettings.
        /// </summary>
        public Dictionary<string, ComponentSetting> VariableSettings
        {
            get { return this.m_variableSettings; }
        }

        /// <summary>
        /// Accessor for default ComponentSetting for System.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting DefaultSystemSetting
        {
            get
            {
                if (m_defaultSystemName != null && m_systemSettings.ContainsKey(m_defaultSystemName))
                    return m_systemSettings[m_defaultSystemName];
                else if (m_systemSettings.Count != 0)
                {
                    ComponentSetting def = null;
                    foreach (ComponentSetting cs in m_systemSettings.Values)
                    {
                        def = cs;
                        break;
                    }
                    return def;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Accessor for default ComponentSetting for Process.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting DefaultProcessSetting
        {
            get
            {
                if (m_defaultProcessName != null && m_processSettings.ContainsKey(m_defaultProcessName))
                    return m_processSettings[m_defaultProcessName];
                else if (m_processSettings.Count != 0)
                {
                    ComponentSetting def = null;
                    foreach (ComponentSetting cs in m_processSettings.Values)
                    {
                        def = cs;
                        break;
                    }
                    return def;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Accessor for default ComponentSetting for Variable.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting DefaultVariableSetting
        {
            get
            {
                if (m_defaultVariableName != null && m_variableSettings.ContainsKey(m_defaultVariableName))
                    return m_variableSettings[m_defaultVariableName];
                else if (m_variableSettings.Count != 0)
                {
                    ComponentSetting def = null;
                    foreach (ComponentSetting cs in m_variableSettings.Values)
                    {
                        def = cs;
                        break;
                    }
                    return def;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Accessor for ComponentSettings.
        /// </summary>
        public List<ComponentSetting> ComponentSettings
        {
            get
            {
                List<ComponentSetting> list = new List<ComponentSetting>();
                list.Add(DefaultSystemSetting);
                list.Add(DefaultVariableSetting);
                list.Add(DefaultProcessSetting);
                return list;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// A constructor.
        /// </summary>
        public ComponentManager()
        {
            m_systemSettings = new Dictionary<string, ComponentSetting>();
            m_processSettings = new Dictionary<string, ComponentSetting>();
            m_variableSettings = new Dictionary<string, ComponentSetting>();
            SetDefaultSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load ComponentSettings from default setting file.
        /// </summary>
        public static ComponentManager LoadComponentSettings()
        {
            // Read component settings from ComopnentSettings.xml
            string settingFile = FindComponentSettingsFile();

            ComponentManager manager = new ComponentManager();

            XmlDocument xmlD = new XmlDocument();
            try
            {
                xmlD.Load(settingFile);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(m_resources.GetString("ErrNotComXml"), "WARNING", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                xmlD = null;
            }

            if (xmlD != null)
            {
                // All ComponentSettings in the xml file are in valid format or not.
                bool isAllValid = true;
                // If any ComponentSettings in the xml file is invalid, these messages are shown.
                List<string> lackMessages = new List<string>();
                int csCount = 1;

                // Read ComponentSettings information from xml file.
                foreach (XmlNode componentNode in xmlD.ChildNodes[0].ChildNodes)
                {
                    ComponentSetting cs = new ComponentSetting();

                    string componentKind = componentNode.Attributes["kind"].Value;

                    bool isDefault = false;
                    if (componentNode.Attributes["isDefault"] != null
                     && componentNode.Attributes["isDefault"].ToString().ToLower().Equals("true"))
                        isDefault = true;

                    try
                    {
                        cs.ComponentType = ComponentManager.ParseComponentKind(componentKind);
                    }
                    catch (NoSuchComponentKindException e)
                    {
                        MessageBox.Show(m_resources.GetString("ErrCreateKind") + "\n\n" + e.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    foreach (XmlNode parameterNode in componentNode.ChildNodes)
                    {
                        if ("Name".Equals(parameterNode.Name))
                        {
                            cs.Name = parameterNode.InnerText;
                        }
                        else if ("Color".Equals(parameterNode.Name))
                        {
                            Brush brush = PathUtil.GetBrushFromString(parameterNode.InnerText);
                            if (brush != null)
                            {
                                cs.NormalBrush = brush;
                            }
                        }
                        else if ("Drawings".Equals(parameterNode.Name))
                        {
                            foreach (XmlNode drawNode in parameterNode.ChildNodes)
                            {
                                if (drawNode.Attributes["type"] != null)
                                    cs.AddFigure(drawNode.Attributes["type"].Value, drawNode.InnerText);
                            }
                        }
                        else if ("Class".Equals(parameterNode.Name))
                        {
                            cs.AddComponentClass(parameterNode.InnerText);
                        }
                    }

                    List<string> lackInfos = cs.Validate();

                    if (lackInfos == null)
                    {
                        switch (cs.ComponentType)
                        {
                            case ComponentType.System:
                                manager.RegisterSystemSetting(cs.Name, cs, isDefault);
                                break;
                            case ComponentType.Process:
                                manager.RegisterProcessSetting(cs.Name, cs, isDefault);
                                break;
                            case ComponentType.Variable:
                                manager.RegisterVariableSetting(cs.Name, cs, isDefault);
                                break;
                        }
                    }
                    else
                    {
                        isAllValid = false;
                        string nameForCs = null;
                        if (cs.Name == null)
                            nameForCs = "ComponentSetting No." + csCount;
                        else
                            nameForCs = cs.Name;
                        foreach (string lackInfo in lackInfos)
                            lackMessages.Add(nameForCs + " lacks " + lackInfo + "\n");
                    }
                    csCount++;
                }

                string warnMessage = "";

                if (manager.DefaultSystemSetting == null)
                    warnMessage += m_resources.GetString("ErrCompSystem") + "\n\n";
                if (manager.DefaultVariableSetting == null)
                    warnMessage += m_resources.GetString("ErrCompVariable") + "\n\n";
                if (manager.DefaultProcessSetting == null)
                    warnMessage += m_resources.GetString("ErrCompProcess") + "\n\n";

                if (!isAllValid)
                {
                    warnMessage += m_resources.GetString("ErrCompInvalid");
                    if (lackMessages.Count != 1)
                        warnMessage += "s";
                    warnMessage += "\n";

                    foreach (string msg in lackMessages)
                    {
                        warnMessage += "    " + msg;
                    }
                }

                if (warnMessage != null && warnMessage.Length != 0)
                {
                    MessageBox.Show(warnMessage, "WARNING by PathwayWindow", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                }
            }
            return manager;
        }

        /// <summary>
        /// Parse a name of kind to ComponentType
        /// </summary>
        /// <param name="kind">a name of kind, to be parsed</param>
        /// <returns></returns>
        public static ComponentType ParseComponentKind(string type)
        {
            if (type == null || type.Equals(""))
            {
                throw new NoSuchComponentKindException("Component kind \"" + type + "\" doesn't" +
                    " exist. One of System or Variable or Process must be set as a component kind.");
            }
            type = type.ToLower();
            if (type.Equals("system"))
            {
                return ComponentType.System;
            }
            else if (type.Equals("variable"))
            {
                return ComponentType.Variable;
            }
            else if (type.Equals("process"))
            {
                return ComponentType.Process;
            }
            else
            {
                throw new NoSuchComponentKindException("Component kind \"" + type + "\" doesn't" +
                    " exist. One of System or Variable or Process must be set as a component kind.");
            }
        }
        /// <summary>
        /// Get a string of type name.
        /// </summary>
        /// <param name="cType"></param>
        /// <returns></returns>
        public static string GetTypeString(ComponentType cType)
        {
            if (cType == ComponentType.System)
            {
                return EcellObject.SYSTEM;
            }
            else if (cType == ComponentType.Variable)
            {
                return EcellObject.VARIABLE;
            }
            else if (cType == ComponentType.Process)
            {
                return EcellObject.PROCESS;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Register ComponentSetting of a system onto this manager
        /// </summary>
        /// <param name="settingName">the name of ComponentSetting </param>
        /// <param name="setting">ComponentSetting to be registered</param>
        /// <param name="isDefault">If true, this setting will be set as default</param>
        public void RegisterSystemSetting(string settingName, ComponentSetting setting, bool isDefault)
        {
            if (settingName == null)
                return;
            if (m_systemSettings.ContainsKey(settingName))
                m_systemSettings.Remove(settingName);
            m_systemSettings.Add(settingName, setting);
            if (isDefault)
                m_defaultSystemName = settingName;
        }

        /// <summary>
        /// Register ComponentSetting of a process onto this manager
        /// </summary>
        /// <param name="settingName">the name of ComponentSetting</param>
        /// <param name="setting">ComponentSetting to be registered</param>
        /// <param name="isDefault">If true, this setting will be set as default</param>
        public void RegisterProcessSetting(string settingName, ComponentSetting setting, bool isDefault)
        {
            if (settingName == null)
                return;
            if (m_processSettings.ContainsKey(settingName))
                m_processSettings.Remove(settingName);
            m_processSettings.Add(settingName, setting);
            if (isDefault)
                m_defaultProcessName = settingName;
        }

        /// <summary>
        /// Register ComponentSetting of a variable onto this manager
        /// </summary>
        /// <param name="settingName">the name of ComponentSetting</param>
        /// <param name="setting">ComponentSetting to be registered</param>
        /// <param name="isDefault">If true, this setting will be set as default</param>
        public void RegisterVariableSetting(string settingName, ComponentSetting setting, bool isDefault)
        {
            if (settingName == null)
                return;
            if (m_variableSettings.ContainsKey(settingName))
                m_variableSettings.Remove(settingName);
            m_variableSettings.Add(settingName, setting);
            if (isDefault)
                m_defaultVariableName = settingName;
        }

        /// <summary>
        /// Remove ComponentSetting of a system from this manager.
        /// </summary>
        /// <param name="settingName">the name of ComponentSetting</param>
        /// <returns>True if this name has existed in the dictionary, otherwise false.</returns>
        public bool RemoveSystemSetting(string settingName)
        {
            if (settingName != null && m_systemSettings.ContainsKey(settingName))
            {
                m_systemSettings.Remove(settingName);
                if (m_defaultSystemName != null && m_defaultSystemName.Equals(settingName))
                    m_defaultSystemName = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove ComponentSetting of a process from this manager.
        /// </summary>
        /// <param name="settingName">the name of ComponentSetting</param>
        /// <returns>True if this name has existed in the dictionary, otherwise false.</returns>
        public bool RemoveProcessSetting(string settingName)
        {
            if (settingName != null && m_processSettings.ContainsKey(settingName))
            {
                m_processSettings.Remove(settingName);
                if (m_defaultProcessName != null && m_defaultProcessName.Equals(settingName))
                    m_defaultProcessName = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove ComponentSetting of a variable from this manager.
        /// </summary>
        /// <param name="settingName">the name of ComponentSetting</param>
        /// <returns>True if this name has existed in the dictionary, otherwise false.</returns>
        public bool RemoveVariableSetting(string settingName)
        {
            if (settingName != null && m_variableSettings.ContainsKey(settingName))
            {
                m_variableSettings.Remove(settingName);
                if (m_defaultVariableName != null && m_defaultVariableName.Equals(settingName))
                    m_defaultVariableName = null;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Private Methods
        private void SetDefaultSettings()
        {
            // Set hard coded default system ComponentSettings
            ComponentSetting defSysCs = new ComponentSetting();
            defSysCs.ComponentType = ComponentType.System;
            defSysCs.Name = DEFAULT_SYSTEM_NAME;
            defSysCs.NormalBrush = Brushes.Black;
            defSysCs.AddComponentClass("PEcellSystem");
            RegisterSystemSetting(defSysCs.Name, defSysCs, true);

            // Set hard coded default variable ComponentSettings
            ComponentSetting defVarCs = new ComponentSetting();
            defVarCs.ComponentType = ComponentType.Variable;
            defVarCs.Name = DEFAULT_VARIABLE_NAME;
            defVarCs.NormalBrush = Brushes.LightBlue;
            defVarCs.AddFigure("Ellipse", "-30,-20,60,40");
            defVarCs.AddComponentClass("PEcellVariable");
            RegisterProcessSetting(defVarCs.Name, defVarCs, true);

            // Set hard coded default process ComponentSettings
            ComponentSetting defProCs = new ComponentSetting();
            defProCs.ComponentType = ComponentType.Process;
            defProCs.Name = DEFAULT_PROCESS_NAME;
            defProCs.NormalBrush = Brushes.LightGreen;
            defProCs.AddFigure("Rectangle","-30,-20,60,40");
            defProCs.AddComponentClass("PEcellProcess");
            RegisterProcessSetting(defProCs.Name, defProCs, true);
        }

        /// <summary>
        /// Find component settings file
        /// </summary>
        private static string FindComponentSettingsFile()
        {
            string[] pluginDirs = EcellLib.Util.GetPluginDirs();
            foreach (string pluginDir in pluginDirs)
            {
                string settingFile = pluginDir + "\\pathway\\ComponentSettings.xml";
                if (File.Exists(settingFile))
                    return settingFile;
            }
            return null;
        }
        #endregion
    }
}
