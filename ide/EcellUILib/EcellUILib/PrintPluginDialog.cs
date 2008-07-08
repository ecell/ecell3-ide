using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.Plugin;

namespace Ecell.IDE
{
    /// <summary>
    /// dialog to select print plugin .
    /// </summary>
    public partial class PrintPluginDialog : Form
    {
        public class Entry
        {
            public IEcellPlugin Plugin
            {
                get { return m_plugin; }
            }

            public string Portion
            {
                get { return m_portion; }
            }

            internal Entry(IEcellPlugin plugin, string portion)
            {
                m_plugin = plugin;
                m_portion = portion;
            }

            public override string ToString()
            {
                return string.Format("{0} ({1})", m_portion, m_plugin.GetPluginName());
            }

            private IEcellPlugin m_plugin;
            private string m_portion;
        }

        public Entry SelectedItem
        {
            get { return m_selectedItem; }
        }

        /// <summary>
        /// constructor of PrintPluginDialog.
        /// </summary>
        public PrintPluginDialog(PluginManager pManager)
        {
            m_pManager = pManager;
            m_selectedItem = null;
            InitializeComponent();

            foreach (IEcellPlugin plugin in m_pManager.Plugins)
            {
                IEnumerable<string> names = plugin.GetEnablePrintNames();
                if (names == null)
                    continue;
                foreach (string name in names)
                {
                    listBox1.Items.Add(new Entry(plugin, name));
                }
            }
        }

        /// <summary>
        /// ok click event after select the print plugin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem == null)
            {
                Ecell.Util.ShowNoticeDialog(MessageResUILib.ErrNoSelectTarget);
                return;
            }
            this.DialogResult = DialogResult.OK;
            m_selectedItem = (Entry)this.listBox1.SelectedItem;
        }

        /// <summary>
        /// cancel click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        /// <summary>
        /// DataManager
        /// </summary>
        private PluginManager m_pManager;

        private Entry m_selectedItem;
    }
}