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
    /// Analysis parameter file for Robust Analysis.
    /// </summary>
    public class RobustAnalysisParameterFile : AnalysisParameterFile
    {
        #region Fields
        private RobustAnalysisParameter m_param;
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public RobustAnalysisParameter Parameter
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
        public RobustAnalysisParameterFile(IAnalysisModule analysis, string path)
            : base(analysis, path)
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
            m_writer.WriteAttributeString(AnalysisParameterConstants.xAnalysisName, "RobustAnalysis");
            m_writer.WriteAttributeString(AnalysisParameterConstants.xClassName, "RobustAnalysis");
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
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, RobustAnalysisConstants.xSimulationTime);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.SimulationTime.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, RobustAnalysisConstants.xWindowSize);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.WinSize.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, RobustAnalysisConstants.xMaxInput);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.MaxData.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, RobustAnalysisConstants.xMaxFreq);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.MaxFreq.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, RobustAnalysisConstants.xMinFreq);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.MinFreq.ToString());
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, RobustAnalysisConstants.xIsRandom);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.IsRandomCheck ? "1" : "0");
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xParameter);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamName, RobustAnalysisConstants.xSampleNum);
            m_writer.WriteElementString(AnalysisParameterConstants.xParamValue, m_param.SampleNum.ToString());
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
                case RobustAnalysisConstants.xWindowSize:
                    m_param.WinSize = Double.Parse(value);
                    break;
                case RobustAnalysisConstants.xSimulationTime:
                    m_param.SimulationTime = Double.Parse(value);
                    break;
                case RobustAnalysisConstants.xMaxInput:
                    m_param.MaxData = Int32.Parse(value);
                    break;
                case RobustAnalysisConstants.xMaxFreq:
                    m_param.MaxFreq = Double.Parse(value);
                    break;
                case RobustAnalysisConstants.xMinFreq:
                    m_param.MinFreq = Double.Parse(value);
                    break;
                case RobustAnalysisConstants.xSampleNum:
                    m_param.SampleNum = Int32.Parse(value);
                    break;
                case RobustAnalysisConstants.xIsRandom:
                    int i = Int32.Parse(value);
                    if (i == 1)
                        m_param.IsRandomCheck = true;
                    else
                        m_param.IsRandomCheck = false;
                    break;   
            }
        }
    }

    /// <summary>
    /// Xml string for parameter of Robust Analysis.
    /// </summary>
    public class RobustAnalysisConstants
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
        /// <summary>
        /// 
        /// </summary>
        public const string xIsRandom = "IsRandom";
        /// <summary>
        /// 
        /// </summary>
        public const string xSampleNum = "SampleNumber";
    }
}
