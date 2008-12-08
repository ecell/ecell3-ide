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
using System.Text;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.Exceptions;
using Ecell.IDE.Plugins.PathwayWindow.Figure;
using System.Diagnostics;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.IDE.Plugins.PathwayWindow.Dialog;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow
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
        /// Accessor for default ComponentSetting for System.
        /// If it doesn't exist, null will be returned.
        /// </summary>
        public ComponentSetting DefaultSystemSetting
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
        public ComponentSetting DefaultProcessSetting
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
        public ComponentSetting DefaultVariableSetting
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
        public ComponentSetting DefaultTextSetting
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
                list.Add(DefaultTextSetting);
                return list;
            }
        }

        /// <summary>
        /// Create TabPage for PathwaySettingDialog
        /// </summary>
        /// <returns></returns>
        public PropertyDialogTabPage DialogTabPage
        {
            get { return new ComponentTabPage(this); }
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
            SetComponentSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load ComponentSettings from default setting file.
        /// </summary>
        public void LoadComponentSettings(string filename)
        {
            List<ComponentSetting> list = null;
            try
            {
                // Load ComponentSettings information from xml file.
                list = LoadFromXML(filename);
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
        /// Save ComponentSettings
        /// </summary>
        public void SaveComponentSettings()
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
                xmlOut.WriteComment(PathwayConstants.xPathFileHeader1);
                xmlOut.WriteComment(PathwayConstants.xPathFileHeader2);

                // Application settings
                xmlOut.WriteStartElement(PathwayConstants.xPathComponentList);
                xmlOut.WriteAttributeString(PathwayConstants.xPathName, Application.ProductName);
                xmlOut.WriteAttributeString(PathwayConstants.xPathFileVersion, PathwayConstants.xPathVersion);

                // Object settings
                foreach (ComponentSetting setting in ComponentSettings)
                {
                    xmlOut.WriteStartElement(PathwayConstants.xPathComponent);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathType, ParseComponentTypeToString(setting.ComponentType));
                    xmlOut.WriteAttributeString(PathwayConstants.xPathIsDafault, setting.IsDefault.ToString());
                    xmlOut.WriteElementString(PathwayConstants.xPathName, setting.Name);
                    xmlOut.WriteElementString(PathwayConstants.xPathClass, setting.Class);
                    xmlOut.WriteElementString(PathwayConstants.xPathIconFile, setting.IconFileName);
                    xmlOut.WriteStartElement(PathwayConstants.xPathFigure);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathMode, PathwayConstants.xPathEdit);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathType, setting.Figure.Type);
                    xmlOut.WriteElementString(PathwayConstants.xPathSize, setting.Figure.Coordinates);
                    xmlOut.WriteElementString(PathwayConstants.xPathTextBrush, BrushManager.ParseBrushToString(setting.TextBrush));
                    xmlOut.WriteElementString(PathwayConstants.xPathLineBrush, BrushManager.ParseBrushToString(setting.LineBrush));
                    xmlOut.WriteElementString(PathwayConstants.xPathFillBrush, BrushManager.ParseBrushToString(setting.FillBrush));
                    xmlOut.WriteElementString(PathwayConstants.xPathCenterBrush, BrushManager.ParseBrushToString(setting.CenterBrush));
                    xmlOut.WriteElementString(PathwayConstants.xPathIsGradation, setting.IsGradation.ToString());
                    xmlOut.WriteEndElement();
                    //xmlOut.WriteStartElement(xPathFigure);
                    //xmlOut.WriteAttributeString(xPathMode, xPathView);
                    //xmlOut.WriteAttributeString(xPathType, setting.Figure.Type);
                    //xmlOut.WriteElementString(xPathSize, setting.Figure.Coordinates);
                    //xmlOut.WriteElementString(xPathLineBrush, BrushManager.ParseBrushToString(setting.LineBrush));
                    //xmlOut.WriteElementString(xPathFillBrush, BrushManager.ParseBrushToString(setting.FillBrush));
                    //xmlOut.WriteEndElement();
                    xmlOut.WriteEndElement();
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

        /// <summary>
        /// Parse a name of kind to ComponentType
        /// </summary>
        /// <param name="type">a name of type, to be parsed</param>
        /// <returns></returns>
        public static ComponentType ParseStringToComponentType(string type)
        {
            if (type.Equals(EcellObject.SYSTEM))
            {
                return ComponentType.System;
            }
            else if (type.Equals(EcellObject.VARIABLE))
            {
                return ComponentType.Variable;
            }
            else if (type.Equals(EcellObject.PROCESS))
            {
                return ComponentType.Process;
            }
            else if (type.Equals(EcellObject.TEXT))
            {
                return ComponentType.Text;
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
        public static string ParseComponentTypeToString(ComponentType cType)
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
            else if (cType == ComponentType.Text)
            {
                return EcellObject.TEXT;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="cType"></param>
        /// <returns></returns>
        public ComponentSetting GetDefaultComponentSetting(ComponentType cType)
        {
            if (cType == ComponentType.System)
            {
                return DefaultSystemSetting;
            }
            else if (cType == ComponentType.Variable)
            {
                return DefaultVariableSetting;
            }
            else if (cType == ComponentType.Process)
            {
                return DefaultProcessSetting;
            }
            else if (cType == ComponentType.Text)
            {
                return DefaultTextSetting;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Register ComponentSetting onto this manager
        /// </summary>
        /// <param name="setting">ComponentSetting</param>
        public void RegisterSetting(ComponentSetting setting)
        {
            Dictionary<string, ComponentSetting> dic = GetSettingDictionary(setting.ComponentType);
            if (dic.ContainsKey(setting.Name))
                dic.Remove(setting.Name);

            // Resister
            dic.Add(setting.Name, setting);
            if (setting.IsDefault)
                SetDefaultSetting(setting);
        }

        #endregion

        #region Private Methods
        private void CreateDefaultSettings()
        {
            // Set hard coded default system ComponentSettings
            ComponentSetting defSysCs = new ComponentSetting();
            defSysCs.ComponentType = ComponentType.System;
            defSysCs.Name = PathwayConstants.NameOfDefaultSystem;
            defSysCs.Class = PathwayConstants.ClassPPathwaySystem;
            defSysCs.IsDefault = true;
            defSysCs.Figure = FigureManager.CreateFigure("SystemRectangle", "0,0,80,80");
            defSysCs.CenterBrush = Brushes.LightBlue;
            defSysCs.FillBrush = Brushes.LightBlue;
            defSysCs.IsGradation = false;
            defSysCs.LineBrush = Brushes.Blue;
            RegisterSetting(defSysCs);

            // Set hard coded default variable ComponentSettings
            ComponentSetting defVarCs = new ComponentSetting();
            defVarCs.ComponentType = ComponentType.Variable;
            defVarCs.Name = PathwayConstants.NameOfDefaultVariable;
            defVarCs.Class = PathwayConstants.ClassPPathwayVariable;
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
            defProCs.ComponentType = ComponentType.Process;
            defProCs.Name = PathwayConstants.NameOfDefaultProcess;
            defProCs.Class = PathwayConstants.ClassPPathwayProcess;
            defProCs.IsDefault = true;
            defProCs.Figure = FigureManager.CreateFigure("RoundedRectangle", "0,0,60,40");
            defProCs.TextBrush = Brushes.DarkGreen;
            defProCs.LineBrush = Brushes.LimeGreen;
            defProCs.CenterBrush = Brushes.White;
            defProCs.FillBrush = Brushes.LimeGreen;
            defProCs.IsGradation = true;
            RegisterSetting(defProCs);

            // Set hard coded default process ComponentSettings
            ComponentSetting defTextCs = new ComponentSetting();
            defTextCs.ComponentType = ComponentType.Text;
            defTextCs.Name = PathwayConstants.NameOfDefaultText;
            defTextCs.Class = PathwayConstants.ClassPPathwayText;
            defTextCs.IsDefault = true;
            defTextCs.Figure = FigureManager.CreateFigure("Rectangle", "0,0,100,80");
            defTextCs.TextBrush = Brushes.Black;
            defTextCs.LineBrush = Brushes.Black;
            defTextCs.CenterBrush = Brushes.White;
            defTextCs.FillBrush = Brushes.White;
            defTextCs.IsGradation = false;
            RegisterSetting(defTextCs);
        }

        /// <summary>
        /// Find component settings file
        /// </summary>
        private static string GetUserSettingsFilePath()
        {
            string path = Util.GetUserDir();
            string filename = Path.Combine(path, "ComponentSettings.xml");
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
                LoadComponentSettings(filename);
            else
                SaveComponentSettings();
        }

        /// <summary>
        /// Get ComponentSetting dictionary.
        /// </summary>
        /// <param name="cType"></param>
        /// <returns></returns>
        private Dictionary<string, ComponentSetting> GetSettingDictionary(ComponentType cType)
        {
            Dictionary<string, ComponentSetting> dic = null;
            switch (cType)
            {
                case ComponentType.System:
                    dic = m_systemSettings;
                    break;
                case ComponentType.Process:
                    dic = m_processSettings;
                    break;
                case ComponentType.Variable:
                    dic = m_variableSettings;
                    break;
                case ComponentType.Text:
                    dic = m_textSettings;
                    break;
            }
            return dic;
        }

        /// <summary>
        /// Load ComponentSettings from xml file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static List<ComponentSetting> LoadFromXML(string filename)
        {
            XmlDocument xmlD = new XmlDocument();
            xmlD.Load(filename);

            // Get Component List.
            XmlNode componentList = null;
            List<ComponentSetting> list = new List<ComponentSetting>();
            foreach (XmlNode node in xmlD.ChildNodes)
            {
                if (node.Name.Equals(PathwayConstants.xPathComponentList))
                    componentList = node;
            }
            CheckFileVersion(componentList);

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
                string type = componentNode.Attributes[PathwayConstants.xPathType].Value;
                string isDefault = componentNode.Attributes[PathwayConstants.xPathIsDafault].Value;
                cs.ComponentType = ParseStringToComponentType(type);
                cs.IsDefault = bool.Parse(isDefault);

                foreach (XmlNode parameterNode in componentNode.ChildNodes)
                {
                    if (parameterNode.Name.Equals(PathwayConstants.xPathName))
                    {
                        cs.Name = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(PathwayConstants.xPathClass))
                    {
                        cs.Class = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(PathwayConstants.xPathIconFile))
                    {
                        cs.IconFileName = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(PathwayConstants.xPathFigure))
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

        private static void LoadFigure(ComponentSetting cs, XmlNode parameterNode)
        {
            foreach (XmlNode figureNode in parameterNode.ChildNodes)
            {
                if (figureNode.Name.Equals(PathwayConstants.xPathSize))
                {
                    cs.Figure = FigureManager.CreateFigure(parameterNode.Attributes[PathwayConstants.xPathType].Value, figureNode.InnerText);
                }
                else if (figureNode.Name.Equals(PathwayConstants.xPathTextBrush))
                {
                    Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                    if (brush != null)
                    {
                        cs.TextBrush = brush;
                    }
                }
                else if (figureNode.Name.Equals(PathwayConstants.xPathLineBrush))
                {
                    Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                    if (brush != null)
                    {
                        cs.LineBrush = brush;
                    }
                }
                else if (figureNode.Name.Equals(PathwayConstants.xPathFillBrush))
                {
                    Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                    if (brush != null)
                    {
                        cs.FillBrush = brush;
                    }
                }
                else if (figureNode.Name.Equals(PathwayConstants.xPathCenterBrush))
                {
                    Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                    if (brush != null)
                    {
                        cs.CenterBrush = brush;
                    }
                }
                else if (figureNode.Name.Equals(PathwayConstants.xPathIsGradation))
                {
                    cs.IsGradation = bool.Parse(figureNode.InnerText);
                }
            }
        }

        /// <summary>
        /// Returns true when the fie version is correct.
        /// </summary>
        /// <param name="xmlNode"></param>
        private static void CheckFileVersion(XmlNode xmlNode)
        {
            if (xmlNode == null
                || xmlNode.Attributes[PathwayConstants.xPathFileVersion] == null
                || !xmlNode.Attributes[PathwayConstants.xPathFileVersion].Value.Equals(PathwayConstants.xPathVersion))
                throw new ArgumentException("Config file format Version error." + Environment.NewLine + "Current version is " + PathwayConstants.xPathVersion);
        }

        /// <summary>
        /// Set Default ComponentSetting.
        /// </summary>
        /// <param name="setting"></param>
        private void SetDefaultSetting(ComponentSetting setting)
        {
            switch (setting.ComponentType)
            {
                case ComponentType.System:
                    m_defaultSystemName = setting.Name;
                    break;
                case ComponentType.Process:
                    m_defaultProcessName = setting.Name;
                    break;
                case ComponentType.Variable:
                    m_defaultVariableName = setting.Name;
                    break;
                case ComponentType.Text:
                    m_defaultTextName = setting.Name;
                    break;
            }
        }

        /// <summary>
        /// Check errors and register each ComponentSetting.
        /// If any ComponentSettings in the xml file is invalid, these messages are shown.
        /// </summary>
        /// <param name="list"></param>
        private void CheckAndRegisterComponent(List<ComponentSetting> list)
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
        #endregion

        /// <summary>
        /// private class for ComponentSettingDialog
        /// </summary>
        internal class ComponentTabPage : PropertyDialogTabPage
        {
            ComponentManager m_manager = null;

            public ComponentTabPage(ComponentManager manager)
            {
                m_manager = manager;

                this.Text = MessageResources.DialogTextComponentSetting;
                this.SuspendLayout();
                int top = 0;
                foreach (ComponentSetting cs in m_manager.ComponentSettings)
                {
                    ComponentSetting.ComponentItem item = new ComponentSetting.ComponentItem(cs);
                    item.Top = top;
                    item.SuspendLayout();
                    this.Controls.Add(item);
                    item.ResumeLayout();
                    item.PerformLayout();
                    top += item.Height;
                }
                this.ResumeLayout();
            }
            public override void ApplyChange()
            {
                base.ApplyChange();
                foreach (ComponentSetting.ComponentItem item in this.Controls)
                {
                    item.ApplyChange();
                }
                m_manager.SaveComponentSettings();
            }
        }
    }
}
