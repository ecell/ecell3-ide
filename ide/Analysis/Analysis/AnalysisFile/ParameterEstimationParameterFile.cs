//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
    /// Analysis parameter file for Parameter Estimation.
    /// </summary>
    public class ParameterEstimationParameterFile : AnalysisParameterFile
    {
        #region Fields
        /// <summary>
        /// The parameter of parameter estimation.
        /// </summary>
        private ParameterEstimationParameter m_param;
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the parameter of parameter estimation.
        /// </summary>
        public ParameterEstimationParameter Parameter
        {
            get { return this.m_param; }
            set { this.m_param = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="analysis">analysis module.</param>
        /// <param name="path">file name.</param>
        public ParameterEstimationParameterFile(IAnalysisModule analysis, string path)
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
            m_writer.WriteAttributeString(AnalysisParameterConstants.xAnalysisName, "ParameterEstimation");
            m_writer.WriteAttributeString(AnalysisParameterConstants.xClassName, "ParameterEstimation");
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
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xSimulationTime);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.SimulationTime.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xGeneration);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.Generation.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xPopulation);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.Population.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xEstimationFormulator);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.EstimationFormulator);
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xInitial);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.Param.Initial.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xK);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.Param.K.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xM);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.Param.M.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xMax);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.Param.Max.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, ParameterEstimationConstants.xUpsilon);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.Param.Upsilon.ToString());
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
                case ParameterEstimationConstants.xSimulationTime:
                    m_param.SimulationTime = Double.Parse(value);
                    break;
                case ParameterEstimationConstants.xGeneration:
                    m_param.Generation = Int32.Parse(value);
                    break;
                case ParameterEstimationConstants.xPopulation:
                    m_param.Population = Int32.Parse(value);
                    break;
                case ParameterEstimationConstants.xEstimationFormulator:
                    m_param.EstimationFormulator = value;
                    break;
                case ParameterEstimationConstants.xInitial:
                    m_param.Param.Initial = Double.Parse(value);
                    break;
                case ParameterEstimationConstants.xK:
                    m_param.Param.K = Double.Parse(value);
                    break;
                case ParameterEstimationConstants.xM:
                    m_param.Param.M = Int32.Parse(value);
                    break;
                case ParameterEstimationConstants.xMax:
                    m_param.Param.Max = Double.Parse(value);
                    break;
                case ParameterEstimationConstants.xUpsilon:
                    m_param.Param.Upsilon = Double.Parse(value);
                    break;
            }
        }
    }

    /// <summary>
    /// Xml string for parameter of Parameter Estimation.
    /// </summary>
    public class ParameterEstimationConstants
    {
        /// <summary>
        /// The label of simulation time.
        /// </summary>
        public const string xSimulationTime = "SimulationTime";
        /// <summary>
        /// The label of generation.
        /// </summary>
        public const string xGeneration = "Generation";
        /// <summary>
        /// The label of population.
        /// </summary>
        public const string xPopulation = "Population";
        /// <summary>
        /// The label of estimation formulator.
        /// </summary>
        public const string xEstimationFormulator = "EstimationFormulator";
        /// <summary>
        /// The label of initial rate.
        /// </summary>
        public const string xInitial = "InitialRate";
        /// <summary>
        /// The label of increase rate.
        /// </summary>
        public const string xK = "IncreaseRate";
        /// <summary>
        /// The label of current rate.
        /// </summary>
        public const string xM = "CurrentRate";
        /// <summary>
        /// The label of max rate.
        /// </summary>
        public const string xMax = "MaxRate";
        /// <summary>
        /// The label of upsilon.
        /// </summary>
        public const string xUpsilon = "Upsilon";
    }
}
