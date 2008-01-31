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
            try
            {
                // Load ComponentSettings information from xml file.
                List<ComponentSetting> list = LoadFromXML(filename);
                // Check and register ComponentSettings.
                CheckAndRegisterComponent(list);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                MessageBox.Show(m_resources.GetString("ErrNotComXml"), "WARNING", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
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
                fs = new FileStream(filepath, FileMode.Create);
                xmlOut = new XmlTextWriter(fs, Encoding.Unicode);

                // Use indenting for readability
                xmlOut.Formatting = Formatting.Indented;
                xmlOut.WriteStartDocument();

                // Always begin file with identification and warning
                xmlOut.WriteComment("PathwayWindow configuration file.");
                xmlOut.WriteComment("Automatically generated file. DO NOT modify!");

                // Application settings
                xmlOut.WriteStartElement("ComponentList");
                xmlOut.WriteAttributeString("Name", Application.ProductName);
                xmlOut.WriteAttributeString("Version", Application.ProductVersion);

                // Object settings
                foreach (ComponentSetting setting in ComponentSettings)
                {
                    xmlOut.WriteStartElement("Component");
                    xmlOut.WriteAttributeString("Type", ParseComponentTypeToString(setting.ComponentType));
                    xmlOut.WriteAttributeString("isDefault", setting.IsDefault.ToString());
                    xmlOut.WriteElementString("Name", setting.Name);
                    xmlOut.WriteElementString("Class", setting.Class);
                    xmlOut.WriteElementString("LineColor", BrushManager.ParseBrushToString(setting.LineBrush));
                    xmlOut.WriteElementString("FillColor", BrushManager.ParseBrushToString(setting.FillBrush));
                    xmlOut.WriteStartElement("Drawings");
                    foreach (FigureBase figure in setting.FigureList)
                    {
                        xmlOut.WriteStartElement("Draw");
                        xmlOut.WriteAttributeString("Type", figure.Type);
                        xmlOut.WriteValue(figure.Coordinates);
                        xmlOut.WriteEndElement();
                    }
                    xmlOut.WriteEndElement();
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
        /// Get an object template.
        /// </summary>
        /// <param name="cType"></param>
        /// <returns></returns>
        public PPathwayObject CreateTemplate(ComponentType cType)
        {
            ComponentSetting setting = GetDefaultComponentSetting(cType);
            if (setting == null)
                return null;
            PPathwayObject obj = setting.CreateTemplate();
            return obj;
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
            defSysCs.Name = DEFAULT_SYSTEM_NAME;
            defSysCs.IsDefault = true;
            defSysCs.FillBrush = Brushes.LightBlue;
            defSysCs.LineBrush = Brushes.Blue;
            defSysCs.AddFigure("RoundCornerRectangle", "0,0,500,500");
            defSysCs.AddComponentClass(ClassPPathwaySystem);
            RegisterSetting(defSysCs);

            // Set hard coded default variable ComponentSettings
            ComponentSetting defVarCs = new ComponentSetting();
            defVarCs.ComponentType = ComponentType.Variable;
            defVarCs.Name = DEFAULT_VARIABLE_NAME;
            defVarCs.IsDefault = true;
            defVarCs.FillBrush = Brushes.LightBlue;
            defVarCs.LineBrush = Brushes.Black;
            defVarCs.AddFigure("Ellipse", "-30,-20,60,40");
            defVarCs.AddComponentClass(ClassPPathwayVariable);
            RegisterSetting(defVarCs);

            // Set hard coded default process ComponentSettings
            ComponentSetting defProCs = new ComponentSetting();
            defProCs.ComponentType = ComponentType.Process;
            defProCs.Name = DEFAULT_PROCESS_NAME;
            defProCs.IsDefault = true;
            defProCs.FillBrush = Brushes.LightGreen;
            defProCs.LineBrush = Brushes.Black;
            defProCs.AddFigure("Rectangle", "-30,-20,60,40");
            defProCs.AddComponentClass(ClassPPathwayProcess);
            RegisterSetting(defProCs);
        }

        /// <summary>
        /// Find component settings file
        /// </summary>
        private static string GetUserSettingsFilePath()
        {
            string filename = Path.Combine(Util.GetUserDir(), "ComponentSettings.xml");
            return filename;
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
                if (node.Name.Equals("ComponentList"))
                    componentList = node;
            }
            if (componentList == null)
                return list;

            // Create component.
            foreach (XmlNode componentNode in componentList.ChildNodes)
            {
                ComponentSetting cs = new ComponentSetting();
                try
                {
                    string type = componentNode.Attributes["Type"].Value;
                    string isDefault = componentNode.Attributes["isDefault"].Value;
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
                    if (parameterNode.Name.Equals("Name"))
                    {
                        cs.Name = parameterNode.InnerText;
                    }
                    else if (parameterNode.Name.Equals("FillColor"))
                    {
                        Brush brush = BrushManager.ParseStringToBrush(parameterNode.InnerText);
                        if (brush != null)
                        {
                            cs.FillBrush = brush;
                        }
                    }
                    else if (parameterNode.Name.Equals("LineColor"))
                    {
                        Brush brush = BrushManager.ParseStringToBrush(parameterNode.InnerText);
                        if (brush != null)
                        {
                            cs.LineBrush = brush;
                        }
                    }
                    else if (parameterNode.Name.Equals("Drawings"))
                    {
                        foreach (XmlNode drawNode in parameterNode.ChildNodes)
                        {
                            if (drawNode.Attributes["Type"] != null)
                                cs.AddFigure(drawNode.Attributes["Type"].Value, drawNode.InnerText);
                        }
                    }
                    else if (parameterNode.Name.Equals("Class"))
                    {
                        cs.AddComponentClass(parameterNode.InnerText);
                    }
                }
                list.Add(cs);
            }
            return list;
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
                    string warnMessage = m_resources.GetString("ErrCompInvalid") + "\n";
                    foreach (string lackInfo in lackInfos)
                        warnMessage += "    " + name + " lacks " + lackInfo + "\n";
                    MessageBox.Show(warnMessage, "WARNING by PathwayWindow", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                csCount++;
            }
        }
        #endregion

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
            }
        }

        /// <summary>
        /// private class for ComponentSettingDialog
        /// </summary>
        private class ComponentItem : GroupBox
        {
            private Label labelName;
            private Label labelFigure;
            private Label labelLineColor;
            private Label labelFillColor;
            private ComboBox cBoxFigure;
            private ComboBox cBoxLineColor;
            private ComboBox cBoxFillColor;
            private PToolBoxCanvas pCanvas;

            public ComponentItem(ComponentSetting cs)
            {
                this.labelName = new Label();
                this.labelFigure = new Label();
                this.labelLineColor = new Label();
                this.labelFillColor = new Label();
                this.cBoxFigure = new ComboBox();
                this.cBoxLineColor = new ComboBox();
                this.cBoxFillColor = new ComboBox();
                this.pCanvas = new PToolBoxCanvas();
                this.SuspendLayout();
                // 
                // groupBox
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left)| AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(this.labelName);
                this.Controls.Add(this.labelFigure);
                this.Controls.Add(this.labelLineColor);
                this.Controls.Add(this.labelFillColor);
                this.Controls.Add(this.cBoxFigure);
                this.Controls.Add(this.cBoxFillColor);
                this.Controls.Add(this.cBoxLineColor);
                this.Controls.Add(this.pCanvas);
                this.Name = "Panel";
                this.TabStop = false;
                // 
                // labelName
                // 
                this.labelName.AutoSize = true;
                this.labelName.Location = new System.Drawing.Point(14, 16);
                this.labelName.Text = cs.Name;
                this.labelName.Size = new System.Drawing.Size(35, 12);
                // 
                // labelFigure
                // 
                this.labelFigure.AutoSize = true;
                this.labelFigure.Location = new System.Drawing.Point(14, 42);
                this.labelFigure.Name = "labelFigure";
                this.labelFigure.Size = new System.Drawing.Size(53, 12);
                this.labelFigure.Text = "Figure";
                // 
                // labelLineColor
                // 
                this.labelLineColor.AutoSize = true;
                this.labelLineColor.Location = new System.Drawing.Point(14, 68);
                this.labelLineColor.Name = "labelLineColor";
                this.labelLineColor.Size = new System.Drawing.Size(53, 12);
                this.labelLineColor.Text = "LineColor";
                // 
                // labelFillColor
                // 
                this.labelFillColor.AutoSize = true;
                this.labelFillColor.Location = new System.Drawing.Point(14, 94);
                this.labelFillColor.Name = "labelFillColor";
                this.labelFillColor.Size = new System.Drawing.Size(48, 12);
                this.labelFillColor.Text = "FillColor";
                // 
                // cBoxFigure
                // 
                this.cBoxFigure.FormattingEnabled = true;
                this.cBoxFigure.Location = new System.Drawing.Point(100, 39);
                this.cBoxFigure.Name = "cBoxLineColor";
                this.cBoxFigure.Size = new System.Drawing.Size(128, 20);
                this.cBoxFigure.TabIndex = 0;
                this.cBoxFigure.Text = cs.FigureList[0].Type;
                // 
                // cBoxLineColor
                // 
                this.cBoxLineColor.FormattingEnabled = true;
                this.cBoxLineColor.Location = new System.Drawing.Point(100, 65);
                this.cBoxLineColor.Name = "cBoxLineColor";
                this.cBoxLineColor.Size = new System.Drawing.Size(128, 20);
                this.cBoxLineColor.TabIndex = 1;
                this.cBoxLineColor.Text = BrushManager.ParseBrushToString(cs.LineBrush);
                this.cBoxLineColor.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
                this.cBoxLineColor.TextChanged += new EventHandler(cBoxLineColor_TextChanged);
                // 
                // cBoxFillColor
                // 
                this.cBoxFillColor.FormattingEnabled = true;
                this.cBoxFillColor.Location = new System.Drawing.Point(100, 91);
                this.cBoxFillColor.Name = "cBoxFillColor";
                this.cBoxFillColor.Size = new System.Drawing.Size(128, 20);
                this.cBoxFillColor.TabIndex = 2;
                this.cBoxFillColor.Text = BrushManager.ParseBrushToString(cs.FillBrush);
                this.cBoxFillColor.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
                this.cBoxFillColor.TextChanged += new EventHandler(cBoxFillColor_TextChanged);
                // 
                // pCanvas
                // 
                this.pCanvas.AllowDrop = true;
                this.pCanvas.GridFitText = false;
                this.pCanvas.Name = "pCanvas";
                this.pCanvas.RegionManagement = true;
                this.pCanvas.Location = new System.Drawing.Point(250, 25);
                this.pCanvas.Size = new System.Drawing.Size(80, 80);
                this.pCanvas.BackColor = System.Drawing.Color.Silver;
                this.pCanvas.Setting = cs;

                this.ResumeLayout(false);
                this.PerformLayout();

                this.Height = 125;
            }

            public void ApplyChange()
            {
                ComponentSetting cs = this.pCanvas.Setting;
                cs.LineBrush = BrushManager.ParseStringToBrush(this.cBoxLineColor.Text);
                cs.FillBrush = BrushManager.ParseStringToBrush(this.cBoxFillColor.Text);
            }

            void cBoxFillColor_TextChanged(object sender, EventArgs e)
            {
                ComboBox colorBox = (ComboBox)sender;
                Brush brush = BrushManager.ParseStringToBrush(colorBox.Text);

                if (brush != null)
                {
                    this.pCanvas.PPathwayObject.FillBrush = brush;
                }
                else
                {
                    colorBox.Text = BrushManager.ParseBrushToString(this.pCanvas.Setting.FillBrush);
                }
            }

            void cBoxLineColor_TextChanged(object sender, EventArgs e)
            {
                ComboBox colorBox = (ComboBox)sender;
                Brush brush = BrushManager.ParseStringToBrush(colorBox.Text);

                if (brush != null)
                {
                    this.pCanvas.PPathwayObject.LineBrush = brush;
                }
                else
                {
                    colorBox.Text = BrushManager.ParseBrushToString(this.pCanvas.Setting.LineBrush);
                }
            }


        }
    }
}
