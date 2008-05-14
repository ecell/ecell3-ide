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

namespace EcellLib.Objects
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
        /// <param name="l_modelID">model ID.</param>
        /// <param name="l_key">key.</param>
        /// <param name="l_type">type(="System").</param>
        /// <param name="l_class">class name.</param>
        /// <param name="l_data">properties.</param>
        public EcellSystem(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.ModelID = l_modelID;
            this.Key = l_key;
            this.Type = l_type;
            this.Classname = l_class;
            this.SetEcellDatas(l_data);
            this.Children = new List<EcellObject>();
        }
        #endregion

        #region Accessors

        /// <summary>
        /// get / set size;
        /// </summary>
        public double Size
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

        /// <summary>
        /// get/set system name.
        /// </summary>
        public new string Text
        {
            get
            {
                string text = base.Text;
                if (Size != 0.1d)
                    text += " (SIZE:" + GetEcellValue(SIZE).ToString() + ")";
                return text;
            }
        }
        #endregion
    }
}
