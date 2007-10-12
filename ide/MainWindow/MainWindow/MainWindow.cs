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
using WeifenLuo.WinFormsUI.Docking;

namespace EcellLib.MainWindow
{
    public partial class MainWindow : Form, PluginBase
    {
        #region Readonly Fields 
        /// <summary>
        /// Setting file of Docking window. 
        /// </summary>
        private readonly string defaultWindowSettingsFile = "window.config";

        #endregion

        #region Fields
        /// <summary>
        /// m_entityListDock (DockContent)
        /// </summary>
        private Dictionary<string, DockContent> m_dockWindowDic;
        /// <summary>
        /// m_entityListDock (DockContent)
        /// </summary>
        private Dictionary<string, ToolStripMenuItem> m_dockMenuDic;
        /// <summary>
        /// defaultWindowSettingPath (string)
        /// </summary>
        private string defaultWindowSettingPath;
        /// <summary>
        /// userWindowSettingPath (string)
        /// </summary>
        private string userWindowSettingPath;
        /// <summary>
        /// m_pManager (PluginManager)
        /// </summary>
        private PluginManager m_pManager;
        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        private DataManager m_dManager;
        /// <summary>
        /// m_newPrjDialog (NewProjectDialog)
        /// </summary>
        private NewProjectDialog m_newPrjDialog;
        /// <summary>
        /// m_openPrjDialog (OpenProjectDialog)
        /// </summary>
        private OpenProjectDialog m_openPrjDialog;
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
        /// base directory of plugin.
        /// </summary>
        private string m_pluginDir;
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
        private int m_type = 0;
        /// <summary>
        /// The number of edit after project is opened.
        /// </summary>
        private int m_editCount = 0;
        /// <summary>
        /// List of plugin to check loaded plugin.
        /// </summary>
        public List<string> m_pluginList;
        /// <summary>
        /// ResourceManager for MainWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResMain));
        /// <summary>
        /// Docking Windows object.
        /// </summary>
        public WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        #endregion

        /// <summary>
        /// get / set projectID.
        /// </summary>
        public String projetID
        {
            get { return this.m_project; }
            set { 
                this.m_project = value;
                if (value == null)
                    this.m_isLoadProject = false;
                else
                    this.m_isLoadProject = true;
            }
        }

