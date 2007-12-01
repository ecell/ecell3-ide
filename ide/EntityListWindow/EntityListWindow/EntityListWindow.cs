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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;

using EcellLib;

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
        /// m_pManager (PluginManager)
        /// </summary>
        private PluginManager m_pManager;
        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        private DataManager m_dManager;
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
        MenuItem m_creProcLogger;
        /// <summary>
        /// Context menu for create logger of variable on popup menu.
        /// </summary>
        MenuItem m_creVarLogger;
        /// <summary>
        /// Context menu for delete logger of process on popup menu.
        /// </summary>
        MenuItem m_delProcLogger;
        /// <summary>
        /// Context menu for delete logger of variable on popup menu.
        /// </summary>
        MenuItem m_delVarLogger;
        /// <summary>
        /// Context menu for create logger of system on popup menu.
        /// </summary>
        MenuItem m_creSysLogger;
        /// <summary>
        /// Context menu for delete logger of system on popup menu.
        /// </summary>
        MenuItem m_delSysLogger;
        /// <summary>
        /// Context menu for create logger of root system on popup menu.
        /// </summary>
        MenuItem m_creTopSysLogger;
        /// <summary>
        /// Context menu for delete logger of root system on popup menu.
        /// </summary>
        MenuItem m_delTopSysLogger;
        /// <summary>
        /// Cotext menu for merge of system on popup menu.
        /// </summary>
        MenuItem m_merge;
        /// <summary>
        /// ComponentResourceManager for EntityListWindow.
        /// </summary>
        static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResEntList));
        #endregion

        /// <summary>
        /// Constructor for EntityListWindow.
        /// </summary>
        public EntityListWindow()
        {
            m_dManager = DataManager.GetDataManager();
            m_propDict = new Dictionary<string, EcellData>();
        }

        /// <summary>
        /// Get EntityList usercontrol.
        /// </summary>
        /// <returns>EntityList.</returns>
        public EntityList GetForm()
        {
            return m_form;
        }

        /// <summary>
        /// Create popup menu of each tree node type.
        /// </summary>
        private void CreatePopupMenu()
        {
            m_pManager = PluginManager.GetPluginManager();

            m_prjMenu = new ContextMenu();
            m_prjLoadMenu = new ContextMenu();
            m_modelMenu = new ContextMenu();
            m_systemMenu = new ContextMenu();
            m_topSystemMenu = new ContextMenu();
            m_procMenu = new ContextMenu();
            m_varMenu = new ContextMenu();

            MenuItem addModel = new MenuItem();
            MenuItem addSystem = new MenuItem();
            MenuItem addVar = new MenuItem();
            MenuItem addProc = new MenuItem();
            MenuItem del = new MenuItem();
            MenuItem searchMenu = new MenuItem();
            MenuItem separator = new MenuItem("-");
            MenuItem sortNameMenu = new MenuItem();
            MenuItem sortTypeMenu = new MenuItem();
            m_creSysLogger = new MenuItem();
            m_delSysLogger = new MenuItem();
            m_creTopSysLogger = new MenuItem();
            m_delTopSysLogger = new MenuItem();
            m_creProcLogger = new MenuItem();
            m_delProcLogger = new MenuItem();
            m_creVarLogger = new MenuItem();
            m_delVarLogger = new MenuItem();
            m_merge = new MenuItem();
            m_creSysLogger.Text = EntityListWindow.s_resources.GetString("PopCreLoggerText");
            m_delSysLogger.Text = EntityListWindow.s_resources.GetString("PopDelLoggerText");
            m_creTopSysLogger.Text = EntityListWindow.s_resources.GetString("PopCreLoggerText");
            m_delTopSysLogger.Text = EntityListWindow.s_resources.GetString("PopDelLoggerText");
            m_creProcLogger.Text = EntityListWindow.s_resources.GetString("PopCreLoggerText");
            m_delProcLogger.Text = EntityListWindow.s_resources.GetString("PopDelLoggerText");
            m_creVarLogger.Text = EntityListWindow.s_resources.GetString("PopCreLoggerText");
            m_delVarLogger.Text = EntityListWindow.s_resources.GetString("PopDelLoggerText");
            m_merge.Text = EntityListWindow.s_resources.GetString("PopMergeText");
            addModel.Text = EntityListWindow.s_resources.GetString("PopAddModelText");
            addSystem.Text = EntityListWindow.s_resources.GetString("PopAddSystemText");
            addVar.Text = EntityListWindow.s_resources.GetString("PopAddVariableText");
            addProc.Text = EntityListWindow.s_resources.GetString("PopAddProcessText");
            del.Text = EntityListWindow.s_resources.GetString("PopDeleteText");
            searchMenu.Text = EntityListWindow.s_resources.GetString("PopSearchText");
            sortNameMenu.Text = EntityListWindow.s_resources.GetString("SortNameText");
            sortTypeMenu.Text = EntityListWindow.s_resources.GetString("SortTypeText");

            addModel.Index = 1;
            addSystem.Index = 2;
            addVar.Index = 3;
            addProc.Index = 4;
            del.Index = 6;
            m_merge.Index = 6;
            m_creProcLogger.Index = 9;
            m_delProcLogger.Index = 10;
            m_creVarLogger.Index = 9;
            m_delVarLogger.Index = 10;
            searchMenu.Index = 20;

            addModel.Click += new EventHandler(TreeviewAddModel);
            addSystem.Click += new EventHandler(TreeviewAddSystem);
            addVar.Click += new EventHandler(TreeviewAddVariable);
            addProc.Click += new EventHandler(TreeviewAddProcess);
            del.Click += new EventHandler(TreeviewDelete);
            m_merge.Click += new EventHandler(TreeviewMerge);
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
                    searchMenu.CloneMenu(),
                    sortNameMenu.CloneMenu(),
                    sortTypeMenu.CloneMenu()
                });
        }

        /// <summary>
        /// Show property window displayed the selected object.
        /// </summary>
        /// <param name="obj">the selected object</param>
        public void ShowPropEditWindow(EcellObject obj)
        {
            PropertyEditor.Show(obj);
        }

        /// <summary>
        /// Get the current selected tree node.
        /// </summary>
        /// <returns></returns>
        public TreeNode GetSelectedNode()
        {
            return m_form.treeView1.SelectedNode;
        }

        /// <summary>
        /// Select the current selected tree node.
        /// </summary>
        public void SetSelectedNode()
        {
            TreeNode node = m_form.treeView1.SelectedNode;
            TagData tag = (TagData)node.Tag;
            m_pManager.SelectChanged(tag.m_modelID, tag.m_key, tag.m_type);
        }

        /// <summary>
        /// Search tree node from current node.
        /// </summary>
        /// <param name="node">current node</param>
        /// <param name="text">search condition</param>
        /// <returns></returns>
        public bool SearchNode(TreeNode node, string text)
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
                if (tag.m_key.Contains(text))
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
        public EcellObject GetObjectFromNode(TreeNode node)
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
        public string GetCurrentPath(string path, TreeNode src)
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
        public string GetParentModelName(TreeNode src)
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
        public TreeNode GetTargetModel(string modelID)
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
        public TreeNode GetTargetTreeNode(TreeNode current, string key, string type)
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

        #region Event
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
                m_currentObj.modelID,
                m_currentObj.key,
                m_currentObj.type);

            // set logger
            foreach (EcellData d in obj.Value)
            {
                if (prop.Equals(d.Name))
                {
                    PluginManager.GetPluginManager().LoggerAdd(
                        m_currentObj.modelID,
                        m_currentObj.key,
                        m_currentObj.type,
                        d.EntityPath);
                    d.Logged = true;
                }
            }
            // modify changes
            m_dManager.DataChanged(obj.modelID,
                obj.key,
                obj.type,
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
                m_currentObj.modelID,
                m_currentObj.key,
                m_currentObj.type);

            // delete logger
            foreach (EcellData d in obj.Value)
            {
                if (prop.Equals(d.Name))
                {
                    d.Logged = false;
                }
            }
            // modify changes
            m_dManager.DataChanged(obj.modelID,
                obj.key,
                obj.type,
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
            m_dManager.CreateDefaultSystem(m_currentObj.modelID, m_currentObj.key);
        }

        /// <summary>
        /// The action of [Add Variable] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddVariable(object sender, EventArgs e)
        {
            m_dManager.CreateDefaultVariable(m_currentObj.modelID, m_currentObj.key);
        }

        /// <summary>
        /// The action of [Add Process] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddProcess(object sender, EventArgs e)
        {
            m_dManager.CreateDefaultProcess(m_currentObj.modelID, m_currentObj.key);
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
//                m_targetNode.Remove();
                if (tag.m_type == "Model") m_dManager.DataDelete(tag.m_modelID, null, "Model");
                else m_dManager.DataDelete(tag.m_modelID, tag.m_key, tag.m_type);
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
                //                m_targetNode.Remove();
                if (tag.m_type == "Model") m_dManager.DataDelete(tag.m_modelID, null, "Model");
                else if (tag.m_type == "System") m_dManager.SystemDeleteAndMove(tag.m_modelID, tag.m_key);
                else m_dManager.DataDelete(tag.m_modelID, tag.m_key, tag.m_type);
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

        void DoubleClick(object sender, EventArgs e)
        {
            TreeView t = (TreeView)sender;
            TreeNode node = t.SelectedNode;
            if (node == null) return;
            TagData tag = (TagData)node.Tag;
            if (tag == null) return;
            if (tag.m_modelID == null) return;
            m_targetNode = node;

        }

        /// <summary>
        /// The action of double clicking TreeNode on EntityListWindow.
        /// </summary>
        /// <param name="sender">TreeView</param>
        /// <param name="e">TreeNodeMouseClickEventArgs</param>
        void NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            List<EcellObject> list;
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
                    m_creSysLogger.MenuItems.Clear();
                    m_delSysLogger.MenuItems.Clear();
                    m_creTopSysLogger.MenuItems.Clear();
                    m_delTopSysLogger.MenuItems.Clear();
                    foreach (EcellData d in obj.Value)
                    {
                        //                        if (d.Logable && !d.Logged)
                        //                        {
                        if (d.Logable)
                        {
                            MenuItem citem = new MenuItem(d.Name);
                            citem.Click += new EventHandler(TreeViewCreLogger);
                            m_creSysLogger.MenuItems.Add(citem.CloneMenu());
                            m_creTopSysLogger.MenuItems.Add(citem.CloneMenu());
                        }
                        if (d.Logable && d.Logged)
                        {
                            MenuItem item = new MenuItem(d.Name);
                            item.Click += new EventHandler(TreeViewDelLogger);
                            m_delSysLogger.MenuItems.Add(item.CloneMenu());
                            m_delTopSysLogger.MenuItems.Add(item.CloneMenu());
                        }
                    }
                    if (node.Text == "/")
                    {
                        m_form.treeView1.ContextMenu = m_topSystemMenu;
                    }
                    else
                    {
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
                    m_creVarLogger.MenuItems.Clear();
                    m_delVarLogger.MenuItems.Clear();
                    foreach (EcellData d in obj.Value)
                    {
//                        if (d.Logable && !d.Logged)
//                        {
                        if (d.Logable)
                        {
                            MenuItem citem = new MenuItem(d.Name);
                            citem.Click += new EventHandler(TreeViewCreLogger);
                            m_creVarLogger.MenuItems.Add(citem.CloneMenu());
                        }
                        if (d.Logable && d.Logged)
                        {
                            MenuItem item = new MenuItem(d.Name);
                            item.Click += new EventHandler(TreeViewDelLogger);
                            m_delVarLogger.MenuItems.Add(item.CloneMenu());
                        }
                    }
                    m_currentObj = obj;
                    m_form.treeView1.ContextMenu = m_varMenu;
                }
                else if (tag.m_type == Constants.xpathProcess)
                {
                    m_targetNode = node;
                    EcellObject obj = GetObjectFromNode(node);
                    m_creProcLogger.MenuItems.Clear();
                    m_delProcLogger.MenuItems.Clear();
                    foreach (EcellData d in obj.Value)
                    {
//                        if (d.Logable && !d.Logged)
                        if (d.Logable)
                        {
                            MenuItem citem = new MenuItem(d.Name);
                            citem.Click += new EventHandler(TreeViewCreLogger);
                            m_creProcLogger.MenuItems.Add(citem.CloneMenu());
                        }
                        if (d.Logable && d.Logged)
                        {
                            MenuItem item = new MenuItem(d.Name);
                            item.Click += new EventHandler(TreeViewDelLogger);
                            m_delProcLogger.MenuItems.Add(item.CloneMenu());
                        }
                    }
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

        #region PluginBase
        /// <summary>
        /// Get menustrips for EntityListWindow plugin.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get toolbar buttons for EntityListWindow plugin.
        /// </summary>
        /// <returns>null</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the window form for EntityListWindow.
        /// This user control add the NodeMouseClick event action.
        /// </summary>
        /// <returns>UserControl.</returns>
        public List<DockContent> GetWindowsForms()
        {
            List<DockContent> list = new List<DockContent>();
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
        public void SelectChanged(string modelID, string key, string type)
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
            foreach (EcellObject obj in data)
            {
                if (obj.type == Constants.xpathProject)
                {
                    m_prjNode = new TreeNode(obj.modelID);
                    m_prjNode.Tag = new TagData("", "", "Project");
                    m_form.treeView1.Nodes.Add(m_prjNode);
                    TreeNode modelNode = new TreeNode("Models");
                    modelNode.Tag = null;
                    TreeNode paramNode = new TreeNode("Parameters");
                    paramNode.Tag = null;
                    m_prjNode.Nodes.Add(modelNode);
                    m_prjNode.Nodes.Add(paramNode);
                    m_modelNodeDic.Add(obj.modelID, modelNode);
                    m_paramNodeDic.Add(obj.modelID, paramNode);
                    continue;
                }
                else if (obj.type == Constants.xpathModel)
                {
                    if (GetTargetModel(obj.modelID) != null) continue;
                    TreeNode node = new TreeNode(obj.modelID);
                    node.ImageIndex = m_pManager.GetImageIndex(obj.type);
                    node.SelectedImageIndex = node.ImageIndex;
                    node.Tag = new TagData(obj.modelID, "", "Model");
                    string currentPrj = m_dManager.CurrentProjectID;
                    if (m_modelNodeDic.ContainsKey(currentPrj))
                        m_modelNodeDic[currentPrj].Nodes.Add(node);
//                    m_prjNode.Nodes.Add(node);
                    continue;
                }
                else if (obj.type == Constants.xpathProcess || obj.type ==Constants.xpathVariable)
                {
                    if (obj.key.EndsWith(Constants.headerSize)) continue;
                    TreeNode current = GetTargetModel(obj.modelID);
                    if (current == null) return;
                    TreeNode node = GetTargetTreeNode(current, obj.key, obj.type);
                    if (node == null)
                    {
                        string[] names = obj.key.Split(new char[] { ':' });
                        string path = names[0];
                        node = GetTargetTreeNode(current, path, null);

                        bool isLog = false;
                        foreach (EcellData d in obj.Value)
                        {
                            if (d.Logged)
                            {
                                isLog = true;
                                break;
                            }
                        }
                        TreeNode childNode = null;
                        if (isLog)
                            childNode = new TreeNode(names[names.Length - 1] + "[logged]");
                        else
                            childNode = new TreeNode(names[names.Length - 1]);
                        childNode.ImageIndex = m_pManager.GetImageIndex(obj.type);
                        childNode.SelectedImageIndex = childNode.ImageIndex;
                        childNode.Tag = new TagData(obj.modelID, obj.key, obj.type);
                        node.Nodes.Add(childNode);
                    }
                }
                else if (obj.type == Constants.xpathSystem)
                {
                    TreeNode current = GetTargetModel(obj.modelID);
                    if (current == null) return;
                    TreeNode node = GetTargetTreeNode(current, obj.key, obj.type);
                    if (node == null)
                    {
                        if (obj.key == "/")
                        {
                            bool isLog = false;
                            foreach (EcellData d in obj.Value)
                            {
                                if (d.Logged)
                                {
                                    isLog = true;
                                    break;
                                }
                            }
                            if (isLog)
                                node = new TreeNode(obj.key + "[logged]");
                            else
                                node = new TreeNode(obj.key);

                            node.ImageIndex = m_pManager.GetImageIndex(obj.type);
                            node.SelectedImageIndex = node.ImageIndex;
                            node.Tag = new TagData(obj.modelID, obj.key, obj.type);
                            current.Nodes.Add(node);
                        }
                        else
                        {
                            string path = "";
                            TreeNode target;
                            string[] elements;
                            if (obj.key.StartsWith("/"))
                            {
                                elements = obj.key.Split(new Char[] { '/' });
                                for (int i = 1; i < elements.Length - 1; i++)
                                {
                                    path = path + "/" + elements[i];
                                }
                                target = GetTargetTreeNode(current, path, null);
                            }
                            else
                            {
                                elements = obj.key.Split(new Char[] { '/' });
                                if (elements.Length > 1)
                                {
                                    path = elements[0];
                                    for (int i = 1; i < elements.Length - 1; i++)
                                    {
                                        path = path + "/" + elements[i];
                                    }
                                    target = GetTargetTreeNode(current, path, null);
                                }
                                else
                                {
                                    target = current;
                                }
                            }

                            if (target != null)
                            {
                                bool isLog = false;
                                foreach (EcellData d in obj.Value)
                                {
                                    if (d.Logged)
                                    {
                                        isLog = true;
                                        break;
                                    }
                                }

                                if (isLog)
                                    node = new TreeNode(elements[elements.Length - 1] + "[logged]");
                                else
                                    node = new TreeNode(elements[elements.Length - 1]);
                                node.ImageIndex = m_pManager.GetImageIndex(obj.type);
                                node.SelectedImageIndex = node.ImageIndex;
                                node.Tag = new TagData(obj.modelID, obj.key, obj.type);
                                target.Nodes.Add(node);
                            }
                        }
                    }
                    if (node != null)
                    {
                        if (obj.Children == null) continue;
                        foreach (EcellObject eo in obj.Children)
                        {
                            if (eo.type != Constants.xpathVariable && eo.type != Constants.xpathProcess) continue;
                            if (eo.key.EndsWith(Constants.headerSize)) continue;
                            string[] names = eo.key.Split(new char[] { ':' });
                            IEnumerator iter = node.Nodes.GetEnumerator();
                            bool isHit = false;
                            while (iter.MoveNext())
                            {
                                TreeNode tmp = (TreeNode)iter.Current;
                                TagData tag = (TagData)tmp.Tag;
                                if (tmp.Text == names[names.Length - 1] &&
                                    tag.m_type == eo.type)
                                {
                                    isHit = true;
                                    break;
                                }
                            }
                            if (isHit == true) continue;

                            bool isLog = false;
                            foreach (EcellData d in eo.Value)
                            {
                                if (d.Logged)
                                {
                                    isLog = true;
                                    break;
                                }
                            }

                            TreeNode childNode = null;
                            if (isLog)
                                childNode = new TreeNode(names[names.Length - 1] + "[logged]");
                            else
                                childNode = new TreeNode(names[names.Length - 1]);

                            childNode.ImageIndex = m_pManager.GetImageIndex(eo.type);
                            childNode.SelectedImageIndex = childNode.ImageIndex;
                            childNode.Tag = new TagData(eo.modelID, eo.key, eo.type);
                            node.Nodes.Add(childNode);
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
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null) return;
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target != null)
            {
                string path = "";
                string[] elements;
                string targetText;
                if (data.key.Contains(":"))
                {
                    elements = data.key.Split(new char[] { ':' });
                    path = elements[0];
                    for (int i = 1; i < elements.Length - 1; i++)
                    {
                        path = path + ":" + elements[i];
                    }
                    targetText = elements[elements.Length - 1];
                }
                else
                {
                    if (data.key == "/")
                    {
                        path = "/";
                        targetText = "/";
                    }
                    else
                    {
                        elements = data.key.Split(new char[] { '/' });
                        for (int i = 1; i < elements.Length - 1; i++)
                        {
                            path = path + "/" + elements[i];
                        }
                        targetText = elements[elements.Length - 1];
                    }
                }
                if (target.Text != targetText)
                {
                    target.Text = targetText;
                }
                bool isLog = false;
                foreach (EcellData d in data.Value)
                {
                    if (d.Logged)
                    {
                        isLog = true;
                        break;
                    }
                }
                if (isLog)
                {
                    target.Text = target.Text + "[logged]";
                }

                if (key != data.key)
                {
                    TreeNode change = GetTargetTreeNode(current, path, "System");
                    if (change == null) return;
                    target.Parent.Nodes.Remove(target);
                    change.Nodes.Add(target);
                    TagData tag = (TagData)target.Tag;
                    tag.m_key = data.key;
                    target.Tag = tag;
                    if (tag.m_type == Constants.xpathSystem)
                    {
                        IDChangeProvide(key, data.key, target);
                    }
                    m_form.treeView1.Sort();
                }
//                m_form.treeView1.SelectedNode = target;
            }
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
            // nothing
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
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
        public void ParameterAdd(string projectID, string parameterID)
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
        /// The event sequence when the simulation parameter is deleted.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterDelete(string projectID, string parameterID)
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
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
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
        public void ChangeStatus(ProjectStatus type)
        {
            m_type = type;
        }

        /// <summary>
        /// Change availability of undo/redo status.
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        /// <summary>
        /// Save the selected model to directory.
        /// </summary>
        /// <param name="modelID">selected model.</param>
        /// <param name="directory">output directory.</param>
        public void SaveModel(string modelID, string directory)
        {
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>        
        public Bitmap Print()
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
        public string GetPluginName()
        {
            return "EntityListWindow";
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
        /// cCeck whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false(this plugin is EntityListWindow)</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public bool IsEnablePrint()
        {
            return true;
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