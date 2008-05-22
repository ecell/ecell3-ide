//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using EcellLib;
using EcellLib.Objects;

namespace EcellLib.StaticDebugWindow
{
    /// <summary>
    /// Static debug for model compliance.
    /// </summary>
    class StaticDebugForModel : StaticDebugPlugin
    {
        /// <summary>
        /// Owner of this object
        /// </summary>
        private StaticDebugWindow m_owner;

        /// <summary>
        /// List of error message.
        /// </summary>
        private List<ErrorMessage> m_errorList = new List<ErrorMessage>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public StaticDebugForModel(StaticDebugWindow owner)
        {
            m_owner = owner;
        }

        /// <summary>
        /// Debugger Name.
        /// </summary>
        /// <returns>"Model Compliance."</returns>
        public string GetDebugName()
        {
            return MessageResStDebug.ModelComplianceName;
        }

        /// <summary>
        /// Execute static debug.
        /// </summary>
        /// <param name="l_data">The list of object to be checked.</param>
        /// <returns>The list of error messages.</returns>
        public List<ErrorMessage> Debug(List<EcellObject> l_data)
        {
            m_errorList.Clear();
            foreach (EcellObject obj in l_data)
            {
                ShareStaticDebug(obj);
                if (obj.Children == null) continue;
                foreach (EcellObject cObj in obj.Children)
                {
                    ShareStaticDebug(cObj);
                }
            }
            return m_errorList;
        }

        /// <summary>
        /// Change the used debug function corresponding with type of object.
        /// </summary>
        /// <param name="obj">debug object.</param>
        private void ShareStaticDebug(EcellObject obj)
        {
            if (obj.Type == Constants.xpathSystem)
            {
                DebugForSystem(obj);
            }
            else if (obj.Type == Constants.xpathVariable)
            {
                DebugForVariable(obj);
            }
            else if (obj.Type == Constants.xpathProcess)
            {
                DebugForProcess(obj);
            }
            else if (obj.Type == Constants.xpathStepper)
            {
                DebugForStepper(obj);
            }
            else if (obj.Type == Constants.xpathModel)
            {
                Debug(m_owner.DataManager.GetStepper(null, obj.ModelID));
            }
        }

        #region ObjectDebug
        /// <summary>
        /// Static debug for System.
        /// This check invalid ID and existed value.
        /// </summary>
        /// <param name="obj">The system object to be checked.</param>
        private void DebugForSystem(EcellObject obj)
        {
            IsInvalidComponentId(obj);
            foreach (EcellData d in obj.Value)
            {
                if (d.Name == Constants.xpathStepperID)
                {
                    IsExistStepperID(obj.ModelID, obj.Type, d);
                }
            }
        }

        /// <summary>
        /// Static debug for Process.
        /// </summary>
        /// <param name="obj">The process object to be checked.</param>
        private void DebugForProcess(EcellObject obj)
        {
            IsInvalidComponentId(obj);
            foreach (EcellData d in obj.Value)
            {
                if (d.Name == Constants.xpathStepperID)
                {
                    IsExistStepperID(obj.ModelID, obj.Type, d);
                }
                if (d.Name == Constants.xpathExpression ||
                    d.Name == Constants.xpathFireMethod)
                {
                    CheckBrackets(obj.ModelID, obj.Type, d);
                }
            }
        }

        /// <summary>
        /// Static debug for Variable.
        /// </summary>
        /// <param name="obj">The variable object to be checked.</param>
        private void DebugForVariable(EcellObject obj)
        {
            IsInvalidComponentId(obj);
            foreach (EcellData d in obj.Value)
            {
                if (d.Name == Constants.xpathMolarConc ||
                    d.Name == Constants.xpathNumberConc)
                {
                    IsPositiveNumberWithZero(obj.ModelID, obj.Type, d);
                }
                if (d.Name == Constants.xpathFixed)
                {
                    IsBool(obj.ModelID, obj.Type, d);
                }
            }
        }

