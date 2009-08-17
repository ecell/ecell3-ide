//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.Console
{
    /// <summary>
    /// User Control for MessageWindow.
    /// </summary>
    public partial class ConsoleControl : EcellDockContent
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleControl()
        {
            InitializeComponent();
            this.TabText = this.Text;
            simText.ContextMenu = new ContextMenu();
        }

        /// <summary>
        /// Append the text data to console.
        /// </summary>
        /// <param name="text">the text data</param>
        public void AppendText(string text)
        {
            simText.Select(simText.TextLength, 0);
            simText.AppendText(text);
        }

        /// <summary>
        /// Press key on Console.
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="keyData">Key data.</param>
        /// <returns>the flag whether this event is handled.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((int)keyData == (int)Keys.Control + (int)Keys.C)
            {
                Clipboard.SetText(simText.SelectedText);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
