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
using Yukichika.Controls;
using Yukichika.Serialization;

namespace EcellLib.MainWindow
{
    public partial class MainWindow : Form, PluginBase
    {
        #region Fields
        /// <summary>
        /// m_entityList (UserControl)
        /// </summary>
        private UserControl m_entityList;
        /// <summary>
        /// m_pathwayWindow (UserControl)
        /// </summary>
        private UserControl m_pathwayWindow;
        /// <summary>
        /// m_messageWindow (UserControl)
        /// </summary>
        private UserControl m_messageWindow;
        /// <summary>
        /// m_objectList (UserControl)
        /// </summary>
        private UserControl m_objectList;
        /// <summary>
        /// m_propertyWindow (UserControl)
        /// </summary>
        private UserControl m_propertyWindow;
        /// <summary>
        /// m_entityListTab (SDockTabPage)
        /// </summary>
        private SDockTabPage m_entityListTab;
        /// <summary>
        /// m_pathwayWindowTab (SDockTabPage)
        /// </summary>
        private SDockTabPage m_pathwayWindowTab;
        /// <summary>
        /// m_messageWindowTab (SDockTabPage)
        /// </summary>
        private SDockTabPage m_messageWindowTab;
        /// <summary>
        /// m_objectListTab (SDockTabPage)
        /// </summary>
        private SDockTabPage m_objectListTab;
        /// <summary>
        /// m_propertyWindowTab (SDockTabPage)
        /// </summary>
        private SDockTabPage m_propertyWindowTab;
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
        #endregion

        /// <summary>
        /// Constructor for MainWindow plugin.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadPlugins();
            //画面設定読込み
            loadDefaultWindow();
            
        }
        
        void loadDefaultWindow()
        {
            //画面のデフォルト設定を呼び出し。
            //設定ファイルの置き場所はプラグインフォルダ。
            string fname = Util.GetPluginDir() + "\\default.sdc";
            Debug.WriteLine(fname);
            if (File.Exists(fname))
            {
                DeserilizeWindow(fname, true);
            }
        }

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
        private void LoadModelData()
        {
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
            m_editCount = 0;
        }

