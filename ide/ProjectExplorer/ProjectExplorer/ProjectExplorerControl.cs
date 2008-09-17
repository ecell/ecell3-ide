using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.ProjectExplorer
{
    public partial class ProjectExplorerControl : EcellDockContent
    {
        /// <summary>
        /// Dictionary of tree node for Models.
        /// </summary>
        private Dictionary<string, TreeNode> m_modelNodeDic = new Dictionary<string, TreeNode>();
        /// <summary>
        /// Dictionary of tree node for Simulation Parameters.
        /// </summary>
        private Dictionary<string, TreeNode> m_paramNodeDic = new Dictionary<string, TreeNode>();
        /// <summary>
        /// Project tree node in TreeView
        /// </summary>
        private TreeNode m_prjNode;
        /// <summary>
        /// DM tree node in TreeView
        /// </summary>
        private TreeNode m_DMNode;
        /// <summary>
        /// DM tree node in TreeView
        /// </summary>
        private TreeNode m_logNode;
        /// <summary>
        /// Last selecte dnode
        /// </summary>
        private TreeNode m_lastSelectedNode;
        /// <summary>
        /// Dictionary of property name and data type
        /// </summary>
        private Dictionary<string, EcellData> m_propDict;
        /// <summary>
        /// DataManager
        /// </summary>
        ProjectExplorer m_owner;

        System.Collections.IComparer m_nameSorter;

        System.Collections.IComparer m_typeSorter;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProjectExplorerControl(ProjectExplorer owner)
        {
            m_owner = owner;
            base.m_isSavable = true;
            InitializeComponent();
            m_nameSorter = new NameSorter();
            m_typeSorter = new TypeSorter();
            treeView1.TreeViewNodeSorter = m_typeSorter;
            treeView1.Environment = m_owner.Environment;
            toolStripButtonSortByName.Checked = false;
            toolStripButtonSortByType.Checked = true;
            this.Text = MessageResources.ProjectExplorer;
            this.TabText = this.Text;
            this.treeView1.ImageList = m_owner.Environment.PluginManager.NodeImageList;
            m_propDict = new Dictionary<string, EcellData>();
            m_lastSelectedNode = null;
        }


        /// <summary>
        /// Create the menu item of popup menu to set and reset the logger.
        /// </summary>
        /// <param name="obj">object to display the popup menu.</param>
        private ToolStripItem[] CreateLoggerPopupMenu(EcellObject obj)
        {
            List<ToolStripItem> retval = new List<ToolStripItem>();
            foreach (EcellData d in obj.Value)
            {
                if (d.Logable)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(d.Name);
                    item.Tag = d.Name;
                    item.Click += new EventHandler(TreeViewCreLogger);
                    item.Checked = d.Logged;
                    retval.Add(item);
                }
            }
            return retval.ToArray();
        }

        private ToolStripItem[] CreateParameterPopupMenu(EcellObject obj)
        {
            List<ToolStripItem> retval = new List<ToolStripItem>();
            foreach (EcellData d in obj.Value)
            {
                if (!d.Settable || !d.Value.IsDouble)
                    continue;
                ToolStripMenuItem item = new ToolStripMenuItem(d.Name);
                item.Tag = d.Name;
                item.Checked = m_owner.Environment.DataManager.IsContainsParameterData(d.EntityPath);
                item.Click += new EventHandler(TreeViewCreParameterData);
                retval.Add(item);
            }
            return retval.ToArray();
        }

        private ToolStripItem[] CreateObservedPopupMenu(EcellObject obj)
        {
            List<ToolStripItem> retval = new List<ToolStripItem>();

            foreach (EcellData d in obj.Value)
            {
                if (!d.Logable) continue;
                ToolStripMenuItem item = new ToolStripMenuItem(d.Name);
                item.Tag = d.Name;
                item.Checked = m_owner.Environment.DataManager.IsContainsObservedData(d.EntityPath);
                item.Click += new EventHandler(TreeViewCreObservedData);
                retval.Add(item);
            }

            return retval.ToArray();
        }

        /// <summary>
        /// Show property window displayed the selected object.
        /// </summary>
        /// <param name="obj">the selected object</param>
        private void ShowPropEditWindow(EcellObject obj)
        {
            PropertyEditor.Show(m_owner.Environment.DataManager, m_owner.Environment.PluginManager, obj);
        }


        private void EnterDragMode(EcellObject obj)
        {
            if (!obj.Type.Equals(EcellObject.PROCESS) &&
                !obj.Type.Equals(EcellObject.VARIABLE)) return;

            foreach (EcellData v in obj.Value)
            {
                if (!v.Name.Equals(Constants.xpathActivity) &&
                    !v.Name.Equals(Constants.xpathMolarConc))
                    continue;

                EcellDragObject dobj = new EcellDragObject(
                    obj.ModelID,
                    obj.Key,
                    obj.Type,
                    v.EntityPath,
                    v.Settable,
                    v.Logable);

                this.DoDragDrop(dobj, DragDropEffects.Move | DragDropEffects.Copy);
                return;
            }
        }

        /// <summary>
        /// Change selected Object
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public void ChangeObject(string modelID, string key, string type)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null)
                return;
            if (key == "")
            {
                treeView1.SelectNode(current, true, true);
                return;
            }
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target != null)
            {
                treeView1.SelectNode(target, true, true);
                return;
            }
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
            TreeNode node = obj.Logged ?
                new TreeNode(name + "[logged]"):
                new TreeNode(name);

            node.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(obj.Type);
            node.SelectedImageIndex = node.ImageIndex;
            node.Tag = new TagData(obj.ModelID, obj.Key, obj.Type);
            parent.Nodes.Add(node);

            return node;
        }

        /// <summary>
        /// Get the object from TreeNode.
        /// </summary>
        /// <param name="node">Target TreeNode.</param>
        /// <returns>EcellObject.</returns>
        private EcellObject GetObjectFromNode(TreeNode node)
        {
            TagData t = (TagData)node.Tag;
            if (t == null) return null;

            EcellObject obj = m_owner.Environment.DataManager.GetEcellObject(t.m_modelID, t.m_key, t.m_type);
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
            string currentPrj = m_owner.Environment.DataManager.CurrentProjectID;
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

            foreach (TreeNode node in current.Nodes)
            {
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

        private void DisplayDMEditor(string path)
        {
            DMEditor edit = new DMEditor(path, m_owner.Environment);
            using (edit) edit.ShowDialog();
        }

        private void DisplayDMWithApp(string path)
        {
            try
            {
                Process p = Process.Start(path);
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrStartupApp);
            }
        }

        #region Event
        /// <summary>
        /// The action of selecting [Create Parameter] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewCreParameterData(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null || m_lastSelectedNode.Tag == null)
                return;
            ToolStripMenuItem m = sender as ToolStripMenuItem;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            string key = m.Tag as string;
            EcellObject obj = m_owner.Environment.DataManager.GetEcellObject(
                    tag.m_modelID,
                    tag.m_key,
                    tag.m_type);

            if (m.Checked)
            {
                m_owner.Environment.DataManager.RemoveParameterData(
                    new EcellParameterData(obj.GetEcellData(key).EntityPath, 0.0));
            }
            else
            {
                EcellData d = obj.GetEcellData(key);
                m_owner.Environment.DataManager.SetParameterData(
                    new EcellParameterData(
                        d.EntityPath, 
                        Convert.ToDouble(d.Value.ToString())));
            }
        }

        /// <summary>
        /// The action of selecting [Create Observed] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewCreObservedData(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null || m_lastSelectedNode.Tag == null)
                return;
            ToolStripMenuItem m = sender as ToolStripMenuItem;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            string key = m.Tag as string;
            EcellObject obj = m_owner.Environment.DataManager.GetEcellObject(
                    tag.m_modelID,
                    tag.m_key,
                    tag.m_type);

            if (m.Checked)
            {
                m_owner.Environment.DataManager.RemoveObservedData(
                    new EcellObservedData(obj.GetEcellData(key).EntityPath, 0.0));
            }
            else
            {
                EcellData d = obj.GetEcellData(key);
                Debug.Assert(d != null);
                m_owner.Environment.DataManager.SetObservedData(
                    new EcellObservedData(d.EntityPath, Convert.ToDouble(d.Value.ToString())));
            }
        }

        /// <summary>
        /// The action of selecting [Create Logger] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void TreeViewCreLogger(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null || m_lastSelectedNode.Tag == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            string prop = m.Tag as string;

            EcellObject obj = m_owner.Environment.DataManager.GetEcellObject(
                tag.m_modelID, tag.m_key, tag.m_type);

            if (m.Checked)
            {
                obj.GetEcellData(prop).Logged = false;
            }
            else
            {
                EcellData d = obj.GetEcellData(prop);
                Debug.Assert(d != null);
                m_owner.Environment.PluginManager.LoggerAdd(
                    tag.m_modelID, tag.m_key, tag.m_type, d.EntityPath);
                d.Logged = true;
            }
            // modify changes
            m_owner.Environment.DataManager.DataChanged(obj.ModelID,
                obj.Key, obj.Type, obj);
        }

        /// <summary>
        /// The action of [Add Model] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddModel(object sender, EventArgs e)
        {
            if (m_lastSelectedNode != null)
                ShowPropEditWindow(GetObjectFromNode(m_lastSelectedNode));
        }

        /// <summary>
        /// The action of [Add System] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddSystem(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null || m_lastSelectedNode.Tag == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            m_owner.Environment.DataManager.CreateDefaultObject(
                tag.m_modelID, 
                tag.m_key, Constants.xpathSystem, true);
        }

        /// <summary>
        /// The action of [Add Variable] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddVariable(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null || m_lastSelectedNode.Tag == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            m_owner.Environment.DataManager.CreateDefaultObject(
                tag.m_modelID,
                tag.m_key, Constants.xpathVariable, true);
        }

        /// <summary>
        /// The action of [Add Process] menu on popup menu.
        /// </summary>
        /// <param name="sender">object (MenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void TreeviewAddProcess(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null || m_lastSelectedNode.Tag == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            m_owner.Environment.DataManager.CreateDefaultObject(
                tag.m_modelID, 
                tag.m_key, Constants.xpathProcess, true);
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
            if (m_lastSelectedNode == null)
                return;
            TagData tag = (TagData)m_lastSelectedNode.Tag;
            if (tag == null)
                return;

            if (m_lastSelectedNode.Parent != null)
            {
                TagData parentTag = (TagData)m_lastSelectedNode.Parent.Tag;
                if (parentTag != null)
                {
                    modelID = parentTag.m_modelID;
                    key = parentTag.m_key;
                    type = parentTag.m_type;
                }
            }

            try
            {
                int i = 0;
                int count = treeView1.SelNodes.Count;
                foreach (TreeNode delNode in treeView1.SelNodes.ToArray())
                {
                    bool isAnchor = false;
                    if (i == count) isAnchor = true;
                    i++;
                    TagData delTag = delNode.Tag as TagData;
                    if (delTag == null) continue;
                    if (tag.m_type == Constants.xpathModel)
                        m_owner.Environment.DataManager.DataDelete(delTag.m_modelID, null, Constants.xpathModel, true, isAnchor);
                    else
                        m_owner.Environment.DataManager.DataDelete(delTag.m_modelID, delTag.m_key, delTag.m_type, true, isAnchor);
                }
                if (modelID != null)
                    m_owner.Environment.PluginManager.SelectChanged(modelID, key, type);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Util.ShowErrorDialog(ex.Message);
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
            if (m_lastSelectedNode == null)
                return;
            TagData tag = (TagData)m_lastSelectedNode.Tag;
            if (tag == null)
                return;
            if (m_lastSelectedNode.Parent != null)
            {
                TagData parentTag = (TagData)m_lastSelectedNode.Parent.Tag;
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
                    m_owner.Environment.DataManager.DataDelete(tag.m_modelID, null, Constants.xpathModel);
                else if (tag.m_type == Constants.xpathSystem)
                    m_owner.Environment.DataManager.SystemDeleteAndMove(tag.m_modelID, tag.m_key);
                else
                    m_owner.Environment.DataManager.DataDelete(tag.m_modelID, tag.m_key, tag.m_type);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
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
            string path = m_owner.Environment.DataManager.GetDMFileName((string)m_lastSelectedNode.Tag);
            DMCompiler.Compile(path, m_owner.Environment);
        }


        private void TreeViewNewDm(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null)
                return;
            string dmDir = m_owner.Environment.DataManager.GetDMDir();
            CreateDMDialog ind = new CreateDMDialog(m_owner.Environment, dmDir, m_lastSelectedNode);
            using (ind)
            {
                DialogResult res = ind.ShowDialog();
                if (res == DialogResult.OK)
                {
                    string path = ind.FilePath;
                    DisplayDMEditor(path);
                }
            }
        }

        private void TreeViewDMDisplay(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            string path = m_owner.Environment.DataManager.GetDMFileName((string)m_lastSelectedNode.Tag);
            if (path == null) return;
            DisplayDMEditor(path);
        }

        private void TreeViewDMDisplayWithApp(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            string path = m_owner.Environment.DataManager.GetDMFileName((String)m_lastSelectedNode.Tag);
            if (path == null) return;
            DisplayDMWithApp(path);
        }

        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.ContextMenuStrip = null;
            TreeView t = (TreeView)sender;
            if (e.Node == null
                || m_owner.Environment.PluginManager.Status == ProjectStatus.Uninitialized)
            {
                return;
            }
            m_lastSelectedNode = e.Node;

            if (e.Button == MouseButtons.Left)
            {
                if (e.Node.Tag != null && e.Node is TagData)
                {
                    TagData tag = e.Node.Tag as TagData;
                    m_owner.Environment.PluginManager.SelectChanged(
                        tag.m_modelID, tag.m_key, tag.m_type);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (e.Node == m_DMNode)
                {
                    treeView1.ContextMenuStrip = contextMenuStripDMCollection;
                }
                else if (e.Node.Parent == m_DMNode)
                {
                    treeView1.ContextMenuStrip = contextMenuStripDM;
                }
                else if (e.Node.Tag is TagData)
                {
                    TagData tag = e.Node.Tag as TagData;
                    if (tag.m_type == Constants.xpathModel)
                    {
                        treeView1.ContextMenuStrip = null;
                    }
                    else if (tag.m_type == Constants.xpathSystem)
                    {
                        toolStripMenuItemAdd.Visible = true;
                        toolStripMenuItemDelete.Visible = true;
                        toolStripMenuItemMerge.Visible = true;
                        if (tag.m_key == "/")
                        {
                            toolStripMenuItemDelete.Enabled = false;
                            toolStripMenuItemMerge.Enabled = false;
                        }
                        else
                        {
                            toolStripMenuItemDelete.Enabled = true;
                            toolStripMenuItemMerge.Enabled = true;
                            string superSys = Util.GetSuperSystemPath(tag.m_key);
                            toolStripMenuItemMerge.Text = string.Format(MessageResources.PopMergeText, superSys);
                        }
                        EcellObject obj = GetObjectFromNode(e.Node);
                        toolStripMenuItemLogging.Visible = true;
                        toolStripMenuItemLogging.DropDownItems.Clear();
                        toolStripMenuItemLogging.DropDownItems.AddRange(
                            CreateLoggerPopupMenu(obj));
                        toolStripMenuItemObservation.Visible = true;
                        toolStripMenuItemObservation.DropDownItems.Clear();
                        toolStripMenuItemObservation.DropDownItems.AddRange(
                            CreateObservedPopupMenu(obj));
                        toolStripMenuItemParameter.Visible = true;
                        toolStripMenuItemParameter.DropDownItems.Clear();
                        toolStripMenuItemParameter.DropDownItems.AddRange(
                            CreateParameterPopupMenu(obj));
                        treeView1.ContextMenuStrip = contextMenuStripStdEntity;
                    }
                    else if (tag.m_type == Constants.xpathVariable
                             || tag.m_type == Constants.xpathProcess)
                    {
                        toolStripMenuItemAdd.Visible = false;
                        toolStripMenuItemDelete.Visible = true;
                        toolStripMenuItemMerge.Visible = false;
                        EcellObject obj = GetObjectFromNode(e.Node);
                        bool isVisible = true;
                        if (treeView1.SelNodes.Count > 1) isVisible = false;
                        toolStripMenuItemLogging.Visible = isVisible;
                        toolStripMenuItemLogging.DropDownItems.Clear();
                        toolStripMenuItemLogging.DropDownItems.AddRange(
                            CreateLoggerPopupMenu(obj));
                        toolStripMenuItemObservation.Visible = isVisible;
                        toolStripMenuItemObservation.DropDownItems.Clear();
                        toolStripMenuItemObservation.DropDownItems.AddRange(
                            CreateObservedPopupMenu(obj));
                        toolStripMenuItemParameter.Visible = isVisible;
                        toolStripMenuItemParameter.DropDownItems.Clear();
                        toolStripMenuItemParameter.DropDownItems.AddRange(
                            CreateParameterPopupMenu(obj));
                        treeView1.ContextMenuStrip = contextMenuStripStdEntity;
                    }
                }
            }
        }

        private void NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (m_owner.Environment.PluginManager.Status == ProjectStatus.Uninitialized)
            {
                return;
            }

            if (e.Node.Parent == m_DMNode)
            {
                string path = m_owner.Environment.DataManager.GetDMFileName(e.Node.Text);
                if (path == null) return;
                DisplayDMEditor(path);
            }
            else if (m_paramNodeDic.ContainsValue(e.Node.Parent))
            {
                m_owner.Environment.PluginManager.SelectChanged(
                    "", (string)e.Node.Tag, Constants.xpathParameters);
            }
            else if (e.Node.Tag != null && e.Node.Tag is TagData)
            {
                TagData tag = (TagData)e.Node.Tag;
                if (tag.m_type.Equals(EcellObject.PROCESS) ||
                    tag.m_type.Equals(EcellObject.VARIABLE) ||
                    tag.m_type.Equals(EcellObject.SYSTEM))
                {
                    EcellObject obj = GetObjectFromNode(e.Node);
                    Debug.Assert(obj != null);
                    ShowPropEditWindow(obj);
                }
            }
        }

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
        public void DataChanged(string modelID, string key, string type, EcellObject data)
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
                if (data.Logged)
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
                    treeView1.Sort();
                }
            }
        }

        public void AddSelect(string modelID, string key, string type)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null) return;
            if (key == "")
            {
                treeView1.SelectNode(current, true, true);
            }
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target != null)
            {
                treeView1.SelectNodes(target);
            }
        }

        public void RemoveSelect(string modelID, string key, string type)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null) return;
            if (key == "")
            {
                treeView1.DeselectNode(current);
                return;
            }
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target != null)
            {
                treeView1.DeselectNode(target);
                return;
            }
        }

        public void DataAdd(List<EcellObject> data)
        {
            if (data == null)
                return;
            foreach (EcellObject obj in data)
            {
                if (obj.Type == Constants.xpathProject)
                {
                    m_prjNode = new TreeNode(obj.ModelID);
                    treeView1.Nodes.Add(m_prjNode);
                    TreeNode modelNode = new TreeNode(MessageResources.NameModel);
                    modelNode.Tag = null;
                    TreeNode paramNode = new TreeNode(MessageResources.NameParameters);
                    paramNode.Tag = null;
                    m_DMNode = new TreeNode(MessageResources.NameDMs);
                    m_DMNode.Tag = null;
                    m_logNode = new TreeNode(MessageResources.NameLogArchives);
                    m_logNode.Tag = null;
                    m_prjNode.Nodes.Add(modelNode);
                    m_prjNode.Nodes.Add(paramNode);
                    m_prjNode.Nodes.Add(m_logNode);
                    m_prjNode.Nodes.Add(m_DMNode);
                    m_modelNodeDic.Add(obj.ModelID, modelNode);
                    m_paramNodeDic.Add(obj.ModelID, paramNode);

                    List<string> fileList = m_owner.Environment.DataManager.GetDMDirData();
                    foreach (string d in fileList)
                    {
                        if (!Util.IsDMFile(d)) continue;
                        TreeNode dNode = new TreeNode(d);
                        dNode.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathDM);
                        dNode.SelectedImageIndex = dNode.ImageIndex;
                        dNode.Tag = d;
                        m_DMNode.Nodes.Add(dNode);
                    }

                    SetLogEntry(m_logNode);

                    continue;
                }
                else if (obj.Type == Constants.xpathModel)
                {
                    if (GetTargetModel(obj.ModelID) != null) continue;
                    TreeNode node = new TreeNode(obj.ModelID);
                    node.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(obj.Type);
                    node.SelectedImageIndex = node.ImageIndex;
                    node.Tag = new TagData(obj.ModelID, "", Constants.xpathModel);
                    string currentPrj = m_owner.Environment.DataManager.CurrentProjectID;
                    if (m_modelNodeDic.ContainsKey(currentPrj))
                        m_modelNodeDic[currentPrj].Nodes.Add(node);
                    continue;
                }
                else if (obj.Type == Constants.xpathProcess ||
                    obj.Type == Constants.xpathVariable)
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
                            bool isHit = false;
                            foreach (TreeNode tmp in node.Nodes)
                            {
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
            treeView1.Sort();
        }

        public void RefreshLogEntry()
        {
            if (m_logNode == null) return;
            m_logNode.Nodes.Clear();
            SetLogEntry(m_logNode);
        }

        private void SetLogEntry(TreeNode node)
        {
            List<string> logList = m_owner.Environment.DataManager.GetLogDataList();
            Dictionary<string, TreeNode> nodeDic = new Dictionary<string, TreeNode>();
            foreach (string name in logList)
            {
                string[] sep = name.Split(new char[] { ';' });
                TreeNode n = new TreeNode(sep[1]);
                n.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathLog);
                n.SelectedImageIndex = n.ImageIndex;
                n.Tag = new  TagData("", "", Constants.xpathLog);

                if (nodeDic.ContainsKey(sep[0]))
                {
                    nodeDic[sep[0]].Nodes.Add(n);
                }
                else
                {
                    TreeNode logNode = new TreeNode(sep[0]);
                    logNode.Tag = null;
                    node.Nodes.Add(logNode);
                    nodeDic.Add(sep[0], logNode);
                    logNode.Nodes.Add(n);
                }
            }
        }

        public void ParameterAdd(string projectID, string parameterID) {
            TreeNode paramsNode = null;
            if (m_paramNodeDic.ContainsKey(projectID))
            {
                paramsNode = m_paramNodeDic[projectID];
            }
            else
            {
                foreach (TreeNode project in treeView1.Nodes)
                {
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
            paramNode.Tag = parameterID;
            paramNode.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathParameters);
            paramNode.SelectedImageIndex = paramNode.ImageIndex;
            paramsNode.Nodes.Add(paramNode);

            return;
        }

        public void ParameterDelete(string projectID, string parameterID)
        {
            TreeNode paramsNode = null;
            if (m_paramNodeDic.ContainsKey(projectID))
            {
                paramsNode = m_paramNodeDic[projectID];
            }
            else
            {
                foreach (TreeNode project in treeView1.Nodes)
                {
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
                if (t.Text == parameterID)
                {
                    paramsNode.Nodes.Remove(t);
                    return;
                }
            }
        }

        public void Clear()
        {
            foreach (TreeNode project in treeView1.Nodes)
            {
                project.Remove();
            }
            m_modelNodeDic.Clear();
            m_paramNodeDic.Clear();
            m_logNode = null;
            m_DMNode = null;
        }

        private void TreeViewSortByName(object sender, EventArgs e)
        {
            treeView1.TreeViewNodeSorter = m_nameSorter;
            treeView1.Sort();
            toolStripButtonSortByName.Checked = true;
            toolStripButtonSortByType.Checked = false;
        }

        private void TreeViewSortByType(object sender, EventArgs e)
        {
            treeView1.TreeViewNodeSorter = m_typeSorter;
            treeView1.Sort();
            toolStripButtonSortByName.Checked = false;
            toolStripButtonSortByType.Checked = true;
        }

        private void TreeViewItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (node == null) return;
            EcellObject obj = GetObjectFromNode(node);
            if (obj == null) return;
            EnterDragMode(obj);
        }
    }

    /// <summary>
    /// Sort class by name of object.
    /// </summary>
    public class NameSorter : IComparer<TreeNode>, System.Collections.IComparer
    {
        /// <summary>
        /// Compare with two object by name.
        /// </summary>
        /// <param name="tx">compared object.</param>
        /// <param name="ty">compare object.</param>
        /// <returns></returns>
        public int Compare(TreeNode tx, TreeNode ty)
        {
            return string.Compare(tx.Text, ty.Text);
        }

        int System.Collections.IComparer.Compare(object x, object y)
        {
            return Compare(x as TreeNode, y as TreeNode);
        }

    }

    /// <summary>
    /// Sort class by type of object.
    /// </summary>
    public class TypeSorter : IComparer<TreeNode>, System.Collections.IComparer
    {
        /// <summary>
        /// Compare with two object.
        /// The first, system sort by the type of object.
        /// The second, system sort by the name of object.
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public int Compare(TreeNode tx, TreeNode ty)
        {
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

        int System.Collections.IComparer.Compare(object x, object y)
        {
            return Compare(x as TreeNode, y as TreeNode);
        }

        /// <summary>
        /// Get the number of type.
        /// </summary>
        /// <param name="type">type of object.</param>
        /// <returns>type number.</returns>
        private static int GetTypeNum(string type)
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

    /// <summary>
    /// Tag Object with the node in ProjectExplorer.
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
        #endregion
    }
}
