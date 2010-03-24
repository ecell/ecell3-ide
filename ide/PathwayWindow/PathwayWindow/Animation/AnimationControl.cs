﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using AviFile;
using Ecell.IDE.Plugins.PathwayWindow.Graphics;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.Collections.Generic;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// AnimationControl
    /// </summary>
    public class AnimationControl : IDisposable 
    {
        #region Constant
        /// <summary>
        /// 
        /// </summary>
        private const string FormatLog = "E2";
        /// <summary>
        /// 
        /// </summary>
        private const string FormatNatural = "0.000000";
        #endregion

        #region Fields

        private List<IAnimationItem> _items = new List<IAnimationItem>();
        /// <summary>
        /// High threshold of edge animation.
        /// </summary>
        private double _thresholdHigh = 100d;
        /// <summary>
        /// Low threshold of edge animation.
        /// </summary>
        private double _thresholdLow = 0d;
        /// <summary>
        /// Normal edge width.
        /// </summary>
        private float _normalEdgeWidth = PPathwayEdge.LINE_WIDTH;
        /// <summary>
        /// Max edge width on edge animation.
        /// </summary>
        private float _maxEdgeWidth = 20f;
        /// <summary>
        /// CanvasBrush on EditMode.
        /// </summary>
        private Brush _editBGBrush = Brushes.White;
        /// <summary>
        /// Edge brush on EditMode.
        /// </summary>
        private Brush _editEdgeBrush = Brushes.Black;
        /// <summary>
        /// CanvasBrush on ViewMode.
        /// </summary>
        private Brush _viewBGBrush = Brushes.White;
        /// <summary>
        /// Edge brush on ViewMode.
        /// </summary>
        private Brush _viewEdgeBrush = Brushes.LightGreen;
        /// <summary>
        /// Low threshold edge brush on ViewMode.
        /// </summary>
        private Brush _lowEdgeBrush = Brushes.Gray;
        /// <summary>
        /// High threshold edge brush on ViewMode.
        /// </summary>
        private Brush _highEdgeBrush = Brushes.Yellow;
        /// <summary>
        /// NG edge brush on ViewMode.
        /// </summary>
        private Brush _ngEdgeBrush = Brushes.Red;
        /// <summary>
        /// Label brush on ViewMode.
        /// </summary>
        private Brush _propBrush = Brushes.Blue;
        /// <summary>
        /// 
        /// </summary>
        private string _format = "";
        /// <summary>
        /// 
        /// </summary>
        private bool _autoThreshold = false;
        #endregion

        #region Object Fields
        /// <summary>
        /// PathwayControl.
        /// </summary>
        protected PathwayControl _con = null;
        /// <summary>
        /// CanvasControl.
        /// </summary>
        protected CanvasControl _canvas = null;
        /// <summary>
        /// DataManager
        /// </summary>
        DataManager _dManager = null;
        /// <summary>
        /// EventTimer for animation.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// EventFlag isPausing
        /// </summary>
        private bool _isPausing = false;
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public PathwayControl Control
        {
            get { return _con; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CanvasControl Canvas
        {
            get { return _con.Canvas; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IAnimationItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool DoesAnimationOnGoing
        {
            get
            {
                bool prjStatus =(_con.ProjectStatus == ProjectStatus.Running
                    || _con.ProjectStatus == ProjectStatus.Stepping
                    || _con.ProjectStatus == ProjectStatus.Suspended);
                bool doesAnimation = prjStatus && _con.IsAnimation;
                return doesAnimation;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal bool IsPausing
        {
            get { return _isPausing; }
            set { _isPausing = value; }
        }

        /// <summary>
        /// Get/Set m_thresholdHigh
        /// </summary>
        public double ThresholdHigh
        {
            get { return _thresholdHigh; }
            set { _thresholdHigh = value; }
        }

        /// <summary>
        /// Get/Set m_thresholdLow
        /// </summary>
        public double ThresholdLow
        {
            get { return _thresholdLow; }
            set { _thresholdLow = value; }
        }

        /// <summary>
        /// Get/Set m_normalEdgeWidth
        /// </summary>
        public float EdgeWidth
        {
            get { return _normalEdgeWidth; }
            set { _normalEdgeWidth = value; }
        }

        /// <summary>
        /// Get/Set m_maxEdgeWidth
        /// </summary>
        public float MaxEdgeWidth
        {
            get { return _maxEdgeWidth; }
            set { _maxEdgeWidth = value; }
        }

        /// <summary>
        /// Get/Set m_editBGBrush
        /// </summary>
        public Brush EditBGBrush
        {
            get { return _editBGBrush; }
            set { _editBGBrush = value; }
        }

        /// <summary>
        /// Get/Set m_viewBGBrush
        /// </summary>
        public Brush ViewBGBrush
        {
            get { return _viewBGBrush; }
            set { _viewBGBrush = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush EdgeBrush
        {
            get
            {
                if (_con.IsAnimation)
                    return _viewEdgeBrush;
                else
                    return _editEdgeBrush;
            }
        }

        /// <summary>
        /// Get/Set m_editEdgeBrush
        /// </summary>
        public Brush EditEdgeBrush
        {
            get { return _editEdgeBrush; }
            set { _editEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_viewEdgeBrush
        /// </summary>
        public Brush ViewEdgeBrush
        {
            get { return _viewEdgeBrush; }
            set { _viewEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_lowEdgeBrush
        /// </summary>
        public Brush LowEdgeBrush
        {
            get { return _lowEdgeBrush; }
            set { _lowEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_highEdgeBrush
        /// </summary>
        public Brush HighEdgeBrush
        {
            get { return _highEdgeBrush; }
            set { _highEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_ngEdgeBrush
        /// </summary>
        public Brush NgEdgeBrush
        {
            get { return _ngEdgeBrush; }
            set { _ngEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_propBrush
        /// </summary>
        public Brush PropertyBrush
        {
            get { return _propBrush; }
            set { _propBrush = value; }
        }

        /// <summary>
        /// Get/Set m_autoThreshold
        /// </summary>
        public bool AutoThreshold
        {
            get { return _autoThreshold; }
            set { _autoThreshold = value; }
        }

        /// <summary>
        /// get PropertyDialogTabPage for Pathway settings.
        /// </summary>
        public PropertyDialogPage PathwaySettingsPage
        {
            get { return new PathwaySettingsPage(this); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public AnimationControl(PathwayControl control)
        {
            _con = control;
            _con.CanvasChange += new EventHandler(Control_CanvasChange);
            _con.ProjectStatusChange += new EventHandler(Control_ProjectStatusChange);
            _con.AnimationChange += new EventHandler(m_con_AnimationChange);
            LoadDefaultSettings();
            _dManager = _con.Window.DataManager;
            _dManager.ApplySteppingModelEvent += new ApplySteppingModelEnvetHandler(_dManager_ApplySteppingModelEvent);
            // Set Timer.
            _timer = new Timer();
            _timer.Interval = 200;
            _timer.Tick += new EventHandler(TimerFire);
        }

        void _dManager_ApplySteppingModelEvent(object o, Ecell.Events.SteppingModelEventArgs e)
        {
            ResetAnimationStatus();
        }

        void m_con_AnimationChange(object sender, EventArgs e)
        {
            ResetAnimationStatus();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetAnimationStatus()
        {
            if (_con.IsAnimation)
            {
                SetAnimation();
                if (_items.Count > 0 &&
                    (_con.ProjectStatus == ProjectStatus.Running ||
                    _con.ProjectStatus == ProjectStatus.Stepping ||
                    _con.ProjectStatus == ProjectStatus.Suspended))
                    TimerStart();
            }
            else
            {
                ResetAnimation();
            }

        }

        /// <summary>
        /// Event on Dispose
        /// </summary>
        public void Dispose()
        {
            ResetAnimation();
            _con.ProjectStatusChange -= Control_ProjectStatusChange;
        }

        #endregion

        #region EventHanlers
        /// <summary>
        /// Event on CnavasChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Control_CanvasChange(object sender, EventArgs e)
        {
            _canvas = _con.Canvas;
        }

        /// <summary>
        /// Event on ProjectStatusChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Control_ProjectStatusChange(object sender, EventArgs e)
        {
            if (_con.IsAnimation)
            {
                if (_con.ProjectStatus == ProjectStatus.Running
                    || _con.ProjectStatus == ProjectStatus.Stepping)
                    StartSimulation();
                else if (_con.ProjectStatus == ProjectStatus.Suspended)
                    PauseSimulation();
                else
                    StopSimulation();
            }
            else
            {
                ResetAnimation();
            }
        }

        #endregion

        #region Methods to control TimerEvent
        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        public void TimerFire(object sender, EventArgs e)
        {
            UpdateAnimation();
        }
        /// <summary>
        /// Start Simulation
        /// </summary>
        public void StartSimulation()
        {
            //
            foreach (PPathwayProcess process in _canvas.Processes.Values)
            {
                if (!process.Visible)
                    continue;
                // Line setting.
                foreach (PPathwayEdge line in process.Edges)
                {
                    line.EdgeBrush = _viewEdgeBrush;
                }
            }

            //
            SetAnimation();

            TimerStart();
            _isPausing = false;

        }
        /// <summary>
        /// Pause Simulation
        /// </summary>
        public void PauseSimulation()
        {
            TimerStop();
            UpdateAnimation();
            _isPausing = true;
        }
        /// <summary>
        /// Stop Simulation
        /// </summary>
        public void StopSimulation()
        {
            TimerStop();
            _isPausing = false;
            // Reset
            StopAnimation();
        }
        /// <summary>
        /// Start Timer.
        /// </summary>
        public void TimerStart()
        {
            _isPausing = false;
            _timer.Start();
        }
        /// <summary>
        /// Stop Timer.
        /// </summary>
        public void TimerStop()
        {
            _timer.Stop();
        }
        #endregion

        #region Methods for Animation
        /// <summary>
        /// 
        /// </summary>
        public void SetAnimation()
        {
            if (_con.Canvas == null)
                return;
            // Set Canvas
            _canvas = _con.Canvas;
            _canvas.BackGroundBrush = _viewBGBrush;
            _format = _con.Window.DataManager.DisplayStringFormat;
            //
            foreach (PPathwayProcess process in _canvas.Processes.Values)
            {
                if (!process.Visible)
                    continue;
                // Line setting.
                foreach (PPathwayEdge line in process.Edges)
                {
                    line.EdgeBrush = _viewEdgeBrush;
                }
            }

            // SetAnimation
            foreach (IAnimationItem item in _items)
            {
                item.SetAnimation();
            }

            if (_isPausing)
                UpdateAnimation();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateAnimation()
        {
            if (_canvas == null)
                return;
            // Do animation
            foreach (IAnimationItem item in _items)
            {
                item.UpdateAnimation();
            }

            _canvas.PCanvas.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopAnimation()
        {
            if (_canvas == null)
                return;
            // Stop animation
            foreach (IAnimationItem item in _items)
            {
                item.StopAnimation();
            }

            _canvas.PCanvas.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetAnimation()
        {
            TimerStop();
            if (_canvas == null)
                return;
            // Set Canvas
            _canvas.BackGroundBrush = _editBGBrush;

            //
            foreach (PPathwayProcess process in _canvas.Processes.Values)
            {
                if (!process.Visible)
                    continue;
                // Line setting.
                foreach (PPathwayEdge line in process.Edges)
                {
                    line.EdgeBrush = _editEdgeBrush;
                }
            }

            // Reset
            foreach (IAnimationItem item in _items)
            {
                item.ResetAnimation();
            }

        }
        #endregion

        /// <summary>
        /// Save Settings
        /// </summary>
        public void SaveSettings()
        {
            string filename = Path.Combine(Util.GetUserDir(), AnimationConstants.xPathFileName);

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
                xmlOut.WriteComment(AnimationConstants.xPathFileHeader1);
                xmlOut.WriteComment(AnimationConstants.xPathFileHeader2);

                xmlOut.WriteStartElement(AnimationConstants.xPathAnimationSettings);
                // Application settings.
                xmlOut.WriteAttributeString(AnimationConstants.xPathName, Application.ProductName);
                xmlOut.WriteAttributeString(AnimationConstants.xPathFileVersion, AnimationConstants.xPathVersion);
                // Save settings.
                xmlOut.WriteElementString(AnimationConstants.xPathHighQuality, _con.HighQuality.ToString());
                xmlOut.WriteElementString(AnimationConstants.xPathEditBGBrush, BrushManager.ParseBrushToString(_editBGBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathEditEdgeBrush, BrushManager.ParseBrushToString(_editEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathNormalEdgeWidth, _normalEdgeWidth.ToString());

                xmlOut.WriteElementString(AnimationConstants.xPathViewBGBrush, BrushManager.ParseBrushToString(_viewBGBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathViewEdgeBrush, BrushManager.ParseBrushToString(_viewEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathMaxEdgeWidth, _maxEdgeWidth.ToString());

                xmlOut.WriteElementString(AnimationConstants.xPathHighEdgeBrush, BrushManager.ParseBrushToString(_highEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathLowEdgeBrush, BrushManager.ParseBrushToString(_lowEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathAutoThreshold, _autoThreshold.ToString());
                xmlOut.WriteElementString(AnimationConstants.xPathThresholdHigh, _thresholdHigh.ToString());
                xmlOut.WriteElementString(AnimationConstants.xPathThresholdLow, _thresholdLow.ToString());

                xmlOut.WriteElementString(AnimationConstants.xPathNGEdgeBrush, BrushManager.ParseBrushToString(_ngEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathPropertyBrush, BrushManager.ParseBrushToString(_propBrush));

                xmlOut.WriteEndElement();
                xmlOut.WriteEndDocument();
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(MessageResources.ErrCompInvalid + Environment.NewLine + filename + Environment.NewLine + ex.Message);

            }
            finally
            {
                if (xmlOut != null) xmlOut.Close();
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        internal void LoadAnimationSettings(XmlNode status)
        {
            // Get animation settings node.
            XmlElement animationSettings = null;
            foreach (XmlNode node in status.ChildNodes)
            {
                if (node.Name.Equals(AnimationConstants.xPathAnimationSettings))
                    animationSettings = (XmlElement)node;
            }
            if (animationSettings == null)
                return;

            // load animation settings
            SetAnimationSettings(animationSettings);
            //
            _con.NotifyAnimaitionChanged(_canvas.ModelID, false);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animationSettings"></param>
        internal void SetAnimationSettings(XmlElement animationSettings)
        {
            // Reset Animation.
            ResetAnimation();
            _items.Clear();
            if (animationSettings == null)
            {
                _con.Menu.SetAnimation(false);
                return;
            }

            foreach (XmlElement animationSetting in animationSettings.ChildNodes)
            {
                // create animation item
                AnimationItemBase animationItem = null;
                switch (animationSetting.Name)
                {
                    case "EdgeAnimationItem":
                        animationItem = new EdgeAnimationItem(this);
                        break;
                    case "EntityAnimationItem":
                        animationItem = new EntityAnimationItem(this);
                        break;
                    case "GraphAnimationItem":
                        animationItem = new GraphAnimationItem(this);
                        break;
                    case "MassCalculationAnimationItem":
                        animationItem = new MassCalculationAnimationItem(this);
                        break;
                    case "MovieAnimationItem":
                        animationItem = new MovieAnimationItem(this);
                        break;
                    case "PropertyViewAnimationItem":
                        animationItem = new PropertyViewAnimationItem(this);
                        break;
                    default:
                        break;
                }
                if (animationItem == null)
                    continue;
                // setup animation item.
                animationItem.SetAnimationStatus(animationSetting);
                _items.Add(animationItem);

            }

            // Activate animation.
            bool isAnimation = false;
            bool.TryParse(animationSettings.GetAttribute("IsAnimation"), out isAnimation);
            _con.Menu.SetAnimation(isAnimation);

        }

        /// <summary>
        /// Load Settings.
        /// </summary>
        public void LoadDefaultSettings()
        {
            string filename = Path.Combine(Util.GetUserDir(), AnimationConstants.xPathFileName);
            if (!File.Exists(filename))
                return;

            // Get Animation settings.
            XmlDocument xmlD = new XmlDocument();
            xmlD.Load(filename);
            XmlNode settings = FindAnimationSettings(xmlD);
            if (settings == null)
                return;

            // Load settings.
            foreach (XmlNode setting in settings.ChildNodes)
            {
                switch (setting.Name)
                {
                    // EditBGBrush
                    case AnimationConstants.xPathEditBGBrush:
                        _editBGBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // EditEdgeBrush
                    case AnimationConstants.xPathEditEdgeBrush:
                        _editEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // NormalEdgeWidth
                    case AnimationConstants.xPathNormalEdgeWidth:
                        _normalEdgeWidth = float.Parse(setting.InnerText);
                        break;
                    // ViewBGBrush
                    case AnimationConstants.xPathViewBGBrush:
                        _viewBGBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // ViewEdgeBrush
                    case AnimationConstants.xPathViewEdgeBrush:
                        _viewEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // MaxEdgeWidth
                    case AnimationConstants.xPathMaxEdgeWidth:
                        _maxEdgeWidth = float.Parse(setting.InnerText);
                        break;
                    // HighEdgeBrush
                    case AnimationConstants.xPathHighEdgeBrush:
                        _highEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // LowEdgeBrush
                    case AnimationConstants.xPathLowEdgeBrush:
                        _lowEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // ThresholdHigh
                    case AnimationConstants.xPathThresholdHigh:
                        _thresholdHigh = float.Parse(setting.InnerText);
                        break;
                    // ThresholdLow
                    case AnimationConstants.xPathThresholdLow:
                        _thresholdLow = float.Parse(setting.InnerText);
                        break;
                    // NGEdgeBrush
                    case AnimationConstants.xPathNGEdgeBrush:
                        _ngEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // PropertyBrush
                    case AnimationConstants.xPathPropertyBrush:
                        _propBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // AutoThreshold
                    case AnimationConstants.xPathAutoThreshold:
                        _autoThreshold = bool.Parse(setting.InnerText);
                        break;
                    // AutoThreshold
                    case AnimationConstants.xPathHighQuality:
                        _con.HighQuality = bool.Parse(setting.InnerText);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlD"></param>
        /// <returns></returns>
        private static XmlNode FindAnimationSettings(XmlDocument xmlD)
        {
            XmlNode settings = null;
            foreach (XmlNode node in xmlD.ChildNodes)
            {
                if (node.Name.Equals(AnimationConstants.xPathAnimationSettings))
                    settings = node;
            }
            return settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        internal XmlElement GetAnimationSettings(XmlDocument doc)
        {
            XmlElement animationSettings = doc.CreateElement(AnimationConstants.xPathAnimationSettings);
            animationSettings.SetAttribute("IsAnimation", _con.IsAnimation.ToString());
            foreach (IAnimationItem animation in _items)
            {
                animationSettings.AppendChild(animation.GetAnimationStatus(doc));
            }
            return animationSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPN"></param>
        /// <returns></returns>
        private float GetFloatValue(string fullPN)
        {
            float num = 0.0f;
            try
            {
                if (_dManager.CurrentProject.SimulationStatus == SimulationStatus.Run ||
                    _dManager.CurrentProject.SimulationStatus == SimulationStatus.Suspended)
                {
                    num = (float)_dManager.GetPropertyValue(fullPN);
                }
                else
                {
                    string key = "";
                    string type = "";
                    string propName = "";
                    Util.ParseFullPN(fullPN, out type, out key, out propName);
                    Ecell.Objects.EcellObject obj = _dManager.GetEcellObject(
                        _dManager.CurrentProject.Model.ModelID, key, type);
                    if (obj != null)
                    {
                        Ecell.Objects.EcellData data = obj.GetEcellData(propName);
                        num = (float)Double.Parse(data.Value.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                num = float.NaN;
            }
            return num;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ShowDialog()
        {
            AnimationDialog dlg = new AnimationDialog(this);            
            using (dlg)
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                dlg.ApplyChange();

                // Reset Animation Status.
                foreach (IAnimationItem item in _items)
                {
                    item.ResetAnimation();
                }
                _items.Clear();
                _items.AddRange(dlg.Items);

                // Set Animation Status.
                _con.Menu.SetAnimation(_items.Count > 0);
                _con.NotifyAnimaitionChanged(_canvas.ModelID, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Clear()
        {
            ResetAnimation();
            _items.Clear();
        }
    }
}
