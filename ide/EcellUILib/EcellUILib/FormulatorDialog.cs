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

using Ecell.UI.Components;

namespace Ecell.IDE
{
    public partial class FormulatorDialog : Form
    {
        private FormulatorControl m_cnt;
        private String m_result;

        public String Result
        {
            get { return this.m_result; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormulatorDialog()
        {
            InitializeComponent();

            m_cnt = new FormulatorControl();
            tableLayoutPanel.Controls.Add(m_cnt, 0, 0);
            m_cnt.Dock = DockStyle.Fill;
        }

        public void AddReserveString(List<string> list)
        {
            m_cnt.AddReserveString(list);
        }

        public void ImportFormulate(string formu)
        {
            m_cnt.ImportFormulate(formu);
        }

        public void SetExpression(bool isExpression)
        {
            m_cnt.IsExpression = isExpression;
        }

        private void FormulatorDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;

            m_result = m_cnt.ExportFormulate();
        }

    }
}