//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
using System.IO;
using System.Reflection;
using System.Text;

using Python.Runtime;

using Ecell;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.ScriptWindow
{
    /// <summary>
    /// Plugin class to display the script window and execute the script.
    /// </summary>
    public class ScriptWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// Control object,
        /// </summary>
        private ScriptCommandWindow m_control;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public ScriptWindow()
        {
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get the window form for ScriptWindow plugin.
        /// </summary>
        /// <returns>Windows form</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            List<EcellDockContent> res = new List<EcellDockContent>();
            m_control = new ScriptCommandWindow();
            res.Add(m_control);

            return res;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"ScriptWindow"</returns>
        public override string GetPluginName()
        {
            return "ScriptWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

    }
}
