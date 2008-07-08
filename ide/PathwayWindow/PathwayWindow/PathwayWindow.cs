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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.PiccoloX.Components;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX.Nodes;
using Ecell;
using Ecell.Layout;
using Ecell.Plugin;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow
{
    /// <summary>
    /// PathwayWindow plugin
    /// </summary>
    public class PathwayWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// PathwayControl, which contains and controls all GUI-related objects.
        /// </summary>
        PathwayControl m_con;

        #endregion

        #region Initializer
        /// <summary>
        /// Initializes the plugin
        /// </summary>
        public override void Initialize()
        {
            m_con = new PathwayControl(this);
        }
        #endregion

        #region Methods to get EcellObject data.
        /// <summary>
        /// Method to get current EcellObject Datas.
        /// </summary>
        public List<EcellObject> GetData(string modelID)
        {
            return m_dManager.GetData(modelID, "/");
        }
        /// <summary>
        /// Method to get a EcellObject.
        /// </summary>
        /// <param name="modelID">the model of object.</param>
        /// <param name="key">the key of object.</param>
        /// <param name="type">the type of object.</param>
        /// <returns>the list of EcellObject.</returns>
        public EcellObject GetEcellObject(string modelID, string key, string type)
        {
            return m_dManager.GetEcellObject(modelID, key, type);
        }
        /// <summary>
        /// Method to get a EcellObject.
        /// </summary>
        /// <param name="eo">the EcellObject.</param>
        public EcellObject GetEcellObject(EcellObject eo)
        {
            return m_dManager.GetEcellObject(eo.ModelID, eo.Key, eo.Type);
        }
        /// <summary>
        /// Check layout algorithm's dlls in a plugin\pathway directory and register them
        /// to m_layoutList
        /// </summary>
        /// <returns></returns>
        public List<ILayoutAlgorithm> GetLayoutAlgorithms()
        {
            return m_pManager.GetLayoutPlugins();
        }
        /// <summary>
        /// Get a temporary key of EcellObject.
        /// </summary>
        /// <param name="modelID">The model ID of EcellObject.</param>
        /// <param name="type">The ID of parent system.</param>
        /// <param name="systemID">The system ID include this object.</param>
        /// <returns>"TemporaryID"</returns> 
        public string GetTemporaryID(string modelID, string type, string systemID)
        {
            return m_dManager.GetTemporaryID(modelID, type, systemID);
        }
        /// <summary>
        /// Get LEML file name.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <returns>LEML file name.</returns>
        public string GetLEMLFileName(string modelID)
        {
            string filepath = m_dManager.GetEMLPath(modelID);
            if (filepath == null)
                return null;
            return filepath.Replace(Constants.FileExtEML, Constants.FileExtLEML);
        }
        #endregion

        #region Inherited from IEcellPlugin
        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"PathwayWindow"</returns> 
        public override string GetPluginName()
        {
            return "PathwayWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override string GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Get menustrips for PathwayWindow plugin.
        /// </summary>
        /// <returns>the list of menu.</returns>
        public override List<ToolStripMenuItem> GetMenuStripItems()
        {
            return m_con.Menu.ToolMenuList;
        }

        /// <summary>
        /// Get toolbar buttons for PathwayWindow plugin.
        /// </summary>
        /// <returns>the list of ToolBarMenu.</returns>
        public override ToolStrip GetToolBarMenuStrip()
        {
            return m_con.Menu.ToolButtons;
        }

        /// <summary>
        /// Called by PluginManager for getting UseControl.
        /// UseControl for pathway is created and configurated in the PathwayView instance actually.
        /// PathwayWindow get it and attach some delegates to them and pass it to PluginManager.
        /// </summary>
        /// <returns>UserControl with pathway canvases, etc.</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return m_con.GetDockContents();
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true.</returns>
        public override IEnumerable<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            names.Add("Network of model.");
            return names;
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public override Bitmap Print(string name)
        {
            return m_con.Print();
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="status">System status.</param>
        public override void ChangeStatus(ProjectStatus status)
        {
            m_con.ChangeStatus(status);
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            m_con.Clear();
        }

        /// <summary>
        /// Called by PluginManager for newly added EcellObjects on the core.
        /// </summary>
        /// <param name="data">List of EcellObjects to be added</param>
        public override void DataAdd(List<EcellObject> data)
        {
            if (data == null || data.Count == 0)
                return;
            m_con.DataAdd(data);
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public override void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            // Null Check.
            if (String.IsNullOrEmpty(modelID) || String.IsNullOrEmpty(key) || String.IsNullOrEmpty(type))
                return;
            if (data == null)
                return;
            try
            {
                m_con.DataChanged(modelID, key, type, data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eo"></param>
        public override void SetPosition(EcellObject eo)
        {
            m_con.SetPosition(eo);
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            if (type == null)
                return;
            if (type.Equals(EcellObject.MODEL))
                this.Clear();
            m_con.DataDelete(modelID, key, type);
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public override void SaveModel(string modelID, string directory)
        {
            // Error Check
            if(string.IsNullOrEmpty(modelID) || string.IsNullOrEmpty(directory))
                return;
            m_con.SaveModel(modelID, directory);
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            m_con.SelectChanged(modelID, key, type);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public override void AddSelect(string modelID, string key, string type)
        {
            m_con.AddSelect(modelID, key, type);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public override void RemoveSelect(string modelID, string key, string type)
        {
            m_con.RemoveSelect(modelID, key, type);
        }

        /// <summary>
        ///  The event process when user reset select.
        /// </summary>
        public override void ResetSelect()
        {
            m_con.ResetSelect();
        }
        #endregion
    }
}
