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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ecell.Logging;
using Ecell.Objects;
using System.Xml;

namespace Ecell.Plugin
{
    /// <summary>
    /// Interface of plugin.
    /// </summary>
    public interface IEcellPlugin
    {
        /// <summary>
        /// get / set the application environment.
        /// </summary>
        ApplicationEnvironment Environment { get; set; }

        /// <summary>
        /// Get the name of this plugin.
        /// PluginName MUST BE unique.
        /// </summary>
        /// <returns>""</returns>
        string GetPluginName();

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        string GetVersionString();

        /// <summary>
        ///  Get the property settings.
        /// </summary>
        List<IPropertyItem> GetPropertySettings();

        /// <summary>
        /// Get the status of plugin.
        /// </summary>
        /// <returns></returns>
        XmlNode GetPluginStatus();

        /// <summary>
        /// Set the status of plugin.
        /// </summary>
        /// <param name="nstatus"></param>
        void SetPluginStatus(XmlNode nstatus);

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        void Initialize();

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        void ChangeStatus(ProjectStatus type); // 0:initial 1:load 2:run 3:suspend

        /// <summary>
        /// Get the public deleagte function of plugin.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Delegate> GetPublicDelegate();
    }
}
