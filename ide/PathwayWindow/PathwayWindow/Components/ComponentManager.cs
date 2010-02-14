//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
// 
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Ecell.IDE.Plugins.PathwayWindow.Exceptions;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using Ecell.IDE.Plugins.PathwayWindow.Graphics;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow.Components
{
    /// <summary>
    /// A manager for ComponentSettings.
    /// </summary>
    public class ComponentManager
    {
        #region Fields
        /// <summary>
        /// Dictionary of ComponentSettings for creating PPathwaySystem.
        /// </summary>
        protected Dictionary<string, ComponentSetting> m_systemSettings;
        
        /// <summary>
        /// Dictionary of ComponentSetting for creating PPathwayProcess.
        /// </summary>
        protected Dictionary<string, ComponentSetting> m_processSettings;

        /// <summary>
        /// Dictionary of ComponentSetting for creating PPathwayVariable.
        /// </summary>
        protected Dictionary<string, ComponentSetting> m_variableSettings;

        /// <summary>
        /// Dictionary of ComponentSettings for creating PPathwayText.
        /// </summary>
        protected Dictionary<string, ComponentSetting> m_textSettings;

        /// <summary>
        /// Dictionary of ComponentSettings for creating PPathwayStepper.
        /// </summary>
        protected Dictionary<string, ComponentSetting> m_stepperSettings;

        /// <summary>
        /// The name of default ComponentSetting for System.
        /// </summary>
        protected string m_defaultSystemName = "";

        /// <summary>
        /// The name of default ComponentSetting for Process.
        /// </summary>
        protected string m_defaultProcessName = "";

        /// <summary>
        /// The name of default ComponentSetting for Variable.
        /// </summary>
        protected string m_defaultVariableName = "";

        /// <summary>
        /// The name of default ComponentSetting for Text.
        /// </summary>
        protected string m_defaultTextName = "";

        /// <summary>
        /// The name of default ComponentSetting for Stepper.
        /// </summary>
        protected string m_defaultStepperName = "";

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
        /// Accessor for m_textSettings.
        /// </summary>
        public Dictionary<string, ComponentSetting> TextSettings
        {
            get { return this.m_textSettings; }
        }

        /// <summary>
        /// Accessor for m_stepperSettings.
        /// </summary>
        public Dictionary<string, ComponentSetting> StepperSettings
        {
            get { return this.m_stepperSettings; }
        }

        /// <summary>
        /// Accessor for default ComponentSetting for System.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting SystemSetting
        {
            get
            {
                if (m_defaultSystemName != null && m_systemSettings.ContainsKey(m_defaultSystemName))
                    return m_systemSettings[m_defaultSystemName];
                else
                    return null;
            }
        }

        /// <summary>
        /// Accessor for default ComponentSetting for Process.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting ProcessSetting
        {
            get
            {
                if (m_defaultProcessName != null && m_processSettings.ContainsKey(m_defaultProcessName))
                    return m_processSettings[m_defaultProcessName];
                else
                    return null;
            }
        }

        /// <summary>
        /// Accessor for default ComponentSetting for Variable.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting VariableSetting
        {
            get
            {
                if (m_defaultVariableName != null && m_variableSettings.ContainsKey(m_defaultVariableName))
                    return m_variableSettings[m_defaultVariableName];
                else
                    return null;
            }
        }

        /// <summary>
        /// Accessor for default ComponentSetting for Variable.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting TextSetting
        {
            get
            {
                if (m_defaultTextName != null && m_textSettings.ContainsKey(m_defaultTextName))
                    return m_textSettings[m_defaultTextName];
                else
                    return null;
            }
        }

        /// <summary>
        /// Accessor for default ComponentSetting for Variable.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting StepperSetting
        {
            get
            {
                if (m_defaultStepperName != null && m_stepperSettings.ContainsKey(m_defaultStepperName))
                    return m_stepperSettings[m_defaultStepperName];
                else
                    return null;
            }
        }

        /// <summary>
        /// Accessor for ComponentSettings.
        /// </summary>
        public List<ComponentSetting> DefaultComponentSettings
        {
            get
            {
                List<ComponentSetting> list = new List<ComponentSetting>();
                list.Add(SystemSetting);
                list.Add(VariableSetting);
                list.Add(ProcessSetting);
                list.Add(TextSetting);
                list.Add(StepperSetting);
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
            m_textSettings = new Dictionary<string, ComponentSetting>();
            m_stepperSettings = new Dictionary<string, ComponentSetting>();
            SetComponentSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ComponentSetting> GetAllSettings()
        {
            List<ComponentSetting> list = new List<ComponentSetting>();
            list.AddRange(m_systemSettings.Values);
            list.AddRange(m_processSettings.Values);
            list.AddRange(m_variableSettings.Values);
            list.AddRange(m_textSettings.Values);
            list.AddRange(m_stepperSettings.Values);

            return list;
        }

        /// <summary>
        /// Load ComponentSettings from default setting file.
        /// </summary>
        public void LoadSettings(string filename)
        {
            List<ComponentSetting> list;
            try
            {
                // Load ComponentSettings information from xml file.
                list = ComponentManager.LoadFromXML(filename);
                // Check and register ComponentSettings.
                UpdateComponent(list);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                UpdateComponent(CreateDefaultSettings());
                return;
            }
        }

        internal void UpdateComponent(List<ComponentSetting> list)
        {
            foreach (ComponentSetting cs in list)
            {
                ComponentSetting setting = GetSetting(cs.Type, cs.Name);
                if (setting == null || !setting.Name.Equals(cs.Name))
                {
                    RegisterSetting(cs);
                    continue;
                }

                setting.Figure = cs.Figure;
                setting.TextBrush = cs.TextBrush;
                setting.LineBrush = cs.LineBrush;
                setting.CenterBrush = cs.CenterBrush;
                setting.FillBrush = cs.FillBrush;
                setting.IsGradation = cs.IsGradation;
                setting.IconFile = cs.IconFile;
                setting.ImageStream = cs.ImageStream;
                setting.RaisePropertyChange();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static XmlNode ConvertToXmlNode(XmlDocument doc, ComponentSetting setting)
        {
            XmlElement cs = doc.CreateElement(ComponentConstants.xPathComponent);
            cs.SetAttribute(ComponentConstants.xPathType, setting.Type);
            cs.SetAttribute(ComponentConstants.xPathIsDafault, setting.IsDefault.ToString());
            cs.SetAttribute(ComponentConstants.xPathIsStencil, setting.IsStencil.ToString());

            XmlNode name = doc.CreateElement(ComponentConstants.xPathName);
            name.AppendChild(doc.CreateTextNode(setting.Name));
            cs.AppendChild(name);

            XmlNode iconFile = doc.CreateElement(ComponentConstants.xPathIconFile);
            iconFile.AppendChild(doc.CreateTextNode(setting.IconFile));
            cs.AppendChild(iconFile);

            XmlNode icon = doc.CreateElement(ComponentConstants.xPathIconImage);
            icon.AppendChild(doc.CreateTextNode(setting.ImageStream));
            cs.AppendChild(icon);

            XmlElement figure = doc.CreateElement(ComponentConstants.xPathFigure);
            figure.SetAttribute(ComponentConstants.xPathMode, ComponentConstants.xPathEdit);
            figure.SetAttribute(ComponentConstants.xPathType, setting.Figure.Type);
            cs.AppendChild(figure);

            XmlNode size = doc.CreateElement(ComponentConstants.xPathSize);
            size.AppendChild(doc.CreateTextNode(setting.Figure.Coordinates));
            figure.AppendChild(size);

            XmlNode textBrush = doc.CreateElement(ComponentConstants.xPathTextBrush);
            textBrush.AppendChild(doc.CreateTextNode(BrushManager.ParseBrushToString(setting.TextBrush)));
            figure.AppendChild(textBrush);

            XmlNode lineBrush = doc.CreateElement(ComponentConstants.xPathLineBrush);
            lineBrush.AppendChild(doc.CreateTextNode(BrushManager.ParseBrushToString(setting.LineBrush)));
            figure.AppendChild(lineBrush);

            XmlNode fillBrush = doc.CreateElement(ComponentConstants.xPathFillBrush);
            fillBrush.AppendChild(doc.CreateTextNode(BrushManager.ParseBrushToString(setting.FillBrush)));
            figure.AppendChild(fillBrush);

            XmlNode centerBrush = doc.CreateElement(ComponentConstants.xPathCenterBrush);
            centerBrush.AppendChild(doc.CreateTextNode(BrushManager.ParseBrushToString(setting.CenterBrush)));
            figure.AppendChild(centerBrush);

            XmlNode isGradation = doc.CreateElement(ComponentConstants.xPathIsGradation);
            isGradation.AppendChild(doc.CreateTextNode(setting.IsGradation.ToString()));
            figure.AppendChild(isGradation);

            return cs;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public PPathwayObject CreateNewComponent(string type, string key)
        {
            ComponentSetting setting = GetSetting(type, key);
            return setting.CreateTemplate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ComponentSetting> GetSettings(string type)
        {
            List<ComponentSetting> list = new List<ComponentSetting>();
            Dictionary<string, ComponentSetting> dic = GetSettingDictionary(type);
            foreach (ComponentSetting cs in dic.Values)
            {
                list.Add(cs);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ComponentSetting GetSetting(string type, string key)
        {
            ComponentSetting setting = null;
            Dictionary<string, ComponentSetting> dic = GetSettingDictionary(type);
            string defaultKey = GetDefaultKey(type);

            if (dic.ContainsKey(key))
                setting = dic[key];
            else if (dic.ContainsKey(defaultKey))
                setting = dic[defaultKey];

            return setting;

        }

        /// <summary>
        /// Register ComponentSetting onto this manager
        /// </summary>
        /// <param name="setting">ComponentSetting</param>
        public void RegisterSetting(ComponentSetting setting)
        {
            Dictionary<string, ComponentSetting> dic = GetSettingDictionary(setting.Type);
            if (dic.ContainsKey(setting.Name))
                dic.Remove(setting.Name);

            // Resister
            dic.Add(setting.Name, setting);
            if (setting.IsDefault)
                SetDefaultKey(setting.Type, setting.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        public void RemoveSetting(ComponentSetting setting)
        {
            Dictionary<string, ComponentSetting> dic = GetSettingDictionary(setting.Type);
            if (dic.ContainsKey(setting.Name))
                dic.Remove(setting.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string GetRandomKey()
        {
            return Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        internal void ClearSettings()
        {
            SetComponentSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ComponentSetting CreateDefaultSetting(string type)
        {
            ComponentSetting cs = null;
            List<ComponentSetting> list = CreateDefaultSettings();
            foreach (ComponentSetting c in list)
            {
                if (c.Type.Equals(type))
                    cs = c;
            }
            return cs;
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<ComponentSetting> CreateDefaultSettings()
        {
            List<ComponentSetting> list = new List<ComponentSetting>();
            // Set hard coded default system ComponentSettings
            ComponentSetting defSysCs = new ComponentSetting();
            defSysCs.Type = EcellObject.SYSTEM;
            defSysCs.Name = ComponentConstants.NameOfDefaultSystem;
            defSysCs.IsDefault = true;
            defSysCs.IsStencil = true;
            defSysCs.Figure = FigureManager.CreateFigure("SystemRectangle", "0,0,80,80");
            defSysCs.CenterBrush = Brushes.LightBlue;
            defSysCs.FillBrush = Brushes.LightBlue;
            defSysCs.IsGradation = false;
            defSysCs.LineBrush = Brushes.Blue;
            list.Add(defSysCs);

            // Set hard coded default variable ComponentSettings
            ComponentSetting defVarCs = new ComponentSetting();
            defVarCs.Type = EcellObject.VARIABLE;
            defVarCs.Name = ComponentConstants.NameOfDefaultVariable;
            defVarCs.IsDefault = true;
            defVarCs.IsStencil = true;
            defVarCs.Figure = FigureManager.CreateFigure("Ellipse", "0,0,60,40");
            defVarCs.TextBrush = Brushes.DarkBlue;
            defVarCs.LineBrush = Brushes.CornflowerBlue;
            defVarCs.CenterBrush = Brushes.White;
            defVarCs.FillBrush = Brushes.CornflowerBlue;
            defVarCs.IsGradation = true;
            list.Add(defVarCs);

            // Set hard coded default process ComponentSettings
            ComponentSetting defProCs = new ComponentSetting();
            defProCs.Type = EcellObject.PROCESS;
            defProCs.Name = ComponentConstants.NameOfDefaultProcess;
            defProCs.IsDefault = true;
            defProCs.IsStencil = true;
            defProCs.Figure = FigureManager.CreateFigure("RoundedRectangle", "0,0,60,40");
            defProCs.TextBrush = Brushes.DarkGreen;
            defProCs.LineBrush = Brushes.LimeGreen;
            defProCs.CenterBrush = Brushes.White;
            defProCs.FillBrush = Brushes.LimeGreen;
            defProCs.IsGradation = true;
            list.Add(defProCs);

            // Set hard coded default text ComponentSettings
            ComponentSetting defTextCs = new ComponentSetting();
            defTextCs.Type = EcellObject.TEXT;
            defTextCs.Name = ComponentConstants.NameOfDefaultText;
            defTextCs.IsDefault = true;
            defTextCs.IsStencil = true;
            defTextCs.Figure = FigureManager.CreateFigure("Rectangle", "0,0,80,26");
            defTextCs.TextBrush = Brushes.Black;
            defTextCs.LineBrush = Brushes.Black;
            defTextCs.CenterBrush = Brushes.White;
            defTextCs.FillBrush = Brushes.White;
            defTextCs.IsGradation = false;
            list.Add(defTextCs);

            // Set hard coded default stepper ComponentSettings
            ComponentSetting defStepperCs = new ComponentSetting();
            defStepperCs.Type = EcellObject.STEPPER;
            defStepperCs.Name = ComponentConstants.NameOfDefaultStepper;
            defStepperCs.IsDefault = true;
            defStepperCs.IsStencil = true;
            defStepperCs.Figure = FigureManager.CreateFigure("Ellipse", "0,0,30,30");
            defStepperCs.TextBrush = Brushes.Black;
            defStepperCs.LineBrush = Brushes.Red;
            defStepperCs.CenterBrush = Brushes.White;
            defStepperCs.FillBrush = Brushes.Red;
            defStepperCs.IsGradation = true;
            list.Add(defStepperCs);

            return list;
        }

        /// <summary>
        /// Find component settings file
        /// </summary>
        private static string GetUserSettingsFilePath()
        {
            string path = Util.GetUserDir();
            string filename = Path.Combine(path, ComponentConstants.xPathFileName);
            return filename;
        }

        /// <summary>
        /// Check file path.
        /// </summary>
        private static void CheckFilePath()
        {
            string path = Util.GetUserDir();
            if (string.IsNullOrEmpty(path))
                return;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Set ComponentSettings.
        /// This method is for loading user setting.
        /// If there is no user setting file, create default setting file.
        /// </summary>
        private void SetComponentSettings()
        {
            UpdateComponent(CreateDefaultSettings());
            string filename = GetUserSettingsFilePath();
            if (File.Exists(filename))
                LoadSettings(filename);
            else
                SaveSettings();
        }

        /// <summary>
        /// Get ComponentSetting dictionary.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Dictionary<string, ComponentSetting> GetSettingDictionary(string type)
        {
            Dictionary<string, ComponentSetting> dic = null;
            switch (type)
            {
                case EcellObject.SYSTEM:
                    dic = m_systemSettings;
                    break;
                case EcellObject.PROCESS:
                    dic = m_processSettings;
                    break;
                case EcellObject.VARIABLE:
                    dic = m_variableSettings;
                    break;
                case EcellObject.TEXT:
                    dic = m_textSettings;
                    break;
                case EcellObject.STEPPER:
                    dic = m_stepperSettings;
                    break;
                default:
                    throw new PathwayException(MessageResources.ErrUnknowType);
            }
            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetDefaultKey(string type)
        {
            string key;
            switch (type)
            {
                case EcellObject.SYSTEM:
                    key = m_defaultSystemName;
                    break;
                case EcellObject.PROCESS:
                    key = m_defaultProcessName;
                    break;
                case EcellObject.VARIABLE:
                    key = m_defaultVariableName;
                    break;
                case EcellObject.TEXT:
                    key = m_defaultTextName;
                    break;
                case EcellObject.STEPPER:
                    key = m_defaultStepperName;
                    break;
                default:
                    throw new PathwayException(MessageResources.ErrUnknowType);
            }
            return key;
        }

        /// <summary>
        /// Set Default ComponentSetting.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        private void SetDefaultKey(string type, string key)
        {
            switch (type)
            {
                case EcellObject.SYSTEM:
                    m_defaultSystemName = key;
                    break;
                case EcellObject.PROCESS:
                    m_defaultProcessName = key;
                    break;
                case EcellObject.VARIABLE:
                    m_defaultVariableName = key;
                    break;
                case EcellObject.TEXT:
                    m_defaultTextName = key;
                    break;
                case EcellObject.STEPPER:
                    m_defaultStepperName = key;
                    break;
                default:
                    throw new PathwayException(MessageResources.ErrUnknowType);
            }
        }
        #endregion

        #region Loader
        /// <summary>
        /// Load ComponentSettings from xml file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<ComponentSetting> LoadFromXML(string filename)
        {
            XmlDocument xmlD = new XmlDocument();
            xmlD.Load(filename);

            return LoadFromXML(xmlD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlD"></param>
        /// <returns></returns>
        public static List<ComponentSetting> LoadFromXML(XmlNode xmlD)
        {
            // Get Component List.
            XmlNode componentList = null;
            List<ComponentSetting> list = new List<ComponentSetting>();
            foreach (XmlNode node in xmlD.ChildNodes)
            {
                if (node.Name.Equals(ComponentConstants.xPathComponentList))
                    componentList = node;
            }

            // Create component.
            foreach (XmlNode componentNode in componentList.ChildNodes)
            {
                ComponentSetting cs = LoadComponentSetting(componentNode);
                list.Add(cs);
            }
            return list;
        }

        /// <summary>
        /// LoadComponentSetting
        /// </summary>
        /// <param name="componentNode"></param>
        /// <returns></returns>
        private static ComponentSetting LoadComponentSetting(XmlNode componentNode)
        {
            ComponentSetting cs = new ComponentSetting();
            try
            {
                cs.Type = GetStringAttribute(componentNode, ComponentConstants.xPathType);
                cs.IsDefault = GetBoolAttribure(componentNode, ComponentConstants.xPathIsDafault);
                cs.IsStencil = GetBoolAttribure(componentNode, ComponentConstants.xPathIsStencil) || cs.IsDefault;

                foreach (XmlNode parameterNode in componentNode.ChildNodes)
                {
                    if (parameterNode.Name.Equals(ComponentConstants.xPathName))
                    {
                        cs.Name = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(ComponentConstants.xPathIconFile))
                    {
                        cs.IconFile = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(ComponentConstants.xPathIconImage))
                    {
                        cs.ImageStream = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(ComponentConstants.xPathFigure))
                    {
                        LoadFigure(cs, parameterNode);
                    }
                }
            }
            catch (Exception e)
            {
                throw new PathwayException(MessageResources.ErrCreateComponent, e);
            }

            return cs;
        }

        /// <summary>
        /// GetStringAttribute
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringAttribute(XmlNode node, string key)
        {
            try
            {
                XmlAttribute attribute = node.Attributes[key];
                if (attribute == null)
                    return "";
                else
                    return attribute.Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBoolAttribure(XmlNode node, string key)
        {
            bool value = false;
            string temp = GetStringAttribute(node, key);
            bool.TryParse(temp, out value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="parameterNode"></param>
        private static void LoadFigure(ComponentSetting cs, XmlNode parameterNode)
        {
            foreach (XmlNode figureNode in parameterNode.ChildNodes)
            {
                if (figureNode.Name.Equals(ComponentConstants.xPathSize))
                {
                    cs.Figure = FigureManager.CreateFigure(parameterNode.Attributes[ComponentConstants.xPathType].Value, figureNode.InnerText);
                }
                else if (figureNode.Name.Equals(ComponentConstants.xPathTextBrush))
                {
                    Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                    if (brush != null)
                    {
                        cs.TextBrush = brush;
                    }
                }
                else if (figureNode.Name.Equals(ComponentConstants.xPathLineBrush))
                {
                    Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                    if (brush != null)
                    {
                        cs.LineBrush = brush;
                    }
                }
                else if (figureNode.Name.Equals(ComponentConstants.xPathFillBrush))
                {
                    Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                    if (brush != null)
                    {
                        cs.FillBrush = brush;
                    }
                }
                else if (figureNode.Name.Equals(ComponentConstants.xPathCenterBrush))
                {
                    Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                    if (brush != null)
                    {
                        cs.CenterBrush = brush;
                    }
                }
                else if (figureNode.Name.Equals(ComponentConstants.xPathIsGradation))
                {
                    cs.IsGradation = bool.Parse(figureNode.InnerText);
                }
            }
        }

        #endregion

        #region Saver

        /// <summary>
        /// Save ComponentSettings
        /// </summary>
        public void SaveSettings()
        {
            string filepath = GetUserSettingsFilePath();
            FileStream fs = null;
            XmlTextWriter xmlOut = null;
            try
            {
                // Create xml file
                CheckFilePath();
                fs = new FileStream(filepath, FileMode.Create);
                xmlOut = new XmlTextWriter(fs, Encoding.UTF8);

                // Use indenting for readability
                xmlOut.Formatting = Formatting.Indented;
                xmlOut.WriteStartDocument();

                // Always begin file with identification and warning
                xmlOut.WriteComment(ComponentConstants.xPathFileHeader1);
                xmlOut.WriteComment(ComponentConstants.xPathFileHeader2);

                // Application settings
                xmlOut.WriteStartElement(ComponentConstants.xPathComponentList);
                xmlOut.WriteAttributeString(ComponentConstants.xPathName, Application.ProductName);
                xmlOut.WriteAttributeString(ComponentConstants.xPathFileVersion, ComponentConstants.xPathVersion);

                // Object settings
                foreach (ComponentSetting setting in GetAllSettings())
                {
                    if (!setting.IsStencil && !setting.IsDefault)
                        continue;
                    XmlNode cs = ConvertToXmlNode(new XmlDocument(), setting);
                    cs.WriteTo(xmlOut);
                }
                xmlOut.WriteEndElement();
                xmlOut.WriteEndDocument();
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(MessageResources.ErrCompInvalid + Environment.NewLine + filepath + Environment.NewLine + ex.Message);

            }
            finally
            {
                if (xmlOut != null) xmlOut.Close();
                if (fs != null) fs.Close();
            }
        }
        
        #endregion
    }
}
