//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.Plugin;
using WeifenLuo.WinFormsUI.Docking;

namespace Ecell.IDE
{
    /// <summary>
    /// dialog to select print plugin .
    /// </summary>
    public partial class PrintPluginDialog : Form
    {
        #region InnerClass
        /// <summary>
        /// The selected entry class.
        /// </summary>
        public class Entry
        {
            /// <summary>
            /// get the plugin.
            /// </summary>
            public IRasterizable Plugin
            {
                get { return m_plugin; }
            }

            /// <summary>
            /// get the portion.
            /// </summary>
            public string Portion
            {
                get { return m_portion; }
            }

            /// <summary>
            /// constructors
            /// </summary>
            /// <param name="plugin">Plugin object.</param>
            /// <param name="portion">portion of plugin.</param>
            internal Entry(IRasterizable plugin, string portion)
            {
                m_plugin = plugin;
                m_portion = portion;
            }

            /// <summary>
            /// convert to string.
            /// </summary>
            /// <returns>object string.</returns>
            public override string ToString()
            {
                return string.Format("{0}", m_portion);
            }

            #region Fields
            /// <summary>
            /// Plugin object.
            /// </summary>
            private IRasterizable m_plugin;
            /// <summary>
            /// Portion of plugin.
            /// </summary>
            private string m_portion;
            #endregion
        }
        #endregion

        #region Fields
        /// <summary>
        /// DataManager.
        /// </summary>
        private PluginManager m_pManager;
        /// <summary>
        /// Selected item.
        /// </summary>
        private Entry m_selectedItem;
        #endregion

        #region Accessors
        /// <summary>
        /// Get selected Item,
        /// </summary>
        public Entry SelectedItem
        {
            get { return m_selectedItem; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// constructor of PrintPluginDialog.
        /// </summary>
        public PrintPluginDialog(PluginManager pManager)
        {
            m_pManager = pManager;
            m_selectedItem = null;
            InitializeComponent();

            foreach (IRasterizable plugin in m_pManager.Rasterizables)
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
        #endregion

        #region Events
        /// <summary>
        /// ok click event after select the print plugin.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem == null)
            {
                Ecell.Util.ShowNoticeDialog(MessageResources.ErrNoSelectTarget);
                return;
            }
            this.DialogResult = DialogResult.OK;
            m_selectedItem = (Entry)this.listBox1.SelectedItem;
        }

        /// <summary>
        /// cancel click event.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Event when ListBox is select changed.
        /// </summary>
        /// <param name="sender">ListBox</param>
        /// <param name="e">EventArgs</param>
        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                ActivateWindow(listBox1.SelectedItem.ToString());
        }
        #endregion

        /// <summary>
        /// Active the selected window.
        /// </summary>
        /// <param name="name">the window name.</param>
        private void ActivateWindow(string name)
        {
            foreach (DockContent content in m_pManager.DockPanel.Contents)
            {
                if (content.Text == name)
                    content.Activate();
            }
        }
    }
}