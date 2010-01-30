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

using Ecell.Job;

namespace Ecell.IDE.Plugins.Analysis.AnalysisFile
{
    /// <summary>
    /// Analysis parameter file for Sensitivity Analysis.
    /// </summary>
    public class SensitivityAnalysisParameterFile : AnalysisParameterFile
    {
        #region Fields
        /// <summary>
        /// Sensitivity analysis parameter.
        /// </summary>
        private SensitivityAnalysisParameter m_param;        
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the sensitivity analysis.
        /// </summary>
        public SensitivityAnalysisParameter Parameter
        {
            get { return this.m_param; }
            set { this.m_param = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="analysis">analysis module</param>
        /// <param name="path">file name.</param>
        public SensitivityAnalysisParameterFile(IAnalysisModule analysis, string path)
            : base(analysis, path)
        {

        }
        #endregion

        /// <summary>
        /// Write the header information of parameter file.
        /// </summary>
        protected override void BeginWrite()
        {
            base.BeginWrite();
            m_writer.WriteStartElement(AnalysisParameterConstants.xAnalysisParameters);
            m_writer.WriteAttributeString(AnalysisParameterConstants.xAnalysisName, "SensitivityAnalysis");
            m_writer.WriteAttributeString(AnalysisParameterConstants.xClassName, "SensitivityAnalysis");
            m_writer.WriteAttributeString(AnalysisParameterConstants.xVersion, AnalysisParameterFile.s_version);
        }

        /// <summary>
        /// Write the footer information of parameter file.
        /// </summary>
        protected override void EndWrite()
        {
            m_writer.WriteEndElement();
            base.EndWrite();
        }

        /// <summary>
        /// Write the analysis parameters.
        /// </summary>
        protected override void WriteAnalysisParameter()
        {
            m_writer.WriteStartElement(AnalysisParameterConstants.xParameters);

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, SensitivityAnalysisConstants.xStep);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.Step.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, SensitivityAnalysisConstants.xAbsolutePerturbation);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.AbsolutePerturbation.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, SensitivityAnalysisConstants.xRelativePerturbation);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.RelativePerturbation.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteEndElement();
        }

        /// <summary>
        /// Set the analysis parameter in the analysis parameter fils.
        /// </summary>
        /// <param name="name">the property name.</param>
        /// <param name="value">the property value.</param>
        protected override void SetAnalysisProperty(string name, string value)
        {
            switch (name)
            {
                case SensitivityAnalysisConstants.xStep:
                    m_param.Step = Int32.Parse(value);
                    break;
                case SensitivityAnalysisConstants.xAbsolutePerturbation:
                    m_param.AbsolutePerturbation = Double.Parse(value);
                    break;
                case SensitivityAnalysisConstants.xRelativePerturbation:
                    m_param.RelativePerturbation = Double.Parse(value);
                    break;
            }
        }
    }

    /// <summary>
    /// Xml string for parameter of bifurcation analysis.
    /// </summary>
    public class SensitivityAnalysisConstants
    {
        /// <summary>
        /// The label of Step.
        /// </summary>
        public const string xStep = "Step";
        /// <summary>
        /// The label of absolute perturbation.
        /// </summary>
        public const string xAbsolutePerturbation = "AbsolutePerturbation";
        /// <summary>
        /// The label of relative perturbation.
        /// </summary>
        public const string xRelativePerturbation = "RelativePerturbation";
    }
}
