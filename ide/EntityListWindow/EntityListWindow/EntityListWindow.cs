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
using System.Text.RegularExpressions;

using EcellLib;

namespace EcellLib.EntityListWindow
{
    public class EntityListWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// m_form (EntityList form) 
        /// </summary>
        private EntityList m_form = null;
        /// <summary>
        /// m_panel (the panel that show this plugin in MainWindow)
        /// </summary>
        private Panel m_panel = null;
        /// <summary>
        /// m_pManager (PluginManager)
        /// </summary>
        private PluginManager m_pManager;
        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        private DataManager m_dManager;
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
        /// m_editor (editable property list window)
        /// </summary>
        private PropertyEditor m_editor;
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
        private int m_type = 0;
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

            MenuItem addModel = new MenuItem("Add Model");
            MenuItem addSystem = new MenuItem("Add System");
            MenuItem addVar = new MenuItem("Add Variable");
            MenuItem addProc = new MenuItem("Add Process");
            MenuItem del = new MenuItem("Delete");
            m_creProcLogger = new MenuItem("Create Logger");
            m_delProcLogger = new MenuItem("Delete Logger");
            m_creSysLogger = new MenuItem("Create Logger");
            m_delSysLogger = new MenuItem("Delete Logger");
            m_creTopSysLogger = new MenuItem("Create Logger");
            m_delTopSysLogger = new MenuItem("Delete Logger");
            m_creVarLogger = new MenuItem("Create Logger");
            m_delVarLogger = new MenuItem("Delete Logger");
            MenuItem searchMenu = new MenuItem("Search ...");
            MenuItem separator = new MenuItem("-");

            addModel.Index = 1;
            addSystem.Index = 3;
            addVar.Index = 5;
            addProc.Index = 7;
            del.Index = 8;
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
            searchMenu.Click += new EventHandler(TreeviewSearch);

            searchMenu.Shortcut = Shortcut.CtrlF;

