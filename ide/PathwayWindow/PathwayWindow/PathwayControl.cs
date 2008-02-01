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
        private Dictionary<string, CanvasControl> m_canvasDict;

        /// <summary>
        /// The CanvasID of currently active canvas.
        /// </summary>
        private string m_activeCanvasID;

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
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));
        #endregion

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
        ///  get/set Dctionary of CanvasViewComponentSet.
        /// </summary>
        public Dictionary<string, CanvasControl> CanvasDictionary
        {
            get { return m_canvasDict; }
        }

        /// <summary>
        /// Accessor for currently active canvas.
        /// </summary>
        public CanvasControl ActiveCanvas
        {
            get
            {
                if (m_activeCanvasID == null)
                    return null;
                else
                    return m_canvasDict[m_activeCanvasID];
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
                if (m_canvasDict == null)
                    return;
                foreach (CanvasControl canvas in m_canvasDict.Values)
                    canvas.ShowingID = m_showingId;
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
                if (m_canvasDict == null)
                    return;
                foreach (CanvasControl canvas in m_canvasDict.Values)
                    canvas.ViewMode = m_isViewMode;
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
            m_canvasDict = new Dictionary<string, CanvasControl>();
            m_csManager = new ComponentManager();
            m_layoutList = m_window.GetLayoutAlgorithms();
            // Create menus
            m_menuCon = new MenuControl(this);
            // Set AnimationControl
            m_animCon = new AnimationControl(this);
            // Preparing Interfaces
            m_pathwayView = new PathwayView(this);
            m_overView = new OverView();
            m_layerView = new LayerView(this);
            m_toolBox = new ToolBox(this);
        }
        #endregion

        #region Public Methods
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
            if (EcellObject.PROJECT.Equals(eo.type))
            {
                this.Clear();
                return;
            }
            // create new canvas
            if (eo.type.Equals(EcellObject.MODEL))
            {
                this.CreateCanvas(eo.modelID);
                return;
            }
            // Error check.
            if (string.IsNullOrEmpty(eo.key))
                throw new PathwayException(m_resources.GetString("ErrKeyNot"));
            if (string.IsNullOrEmpty(eo.modelID) || !m_canvasDict.ContainsKey(eo.modelID))
                throw new PathwayException(m_resources.GetString("ErrNotSetCanvas") + eo.key);
            if (eo.key.EndsWith(":SIZE"))
                return;

            // Create PathwayObject and set to canvas.
            CanvasControl canvas = m_canvasDict[eo.modelID];
            ComponentType cType = ComponentManager.ParseStringToComponentType(eo.type);
            ComponentSetting cs = m_csManager.GetDefaultComponentSetting(cType);
            PPathwayObject obj = cs.CreateNewComponent(eo, canvas);
            canvas.DataAdd(eo.parentSystemID, obj, eo.IsPosSet, isFirst);
            NotifyDataChanged(eo.key, eo.key, obj, !isFirst, false);
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
            CanvasControl canvas = m_canvasDict[modelID];
            if (canvas == null)
                return;
            // If case SystemSize
            if (oldKey.EndsWith(":SIZE"))
                return;
            // Select changed object.
            PPathwayObject obj = canvas.GetSelectedObject(oldKey, type);
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
            canvas.DataChanged(oldKey, eo.key, obj);
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            CanvasControl canvas = m_canvasDict[modelID];
            if (canvas == null)
                return;
            // If case SystemSize
            if (key.EndsWith(":SIZE"))
                return;

            // Delete object.
            canvas.DataDelete(key, type);
        }

        /// <summary>
        /// Get the list of system in the target mode.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <returns>The list of system.</returns>
        public List<EcellObject> GetSystemList(string modelID)
        {
            List<EcellObject> systemList = new List<EcellObject>();
            foreach (PPathwayObject obj in m_canvasDict[modelID].GetSystemList())
                systemList.Add(m_window.GetEcellObject(modelID, obj.EcellObject.key, obj.EcellObject.type));
            return systemList;
        }

        /// <summary>
        /// Get the list of EcellObject in the target model.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        /// <returns>the list of EcellObject.</returns>
        public List<EcellObject> GetNodeList(string modelID)
        {
            List<EcellObject> nodeList = new List<EcellObject>();
            foreach (PPathwayObject obj in m_canvasDict[modelID].GetNodeList())
                nodeList.Add(m_window.GetEcellObject(modelID, obj.EcellObject.key, obj.EcellObject.type));

            return nodeList;
        }

        /// <summary>
        /// Clears the contents of the pathway window, and Returns it to an
        /// initial state.
        /// </summary>
        public void Clear()
        {
            // Reset Interfaces
            m_overView.Clear();
            m_pathwayView.TabControl.TabPages.Clear();
            m_layerView.DataGridView.DataSource = null;
            m_animCon.StopSimulation();
            // Clear Canvas dictionary.
            if (m_canvasDict != null)
                foreach (CanvasControl set in m_canvasDict.Values)
                    set.Dispose();
            m_canvasDict = null;
        }

        /// <summary>
        /// get bitmap image of this pathway.
        /// </summary>
        /// <returns>Bitmap of this pathway.</returns>
        public Bitmap Print()
        {
            if (m_canvasDict == null || m_canvasDict.Count == 0)
                return new Bitmap(1, 1);

            return ActiveCanvas.ToImage();
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
            if (modelID == null || !m_canvasDict.ContainsKey(modelID))
                return;
            CanvasControl canvas = m_canvasDict[modelID];
            if (canvas == null)
                return;
            canvas.SelectChanged(key, type);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // not implement
            CanvasControl canvas = m_canvasDict[modelID];
            if (canvas == null)
                return;
            canvas.AddSelect(key, type);
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
            if (ActiveCanvas == null)
                return;
            ActiveCanvas.ResetNodeToBeConnected();
            ActiveCanvas.LineHandler.SetLineVisibility(false);
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
            EcellObject eo = m_window.GetEcellObject(obj.EcellObject.modelID, oldKey, obj.EcellObject.type);
            if (eo == null)
                throw new Exception();

            eo.key = newKey;
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
                m_window.NotifyDataChanged(oldKey, newKey, eo, isRecorded, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                DataChanged(eo.modelID, oldKey, eo.type, obj.EcellObject);
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
                m_window.NotifyDataDelete(eo.modelID, eo.key, eo.type, isAnchor);
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
            EcellObject eo = m_window.GetEcellObject(obj.EcellObject.modelID, obj.EcellObject.key, obj.EcellObject.type);

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
                    eo.modelID,
                    eo.key,
                    eo.type,
                    d.EntityPath);
            }
            m_window.NotifyDataChanged(eo.key, eo.key, eo, true, true);
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
            EcellProcess ep = (EcellProcess)m_window.GetEcellObject(ActiveCanvas.ModelID, proKey, EcellObject.PROCESS);
            // End if obj is null.
            if (null == ep)
                return;
            // Get EcellReference List.
            List<EcellReference> refList = ep.ReferenceList;
            List<EcellReference> newList = new List<EcellReference>();
            EcellReference changedRef = null;

            foreach (EcellReference v in refList)
            {
                if (v.fullID.EndsWith(varKey))
                    changedRef = v;
                else
                    newList.Add(v);
            }

            if (changedRef != null && changeType != RefChangeType.Delete)
            {
                switch(changeType)
                {
                    case RefChangeType.SingleDir:
                        changedRef.coefficient = coefficient;
                        changedRef.name = PathUtil.GetNewReferenceName(newList, coefficient);
                        newList.Add(changedRef);
                        break;
                    case RefChangeType.BiDir:
                        EcellReference copyRef = changedRef.Copy();
                        changedRef.coefficient = -1;
                        changedRef.name = PathUtil.GetNewReferenceName(newList, -1);
                        copyRef.coefficient = 1;
                        copyRef.name = PathUtil.GetNewReferenceName(newList, 1);
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
                        addRef.coefficient = coefficient;
                        addRef.Key = varKey;
                        addRef.name = PathUtil.GetNewReferenceName(newList, coefficient);
                        addRef.isAccessor = 1;
                        newList.Add(addRef);
                        break;
                    case RefChangeType.BiDir:
                        EcellReference addSRef = new EcellReference();
                        addSRef.coefficient = -1;
                        addSRef.Key = varKey;
                        addSRef.name = PathUtil.GetNewReferenceName(newList, -1);
                        addSRef.isAccessor = 1;
                        newList.Add(addSRef);

                        EcellReference addPRef = new EcellReference();
                        addPRef.coefficient = 1;
                        addPRef.Key = varKey;
                        addPRef.name = PathUtil.GetNewReferenceName(newList, 1);
                        addPRef.isAccessor = 1;
                        newList.Add(addPRef);
                        break;
                }
            }
            ep.ReferenceList = newList;
            m_window.NotifyDataChanged(ep.key, ep.key, ep, true, isAnchor);
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
            CanvasControl canvas = ActiveCanvas;
            if (canvas == null || canvas.SelectedSystem == null)
                return;
            PPathwaySystem system = canvas.SelectedSystem;
            if (system.EcellObject.key.Equals("/"))
            {
                MessageBox.Show(m_resources.GetString("ErrDelRoot"),
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            // Check Object duplication.
            string sysKey = system.EcellObject.key;
            string parentSysKey = system.EcellObject.parentSystemID;
            foreach (PPathwayObject obj in ActiveCanvas.GetAllObjectUnder(sysKey))
            {
                string newKey = PathUtil.GetMovedKey(obj.EcellObject.key, sysKey, parentSysKey);
                if (ActiveCanvas.GetSelectedObject(newKey, obj.EcellObject.type) != null)
                {
                    MessageBox.Show(newKey + m_resources.GetString("ErrAlrExist"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
            }

            // Confirm system merge.
            DialogResult result = MessageBox.Show(m_resources.GetString("ConfirmMerge"),
                "Merge",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Cancel)
                return;

            try
            {
                // Move systems and nodes under merged system.
                foreach (PPathwayObject obj in ActiveCanvas.GetAllObjectUnder(sysKey))
                {
                    string newKey = PathUtil.GetMovedKey(obj.EcellObject.key, sysKey, parentSysKey);
                    //ActiveCanvas.TransferObject(newKey, obj.EcellObject.key, obj);
                    NotifyDataChanged(obj.EcellObject.key, newKey, obj, true, false);
                }
                NotifyDataDelete(system.EcellObject, true);
                //m_window.NotifyDataMerge(system.EcellObject.modelID, system.EcellObject.key);
            }
            catch (IgnoreException)
            {
                return;
            }
            if (system.IsHighLighted)
                ActiveCanvas.ResetSelectedSystem();
        }

        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteClick(object sender, EventArgs e)
        {
            // Check active canvas.
            CanvasControl canvas = this.ActiveCanvas;
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
                if (string.IsNullOrEmpty(system.EcellObject.key))
                    return;
                if (system.EcellObject.key.Equals("/"))
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
            if (!(ActiveCanvas.FocusNode is PPathwayObject))
                return;

            string logger = ((ToolStripItem)sender).Text;
            PPathwayObject obj = (PPathwayObject)ActiveCanvas.FocusNode;
            Debug.WriteLine("Create " + obj.EcellObject.type + " Logger:" + obj.EcellObject.key);
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
            if (!(ActiveCanvas.FocusNode is PPathwayObject))
                return;
            string logger = ((ToolStripItem)sender).Text;

            PPathwayObject obj = (PPathwayObject)ActiveCanvas.FocusNode;
            Debug.WriteLine("Delete " + obj.EcellObject.type + " Logger:" + obj.EcellObject.key);
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
            CanvasControl canvas = ActiveCanvas;
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
            if (this.ActiveCanvas == null)
                return;
            CanvasControl canvas = this.ActiveCanvas;
            // Select Layer
            List<string> layerList = canvas.GetLayerNameList();
            string name = SelectBoxDialog.Show("Select Layer", null, layerList);
            if (name == null || name.Equals(""))
                return;
            if (!canvas.Layers.ContainsKey(name))
                canvas.AddLayer(name);

            // Change layer of selected objects.
            PPathwayLayer layer = canvas.Layers[name];
            List<PPathwayObject> objList = canvas.SelectedNodes;
            int i = 0;
            foreach (PPathwayObject obj in objList)
            {
                obj.Layer = layer;
                i++;
                NotifyDataChanged(
                    obj.EcellObject.key,
                    obj.EcellObject.key,
                    obj,
                    true,
                    (i == objList.Count));
            }
        }

        /// <summary>
        /// Called when a copy menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CopyClick(object sender, EventArgs e)
        {
            if (this.ActiveCanvas == null)
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
            if (this.ActiveCanvas == null)
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
            dialog.TabControl.Controls.Add(componentPage);
            dialog.TabControl.Controls.Add(animationPage);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                componentPage.ApplyChange();
                animationPage.ApplyChange();
            }
            dialog.Dispose();

            foreach (CanvasControl canvas in m_canvasDict.Values)
            {
                canvas.ResetObjectSettings();
            }
            m_toolBox.ResetTemplate();
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
            if (this.CanvasDictionary == null)
                return;
            foreach(CanvasControl canvas in this.CanvasDictionary.Values)
            {
                canvas.Zoom(rate);
            }
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
            // Clear current canvas (TODO: Remove when support multiple canvas).
            m_canvasDict = new Dictionary<string, CanvasControl>();

            // Create canvas
            CanvasControl canvas = new CanvasControl(this, modelID);
            m_activeCanvasID = modelID;
            m_canvasDict.Add(modelID, canvas);
            canvas.PathwayCanvas.AddInputEventListener(m_selectedHandle.EventHandler);
            // Set Pathwayview
            m_pathwayView.Clear();
            m_pathwayView.TabControl.Controls.Add(canvas.TabPage);
            // Set Overview
            m_overView.SetCanvas(canvas.OverviewCanvas);
            canvas.UpdateOverview();
            // Set Layerview
            m_layerView.DataGridView.DataSource = canvas.LayerTable;
        }

        /// <summary>
        /// Freeze all objects to be unpickable.
        /// </summary>
        private void Freeze()
        {
            if (m_isFreezed)
                return;
            if (null != m_canvasDict)
            {
                foreach (CanvasControl canvas in m_canvasDict.Values)
                    canvas.Freeze();
            }
            m_isFreezed = true;
        }

        /// <summary>
        /// Cancel freezed status.
        /// </summary>
        private void Unfreeze()
        {
            if (!m_isFreezed)
                return;
            if (null != m_canvasDict)
            {
                foreach (CanvasControl canvas in m_canvasDict.Values)
                    canvas.Unfreeze();
            }
            m_isFreezed = false;
        }

        /// <summary>
        /// Set copied nodes.
        /// </summary>
        /// <returns>A list of copied nodes.</returns>
        private List<EcellObject> SetCopyNodes()
        {
            List<EcellObject> copyNodes = new List<EcellObject>();
            // Copy System
            PPathwaySystem system = ActiveCanvas.SelectedSystem;
            EcellObject eo;
            if (system != null)
            {
                eo = system.EcellObject;
                copyNodes.Add(m_window.GetEcellObject(eo.modelID, eo.key, eo.type));
                foreach (PPathwayObject child in ActiveCanvas.GetAllObjectUnder(system.EcellObject.key))
                {
                    if (child is PPathwaySystem)
                    {
                        eo = child.EcellObject;
                        copyNodes.Add(m_window.GetEcellObject(eo.modelID, eo.key, eo.type));
                    }
                }
            }
            //Copy Variavles
            foreach (PPathwayObject node in ActiveCanvas.SelectedNodes)
            {
                if (node is PPathwayVariable)
                {
                    eo = node.EcellObject;
                    copyNodes.Add(m_window.GetEcellObject(eo.modelID, eo.key, eo.type));
                }
            }
            //Copy Processes
            foreach (PPathwayObject node in ActiveCanvas.SelectedNodes)
            {
                if (node is PPathwayProcess)
                {
                    eo = node.EcellObject;
                    copyNodes.Add(m_window.GetEcellObject(eo.modelID, eo.key, eo.type));
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
            CanvasControl canvas = this.ActiveCanvas;
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
                string nodeKey = eo.key;
                // Check parent system
                eo.modelID = canvas.ModelID;
                eo.MovePosition(diff);
                sysKey = canvas.GetSurroundingSystemKey(eo.PointF);
                if (sysKey == null)
                    sysKey = canvas.GetSurroundingSystemKey(this.m_mousePos);
                if (eo.parentSystemID != sysKey)
                    eo.parentSystemID = sysKey;
                // Check Position
                if (!canvas.CheckNodePosition(eo.parentSystemID, eo.PointF))
                    eo.PointF = canvas.GetVacantPoint(sysKey, eo.Rect);
                // Check duplicated object.
                if (canvas.GetSelectedObject(eo.key, eo.type) != null)
                    eo.key = canvas.GetTemporaryID(eo.type, sysKey);

                copiedNodes.Add(eo);
                // Set Variable name.
                if (!(eo is EcellVariable))
                    continue;
                varKeys.Add(nodeKey, eo.key);
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
            CanvasControl canvas = this.ActiveCanvas;
            if (canvas == null || systemList == null || systemList.Count == 0)
                return copiedSystems;

            // Get position diff
            PointF basePos = systemList[0].PointF;
            PointF diff = GetDistance(this.m_mousePos, basePos);
            // Get parent System
            string oldSysKey = systemList[0].parentSystemID;
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
                system.modelID = canvas.ModelID;
                system.key = PathUtil.GetMovedKey(system.key, oldSysKey, newSysKey);
                if (canvas.GetSelectedObject(system.key, system.type) != null)
                {
                    oldSysKey = system.key;
                    newSysKey = canvas.GetTemporaryID(system.type, newSysKey);
                    system.key = newSysKey;
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
                    string oldNodeKey = eo.key;
                    eo.parentSystemID = system.key;
                    eo.MovePosition(diff);
                    // Set node name.
                    if (!(eo is EcellVariable))
                        continue;
                    varKeys.Add(oldNodeKey, eo.key);
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
            foreach (PPathwayObject obj in canvas.GetAllObjectUnder(system.EcellObject.key))
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
            List<EcellObject> systemList = this.GetSystemList(ActiveCanvas.ModelID);
            List<EcellObject> nodeList = this.GetNodeList(ActiveCanvas.ModelID);

            // Check Selected nodes when the layout algorithm uses selected objects.
            if (algorithm.GetLayoutType() == LayoutType.Selected)
                foreach (EcellObject node in nodeList)
                    node.isFixed = this.ActiveCanvas.GetSelectedObject(node.key, node.type).IsHighLighted;
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
                this.m_window.NotifyDataChanged(system.key, system.key, system, isRecorded, false);
            int i = 0;
            foreach (EcellObject node in nodeList)
            {
                node.isFixed = false;
                if(i != nodeList.Count)
                    this.m_window.NotifyDataChanged(node.key, node.key, node, isRecorded, false);
                else
                    this.m_window.NotifyDataChanged(node.key, node.key, node, isRecorded, true);
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
            if (m_canvasDict == null)
                return;

            foreach (CanvasControl canvas in m_canvasDict.Values)
                canvas.PathwayCanvas.AddInputEventListener(handler);
        }

        /// <summary>
        /// Delete the selected EventHandler from event listener.
        /// </summary>
        /// <param name="handler">deleted EventHandler.</param>
        private void RemoveInputEventListener(PBasicInputEventHandler handler)
        {
            // Exception condition 
            if (m_canvasDict == null)
                return;

            foreach(CanvasControl canvas in m_canvasDict.Values)
                canvas.PathwayCanvas.RemoveInputEventListener(handler);
        }
        #endregion
    }
}