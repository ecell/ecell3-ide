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
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
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
        /// PathwayControl
        /// </summary>
        protected PathwayControl m_con = null;
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
        /// The name of default ComponentSetting for Text.
        /// </summary>
        protected string m_defaultTextName;

        /// <summary>
        /// The name of default ComponentSetting for Stepper.
        /// </summary>
        protected string m_defaultStepperName;

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_con.
        /// </summary>
        public PathwayControl Control
        {
            get { return this.m_con; }
            set { m_con = value; }
        }

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
        public List<ComponentSetting> ComponentSettings
        {
            get
            {
                List<ComponentSetting> list = new List<ComponentSetting>();
                list.Add(SystemSetting);
                list.Add(VariableSetting);
                list.Add(ProcessSetting);
                list.Add(TextSetting);
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
                CheckAndRegisterComponent(list);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                CreateDefaultSettings();
                return;
            }
        }

        /// <summary>
        /// Check errors and register each ComponentSetting.
        /// If any ComponentSettings in the xml file is invalid, these messages are shown.
        /// </summary>
        /// <param name="list"></param>
        internal void CheckAndRegisterComponent(List<ComponentSetting> list)
        {
            int csCount = 0;
            string warnMessage = "";
            foreach (ComponentSetting cs in list)
            {
                List<string> lackInfos = cs.Validate();
                if (lackInfos == null)
                {
                    RegisterSetting(cs);
                }
                else
                {
                    string name = (cs.Name == null) ? cs.Name : "ComponentSetting No." + csCount.ToString();
                    warnMessage += MessageResources.ErrCompInvalid + "\n";
                    foreach (string lackInfo in lackInfos)
                        warnMessage += "    " + name + " lacks " + lackInfo + "\n";
                }
                csCount++;
            }

            if (!string.IsNullOrEmpty(warnMessage))
            {
                Debug.Print(warnMessage);
                throw new ArgumentException(warnMessage);
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

            XmlNode name = doc.CreateElement(ComponentConstants.xPathName);
            name.AppendChild(doc.CreateTextNode(setting.Name));
            cs.AppendChild(name);

            XmlNode icon = doc.CreateElement(ComponentConstants.xPathIconFile);
            icon.AppendChild(doc.CreateTextNode(setting.IconFileName));
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
        /// <param name="key"></param>
        /// <returns></returns>
        public ComponentSetting GetSetting(string type, string key)
        {
            ComponentSetting setting;
            Dictionary<string, ComponentSetting> dic = GetSettingDictionary(type);
            string defaultKey = GetDefaultKey(type);

            if (dic.ContainsKey(key))
                setting = dic[key];
            else
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
            ComponentSetting cs;
            
            cs = StepperSetting;
            m_stepperSettings.Clear();
            RegisterSetting(cs);

            cs = SystemSetting;
            m_systemSettings.Clear();
            RegisterSetting(cs);

            cs = TextSetting;
            m_textSettings.Clear();
            RegisterSetting(cs);

            cs = ProcessSetting;
            m_processSettings.Clear();
            RegisterSetting(cs);

            cs = VariableSetting;
            m_variableSettings.Clear();
            RegisterSetting(cs);
            
        }

        /// <summary>
        /// 
        /// </summary>
        internal void CreateDefaultSettings()
        {
            // Set hard coded default system ComponentSettings
            ComponentSetting defSysCs = new ComponentSetting();
            defSysCs.Type = EcellObject.SYSTEM;
            defSysCs.Name = ComponentConstants.NameOfDefaultSystem;
            defSysCs.IsDefault = true;
            defSysCs.Figure = FigureManager.CreateFigure("SystemRectangle", "0,0,80,80");
            defSysCs.CenterBrush = Brushes.LightBlue;
            defSysCs.FillBrush = Brushes.LightBlue;
            defSysCs.IsGradation = false;
            defSysCs.LineBrush = Brushes.Blue;
            RegisterSetting(defSysCs);

            // Set hard coded default variable ComponentSettings
            ComponentSetting defVarCs = new ComponentSetting();
            defVarCs.Type = EcellObject.VARIABLE;
            defVarCs.Name = ComponentConstants.NameOfDefaultVariable;
            defVarCs.IsDefault = true;
            defVarCs.Figure = FigureManager.CreateFigure("Ellipse", "0,0,60,40");
            defVarCs.TextBrush = Brushes.DarkBlue;
            defVarCs.LineBrush = Brushes.CornflowerBlue;
            defVarCs.CenterBrush = Brushes.White;
            defVarCs.FillBrush = Brushes.CornflowerBlue;
            defVarCs.IsGradation = true;
            RegisterSetting(defVarCs);

            // Set hard coded default process ComponentSettings
            ComponentSetting defProCs = new ComponentSetting();
            defProCs.Type = EcellObject.PROCESS;
            defProCs.Name = ComponentConstants.NameOfDefaultProcess;
            defProCs.IsDefault = true;
            defProCs.Figure = FigureManager.CreateFigure("RoundedRectangle", "0,0,60,40");
            defProCs.TextBrush = Brushes.DarkGreen;
            defProCs.LineBrush = Brushes.LimeGreen;
            defProCs.CenterBrush = Brushes.White;
            defProCs.FillBrush = Brushes.LimeGreen;
            defProCs.IsGradation = true;
            RegisterSetting(defProCs);

            // Set hard coded default text ComponentSettings
            ComponentSetting defTextCs = new ComponentSetting();
            defTextCs.Type = EcellObject.TEXT;
            defTextCs.Name = ComponentConstants.NameOfDefaultText;
            defTextCs.IsDefault = true;
            defTextCs.Figure = FigureManager.CreateFigure("Rectangle", "0,0,80,26");
            defTextCs.TextBrush = Brushes.Black;
            defTextCs.LineBrush = Brushes.Black;
            defTextCs.CenterBrush = Brushes.White;
            defTextCs.FillBrush = Brushes.White;
            defTextCs.IsGradation = false;
            RegisterSetting(defTextCs);

            // Set hard coded default stepper ComponentSettings
            ComponentSetting defStepperCs = new ComponentSetting();
            defStepperCs.Type = EcellObject.STEPPER;
            defStepperCs.Name = ComponentConstants.NameOfDefaultStepper;
            defStepperCs.IsDefault = true;
            defStepperCs.Figure = FigureManager.CreateFigure("Ellipse", "0,0,30,30");
            defStepperCs.TextBrush = Brushes.Black;
            defStepperCs.LineBrush = Brushes.Red;
            defStepperCs.CenterBrush = Brushes.White;
            defStepperCs.FillBrush = Brushes.Red;
            defStepperCs.IsGradation = true;
            RegisterSetting(defStepperCs);
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
            CreateDefaultSettings();
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
                string type = componentNode.Attributes[ComponentConstants.xPathType].Value;
                string isDefault = componentNode.Attributes[ComponentConstants.xPathIsDafault].Value;
                cs.Type = type;
                cs.IsDefault = bool.Parse(isDefault);

                foreach (XmlNode parameterNode in componentNode.ChildNodes)
                {
                    if (parameterNode.Name.Equals(ComponentConstants.xPathName))
                    {
                        cs.Name = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(ComponentConstants.xPathIconFile))
                    {
                        cs.IconFileName = parameterNode.InnerText;
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
                foreach (ComponentSetting setting in ComponentSettings)
                {
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
