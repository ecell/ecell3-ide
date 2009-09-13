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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Objects;
using System.Xml;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

using Ecell.Logger;

namespace Ecell
{
    /// <summary>
    /// LEML class.
    /// </summary>
    public class Leml
    {
        /// <summary>
        /// Version of config file.
        /// </summary>
        private const string CONFIG_FILE_VERSION = "1.0";

        /// <summary>
        /// LoadLEML
        /// </summary>
        /// <param name="env">ApplicationEnvironment.</param>
        /// <param name="model">the model ID.</param>
        /// <param name="filename">the filename.</param>
        public static void LoadLEML(ApplicationEnvironment env, EcellModel model,string filename)
        {
            if (!File.Exists(filename))
                return;

            XmlDocument xmlD = new XmlDocument();
            try
            {
                xmlD.Load(filename);
                XmlNode applicationData = GetNodeByKey(xmlD, LemlConstants.xPathApplication);

                // Load Layers
                XmlNode layers = GetNodeByKey(applicationData, LemlConstants.xPathLayerList);
                SetLayers(model, layers);

                // Load Aliases
                XmlNode aliases = GetNodeByKey(applicationData, LemlConstants.xPathAliasList);
                SetAliases(model, aliases);

                // Load EcellObjects
                XmlNode ecellObjects = GetNodeByKey(applicationData, LemlConstants.xPathEcellObjectList);
                SetEcellObjects(model, ecellObjects);

                // Load Logger
                XmlNode loggers = GetNodeByKey(applicationData, LemlConstants.xPathLoggerList);
                SetLogger(env, model, loggers);

                // Load plugin settings
                XmlNode settings = GetNodeByKey(applicationData, LemlConstants.xPathPluginSettings);
                SetPluginSettings(env, settings);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(string.Format(MessageResources.ErrLoadFile, filename) + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// GetNodeByKey
        /// </summary>
        /// <param name="xml">XmlNode</param>
        /// <param name="key">the string key.</param>
        /// <returns>Selected XmlNode</returns>
        public static XmlNode GetNodeByKey(XmlNode xml, string key)
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
        /// Set the Layers to XML node.
        /// </summary>
        /// <param name="model">the model object.</param>
        /// <param name="layers">Layer XML node.</param>
        private static void SetLayers(EcellModel model, XmlNode layers)
        {
            if (layers == null || layers.ChildNodes.Count <= 0)
                return;
            List<EcellLayer> elList = new List<EcellLayer>();
            foreach (XmlNode node in layers.ChildNodes)
            {
                if (!LemlConstants.xPathLayer.Equals(node.Name))
                    continue;

                string name = GetStringAttribute(node, LemlConstants.xPathName);
                string visible = GetStringAttribute(node, LemlConstants.xPathVisible);
                elList.Add(new EcellLayer(name, bool.Parse(visible)));
            }
            model.Layers = elList;
        }

        /// <summary>
        /// Set the logger to XML node.
        /// </summary>
        /// <param name="env">ApplicationEnvironment</param>
        /// <param name="model">the model object.</param>
        /// <param name="loggers">Logger XML node.</param>
        private static void SetLogger(ApplicationEnvironment env, EcellModel model, XmlNode loggers)
        {
            if (loggers == null || loggers.ChildNodes.Count <= 0)
                return;

            foreach (XmlNode node in loggers.ChildNodes)
            {
                if (!node.Name.Equals(LemlConstants.xPathLogger))
                    continue;

                string modelID = GetStringAttribute(node, LemlConstants.xPathModelID);
                string key = GetStringAttribute(node, LemlConstants.xPathKey);
                string type = GetStringAttribute(node, LemlConstants.xPathType);
                EcellObject eo = GetEcellObject(model, type, key);

                if (eo == null)
                    continue;

                string fullPN = GetStringAttribute(node, LemlConstants.xpathFullPN);
                LoggerEntry entry = new LoggerEntry(modelID, key, type, fullPN);
                entry.Color = Color.FromName(GetStringAttribute(node, LemlConstants.xpathColor));
                entry.LineStyleInt = Int32.Parse(GetStringAttribute(node, LemlConstants.xpathLineStyle));
                entry.LineWidth = Int32.Parse(GetStringAttribute(node, LemlConstants.xpathLineWidth));
                entry.IsShowInt = Int32.Parse(GetStringAttribute(node, LemlConstants.xpathIsShown));
                entry.IsY2AxisInt = Int32.Parse(GetStringAttribute(node, LemlConstants.xpathIsY2));

                foreach (EcellData d in eo.Value)
                {
                    if (!d.EntityPath.Equals(fullPN))
                        continue;
                    d.Logged = true;
                    env.LoggerManager.AddLoggerEntry(entry);
                }
            }
        }

        /// <summary>
        /// Set the alias to XML node.
        /// </summary>
        /// <param name="model">the model object.</param>
        /// <param name="aliases">Alias XML node.</param>
        private static void SetAliases(EcellModel model, XmlNode aliases)
        {
            if (aliases == null || aliases.ChildNodes.Count <= 0)
                return;

            foreach (XmlNode node in aliases.ChildNodes)
            {
                if (!node.Name.Equals(LemlConstants.xPathAlias))
                    continue;

                string modelID = GetStringAttribute(node, LemlConstants.xPathModelID);
                string key = GetStringAttribute(node, LemlConstants.xPathKey);
                string x = GetStringAttribute(node, LemlConstants.xPathX);
                string y = GetStringAttribute(node, LemlConstants.xPathY);
                string layer = GetStringAttribute(node, LemlConstants.xPathLayer);
                EcellVariable variable = (EcellVariable)GetEcellObject(model, EcellObject.VARIABLE, key);
                if (variable == null)
                    continue;

                EcellLayout alias = new EcellLayout();
                alias.X = float.Parse(x);
                alias.Y = float.Parse(y);
                alias.Layer = layer;
                variable.Aliases.Add(alias);
            }
        }

        /// <summary>
        /// Set EcellObjects.
        /// </summary>
        /// <param name="model">the model object.</param>
        /// <param name="ecellObjects">object XML node.</param>
        private static void SetEcellObjects(EcellModel model, XmlNode ecellObjects)
        {
            if (ecellObjects == null || ecellObjects.ChildNodes.Count <= 0)
                return;

            foreach (XmlNode node in ecellObjects.ChildNodes)
            {
                if (!node.Name.Equals(LemlConstants.xPathEcellObject))
                    continue;

                string modelID = GetStringAttribute(node, LemlConstants.xPathModelID);
                string key = GetStringAttribute(node, LemlConstants.xPathKey);
                string type = GetStringAttribute(node, LemlConstants.xPathType);
                EcellObject eo = GetEcellObject(model, type, key);

                if(eo == null)
                    continue;

                eo.Classname = GetStringAttribute(node, LemlConstants.xPathClass);
                eo.X = GetFloatAttribute(node, LemlConstants.xPathX);
                eo.Y = GetFloatAttribute(node, LemlConstants.xPathY);
                eo.OffsetX = GetFloatAttribute(node, LemlConstants.xPathOffsetX);
                eo.OffsetY = GetFloatAttribute(node, LemlConstants.xPathOffsetY);
                eo.Width = GetFloatAttribute(node, LemlConstants.xPathWidth);
                eo.Height = GetFloatAttribute(node, LemlConstants.xPathHeight);
                eo.Layer = GetStringAttribute(node, LemlConstants.xPathLayer);
                eo.Layout.Figure  = GetStringAttribute(node, LemlConstants.xPathFigure);
                eo.isFixed = true;
            }
        }

        /// <summary>
        /// GetEcellObject
        /// </summary>
        /// <param name="model">The model object.</param>
        /// <param name="type">The type of object.</param>
        /// <param name="key">The key of object.</param>
        /// <returns>The object from XML node.</returns>
        private static EcellObject GetEcellObject(EcellModel model, string type, string key)
        {
            EcellObject eo = null;

            foreach (EcellObject sys in model.Children)
            {
                // Check
                if (sys.Type.Equals(type) && sys.Key.Equals(key))
                {
                    eo = sys;
                    break;
                }

                foreach (EcellObject child in sys.Children)
                {
                    // Check
                    if (child.Type.Equals(type) && child.Key.Equals(key))
                    {
                        eo = child;
                        break;
                    }
                }
            }

            return eo;
        }

        /// <summary>
        /// GetStringAttribute
        /// </summary>
        /// <param name="node">XML node.</param>
        /// <param name="key">The attribute string.</param>
        /// <returns>The attribute value string.</returns>
        public static string GetStringAttribute(XmlNode node, string key)
        {
            try
            {
                XmlAttribute attribute = node.Attributes[key];
                if (attribute == null)
                    return "";
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
        /// GetXMLAttributeFloat
        /// </summary>
        /// <param name="node">The xml node.</param>
        /// <param name="key">The attribute key.</param>
        /// <returns>the attribute value.</returns>
        public static float GetFloatAttribute(XmlNode node, string key)
        {
            string value = GetStringAttribute(node, key);
            try
            {
                return (float)Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return 0f;
            }
        }

        /// <summary>
        /// Set the plugin settings.
        /// </summary>
        /// <param name="env">the application environment.</param>
        /// <param name="settings">XMLNode.</param>
        private static void SetPluginSettings(ApplicationEnvironment env, XmlNode settings)
        {
            if (settings == null)
                return;
            foreach (XmlElement plugin in settings.ChildNodes)
            {
                env.PluginManager.SetPluginStatus(plugin.Name, plugin); 
            }
        }

        /// <summary>
        /// Save EcellObjects in LEML format.
        /// </summary>
        /// <param name="env">ApplicationEnvironment</param>
        /// <param name="model">The model object.</param>
        /// <param name="filename">the file name.</param>
        public static void SaveLEML(ApplicationEnvironment env, EcellModel model, string filename)
        {
            CheckFilePath(filename);

            FileStream fs = null;
            XmlTextWriter xmlOut = null;
            try
            {
                // Create xml file
                fs = new FileStream(filename, FileMode.Create);
                xmlOut = new XmlTextWriter(fs, Encoding.UTF8);

                // Use indenting for readability
                xmlOut.Formatting = Formatting.Indented;
                xmlOut.WriteStartDocument();

                // Always begin file with identification and warning
                xmlOut.WriteComment(LemlConstants.xPathFileHeader1);
                xmlOut.WriteComment(LemlConstants.xPathFileHeader2);

                // Application settings
                xmlOut.WriteStartElement(LemlConstants.xPathApplication);
                xmlOut.WriteAttributeString(LemlConstants.xPathName, Application.ProductName);
                xmlOut.WriteAttributeString(LemlConstants.xPathApplicationVersion, Application.ProductVersion);
                xmlOut.WriteAttributeString(LemlConstants.xPathConfigFileVersion, CONFIG_FILE_VERSION);

                // Layer settings
                xmlOut.WriteStartElement(LemlConstants.xPathLayerList);
                foreach (EcellLayer layer in model.Layers)
                {
                    xmlOut.WriteStartElement(LemlConstants.xPathLayer);
                    xmlOut.WriteAttributeString(LemlConstants.xPathName, layer.Name);
                    xmlOut.WriteAttributeString(LemlConstants.xPathVisible, layer.Visible.ToString());
                    xmlOut.WriteEndElement();
                }
                xmlOut.WriteEndElement();

                // Alias
                xmlOut.WriteStartElement(LemlConstants.xPathAliasList);
                foreach (EcellObject eo in model.Children)
                {
                    foreach (EcellObject child in eo.Children)
                    {
                        if (!(child is EcellVariable))
                            continue;
                        WriteAliases(xmlOut, child);
                    }
                }
                xmlOut.WriteEndElement();

                // Object settings
                xmlOut.WriteStartElement(LemlConstants.xPathEcellObjectList);
                foreach (EcellObject eo in model.Children)
                {
                    WriteObjectElement(xmlOut, eo);
                    foreach (EcellObject child in eo.Children)
                    {
                        WriteObjectElement(xmlOut, child);
                    }
                }
                xmlOut.WriteEndElement();

                // Logger
                xmlOut.WriteStartElement(LemlConstants.xPathLoggerList);
                foreach (string name in env.LoggerManager.GetLoggerList())
                {
                    LoggerEntry entry = env.LoggerManager.GetLoggerEntryForFullPN(name);
                    WriteLoggerElement(xmlOut, entry);
                }
                xmlOut.WriteEndElement();

                // Plugin settings.
                xmlOut.WriteStartElement(LemlConstants.xPathPluginSettings);
                foreach (XmlNode status in env.PluginManager.GetPluginStatus())
                {
                    status.WriteTo(xmlOut);
                }
                xmlOut.WriteEndElement();

                xmlOut.WriteEndElement();
                xmlOut.WriteEndDocument();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(string.Format(MessageResources.ErrSaveFile, filename) + Environment.NewLine + ex.Message);
            }
            finally
            {
                if (xmlOut != null) xmlOut.Close();
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// Write the alias information.
        /// </summary>
        /// <param name="xmlOut">Writer object.</param>
        /// <param name="eo">The alias object.</param>
        private static void WriteAliases(XmlTextWriter xmlOut, EcellObject eo)
        {
            EcellVariable var = (EcellVariable)eo;
            if (var.Aliases.Count <= 0)
                return;

            foreach (EcellLayout alias in var.Aliases)
            {
                xmlOut.WriteStartElement(LemlConstants.xPathAlias);
                xmlOut.WriteAttributeString(LemlConstants.xPathModelID, eo.ModelID);
                xmlOut.WriteAttributeString(LemlConstants.xPathKey, eo.Key);
                xmlOut.WriteAttributeString(LemlConstants.xPathX, alias.X.ToString());
                xmlOut.WriteAttributeString(LemlConstants.xPathY, alias.Y.ToString());
                xmlOut.WriteAttributeString(LemlConstants.xPathLayer, alias.Layer);
                xmlOut.WriteEndElement();
            }
        }
        /// <summary>
        /// WriteEcellObject
        /// </summary>
        /// <param name="xmlOut">Write object.</param>
        /// <param name="eo">the wrote object.</param>
        private static void WriteObjectElement(XmlTextWriter xmlOut, EcellObject eo)
        {
            xmlOut.WriteStartElement(LemlConstants.xPathEcellObject);
            xmlOut.WriteAttributeString(LemlConstants.xPathClass, eo.Classname);
            xmlOut.WriteAttributeString(LemlConstants.xPathModelID, eo.ModelID);
            xmlOut.WriteAttributeString(LemlConstants.xPathType, eo.Type);
            xmlOut.WriteAttributeString(LemlConstants.xPathKey, eo.Key);
            xmlOut.WriteAttributeString(LemlConstants.xPathLayer, eo.Layer);
            xmlOut.WriteAttributeString(LemlConstants.xPathFigure, eo.Layout.Figure);
            xmlOut.WriteAttributeString(LemlConstants.xPathX, eo.X.ToString());
            xmlOut.WriteAttributeString(LemlConstants.xPathY, eo.Y.ToString());
            xmlOut.WriteAttributeString(LemlConstants.xPathOffsetX, eo.OffsetX.ToString());
            xmlOut.WriteAttributeString(LemlConstants.xPathOffsetY, eo.OffsetY.ToString());
            xmlOut.WriteAttributeString(LemlConstants.xPathWidth, eo.Width.ToString());
            xmlOut.WriteAttributeString(LemlConstants.xPathHeight, eo.Height.ToString());
            xmlOut.WriteEndElement();
        }
        /// <summary>
        /// Write the logge elements.
        /// </summary>
        /// <param name="xmlOut">Xml writer object.</param>
        /// <param name="entry">The logger entry.</param>
        private static void WriteLoggerElement(XmlTextWriter xmlOut, LoggerEntry entry)
        {
            xmlOut.WriteStartElement(LemlConstants.xPathLogger);
            xmlOut.WriteAttributeString(LemlConstants.xPathModelID, entry.ModelID);
            xmlOut.WriteAttributeString(LemlConstants.xPathKey, entry.ID);
            xmlOut.WriteAttributeString(LemlConstants.xPathType, entry.Type);
            xmlOut.WriteAttributeString(LemlConstants.xpathFullPN, entry.FullPN);
            xmlOut.WriteAttributeString(LemlConstants.xpathColor, entry.Color.Name);
            xmlOut.WriteAttributeString(LemlConstants.xpathLineStyle, entry.LineStyleInt.ToString());
            xmlOut.WriteAttributeString(LemlConstants.xpathLineWidth, entry.LineWidth.ToString());
            xmlOut.WriteAttributeString(LemlConstants.xpathIsShown, entry.IsShowInt.ToString());
            xmlOut.WriteAttributeString(LemlConstants.xpathIsY2, entry.IsY2AxisInt.ToString());            
            xmlOut.WriteEndElement();
        }

        /// <summary>
        /// Check file path.
        /// </summary>
        private static void CheckFilePath(string filename)
        {
            string path = Path.GetDirectoryName(filename);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// LEML Constants class.
    /// </summary>
    internal class LemlConstants
    {
        /// <summary>
        /// Reserved string for Application.
        /// </summary>
        public const string xPathApplication = "Application";
        /// <summary>
        /// Reserved string for Version
        /// </summary>
        public const string xPathApplicationVersion = "Version";
        /// <summary>
        /// Reserved string for Layout configuration file.
        /// </summary>
        public const string xPathFileHeader1 = "Layout configuration file.";
        /// <summary>
        /// Reserved string for Automatically generated file. DO NOT modify!
        /// </summary>
        public const string xPathFileHeader2 = "Automatically generated file. DO NOT modify!";
        /// <summary>
        /// Reserved string for ConfigFileVersion
        /// </summary>
        public const string xPathConfigFileVersion = "ConfigFileVersion";
        /// <summary>
        /// Reserved string for CommentList
        /// </summary>
        public const string xPathCommentList = "CommentList";
        /// <summary>
        /// Reserved string for Comment
        /// </summary>
        public const string xPathComment = "Comment";
        /// <summary>
        /// Reserved string for LayerList
        /// </summary>
        public const string xPathLayerList = "LayerList";
        /// <summary>
        /// Reserved string for Layer
        /// </summary>
        public const string xPathLayer = "Layer";
        /// <summary>
        /// Reserved string for Figure
        /// </summary>
        public const string xPathFigure = "Figure";
        /// <summary>
        /// Reserved string for AliasList
        /// </summary>
        public const string xPathAliasList = "AliasList";
        /// <summary>
        /// Reserved string for Alias
        /// </summary>
        public const string xPathAlias = "Alias";
        /// <summary>
        /// Reserved string for PluginSettings
        /// </summary>
        public const string xPathPluginSettings = "PluginSettings";
        /// <summary>
        /// Reserved string for Visible
        /// </summary>
        public const string xPathVisible = "Visible";
        /// <summary>
        /// Reserved string for EcellObjectList
        /// </summary>
        public const string xPathEcellObjectList = "EcellObjectList";
        /// <summary>
        /// Reserved string for LoggerList
        /// </summary>
        public const string xPathLoggerList = "LoggerList";
        /// <summary>
        /// Reserved string for EcellObject
        /// </summary>
        public const string xPathEcellObject = "EcellObject";
        /// <summary>
        /// Reserved string for Logger
        /// </summary>
        public const string xPathLogger = "Logger";
        /// <summary>
        /// Reserved string for Name
        /// </summary>
        public const string xPathName = "Name";
        /// <summary>
        /// Reserved string for Type
        /// </summary>
        public const string xPathType = "Type";
        /// <summary>
        /// Reserved string for Class
        /// </summary>
        public const string xPathClass = "Class";
        /// <summary>
        /// Reserved string for ModelID
        /// </summary>
        public const string xPathModelID = "ModelID";
        /// <summary>
        /// Reserved string for Key
        /// </summary>
        public const string xPathKey = "Key";
        /// <summary>
        /// Reserved string for X
        /// </summary> 
        public const string xPathX = "X";
        /// <summary>
        /// Reserved string for Y
        /// </summary>
        public const string xPathY = "Y";
        /// <summary>
        /// Reserved string for OffsetX
        /// </summary>
        public const string xPathOffsetX = "OffsetX";
        /// <summary>
        /// Reserved string for OffsetY
        /// </summary>
        public const string xPathOffsetY = "OffsetY";
        /// <summary>
        /// Reserved string for Width
        /// </summary>
        public const string xPathWidth = "Width";
        /// <summary>
        /// Reserved string for Height
        /// </summary>
        public const string xPathHeight = "Height";

        #region Logger
        /// <summary>
        /// Reserved string for FullPN
        /// </summary>
        public const string xpathFullPN = "FullPN";
        /// <summary>
        /// Reserved string for Color
        /// </summary>
        public const string xpathColor = "Color";
        /// <summary>
        /// Reserved string for LineStyle
        /// </summary>
        public const string xpathLineStyle = "LineStyle";
        /// <summary>
        /// Reserved string for LineWidth
        /// </summary>
        public const string xpathLineWidth = "LineWidth";
        /// <summary>
        /// Reserved string for IsShown
        /// </summary>
        public const string xpathIsShown = "IsShown";
        /// <summary>
        /// Reserved string for IsY2
        /// </summary>
        public const string xpathIsY2 = "IsY2";
        #endregion
    }
}
