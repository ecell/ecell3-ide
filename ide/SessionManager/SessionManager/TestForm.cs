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
            m_manager.SetEnvironment(m_envComboBox.Text);
            m_manager.Concurrency = (int)m_conTextBox.Text;
            m_manager.RegisterJob(m_fileName, "", new List<string>());
            m_manager.Run();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            m_manager.Stop();
        }
    }
}