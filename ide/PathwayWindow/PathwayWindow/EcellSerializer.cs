//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;

namespace Ecell.IDE.Plugins.PathwayWindow {
    /// <summary>
    /// Class to serialize the object.
    /// </summary>
    public partial class EcellSerializer
    {

        /// <summary>
        /// Version of config file.
        /// </summary>
        private const string CONFIG_FILE_VERSION = "1.0";

        /// <summary>
        /// Save EcellObjects in LEML format.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="filename"></param>
        public static void SaveAsLEML(PathwayControl control, string filename)
        {
            CanvasControl canvas = control.Canvas;
            List<EcellObject> list = control.GetObjectList();

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
                xmlOut.WriteComment(PathwayConstants.xPathFileHeader1);
                xmlOut.WriteComment(PathwayConstants.xPathFileHeader2);

                // Application settings
                xmlOut.WriteStartElement(PathwayConstants.xPathApplication);
                xmlOut.WriteAttributeString(PathwayConstants.xPathName, Application.ProductName);
                xmlOut.WriteAttributeString(PathwayConstants.xPathApplicationVersion, Application.ProductVersion);
                xmlOut.WriteAttributeString(PathwayConstants.xPathConfigFileVersion, CONFIG_FILE_VERSION);

                // Layer settings
                xmlOut.WriteStartElement(PathwayConstants.xPathLayerList);
                foreach (PPathwayLayer layer in canvas.Layers.Values)
                {
                    xmlOut.WriteStartElement(PathwayConstants.xPathLayer);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathName, layer.Name);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathVisible, layer.Visible.ToString());
                    xmlOut.WriteEndElement();
                }
                xmlOut.WriteEndElement();

                // Object settings
                xmlOut.WriteStartElement(PathwayConstants.xPathEcellObjectList);
                foreach (EcellObject eo in list)
                {
                    xmlOut.WriteStartElement(PathwayConstants.xPathEcellObject);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathClass, eo.Classname);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathModelID, eo.ModelID);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathType, eo.Type);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathKey, eo.Key);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathLayer, eo.Layer);
                    xmlOut.WriteAttributeString(PathwayConstants.xPathX, eo.X.ToString());
                    xmlOut.WriteAttributeString(PathwayConstants.xPathY, eo.Y.ToString());
                    xmlOut.WriteAttributeString(PathwayConstants.xPathOffsetX, eo.OffsetX.ToString());
                    xmlOut.WriteAttributeString(PathwayConstants.xPathOffsetY, eo.OffsetY.ToString());
                    xmlOut.WriteAttributeString(PathwayConstants.xPathWidth, eo.Width.ToString());
                    xmlOut.WriteAttributeString(PathwayConstants.xPathHeight, eo.Height.ToString());
                    xmlOut.WriteEndElement();
                }
                xmlOut.WriteEndElement();

                xmlOut.WriteEndElement();
                xmlOut.WriteEndDocument();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(MessageResources.ErrSaveLEML + Environment.NewLine + filename + Environment.NewLine + ex.Message);
            }
            finally
            {
                if (xmlOut != null) xmlOut.Close();
                if (fs != null) fs.Close();
            }
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

        /// <summary>
        /// LoadFromLEML
        /// </summary>
        /// <param name="control"></param>
        /// <param name="filename"></param>
        public static void LoadFromLEML(PathwayControl control, string filename)
        {
            // Set CanvasControl
            CanvasControl canvas = control.Canvas;
            if(canvas == null)
                return;

            XmlDocument xmlD = new XmlDocument();
            try
            {
                xmlD.Load(filename);
                XmlNode applicationData = GetNodeByKey(xmlD, PathwayConstants.xPathApplication);
                // Load EcellObjects
                XmlNode ecellObjects = GetNodeByKey(applicationData, PathwayConstants.xPathEcellObjectList);
                SetEcellObjects(canvas, ecellObjects);
                // Load Layers
                XmlNode layers = GetNodeByKey(applicationData, PathwayConstants.xPathLayerList);
                SetLayers(canvas, layers);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(MessageResources.ErrLoadLEML + Environment.NewLine + filename + Environment.NewLine + ex.Message);
            }
        }

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
        /// Set Layers.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="layers"></param>
        private static void SetLayers(CanvasControl canvas, XmlNode layers)
        {
            if (layers == null || layers.ChildNodes.Count <= 0)
                return;

            foreach (XmlNode node in layers.ChildNodes)
            {
                if (!PathwayConstants.xPathLayer.Equals(node.Name))
                    continue;

                string name = GetStringAttribute(node, PathwayConstants.xPathName);
                string visible = GetStringAttribute(node, PathwayConstants.xPathVisible);
                if (canvas.Layers.ContainsKey(name))
                    canvas.LayerMoveToFront(name);
                else
                    canvas.AddLayer(name);
                canvas.ChangeLayerVisibility(name, bool.Parse(visible));
            }
            canvas.RaiseLayerChange();
        }

        /// <summary>
        /// Set EcellObjects.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="ecellObjects"></param>
        private static void SetEcellObjects(CanvasControl canvas, XmlNode ecellObjects)
        {            
            if (ecellObjects == null || ecellObjects.ChildNodes.Count <= 0)
                return;

            string mes = MessageResources.MessageLoadModel;
            canvas.Control.Progress(mes, 100, 50);
            int allcount = ecellObjects.ChildNodes.Count;
            int count = 0;
            foreach (XmlNode node in ecellObjects.ChildNodes)
            {
                if (!node.Name.Equals(PathwayConstants.xPathEcellObject))
                {
                    count++;
                    canvas.Control.Progress(mes, 100, count * 50 / allcount + 50);
                    continue;
                }

                string modelID = GetStringAttribute(node, PathwayConstants.xPathModelID);
                string key = GetStringAttribute(node, PathwayConstants.xPathKey);
                string type = GetStringAttribute(node, PathwayConstants.xPathType);
                string classname = GetStringAttribute(node, PathwayConstants.xPathClass);
                SetPPathwayObject(canvas, node, key, type);
                canvas.Control.Progress(mes, 100, count * 50 / allcount + 50);
                count++;
            }
            canvas.Control.Progress(mes, 100, 100);
        }
        /// <summary>
        /// Set PPathwayObject
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        private static void SetPPathwayObject(CanvasControl canvas, XmlNode node, string key, string type)
        {
            PPathwayObject obj = canvas.GetSelectedObject(key, type);
            if (obj == null)
                return;

            EcellObject eo = obj.EcellObject;
            eo.Layer = GetStringAttribute(node, PathwayConstants.xPathLayer);
            eo.X = GetFloatAttribute(node, PathwayConstants.xPathX);
            eo.Y = GetFloatAttribute(node, PathwayConstants.xPathY);
            eo.OffsetX = GetFloatAttribute(node, PathwayConstants.xPathOffsetX);
            eo.OffsetY = GetFloatAttribute(node, PathwayConstants.xPathOffsetY);
            eo.Width = GetFloatAttribute(node, PathwayConstants.xPathWidth);
            eo.Height = GetFloatAttribute(node, PathwayConstants.xPathHeight);

            canvas.Control.NotifySetPosition(eo);
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
        /// GetXMLAttributeFloat
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static float GetFloatAttribute(XmlNode node, string key)
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
    }
}
