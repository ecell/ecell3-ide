﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Objects
{

    /// <summary>
    /// Object class for Variable.
    /// </summary>
    public class EcellVariable : EcellObject
    {
        #region Fields
        List<EcellLayout> m_aliases = new List<EcellLayout>();
        #endregion

        #region Constractors
        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="key">key.</param>
        /// <param name="type">type(="Variable").</param>
        /// <param name="classname">class name.</param>
        /// <param name="data">properties.</param>
        public EcellVariable(string modelID, string key,
            string type, string classname, List<EcellData> data)
            : base(modelID, key, VARIABLE, VARIABLE, data)
        {
        }
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public List<EcellLayout> Aliases
        {
            get
            {
                return this.m_aliases;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override EcellObject Clone()
        {
            EcellVariable newVar = (EcellVariable)base.Clone();
            foreach (EcellLayout layout in m_aliases)
            {
                newVar.Aliases.Add(layout.Clone());
            }
            return newVar;
        }
        #endregion
    }
}
