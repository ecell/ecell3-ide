//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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

namespace EcellLib.Analysis
{
    /// <summary>
    /// Class managed the parameter to judge the result of analysis.
    /// </summary>
    public class AnalysisJudgementParam
    {
        private string m_path = "";
        private double m_max = 0.0;
        private double m_min = 0.0;
        private double m_difference = 0.0;
        private double m_rate = 0.0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AnalysisJudgementParam()
        {
        }

        /// <summary>
        /// Constructor with initial parameters.
        /// </summary>
        /// <param name="path">entry path to judge the logger data.</param>
        /// <param name="max">the maximum value of logger data.</param>
        /// <param name="min">the minimum value of logger data.</param>
        /// <param name="diff">the difference value of logger data.</param>
        /// <param name="rate">the rate of FFT.</param>
        public AnalysisJudgementParam(string path, double max, double min, double diff, double rate)
        {
            m_path = path;
            m_max = max;
            m_min = min;
            m_difference = diff;
            m_rate = rate;
        }

        /// <summary>
        /// get/set the entry path to use by the judgement of robustness.
        /// </summary>
        public string Path
        {
            get { return this.m_path; }
            set { this.m_path = value; }
        }

        /// <summary>
        /// get/set the maximum value of logger data.
        /// </summary>
        public double Max
        {
            get { return this.m_max; }
            set { this.m_max = value; }
        }

        /// <summary>
        /// get/set the minimum value of logger data.
        /// </summary>
        public double Min
        {
            get { return this.m_min; }
            set { this.m_min = value; }
        }

        /// <summary>
        /// get/set the differnce value of logger data.
        /// </summary>
        public double Difference
        {
            get { return this.m_difference; }
            set { this.m_difference = value; }
        }

        /// <summary>
        /// get/set the rate of FFT.
        /// </summary>
        public double Rate
        {
            get { return this.m_rate; }
            set { this.m_rate = value; }
        }
    }
}
