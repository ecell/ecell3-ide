using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    /// <summary>
    /// Form to setup the distributed environment.
    /// </summary>
    public partial class DistributedEnvSetupWindow : Form
    {
        private Dictionary<string, object> m_propDict = new Dictionary<string, object>();
        private SessionManager.SessionManager m_manager = null;
        /// <summary>
        /// ResourceManager for MainWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResMain));

        /// <summary>
        /// Constructor.
        /// </summary>
        public DistributedEnvSetupWindow()
        {
            InitializeComponent();
            m_manager = SessionManager.SessionManager.GetManager();
        }

        /// <summary>
        /// The event to show the window.
        /// </summary>
        /// <param name="sender">this form.</param>
        /// <param name="e">EventArgs.</param>
        public void WindowShown(object sender, EventArgs e)
        {
            List<string> list = m_manager.GetEnvironmentList();
            string envName = m_manager.GetEnvironment();
            foreach (string env in list)
            {
                DEEnvComboBox.Items.Add(env);
            }
            DEEnvComboBox.SelectedText = envName;
            DEConcTextBox.Text = Convert.ToString(m_manager.GetDefaultConcurrency());

            m_propDict = m_manager.GetDefaultEnvironmentProperty(envName);
            if (m_propDict != null)
            {
                foreach (string propName in m_propDict.Keys)
                {
                    DEOptionGridView.Rows.Add(new object[] { propName, m_propDict[propName].ToString() });
                }
            }
            DEEnvComboBox.SelectedIndexChanged += new EventHandler(DEEnvComboBox_SelectedIndexChanged);
        }

        private void DEEnvComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string envName = DEEnvComboBox.Text;

            int dConv = m_manager.GetDefaultConcurrency(envName);
            DEConcTextBox.Text = Convert.ToString(dConv);
            DEOptionGridView.Rows.Clear();
            m_propDict = m_manager.GetDefaultEnvironmentProperty(envName);
            if (m_propDict != null)
            {
                foreach (string propName in m_propDict.Keys)
                {
                    DEOptionGridView.Rows.Add(new object[] { propName, m_propDict[propName].ToString() });
                }
            }
        }

        private void DECloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DEApplyButton_Click(object sender, EventArgs e)
        {
            try
            {
                int conc = Convert.ToInt32(DEConcTextBox.Text);
                if (conc <= 0)
                {
                    string errmes = m_resources.GetString("ErrConcInvalid");
                    MessageBox.Show(errmes, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (DEWorkDirTextBox.Text == null ||
                    DEWorkDirTextBox.Text == "")
                {
                    string errmes = m_resources.GetString("ErrNoWorkDir");
                    MessageBox.Show(errmes, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string envName = DEEnvComboBox.Text;
                m_manager.SetEnvironment(envName);
                m_manager.TmpRootDir = DEWorkDirTextBox.Text;
                m_manager.Concurrency = conc;
                for (int i = 1; i < DEOptionGridView.Rows.Count; i++)
                {
                    string propName = DEOptionGridView[0, i].Value.ToString();
                    if (m_propDict.ContainsKey(propName))
                    {
                        m_propDict[propName] = DEOptionGridView[1, i].Value.ToString();
                    }
                }
                m_manager.SetEnvironmentProperty(m_propDict);
            }
            catch (Exception ex)
            {
                ex.ToString();
                string errmes = m_resources.GetString("ErrUpdateDistEnv");
                MessageBox.Show(errmes, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Close();
        }

        private void DESearchDir_Click(object sender, EventArgs e)
        {
            if (m_folderSelectDialog.ShowDialog() == DialogResult.OK)
            {
                DEWorkDirTextBox.Text = m_folderSelectDialog.SelectedPath;
            }
            else
            {
                // nothing.
            }
        }
    }
}