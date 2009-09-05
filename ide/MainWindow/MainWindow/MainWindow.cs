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
using Ecell.Logger;
using Ecell.Plugin;
using Ecell.Objects;
using Ecell.Reporting;

using WeifenLuo.WinFormsUI.Docking;

using IronPython.Hosting;
using IronPython.Runtime;
using Ecell.IDE.MainWindow.UIComponents;
using Ecell.Exceptions;
using Ecell.Action;

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
        /// userWindowSettingPath (string)
        /// </summary>
        private string m_userWindowSettingPath;
        /// <summary>
        /// userWindowSettingPath2 (string)
        /// </summary>
        private string m_userWindowSettingPath2;
        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        private ApplicationEnvironment m_env;
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
        private ProjectStatus m_status = ProjectStatus.Uninitialized;
        /// <summary>
        /// The number of edit after project is opened.
        /// </summary>
        private int m_editCount = 0;
        /// <summary>
        /// Docking Windows object.
        /// </summary>
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        /// <summary>
        /// RecentProjects
        /// </summary>
        private List<KeyValuePair<string, string>> m_recentProjects = new List<KeyValuePair<string, string>>();
        /// <summary>
        /// 
        /// </summary>
        private GridJobStatusDialog m_statusDialog;
        /// <summary>
        /// 
        /// </summary>
        private string m_title;
        /// <summary>
        /// 
        /// </summary>
        private EcellWebBrowser m_browser;
        /// <summary>
        /// 
        /// </summary>
        private ScriptEditor m_scriptEditor;
        /// <summary>
        /// Result of save confirm
        /// </summary>
        private enum ConfirmState
        {
            /// <summary>
            /// Save
            /// </summary>
            Yes,
            /// <summary>
            /// Not saved.
            /// </summary>
            No,
            /// <summary>
            /// Cancel to save Change.
            /// </summary>
            Canceled,
            /// <summary>
            /// Changed but not saved.
            /// </summary>
            NotSaved
        }
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
            InitializeComponent();
        }

        #endregion

        private string tmpPath;

        #region Initializer
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            setFilePath();
            CheckDefaultWindowSetting();
            
            // Load plugins
            LoadPlugins();
            SetRecentProject();
            // Create Windows
            m_statusDialog = new GridJobStatusDialog(m_env.JobManager);
            SetDockContent(m_statusDialog);
            m_scriptEditor = new ScriptEditor(m_env);
            m_scriptEditor.Text = MessageResources.NameScriptEditor;
            m_scriptEditor.Name = MessageResources.NameScriptEditor;
            SetDockContent(m_scriptEditor);
            // Load default window settings.
            LoadDefaultWindowSetting();
            LoadDefaultWindowSetting();

            SetStartUpWindow();
            m_browser.Activate();
            m_title = this.Text;
            m_env.ReportManager.StatusUpdated += new StatusUpdatedEventHandler(ReportManager_StatusUpdated);
            m_env.ReportManager.ProgressValueUpdated += new ProgressReportEventHandler(ReportManager_ProgressValueUpdated);
            m_env.ActionManager.UndoStatusChanged += new UndoStatusChangedEvent(ActionManager_UndoStatusChanged);
        }

        /// <summary>
        /// Set window setting file path.
        /// </summary>
        private void setFilePath()
        {
            m_userWindowSettingPath = Util.GetUserDir();
            m_userWindowSettingPath2 = Path.Combine(m_userWindowSettingPath, Constants.fileWinSetting + ".view");
            m_userWindowSettingPath = Path.Combine(m_userWindowSettingPath, Constants.fileWinSetting);
        }

        /// <summary>
        /// Load plugins.
        /// </summary>
        void LoadPlugins()
        {
            m_env.PluginManager.AppVersion = Assembly.GetExecutingAssembly().GetName().Version;

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
                                if (menu is GraphToolStripMenuItem)
                                {
                                    graphGroupToolStripMenuItem.DropDown.Items.Add(menu);
                                }
                                else
                                {
                                    this.menustrip.Items.AddRange(new ToolStripItem[] { menu });
                                }
                                continue;
                            }



                            while (menu.DropDownItems.Count > 0)
                            {
                                ToolStripItem[] tmp = this.menustrip.Items.Find(menu.Name, false);
                                ToolStripMenuItem menuItem = (ToolStripMenuItem)tmp[0];
                                ToolStripItem item = menu.DropDownItems[0];
                                IEnumerator iter = menuItem.DropDownItems.GetEnumerator();
                                int i = 0;

                                if (item is GraphToolStripMenuItem)
                                {
                                    graphGroupToolStripMenuItem.DropDown.Items.Add(item);
                                    continue;
                                }

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
                        else
                        {
                            menu.Enabled = true;
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
            if (CloseConfirm() == ConfirmState.Canceled)
                return;
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string filename = (string)item.Tag;
            try
            {
                m_env.DataManager.LoadProject(filename);
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
        private void SaveWindowSetting(string filename, bool isClosing)
        {
            //Save current window settings.
            try
            {
                Trace.WriteLine("Saving window settings: " + filename);
                DockWindowSerializer.SaveAsXML(this, filename, isClosing);
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
            return LoadWindowSetting(filename, false);
        }
        /// <summary>
        /// Load window settings.
        /// </summary>
        private bool LoadWindowSetting(string filename, bool initializeWindow)
        {
            try
            {
                if (File.Exists(filename))
                {
                    Trace.WriteLine("Loading window settings: " + filename);
                    DockWindowSerializer.LoadFromXML(this, filename, initializeWindow);
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
        /// Check default window settings.
        /// </summary>
        public void CheckDefaultWindowSetting()
        {
            // Load user window settings.
            if (File.Exists(m_userWindowSettingPath))
                return;

            // Load default window settings when failed.
            InitialPreferencesDialog win = new InitialPreferencesDialog(true);
            using (win)
            {
                if (win.ShowDialog() == DialogResult.OK)
                {
                    tmpPath = win.FilePath;
                }
                else
                {
                    string setting = Path.Combine(Util.GetWindowSettingDir(), "Base.xml");
                    tmpPath = setting;
                }
                Util.SetLanguage(win.Language);
                Util.InitialLanguage();
            }
        }

        /// <summary>
        /// Load default window settings.
        /// </summary>
        public void LoadDefaultWindowSetting()
        {
            // Load user window settings.
            if (LoadWindowSetting(m_userWindowSettingPath, true))
                return;

            // Load default window settings when failed.
            if (tmpPath != null && File.Exists(tmpPath))
                LoadWindowSetting(tmpPath);
        }
        #endregion

        /// <summary>
        /// Close the project.
        /// </summary>
        private void CloseProject()
        {
            try
            {
                m_env.DataManager.CloseProject();
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void SetStartUpWindow()
        {
            EcellWebBrowser content = new EcellWebBrowser(m_env, m_recentProjects);
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
            if (content.ContentType == DockContentType.GRAPH)
            {
                content.FormClosing += new FormClosingEventHandler(this.Graph_Closing);
            }
            else
            {
                content.FormClosing += new FormClosingEventHandler(this.DockContent_Closing);
            }

            //Create DockWindow Menu
            DockToolStripMenuItem item = new DockToolStripMenuItem(content);
            if (content.ContentType == DockContentType.NONE)
            {
                this.showWindowToolStripMenuItem.DropDown.Items.Add(item);
            }
            else if (content.ContentType == DockContentType.ANALYSIS)
            {
                this.analysisGroupToolStripMenuItem.DropDown.Items.Add(item);
            }
            else if (content.ContentType == DockContentType.GRAPH)
            {
                this.graphGroupToolStripMenuItem.DropDown.Items.Add(item);
            }

            m_dockWindowDic.Add(content.Name, content);
            content.ResumeLayout();
            content.Show(this.dockPanel, DockState.Document);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                e.Cancel = true;
            }
            else
            {
                ((Form)sender).Controls.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Graph_Closing(object sender, FormClosingEventArgs e)
        {
            DockContent cont = sender as DockContent;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                ToolStripItem[] items = graphGroupToolStripMenuItem.DropDownItems.Find(cont.Name, true);
                for (int i = 0; i < items.Length; i++)
                {
                    graphGroupToolStripMenuItem.DropDownItems.Remove(items[i]);
                }
            }
            else
            {
                ((Form)sender).Controls.Clear();
            }
        }

        #region PluginBase
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
            if (data.Type.Equals(Constants.xpathProject))
            {
                string projectID = m_env.DataManager.CurrentProjectID;
                this.Text = projectID + " - " + m_title;
                m_editCount = 0;
                return;
            }
            m_editCount++;
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="entry"></param>
        public void LoggerAdd(LoggerEntry entry)
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

        /// <summary>
        /// The event sequence when the simulation parameter is updated.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The updated parameter ID.</param>
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
            m_editCount = 0;
            m_statusDialog.Clear();
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
        public void ChangeStatus(ProjectStatus status)
        {
            bool unInitialized = status == ProjectStatus.Uninitialized;
            bool loaded = status == ProjectStatus.Loaded;
            bool suspend = status == ProjectStatus.Suspended;
            bool revision = false;
            if (m_env.DataManager.CurrentProject != null)
                revision = m_env.DataManager.CurrentProject.Info.ProjectType == ProjectType.Revision;

            // file menu.
            newProjectToolStripMenuItem.Enabled = unInitialized || loaded;
            openProjectToolStripMenuItem.Enabled = unInitialized || loaded;
            saveProjectToolStripMenuItem.Enabled = (suspend || loaded) && !revision;
            saveAsToolStripMenuItem.Enabled = (suspend || loaded);
            recentProejctToolStripMenuItem.Enabled = unInitialized || loaded;
            projectWizardMenuItem.Enabled = unInitialized || loaded;
            closeProjectToolStripMenuItem.Enabled = loaded;
            exportModelToolStripMenuItem.Enabled = (suspend || loaded);
            exportSBMLMenuItem.Enabled = (suspend || loaded);
            importModelToolStripMenuItem.Enabled = unInitialized || loaded;
            importScriptToolStripMenuItem.Enabled = loaded;
            saveScriptToolStripMenuItem.Enabled = loaded;
            printToolStripMenuItem.Enabled = !unInitialized;
            exitToolStripMenuItem.Enabled = true;
            // layout
            MenuItemLayout.Enabled = loaded;
            importSBMLMenuItem.Enabled = unInitialized || loaded;

            // Button.
            toolStripOpenProjectButton.Enabled = unInitialized || loaded;
            toolStripSaveProjectButton.Enabled = (suspend || loaded) && !revision;
            toolStripSaveAsButton.Enabled = (suspend || loaded);

            m_scriptEditor.ChangeStatus(status);            

            // Reset edit count.
            if (unInitialized || (m_status == ProjectStatus.Loading && loaded))
                m_editCount = 0;
            // Set recent Project.
            if (loaded)
            {
                string projectID = m_env.DataManager.CurrentProjectID;
                this.Text = projectID + " - " + m_title;
                ProjectInfo info = m_env.DataManager.CurrentProject.Info;
                CheckAndReplaceRecentProject(info);
                ResetRecentProjectMenu();                
            }
            else if (unInitialized)
            {
                this.Text = m_title;
            }

            // Set Window Setting.
            if (m_status == ProjectStatus.Loaded && (status == ProjectStatus.Running || status == ProjectStatus.Stepping))
            {
                SaveWindowSetting(m_userWindowSettingPath, true);
                LoadWindowSetting(m_userWindowSettingPath2);
            }
            else if (status == ProjectStatus.Loaded && (m_status == ProjectStatus.Suspended || m_status == ProjectStatus.Running || m_status == ProjectStatus.Stepping))
            {
                SaveWindowSetting(m_userWindowSettingPath2, true);
                LoadWindowSetting(m_userWindowSettingPath);
            }

            // Set Status.
            m_status = status;
            ChangeUndoStatus(m_env.ActionManager.UndoStatus);
        }

        /// <summary>
        /// Replace the recent project,
        /// </summary>
        /// <param name="info">the recent project.</param>
        private void CheckAndReplaceRecentProject(ProjectInfo info)
        {
            if (info.ProjectType == ProjectType.Template)
                return;
            KeyValuePair<string, string> oldProject = new KeyValuePair<string,string>();
            foreach (KeyValuePair<string, string> project in m_recentProjects)
            {
                if (project.Key.Equals(info.Name))
                    oldProject = project;
            }
            if (oldProject.Key != null)
                m_recentProjects.Remove(oldProject);
            if (File.Exists(info.ProjectFile))
                m_recentProjects.Add(new KeyValuePair<string, string>(info.Name, info.ProjectFile));
            ResetRecentProjectMenu();
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
        /// Get the list of property shown in Common Setting Dialog.
        /// </summary>
        /// <returns>the list of IPropertyItem.</returns>
        public List<IPropertyItem> GetPropertySettings()
        {
            PropertyNode node1 = new PropertyNode(MessageResources.NameJobManage);            
            node1.Nodes.Add(new PropertyNode(new JobManagerSettingPage(m_env.JobManager)));

            PropertyNode node2 = new PropertyNode(MessageResources.NameGeneral);
            node2.Nodes.Add(new PropertyNode(new GeneralConfigurationPage(m_env.DataManager)));
            node2.Nodes.Add(new PropertyNode(new LanguageSettingPage()));
            node2.Nodes.Add(new PropertyNode(new RootFolderSettingPage()));

            List<IPropertyItem> nodeList = new List<IPropertyItem>();
            nodeList.Add(node1);
            nodeList.Add(node2);

            return nodeList;
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
        /// Get the list of public delegation function.
        /// </summary>
        /// <returns>the dictionary of name and delegation function</returns>
        public Dictionary<string, Delegate> GetPublicDelegate()
        {
            Dictionary<string, Delegate> list = new Dictionary<string, Delegate>();
            list.Add(Constants.delegateSetDockContents, new SetDockContentDelegate( this.SetDockContent));
            list.Add(Constants.delegateShowGridDialog, new ShowDialogDelegate(this.ShowGridStatusDialog));
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
            if (CloseConfirm() == ConfirmState.Canceled)
                return;
            m_env.DataManager.CreateNewProject(Util.GetNewProjectName(), "");
        }

        /// <summary>
        /// Save confirm.
        /// </summary>
        /// <returns>
        /// When the user select...
        /// Saving, return Yes.
        /// Not Saving, returns NotSaved.
        /// Cancel, returns Canceled.
        /// and when there is no change, returns No.
        /// </returns>
        private ConfirmState CloseConfirm()
        {
            ConfirmState state = ConfirmState.No;
            List<string> runGroupList = GetRunningAnalysisCount();
            if (runGroupList.Count > 0)
            {
                string mes = "\n" + runGroupList[0];
                for (int i = 1; i < runGroupList.Count; i++)
                    mes = mes + "\n" + runGroupList[i];
                if (!Util.ShowYesNoDialog(string.Format(MessageResources.ConfirmAnalysisStop, mes)))
                {
                    state = ConfirmState.Canceled;
                    return state;
                }
                foreach (string name in runGroupList)
                {
                    if (m_env.JobManager.GroupDic.ContainsKey(name))
                        m_env.JobManager.Stop(name, 0);
                }
            }
            // No change
            Project project = m_env.DataManager.CurrentProject;
            if (project == null || (m_editCount == 0 && project.Info.ProjectType == ProjectType.Project))
                return state;

            // Confirm saving.
            try
            {
                // Save if answer is yes.
                if (Util.ShowYesNoCancelDialog(MessageResources.SaveConfirm, MessageBoxDefaultButton.Button3))
                {
                    SaveProject();
                    state = ConfirmState.Yes;
                }
                else
                    state = ConfirmState.NotSaved;
            }
            catch (Exception)
            {
                // Return canceled
                state = ConfirmState.Canceled;
            }

            return state;
        }

        /// <summary>
        /// The action of [open project] menu click.
        /// Show OpenProjectDialog and select the target project.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void LoadProjectMenuClick(object sender, EventArgs e)
        {

            ProjectExplorerDialog ped = new ProjectExplorerDialog(Util.GetBaseDir());
            using (ped)
            {
                if (ped.ShowDialog() != DialogResult.OK)
                    return;
                // Check the modification and confirm save.
                if (CloseConfirm() == ConfirmState.Canceled)
                    return;
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
        /// Event when setup IDE button is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void LoadProjectWizardMenuClick(object sender, EventArgs e)
        {

            ProjectWizardWindow win = new ProjectWizardWindow();
            if (win.ShowDialog() == DialogResult.OK && win.SelectedProject != null)
            {
                // Check the modification and confirm save.
                if (CloseConfirm() == ConfirmState.Canceled)
                    return;
                ProjectInfo info = win.SelectedProject;
                info.ProjectType = ProjectType.Template;
                info.DMDirList.AddRange(win.DMList);
                try
                {
                    m_env.DataManager.LoadProject(info);
                    m_env.DataManager.CurrentProject.Info.ProjectPath = null;
                }
                catch (Exception)
                {
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
            try
            {
                Project project = m_env.DataManager.CurrentProject;
                if (!ConfirmOverwrite(project))
                    return;

                m_env.DataManager.SaveProject();
                CheckAndReplaceRecentProject(project.Info);
                m_editCount = 0;
            }
            catch (Util.CancelException)
            {
                return;
            }
            catch (EcellException e)
            {
                if(e.InnerException is UnauthorizedAccessException)
                    Util.ShowErrorDialog(string.Format(MessageResources.ErrUnauthorizedAccess, Util.GetBaseDir()));
                else
                    Util.ShowErrorDialog(e.Message);
            }
        }

        /// <summary>
        /// Show the dialog to confirm to override the project.
        /// </summary>
        /// <param name="project">the override project.</param>
        /// <returns>if ok is true.</returns>
        private bool ConfirmOverwrite(Project project)
        {
            if(!Util.IsExistProject(project.Info.Name))
                return true;

            string msg = MessageResources.ConfirmOverwrite;

            // In case new project.
            if(!Util.IsExistProject(project.Info.Name))
                return true;

            // If Already exists.
            if (project.Info.ProjectType != ProjectType.Project)
            {
                msg = string.Format(MessageResources.ErrExistProject, project.Info.Name)
                        + "\n" + MessageResources.ConfirmOverwrite;
                return Util.ShowOKCancelDialog(msg);
            }
            else if(Path.GetFileName(project.Info.ProjectPath) != project.Info.Name)
            {
                return Util.ShowOKCancelDialog(msg);
            }
            return true;
        }

        /// <summary>
        /// Click ToolStripMenuItem to save as ...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Å@Get current project.
            ProjectInfo info = m_env.DataManager.CurrentProject.Info;
            string oldName = info.Name;

            // Show project dialog.
            ProjectSettingDialog dialog = new ProjectSettingDialog(info);
            dialog.Text = MessageResources.DialogTitleSaveAs;
            using (dialog)
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                m_env.JobManager.Clear();

                if (!ConfirmOverwrite(m_env.DataManager.CurrentProject))
                    return;

                // Save as new project.
                m_env.DataManager.SaveProject();
            }
        }
        /// <summary>
        /// The action of [close projct] menu click.
        /// Show confirm dialog. if you select yes, system show SaveProjectDialog.
        /// </summary>
        /// <param name="sender">obeject(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void CloseProjectMenuClick(object sender, EventArgs e)
        {
            if (CloseConfirm() == ConfirmState.Canceled)
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
            // Show OpenFileDialog.
            try
            {
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = Constants.FilterEmlFile;
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                // Check current project and save it.
                if (CloseConfirm() == ConfirmState.Canceled)
                    return;
                // Close project
                if (!string.IsNullOrEmpty(m_env.DataManager.CurrentProjectID))
                    CloseProject();
                // Load new project.
                string filepath = openFileDialog.FileName;
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
                saveFileDialog.FileName = "";
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

        private void exportSBMLMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog.FileName = "";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Filter = Constants.FilterSBMLFile;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    m_env.DataManager.ExportSBML(saveFileDialog.FileName);
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
                saveFileDialog.FileName = "";
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
            if (result == DialogResult.OK)
            {
                if (d.SelectedItem.Plugin.IsDirect())
                {
                    d.SelectedItem.Plugin.Print(d.SelectedItem.Portion);
                    d.Dispose();
                    return;
                }
                PrintDocument pd = new PrintDocument();
                PrintDialog pdlg = new PrintDialog();
                pdlg.Document = pd;
                pdlg.AllowSomePages = true;
                if (pdlg.ShowDialog() == DialogResult.OK)
                {
                    pd.PrintPage += delegate(object o, PrintPageEventArgs pe)
                    {
                        Bitmap bmp = new Bitmap(d.SelectedItem.Plugin.Print(d.SelectedItem.Portion), pe.PageBounds.Width, pe.PageBounds.Height);
                        pe.Graphics.DrawImage(bmp, new Point(0, 0));
                        bmp.Dispose();
                    };
                    pd.Print();
                }
                pd.Dispose();
            }
            d.Dispose();
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
                try
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
                catch (Exception)
                {
                    Util.ShowErrorDialog(string.Format(MessageResources.ErrLoadFile,
                        new object[] { openScriptDialog.FileName }));
                }
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
            m_env.ActionManager.UndoAction();
        }

        /// <summary>
        /// The action of clicking the redo action menu.
        /// Redo action.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void RedoMenuClick(object sender, EventArgs e)
        {
            m_env.ActionManager.RedoAction();
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

        /// <summary>
        /// Click ToolStripMenuItem to display the about E-Cell IDE.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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

        /// <summary>
        /// Activate the grid status dialog.
        /// </summary>
        private void ShowGridStatusDialog()
        {
            m_statusDialog.Activate();
        }

        /// <summary>
        /// Event when Save Window setting menu is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void SaveWindowSettingsClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.InitialDirectory = System.Environment.GetFolderPath( System.Environment.SpecialFolder.DesktopDirectory);
            sfd.Filter = Constants.FilterWinSetFile;
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save window settings.
                SaveWindowSetting(sfd.FileName, false);
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
            //ofd.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
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
        /// Event when Settings menu is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void SettingsMenuClick(object sender, EventArgs e)
        {
            PropertyDialog dialog = new PropertyDialog(m_env.PluginManager.GetPropertySettings());
            using (dialog)
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                dialog.ApplyChanges();
            }

        }

        private List<string> GetRunningAnalysisCount()
        {
            List<string> result = new List<string>();
            foreach (string name in m_env.JobManager.GroupDic.Keys)
            {
                if (m_env.JobManager.GroupDic[name].Status == AnalysisStatus.Running ||
                    m_env.JobManager.GroupDic[name].Status == AnalysisStatus.Waiting)
                    result.Add(name);
            }
            return result;
        }

        /// <summary>
        /// Event when this form is closed.
        /// </summary>
        /// <param name="sender">this form.</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void MainWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_status == ProjectStatus.Running ||
                m_status == ProjectStatus.Stepping ||
                m_status == ProjectStatus.Suspended)
            {
                if (!Util.ShowYesNoDialog(MessageResources.ConfirmClose))
                {
                    e.Cancel = true;
                    return;
                }
                m_env.DataManager.SimulationStop();
                Thread.Sleep(1000);
            }

            // Confirm saving
            ConfirmState state = CloseConfirm();
            if (state == ConfirmState.Canceled)
            {
                e.Cancel = true;
                return;
            }
            else if (state == ConfirmState.NotSaved && !Util.ShowOKCancelDialog(MessageResources.ConfirmUnsaved))
            {
                e.Cancel = true;
                return;
            }

            SaveRecentProject();
            SaveWindowSetting(m_userWindowSettingPath, true);
            m_browser.EcellWebBrowserClear();

            base.OnClosing(e);
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

        /// <summary>
        /// Event when the status of ReportManager is updated.
        /// </summary>
        /// <param name="o">ReportManager.</param>
        /// <param name="e">StatusUpdateEventArgs</param>
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
        /// Event when Undo status is changed.
        /// </summary>
        /// <param name="sender">ActionManager.</param>
        /// <param name="e">UndoStatusChangedEventArgs</param>
        void ActionManager_UndoStatusChanged(object sender, UndoStatusChangedEventArgs e)
        {
            ChangeUndoStatus(e.Status);
        }

        /// <summary>
        /// Change availability of undo/redo function
        /// </summary>
        /// <param name="status">UndoStatus.</param>
        private void ChangeUndoStatus(UndoStatus status)
        {
            bool prjStatus = m_status == ProjectStatus.Loaded;
            bool isRunning = m_env.ActionManager.IsLoadAction;
            bool undoEnabled = (status == UndoStatus.UNDO_ONLY || status == UndoStatus.UNDO_REDO) && prjStatus && !isRunning;
            bool redoEnabled = (status == UndoStatus.REDO_ONLY || status == UndoStatus.UNDO_REDO) && prjStatus && !isRunning;
            undoToolStripMenuItem.Enabled = undoEnabled;
            redoToolStripMenuItem.Enabled = redoEnabled;

            toolStripUndoButton.Enabled = undoEnabled;
            toolStripRedoButton.Enabled = redoEnabled;

        }

        /// <summary>
        /// Event to change the value of progress bar.
        /// </summary>
        private void ReportManager_ProgressValueUpdated(object o, ProgressReportEventArgs e)
        {
            genericProgressBar.Value = (e.Value == 100 ? 0 : e.Value);
        }

        /// <summary>
        /// Click ToolStripMenuItem to import SBML,
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ImportSBMLMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = Constants.FilterSBMLFile;
            using (dialog)
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                if (CloseConfirm() == ConfirmState.Canceled)
                    return;
                string filename = dialog.FileName;
                try
                {
                    m_env.DataManager.LoadSBML(filename);
                    //LoadSBML(dialog.FileName);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.StackTrace);
                    Util.ShowErrorDialog(string.Format( MessageResources.ErrLoadFile, filename));
                }
            }
        }

        /// <summary>
        /// Get the ToolStripMenuItem from menu name.
        /// </summary>
        /// <param name="name">the menu name.</param>
        /// <returns> the ToolStripMenuItem</returns>
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

        /// <summary>
        /// Click ToolStripMenuItem to display home page of feedback.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_browser.Url = new Uri("http://chaperone.e-cell.org/services/feedback/");
        }


        #region IEcellPlugin ÉÅÉìÉo

        /// <summary>
        /// Get the information of Plugin.
        /// </summary>
        /// <returns>the information of Plugin.</returns>
        public XmlNode GetPluginStatus()
        {
            return null;
        }

        /// <summary>
        /// Set the information of Plugin.
        /// </summary>
        /// <param name="status">the information of Plugin.</param>
        public void SetPluginStatus(XmlNode nstatus)
        {
            ;
        }

        #endregion
    }
}
