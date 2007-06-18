//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.ObjectList
{
    public partial class SearchInstance : Form
    {
        #region Fields
        /// <summary>
        /// the plugin control this windows form.
        /// </summary>
        private ObjectList m_plugin;
        #endregion

        /// <summary>
        /// Constructor for SearchInstance.
        /// </summary>
        public SearchInstance()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set plugin that control this window.
        /// </summary>
        /// <param name="p">ObjectList</param>
        public void SetPlugin(ObjectList p)
        {
            this.m_plugin = p;
        }

        #region Event
        /// <summary>
        /// The action of clicking search button on Search Window.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void SearchButtonClick(object sender, EventArgs e)
        {
            string text = this.searchText.Text;
            m_plugin.SearchObjectFromDgv(text);
        }

        /// <summary>
        /// The action of clicking close button on Search Window.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void SearchCloseButtonClick(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// The action of pressing the key of return on Search Window.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">KeyPressEventArgs</param>
        public void SearchTextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchButtonClick(sender, new EventArgs());
            }
        }
        #endregion

        private void SearchInstanceShown(object sender, EventArgs e)
        {
            this.searchText.Focus();
        }
    }
}