using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Ecell.IDE;
using Ecell.Objects;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.ProjectExplorer
{
    public partial class ProjectExplorerControl : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private bool m_isExpland = false;
        /// <summary>
        /// Project tree node in TreeView
        /// </summary>
        private TreeNode m_prjNode;
        /// <summary>
        /// Model tree node in TreeView
        /// </summary>
        private TreeNode m_modelNode;
        /// <summary>
        /// Revision tree node in TreeView
        /// </summary>
        private TreeNode m_revisionNode;
        /// <summary>
        /// DM tree node in TreeView
        /// </summary>
        private TreeNode m_DMNode;
        /// <summary>
        /// Log tree node in TreeView
        /// </summary>
        private TreeNode m_logNode;
        /// <summary>
        /// Parameter tree node in TreeView
        /// </summary>
        private TreeNode m_paramNode;
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
        /// <summary>
        /// 
        /// </summary>
        System.Collections.IComparer m_nameSorter;
        /// <summary>
        /// 
        /// </summary>
        System.Collections.IComparer m_typeSorter;

        #endregion

        #region Constructor
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
        #endregion

        #region Methods for EcellPlugin
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void DataAdd(List<EcellObject> data)
        {
            if (data == null)
                return;
            foreach (EcellObject obj in data)
            {
                if (obj.Type == Constants.xpathProject)
                {
                    CreateProjectNode(obj);
                    continue;
                }
                else if (obj.Type == Constants.xpathModel)
                {
                    if (GetTargetModel(obj.ModelID) != null)
                        continue;
                    TreeNode node = new TreeNode(obj.ModelID);
                    node.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(obj.Type);
                    node.SelectedImageIndex = node.ImageIndex;
                    node.Tag = new TagData(obj.ModelID, "", Constants.xpathModel);
                    m_modelNode.Nodes.Add(node);
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
                        node = GetTargetTreeNode(current, obj.ParentSystemID, null);
                        TreeNode childNode = AddTreeNode(obj.LocalID, obj, node);
                    }
                }
                else if (obj.Type == Constants.xpathSystem)
                {
                    TreeNode current = GetTargetModel(obj.ModelID);
                    if (current == null)
                        return;
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
                            target = GetTargetTreeNode(current, obj.ParentSystemID, null);

                            if (target != null)
                            {
                                node = AddTreeNode(obj.LocalID, obj, target);
                            }
                        }
                    }
                    if (node != null)
                    {
                        if (obj.Children == null)
                            continue;
                        foreach (EcellObject eo in obj.Children)
                        {
                            if (eo.Type != Constants.xpathVariable && eo.Type != Constants.xpathProcess)
                                continue;
                            if (eo.Key.EndsWith(Constants.headerSize))
                                continue;
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
                            if (isHit == true)
                                continue;

                            TreeNode childNode = null;
                            childNode = AddTreeNode(names[names.Length - 1], eo, node);
                        }
                    }
                }
            }
            m_isExpland = false;
            treeView1.ExpandAll();
            treeView1.Sort();
        }

        /// <summary>
        /// Create Project Node.
        /// </summary>
        /// <param name="obj"></param>
        private void CreateProjectNode(EcellObject obj)
        {
            // Create project node.
            m_prjNode = new TreeNode(obj.ModelID);
            treeView1.Nodes.Add(m_prjNode);

            m_modelNode = new TreeNode(MessageResources.NameModel);
            m_revisionNode = new TreeNode(MessageResources.NameRevisions);
            m_paramNode = new TreeNode(MessageResources.NameParameters);
            m_DMNode = new TreeNode(MessageResources.NameDMs);
            m_logNode = new TreeNode(MessageResources.NameLogArchives);

            m_modelNode.Tag = null;
            m_revisionNode.Tag = null;
            m_paramNode.Tag = null;
            m_DMNode.Tag = null;
            m_logNode.Tag = null;

            m_prjNode.Nodes.Add(m_modelNode);
            m_prjNode.Nodes.Add(m_revisionNode);
            m_prjNode.Nodes.Add(m_paramNode);
            m_prjNode.Nodes.Add(m_logNode);
            m_prjNode.Nodes.Add(m_DMNode);

            SetDMNodes();
            SetRevisions();

            SetLogEntry(m_logNode);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDMNodes()
        {
            List<string> fileList = m_owner.Environment.DataManager.GetDMNameList();
            foreach (string d in fileList)
            {
                if (!Util.IsDMFile(d))
                    continue;
                TreeNode dNode = new TreeNode(d);
                dNode.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathDM);
                dNode.SelectedImageIndex = dNode.ImageIndex;
                dNode.Tag = d;
                m_DMNode.Nodes.Add(dNode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetRevisions()
        {
            m_revisionNode.Nodes.Clear();
            List<string> revisionList = m_owner.Environment.DataManager.CurrentProject.GetRevisions();
            foreach (string revision in revisionList)
            {
                TreeNode rNode = new TreeNode(revision);
                rNode.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathModel);
                rNode.SelectedImageIndex = rNode.ImageIndex;
                rNode.Tag = revision;
                m_revisionNode.Nodes.Add(rNode);
            }

            // Set Current Revision
            TreeNode current = new TreeNode(Constants.xpathLatest);
            current.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathModel);
            current.SelectedImageIndex = current.ImageIndex;
            current.Tag = Constants.xpathLatest;
            m_revisionNode.Nodes.Add(current);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null)
                return;
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target != null)
            {
                string targetText = data.LocalID;

                if (target.Text != targetText)
                {
                    target.Text = targetText;
                    target.Name = targetText;
                }
                if (data.Logged)
                {
                    target.Text = target.Text + "[logged]";
                }

                if (key != data.Key)
                {
                    TreeNode change = GetTargetTreeNode(current, data.ParentSystemID, Constants.xpathSystem);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
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
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
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
                treeView1.SelectNodes(target, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            TreeNode current = GetTargetModel(modelID);
            if (current == null) return;
            if (key == "")
            {
                treeView1.DeselectNode(current, false);
                return;
            }
            TreeNode target = GetTargetTreeNode(current, key, type);
            if (target != null)
            {
                treeView1.DeselectNode(target, false);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
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
                n.Tag = new TagData("", sep[2], Constants.xpathLog);

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

        /// <summary>
        /// 
        /// </summary>
        public void RefreshLogEntry()
        {
            if (m_logNode == null)
                return;
            m_logNode.Nodes.Clear();
            SetLogEntry(m_logNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="parameterID"></param>
        public void ParameterAdd(string projectID, string parameterID)
        {
            TreeNode paramsNode = null;
            if (m_paramNode != null)
            {
                paramsNode = m_paramNode;
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
                        m_paramNode = paramsNode;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="parameterID"></param>
        public void ParameterDelete(string projectID, string parameterID)
        {
            TreeNode paramsNode = null;
            if (m_paramNode != null)
            {
                paramsNode = m_paramNode;
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
                        m_paramNode = paramsNode;
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

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (TreeNode project in treeView1.Nodes)
            {
                project.Remove();
            }
            m_modelNode = null;
            m_paramNode = null;
            m_logNode = null;
            m_DMNode = null;
        }

        #endregion

        #region Methods for Internal use.
        /// <summary>
        /// Show property window displayed the selected object.
        /// </summary>
        /// <param name="obj">the selected object</param>
        private void ShowPropEditWindow(EcellObject obj)
        {
            PropertyEditor.Show(m_owner.Environment, obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oList"></param>
        /// <param name="fileList"></param>
        private void EnterDragMode(List<EcellObject> oList, List<string> fileList)
        {
            EcellDragObject dobj = null;
            foreach (EcellObject obj in oList)
            {
                if (!obj.Type.Equals(EcellObject.PROCESS) &&
                    !obj.Type.Equals(EcellObject.VARIABLE) &&
                    !obj.Type.Equals(EcellObject.SYSTEM))
                    continue;
                // Create new EcellDragObject.
                if (dobj == null)
                    dobj = new EcellDragObject(obj.ModelID);
                foreach (EcellData v in obj.Value)
                {
                    if (!v.Name.Equals(Constants.xpathActivity) &&
                        !v.Name.Equals(Constants.xpathMolarConc) &&
                        !v.Name.Equals(Constants.xpathSize))
                        continue;

                    // Add new EcellDragEntry.
                    dobj.Entries.Add(new EcellDragEntry(
                        obj.Key,
                        obj.Type,
                        v.EntityPath,
                        v.Settable,
                        v.Logable));
                    break;
                }
            }
            // If there is no process or variable, return.
            if (dobj == null && fileList.Count <= 0)
                return;

            // Set Log List.
            if (dobj == null)
                dobj = new EcellDragObject();
            dobj.LogList = fileList;

            // Drag & Drop Event.
            this.treeView1.IsDrag = true;
            this.DoDragDrop(dobj, DragDropEffects.Move | DragDropEffects.Copy);
            return;            
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
            node.Name = name;
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
            TagData t = node.Tag as TagData;
            if (t == null)
                return null;
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
            if (src.Parent == null)
                return null;
            TreeNode node = src;

            while (node != null)
            {
                TagData tag = (TagData)node.Tag;
                if (tag == null)
                    return null;
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
            TreeNode model = null;
            if (m_modelNode == null)
                return model;
            foreach (TreeNode node in m_modelNode.Nodes)
            {
                TagData tag = node.Tag as TagData;
                if (tag.m_type == Constants.xpathModel && node.Text == modelID)
                    model = node;
            }
            return model;
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
            if (current.Nodes.Count <= 0)
                return null;

            keydata = key.Split(new Char[] { '/' });
            if (keydata.Length == 0)
                return null;

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
                    IDChangeProvide(oldKey + "/" + t.Name,
                        newKey + "/" + t.Name, t);
                    tag.m_key = newKey + "/" + t.Name;
                    continue;
                }
                tag.m_key = newKey + ":" + t.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void DisplayDMEditor(string path)
        {
            DMEditor edit = new DMEditor(path, m_owner.Environment);
            using (edit) edit.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
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

        #endregion

        #region Event
        /// <summary>
        /// The action of clicking [Compile] menu on popup menu.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs.</param>
        private void TreeViewCompile(object sender, EventArgs e)
        {
            string path = m_owner.Environment.DataManager.GetDMFileName((string)m_lastSelectedNode.Tag);
            if (path == null) return;
            DMCompiler.Compile(path, m_owner.Environment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewNewDm(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null)
                return;

            // Get DM dir.
            string dmDir = m_owner.Environment.DataManager.GetDMDir();
            if (dmDir == null)
            {
                Util.ShowErrorDialog(MessageResources.ErrProjectUnsaved);
                return;
            }

            // Create new DM.
            CreateDMDialog ind = new CreateDMDialog(m_owner.Environment, dmDir, m_lastSelectedNode);
            using (ind)
            {
                DialogResult res = ind.ShowDialog();
                if (res == DialogResult.OK)
                {
                    ind.CreateDM();
                    string path = ind.FilePath;
                    DisplayDMEditor(path);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewDMDisplay(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            string path = m_owner.Environment.DataManager.GetDMFileName((string)m_lastSelectedNode.Tag);
            if (path == null) return;
            DisplayDMEditor(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewLogDisplayWithApp(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null) return;
            if (tag.m_type.Equals(Constants.xpathLog))
            {
                string path = tag.m_key;
                DisplayDMWithApp(path);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                m_isExpland = false;
                if (e.Node.Tag != null && e.Node.Tag is TagData)
                {
                    TagData tag = e.Node.Tag as TagData;
                    if (tag.m_type.Equals(EcellObject.SYSTEM) &&
                        e.Node.Bounds.Contains(e.X, e.Y))
                        m_isExpland = true;
                }                
            }

            if (e.Button == MouseButtons.Right)
            {
                if (!treeView1.SelNodes.Contains(e.Node))
                {
                    if (e.Node.Tag != null && e.Node.Tag is TagData)
                    {
                        TagData tag = e.Node.Tag as TagData;
                        m_owner.Environment.PluginManager.SelectChanged(
                            tag.m_modelID, tag.m_key, tag.m_type);
                    }
                    treeView1.ClearSelNode();
                    treeView1.SelectedNode = e.Node;
                }

                if (e.Node == m_DMNode)
                {
                    treeView1.ContextMenuStrip = contextMenuStripDMCollection;
                }
                else if (e.Node == m_prjNode)
                {
                    treeView1.ContextMenuStrip = contextMenuStripProject;
                }
                else if (e.Node.Parent != null && e.Node.Parent == m_DMNode)
                {
                    string path = m_owner.Environment.DataManager.GetDMFileName((string)e.Node.Tag);
                    if (!Util.IsInstalledSDK() || path == null)
                        compileToolStripMenuItem.Enabled = false;
                    else 
                        compileToolStripMenuItem.Enabled = true;
                    if (path == null)
                        editToolStripMenuItem.Enabled = false;
                    else
                        editToolStripMenuItem.Enabled = true;

                    treeView1.ContextMenuStrip = contextMenuStripDM;
                }
                else if (e.Node == m_revisionNode)
                {
                    treeView1.ContextMenuStrip = contextMenuStripRevisions;
                }
                else if (e.Node.Parent == m_revisionNode)
                {
                    treeView1.ContextMenuStrip = contextMenuStripRevision;
                }
                else if (e.Node == m_paramNode)
                {
                    treeView1.ContextMenuStrip = contextMenuSimulationSetCollection;
                }
                else if (e.Node.Parent == m_paramNode)
                {
                    treeView1.ContextMenuStrip = contextMenuStripSimulationSet;
                }
                else if (e.Node.Tag is TagData)
                {
                    TagData tag = e.Node.Tag as TagData;
                    if (tag.m_type == Constants.xpathModel)
                    {
                        treeView1.ContextMenuStrip = contextMenuStripModel;
                    }
                    else if (tag.m_type == Constants.xpathSystem ||
                        tag.m_type == Constants.xpathProcess ||
                        tag.m_type == Constants.xpathVariable)
                    {
                        EcellObject obj = GetObjectFromNode(e.Node);
                        CommonContextMenu m = new CommonContextMenu(obj, m_owner.Environment);
                        treeView1.ContextMenuStrip = m.Menu;
                    }
                    else if (tag.m_type == Constants.xpathLog)
                    {
                        treeView1.ContextMenuStrip = contextMenuStripLog;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (m_owner.Environment.PluginManager.Status == ProjectStatus.Uninitialized)
            {
                return;
            }

            // ノード以外の場所(+や空白)を選択したときにプロパティ編集ダイアログを
            // 表示しないように変更
            if (e.Node == null || !e.Node.Bounds.Contains(e.X, e.Y))
                return;

            if (e.Node.Parent == m_DMNode)
            {
                string path = m_owner.Environment.DataManager.GetDMFileName(e.Node.Text);
                if (path == null) return;
                DisplayDMEditor(path);
            }
            else if (m_paramNode == e.Node.Parent)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewSortByName(object sender, EventArgs e)
        {
            treeView1.TreeViewNodeSorter = m_nameSorter;
            treeView1.Sort();
            toolStripButtonSortByName.Checked = true;
            toolStripButtonSortByType.Checked = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewSortByType(object sender, EventArgs e)
        {
            treeView1.TreeViewNodeSorter = m_typeSorter;
            treeView1.Sort();
            toolStripButtonSortByName.Checked = false;
            toolStripButtonSortByType.Checked = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItemDrag(object sender, ItemDragEventArgs e)
        {
            List<string> fileList = new List<string>();
            List<EcellObject> oList = new List<EcellObject>();
            if (!this.treeView1.SelNodes.Contains((TreeNode)e.Item))
            {
                this.treeView1.ClearSelNode();
                this.treeView1.SelectNode((TreeNode)e.Item, false, false);
                this.treeView1.Refresh();
            }
            foreach (TreeNode node in this.treeView1.SelNodes)
            {
                if (node.Tag != null && node.Tag is TagData)
                {
                    TagData t = node.Tag as TagData;
                    if (t.m_type.Equals(Constants.xpathLog))
                    {
                        fileList.Add(t.m_key);
                    }
                }
                EcellObject obj = GetObjectFromNode(node);
                if (obj == null)
                    continue;
                oList.Add(obj);
            }            
            EnterDragMode(oList, fileList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewBeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (m_isExpland)
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (m_isExpland)
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewExportLog(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null || tag.m_type != Constants.xpathLog) return;

            string logFile = tag.m_key;
            string ext = Path.GetExtension(logFile);
            if (!string.IsNullOrEmpty(ext) &&
                ext.ToLower().EndsWith("csv"))
                m_saveFileDialog.Filter = Constants.FilterCSVFile;
            else
                m_saveFileDialog.Filter = Constants.FilterECDFile;
            if (m_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string text = "";
                string fileName = m_saveFileDialog.FileName;
                using (StreamReader sr = new StreamReader(
                        tag.m_key, Encoding.GetEncoding("Shift_JIS")))
                {
                    text = sr.ReadToEnd();
                }
                using (StreamWriter sw = new StreamWriter(
                    fileName, false, Encoding.GetEncoding("Shift_JIS")))
                {
                    sw.Write(text);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewShowLogOnGraph(object sender, EventArgs e)
        {       
            if (m_lastSelectedNode == null) return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null || tag.m_type != Constants.xpathLog) return;
     
            ShowGraphDelegate dlg = m_owner.PluginManager.GetDelegate("ShowGraphWithLog") as ShowGraphDelegate;

            dlg(tag.m_key, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewAddSimulationSet(object sender, EventArgs e)
        {
            InputNameDialog dlg = new InputNameDialog();
            dlg.OwnerForm = m_owner;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                m_owner.DataManager.CopySimulationParameter(dlg.InputText,
                    m_owner.DataManager.CurrentProject.Info.SimulationParam);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewCopySimulationSet(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            String name = m_lastSelectedNode.Tag as string;
            if (String.IsNullOrEmpty(name)) return;

            List<string> paraList = m_owner.DataManager.GetSimulationParameterIDs();

            string newParam = name + "_copy";

            int i = 0;
            while (paraList.Contains(newParam))
            {
                i++;
                newParam = name + "_copy" + i;
            }

            m_owner.DataManager.CopySimulationParameter(newParam, name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewDeleteSimulationSet(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            String name = m_lastSelectedNode.Tag as string;
            if (String.IsNullOrEmpty(name)) return;

            try
            {
                m_owner.DataManager.DeleteSimulationParameter(name);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewExportModel(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null || tag.m_type != Constants.xpathModel) return;
            String name = tag.m_modelID;
            List<string> modelList = new List<string>();
            modelList.Add(name);

            m_saveFileDialog.Filter = Constants.FilterEmlFile;
            if (m_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = m_saveFileDialog.FileName;

                m_owner.DataManager.ExportModel(modelList, fileName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewCreateNewRevision(object sender, EventArgs e)
        {
            m_owner.DataManager.CreateNewRevision();
            SetRevisions();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewLoadRevision(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null)
                return;

            m_owner.DataManager.LoadRevision(m_lastSelectedNode.Text);
            SetRevisions();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewConfigureSimulationSet(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null)
                return;
            if (m_paramNode == m_lastSelectedNode.Parent)
            {
                m_owner.Environment.PluginManager.SelectChanged(
                    "", (string)m_lastSelectedNode.Tag, Constants.xpathParameters);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewCompressZip(object sender, EventArgs e)
        {
            m_saveFileDialog.Filter = Constants.FilterZipFile;
            if (m_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = m_saveFileDialog.FileName;
                ZipUtil.ZipFolder(fileName,
                m_owner.Environment.DataManager.CurrentProject.Info.ProjectPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewCloseProject(object sender, EventArgs e)
        {
            m_owner.Environment.DataManager.CloseProject();
        }

        #endregion

        #region ShortCuts
        private void DeletedSelectionRow()
        {
            List<TagData> delList = new List<TagData>();
            foreach (TreeNode node in treeView1.SelNodes)
            {
                if (node.Tag == null) continue;
                TagData obj = node.Tag as TagData;
                if (obj != null) delList.Add(obj);
            }

            for (int i = 0; i < delList.Count; i++)
            {
                TagData obj = delList[i];
                if (i == delList.Count - 1)
                    m_owner.DataManager.DataDelete(obj.m_modelID, obj.m_key, obj.m_type, true, true);
                else
                    m_owner.DataManager.DataDelete(obj.m_modelID, obj.m_key, obj.m_type, true, false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((int)keyData == (int)Keys.Control + (int)Keys.D ||
                (int)keyData == (int)Keys.Delete)
            {
                DeletedSelectionRow();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
#endregion
    }

    #region Internal Classes
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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
    }

    #endregion

}
