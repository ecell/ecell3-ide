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
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Threading;
using System.Reflection;
using System.Xml.Serialization;
using IronPython.Hosting;
using IronPython.Runtime;

using EcellLib;
using EcellLib.Message;
using EcellLib.Plugin;
using WeifenLuo.WinFormsUI.Docking;
using EcellLib.Objects;
using System.Xml;

namespace EcellLib.MainWindow
{
    public partial class MainWindow : Form, IEcellPlugin, IDockOwner
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
        /// m_newPrjDialog (NewProjectDialog)
        /// </summary>
        private NewProjectDialog m_newPrjDialog;
        /// <summary>
        /// m_openPrjDialog (OpenProjectDialog)
        /// </summary>
        private ManageProjectDialog m_managePrjDialog;
        /// <summary>
        /// m_savePrjDialog (SaveProjectDialog)
        /// </summary>
        private SaveProjectDialog m_savePrjDialog;
        /// <summary>
        /// flag which load model already.
        /// </summary>
        private bool m_isLoadProject;
        /// <summary>
        /// base directory of loading project.
        /// </summary>
        private string m_currentDir;
        /// <summary>
        /// loading project.
        /// </summary>
        private string m_project = null;
        /// <summary>
        /// delegate function of loading model.
        /// </summary>
        public delegate void LoadModelDelegate(string modelID);
        /// <summary>
        /// delegate function of close project.
        /// </summary>
        public delegate void CloseProjectDelegate(string projectID);
        /// <summary>
        /// The flag whether close the project.
        /// </summary>
        private bool m_isClose = false;
        /// <summary>
        /// System status.
        /// </summary>
        private ProjectStatus m_type = ProjectStatus.Uninitialized;
        /// <summary>
        /// The number of edit after project is opened.
        /// </summary>
        private int m_editCount = 0;
        /// <summary>
        /// List of plugin to check loaded plugin.
        /// </summary>
        public List<string> m_pluginList;
        /// <summary>
        /// Docking Windows object.
        /// </summary>
        public WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        /// <summary>
        /// RecentProjects
        /// </summary>
        private Dictionary<string, string> m_recentProjects = new Dictionary<string, string>();
        #endregion

        #region Accessor
        /// <summary>
        /// RecentProjects
        /// </summary>
        public Dictionary<string, string> RecentProjects
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
            // Load plugins
            LoadPlugins();
            //Load default window settings.
            setFilePath();
            LoadRecentProject();
            LoadDefaultWindowSetting();
            SetStartUpWindow();
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

