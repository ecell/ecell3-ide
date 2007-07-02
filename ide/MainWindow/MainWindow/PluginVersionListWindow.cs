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
    /// Dialog to display the version list of plugin.
    /// </summary>
    public partial class PluginVersionListWindow : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PluginVersionListWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event process when this dialog is shown.
        /// </summary>
        /// <param name="sender">this dialog.</param>
        /// <param name="e">EventArgs.</param>
        private void WindowShown(object sender, EventArgs e)
        {
            PluginManager pManager = PluginManager.GetPluginManager();

            Dictionary<String, String> list = pManager.GetPluginVersionList();
            List<String> tmpList = new List<string>();
            foreach (String n in list.Keys)
            {
                tmpList.Add(n);
            }

            tmpList.Sort();
            foreach (String n in tmpList)
            {
                versionListView.Rows.Add(new object[] { n, list[n] });
            }
        }

        /// <summary>
        /// Event process when user click close button.
        /// </summary>
        /// <param name="sender">close button.</param>
        /// <param name="e">EventArgs.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}