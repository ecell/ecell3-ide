using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    public partial class PluginVersionListWindow : Form
    {
        public PluginVersionListWindow()
        {
            InitializeComponent();
        }

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

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}