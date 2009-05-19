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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Ecell.Action;
using Ecell.IDE.Plugins.PathwayWindow.Animation;
using Ecell.IDE.Plugins.PathwayWindow.Components;
using Ecell.IDE.Plugins.PathwayWindow.Exceptions;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.Objects;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.PathwayWindow
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
        private MenuControl m_menu;

        /// <summary>
        /// AnimationControl for simulation.
        /// </summary>
        private AnimationControl m_animCon;

        /// <summary>
        /// OverView interface.
        /// </summary>
        private PathwayView m_pathwayView;
        /// <summary>
        /// LayerView interface.
        /// </summary>
        private LayerView m_layerView;
        /// <summary>
        /// ToolBox interface.
        /// </summary>
        private Stencils m_toolBox;

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
        private bool m_isAnimation = true;

        /// <summary>
        /// 
        /// </summary>
        private bool m_highQuality = true;

        /// <summary>
        /// Focus Mode
        /// </summary>
        private bool m_focusMode = true;

        /// <summary>
        /// Unreached flag for DataChange event.
        /// </summary>
        private bool m_unreachedFlag = false;
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
        /// Accessor for m_layerView.
        /// </summary>
        public LayerView LayerView
        {
            get { return m_layerView; }
        }
        /// <summary>
        /// Accessor for m_layerView.
        /// </summary>
        public Stencils ToolBox
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
            get { return m_menu; }
            set { m_menu = value; }
        }

        /// <summary>
        /// Get the TabPages for PropertyDialog.
        /// </summary>
        public List<IPropertyItem> PropertySettings
        {
            get
            {
                PropertyNode node = new PropertyNode(MessageResources.WindowPathway);
                node.Nodes.Add(new PropertyNode(m_animCon.PathwaySettingsPage));
                node.Nodes.Add(new PropertyNode(m_animCon.AnimationSettingsPage));
                node.Nodes.Add(new PropertyNode(m_csManager.ComponentSettingsPage));

                List<IPropertyItem> nodeList = new List<IPropertyItem>();
                nodeList.Add(node);
                return nodeList;
            }
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
        /// 
        /// </summary>
        public ProjectStatus ProjectStatus
        {
            get { return m_status; }
            set 
            {
                m_status = value;
                RaiseProjectStatusChange();
            }
        }
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
                m_canvas.PCanvas.AddInputEventListener(m_menu.Handle.EventHandler);
            }
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
        public bool IsAnimation
        {
            get { return m_isAnimation; }
            set
            {
                m_isAnimation = value;
                RaiseAnimationChange();
            }
        }

        /// <summary>
        /// get/set the flag of showing id.
        /// </summary>
        public bool HighQuality
        {
            get { return m_highQuality; }
            set
            {
                m_highQuality = value;
                if (m_canvas != null)
                    m_canvas.PCanvas.HighQuality = value;
            }
        }

        /// <summary>
        /// Accessor for m_focusMode.
        /// </summary>
        public bool FocusMode
        {
            get { return m_focusMode; }
            set 
            {
                m_focusMode = value;
                if (m_canvas != null)
                    m_canvas.FocusMode = value;
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
            m_csManager = new ComponentManager();
            m_csManager.Control = this;
            SetNodeIcons();
            // Create menus
            m_menu = new MenuControl(this);
            // Set AnimationControl
            m_animCon = new AnimationControl(this);
            // Preparing Interfaces
            m_pathwayView = new PathwayView(this);
            m_layerView = new LayerView(this);
            m_toolBox = new Stencils(this);

            m_window.PluginManager.Refresh += new EventHandler(PluginManager_Refresh);
        }

        void PluginManager_Refresh(object sender, EventArgs e)
        {
            m_canvas.RefreshEdges();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add new objects to this canvas.
        /// </summary>
        /// <param name="data"></param>
        public void DataAdd(List<EcellObject> data)
        {
            string mes = MessageResources.MessageLoadModel;
            int allcount = data.Count;
            int count = 0;

            // Load Model.
            try
            {
                // Check New Model.
                string modelId = CheckNewModel(data);
                bool layoutFlag = CheckLayout(data);
                bool isFirst = (modelId != null);
                if (isFirst)
                    Progress(mes, 100, 0);

                // Load each EcellObject onto the canvas.
                foreach (EcellObject obj in data)
                {
                    DataAdd(obj, true);
                    if (!(obj is EcellSystem))
                    {
                        if (isFirst)
                        {
                            Progress(mes, 100, count * 50 / allcount);
                            count++;
                        }
                        continue;
                    }
                    foreach (EcellObject node in obj.Children)
                        DataAdd(node, true);
                    if (isFirst)
                    {
                        Progress(mes, 100, count * 50 / allcount);
                        count++;
                    }
                }
                if (isFirst)
                    Progress(mes, 100, 50);
                // Set layout.
                SetLayout(data, layoutFlag, isFirst);
                if (isFirst)
                {
                    Progress(mes, 100, 100);
                    Window.Environment.ReportManager.SetStatus(StatusBarMessageKind.Generic, "");
                    m_canvas.RefreshEdges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw new PathwayException(MessageResources.ErrUnknowType, e);
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool CheckLayout(List<EcellObject> data)
        {
            bool layoutFlag = false;
            foreach (EcellObject eo in data)
            {
                if (eo.Type.Equals(EcellObject.SYSTEM))
                {
                    layoutFlag = eo.Layout.IsEmpty;
                    break;
                }
            }
            return layoutFlag;
        }

        /// <summary>
        /// Set layout.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="layoutFlag"></param>
        /// <param name="isFirst"></param>
        private void SetLayout(List<EcellObject> data, bool layoutFlag, bool isFirst)
        {
            // Load layout information from LEML.
            if (!isFirst)
                return;
            if (!layoutFlag)
                return;

            // Do Layout.
            foreach (ILayoutAlgorithm la in m_window.PluginManager.LayoutAlgorithms)
            {
                if (la.GetLayoutName() == "Grid")
                {
                    DoLayout(la, 0, false);
                    break;
                }
            }
        }

        /// <summary>
        /// Set the percent of progress.
        /// </summary>
        /// <param name="msg">the message of progress.</param>
        /// <param name="max">the max progress of process.</param>
        /// <param name="val">the value of progress.</param>
        public void Progress(string msg, int max, int val)
        {
            Window.Environment.ReportManager.SetStatus(
                StatusBarMessageKind.Generic,
                msg
            );
            Window.Environment.ReportManager.SetProgress(100 * val / max);
        }

        /// <summary>
        /// Get DockContents of PathwayWindow.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<EcellDockContent> GetDockContents()
        {
            return new EcellDockContent[] {
                m_pathwayView,
                m_layerView,
                //m_overView,
                m_toolBox,
            };
        }

        /// <summary>
        /// Add new object to this canvas.
        /// </summary>
        /// <param name="eo">EcellObject</param>
        /// <param name="isAnchor">True is default. If undo unit contains multiple actions,
        /// only the last action's isAnchor is true, the others' isAnchor is false</param>
        public void DataAdd(EcellObject eo, bool isAnchor)
        {
            // Null check.
            if (eo == null)
                return;

            // Load new project
            if (EcellObject.PROJECT.Equals(eo.Type))
            {
                this.Clear();
                return;
            }

            // create new canvas
            if (eo.Type.Equals(EcellObject.MODEL))
            {
                this.CreateCanvas((EcellModel)eo);
                //foreach (EcellObject child in eo.Children)
                //{
                //    if (!(child is EcellStepper))
                //        continue;
                //    DataAdd(child, isAnchor);
                //}
                return;
            }

            // Ignore system size.
            if (eo.Type == Constants.xpathVariable && eo.LocalID == "SIZE")
            {
                return;
            }

            // Create PathwayObject and set to canvas.
            PPathwayObject obj = m_csManager.CreateNewComponent(eo.Type);
            obj.Canvas = m_canvas;
            obj.EcellObject = eo;

            m_canvas.DataAdd(obj, eo.IsPosSet);
            NotifySetPosition(obj);
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

            // Reset unreached flag.
            m_unreachedFlag = false;

            if (type.Equals(EcellObject.MODEL))
            {
                m_layerView.ResetLayers(((EcellModel)eo).Layers);
                m_canvas.RefreshEdges();
            }

            PPathwayObject obj;
            // If case SystemSize
            if (type == Constants.xpathVariable)
            {
                if (eo.LocalID == "SIZE")
                {
                    obj = m_canvas.Systems[eo.ParentSystemID];
                    ((EcellSystem)obj.EcellObject).SizeInVolume = (double)eo.GetEcellValue("Value");
                    obj.Refresh();
                }
            }

            // Select changed object.
            obj = m_canvas.GetObject(oldKey, type);
            if (obj == null)
                return;
            // Change data.
            RectangleF rect = obj.Rect;
            obj.EcellObject = eo;
            m_canvas.DataChanged(oldKey, eo.Key, obj, rect);
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
            if (type == Constants.xpathVariable)
            {
                string superSystemPath, localID;
                Util.ParseEntityKey(key, out superSystemPath, out localID);
                if (localID == "SIZE")
                    return;
            }

            // Delete object.
            m_canvas.DataDelete(key, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eo"></param>
        public void SetPosition(EcellObject eo)
        {
            // Select Canvas
            if (m_canvas == null)
                return;
            // If case SystemSize
            if (eo.Type == Constants.xpathVariable)
            {
                string superSystemPath, localID;
                Util.ParseEntityKey(eo.Key, out superSystemPath, out localID);
                if (localID == "SIZE")
                    return;
            }

            // Select changed object.
            PPathwayObject obj = m_canvas.GetObject(eo.Key, eo.Type);
            if (obj == null)
                return;

            // Change data.
            obj.EcellObject = eo;
            m_canvas.SetLayer(obj);
            obj.RefreshView();
        }

        /// <summary>
        /// When save the model, plugin save the specified information of model using only this plugin.
        /// </summary>
        /// <param name="modelID">the id of saved model.</param>
        /// <param name="directory">the directory of save.</param>
        public void SaveModel(string modelID, string directory)
        {
            if (m_canvas == null || !m_canvas.ModelID.Equals(modelID))
                return;

            string fileName = Path.Combine(directory, modelID + Constants.FileExtLEML);
            Bitmap image = m_canvas.OverviewCanvas.ToImage(400, 400);
            fileName = directory + Constants.FileExtPNG;
            if (File.Exists(fileName))
                File.Delete(fileName);
            image.Save(fileName);
        }

        /// <summary>
        /// Clears the contents of the pathway window, and Returns it to an
        /// initial state.
        /// </summary>
        public void Clear()
        {
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
        /// The event process when user reset select.
        /// </summary>
        public void ResetSelect()
        {
            if (m_canvas == null)
                return;
            m_canvas.ResetSelect();

        }

        /// <summary>
        /// Reset object settings.
        /// </summary>
        internal void ResetObjectSettings()
        {
            if (m_canvas == null)
                return;
            SetNodeIcons();
            if (m_animCon.DoesAnimationOnGoing)
                m_animCon.SetPropForSimulation();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="system"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal EcellObject CreateDefaultObject(string model, string system, string type)
        {
            EcellObject eo = m_window.DataManager.CreateDefaultObject(model, system, type);
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
        /// Set copied nodes.
        /// </summary>
        /// <returns>A list of copied nodes.</returns>
        private List<EcellObject> SetCopyingNodes()
        {
            List<EcellObject> copyNodes = new List<EcellObject>();
            //Copy Systems
            foreach (PPathwayObject obj in m_canvas.Systems.Values)
            {
                if (obj.EcellObject.Key == Constants.delimiterPath)
                    continue;
                if (m_canvas.SelectedNodes.Contains(obj) || copyNodes.Contains(obj.ParentObject.EcellObject))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));
            }
            //Copy Variables
            foreach (PPathwayObject obj in m_canvas.Variables.Values)
                if (m_canvas.SelectedNodes.Contains(obj) && !m_canvas.SelectedNodes.Contains(obj.ParentObject))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));
            //Copy Processes
            foreach (PPathwayObject obj in m_canvas.Processes.Values)
                if (m_canvas.SelectedNodes.Contains(obj) && !m_canvas.SelectedNodes.Contains(obj.ParentObject))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));
            //Copy Texts
            foreach (PPathwayObject obj in Canvas.Texts.Values)
                if (m_canvas.SelectedNodes.Contains(obj))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));
            //Copy Stepper
            foreach (PPathwayObject obj in Canvas.Steppers.Values)
                if (m_canvas.SelectedNodes.Contains(obj))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));

            return copyNodes;
        }

        /// <summary>
        /// PasteNodes
        /// </summary>
        internal void PasteNodes()
        {
            if (this.m_copiedNodes == null || this.m_copiedNodes.Count == 0)
                return;

            // Get parent System
            string oldSysKey = m_copiedNodes[0].ParentSystemID;
            string newSysKey = m_canvas.GetSurroundingSystemKey(this.m_mousePos);

            // Get position diff
            PointF diff = GetDistance(this.m_mousePos, m_copiedNodes[0].PointF);
            PPathwaySystem system = m_canvas.Systems[newSysKey];
            foreach (EcellObject eo in m_copiedNodes)
            {
                //Create new EcellObject
                if (eo.X + diff.X < system.X + PPathwaySystem.SYSTEM_MARGIN)
                    diff.X = system.X - eo.X + PPathwaySystem.SYSTEM_MARGIN;
                if (eo.Y + diff.Y < system.Y + PPathwaySystem.SYSTEM_MARGIN)
                    diff.Y = system.Y - eo.Y + PPathwaySystem.SYSTEM_MARGIN;
            }

            // Set m_copiedNodes.
            List<EcellObject> copiedObjects = new List<EcellObject>();
            foreach (EcellObject eo in m_copiedNodes)
                copiedObjects.Add(eo.Clone());
            Dictionary<string, string> keyDic = new Dictionary<string, string>();
            foreach (EcellObject eo in copiedObjects)
            {
                //Create new EcellObject
                eo.ModelID = m_canvas.ModelID;
                eo.X = eo.X + diff.X;
                eo.Y = eo.Y + diff.Y;

                // Get new key.
                string oldKey = eo.Key;
                if( keyDic.ContainsKey(eo.ParentSystemID))
                    eo.Key = GetCopiedID(eo.Type, Util.GetMovedKey(eo.Key, eo.ParentSystemID, keyDic[eo.ParentSystemID]));
                else if (eo is EcellText || eo is EcellStepper)
                    eo.Key = GetCopiedID(eo.Type, eo.Key);
                else
                    eo.Key = GetCopiedID(eo.Type, Util.GetMovedKey(eo.Key, eo.ParentSystemID, newSysKey));
                // Set Keydic.
                keyDic.Add(oldKey, eo.Key);

                // Check child nodes.
                foreach (EcellObject child in eo.Children)
                {
                    string oldNodeKey = child.Key;
                    child.ModelID = m_canvas.ModelID;
                    child.ParentSystemID = eo.Key;
                    child.X = child.X + diff.X;
                    child.Y = child.Y + diff.Y;
                    // Set node name.
                    if (child is EcellVariable)
                        keyDic.Add(oldNodeKey, child.Key);

                }
                eo.isFixed = false;
                NotifyDataAdd(eo, false);
            }

            // Reset edges.
            List<EcellObject> processes = new List<EcellObject>();
            foreach (EcellObject obj in copiedObjects)
            {
                foreach (EcellObject child in obj.Children)
                {
                    if (!(child is EcellProcess))
                        continue;
                    EcellObject p2 = m_window.GetEcellObject(child);
                    if (ReplaceVarRef(keyDic, p2))
                        processes.Add(p2);
                }
                if (!(obj is EcellProcess))
                    continue;
                EcellObject p1 = m_window.GetEcellObject(obj);
                if (ReplaceVarRef(keyDic, p1))
                    processes.Add(p1);
            }
            foreach (EcellObject eo in processes)
            {
                NotifyDataChanged(eo.Key, eo.Clone(), true, false);
            }

            // Set Anchor.
            m_window.Environment.ActionManager.AddAction(new AnchorAction());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="varKeys"></param>
        /// <param name="obj"></param>
        private static bool ReplaceVarRef(Dictionary<string, string> varKeys, EcellObject obj)
        {
            bool flag = false;
            if (!(obj is EcellProcess))
                return flag;

            // Set VariableReferenceList.
            EcellProcess ep = (EcellProcess)obj;
            List<EcellReference> list = ep.ReferenceList;
            List<EcellReference> newlist = new List<EcellReference>();
            foreach (EcellReference er in list)
            {
                if (!varKeys.ContainsKey(er.Key))
                    continue;
                er.Key = varKeys[er.Key];
                newlist.Add(er);
                flag = true;
            }
            ep.ReferenceList = newlist;
            return flag;
        }

        /// <summary>
        /// DeteleNodes
        /// </summary>
        internal void DeteleNodes()
        {
            // Delete Selected Line
            PPathwayLine line = m_canvas.LineHandler.SelectedLine;
            if (line != null)
            {
                NotifyVariableReferenceChanged(
                    line.Info.ProcessKey,
                    line.Info.VariableKey,
                    RefChangeType.Delete,
                    0,
                    true);
                m_canvas.ResetSelectedLine();
            }

            // Delete Selected Nodes
            List<EcellObject> slist = new List<EcellObject>();
            foreach (PPathwayObject node in m_canvas.SelectedNodes)
            {
                if (!m_canvas.SelectedNodes.Contains(node.ParentObject))
                    slist.Add(node.EcellObject);
            }
            int i = 0;
            foreach (EcellObject deleteNode in slist)
            {
                i++;
                bool isAnchor = (i == slist.Count);
                NotifyDataDelete(deleteNode, isAnchor);
            }
        }

        /// <summary>
        /// CutNodes
        /// </summary>
        internal void CutNodes()
        {
            CopyNodes();

            int i = 0;
            bool isAnchor;
            foreach (EcellObject eo in m_copiedNodes)
            {
                i++;
                isAnchor = (i == m_copiedNodes.Count);
                NotifyDataDelete(eo, isAnchor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Undo()
        {
            m_window.Environment.ActionManager.UndoAction();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Redo()
        {
            m_window.Environment.ActionManager.RedoAction();
        }

        /// <summary>
        /// Set NodeIconImages.
        /// </summary>
        internal void SetNodeIcons()
        {
            m_window.PluginManager.SetIconImage(Constants.xpathSystem, m_csManager.DefaultSystemSetting.IconImage, false);
            m_window.PluginManager.SetIconImage(Constants.xpathProcess, m_csManager.DefaultProcessSetting.IconImage, false);
            m_window.PluginManager.SetIconImage(Constants.xpathVariable, m_csManager.DefaultVariableSetting.IconImage, false);
            m_window.PluginManager.SetIconImage(Constants.xpathText, m_csManager.DefaultTextSetting.IconImage, false);
            m_window.PluginManager.SetIconImage(Constants.xpathStepper, m_csManager.DefaultStepperSetting.IconImage, true);
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
            try
            {
                m_window.NotifyDataAdd(eo, isAnchor);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Notify DataAdd event to outside.
        /// </summary>
        /// <param name="list">Added EcellObject</param>
        public void NotifyDataAdd(List<EcellObject> list)
        {
            try
            {
                m_window.NotifyDataAdd(list, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        
        /// <summary>
        /// NotifySetPosition
        /// </summary>
        /// <param name="obj"></param>
        public void NotifySetPosition(PPathwayObject obj)
        {
            PointF offset = obj.Offset;
            EcellObject eo = obj.EcellObject;
            eo.Layer = obj.Layer.Name;
            eo.X = obj.X + offset.X;
            eo.Y = obj.Y + offset.Y;
            eo.Width = obj.Width;
            eo.Height = obj.Height;
            eo.OffsetX = 0f;
            eo.OffsetY = 0f;

            NotifySetPosition(eo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eo"></param>
        public void NotifySetPosition(EcellObject eo)
        {
            m_window.NotifySetPosition(eo);
        }

        /// <summary>
        /// Notify DataChanged event to outside (PathwayControl -> PathwayWindow -> DataManager)
        /// To notify position or size change.
        /// </summary>
        /// <param name="obj">x coordinate of object.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(PPathwayObject obj, bool isAnchor)
        {
            EcellObject eo = obj.EcellObject;
            NotifyDataChanged(eo.Key, eo.Key, obj, true, isAnchor);
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
            try
            {
                EcellObject eo = m_window.GetEcellObject(obj.EcellObject);
                eo.Key = newKey;
                PathUtil.SetLayout(eo, obj);
                
                if (eo is EcellVariable)
                    ResetAlias((EcellVariable)eo, (PPathwayVariable)obj);

                NotifyDataChanged(oldKey, eo, isRecorded, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                if (m_isAnimation)
                    m_animCon.SetPropForSimulation();
                throw new PathwayException("Error DataChange: " + oldKey, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="variable"></param>
        private void ResetAlias(EcellVariable ev, PPathwayVariable variable)
        {
            if (variable.Aliases.Count <= 0)
                return;

            ev.Aliases.Clear();
            foreach (PPathwayAlias alias in variable.Aliases)
            {
                EcellLayout layout = new EcellLayout();
                layout.X = alias.Left;
                layout.Y = alias.Top;
                layout.Layer = ((PPathwayLayer)alias.Parent).Name;
                ev.Aliases.Add(layout);
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
            m_unreachedFlag = true;
            m_window.NotifyDataChanged(oldKey, eo, isRecorded, isAnchor);
            if (m_unreachedFlag)
                throw new PathwayException("Unreached DataChangedEvent.");
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
            EcellProcess process = (EcellProcess)m_window.GetEcellObject(m_canvas.ModelID, proKey, EcellObject.PROCESS);
            // End if obj is null.
            if (null == process)
                return;

            // Get EcellReference List.
            List<EcellReference> refList = process.ReferenceList;
            List<EcellReference> newList = new List<EcellReference>();
            EcellReference oldRef = null;
            EcellReference newRef = null;

            foreach (EcellReference er in refList)
            {
                if (CheckReference(er, varKey, changeType, coefficient))
                    oldRef = er.Clone();
                else
                    newList.Add(er.Clone());
            }

            if (oldRef != null && changeType != RefChangeType.Delete)
            {
                newRef = oldRef.Clone();
                switch(changeType)
                {
                    case RefChangeType.SingleDir:
                        newRef.Coefficient = coefficient;
                        newRef.Name = PathUtil.GetNewReferenceName(newList, coefficient);
                        newList.Add(newRef);
                        if(oldRef.Coefficient != 0 && coefficient != 0 && oldRef.Coefficient != coefficient)
                            newList.Add(oldRef);
                        break;
                    case RefChangeType.BiDir:
                        EcellReference copyRef = newRef.Clone();
                        newRef.Coefficient = -1;
                        newRef.Name = PathUtil.GetNewReferenceName(newList, -1);
                        copyRef.Coefficient = 1;
                        copyRef.Name = PathUtil.GetNewReferenceName(newList, 1);
                        newList.Add(newRef);
                        newList.Add(copyRef);
                        break;
                }
            }
            else if(newRef == null)
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
            process.ReferenceList = newList;
            try
            {
                NotifyDataChanged(proKey, process, true, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new PathwayException(MessageResources.ErrCreateEdge);
            }

        }

        /// <summary>
        /// Check Selected reference
        /// </summary>
        /// <param name="er"></param>
        /// <param name="varKey"></param>
        /// <param name="changeType"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        private static bool CheckReference(EcellReference er, string varKey, RefChangeType changeType, int coefficient)
        {
            if (!varKey.Equals(er.Key))
                return false;
            if (changeType == RefChangeType.BiDir || coefficient == 0)
                return true;
            if (er.Coefficient == coefficient)
                return true;
            return false;
        }

        /// <summary>
        /// NotifyLayerChanged
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="layers"></param>
        /// <param name="isAnchored"></param>
        public void NotifyLayerChanged(string modelID, List<PPathwayLayer> layers, bool isAnchored)
        {
            EcellModel model = (EcellModel)m_window.GetEcellObject(modelID, null, EcellObject.MODEL);
            List<EcellLayer> elList = new List<EcellLayer>();
            model.Layers.Clear();
            foreach (PPathwayLayer layer in layers)
            {
                elList.Add(new EcellLayer(layer.Name, layer.Visible));
            }
            model.Layers = elList;
            bool isRecorded = !(m_status == ProjectStatus.Loading);
            NotifyDataChanged("", model, isRecorded, isRecorded && isAnchored);
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

        #region EventHandler for IsAnimationChange
        private EventHandler m_onAnimationChange;
        /// <summary>
        /// Event on canvas change.
        /// </summary>
        public event EventHandler AnimationChange
        {
            add { m_onAnimationChange += value; }
            remove { m_onAnimationChange -= value; }
        }
        /// <summary>
        /// Event on view mode change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAnimationChange(EventArgs e)
        {
            if (m_onAnimationChange != null)
                m_onAnimationChange(this, e);
        }
        private void RaiseAnimationChange()
        {
            EventArgs e = new EventArgs();
            OnAnimationChange(e);
        }
        #endregion

        #region EventHandler for ProjectStatusChange
        private EventHandler m_onProjectStatusChange;
        /// <summary>
        /// Event on canvas change.
        /// </summary>
        public event EventHandler ProjectStatusChange
        {
            add { m_onProjectStatusChange += value; }
            remove { m_onProjectStatusChange -= value; }
        }
        /// <summary>
        /// Event on view mode change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnProjectStatusChange(EventArgs e)
        {
            if (m_onProjectStatusChange != null)
                m_onProjectStatusChange(this, e);
        }
        /// <summary>
        /// Raise ProjectStatusChange event.
        /// </summary>
        private void RaiseProjectStatusChange()
        {
            EventArgs e = new EventArgs();
            OnProjectStatusChange(e);
        }
        #endregion

        #region private Methods.
        /// <summary>
        /// Create pathway canvas.
        /// </summary>
        /// <param name="model">the EcellModel.</param>
        private void CreateCanvas(EcellModel model)
        {
            // Create canvas
            Canvas = new CanvasControl(this, model.ModelID);
            m_layerView.ResetLayers(model.Layers);

            m_menu.ResetEventHandler();
            m_menu.Zoom(1f);
            RaiseCanvasChange();
        }

        /// <summary>
        /// Get the list of EcellObject in the target model.
        /// </summary>
        /// <returns></returns>
        internal List<EcellObject> GetObjectList()
        {
            List<EcellObject> list = new List<EcellObject>();
            list.AddRange(GetSystemList());
            list.AddRange(GetNodeList());
            list.AddRange(GetCommentList());
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
                systemList.Add(m_window.GetEcellObject(obj.EcellObject));
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
                nodeList.Add(m_window.GetEcellObject(obj.EcellObject));

            return nodeList;
        }

        /// <summary>
        /// Get the list of EcellObject in the target model.
        /// </summary>
        /// <returns>the list of EcellObject.</returns>
        private List<EcellObject> GetCommentList()
        {
            List<EcellObject> nodeList = new List<EcellObject>();
            foreach (PPathwayText obj in m_canvas.Texts.Values)
            {
                nodeList.Add(obj.EcellObject);
            }
            return nodeList;
        }

        /// <summary>
        /// Get a temporary key of EcellObject.
        /// </summary>
        /// <param name="type">The data type of EcellObject.</param>
        /// <param name="key">The key of EcellObject.</param>
        /// <returns>"TemporaryID"</returns> 
        public string GetCopiedID(string type, string key)
        {
            return m_window.DataManager.GetCopiedID(m_canvas.ModelID, type, key);
        }

        /// <summary>
        /// Get distance of two pointF
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        private static PointF GetDistance(PointF posA, PointF posB)
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
        /// 
        /// </summary>
        /// <param name="system"></param>
        private void DeleteSystemUnder(PPathwaySystem system)
        {
            CanvasControl canvas = system.Canvas;
            foreach (PPathwayObject obj in canvas.GetAllObjectUnder(system.EcellObject.Key))
                if (obj is PPathwaySystem)
                    DeleteSystemUnder((PPathwaySystem)obj);

            NotifyDataDelete(system, true);
        }

        /// <summary>
        /// Do object layout.
        /// </summary>
        /// <param name="algorithm">ILayoutAlgorithm</param>
        /// <param name="subIdx">int</param>
        /// <param name="isRecorded">Whether to record this change.</param>
        public void DoLayout(ILayoutAlgorithm algorithm, int subIdx, bool isRecorded)
        {
            List<EcellObject> systemList = GetSystemList();
            List<EcellObject> nodeList = new List<EcellObject>();
            int nodeNum = 0;
            // Check Selected nodes when the layout algorithm uses selected objects.
            if (algorithm.GetLayoutType() == LayoutType.Selected)
            {
                foreach (EcellObject node in GetNodeList())
                {
                    node.isFixed = m_canvas.GetObject(node.Key, node.Type).Selected;
                    nodeList.Add(node);
                    if (node.isFixed)
                        nodeNum++;
                }
            }
            else if (algorithm.GetLayoutType() == LayoutType.Whole)
            {
                foreach (EcellObject node in GetNodeList())
                {
                    node.isFixed = true;
                    nodeList.Add(node);
                    nodeNum++;
                }
            }
            // Do Layout
            try
            {
                algorithm.DoLayout(subIdx, false, systemList, nodeList);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Util.ShowNoticeDialog(MessageResources.ErrLayout);
                return;
            }

            // Set Layout.
            foreach (EcellObject system in systemList)
            {
                if (!system.isFixed)
                    continue;
                system.isFixed = false;
                NotifySetPosition(system);
            }
            int i = 0;
            int allcount = nodeList.Count;
            int count = 0;
            string mes = MessageResources.MessageLayout;
            Progress(mes, 100, 50);
            foreach (EcellObject node in nodeList)
            {
                if (!node.isFixed)
                {
                    Progress(mes, 100, count * 50 / allcount + 50);
                    count++;
                    continue;
                }

                i++;
                node.isFixed = false;

                if (isRecorded)
                {
                    if (i != nodeNum)
                        NotifyDataChanged(node.Key,node,isRecorded, false);
                    else
                        NotifyDataChanged(node.Key, node, isRecorded, true);
                }
                else
                {
                    if (i != nodeNum)
                        NotifySetPosition(node);
                    else
                        NotifySetPosition(node);
                }
                Progress(mes, 100, count * 50 / allcount + 50);
                count++;
            }
            Progress(mes, 100, 100);
        }

        #endregion
    }
}
