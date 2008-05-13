using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib
{
    /// <summary>
    /// dialog to select print plugin .
    /// </summary>
    public partial class PrintPluginDialog : Form
    {
        /// <summary>
        /// constructor of PrintPluginDialog.
        /// </summary>
        public PrintPluginDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ok click event after select the print plugin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            PluginManager manager = PluginManager.GetPluginManager();
            if (this.listBox1.SelectedItem != null)
            {
                manager.Print(this.listBox1.SelectedItem.ToString());
                this.Close();
            }
            else
            {
                Util.__showWarningDialog("Please select the plugin to print.");
            }
        }

        /// <summary>
        /// cancel click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}