using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SessionManager;

namespace EcellLib.MainWindow
{
    /// <summary>
    /// Window class to display the status of jobs.
    /// </summary>
    public partial class DistributedEnvWindow : Form
    {
        SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();
        /// <summary>
        /// Constructor.
        /// </summary>
        public DistributedEnvWindow()
        {
            InitializeComponent();
            JobGridView.CellDoubleClick += new DataGridViewCellEventHandler(JobGridViewDoubleClick);
        }

        void JobGridViewDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();

            if (e.RowIndex < 0) return;
            int jobid = Convert.ToInt32(JobGridView[0, e.RowIndex].Value);
            if (!manager.SessionList.ContainsKey(jobid)) return;
            string data = manager.SessionList[jobid].StdErr;

            MessageBox.Show(data, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void WinShown(object sender, EventArgs e)
        {
            SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();

            foreach (SessionProxy s in manager.SessionList.Values)
            {
                JobGridView.Rows.Add(new object[] { s.JobID, s.Status, s.Machine, s.ScriptFile, s.Argument });
            }
        }

        private void DEWClearButtonClick(object sender, EventArgs e)
        {
            SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();

            manager.ClearJob(0);
            JobGridView.Rows.Clear();
        }

        private void DEWDeleteButtonClick(object sender, EventArgs e)
        {
            SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();

            foreach (DataGridViewRow r in JobGridView.SelectedRows)
            {
                try
                {
                    int jobid = Convert.ToInt32(r.Cells[0].Value);
                    manager.ClearJob(jobid);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            JobGridView.Rows.Clear();
            foreach (SessionProxy s in manager.SessionList.Values)
            {
                JobGridView.Rows.Add(new object[] { s.JobID, s.Status, s.Machine, s.ScriptFile, s.Argument });
            }
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DEWUpdateButton_Click(object sender, EventArgs e)
        {
            SessionManager.SessionManager manager = SessionManager.SessionManager.GetManager();

            JobGridView.Rows.Clear();
            foreach (SessionProxy s in manager.SessionList.Values)
            {
                JobGridView.Rows.Add(new object[] { s.JobID, s.Status, s.Machine, s.ScriptFile, s.Argument });
            }
        }
    }
}