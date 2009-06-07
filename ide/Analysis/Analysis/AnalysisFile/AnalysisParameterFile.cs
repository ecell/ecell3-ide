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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using Ecell;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.Analysis.AnalysisFile
{
    /// <summary>
    /// AnalysisParameter.
    /// </summary>
    public class AnalysisParameterFile
    {
        #region Fields
        /// <summary>
        /// ApplicationEnvironment.
        /// </summary>
        protected ApplicationEnvironment m_env;
        /// <summary>
        /// File path.
        /// </summary>
        protected string m_path;
        /// <summary>
        /// 
        /// </summary>
        protected XmlTextWriter m_writer;
        /// <summary>
        /// 
        /// </summary>
        public static string s_version = "1.0";
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env">Environment.</param>
        /// <param name="path">filename,</param>
        public AnalysisParameterFile(ApplicationEnvironment env, string path)
        {
            m_env = env;
            m_path = path;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        protected virtual void BeginWrite()
        {
            m_writer = new XmlTextWriter(m_path, Encoding.UTF8);
            m_writer.Formatting = Formatting.Indented;
            m_writer.WriteStartDocument();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void EndWrite()
        {
            m_writer.WriteEndDocument();
            m_writer.Close();
            m_writer = null;
        }

        /// <summary>
        /// Write the common properties of analysis.
        /// </summary>
        private void WriteCommonProperties()
        {
            m_writer.WriteStartElement(AnalysisParameterConstants.xProperties);
            m_writer.WriteStartElement(AnalysisParameterConstants.xUnknownProperties);
            List<EcellParameterData> paramList = m_env.DataManager.GetParameterData();
            foreach (EcellParameterData param in paramList)
            {               
                m_writer.WriteStartElement(AnalysisParameterConstants.xUnknownData);
                m_writer.WriteAttributeString(AnalysisParameterConstants.xClassName, param.GetType().Name);
                m_writer.WriteAttributeString(AnalysisParameterConstants.xVersion, s_version);

                m_writer.WriteElementString(AnalysisParameterConstants.xFullPN, param.Key);
                m_writer.WriteElementString(AnalysisParameterConstants.xMax, param.Max.ToString());
                m_writer.WriteElementString(AnalysisParameterConstants.xMin, param.Min.ToString());
                m_writer.WriteElementString(AnalysisParameterConstants.xStep, param.Step.ToString());
                m_writer.WriteEndElement();
            }
            m_writer.WriteEndElement();

            m_writer.WriteStartElement(AnalysisParameterConstants.xObservedProperties);
            List<EcellObservedData> obserList = m_env.DataManager.GetObservedData();
            foreach (EcellObservedData obj in obserList)
            {
                m_writer.WriteStartElement(AnalysisParameterConstants.xObservedData);
                m_writer.WriteAttributeString(AnalysisParameterConstants.xClassName, obj.GetType().Name);
                m_writer.WriteAttributeString(AnalysisParameterConstants.xVersion, s_version);

                m_writer.WriteElementString(AnalysisParameterConstants.xFullPN, obj.Key);
                m_writer.WriteElementString(AnalysisParameterConstants.xMax, obj.Max.ToString());
                m_writer.WriteElementString(AnalysisParameterConstants.xMin, obj.Min.ToString());
                m_writer.WriteElementString(AnalysisParameterConstants.xDiffer, obj.Differ.ToString());
                m_writer.WriteElementString(AnalysisParameterConstants.xRate, obj.Rate.ToString());
                m_writer.WriteEndElement();
            }
            m_writer.WriteEndElement();

            m_writer.WriteEndElement();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void WriteAnalysisParameter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Write()
        {
            try
            {
                BeginWrite();
                WriteCommonProperties();
                WriteAnalysisParameter();
                EndWrite();
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSaveFile, m_path));
            }
        }

        /// <summary>
        /// GetNodeByKey
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="key"></param>
        /// <returns>Selected XmlNode</returns>
        protected XmlNode GetNodeByKey(XmlNode xml, string key)
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
        /// GetStringAttribute
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetStringAttribute(XmlNode node, string key)
        {
            try
            {
                XmlAttribute attribute = node.Attributes[key];
                if (attribute == null)
                    return null;
                else
                    return attribute.Value;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Read()
        {
            if (!File.Exists(m_path))
                return;

            XmlDocument xmlD = new XmlDocument();
            try
            {
                xmlD.Load(m_path);
                XmlNode indexData = GetNodeByKey(xmlD, AnalysisParameterConstants.xAnalysisParameters);
                XmlNode properData = GetNodeByKey(indexData, AnalysisParameterConstants.xProperties);
                SetCommonProperties(properData);
                XmlNode paramData = GetNodeByKey(indexData, AnalysisParameterConstants.xParameters);
                SetAnalysisParameter(paramData);
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLoadFile, m_path));
            }
        }

        private void SetAnalysisParameter(XmlNode parameters)
        {
            foreach (XmlNode node in parameters.ChildNodes)
            {
                if (!node.Name.Equals(AnalysisParameterConstants.xParameter))
                    continue;

                string name = GetElementString(node, AnalysisParameterConstants.xParamName);
                string value = GetElementString(node, AnalysisParameterConstants.xParamValue);

                SetAnalysisProperty(name, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected virtual void SetAnalysisProperty(string name, string value)
        {

        }

        private void SetCommonProperties(XmlNode node)
        {
            XmlNode unknownData = GetNodeByKey(node, AnalysisParameterConstants.xUnknownProperties);
            SetUnknownData(unknownData);
            XmlNode observedData = GetNodeByKey(node, AnalysisParameterConstants.xObservedProperties);
            SetObservedData(observedData);            
        }

        private void SetUnknownData(XmlNode unknowns)
        {
            foreach (XmlNode node in unknowns.ChildNodes)
            {
                if (!node.Name.Equals(AnalysisParameterConstants.xUnknownData))
                    continue;

                string fullPN = GetElementString(node, AnalysisParameterConstants.xFullPN);
                double max = Double.Parse(GetElementString(node, AnalysisParameterConstants.xMax));
                double min = Double.Parse(GetElementString(node, AnalysisParameterConstants.xMin));
                double step = Double.Parse(GetElementString(node, AnalysisParameterConstants.xStep));

                EcellParameterData p = new EcellParameterData(fullPN, max, min, step);
                //m_env.DataManager.SetParameterData(p);
            }
        }

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

        private void SetObservedData(XmlNode observers)
        {
            foreach (XmlNode node in observers.ChildNodes)
            {
                if (!node.Name.Equals(AnalysisParameterConstants.xObservedData))
                    continue;

                string fullPN = GetElementString(node, AnalysisParameterConstants.xFullPN);
                double max = Double.Parse(GetElementString(node, AnalysisParameterConstants.xMax));
                double min = Double.Parse(GetElementString(node, AnalysisParameterConstants.xMin));
                double differ = Double.Parse(GetElementString(node, AnalysisParameterConstants.xDiffer));
                double rate = Double.Parse(GetElementString(node, AnalysisParameterConstants.xRate));

                EcellObservedData o = new EcellObservedData(fullPN, max, min, differ, rate);
                //m_env.DataManager.SetObservedData(o);
            }
        }
    }

    /// <summary>
    /// Constant parameter for analysis.
    /// </summary>
    public class AnalysisParameterConstants
    {
        #region CommonIndex
        /// <summary>
        /// 
        /// </summary>
        public const string xModelName = "model";
        /// <summary>
        /// 
        /// </summary>
        public const string xClassName = "classname";
        /// <summary>
        /// 
        /// </summary>
        public const string xVersion = "version";
        #endregion

        #region CommonPropertyIndex
        /// <summary>
        /// 
        /// </summary>
        public const string xProperties = "Properties";
        /// <summary>
        /// 
        /// </summary>
        public const string xUnknownProperties = "UnknownProperties";
        /// <summary>
        /// 
        /// </summary>
        public const string xObservedProperties = "OnservedProperties";
        /// <summary>
        /// 
        /// </summary>
        public const string xUnknownData = "UnknownData";
        /// <summary>
        /// 
        /// </summary>
        public const string xObservedData = "ObservedData";
        /// <summary>
        /// 
        /// </summary>
        public const string xFullPN = "FullPN";
        /// <summary>
        /// 
        /// </summary>
        public const string xMax = "Max";
        /// <summary>
        /// 
        /// </summary>
        public const string xMin = "Min";
        /// <summary>
        /// 
        /// </summary>
        public const string xStep = "Step";
        /// <summary>
        /// 
        /// </summary>
        public const string xDiffer = "Differ";
        /// <summary>
        /// 
        /// </summary>
        public const string xRate = "Rate";
        #endregion

        #region AnalysisPropertyIndex
        /// <summary>
        /// 
        /// </summary>
        public const string xAnalysisParameters = "AnalysisParameters";
        /// <summary>
        /// 
        /// </summary>
        public const string xAnalysisName = "analysisname";
        /// <summary>
        /// 
        /// </summary>
        public const string xParameters = "Parameters";
        /// <summary>
        /// 
        /// </summary>
        public const string xParameter = "Param";
        /// <summary>
        /// 
        /// </summary>
        public const string xParamName = "Name";
        /// <summary>
        /// 
        /// </summary>
        public const string xParamValue = "Value";
        #endregion
    }
}
