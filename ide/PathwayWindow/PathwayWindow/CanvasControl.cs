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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.PiccoloX.Components;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Exceptions;

namespace Ecell.IDE.Plugins.PathwayWindow
{
    /// <summary>
    /// This class manages resources related to one canvas.
    ///  ex) PathwayCanvas, Layer, etc.
    /// </summary>
    public class CanvasControl : IDisposable
    {
        #region Constant
        /// <summary>
        /// Least canvas size when a node is focused.
        /// </summary>
        private const float LEAST_FOCUS_SIZE = 560f;

        /// <summary>
        /// Duration for camera centering animation when a node is selected.
        /// this will be used for the argument of PCamera.AnimateViewToCenterBounds()
        /// </summary>
        private const int CAMERA_ANIM_DURATION = 700;

        /// <summary>
        /// Minimum scale.
        /// </summary>
        private const float MIN_SCALE = .1f;

        /// <summary>
        /// Maximum scale
        /// </summary>
        private const float MAX_SCALE = 5;
        #endregion

        #region Fields
        /// <summary>
        /// The PathwayView, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        private PathwayControl m_con;

        /// <summary>
        /// PCanvas for pathways.
        /// </summary>
        private PPathwayCanvas m_pCanvas;

        /// <summary>
        /// the canvas of overview.
        /// </summary>
        private POverviewCanvas m_overviewCanvas;

        /// <summary>
        /// The dictionary for all systems on this canvas.
        /// </summary>
        protected SortedDictionary<string, PPathwaySystem> m_systems = new SortedDictionary<string, PPathwaySystem>();

        /// <summary>
        /// The dictionary for all variables on this canvas.
        /// </summary>
        protected SortedDictionary<string, PPathwayVariable> m_variables = new SortedDictionary<string, PPathwayVariable>();

        /// <summary>
        /// The dictionary for all processes on this canvas.
        /// </summary>
        protected SortedDictionary<string, PPathwayProcess> m_processes = new SortedDictionary<string, PPathwayProcess>();

        /// <summary>
        /// The dictionary for all comments
        /// </summary>
        protected SortedDictionary<string, PPathwayText> m_texts = new SortedDictionary<string, PPathwayText>();

        /// <summary>
        /// The dictionary for all layers.
        /// </summary>
        protected Dictionary<string, PPathwayLayer> m_layers;

        /// <summary>
        /// PLayer for control use.
        /// For example, resize handlers for System.
        /// </summary>
        protected PPathwayLayer m_ctrlLayer;

        /// <summary>
        /// PLayer for system.
        /// </summary>
        protected PPathwayLayer m_sysLayer;

        /// <summary>
        /// List of PPathwayNode for selected object.
        /// </summary>
        List<PPathwayObject> m_selectedNodes = new List<PPathwayObject>();

        /// <summary>
        /// The unique ID of this canvas.
        /// </summary>
        private string m_modelId;

        /// <summary>
        /// Whether each node is showing it's ID or not;
        /// </summary>
        private bool m_showingId = true;

        /// <summary>
        /// SelectChange flag.
        /// </summary>
        bool m_isOwner = false;

        /// <summary>
        /// ViewMode flag.
        /// </summary>
        bool m_isViewMode = false;

        /// <summary>
        /// Line handle on the end for a variable
        /// </summary>
        private LineHandler m_lineHandler = null;

        /// <summary>
        /// Clicked PathwayObject.
        /// </summary>
        private PNode m_focusNode = null;
        /// <summary>
        /// BackGroundBrush
        /// </summary>
        private Brush m_bgBrush = null;

        /// <summary>
        /// Focus Mode
        /// </summary>
        private bool m_focusMode = true;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_canvasId.
        /// </summary>
        public string ModelID
        {
            get { return m_modelId; }
            set { m_modelId = value; }
        }

        /// <summary>
        /// Accessor for m_pathwayView.
        /// </summary>
        public PathwayControl Control
        {
            get { return m_con; }
            set { m_con = value; }
        }

        /// <summary>
        /// Accessor for m_pathwayCanvas.
        /// </summary>
        public PCanvas PCanvas
        {
            get { return m_pCanvas; }
        }

        /// <summary>
        /// Accessor for m_overviewCanvas.
        /// </summary>
        public POverviewCanvas OverviewCanvas
        {
            get { return m_overviewCanvas; }
        }

        /// <summary>
        /// Accessor for m_clickedNode.
        /// </summary>
        public PNode FocusNode
        {
            get { return m_focusNode; }
            set { this.m_focusNode = value; }
        }

        /// <summary>
        /// Accessor for m_selectedNodes.
        /// </summary>
        public List<PPathwayObject> SelectedNodes
        {
            get { return m_selectedNodes; }
        }

        /// <summary>
        /// Accessor for m_ctrlLayer.
        /// </summary>
        public PLayer ControlLayer
        {
            get { return m_ctrlLayer; }
        }

        /// <summary>
        /// Accessor for m_layers.
        /// </summary>
        public Dictionary<string, PPathwayLayer> Layers
        {
            get { return m_layers; }
        }

        /// <summary>
        /// Accessor for m_systems.
        /// </summary>
        public SortedDictionary<string, PPathwaySystem> Systems
        {
            get { return m_systems; }
        }

        /// <summary>
        /// Accessor for m_variables.
        /// </summary>
        public SortedDictionary<string, PPathwayVariable> Variables
        {
            get { return m_variables; }
        }

        /// <summary>
        /// Accessor for m_processes.
        /// </summary>
        public SortedDictionary<string, PPathwayProcess> Processes
        {
            get { return m_processes; }
        }

        /// <summary>
        /// Accessor for m_comments.
        /// </summary>
        public SortedDictionary<string, PPathwayText> Texts
        {
            get { return m_texts; }
            set { m_texts = value; }
        }

        /// <summary>
        /// Accessor for m_showingId.
        /// </summary>
        public bool ShowingID
        {
            get { return m_showingId; }
            set
            {
                m_showingId = value;
                foreach (PPathwayNode node in m_variables.Values)
                    node.ShowingID = m_showingId;
                foreach (PPathwayNode node in m_processes.Values)
                    node.ShowingID = m_showingId;
                foreach (PPathwaySystem system in m_systems.Values)
                    system.ShowingID = m_showingId;
            }
        }

        /// <summary>
        /// Accessor for m_focusMode.
        /// </summary>
        public bool FocusMode
        {
            get { return m_focusMode; }
            set { m_focusMode = value; }
        }