        /// <summary>
        /// Load plugins.
        /// </summary>
        void LoadPlugins()
        {
            m_env.PluginManager.AppVersion = Assembly.GetExecutingAssembly().GetName().Version;
            m_env.PluginManager.CopyRight = global::EcellLib.MainWindow.Properties.Resources.CopyrightNotice;

            m_pluginList = new List<string>();
            m_isLoadProject = false;

            m_currentDir = Util.GetBaseDir();
            if (m_currentDir == null)
            {
                m_currentDir =
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                m_currentDir = m_currentDir + "\\e-cell\\project";
            }
            // Load plugins
            foreach (string pluginDir in Util.GetPluginDirs())
            {
                string[] files = Directory.GetFiles(
                    pluginDir,
                    Constants.delimiterWildcard + Constants.FileExtPlugin);
                foreach (string fileName in files)
                {
                    LoadPlugin(fileName);
                }
            }
            m_env.PluginManager.ChangeStatus(ProjectStatus.Uninitialized);

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

        /// <summary>
        /// Load plugin in plugin directory and add the plugin menus to MainWindow.
        /// </summary>
        /// <param name="path">path of plugin.</param>
        private void LoadPlugin(string path)
        {
            IEcellPlugin pb = null;
            string pName = Path.GetFileNameWithoutExtension(path);
            string className = "EcellLib." + pName + "." + pName;

            if (m_pluginList.Contains(pName)) return;
            m_pluginList.Add(pName);

            try
            {
                pb = m_env.PluginManager.LoadPlugin(path, className);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                String errmes = MessageResMain.ErrLoadPlugin;
                m_env.MessageManager.Append(
                    new ApplicationMessageEntry(
                        MessageType.Error,
                        String.Format(errmes, className, path), this));
                return;
            }
            // Set DockContent.
            IEnumerable<EcellDockContent> winList = pb.GetWindowsForms();
            if (winList != null)
            {
                foreach (EcellDockContent dock in winList)
                {
                    SetDockContent(dock);
                }
            }

            // Set Menu.
            List<ToolStripMenuItem> menuList = pb.GetMenuStripItems();
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
            // Set ToolBar
            ToolStrip toolList = pb.GetToolBarMenuStrip();
            if (toolList != null)
            {
                this.toolStripContainer.TopToolStripPanel.Join(toolList,toolList.Location);
            }
        }

        #endregion

        private void LoadRecentProject()
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
                    m_recentProjects.Add(name, file);
                }
                ResetRecentProject();
            }
            catch (Exception)
            {
            }
        }

        private void ResetRecentProject()
        {
            openProjectToolStripMenuItem.DropDownItems.Clear();
            foreach (KeyValuePair<string, string> project in m_recentProjects)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(project.Key);
                item.ToolTipText = project.Value;
                item.Click += new EventHandler(RecentProject_Click);
                openProjectToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// Event on RecentProject_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RecentProject_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            LoadProject(item.Text, item.ToolTipText);
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
                Util.ShowErrorDialog(MessageResMain.ErrSaveWindowSettings);
            }
        }

        /// <summary>
        /// Load window settings.
        /// </summary>
        private bool loadWindowSetting(string filename)
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
                Util.ShowErrorDialog(MessageResMain.ErrLoadWindowSettings);

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
            if (!loadWindowSetting(m_userWindowSettingPath))
            {
                
                SelectWinSettingWindow win = new SelectWinSettingWindow();
                try
                {
                    string filePath = win.ShowWindow(true);
                    if (filePath != null)
                    {
                        loadWindowSetting(filePath);
                        return;
                    }
                }
                catch (IgnoreException ex)
                {
                    ex.ToString();
                }
                finally
                {
                    loadWindowSetting(m_defaultWindowSettingPath);
                }
            }
        }
        #endregion

        /// <summary>
        /// Load model in the thread, if this thread is sub thread.
        /// </summary>
        private void LoadModelData()
        {
            Util.InitialLanguage();
            try
            {
                string modelID = m_env.DataManager.LoadModel(m_openFileDialog.FileName, true);
                if (this.InvokeRequired)
                {
                    LoadModelDelegate dlg = new LoadModelDelegate(LoadModelThread);
                    this.Invoke(dlg, new object[] { modelID });
                }
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
                CloseProjectDelegate dlg = new CloseProjectDelegate(CloseProject);
                this.Invoke(dlg, new object[] { m_project });
            }
        }

        /// <summary>
        /// Create the project.
        /// </summary>
        /// <param name="l_prjID">Project ID</param>
        /// <param name="l_modelDir">Directory of model.</param>
        /// <param name="l_comment">Comment</param>
        /// <param name="l_dmList">The list of dm directory.</param>
        private void CreateProject(string l_prjID, string l_modelDir, string l_comment, List<string> l_dmList)
        {
            m_env.DataManager.CreateProject(l_prjID, l_comment, l_modelDir, l_dmList);
            m_project = l_prjID;
            m_isLoadProject = true;
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
            m_editCount = 0;
        }

        /// <summary>
        /// Close the project.
        /// </summary>
        /// <param name="l_prjID"></param>
        private void CloseProject(String l_prjID)
        {
            m_isLoadProject = false;
            m_env.PluginManager.ChangeStatus(ProjectStatus.Uninitialized);
            m_env.DataManager.CloseProject(l_prjID);
            m_project = null;
            m_editCount = 0;
        }

        private bool CheckProjectID(string l_prjID)
        {
            if (String.IsNullOrEmpty(l_prjID))
            {
                Util.ShowWarningDialog(String.Format(MessageResMain.ErrNoSet,
                    new object[] { "Project ID" }));
                return false;
            }
            if (Util.IsNGforIDonWindows(l_prjID) || l_prjID.Length > 64)
            {
                Util.ShowWarningDialog(String.Format(MessageResMain.ErrIDNG,
                    new object[] { "Project ID" }));
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_modelID"></param>
        /// <returns></returns>
        private bool CheckModelID(string l_modelID)
        {
            if (String.IsNullOrEmpty(l_modelID))
            {
                Util.ShowWarningDialog(String.Format(MessageResMain.ErrNoSet,
                    new object[] { "Model ID" }));
                return false;
            }
            if (l_modelID.Length > 64 || Util.IsNGforIDonWindows(l_modelID))
            {
                Util.ShowWarningDialog(String.Format(MessageResMain.ErrIDNG,
                    new object[] { "Model ID" }));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load model by DataManager.
        /// </summary>
        public void LoadModelThread(string modelID)
        {
            m_env.PluginManager.LoadData(modelID);
            m_editCount = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        internal void SetStartUpWindow()
        {
            EcellDockContent content = new StartUpWindow(this);
            content.Name = "StartUpWindow";
            content.Text = "StartUpWindow";
            content.DockHandler.DockPanel = this.dockPanel;
            content.Show(this.dockPanel, DockState.Document);
        }

        /// <summary>
        /// set DockContent
        /// </summary>
        private void SetDockContent(EcellDockContent content)
        {
            Trace.WriteLine("Create dock: " + content.Text);
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
            Trace.WriteLine("Closing dock: " + ((DockContent)sender).Name + " - " + e.CloseReason);
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
            // nothing
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            // nothing
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence to display the message.
        /// </summary>
        /// <param name="message">the message entry object.</param>
        public virtual void Message2(IMessageEntry message)
        {
            // nothing.
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
                closeProjectToolStripMenuItem.Enabled = false;
                exportModelToolStripMenuItem.Enabled = false;
                importModelToolStripMenuItem.Enabled = true;
                importScriptToolStripMenuItem.Enabled = true;
                saveScriptToolStripMenuItem.Enabled = false;
                printToolStripMenuItem.Enabled = false;
                saveActionMenuItem.Enabled = false;
                exitToolStripMenuItem.Enabled = true;
            }
            else if (type == ProjectStatus.Loaded || type == ProjectStatus.Stepping)
            {
                newProjectToolStripMenuItem.Enabled = true;
                openProjectToolStripMenuItem.Enabled = true;
                saveProjectToolStripMenuItem.Enabled = true;
                closeProjectToolStripMenuItem.Enabled = true;
                exportModelToolStripMenuItem.Enabled = true;
                importModelToolStripMenuItem.Enabled = true;
                importScriptToolStripMenuItem.Enabled = false;
                saveScriptToolStripMenuItem.Enabled = true;
                printToolStripMenuItem.Enabled = true;
                saveActionMenuItem.Enabled = true;
                exitToolStripMenuItem.Enabled = true;
            }
            else
            {
                newProjectToolStripMenuItem.Enabled = false;
                openProjectToolStripMenuItem.Enabled = false;
                saveProjectToolStripMenuItem.Enabled = false;
                closeProjectToolStripMenuItem.Enabled = false;
                exportModelToolStripMenuItem.Enabled = false;
                importModelToolStripMenuItem.Enabled = false;
                saveScriptToolStripMenuItem.Enabled = false;
                importScriptToolStripMenuItem.Enabled = false;
                saveScriptToolStripMenuItem.Enabled = false;
                printToolStripMenuItem.Enabled = true;
                saveActionMenuItem.Enabled = false;
                exitToolStripMenuItem.Enabled = true;
            }
            m_type = type;
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
        public List<string> GetEnablePrintNames()
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
            if (m_editCount > 0)
            {
                try
                {
                    if (Util.ShowYesNoCancelDialog(MessageResMain.SaveConfirm))
                    {
                        m_isClose = true;
                        SaveProjectMenuClick(sender, e);
                        m_editCount = 0;
                    }
                    else
                    {
                        CloseProject(m_project);
                    }
                }
                catch (Util.CancelException)
                {
                    return;
                }
            }
            m_newPrjDialog = new NewProjectDialog();
            if (m_newPrjDialog.ShowDialog() == DialogResult.OK)
                CreateNewProject();
            else
                CancelNewProject();

        }

        /// <summary>
        /// The action when you click OK or Cancel in NewProjectDialog.
        /// If you don't set name or model name, system show warning message.
        /// </summary>
        private void CreateNewProject()
        {
            if (!CheckProjectID(m_newPrjDialog.textName.Text))
                return;
            if (!CheckModelID(m_newPrjDialog.textModelName.Text))
                return;

            try
            {
                CreateProject(m_newPrjDialog.textName.Text,
                    null, m_newPrjDialog.textComment.Text,
                    m_newPrjDialog.GetDmList());
                List<EcellObject> list = new List<EcellObject>();
                list.Add(EcellObject.CreateObject(m_newPrjDialog.textModelName.Text, null, Constants.xpathModel, null, null));
                m_env.DataManager.DataAdd(list);
                foreach (string paramID in m_env.DataManager.GetSimulationParameterIDs())
                {
                    m_env.PluginManager.ParameterAdd(m_newPrjDialog.textName.Text, paramID);
                }
                m_env.PluginManager.ParameterSet(m_env.DataManager.CurrentProjectID, m_env.DataManager.GetCurrentSimulationParameterID());
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
                CloseProject(m_newPrjDialog.textName.Text);
            }
            CancelNewProject();
        }

        /// <summary>
        /// Cancel to create project.
        /// </summary>
        private void CancelNewProject()
        {
            m_newPrjDialog.Close();
            m_newPrjDialog.Dispose();
            m_newPrjDialog = null;
        }

        /// <summary>
        /// The action of [open project] menu click.
        /// Show OpenProjectDialog and select the target project.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void OpenProjectMenuClick(object sender, EventArgs e)
        {
            // Check the modification and confirm save.
            if (m_editCount > 0)
            {
                if (Util.ShowYesNoDialog(MessageResMain.SaveConfirm))
                {
                    m_isClose = true;
                    SaveProjectMenuClick(sender, e);
                    m_editCount = 0;
                }
            }

            m_managePrjDialog = new ManageProjectDialog();
            m_managePrjDialog.CreateProjectTreeView(null, m_currentDir);
            if (m_managePrjDialog.ShowDialog() == DialogResult.OK)
            {
                // Close current project.
                if (m_project != null)
                    CloseProject(m_project);
                // Load new project.
                OpenProject();
            }
            else
            {
                CloseOpenProjectDialog();
            }
        }

        /// <summary>
        /// Cancel to open project.
        /// </summary>
        private void CloseOpenProjectDialog()
        {
            m_managePrjDialog.Close();
            m_managePrjDialog.Dispose();
            m_managePrjDialog = null;
        }

        /// <summary>
        /// The action when you click OK or Cancel in OpenProjectDialog.
        /// If you don't select the project, system show warning message.
        /// </summary>
        private void OpenProject()
        {
            try
            {
                string prjID = m_managePrjDialog.MPPrjIDText.Text;
                string comment = m_managePrjDialog.MPPrjCommentText.Text;
                string fileName = m_managePrjDialog.FileName;

                if (!CheckProjectID(prjID))
                    return;

                if (fileName.EndsWith("eml"))
                {
                    string modelDir = Path.GetDirectoryName(fileName);
                    if (modelDir.EndsWith(Constants.xpathModel))
                    {
                        modelDir = modelDir.Substring(0, modelDir.Length - 5);
                    }
                    CreateProject(prjID, modelDir, comment, new List<string>());
                    LoadModel(fileName);

                    CloseOpenProjectDialog();
                    return;
                }
                Project project = m_managePrjDialog.Project;
                if (!project.Name.Equals(prjID) || !project.Comment.Equals(comment))
                {
                    project.Name = prjID;
                    project.Comment = comment;
                    project.Save(m_managePrjDialog.FileName);
                }
                CloseOpenProjectDialog();
                LoadProject(prjID, fileName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
            }            
        }

        /// <summary>
        /// Load Project.
        /// </summary>
        /// <param name="prjID"></param>
        /// <param name="fileName"></param>
        public void LoadProject(string prjID, string fileName)
        {
            if (m_project != null)
                CloseProject(m_project);

            
            string prjDir = Path.GetDirectoryName(fileName);
            string dmDir = Path.Combine(prjDir, Constants.DMDirName);
            string tmpDir = Path.Combine(dmDir, Constants.TmpDirName);
            Dictionary<string, string> fileDic = new Dictionary<string, string>();
            if (Directory.Exists(tmpDir))
            {
                Util.CopyDirectory(tmpDir, dmDir);
                Directory.Delete(tmpDir, true);
            }

            m_env.DataManager.LoadProject(prjID, fileName);
            m_isLoadProject = true;
            m_project = prjID;
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
            m_editCount = 0;
            // Set recent Project.
            if (m_recentProjects.ContainsKey(prjID))
                m_recentProjects.Remove(prjID);
            m_recentProjects.Add(prjID, fileName);
            ResetRecentProject();
        }

        /// <summary>
        /// The action of [save project] menu click.
        /// Show SaveProjectDialog and select the save instance.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void SaveProjectMenuClick(object sender, EventArgs e)
        {
            SetUpSaveProjectDialog();
            if (m_savePrjDialog.ShowDialog() == DialogResult.OK)
            {
                SaveProject();
            }
            CloseSaveProjectDialog();
        }

        private void SetUpSaveProjectDialog()
        {
            m_savePrjDialog = new SaveProjectDialog();
            CheckedListBox box = m_savePrjDialog.CheckedListBox;
            List<string> list = m_env.DataManager.GetSavableModel();
            if (list != null)
            {
                foreach (string s in list)
                {
                    box.Items.Add(s + " : [Model]");
                }
            }

            list = m_env.DataManager.GetSavableSimulationParameter();
            if (list != null)
            {
                foreach (string s in list)
                {
                    box.Items.Add(s + " : [SimulationParameter]");
                }
            }

            String res = m_env.DataManager.GetSavableSimulationResult();
            if (res != null)
            {
                box.Items.Add(res + " : [SimulationResult]");
            }

            int i = 0, count = box.Items.Count;
            box.CheckOnClick = true;

            for (i = 0; i < count; i++)
            {
                box.SetItemChecked(i, true);
            }
        }

        /// <summary>
        /// Event when to save the project is canceled.
        /// </summary>
        private void CloseSaveProjectDialog()
        {
            m_savePrjDialog.Close();
            m_savePrjDialog.Dispose();
            m_savePrjDialog = null;
            if (m_isClose)
            {
                CloseProject(m_project);
                m_isClose = false;
            }
        }

        /// <summary>
        /// The action when you click OK or Cancel in SaveProjectDialog.
        /// If you don't select one instance, system do nothing.
        /// </summary>
        private void SaveProject()
        {
            try
            {
                CheckedListBox box = m_savePrjDialog.CheckedListBox;
                if (box.CheckedItems.Count <= 0)
                {
                    Util.ShowWarningDialog(MessageResMain.ErrNoSelect);
                    return;
                }
                foreach (string s in box.CheckedItems)
                {
                    if (s.EndsWith(" : [Model]"))
                    {
                        int end = s.LastIndexOf(" : [Model]");
                        string p = s.Substring(0, end);
                        m_env.DataManager.SaveModel(p);
                    }
                    else if (s.EndsWith(" : [SimulationParameter]"))
                    {
                        int end = s.LastIndexOf(" : [SimulationParameter]");
                        string p = s.Substring(0, end);
                        m_env.DataManager.SaveSimulationParameter(p);
                    }
                    else if (s.EndsWith(" : [SimulationResult]"))
                    {
                        m_env.DataManager.SaveSimulationResult();
                    }
                }
                m_editCount = 0;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
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
            if (m_editCount > 0)
            {
                try
                {
                    if (Util.ShowYesNoCancelDialog(MessageResMain.SaveConfirm))
                    {
                        m_isClose = true;
                        SaveProjectMenuClick(sender, e);
                        m_editCount = 0;
                        CloseProject(m_project);
                    }
                    else
                    {
                        CloseProject(m_project);
                    }
                }
                catch (Util.CancelException)
                {
                    return;
                }
            }
            else
            {
                CloseProject(m_project);
            }
        }

        /// <summary>
        /// The action of [import model] menu click.
        /// Show OpenFileDialog and select the sbml file.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ImportModelMenuClick(object sender, EventArgs e)
        {
            if (m_editCount > 0)
            {
                if (Util.ShowYesNoDialog(MessageResMain.SaveConfirm))
                {
                    m_isClose = true;
                    SaveProjectMenuClick(sender, e);
                    m_editCount = 0;
                }
            }
            // Show OpenFileDialog.
            try
            {
                m_openFileDialog.RestoreDirectory = true;
                m_openFileDialog.Filter = Constants.FilterEmlFile;
                if (m_openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Close old project.
                    if (m_project != null)
                        CloseProject(m_project);
                    // Load new project.
                    if (m_isLoadProject == false)
                    {
                        string filepath = m_openFileDialog.FileName;
                        string modelDir = Path.GetDirectoryName(filepath);
                        string modelName = Path.GetFileNameWithoutExtension(filepath);
                        if (modelDir.EndsWith(Constants.xpathModel))
                        {
                            modelDir = modelDir.Substring(0, modelDir.Length - 6);
                        }
                        string dmDir = modelDir + Constants.delimiterPath + Constants.DMDirName;
                        List<string> dirList = new List<string>();
                        if (Directory.Exists(dmDir))
                        {
                            dirList.Add(dmDir);
                        }
                        string prjDir = m_env.DataManager.DefaultDir + Constants.delimiterPath + modelName;
                        CreateProject(modelName, prjDir, Constants.defaultComment, dirList);
                    }

                    Thread t = new Thread(new ThreadStart(LoadModelData));
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
                return;
            }

        }

        /// <summary>
        /// Load model from input file.
        /// </summary>
        /// <param name="path">file nane.</param>
        internal void LoadModel(string path)
        {
            if (m_isLoadProject == false)
            {
                string modelDir = Path.GetDirectoryName(path);
                if (modelDir.EndsWith(Constants.xpathModel))
                {
                    modelDir = modelDir.Substring(0, modelDir.Length - 5);
                }
                CreateProject(Constants.defaultPrjID, modelDir, Constants.defaultComment, new List<string>());
            }
            m_openFileDialog.FileName = path;
            Thread t = new Thread(new ThreadStart(LoadModelData));
            t.Start();
        }


        /// <summary>
        /// The action of [export model] menu click.
        /// Show SaveFileDialog and select the sbml file.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void ExportModelMenuClick(object sender, EventArgs e)
        {
            m_savePrjDialog = new SaveProjectDialog();
            m_savePrjDialog.Text = MessageResMain.ExportModelDialog;
            m_savePrjDialog.SPSaveButton.Click += new EventHandler(ExportModel);
            m_savePrjDialog.SPCancelButton.Click += new EventHandler(ExportModelCancel);

            List<string> list = m_env.DataManager.GetModelList();
            CheckedListBox box = m_savePrjDialog.CheckedListBox;
            foreach (string s in list)
            {
                box.Items.Add(s);
            }

            m_savePrjDialog.ShowDialog();
        }

        /// <summary>
        /// Cancel to export the selected model.
        /// </summary>
        /// <param name="sender">Cancel Button.</param>
        /// <param name="e">EventArgs.</param>
        public void ExportModelCancel(object sender, EventArgs e)
        {
            m_savePrjDialog.Close();
            m_savePrjDialog.Dispose();
        }

        /// <summary>
        /// Export the selected models.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void ExportModel(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            CheckedListBox box = m_savePrjDialog.CheckedListBox;
            foreach (string s in box.CheckedItems)
                list.Add(s);

            if (list.Count <= 0)
            {
                Util.ShowWarningDialog(MessageResMain.ErrNoSelect);
                return;
            }
            else
            {
                try
                {
                    saveFileDialog.RestoreDirectory = true;
                    //                    saveFileDialog.Filter = "model file(*.eml,*.sbml)|*.eml;*.sbml|model file(*.sbml)|*.sbml|model file(*.eml)|*.eml|all(*.*)|*.*";
                    saveFileDialog.Filter = Constants.FilterEmlFile;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        m_env.DataManager.ExportModel(list, saveFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    Util.ShowErrorDialog(ex.Message);
                }
            }
            m_savePrjDialog.Close();
            m_savePrjDialog.Dispose();
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
            if (m_env.PluginManager != null)
            {
                m_env.PluginManager.ShowSelectPlugin();
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
            if (m_type != ProjectStatus.Uninitialized)
            {
                if (m_editCount > 0)
                {
                    try
                    {
                        if (Util.ShowYesNoCancelDialog(MessageResMain.SaveConfirm))
                        {
                            m_env.DataManager.SimulationStop();
                            Thread.Sleep(1000);
                            m_isClose = true;
                            SaveProjectMenuClick(sender, e);
                        }
                        else
                        {
                            m_env.DataManager.SimulationStop();
                            Thread.Sleep(1000);
                            CloseProject(m_project);
                        }
                    }
                    catch (Util.CancelException)
                    {
                        return;
                    }
                }
                else
                {
                    m_env.DataManager.SimulationStop();
                    Thread.Sleep(1000);

                    CloseProject(m_project);
                }
            }
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
            m_currentDir = Util.GetBaseDir();

            SelectDirectory dialog = new SelectDirectory();
            String mes = MessageResMain.ExpModelMes;
            dialog.Description = mes;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_currentDir = dialog.DirectoryPath;
                Util.SetBaseDir(m_currentDir);
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

                //PythonEngine engine = new PythonEngine();

                //string crDir = Directory.GetCurrentDirectory();
                //Console.WriteLine(crDir);
                //engine.AddToPath(Directory.GetCurrentDirectory());
                //string scriptFile = openScriptDialog.FileName;

                //try
                //{
                //    MemoryStream standardOutput = new MemoryStream();
                //    engine.SetStandardOutput(standardOutput);
                //    engine.ExecuteFile(scriptFile);
                //    string stdOut = ASCIIEncoding.ASCII.GetString(standardOutput.ToArray());
                //}
                //catch (Exception)
                //{
                //    Util.ShowErrorDialog(String.Format(MessageResMain.ErrLoadFile,
                //        new object[] { scriptFile }));
                //}

                //
                // Executes continuously.
                //
                if (this.m_env.DataManager.CurrentProjectID != null)
                {
                    this.m_isLoadProject = true;
                    this.m_project = this.m_env.DataManager.CurrentProjectID;
                    if (this.m_env.DataManager.GetCurrentSimulationTime() > 0.0)
                    {
                        this.m_env.DataManager.SimulationSuspend();
                        this.m_env.PluginManager.ChangeStatus(ProjectStatus.Suspended);
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
                        m_env.DataManager.LoadUserActionFile(m_openFileDialog.FileName);
                    }
                    else
                    {
                        Util.ShowWarningDialog(MessageResMain.FileNotFound);
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
            PluginVersionListWindow w = new PluginVersionListWindow(m_env.PluginManager);
            w.ShowDialog();
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
            sfd.CreatePrompt = true;
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
                loadWindowSetting(ofd.FileName);
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
            SelectWinSettingWindow win = new SelectWinSettingWindow();

            string path = win.ShowWindow(false);
            if (path != null)
            {
                loadWindowSetting(path);
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
                Project prj = win.SelectedProject;
                LoadProject(prj.Name, prj.FilePath);
                Project project = m_env.DataManager.CurrentProject;

                foreach (List<string> list in project.DmDic.Values)
                {
                    List<string> temp = new List<string>();
                    foreach (string dm in list)
                        if (!win.ProjectDMListBox.Items.Contains(dm))
                            temp.Add(dm);
                    foreach (string dm in temp)
                        list.Remove(dm);
                }
            }
        }

        /// <summary>
        /// Event when this form is closed.
        /// </summary>
        /// <param name="sender">this form.</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void MainWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(m_project))
            {
                CloseProject(m_project);
            }
            SaveRecentProject();
            SaveWindowSetting(m_userWindowSettingPath);
        }

        /// <summary>
        /// Event when the distributed environment button is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ClickDistributedEnvMenu(object sender, EventArgs e)
        {
            DistributedEnvSetupWindow win = new DistributedEnvSetupWindow(m_env.JobManager);
            win.Shown += new EventHandler(win.WindowShown);
            win.ShowDialog();
        }

        /// <summary>
        /// Event when Job status Button is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ClickJobStatusMenu(object sender, EventArgs e)
        {
            DistributedEnvWindow win = new DistributedEnvWindow(m_env.JobManager);

            win.Show();
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
            ScriptEditor edit = new ScriptEditor(m_env);
            edit.ShowDialog();
        }

    }
}
