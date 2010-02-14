//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
        public const string SIZE = "SIZE";
        /// <summary>
        /// Default size of system.
        /// </summary>
        public const double DefaultSize = 1.0d;
        #endregion

        #region Fields
        /// <summary>
        /// List of child systems;
        /// </summary>
        List<EcellSystem> m_childSystems = new List<EcellSystem>();
        #endregion

        #region Constractors
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
            : base(modelID, key, SYSTEM, SYSTEM, data)
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
                double size = DefaultSize;
                // Get size object.
                foreach (EcellObject child in m_children)
                {
                    if (!child.LocalID.Equals(SIZE))
                        continue;
                    return (double)child.GetEcellValue(Constants.xpathValue);
                }
                // Get size parameter
                if (IsEcellValueExists(Constants.xpathSize))
                    size = (double)GetEcellValue(Constants.xpathSize);

                // return 1 when Size doesn't exist.
                return size;
            }
            set
            {
                // Set size parameter
                SetEcellValue(Constants.xpathSize, new EcellValue(value));

                // Set size object.
                foreach (EcellObject child in m_children)
                {
                    if (!child.LocalID.Equals(SIZE))
                        continue;

                    child.SetEcellValue(Constants.xpathValue, new EcellValue(value));
                    return;
                }

                // Create Size object if "Size" does not exist.
                string key = m_key + Constants.delimiterColon + SIZE;
                EcellObject size = EcellObject.CreateObject(m_modelID, key, EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
                size.SetEcellValue(Constants.xpathValue, new EcellValue(value));
                m_children.Add(size);
            }
        }

        /// <summary>
        /// get / set Stepper ID.
        /// </summary>
        public string StepperID
        {
            get
            {
                if (IsEcellValueExists(Constants.xpathStepperID))
                    return GetEcellValue(Constants.xpathStepperID).ToString();
                else
                    return null;
            }
            set
            {
                SetEcellValue(Constants.xpathStepperID, new EcellValue(value));
            }
        }
        #endregion

    }
}
