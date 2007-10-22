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
        private SessionManager.SessionManager m_manager = SessionManager.SessionManager.GetManager();
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
            if (e.RowIndex < 0) return;
            int jobid = Convert.ToInt32(JobGridView[0, e.RowIndex].Value);
            if (!m_manager.SessionList.ContainsKey(jobid)) return;
            string data = m_manager.SessionList[jobid].StdErr;

            MessageBox.Show(data, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WinShown(object sender, EventArgs e)
        {
            foreach (SessionProxy s in m_manager.SessionList.Values)
            {
                JobGridView.Rows.Add(new object[] { s.JobID, s.Status, s.ScriptFile, s.Argument });
            }
        }
    }
}