        /// <summary>
        /// Static debug for Stepper.
        /// </summary>
        /// <param name="obj">The stepper object to be checked.</param>
        private void DebugForStepper(EcellObject obj)
        {
            EcellData l_max, l_min;
            l_max = null;
            l_min = null;
            foreach (EcellData d in obj.Value)
            {
                if (d.Name == Constants.headerMaximum ||
                    d.Name == Constants.headerMinimum ||
                    d.Name == Constants.xpathStepInterval ||
                    d.Name == Constants.headerTolerable)
                {
                    IsPositiveNumber(obj.ModelID, obj.Type, d);
                }
                if (d.Name == Constants.xpathIsEpsilonChecked)
                {
                    IsBool(obj.ModelID, obj.Type, d);
                }
                if (d.Name == Constants.headerMaximum) l_max = d;
                if (d.Name == Constants.headerMinimum) l_min = d;
            }
            if (l_max != null && l_min != null)
                CompareMaxAndMin(obj.ModelID, obj.Type, l_max, l_min);
        }

        #endregion

        /// <summary>
        /// Check whether ID is correct format.
        /// </summary>
        /// <param name="obj">The object to be checked.</param>
        private void IsInvalidComponentId(EcellObject obj)
        {
            if (obj.Type == Constants.xpathSystem)
            {
                if (Util.IsNGforSystemFullID(obj.Key))
                {
                    ErrorMessage mes = new ErrorMessage(obj.ModelID, obj.Type,
                        obj.Key + ":" + Constants.xpathID,
                        MessageResStDebug.ErrInvalidID);
                    m_errorList.Add(mes);
                }
            }
            else if (obj.Type == Constants.xpathProcess ||
                obj.Type == Constants.xpathVariable)
            {
                if (Util.IsNGforComponentFullID(obj.Key))
                {
                    ErrorMessage mes = new ErrorMessage(obj.ModelID, obj.Type,
                        obj.Key + ":" + Constants.xpathID,
                        MessageResStDebug.ErrInvalidID);
                    m_errorList.Add(mes);
                }
            }
        }

        /// <summary>
        /// Check whether the stepper exist.
        /// </summary>
        /// <param name="l_modelID">The model Id of object to be checked.</param>
        /// <param name="l_type">The type of object to be checked.</param>
        /// <param name="l_data">The data to be checked.</param>
        private void IsExistStepperID(string l_modelID, string l_type, EcellData l_data)
        {
            string l_entPath = l_data.EntityPath;
            string l_stepperID = l_data.Value.ToString();
            if (l_stepperID == null || l_stepperID.Equals("")) return;
            List<EcellObject> stepList = m_owner.DataManager.GetStepper(m_owner.DataManager.GetCurrentSimulationParameterID(), l_modelID);
            bool isHit = false;
            foreach (EcellObject step in stepList)
            {
                if (step.Key == l_stepperID)
                {
                    isHit = true;
                    break;
                }
            }
            if (isHit == false)
            {
                ErrorMessage mes = new ErrorMessage(l_modelID, l_type, l_entPath,
                                        MessageResStDebug.ErrNotExistStepper + "[" + l_stepperID + "]");
                m_errorList.Add(mes);
            }
        }

        /// <summary>
        /// Check whether tha type of property is bool.
        /// </summary>
        /// <param name="l_modelID">The model Id of object to be checked.</param>
        /// <param name="l_type">The type of object to be checked.</param>
        /// <param name="l_data">The data to be checked.</param>
        private void IsBool(string l_modelID, string l_type, EcellData l_data)
        {
            EcellValue val = l_data.Value;
            if (val == null)
            {
                ErrorMessage mes = new ErrorMessage(l_modelID, l_type, l_data.EntityPath,
                        MessageResStDebug.ErrNoSet);
                m_errorList.Add(mes);
                return;
            }
        }

