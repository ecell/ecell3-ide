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
        private ProjectNode m_prjNode = null;
        /// <summary>
        /// Model tree node in TreeView
        /// </summary>
        private ModelRootNode m_modelNode = null;
        /// <summary>
        /// Revision tree node in TreeView
        /// </summary>
        private RevisionRootNode m_revisionNode = null;
        /// <summary>
        /// DM tree node in TreeView
        /// </summary>
        private DMRootNode m_DMNode = null;
        /// <summary>
        /// Log tree node in TreeView
        /// </summary>
        private LogRootNode m_logNode = null;
        /// <summary>
        /// Parameter tree node in TreeView
        /// </summary>
        private ParamRootNode m_paramNode = null;
        /// <summary>
        /// Last selecte dnode
        /// </summary>
        private TreeNode m_lastSelectedNode = null;
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
        /// <param name="type"></param>
        internal void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Loaded)
                m_prjNode.Text = m_owner.Environment.DataManager.CurrentProjectID;
        }
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
                    node.ContextMenuStrip = this.contextMenuStripModel;
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
                                    tag.Type == eo.Type)
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
                else if (obj.Type == Constants.xpathStepper)
                {
                    TreeNode current = GetTargetModel(obj.ModelID);
                    if (current == null)
                        return;

                    TreeNode childNode = AddTreeNode(obj.Key, obj, current);
                    childNode.ContextMenuStrip = contextMenuStripStepper;
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
            m_prjNode = new ProjectNode(obj.ModelID);
            m_prjNode.ContextMenuStrip = this.contextMenuStripProject;
            treeView1.Nodes.Add(m_prjNode);

            // Create DM node.
            m_DMNode = new DMRootNode(MessageResources.NameDMs);
            m_DMNode.ContextMenuStrip = this.contextMenuStripDMCollection;
            m_DMNode.Tag = null;
            m_prjNode.Nodes.Add(m_DMNode);
            SetDMNodes();

            // Create ModelNode.
            m_modelNode = new ModelRootNode(MessageResources.NameModel);
            m_modelNode.Tag = null;
            m_prjNode.Nodes.Add(m_modelNode);

            // Create RevisionNode.
            m_revisionNode = new RevisionRootNode(MessageResources.NameRevisions);
            m_revisionNode.ContextMenuStrip = this.contextMenuStripRevisions;
            m_revisionNode.Tag = null;
            m_prjNode.Nodes.Add(m_revisionNode);
            SetRevisions();

            // Create parameter Node.
            m_paramNode = new ParamRootNode(MessageResources.NameParameters);
            m_paramNode.ContextMenuStrip = this.contextMenuSimulationSetCollection;
            m_paramNode.Tag = null;
            m_prjNode.Nodes.Add(m_paramNode);

            // Create LogNode.
            m_logNode = new LogRootNode(MessageResources.NameLogArchives);
            m_logNode.Tag = null;
            m_prjNode.Nodes.Add(m_logNode);
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
                DMNode dNode = new DMNode(d);
                dNode.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathDM);
                dNode.SelectedImageIndex = dNode.ImageIndex;
                dNode.Tag = d;
                dNode.ContextMenuStrip = this.contextMenuStripDM;
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
                RevisionNode rNode = new RevisionNode(revision);
                rNode.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathModel);
                rNode.SelectedImageIndex = rNode.ImageIndex;
                rNode.Tag = revision;
                rNode.ContextMenuStrip = this.contextMenuStripRevision;
                m_revisionNode.Nodes.Add(rNode);
            }

            // Set Current Revision
            TreeNode current = new TreeNode(Constants.xpathCurrent);
            current.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathModel);
            current.SelectedImageIndex = current.ImageIndex;
            current.Tag = Constants.xpathCurrent;
            current.ContextMenuStrip = this.contextMenuStripRevision;
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

            if (type == Constants.xpathStepper)
            {
                foreach (TreeNode node in current.Nodes)
                {
                    TagData tag = (TagData)node.Tag;
                    if (tag == null) continue;
                    if (node.Text.Equals(key) && tag.Type == type)
                    {
                        node.Text = data.Key;
                        node.Tag = new TagData(data.ModelID,
                            data.Key, data.Type);
                    }
                }
            }

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
                    tag.Key = data.Key;
                    target.Tag = tag;
                    if (tag.Type == Constants.xpathSystem)
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
            if (type == Constants.xpathStepper)
            {
                foreach (TreeNode node in current.Nodes)
                {
                    TagData tag = (TagData)node.Tag;
                    if (tag == null) continue;
                    if (node.Text.Equals(key) && tag.Type == type)
                    {
                        node.Remove();
                        return;
                    }
                }
            }

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
                LogNode n = new LogNode(sep[1]);
                n.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathLog);
                n.SelectedImageIndex = n.ImageIndex;
                n.Tag = new TagData("", sep[2], Constants.xpathLog);
                n.ContextMenuStrip = this.contextMenuStripLog;

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
            if (m_paramNode == null)
                return;

            foreach (TreeNode t in m_paramNode.Nodes)
            {
                if (t.Text == parameterID)
                    return;
            }
            TreeNode paramNode = new TreeNode(parameterID);
            paramNode.Tag = parameterID;
            paramNode.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathParameters);
            paramNode.SelectedImageIndex = paramNode.ImageIndex;
            paramNode.ContextMenuStrip = this.contextMenuStripSimulationSet;
            m_paramNode.Nodes.Add(paramNode);

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="parameterID"></param>
        public void ParameterDelete(string projectID, string parameterID)
        {
            if (m_paramNode == null)
                return;

            foreach (TreeNode t in m_paramNode.Nodes)
            {
                if (t.Text == parameterID)
                {
                    m_paramNode.Nodes.Remove(t);
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
            ShowDialogDelegate dlg = m_owner.PluginManager.GetDelegate(Constants.delegateShowPropertyWindow) as ShowDialogDelegate;
            if (dlg != null)
                dlg();
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
            EcellObject obj = m_owner.Environment.DataManager.GetEcellObject(t.ModelID, t.Key, t.Type);
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
            if (src.Parent == null)
                return null;
            TagData tag = (TagData)src.Parent.Tag;
            if (tag == null)
                return null;
            if (tag.Type == Constants.xpathModel)
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
                if (tag.Type == Constants.xpathModel)
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
                if (tag.Type == Constants.xpathModel && node.Text == modelID)
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
                        (tag.Type == type || type == null))
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
                if (tag.Type == Constants.xpathSystem)
                {
                    IDChangeProvide(oldKey + "/" + t.Name,
                        newKey + "/" + t.Name, t);
                    tag.Key = newKey + "/" + t.Name;
                    continue;
                }
                tag.Key = newKey + ":" + t.Name;
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
            if (tag.Type.Equals(Constants.xpathLog))
            {
                string path = tag.Key;
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
            // Check null.
            if (e.Node == null || m_prjNode == null
                || m_owner.Environment.PluginManager.Status == ProjectStatus.Uninitialized)
                return;

            // Reset node.
            treeView1.ContextMenuStrip = null;
            TreeView t = (TreeView)sender;
            m_lastSelectedNode = e.Node;

            if (e.Button == MouseButtons.Left)
            {
                m_isExpland = false;
                if (e.Node.Tag != null && e.Node.Tag is TagData)
                {
                    TagData tag = e.Node.Tag as TagData;
                    if (tag.Type.Equals(EcellObject.SYSTEM) &&
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
                            tag.ModelID, tag.Key, tag.Type);
                    }
                    treeView1.ClearSelNode();
                    treeView1.SelectedNode = e.Node;
                }

                // Set ContextMenuStrip for node.
                treeView1.ContextMenuStrip = e.Node.ContextMenuStrip;
                if (e.Node.Tag is TagData)
                {
                    TagData tag = e.Node.Tag as TagData;
                    if (tag.Type == Constants.xpathSystem ||
                        tag.Type == Constants.xpathProcess ||
                        tag.Type == Constants.xpathVariable)
                    {
                        EcellObject obj = GetObjectFromNode(e.Node);
                        CommonContextMenu m = new CommonContextMenu(obj, m_owner.Environment);
                        treeView1.ContextMenuStrip = m.Menu;
                    }
                }

                // Set availability.
                SetPopupMenuAvailability();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetPopupMenuAvailability()
        {
            ProjectStatus status = m_owner.PluginManager.Status;
            bool saved = m_owner.Environment.DataManager.CurrentProject.Info.ProjectType == ProjectType.Project;
            bool revision = m_owner.Environment.DataManager.CurrentProject.Info.ProjectType == ProjectType.Revision;
            bool simulation = (status == ProjectStatus.Running || status == ProjectStatus.Stepping || status == ProjectStatus.Suspended);
            bool current = m_lastSelectedNode is RevisionNode && m_lastSelectedNode.Text == Constants.xpathCurrent;

            // SimulationStatus
            configureSimulationSetToolStripMenuItem.Enabled = !simulation;
            // Revision
            loadRevisionMenuItem.Enabled = !simulation && (saved || revision);
            createNewRevisionMenuItem.Enabled = !simulation && saved;
            createNewRevisionOnProjectToolStripMenuItem.Enabled = !simulation && saved;
            exportRevisionEMLMenuItem.Enabled = !current;
            exportRevisionZipMenuItem.Enabled = !current;

            // Zip
            zipToolStripMenuItem.Enabled = !simulation && saved;

            if (m_lastSelectedNode is DMNode)
            {
                string path = m_owner.Environment.DataManager.GetDMFileName((string)m_lastSelectedNode.Tag);
                compileToolStripMenuItem.Enabled = (Util.IsInstalledSDK() && path != null);
                editToolStripMenuItem.Enabled = (path != null);
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

            if (e.Node is DMNode)
            {
                string path = m_owner.Environment.DataManager.GetDMFileName(e.Node.Text);
                if (path == null)
                    return;
                DisplayDMEditor(path);
            }
            else if (e.Node is ParamNode)
            {
                if (m_owner.PluginManager.Status == ProjectStatus.Running ||
                    m_owner.PluginManager.Status == ProjectStatus.Suspended ||
                    m_owner.PluginManager.Status == ProjectStatus.Stepping)
                {
                }
                else
                {
                    m_owner.Environment.PluginManager.SelectChanged(
                        "", (string)e.Node.Tag, Constants.xpathParameters);
                }
            }
            else if (e.Node.Tag != null && e.Node.Tag is TagData)
            {
                TagData tag = (TagData)e.Node.Tag;
                if (tag.Type.Equals(EcellObject.PROCESS) ||
                    tag.Type.Equals(EcellObject.VARIABLE) ||
                    tag.Type.Equals(EcellObject.SYSTEM))
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
                    if (t.Type.Equals(Constants.xpathLog))
                    {
                        fileList.Add(t.Key);
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
            if (m_lastSelectedNode == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null || tag.Type != Constants.xpathLog)
                return;

            string logFile = tag.Key;
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
                        tag.Key, Encoding.GetEncoding("Shift_JIS")))
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
            if (m_lastSelectedNode == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null || tag.Type != Constants.xpathLog)
                return;
     
            ShowGraphDelegate dlg = m_owner.PluginManager.GetDelegate("ShowGraphWithLog") as ShowGraphDelegate;

            dlg(tag.Key, true);
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
            if (m_lastSelectedNode == null)
                return;
            String name = m_lastSelectedNode.Tag as string;
            if (String.IsNullOrEmpty(name))
                return;

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
            if (m_lastSelectedNode == null)
                return;
            String name = m_lastSelectedNode.Tag as string;
            if (String.IsNullOrEmpty(name))
                return;

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
            if (m_lastSelectedNode == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null || tag.Type != Constants.xpathModel)
                return;
            String name = tag.ModelID;
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
            Project project = m_owner.Environment.DataManager.CurrentProject;
            string dir = project.Info.ProjectPath;
            string filename = project.Info.Name + Constants.FileExtZip;
            CompressZip(dir, filename);
        }

        private void CompressZip(string dir, string filename)
        {
            m_saveFileDialog.Filter = Constants.FilterZipFile;
            m_saveFileDialog.FileName = filename;
            if (m_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = m_saveFileDialog.FileName;
                ZipUtil.ZipFolder(filename, dir);
            }
        }

        private void projectSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectInfo info = m_owner.Environment.DataManager.CurrentProject.Info;
            ProjectSettingDialog dialog = new ProjectSettingDialog(info);
            using (dialog)
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                // Set new project.
                m_prjNode.Text = info.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewCloseProject(object sender, EventArgs e)
        {
            if (m_owner.Environment.ActionManager.Undoable)
            {
                try
                {
                    // Save if answer is yes.
                    if (Util.ShowYesNoCancelDialog(MessageResources.SaveConfirm))
                        m_owner.Environment.DataManager.SaveProject();
                }
                catch (Exception)
                {
                    // Return false when canceled
                    return;
                }
            }
            m_owner.Environment.DataManager.CloseProject();
        }

        private void importDMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog win = new FolderBrowserDialog();
            win.Description = MessageResources.SelectDMDir;
            using (win)
            {
                if (win.ShowDialog() != DialogResult.OK)
                    return;

                m_owner.Environment.DataManager.ImportDM(win.SelectedPath);
                m_DMNode.Nodes.Clear();
                SetDMNodes();
            }
        }

        private void exportRevisionEMLMenuItem_Click(object sender, EventArgs e)
        {
            Project project = m_owner.Environment.DataManager.CurrentProject;
            // Set Dialog.
            m_saveFileDialog.Filter = Constants.FilterEmlFile;
            m_saveFileDialog.FileName = project.Info.Name + "_" + m_lastSelectedNode.Text + Constants.FileExtEML;
            if (m_saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            string fileName = m_saveFileDialog.FileName;

            string dir = project.Info.ProjectPath;
            string revision = Path.Combine(dir, m_lastSelectedNode.Text);
            string modelDir = Path.Combine(revision, Constants.xpathModel);
            string[] models = Directory.GetFiles(modelDir, "*.eml");
            foreach (string model in models)
            {
                File.Copy(model, fileName, true);
            }
        }

        private void exportRevisionZipMenuItem_Click(object sender, EventArgs e)
        {
            Project project = m_owner.Environment.DataManager.CurrentProject;
            string dir = project.Info.ProjectPath;
            string revision = Path.Combine(dir, m_lastSelectedNode.Text);
            string filename = project.Info.Name + "_" + m_lastSelectedNode.Text + Constants.FileExtZip;
            CompressZip(revision, filename);
        }

        private void TreeViewDeleteStepper(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null)
                return;
            String name = m_lastSelectedNode.Text as string;
            if (String.IsNullOrEmpty(name))
                return;

            try
            {
                List<EcellObject> list = m_owner.DataManager.GetStepper(m_owner.DataManager.CurrentProject.Model.ModelID);
                EcellObject sobj = null;
                if (list == null || list.Count <= 1)
                {
                    Util.ShowErrorDialog(MessageResources.ErrDelStep);
                    return;
                }
                foreach (EcellObject obj in list)
                {
                    if (obj.Key.Equals(name))
                    {
                        sobj = obj;
                        break;
                    }
                }
                if (sobj != null)
                    m_owner.DataManager.DataDelete(sobj);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        private void TreeViewAddStepper(object sender, EventArgs e)
        {
            InputNameDialog dlg = new InputNameDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string name = dlg.InputText;
                string modelID = m_owner.DataManager.CurrentProject.Model.ModelID;
                List<EcellObject> list = m_owner.DataManager.GetStepper(modelID);
                foreach (EcellObject obj in list)
                {
                    if (obj.Key.Equals(name))
                    {
                        Util.ShowErrorDialog(MessageResources.ErrAlreadyExistStepper);
                        return;
                    }
                }
                EcellObject sobj = m_owner.DataManager.CreateDefaultObject(modelID, name, Constants.xpathStepper);
                m_owner.DataManager.DataAdd(sobj);
            }
        }
        #endregion

        #region ShortCuts
        private void DeletedSelectionRow()
        {
            List<TagData> delList = new List<TagData>();
            foreach (TreeNode node in treeView1.SelNodes)
            {
                if (node.Tag == null) 
                    continue;
                TagData obj = node.Tag as TagData;
                if (obj == null)
                    continue;
                if (obj.Type.Equals(Constants.xpathLog))
                    continue;
                if (obj != null) delList.Add(obj);
            }

            for (int i = 0; i < delList.Count; i++)
            {
                TagData obj = delList[i];
                if (i == delList.Count - 1)
                    m_owner.DataManager.DataDelete(obj.ModelID, obj.Key, obj.Type, true, true);
                else
                    m_owner.DataManager.DataDelete(obj.ModelID, obj.Key, obj.Type, true, false);
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

    #region Node classes
    /// <summary>
    /// 
    /// </summary>
    internal class ProjectNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal ProjectNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class ModelRootNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal ModelRootNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class ModelNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal ModelNode(string text)
            : base(text)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class ParamRootNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal ParamRootNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class ParamNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal ParamNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class DMRootNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal DMRootNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class DMNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal DMNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class LogRootNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal LogRootNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class LogNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal LogNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class RevisionRootNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal RevisionRootNode(string text)
            : base(text)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    internal class RevisionNode : TreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal RevisionNode(string text)
            : base(text)
        {
        }
    }
    #endregion

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
            if (tagx.Type == tagy.Type)
            {
                return string.Compare(tx.Text, ty.Text);
            }
            return GetTypeNum(tagx.Type) - GetTypeNum(tagy.Type);
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
        public string ModelID;
        /// <summary>
        /// m_key (key ID of tree node tag)
        /// </summary>
        public string Key;
        /// <summary>
        /// m_type (type ID of tree node tag)
        /// </summary>
        public string Type;

        /// <summary>
        /// constructor for TagData.
        /// </summary>
        public TagData()
        {
            this.ModelID = "";
            this.Key = "";
            this.Type = "project";
        }

        /// <summary>
        /// constructor for TagData with initial value.
        /// </summary>
        /// <param name="modelID">the initial model ID</param>
        /// <param name="key">the initial key ID</param>
        /// <param name="type">the initial type ID</param>
        public TagData(string modelID, string key, string type)
        {
            this.ModelID = modelID;
            this.Key = key;
            this.Type = type;
        }
    }

    #endregion

}
