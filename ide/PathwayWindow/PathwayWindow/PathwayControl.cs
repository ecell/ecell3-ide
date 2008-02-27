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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.IO;
using System.ComponentModel;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.Piccolo.Util;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Handler;
using EcellLib.PathwayWindow.Resources;
using EcellLib.PathwayWindow.Exceptions;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// PathwayView plays a role of View part of MVC-model.
    /// </summary>
    public class PathwayControl
    {
        #region Fields
        /// <summary>
        /// PathwayWindow.
        /// </summary>
        private PathwayWindow m_window;

        /// <summary>
        /// ComponentSettingsManager for creating Systems and Nodes
        /// </summary>
        private ComponentManager m_csManager;

        /// <summary>
        /// MenuControl for PathwayWindow.
        /// </summary>
        private MenuControl m_menuCon;

        /// <summary>
        /// AnimationControl for simulation.
        /// </summary>
        private AnimationControl m_animCon;

        /// <summary>
        /// OverView interface.
        /// </summary>
        private PathwayView m_pathwayView;
        /// <summary>
        /// OverView interface.
        /// </summary>
        private OverView m_overView;
        /// <summary>
        /// LayerView interface.
        /// </summary>
        private LayerView m_layerView;
        /// <summary>
        /// ToolBox interface.
        /// </summary>
        private ToolBox m_toolBox;

        /// <summary>
        /// Default Layout Algorithm.
        /// </summary>
        private ILayoutAlgorithm m_defAlgorithm = new GridLayout();

        /// <summary>
        /// A list for layout algorithms, which implement ILayoutAlgorithm.
        /// </summary>
        private List<ILayoutAlgorithm> m_layoutList = new List<ILayoutAlgorithm>();

        /// <summary>
        /// Indicate which pathway-related toolbar button is selected.
        /// </summary>
        private Handle m_selectedHandle;

        /// <summary>
        /// ProjectStatus
        /// </summary>
        private ProjectStatus m_status = ProjectStatus.Uninitialized;

        /// <summary>
        /// Dictionary for canvases.
        ///  key: canvas ID
        ///  value: CanvasViewComponentSet
        /// </summary>
        private CanvasControl m_canvas;

        /// <summary>
        /// List of PPathwayNode for copied object.
        /// </summary>
        private List<EcellObject> m_copiedNodes = new List<EcellObject>();

        /// <summary>
        /// Point of mouse cursor.
        /// </summary>
        private PointF m_mousePos;

        /// <summary>
        /// Point of mouse cursor.
        /// </summary>
        private PointF m_copyPos;

        /// <summary>
        /// Whether each node is showing it's ID or not;
        /// </summary>
        private bool m_showingId = true;

        /// <summary>
        /// Whether each node is showing it's ID or not;
        /// </summary>
        private bool m_isViewMode = false;

        /// <summary>
        /// Whether PathwayView is freezed or not.
        /// </summary>
        private bool m_isFreezed = false;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources;

        #region Accessors
        /// <summary>
        ///  get/set PathwayWindow related this object.
        /// </summary>
        public PathwayWindow Window
        {
            get { return this.m_window; }
            set { this.m_window = value; }
        }

        #region GUI
        /// <summary>
        /// Accessor for m_overviewCanvas.
        /// </summary>
        public PathwayView PathwayView
        {
            get { return m_pathwayView; }
        }
        /// <summary>
        /// Accessor for m_overView.
        /// </summary>
        public OverView OverView
        {
            get { return m_overView; }
        }
        /// <summary>
        /// Accessor for m_layerView.
        /// </summary>
        public LayerView LayerView
        {
            get { return m_layerView; }
        }
        /// <summary>
        /// Accessor for m_layerView.
        /// </summary>
        public ToolBox ToolBox
        {
            get { return m_toolBox; }
        }
        #endregion

        /// <summary>
        /// get/set the CoponentSettingManager.
        /// </summary>
        public ComponentManager ComponentManager
        {
            get { return this.m_csManager; }
            set { this.m_csManager = value; }
        }

        /// <summary>
        /// get/set the MenuControl.
        /// </summary>
        public MenuControl MenuControl
        {
            get { return m_menuCon; }
            set { m_menuCon = value; }
        }

        /// <summary>
        /// get/set the AnimationControl.
        /// </summary>
        public AnimationControl AnimationControl
        {
            get { return m_animCon; }
            set { m_animCon = value; }
        }

        /// <summary>
        /// MessageResourses
        /// </summary>
        public ComponentResourceManager Resources
        {
            get { return m_resources; }
            set { m_resources = value; }
        }
        #endregion

        /// <summary>
        /// Accessor for currently active canvas.
        /// </summary>
        public CanvasControl CanvasControl
        {
            get { return m_canvas; }
            set
            {
                m_canvas = value;
                RaiseCanvasChange();
                if (m_canvas == null)
                    return;
                m_canvas.PathwayCanvas.AddInputEventListener(m_selectedHandle.EventHandler);
            }
        }

        /// <summary>
        /// LayoutList
        /// </summary>
        public List<ILayoutAlgorithm> LayoutList
        {
            get { return m_layoutList; }
            set { m_layoutList = value; }
        }

        /// <summary>
        /// get/set the number of checked component.
        /// </summary>
        public Handle SelectedHandle
        {
            get { return m_selectedHandle; }
            set { m_selectedHandle = value; }
        }

        /// <summary>
        /// get/set the flag of showing id.
        /// </summary>
        public bool ShowingID
        {
            get { return m_showingId; }
            set
            {
                m_showingId = value;
                if (m_canvas == null)
                    return;
                m_canvas.ShowingID = m_showingId;
            }
        }
        /// <summary>
        /// get/set the flag of showing id.
        /// </summary>
        public bool ViewMode
        {
            get { return m_isViewMode; }
            set
            {
                m_isViewMode = value;
                if (m_canvas == null)
                    return;
                m_canvas.ViewMode = m_isViewMode;
                if (m_isViewMode)
                {
                    if (m_status == ProjectStatus.Running)
                        m_animCon.StartSimulation();
                    else
                        m_animCon.SetPropForSimulation();
                }
                else
                {
                    m_animCon.ResetPropForSimulation();
                }
            }
        }

        /// <summary>
        /// get/set the number of checked component.
        /// </summary>
        public PointF MousePosition
        {
            get { return m_mousePos; }
            set { m_mousePos = value; }
        }
        /// <summary>
        /// Accessor for m_copiedNodes.
        /// </summary>
        public List<EcellObject> CopiedNodes
        {
            get { return m_copiedNodes; }
            set { m_copiedNodes = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// the constructor for PathwayView.
        /// set the handler of event and user control.
        /// </summary>
        public PathwayControl(PathwayWindow window)
        {
            this.m_window = window;
            // Create Internal object.
            m_canvas = null;
            m_resources = new ComponentResourceManager(typeof(MessageResPathway));
            m_csManager = new ComponentManager();
            SetNodeIcons();
            m_layoutList = m_window.GetLayoutAlgorithms();
            // Create menus
            m_menuCon = new MenuControl(this);
            // Set AnimationControl
            m_animCon = new AnimationControl(this);
            // Preparing Interfaces
            m_pathwayView = new PathwayView(this);
            m_overView = new OverView(this);
            m_layerView = new LayerView(this);
            m_toolBox = new ToolBox(this);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add new objects to this canvas.
        /// </summary>
        /// <param name="data"></param>
        public void DataAdd(List<EcellObject> data)
        {
            // Load Model.
            try
            {
                // Check New Model.
                string modelId = CheckNewModel(data);
                // Load layout information from LEML.
                bool isFirst = (modelId != null);
                bool layoutFlag = false;
                if (isFirst)
                {
                    string fileName = m_window.GetLEMLFileName(modelId);
                    if (File.Exists(fileName))
                        this.LoadFromLeml(fileName, data);
                    else
                        layoutFlag = true;
                }
                // Load each EcellObject onto the canvas.
                foreach (EcellObject obj in data)
                {
                    DataAdd(obj, true, isFirst);
                    if (!(obj is EcellSystem))
                        continue;
                    foreach (EcellObject node in obj.Children)
                        DataAdd(node, true, isFirst);
                }
                // Perform layout if layoutFlag is true.
                if (layoutFlag)
                    DoLayout(m_defAlgorithm, 0, false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new PathwayException(m_resources.GetString("ErrUnknowType") + "\n" + e.StackTrace);
            }
        }
        /// <summary>
        /// Check new model.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string CheckNewModel(List<EcellObject> data)
        {
            string modelId = null;
            foreach (EcellObject eo in data)
            {
                if (eo.Type.Equals(EcellObject.MODEL))
                {
                    modelId = eo.ModelID;
                    break;
                }
            }
            return modelId;
        }

        /// <summary>
        /// This method was made for dividing long and redundant DataAdd method.
        /// So, used by DataAdd only.
        /// </summary>
        /// <param name="fileName">Leml file path</param>
        /// <param name="data">The same argument for DataAdd</param>
        private void LoadFromLeml(string fileName, List<EcellObject> data)
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
                foreach (EcellObject child in eo.Children)
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

        /// <summary>
        /// Add new object to this canvas.
        /// </summary>
        /// <param name="eo">EcellObject</param>
        /// <param name="isAnchor">True is default. If undo unit contains multiple actions,
        /// only the last action's isAnchor is true, the others' isAnchor is false</param>
        /// <param name="isFirst"></param>
        public void DataAdd(EcellObject eo,
            bool isAnchor,
            bool isFirst)
        {
            // Null check.
            if (eo == null)
                throw new PathwayException(m_resources.GetString("ErrAddObjNot"));
            // Load new project
            if (EcellObject.PROJECT.Equals(eo.Type))
            {
                this.Clear();
                return;
            }
            // create new canvas
            if (eo.Type.Equals(EcellObject.MODEL))
            {
                this.CreateCanvas(eo.ModelID);
                return;
            }
            // Error check.
            if (string.IsNullOrEmpty(eo.Key))
                throw new PathwayException(m_resources.GetString("ErrKeyNot"));
            if (string.IsNullOrEmpty(eo.ModelID) || !m_canvas.ModelID.Equals(eo.ModelID))
                throw new PathwayException(m_resources.GetString("ErrNotSetCanvas") + eo.Key);
            if (eo.Key.EndsWith(":SIZE"))
                return;

            // Create PathwayObject and set to canvas.
            ComponentType cType = ComponentManager.ParseStringToComponentType(eo.Type);
            ComponentSetting cs = m_csManager.GetDefaultComponentSetting(cType);
            PPathwayObject obj = cs.CreateNewComponent(eo);
            m_canvas.DataAdd(eo.ParentSystemID, obj, eo.IsPosSet, isFirst);
            NotifyDataChanged(eo.Key, eo.Key, obj, !isFirst, false);
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="oldKey">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="eo">Changed value of object.</param>
        public void DataChanged(string modelID, string oldKey, string type, EcellObject eo)
        {
            // Select Canvas
            if (m_canvas == null)
                return;
            // If case SystemSize
            if (oldKey.EndsWith(":SIZE"))
                return;
            // Select changed object.
            PPathwayObject obj = m_canvas.GetSelectedObject(oldKey, type);
            if (obj == null)
                return;

            // Change data.
            obj.ViewMode = false;
            obj.EcellObject = eo;
            if (obj is PPathwaySystem)
            {
                obj.Width = eo.Width;
                obj.Height = eo.Height;
            }
            obj.ViewMode = m_isViewMode;
            obj.Refresh();
            m_canvas.DataChanged(oldKey, eo.Key, obj);
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            if (m_canvas == null)
                return;
            // If case SystemSize
            if (key.EndsWith(":SIZE"))
                return;

            // Delete object.
            m_canvas.DataDelete(key, type);
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public void SaveModel(string modelID, string directory)
        {
            if (!CanvasControl.ModelID.Equals(modelID))
                return;

            List<EcellObject> list = new List<EcellObject>();
            list.AddRange(GetSystemList());
            list.AddRange(GetNodeList());
            string fileName = directory + "\\" + modelID + ".leml";
            EcellSerializer.SaveAsXML(list, fileName);
        }

        /// <summary>
        /// Clears the contents of the pathway window, and Returns it to an
        /// initial state.
        /// </summary>
        public void Clear()
        {
            // Reset Interfaces
            m_animCon.StopSimulation();
            // Clear Canvas dictionary.
            if (m_canvas != null)
                m_canvas.Dispose();
            CanvasControl = null;
        }

        /// <summary>
        /// get bitmap image of this pathway.
        /// </summary>
        /// <returns>Bitmap of this pathway.</returns>
        public Bitmap Print()
        {
            if (m_canvas == null)
                return new Bitmap(1, 1);

            return m_canvas.ToImage();
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            // Error check.
            if (modelID == null || m_canvas == null)
                return;
            if (!m_canvas.ModelID.Equals(modelID))
                return;
            m_canvas.SelectChanged(key, type);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // Error check.
            if (modelID == null || m_canvas == null)
                return;
            if (!m_canvas.ModelID.Equals(modelID))
                return;
            m_canvas.AddSelect(key, type);
        }

        /// <summary>
        ///  When change project status, change menu enable/disable.
        /// </summary>
        /// <param name="status">System status.</param>
        public void ChangeStatus(ProjectStatus status)
        {
            m_status = status;
            // When a project is loaded or unloaded.
            if (status == ProjectStatus.Loaded)
            {
                foreach (ToolStripMenuItem item in m_menuCon.LayoutMenus)
                    item.Enabled = true;
            }
            else if (status == ProjectStatus.Uninitialized)
            {
                foreach (ToolStripMenuItem item in m_menuCon.LayoutMenus)
                    item.Enabled = false;
            }
            // When simulation started.
            if (status == ProjectStatus.Running && m_isViewMode)
            {
                m_animCon.StartSimulation();
            }
            else if (status == ProjectStatus.Stepping && m_isViewMode)
            {
                m_animCon.StepSimulation();
            }
            else if (status == ProjectStatus.Suspended)
            {
                m_animCon.PauseSimulation();
            }
            else
            {
                m_animCon.StopSimulation();
            }
        }

        /// <summary>
        /// Set EventHandler.
        /// </summary>
        /// <param name="handle"></param>
        public void SetEventHandler(Handle handle)
        {
            // Remove old EventHandler
            PBasicInputEventHandler handler = m_selectedHandle.EventHandler;
            if (handler is PPathwayInputEventHandler)
                ((PPathwayInputEventHandler)handler).Reset();
            RemoveInputEventListener(handler);

            // Set new EventHandler 
            m_selectedHandle = handle;
            handler = m_selectedHandle.EventHandler;
            foreach (ToolStripItem item in m_menuCon.ToolButtonList)
            {
                if (!(item is PathwayToolStripButton))
                    continue;
                PathwayToolStripButton button = (PathwayToolStripButton)item;
                if (button.Handle == m_selectedHandle)
                    button.Checked = true;
                else
                    button.Checked = false;
            }
            if (handler is PPathwayInputEventHandler)
                ((PPathwayInputEventHandler)handler).Initialize();
            AddInputEventListener(handler);

            if (m_selectedHandle.Mode == Mode.Pan)
            {
                m_pathwayView.Cursor = new Cursor(new MemoryStream(PathwayResource.move));
                Freeze();
            }
            else
            {
                m_pathwayView.Cursor = Cursors.Arrow;
                Unfreeze();
            }
            if (CanvasControl == null)
                return;
            CanvasControl.ResetNodeToBeConnected();
            CanvasControl.LineHandler.SetLineVisibility(false);
        }
        #endregion

        #region Methods to notify changes to Interface(PathwayWindow)
        /// <summary>
        /// Notify DataAdd event to outside.
        /// </summary>
        /// <param name="eo">Added EcellObject</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void NotifyDataAdd(EcellObject eo, bool isAnchor)
        {
            List<EcellObject> list = new List<EcellObject>();
            list.Add(eo);
            if (m_window != null)
                m_window.NotifyDataAdd(list, isAnchor);
        }
        
        /// <summary>
        /// Notify DataChanged event to outside (PathwayView -> PathwayWindow -> DataManager)
        /// To notify position or size change.
        /// </summary>
        /// <param name="oldKey">the key before adding.</param>
        /// <param name="eo">Changed EcellObject.</param>
        /// <param name="isRecorded">Whether to record this change.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            EcellObject eo,
            bool isRecorded,
            bool isAnchor)
        {
            try
            {
                m_window.NotifyDataChanged(oldKey, eo, isRecorded, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                DataChanged(eo.ModelID, oldKey, eo.Type, eo);
                if (m_isViewMode && m_status == ProjectStatus.Running)
                    m_animCon.UpdatePropForSimulation();
            }

        }
        /// <summary>
        /// Notify DataChanged event to outside (PathwayView -> PathwayWindow -> DataManager)
        /// To notify position or size change.
        /// </summary>
        /// <param name="oldKey">the key before adding.</param>
        /// <param name="newKey">the key after adding.</param>
        /// <param name="obj">x coordinate of object.</param>
        /// <param name="isRecorded">Whether to record this change.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            string newKey,
            PPathwayObject obj,
            bool isRecorded,
            bool isAnchor)
        {
            obj.ViewMode = false;
            EcellObject eo = m_window.GetEcellObject(obj.EcellObject.ModelID, oldKey, obj.EcellObject.Type);
            if (eo == null)
                throw new Exception();

            eo.Key = newKey;
            eo.LayerID = obj.Layer.Name;
            eo.X = obj.X;
            eo.Y = obj.Y;
            eo.Width = obj.Width;
            eo.Height = obj.Height;
            eo.OffsetX = obj.OffsetX;
            eo.OffsetY = obj.OffsetY;

            obj.ViewMode = m_isViewMode;
            try
            {
                NotifyDataChanged(oldKey, eo, isRecorded, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                DataChanged(eo.ModelID, oldKey, eo.Type, obj.EcellObject);
                if (m_isViewMode)
                    m_animCon.UpdatePropForSimulation();
            }
        }

        /// <summary>
        /// Notify DataDelete event to outsite.
        /// </summary>
        /// <param name="eo">the deleted object.</param>
        /// <param name="isAnchor">the type of deleted object.</param>
        public void NotifyDataDelete(EcellObject eo, bool isAnchor)
        {
            try
            {
                m_window.NotifyDataDelete(eo.ModelID, eo.Key, eo.Type, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Notify logger change.
        /// </summary>
        /// <param name="obj">The object to change the logger property.</param>
        /// <param name="logger">The logger entity.</param>
        /// <param name="isLogger">The flag whether the entity is logged.</param>
        private void NotifyLoggerChanged(PPathwayObject obj, string logger, bool isLogger)
        {
            EcellObject eo = m_window.GetEcellObject(obj.EcellObject.ModelID, obj.EcellObject.Key, obj.EcellObject.Type);

            // set logger
            foreach (EcellData d in eo.Value)
            {
                if (!logger.Equals(d.Name))
                    continue;
                // Set isLogger
                d.Logged = isLogger;

                // If isLogger, 
                if (!isLogger)
                    continue;
                m_window.NotifyLoggerAdd(
                    eo.ModelID,
                    eo.Key,
                    eo.Type,
                    d.EntityPath);
            }
            NotifyDataChanged(eo.Key, eo, true, true);
        }

        /// <summary>
        /// Inform the changing of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="proKey">key of process</param>
        /// <param name="varKey">key of variable</param>
        /// <param name="changeType">type of change</param>
        /// <param name="coefficient">coefficient of VariableReference</param>
        /// <param name="isAnchor">is anchored or not.</param>
        public void NotifyVariableReferenceChanged(string proKey, string varKey, RefChangeType changeType, int coefficient, bool isAnchor)
        {
            // Get EcellObject of identified process.
            EcellProcess ep = (EcellProcess)m_window.GetEcellObject(CanvasControl.ModelID, proKey, EcellObject.PROCESS);
            // End if obj is null.
            if (null == ep)
                return;
            // Get EcellReference List.
            List<EcellReference> refList = ep.ReferenceList;
            List<EcellReference> newList = new List<EcellReference>();
            EcellReference changedRef = null;

            foreach (EcellReference v in refList)
            {
                if (v.FullID.EndsWith(varKey))
                    changedRef = v;
                else
                    newList.Add(v);
            }

            if (changedRef != null && changeType != RefChangeType.Delete)
            {
                switch(changeType)
                {
                    case RefChangeType.SingleDir:
                        changedRef.Coefficient = coefficient;
                        changedRef.Name = PathUtil.GetNewReferenceName(newList, coefficient);
                        newList.Add(changedRef);
                        break;
                    case RefChangeType.BiDir:
                        EcellReference copyRef = changedRef.Copy();
                        changedRef.Coefficient = -1;
                        changedRef.Name = PathUtil.GetNewReferenceName(newList, -1);
                        copyRef.Coefficient = 1;
                        copyRef.Name = PathUtil.GetNewReferenceName(newList, 1);
                        newList.Add(changedRef);
                        newList.Add(copyRef);
                        break;
                }
            }
            else if(changedRef == null)
            {
                switch(changeType)
                {
                    case RefChangeType.SingleDir:
                        EcellReference addRef = new EcellReference();
                        addRef.Coefficient = coefficient;
                        addRef.Key = varKey;
                        addRef.Name = PathUtil.GetNewReferenceName(newList, coefficient);
                        addRef.IsAccessor = 1;
                        newList.Add(addRef);
                        break;
                    case RefChangeType.BiDir:
                        EcellReference addSRef = new EcellReference();
                        addSRef.Coefficient = -1;
                        addSRef.Key = varKey;
                        addSRef.Name = PathUtil.GetNewReferenceName(newList, -1);
                        addSRef.IsAccessor = 1;
                        newList.Add(addSRef);

                        EcellReference addPRef = new EcellReference();
                        addPRef.Coefficient = 1;
                        addPRef.Key = varKey;
                        addPRef.Name = PathUtil.GetNewReferenceName(newList, 1);
                        addPRef.IsAccessor = 1;
                        newList.Add(addPRef);
                        break;
                }
            }
            ep.ReferenceList = newList;
            NotifyDataChanged(ep.Key, ep, true, isAnchor);
        }

        /// <summary>
        /// Notify DataDelete event to outsite.
        /// </summary>
        /// <param name="obj">the deleted object.</param>
        /// <param name="isAnchor">the type of deleted object.</param>
        public void NotifyDataDelete(PPathwayObject obj, bool isAnchor)
        {
            NotifyDataDelete(obj.EcellObject, isAnchor);
        }
        #endregion

        #region EventHandler for CanvasChange
        private EventHandler m_onCanvasChange;
        /// <summary>
        /// Event on canvas change.
        /// </summary>
        public event EventHandler CanvasChange
        {
            add { m_onCanvasChange += value; }
            remove { m_onCanvasChange -= value; }
        }
        /// <summary>
        /// Event on canvas change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCanvasChange(EventArgs e)
        {
            if (m_onCanvasChange != null)
                m_onCanvasChange(this, e);
        }
        private void RaiseCanvasChange()
        {
            EventArgs e = new EventArgs();
            OnCanvasChange(e);
        }
        #endregion

        #region EventHandlers for MenuClick
#if DEBUG
        /// <summary>
        /// Called when a debug menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DebugClick(object sender, EventArgs e)
        {
            if (ActiveCanvas.FocusNode is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)ActiveCanvas.FocusNode;
                MessageBox.Show(
                    "Name:" + obj.EcellObject.key
                    + "\nLayer:" + obj.EcellObject.LayerID
                    + "\nX:" + obj.X + "\nY:" + obj.Y
                    + "\nWidth:" + obj.Width + "\nHeight:" + obj.Height
                    + "\nOffsetX:" + obj.OffsetX + "\nOffsetY:" + obj.OffsetY 
                    + "\nToString():" + obj.ToString());
            }
            else
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                MessageBox.Show("X:" + m_mousePos.X + "Y:" + m_mousePos.Y);
            }
        }
#endif
        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MergeClick(object sender, EventArgs e)
        {
            // Check exception.
            CanvasControl canvas = CanvasControl;
            if (canvas == null || canvas.SelectedSystem == null)
                return;

            PPathwaySystem system = canvas.SelectedSystem;
            m_window.NotifyDataMerge(system.EcellObject.ModelID, system.EcellObject.Key);
            if (system.IsHighLighted)
                CanvasControl.ResetSelectedSystem();
        }

        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteClick(object sender, EventArgs e)
        {
            // Check active canvas.
            CanvasControl canvas = this.CanvasControl;
            if (canvas == null)
                return;

            // Delete Selected Line
            PPathwayLine line = canvas.LineHandler.SelectedLine;
            if (line != null)
            {
                NotifyVariableReferenceChanged(
                    line.Info.ProcessKey,
                    line.Info.VariableKey,
                    RefChangeType.Delete,
                    0,
                    true);
                canvas.ResetSelectedLine();
            }
            // Delete Selected Nodes
            if (canvas.SelectedNodes != null)
            {
                List<EcellObject> slist = new List<EcellObject>();
                foreach (PPathwayObject node in canvas.SelectedNodes)
                    slist.Add(node.EcellObject);

                int i = 0;
                foreach (EcellObject deleteNode in slist)
                {
                    i++;
                    bool isAnchor = (i == slist.Count);
                    NotifyDataDelete(deleteNode, isAnchor);
                }
            }
            // Delete Selected System
            PPathwaySystem system = canvas.SelectedSystem;
            if (system != null)
            {
                // Return if system is null or root.
                if (string.IsNullOrEmpty(system.EcellObject.Key))
                    return;
                if (system.EcellObject.Key.Equals("/"))
                {
                    MessageBox.Show(m_resources.GetString("ErrDelRoot"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                // Delete sys.
                NotifyDataDelete(system.EcellObject, true);
                canvas.ResetSelectedSystem();
            }
        }

        /// <summary>
        /// Called when a create logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateLoggerClick(object sender, EventArgs e)
        {
            if (!(CanvasControl.FocusNode is PPathwayObject))
                return;

            string logger = ((ToolStripItem)sender).Text;
            PPathwayObject obj = (PPathwayObject)CanvasControl.FocusNode;
            Debug.WriteLine("Create " + obj.EcellObject.Type + " Logger:" + obj.EcellObject.Key);
            // set logger
            NotifyLoggerChanged(obj, logger, true);
        }

        /// <summary>
        /// Called when a delete logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteLoggerClick(object sender, EventArgs e)
        {
            if (!(CanvasControl.FocusNode is PPathwayObject))
                return;
            string logger = ((ToolStripItem)sender).Text;

            PPathwayObject obj = (PPathwayObject)CanvasControl.FocusNode;
            Debug.WriteLine("Delete " + obj.EcellObject.Type + " Logger:" + obj.EcellObject.Key);
            // delete logger
            NotifyLoggerChanged(obj, logger, false);
        }
        
        /// <summary>
        /// Called when one of layout menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LayoutItem_Click(object sender, EventArgs e)
        {
            if (!(sender is PathwayToolStripMenuItem))
                return;
            PathwayToolStripMenuItem item = (PathwayToolStripMenuItem)sender;
            int layoutIdx = item.ID;
            int subIdx = item.SubID;
            ILayoutAlgorithm algorithm = m_layoutList[layoutIdx];
            DoLayout(algorithm, subIdx, true);
        }

        /// <summary>
        /// Called when a change line menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChangeLineClick(object sender, EventArgs e)
        {
            // Selected MenuItem.
            if (!(sender is ToolStripItem))
                return;
            ToolStripItem item = (ToolStripItem)sender;
            // Selected Canvas
            CanvasControl canvas = CanvasControl;
            if (canvas == null)
                return;
            // Selected Line
            PPathwayLine line = canvas.LineHandler.SelectedLine;
            if (line == null)
                return;
            canvas.ResetSelectedLine();

            // Change edgeInfo.
            RefChangeType changeType = RefChangeType.SingleDir;
            int coefficient = 0;
            if (item.Name == MenuControl.CanvasMenuRightArrow)
            {
                changeType = RefChangeType.SingleDir;
                coefficient = 1;
            }
            else if (item.Name == MenuControl.CanvasMenuLeftArrow)
            {
                changeType = RefChangeType.SingleDir;
                coefficient = -1;
            }
            else if (item.Name == MenuControl.CanvasMenuBidirArrow)
            {
                changeType = RefChangeType.BiDir;
                coefficient = 0;
            }
            else
            {
                changeType = RefChangeType.SingleDir;
                coefficient = 0;
            }
            NotifyVariableReferenceChanged(
                line.Info.ProcessKey,
                line.Info.VariableKey,
                changeType,
                coefficient,
                true);
        }

        /// <summary>
        /// Change the Layer of Selected Objects.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChangeLeyerClick(object sender, EventArgs e)
        {
            if (this.CanvasControl == null)
                return;
            CanvasControl canvas = this.CanvasControl;
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;

            // Get new layer name.
            string name;
            if (menu.Text.Equals(m_resources.GetString(MenuControl.CanvasMenuCreateLayer)))
            {
                // Select Layer
                List<string> layerList = canvas.GetLayerNameList();
                name = SelectBoxDialog.Show(m_resources.GetString(MenuControl.CanvasMenuCreateLayer), m_resources.GetString(MenuControl.CanvasMenuCreateLayer), layerList);
                if (name == null || name.Equals(""))
                    return;
                if (!canvas.Layers.ContainsKey(name))
                    canvas.AddLayer(name);
            }
            else
            {
                name = menu.Text;
            }

            // Change layer of selected objects.
            PPathwayLayer layer = canvas.Layers[name];
            List<PPathwayObject> objList = canvas.SelectedNodes;
            int i = 0;
            foreach (PPathwayObject obj in objList)
            {
                obj.Layer = layer;
                i++;
                NotifyDataChanged(
                    obj.EcellObject.Key,
                    obj.EcellObject.Key,
                    obj,
                    true,
                    (i == objList.Count));
            }
        }

        /// <summary>
        /// Layer move to front.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MoveToFrontClick(object sender, EventArgs e)
        {
            CanvasControl canvas = this.CanvasControl;
            PPathwayObject obj = (PPathwayObject)canvas.FocusNode;
            canvas.LayerMoveToFront(obj.Layer);
        }

        /// <summary>
        /// Layer move to back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MoveToBackClick(object sender, EventArgs e)
        {
            CanvasControl canvas = this.CanvasControl;
            PPathwayObject obj = (PPathwayObject)canvas.FocusNode;
            canvas.LayerMoveToBack(obj.Layer);
        }

        /// <summary>
        /// Called when a copy menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CopyClick(object sender, EventArgs e)
        {
            if (this.CanvasControl == null)
                return;
            this.CopiedNodes.Clear();
            this.m_copyPos = this.m_mousePos;

            this.CopiedNodes = SetCopyNodes();
        }

        /// <summary>
        /// Called when a cut menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CutClick(object sender, EventArgs e)
        {
            if (this.CanvasControl == null)
                return;
            this.CopiedNodes.Clear();
            this.m_copyPos = this.m_mousePos;

            this.CopiedNodes = SetCopyNodes();

            int i = 0;
            bool isAnchor;
            foreach (EcellObject eo in this.CopiedNodes)
            {
                i++;
                isAnchor = (i == this.CopiedNodes.Count);
                this.NotifyDataDelete(eo, isAnchor);
            }

        }

        /// <summary>
        /// Called when a paste menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PasteClick(object sender, EventArgs e)
        {
            if (m_copiedNodes == null || m_copiedNodes.Count == 0)
                return;
            // Copy objects.
            List<EcellObject> nodeList;
            if (m_copiedNodes[0] is EcellSystem)
            {
                nodeList = CopySystems(m_copiedNodes);
            }
            else
                nodeList = CopyNodes(m_copiedNodes);
            // Add objects.
            int i = 0;
            bool isAnchor;
            foreach (EcellObject eo in nodeList)
            {
                i++;
                isAnchor = (i == m_copiedNodes.Count);
                this.NotifyDataAdd(eo, isAnchor);
            }
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        public void ShowDialogClick(object sender, EventArgs e)
        {
            PropertyDialog dialog = new PropertyDialog();
            dialog.Text = "PathwaySettings";
            PropertyDialogTabPage componentPage = m_csManager.CreateTabPage();
            PropertyDialogTabPage animationPage = m_animCon.CreateTabPage();
            dialog.TabControl.Controls.Add(animationPage);
            dialog.TabControl.Controls.Add(componentPage);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                componentPage.ApplyChange();
                animationPage.ApplyChange();
            }
            dialog.Dispose();
           
            if (m_canvas == null)
                return;
            m_canvas.ResetObjectSettings();
            SetNodeIcons();
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        public void ShowIdClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.CheckState == CheckState.Checked)
                ShowingID = true;
            else
                ShowingID = false;
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        public void ViewModeClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            ViewMode = item.Checked;
        }

        /// <summary>
        /// When select the button in ToolBox,
        /// system change the listener for event
        /// </summary>
        /// <param name="sender">ToolBoxMenuButton.</param>
        /// <param name="e">EventArgs.</param>
        public void ButtonStateChanged(object sender, EventArgs e)
        {
            if (!(sender is PathwayToolStripButton))
                return;
            PathwayToolStripButton selectedButton = (PathwayToolStripButton)sender;
            SetEventHandler(selectedButton.Handle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ZoomButton_Click(object sender, EventArgs e)
        {
            if (!(sender is PathwayToolStripButton))
                return;
            PathwayToolStripButton button = (PathwayToolStripButton)sender;
            float rate = button.Handle.ZoomingRate;
            if (this.CanvasControl == null)
                return;
            CanvasControl.Zoom(rate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="system"></param>
        /// <param name="type"></param>
        /// <param name="isRecorded"></param>
        /// <returns></returns>
        internal EcellObject CreateDefaultObject(string model, string system, string type, bool isRecorded)
        {
            EcellObject eo = m_window.DataManager.CreateDefaultObject(model, system, type, isRecorded);
            return eo;
        }
        #endregion

        #region private Methods.
        /// <summary>
        /// Create pathway canvas.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        private void CreateCanvas(string modelID)
        {
            // Create canvas
            CanvasControl = new CanvasControl(this, modelID);
            RaiseCanvasChange();
        }

        /// <summary>
        /// Set NodeIconImages.
        /// </summary>
        private void SetNodeIcons()
        {
            ImageList list = m_window.PluginManager.NodeImageList;
            list.Images.RemoveByKey(Constants.xpathSystem);
            list.Images.RemoveByKey(Constants.xpathProcess);
            list.Images.RemoveByKey(Constants.xpathVariable);

            list.Images.Add(Constants.xpathSystem, m_csManager.DefaultSystemSetting.IconImage);
            list.Images.Add(Constants.xpathProcess, m_csManager.DefaultProcessSetting.IconImage);
            list.Images.Add(Constants.xpathVariable, m_csManager.DefaultVariableSetting.IconImage);
        }

        /// <summary>
        /// Freeze all objects to be unpickable.
        /// </summary>
        private void Freeze()
        {
            if (m_isFreezed)
                return;
            if (m_canvas != null)
                m_canvas.Freeze();

            m_isFreezed = true;
        }

        /// <summary>
        /// Cancel freezed status.
        /// </summary>
        private void Unfreeze()
        {
            if (!m_isFreezed)
                return;
            if (m_canvas != null)
                m_canvas.Unfreeze();

            m_isFreezed = false;
        }

        /// <summary>
        /// Get the list of system in the target mode.
        /// </summary>
        /// <returns>The list of system.</returns>
        private List<EcellObject> GetSystemList()
        {
            List<EcellObject> systemList = new List<EcellObject>();
            foreach (PPathwayObject obj in m_canvas.GetSystemList())
                systemList.Add(m_window.GetEcellObject(obj.EcellObject.ModelID, obj.EcellObject.Key, obj.EcellObject.Type));
            return systemList;
        }

        /// <summary>
        /// Get the list of EcellObject in the target model.
        /// </summary>
        /// <returns>the list of EcellObject.</returns>
        private List<EcellObject> GetNodeList()
        {
            List<EcellObject> nodeList = new List<EcellObject>();
            foreach (PPathwayObject obj in m_canvas.GetNodeList())
                nodeList.Add(m_window.GetEcellObject(obj.EcellObject.ModelID, obj.EcellObject.Key, obj.EcellObject.Type));

            return nodeList;
        }

        /// <summary>
        /// Set copied nodes.
        /// </summary>
        /// <returns>A list of copied nodes.</returns>
        private List<EcellObject> SetCopyNodes()
        {
            List<EcellObject> copyNodes = new List<EcellObject>();
            // Copy System
            PPathwaySystem system = CanvasControl.SelectedSystem;
            EcellObject eo;
            if (system != null)
            {
                eo = system.EcellObject;
                copyNodes.Add(m_window.GetEcellObject(eo.ModelID, eo.Key, eo.Type));
                foreach (PPathwayObject child in CanvasControl.GetAllObjectUnder(system.EcellObject.Key))
                {
                    if (child is PPathwaySystem)
                    {
                        eo = child.EcellObject;
                        copyNodes.Add(m_window.GetEcellObject(eo.ModelID, eo.Key, eo.Type));
                    }
                }
            }
            //Copy Variavles
            foreach (PPathwayObject node in CanvasControl.SelectedNodes)
            {
                if (node is PPathwayVariable)
                {
                    eo = node.EcellObject;
                    copyNodes.Add(m_window.GetEcellObject(eo.ModelID, eo.Key, eo.Type));
                }
            }
            //Copy Processes
            foreach (PPathwayObject node in CanvasControl.SelectedNodes)
            {
                if (node is PPathwayProcess)
                {
                    eo = node.EcellObject;
                    copyNodes.Add(m_window.GetEcellObject(eo.ModelID, eo.Key, eo.Type));
                }
            }
            return copyNodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        private List<EcellObject> CopyNodes(List<EcellObject> nodeList)
        {
            List<EcellObject> copiedNodes = new List<EcellObject>();
            Dictionary<string, string> varKeys = new Dictionary<string, string>();
            CanvasControl canvas = this.CanvasControl;
            if (canvas == null || nodeList == null)
                return copiedNodes;

            // Get position diff
            PointF diff = GetDistance(this.m_mousePos, this.m_copyPos);
            // Get parent System
            string sysKey;
            // Set m_copiedNodes.
            for (int i = 0; i < nodeList.Count; i++)
            {
                //Create new EcellObject
                EcellObject eo = nodeList[i].Copy();
                string nodeKey = eo.Key;
                // Check parent system
                eo.ModelID = canvas.ModelID;
                eo.MovePosition(diff);
                sysKey = canvas.GetSurroundingSystemKey(eo.PointF);
                if (sysKey == null)
                    sysKey = canvas.GetSurroundingSystemKey(this.m_mousePos);
                if (eo.ParentSystemID != sysKey)
                    eo.ParentSystemID = sysKey;
                // Check Position
                if (!canvas.CheckNodePosition(eo.ParentSystemID, eo.PointF))
                    eo.PointF = canvas.GetVacantPoint(sysKey, eo.Rect);
                // Check duplicated object.
                if (canvas.GetSelectedObject(eo.Key, eo.Type) != null)
                    eo.Key = canvas.GetTemporaryID(eo.Type, sysKey);

                copiedNodes.Add(eo);
                // Set Variable name.
                if (!(eo is EcellVariable))
                    continue;
                varKeys.Add(nodeKey, eo.Key);
            }
            // Reset edges.
            foreach (EcellObject eo in copiedNodes)
            {
                if (!(eo is EcellProcess))
                    continue;
                EcellProcess ep = (EcellProcess)eo;
                List<EcellReference> list = ep.ReferenceList;
                List<EcellReference> newlist = new List<EcellReference>();
                foreach (EcellReference er in list)
                {
                    if (!varKeys.ContainsKey(er.Key))
                        continue;
                    er.Key = varKeys[er.Key];
                    newlist.Add(er);
                }
                ep.ReferenceList = newlist;
            }
            return copiedNodes;
        }

        /// <summary>
        /// Get distance of two pointF
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        private PointF GetDistance(PointF posA, PointF posB)
        {
            PointF diff = new PointF(posA.X - posB.X, posA.Y - posB.Y);
            if (diff.X == 0 && diff.Y == 0)
            {
                diff.X = diff.X + 10;
                diff.Y = diff.Y + 10;
            }
            return diff;
        }

        /// <summary>
        /// Copy selected systems.
        /// </summary>
        /// <param name="systemList"></param>
        /// <returns></returns>
        private List<EcellObject> CopySystems(List<EcellObject> systemList)
        {
            List<EcellObject> copiedSystems = new List<EcellObject>();
            Dictionary<string, string> varKeys = new Dictionary<string, string>();
            CanvasControl canvas = this.CanvasControl;
            if (canvas == null || systemList == null || systemList.Count == 0)
                return copiedSystems;

            // Get position diff
            PointF basePos = systemList[0].PointF;
            PointF diff = GetDistance(this.m_mousePos, basePos);
            // Get parent System
            string oldSysKey = systemList[0].ParentSystemID;
            string newSysKey = canvas.GetSurroundingSystemKey(this.m_mousePos);

            // Set m_copiedNodes.
            for (int i = 0; i < systemList.Count; i++)
            {
                //Create new EcellObject
                EcellObject system = systemList[i].Copy();
                // Check system position
                system.MovePosition(diff);
                if (canvas.DoesSystemOverlaps(newSysKey, system.Rect))
                {
                    system.PointF = canvas.GetVacantPoint(newSysKey, system.Rect);
                    diff = GetDistance(system.PointF, basePos);
                }
                // Check duplicated key.
                system.ModelID = canvas.ModelID;
                system.Key = PathUtil.GetMovedKey(system.Key, oldSysKey, newSysKey);
                if (canvas.GetSelectedObject(system.Key, system.Type) != null)
                {
                    oldSysKey = system.Key;
                    newSysKey = canvas.GetTemporaryID(system.Type, newSysKey);
                    system.Key = newSysKey;
                }
                // Check system overlap
                if (canvas.DoesSystemOverlaps(system.Rect))
                {
                    MessageBox.Show(m_resources.GetString("ErrSystemOverlap"));
                    break;
                }

                // Check child nodes.
                foreach (EcellObject eo in system.Children)
                {
                    string oldNodeKey = eo.Key;
                    eo.ParentSystemID = system.Key;
                    eo.MovePosition(diff);
                    // Set node name.
                    if (!(eo is EcellVariable))
                        continue;
                    varKeys.Add(oldNodeKey, eo.Key);
                }
                copiedSystems.Add(system);
            }
            // Reset edges.
            foreach (EcellObject system in copiedSystems)
            {
                foreach (EcellObject eo in system.Children)
                {
                    if (!(eo is EcellProcess))
                        continue;
                    EcellProcess ep = (EcellProcess)eo;
                    List<EcellReference> list = ep.ReferenceList;
                    List<EcellReference> newlist = new List<EcellReference>();
                    foreach (EcellReference er in list)
                    {
                        if (!varKeys.ContainsKey(er.Key))
                            continue;
                        er.Key = varKeys[er.Key];
                        newlist.Add(er);
                    }
                    ep.ReferenceList = newlist;
                }
            }
            return copiedSystems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="system"></param>
        private void DeleteSystemUnder(PPathwaySystem system)
        {
            CanvasControl canvas = system.CanvasControl;
            foreach (PPathwayObject obj in canvas.GetAllObjectUnder(system.EcellObject.Key))
                if (obj is PPathwaySystem)
                    DeleteSystemUnder((PPathwaySystem)obj);

            NotifyDataDelete(system.EcellObject, true);
        }

        /// <summary>
        /// Do object layout.
        /// </summary>
        /// <param name="algorithm">ILayoutAlgorithm</param>
        /// <param name="subIdx">int</param>
        /// <param name="isRecorded">Whether to record this change.</param>
        private void DoLayout(ILayoutAlgorithm algorithm, int subIdx, bool isRecorded)
        {
            List<EcellObject> systemList = this.GetSystemList();
            List<EcellObject> nodeList = this.GetNodeList();

            // Check Selected nodes when the layout algorithm uses selected objects.
            if (algorithm.GetLayoutType() == LayoutType.Selected)
                foreach (EcellObject node in nodeList)
                    node.isFixed = this.CanvasControl.GetSelectedObject(node.Key, node.Type).IsHighLighted;
            try
            {
                algorithm.DoLayout(subIdx, false, systemList, nodeList);
            }
            catch (Exception)
            {
                MessageBox.Show(m_resources.GetString("ErrLayout"));
                return;
            }

            // Set Layout.
            foreach (EcellObject system in systemList)
                this.NotifyDataChanged(system.Key, system, isRecorded, false);
            int i = 0;
            foreach (EcellObject node in nodeList)
            {
                node.isFixed = false;
                if(i != nodeList.Count)
                    this.NotifyDataChanged(node.Key, node, isRecorded, false);
                else
                    this.NotifyDataChanged(node.Key, node, isRecorded, true);
                i++;
            }
        }

        /// <summary>
        /// Add the selected EventHandler to event listener.
        /// </summary>
        /// <param name="handler">added EventHandler.</param>
        private void AddInputEventListener(PBasicInputEventHandler handler)
        {
            // Exception condition 
            if (m_canvas == null)
                return;
            m_canvas.PathwayCanvas.AddInputEventListener(handler);
        }

        /// <summary>
        /// Delete the selected EventHandler from event listener.
        /// </summary>
        /// <param name="handler">deleted EventHandler.</param>
        private void RemoveInputEventListener(PBasicInputEventHandler handler)
        {
            // Exception condition 
            if (m_canvas == null)
                return;

            m_canvas.PathwayCanvas.RemoveInputEventListener(handler);
        }
        #endregion
    }
}