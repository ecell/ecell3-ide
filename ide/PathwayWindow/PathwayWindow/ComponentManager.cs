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

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// A manager for ComponentSettings.
    /// </summary>
    public class ComponentManager
    {
        #region Constractor
        public const string DEFAULT_SYSTEM_NAME = "DefaultSystem";
        public const string DEFAULT_PROCESS_NAME = "DefaultProcess";
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
        /// Parse a name of kind to ComponentType
        /// </summary>
        /// <param name="kind">a name of kind, to be parsed</param>
        /// <returns></returns>
        public static ComponentType ParseComponentKind(String kind)
        {
            if (kind == null || kind.Equals(""))
            {
                throw new NoSuchComponentKindException("Component kind \"" + kind + "\" doesn't" +
                    " exist. One of System or Variable or Process must be set as a component kind.");
            }
            kind = kind.ToLower();
            if (kind.Equals("system"))
            {
                return ComponentType.System;
            }
            else if (kind.Equals("variable"))
            {
                return ComponentType.Variable;
            }
            else if (kind.Equals("process"))
            {
                return ComponentType.Process;
            }
            else
            {
                throw new NoSuchComponentKindException("Component kind \"" + kind + "\" doesn't" +
                    " exist. One of System or Variable or Process must be set as a component kind.");
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
        #endregion
    }
}
