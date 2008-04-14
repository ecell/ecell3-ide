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

namespace EcellLib.Objects
{
    /// <summary>
    /// Class to manager the observed data.
    /// </summary>
    public class EcellObservedData
    {
        private string m_key;
        private double m_max;
        private double m_min;
        private double m_differ;
        private double m_rate;
        static private double s_multi = 0.5;

        /// <summary>
        /// Construcotor.
        /// </summary>
        public EcellObservedData()
        {
            m_key = "";
            m_max = 100.0;
            m_min = 0.0;
            m_differ = 100.0;
            m_rate = 0.5;
        }

        /// <summary>
        /// Construcotor with key and current value.
        /// </summary>
        /// <param name="key">the key of observed data.</param>
        /// <param name="current">the current value of observed data.</param>
        public EcellObservedData(string key, double current)
        {
            m_key = key;
            m_max = current * (1.0 + s_multi);
            m_min = current * (1.0 - s_multi);
            m_differ = m_max - m_min;
            m_rate = 0.5;
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="key">the key of observed data.</param>
        /// <param name="max">the max value of observed data.</param>
        /// <param name="min">the min value of observed data.</param>
        /// <param name="differ">the differ value of observed data.</param>
        /// <param name="rate">the rate value of observed data.</param>
        public EcellObservedData(string key, double max, double min,
            double differ, double rate)
        {
            m_key = key;
            m_max = max;
            m_min = min;
            m_differ = differ;
            m_rate = rate;
        }

        /// <summary>
        /// get / set the key of observed data.
        /// </summary>
        public string Key
        {
            get { return this.m_key; }
            set { this.m_key = value; }
        }

        /// <summary>
        /// get / set the max value of observed data.
        /// </summary>
        public double Max
        {
            get { return m_max; }
            set { m_max = value; }
        }

        /// <summary>
        /// get / set the min value of observed data.
        /// </summary>
        public double Min
        {
            get { return m_min; }
            set { m_min = value; }
        }

        /// <summary>
        /// get / set the differ value of observed data.
        /// </summary>
        public double Differ
        {
            get { return m_differ; }
            set { m_differ = value; }
        }

        /// <summary>
        /// get / set the rate value of observed data.
        /// </summary>
        public double Rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }

        /// <summary>
        /// Check whether this object is same.
        /// </summary>
        /// <param name="obj">the compared object.</param>
        /// <returns>if this object is same, return true.</returns>
        public override bool Equals(object obj)
        {
            bool res = base.Equals(obj);
            if (res) return res;
            EcellObservedData d = obj as EcellObservedData;
            if (d.Key == this.Key) return true;
            return false;
        }

        /// <summary>
        /// Get the hash code of this object.
        /// </summary>
        /// <returns>the hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
