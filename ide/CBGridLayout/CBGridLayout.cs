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
using Ecell.Plugin;
using System.Reflection;

namespace CBGridLayout
{
    /// <summary>
    /// 
    /// </summary>
    public class CBGridLayout : LayoutBase
    {
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public CBGridLayout()
        {
            m_panel = new CBGridLayoutPanel(this);
        }
        #endregion

        #region Inherited from ILayout
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetPluginName()
        {
            return "CBGridLayout";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subCommandNum"></param>
        /// <param name="layoutSystem"></param>
        /// <param name="systemList"></param>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        public override bool DoLayout(int subCommandNum, bool layoutSystem, List<Ecell.Objects.EcellObject> systemList, List<Ecell.Objects.EcellObject> nodeList)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override LayoutType GetLayoutType()
        {
            return LayoutType.Whole;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetLayoutName()
        {
            return "CBGrid";
        }

        #endregion
    }
}
