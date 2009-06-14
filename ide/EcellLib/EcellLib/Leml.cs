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
    /// 
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
        /// <param name="model"></param>
        /// <param name="filename"></param>
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
        /// <param name="xml"></param>
        /// <param name="key"></param>
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
        /// Set Layers.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="layers"></param>
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
        /// <param name="model"></param>
        /// <param name="ecellObjects"></param>
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
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static EcellObject GetEcellObject(EcellModel model, string type, string key)
        {
            EcellObject eo = null;

            foreach (EcellObject sys in model.Children)
            {
                // Check
                if (sys.Type.Equals(type) && sys.Key.Equals(key))
                {
                    return sys;
                }

                foreach (EcellObject child in sys.Children)
                {
                    // Check
                    if (child.Type.Equals(type) && child.Key.Equals(key))
                    {
                        return child;
                    }
                }
            }

            return eo;
        }

        /// <summary>
        /// GetStringAttribute
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
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
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <param name="settings"></param>
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
        /// <param name="model"></param>
        /// <param name="filename"></param>
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
        /// <param name="xmlOut"></param>
        /// <param name="eo"></param>
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
        /// 
        /// </summary>
        /// <param name="xmlOut"></param>
        /// <param name="entry"></param>
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

    internal class LemlConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string xPathApplication = "Application";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathApplicationVersion = "Version";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFileHeader1 = "Layout configuration file.";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFileHeader2 = "Automatically generated file. DO NOT modify!";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathConfigFileVersion = "ConfigFileVersion";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathCommentList = "CommentList";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathComment = "Comment";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathLayerList = "LayerList";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathLayer = "Layer";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFigure = "Figure";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathAliasList = "AliasList";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathAlias = "Alias";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathPluginSettings = "PluginSettings";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathVisible = "Visible";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathEcellObjectList = "EcellObjectList";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathLoggerList = "LoggerList";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathEcellObject = "EcellObject";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathLogger = "Logger";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathName = "Name";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathType = "Type";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathClass = "Class";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathModelID = "ModelID";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathKey = "Key";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathX = "X";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathY = "Y";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathOffsetX = "OffsetX";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathOffsetY = "OffsetY";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathWidth = "Width";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathHeight = "Height";
        #region Logger
        public const string xpathFullPN = "FullPN";
        public const string xpathColor = "Color";
        public const string xpathLineStyle = "LineStyle";
        public const string xpathLineWidth = "LineWidth";
        public const string xpathIsShown = "IsShown";
        public const string xpathIsY2 = "IsY2";
        #endregion
    }
}
