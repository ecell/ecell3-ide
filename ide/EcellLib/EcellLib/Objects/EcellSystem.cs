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
    /// Object class for System.
    /// </summary>
    public class EcellSystem : EcellObject
    {

        #region Constant
        /// <summary>
        /// Size name. The reserved name.
        /// </summary>
        public const string SIZE = "Size";
        #endregion

        #region Fields
        /// <summary>
        /// List of child systems;
        /// </summary>
        List<EcellSystem> m_childSystems = new List<EcellSystem>();
        #endregion

        #region Constractors
        /// <summary>
        /// Constructor.
        /// </summary>
        public EcellSystem()
        {
        }

        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="key">key.</param>
        /// <param name="type">type(="System").</param>
        /// <param name="classname">class name.</param>
        /// <param name="data">properties.</param>
        public EcellSystem(string modelID, string key,
            string type, string classname, List<EcellData> data)
            : base(modelID, key, type, classname, data)
        {
            m_children = new List<EcellObject>();
        }
        #endregion

        #region Accessors

        /// <summary>
        /// get / set size;
        /// </summary>
        public double SizeInVolume
        {
            get
            {
                if (IsEcellValueExists(SIZE))
                    return GetEcellValue(SIZE).CastToDouble();
                else
                    return 0.1d;
            }
            set
            {
                if (IsEcellValueExists(SIZE))
                    GetEcellValue(SIZE).Value = value;
                else
                    AddEcellValue(SIZE, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set Stepper ID.
        /// </summary>
        public string StepperID
        {
            get
            {
                if (IsEcellValueExists("StepperID"))
                    return GetEcellValue("StepperID").ToString();
                else
                    return null;
            }
            set
            {
                if (IsEcellValueExists("StepperID"))
                    GetEcellValue("StepperID").Value = value;
                else
                    AddEcellValue("StepperID", new EcellValue(value));
            }
        }
        #endregion
    }
}
