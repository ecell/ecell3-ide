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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.IO;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Resources;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.InteropServices;
using System.Net;

using Ecell;
using Ecell.Logging;
using Ecell.Plugin;
using Ecell.Objects;
using Ecell.Reporting;

using WeifenLuo.WinFormsUI.Docking;

using IronPython.Hosting;
using IronPython.Runtime;

namespace Ecell.IDE.MainWindow
{
    [ComVisible(true)]
    [Guid("758E6028-5769-4048-B3CB-AC633B9CABAF")]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public partial class MainWindow : Form, IEcellPlugin, IDockOwner, IRootMenuProvider, IDataHandler
    {
        #region Fields
        /// <summary>
        /// m_entityListDock (DockContent)
        /// </summary>
        private Dictionary<string, EcellDockContent> m_dockWindowDic;
        /// <summary>
        /// m_entityListDock (DockContent)
        /// </summary>
        private Dictionary<string, ToolStripMenuItem> m_dockMenuDic;
        /// <summary>
        /// defaultWindowSettingPath (string)
        /// </summary>
        private string m_defaultWindowSettingPath;
        /// <summary>
        /// userWindowSettingPath (string)
        /// </summary>
        private string m_userWindowSettingPath;
        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        private ApplicationEnvironment m_env;
        /// <summary>
        /// base directory of loading project.
        /// </summary>
        private string m_currentDir;
        /// <summary>
        /// delegate function of loading model.
        /// </summary>
        public delegate void LoadModelDelegate(string modelID);
        /// <summary>
        /// delegate function of close project.
        /// </summary>
        public delegate void CloseProjectDelegate(string projectID);
        /// <summary>
        /// System status.
        /// </summary>
        private ProjectStatus m_type = ProjectStatus.Uninitialized;
        /// <summary>
        /// The number of edit after project is opened.
        /// </summary>
        private int m_editCount = 0;
        /// <summary>
        /// Docking Windows object.
        /// </summary>
        public WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        /// <summary>
        /// RecentProjects
        /// </summary>
        private List<KeyValuePair<string, string>> m_recentProjects = new List<KeyValuePair<string, string>>();
        private GridJobStatusDialog m_statusDialog;
        private string m_title;
        private EcellWebBrowser m_browser;
        #endregion

        #region Accessor
        /// <summary>
        /// RecentProjects
        /// </summary>
        public List<KeyValuePair<string, string>> RecentProjects
        {
            get { return m_recentProjects; }
            set { m_recentProjects = value; }
        }

        /// <summary>
        /// get DockPanel.
        /// </summary>
        public virtual DockPanel DockPanel
        {
            get { return dockPanel; }
        }

        /// <summary>
        /// get / set ApplicationEnvironment object.
        /// </summary>
        public virtual ApplicationEnvironment Environment
        {
            get { return m_env; }
            set { m_env = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            m_dockWindowDic = new Dictionary<string, EcellDockContent>();
            m_dockMenuDic = new Dictionary<string, ToolStripMenuItem>();
        }
        #endregion

        #region Initializer
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            InitializeComponent();
            dockPanel.ShowDocumentIcon = true;

            m_statusDialog = new GridJobStatusDialog(m_env.JobManager);
            SetDockContent(m_statusDialog);
            // Load plugins
            LoadPlugins();
            //Load default window settings.
            setFilePath();
            SetRecentProject();
            LoadDefaultWindowSetting();
            SetStartUpWindow();
            m_title = this.Text;
            m_env.ReportManager.StatusUpdated += new StatusUpdatedEventHandler(ReportManager_StatusUpdated);
            m_env.ReportManager.ProgressValueUpdated += new ProgressReportEventHandler(ReportManager_ProgressValueUpdated);
        }

        /// <summary>
        /// Set window setting file path.
        /// </summary>
        private void setFilePath()
        {
            m_defaultWindowSettingPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Constants.fileWinSetting);
            m_userWindowSettingPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Application.ProductName);
            m_userWindowSettingPath = Path.Combine(m_userWindowSettingPath, Constants.fileWinSetting);
        }

        private void ResetCurrentDirectory()
        {
            string currentDir = Util.GetBaseDir();

            if (currentDir == null)
            {
                currentDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                    + "\\e-cell\\project";
            }
            m_currentDir = currentDir;
        }

        /// <summary>
        /// Load plugins.
        /// </summary>
        void LoadPlugins()
        {
            m_env.PluginManager.AppVersion = Assembly.GetExecutingAssembly().GetName().Version;
            ResetCurrentDirectory();

            // Load plugins
            foreach (string pluginDir in Util.GetPluginDirs())
            {
                string[] files = Directory.GetFiles(
                    pluginDir,
                    Constants.delimiterWildcard + Constants.FileExtPlugin);
                foreach (string fileName in files)
                {
                    m_env.PluginManager.LoadPlugin(fileName);
                }
            }

            foreach (IEcellPlugin pb in m_env.PluginManager.Plugins)
            {
                if (pb is IDockContentProvider)
                {
                    // Set DockContent.
                    IEnumerable<EcellDockContent> winList = ((IDockContentProvider)pb).GetWindowsForms();
                    if (winList != null)
                    {
                        foreach (EcellDockContent dock in winList)
                        {
                            SetDockContent(dock);
                        }
                    }
                }

                if (pb is IMenuStripProvider)
                {
                    // Set Menu.
                    IEnumerable<ToolStripMenuItem> menuList = ((IMenuStripProvider)pb).GetMenuStripItems();
                    if (menuList != null)
                    {
                        foreach (ToolStripMenuItem menu in menuList)
                        {
                            if (!this.menustrip.Items.ContainsKey(menu.Name))
                            {
                                // if you want to sort menu item at first plugin,
                                // you copy above program sequence.
                                this.menustrip.Items.AddRange(new ToolStripItem[] { menu });
                                continue;
                            }

                            while (menu.DropDownItems.Count > 0)
                            {
                                ToolStripItem[] tmp = this.menustrip.Items.Find(menu.Name, false);
                                ToolStripMenuItem menuItem = (ToolStripMenuItem)tmp[0];
                                ToolStripItem item = menu.DropDownItems[0];
                                IEnumerator iter = menuItem.DropDownItems.GetEnumerator();
                                int i = 0;
                                while (iter.MoveNext())
                                {
                                    ToolStripItem t = (ToolStripItem)iter.Current;
                                    if (Convert.ToInt32(t.Tag) > Convert.ToInt32(item.Tag))
                                    {
                                        menuItem.DropDownItems.Insert(i, item);
                                        i = -1;
                                        break;
                                    }
                                    i++;
                                }
                                if (i != -1)
                                {
                                    menuItem.DropDownItems.AddRange(new ToolStripItem[] { item });
                                }
                            }
                        }
                    }
                }

                if (pb is IToolStripProvider)
                {
                    // Set ToolBar
                    ToolStrip toolList = ((IToolStripProvider)pb).GetToolBarMenuStrip();
                    if (toolList != null)
                    {
                        this.toolStripContainer.TopToolStripPanel.Join(toolList, toolList.Location);
                    }
                    // Set menu availability.
                    foreach (ToolStripItem tool in menustrip.Items)
                    {
                        ToolStripMenuItem menu = (ToolStripMenuItem)tool;

                        if (menu.DropDownItems.Count <= 0)
                        {
                            menu.Enabled = false;
                        }
                    }
                }
            }
        }
        #endregion

        private void SetRecentProject()
        {
            // load xml file
            string filename = Path.Combine(Util.GetUserDir(), "RecentFile.xml");
            if (!File.Exists(filename))
                return;
            XmlDocument xmlD = new XmlDocument();
            try
            {
                xmlD.Load(filename);
                m_recentProjects.Clear();
                XmlNode recentFiles = GetNodeByKey(xmlD, "RecentFiles");
                foreach (XmlNode node in recentFiles.ChildNodes)
                {
                    string name = GetStringChild(node, "Name");
                    string file = GetStringChild(node, "File");
                    if (name == null || file == null || !File.Exists(file))
                        continue;
                    m_recentProjects.Add(new KeyValuePair<string, string>(name, file));
                }
                ResetRecentProjectMenu();
            }
            catch (Exception)
            {
            }
        }

        private void ResetRecentProjectMenu()
        {
            recentProejctToolStripMenuItem.DropDownItems.Clear();
            foreach (KeyValuePair<string, string> project in m_recentProjects)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(project.Key);
                item.ToolTipText = project.Value;
                item.Tag = project.Value;
                item.Click += new EventHandler(LoadRecentProjectMenuClick);
                recentProejctToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// Event on RecentProject_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoadRecentProjectMenuClick(object sender, EventArgs e)
        {
            if (!SaveConfirm())
                return;
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string filename = (string)item.Tag;
            try
            {
                m_env.DataManager.LoadProject((string)filename);
            }
            catch(Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
                
        }
        /// <summary>
        /// GetNodeByKey
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="key"></param>
        /// <returns>Selected XmlNode</returns>
        private static XmlNode GetNodeByKey(XmlNode xml, string key)
        {
            XmlNode selected = null;
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node.Name.Equals(key))
                    selected = node;
            }
            return selected;
        }

        /// <summary>
        /// GetNodeByKey
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="key"></param>
        /// <returns>Selected XmlNode</returns>
        private static string GetStringChild(XmlNode xml, string key)
        {
            XmlNode selected = null;
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node.Name.Equals(key))
                    selected = node;
            }
            return selected.InnerText;
        }

        /// <summary>
        /// GetStringAttribute
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetStringAttribute(XmlNode node, string key)
        {
            try
            {
                XmlAttribute attribute = node.Attributes[key];
                if (attribute == null)
                    return null;
                else
                    return attribute.Value;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return null;
            }
        }
        /// <summary>
        /// SaveRecentProject
        /// </summary>
        private void SaveRecentProject()
        {
            // Create xml file
            if (!Directory.Exists(Util.GetUserDir()))
                Directory.CreateDirectory(Util.GetUserDir());

            string filename = Path.Combine(Util.GetUserDir(), "RecentFile.xml");

            FileStream fs = null;
            XmlTextWriter xmlOut = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create);
                xmlOut = new XmlTextWriter(fs, Encoding.UTF8);

                // Use indenting for readability
                xmlOut.Formatting = Formatting.Indented;
                xmlOut.WriteStartDocument();

                // Always begin file with identification and warning
                xmlOut.WriteComment(Constants.xPathFileHeader1);
                xmlOut.WriteComment(Constants.xPathFileHeader2);

                // Application settings
                xmlOut.WriteStartElement("RecentFiles");
                int count = m_recentProjects.Count;
                int i = 0;
                foreach (KeyValuePair<string, string> project in m_recentProjects)
                {
                    i++;
                    if (i <= count - 5)
                        continue;
                    xmlOut.WriteStartElement("RecentFile");
                    xmlOut.WriteElementString("Name", project.Key);
                    xmlOut.WriteElementString("File", project.Value);
                    xmlOut.WriteEndElement();
                }
                xmlOut.WriteEndElement();
                xmlOut.WriteEndDocument();
            }
            finally
            {
                if (xmlOut != null) xmlOut.Close();
                if (fs != null) fs.Close();
            }

        }

        #region WindowSetting
        /// <summary>
        /// Save default window settings.
        /// </summary>
        private void SaveWindowSetting(string filename)
        {
            //Save current window settings.
            try
            {
                Trace.WriteLine("Saving window settings: " + filename);
                DockWindowSerializer.SaveAsXML(this, filename);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(MessageResources.ErrSaveWindowSettings);
            }
        }

        /// <summary>
        /// Load window settings.
        /// </summary>
        private bool LoadWindowSetting(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    Trace.WriteLine("Loading window settings: " + filename);
                    DockWindowSerializer.LoadFromXML(this, filename);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(MessageResources.ErrLoadWindowSettings);

                return false;
            }
            return false;
        }

        /// <summary>
        /// Load default window settings.
        /// </summary>
        public void LoadDefaultWindowSetting()
        {
            //Load user window settings.
            // Load default window settings when failed.
            if (!LoadWindowSetting(m_userWindowSettingPath))
            {
                InitialPreferencesDialog win = new InitialPreferencesDialog(true);
                using (win)
                {
                    if (win.ShowDialog() != DialogResult.OK)
                        return;
                    LoadWindowSetting(win.FilePath);
                    LoadWindowSetting(m_defaultWindowSettingPath);
                }
            }
        }
        #endregion

        /// <summary>
        /// Close the project.
        /// </summary>
        private void CloseProject()
        {
            m_env.DataManager.CloseProject();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void SetStartUpWindow()
        {
            EcellWebBrowser content = new EcellWebBrowser(m_env, m_recentProjects);
            content.Name = "StartUpWindow";
            content.Text = "StartUpWindow";
            content.DockHandler.DockPanel = this.dockPanel;
            SetDockContent(content);
            m_browser = content;
        }

        /// <summary>
        /// set DockContent
        /// </summary>
        private void SetDockContent(EcellDockContent content)
        {
            m_env.LogManager.Append(new ApplicationLogEntry(
                MessageType.Information,
                string.Format(MessageResources.DockPreparing, content.Text),
                this
            ));
            content.SuspendLayout();
            //Create New DockContent
            content.Pane = null;
            content.PanelPane = null;
            content.FloatPane = null;
            content.DockHandler.DockPanel = this.dockPanel;
            content.IsHidden = false;
            content.FormClosing += new FormClosingEventHandler(this.DockContent_Closing);

            //Create DockWindow Menu
            SetDockContentMenu(content);
            m_dockWindowDic.Add(content.Name, content);
            content.ResumeLayout();
            content.Show(this.dockPanel, DockState.Document);
        }
        /// <summary>
        /// set Window Menu
        /// </summary>
        private void SetDockContentMenu(DockContent content)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(
                content.TabText,
                (System.Drawing.Image)
                TypeDescriptor.GetConverter(content.Icon)
                    .ConvertTo(content.Icon,
                        typeof(System.Drawing.Image)),
                new System.EventHandler(DockWindowMenuClick),
                content.Name);
            item.Tag = content.Name;
            item.Checked = true;
            item.Tag = content.Name;
            this.showWindowToolStripMenuItem.DropDown.Items.Add(item);
            m_dockMenuDic.Add(content.Name, item);
        }

        /// <summary>
        /// Get specified DockContent
        /// </summary>
        public DockContent GetDockContent(string name)
        {
            EcellDockContent retval = null;
            m_dockWindowDic.TryGetValue(name, out retval);
            return retval;
        }

        private void DockContent_Closing(object sender, FormClosingEventArgs e)
        {
            m_env.LogManager.Append(new ApplicationLogEntry(
                MessageType.Debug,
                string.Format(MessageResources.DockClosing, ((DockContent)sender).Name),
                this
            ));
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // hide dock window
                ((DockContent)sender).Hide();
                // uncheck dock window menu
                CheckWindowMenu(((DockContent)sender).Name, false);
                e.Cancel = true;
            }
            else
            {
                ((Form)sender).Controls.Clear();
            }
        }

        /// <summary>
        /// Set the check information of window menu.
        /// </summary>
        /// <param name="name">window menu name.</param>
        /// <param name="bChecked">input data.</param>
        public void CheckWindowMenu(String name, bool bChecked)
        {
            m_dockMenuDic[name].Checked = bChecked;
        }

        #region PluginBase
        /// <summary>
        /// Get menustrips for MainWindow plugin.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get toolbar buttons for MainWindow plugin.
        /// </summary>
        /// <returns>null.</returns>
        public ToolStrip GetToolBarMenuStrip()
        {
            return null;
        }

        /// <summary>
        /// Get the window form for MainWindow plugin.
        /// </summary>
        /// <returns>Windows form</returns>
        public IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            // not implement
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            m_editCount++;
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            m_editCount++;
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public void LoggerAdd(string modelID, string key, string type, string path)
        {
            m_editCount++;
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            m_editCount++;
        }

        /// <summary>
        /// The event sequence when the simulation parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID.</param>
        public void ParameterAdd(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is deleted.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterDelete(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is set.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterSet(string projectID, string parameterID)
        {
            // nothing
        }

        public void ParameterUpdate(string projectID, string parameterID)
        {
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="data">Changed value of object.</param>
        public void LogData(string modelID, string key, string type,
            string propName, List<LogData> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            this.Text = m_title;
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
            // nothing
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Uninitialized)
            {
                newProjectToolStripMenuItem.Enabled = true;
                openProjectToolStripMenuItem.Enabled = true;
                saveProjectToolStripMenuItem.Enabled = false;
                projectWizardMenuItem.Enabled = true;
                recentProejctToolStripMenuItem.Enabled = true;
                closeProjectToolStripMenuItem.Enabled = false;
                exportModelToolStripMenuItem.Enabled = false;
                importModelToolStripMenuItem.Enabled = true;
                importScriptToolStripMenuItem.Enabled = false;
                saveScriptToolStripMenuItem.Enabled = false;
                printToolStripMenuItem.Enabled = false;
                exitToolStripMenuItem.Enabled = true;
                MenuItemLayout.Enabled = false;
            }
            else if (type == ProjectStatus.Loaded)
            {
                newProjectToolStripMenuItem.Enabled = true;
                openProjectToolStripMenuItem.Enabled = true;
                recentProejctToolStripMenuItem.Enabled = true;
                saveProjectToolStripMenuItem.Enabled = true;
                projectWizardMenuItem.Enabled = true;
                closeProjectToolStripMenuItem.Enabled = true;
                exportModelToolStripMenuItem.Enabled = true;
                importModelToolStripMenuItem.Enabled = true;
                importScriptToolStripMenuItem.Enabled = true;
                saveScriptToolStripMenuItem.Enabled = true;
                printToolStripMenuItem.Enabled = true;
                exitToolStripMenuItem.Enabled = true;
                MenuItemLayout.Enabled = true;
            }
            else
            {
                newProjectToolStripMenuItem.Enabled = false;
                openProjectToolStripMenuItem.Enabled = false;
                saveProjectToolStripMenuItem.Enabled = false;
                projectWizardMenuItem.Enabled = false;
                recentProejctToolStripMenuItem.Enabled = false;
                closeProjectToolStripMenuItem.Enabled = false;
                exportModelToolStripMenuItem.Enabled = false;
                importModelToolStripMenuItem.Enabled = false;
                saveScriptToolStripMenuItem.Enabled = false;
                importScriptToolStripMenuItem.Enabled = false;
                saveScriptToolStripMenuItem.Enabled = false;
                printToolStripMenuItem.Enabled = true;
                exitToolStripMenuItem.Enabled = true;
                MenuItemLayout.Enabled = false;
            }
            // Reset edit count.
            if (type == ProjectStatus.Uninitialized || type == ProjectStatus.Loaded)
                m_editCount = 0;
            // Set recent Project.
            if (type == ProjectStatus.Loaded)
            {
                ProjectInfo info = m_env.DataManager.CurrentProject.Info;
                this.Text = info.Name + " - " + m_title;
                CheckAndReplaceRecentProject(info);
                ResetRecentProjectMenu();                
            }
            else if (type == ProjectStatus.Uninitialized)
            {
                this.Text = m_title;
            }
            m_statusDialog.ChangeStatus(type);
            m_type = type;
        }

        private void CheckAndReplaceRecentProject(ProjectInfo info)
        {
            string projectID = info.Name;
            string filename = info.FilePath;
            KeyValuePair<string, string> oldProject = new KeyValuePair<string,string>();
            foreach (KeyValuePair<string, string> project in m_recentProjects)
            {
                if (project.Key.Equals(projectID))
                    oldProject = project;
            }
            if (oldProject.Key != null)
                m_recentProjects.Remove(oldProject);
            if(File.Exists(filename))
                m_recentProjects.Add(new KeyValuePair<string, string>(projectID, filename));
            ResetRecentProjectMenu();
        }

        /// <summary>
        /// Change availability of undo/redo function
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            switch (status)
            {
                case UndoStatus.UNDO_REDO:
                    undoToolStripMenuItem.Enabled = true;
                    redoToolStripMenuItem.Enabled = true;
                    break;
                case UndoStatus.UNDO_ONLY:
                    undoToolStripMenuItem.Enabled = true;
                    redoToolStripMenuItem.Enabled = false;
                    break;
                case UndoStatus.REDO_ONLY:
                    undoToolStripMenuItem.Enabled = false;
                    redoToolStripMenuItem.Enabled = true;
                    break;
                case UndoStatus.NOTHING:
                    undoToolStripMenuItem.Enabled = false;
                    redoToolStripMenuItem.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public void SaveModel(string modelID, string directory)
        {
            // do nothing
        }


        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print(string name)
        {
            return null;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"MainWindow"</returns>        
        public string GetPluginName()
        {
            return "MainWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false.</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true.</returns>
        public IEnumerable<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            return names;
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetObservedData(EcellObservedData data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void RemoveObservedData(EcellObservedData data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetParameterData(EcellParameterData data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void RemoveParameterData(EcellParameterData data)
        {
        }

        public Dictionary<string, Delegate> GetPublicDelegate()
        {
            Dictionary<string, Delegate> list = new Dictionary<string, Delegate>();
            list.Add("SetDockContent", new SetDockContentDelegate( this.SetDockContent));
            list.Add("ShowGridStatus", new ShowDialogDelegate(this.ShowGridStatusDialog));
            return list;
        }
        #endregion

        #region Event
        /// <summary>
        /// The action of [new project] menu click.
        /// Show NewProjectDialog dialog and input project information.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void NewProjectMenuClick(object sender, EventArgs e)
        {
            if (!SaveConfirm())
                return;
            NewProjectDialog npd = new NewProjectDialog();
            using (npd)
            {
                if (npd.ShowDialog() != DialogResult.OK)
                    return;
                m_env.DataManager.CreateNewProject(npd.ProjectName, npd.ProjectName, npd.Comment, npd.DMList);
            }
        }

        /// <summary>
        /// Save confirm.
        /// </summary>
        /// <returns>
        /// It return true when the current project was closed successfully
        /// and returns false when SaveProject is canceled.
        /// </returns>
        private bool SaveConfirm()
        {
            if (m_editCount > 0)
            {
                try
                {
                    // Save if answer is yes.
                    if (Util.ShowYesNoCancelDialog(MessageResources.SaveConfirm))
                        SaveProject();
                    // Close project.
                    CloseProject();
                }
                catch (Exception)
                {
                    // Return false when canceled
                    return false;
                }
            }
            // Return true when the current project was closed successfully.
            return true;
        }

        /// <summary>
        /// The action of [open project] menu click.
        /// Show OpenProjectDialog and select the target project.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void LoadProjectMenuClick(object sender, EventArgs e)
        {
            // Check the modification and confirm save.
            if (!SaveConfirm())
                return;

            ProjectExplorerDialog ped = new ProjectExplorerDialog(m_currentDir);
            using (ped)
            {
                if (ped.ShowDialog() != DialogResult.OK)
                    return;
                // Close current project.
                if (!string.IsNullOrEmpty(m_env.DataManager.CurrentProjectID))
                    CloseProject();
                try
                {
                    m_env.DataManager.LoadProject(ped.Project);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    Util.ShowErrorDialog(ex.Message);
                }            
            }
        }

        /// <summary>
        /// The action of [save project] menu click.
        /// Show SaveProjectDialog and select the save instance.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void SaveProjectMenuClick(object sender, EventArgs e)
        {
            SaveProject();
        }

        /// <summary>
        /// Save Project
        /// </summary>
        private void SaveProject()
        {
            m_env.DataManager.SaveProject();
            ProjectInfo Info = m_env.DataManager.CurrentProject.Info;
            CheckAndReplaceRecentProject(Info);
            m_editCount = 0;
        }

        /// <summary>
        /// The action of [close projct] menu click.
        /// Show confirm dialog. if you select yes, system show SaveProjectDialog.
        /// </summary>
        /// <param name="sender">obeject(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void CloseProjectMenuClick(object sender, EventArgs e)
        {
            if (!SaveConfirm())
                return;
            CloseProject();
        }

        /// <summary>
        /// The action of [import model] menu click.
        /// Show OpenFileDialog and select the sbml file.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ImportModelMenuClick(object sender, EventArgs e)
        {
            // Check current project and save it.
            if (!SaveConfirm())
                return;
            // Show OpenFileDialog.
            try
            {
                m_openFileDialog.RestoreDirectory = true;
                m_openFileDialog.Filter = Constants.FilterEmlFile;
                if (m_openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                // Close project
                if (!string.IsNullOrEmpty(m_env.DataManager.CurrentProjectID))
                    CloseProject();
                // Load new project.
                string filepath = m_openFileDialog.FileName;
                m_env.DataManager.LoadProject(filepath);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
                return;
            }

        }

        /// <summary>
        /// The action of [export model] menu click.
        /// Show SaveFileDialog and select the sbml file.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ExportModelMenuClick(object sender, EventArgs e)
        {
            //            List<SaveProjectDialog.ProjectItem> items = new List<SaveProjectDialog.ProjectItem>();
            List<string> items = new List<string>();
            {
                foreach (string s in m_env.DataManager.GetModelList())
                    items.Add(s);
            }


            try
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = Constants.FilterEmlFile;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    m_env.DataManager.ExportModel(items, saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// The action of [save script] menu click.
        /// show SaveFileDialog and select the eml file.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void SaveScriptMenuClick(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = Constants.FilterEssFile;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    m_env.DataManager.SaveScript(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// The action of [print] menu click.
        /// show SelectPluginDialog of PluginManager and select the plugin.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void PrintMenuClick(object sender, EventArgs e)
        {
            PrintPluginDialog d = new PrintPluginDialog(m_env.PluginManager);
            DialogResult result = d.ShowDialog();
            d.Dispose();
            if (result == DialogResult.OK)
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += delegate(object o, PrintPageEventArgs pe)
                {
                    Bitmap bmp = d.SelectedItem.Plugin.Print(d.SelectedItem.Portion);
                    pe.Graphics.DrawImage(bmp, new Point(0, 0));
                    bmp.Dispose();
                };
                pd.Print();
                pd.Dispose();
            }
        }

        /// <summary>
        /// The action of [exit] menu click.
        /// Exit this program.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ExitMenuClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The action of [Setup]->[ModelEditor] menu click.
        /// Set base directory of e-cell for loading plugin.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ModelEditorMenuClick(object sender, EventArgs e)
        {
            string currentDir = Util.GetBaseDir();

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = currentDir;
            using (dialog)
            {
                dialog.Description = MessageResources.ExpModelMes;
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                Util.SetBaseDir(dialog.SelectedPath);
                ResetCurrentDirectory();
            }
        }

        /// <summary>
        /// The action of disposing MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindowDisposed(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// The action of [File] -> [Import Script] menu click.
        /// Show OpenFileDialog and select the IronPython's script.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ImportScriptMenuClick(object sender, EventArgs e)
        {
            if (openScriptDialog.ShowDialog() == DialogResult.OK)
            {
                //
                // IronPython
                //
                m_env.DataManager.ExecuteScript(openScriptDialog.FileName);

                //
                // Executes continuously.
                //
                if (m_env.DataManager.CurrentProjectID != null)
                {
                    if (m_env.DataManager.GetCurrentSimulationTime() > 0.0)
                    {
                        m_env.DataManager.SimulationSuspend();
                    }
                }
            }
        }

        /// <summary>
        /// The action of clicking the import action menu.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ImportActionMenuClick(object sender, EventArgs e)
        {
            try
            {
                m_openFileDialog.RestoreDirectory = true;
                m_openFileDialog.Filter = Constants.FilterActionFile;

                if (m_openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(m_openFileDialog.FileName))
                    {
                        LoadUserActionFile(m_openFileDialog.FileName);
                    }
                    else
                    {
                        Util.ShowWarningDialog(MessageResources.FileNotFound);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void LoadUserActionFile(string filename)
        {
            m_env.DataManager.LoadUserActionFile(filename);
        }
        /// <summary>
        /// The action of clicking the save action menu.
        /// Save the action to the file.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void SaveActionMenuClick(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = Constants.FilterActionFile;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    m_env.DataManager.SaveUserAction(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// The action of clicking the undo action menu.
        /// Undo action.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void UndoMenuClick(object sender, EventArgs e)
        {
            m_env.DataManager.UndoAction();
        }

        /// <summary>
        /// The action of clicking the redo action menu.
        /// Redo action.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void RedoMenuClick(object sender, EventArgs e)
        {
            m_env.DataManager.RedoAction();
        }

        /// <summary>
        /// Event when version display menu is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ShowPluginVersionClick(object sender, EventArgs e)
        {
            PluginListDialog w = new PluginListDialog(m_env.PluginManager);
            using (w)
            {
                w.ShowDialog();
            }
        }

        private void ShowAboutDialog(object sender, EventArgs e)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string versionText = executingAssembly.GetName().Version.ToString();
            string copyrightText = ((AssemblyCopyrightAttribute)executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            string informationVersionText = ((AssemblyProductAttribute)executingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0]).Product;

            AboutDialog dlg = new AboutDialog(versionText, copyrightText, informationVersionText);
            using (dlg)
            {
                dlg.ShowDialog();
            }
        }

        private void ShowGridConfigurationDialog()
        {
            GridConfigurationDialog win = new GridConfigurationDialog(m_env.JobManager);
            using (win)
            {
                win.ShowDialog();
            }
        }

        private void ShowGridStatusDialog()
        {
            if (m_dockMenuDic.ContainsKey(m_statusDialog.Name))
            {
                if (!m_dockMenuDic[m_statusDialog.Name].Checked)
                {
                    m_statusDialog.Show();
                    m_dockMenuDic[m_statusDialog.Name].Checked = true;
                }
            }
        }

        private void ShowScriptEditor()
        {
            ScriptEditor edit = new ScriptEditor(m_env);
            using (edit)
            {
                edit.ShowDialog();
            }
        }

        /// <summary>
        /// Event when Save Window setting menu is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void SaveWindowSettingsClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = Constants.FilterWinSetFile;
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save window settings.
                SaveWindowSetting(sfd.FileName);
            }

        }

        /// <summary>
        /// Event when Load Windows setting menu is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void LoadWindowSettingsClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = Constants.FilterWinSetFile;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Load window settings.
                LoadWindowSetting(ofd.FileName);
            }
        }

        /// <summary>
        /// Event when docking window menu is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void DockWindowMenuClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.Checked)
            {
                //Hide EntityList
                m_dockWindowDic[(string)item.Tag].Hide();
                item.Checked = false;
            }
            else
            {
                //Show EntityList
                Debug.Assert(m_dockWindowDic[(string)item.Tag].Pane != null);
                m_dockWindowDic[(string)item.Tag].Show();
                item.Checked = true;
            }
        }

        /// <summary>
        /// Event when setup IDE button is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void SetupIDEMenuClick(object sender, EventArgs e)
        {
            InitialPreferencesDialog ipd = new InitialPreferencesDialog(false);
            using (ipd)
            {
                if (ipd.ShowDialog() != DialogResult.OK)
                    return;
                Util.SetLanguage(ipd.Language);
                LoadWindowSetting(ipd.FilePath);
                Util.ShowNoticeDialog(MessageResources.ResourceManager.GetString("ConfirmRestart", ipd.Language));
            }
        }

        /// <summary>
        /// Event when setup IDE button is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ProjectWizardMenuClick(object sender, EventArgs e)
        {
            ProjectWizardWindow win = new ProjectWizardWindow();
            if (win.ShowDialog() == DialogResult.OK && win.SelectedProject != null)
            {
                ProjectInfo info = win.SelectedProject;
                m_env.DataManager.LoadProject(info);
            }
        }

        /// <summary>
        /// Event when this form is closed.
        /// </summary>
        /// <param name="sender">this form.</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void MainWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_type == ProjectStatus.Running ||
                m_type == ProjectStatus.Stepping ||
                m_type == ProjectStatus.Suspended)
            {
                if (!Util.ShowYesNoDialog(MessageResources.ConfirmClose))
                {
                    e.Cancel = true;
                    return;
                }
                m_env.DataManager.SimulationStop();
                Thread.Sleep(1000);
            }

            if (!SaveConfirm())
                e.Cancel = true;

            SaveRecentProject();
            SaveWindowSetting(m_userWindowSettingPath);

            base.OnClosing(e);
        }

        /// <summary>
        /// Event when the distributed environment button is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ClickDistributedEnvMenu(object sender, EventArgs e)
        {
            ShowGridConfigurationDialog();
        }
        #endregion

        /// <summary>
        /// Set the process size.
        /// </summary>
        /// <param name="hwnd">window descriptor.</param>
        /// <param name="min">min size.</param>
        /// <param name="max">max size.</param>
        /// <returns>If contains size, retur true.</returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr hwnd, int min, int max);

        private void ClickScriptEditorMenu(object sender, EventArgs e)
        {
            ShowScriptEditor();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReportManager_StatusUpdated(object o, StatusUpdateEventArgs e)
        {
            bool isExeDoEvents = false;
            switch (e.Type)
            {
                case StatusBarMessageKind.Generic:
                    isExeDoEvents = !genericStatusText.Text.Equals(e.Text);
                    genericStatusText.Text = e.Text;
                    break;
                case StatusBarMessageKind.QuickInspector:
                    quickInspectorText.Text = e.Text;
                    break;
            }
            if (isExeDoEvents)
                Application.DoEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReportManager_ProgressValueUpdated(object o, ProgressReportEventArgs e)
        {
            genericProgressBar.Value = (e.Value == 100 ? 0 : e.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportSBMLMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveConfirm())
                return;
            OpenFileDialog dialog = new OpenFileDialog();
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadSBML(dialog.FileName);
                }
            }
        }

        private void LoadSBML(string filepath)
        {
            try
            {
                Encoding enc = Encoding.GetEncoding("shift_jis");
                string boundary = System.Environment.TickCount.ToString();
                string url = "http://chaperone.e-cell.org/services/sbml2eml/";
                string filename = Path.GetFileName(filepath);

                // Send HTTP request
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "multipart/form-data; boundary=" + boundary;
                string postData = "";
                postData = "--" + boundary + "\r\n" +
                    "Content-Disposition: form-data; name=\"file\"; filename=\"" +
                        filename + "\"\r\n" +
                    "Content-Type: application/octet-stream\r\n" +
                    "Content-Transfer-Encoding: binary\r\n\r\n";
                byte[] startData = enc.GetBytes(postData);
                postData = "\r\n--" + boundary + "\r\n";
                byte[] endData = enc.GetBytes(postData);

                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);

                req.ContentLength = startData.Length + endData.Length + fs.Length;
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(startData, 0, startData.Length);
                byte[] readData = new byte[0x1000];
                int readSize = 0;
                while (true)
                {
                    readSize = fs.Read(readData, 0, readData.Length);
                    if (readSize == 0)
                        break;
                    reqStream.Write(readData, 0, readSize);
                }
                fs.Close();
                reqStream.Write(endData, 0, endData.Length);
                reqStream.Close();

                // Get HTTP response
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                Stream resStream = res.GetResponseStream();
                StreamReader reader = new StreamReader(resStream, enc);

                // Save temporary file
                string savefile = Path.Combine(Util.GetTmpDir(), filename + Constants.FileExtEML);
                FileStream saveFs = new FileStream(savefile, FileMode.Create, FileAccess.Write);
                int b;
                while((b=resStream.ReadByte()) != -1)
                    saveFs.WriteByte(Convert.ToByte(b));

                saveFs.Close();
                resStream.Close();

                m_env.DataManager.LoadProject(savefile);
            }
            catch (Exception e)
            {
                Util.ShowErrorDialog("Failed to convert SBML.\n" + e.StackTrace);
            }

        }

        public ToolStripMenuItem GetRootMenuItem(string name)
        {
            switch (name)
            {
                case MenuConstants.MenuItemTools:
                    return MenuItemTools;
                case MenuConstants.MenuItemEdit:
                    return MenuItemEdit;
                case MenuConstants.MenuItemFile:
                    return MenuItemFile;
                case MenuConstants.MenuItemHelp:
                    return MenuItemHelp;
                case MenuConstants.MenuItemLayout:
                    return MenuItemLayout;
                case MenuConstants.MenuItemRun:
                    return MenuItemRun;
                case MenuConstants.MenuItemSetup:
                    return MenuItemSetup;
                case MenuConstants.MenuItemView:
                    return MenuItemView;
            }
            return null;
        }

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_browser.Url = new Uri("http://chaperone.e-cell.org/services/feedback/");
        }
    }
}
