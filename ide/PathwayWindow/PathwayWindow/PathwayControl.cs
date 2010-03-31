//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using Ecell.Reporting;
using System.Xml;
using System.Windows.Forms;

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
        /// 
        /// </summary>
        private LayoutPane m_layout;

        /// <summary>
        /// ProjectStatus
        /// </summary>
        private ProjectStatus m_status = ProjectStatus.Uninitialized;

        /// <summary>
        /// -
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
        private bool m_isAnimation = false;

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

        private List<EdgeLoader> m_edgeLoaders = new List<EdgeLoader>();
        /// <summary>
        /// Report Session.
        /// </summary>
        private Ecell.Reporting.ReportingSession m_session = null;
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
        public Stencils Stencil
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
                PropertyNode compNode = new PropertyNode(MessageResources.WindowComponent);
                foreach (ComponentSetting s in m_csManager.DefaultComponentSettings)
                {
                    compNode.Nodes.Add(new PropertyNode(new ComponentSettingsPage(this, s)));
                }
                node.Nodes.Add(compNode);
//                node.Nodes.Add(new PropertyNode(m_animCon.AnimationSettingsPage));
//                node.Nodes.Add(new PropertyNode(this.ComponentSettingsPage));

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
                if (m_status == ProjectStatus.Loaded)
                {
                    if (m_session != null)
                        m_session.Clear();
                }
                else if (m_status == ProjectStatus.Running ||
                    m_status == ProjectStatus.Stepping)
                {
                    m_session = m_window.Environment.ReportManager.GetReportingSession(Constants.groupDynamic);
                }
                RaiseProjectStatusChange();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Ecell.Reporting.ReportingSession Session
        {
            get { return this.m_session; }
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
            SetNodeIcons();
            // Create menus
            m_menu = new MenuControl(this);
            // Set AnimationControl
            m_animCon = new AnimationControl(this);
            // Preparing Interfaces
            m_pathwayView = new PathwayView(this);
            m_layerView = new LayerView(this);
            m_toolBox = new Stencils(this);
            m_layout = new LayoutPane(m_window.Environment);
            m_layout.ApplyButton.Click += new EventHandler(ApplyButton_Click);

            m_window.PluginManager.Refresh += new EventHandler(PluginManager_Refresh);
        }

        void PluginManager_Refresh(object sender, EventArgs e)
        {
            m_canvas.RefreshEdges();
            m_animCon.ResetAnimationStatus();
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
                    m_canvas.RefreshEdges();
                    foreach (PPathwayProcess process in m_canvas.Processes.Values)
                    {
                        SetEdgeConnector(process);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw new PathwayException(MessageResources.ErrCreateComponent, e);
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


        internal void ApplyButton_Click(object sender, EventArgs e)
        {
            ILayoutAlgorithm algorithm = m_layout.Algorithm;
            algorithm.Panel.ApplyChange();

            if (m_status == ProjectStatus.Running
                || m_status == ProjectStatus.Stepping
                || m_status == ProjectStatus.Suspended)
            {
                Util.ShowErrorDialog(MessageResources.ErrOnSimulation);
                return;
            }

            int subIndex = algorithm.SubIndex;
            DoLayout(algorithm, subIndex, true);
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
            Window.Environment.ReportManager.SetProgress(100 * val / max);
        }

        /// <summary>
        /// Get DockContents of PathwayWindow.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<EcellDockContent> GetWindowForms()
        {
            return new EcellDockContent[] {
                m_pathwayView,
                m_layerView,
                m_toolBox,
                m_layout
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
            else if (eo.Type.Equals(EcellObject.PROJECT))
            {
                return;
            }
            // create new canvas
            else if (eo.Type.Equals(EcellObject.MODEL))
            {
                this.CreateCanvas((EcellModel)eo);
                return;
            }
            // Ignore system size.
            else if (eo.Type == Constants.xpathVariable && eo.LocalID == "SIZE")
            {
                return;
            }

            // Create PathwayObject and set to canvas.
            PPathwayObject obj = m_csManager.CreateNewComponent(eo.Type, eo.Layout.Figure);
            obj.Canvas = m_canvas;
            obj.EcellObject = eo;

            m_canvas.DataAdd(obj, eo.IsPosSet);
            NotifySetPosition(obj);

            // Update Animation.
            if (IsAnimation)
                m_animCon.SetAnimation();
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
                EcellModel model = (EcellModel)eo;
                m_layerView.ResetLayers(model.Layers);
                m_canvas.RefreshEdges();
                m_animCon.SetAnimationSettings((XmlElement)model.Animations);
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
            if (obj != null)
            {
                // Change data.
                if (!obj.Setting.Name.Equals(eo.Layout.Figure))
                    obj.Setting = m_csManager.GetSetting(eo.Type, eo.Layout.Figure);
                obj.EcellObject = eo;
                m_canvas.DataChanged(oldKey, eo.Key, obj);
            }

            // Update Animation.
            if (IsAnimation)
                m_animCon.SetAnimation();

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

            // Update Animation.
            if (IsAnimation)
                m_animCon.SetAnimation();
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
            m_canvas.SetLayer(obj, eo.Layer);
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
            m_menu.ResetEventHandler();
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
            // Reset Animation.
            m_animCon.Clear();
            m_menu.SetAnimation(false);

            // Clear Canvas dictionary.
            if (m_canvas != null)
                m_canvas.Dispose();
            Canvas = null;
            // Clear ComponentSettings
            m_csManager.ClearSettings();
            SetNodeIcons();
            // Clear ReportingSession.
            if (m_session != null)
            {
                m_session.Close();
                m_session = null;
            }
        }

        /// <summary>
        /// get bitmap image of this pathway.
        /// </summary>
        /// <returns>Bitmap of this pathway.</returns>
        public Bitmap Print()
        {
            if (m_canvas == null)
                return new Bitmap(1, 1);
            m_menu.ResetEventHandler();
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
            // Update Animation.
            if (IsAnimation)
                m_animCon.SetAnimation();

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
            // Update Animation.
            if (IsAnimation)
                m_animCon.SetAnimation();

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
            // Update Animation.
            if (IsAnimation)
                m_animCon.SetAnimation();

        }

        /// <summary>
        /// The event process when user reset select.
        /// </summary>
        public void ResetSelect()
        {
            if (m_canvas == null)
                return;
            m_canvas.ResetSelect();
            // Update Animation.
            if (IsAnimation)
                m_animCon.SetAnimation();

        }

        /// <summary>
        /// Reset object settings.
        /// </summary>
        internal void ResetObjectSettings()
        {
            if (m_canvas == null)
                return;
            SetNodeIcons();
            if (IsAnimation)
                m_animCon.SetAnimation();
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

            if (m_copiedNodes == null || m_copiedNodes.Count == 0)
                return;
            if (this.ProjectStatus != ProjectStatus.Loaded)
                return;

            String clipboardString = Util.GetClipBoardString(m_copiedNodes);
            Clipboard.SetText(clipboardString);
        }

        /// <summary>
        /// Set copied nodes.
        /// </summary>
        /// <returns>A list of copied nodes.</returns>
        private List<EcellObject> SetCopyingNodes()
        {
            List<EcellObject> copyNodes = new List<EcellObject>();
            List<string> sysList = new List<string>();
            //Copy Systems
            foreach (PPathwayObject obj in m_canvas.Systems.Values)
            {
                if (obj.EcellObject.Key == Constants.delimiterPath)
                    continue;
                if (m_canvas.SelectedNodes.Contains(obj) || copyNodes.Contains(obj.ParentObject.EcellObject))
                {
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));
                    sysList.Add(obj.EcellObject.Key);
                }
            }
            //Copy Variables
            foreach (PPathwayObject obj in m_canvas.Variables.Values)
                if ((m_canvas.SelectedNodes.Contains(obj) || sysList.Contains(obj.EcellObject.ParentSystemID)) && 
                    !copyNodes.Contains(obj.EcellObject))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));
            //Copy Processes
            foreach (PPathwayObject obj in m_canvas.Processes.Values)
                if ((m_canvas.SelectedNodes.Contains(obj) || sysList.Contains(obj.EcellObject.ParentSystemID)) && 
                    !copyNodes.Contains(obj.EcellObject))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));
            //Copy Texts
            foreach (PPathwayObject obj in Canvas.Texts.Values)
                if (m_canvas.SelectedNodes.Contains(obj))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));
            //Copy Stepper
            foreach (PPathwayObject obj in Canvas.Steppers.Values)
                if (m_canvas.SelectedNodes.Contains(obj))
                    copyNodes.Add(m_window.GetEcellObject(obj.EcellObject));

            // Reset logger and reference.
            foreach (EcellObject eo in copyNodes)
            {
                // Reset logger
                foreach (EcellData data in eo.Value)
                {
                    data.Logged = false;
                }
                if (!(eo is EcellProcess))
                    continue;

                // Reset Reference.
                EcellProcess ep = (EcellProcess)eo;
                List<EcellReference> list = new List<EcellReference>();
                foreach (EcellReference er in ep.ReferenceList)
                {
                    foreach (EcellObject referer in copyNodes)
                    {
                        if(referer.Type.Equals(EcellObject.VARIABLE) && referer.Key.Equals(er.Key))
                            list.Add(er);
                    }
                }
                ep.ReferenceList = list;
            }

            // sort systems.
            List<EcellObject> tempList = new List<EcellObject>();
            if (copyNodes.Count >= 2)
            {
                for (int i = 1; i < copyNodes.Count; i++)
                {
                    for (int j = 1; j < copyNodes.Count - i; j++)
                    {
                        EcellObject obj1 = copyNodes[j - 1];
                        EcellObject obj2 = copyNodes[j];
                        if (!(obj1 is EcellSystem) || !(obj2 is EcellSystem))
                            break;
                        if (obj1.X > obj2.X)
                        {
                            copyNodes[j - 1] = obj2;
                            copyNodes[j] = obj1;
                        }
                    }
                }
            }

            return copyNodes;
        }

        /// <summary>
        /// PasteNodes
        /// </summary>
        internal void PasteNodes(bool isContext)
        {
            string ddd = Clipboard.GetText();
            string modelID = m_window.DataManager.CurrentProject.Model.ModelID;
            this.m_copiedNodes = Util.GetClipboardObject(m_window.DataManager, modelID, ddd);
            if (this.m_copiedNodes == null || this.m_copiedNodes.Count == 0)
                return;
            if (this.ProjectStatus != ProjectStatus.Loaded)
                return;

            // Get LeftTop position.
            PointF leftTop = m_copiedNodes[0].PointF;
            foreach (EcellObject eo in m_copiedNodes)
            {
                if (eo.X < leftTop.X)
                    leftTop.X = eo.X;
                if (eo.Y < leftTop.Y)
                    leftTop.Y = eo.Y;
            }
            
            // Get position diff
            PointF diff = new PointF(10, 10);
            if (isContext)
                diff = GetDistance(this.m_mousePos, leftTop);
            PointF newPos = new PointF(leftTop.X + diff.X, leftTop.Y + diff.Y);

            // Get parent System
            string oldSysKey = m_copiedNodes[0].ParentSystemID;
            string newSysKey = m_canvas.GetSurroundingSystemKey(newPos);
            if (newSysKey == null)
                newSysKey = "/";

            PPathwaySystem system = null;
            system = m_canvas.Systems[newSysKey];
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
            Dictionary<string, string> skeyDic = new Dictionary<string, string>();
            Dictionary<string, string> pkeyDic = new Dictionary<string, string>();
            Dictionary<string, string> vkeyDic = new Dictionary<string, string>();
            foreach (EcellObject eo in copiedObjects)
            {
                //Create new EcellObject
                eo.ModelID = m_canvas.ModelID;
                eo.X = eo.X + diff.X;
                eo.Y = eo.Y + diff.Y;

                // Get new key.
                string oldKey = eo.Key;
                if( skeyDic.ContainsKey(eo.ParentSystemID))
                    eo.Key = GetCopiedID(eo.Type, Util.GetMovedKey(eo.Key, eo.ParentSystemID, skeyDic[eo.ParentSystemID]));
                else if (eo is EcellText || eo is EcellStepper)
                    eo.Key = GetCopiedID(eo.Type, eo.Key);
                else
                    eo.Key = GetCopiedID(eo.Type, Util.GetMovedKey(eo.Key, eo.ParentSystemID, newSysKey));
                // Set Keydic.
                switch (eo.Type)
                {
                    case EcellObject.PROCESS:
                        pkeyDic.Add(oldKey, eo.Key);
                        break;
                    case EcellObject.VARIABLE:
                        vkeyDic.Add(oldKey, eo.Key);
                        break;
                    case EcellObject.SYSTEM:
                        skeyDic.Add(oldKey, eo.Key);
                        break;
                }

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
                    {
                        EcellVariable var = (EcellVariable)child;
                        vkeyDic.Add(oldNodeKey, var.Key);
                        foreach (EcellLayout alias in var.Aliases)
                        {
                            alias.X = alias.X + diff.X;
                            alias.Y = alias.Y + diff.Y;
                        }
                    }
                    // Process
                    if (child is EcellProcess)
                    {
                        string classname = child.Classname;
                        foreach (EcellData d in child.Value)
                        {
                            if (d.Settable)
                                continue;
                            DMDescriptor dm = m_window.Environment.DMDescriptorKeeper.GetDMDescriptor(child.Type, classname);
                            d.Value = dm[d.Name].DefaultValue;
                        }
                    }

                }

                // Process and Stepper
                if (eo is EcellProcess || eo is EcellStepper)
                {
                    string classname = eo.Classname;
                    foreach (EcellData d in eo.Value)
                    {
                        if (d.Settable) continue;
                        DMDescriptor dm = m_window.Environment.DMDescriptorKeeper.GetDMDescriptor(eo.Type, classname);
                        d.Value = dm[d.Name].DefaultValue;                        
                    }
                }

                // Variable
                // Remove alias.
                if (eo is EcellVariable)
                {
                    EcellVariable var = (EcellVariable)eo;
                    var.Aliases.Clear();
                }
                // DataAdd
                eo.IsLayouted = false;
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
                    if (ReplaceVarRef(vkeyDic, p2))
                        processes.Add(p2);
                }
                if (!(obj is EcellProcess))
                    continue;
                EcellObject p1 = m_window.GetEcellObject(obj);
                if (ReplaceVarRef(vkeyDic, p1))
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
            bool delete = false;
            PPathwayEdge line = m_canvas.LineHandler.SelectedLine;
            if (line != null)
            {
                NotifyVariableReferenceChanged(
                    line.Info.ProcessKey,
                    line.Info.VariableKey,
                    RefChangeType.Delete,
                    0,
                    true);
                m_canvas.ResetSelectedLine();
                delete = true;
            }

            // Delete Selected Nodes
            List<PPathwayObject> slist = new List<PPathwayObject>();
            slist.AddRange(m_canvas.SelectedNodes);
            foreach (PPathwayObject deleteNode in slist)
            {
                NotifyDataDelete(deleteNode, false);
                delete = true;
            }
            if(delete)
                m_window.Environment.ActionManager.AddAction(new AnchorAction());
        }

        /// <summary>
        /// CutNodes
        /// </summary>
        internal void CutNodes()
        {
            if (this.ProjectStatus != ProjectStatus.Loaded)
                return;
            CopyNodes();
            if (m_copiedNodes == null || m_copiedNodes.Count == 0)
                return;


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
            List<ComponentSetting> settings = m_csManager.GetAllSettings();
            int i = 0;
            int count = settings.Count;
            bool flag;
            foreach (ComponentSetting cs in settings)
            {
                i++;
                flag = (i == count);
                string key = (cs.IsDefault)? cs.Type : cs.Name;
                m_window.PluginManager.SetIconImage(key, cs.Icon, flag);
            }
            m_csManager.SaveSettings();
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
                Debug.WriteLine(e.Message);
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
                m_canvas.PCanvas.Camera.Pickable = false;
                m_window.NotifyDataAdd(list, true);
                m_canvas.PCanvas.Camera.Pickable = true;
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
        /// 
        /// </summary>
        /// <param name="objList"></param>
        public void NotifyDataChanged(List<PPathwayObject> objList)
        {
            if (objList == null || objList.Count <= 0)
                return;
            m_canvas.PCanvas.Camera.Pickable = false;
            // Check Alias
            List<PPathwayObject> list = new List<PPathwayObject>();
            foreach (PPathwayObject obj in objList)
            {
                if (obj is PPathwayAlias)
                {
                    PPathwayAlias alias = (PPathwayAlias)obj;
                    if( !(objList.Contains(alias.Variable )) )
                        list.Add(alias.Variable);
                }
                else
                {
                    list.Add(obj);
                }
            }
            foreach (PPathwayObject obj in list)
            {
                NotifyDataChanged(obj, false);
            }
            // Set Anchor.
            m_window.Environment.ActionManager.AddAction(new AnchorAction());
            m_canvas.PCanvas.Camera.Pickable = true;
        }

        /// <summary>
        /// Notify DataChanged event to outside (PathwayControl -> PathwayWindow -> DataManager)
        /// To notify position or size change.
        /// </summary>
        /// <param name="obj">x coordinate of object.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(PPathwayObject obj, bool isAnchor)
        {
            if (obj is PPathwayAlias)
                obj = ((PPathwayAlias)obj).Variable;
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
                Debug.WriteLine("Key:" + oldKey + ", x:" + obj.X.ToString() + ", y:" + obj.Y.ToString() + ", OffsetX:" + obj.OffsetX.ToString() + ", OffsetY:" + obj.OffsetY.ToString());
                Debug.WriteLine("Key:" + oldKey + ", x:" + eo.X.ToString() + ", y:" + eo.Y.ToString());
                
                if (eo is EcellVariable)
                    ResetAlias((EcellVariable)eo, (PPathwayVariable)obj);

                NotifyDataChanged(oldKey, eo, isRecorded, isAnchor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                if (m_isAnimation)
                    m_animCon.SetAnimation();
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
            ev.Aliases.Clear();
            if (variable.Aliases.Count <= 0)
                return;

            foreach (PPathwayAlias alias in variable.Aliases)
            {
                EcellLayout layout = new EcellLayout();
                layout.Center = alias.Center;
                layout.Layer = alias.Layer.Name;
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
            // Check MassCalc
            if (process.Classname == EcellProcess.MASSCALCULATIONPROCESS)
            {
                foreach (EcellReference er in newList)
                {
                    er.Coefficient = 0;
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
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="isAnchored"></param>
        public void NotifyAnimaitionChanged(string modelID, bool isAnchored)
        {
            EcellModel model = (EcellModel)m_window.GetEcellObject(modelID, null, EcellObject.MODEL);
            XmlDocument doc = new XmlDocument();
            model.Animations = m_animCon.GetAnimationSettings(doc);
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
                if (obj is PPathwayAlias)
                {
                    PPathwayAlias alias = (PPathwayAlias)obj;
                    alias.Variable.Aliases.Remove(alias);
                    alias.RemoveFromParent();
                    foreach (PPathwayEdge edge in alias.Variable.Edges)
                    {
                        edge.VIndex = -1;
                    }
                    NotifyDataChanged(alias.Variable, isAnchor);
                }
                else
                {
                    NotifyDataDelete(obj.EcellObject, isAnchor);
                }
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
                Util.ShowErrorDialog(e.Message);
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
            if (m_canvas == null)
                return;
            List<EcellObject> systemList = GetSystemList();
            List<EcellObject> nodeList = new List<EcellObject>();
            int nodeNum = 0;
            // Check Selected nodes when the layout algorithm uses selected objects.
            if (algorithm.GetLayoutType() == LayoutType.Selected)
            {
                foreach (EcellObject node in GetNodeList())
                {
                    node.IsLayouted = m_canvas.GetObject(node.Key, node.Type).Selected;
                    nodeList.Add(node);
                    if (node.IsLayouted)
                        nodeNum++;
                }
            }
            else if (algorithm.GetLayoutType() == LayoutType.Whole)
            {
                foreach (EcellObject node in GetNodeList())
                {
                    node.IsLayouted = true;
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
                Debug.WriteLine(ex);
                Util.ShowNoticeDialog(MessageResources.ErrLayout);
                return;
            }

            // Set Layout.
            foreach (EcellObject system in systemList)
            {
                if (!system.IsLayouted)
                    continue;
                system.IsLayouted = false;
                NotifySetPosition(system);
            }
            int i = 0;
            int allcount = nodeList.Count;
            int count = 0;
            string mes = MessageResources.MessageLayout;
            Progress(mes, 100, 50);
            foreach (EcellObject node in nodeList)
            {
                if (!node.IsLayouted)
                {
                    Progress(mes, 100, count * 50 / allcount + 50);
                    count++;
                    continue;
                }

                i++;
                node.IsLayouted = false;
                PPathwayObject obj = m_canvas.GetObject(node.Key, node.Type);
                if (obj is PPathwayEntity)
                {
                    foreach (PPathwayEdge edge in ((PPathwayEntity)obj).Edges)
                    {
                        edge.VIndex = -1;
                        edge.PIndex = -1;
                    }
                }

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


        /// <summary>
        /// Save plugin settings to LEML.
        /// </summary>
        /// <returns></returns>
        public XmlNode GetPluginStatus()
        {
            XmlDocument doc =new XmlDocument();
            XmlElement status = doc.CreateElement(m_window.GetPluginName());
            // Save ComponentSettings.
            XmlElement componentList = doc.CreateElement(ComponentConstants.xPathComponentList);
            status.AppendChild(componentList);
            foreach (ComponentSetting cs in m_csManager.GetAllSettings())
            {
                if (cs.IsStencil || cs.IsDefault)
                    continue;
                componentList.AppendChild(ComponentManager.ConvertToXmlNode(doc, cs));
            }
            if(m_canvas == null)
                return status;

            // Save Edge connectors.
            XmlElement edgeList = doc.CreateElement(ComponentConstants.xPathEdgeList);
            status.AppendChild(edgeList);
            foreach (PPathwayProcess process in m_canvas.Processes.Values)
            {
                foreach (PPathwayEdge edge in process.Edges)
                {
                    if (edge.VIndex == -1 && edge.PIndex == -1)
                        continue;
                    XmlElement xmlEdge = doc.CreateElement(ComponentConstants.xPathEdge);
                    xmlEdge.SetAttribute("ProcessKey", edge.Info.ProcessKey);
                    xmlEdge.SetAttribute("VariableKey", edge.Info.VariableKey);
                    xmlEdge.SetAttribute("VIndex", edge.VIndex.ToString());
                    xmlEdge.SetAttribute("PIndex", edge.PIndex.ToString());
                    edgeList.AppendChild(xmlEdge);
                }
            }

            // Save Animation Settings.
            XmlElement animationSettings = m_animCon.GetAnimationSettings(doc);
            status.AppendChild(animationSettings);

            return status;
        }

        /// <summary>
        /// load plugin settings from LEML.
        /// </summary>
        /// <param name="status"></param>
        public void SetPluginStatus(XmlNode status)
        {
            // Load ComponentSettings.
            List<ComponentSetting> list = ComponentManager.LoadFromXML(status);
            m_csManager.UpdateComponent(list);
            SetNodeIcons();

            // Load Edge connectors.
            LoadEdgeConectors(status);

            // Load AnimationSettings.
            m_animCon.LoadAnimationSettings(status);
            //
            EcellModel model = m_window.DataManager.CurrentProject.Model;
            XmlDocument doc = new XmlDocument();
            model.Animations = m_animCon.GetAnimationSettings(doc);
        }

        /// <summary>
        /// Load Edge connectors from LEML.
        /// </summary>
        /// <param name="status"></param>
        private void LoadEdgeConectors(XmlNode status)
        {
            m_edgeLoaders.Clear();
            XmlElement edgeList = null;
            foreach (XmlNode node in status.ChildNodes)
            {
                if (node.Name.Equals(ComponentConstants.xPathEdgeList))
                    edgeList = (XmlElement)node;
            }
            if (edgeList == null)
                return;

            // Set edge connectors.
            List<EdgeInfo> list = new List<EdgeInfo>();
            foreach (XmlElement xmlEdge in edgeList.ChildNodes)
            {
                EdgeLoader loader = new EdgeLoader();
                loader.ProcessKey = xmlEdge.GetAttribute("ProcessKey");
                loader.VariableKey = xmlEdge.GetAttribute("VariableKey");
                int.TryParse(xmlEdge.GetAttribute("PIndex"), out loader.PIndex);
                int.TryParse(xmlEdge.GetAttribute("VIndex"), out loader.VIndex);
                m_edgeLoaders.Add(loader);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        private void SetEdgeConnector(PPathwayProcess process)
        {
            foreach (PPathwayEdge edge in process.Edges)
            {
                foreach (EdgeLoader loader in m_edgeLoaders)
                {
                    if (edge.Info.VariableKey != loader.VariableKey || edge.Info.ProcessKey != loader.ProcessKey)
                        continue;
                    edge.VIndex = loader.VIndex;
                    edge.PIndex = loader.PIndex;
                    edge.Refresh();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal class EdgeLoader
        {
            internal string ProcessKey = "";
            internal string VariableKey = "";
            internal int PIndex = -1;
            internal int VIndex = -1;
        }
    }
}
