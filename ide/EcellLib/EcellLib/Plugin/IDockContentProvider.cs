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
// written by Moriyoshi Koizumi <mozo@sfc.keio.ac.jp>
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Plugin
{
    /// <summary>
    /// Plugin interface provide the dock content.
    /// </summary>
    public interface IDockContentProvider
    {
        /// <summary>
        /// Get the window forms of each plugin.
        /// DockContent is a docking window class of WeifenLuo.WinFormsUI plugin.
        /// </summary>
        /// <returns>UserControl.</returns>
        //List<UserControl> GetWindowsForms();
        IEnumerable<EcellDockContent> GetWindowsForms();
    }
}
