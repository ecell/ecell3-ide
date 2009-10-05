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

namespace Ecell.Objects
{
    /// <summary>
    /// The class to manage the parameter of data.
    /// </summary>
    public class EcellParameterData
    {
        private string m_key;
        private double m_max;
        private double m_min;
        private double m_step;
        static private double s_multi = 0.5;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EcellParameterData()
            : this("", 100.0, 0.0, 0.0)
        {
        }

        /// <summary>
        /// Construcotor with key and current data.
        /// </summary>
        /// <param name="key">the key of parameter data.</param>
        /// <param name="current">the current value of parameter data.</param>
        public EcellParameterData(string key, double current)
        {
            m_key = key;
            m_max = current + Math.Abs(current * s_multi);
            m_min = current - Math.Abs(current * s_multi);
            m_step = (m_max - m_min) / 10.0;
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="key">the key of parameter data.</param>
        /// <param name="max">the max value of parameter data.</param>
        /// <param name="min">the min value of parameter data.</param>
        /// <param name="step">the step value of parameter data.</param>
        public EcellParameterData(string key, double max, double min, double step)
        {
            m_key = key;
            m_max = max;
            m_min = min;
            m_step = step;
        }

        /// <summary>
        /// get / set the key of parameter data.
        /// </summary>
        public string Key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        /// <summary>
        /// get / set the max value of parameter data.
        /// </summary>
        public double Max
        {
            get { return m_max; }
            set { m_max = value; }
        }

        /// <summary>
        /// get / set the min value of parameter data.
        /// </summary>
        public double Min
        {
            get { return m_min; }
            set { m_min = value; }
        }

        /// <summary>
        /// get / set the step value of parameter data.
        /// </summary>
        public double Step
        {
            get { return m_step; }
            set { m_step = value; }
        }

        /// <summary>
        /// Check whether this object is same.
        /// </summary>
        /// <param name="obj">the compared object.</param>
        /// <returns>if this object is same, return true.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is EcellParameterData))
                return false;

            EcellParameterData data = obj as EcellParameterData;
            return (data.Key == this.Key);
        }

        /// <summary>
        /// Get the hash code of this object.
        /// </summary>
        /// <returns>the hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Copy this object.
        /// </summary>
        /// <returns></returns>
        public EcellParameterData Copy()
        {
            return new EcellParameterData(m_key, m_max, m_min, m_step);
        }
    }
}
