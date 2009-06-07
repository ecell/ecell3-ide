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

namespace Ecell.IDE.Plugins.Analysis.AnalysisFile
{
    /// <summary>
    /// Analysis parameter file for Bifurcation Analysis.
    /// </summary>
    public class BifurcationAnlaysisParameterFile : AnalysisParameterFile
    {
        #region Fields
        private BifurcationAnalysisParameter m_param;        
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public BifurcationAnalysisParameter Parameter
        {
            get { return this.m_param; }
            set { this.m_param = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env">Environment.</param>
        /// <param name="path">file name.</param>
        public BifurcationAnlaysisParameterFile(ApplicationEnvironment env, string path)
            : base(env, path)
        {

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected override void BeginWrite()
        {
            base.BeginWrite();
            m_writer.WriteStartElement(AnalysisParameterConstants.xAnalysisParameters);
            m_writer.WriteAttributeString(AnalysisParameterConstants.xAnalysisName, "BifurcationAnalysis");
            m_writer.WriteAttributeString(AnalysisParameterConstants.xClassName, "BifurcationAnalysis");
            m_writer.WriteAttributeString(AnalysisParameterConstants.xVersion, AnalysisParameterFile.s_version);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void EndWrite()
        {
            m_writer.WriteEndElement();
            base.EndWrite();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void WriteAnalysisParameter()
        {
            m_writer.WriteStartElement(AnalysisParameterConstants.xParameters);

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, BifurcationAnalysisConstants.xSimulationTime);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.SimulationTime.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, BifurcationAnalysisConstants.xWindowSize);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.WindowSize.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, BifurcationAnalysisConstants.xMaxInput);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.MaxInput.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, BifurcationAnalysisConstants.xMaxFreq);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.MaxFreq.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, BifurcationAnalysisConstants.xMinFreq);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.MinFreq.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected override void SetAnalysisProperty(string name, string value)
        {
            switch (name)
            {
                case BifurcationAnalysisConstants.xWindowSize:
                    m_param.WindowSize = Double.Parse(value);
                    break;
                case BifurcationAnalysisConstants.xSimulationTime:
                    m_param.SimulationTime = Double.Parse(value);
                    break;
                case BifurcationAnalysisConstants.xMaxInput:
                    m_param.MaxInput = Int32.Parse(value);
                    break;
                case BifurcationAnalysisConstants.xMaxFreq:
                    m_param.MaxFreq = Double.Parse(value);
                    break;
                case BifurcationAnalysisConstants.xMinFreq:
                    m_param.MinFreq = Double.Parse(value);
                    break;
            }
        }
    }

    /// <summary>
    /// Xml string for parameter of bifurcation analysis.
    /// </summary>
    public class BifurcationAnalysisConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string xWindowSize = "WindowSize";
        /// <summary>
        /// 
        /// </summary>
        public const string xSimulationTime = "SimulationTime";
        /// <summary>
        /// 
        /// </summary>
        public const string xMaxInput = "MaxInput";
        /// <summary>
        /// 
        /// </summary>
        public const string xMaxFreq = "MaxFrequency";
        /// <summary>
        /// 
        /// </summary>
        public const string xMinFreq = "MinFrequency";
    }
}
