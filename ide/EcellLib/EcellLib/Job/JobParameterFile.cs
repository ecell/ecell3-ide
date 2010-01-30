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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Ecell.Job
{
    /// <summary>
    /// Class for parameter of job.
    /// </summary>
    public class JobParameterFile
    {
        #region Fields
        /// <summary>
        /// job.
        /// </summary>
        private Job m_job;
        /// <summary>
        /// File name path.
        /// </summary>
        private string m_path;
        /// <summary>
        /// the writer object.
        /// </summary>
        protected XmlTextWriter m_writer;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public JobParameterFile(Job job, string path)
        {
            m_job = job;
            m_path = path;
        }
        #endregion
        /// <summary>
        /// Read the job property file.
        /// </summary>
        public void Read()
        {
            if (!File.Exists(m_path))
                return;

            XmlDocument xmlD = new XmlDocument();
            try
            {
                xmlD.Load(m_path);
                XmlNode indexData = GetNodeByKey(xmlD, JobParameterConsts.xExecParameters);
                SetParameterData(indexData);
                string data = GetElementString(indexData, JobParameterConsts.xStatus);
                m_job.Status = JobParameterConsts.IntConvert(Int32.Parse(data));
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLoadFile, m_path));
                return;
            }
        }

        private void SetParameterData(XmlNode topnode)
        {
            foreach (XmlNode node in topnode.ChildNodes)
            {
                if (!node.Name.Equals(JobParameterConsts.xParameter))
                    continue;

                string fullPN = GetElementString(node, JobParameterConsts.xFullPN);
                string value = GetElementString(node, JobParameterConsts.xValue);
                double d = Double.Parse(value);

                m_job.ExecParam.ParamDic[fullPN] = d;
            }
        }

        /// <summary>
        /// Write the job property file.
        /// </summary>
        public void Write()
        {
            BeginWrite();
            WriteParameter();
            EndWrite();
        }

        /// <summary>
        /// Pre procedure before the job property is wrote.
        /// </summary>
        private void BeginWrite()
        {
            m_writer = new XmlTextWriter(m_path, Encoding.UTF8);
            m_writer.Formatting = Formatting.Indented;
            m_writer.WriteStartDocument();
        }

        /// <summary>
        /// The job property is wrote.
        /// </summary>
        private void WriteParameter()
        {
            m_writer.WriteStartElement(JobParameterConsts.xExecParameters);
            m_writer.WriteElementString(JobParameterConsts.xStatus, JobParameterConsts.TypeConvert(m_job.Status).ToString());
            foreach (string name in m_job.ExecParam.ParamDic.Keys)
            {
                m_writer.WriteStartElement(JobParameterConsts.xParameter);
                m_writer.WriteElementString(JobParameterConsts.xFullPN, name);
                m_writer.WriteElementString(JobParameterConsts.xValue, m_job.ExecParam.ParamDic[name].ToString());
                m_writer.WriteEndElement();
            }

            m_writer.WriteEndElement();
        }

        /// <summary>
        /// Post procedure after the job property is wrote.
        /// </summary>
        private void EndWrite()
        {
            m_writer.WriteEndDocument();
            m_writer.Close();
            m_writer = null;
        }

        /// <summary>
        /// GetNodeByKey
        /// </summary>
        /// <param name="xml">XML node.</param>
        /// <param name="key">the key string.</param>
        /// <returns>Selected XmlNode</returns>
        private XmlNode GetNodeByKey(XmlNode xml, string key)
        {
            XmlNode selected = null;
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node.Name.Equals(key))
                    selected = node;
            }
            return selected;
        }

        /// <summary>
        /// Get element string.
        /// </summary>
        /// <param name="node">XML node.</param>
        /// <param name="name">the key name.</param>
        /// <returns>the element string.</returns>
        private string GetElementString(XmlNode node, string name)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name.Equals(name))
                {
                    return n.InnerText;
                }
                string tmp = GetElementString(n, name);
                if (!string.IsNullOrEmpty(tmp))
                    return tmp;
            }
            return null;
        }

    }

    /// <summary>
    /// Constant class for job parameters.
    /// </summary>
    public class JobParameterConsts
    {
        /// <summary>
        /// ExecParameters.
        /// </summary>
        public const string xExecParameters = "ExecParameters";
        /// <summary>
        /// Parameter
        /// </summary>
        public const string xParameter = "Parameter";
        /// <summary>
        /// FullPN
        /// </summary>
        public const string xFullPN = "FullPN";
        /// <summary>
        /// Value
        /// </summary>
        public const string xValue = "Value";
        /// <summary>
        /// Status
        /// </summary>
        public const string xStatus = "Status";

        /// <summary>
        /// Convert JobStatus to Int.
        /// </summary>
        /// <param name="status">the job status.</param>
        /// <returns>job status integer.</returns>
        public static int TypeConvert(JobStatus status)
        {
            switch (status)
            {
                case JobStatus.FINISHED:
                    return 1;
                case JobStatus.ERROR:
                    return 2;
                case JobStatus.QUEUED:
                    return 3;
                case JobStatus.RUNNING:
                    return 4;
                case JobStatus.STOPPED:
                    return 5;
            }
            return -1;
        }

        /// <summary>
        /// Convert int to JobStatus.
        /// </summary>
        /// <param name="id">the job status integer.</param>
        /// <returns>JobStatus</returns>
        public static JobStatus IntConvert(int id)
        {
            switch (id)
            {
                case 1:
                    return JobStatus.FINISHED;
                case 2:
                    return JobStatus.ERROR;
                case 3:
                    return JobStatus.QUEUED;
                case 4:
                    return JobStatus.RUNNING;
                case 5:
                    return JobStatus.STOPPED;
            }
            return JobStatus.NONE;
        }
    }
}
