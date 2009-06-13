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

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Objects;

namespace Ecell.Plugin
{
    /// <summary>
    /// Type of layout.
    /// </summary>
    public enum LayoutType
    {
        /// <summary>
        /// Whole objects on canvas will be layouted
        /// </summary>
        Whole,
        /// <summary>
        /// Only selected nodes will be layouted.
        /// </summary>
        Selected  // Only selected nodes will be layouted.
    };

    /// <summary>
    /// Interface for layout algorithm.
    /// </summary>
    public interface ILayoutAlgorithm
    {
        /// <summary>
        /// Execute layout
        /// </summary>
        /// <param name="subCommandNum">
        /// An index of sub command which was clicked on subMenu.
        /// Sub command which is in subCommandNum position in the list returned by GetSubCommands() [0 origin]
        /// If layout name itself was clicked, subCommandNum = -1.
        /// </param>
        /// <param name="layoutSystem">Whether systems should be layouted or not</param>
        /// <param name="systemList">Systems</param>
        /// <param name="nodeList">Nodes (Variables, Processes)</param>
        /// <returns>Whether layout is completed or aborted</returns>       
        bool DoLayout(int subCommandNum,
                      bool layoutSystem,
                      List<EcellObject> systemList,
                      List<EcellObject> nodeList);

        /// <summary>
        /// Which LayoutType does this algorithm serve.
        /// </summary>
        /// <returns>layout type of this algorithm</returns>
        LayoutType GetLayoutType();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetLayoutName();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ILayoutPanel GetPanel();

    }
}
