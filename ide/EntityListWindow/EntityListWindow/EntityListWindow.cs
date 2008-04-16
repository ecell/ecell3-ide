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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;

using EcellLib;
using EcellLib.Plugin;
using EcellLib.Objects;

namespace EcellLib.EntityListWindow
{
    /// <summary>
    /// Plugin of EntityListWindow.
    /// </summary>
    public class EntityListWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// m_form (EntityList form) 
        /// </summary>
        private EntityList m_form = null;
        /// <summary>
        /// Dictionary of tree node for Models.
        /// </summary>
        private Dictionary<string, TreeNode> m_modelNodeDic = new Dictionary<string, TreeNode>();
        /// <summary>
        /// Dictionary of tree node for Simulation Parameters.
        /// </summary>
        private Dictionary<string, TreeNode> m_paramNodeDic = new Dictionary<string, TreeNode>();
        /// <summary>
        /// m_prjMenu (popup menu for tree node of project)
        /// </summary>
        private ContextMenu m_prjMenu;
        /// <summary>
        /// m_prjNoLoadMenu (popup menu for tree node of project)
        /// </summary>
        private ContextMenu m_prjLoadMenu;
        /// <summary>
        /// m_modelMenu (popup menu for tree node of model)
        /// </summary>
        private ContextMenu m_modelMenu;
        /// <summary>
        /// m_systemMenu (popup menu for tree node of system)
        /// </summary>
        private ContextMenu m_systemMenu;
        /// <summary>
        /// m_topSystemMenu (popup menu for tree node of system(/))
        /// </summary>
        private ContextMenu m_topSystemMenu;
        /// <summary>
        /// m_varMenu (popup menu for tree node for variable)
        /// </summary>
        private ContextMenu m_varMenu;
        /// <summary>
        /// m_procMenu (popup menu for tree node for process)
        /// </summary>
        private ContextMenu m_procMenu;
        /// <summary>
        /// m_dmMenu (popup menu for tree node for dm)
        /// </summary>
        private ContextMenu m_dmMenu;
        /// <summary>
        /// m_targetNode (selected tree node when show popup menu)
        /// </summary>
        private TreeNode m_targetNode;
        /// <summary>
        /// m_prjNode (project tree node in TreeView)
        /// </summary>
        private TreeNode m_prjNode;
        /// <summary>
        /// m_propDict (dictionary of property name and data type)
        /// </summary>
        private Dictionary<string, EcellData> m_propDict;
        /// <summary>
        /// editing object.
        /// </summary>
        private EcellObject m_currentObj;
        /// <summary>
        /// simple search.
        /// </summary>
        private SearchInstance m_searchWin;
        /// <summary>
        /// System status.
        /// </summary>
        private ProjectStatus m_type = ProjectStatus.Uninitialized;
        /// <summary>
        /// Cotext menu for create logger of process on popup menu.
        /// </summary>
        private MenuItem m_creProcLogger;
        /// <summary>
        /// Context menu for create logger of variable on popup menu.
        /// </summary>
        private MenuItem m_creVarLogger;
        /// <summary>
        /// Context menu for delete logger of process on popup menu.
        /// </summary>
        private MenuItem m_delProcLogger;
        /// <summary>
        /// Context menu for delete logger of variable on popup menu.
        /// </summary>
        private MenuItem m_delVarLogger;
        /// <summary>
        /// Context menu for create logger of system on popup menu.
        /// </summary>
        private MenuItem m_creSysLogger;
        /// <summary>
        /// Context menu for delete logger of system on popup menu.
        /// </summary>
        private MenuItem m_delSysLogger;
        /// <summary>
        /// Context menu for create logger of root system on popup menu.
        /// </summary>
        private MenuItem m_creTopSysLogger;
        /// <summary>
        /// Context menu for delete logger of root system on popup menu.
        /// </summary>
        private MenuItem m_delTopSysLogger;
        private MenuItem m_creVarParameterData;
        private MenuItem m_delVarParameterData;
        private MenuItem m_creProParameterData;
        private MenuItem m_delProParameterData;
        private MenuItem m_creVarObservedData;
        private MenuItem m_delVarObservedData;
        private MenuItem m_creProObservedData;
        private MenuItem m_delProObservedData;
        /// <summary>
        /// Cotext menu for merge of system on popup menu.
        /// </summary>
        private MenuItem m_merge;
        /// <summary>
        /// Cotext menu to compile dm on popup menu.
        /// </summary>
        private MenuItem m_compileDM;
        /// <summary>
        /// ComponentResourceManager for EntityListWindow.
        /// </summary>
        static public ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResEntList));
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for EntityListWindow.
        /// </summary>
        public EntityListWindow()
        {
            m_dManager = DataManager.GetDataManager();
            m_pManager = PluginManager.GetPluginManager();
            m_propDict = new Dictionary<string, EcellData>();

            m_prjMenu = new ContextMenu();
            m_prjLoadMenu = new ContextMenu();
            m_modelMenu = new ContextMenu();
            m_systemMenu = new ContextMenu();
            m_topSystemMenu = new ContextMenu();
            m_procMenu = new ContextMenu();
            m_varMenu = new ContextMenu();
            m_dmMenu = new ContextMenu();
            m_creSysLogger = new MenuItem();
            m_delSysLogger = new MenuItem();
            m_creTopSysLogger = new MenuItem();
            m_delTopSysLogger = new MenuItem();
            m_creProcLogger = new MenuItem();
            m_delProcLogger = new MenuItem();
            m_creVarLogger = new MenuItem();
            m_delVarLogger = new MenuItem();
            m_creVarParameterData = new MenuItem();
            m_delVarParameterData = new MenuItem();
            m_creProParameterData = new MenuItem();
            m_delProParameterData = new MenuItem();
            m_creVarObservedData = new MenuItem();
            m_delVarObservedData = new MenuItem();
            m_creProObservedData = new MenuItem();
            m_delProObservedData = new MenuItem();
            m_merge = new MenuItem();
            m_compileDM = new MenuItem();
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get the window form for EntityListWindow.
        /// This user control add the NodeMouseClick event action.
        /// </summary>
        /// <returns>UserControl.</returns>
        public override List<EcellDockContent> GetWindowsForms()
        {
            List<EcellDockContent> list = new List<EcellDockContent>();
            m_form = new EntityList();
            m_form.treeView1.NodeMouseClick +=
                new TreeNodeMouseClickEventHandler(this.NodeMouseClick);
            m_form.treeView1.NodeMouseDoubleClick +=
                new TreeNodeMouseClickEventHandler(this.NodeDoubleClick);
            m_form.treeView1.TreeViewNodeSorter = new TypeSorter();

            CreatePopupMenu();
            m_form.Text = "EntityList";
            list.Add(m_form);

            return list;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            ChangeObject(modelID, key, type);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public override void AddSelect(string modelID, string key, string type)
        {
            ChangeObject(modelID, key, type);
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            if (data == null) return;
            foreach (EcellObject obj in data)
            {
                if (obj.Type == Constants.xpathProject)
                {
                    m_prjNode = new TreeNode(obj.ModelID);
                    m_prjNode.Tag = new TagData("", "", Constants.xpathProject);
                    m_form.treeView1.Nodes.Add(m_prjNode);
                    TreeNode modelNode = new TreeNode("Models");
                    modelNode.Tag = null;
                    TreeNode paramNode = new TreeNode("Parameters");
                    paramNode.Tag = null;
                    TreeNode dmNode = new TreeNode("DMs");
                    dmNode.Tag = null;
                    m_prjNode.Nodes.Add(modelNode);
                    m_prjNode.Nodes.Add(paramNode);
                    m_prjNode.Nodes.Add(dmNode);
                    m_modelNodeDic.Add(obj.ModelID, modelNode);
                    m_paramNodeDic.Add(obj.ModelID, paramNode);

                    List<string> fileList = m_dManager.GetDMDirData();
                    foreach (string d in fileList)
                    {
                        TreeNode dNode = new TreeNode(d);
                        dNode.Tag = new TagData("", "", Constants.xpathDM);
                        dmNode.Nodes.Add(dNode);
                    }

                    continue;
                }
                else if (obj.Type == Constants.xpathModel)
                {
                    if (GetTargetModel(obj.ModelID) != null) continue;
                    TreeNode node = new TreeNode(obj.ModelID);
                    node.ImageIndex = m_pManager.GetImageIndex(obj.Type);
                    node.SelectedImageIndex = node.ImageIndex;
                    node.Tag = new TagData(obj.ModelID, "", Constants.xpathModel);
                    string currentPrj = m_dManager.CurrentProjectID;
                    if (m_modelNodeDic.ContainsKey(currentPrj))
                        m_modelNodeDic[currentPrj].Nodes.Add(node);
                    continue;
                }
                else if (obj.Type == Constants.xpathProcess || 
                    obj.Type ==Constants.xpathVariable)
                {
                    if (obj.Key.EndsWith(Constants.headerSize))
                        continue;
                    TreeNode current = GetTargetModel(obj.ModelID);
                    if (current == null)
                        return;
                    TreeNode node = GetTargetTreeNode(current, obj.Key, obj.Type);
                    if (node == null)
                    {
                        string path = "";
                        string name = Util.GetNameFromPath(obj.Key, ref path);
                        node = GetTargetTreeNode(current, path, null);

                        TreeNode childNode = AddTreeNode(name, obj, node);
                    }
                }
                else if (obj.Type == Constants.xpathSystem)
                {
                    TreeNode current = GetTargetModel(obj.ModelID);
                    if (current == null) return;
                    TreeNode node = GetTargetTreeNode(current, obj.Key, obj.Type);
                    if (node == null)
                    {
                        if (obj.Key == "/")
                        {
                            node = AddTreeNode(obj.Key, obj, current);
                        }
                        else
                        {
                            TreeNode target = null;
                            string path = "";
                            string name = Util.GetNameFromPath(obj.Key, ref path);
                            target = GetTargetTreeNode(current, path, null);

                            if (target != null)
                            {
                                node = AddTreeNode(name, obj, target);
                            }
                        }
                    }
                    if (node != null)
                    {
                        if (obj.Children == null) continue;
                        foreach (EcellObject eo in obj.Children)
                        {
                            if (eo.Type != Constants.xpathVariable && eo.Type != Constants.xpathProcess) continue;
                            if (eo.Key.EndsWith(Constants.headerSize)) continue;
                            string[] names = eo.Key.Split(new char[] { ':' });
                            IEnumerator iter = node.Nodes.GetEnumerator();
                            bool isHit = false;
                            while (iter.MoveNext())
                            {
                                TreeNode tmp = (TreeNode)iter.Current;
                                TagData tag = (TagData)tmp.Tag;
                                if (tmp.Text == names[names.Length - 1] &&
                                    tag.m_type == eo.Type)
                                {
                                    isHit = true;
                                    break;
                                }
                            }
                            if (isHit == true) continue;

                            TreeNode childNode = null;
                            childNode = AddTreeNode(names[names.Length - 1], eo, node);
                        }
                    }
                }
            }
            m_form.treeView1.Sort();
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public override void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null) return;
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target != null)
            {
                string path = "";
                string targetText = Util.GetNameFromPath(data.Key, ref path);

                if (target.Text != targetText)
                {
                    target.Text = targetText;
                }
                bool isLog = Util.IsLogged(data);
                if (isLog)
                {
                    target.Text = target.Text + "[logged]";
                }

                if (key != data.Key)
                {
                    TreeNode change = GetTargetTreeNode(current, path, Constants.xpathSystem);
                    if (change == null) return;
                    target.Parent.Nodes.Remove(target);
                    change.Nodes.Add(target);
                    TagData tag = (TagData)target.Tag;
                    tag.m_key = data.Key;
                    target.Tag = tag;
                    if (tag.m_type == Constants.xpathSystem)
                    {
                        IDChangeProvide(key, data.Key, target);
                    }
                    m_form.treeView1.Sort();
                }
            }
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null) return;
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target != null)
            {
                target.Remove();
            }
            return;
        }

        /// <summary>
        /// The event sequence when the simulation parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID.</param>
        public override void ParameterAdd(string projectID, string parameterID)
        {
            TreeNode paramsNode = null;
            if (m_paramNodeDic.ContainsKey(projectID))
            {
                paramsNode = m_paramNodeDic[projectID];
            }
            else
            {
                IEnumerator nodeEnumerator = m_form.treeView1.Nodes.GetEnumerator();
                while (nodeEnumerator.MoveNext())
                {
                    TreeNode project = (TreeNode)nodeEnumerator.Current;
                    if (project.Text.Equals(projectID))
                    {
                        paramsNode = new TreeNode(Constants.xpathParameters);
                        paramsNode.Tag = null;
                        project.Nodes.Add(paramsNode);
                        m_paramNodeDic.Add(projectID, paramsNode);
                        break;
                    }
                }
                if (paramsNode == null)
                {
                    return;
                }
            }

            foreach (TreeNode t in paramsNode.Nodes)
            {
                if (t.Text == parameterID) return;
            }
            TreeNode paramNode = new TreeNode(parameterID);
            paramNode.Tag = new TagData("", "", Constants.xpathParameters);
            paramsNode.Nodes.Add(paramNode);

            return;
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            IEnumerator nodeEnumerator = m_form.treeView1.Nodes.GetEnumerator();
            while (nodeEnumerator.MoveNext())
            {
                TreeNode project = (TreeNode)nodeEnumerator.Current;
                if (project != null)
                    project.Remove();
            }
            m_modelNodeDic.Clear();
            m_paramNodeDic.Clear();
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            m_type = type;
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>        
        public override Bitmap Print(string name)
        {
            if (m_form == null) return null;

            try
            {
                Bitmap bitmap = new Bitmap(m_form.treeView1.Width, m_form.treeView1.Height);
                m_form.treeView1.DrawToBitmap(bitmap, m_form.treeView1.ClientRectangle);
                return bitmap;
            }
            catch (Exception ex)
            {
                String errmese = EntityListWindow.s_resources.GetString("ErrPrintData");
                MessageBox.Show(errmese + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"EntityListWindow"</returns>
        public override string GetPluginName()
        {
            return "EntityListWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public override List<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            names.Add("TreeView of entity.");
            return names;
        }
        #endregion

        #region internal methods
        /// <summary>
        /// Get EntityList usercontrol.
        /// </summary>
        /// <returns>EntityList.</returns>
        internal EntityList GetForm()
        {
            return m_form;
        }

        /// <summary>
        /// Check whether E-Cell SDK is already installed.
        /// </summary>
        /// <returns>if E-Cell SDK is installed, retur true.</returns>
        private bool CheckInstalledSDK()
        {
            return false;
        }

        /// <summary>
        /// Create popup menu of each tree node type.
        /// </summary>
        private void CreatePopupMenu()
        {
            MenuItem addModel = new MenuItem();
            MenuItem addSystem = new MenuItem();
            MenuItem addVar = new MenuItem();
            MenuItem addProc = new MenuItem();
            MenuItem del = new MenuItem();
            MenuItem searchMenu = new MenuItem();
            MenuItem separator = new MenuItem("-");
            MenuItem sortNameMenu = new MenuItem();
            MenuItem sortTypeMenu = new MenuItem();

            m_creSysLogger.Text = EntityListWindow.s_resources.GetString("PopCreLoggerText");
            m_delSysLogger.Text = EntityListWindow.s_resources.GetString("PopDelLoggerText");
            m_creTopSysLogger.Text = EntityListWindow.s_resources.GetString("PopCreLoggerText");
            m_delTopSysLogger.Text = EntityListWindow.s_resources.GetString("PopDelLoggerText");
            m_creProcLogger.Text = EntityListWindow.s_resources.GetString("PopCreLoggerText");
            m_delProcLogger.Text = EntityListWindow.s_resources.GetString("PopDelLoggerText");
            m_creVarLogger.Text = EntityListWindow.s_resources.GetString("PopCreLoggerText");
            m_delVarLogger.Text = EntityListWindow.s_resources.GetString("PopDelLoggerText");
            m_merge.Text = EntityListWindow.s_resources.GetString("PopMergeText");
            m_compileDM.Text = EntityListWindow.s_resources.GetString("PopCompileText");
            m_creVarParameterData.Text = "Create parameter";
            m_creProParameterData.Text = "Create parameter";
            m_delVarParameterData.Text = "Remove parameter";
            m_delProParameterData.Text = "Remove parameter";
            m_creVarObservedData.Text = "Create observed";
            m_creProObservedData.Text = "Create observed";
            m_delVarObservedData.Text = "Remove observed";
            m_delProObservedData.Text = "Remove observed";
            addModel.Text = EntityListWindow.s_resources.GetString("PopAddModelText");
            addSystem.Text = EntityListWindow.s_resources.GetString("PopAddSystemText");
            addVar.Text = EntityListWindow.s_resources.GetString("PopAddVariableText");
            addProc.Text = EntityListWindow.s_resources.GetString("PopAddProcessText");
            del.Text = EntityListWindow.s_resources.GetString("PopDeleteText");
            searchMenu.Text = EntityListWindow.s_resources.GetString("PopSearchText");
            sortNameMenu.Text = EntityListWindow.s_resources.GetString("SortNameText");
            sortTypeMenu.Text = EntityListWindow.s_resources.GetString("SortTypeText");

            addModel.Click += new EventHandler(TreeviewAddModel);
            addSystem.Click += new EventHandler(TreeviewAddSystem);
            addVar.Click += new EventHandler(TreeviewAddVariable);
            addProc.Click += new EventHandler(TreeviewAddProcess);
            del.Click += new EventHandler(TreeviewDelete);
            m_merge.Click += new EventHandler(TreeviewMerge);
            m_compileDM.Click += new EventHandler(TreeViewCompile);
            searchMenu.Click += new EventHandler(TreeviewSearch);
            sortNameMenu.Click += new EventHandler(TreeViewSortName);
            sortTypeMenu.Click += new EventHandler(TreeViewSortType);

            searchMenu.Shortcut = Shortcut.CtrlF;

            m_prjMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    addModel.CloneMenu() ,
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu(),
                    sortNameMenu.CloneMenu(),
                    sortTypeMenu.CloneMenu()
                });
            m_prjLoadMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    searchMenu.CloneMenu(),
                    sortNameMenu.CloneMenu(),
                    sortTypeMenu.CloneMenu()
                });
            m_modelMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    del.CloneMenu(),
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu(),
                    sortNameMenu.CloneMenu(),
                    sortTypeMenu.CloneMenu()
                });
            m_systemMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    addSystem.CloneMenu(), 
                    addVar.CloneMenu(), 
                    addProc.CloneMenu(), 
                    separator.CloneMenu(),
                    del.CloneMenu(),
                    m_merge,
                    separator.CloneMenu(), 
                    m_creSysLogger, 
                    m_delSysLogger,
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu(),
                    sortNameMenu.CloneMenu(),
                    sortTypeMenu.CloneMenu()
                });
            m_topSystemMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    addSystem.CloneMenu(), 
                    addVar.CloneMenu(), 
                    addProc.CloneMenu(),
                    separator.CloneMenu(), 
                    m_creTopSysLogger, 
                    m_delTopSysLogger,
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu(),
                    sortNameMenu.CloneMenu(),
                    sortTypeMenu.CloneMenu()
                });
            m_varMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    del.CloneMenu(), 
                    separator.CloneMenu(),
                    m_creVarLogger, 
                    m_delVarLogger,
                    separator.CloneMenu(), 
                    m_creVarParameterData,
                    m_delVarParameterData,
                    separator.CloneMenu(), 
                    m_creVarObservedData,
                    m_delVarObservedData,
                    separator.CloneMenu(),
                    searchMenu.CloneMenu(),
                    sortNameMenu.CloneMenu(),
                    sortTypeMenu.CloneMenu()
                });
            m_procMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    del.CloneMenu() ,
                    separator.CloneMenu(),
                    m_creProcLogger, 
                    m_delProcLogger,
                    separator.CloneMenu(), 
                    m_creProParameterData,
                    m_delProParameterData,
                    separator.CloneMenu(), 
                    m_creProObservedData,
                    m_delProObservedData,
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu(),
                    sortNameMenu.CloneMenu(),
                    sortTypeMenu.CloneMenu()
                });
            m_dmMenu.MenuItems.AddRange(new MenuItem[]
            {
                m_compileDM.CloneMenu()
            });
        }

        /// <summary>
        /// Create the menu item of popup menu to set and reset the logger.
        /// </summary>
        /// <param name="creLogger">menu item to set the logger.</param>
        /// <param name="delLogger">menu item to reset the logger.</param>
        /// <param name="obj">object to display the popup menu.</param>
        private void CrateLoggerPopupMenu(MenuItem creLogger, MenuItem delLogger, EcellObject obj)
        {
            creLogger.MenuItems.Clear();
            delLogger.MenuItems.Clear();

            foreach (EcellData d in obj.Value)
            {
                if (d.Logable)
                {
                    MenuItem citem = new MenuItem(d.Name);
                    citem.Click += new EventHandler(TreeViewCreLogger);
                    creLogger.MenuItems.Add(citem.CloneMenu());
                }
                if (d.Logable && d.Logged)
                {
                    MenuItem item = new MenuItem(d.Name);
                    item.Click += new EventHandler(TreeViewDelLogger);
                    delLogger.MenuItems.Add(item.CloneMenu());
                }
            }
        }

        private void CreateParameterPopupMenu(MenuItem creLogger, MenuItem delLogger, EcellObject obj)
        {
            creLogger.MenuItems.Clear();
            delLogger.MenuItems.Clear();

            foreach (EcellData d in obj.Value)
            {
                if (!d.Settable || !d.Logable) continue;
                MenuItem item = new MenuItem(d.Name);
                item.Tag = d.EntityPath;
                if (m_dManager.IsContainsParameterData(d.EntityPath))
                {
                    item.Click += new EventHandler(TreeViewDelParameterData);
                    delLogger.MenuItems.Add(item);
                }
                else
                {
                    item.Click += new EventHandler(TreeViewCreParameterData);
                    creLogger.MenuItems.Add(item);
                }
            }
        }

        private void CreateObservedPopupMenu(MenuItem creLogger, MenuItem delLogger, EcellObject obj)
        {
            creLogger.MenuItems.Clear();
            delLogger.MenuItems.Clear();

            foreach (EcellData d in obj.Value)
            {
                if (!d.Logable) continue;
                MenuItem item = new MenuItem(d.Name);
                item.Tag = d.EntityPath;
                if (m_dManager.IsContainsObservedData(d.EntityPath))
                {
                    item.Click += new EventHandler(TreeViewDelObservedData);
                    delLogger.MenuItems.Add(item);
                }
                else
                {
                    item.Click += new EventHandler(TreeViewCreObservedData);
                    creLogger.MenuItems.Add(item);
                }
            }
        }

        /// <summary>
        /// Show property window displayed the selected object.
        /// </summary>
        /// <param name="obj">the selected object</param>
        private void ShowPropEditWindow(EcellObject obj)
        {
            PropertyEditor.Show(obj);
        }

        /// <summary>
        /// Change selected Object
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        private void ChangeObject(string modelID, string key, string type)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null) return;
            if (key == "")
            {
                m_form.treeView1.SelectedNode = current;
                return;
            }
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target == null)
            {
                m_form.treeView1.SelectedNode = current;
                return;
            }
            m_form.treeView1.SelectedNode = target;
        }

        /// <summary>
        /// Get the current selected tree node.
        /// </summary>
        /// <returns></returns>
        internal TreeNode GetSelectedNode()
        {
            return m_form.treeView1.SelectedNode;
        }

        /// <summary>
        /// Select the current selected tree node.
        /// </summary>
        internal void SetSelectedNode()
        {
            TreeNode node = m_form.treeView1.SelectedNode;
            TagData tag = (TagData)node.Tag;
            m_pManager.SelectChanged(tag.m_modelID, tag.m_key, tag.m_type);
        }

        /// <summary>
        /// Add the tree node to TreeView.
        /// </summary>
        /// <param name="name">the tree name.</param>
        /// <param name="obj">the added EcellObject.</param>
        /// <param name="parent">the parent tree node.</param>
        /// <returns>Created TreeNode.</returns>
        private TreeNode AddTreeNode(string name, EcellObject obj, TreeNode parent)
        {
            TreeNode node = null;
            bool isLog = Util.IsLogged(obj);
            if (isLog) node = new TreeNode(name + "[logged]");
            else node = new TreeNode(name);

            node.ImageIndex = m_pManager.GetImageIndex(obj.Type);
            node.SelectedImageIndex = node.ImageIndex;
            node.Tag = new TagData(obj.ModelID, obj.Key, obj.Type);
            parent.Nodes.Add(node);

            return node;
        }

        /// <summary>
        /// Search tree node from current node.
        /// </summary>
        /// <param name="node">current node</param>
        /// <param name="text">search condition</param>
        /// <returns></returns>
        internal bool SearchNode(TreeNode node, string text)
        {
            bool result = false;
            foreach (TreeNode t in node.Nodes) // childs
            {
                TagData tag = (TagData)t.Tag;
                if (tag.m_key.Contains(text))
                {
                    m_form.treeView1.SelectedNode = t;
                    return true;
                }
                result = SearchNode(t, text);
                if (result) return true;
            }
            if (node.NextNode != null) // brothers
            {
                TagData tag = (TagData)node.NextNode.Tag;
                if (tag.m_key.Contains(text))
                {
                    m_form.treeView1.SelectedNode = node.NextNode;
                    return true;
                }
                result = SearchNode(node.NextNode, text);
                if (result) return true;
            }

            TreeNode parentNode = null;
            while (true) // search no checked parent 
            {
                if (node.Parent == null) break;
                if (node.Parent.NextNode != null)
                {
                    parentNode = node.Parent.NextNode;
                    break;
                }
                else
                {
                    node = node.Parent;
                }
            }

            if (parentNode == null)
            {
                return false;
            }
            else
            {
                TagData tag = (TagData)parentNode.Tag;
                if (tag != null && tag.m_key.Contains(text))
                {
                    m_form.treeView1.SelectedNode = parentNode;
                    return true;
                }
                return SearchNode(parentNode, text);
            }
        }

        /// <summary>
        /// Get the object from TreeNode.
        /// </summary>
        /// <param name="node">Target TreeNode.</param>
        /// <returns>EcellObject.</returns>
        internal EcellObject GetObjectFromNode(TreeNode node)
        {
            TagData t = (TagData)node.Tag;
            if (t == null) return null;

            EcellObject obj = m_dManager.GetEcellObject(t.m_modelID, t.m_key, t.m_type);
            return obj;
        }

        /// <summary>
        /// Get the path of selected node.
        /// </summary>
        /// <param name="path">Target node path.</param>
        /// <param name="src">Target node.</param>
        /// <returns>string(path)</returns>
        internal string GetCurrentPath(string path, TreeNode src)
        {
            if (src.Parent == null) return null;
            TagData tag = (TagData)src.Parent.Tag;
            if (tag == null) return null;
            if (tag.m_type == Constants.xpathModel)
            {
                path = src.Text + path; // top is "/"
                return path;
            }
            path = src.Text + "/" + path;
            return this.GetCurrentPath(path, src.Parent);
        }

        /// <summary>
        /// Get the mode ID of selected node.
        /// </summary>
        /// <param name="src">Target node</param>
        /// <returns>string(model name)</returns>
        internal string GetParentModelName(TreeNode src)
        {
            if (src.Parent == null) return null;
            TreeNode node = src;

            while (node != null)
            {
                TagData tag = (TagData)node.Tag;
                if (tag == null) return null;
                if (tag.m_type == Constants.xpathModel)
                {
                    return node.Text;
                }
                node = node.Parent;
            }
            return null;
        }

        /// <summary>
        /// Get the tree node of model.
        /// </summary>
        /// <param name="modelID">Target model ID.</param>
        /// <returns>TreeNode</returns>
        internal TreeNode GetTargetModel(string modelID)
        {
            string currentPrj = m_dManager.CurrentProjectID;
            if (m_modelNodeDic.ContainsKey(currentPrj))
            {
                TreeNode modelsNode = m_modelNodeDic[currentPrj];
                foreach (TreeNode t in modelsNode.Nodes)
                {
                    TagData tag = t.Tag as TagData;
                    if (tag.m_type == Constants.xpathModel && t.Text == modelID)
                        return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the tree node of target node with key ID.
        /// </summary>
        /// <param name="current">Current TreeNode.</param>
        /// <param name="key">Target ID.</param>
        /// <param name="type">Target data type.</param>
        /// <returns>TreeNode(target node)</returns>
        internal TreeNode GetTargetTreeNode(TreeNode current, string key, string type)
        {
            string[] keydata;
            if (current.Nodes.Count <= 0) return null;

            keydata = key.Split(new Char[] { '/' });
            if (keydata.Length == 0) return null;

            if (keydata[0].Contains(":"))
            {
                string[] keys = keydata[0].Split(new char[] { ':' });
                if (keydata[0].StartsWith(":")) keydata[0] = keys[keys.Length - 1];
                else keydata = keydata[0].Split(new char[] { ':' });
            }

            IEnumerator nodes = current.Nodes.GetEnumerator();
            while (nodes.MoveNext())
            {
                TreeNode node = (TreeNode)nodes.Current;
                TagData tag = (TagData)node.Tag;
                String tmpText = node.Text;
                tmpText = tmpText.Replace("[logged]", "");
                if ((tmpText == keydata[0])
                    || (keydata[0] == "" && tmpText == "/"))
                {
                    if ((keydata.Length == 1 || key == "/") &&
                        (tag.m_type == type || type == null)) 
                        return node;
                    if (keydata.Length == 1 || key == "/") continue;
                    key = keydata[1];
                    for (int i = 2; i < keydata.Length; i++)
                    {
                        key = key + "/" + keydata[i];
                    }
                    return GetTargetTreeNode(node, key, type);
                }
            }
            return null;
        }

        /// <summary>
        /// Change the id of child node if the id of parent node is changed.
        /// </summary>
        /// <param name="oldKey">the old id of parent node.</param>
        /// <param name="newKey">the new id of parent node.</param>
        /// <param name="node">the current node.</param>
        private void IDChangeProvide(string oldKey, string newKey, TreeNode node)
        {
            foreach (TreeNode t in node.Nodes)
            {
                TagData tag = t.Tag as TagData;
                if (tag == null) continue;
                if (tag.m_type == Constants.xpathSystem)
                {
                    IDChangeProvide(oldKey + "/" + t.Text,
                        newKey + "/" + t.Text, t);
                    tag.m_key = newKey + "/" + t.Text;
                    continue;
                }
                tag.m_key = newKey + ":" + t.Text;
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// The action of selecting [Create Parameter] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewCreParameterData(object sender, EventArgs e)
        {
            MenuItem m = sender as MenuItem;
            string key = m.Tag as string;
            if (key == null) return;

            EcellObject obj = m_dManager.GetEcellObject(
                    m_currentObj.ModelID,
                    m_currentObj.Key,
                    m_currentObj.Type);

            // set logger
            foreach (EcellData d in obj.Value)
            {
                if (key.Equals(d.EntityPath))
                {
                    m_dManager.SetParameterData(new EcellParameterData(key, Convert.ToDouble(d.Value.ToString())));
                    return;
                }
            }
        }

        /// <summary>
        /// The action of selecting [Delete Parameter] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewDelParameterData(object sender, EventArgs e)
        {
            MenuItem m = sender as MenuItem;
            string key = m.Tag as string;
            if (key == null) return;

            m_dManager.RemoveParameterData(new EcellParameterData(key, 0.0));
        }

        /// <summary>
        /// The action of selecting [Create Observed] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewCreObservedData(object sender, EventArgs e)
        {
            MenuItem m = sender as MenuItem;
            string key = m.Tag as string;
            if (key == null) return;

            EcellObject obj = m_dManager.GetEcellObject(
                    m_currentObj.ModelID,
                    m_currentObj.Key,
                    m_currentObj.Type);

            // set logger
            foreach (EcellData d in obj.Value)
            {
                if (key.Equals(d.EntityPath))
                {
                    m_dManager.SetObservedData(new EcellObservedData(key, Convert.ToDouble(d.Value.ToString())));
                    return;
                }
            }
        }

        /// <summary>
        /// The action of selecting [Delete Observed] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewDelObservedData(object sender, EventArgs e)
        {
            MenuItem m = sender as MenuItem;
            string key = m.Tag as string;
            if (key == null) return;

            m_dManager.RemoveObservedData(new EcellObservedData(key, 0.0));
        }

        /// <summary>
        /// The action of selecting [Create Logger] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewCreLogger(object sender, EventArgs e)
        {
            MenuItem m = (MenuItem)sender;
            string prop = m.Text;
            // Get EcellObject
            EcellObject obj = m_dManager.GetEcellObject(
                m_currentObj.ModelID,
                m_currentObj.Key,
                m_currentObj.Type);

            // set logger
            foreach (EcellData d in obj.Value)
            {
                if (prop.Equals(d.Name))
                {
                    PluginManager.GetPluginManager().LoggerAdd(
                        m_currentObj.ModelID,
                        m_currentObj.Key,
                        m_currentObj.Type,
                        d.EntityPath);
                    d.Logged = true;
                }
            }
            // modify changes
            m_dManager.DataChanged(obj.ModelID,
                obj.Key,
                obj.Type,
                obj);
        }

        /// <summary>
        /// The action of selecting [Delete Logger] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewDelLogger(object sender, EventArgs e)
        {
            MenuItem m = (MenuItem)sender;
            string prop = m.Text;
            // Get EcellObject
            EcellObject obj = m_dManager.GetEcellObject(
                m_currentObj.ModelID,
                m_currentObj.Key,
                m_currentObj.Type);

            // delete logger
            foreach (EcellData d in obj.Value)
            {
                if (prop.Equals(d.Name))
                {
                    d.Logged = false;
                }
            }
            // modify changes
            m_dManager.DataChanged(obj.ModelID,
                obj.Key,
                obj.Type,
                obj);
        }

        /// <summary>
        /// The action of [Add Model] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddModel(object sender, EventArgs e)
        {
            ShowPropEditWindow(m_currentObj);
        }

        /// <summary>
        /// The action of [Add System] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddSystem(object sender, EventArgs e)
        {
            m_dManager.CreateDefaultObject(m_currentObj.ModelID, m_currentObj.Key, Constants.xpathSystem, true);
        }

        /// <summary>
        /// The action of [Add Variable] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddVariable(object sender, EventArgs e)
        {
            m_dManager.CreateDefaultObject(m_currentObj.ModelID, m_currentObj.Key, Constants.xpathVariable, true);
        }

        /// <summary>
        /// The action of [Add Process] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddProcess(object sender, EventArgs e)
        {
            m_dManager.CreateDefaultObject(m_currentObj.ModelID, m_currentObj.Key, Constants.xpathProcess, true);
        }

        /// <summary>
        /// The action of [Delete] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewDelete(object sender, EventArgs e)
        {
            string modelID = null;
            string key = null;
            string type = null;
            TagData tag = (TagData)m_targetNode.Tag;
            if (tag == null) return;
            if (m_targetNode.Parent != null)
            {
                TagData parentTag = (TagData)m_targetNode.Parent.Tag;
                if (parentTag != null)
                {
                    modelID = parentTag.m_modelID;
                    key = parentTag.m_key;
                    type = parentTag.m_type;
                }
            }

            try
            {
                if (tag.m_type == Constants.xpathModel) 
                    m_dManager.DataDelete(tag.m_modelID, null, Constants.xpathModel);
                else 
                    m_dManager.DataDelete(tag.m_modelID, tag.m_key, tag.m_type);
                if (modelID != null) m_pManager.SelectChanged(modelID, key, type);
            }
            catch (Exception ex)
            {
                String errmes = EntityListWindow.s_resources.GetString("ErrDelData");
                MessageBox.Show(errmes + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// The action of [Delete with component] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewMerge(object sender, EventArgs e)
        {
            string modelID = null;
            string key = null;
            string type = null;
            TagData tag = (TagData)m_targetNode.Tag;
            if (tag == null) return;
            if (m_targetNode.Parent != null)
            {
                TagData parentTag = (TagData)m_targetNode.Parent.Tag;
                if (parentTag != null)
                {
                    modelID = parentTag.m_modelID;
                    key = parentTag.m_key;
                    type = parentTag.m_type;
                }
            }

            try
            {
                if (tag.m_type == Constants.xpathModel) 
                    m_dManager.DataDelete(tag.m_modelID, null, Constants.xpathModel);
                else if (tag.m_type == Constants.xpathSystem) 
                    m_dManager.SystemDeleteAndMove(tag.m_modelID, tag.m_key);
                else m_dManager.DataDelete(tag.m_modelID, tag.m_key, tag.m_type);
//                if (modelID != null) m_pManager.SelectChanged(modelID, key, type);
            }
            catch (Exception ex)
            {
                String errmes = EntityListWindow.s_resources.GetString("ErrDelData");
                MessageBox.Show(errmes + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// The action of clicking [Compile] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs.</param>
        private void TreeViewCompile(object sender, EventArgs e)
        {
            string path = m_dManager.GetDMFileName(m_targetNode.Text);
            if (!CheckInstalledSDK())
            {
                String errmes = EntityListWindow.s_resources.GetString("ErrNotInstallSDK");
                MessageBox.Show(errmes + "\n",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // not implement.
        }

        /// <summary>
        /// The action of clicking [Sort by Name] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewSortName(object sender, EventArgs e)
        {
            m_form.treeView1.TreeViewNodeSorter = new NameSorter();
            m_form.treeView1.Sort();
        }

        /// <summary>
        /// The action of clicking [Sort by Type] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewSortType(object sender, EventArgs e)
        {
            m_form.treeView1.TreeViewNodeSorter = new TypeSorter();
            m_form.treeView1.Sort();
        }

        /// <summary>
        /// The action of clicking [search] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeviewSearch(object sender, EventArgs e)
        {
            if (m_form.treeView1.Nodes.Count <= 0) return;

            m_searchWin = new SearchInstance();
            m_searchWin.SetPlugin(this);
            m_searchWin.SISearchButton.Click += new EventHandler(m_searchWin.SearchButtonClick);
            m_searchWin.searchText.KeyPress += new KeyPressEventHandler(m_searchWin.SearchTextKeyPress);

            m_searchWin.ShowDialog();
        }

        /// <summary>
        /// The action of double clicking TreeNode on EntityListWindow.
        /// </summary>
        /// <param name="sender">TreeView</param>
        /// <param name="e">TreeNodeMouseClickEventArgs</param>
        void NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView t = (TreeView)sender;
            TreeNode node = t.SelectedNode;
            if (node == null) return;
            TagData tag = (TagData)node.Tag;
            if (tag == null) return;
            if (tag.m_modelID == null) return;
            if (tag.m_type == Constants.xpathParameters)
            {
                m_pManager.SelectChanged("", node.Text, tag.m_type);
                return;
            }
            if (tag.m_type == Constants.xpathDM)
            {
                string path = m_dManager.GetDMFileName(node.Text);
                if (path == null) return;
                try
                {
                    Process p = Process.Start(path);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    String errmes = s_resources.GetString("ErrStartupApp");
                    MessageBox.Show(errmes + "\n\n" + path,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                return;
            }

            m_targetNode = node;
            if (m_type != ProjectStatus.Uninitialized &&
                m_type != ProjectStatus.Loaded)
                return;
            try
            {
                EcellObject obj = m_dManager.GetEcellObject(tag.m_modelID,
                    tag.m_key, tag.m_type);
                if (obj == null)
                {
                    String errmes = EntityListWindow.s_resources.GetString("ErrGetData");
                    MessageBox.Show(
                    errmes + "(" + tag.m_modelID + "," + tag.m_key + ")",
                    "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ShowPropEditWindow(obj);
                return;
            }
            catch (Exception ex)
            {
                String errmes = EntityListWindow.s_resources.GetString("ErrGetData");
                MessageBox.Show(errmes + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// The action after the node mouse click.
        /// If click right button on tree node, show popup menu and
        /// If click left button on tree node, change selected tree node.
        /// </summary>
        /// <param name="sender">object(TreeView)</param>
        /// <param name="e">TreeNodeMouseClickEventArgs</param>
        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeView t = (TreeView)sender;
            if (t == null) return;
            TreeNode node = t.GetNodeAt(e.X, e.Y);
            if (node == null) return;
            TagData tag = (TagData)node.Tag;
            if (tag == null) return;
            if (tag.m_type == Constants.xpathParameters) return;
            if (e.Button == MouseButtons.Right)
            {
                if (m_type != ProjectStatus.Loaded && m_type != ProjectStatus.Stepping)
                {
                    m_form.treeView1.ContextMenu = null;
                    return;
                }
                if (tag.m_type == Constants.xpathDM)
                {
                    m_targetNode = node;
                    string file = m_dManager.GetDMFileName(node.Text);
                    if (file != null)
                        m_form.treeView1.ContextMenu = m_dmMenu;
                    else
                        m_form.treeView1.ContextMenu = null;
                    return;
                }
                
                if (tag.m_type == Constants.xpathProject)
                {
                    m_targetNode = node;
                    if (m_type == ProjectStatus.Uninitialized)
                        m_form.treeView1.ContextMenu = m_prjMenu;
                    else
                        m_form.treeView1.ContextMenu = m_prjLoadMenu;
                }
                else if (tag.m_type == Constants.xpathModel)
                {
                    m_targetNode = node;
                    m_form.treeView1.ContextMenu = m_modelMenu;
                }
                else if (tag.m_type == Constants.xpathSystem)
                {
                    m_targetNode = node;
                    List<EcellObject> list = m_dManager.GetData(tag.m_modelID, tag.m_key);
                    if (list == null || list.Count == 0) return;
                    EcellObject obj = list[0];

                    if (node.Text == "/")
                    {
                        CrateLoggerPopupMenu(m_creTopSysLogger, m_delTopSysLogger, obj);
                        m_form.treeView1.ContextMenu = m_topSystemMenu;
                    }
                    else
                    {
                        CrateLoggerPopupMenu(m_creSysLogger, m_delSysLogger, obj);
                        String superSys = tag.m_key.Substring(0, tag.m_key.LastIndexOf("/"));
                        if (superSys == "") superSys = "/";
                        m_merge.Text = EntityListWindow.s_resources.GetString("PopMergeText") + "(" + superSys + ")";
                        m_form.treeView1.ContextMenu = m_systemMenu;
                    }
                    m_currentObj = obj;
                }
                else if (tag.m_type == Constants.xpathVariable)
                {
                    m_targetNode = node;
                    EcellObject obj = GetObjectFromNode(node);
                    CrateLoggerPopupMenu(m_creVarLogger, m_delVarLogger, obj);
                    CreateParameterPopupMenu(m_creVarParameterData, m_delVarParameterData, obj);
                    CreateObservedPopupMenu(m_creVarObservedData, m_delVarObservedData, obj);
                    m_currentObj = obj;
                    m_form.treeView1.ContextMenu = m_varMenu;
                }
                else if (tag.m_type == Constants.xpathProcess)
                {
                    m_targetNode = node;
                    EcellObject obj = GetObjectFromNode(node);
                    CrateLoggerPopupMenu(m_creProcLogger, m_delProcLogger, obj);
                    CreateParameterPopupMenu(m_creProParameterData, m_delProParameterData, obj);
                    CreateObservedPopupMenu(m_creProObservedData, m_delProObservedData, obj);
                    m_currentObj = obj;
                    m_form.treeView1.ContextMenu = m_procMenu;
                }
                else
                {
                    m_form.treeView1.ContextMenu = null;
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (tag.m_type != Constants.xpathProject)
                {
                    PluginManager.GetPluginManager().SelectChanged(tag.m_modelID, tag.m_key, tag.m_type);
                    m_form.treeView1.SelectedNode = node;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Tag Object with the node in EntityListWindow.
    /// </summary>
    public class TagData
    {
        /// <summary>
        /// m_modelID (model ID of tree node tag) 
        /// </summary>
        public string m_modelID;
        /// <summary>
        /// m_key (key ID of tree node tag)
        /// </summary>
        public string m_key;
        /// <summary>
        /// m_type (type ID of tree node tag)
        /// </summary>
        public string m_type;

        /// <summary>
        /// constructor for TagData.
        /// </summary>
        public TagData()
        {
            this.m_modelID = "";
            this.m_key = "";
            this.m_type = "project";
        }

        /// <summary>
        /// constructor for TagData with initial value.
        /// </summary>
        /// <param name="modelID">the initial model ID</param>
        /// <param name="key">the initial key ID</param>
        /// <param name="type">the initial type ID</param>
        public TagData(string modelID, string key, string type)
        {
            this.m_modelID = modelID;
            this.m_key = key;
            this.m_type = type;
        }
    }

    /// <summary>
    /// Sort class by name of object.
    /// </summary>
    public class NameSorter : IComparer
    {
        /// <summary>
        /// Compare with two object by name.
        /// </summary>
        /// <param name="x">compared object.</param>
        /// <param name="y">compare object.</param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            return string.Compare(tx.Text, ty.Text);
        }
    }

    /// <summary>
    /// Sort class by type of object.
    /// </summary>
    public class TypeSorter : IComparer
    {
        /// <summary>
        /// Compare with two object.
        /// The first, system sort by the type of object.
        /// The second, system sort by the name of object.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            TagData tagx = tx.Tag as TagData;
            TagData tagy = ty.Tag as TagData;

            if (tagx == null && tagy == null) return string.Compare(tx.Text, ty.Text);
            if (tagx == null) return 1;
            if (tagy == null) return -1;
            if (tagx.m_type == tagy.m_type)
            {
                return string.Compare(tx.Text, ty.Text);
            }
            return GetTypeNum(tagx.m_type) - GetTypeNum(tagy.m_type);
        }

        /// <summary>
        /// Get the number of type.
        /// </summary>
        /// <param name="type">type of object.</param>
        /// <returns>type number.</returns>
        public int GetTypeNum(string type)
        {
            switch (type)
            {
                case Constants.xpathProject:
                    return 0;
                case Constants.xpathModel:
                    return 1;
                case Constants.xpathSystem:
                    return 2;
                case Constants.xpathProcess:
                    return 3;
                case Constants.xpathVariable:
                    return 4;
            }
            return 5;
        }
    }
}