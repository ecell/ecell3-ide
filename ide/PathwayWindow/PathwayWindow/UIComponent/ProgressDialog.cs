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
// written by by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.PathwayWindow.UIComponent
{
    /// <summary>
    /// Form for progress bar
    /// </summary>
    public partial class ProgressDialog : PathwayDialog
    {
        /// <summary>
        /// Accessor for ProgressBar.
        /// </summary>
        public ProgressBar Bar
        {
            get { return this.progressBar; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressDialog(int min, int max)
        {
            InitializeComponent();
            Bar.Minimum = min;
            Bar.Maximum = max;
            Bar.Value = min;
            Bar.Step = 1;
        }
    }
}