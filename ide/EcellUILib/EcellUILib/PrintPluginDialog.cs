using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EcellLib.Plugin;

namespace EcellLib
{
    /// <summary>
    /// dialog to select print plugin .
    /// </summary>
    public partial class PrintPluginDialog : Form
    {
        /// <summary>
        /// DataManager
        /// </summary>
        private PluginManager m_pManager;

        /// <summary>
        /// constructor of PrintPluginDialog.
        /// </summary>
        public PrintPluginDialog(PluginManager pManager)
        {
            m_pManager = pManager;

            foreach (IEcellPlugin plugin in m_pManager.Plugins)
            {
                List<string> names = plugin.GetEnablePrintNames();
                foreach (string name in names)
                {
                    listBox1.Items.Add(name);
                }
            }

            InitializeComponent();
        }

        /// <summary>
        /// ok click event after select the print plugin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// cancel click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}