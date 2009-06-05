//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ecell.Job;
using Ecell.Plugin;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// Form to display the status of jobs.
    /// </summary>
    public partial class GridJobStatusDialog : EcellDockContent
    {
        #region
        /// <summary>
        /// SessionManager
        /// </summary>
        private IJobManager m_manager;
        private TreeNode m_topNode = null;
        private Job.Job m_job = null;
        private JobGroup m_group = null;
        private Dictionary<string, TreeNode> m_pointDic = new Dictionary<string, TreeNode>();
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public GridJobStatusDialog(IJobManager manager)
            : base()
        {
            InitializeComponent();

            m_manager = manager;
            m_manager.JobUpdateEvent += new JobUpdateEventHandler(UpdateJobStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void AddJobGroup(string name)
        {
            JobGroup group = m_manager.GroupDic[name];
            string analysisName = group.AnalysisName;
            if (!m_pointDic.ContainsKey(analysisName))
            {
                TreeNode node = new TreeNode(analysisName);
                m_topNode.Nodes.Add(node);
                m_pointDic[analysisName] = node;
            }

            foreach (TreeNode node in m_pointDic[analysisName].Nodes)
            {
                if (node.Text.Equals(name))
                    return;
            }

            JobGroupTreeNode groupNode = new JobGroupTreeNode(name);
            groupNode.ContextMenuStrip = jobGroupContextMenuStrip;
            SetImageAtJobGroupStatus(groupNode, group.Status);
            m_pointDic[analysisName].Nodes.Add(groupNode);
            foreach (Job.Job job in group.Jobs)
            {
                JobTreeNode jobNode = new JobTreeNode(name, job.JobID.ToString());
                SetImageAtJobStatus(jobNode, job.Status);
                jobNode.ContextMenuStrip = jobContextMenuStrip;
                groupNode.Nodes.Add(jobNode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void DeleteJobGroup(string name)
        {
            JobGroup group = m_manager.GroupDic[name];
            string analysisName = group.AnalysisName;
            if (!m_pointDic.ContainsKey(analysisName))
                return;
            foreach (TreeNode node in m_pointDic[analysisName].Nodes)
            {
                if (node.Text.Equals(name))
                {
                    m_pointDic[analysisName].Nodes.Remove(node);
                    return;
                }
            }
        }

        /// <summary>
        /// Event when the close button is clicked.
        /// Close this form.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void CloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Clear information of job or job group.
        /// </summary>
        private void ClearInformation()
        {
            jobIDTextBox.Text = "";
            statusTextBox.Text = "";
            parameterDataGridView.Rows.Clear();            
        }

        /// <summary>
        /// Project is closed.
        /// </summary>
        public void Clear()
        {
            ClearInformation();
            jobTreeView.Nodes.Clear();
        }

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void UpdateJobStatus(object o, EventArgs e)
        {
            foreach (string name in m_manager.GroupDic.Keys)
            {
                AddJobGroup(name);
                JobGroup group = m_manager.GroupDic[name];
                JobGroupTreeNode node = null;
                foreach (TreeNode n in m_pointDic[group.AnalysisName].Nodes)
                {
                    if (n.Text.Equals(name))
                    {
                        node = (JobGroupTreeNode)n;
                        break;
                    }
                }

                SetImageAtJobGroupStatus(node, group.Status);

                foreach (Job.Job j in group.Jobs)
                {
                    bool isHit = false;
                    foreach (TreeNode n in node.Nodes)
                    {
                        if (n.Text.Equals(j.JobID.ToString()))
                        {
                            SetImageAtJobStatus(n, j.Status);
                            isHit = true;
                            break;
                        }
                    }
                    if (isHit)
                        continue;

                    JobTreeNode jn = new JobTreeNode(j.GroupName, j.JobID.ToString());
                    jn.ContextMenuStrip = jobContextMenuStrip;
                    SetImageAtJobStatus(jn, j.Status);
                    node.Nodes.Add(jn);
                }
            }
        }

        /// <summary>
        /// Event when this form is shown.
        /// Display the all list of job that is got from SessionManager.
        /// </summary>
        /// <param name="sender">Form.</param>
        /// <param name="e">EventArgs.</param>
        private void WinShown(object sender, EventArgs e)
        {
            jobTreeView.Nodes.Clear();
            m_pointDic.Clear();

            m_topNode = new TreeNode(m_manager.Environment.DataManager.CurrentProject.Info.Name);
            jobTreeView.Nodes.Add(m_topNode);

            foreach (string name in m_manager.GroupDic.Keys)
            {
                AddJobGroup(name);
            }
        }

        private void JobTee_MouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            ClearInformation();
            m_job = null;
            m_group = null;
            if (!(node is JobTreeNode) && !(node is JobGroupTreeNode))
            {
                return;
            }
            if (node is JobTreeNode)
            {                
                JobTreeNode jobNode = node as JobTreeNode;
                Job.Job job = m_manager.GroupDic[jobNode.GroupName].GetJob(Int32.Parse(node.Text));
                jobIDTextBox.Text = job.JobID.ToString();
                statusTextBox.Text = GetJobStatusString(job.Status);
                foreach (string fullPN in job.ExecParam.ParamDic.Keys)
                {
                    double data = job.ExecParam.ParamDic[fullPN];
                    parameterDataGridView.Rows.Add(new object[] { fullPN, data });
                }
                m_job = job;
            }
            if (node is JobGroupTreeNode)
            {
                JobGroup group = m_manager.GroupDic[node.Text];
                jobIDTextBox.Text = group.GroupName;
                statusTextBox.Text = GetAnalysisStatusString(group.Status);
                foreach (string name in group.AnalysisParameter.Keys)
                {
                    string data = group.AnalysisParameter[name];
                    parameterDataGridView.Rows.Add(new object[] { name, data });
                }
                m_group = group;
            }
        }

        private void JobTree_RunJob(object sender, EventArgs e)
        {
            TreeNode node = jobTreeView.SelectedNode;
            if (node == null || !(node is JobTreeNode))
                return;
            JobTreeNode jnode = node as JobTreeNode;

            m_manager.Run(jnode.GroupName, Int32.Parse(jnode.ID));
        }

        private void JobTree_StopJob(object sender, EventArgs e)
        {
            TreeNode node = jobTreeView.SelectedNode;
            if (node == null || !(node is JobTreeNode))
                return;
            JobTreeNode jnode = node as JobTreeNode;

            m_manager.Stop(jnode.GroupName, Int32.Parse(jnode.ID));
        }

        private void JobTree_DeleteJob(object sender, EventArgs e)
        {
            TreeNode node = jobTreeView.SelectedNode;
            if (node == null || !(node is JobTreeNode))
                return;
            JobTreeNode jnode = node as JobTreeNode;

            m_manager.DeleteJob(jnode.GroupName, Int32.Parse(jnode.ID));
        }

        private void JobTree_ChangeJobStatus(object sender, EventArgs e)
        {
            ToolStripMenuItem m = sender as ToolStripMenuItem;
            if (m == null) return;
            string status = m.Tag.ToString();
            TreeNode node = jobTreeView.SelectedNode;
            if (node == null || !(node is JobTreeNode))
                return;
            JobTreeNode jnode = node as JobTreeNode;

            Job.Job j = m_manager.GroupDic[jnode.GroupName].GetJob(Int32.Parse(jnode.ID));
            if (status.Equals("Queued"))
            {
                j.Status = JobStatus.QUEUED;
                m_manager.GroupDic[jnode.GroupName].UpdateStatus();
            }
            else if (status.Equals("Error"))
            {
                j.Status = JobStatus.ERROR;
                m_manager.GroupDic[jnode.GroupName].UpdateStatus();
            }
            UpdateJobStatus(this, new EventArgs());
        }

        private void JobTree_RunJobGroup(object sender, EventArgs e)
        {
            ToolStripMenuItem m = sender as ToolStripMenuItem;
            if (m == null) return;
            TreeNode node = jobTreeView.SelectedNode;
            if (node == null || !(node is JobGroupTreeNode))
                return;
            JobGroupTreeNode jnode = node as JobGroupTreeNode;

            m_manager.GroupDic[jnode.GroupName].AnalysisModule.PrepareReAnalysis();
            m_manager.Run(jnode.GroupName, true);
        }

        private void JobTree_StopJobGroup(object sender, EventArgs e)
        {
            ToolStripMenuItem m = sender as ToolStripMenuItem;
            if (m == null) return;
            TreeNode node = jobTreeView.SelectedNode;
            if (node == null || !(node is JobGroupTreeNode))
                return;
            JobGroupTreeNode jnode = node as JobGroupTreeNode;

            m_manager.Stop(jnode.GroupName, 0);
        }

        private void JobTree_DelteJobGroup(object sender, EventArgs e)
        {
            ToolStripMenuItem m = sender as ToolStripMenuItem;
            if (m == null) return;
            TreeNode node = jobTreeView.SelectedNode;
            if (node == null || !(node is JobGroupTreeNode))
                return;
            JobGroupTreeNode jnode = node as JobGroupTreeNode;
            string name = jnode.GroupName;
            DeleteJobGroup(name);
            m_manager.RemoveJobGroup(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobTree_SaveJobGroup(object sender, EventArgs e)
        {
            // not implements
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobTree_LoadJobGroup(object sender, EventArgs e)
        {
            // not implements
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobTree_JobGroupContextOpening(object sender, CancelEventArgs e)
        {
            TreeNode node = jobTreeView.SelectedNode;
            if (node == null || !(node is JobGroupTreeNode))
            {
                e.Cancel = true;
                return;
            }

            JobGroupTreeNode jnode = node as JobGroupTreeNode;
            JobGroup g = m_manager.GroupDic[jnode.GroupName];
            jobGroupRunToolStripMenuItem.Enabled = g.Status == AnalysisStatus.Finished ||
                g.Status == AnalysisStatus.Stopped || g.Status == AnalysisStatus.Error;
            jobGroupStopToolStripMenuItem.Enabled = g.Status == AnalysisStatus.Running ||
                g.Status == AnalysisStatus.Waiting;
            jobGroupSaveStripMenuItem.Enabled = g.Status == AnalysisStatus.Finished ||
                g.Status == AnalysisStatus.Error;
            jobGroupLoadToolStripMenuItem.Enabled = g.IsSaved;
            jobGroupDeleteToolStripMenuItem.Enabled = g.Status != AnalysisStatus.Running &&
                g.Status != AnalysisStatus.Waiting;
        }
       
        private void JobGrid_ChangeProperty(object sender, DataGridViewCellParsingEventArgs e)
        {
            e.ParsingApplied = true;
            string name = "";
            string orgdata = parameterDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
            try
            {
                if (m_job != null)
                {
                    Dictionary<string, double> newParam = new Dictionary<string, double>();
                    foreach (DataGridViewRow r in parameterDataGridView.Rows)
                    {
                        string paramName = r.Cells[PropNameColumn.Index].Value.ToString();
                        name = paramName;                        
                        double value;
                        if (r.Index == e.RowIndex)
                            value = Double.Parse(e.Value.ToString());
                        else
                            value = Double.Parse(r.Cells[PropValueColumn.Index].Value.ToString());
                        newParam.Add(paramName, value);
                    }
                    m_job.ExecParam = new ExecuteParameter(newParam);
                }
                else if (m_group != null)
                {
                    Dictionary<string, string> newParame = new Dictionary<string, string>();
                    foreach (DataGridViewRow r in parameterDataGridView.Rows)
                    {
                        string paramName = r.Cells[PropNameColumn.Index].Value.ToString();
                        string value;
                        if (r.Index == e.RowIndex)
                            value = e.Value.ToString();
                        else
                            value = r.Cells[PropValueColumn.Index].Value.ToString();
                        newParame.Add(paramName, value);
                    }
                    name = m_group.GroupName;
                    m_group.AnalysisParameter = newParame;
                }
            }
            catch (Exception)
            {
                e.Value = orgdata.ToString();
                parameterDataGridView.Refresh();
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private static string GetJobStatusString(JobStatus status)
        {
            switch (status)
            {
                case JobStatus.NONE:
                    return MessageResources.NameStatusNone;
                case JobStatus.QUEUED:
                    return MessageResources.NameStatusQueue;
                case JobStatus.RUNNING:
                    return MessageResources.NameStatusRunning;
                case JobStatus.FINISHED:
                    return MessageResources.NameStatusFinished;
                case JobStatus.STOPPED:
                    return MessageResources.NameStatusStopped;
                case JobStatus.ERROR:
                    return MessageResources.NameStatusError;
            }
            return MessageResources.NameStatusNone;
        }

        private static string GetAnalysisStatusString(AnalysisStatus status)
        {
            switch (status)
            {
                case AnalysisStatus.Waiting:
                    return MessageResources.NameStatusQueue;
                case AnalysisStatus.Running:
                    return MessageResources.NameStatusRunning;
                case AnalysisStatus.Finished:
                    return MessageResources.NameStatusFinished;
                case AnalysisStatus.Stopped:
                    return MessageResources.NameStatusStopped;
                case AnalysisStatus.Error:
                    return MessageResources.NameStatusError;
            }
            return MessageResources.NameStatusNone;
        }

        private static void SetImageAtJobStatus(TreeNode node, JobStatus status)
        {
            switch (status)
            {
                case JobStatus.QUEUED:
                    if (node.ImageIndex != 1)
                    {
                        node.ImageIndex = 1;
                        node.SelectedImageIndex = 1;
                    }
                    return;
                case JobStatus.RUNNING:
                    if (node.ImageIndex != 2)
                    {
                        node.ImageIndex = 2;
                        node.SelectedImageIndex = 2;
                    }
                    return;
                case JobStatus.FINISHED:
                    if (node.ImageIndex != 3)
                    {
                        node.ImageIndex = 3;
                        node.SelectedImageIndex = 3;
                    }
                    return;
                case JobStatus.STOPPED:
                    if (node.ImageIndex != 4)
                    {
                        node.ImageIndex = 4;
                        node.SelectedImageIndex = 4;
                    }
                    return;
                case JobStatus.ERROR:
                    if (node.ImageIndex != 5)
                    {
                        node.ImageIndex = 5;
                        node.SelectedImageIndex = 5;
                    }
                    return;
            }
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
        }

        private static void SetImageAtJobGroupStatus(TreeNode node, AnalysisStatus status)
        {
            switch (status)
            {
                case AnalysisStatus.Waiting:
                    if (node.ImageIndex != 6)
                    {
                        node.ImageIndex = 6;
                        node.SelectedImageIndex = 6;
                    }
                    return;
                case AnalysisStatus.Running:
                    if (node.ImageIndex != 7)
                    {
                        node.ImageIndex = 7;
                        node.SelectedImageIndex = 7;
                    }
                    return;
                case AnalysisStatus.Finished:
                    if (node.ImageIndex != 8)
                    {
                        node.ImageIndex = 8;
                        node.SelectedImageIndex = 8;
                    }
                    return;
                case AnalysisStatus.Stopped:
                    if (node.ImageIndex != 9)
                    {
                        node.ImageIndex = 9;
                        node.SelectedImageIndex = 9;
                    }
                    return;
                case AnalysisStatus.Error:
                    if (node.ImageIndex != 10)
                    {
                        node.ImageIndex = 10;
                        node.SelectedImageIndex = 10;
                    }
                    return;
            }
            node.ImageIndex = -1;
            node.SelectedImageIndex = -1;
        }
    }

    /// <summary>
    /// TreeNode for JobGroup.
    /// </summary>
    public class JobGroupTreeNode : TreeNode
    {
        #region Fields
        /// <summary>
        /// group name.
        /// </summary>
        private string m_groupName;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="text">group name.</param>
        public JobGroupTreeNode(string text)
            : base(text)
        {
            m_groupName = text;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set group name.
        /// </summary>
        public string GroupName
        {
            get { return this.m_groupName; }
            set { this.m_groupName = value; }
        }
        #endregion
    }

    /// <summary>
    /// TreeNode for Job.
    /// </summary>
    public class JobTreeNode : TreeNode
    {
        #region Fields
        /// <summary>
        /// group name.
        /// </summary>
        private string m_groupName;
        /// <summary>
        /// job id.
        /// </summary>
        private string m_id;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="groupName">group name.</param>
        /// <param name="text">job id.</param>
        public JobTreeNode(string groupName, string text)
            : base(text)
        {
            m_groupName = groupName;
            m_id = text;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set group name.
        /// </summary>
        public string GroupName
        {
            get { return this.m_groupName; }
            set { this.m_groupName = value; }
        }

        /// <summary>
        /// get / set job id.
        /// </summary>
        public string ID
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }
        #endregion
    }
}