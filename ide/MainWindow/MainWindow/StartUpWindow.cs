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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    class StartUpWindow : EcellDockContent
    {
        private WebBrowser EcellBrowser;
        private static string URL = "http://chaperone.e-cell.org/trac/ecell-ide";

        public StartUpWindow()
        {
            InitializeComponent();
            this.EcellBrowser.Navigate(URL);
        }

        private void InitializeComponent()
        {
            this.EcellBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.EcellBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EcellBrowser.Location = new System.Drawing.Point(0, 0);
            this.EcellBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.EcellBrowser.Name = "webBrowser1";
            this.EcellBrowser.Size = new System.Drawing.Size(688, 501);
            this.EcellBrowser.TabIndex = 0;
            this.EcellBrowser.Url = new System.Uri(URL, System.UriKind.Absolute);
            // 
            // StartUpWindow
            // 
            this.ClientSize = new System.Drawing.Size(688, 501);
            this.Controls.Add(this.EcellBrowser);
            this.Name = "StartUpWindow";
            this.ResumeLayout(false);

        }
    }
}
