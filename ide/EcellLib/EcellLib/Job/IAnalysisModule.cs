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
using System;
using System.Collections.Generic;
using System.Text;

using Ecell.Objects;

namespace Ecell.Job
{
    /// <summary>
    /// Analysis interface class.
    /// </summary>
    public interface IAnalysisModule
    {
        #region Accessors
        /// <summary>
        /// get / set the job group.
        /// </summary>
        JobGroup Group { get; set; }
        /// <summary>
        /// set the analysis parameter.
        /// </summary>
        object AnalysisParameter { set; }
        /// <summary>
        /// get / set the parameter list.
        /// </summary>
        List<EcellParameterData> ParameterDataList { get; set; }
        /// <summary>
        /// get / set the observed list.
        /// </summary>
        List<EcellObservedData> ObservedDataList { get; set; }
        /// <summary>
        /// get the flag this analysis is enable to judge.
        /// </summary>
        bool IsEnableReJudge { get; }
        /// <summary>
        /// get the flag this analysis is step execution.
        /// </summary>
        bool IsStep { get; }
        /// <summary>
        /// get the simulation time or the step count.
        /// </summary>
        double Count { get; }
        /// <summary>
        /// get the flag whether this analysis have any result.
        /// </summary>
        bool IsExistResult { get; }
        #endregion

        /// <summary>
        /// Set the property of analysis.
        /// </summary>
        /// <param name="paramDic">the dictionary of parameters.</param>
        void SetAnalysisProperty(Dictionary<string, string> paramDic);
        /// <summary>
        /// Get the property of analysis.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetAnalysisProperty();
        /// <summary>
        /// Execute this function when this analysis is finished.
        /// </summary>
        void NotifyAnalysisFinished();
        /// <summary>
        /// Create the analysis instance.
        /// </summary>
        /// <param name="group">the analysis group name.</param>
        /// <returns></returns>
        IAnalysisModule CreateNewInstance(JobGroup group);
        /// <summary>
        /// Execute this analysis.
        /// </summary>
        void ExecuteAnalysis();
        /// <summary>
        /// Prepare to execute the analysis again.
        /// </summary>
        void PrepareReAnalysis();
        /// <summary>
        /// Load the analysis model, parameters and result.
        /// </summary>
        /// <param name="dirName">the top directory[Model] of the loaded analysis.</param>
        void LoadAnalysisInfo(string dirName);
        /// <summary>
        /// Save the analysis model, parameters and result.
        /// </summary>
        /// <param name="dirName">the top directory[Model] of the saved analysis.</param>
        void SaveAnalysisInfo(string dirName);
        /// <summary>
        /// Print the current result of analysis.
        /// </summary>
        void PrintResult();
        /// <summary>
        /// Judgement.
        /// </summary>
        void Judgement();
        /// <summary>
        /// Get the flag whether this property is editable.
        /// </summary>
        /// <param name="key">the property name.</param>
        /// <returns>true or false.</returns>
        bool IsEnableEditProperty(string key);
    }
}