        /// <summary>
        /// Constructor for MainWindow plugin.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            m_dockWindowDic = new Dictionary<string,DockContent>();
            m_dockMenuDic = new Dictionary<string,ToolStripMenuItem>();
            PluginManager pm = PluginManager.GetPluginManager();
            pm.DockPanel = this.dockPanel;
            LoadPlugins();
            //Load default window settings.
            setFilePath();
            loadDefaultWindowSetting();
        }
        /// <summary>
        /// Set window setting file path.
        /// </summary>
        private void setFilePath()
        {
            defaultWindowSettingPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), defaultWindowSettingsFile);
            userWindowSettingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);
            userWindowSettingPath = Path.Combine(userWindowSettingPath, defaultWindowSettingsFile);
        }
        /// <summary>
        /// Save default window settings.
        /// </summary>
        private void saveWindowSetting(string filename)
        {
            //Save current window settings.
            try
            {
                ECellSerializer.SaveAsXML(this, filename);
                Debug.WriteLine("save window settings: " + filename);
            }
            catch (Exception ex)
            {
                string errmsg = m_resources.GetString("ErrSaveWindowSettings") + Environment.NewLine + filename + Environment.NewLine + ex.Message;
                Debug.WriteLine(errmsg);
                //MessageBox.Show(errmsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Load window settings.
        /// </summary>
        private bool loadWindowSetting(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                    throw new Exception(m_resources.GetString("FileNotFound"));
                ECellSerializer.LoadFromXML(this, filename);
                Debug.WriteLine("load window settings: " + filename);
                return true;
            }
            catch (Exception ex)
            {
                string errmsg = m_resources.GetString("ErrLoadWindowSettings") + Environment.NewLine + filename + Environment.NewLine + ex.Message;
                Debug.WriteLine(errmsg);
                //MessageBox.Show(errmsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <summary>
        /// Load default window settings.
        /// </summary>
        private void loadDefaultWindowSetting()
        {
            //Load user window settings.
            if (!loadWindowSetting(userWindowSettingPath))
                // Load default window settings when failed.
                loadWindowSetting(defaultWindowSettingPath);
        }

        /// <summary>
        /// Load plugins.
        /// </summary>
        void LoadPlugins()
        {
            try
            {
                m_dManager = DataManager.GetDataManager();
            }
            catch (Exception e)
            {
                String errmes = m_resources.GetString("ErrStartup");
                MessageBox.Show(errmes + "\n\n" + e.Message,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            m_pManager = PluginManager.GetPluginManager();
            m_pManager.AddPlugin(this);
            m_pManager.AppVersion = Assembly.GetExecutingAssembly().GetName().Version;
            m_pManager.CopyRight = global::EcellLib.MainWindow.Properties.Resources.CopyrightNotice; ;


            m_pluginList = new List<string>();
            m_isLoadProject = false;
            LoadAllPlugin();
            m_pManager.ChangeStatus(0);

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
        /// Load model in the thread, if this thread is sub thread.
        /// </summary>
        void LoadModelData()
        {
            Util.InitialLanguage();
            try
            {
                string modelID = m_dManager.LoadModel(openFileDialog.FileName, true);
                if (this.InvokeRequired)
                {
                    LoadModelDelegate dlg = new LoadModelDelegate(LoadModelThread);
                    this.Invoke(dlg, new object[] { modelID });
                }
            }
            catch (Exception ex)
            {
                string errmes = m_resources.GetString("ErrLoadModel");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseProjectDelegate dlg = new CloseProjectDelegate(CloseProject);
                this.Invoke(dlg, new object[] { m_project });
            }
        }

        private void CloseProject(String m_prjID)
        {
            if (m_prjID == null) return;
            m_isLoadProject = false;
            m_pManager.ChangeStatus(Util.NOTLOAD);
            m_dManager.CloseProject(m_prjID);
            m_project = null;
        }

        /// <summary>
        /// Load model by DataManager.
        /// </summary>
        public void LoadModelThread(string modelID)
        {
            m_pManager.LoadData(modelID);
            DataManager.GetDataManager().SetPositions(modelID);
            m_editCount = 0;
        }

        /// <summary>
        /// Load plugin in plugin directory and add the plugin menus to MainWindow.
        /// </summary>
        /// <param name="path">path of plugin.</param>
        void LoadPlugin(string path)
        {
            PluginBase pb = null;
            List<DockContent> winList = null;
            List<ToolStripMenuItem> menuList = null;
            List<ToolStripItem> toolList = null;

            string pName = Path.GetFileNameWithoutExtension(path);
            string className = "EcellLib." + pName + "." + pName;

            if (m_pluginList.Contains(pName)) return;
            m_pluginList.Add(pName);

            try
            {
                pb = m_pManager.LoadPlugin(path, className);
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrLoadPlugin");
                MessageBox.Show(errmes + "(" + className + ")\n\n" + ex.Message,
                    "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Set DockContent.
            winList = pb.GetWindowsForms();
            if (winList != null && winList.Count > 0)
                foreach (DockContent dock in winList)
                    SetDockContent(dock);
            // Set Menu.
            menuList = pb.GetMenuStripItems();
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
                            ToolStripMenuItem t = (ToolStripMenuItem)iter.Current;
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
            toolList = pb.GetToolBarMenuStripItems();
            if (toolList != null)
            {
                if (this.toolstrip.Items.Count > 0)
                    this.toolstrip.Items.AddRange(new ToolStripItem[] { new ToolStripSeparator() });
                foreach (ToolStripItem tool in toolList)
                    this.toolstrip.Items.AddRange(new ToolStripItem[] { tool });
            }
        }
        
        /// <summary>
        /// set DockContent
        /// </summary>
        private void SetDockContent(DockContent content)
        {
            Debug.WriteLine("create dock: " + content.Text);
            //Create New DockContent
            content.Name = content.Text;
            content.Tag = content.Text;
            content.Pane = null;
            content.PanelPane = null;
            content.FloatPane = null;
            content.DockHandler.DockPanel = this.dockPanel;
            content.IsHidden = false;
            content.FormClosing += new FormClosingEventHandler(this.DockContent_Closing);

            //Create DockWindow Menu
            setDockMenu(content.Text);
            m_dockWindowDic.Add(content.Text, content);
            content.Show(this.dockPanel, DockState.Document);
        }
        /// <summary>
        /// set Window Menu
        /// </summary>
        private void setDockMenu(string name)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(name);
            item.Text = name;
            item.Checked = true;
            item.Click += new System.EventHandler(this.dockWindowToolStripMenuItem_Click);
            this.showWindowToolStripMenuItem.DropDown.Items.Add(item);
            m_dockMenuDic.Add(name, item);
        }
        /// <summary>
        /// Get specified DockContent
        /// </summary>
        public DockContent getDockContent(string name)
        {
            return m_dockWindowDic[name];
        }


        private void DockContent_Closing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine(((DockContent)sender).Name + ":" + e.CloseReason);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // hide dock window
                ((DockContent)sender).Hide();
                // uncheck dock window menu
                checkWindowMenu(((DockContent)sender).Name, false);
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
        public void checkWindowMenu(String name, bool bChecked)
        {
            m_dockMenuDic[name].Checked = bChecked;
        }

        /// <summary>
        /// set base plugin data and load plugin.
        /// </summary>
        void LoadAllPlugin()
        {
            List<string> pluginList = new List<string>();

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(Util.s_registryEnvKey);
            m_pluginDir = (string)subkey.GetValue(Util.s_registryPluginDirKey);
            if (m_pluginDir != null)
            {
                if (Directory.Exists(m_pluginDir))
                {
                    foreach (string fileName in Directory.GetFiles(
                        m_pluginDir, Util.s_delimiterWildcard + Util.s_dmFileExtension))
                    {
                        pluginList.Add(fileName);
                    }
                }
            }
            subkey.Close();
            key.Close();

            subkey = key.OpenSubKey(Util.s_registrySWKey);
            if (subkey != null)
            {
                m_pluginDir = (string)subkey.GetValue(Util.s_registryPluginDirKey);
                if (m_pluginDir != null)
                {
                    if (Directory.Exists(m_pluginDir))
                    {
                        foreach (string fileName in Directory.GetFiles(
                            m_pluginDir, Util.s_delimiterWildcard + Util.s_dmFileExtension))
                        {
                            pluginList.Add(fileName);
                        }
                    }
                }
                subkey.Close();
            }
            key.Close();

            key = Microsoft.Win32.Registry.LocalMachine;
            subkey = key.OpenSubKey(Util.s_registrySWKey);
            if (subkey != null)
            {
                m_pluginDir = (string)subkey.GetValue(Util.s_registryPluginDirKey);
                if (m_pluginDir != null)
                {
                    if (Directory.Exists(m_pluginDir))
                    {
                        foreach (string fileName in Directory.GetFiles(
                            m_pluginDir, Util.s_delimiterWildcard + Util.s_dmFileExtension))
                        {
                            pluginList.Add(fileName);
                        }
                    }
                }
                subkey.Close();
            }
            key.Close();

            m_currentDir = Util.GetBaseDir();
            if (m_currentDir == null)
            {
                m_currentDir =
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                m_currentDir = m_currentDir + "/e-cell/project";
            }

            foreach (string pName in pluginList)
            {
                LoadPlugin(pName);
            }
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
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the window form for MainWindow plugin.
        /// </summary>
        /// <returns>Windows form</returns>
        public List<DockContent> GetWindowsForms()
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
        public void ChangeStatus(int type)
        {
            if (type == Util.NOTLOAD)
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
                exitToolStripMenuItem.Enabled = true;
            }
            else if (type == Util.LOADED || type == Util.STEP)
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
            switch(status)
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
        public Bitmap Print()
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
        public bool IsEnablePrint()
        {
            return false;
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
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
                String mes = m_resources.GetString("SaveConfirm");
                DialogResult res = MessageBox.Show(mes,
                    "Confirm Dialog", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    m_isClose = true;
                    SaveProjectMenuClick(sender, e);
                    m_editCount = 0;
                }
                else if (res == DialogResult.No)
                {
                    m_isLoadProject = false;
                    m_pManager.ChangeStatus(0);
                    m_dManager.CloseProject(m_project);
                    m_editCount = 0;
                    m_project = null;
                }
                else
                {
                    return;
                }
            }
            m_newPrjDialog = new NewProjectDialog();
            m_newPrjDialog.CPCreateButton.Click += new System.EventHandler(this.NewProject);
            m_newPrjDialog.CPCancelButton.Click += new System.EventHandler(this.NewProjectCancel);

            m_newPrjDialog.ShowDialog();
        }

        /// <summary>
        /// The action when you click OK or Cancel in NewProjectDialog.
        /// If you don't set name or model name, system show warning message.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void NewProject(object sender, EventArgs e)
        {
                if (m_newPrjDialog.textName.Text == "") 
                {
                    String errmes = m_resources.GetString("ErrPrjIdNull");
                    MessageBox.Show(errmes,
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (m_newPrjDialog.textModelName.Text == "")
                {
                    String errmes = m_resources.GetString("ErrModelNull");
                    MessageBox.Show(errmes,
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (Util.IsNGforID(m_newPrjDialog.textName.Text))
                {
                    String errmes = m_resources.GetString("ErrPrjIdNG");
                    MessageBox.Show(errmes,
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (Util.IsNGforID(m_newPrjDialog.textModelName.Text))
                {
                    String errmes = m_resources.GetString("ErrModelNG");
                    MessageBox.Show(errmes,
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    m_dManager.NewProject(m_newPrjDialog.textName.Text,
                        m_newPrjDialog.textComment.Text);
                    List<EcellObject> list = new List<EcellObject>();
                    list.Add(EcellObject.CreateObject(m_newPrjDialog.textModelName.Text, null, "Model", null, null));
                    m_dManager.DataAdd(list);
                    m_isLoadProject = true;
                    m_project = m_newPrjDialog.textName.Text;
                    m_pManager.ChangeStatus(1);
                    m_editCount = 0;
                }
                catch (Exception ex)
                {
                    String errmes = m_resources.GetString("ErrCreatePrj");
                    MessageBox.Show(errmes + "\n\n" + ex.Message,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            m_newPrjDialog.Close();
            m_newPrjDialog.Dispose();
            m_newPrjDialog = null;
        }

        /// <summary>
        /// Cancel to create project.
        /// </summary>
        /// <param name="sender">Cancel Button.</param>
        /// <param name="e">EventArgs.</param>
        public void NewProjectCancel(object sender, EventArgs e)
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
            if (m_editCount > 0)
            {
                String mes = m_resources.GetString("SaveConfirm");
                DialogResult res = MessageBox.Show(mes,
                    "Confirm Dialog", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    m_isClose = true;
                    SaveProjectMenuClick(sender, e);
                    m_editCount = 0;
                }
                else if (res == DialogResult.No)
                {
                    m_isLoadProject = false;
                    m_pManager.ChangeStatus(0);
                    m_dManager.CloseProject(m_project);
                    m_editCount = 0;
                    m_project = null;
                }
                else
                {
                    return;
                }
            }

            m_openPrjDialog = new OpenProjectDialog();
            m_openPrjDialog.OPOpenButton.Click += new System.EventHandler(this.OpenProject);
            m_openPrjDialog.OPCancelButton.Click += new System.EventHandler(this.OpenProjectCancel);

            try
            {
                List<Project> list = m_dManager.GetProjects(m_currentDir);
                if (list != null)
                {
                    foreach (Project p in list)
                    {
                        string[] s = new string[] { p.M_prjName, p.M_updateTime, p.M_comment };
                        m_openPrjDialog.dataGridView1.Rows.Add(s);
                    }
                }
                m_openPrjDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrShowOpenPrj");
                MessageBox.Show(errmes + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Cancel to open project.
        /// </summary>
        /// <param name="sender">Cancel Button.</param>
        /// <param name="e">EventArgs.</param>
        public void OpenProjectCancel(object sender, EventArgs e)
        {
            m_openPrjDialog.Close();
            m_openPrjDialog.Dispose();
            m_openPrjDialog = null;
        }

        /// <summary>
        /// The action when you click OK or Cancel in OpenProjectDialog.
        /// If you don't select the project, system show warning message.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void OpenProject(object sender, EventArgs e)
        {
            try
            {
                if (m_openPrjDialog.dataGridView1.SelectedRows.Count <= 0)
                {
                    String mes = m_resources.GetString("ErrNoSelectPrj");
                    MessageBox.Show(mes,
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                m_dManager.LoadProject((string)m_openPrjDialog.dataGridView1.CurrentRow.Cells["PrjName"].Value);
                m_isLoadProject = true;
                m_project = (string)m_openPrjDialog.dataGridView1.CurrentRow.Cells["PrjName"].Value;
                m_pManager.ChangeStatus(1);
                m_editCount = 0;
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrOpenPrj");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            m_openPrjDialog.Close();
            m_openPrjDialog.Dispose();
            m_openPrjDialog = null;
        }

        /// <summary>
        /// The action of [save project] menu click.
        /// Show SaveProjectDialog and select the save instance.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void SaveProjectMenuClick(object sender, EventArgs e)
        {
            m_savePrjDialog = new SaveProjectDialog();
            m_savePrjDialog.SPSaveButton.Click += new System.EventHandler(this.SaveProject);
            m_savePrjDialog.SPCancelButton.Click += new System.EventHandler(this.SaveProjectCancel);

            try
            {
                CheckedListBox box = m_savePrjDialog.checkedListBox1;
                List<string> list = m_dManager.GetSavableModel();
                if (list != null)
                {
                    foreach (string s in list)
                    {
                        box.Items.Add(s + " : [Model]");
                    }
                }

                list = m_dManager.GetSavableSimulationParameter();
                if (list != null)
                {
                    foreach (string s in list)
                    {
                        box.Items.Add(s + " : [SimulationParameter]");
                    }
                }

                String res = m_dManager.GetSavableSimulationResult();
                if (res != null)
                {
                    box.Items.Add(res + " : [SimulationResult]");
                }

                int i = 0, count = box.Items.Count;
                box.CheckOnClick = true;
                
                for ( i = 0 ; i < count ; i++ )
                {
                    box.SetItemChecked(i, true);
                }

                m_savePrjDialog.ShowDialog();

            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrShowSavePrj");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (m_savePrjDialog != null) m_savePrjDialog.Dispose();
            }
        }

        private void SaveProjectCancel(object sender, EventArgs e)
        {
            m_savePrjDialog.Close();
            m_savePrjDialog.Dispose();
            m_savePrjDialog = null;
            if (m_isClose)
            {
                m_isLoadProject = false;
                m_pManager.ChangeStatus(0);
                m_dManager.CloseProject(m_project);
                m_project = null;
                m_isClose = false;
            }
        }

        /// <summary>
        /// The action when you click OK or Cancel in SaveProjectDialog.
        /// If you don't select one instance, system do nothing.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void SaveProject(object sender, EventArgs e)
        {
            try
            {
                    CheckedListBox box = m_savePrjDialog.checkedListBox1;
                    if (box.CheckedItems.Count <= 0)
                    {
                        String errmes = m_resources.GetString("ErrSelectSave");
                        MessageBox.Show(errmes,
                            "Warning", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                    foreach (string s in box.CheckedItems)
                    {
                        if (s.EndsWith(" : [Model]"))
                        {
                            int end = s.LastIndexOf(" : [Model]");
                            string p = s.Substring(0, end);
                            m_dManager.SaveModel(p);

                        }
                        else if (s.EndsWith(" : [SimulationParameter]"))
                        {
                            int end = s.LastIndexOf(" : [SimulationParameter]");
                            string p = s.Substring(0, end);
                            m_dManager.SaveSimulationParameter(p);
                        }
                        else if (s.EndsWith(" : [SimulationResult]"))
                        {
                            m_dManager.SaveSimulationResult(
                                    null, 0.0, m_dManager.GetCurrentSimulationTime(), null, m_dManager.GetLoggerList());
                        }
                    }
                    m_editCount = 0;
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrSavePrj");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            m_savePrjDialog.Close();
            m_savePrjDialog.Dispose();
            m_savePrjDialog = null;
            if (m_isClose)
            {
                m_isLoadProject = false;
                m_pManager.ChangeStatus(0);
                m_dManager.CloseProject(m_project);
                m_project = null;
                m_isClose = false;
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
                String mes = m_resources.GetString("SaveConfirm");
                DialogResult res = MessageBox.Show(mes,
                    "Confirm Dialog", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    m_isClose = true;
                    SaveProjectMenuClick(sender, e);
                    m_editCount = 0;
                }
                else if (res == DialogResult.No)
                {
                    m_isLoadProject = false;
                    m_pManager.ChangeStatus(Util.NOTLOAD);
                    m_dManager.CloseProject(m_project);
                    m_editCount = 0;
                    m_project = null;
                }
                else
                {
                    return;
                }
            }
            else
            {
                m_isLoadProject = false;
                m_pManager.ChangeStatus(0);
                m_dManager.CloseProject(m_project);
                m_project = null;
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
                String mes = m_resources.GetString("SaveConfirm");
                DialogResult res = MessageBox.Show(mes,
                    "Confirm Dialog", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    m_isClose = true;
                    SaveProjectMenuClick(sender, e);
                    m_editCount = 0;
                }
                else if (res == DialogResult.No)
                {
                    m_isLoadProject = false;
                    m_pManager.ChangeStatus(Util.NOTLOAD);
                    m_dManager.CloseProject(m_project);
                    m_project = null;
                    m_editCount = 0;
                }
                else
                {
                    return;
                }
            }
            if (m_project != null)
            {
                m_isLoadProject = false;
                m_pManager.ChangeStatus(Util.NOTLOAD);
                m_dManager.CloseProject(m_project);
                m_project = null;
            }

            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "model file(*.sbml,*.eml)|*.sbml;*.eml|model file(*.sbml)|*.sbml|model file(*.eml)|*.eml|all(*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (m_isLoadProject == false)
                {
                    m_dManager.NewProject("project", "comment");
                    m_project = "project";
                    m_isLoadProject = true;
                    m_pManager.ChangeStatus(1);
                    m_editCount = 0;
                }
                
                Thread t = new Thread(new ThreadStart(LoadModelData));
                t.Start();
            }
        }

        /// <summary>
        /// Load model from input file.
        /// </summary>
        /// <param name="path">file nane.</param>
        public void LoadModel(string path)
        {
            if (m_isLoadProject == false)
            {
                m_dManager.NewProject("project", "comment");
                m_project = "project";
                m_isLoadProject = true;
                m_pManager.ChangeStatus(1);
                m_editCount = 0;
            }
            openFileDialog.FileName = path;
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
            m_savePrjDialog.Text = m_resources.GetString("ExportModelDialog");
            m_savePrjDialog.SPSaveButton.Click += new EventHandler(ExportModel);
            m_savePrjDialog.SPCancelButton.Click += new EventHandler(ExportModelCancel);

            List<string> list = m_dManager.GetModelList();
            CheckedListBox box = m_savePrjDialog.checkedListBox1;
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
                CheckedListBox box = m_savePrjDialog.checkedListBox1;
                foreach (string s in box.CheckedItems)
                    list.Add(s);

                if (list.Count <= 0)
                {
                    String errmes = m_resources.GetString("ErrNoSelectExp");
                    MessageBox.Show(errmes, "WARNING", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        saveFileDialog.RestoreDirectory = true;
                        //                    saveFileDialog.Filter = "model file(*.eml,*.sbml)|*.eml;*.sbml|model file(*.sbml)|*.sbml|model file(*.eml)|*.eml|all(*.*)|*.*";
                        saveFileDialog.Filter = "model file(*.eml)|*.eml|all(*.*)|*.*";
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            m_dManager.ExportModel(list, saveFileDialog.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        String errmes = m_resources.GetString("ErrExpModel");
                        MessageBox.Show(errmes + "\n\n" + ex.Message,
                            "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                saveFileDialog.Filter = "ess file(*.ess)|*.ess";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    m_dManager.SaveScript(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrSaveScript");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (m_pManager != null)
            {
                m_pManager.ShowSelectPlugin();
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
            if (m_type != Util.NOTLOAD)
            {
                if (m_editCount > 0)
                {
                    String mes = m_resources.GetString("SaveConfirm");
                    DialogResult res = MessageBox.Show(mes,
                        "Confirm Dialog", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        m_dManager.SimulationStop();
                        Thread.Sleep(1000); 
                        m_isClose = true;
                        SaveProjectMenuClick(sender, e);
                    }
                    else if (res == DialogResult.No)
                    {
                        m_dManager.SimulationStop();
                        Thread.Sleep(1000); 
                        m_isLoadProject = false;
                        m_pManager.ChangeStatus(0);
                        m_dManager.CloseProject(m_project);
                        m_project = null;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    m_dManager.SimulationStop();
                    Thread.Sleep(1000);
                    m_isLoadProject = false;
                    m_pManager.ChangeStatus(0);
                    m_dManager.CloseProject(m_project);
                    m_project = null;
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
            String mes = m_resources.GetString("ExpModelMes");
            dialog.Description = mes;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_currentDir = dialog.DirectoryPath;
                Util.SetBaseDir(m_currentDir);
                /*
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser;
                key = key.OpenSubKey("Environment");

                m_currentDir = dialog.DirectoryPath;
                key.SetValue("ecellide_basedir", m_currentDir);
                key.Close();
                 */
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
                PythonEngine engine = new PythonEngine();
                engine.AddToPath(Directory.GetCurrentDirectory());
                string startup = Environment.GetEnvironmentVariable("IRONPYTHONSTARTUP");
                if (startup != null && startup.Length > 0)
                {
                    try
                    {
                        engine.ExecuteFile(startup);
                    }
                    catch
                    {
                    }
                }
                engine.ExecuteFile(openScriptDialog.FileName);
                //
                // Executes continuously.
                //
                if (this.m_dManager.CurrentProjectID != null)
                {
                    this.m_isLoadProject = true;
                    this.m_project = this.m_dManager.CurrentProjectID;
                    if (this.m_dManager.GetCurrentSimulationTime() > 0.0)
                    {
                        this.m_dManager.SimulationSuspend();
                        this.m_pManager.ChangeStatus(3);
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
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "action file(*.xml)|*.xml;*xml|all(*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(openFileDialog.FileName))
                    {
                        m_dManager.LoadUserActionFile(openFileDialog.FileName);
                    }
                    else
                    {
                        String errmes = m_resources.GetString("FileNotFound");
                        MessageBox.Show(errmes, "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrImpScript");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                saveFileDialog.Filter = "action file(*.xml)|*.xml";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    m_dManager.SaveUserAction(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrSaveAction");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            m_dManager.UndoAction();
        }

        /// <summary>
        /// The action of clicking the redo action menu.
        /// Redo action.
        /// </summary>
        /// <param name="sender">MenuItem.</param>
        /// <param name="e">EventArgs.</param>
        private void RedoMenuClick(object sender, EventArgs e)
        {
            m_dManager.RedoAction();
        }
        #endregion

        private void ShowPluginVersionClick(object sender, EventArgs e)
        {
            PluginVersionListWindow w = new PluginVersionListWindow();
            w.ShowDialog();
        }
        
        private void saveWindowSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Window Setting File(*.xml) |*.xml";
            sfd.CheckPathExists = true;
            sfd.CreatePrompt = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save window settings.
                saveWindowSetting(sfd.FileName);
            }

        }

        private void loadWindowSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Window Setting File(*.xml) |*.xml";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Load window settings.
                loadWindowSetting(ofd.FileName);
            }
       }

        /// <summary>
        /// Set the process size.
        /// </summary>
        /// <param name="hwnd">window descriptor.</param>
        /// <param name="min">min size.</param>
        /// <param name="max">max size.</param>
        /// <returns>If contains size, retur true.</returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr hwnd, int min, int max);

        private void dockWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.Checked)
            {
                //Hide EntityList
                m_dockWindowDic[item.Text].Hide();
                item.Checked = false;
            }
            else
            {
                //Show EntityList
                m_dockWindowDic[item.Text].Show();
                item.Checked = true;
            }
        }

        private void SetupIDEMenuClick(object sender, EventArgs e)
        {
            SetupIDEWindow win = new SetupIDEWindow();
            win.ShowDialog();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveWindowSetting(userWindowSettingPath);
        }

        private void DistributedEnvMenuClick(object sender, EventArgs e)
        {
            DistributedEnvSetupWindow win = new DistributedEnvSetupWindow();
            win.Shown += new EventHandler(win.WindowShown);
            win.ShowDialog();
        }

    }

    /// <summary>
    /// select directory dialog for set e-cell base directory.
    /// </summary>
    public class SelectDirectory : FolderNameEditor
    {
        private String m_directoryPath;
        private String m_description;

        /// <summary>
        /// get selected directry at dialog.
        /// </summary>
        public String DirectoryPath
        {
            get { return m_directoryPath; }
        }

        /// <summary>
        /// get/set description of dialog.
        /// </summary>
        public String Description
        {
            set { m_description = value; }
            get { return m_description; }
        }

        /// <summary>
        /// display select directory dialog.
        /// </summary>
        /// <returns>button of dialog</returns>
        public DialogResult ShowDialog()
        {
            FolderBrowser fb = new FolderBrowser();
            fb.Description = m_description;
            fb.StartLocation = FolderBrowserFolder.MyComputer;
            DialogResult result = fb.ShowDialog();
            m_directoryPath = fb.DirectoryPath;

            if (m_directoryPath == "")
            {
                return DialogResult.Cancel;
            }
            return result;
        }
    }

}