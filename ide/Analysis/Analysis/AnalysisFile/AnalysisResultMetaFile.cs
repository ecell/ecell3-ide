﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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

namespace Ecell.IDE.Plugins.Analysis.AnalysisFile
{
    /// <summary>
    /// Meta file class of analysis result.
    /// </summary>
    public class AnalysisResultMetaFile
    {
        #region Fields
        /// <summary>
        /// Version string.
        /// </summary>
        private static string s_version = "1.0";
        #endregion

        #region Common
        private static XmlTextWriter CreateWriter(string path)
        {
            XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            return writer;
        }

        private static void BeginWrite(XmlTextWriter writer, string metaType, string analysisName)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(metaType);
            writer.WriteAttributeString(AnalysisResultMetaFileConst.xName, analysisName);
            writer.WriteAttributeString(AnalysisResultMetaFileConst.xClassName, analysisName);
            writer.WriteAttributeString(AnalysisResultMetaFileConst.xVersion, AnalysisResultMetaFile.s_version);
        }

        private static void EndWrite(XmlTextWriter writer)
        {
            writer.WriteEndDocument();
            writer.Close();
        }
        #endregion

        #region PublicFunction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="analysisName"></param>
        /// <param name="labels"></param>
        public static void CreatePlotMetaFile(string path, string analysisName, List<string> labels)
        {
            try
            {
                XmlTextWriter writer = CreateWriter(path);
                BeginWrite(writer, AnalysisResultMetaFileConst.xPlotData, analysisName);

                foreach (string labelName in labels)
                {
                    writer.WriteElementString(AnalysisResultMetaFileConst.xLabel, labelName);
                }

                EndWrite(writer);
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSaveFile, path));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="analysisName"></param>
        /// <param name="tables"></param>
        public static void CreateTableMetaFile(string path, string analysisName, List<string> tables)
        {
            try
            {
                XmlTextWriter writer = CreateWriter(path);
                BeginWrite(writer, AnalysisResultMetaFileConst.xTableData, analysisName);

                foreach (string tableName in tables)
                {
                    writer.WriteElementString(AnalysisResultMetaFileConst.xTableName, tableName);
                }

                EndWrite(writer);
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSaveFile, path));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="analysisName"></param>
        public static void CreateGenerationData(string path, string analysisName, List<string> labels)
        {
            try
            {
                XmlTextWriter writer = CreateWriter(path);
                BeginWrite(writer, AnalysisResultMetaFileConst.xGenerationData, analysisName);
                foreach (string labelName in labels)
                {
                    writer.WriteElementString(AnalysisResultMetaFileConst.xLabel, labelName);
                }
                EndWrite(writer);
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSaveFile, path));
            }
        }

        public static bool LoadFile(string fileName, out string analysisName, out List<string> result)
        {
            analysisName = null;
            result = null;
            if (!File.Exists(fileName))
            {
                return false;
            }

            XmlDocument xmlD = new XmlDocument();
            XmlNode indexData;
            try
            {
                xmlD.Load(fileName);
                result = new List<string>();
                indexData = GetNodeByKey(xmlD, AnalysisResultMetaFileConst.xGenerationData);
                if (indexData != null)
                {
                    analysisName = GetStringAttribute(indexData, AnalysisResultMetaFileConst.xName);
                    foreach (XmlNode node in indexData.ChildNodes)
                    {
                        if (!node.Name.Equals(AnalysisResultMetaFileConst.xLabel))
                            continue;
                        result.Add(node.InnerText);
                    }
                    return true;
                }
                indexData = GetNodeByKey(xmlD, AnalysisResultMetaFileConst.xPlotData);
                if (indexData != null)
                {
                    analysisName = GetStringAttribute(indexData, AnalysisResultMetaFileConst.xName);
                    foreach (XmlNode node in indexData.ChildNodes)
                    {
                        if (!node.Name.Equals(AnalysisResultMetaFileConst.xLabel))
                            continue;
                        result.Add(node.InnerText);
                    }

                    return true;
                }
                indexData = GetNodeByKey(xmlD, AnalysisResultMetaFileConst.xTableData);
                if (indexData != null)
                {
                    analysisName = GetStringAttribute(indexData, AnalysisResultMetaFileConst.xName);
                    foreach (XmlNode node in indexData.ChildNodes)
                    {
                        if (!node.Name.Equals(AnalysisResultMetaFileConst.xTableName))
                            continue;
                        result.Add(node.InnerText);
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLoadFile, fileName));
            }
            return false;
        }
        #endregion

        /// <summary>
        /// GetNodeByKey
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="key"></param>
        /// <returns>Selected XmlNode</returns>
        private static XmlNode GetNodeByKey(XmlNode xml, string key)
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
    }

    /// <summary>
    /// Constant parameter for meta file of analysis result.
    /// </summary>
    public class AnalysisResultMetaFileConst
    {
        /// <summary>
        /// 
        /// </summary>
        public const string xPlotData = "PlotData";
        /// <summary>
        /// 
        /// </summary>
        public const string xTableData = "TableData";
        /// <summary>
        /// 
        /// </summary>
        public const string xGenerationData = "GenerationData";
        /// <summary>
        /// 
        /// </summary>
        public const string xLabel = "Label";
        /// <summary>
        /// 
        /// </summary>
        public const string xTableName = "TableName";
        /// <summary>
        /// 
        /// </summary>
        public const string xName = "name";
        /// <summary>
        /// 
        /// </summary>
        public const string xClassName = "classname";
        /// <summary>
        /// 
        /// </summary>
        public const string xVersion = "version";
    }
}