        /// <summary>
        /// Load plugin in plugin directory and add the plugin menus to MainWindow.
        /// </summary>
        /// <param name="path">path of plugin.</param>
        void LoadPlugin(string path)
        {
            PluginBase p = null;
            List<System.Windows.Forms.UserControl> winList = null;
            List<ToolStripMenuItem> menuList = null;
            List<ToolStripItem> toolList = null;

            string pName = Path.GetFileNameWithoutExtension(path);
            string className = "EcellLib." + pName + "." + pName;

            if (m_pluginList.Contains(pName)) return;
            m_pluginList.Add(pName);

            try
            {
                p = m_pManager.LoadPlugin(path, className);
                winList = p.GetWindowsForms();
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrLoadPlugin");
                MessageBox.Show(errmes + "(" + className + ")\n\n" + ex.Message,
                    "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (winList != null && winList.Count > 0)
            {
                foreach (UserControl win in winList)
                {
                    // SDockTabを作成する
                    if (pName == "EntityListWindow")
                    {
                        //EntityListを作成
                        this.m_entityList = win;
                        this.m_entityListTab = setSDockTab("EntityList", this.m_entityList);
                        AddTabPage(this.sDockBay1.MainPane, this.m_entityListTab, SDockDirection.Left);
                    }
                    else if (pName == "PathwayWindow")
                    {
                        //PathwayWindowを作成
                        this.m_pathwayWindow = win;
                        this.m_pathwayWindowTab = setSDockTab("PathwayWindow", this.m_pathwayWindow);
                        AddTabPage(this.sDockBay1.MainPane, this.m_pathwayWindowTab);
                    }
                    else if (pName == "MessageWindow")
                    {
                        //MessageWindowを作成
                        this.m_messageWindow = win;
                        this.m_messageWindowTab = setSDockTab("MessageWindow", this.m_messageWindow);
                        AddTabPage(this.sDockBay1.MainPane, this.m_messageWindowTab, SDockDirection.Bottom);
                    }
                    else if (pName == "ObjectList")
                    {
                        //ObjectListを作成
                        this.m_objectList = win;
                        this.m_objectListTab = setSDockTab("ObjectList", this.m_objectList);
                        AddTabPage(this.sDockBay1.MainPane, this.m_objectListTab, SDockDirection.Right);
                    }
                    else if (pName == "PropertyWindow")
                    {
                        //PropertyWindowを作成
                        this.m_propertyWindow = win;
                        this.m_propertyWindowTab = setSDockTab("PropertyWindow", this.m_propertyWindow);
                        AddTabPage(this.sDockBay1.MainPane, this.m_propertyWindowTab, SDockDirection.Right);
                    }
                }
            }
            menuList = p.GetMenuStripItems();
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
            toolList = p.GetToolBarMenuStripItems();
            if (toolList != null)
            {
                if (this.toolstrip.Items.Count > 0)
                    this.toolstrip.Items.AddRange(new ToolStripItem[] { new ToolStripSeparator() });
                foreach (ToolStripItem tool in toolList)
                    this.toolstrip.Items.AddRange(new ToolStripItem[] { tool });
            }
        }

        /// <summary>
        /// set SDockTag.
        /// </summary>
        SDockTabPage setSDockTab(string name, UserControl win)
        {
            Debug.WriteLine("create tab " + name);
            //新規タブの作成
            SDockTabPage page = new SDockTabPage(name);
            page.Keyword = name;
            win.Dock = DockStyle.Fill;
            page.Controls.Add(win);

            return page;
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
        public List<UserControl> GetWindowsForms()
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
                saveScriptToolStripMenuItem.Enabled = false;
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
                saveFileDialog.Filter = "eml file(*.ems)|*.ems";
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
        #endregion

        private void ShowPluginVersionClick(object sender, EventArgs e)
        {
            PluginVersionListWindow w = new PluginVersionListWindow();
            w.ShowDialog();
        }

        private void sDockBay1_BeforeCloseTabPage(object sender, BeforeCloseTabPageEventArgs e)
        {
            e.Cancel = false;
            e.DoDispose = true;
            foreach(SDockTabPage page in e.Tab) 
            {
                resetTab(page);
            }

        }
        void resetTab(SDockTabPage page)
        {
            // タブの中身を削除
            page.Controls.Clear();
            Debug.WriteLine("remove tag " + page.Text);
            //メニューからタブのチェックをオフ
            tsmCheckOff(page.Text);
        }
        
        void tsmCheckOff(string name)
        {
            if (name == "EntityList")
            {
                //EntityListメニューをチェックオフ
                entityListToolStripMenuItem.Checked = false;
            }
            else if (name == "PathwayWindow")
            {
                //PathwayWindowメニューをチェックオフ
                pathwayWindowToolStripMenuItem.Checked = false;
            }
            else if (name == "MessageWindow")
            {
                //MessageWindowメニューをチェックオフ
                messageWindowToolStripMenuItem.Checked = false;
            }
            else if (name == "ObjectList")
            {
                //ObjectListメニューをチェックオフ
                objectListToolStripMenuItem.Checked = false;
            }
            else if (name == "PropertyWindow")
            {
                //PropertyWindowメニューをチェックオフ
                propertyWindowToolStripMenuItem.Checked = false;
            }
        }


        private void dockManager1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void saveWindowSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "SDock設定ファイル(*.sdc) |*.sdc";
            sfd.CheckPathExists = true;
            sfd.CreatePrompt = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fname = sfd.FileName;
                string err = this.SerializeWindow(fname);
                if (err.Length > 0) MessageBox.Show(err);
            }
        }

        private void loadWindowSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "SDock設定ファイル(*.sdc) |*.sdc";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fname = ofd.FileName;
                string err = this.DeserilizeWindow(fname, true);
                if (err.Length > 0) MessageBox.Show(err);
            }
        }

        /// <summary>
        /// シリアル化します。
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        private string SerializeWindow(string fname)
        {
            string errMsg = "";
            try
            {
                //Docking Windowの指定。
                using (SDockBayConfig bayConf = new SDockBayConfig(this.sDockBay1))
                {
                    //オプション情報があればそれをタグに設定。
                    MainFormState mfs = this.GetOption();

                    //タグの使い方についてはMainFormStateクラスのコメントを参照下さい。
                    bayConf.Tag = mfs;

                    // 出力します。
                    //bayConf.WriteToBinary(fname);
                    bayConf.WriteToSoap(fname);
                }
            }
            catch (Exception ex)
            {
                errMsg = "設定の保存に失敗しました" + ex.StackTrace;
            }
            return errMsg;
        }

        private MainFormState GetOption()
        {
            MainFormState mfs = new MainFormState();
            mfs.FormRect = this.Bounds;
            mfs.WindowState = this.WindowState;
            Yukichika.Controls.ToolStripManagerEx tsm = new ToolStripManagerEx(this);
            mfs.ToolStripManager = tsm;
            return mfs;
        }

        /// <summary>
        /// 逆シリアル化します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string DeserilizeWindow(string fname, bool useHidedelay)
        {
            string errMsg = "";
            if (!File.Exists(fname)) return "存在しないファイルです";

            bool isMaxim = false;
            Rectangle cRect = this.Bounds;
            Cursor cur = Cursor.Current;

            {

                Cursor.Current = Cursors.WaitCursor;
                //ここから逆シリアル化

                if (useHidedelay)
                {
                    this.WindowState = FormWindowState.Normal;
                    foreach (SDockForm sfm in this.sDockBay1.Forms)
                    {
                        sfm.Visible = false;
                    }
                    this.Opacity = 0.9;
                }
                //Application.DoEvents();
                try
                {

                    ///ファイル読み取り
                    //using (SDockBayConfig bc = SDockBayConfig.ReadFromBinary(fname))
                    using (SDockBayConfig bc = SDockBayConfig.ReadFromSoap(fname))

                    {
                        //②ファイルが読み取れなかったり、内容が明らかにおかしい
                        //　（ドッキング領域が正しくないなど）の場合、HasErrorがtrueになります。
                        bool ok = bc != null && !bc.HasError;
                        if (ok)
                        {

                            ///③どのペインにどのコンテンツが含まれていたのか、確認します。
                            int cnt = bc.AllPanes.Count;
                            double rate = 0.9 / (double)Math.Max(1, cnt);
                            resetWindowSelect();
                            for (int i = 0; i < bc.AllPanes.Count; i++)
                            {
                                SDockPaneConfig pc = bc.AllPanes[i];
                                double rate2 = rate / Math.Max(1.0, pc.PageList.Count);
                                for (int j = 0; j < pc.PageList.Count; j++)
                                {
                                    //このインスタンスがページのKeywordを持っています。
                                    SDockTabPageInfo tpi = pc.PageList[j];

                                    //該当コンテンツを探す、もしくはキーを元に新しいインスタンスを作成します。
                                    SDockTabPage tp = this.GetPageForDeserialize(tpi.Keyword);

                                    //③－１ 復元対象のコンテンツをセットしてあげます。
                                    if (tp != null) tpi.SetInstance(tp);
                                    if (useHidedelay) this.Opacity = Math.Max(0, this.Opacity - rate2);
                                }
                            }
                            
                            if (this.Visible) this.Visible = false;
                            //④復元します。
                            //なお、この時SDockBayはPaneをリサイクルします。
                            //つまり、足りなくなった数だけ、あらたにSDockPaneをインスタンス化します。
                            //余ったSDockPane,STabPageに関しては、⑤で後処理をしてあげます。


                            ///SDockBay以外のアプリケーション設定です。
                            MainFormState mfs = bc.Tag as MainFormState;

                            if (mfs != null)
                            {
                                if (mfs.WindowState == FormWindowState.Maximized)
                                {
                                    //最大化は可視化後に。こうすると、分離していたFormの位置が正しくなります。
                                    //this.WindowState = FormWindowState.Maximized;
                                    isMaxim = true;
                                }
                                if (!mfs.FormRect.IsEmpty)
                                {
                                    Rectangle r = System.Windows.Forms.Screen.GetBounds(this);
                                    this.ResetRect(r, mfs.FormRect);
                                }

                                //④－１　このメソッドでドッキングの再現が行われます。
                                this.sDockBay1.LoadFrom(bc, false);
                                
                            }
                            else
                            {
                                //④－１　このメソッドでドッキングの再現が行われます。
                                this.sDockBay1.LoadFrom(bc, false);

                            }

                            //⑤後処理です。不要になったインスタンスがあればそれをDisposeしてあげます。
                            for (int i = 0; i < bc.TrashAllTabPages.Count; i++)
                            {
                                SDockTabPage tp = bc.TrashAllTabPages[i];
                                tp.Controls.Clear();
                                tp.Dispose(true, true);
                            }
                            for (int i = 0; i < bc.TrashPanes.Count; i++)
                            {
                                bc.TrashPanes[i].Dispose();
                            }

                        }
                        else
                        {
                            errMsg = "設定の取得に失敗しました。ファイルの内容が正しくありません。";
                        }
                    }
                }
                catch (Exception ex)
                {
                    errMsg = "設定の取得に失敗しました" + ex.StackTrace;
                }


                if (isMaxim)
                {
                    this.SuspendLayout();
                    this.Bounds = cRect;
                    this.Opacity = 0.01;
                    this.Visible = true;
                    ////一旦最小化をすると、なぜかメモリ使用率減ります。いい機会に。
                    //あんまりかわらなかったので無茶はしないことに。
                    this.WindowState = FormWindowState.Minimized;
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                    //GC.Collect();
                    this.WindowState = FormWindowState.Maximized;
                    this.ResumeLayout();
                    Application.DoEvents();
                }
                else
                {
                    this.Opacity = 0.01;
                    this.Visible = true;
                    //一旦最小化をすると、なぜかメモリ使用率減ります。いい機会に。
                    //あんまりかわらなかったので無茶はしないことに。
                    this.WindowState = FormWindowState.Minimized;
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                    //GC.Collect();
                    this.WindowState = FormWindowState.Normal;
                    Application.DoEvents();
                }
                ///SDockFormではforeachでVisible=true,Show等を使用しないで下さい。
                ///これらの操作では、zオーダー維持のために内部で配列の順序入れ替えが行われています。
                int fcnt = this.sDockBay1.Forms.Count;
                List<SDockForm> fmlist = new List<SDockForm>(this.sDockBay1.Forms);
                for (int i = 0; i < fcnt; i++)
                {
                    SDockForm sfm = fmlist[i];
                    sfm.Opacity = 0;
                    this.sDockBay1.ShowForm(sfm);
                    sfm.Opacity = 0.01;
                }

                Application.DoEvents();


                //最後に飾り付けで透明度でフェードイン

                for (int i = 0; i < 10; i++)
                {
                    this.Opacity += 0.09;
                    for (int j = 0; j < fcnt; j++)
                    {
                        SDockForm sfm = fmlist[j];
                        sfm.Opacity += 0.09;
                    }
                    Application.DoEvents();
                }

                for (int i = 0; i < fcnt; i++)
                {
                    SDockForm sfm = fmlist[i];
                    sfm.Opacity = 1.0;
                    Application.DoEvents();
                }
                this.Opacity = 1;
                Application.DoEvents();

            }
            Cursor.Current = cur;
            return errMsg;
        }

        void resetWindowSelect()
        {
            this.entityListToolStripMenuItem.Checked = false;
            this.pathwayWindowToolStripMenuItem.Checked = false;
            this.messageWindowToolStripMenuItem.Checked = false;
            this.objectListToolStripMenuItem.Checked = false;
            this.propertyWindowToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// 必要なコンテンツを返します。
        /// このアプリケーションでは、いくつかの種類のコンテンツに分かれているため、
        /// このような形式を取ってます。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private SDockTabPage GetPageForDeserialize(string key)
        {
            if (key.StartsWith("EntityList"))
            {
                this.m_entityListTab = setSDockTab("EntityList", this.m_entityList);
                this.entityListToolStripMenuItem.Checked = true;
                return this.m_entityListTab;
            }
            else if (key.Equals("PathwayWindow"))
            {
                this.m_pathwayWindowTab = setSDockTab("PathwayWindow", this.m_pathwayWindow);
                this.pathwayWindowToolStripMenuItem.Checked = true;
                return this.m_pathwayWindowTab;
            }
            else if (key.Equals("MessageWindow"))
            {
                this.m_messageWindowTab = setSDockTab("MessageWindow", this.m_messageWindow);
                this.messageWindowToolStripMenuItem.Checked = true;
                return this.m_messageWindowTab;
            }
            else if (key.Equals("ObjectList"))
            {
                this.m_objectListTab = setSDockTab("ObjectList", this.m_objectList);
                this.objectListToolStripMenuItem.Checked = true;
                return this.m_objectListTab;
            }
            else if (key.Equals("PropertyWindow"))
            {
                this.m_propertyWindowTab = setSDockTab("PropertyWindow", this.m_propertyWindow);
                this.propertyWindowToolStripMenuItem.Checked = true;
                return this.m_propertyWindowTab;
            }
            else
            {
                return null;
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr hwnd, int min, int max);

        private void ResetRect(Rectangle scrRect, Rectangle dstRect)
        {
            Rectangle r = dstRect;
            int t = r.Top;
            int l = r.Left;
            if (scrRect.Left > l) l = scrRect.Left;
            else if (scrRect.Right < r.Right)
            {
                l = Math.Max(scrRect.Left, l - r.Right + scrRect.Right);
            }

            if (scrRect.Top > t) t = scrRect.Top;
            else if (scrRect.Bottom < r.Bottom)
            {
                t = Math.Max(scrRect.Top, t - r.Bottom + scrRect.Bottom);
            }
            this.Bounds = r;
        }

        public void SetPanel(Panel panel)
        {
        }

        private void entityListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (entityListToolStripMenuItem.Checked)
            {
                //EntityListを閉じる
                resetTab(this.m_entityListTab);
                this.m_entityListTab.Dispose();
                entityListToolStripMenuItem.Checked = false;
            }
            else
            {
                this.m_entityListTab = setSDockTab("EntityList", this.m_entityList);
                AddTabPage(this.sDockBay1.MainPane, this.m_entityListTab, SDockDirection.Left);
                entityListToolStripMenuItem.Checked = true;
            }
            this.sDockBay1.MainPane.Refresh();
        }

        private void pathwayWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pathwayWindowToolStripMenuItem.Checked)
            {
                //PathwayWindowを閉じる
                resetTab(this.m_pathwayWindowTab);
                this.m_pathwayWindowTab.Dispose();
                pathwayWindowToolStripMenuItem.Checked = false;
            }
            else
            {
                SDockTabPage page = setSDockTab("PathwayWindow", this.m_pathwayWindow);
                AddTabPage(this.sDockBay1.MainPane, page);
                pathwayWindowToolStripMenuItem.Checked = true;
            }
            this.sDockBay1.MainPane.Refresh();
        }

        private void messageWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (messageWindowToolStripMenuItem.Checked)
            {
                //MessageWindowを閉じる
                resetTab(this.m_messageWindowTab);
                this.m_messageWindowTab.Dispose();
                messageWindowToolStripMenuItem.Checked = false;
            }
            else
            {
                SDockTabPage page = setSDockTab("MessageWindow", this.m_messageWindow);
                AddTabPage(this.sDockBay1.MainPane, page, SDockDirection.Bottom);
                messageWindowToolStripMenuItem.Checked = true;
            }
            this.sDockBay1.MainPane.Refresh();
        }

        private void objectListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListToolStripMenuItem.Checked)
            {
                //ObjectListを閉じる
                resetTab(this.m_objectListTab);
                this.m_objectListTab.Dispose();
                objectListToolStripMenuItem.Checked = false;
            }
            else
            {
                SDockTabPage page = setSDockTab("ObjectList", this.m_objectList);
                AddTabPage(this.sDockBay1.MainPane, page, SDockDirection.Right);
                objectListToolStripMenuItem.Checked = true;
            }
            this.sDockBay1.MainPane.Refresh();
        }

        private void propertyWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (propertyWindowToolStripMenuItem.Checked)
            {
                //PropertyWindowを閉じる
                resetTab(this.m_propertyWindowTab);
                this.m_propertyWindowTab.Dispose();
                propertyWindowToolStripMenuItem.Checked = false;
            }
            else
            {
                SDockTabPage page = setSDockTab("PropertyWindow", this.m_propertyWindow);
                AddTabPage(this.sDockBay1.MainPane, page, SDockDirection.Right);
                propertyWindowToolStripMenuItem.Checked = true;
            }
            this.sDockBay1.MainPane.Refresh();
        }

        private void AddTabPage(SDockPane pane, SDockTabPage page, SDockDirection direction)
        {
            Debug.WriteLine("add tab " + page.Text);
            //新規Paneの作成
            SDockPane newPane = new SDockPane(this.sDockBay1);
            newPane.SuspendLayout();
            page.SuspendLayout();

            newPane.Add(page);
            this.sDockBay1.Add(newPane, pane, direction);

            page.ResumeLayout();
            newPane.ResumeLayout();
        }
        private void AddTabPage(SDockPane pane, SDockTabPage page)
        {
            Debug.WriteLine("add tab " + page.Text);
            pane.SuspendLayout();
            page.SuspendLayout();

            pane.Add(page);

            page.ResumeLayout();
            pane.ResumeLayout();
        }

        private void SetupIDEMenuClick(object sender, EventArgs e)
        {
            SetupIDEWindow win = new SetupIDEWindow();
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

    /// <summary>
    /// サンプルフォームの状態を保持しておくクラスです。
    /// </summary>
    [Serializable]
    [YukSerialize("sdmmbconf")] //この属性を付けない場合は、Type.FullNameプロパティが使用されます。難読化アセンブリ対応の属性です。
    public class MainFormState : IYukiSerializable
    {
        #region SDockBayConfig.Tagプロパティに関して
        /*
         * IYukiSerializableでbayをシリアル化する場合は、逆シリアル化に
         * IYukiSerializableインターフェースを実装していなければなりません。
         * Binary及びSoapシリアル化しか使用しない場合は
         * クラスの先頭にSerializable属性を付与しておけば良いだけなのですが、
         * IYukiSerializableでは今のところそうはいかないため、
         * このような仕様になっています。
         * 
         * Binary,Soapのみ利用する場合は空実装で問題ありません。
         * */
        #endregion

        public MainFormState()
        {
            mHoverSelRange = 2;
            mHoverSelTime = 2;
        }

        private Rectangle mFormRect;
        public Rectangle FormRect
        {
            get { return mFormRect; }
            set { mFormRect = value; }
        }

        private FormWindowState mWindowState;
        public FormWindowState WindowState
        {
            get { return mWindowState; }
            set { mWindowState = value; }
        }

        private bool mEnabledEventLog;
        public bool BlockLogging
        {
            get { return mEnabledEventLog; }
            set { mEnabledEventLog = value; }
        }


        private bool mEnabledHoverSelect;
        public bool EnabledHoverSelect
        {
            get { return mEnabledHoverSelect; }
            set { mEnabledHoverSelect = value; }
        }
        private int mHoverSelTime;
        public int HoverSelTime
        {
            get
            {
                if (mHoverSelTime < 0 || mHoverSelTime > 4) mHoverSelTime = 2;
                return mHoverSelTime;
            }
            set { mHoverSelTime = value; }
        }
        private int mHoverSelRange;
        public int HoverSelRange
        {
            get
            {
                if (mHoverSelRange < 0 || mHoverSelRange > 4) mHoverSelRange = 2;
                return mHoverSelRange;
            }
            set
            {
                mHoverSelRange = value;
            }
        }

        private int mMaxPageCount;
        public int MaxPageCount
        {
            get { return mMaxPageCount; }
            set { mMaxPageCount = Math.Max(0, value); }
        }

        #region ツールストリップ
        private Yukichika.Controls.ToolStripManagerEx mToolStripManager;
        public Yukichika.Controls.ToolStripManagerEx ToolStripManager
        {
            get { return mToolStripManager; }
            set { mToolStripManager = value; }
        }

        #endregion

        #region IYukiSerializable メンバ

        public void DeSerialize(ref YukSerializeNode node)
        {
            mFormRect = node.GetRectangle("formrect", Rectangle.Empty);

            mWindowState = (FormWindowState)Enum.ToObject(typeof(FormWindowState)
                        , node.GetInt("windowstate", (int)FormWindowState.Normal));

            mEnabledEventLog = node.GetBool("enabledeventlog", true);

            mHoverSelRange = node.GetInt("hoverselrange", 2);
            mHoverSelTime = node.GetInt("hoverseltime", 2);
            mEnabledHoverSelect = node.GetBool("enabledhoversel", false);

            mToolStripManager = node.GetObject("toolstrips", null) as ToolStripManagerEx;
        }

        public void Serialize(ref YukSerializeNode node)
        {
            node.Set(mFormRect, "formrect");
            node.Set(mWindowState, "windowstate");

            node.Set(mEnabledEventLog, "useeventlog");

            node.Set(mEnabledHoverSelect, "enabledhoversel");
            node.Set(mHoverSelTime, "hoverseltime");
            node.Set(mHoverSelRange, "hoverselrange");

            node.Set(mToolStripManager, "toolstrips");
        }

        #endregion
    }

}