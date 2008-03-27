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
using EcellLib.PathwayWindow.Resources;

namespace EcellLib.PathwayWindow {
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
        /// ResourceManager for PathwayWindow.
        /// </summary>
        private static ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        /// <summary>
        /// Save ECell window settings.
        /// </summary>
        public static void SaveAsXML(List<EcellObject> list, string filename)
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
                xmlOut.WriteComment("PathwayWindow configuration file.");
                xmlOut.WriteComment("Automatically generated file. DO NOT modify!");

                // Application settings
                xmlOut.WriteStartElement("Application");
                xmlOut.WriteAttributeString("Name", Application.ProductName);
                xmlOut.WriteAttributeString("Version", Application.ProductVersion);
                xmlOut.WriteAttributeString("ConfigFileVersion", CONFIG_FILE_VERSION);

                // Object settings
                xmlOut.WriteStartElement("EcellObjectList");
                foreach (EcellObject eo in list)
                {
                    xmlOut.WriteStartElement("EcellObject");
                    xmlOut.WriteAttributeString("Class", eo.Classname);
                    xmlOut.WriteAttributeString("ModelID", eo.ModelID);
                    xmlOut.WriteAttributeString("Type", eo.Type);
                    xmlOut.WriteAttributeString("Key", eo.Key);
                    xmlOut.WriteAttributeString("Layer", eo.LayerID);
                    xmlOut.WriteAttributeString("X", eo.X.ToString());
                    xmlOut.WriteAttributeString("Y", eo.Y.ToString());
                    xmlOut.WriteAttributeString("OffsetX", eo.OffsetX.ToString());
                    xmlOut.WriteAttributeString("OffsetY", eo.OffsetY.ToString());
                    xmlOut.WriteAttributeString("Width", eo.Width.ToString());
                    xmlOut.WriteAttributeString("Height", eo.Height.ToString());
                    xmlOut.WriteEndElement();
                }
                xmlOut.WriteEndElement();
                xmlOut.WriteEndElement();
                xmlOut.WriteEndDocument();
            }
            catch (Exception ex)
            {
                string errmsg = m_resources.GetString("ErrLoadWindowSettings") + Environment.NewLine + filename + Environment.NewLine + ex.Message;
                MessageBox.Show(errmsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //if (File.Exists(filename))
        }

        /// <summary>
        /// Load EcellObject position settings.
        /// </summary>
        public static List<EcellObject> LoadFromXML(string filename)
        {
            FileStream fs = null;
            XmlTextReader xmlIn = null;
            List<EcellObject> list = null;
            try
            {
                // Load XML file
                fs = new FileStream(filename, FileMode.Open);
                xmlIn = new XmlTextReader(fs);
                xmlIn.WhitespaceHandling = WhitespaceHandling.None;
                xmlIn.MoveToContent();
                // Check XML file
                while (!xmlIn.Name.Equals("Application"))
                {
                    if (!MoveToNextElement(xmlIn))
                        throw new ArgumentException();
                }
                // version check
                string formatVersion = xmlIn.GetAttribute("ConfigFileVersion");
                if (formatVersion == null || !IsFormatVersionValid(formatVersion))
                    throw new ArgumentException("Pathway setting file version error." + Environment.NewLine + "Current version is " + CONFIG_FILE_VERSION);

                // Load EcellObject
                MoveToNextElement(xmlIn);
                if (xmlIn.Name != "EcellObjectList")
                    throw new ArgumentException("No EcellObjects.");
                list = LoadEcellObjects(xmlIn);
            }
            catch (Exception ex)
            {
                string errmsg = m_resources.GetString("ErrLoadWindowSettings") + Environment.NewLine + filename + Environment.NewLine + ex.Message;
                MessageBox.Show(errmsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (xmlIn != null) xmlIn.Close();
                if (fs != null) fs.Close();
            }
            return list;
        }

        private static bool MoveToNextElement(XmlTextReader xmlIn)
        {
            if (!xmlIn.Read())
                return false;

            while (xmlIn.NodeType == XmlNodeType.EndElement)
            {
                if (!xmlIn.Read())
                    return false;
            }
            
            return true;
        }

        private static bool IsFormatVersionValid(string formatVersion)
        {
            if (formatVersion == CONFIG_FILE_VERSION)
                return true;

            return false;
        }

        private static List<EcellObject> LoadEcellObjects(XmlTextReader xmlIn)
        {
            List<EcellObject> list = new List<EcellObject>();
            while (MoveToNextElement(xmlIn) && xmlIn.Name == "EcellObject")
            {
                string modelID = GetXMLAttributeString(xmlIn, "ModelID");
                string key = GetXMLAttributeString(xmlIn, "Key");
                string type = GetXMLAttributeString(xmlIn, "Type");
                string classname = GetXMLAttributeString(xmlIn, "Class");
                EcellObject eo = EcellObject.CreateObject(modelID, key, type, classname, null);
                eo.LayerID = GetXMLAttributeString(xmlIn, "Layer");
                eo.X = GetXMLAttributeFloat(xmlIn, "X");
                eo.Y = GetXMLAttributeFloat(xmlIn, "Y");
                eo.OffsetX = GetXMLAttributeFloat(xmlIn, "OffsetX");
                eo.OffsetY = GetXMLAttributeFloat(xmlIn, "OffsetY");
                eo.Width = GetXMLAttributeFloat(xmlIn, "Width");
                eo.Height = GetXMLAttributeFloat(xmlIn, "Height");
                list.Add(eo);
            }
            return list;
        }

        private static string GetXMLAttributeString(XmlTextReader xmlIn, string key)
        {
            string value = xmlIn.GetAttribute(key);
            if (value == null)
                return "";
            else
                return value;
        }
        private static float GetXMLAttributeFloat(XmlTextReader xmlIn, string key)
        {
            string value = GetXMLAttributeString(xmlIn, key);
            try
            {
                return (float)Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return 0f;
            }
        }
    }
}
