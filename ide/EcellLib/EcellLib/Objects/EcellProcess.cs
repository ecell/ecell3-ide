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
        /// MolarActivity.
        /// </summary>
        public const string MOLARACTIVITY = "MolarActivity";
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
        /// <summary>
        /// 
        /// </summary>
        public const string MASSCALCULATIONPROCESS = "MassCalculationProcess";
        #endregion

        #region Fields
        #endregion

        #region Constractors
        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="modelID">model ID.</param>
        /// <param name="key">key.</param>
        /// <param name="type">type(="Process").</param>
        /// <param name="classname">class name.</param>
        /// <param name="data">properties.</param>
        public EcellProcess(string modelID, string key,
            string type, string classname, List<EcellData> data)
            : base(modelID, key, PROCESS, classname, data)
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
                    return (double)GetEcellValue(ACTIVITY);
                else
                    return 0;
            }
            set
            {
                SetEcellValue(ACTIVITY, new EcellValue(value));
            }
        }

        /// <summary>
        /// get /set the activity.
        /// </summary>
        public double MolarActivity
        {
            get
            {
                if (IsEcellValueExists(MOLARACTIVITY))
                    return (double)GetEcellValue(MOLARACTIVITY);
                else
                    return 0;
            }
            set
            {
                SetEcellValue(MOLARACTIVITY, new EcellValue(value));
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
                SetEcellValue(EXPRESSION, new EcellValue(value));
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
                    return (int)GetEcellValue(ISCONTINUOUS);
                else
                    return 0;
            }
            set
            {
                SetEcellValue(ISCONTINUOUS, new EcellValue(value));
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
                    return (int)GetEcellValue(PRIORITY);
                else
                    return 0;
            }
            set
            {
                SetEcellValue(PRIORITY, new EcellValue(value));
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
                SetEcellValue(STEPPERID, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set the property of VariableReferenceList.
        /// </summary>
        public List<EcellReference> ReferenceList
        {
            get 
            {
                EcellValue varRef = this.GetEcellValue(VARIABLEREFERENCELIST);
                return EcellReference.ConvertFromEcellValue(varRef);
            }
            set
            {
                EcellValue varRef = EcellReference.ConvertToEcellValue(value);
                SetEcellValue(VARIABLEREFERENCELIST, varRef);
            }
        }
        #endregion

        #region Methods
        #endregion
    }
}
