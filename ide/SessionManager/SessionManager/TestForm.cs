using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SessionManager
{
    public partial class TestForm : Form
    {
        private SessionManager m_manager;
        public TestForm()
        {
            InitializeComponent();
            m_manager = new SessionManager();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String fileName = m_openFileDialog.FileName;
                m_fileName.Text = fileName;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (m_fileName.Text == null || m_fileName.Text.Equals("")) return;

            m_manager.SetEnvironment(m_envComboBox.Text);
            m_manager.Concurrency = Convert.ToInt32(m_conTextBox.Text);
            m_manager.RegisterJob("ipy.exe", m_fileName.Text, new List<string>());
            m_manager.Run();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            m_manager.Stop(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (int jid in m_manager.SessionList.Keys)
            {
                Console.WriteLine(
                    "JobID      : " + jid + "\n" +
                    "ScriptFile : " + m_manager.SessionList[jid].ScriptFile + "\n" +
                    "Argument   : " + m_manager.SessionList[jid].Argument + "\n" +
                    "Status     : " + m_manager.SessionList[jid].Status + "\n");
            }
        }
    }
}