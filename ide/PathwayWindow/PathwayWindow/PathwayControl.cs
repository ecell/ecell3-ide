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
using EcellLib.Layout;
using EcellLib.Objects;

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
        public MenuControl Menu
        {
            get { return m_menuCon; }
            set { m_menuCon = value; }
        }

        /// <summary>
        /// get/set the AnimationControl.
        /// </summary>
        public AnimationControl Animation
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
        public CanvasControl Canvas
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
                bool isFirst = (modelId != null);
                // Load each EcellObject onto the canvas.
                foreach (EcellObject obj in data)
                {
                    DataAdd(obj, true, isFirst);
                    if (!(obj is EcellSystem))
                        continue;
                    foreach (EcellObject node in obj.Children)
                        DataAdd(node, true, isFirst);
                }
                // Set layout.
                SetLayout(data, modelId, isFirst);
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
        /// Set layout.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="modelId"></param>
        /// <param name="isFirst"></param>
        private void SetLayout(List<EcellObject> data, string modelId, bool isFirst)
        {
            // Load layout information from LEML.
            if (isFirst)
            {
                string fileName = m_window.GetLEMLFileName(modelId);
                if (File.Exists(fileName))
                    this.LoadFromLeml(fileName);
                else
                    DoLayout(m_defAlgorithm, 0, false);
            }
            if(m_canvas != null)
                m_canvas.Refresh();
        }

        /// <summary>
        /// This method was made for dividing long and redundant DataAdd method.
        /// So, used by DataAdd only.
        /// </summary>
        /// <param name="fileName">Leml file path</param>
        private void LoadFromLeml(string fileName)
        {
            // Load Object Settings.
            List<EcellObject> objList = EcellSerializer.LoadFromXML(fileName);

            // Set position.
            foreach (EcellObject eo in objList)
            {
                PPathwayObject obj = m_canvas.GetSelectedObject(eo.Key, eo.Type);
                if (obj == null)
                    continue;

                obj.EcellObject.SetPosition(eo);
                if (!string.IsNullOrEmpty(eo.LayerID))
                    obj.EcellObject.LayerID = eo.LayerID;

                this.NotifyDataChanged(eo.Key, obj.EcellObject, false, false);
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
            if (!Canvas.ModelID.Equals(modelID))
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
            Canvas = null;
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
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // Error check.
            if (modelID == null || m_canvas == null)
                return;
            if (!m_canvas.ModelID.Equals(modelID))
                return;
            m_canvas.RemoveSelect(key, type);
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
        internal void SetEventHandler(Handle handle)
        {
            // Remove old EventHandler
            PBasicInputEventHandler handler = m_selectedHandle.EventHandler;
            ((IPathwayEventHandler)handler).Reset();
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
            ((IPathwayEventHandler)handler).Initialize();
            AddInputEventListener(handler);
            if (Canvas == null)
                return;
            Canvas.ResetNodeToBeConnected();
            Canvas.LineHandler.SetLineVisibility(false);
        }
        /// <summary>
        /// Reset object settings.
        /// </summary>
        internal void ResetObjectSettings()
        {
            if (m_canvas == null)
                return;
            m_canvas.ResetObjectSettings();
            SetNodeIcons();
            if (m_isViewMode)
                m_animCon.SetPropForSimulation();
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
        /// <summary>
        /// CopyNodes
        /// </summary>
        internal void CopyNodes()
        {
            if (this.Canvas == null)
                return;
            this.m_copiedNodes.Clear();
            this.m_copyPos = this.m_mousePos;
            this.m_copiedNodes = this.SetCopyingNodes();
        }
        /// <summary>
        /// PasteNodes
        /// </summary>
        internal void PasteNodes()
        {
            if (this.m_copiedNodes == null || this.m_copiedNodes.Count == 0)
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
                isAnchor = (i == this.m_copiedNodes.Count);
                NotifyDataAdd(eo, isAnchor);
            }
        }
        /// <summary>
        /// Set NodeIconImages.
        /// </summary>
        internal void SetNodeIcons()
        {
            ImageList list = m_window.PluginManager.NodeImageList;
            list.Images.RemoveByKey(Constants.xpathSystem);
            list.Images.RemoveByKey(Constants.xpathProcess);
            list.Images.RemoveByKey(Constants.xpathVariable);

            list.Images.Add(Constants.xpathSystem, m_csManager.DefaultSystemSetting.IconImage);
            list.Images.Add(Constants.xpathProcess, m_csManager.DefaultProcessSetting.IconImage);
            list.Images.Add(Constants.xpathVariable, m_csManager.DefaultVariableSetting.IconImage);
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
            try
            {
                m_window.NotifyDataAdd(list, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        
        /// <summary>
        /// Notify DataChanged event to outside (PathwayControl -> PathwayWindow -> DataManager)
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
            m_window.NotifyDataChanged(oldKey, eo, isRecorded, isAnchor);
        }

        /// <summary>
        /// Notify DataChanged event to outside (PathwayControl -> PathwayWindow -> DataManager)
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
            EcellObject eo = obj.EcellObject;

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
        public void NotifyLoggerChanged(PPathwayObject obj, string logger, bool isLogger)
        {
            EcellObject eo = obj.EcellObject;

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
            try
            {
                NotifyDataChanged(eo.Key, eo.Key, obj, true, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
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
            PPathwayObject node = m_canvas.GetSelectedObject(proKey, EcellObject.PROCESS);
            EcellProcess ep = (EcellProcess)node.EcellObject;

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
            try
            {
                NotifyDataChanged(proKey, proKey, node, true, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Notify DataDelete event to outsite.
        /// </summary>
        /// <param name="obj">the deleted object.</param>
        /// <param name="isAnchor">the type of deleted object.</param>
        public void NotifyDataDelete(PPathwayObject obj, bool isAnchor)
        {
            try
            {
                NotifyDataDelete(obj.EcellObject, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

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

        #region private Methods.
        /// <summary>
        /// Create pathway canvas.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        private void CreateCanvas(string modelID)
        {
            // Create canvas
            Canvas = new CanvasControl(this, modelID);
            SetEventHandler(m_selectedHandle);
            RaiseCanvasChange();
        }

        /// <summary>
        /// Get the list of EcellObject in the target model.
        /// </summary>
        /// <returns></returns>
        private List<EcellObject> GetObjectList()
        {
            List<EcellObject> list = new List<EcellObject>();
            list.AddRange(GetSystemList());
            list.AddRange(GetNodeList());
            return list;
        }

        /// <summary>
        /// Get the list of system in the target model.
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
        private List<EcellObject> SetCopyingNodes()
        {
            List<EcellObject> copyNodes = new List<EcellObject>();
            // Copy System
            PPathwaySystem system = Canvas.SelectedSystem;
            EcellObject eo;
            if (system != null)
            {
                eo = system.EcellObject;
                copyNodes.Add(m_window.GetEcellObject(eo.ModelID, eo.Key, eo.Type));
                foreach (PPathwayObject child in Canvas.GetAllObjectUnder(system.EcellObject.Key))
                {
                    if (child is PPathwaySystem)
                    {
                        eo = child.EcellObject;
                        copyNodes.Add(m_window.GetEcellObject(eo.ModelID, eo.Key, eo.Type));
                    }
                }
            }
            //Copy Variavles
            foreach (PPathwayObject node in Canvas.SelectedNodes)
            {
                if (node is PPathwayVariable)
                {
                    eo = node.EcellObject;
                    copyNodes.Add(m_window.GetEcellObject(eo.ModelID, eo.Key, eo.Type));
                }
            }
            //Copy Processes
            foreach (PPathwayObject node in Canvas.SelectedNodes)
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
            CanvasControl canvas = this.Canvas;
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
            CanvasControl canvas = this.Canvas;
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
        public void DoLayout(ILayoutAlgorithm algorithm, int subIdx, bool isRecorded)
        {
            List<EcellObject> systemList = this.GetSystemList();
            List<EcellObject> nodeList = this.GetNodeList();

            // Check Selected nodes when the layout algorithm uses selected objects.
            if (algorithm.GetLayoutType() == LayoutType.Selected)
                foreach (EcellObject node in nodeList)
                    node.isFixed = this.Canvas.GetSelectedObject(node.Key, node.Type).IsHighLighted;
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