            m_prjMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    addModel.CloneMenu() ,
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu()
                });
            m_prjLoadMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    searchMenu.CloneMenu()
                });
            m_modelMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    del.CloneMenu(),
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu()
                });
            m_systemMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    addSystem.CloneMenu(), 
                    addVar.CloneMenu(), 
                    addProc.CloneMenu(), 
                    separator.CloneMenu(),
                    del.CloneMenu(),
                    separator.CloneMenu(), 
                    m_creSysLogger, 
                    m_delSysLogger,
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu()
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
                    searchMenu.CloneMenu()
                });
            m_varMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    del.CloneMenu(), 
                    separator.CloneMenu(),
                    m_creVarLogger, 
                    m_delVarLogger,
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu()
                });
            m_procMenu.MenuItems.AddRange(new MenuItem[] 
                { 
                    del.CloneMenu() ,
                    separator.CloneMenu(),
                    m_creProcLogger, 
                    m_delProcLogger,
                    separator.CloneMenu(), 
                    searchMenu.CloneMenu()
                });
        }

        /// <summary>
        /// Show property window displayed the selected object.
        /// </summary>
        /// <param name="obj">the selected object</param>
        public void ShowPropEditWindow(EcellObject obj)
        {
            try
            {
                m_editor = new PropertyEditor();
                m_editor.layoutPanel.SuspendLayout();
                m_editor.SetCurrentObject(obj);
                m_editor.SetDataType(obj.type);
                m_editor.button1.Click += new EventHandler(m_editor.UpdateProperty);
                m_editor.LayoutPropertyEditor();
                m_editor.layoutPanel.ResumeLayout(false);
                m_editor.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to show property editor.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_editor.Dispose();
                return;
            }
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
            string[] keys = t.m_key.Split(new char[] { ':' });
            List<EcellObject> list = m_dManager.GetData(t.m_modelID, keys[0]);
            if (list == null || list.Count == 0)
            {
                MessageBox.Show(
                "Can't find data in DataManager [" + t.m_modelID + "," + t.m_key + "]",
                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            for (int i = 0; i < list.Count; i++)
            {
                List<EcellObject> insList = list[i].M_instances;
                if (insList == null || insList.Count == 0) continue;
                for (int j = 0; j < insList.Count; j++)
                {
                    if (insList[j].key == t.m_key && insList[j].type == t.m_type)
                    {
                        return insList[j];
                    }
                }
            }
            return null;
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
            if (tag.m_type == "Model")
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
                if (tag.m_type == "Model")
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
            IEnumerator nodeEnumerator = m_form.treeView1.Nodes.GetEnumerator();
            while (nodeEnumerator.MoveNext())
            {
                TreeNode project = (TreeNode)nodeEnumerator.Current;
                IEnumerator models = project.Nodes.GetEnumerator();
                if (models == null) continue;
                while (models.MoveNext())
                {
                    TreeNode current = (TreeNode)models.Current;
                    TagData tag = (TagData)current.Tag;
                    if (tag.m_type == "Model" && current.Text == modelID)
                    {
                        return current;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get the tree node of target node with key ID.
        /// </summary>
        /// <param name="current">Current TreeNode.</param>
        /// <param name="key">Target ID.</param>
        /// <param name="key">Target data type.</param>
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

            EcellObject obj = EcellObject.CreateObject(m_currentObj.modelID,
                m_currentObj.key,
                m_currentObj.type,
                m_currentObj.classname,
                new List<EcellData>());

            foreach (EcellData d in m_currentObj.M_value)
            {
                if (d.M_name == prop)
                {
                    PluginManager.GetPluginManager().LoggerAdd(
                        m_currentObj.modelID,
                        m_currentObj.key,
                        m_currentObj.type,
                        d.M_entityPath);
                    if (d.M_isLogger == true) return;
                    d.M_isLogger = true;
                }
                obj.M_value.Add(d);
            }

            m_dManager.DataChanged(m_currentObj.modelID,
                m_currentObj.key,
                m_currentObj.type,
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

            EcellObject obj = EcellObject.CreateObject(m_currentObj.modelID,
                m_currentObj.key,
                m_currentObj.type,
                m_currentObj.classname,
                new List<EcellData>());

            foreach (EcellData d in m_currentObj.M_value)
            {
                if (d.M_name == prop)
                {
                    d.M_isLogger = false;
                }
                obj.M_value.Add(d);
            }

            m_dManager.DataChanged(m_currentObj.modelID,
                m_currentObj.key,
                m_currentObj.type,
                obj);
        }

        /// <summary>
        /// The action of [Add Model] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddModel(object sender, EventArgs e)
        {
            try
            {
                m_editor = new PropertyEditor();
                m_editor.SetParentObject(m_currentObj);
                m_editor.SetDataType("Model");
                m_editor.button1.Click += new EventHandler(m_editor.AddModel);
                m_editor.LayoutPropertyEditor();
                m_editor.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to show property editor.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// The action of [Add System] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddSystem(object sender, EventArgs e)
        {
            try
            {
                m_editor = new PropertyEditor();
                m_editor.SetParentObject(m_currentObj);
                m_editor.SetDataType("System");
                m_editor.button1.Click += new EventHandler(m_editor.AddSystem);
                m_editor.LayoutPropertyEditor();
                m_editor.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to show property editor.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        /// <summary>
        /// The action of [Add Variable] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddVariable(object sender, EventArgs e)
        {
            try
            {
                m_editor = new PropertyEditor();
                m_editor.SetParentObject(m_currentObj);
                m_editor.SetDataType("Variable");
                m_editor.button1.Click += new EventHandler(m_editor.AddNodeElement);
                m_editor.LayoutPropertyEditor();
                m_editor.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to show property editor.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        /// <summary>
        /// The action of [Add Process] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddProcess(object sender, EventArgs e)
        {
            try
            {
                m_editor = new PropertyEditor();
                m_editor.layoutPanel.SuspendLayout();
                m_editor.SetParentObject(m_currentObj);
                m_editor.SetDataType("Process");
                m_editor.button1.Click += new EventHandler(m_editor.AddNodeElement);
                m_editor.m_propName = "ExpressionFluxProcess";
                m_editor.LayoutPropertyEditor();
                m_editor.layoutPanel.ResumeLayout(false);
                m_editor.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to show property editor.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
                m_targetNode.Remove();
                if (tag.m_type == "Model") m_dManager.DataDelete(tag.m_modelID, null, "Model");
                else m_dManager.DataDelete(tag.m_modelID, tag.m_key, tag.m_type);
                if (modelID != null) m_pManager.SelectChanged(modelID, key, type);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get exception while deleting data.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            m_searchWin.searchButton.Click += new EventHandler(m_searchWin.SearchButtonClick);
            m_searchWin.closeButton.Click += new EventHandler(m_searchWin.SearchCloseButtonClick);
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
            m_targetNode = node;

            if (m_type != Util.NOTLOAD && m_type != Util.LOADED) return;
            try
            {
                if (tag.m_key.Contains(":"))
                { // not system
                    string[] keys = tag.m_key.Split(new char[] { ':' });
                    list = m_dManager.GetData(tag.m_modelID, keys[0]);
                    if (list == null || list.Count == 0)
                    {
                        MessageBox.Show(
                        "Can't find data in DataManager [" + tag.m_modelID + "," + tag.m_key + "]",
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        List<EcellObject> insList = list[i].M_instances;
                        if (insList == null || insList.Count == 0) continue;
                        for (int j = 0; j < insList.Count; j++)
                        {
                            if (insList[j].key == tag.m_key && insList[j].type == tag.m_type)
                            {
                                ShowPropEditWindow(insList[j]);
                                return;
                            }
                        }
                    }
                }
                else
                { // system
                    list = m_dManager.GetData(tag.m_modelID, tag.m_key);
                    if (list == null || list.Count == 0) return;
                    ShowPropEditWindow(list[0]);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to show property window.\n\n" + ex,
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
            if (e.Button == MouseButtons.Right)
            {
                if (m_type != Util.LOADED && m_type != Util.STEP)
                {
                    m_form.treeView1.ContextMenu = null;
                    return;
                }
                
                if (tag.m_type == "Project")
                {
                    m_targetNode = node;
                    if (m_type == Util.NOTLOAD)
                        m_form.treeView1.ContextMenu = m_prjMenu;
                    else
                        m_form.treeView1.ContextMenu = m_prjLoadMenu;
                }
                else if (tag.m_type == "Model")
                {
                    m_targetNode = node;
                    m_form.treeView1.ContextMenu = m_modelMenu;
                }
                else if (tag.m_type == "System")
                {
                    m_targetNode = node;
                    List<EcellObject> list = m_dManager.GetData(tag.m_modelID, tag.m_key);
                    if (list == null || list.Count == 0) return;
                    EcellObject obj = list[0];
                    m_creSysLogger.MenuItems.Clear();
                    m_delSysLogger.MenuItems.Clear();
                    m_creTopSysLogger.MenuItems.Clear();
                    m_delTopSysLogger.MenuItems.Clear();
                    foreach (EcellData d in obj.M_value)
                    {
                        //                        if (d.M_isLogable && !d.M_isLogger)
                        //                        {
                        if (d.M_isLogable)
                        {
                            MenuItem citem = new MenuItem(d.M_name);
                            citem.Click += new EventHandler(TreeViewCreLogger);
                            m_creSysLogger.MenuItems.Add(citem.CloneMenu());
                            m_creTopSysLogger.MenuItems.Add(citem.CloneMenu());
                        }
                        if (d.M_isLogable && d.M_isLogger)
                        {
                            MenuItem item = new MenuItem(d.M_name);
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
                        m_form.treeView1.ContextMenu = m_systemMenu;
                    }
                    m_currentObj = obj;
                }
                else if (tag.m_type == "Variable")
                {
                    m_targetNode = node;
                    EcellObject obj = GetObjectFromNode(node);
                    m_creVarLogger.MenuItems.Clear();
                    m_delVarLogger.MenuItems.Clear();
                    foreach (EcellData d in obj.M_value)
                    {
//                        if (d.M_isLogable && !d.M_isLogger)
//                        {
                        if (d.M_isLogable)
                        {
                            MenuItem citem = new MenuItem(d.M_name);
                            citem.Click += new EventHandler(TreeViewCreLogger);
                            m_creVarLogger.MenuItems.Add(citem.CloneMenu());
                        }
                        if (d.M_isLogable && d.M_isLogger)
                        {
                            MenuItem item = new MenuItem(d.M_name);
                            item.Click += new EventHandler(TreeViewDelLogger);
                            m_delVarLogger.MenuItems.Add(item.CloneMenu());
                        }
                    }
                    m_currentObj = obj;
                    m_form.treeView1.ContextMenu = m_varMenu;
                }
                else if (tag.m_type == "Process")
                {
                    m_targetNode = node;
                    EcellObject obj = GetObjectFromNode(node);
                    m_creProcLogger.MenuItems.Clear();
                    m_delProcLogger.MenuItems.Clear();
                    foreach (EcellData d in obj.M_value)
                    {
//                        if (d.M_isLogable && !d.M_isLogger)
                        if (d.M_isLogable)
                        {
                            MenuItem citem = new MenuItem(d.M_name);
                            citem.Click += new EventHandler(TreeViewCreLogger);
                            m_creProcLogger.MenuItems.Add(citem.CloneMenu());
                        }
                        if (d.M_isLogable && d.M_isLogger)
                        {
                            MenuItem item = new MenuItem(d.M_name);
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
                if (tag.m_type != "Project")
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
        public List<UserControl> GetWindowsForms()
        {
            List<UserControl> array = new List<UserControl>();
            m_form = new EntityList();
            m_form.treeView1.NodeMouseClick +=
                new TreeNodeMouseClickEventHandler(this.NodeMouseClick);
            m_form.treeView1.NodeMouseDoubleClick +=
                new TreeNodeMouseClickEventHandler(this.NodeDoubleClick);

            CreatePopupMenu();
            array.Add(m_form);

            return array;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="key">Selected the data type.</param>
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
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            foreach (EcellObject obj in data)
            {
                if (obj.type == "Project")
                {
                    m_prjNode = new TreeNode(obj.modelID);
                    m_prjNode.Tag = new TagData("", "", "Project");
                    m_form.treeView1.Nodes.Add(m_prjNode);
                    continue;
                }
                else if (obj.type == "Model")
                {
                    if (GetTargetModel(obj.modelID) != null) continue;
                    TreeNode node = new TreeNode(obj.modelID);
                    node.ImageIndex = m_pManager.GetImageIndex(obj.type);
                    node.SelectedImageIndex = node.ImageIndex;
                    node.Tag = new TagData(obj.modelID, "", "Model");
                    m_prjNode.Nodes.Add(node);
                    continue;
                }
                else if (obj.type == "Process" || obj.type == "Variable")
                {
                    if (obj.key.EndsWith("SIZE")) continue;
                    TreeNode current = GetTargetModel(obj.modelID);
                    if (current == null) return;
                    TreeNode node = GetTargetTreeNode(current, obj.key, obj.type);
                    if (node == null)
                    {
                        string[] names = obj.key.Split(new char[] { ':' });
                        string path = names[0];
                        node = GetTargetTreeNode(current, path, null);

                        bool isLog = false;
                        foreach (EcellData d in obj.M_value)
                        {
                            if (d.M_isLogger)
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
                else if (obj.type == "System")
                {
                    TreeNode current = GetTargetModel(obj.modelID);
                    if (current == null) return;
                    TreeNode node = GetTargetTreeNode(current, obj.key, obj.type);
                    if (node == null)
                    {
                        if (obj.key == "/")
                        {
                            bool isLog = false;
                            foreach (EcellData d in obj.M_value)
                            {
                                if (d.M_isLogger)
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
                                foreach (EcellData d in obj.M_value)
                                {
                                    if (d.M_isLogger)
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
                        if (obj.M_instances == null) continue;
                        foreach (EcellObject eo in obj.M_instances)
                        {
                            if (eo.type != "Variable" && eo.type != "Process") continue;
                            if (eo.key.EndsWith("SIZE")) continue;
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
                            foreach (EcellData d in eo.M_value)
                            {
                                if (d.M_isLogger)
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
                foreach (EcellData d in data.M_value)
                {
                    if (d.M_isLogger)
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
                }
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
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="key">The data type generating warning data.</param>
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
            m_type = type;
        }

        public void SaveModel(string model, string directory)
        {
        }

        /// <summary>
        /// Set the panel that show this plugin in MainWindow.
        /// </summary>
        /// <param name="panel">The set panel.</param>
        public void SetPanel(Panel panel)
        {
            this.m_panel = panel;
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
                MessageBox.Show("Fail to create bitmap data.\n\n" + ex,
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
}