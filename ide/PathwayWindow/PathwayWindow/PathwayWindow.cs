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
using EcellLib;
using EcellLib.PathwayWindow.Exceptions;
using EcellLib.PathwayWindow.Resources;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// PathwayWindow plugin
    /// </summary>
    public class PathwayWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// PathwayView, which contains and controls all GUI-related objects.
        /// </summary>
        PathwayControl m_con;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        private DataManager m_dManager;

        /// <summary>
        /// m_dManager (DataManager)
        /// </summary>
        private PluginManager m_pManager;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for default layout algorithm.
        /// </summary>
        public ILayoutAlgorithm DefaultLayoutAlgorithm
        {
            get { return new GridLayout(); }
        }

        /// <summary>
        /// Returns the DataManager instance associated to this plugin.
        /// </summary>
        public DataManager DataManager
        {
            get { return m_dManager; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// The constructor
        /// </summary>
        public PathwayWindow()
        {
            m_dManager = DataManager.GetDataManager();
            m_pManager = PluginManager.GetPluginManager();
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
            return m_dManager.GetEcellObject(modelID, key ,type);
        }
        #endregion

        #region Methods to notify change from inside (pathway) to outside(ECell Core)
        /// <summary>
        /// Inform the adding of EcellOBject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="list">the list of added object.</param>
        /// <param name="isAnchor">Whether this action is anchor or not.</param>
        public void NotifyDataAdd(List<EcellObject> list, bool isAnchor)
        {
            m_dManager.DataAdd(list, true, isAnchor);
        }

        /// <summary>
        /// Inform the adding of logger in PathwayEditor to PluginManager.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="entityPath"></param>
        public void NotifyLoggerAdd(string modelID, string key, string type, string entityPath)
        {
            m_pManager.LoggerAdd(
                modelID,
                key,
                type,
                entityPath);
        }

        /// <summary>
        /// Inform the changing of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="oldKey">the key of object before edit.</param>
        /// <param name="eo">The EcellObject changed the property.</param>
        /// <param name="isRecorded">Whether to record this change.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            EcellObject eo,
            bool isRecorded,
            bool isAnchor)
        {
            if(oldKey == null || eo.Type == null)
                return;
            m_dManager.DataChanged(eo.ModelID, oldKey, eo.Type, eo, isRecorded, isAnchor);
        }

        /// <summary>
        /// Inform the deleting of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="isAnchor"></param>
        public void NotifyDataDelete(string modelID, string key, string type, bool isAnchor)
        {
            m_dManager.DataDelete(modelID, key, type, true, isAnchor);
        }

        /// <summary>
        /// Inform the deleting of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        public void NotifyDataMerge(string modelID, string key)
        {
            try
            {
                m_dManager.SystemDeleteAndMove(modelID, key);
            }
            catch (Exception ex)
            {
                ex.ToString();
                String errmes = m_resources.GetString("ErrMerge");
                MessageBox.Show(errmes + "\n" + ex.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        /// <summary>
        /// Inform the selected EcellObject in PathwayEditor to PluginManager.
        /// </summary>
        /// <param name="modelID">the modelID of selected object.</param>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void NotifySelectChanged(string modelID, string key, string type)
        {
            m_pManager.SelectChanged(modelID, key, type);

        }
        /// <summary>
        /// Inform the selected EcellObject in PathwayEditor to PluginManager.
        /// </summary>
        /// <param name="modelID">the modelID of selected object.</param>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        /// <param name="isSelected">Is object is selected or not</param>
        public void NotifyAddSelect(string modelID, string key, string type, bool isSelected)
        {
            if (isSelected)
                m_pManager.AddSelect(modelID, key, type);
            else
                m_pManager.RemoveSelect(modelID, key, type);

        }
        /// <summary>
        /// Inform the plugin message.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void NotifyMessage(string type, string message)
        {
            m_pManager.Message(type, message);
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for PathwayWindow plugin.
        /// </summary>
        /// <returns>the list of menu.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            return m_con.MenuControl.ToolMenuList;
        }

        /// <summary>
        /// Get toolbar buttons for PathwayWindow plugin.
        /// </summary>
        /// <returns>the list of ToolBarMenu.</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return m_con.MenuControl.ToolButtonList;
        }

        /// <summary>
        /// Called by PluginManager for getting UseControl.
        /// UseControl for pathway is created and configurated in the PathwayView instance actually.
        /// PathwayWindow get it and attach some delegates to them and pass it to PluginManager.
        /// </summary>
        /// <returns>UserControl with pathway canvases, etc.</returns>
        public List<EcellDockContent> GetWindowsForms()
        {
            List<EcellDockContent> list = new List<EcellDockContent>();
            list.Add(m_con.PathwayView);
            list.Add(m_con.OverView);
            list.Add(m_con.LayerView);
            list.Add(m_con.ToolBox);
            return list;
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="status">System status.</param>
        public void ChangeStatus(ProjectStatus status)
        {
            m_con.ChangeStatus(status);
        }

        /// <summary>
        /// Change availability of undo/redo status
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            //if (!m_hasComponentSetting)
            //    return;

            try
            {
                m_con.Clear();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Called by PluginManager for newly added EcellObjects on the core.
        /// </summary>
        /// <param name="data">List of EcellObjects to be added</param>
        public void DataAdd(List<EcellObject> data)
        {
            if (data == null || data.Count == 0)
                return;
            // Check Model.
            string modelId = null;
            foreach (EcellObject eo in data)
            {
                if (eo.Type.Equals(EcellObject.MODEL))
                {
                    modelId = eo.ModelID;
                    break;
                }
            }
            // Load Model.
            try
            {
                bool layoutFlag = false;
                if (modelId != null)
                {
                    string fileName = m_dManager.GetDirPath(modelId) + "\\" + modelId + ".leml";
                    if (File.Exists(fileName))
                        this.SetPositionFromLeml(fileName, data);
                    else
                        layoutFlag = true;
                }
                this.NewDataAddToModel(data, layoutFlag, (modelId != null) );
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
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
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            if (type == null)
                return;
            if (type.Equals(EcellObject.MODEL))
                this.Clear();
            m_con.DataDelete(modelID, key, type);
        }

        /// <summary>
        /// The event sequence when the simulation parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID.</param>
        public void ParameterAdd(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is deleted.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterDelete(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is set.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterSet(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true.</returns>
        public List<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            names.Add("Network of model.");
            return names;
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false.</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="log">a list of LogData</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> log)
        {
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public void LoggerAdd(string modelID, string type, string key, string path)
        {
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print(string name)
        {
            if (m_con != null)
                return m_con.Print();
            else
                return new Bitmap(1,1);
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public void SaveModel(string modelID, string directory)
        {
            // Error Check
            if(String.IsNullOrEmpty(modelID) || String.IsNullOrEmpty(directory))
                return;
            if (!m_con.CanvasControl.ModelID.Equals(modelID))
                return;

            List<EcellObject> list = new List<EcellObject>();
            list.AddRange(m_con.GetSystemList(modelID));
            list.AddRange(m_con.GetNodeList(modelID));
            string fileName = directory + "\\" + modelID + ".leml";
            EcellSerializer.SaveAsXML(list, fileName);
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            m_con.SelectChanged(modelID, key, type);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            m_con.AddSelect(modelID, key, type);
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"PathwayWindow"</returns> 
        public string GetPluginName()
        {
            return "PathwayWindow";
        }
        /// <summary>
        /// Get a temporary key of EcellObject.
        /// </summary>
        /// <param name="modelID">The mode ID of EcellObject.</param>
        /// <param name="type">The ID of parent system.</param>
        /// <param name="systemID">The system ID include this object.</param>
        /// <returns>"TemporaryID"</returns> 
        public string GetTemporaryID(string modelID, string type, string systemID)
        {
            return m_dManager.GetTemporaryID(modelID, type, systemID);
        }

        /// <summary>
        /// Get version of this plugin.
        /// </summary>
        /// <returns>version</returns>
        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
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
        #endregion

        #region Internal use
        /// <summary>
        /// This method was made for dividing long and redundant DataAdd method.
        /// So, used by DataAdd only.
        /// </summary>
        /// <param name="data">The same argument for DataAdd</param>
        /// <param name="layoutFlag"></param>
        /// <param name="isFirst"></param>
        private void NewDataAddToModel(List<EcellObject> data, bool layoutFlag, bool isFirst)
        {
            // Load each EcellObject onto the canvas currently displayed
            foreach (EcellObject obj in data)
            {
                try
                {
                    m_con.DataAdd(obj, true, isFirst);
                    if (obj is EcellSystem)
                        foreach (EcellObject node in obj.Children)
                            m_con.DataAdd(node, true, isFirst);

                } catch (Exception ex)
                {
                    throw new PathwayException(m_resources.GetString("ErrUnknowType") + "\n" + ex.StackTrace);
                }
            }
            // Perform layout if layoutFlag is true.
            if(layoutFlag)
                m_con.DoLayout(DefaultLayoutAlgorithm, 0, false);
        }

        /// <summary>
        /// This method was made for dividing long and redundant DataAdd method.
        /// So, used by DataAdd only.
        /// </summary>
        /// <param name="fileName">Leml file path</param>
        /// <param name="data">The same argument for DataAdd</param>
        private void SetPositionFromLeml(string fileName, List<EcellObject> data)
        {
            // Deserialize objects from a file
            List<EcellObject> objList = EcellSerializer.LoadFromXML(fileName);

            // Create Object dictionary.
            Dictionary<string, EcellObject> objDict = new Dictionary<string, EcellObject>();
            foreach (EcellObject eo in objList)
                objDict.Add(eo.Type + ":" + eo.Key, eo);
            // Set position.
            string dictKey;
            foreach (EcellObject eo in data)
            {
                dictKey = eo.Type + ":" + eo.Key;
                if (!objDict.ContainsKey(dictKey))
                    continue;

                eo.SetPosition(objDict[dictKey]);
                if (!objDict[dictKey].LayerID.Equals(""))
                    eo.LayerID = objDict[dictKey].LayerID;

                if (eo.Children == null)
                    continue;
                foreach(EcellObject child in eo.Children)
                {
                    dictKey = child.Type + ":" + child.Key;
                    if (!objDict.ContainsKey(dictKey))
                        continue;

                    child.SetPosition(objDict[dictKey]);
                    if (!objDict[dictKey].LayerID.Equals(""))
                        child.LayerID = objDict[dictKey].LayerID;
                }
            }
        }
        #endregion
    }
}