        /// <summary>
        /// BackGroundBrush
        /// </summary>
        public Brush BackGroundBrush
        {
            get { return m_bgBrush; }
            set
            {
                if (m_pCanvas == null)
                    return;
                m_bgBrush = value;
                m_pCanvas.BackColor = BrushManager.ParseBrushToColor(value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public LineHandler LineHandler
        {
            get { return m_lineHandler; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// the constructor with initial parameters.
        /// </summary>
        /// <param name="control">PathwayControl.</param>
        /// <param name="modelID">Model id.</param>
        public CanvasControl(PathwayControl control, string modelID)
        {
            m_con = control;
            m_con.ViewModeChange += new EventHandler(Control_ViewModeChange);

            m_modelId = modelID;

            // Preparing PathwayViewCanvas
            m_pCanvas = new PPathwayCanvas(this);
            // Preparing OverviewCanvas
            m_overviewCanvas = new POverviewCanvas(this);
            m_pCanvas.Camera.RemoveLayer(m_pCanvas.Layer);
            m_pCanvas.Camera.ViewTransformChanged += new PPropertyEventHandler(Camera_ViewChanged);
            m_pCanvas.Camera.BoundsChanged += new PPropertyEventHandler(Camera_ViewChanged);
            m_pCanvas.BackColor = BrushManager.ParseBrushToColor(m_bgBrush);

            // Preparing layer list
            m_layers = new Dictionary<string, PPathwayLayer>();

            // Preparing system layer
            m_sysLayer = new PPathwayLayer("");
            AddLayer(m_sysLayer);
            // Preparing control layer
            m_ctrlLayer = new PPathwayLayer("");
            m_pCanvas.Root.AddChild(m_ctrlLayer);
            m_pCanvas.Camera.AddLayer(m_ctrlLayer);

            // Preparing system ResizeHandlers
            m_lineHandler = new LineHandler(this);

            // Set ViewMode
            m_isViewMode = m_con.ViewMode;
            m_showingId = m_con.ShowingID;
            m_focusMode = m_con.FocusMode;
            ResetObjectSettings();
        }

        /// <summary>
        /// RefreshEdges
        /// </summary>
        internal void RefreshEdges()
        {
            foreach (PPathwayProcess process in m_processes.Values)
            {
                process.ResetEdges();
            }
        }

        /// <summary>
        /// Event on Viewmode change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Control_ViewModeChange(object sender, EventArgs e)
        {
            m_isViewMode = m_con.ViewMode;
            ResetObjectSettings();
        }

        #endregion

        /// <summary>
        /// Notify SelectChanged event to outside.
        /// <param name="obj">the selected object.</param>
        /// </summary>
        public void NotifySelectChanged(PPathwayObject obj)
        {
            if (obj.EcellObject == null)
                return;
            m_isOwner = true;
            m_con.Window.NotifySelectChanged(
                this.m_modelId, 
                obj.EcellObject.Key, 
                obj.EcellObject.Type);
        }

        /// <summary>
        /// Notify SelectChanged event to outside.
        /// <param name="obj">the selected object.</param>
        /// </summary>
        public void NotifyAddSelect(PPathwayObject obj)
        {
            if (obj.EcellObject == null)
                return;
            m_con.Window.NotifyAddSelect(
                this.m_modelId, 
                obj.EcellObject.Key, 
                obj.EcellObject.Type);
        }

        /// <summary>
        /// Notify SelectChanged event to outside.
        /// <param name="obj">the selected object.</param>
        /// </summary>
        public void NotifyRemoveSelect(PPathwayObject obj)
        {
            if (obj.EcellObject == null)
                return;
            m_con.Window.NotifyRemoveSelect(
                this.m_modelId,
                obj.EcellObject.Key,
                obj.EcellObject.Type);
        }

        /// <summary>
        /// Check if any system of this canvas overlaps given rectangle.
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
        public bool DoesSystemOverlaps(RectangleF rect)
        {
            bool isOverlaping = false;
            foreach (PPathwaySystem system in m_systems.Values)
                if (system.Overlaps(rect))
                    return true;
            return isOverlaping;
        }

        /// <summary>
        /// Check if any system of this canvas (exclude specified system) overlaps given rectangle.
        /// </summary>
        /// <param name="system">PPathwaySystem to be checked</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, false otherwise</returns>
        public bool DoesSystemOverlaps(PPathwaySystem system)
        {
            bool isOverlaping = false;
            foreach (PPathwaySystem sys in m_systems.Values)
                if (system != sys && system.Overlaps(sys.Rect))
                    return true;
            return isOverlaping;
        }

        /// <summary>
        /// Check if any system of this canvas overlaps given rectangle.
        /// </summary>
        /// <param name="systemName">Parent system.</param>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
        public bool DoesSystemOverlaps(string systemName, RectangleF rect)
        {
            bool isOverlaping = false;
            foreach (PPathwaySystem system in m_systems.Values)
            {
                if (!system.EcellObject.Key.StartsWith(systemName) || system.EcellObject.Key.Equals(systemName))
                    continue;
                if (system.Overlaps(rect))
                    isOverlaping = true;
            }
            return isOverlaping;
        }
        /// <summary>
        /// Check if any system of this canvas overlaps given rectangle.
        /// </summary>
        /// <param name="key">key of EcellObject</param>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
        public bool DoesSystemContains(string key, RectangleF rect)
        {
            bool contains = true;
            foreach (PPathwaySystem system in m_systems.Values)
            {
                if (key.StartsWith(system.EcellObject.Key))
                    contains = contains & system.Rect.Contains(rect);
                else if (!key.Equals(system.EcellObject.Key))
                    contains = contains & !system.Rect.Contains(rect);
            }
            return contains;
        }
        /// <summary>
        /// Check if any system of this canvas overlaps given rectangle.
        /// </summary>
        /// <param name="key">key of EcellObject</param>
        /// <param name="point">RectangleF to be checked</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
        public bool DoesSystemContains(string key, PointF point)
        {
            bool contains = true;
            foreach (PPathwaySystem system in m_systems.Values)
            {
                if (key.StartsWith(system.EcellObject.Key))
                    contains = contains & system.Rect.Contains(point);
                else if (!key.Equals(system.EcellObject.Key))
                    contains = contains & !system.Rect.Contains(point);
            }
            return contains;
        }
        /// <summary>
        /// Convert CanvasPos to SystemPos
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Point CanvasPosToSystemPos(PointF pos)
        {
            int width = m_pCanvas.Size.Width;
            int height = m_pCanvas.Size.Height;
            RectangleF canvasRect = m_pCanvas.Camera.ViewBounds;
            int x = m_pCanvas.Location.X + (int)((pos.X - canvasRect.X) / canvasRect.Width * width);
            int y = m_pCanvas.Location.Y + (int)((pos.Y - canvasRect.Y) / canvasRect.Height * height);
            return new Point(x, y);
        }
        /// <summary>
        /// Convert SystemPos to CanvasPos
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public PointF SystemPosToCanvasPos(Point pos)
        {
            int width = m_pCanvas.Width;
            int height = m_pCanvas.Height;
            Point location = m_con.PathwayView.GetDesktopLocation();
            RectangleF canvasRect = m_pCanvas.Camera.ViewBounds;
            float x = canvasRect.X + ((float)(pos.X - location.X) / (float)width * canvasRect.Width);
            float y = canvasRect.Y + ((float)(pos.Y - location.Y) / (float)height * canvasRect.Height);
            return new PointF(x, y);
        }

        /// <summary>
        /// Add PPathwayObject to this canvas.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="hasCoords"></param>
        public void DataAdd(PPathwayObject obj, bool hasCoords)
        {
            // Set Layer
            obj.Canvas = this;
            obj.ShowingID = m_showingId;
            obj.ViewMode = m_isViewMode;
            obj.AddInputEventListener(new NodeDragHandler(this));
            SetLayer(obj);

            RegisterObject(obj);
            if (obj is PPathwayNode)
                ((PPathwayNode)obj).ShowingID = this.m_showingId;

            // Set ParentObject.
            string sysKey = obj.EcellObject.ParentSystemID;
            PPathwaySystem system = null;
            if (!string.IsNullOrEmpty(sysKey))
            {
                system = m_systems[sysKey];
                obj.ParentObject = system;
            }
            // If obj hasn't coordinate, it will be settled. 
            if (obj is PPathwayNode)
            {
                if (m_con.Window.IsLoading)
                {
                }
                else if (!hasCoords)
                {
                    obj.CenterPointF = GetVacantPoint(sysKey);
                }
                else if (!obj.EcellObject.isFixed)
                {
                    MakeSpace(system, obj, false);
                }
                else if (!DoesSystemContains(sysKey, obj.CenterPointF))
                {
                    obj.CenterPointF = GetVacantPoint(sysKey, obj.CenterPointF);
                }
            }
            if (obj is PPathwaySystem)
            {
                if (!hasCoords && !string.IsNullOrEmpty(sysKey))
                {
                    float maxX = system.X + system.OffsetX;
                    float x = 0f;
                    List<PPathwayObject> list = GetAllObjectUnder(system.EcellObject.Key);
                    foreach (PPathwayObject child in list)
                    {
                        if (child == obj)
                            continue;
                        x = child.X + child.Width;
                        if (maxX < x)
                            maxX = x;
                    }
                    // Set obj's coordinate
                    obj.X = maxX + PPathwaySystem.SYSTEM_MARGIN;
                    obj.Y = system.Y + PPathwaySystem.SYSTEM_MARGIN;
                    SetSystemSize(obj);
                }
                else if (!hasCoords)
                    SetSystemSize(obj);

                if (!obj.EcellObject.isFixed)
                    MakeSpace(system, obj, false);
            }
            obj.Refresh();
            // Refresh OverView.
            RefreshOverView();
        }

        /// <summary>
        /// register the object to this set.
        /// </summary>
        /// <param name="obj">the registered object.</param>
        private void RegisterObject(PPathwayObject obj)
        {
            if (obj is PPathwaySystem)
            {
                PPathwaySystem system = (PPathwaySystem)obj;
                if (m_systems.ContainsKey(system.EcellObject.Key))
                    throw new PathwayException(string.Format(
                        MessageResources.ErrAlrExist,
                        new object[] { system.EcellObject.Key }));
                m_systems.Add(system.EcellObject.Key, system);
            }
            else if (obj is PPathwayVariable)
            {
                PPathwayVariable node = (PPathwayVariable)obj;
                if (m_variables.ContainsKey(node.EcellObject.Key))
                    throw new PathwayException(string.Format(
                        MessageResources.ErrAlrExist,
                        new object[] { node.EcellObject.Key }));
                m_variables.Add(node.EcellObject.Key, node);
            }
            else if (obj is PPathwayProcess)
            {
                PPathwayProcess node = (PPathwayProcess)obj;
                if (m_processes.ContainsKey(node.EcellObject.Key))
                    throw new PathwayException(string.Format(
                        MessageResources.ErrAlrExist,
                        new object[] { node.EcellObject.Key }));
                m_processes.Add(node.EcellObject.Key, node);
            }
            else if (obj is PPathwayText)
            {
                PPathwayText node = (PPathwayText)obj;
                if (m_processes.ContainsKey(node.EcellObject.Key))
                    throw new PathwayException(string.Format(
                        MessageResources.ErrAlrExist,
                        new object[] { node.EcellObject.Key }));
                m_texts.Add(node.EcellObject.Key, node);
            }
        }

        /// <summary>
        /// Set the system size.
        /// </summary>
        /// <param name="system">The system object.</param>
        private static void SetSystemSize(PPathwayObject system)
        {
            int length = 200 * (int)Math.Sqrt(system.EcellObject.Children.Count);
            if (length > PPathwaySystem.DEFAULT_WIDTH)
            {
                system.Width = length;
                system.Height = length;
            }
            else
            {
                system.Width = PPathwaySystem.DEFAULT_WIDTH;
                system.Height = PPathwaySystem.DEFAULT_HEIGHT;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="system"></param>
        /// <param name="obj"></param>
        /// <param name="isDataChanged"></param>
        private void MakeSpace(PPathwaySystem system, PPathwayObject obj, bool isDataChanged)
        {
            // Offset position of given object.
            PointF offset = PointF.Empty;
            //if (obj.X <= system.Left + PPathwaySystem.SYSTEM_MARGIN)
            //    obj.OffsetX = system.Left + PPathwaySystem.SYSTEM_MARGIN - obj.X;
            //if (obj.Y <= system.Top + PPathwaySystem.SYSTEM_MARGIN)
            //    obj.OffsetY = system.Top + PPathwaySystem.SYSTEM_MARGIN - obj.Y;
            // Enlarge this system
            //if (system.Right < obj.Right + PPathwaySystem.SYSTEM_MARGIN)
            //    system.Width = obj.Right + PPathwaySystem.SYSTEM_MARGIN - system.X;
            //if (system.Bottom < obj.Bottom + PPathwaySystem.SYSTEM_MARGIN)
            //    system.Height = obj.Bottom + PPathwaySystem.SYSTEM_MARGIN - system.Y;
            float margin = (isDataChanged) ? 1 : PPathwaySystem.SYSTEM_MARGIN;
            if (system.Right < obj.Right)
                system.Width = obj.Right - system.X + margin;
            if (system.Bottom < obj.Bottom)
                system.Height = obj.Bottom - system.Y + margin;

            // Check Intersecting objects.
            List<PPathwayObject> list = GetAllObjectUnder(system.EcellObject.Key);
            bool enlargeFlag = false;
            float offsetx = 0;
            RectangleF rect = obj.Rect;
            foreach (PPathwayObject child in list)
            {
                if (child is PPathwayText)
                    continue;
                if (child.EcellObject.Key.Equals(obj.EcellObject.Key))
                    continue;
                if (!child.EcellObject.ParentSystemID.Equals(system.EcellObject.Key))
                    continue;

                if (rect.IntersectsWith(child.Rect) || rect.Contains(child.Rect) || child.Rect.Contains(rect))
                {
                    enlargeFlag = true;
                    offsetx = obj.Right + PPathwaySystem.SYSTEM_MARGIN - child.X;
                    if (offsetx > offset.X)
                        offset.X = offsetx;
                }
            }
            // Make new space.
            if (enlargeFlag)
            {
                foreach (PPathwayObject child in list)
                {
                    if (child is PPathwayText)
                        continue;
                    if (child.EcellObject.Key.Equals(obj.EcellObject.Key))
                        continue;
                    if (!child.EcellObject.ParentSystemID.Equals(system.EcellObject.Key))
                        continue;

                    if (child.X >= obj.X || rect.IntersectsWith(child.Rect) || rect.Contains(child.Rect) || child.Rect.Contains(rect))
                        child.OffsetX = offset.X;

                    if (child.OffsetX == 0 && child.OffsetY == 0)
                        continue;

                    offset = child.Offset;
                    m_con.NotifyDataChanged(child, false);

                    // Enlarge this system
                    //if (system.Right < child.Right + PPathwaySystem.SYSTEM_MARGIN)
                    //    system.Width = child.Right + PPathwaySystem.SYSTEM_MARGIN - system.X;
                    //if (system.Bottom < child.Bottom + PPathwaySystem.SYSTEM_MARGIN)
                    //    system.Height = child.Bottom + PPathwaySystem.SYSTEM_MARGIN - system.Y;
                    if (system.Right < child.Right)
                        system.Width = child.Right - system.X + 1;
                    if (system.Bottom < child.Bottom)
                        system.Height = child.Bottom - system.Y + 1;

                    // Move system's children.
                    if (!(child is PPathwaySystem))
                        continue;
                    foreach (PPathwayObject grandchild in GetAllObjectUnder(child.EcellObject.Key))
                    {
                        grandchild.Offset = offset;
                        m_con.NotifyDataChanged(grandchild, false);
                    }
                }
            }
            m_con.NotifyDataChanged(system, false);

            // Make parent system create space for this system.
            if (system.ParentObject != null)
                MakeSpace(system.ParentObject, system, isDataChanged);
        }

        #region Methods to control Layer.
        /// <summary>
        /// add the selected layer.
        /// </summary>
        /// <param name="name">the name of layer.</param>
        /// <param name="visible">the visibility of layer.</param>
        public void AddLayer(string name, bool visible)
        {
            // Error check.
            if (string.IsNullOrEmpty(name))
                throw new PathwayException("Error null string.");
            if (m_layers.ContainsKey(name))
                throw new PathwayException(string.Format(MessageResources.ErrAlrExist, name));

            PPathwayLayer layer = new PPathwayLayer(name);
            layer.Visible = visible;
            AddLayer(layer);
            AddLayer(m_ctrlLayer);
            m_layers.Add(layer.Name, layer);
        }

        /// <summary>
        /// Add new layer.
        /// </summary>
        /// <param name="layer"></param>
        private void AddLayer(PLayer layer)
        {
            m_pCanvas.Root.AddChild(layer);
            if (m_pCanvas.Camera.LayersReference.Contains(layer))
                m_pCanvas.Camera.RemoveLayer(layer);
            m_pCanvas.Camera.AddLayer(layer);
            m_overviewCanvas.AddObservedLayer(layer);
        }

        /// <summary>
        /// Delete selected layer.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveLayer(string name)
        {
            PPathwayLayer layer = m_layers[name];

            // Delete Nodes under this layer
            List<PPathwayObject> list = layer.GetNodes();
            m_selectedNodes.Clear();
            m_selectedNodes.AddRange(list);
            m_con.DeteleNodes();

            // Delete Layer.
            m_layers.Remove(name);
            m_overviewCanvas.RemoveObservedLayer(layer);
            m_pCanvas.Camera.RemoveLayer(layer);
            m_pCanvas.Root.RemoveChild(layer);
        }

        /// <summary>
        /// 
        /// Set Layer Visibility.
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="isShown"></param>
        public void ChangeLayerVisibility(string layerName, bool isShown)
        {
            if (!m_layers.ContainsKey(layerName))
                return;
            PPathwayLayer layer = m_layers[layerName];
            // Set Visibility.
            layer.Visible = isShown;
            NotifyLayerChange(true);
        }

        /// <summary>
        /// Set Layer.
        /// </summary>
        /// <param name="obj"></param>
        public void SetLayer(PPathwayObject obj)
        {
            // if obj is root system or layerID is null.
            PPathwayLayer layer = m_sysLayer;
            string layerID = obj.EcellObject.Layer;
            if (obj.EcellObject.Key.Equals(Constants.delimiterPath) || string.IsNullOrEmpty(layerID))
            {
                obj.Layer = layer;
                obj.Layer.AddChild(obj);
                return;
            }

            // Set new layer.
            if (m_layers.ContainsKey(layerID))
                layer = m_layers[layerID]; //throw new PathwayException(string.Format("Layer [{0}] not found.", layerID));
            obj.Layer = layer;
        }

        /// <summary>
        /// LayerMoveToFront
        /// </summary>
        /// <param name="name"></param>
        public void LayerMoveToFront(string name)
        {
            if (!m_layers.ContainsKey(name))
                return;
            PLayer layer = m_layers[name];
            LayerMoveToFront(layer);
        }

        /// <summary>
        /// LayerMoveToFront
        /// </summary>
        /// <param name="layer"></param>
        public void LayerMoveToFront(PLayer layer)
        {
            AddLayer(layer);
            AddLayer(m_ctrlLayer);
            NotifyLayerChange(true);
            m_con.Canvas.OverviewCanvas.Refresh();
            NotifyLayerChange(true);
        }

        /// <summary>
        /// LayerMoveToBack
        /// </summary>
        /// <param name="name"></param>
        public void LayerMoveToBack(string name)
        {
            if (!m_layers.ContainsKey(name))
                return;
            PLayer layer = m_layers[name];
            LayerMoveToBack(layer);
        }

        /// <summary>
        /// LayerMoveToBack
        /// </summary>
        /// <param name="layer"></param>
        public void LayerMoveToBack(PLayer layer)
        {
            AddLayer(layer);
            foreach (PPathwayLayer obj in m_layers.Values)
                if (obj != layer)
                    AddLayer(obj);
            AddLayer(m_ctrlLayer);
            NotifyLayerChange(true);
        }

        /// <summary>
        /// Get a list of layers.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetLayerNameList()
        {
            List<string> list = new List<string>();
            foreach (PPathwayLayer layer in m_layers.Values)
                list.Add(layer.Name);

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isAnchored"></param>
        internal void NotifyAddLayer(string name, bool isAnchored)
        {
            // Error check.
            if (string.IsNullOrEmpty(name))
                return;
            if (m_layers.ContainsKey(name))
            {
                Util.ShowNoticeDialog(name + MessageResources.ErrAlrExist);
                return;
            }
            PPathwayLayer layer = new PPathwayLayer(name);
            AddLayer(layer);
            AddLayer(m_ctrlLayer);
            m_layers.Add(layer.Name, layer);
            NotifyLayerChange(isAnchored);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isAnchored"></param>
        internal void NotifyRemoveLayer(string name, bool isAnchored)
        {
            PPathwayLayer layer = m_layers[name];

            // Delete Nodes under this layer
            List<PPathwayObject> list = layer.GetNodes();
            foreach (PPathwayObject obj in list)
            {
                obj.Layer = m_sysLayer;
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    obj.EcellObject.Key,
                    obj,
                    true,
                    false);
            }

            // Delete Layer.
            m_layers.Remove(name);
            m_overviewCanvas.RemoveObservedLayer(layer);
            m_pCanvas.Camera.RemoveLayer(layer);
            m_pCanvas.Root.RemoveChild(layer);
            NotifyLayerChange(isAnchored);
        }

        /// <summary>
        /// Rename selected layer.
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void NotifyRenameLayer(string oldName, string newName)
        {
            NotifyAddLayer(newName, false);
            PPathwayLayer oldlayer = m_layers[oldName];
            PPathwayLayer newlayer = m_layers[newName];
            // Change Nodes under this layer
            List<PPathwayObject> list = oldlayer.GetNodes();
            foreach (PPathwayObject obj in list)
            {
                obj.Layer = newlayer;
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    obj.EcellObject.Key,
                    obj,
                    true,
                    false);
            }
            NotifyRemoveLayer(oldName, true);
        }

        /// <summary>
        /// Merge two layers
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        internal void NotifyMergeLayer(string oldName, string newName)
        {
            if (oldName.Equals(newName))
                return;

            PPathwayLayer oldlayer = m_layers[oldName];
            PPathwayLayer newlayer = m_layers[newName];
            // Change Nodes under this layer
            List<PPathwayObject> list = oldlayer.GetNodes();
            foreach (PPathwayObject obj in list)
            {
                obj.Layer = newlayer;
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    obj.EcellObject.Key,
                    obj,
                    true,
                    false);
            }
            NotifyRemoveLayer(oldName, true);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void NotifyLayerChange(bool isAnchored)
        {
            List<PPathwayLayer> layers = new List<PPathwayLayer>();
            foreach (PNode obj in m_pCanvas.Root.ChildrenReference)
            {
                if (!(obj is PPathwayLayer))
                    continue;
                PPathwayLayer layer = (PPathwayLayer)obj;
                if (string.IsNullOrEmpty(layer.Name))
                    continue;
                layers.Add(layer);
            }
            m_con.NotifyLayerChanged(this.m_modelId, layers, isAnchored);
        }
        #endregion
        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <param name="excludedSystem">If this parameter is set, this system is excluded from searching</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public PPathwayObject GetSurroundingSystem(PointF point, string excludedSystem)
        {
            PPathwayObject obj = null;

            foreach (PPathwaySystem sys in this.m_systems.Values)
                if (sys.Rect.Contains(point) && !sys.EcellObject.Key.Equals(excludedSystem))
                    obj = sys;
            return obj;
        }

        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public string GetSurroundingSystemKey(PointF point)
        {
            return GetSurroundingSystemKey(point, null);
        }
        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <param name="excludedSystem">If this parameter is set, this system is excluded from searching</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public string GetSurroundingSystemKey(PointF point, string excludedSystem)
        {
            PPathwayObject obj = GetSurroundingSystem(point, excludedSystem);
            if (obj == null)
                return null;
            return obj.EcellObject.Key;
        }

        /// <summary>
        /// AddSelect PPathwayNode
        /// </summary>
        /// <param name="obj">Newly selected object</param>
        public void AddSelectedNode(PPathwayObject obj)
        {
            if (!m_selectedNodes.Contains(obj))
                m_selectedNodes.Add(obj);
            obj.Selected = true;
        }

        /// <summary>
        /// Get object under the point.
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public PPathwayNode GetPickedNode(PointF pointF)
        {
            PPathwayNode pickedObj = null;
            List<PPathwayLayer> layers = new List<PPathwayLayer>();
            layers.Add(m_sysLayer);
            layers.AddRange(m_layers.Values);

            foreach (PPathwayLayer layer in layers)
            {
                foreach (PPathwayObject obj in layer.GetNodes())
                {
                    if (!(obj is PPathwayNode))
                        continue;
                    if (!obj.Visible || !obj.Rect.Contains(pointF))
                        continue;
                    pickedObj = (PPathwayNode)obj;
                }
            }
            return pickedObj;
        }

        /// <summary>
        /// Get all EcellObjects of this object.
        /// </summary>
        /// <param name="key">The key of selected object.</param>
        /// <param name="type">The type of selected object.</param>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public PPathwayObject GetObject(string key, string type)
        {
            if (key == null || type == null)
                return null;
            if (type.Equals(EcellObject.SYSTEM) && m_systems.ContainsKey(key))
                return m_systems[key];
            if (type.Equals(EcellObject.PROCESS) && m_processes.ContainsKey(key))
                return m_processes[key];
            if (type.Equals(EcellObject.VARIABLE) && m_variables.ContainsKey(key))
                return m_variables[key];
            if (type.Equals(EcellObject.TEXT) && m_texts.ContainsKey(key))
                return m_texts[key];
            return null;
        }

        /// <summary>
        /// GetSurroundedObject
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        internal List<PPathwayObject> GetSurroundedObject(RectangleF rect)
        {
            List<PPathwayObject> list = new List<PPathwayObject>();
            list.AddRange(m_sysLayer.GetNodes(rect));
            list.Remove(m_systems[Constants.delimiterPath]);
            foreach (PPathwayLayer layer in m_layers.Values)
                list.AddRange(layer.GetNodes(rect));
            return list;
        }

        /// <summary>
        /// Get all PPathwayObject of this canvas.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<PPathwayObject> GetAllObjects()
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            returnList.AddRange(GetSystemList());
            returnList.AddRange(GetNodeList());
            returnList.AddRange(GetTextList());

            return returnList;
        }
        /// <summary>
        /// Get a key list of systems under a given system.
        /// </summary>
        /// <param name="systemKey"></param>
        /// <returns>list of Ecell key of systems</returns>
        public List<PPathwayObject> GetAllObjectUnder(string systemKey)
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            foreach (PPathwayObject obj in this.GetAllObjects())
            {
                if (systemKey.Equals(Constants.delimiterPath) && !obj.EcellObject.Key.Equals(Constants.delimiterPath))
                    returnList.Add(obj);
                else if (obj.EcellObject.Key.StartsWith(systemKey + Constants.delimiterPath) || obj.EcellObject.Key.StartsWith(systemKey + Constants.delimiterColon))
                    returnList.Add(obj);
            }
            return returnList;
        }
        /// <summary>
        /// Get all PPathwaySystem of this canvas.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<PPathwayObject> GetSystemList()
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            foreach (PPathwaySystem system in this.m_systems.Values)
                returnList.Add(system);

            return returnList;
        }
        /// <summary>
        /// Get all PPathwayNode of this canvas.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<PPathwayObject> GetNodeList()
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            foreach (PPathwayVariable variable in this.m_variables.Values)
                returnList.Add(variable);
            foreach (PPathwayProcess process in this.m_processes.Values)
                returnList.Add(process);

            return returnList;
        }
        /// <summary>
        /// Get all PPathwayNode of this canvas.
        /// </summary>
        /// <returns></returns>
        public List<PPathwayObject> GetTextList()
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            foreach (PPathwayText text in this.m_texts.Values)
                returnList.Add(text);

            return returnList;
        }

        #region From PathwayView
        /// <summary>
        /// change the display rectangles of PathwayEditor.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="delta"></param>
        public void PanCanvas(Direction direction, int delta)
        {
            if (direction == Direction.Vertical)
                m_pCanvas.Camera.TranslateViewBy(0, delta);
            else
                m_pCanvas.Camera.TranslateViewBy(delta, 0);
        }

        /// <summary>
        /// get bitmap image projected PathwayEditor.
        /// </summary>
        /// <returns></returns>
        public Bitmap ToImage()
        {
            Bitmap image = m_overviewCanvas.ToImage();
            return image;
        }
        
        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="oldKey">The ID before value change.</param>
        /// <param name="newKey">The data type before value change.</param>
        /// <param name="obj">Changed value of object.</param>
        /// <param name="oldRect">oldRect.</param>
        public void DataChanged(string oldKey, string newKey, PPathwayObject obj, RectangleF oldRect)
        {
            if (!oldKey.Equals(newKey))
            {
                UpdateDictionaryKey(oldKey, newKey, obj);
                if( !m_con.Window.IsLoading)
                    MoveObject(oldKey, newKey, obj, oldRect);
                RefreshEdges();
            }
            // Set Layer
            SetLayer(obj);
            // Set visibility
            obj.RefreshView();
            // If this obj is root system, Refresh OverView.
            if (newKey.Equals(Constants.delimiterPath))
                RefreshOverView();
        }

        /// <summary>
        /// Transfer the EcellObject from the old key to the new key.
        /// </summary>
        /// <param name="oldkey">The old key.</param>
        /// <param name="newkey">The new key.</param>
        /// <param name="obj">The transfered EcellObject.</param>
        private void UpdateDictionaryKey(string oldkey, string newkey, PPathwayObject obj)
        {
            // Change Path
            PPathwaySystem system = m_systems[obj.EcellObject.ParentSystemID];
            obj.ParentObject = system;
            if (obj is PPathwaySystem)
            {
                if (!m_systems.ContainsKey(oldkey))
                    throw new PathwayException(string.Format(MessageResources.ErrNotFound, oldkey));
                m_systems.Remove(oldkey);
                m_systems.Add(newkey, (PPathwaySystem)obj);
                foreach (PPathwayObject child in GetAllObjectUnder(oldkey))
                {
                    if (child.EcellObject.ParentSystemID != oldkey)
                        continue;
                    string oldChildKey = child.EcellObject.Key;
                    child.EcellObject.Key = Util.GetMovedKey(oldChildKey, oldkey, newkey);
                    UpdateDictionaryKey(oldChildKey, child.EcellObject.Key, child);
                }
            }
            else if (obj is PPathwayVariable)
            {
                if (!m_variables.ContainsKey(oldkey))
                    throw new PathwayException(string.Format(MessageResources.ErrNotFound, oldkey));
                m_variables.Remove(oldkey);
                m_variables.Add(newkey, (PPathwayVariable)obj);
            }
            else if (obj is PPathwayProcess)
            {
                if (!m_processes.ContainsKey(oldkey))
                    throw new PathwayException(string.Format(MessageResources.ErrNotFound, oldkey));
                m_processes.Remove(oldkey);
                m_processes.Add(newkey, (PPathwayProcess)obj);
            }
            else if (obj is PPathwayText)
            {
                if (!m_texts.ContainsKey(oldkey))
                    throw new PathwayException(string.Format(MessageResources.ErrNotFound, oldkey));
                m_texts.Remove(oldkey);
                m_texts.Add(newkey, (PPathwayText)obj);
            }

        }

        /// <summary>
        /// Check and Move Position.
        /// </summary>
        /// <param name="oldkey"></param>
        /// <param name="newkey"></param>
        /// <param name="obj"></param>
        /// <param name="oldRect"></param>
        private void MoveObject(string oldkey, string newkey, PPathwayObject obj, RectangleF oldRect)
        {
            PPathwaySystem system = m_systems[obj.EcellObject.ParentSystemID];
            string sysKey = system.EcellObject.Key;

            // Move Node.
            if (obj is PPathwayNode)
            {
                if (DoesSystemContains(newkey, obj.Rect))
                    return;

                obj.CenterPointF = GetVacantPoint(sysKey, obj.CenterPointF);
                m_con.NotifyDataChanged(obj, false);
            }
            // Move System.
            else if( obj is PPathwaySystem && !m_isOwner)
            {
                // Set Object Position.
                MakeSpace(system, obj, true);
            }
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="obj">Changed value of object.</param>
        public void SetPosition(PPathwayObject obj)
        {
            // Set Layer
            SetLayer(obj);
            // Set visibility
            obj.RefreshView();
        }


        /// <summary>
        /// event sequence of deleting the object.
        /// </summary>
        /// <param name="key">the key of deleted object.</param>
        /// <param name="type">the type of deleted object.</param>
        public void DataDelete(string key, string type)
        {
            PPathwayObject obj = GetObject(key, type);
            if (obj == null)
                return;

            ResetSelect();

            if (obj is PPathwaySystem)
            {
                RemoveNodeUnder(key);
                m_systems.Remove(key);
            }
            else if (obj is PPathwayProcess)
            {
                m_processes.Remove(key);
            }
            else if (obj is PPathwayVariable)
            {
                m_variables.Remove(key);
            }
            else if (obj is PPathwayText)
            {
                m_texts.Remove(key);
            }

            RemoveObject(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveObject(PPathwayObject obj)
        {
            if (obj == null)
                return;
            obj.PText.RemoveFromParent();
            obj.Parent.RemoveChild(obj);
            obj.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysKey"></param>
        private void RemoveNodeUnder(string sysKey)
        {
            foreach (PPathwayObject obj in GetAllObjectUnder(sysKey))
                DataDelete(obj.EcellObject.Key, obj.EcellObject.Type);
        }

        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void AddSelect(string key, string type)
        {
            PPathwayObject obj = GetObject(key, type);
            if (obj == null)
                return;

            AddSelect(obj);
        }
        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="obj"></param>
        public void AddSelect(PPathwayObject obj)
        {
            ResetSelectedLine();
            AddSelectedNode(obj);
        }
        
        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void RemoveSelect(string key, string type)
        {
            PPathwayObject obj = GetObject(key, type);
            if (obj == null)
                return;

            if (m_focusNode == obj)
                m_focusNode = null;
            if (m_selectedNodes.Contains(obj))
                m_selectedNodes.Remove(obj);
            obj.Selected = false;
        }

        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void SelectChanged(string key, string type)
        {
            // Error check.
            if (key == null || type == null)
                return;
            PPathwayObject obj = GetObject(key, type);
            m_focusNode = obj;
            if (obj == null)
                return;
            // Set select change.
            ResetSelect();
            AddSelectedNode(obj);

            // Exit if the event came from this plugin.
            if (m_isOwner)
            {
                m_isOwner = false;
                return;
            }
            // Move camera view.
            if (m_focusMode && obj.Visible)
            {
                RectangleF centerBounds = PathUtil.GetFocusBound(obj.Rect, m_pCanvas.Camera.ViewBounds);
                m_pCanvas.Camera.AnimateViewToCenterBounds(centerBounds,
                                                 true,
                                                 CAMERA_ANIM_DURATION);
                UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
            }
        }
        #endregion

        /// <summary>
        /// Call the UpdateOverview() method after a certain time passed
        /// </summary>
        /// <param name="miliSec">Called after this time passed</param>
        public void UpdateOverviewAfterTime(int miliSec)
        {
            Timer timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = miliSec;
            timer.Start();
        }

        /// <summary>
        /// Refresh overview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Camera_ViewChanged(object sender, PPropertyEventArgs e)
        {
            // Refresh OverView.
            RefreshOverView();
        }

        /// <summary>
        /// Refresh OverView.
        /// </summary>
        internal void RefreshOverView()
        {
            RectangleF rect = m_pCanvas.Camera.ViewBounds;
            m_overviewCanvas.UpdateOverview(rect);
        }
        /// <summary>
        /// This timer delegate is called for updating overview after object-moving anime has finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            m_con.Menu.Zoom(1f);
            ((Timer)sender).Stop();
            ((Timer)sender).Dispose();
        }

        /// <summary>
        /// Zoom in/out this canvas.
        /// </summary>
        /// <param name="rate"></param>
        public void Zoom(float rate)
        {
            float newScale = m_pCanvas.Camera.ViewScale * rate;
            if (newScale < MIN_SCALE || MAX_SCALE < newScale)
                rate = 1f;

            float zoomX = this.PCanvas.Camera.ViewBounds.X + (this.PCanvas.Camera.ViewBounds.Width / 2);
            float zoomY = this.PCanvas.Camera.ViewBounds.Y + (this.PCanvas.Camera.ViewBounds.Height / 2);
            this.PCanvas.Camera.ScaleViewBy(rate, zoomX, zoomY);
        }

        /// <summary>
        /// Release all the unmanaged resources in this object
        /// </summary>
        public void Dispose()
        {
            if (m_pCanvas != null)
                m_pCanvas.Dispose();

            if (m_overviewCanvas != null)
                m_overviewCanvas.Dispose();

            m_con.ViewModeChange -= this.Control_ViewModeChange;
        }

        /// <summary>
        /// reset the seletect nodes.
        /// </summary>
        public void ResetSelectedNodes()
        {
            if (m_selectedNodes.Count == 0)
                return;
            foreach (PPathwayObject obj in m_selectedNodes)
                GetObject(obj.EcellObject.Key, obj.EcellObject.Type).Selected = false;
            lock (this)
                m_selectedNodes.Clear();
        }

        /// <summary>
        /// Reset selected line
        /// </summary>
        public void ResetSelectedLine()
        {
            m_lineHandler.ResetSelectedLine();
        }

        /// <summary>
        /// reset the selected object(system and node).
        /// </summary>
        public void NotifyResetSelect()
        {
            m_con.Window.NotifyResetSelect();
        }

        /// <summary>
        /// reset the selected object(system and node).
        /// </summary>
        public void ResetSelect()
        {
            ResetSelectedNodes();
            ResetSelectedLine();
        }

        /// <summary>
        /// Return true if EcellSystem contains a point.
        /// </summary>
        /// <param name="sysKey">string</param>
        /// <param name="point">PointF</param>
        /// <returns>bool</returns>
        public bool CheckNodePosition(string sysKey, PointF point)
        {
            PointF center = new PointF(point.X + PPathwayNode.DEFAULT_WIDTH / 2, point.Y + PPathwayNode.DEFAULT_HEIGHT / 2);
            bool sysContains = false;
            bool childContains = false;
            foreach (PPathwaySystem sys in this.m_systems.Values)
                if (sys.EcellObject.Key.Equals(sysKey) && sys.Rect.Contains(point) && sys.Rect.Contains(center))
                    sysContains = true;
                else if (sys.EcellObject.Key.StartsWith(sysKey) && (sys.Rect.Contains(point) || sys.Rect.Contains(center) ) )
                    childContains = true;
            return sysContains && !childContains;
        }

        /// <summary>
        /// Return nearest vacant point of EcellSystem.
        /// </summary>
        /// <param name="sysKey">Key of system.</param>
        ///<returns>PointF.</returns>
        public PointF GetVacantPoint(string sysKey)
        {
            PPathwaySystem sys = m_systems[sysKey];
            Random hRandom = new Random();
            PointF basePos = new PointF(
                (float)hRandom.Next((int)sys.X, (int)(sys.X + sys.Width)),
                (float)hRandom.Next((int)sys.Y, (int)(sys.Y + sys.Height)));
            return GetVacantPoint(sysKey, basePos);
        }

        /// <summary>
        /// Return nearest vacant point for node.
        /// </summary>
        /// <param name="sysKey"></param>
        /// <param name="point"></param>
        public PointF GetVacantPoint(string sysKey, PointF point)
        {
            if (string.IsNullOrEmpty(sysKey))
                sysKey = Constants.delimiterPath;
            PPathwaySystem sys = m_systems[sysKey];
            PointF basePos = new PointF(point.X, point.Y);
            double rad = Math.PI * 0.25f;
            float r = 0f;

            do
            {
                // Check 
                if (DoesSystemContains(sysKey, point))
                    return new PointF(point.X, point.Y);
                r += 1f;
                point.X = basePos.X + r * (float)Math.Cos(rad * r);
                point.Y = basePos.Y + r * (float)Math.Sin(rad * r);
            } while (r < sys.Width || r < sys.Height);
            // if there si no vacant point, return basePos.
            return basePos;
        }
        /// <summary>
        /// Return nearest vacant point for system.
        /// </summary>
        /// <param name="sysKey">The key of system.</param>
        /// <param name="rectF">Target position.</param>
        /// <returns>PointF.</returns>
        public PointF GetVacantPoint(string sysKey, RectangleF rectF)
        {
            if(string.IsNullOrEmpty(sysKey))
                sysKey = Constants.delimiterPath;
            PPathwaySystem sys = m_systems[sysKey];
            PointF basePos = new PointF(rectF.X, rectF.Y);
            double rad = Math.PI * 0.25f;
            float r = 0f;

            do
            {
                // Check 
                if (DoesSystemContains(sysKey, rectF) && !sys.Overlaps(rectF))
                    return new PointF(rectF.X, rectF.Y);
                r += 1f;
                rectF.X = basePos.X + r * (float)Math.Cos(rad * r);
                rectF.Y = basePos.Y + r * (float)Math.Sin(rad * r);
            } while (r < sys.Width || r < sys.Height);
            // if there si no vacant point, return basePos.
            return basePos;
        }

        internal void ResetObjectSettings()
        {
            foreach (PPathwayObject obj in GetAllObjects())
            {
                obj.ViewMode = m_isViewMode;
                if (obj is PPathwayProcess)
                    ((PPathwayProcess)obj).EdgeBrush = (m_isViewMode)? m_con.Animation.ViewEdgeBrush: m_con.Animation.EditEdgeBrush;
            }

            if (m_isViewMode)
                this.BackGroundBrush = m_con.Animation.ViewBGBrush;
            else
                this.BackGroundBrush = m_con.Animation.EditBGBrush;
        }

        /// <summary>
        /// Move Selected Objects
        /// </summary>
        /// <param name="offset"></param>
        internal void MoveSelectedObjects(PointF offset)
        {
            foreach (PPathwayObject obj in m_selectedNodes)
            {
                obj.Offset = offset;
                // Move Nodes.
                if (obj is PPathwaySystem)
                {
                    PPathwaySystem system = (PPathwaySystem)obj;
                    // Change color if the system overlaps other system
                    if (DoesSystemOverlaps(system) || !IsInsideRoot(system.Rect))
                        system.Invalid = true;
                    else
                        system.Invalid = false;
                    foreach (PPathwayObject child in GetAllObjectUnder(system.EcellObject.Key))
                    {
                        child.Offset = offset;
                    }
                }
                obj.Refresh();
            }

        }

        /// <summary>
        /// NotifyMoveObjects
        /// </summary>
        /// <param name="isAnchored"></param>
        internal void NotifyMoveObjects(bool isAnchored)
        {
            m_isOwner = true;
            List<PPathwayObject> objList = new List<PPathwayObject>();
            // Check 
            try
            {
                // Check KeyChange.
                foreach (PPathwayObject obj in GetAllObjects())
                {
                    if (CheckMoveErrorAndKeyChange(obj))
                        objList.Add(obj);
                }
                // Check Moved Object.
                foreach (PPathwayObject obj in GetAllObjects())
                {
                    if (obj.OffsetX != 0.0 && obj.OffsetY != 0.0 && !objList.Contains(obj))
                        objList.Add(obj);
                }
                // Move Objects.
                bool isLast = false;
                int i = 0;
                // MoveSystems
                foreach (PPathwayObject obj in objList)
                {
                    if (!(obj is PPathwaySystem))
                        continue;
                    i++;
                    isLast = (i == objList.Count);
                    NotifyMoveSystem(obj, isLast && isAnchored);
                }
                // Move Processes
                foreach (PPathwayObject obj in objList)
                {
                    if (!(obj is PPathwayProcess))
                        continue;
                    i++;
                    isLast = (i == objList.Count);
                    NotifyMoveNode(obj, isLast && isAnchored);
                }
                // Move Variables and Texts.
                foreach (PPathwayObject obj in objList)
                {
                    if (!(obj is PPathwayVariable) && !(obj is PPathwayText))
                        continue;
                    i++;
                    isLast = (i == objList.Count);
                    NotifyMoveNode(obj, isLast && isAnchored);
                }
            }
            catch (PathwayException)
            {
                foreach (PPathwayObject obj in GetAllObjects())
                {
                    obj.ResetPosition();
                    obj.Invalid = false;
                    obj.RefreshView();
                }
                NotifyResetSelect();
            }
            m_isOwner = false;

        }

        /// <summary>
        /// NotyfyMoveNode
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isLast"></param>
        private void NotifyMoveNode(PPathwayObject node, bool isLast)
        {
            string oldKey = node.EcellObject.Key;
            string newKey;
            // 
            if (node is PPathwayText)
                newKey = node.EcellObject.Key;
            else
                newKey = GetSurroundingSystemKey(node.CenterPointF) + ":" + node.EcellObject.LocalID;

            //if (oldKey == newKey && node.OffsetX == 0.0 && node.OffsetY == 0.0)
            //    return;

            m_con.NotifyDataChanged(
                oldKey,
                newKey,
                node,
                true,
                isLast);
        }

        /// <summary>
        /// NotifyMoveSystem
        /// </summary>
        /// <param name="system"></param>
        /// <param name="isAnchor"></param>
        private void NotifyMoveSystem(PPathwayObject system, bool isAnchor)
        {
            string oldSysKey = system.EcellObject.Key;
            string parentSysKey = GetSurroundingSystemKey(system.PointF, oldSysKey);
            string newSysKey = null;
            if (parentSysKey == null)
                newSysKey = Constants.delimiterPath;
            else if (parentSysKey.Equals(Constants.delimiterPath))
                newSysKey = Constants.delimiterPath + system.EcellObject.LocalID;
            else
                newSysKey = parentSysKey + Constants.delimiterPath + system.EcellObject.LocalID;

            // Move system position.
            m_con.NotifyDataChanged(
                oldSysKey,
                oldSysKey,
                system,
                true,
                false);

            // Move objects under this system.
            // TODO: This process should be implemented in EcellLib.DataChanged().
            foreach (PPathwayObject obj in GetAllObjectUnder(oldSysKey))
            {
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    obj.EcellObject.Key,
                    obj,
                    true,
                    false);
            }

            // Move system path.
            m_con.NotifyDataChanged(
                oldSysKey,
                newSysKey,
                system,
                true,
                false);

            // Import Systems and Nodes
            RectangleF rect = system.Rect;
            string parentSystemName = system.EcellObject.ParentSystemID;
            foreach (PPathwayObject obj in GetAllObjects())
            {
                if (obj == system)
                    continue;
                if (obj.EcellObject.ParentSystemID.StartsWith(newSysKey))
                    continue;
                if (obj is PPathwayText)
                    continue;
                if (obj is PPathwaySystem && !rect.Contains(obj.Rect))
                    continue;
                if (obj is PPathwayNode && !rect.Contains(obj.CenterPointF))
                    continue;

                string newNodeKey = PathUtil.GetMovedKey(obj.EcellObject.Key, parentSystemName, newSysKey);
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    newNodeKey,
                    obj,
                    true,
                    false);
            }

            // Refresh system.
            m_con.NotifyDataChanged(
                newSysKey,
                newSysKey,
                system,
                true,
                true && isAnchor);
        }

        /// <summary>
        /// This method check object for Offset and the change of surrounding system.
        /// Returns true if changed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CheckMoveErrorAndKeyChange(PPathwayObject obj)
        {
            string newKey = null;
            string newSysKey;
            if (obj is PPathwaySystem)
            {
                string oldSysKey = obj.EcellObject.Key;
                newSysKey = GetSurroundingSystemKey(obj.PointF, oldSysKey);
                if (newSysKey == null)
                    newKey = Constants.delimiterPath;
                else if (newSysKey.Equals(Constants.delimiterPath))
                    newKey = Constants.delimiterPath + obj.EcellObject.LocalID;
                else
                    newKey = newSysKey + Constants.delimiterPath + obj.EcellObject.LocalID;

                // Reset system movement when the system is overlapping other system or out of root.
                if (!IsInsideRoot(obj.Rect))
                {
                    throw new PathwayException(MessageResources.ErrOutRoot);
                }
                else if (DoesSystemOverlaps((PPathwaySystem)obj))
                {
                    throw new PathwayException(MessageResources.ErrOverSystem);
                }
                // Reset if system is duplicated.
                else if (!oldSysKey.Equals(newKey) && m_systems.ContainsKey(newKey))
                {
                    throw new PathwayException(string.Format(
                        MessageResources.ErrAlrExist,
                        new object[] { newKey }));
                }
                obj.Invalid = false;
            }
            else if (obj is PPathwayNode)
            {
                newSysKey = GetSurroundingSystemKey(obj.CenterPointF);
                // When node is out of root.
                if (newSysKey == null)
                {
                    throw new PathwayException(obj.EcellObject.LocalID + ":" + MessageResources.ErrOutRoot);
                }

                // When node is duplicated.
                newKey = newSysKey + ":" + obj.EcellObject.LocalID;
                if (!newSysKey.Equals(obj.EcellObject.ParentSystemID)
                    && GetObject(newKey, obj.EcellObject.Type) != null)
                {
                    throw new PathwayException(string.Format(
                        MessageResources.ErrAlrExist,
                        new object[] { obj.EcellObject.LocalID }));
                }
            }
            else
            {
                return false;
            }
            bool isMoved = !obj.EcellObject.Key.Equals(newKey);
            return (isMoved);
        }

        /// <summary>
        /// Whether given rectangle is inside the root system or not.
        /// </summary>
        /// <param name="rectF">a rectangle to be checked.</param>
        /// <returns>True, if the given rectangle is inside the root system.
        ///          False, if the given rectangle is outside the root system.
        /// </returns>
        public bool IsInsideRoot(RectangleF rectF)
        {
            RectangleF rootRect = m_systems[Constants.delimiterPath].Rect;
            return rootRect.Contains(rectF);
        }
    }
}
