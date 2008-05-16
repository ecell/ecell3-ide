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
// modified by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Type of component.
    /// </summary>
    public enum ComponentType
    {
        /// <summary>
        /// type of system
        /// </summary>
        System,
        /// <summary>
        /// typs of variable
        /// </summary>
        Variable,
        /// <summary>
        /// type of process
        /// </summary>
        Process,
        /// <summary>
        /// type of text
        /// </summary>
        Text
    };

    /// <summary>
    /// Type of change to one reference of VariableReference
    /// </summary>
    public enum RefChangeType
    {
        /// <summary>
        /// Change VariableReference to single direction
        /// </summary>
        SingleDir,
        /// <summary>
        /// Change VariableReference to dual direction
        /// </summary>
        BiDir,
        /// <summary>
        /// Delete VariableReference
        /// </summary>
        Delete
    };

    /// <summary>
    /// Direction of scrolling the canvas.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Vertical direction
        /// </summary>
        Vertical,
        /// <summary>
        /// Horizontal direction
        /// </summary>
        Horizontal
    };

    /// <summary>
    /// Mode
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Select objects
        /// </summary>
        Select,
        /// <summary>
        /// Pan canvas
        /// </summary>
        Pan,
        /// <summary>
        /// Zoom canvas
        /// </summary>
        Zoom,
        /// <summary>
        /// Create reaction
        /// </summary>
        CreateOneWayReaction,
        /// <summary>
        /// Create mutual(Interactive) reaction
        /// </summary>
        CreateMutualReaction,
        /// <summary>
        /// Create constant
        /// </summary>
        CreateConstant,
        /// <summary>
        /// Create system
        /// </summary>
        CreateSystem,
        /// <summary>
        /// Create node
        /// </summary>
        CreateNode,
        /// <summary>
        /// Create text
        /// </summary>
        CreateText
    };
}
