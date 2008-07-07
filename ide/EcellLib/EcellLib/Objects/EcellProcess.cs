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
    /// Object class for Process.
    /// </summary>
    public class EcellProcess : EcellObject
    {
        #region Constants
        /// <summary>
        /// VariableReferenceList. The reserved name.
        /// </summary>
        public const string VARIABLEREFERENCELIST = Constants.xpathVRL;
        /// <summary>
        /// Activity. The reserved name.
        /// </summary>
        public const string ACTIVITY = "Activity";
        /// <summary>
        /// Expression. The reserved name.
        /// </summary>
        public const string EXPRESSION = "Expression";
        /// <summary>
        /// IsContinuous. The reserved name.
        /// </summary>
        public const string ISCONTINUOUS = "IsContinuous";
        /// <summary>
        /// Name. The reserved name.
        /// </summary>
        public const string NAME = "Name";
        /// <summary>
        /// Priority. The reserved name.
        /// </summary>
        public const string PRIORITY = "Priority";
        /// <summary>
        /// StepperID. The reserved name.
        /// </summary>
        public const string STEPPERID = "StepperID";
        #endregion

        #region Fields
        #endregion

        #region Constractors
        /// <summary>
        /// Constructor.
        /// </summary>
        public EcellProcess()
        {
        }
        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="l_modelID">model ID.</param>
        /// <param name="l_key">key.</param>
        /// <param name="l_type">type(="Variable").</param>
        /// <param name="l_class">class name.</param>
        /// <param name="l_data">properties.</param>
        public EcellProcess(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
            : base(l_modelID, l_key, l_type, l_class, l_data)
        {
            
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get /set the activity.
        /// </summary>
        public double Activity
        {
            get
            {
                if (IsEcellValueExists(ACTIVITY))
                    return GetEcellValue(ACTIVITY).CastToDouble();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists(ACTIVITY))
                    GetEcellValue(ACTIVITY).Value = value;
                else
                    AddEcellValue(ACTIVITY, new EcellValue(value));
            }
        }

        /// <summary>
        /// get /set the expression of process.
        /// </summary>
        public string Expression
        {
            get
            {
                if (IsEcellValueExists(EXPRESSION))
                    return GetEcellValue(EXPRESSION).ToString();
                else
                    return null;
            }
            set
            {
                if (IsEcellValueExists(EXPRESSION))
                    GetEcellValue(EXPRESSION).Value = value;
                else
                    AddEcellValue(EXPRESSION, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set whether this property is continious.
        /// </summary>
        public int IsContinuous
        {
            get
            {
                if (IsEcellValueExists(ISCONTINUOUS))
                    return GetEcellValue(ISCONTINUOUS).CastToInt();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists(ISCONTINUOUS))
                    GetEcellValue(ISCONTINUOUS).Value = value;
                else
                    AddEcellValue(ISCONTINUOUS, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set priority.
        /// </summary>
        public int Priority
        {
            get
            {
                if (IsEcellValueExists(PRIORITY))
                    return GetEcellValue(PRIORITY).CastToInt();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists(PRIORITY))
                    GetEcellValue(PRIORITY).Value = value;
                else
                    AddEcellValue(PRIORITY, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set stepperID.
        /// </summary>
        public string StepperID
        {
            get
            {
                if (IsEcellValueExists(STEPPERID))
                    return GetEcellValue(STEPPERID).ToString();
                else
                    return null;
            }
            set
            {
                if (IsEcellValueExists(STEPPERID))
                    GetEcellValue(STEPPERID).Value = value;
                else
                    AddEcellValue(STEPPERID, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set the property of VariableReferenceList.
        /// </summary>
        public List<EcellReference> ReferenceList
        {
            get { return EcellReference.ConvertFromVarRefList(this.GetEcellValue(VARIABLEREFERENCELIST)); }
            set
            {
                EcellValue varRef = EcellReference.ConvertToVarRefList(value);
                this.GetEcellData(VARIABLEREFERENCELIST).Value = varRef;
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}
