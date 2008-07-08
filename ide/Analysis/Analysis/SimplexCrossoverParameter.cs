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

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Class managed the parameter of simplex cross over by using Parameter Estimation.
    /// </summary>
    public class SimplexCrossoverParameter
    {
        private Int32 m;
        private double m0;
        private double mmax;
        private double k;
        private double upsilon;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimplexCrossoverParameter()
        {
            m = 3;
            m0 = 1.05;
            mmax = 50.0;
            k = 1.8;
            upsilon = 1.2;
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="m0"></param>
        /// <param name="mmax"></param>
        /// <param name="k"></param>
        /// <param name="upsilon"></param>
        public SimplexCrossoverParameter(int m, double m0, double mmax, 
                                        double k, double upsilon)
        {
            this.m = m;
            this.m0 = m0;
            this.mmax = mmax;
            this.k = k;
            this.upsilon = upsilon;
        }

        /// <summary>
        /// get / set the number of choosed parameter.
        /// </summary>
        public int M
        {
            get { return this.m; }
            set { this.m = value; }
        }

        /// <summary>
        /// get / set the initial value of mutation rate.
        /// </summary>
        public double Initial
        {
            get { return this.m0; }
            set { this.m0 = value; }
        }

        /// <summary>
        /// get / set the max value of mutation rate.
        /// </summary>
        public double Max
        {
            get { return this.mmax; }
            set { this.mmax = value; }
        }

        /// <summary>
        /// get / set the scale factor of mutation rate.
        /// </summary>
        public double K
        {
            get { return this.k; }
            set { this.k = value; }
        }

        /// <summary>
        /// get / set the upsilon.
        /// </summary>
        public double Upsilon
        {
            get { return this.upsilon; }
            set { this.upsilon = value; }
        }

    }
}
