using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Job
{

    /// <summary>
    /// Manage the execution parameter to analysis.
    /// </summary>
    public class ExecuteParameter
    {
        private Dictionary<string, double> m_paramDic = new Dictionary<string, double>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExecuteParameter()
        {
        }

        /// <summary>
        /// Constructor with the initial parameter.
        /// </summary>
        /// <param name="data">the list of parameter.</param>
        public ExecuteParameter(Dictionary<string, double> data)
        {
            foreach (string d in data.Keys)
            {
                m_paramDic.Add(d, data[d]);
            }
        }

        /// <summary>
        /// get / set the list of execution parameter.
        /// </summary>
        public Dictionary<string, double> ParamDic
        {
            get { return this.m_paramDic; }
            set { this.m_paramDic = value; }
        }

        /// <summary>
        /// Add the execution parameter.
        /// </summary>
        /// <param name="path">path.</param>
        /// <param name="value">execution parameter.</param>
        public void AddParameter(string path, double value)
        {
            m_paramDic.Add(path, value);
        }

        /// <summary>
        /// Get the execution parameter of target path.
        /// </summary>
        /// <param name="path">path.</param>
        /// <returns>execution parameter.</returns>
        public double GetParameter(string path)
        {
            if (m_paramDic.ContainsKey(path))
            {
                return m_paramDic[path];
            }
            return 0.0;
        }

        /// <summary>
        /// Remove the execution paramter from list.
        /// </summary>
        /// <param name="path">path.</param>
        public void RemoveParameter(string path)
        {
            m_paramDic.Remove(path);
        }

    }
}