        /// <summary>
        /// Check whether the value of property is the positive number.
        /// </summary>
        /// <param name="l_modelID">The model Id of object to be checked.</param>
        /// <param name="l_type">The type of object to be checked.</param>
        /// <param name="l_data">The data to be checked.</param>
        private void IsPositiveNumber(string l_modelID, string l_type, EcellData l_data)
        {
            EcellValue val = l_data.Value;
            if (val == null)
            {
                ErrorMessage mes = new ErrorMessage(l_modelID, l_type, l_data.EntityPath,
                        MessageResStDebug.ErrNoSet);
                m_errorList.Add(mes);
                return;
            }
            double d = val.CastToDouble();
            if (d <= 0.0)
            {
                ErrorMessage mes = new ErrorMessage(l_modelID, l_type, l_data.EntityPath,
                        MessageResStDebug.ErrPositive);
                m_errorList.Add(mes);
                return;
            }
        }

        /// <summary>
        /// Check whether the value of property is 0 or positive number.
        /// </summary>
        /// <param name="l_modelID">The model Id of object to be checked.</param>
        /// <param name="l_type">The type of object to be checked.</param>
        /// <param name="l_data">The data to be checked.</param>
        private void IsPositiveNumberWithZero(string l_modelID, string l_type, EcellData l_data)
        {
            EcellValue val = l_data.Value;
            if (val == null)
            {
                ErrorMessage mes = new ErrorMessage(l_modelID, l_type, l_data.EntityPath,
                        MessageResStDebug.ErrNoSet);
                m_errorList.Add(mes);
                return;
            }
            double d = val.CastToDouble();
            if (d < 0.0)
            {
                ErrorMessage mes = new ErrorMessage(l_modelID, l_type, l_data.EntityPath,
                        MessageResStDebug.ErrPositiveZero);
                m_errorList.Add(mes);
                return;
            }
        }

        /// <summary>
        /// Check whether min value is smaller than max value.
        /// </summary>
        /// <param name="l_modelID">The model Id of object to be checked.</param>
        /// <param name="l_type">The type of object to be checked.</param>
        /// <param name="l_max">The max data to be checked.</param>
        /// <param name="l_min">The min data to be checked.</param>
        private void CompareMaxAndMin(string l_modelID, string l_type, EcellData l_max, EcellData l_min)
        {
            double maxValue = l_max.Value.CastToDouble();
            double minValue = l_min.Value.CastToDouble();

            if (minValue > maxValue)
            {
                ErrorMessage mes = new ErrorMessage(l_modelID, l_type, l_max.EntityPath,
                        MessageResStDebug.ErrMaxMin);
                m_errorList.Add(mes);
                return;
            }
        }

        /// <summary>
        /// Check whether the number of bracket is correct.
        /// </summary>
        /// <param name="l_modelID">The model Id of object to be checked.</param>
        /// <param name="l_type">The type of object to be checked.</param>
        /// <param name="l_data">The data to be checked.</param>
        private void CheckBrackets(string l_modelID, string l_type, EcellData l_data)
        {
            Regex regBrackets = new Regex("[\\(\\)]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matchBrackets = null;
            int leftBracketsCount = 0;
            int rightBracketsCount = 0;
            for (matchBrackets = regBrackets.Match(l_data.Value.CastToString());
                matchBrackets.Success; matchBrackets = matchBrackets.NextMatch())
            {
                if (l_data.Value.CastToString()[matchBrackets.Groups[0].Index] == '(')
                {
                    leftBracketsCount++;
                }
                else
                {
                    rightBracketsCount++;
                }
            }
            if (leftBracketsCount != rightBracketsCount)
            {
                ErrorMessage mes = new ErrorMessage(l_modelID, l_type, l_data.EntityPath, MessageResStDebug.ErrBrackets);
                m_errorList.Add(mes);
                return;            
            }
        }        
    }
}
