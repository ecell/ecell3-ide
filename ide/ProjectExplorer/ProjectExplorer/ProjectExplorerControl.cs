//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
    /// <summary>
    /// Control to display the project by TreeView.
    /// </summary>
    public partial class ProjectExplorerControl : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// The flag whether tree node is expand.
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
        /// Analysis tree node in TreeView.
        /// </summary>
        private AnalysisRootNode m_analysisNode = null;
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
        /// Sorter by name.
        /// </summary>
        System.Collections.IComparer m_nameSorter;
        /// <summary>
        /// Sorter by type.
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
            m_owner.Environment.JobManager.JobUpdateEvent += new Ecell.Job.JobUpdateEventHandler(UpdateJobStatus);
            m_owner.Environment.PluginManager.NodeImageListChange += new EventHandler(PluginManager_NodeImageListChange);
            m_owner.Environment.DataManager.ReloadSimulatorEvent += new ReloadSimulatorEventHandler(DataManager_ReloadSimulatorEvent);
            deleteDMToolStripMenuItem.Visible = true;
        }
        #endregion

        #region Methods for EcellPlugin
        /// <summary>
        /// Change the status of project.
        /// </summary>
        /// <param name="type">project status.</param>
        internal void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Loaded)
                m_prjNode.Text = m_owner.Environment.DataManager.CurrentProjectID;
        }
        /// <summary>
        /// Add the node with loading the project.
        /// </summary>
        /// <param name="data">the list of EcellObjects.</param>
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
                    node.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(obj);
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
        /// Change the object in the load project.
        /// </summary>
        /// <param name="modelID">the original model ID.</param>
        /// <param name="key">the original key.</param>
        /// <param name="type">the original type.</param>
        /// <param name="data">the new object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            // Set Project
            if (data is EcellProject)
            {
                ClearFolderNodes();
                m_prjNode.Text = data.ModelID;
                SetLogEntry(m_logNode);
                SetRevisions();
                SetDMNodes();
                SetAnalysisNode();
                return;
            }

            TreeNode current = GetTargetModel(modelID);
            if (current == null)
                return;

            if (type == Constants.xpathStepper)
            {
                foreach (TreeNode node in current.Nodes)
                {
                    TagData tag = (TagData)node.Tag;
                    if (tag == null)
                        continue;
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
                target.ImageIndex = m_owner.PluginManager.GetImageIndex(data);
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
        /// Delete the object from the load project.
        /// </summary>
        /// <param name="modelID">the model ID of deleted object.</param>
        /// <param name="key">the key of deleted object.</param>
        /// <param name="type">the type of deleted object.</param>
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
        /// Select the set node.
        /// </summary>
        /// <param name="modelID">the model ID of selected object.</param>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
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
        /// Unselect the set node.
        /// </summary>
        /// <param name="modelID">the model ID of unselected object.</param>
        /// <param name="key">the key of unselected object.</param>
        /// <param name="type">the type of unselected object.</param>
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
        /// Add the parameter node.
        /// </summary>
        /// <param name="projectID">the project id.</param>
        /// <param name="parameterID">the parameter id.</param>
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
        /// Delete the parameter node.
        /// </summary>
        /// <param name="projectID">the project id.</param>
        /// <param name="parameterID">the parameter id.</param>
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
        /// Clear the all nodes.
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
            m_analysisNode = null;
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
        /// Set the log entry in the load project.
        /// </summary>
        /// <param name="node">the top node.</param>
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
        /// Refresh the log entry node.
        /// </summary>
        public void RefreshLogEntry()
        {
            if (m_logNode == null)
                return;
            m_logNode.Nodes.Clear();
            SetLogEntry(m_logNode);
        }

        /// <summary>
        /// Create Project Node.
        /// </summary>
        /// <param name="obj">the project object.</param>
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

            // Create AnalysisNode.
            m_analysisNode = new AnalysisRootNode(MessageResources.NameAnalysis);
            m_analysisNode.Tag = null;
            m_prjNode.Nodes.Add(m_analysisNode);
            SetAnalysisNode();
        }

        /// <summary>
        /// Set the DMs to nodes.
        /// </summary>
        private void SetDMNodes()
        {
            List<string> fileList = m_owner.Environment.DataManager.GetDMNameList();
            foreach (string d in fileList)
            {
                if (!Util.IsDMFile(d))
                    continue;
                AddDM(d, null);
            }
        }

        /// <summary>
        /// Reset DM information.
        /// </summary>
        public void ResetDM()
        {
            m_DMNode.Nodes.Clear();
            SetDMNodes();
        }

        /// <summary>
        /// Add the DM node from the selected path.
        /// </summary>
        /// <param name="dmName">the DM name.</param>
        /// <param name="path">the DM file path.</param>
        public void AddDM(string dmName, string path)
        {
            DMNode dNode = new DMNode(dmName);
            dNode.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathDM);
            dNode.SelectedImageIndex = dNode.ImageIndex;
            dNode.Tag = dmName;
            dNode.ContextMenuStrip = this.contextMenuStripDM;
            m_DMNode.Nodes.Add(dNode);
        }

        /// <summary>
        /// Set the analysis node in the load project.
        /// </summary>
        private void SetAnalysisNode()
        {
            m_analysisNode.Nodes.Clear();

            foreach (string groupname in m_owner.Environment.JobManager.GroupDic.Keys)
            {
                if (!m_owner.Environment.JobManager.GroupDic[groupname].IsSaved)
                    continue;
                AnalysisNode node = new AnalysisNode(groupname);
                node.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(Constants.xpathAnalysis);
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = groupname;
                node.ContextMenuStrip = contextMenuStripJobGroup;

                m_analysisNode.Nodes.Add(node);
            }
        }

        /// <summary>
        /// Set the revision node in the load project.
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
        /// Clear the nodes of project.
        /// </summary>
        private void ClearFolderNodes()
        {
            m_logNode.Nodes.Clear();
            m_analysisNode.Nodes.Clear();
            m_revisionNode.Nodes.Clear();
            m_DMNode.Nodes.Clear();
        }

        /// <summary>
        /// Reset icon of TreeNode.
        /// </summary>
        /// <param name="node">the reset node.</param>
        private void ResetIcon(TreeNode node)
        {
            // Reset children
            foreach (TreeNode child in node.Nodes)
            {
                ResetIcon(child);
            }

            // Reset Icon.
            if (node.Tag == null || !(node.Tag is TagData))
                return;
            TagData tag = (TagData)node.Tag;
            EcellObject eo = m_owner.DataManager.GetEcellObject(tag.ModelID, tag.Key, tag.Type);
            if (eo == null)
                return;
            int index = m_owner.PluginManager.GetImageIndex(eo);
            node.ImageIndex = index;
            node.SelectedImageIndex = index;
        }

        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="oList">the list of dragged object.</param>
        /// <param name="fileList">the list of log file.</param>
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
        /// <param name="modelID">the model ID of changed object.</param>
        /// <param name="key">the key of changed obect.</param>
        /// <param name="type">the type of changed object,</param>
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
                treeView1.SelectedNode = target;
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

            node.ImageIndex = m_owner.Environment.PluginManager.GetImageIndex(obj);
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
        /// Show DM editor.
        /// </summary>
        /// <param name="path">the dm file.</param>
        private void DisplayDMEditor(string path)
        {
            m_owner.ShowDMEditor(path);
        }

        /// <summary>
        /// Show DM editor with application.
        /// </summary>
        /// <param name="path">the dm file.</param>
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

        /// <summary>
        /// Set the enable of popupmenu.
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
            //            exportRevisionEMLMenuItem.Enabled = !current;
            exportRevisionZipMenuItem.Enabled = !current;

            // Zip
            zipToolStripMenuItem.Enabled = !simulation && saved;

            if (m_lastSelectedNode is DMNode)
            {
                string path = m_owner.Environment.DataManager.GetDMSourceFileName((string)m_lastSelectedNode.Tag);
                compileToolStripMenuItem.Enabled = (Util.IsInstalledSDK() && path != null);
                deleteDMToolStripMenuItem.Enabled = m_owner.DataManager.CurrentProject.SimulationStatus == SimulationStatus.Wait;
                editToolStripMenuItem.Enabled = (path != null);
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
            string path = m_owner.Environment.DataManager.GetDMSourceFileName((string)m_lastSelectedNode.Tag);
            if (path == null) return;
            DMCompiler.Compile(path, m_owner.Environment);
        }

        /// <summary>
        /// Click the create DM menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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
            CreateDMDialog ind = new CreateDMDialog(m_owner.Environment, dmDir, m_lastSelectedNode, contextMenuStripDM);
            using (ind)
            {
                try
                {
                    DialogResult res = ind.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        ind.CreateDM();
                        string path = ind.FilePath;
                        DisplayDMEditor(path);
                    }
                }
                catch (Ecell.Exceptions.IgnoreException)
                {
                    // nothing.
                }
                catch (Exception)
                {
                    Util.ShowErrorDialog(string.Format(MessageResources.ErrCreateFile, ind.FilePath));
                }
            }
        }

        /// <summary>
        /// Click the display dm menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs.</param>
        private void TreeViewDMDisplay(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null) return;
            string path = m_owner.Environment.DataManager.GetDMSourceFileName((string)m_lastSelectedNode.Tag);
            if (path == null) return;
            DisplayDMEditor(path);
        }

        /// <summary>
        /// Click the display dm with application menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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
        /// Click the node.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs</param>
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
        /// Double click the node.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs</param>
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
                string path = m_owner.Environment.DataManager.GetDMSourceFileName(e.Node.Text);
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
        /// Sort by name.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewSortByName(object sender, EventArgs e)
        {
            treeView1.TreeViewNodeSorter = m_nameSorter;
            treeView1.Sort();
            toolStripButtonSortByName.Checked = true;
            toolStripButtonSortByType.Checked = false;
        }

        /// <summary>
        /// Sort by type.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewSortByType(object sender, EventArgs e)
        {
            treeView1.TreeViewNodeSorter = m_typeSorter;
            treeView1.Sort();
            toolStripButtonSortByName.Checked = false;
            toolStripButtonSortByType.Checked = true;
        }

        /// <summary>
        /// Drag the object in TreeView.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">ItemDragEventArgs</param>
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
        /// Event before collapse.
        /// </summary>
        /// <param name="sender">TreeView</param>
        /// <param name="e">TreeViewCancelEventArgs</param>
        private void TreeViewBeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (m_isExpland)
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// Event before expand.
        /// </summary>
        /// <param name="sender">TreeView</param>
        /// <param name="e">TreeViewCancelEventArgs</param>
        private void TreeViewBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (m_isExpland)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Click the export log menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewExportLog(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null || tag.Type != Constants.xpathLog)
                return;

            m_saveFileDialog.FileName = "";
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
        /// Click the show log menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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
        /// Click the add simulation sets menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs.</param>
        private void TreeViewAddSimulationSet(object sender, EventArgs e)
        {
            InputNameDialog dlg = new InputNameDialog(false);
            dlg.OwnerForm = m_owner;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                m_owner.DataManager.CopySimulationParameter(dlg.InputText,
                    m_owner.DataManager.CurrentProject.Info.SimulationParam);
            }
        }

        /// <summary>
        /// Click the copy simulation sets menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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
        /// Click the delete simulation sets menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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
        /// Click the export model to eml menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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

            m_saveFileDialog.FileName = "";
            m_saveFileDialog.Filter = Constants.FilterEmlFile;
            if (m_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = m_saveFileDialog.FileName;

                m_owner.DataManager.ExportModel(modelList, fileName);
            }
        }

        /// <summary>
        /// Click the export model to sbml menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewExportModel2SBML(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null)
                return;
            TagData tag = m_lastSelectedNode.Tag as TagData;
            if (tag == null || tag.Type != Constants.xpathModel)
                return;
            String name = tag.ModelID;
            List<string> modelList = new List<string>();
            modelList.Add(name);

            m_saveFileDialog.FileName = "";
            m_saveFileDialog.Filter = Constants.FilterSBMLFile;
            if (m_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = m_saveFileDialog.FileName;

                m_owner.DataManager.ExportSBML(fileName);
            }
        }

        /// <summary>
        /// Click the create revision menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewCreateNewRevision(object sender, EventArgs e)
        {
            m_owner.DataManager.CreateNewRevision();
            SetRevisions();
        }

        /// <summary>
        /// Click the load revision menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewLoadRevision(object sender, EventArgs e)
        {
            if (m_lastSelectedNode == null)
                return;

            m_owner.DataManager.LoadRevision(m_lastSelectedNode.Text);
            SetRevisions();
        }

        /// <summary>
        /// Click the config the simulation sets menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs.</param>
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
        /// Click the compress zip menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewCompressZip(object sender, EventArgs e)
        {
            Project project = m_owner.Environment.DataManager.CurrentProject;
            string dir = project.Info.ProjectPath;
            string filename = project.Info.Name + Constants.FileExtZip;
            CompressZip(dir, filename);
        }

        /// <summary>
        /// Compress to zip file.
        /// </summary>
        /// <param name="dir">the source directory.</param>
        /// <param name="filename">the destination file name.</param>
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

        /// <summary>
        /// Click the set the project menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewSetProject(object sender, EventArgs e)
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
        /// Click the close project menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewCloseProject(object sender, EventArgs e)
        {
            if (m_owner.Environment.ActionManager.Undoable && m_owner.Environment.DataManager.CurrentProject.Info.ProjectType != ProjectType.Revision)
            {
                try
                {
                    // Save if answer is yes.
                    if (Util.ShowYesNoCancelDialog(MessageResources.SaveConfirm))
                        m_owner.Environment.DataManager.SaveProject(ProjectType.Project);
                }
                catch (Exception)
                {
                    // Return false when canceled
                    return;
                }
            }
            m_owner.Environment.DataManager.CloseProject();
        }

        /// <summary>
        /// Click the import DM menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewImportDM(object sender, EventArgs e)
        {
            // Get DM dir.
            string dmDir = m_owner.Environment.DataManager.GetDMDir();
            if (dmDir == null)
            {
                Util.ShowErrorDialog(MessageResources.ErrProjectUnsavedImport);
                return;
            }

            FolderBrowserDialog win = new FolderBrowserDialog();
            win.Description = MessageResources.SelectDMDir;
            using (win)
            {
                if (win.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    m_owner.Environment.DataManager.ImportDM(win.SelectedPath);
                }
                catch (Exception)
                {
                    Util.ShowErrorDialog(MessageResources.ErrImportDM);
                }
                // 20090727
                m_owner.DataManager.UnloadSimulator();
                m_owner.DataManager.ReloadSimulator();
                m_DMNode.Nodes.Clear();
                SetDMNodes();
            }
        }

        /// <summary>
        /// Click the export revision menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewExportRevision(object sender, EventArgs e)
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

        /// <summary>
        /// Click the export revision to Zip menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewExportRevisionToZip(object sender, EventArgs e)
        {
            Project project = m_owner.Environment.DataManager.CurrentProject;
            string dir = project.Info.ProjectPath;
            string revision = Path.Combine(dir, m_lastSelectedNode.Text);
            string filename = project.Info.Name + "_" + m_lastSelectedNode.Text + Constants.FileExtZip;
            CompressZip(revision, filename);
        }

        /// <summary>
        /// Click the delete stepper menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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

        /// <summary>
        /// Click the add Stepper menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewAddStepper(object sender, EventArgs e)
        {
            InputNameDialog dlg = new InputNameDialog(true);
            dlg.OwnerForm = m_owner;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string name = dlg.InputText;
                string modelID = m_owner.DataManager.CurrentProject.Model.ModelID;
                EcellObject sobj = m_owner.DataManager.CreateDefaultObject(modelID, name, Constants.xpathStepper);
                sobj.Key = name;
                m_owner.DataManager.DataAdd(sobj);
            }
        }

        /// <summary>
        /// Click the property of DM menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeViewDMProperty(object sender, EventArgs e)
        {
            DMDescriptor desc = null;
            string dmName = m_lastSelectedNode.Text;
            if (dmName.EndsWith(Constants.xpathProcess))
            {
                desc = m_owner.Environment.DMDescriptorKeeper.GetDMDescriptor(Constants.xpathProcess, dmName);
            }
            else if (dmName.EndsWith(Constants.xpathStepper))
            {
                desc = m_owner.Environment.DMDescriptorKeeper.GetDMDescriptor(Constants.xpathStepper, dmName);
            }
            if (desc == null) return;
            Util.ShowNoticeDialog(desc.Description);
        }

        /// <summary>
        /// Update the job status.
        /// </summary>
        /// <param name="o">JobManager</param>
        /// <param name="e">JobUpdateEventArgs</param>
        private void UpdateJobStatus(object o, Ecell.Job.JobUpdateEventArgs e)
        {
            if (m_analysisNode == null)
                return;
            if (e.Type == Ecell.Job.JobUpdateType.AddJobGroup ||
                e.Type == Ecell.Job.JobUpdateType.DeleteJobGroup ||
                e.Type == Ecell.Job.JobUpdateType.SaveJobGroup)
            {
                SetAnalysisNode();
            }
        }

        /// <summary>
        /// View result on the job group.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeView_ViewResult(object sender, EventArgs e)
        {
            string groupName = m_lastSelectedNode.Text;
            if (m_owner.Environment.JobManager.GroupDic.ContainsKey(groupName))
            {
                m_owner.Environment.JobManager.GroupDic[groupName].AnalysisModule.PrintResult();
            }
        }

        /// <summary>
        /// Delete the job group.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeView_DeleteJobGroup(object sender, EventArgs e)
        {
            string groupName = m_lastSelectedNode.Text;
            if (m_owner.Environment.JobManager.GroupDic.ContainsKey(groupName))
            {
                m_owner.Environment.JobManager.GroupDic[groupName].IsSaved = false;
                m_owner.Environment.JobManager.RemoveJobGroup(groupName);
            }
        }

        /// <summary>
        /// Delete the DM.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void TreeView_DeleteDM(object sender, EventArgs e)
        {
            if (m_lastSelectedNode is DMNode)
            {
                DMNode node = m_lastSelectedNode as DMNode;
                string classname = node.Text;
                if (classname.EndsWith(Constants.xpathProcess))
                {
                    if (m_owner.DataManager.CurrentProject.IsUsedProcessClass(classname))
                    {
                        Util.ShowErrorDialog(MessageResources.WarnUsedDM);
                        return;
                    }
                }
                else
                {
                    if (m_owner.DataManager.CurrentProject.IsUsedStepperClass(classname))
                    {
                        Util.ShowErrorDialog(MessageResources.WarnUsedDM);
                        return;
                    }
                }
                string path = m_owner.DataManager.GetDMDLLFileName((string)m_lastSelectedNode.Tag);
                string source = m_owner.DataManager.GetDMSourceFileName((string)m_lastSelectedNode.Tag);

                string destpath = path + ".tmp";
                try
                {
                    if (File.Exists(destpath))
                        File.Delete(destpath);
                }
                catch (Exception)
                {
                    Util.ShowWarningDialog(MessageResources.WarnDeleteDM);
                    return;
                }
                m_owner.DataManager.UnloadSimulator();
                if (File.Exists(path))
                    File.Move(path, destpath);
                if (File.Exists(source))
                {
                    File.WriteAllText(source, "");
                    m_owner.DeleteDM(source);
                    File.Delete(source);
                }
                m_owner.DataManager.ReloadSimulator();
                //if (File.Exists(destpath))
                //    File.Delete(destpath);
            }
        }

        /// <summary>
        /// Event when node image list is changed.
        /// </summary>
        /// <param name="sender">PluginManager</param>
        /// <param name="e">EventArgs</param>
        void PluginManager_NodeImageListChange(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView1.Nodes)
                ResetIcon(node);
        }

        /// <summary>
        /// Event when the simulator is reloaded.
        /// </summary>
        /// <param name="o">DataManager</param>
        /// <param name="e">EventArgs</param>
        private void DataManager_ReloadSimulatorEvent(object o, EventArgs e)
        {
            ResetDM();
        }
        #endregion

        #region ShortCuts
        /// <summary>
        /// Delete the selected rows.
        /// </summary>
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

            try
            {
                for (int i = 0; i < delList.Count; i++)
                {
                    TagData obj = delList[i];
                    if (i == delList.Count - 1)
                        m_owner.DataManager.DataDelete(obj.ModelID, obj.Key, obj.Type, true, true);
                    else
                        m_owner.DataManager.DataDelete(obj.ModelID, obj.Key, obj.Type, true, false);
                }
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// Press key on TreeView.
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="keyData">Key data.</param>
        /// <returns>the flag whether this event is handled.</returns>
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
    /// TreeNode for the project.
    /// </summary>
    internal class ProjectNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">project ID.</param>
        internal ProjectNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the model root.
    /// </summary>
    internal class ModelRootNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">Models</param>
        internal ModelRootNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the model.
    /// </summary>
    internal class ModelNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">model name.</param>
        internal ModelNode(string text)
            : base(text)
        {

        }
    }

    /// <summary>
    /// TreeNode for the parameter root.
    /// </summary>
    internal class ParamRootNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">Parameters.</param>
        internal ParamRootNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the parameter.
    /// </summary>
    internal class ParamNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">the parameter name.</param>
        internal ParamNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the DM root.
    /// </summary>
    internal class DMRootNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">DMs</param>
        internal DMRootNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the DM.
    /// </summary>
    public class DMNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">the dm name.</param>
        internal DMNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the Log root.
    /// </summary>
    internal class LogRootNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">Logs</param>
        internal LogRootNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the Log.
    /// </summary>
    internal class LogNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">the log entry.</param>
        internal LogNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the revision root.
    /// </summary>
    internal class RevisionRootNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">Revisions</param>
        internal RevisionRootNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the revision.
    /// </summary>
    internal class RevisionNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">the revision number.</param>
        internal RevisionNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the root of analysis.
    /// </summary>
    internal class AnalysisRootNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">"Analysis"</param>
        internal AnalysisRootNode(string text)
            : base(text)
        {
        }
    }

    /// <summary>
    /// TreeNode for the analysis.
    /// </summary>
    internal class AnalysisNode : TreeNode
    {
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">group name.</param>
        internal AnalysisNode (string text)
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
        /// Compare with two TreeNode by name.
        /// </summary>
        /// <param name="tx">the compared object.</param>
        /// <param name="ty">the compare object.</param>
        /// <returns>the compare result.</returns>
        public int Compare(TreeNode tx, TreeNode ty)
        {
            return string.Compare(tx.Text, ty.Text);
        }

        /// <summary>
        /// Compare with two object.
        /// </summary>
        /// <param name="x">the compared object.</param>
        /// <param name="y">the compare object.</param>
        /// <returns>the compare result.</returns>
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
        /// <param name="tx">the compared object.</param>
        /// <param name="ty">the compare object.</param>
        /// <returns>the compare result.</returns>
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
        /// Compare with two object.
        /// </summary>
        /// <param name="x">the compared object.</param>
        /// <param name="y">the compare object.</param>
        /// <returns>the compare result.</returns>
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
