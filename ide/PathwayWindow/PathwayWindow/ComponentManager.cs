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
using System.ComponentModel;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.Exceptions;
using EcellLib.PathwayWindow.Resources;
using EcellLib.PathwayWindow.Figure;
using System.Diagnostics;
using EcellLib.PathwayWindow.UIComponent;
using System.Drawing.Drawing2D;

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
        public const string NameOfDefaultSystem = "DefaultSystem";
        /// <summary>
        /// The default name of process.
        /// </summary>
        public const string NameOfDefaultProcess = "DefaultProcess";
        /// <summary>
        /// The default name of variable.
        /// </summary>
        public const string NameOfDefaultVariable = "DefaultVariable";
        /// <summary>
        /// Class name of PPathwaySystem.
        /// </summary>
        public const string ClassPPathwaySystem = "PPathwaySystem";
        /// <summary>
        /// Class name of PPathwayProcess.
        /// </summary>
        public const string ClassPPathwayProcess = "PPathwayProcess";
        /// <summary>
        /// Class name of PPathwayVariable.
        /// </summary>
        public const string ClassPPathwayVariable = "PPathwayVariable";

        #region XMLPath
        /// <summary>
        /// 
        /// </summary>
        private const string xPathFileHeader1 = "PathwayWindow configuration file.";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathFileHeader2 = "Automatically generated file. DO NOT modify!";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathComponentList = "ComponentList";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathFileVersion = "FileVersion";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathVersion = "1.0";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathComponent = "Component";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathType = "Type";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathIsDafault = "IsDefault";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathName = "Name";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathClass = "Class";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathIconFile = "IconFile";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathFigure = "Figure";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathMode = "Mode";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathEdit = "Edit";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathView = "View";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathSize = "Size";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathTextBrush = "TextBrush";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathLineBrush = "LineBrush";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathFillBrush = "FillBrush";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathCenterBrush = "CenterBrush";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathIsGradation = "IsGradation";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathNormal = "Normal";
        /// <summary>
        /// 
        /// </summary>
        private const string xPathGradation = "Gradation";

        #endregion
        #endregion

        #region Fields
        /// <summary>
        /// Dictionary of ComponentSettings for creating PPathwaySystems.
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
                //MessageBox.Show(m_resources.GetString("ErrNotComXml"), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                xmlOut.WriteComment(xPathFileHeader1);
                xmlOut.WriteComment(xPathFileHeader2);

                // Application settings
                xmlOut.WriteStartElement(xPathComponentList);
                xmlOut.WriteAttributeString(xPathName, Application.ProductName);
                xmlOut.WriteAttributeString(xPathFileVersion, xPathVersion);

                // Object settings
                foreach (ComponentSetting setting in ComponentSettings)
                {
                    xmlOut.WriteStartElement(xPathComponent);
                    xmlOut.WriteAttributeString(xPathType, ParseComponentTypeToString(setting.ComponentType));
                    xmlOut.WriteAttributeString(xPathIsDafault, setting.IsDefault.ToString());
                    xmlOut.WriteElementString(xPathName, setting.Name);
                    xmlOut.WriteElementString(xPathClass, setting.Class);
                    xmlOut.WriteElementString(xPathIconFile, setting.IconFileName);
                    xmlOut.WriteStartElement(xPathFigure);
                    xmlOut.WriteAttributeString(xPathMode, xPathEdit);
                    xmlOut.WriteAttributeString(xPathType, setting.EditModeFigure.Type);
                    xmlOut.WriteElementString(xPathSize, setting.EditModeFigure.Coordinates);
                    xmlOut.WriteElementString(xPathTextBrush, BrushManager.ParseBrushToString(setting.TextBrush));
                    xmlOut.WriteElementString(xPathLineBrush, BrushManager.ParseBrushToString(setting.LineBrush));
                    xmlOut.WriteElementString(xPathFillBrush, BrushManager.ParseBrushToString(setting.FillBrush));
                    xmlOut.WriteElementString(xPathCenterBrush, BrushManager.ParseBrushToString(setting.CenterBrush));
                    xmlOut.WriteElementString(xPathIsGradation, setting.IsGradation.ToString());
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
                string errmsg = m_resources.GetString("ErrCompInvalid") + Environment.NewLine + filepath + Environment.NewLine + ex.Message;
                MessageBox.Show(errmsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <summary>
        /// Show ComponentManagerDialog.
        /// </summary>
        public void ShowDialog()
        {
            PropertyDialog dialog = new PropertyDialog();
            dialog.Text = "ComponentManagerDialog";
            PropertyDialogTabPage page = CreateTabPage();
            dialog.TabControl.Controls.Add(page);
            if (dialog.ShowDialog() == DialogResult.OK)
                page.ApplyChange();
            dialog.Dispose();
        }

        /// <summary>
        /// Create TabPage for PathwaySettingDialog
        /// </summary>
        /// <returns></returns>
        public PropertyDialogTabPage CreateTabPage()
        {
            PropertyDialogTabPage page = new ComponentTabPage(this);
            return page;
        }
        #endregion

        #region Private Methods
        private void CreateDefaultSettings()
        {
            // Set hard coded default system ComponentSettings
            ComponentSetting defSysCs = new ComponentSetting();
            defSysCs.ComponentType = ComponentType.System;
            defSysCs.Name = NameOfDefaultSystem;
            defSysCs.Class = ClassPPathwaySystem;
            defSysCs.IsDefault = true;
            defSysCs.EditModeFigure = FigureBase.CreateFigure("SystemRectangle", "0,0,500,500");
            defSysCs.CenterBrush = Brushes.LightBlue;
            defSysCs.FillBrush = Brushes.LightBlue;
            defSysCs.IsGradation = false;
            defSysCs.LineBrush = Brushes.Blue;
            RegisterSetting(defSysCs);

            // Set hard coded default variable ComponentSettings
            ComponentSetting defVarCs = new ComponentSetting();
            defVarCs.ComponentType = ComponentType.Variable;
            defVarCs.Name = NameOfDefaultVariable;
            defVarCs.Class = ClassPPathwayVariable;
            defVarCs.IsDefault = true;
            defVarCs.EditModeFigure = FigureBase.CreateFigure("Ellipse", "0,0,60,40");
            defVarCs.LineBrush = Brushes.Black;
            defVarCs.CenterBrush = Brushes.LightBlue;
            defVarCs.FillBrush = Brushes.LightBlue;
            defVarCs.IsGradation = false;
            RegisterSetting(defVarCs);

            // Set hard coded default process ComponentSettings
            ComponentSetting defProCs = new ComponentSetting();
            defProCs.ComponentType = ComponentType.Process;
            defProCs.Name = NameOfDefaultProcess;
            defProCs.Class = ClassPPathwayProcess;
            defProCs.IsDefault = true;
            defProCs.EditModeFigure = FigureBase.CreateFigure("RoundedRectangle", "0,0,60,40");
            defProCs.LineBrush = Brushes.Black;
            defProCs.CenterBrush = Brushes.LightGreen;
            defProCs.FillBrush = Brushes.LightGreen;
            defProCs.IsGradation = false;
            RegisterSetting(defProCs);
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
                    dic = SystemSettings;
                    break;
                case ComponentType.Process:
                    dic = ProcessSettings;
                    break;
                case ComponentType.Variable:
                    dic = VariableSettings;
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
                if (node.Name.Equals(xPathComponentList))
                    componentList = node;
            }
            CheckFileVersion(componentList);

            // Create component.
            foreach (XmlNode componentNode in componentList.ChildNodes)
            {
                ComponentSetting cs = new ComponentSetting();
                try
                {
                    string type = componentNode.Attributes[xPathType].Value;
                    string isDefault = componentNode.Attributes[xPathIsDafault].Value;
                    cs.ComponentType = ParseStringToComponentType(type);
                    cs.IsDefault = bool.Parse(isDefault);
                }
                catch (NoSuchComponentKindException e)
                {
                    MessageBox.Show(m_resources.GetString("ErrCreateKind") + "\n\n" + e.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                foreach (XmlNode parameterNode in componentNode.ChildNodes)
                {
                    if (parameterNode.Name.Equals(xPathName))
                    {
                        cs.Name = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(xPathClass))
                    {
                        cs.Class = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(xPathIconFile))
                    {
                        cs.IconFileName = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals(xPathFigure))
                    {
                        foreach (XmlNode figureNode in parameterNode.ChildNodes)
                        {
                            if (figureNode.Name.Equals(xPathSize))
                            {
                                cs.EditModeFigure = FigureBase.CreateFigure(parameterNode.Attributes[xPathType].Value, figureNode.InnerText);
                            }
                            else if (figureNode.Name.Equals(xPathTextBrush))
                            {
                                Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                                if (brush != null)
                                {
                                    cs.TextBrush = brush;
                                }
                            }
                            else if (figureNode.Name.Equals(xPathLineBrush))
                            {
                                Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                                if (brush != null)
                                {
                                    cs.LineBrush = brush;
                                }
                            }
                            else if (figureNode.Name.Equals(xPathFillBrush))
                            {
                                Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                                if (brush != null)
                                {
                                    cs.FillBrush = brush;
                                }
                            }
                            else if (figureNode.Name.Equals(xPathCenterBrush))
                            {
                                Brush brush = BrushManager.ParseStringToBrush(figureNode.InnerText);
                                if (brush != null)
                                {
                                    cs.CenterBrush = brush;
                                }
                            }
                            else if (figureNode.Name.Equals(xPathIsGradation))
                            {
                                cs.IsGradation = bool.Parse(figureNode.InnerText);
                            }
                        }
                    }
                }
                list.Add(cs);
            }
            return list;
        }

        /// <summary>
        /// Returns true when the fie version is correct.
        /// </summary>
        /// <param name="xmlNode"></param>
        private static void CheckFileVersion(XmlNode xmlNode)
        {
            if (xmlNode == null
                || xmlNode.Attributes[xPathFileVersion] == null 
                ||!xmlNode.Attributes[xPathFileVersion].Value.Equals(xPathVersion))
                throw new ArgumentException("Config file format Version error." + Environment.NewLine + "Current version is " + xPathVersion);
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
                    warnMessage += m_resources.GetString("ErrCompInvalid") + "\n";
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
        private class ComponentTabPage : PropertyDialogTabPage
        {
            ComponentManager m_manager = null;

            public ComponentTabPage(ComponentManager manager)
            {
                m_manager = manager;

                this.Text = "ComponentSettings";
                this.SuspendLayout();
                int top = 0;
                foreach (ComponentSetting cs in m_manager.ComponentSettings)
                {
                    ComponentItem item = new ComponentItem(cs);
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
                foreach (ComponentItem item in this.Controls)
                {
                    item.ApplyChange();
                }
                m_manager.SaveComponentSettings();
            }
        }

        /// <summary>
        /// private class for ComponentSettingDialog
        /// </summary>
        private class ComponentItem : GroupBox
        {
            #region Fields
            private PropertyComboboxItem figureBox;
            private PropertyBrushItem textBrush;
            private PropertyBrushItem lineBrush;
            private PropertyBrushItem fillBrush;
            private PropertyBrushItem centerBrush;
            private PropertyCheckBoxItem isGradation;
            private PropertyFileItem iconFile;

            private PToolBoxCanvas pCanvas;
            #endregion

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="cs"></param>
            public ComponentItem(ComponentSetting cs)
            {
                // Create UI Object
                List<string> brushList = BrushManager.GetBrushNameList();

                this.figureBox = new PropertyComboboxItem("Figure", cs.EditModeFigure.Type, new List<string>());
                this.textBrush = new PropertyBrushItem("Text Brush", cs.TextBrush, brushList);
                this.lineBrush = new PropertyBrushItem("Line Brush", cs.LineBrush, brushList);
                this.fillBrush = new PropertyBrushItem("Fill Brush", cs.FillBrush, brushList);
                this.centerBrush = new PropertyBrushItem("", cs.CenterBrush, brushList);
                this.isGradation = new PropertyCheckBoxItem("Gradation", cs.IsGradation);
                this.iconFile = new PropertyFileItem("Icon File", cs.IconFileName);
                this.pCanvas = new PToolBoxCanvas();
                this.SuspendLayout();
                // Set Gradation
                this.centerBrush.ComboBox.Enabled = isGradation.Checked;

                // Set to GroupBox
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left)| AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(this.isGradation);
                this.Controls.Add(this.figureBox);
                this.Controls.Add(this.textBrush);
                this.Controls.Add(this.lineBrush);
                this.Controls.Add(this.fillBrush);
                this.Controls.Add(this.centerBrush);
                this.Controls.Add(this.iconFile);
                this.Controls.Add(this.pCanvas);
                this.Name = "GroupBox";
                this.Text = cs.Name;
                this.TabStop = false;
                // Set Position
                this.figureBox.Location = new Point(5, 15);
                this.textBrush.Location = new Point(5, 40);
                this.lineBrush.Location = new Point(5, 65);
                this.fillBrush.Location = new Point(5, 90);
                this.centerBrush.Location = new Point(5, 115);
                this.isGradation.Location = new Point(5, 115);
                this.isGradation.CheckBox.Left = 90;
                this.isGradation.Width = 120;
                this.iconFile.Location = new Point(5, 140);
                // Set EventHandler
                this.textBrush.BrushChange += new EventHandler(textBrush_BrushChange);
                this.lineBrush.BrushChange += new EventHandler(lineBrush_BrushChange);
                this.fillBrush.BrushChange += new EventHandler(fillBrush_BrushChange);
                this.centerBrush.BrushChange += new EventHandler(fillBrush_BrushChange);
                this.isGradation.CheckedChanged += new EventHandler(isGradation_CheckedChanged);
                // Set pCanvas
                this.pCanvas.AllowDrop = true;
                this.pCanvas.GridFitText = false;
                this.pCanvas.Name = "pCanvas";
                this.pCanvas.RegionManagement = true;
                this.pCanvas.Location = new System.Drawing.Point(300, 25);
                this.pCanvas.Size = new System.Drawing.Size(80, 80);
                this.pCanvas.BackColor = System.Drawing.Color.Silver;
                this.pCanvas.Setting = cs;
                this.pCanvas.PPathwayObject.PText.Text = "Sample";
                this.pCanvas.PPathwayObject.Refresh();
                // Set FileDialog

                this.iconFile.Dialog.Filter = "All Supported Format|*.BMP;*.DIB;*.RLE;*.JPG;*.JPEG;*.JPE;*.JFIF;*.GIF;*.PNG;*.ICO;*.EMF;*.WMF;*.TIF;*.TIFF|BMP File|*.BMP;*.DIB;*.RLE|JPEG File|*.JPG;*.JPEG;*.JPE;*.JFIF|GIF File|*.GIF|PNG File|*.PNG|ICO File|*.ICO|EMF File, WMF File|*.EMF;*.WMF|TIFF File|*.TIF;*.TIFF";
                this.iconFile.Dialog.FilterIndex = 0;

                this.ResumeLayout(false);
                this.PerformLayout();
                this.Height = 200;
            }

            /// <summary>
            /// Apply changes to ComponentSettings.
            /// </summary>
            public void ApplyChange()
            {
                ComponentSetting cs = this.pCanvas.Setting;
                cs.TextBrush = textBrush.Brush;
                cs.LineBrush = lineBrush.Brush;
                cs.FillBrush = fillBrush.Brush;
                cs.CenterBrush = centerBrush.Brush;
                cs.IsGradation = isGradation.Checked;
                cs.IconFileName = iconFile.FileName;
            }

            #region EventHandlers
            private void textBrush_BrushChange(object sender, EventArgs e)
            {
                this.pCanvas.PPathwayObject.PText.TextBrush = textBrush.Brush;
            }

            private void lineBrush_BrushChange(object sender, EventArgs e)
            {
                this.pCanvas.PPathwayObject.LineBrush = lineBrush.Brush;
            }

            private void fillBrush_BrushChange(object sender, EventArgs e)
            {
                PropertyBrushItem brushBox = (PropertyBrushItem)sender;
                if (brushBox.Brush == null)
                    return;
                ChangeFillBrush();
            }

            private void isGradation_CheckedChanged(object sender, EventArgs e)
            {
                centerBrush.ComboBox.Enabled = isGradation.Checked;
                ChangeFillBrush();
            }

            private void ChangeFillBrush()
            {
                if (isGradation.Checked)
                {
                    PathGradientBrush pthGrBrush = new PathGradientBrush(this.pCanvas.PPathwayObject.Path);
                    pthGrBrush.CenterColor = BrushManager.ParseBrushToColor(centerBrush.Brush);
                    pthGrBrush.SurroundColors = new Color[] { BrushManager.ParseBrushToColor(fillBrush.Brush) };
                    this.pCanvas.PPathwayObject.FillBrush = pthGrBrush;
                }
                else
                {
                    this.pCanvas.PPathwayObject.FillBrush = fillBrush.Brush;
                }
            }
            #endregion
        }
    }
}
