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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.Drawing;
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using System.Diagnostics;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using System.IO;
using System.Xml;
using Ecell.Objects;
using AviFile;

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
        /// <summary>
        /// High threshold of edge animation.
        /// </summary>
        private float m_thresholdHigh = 100f;
        /// <summary>
        /// Low threshold of edge animation.
        /// </summary>
        private float m_thresholdLow = 0f;
        /// <summary>
        /// Normal edge width.
        /// </summary>
        private float m_normalEdgeWidth = PPathwayLine.LINE_WIDTH;
        /// <summary>
        /// Max edge width on edge animation.
        /// </summary>
        private float m_maxEdgeWidth = 20f;
        /// <summary>
        /// CanvasBrush on EditMode.
        /// </summary>
        private Brush m_editBGBrush = Brushes.White;
        /// <summary>
        /// Edge brush on EditMode.
        /// </summary>
        private Brush m_editEdgeBrush = Brushes.Black;
        /// <summary>
        /// CanvasBrush on ViewMode.
        /// </summary>
        private Brush m_viewBGBrush = Brushes.White;
        /// <summary>
        /// Edge brush on ViewMode.
        /// </summary>
        private Brush m_viewEdgeBrush = Brushes.LightGreen;
        /// <summary>
        /// Low threshold edge brush on ViewMode.
        /// </summary>
        private Brush m_lowEdgeBrush = Brushes.Gray;
        /// <summary>
        /// High threshold edge brush on ViewMode.
        /// </summary>
        private Brush m_highEdgeBrush = Brushes.Yellow;
        /// <summary>
        /// NG edge brush on ViewMode.
        /// </summary>
        private Brush m_ngEdgeBrush = Brushes.Red;
        /// <summary>
        /// Label brush on ViewMode.
        /// </summary>
        private Brush m_propBrush = Brushes.Blue;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isLogarithmic = true;
        /// <summary>
        /// 
        /// </summary>
        private bool m_autoThreshold = false;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isRecordMovie = false;
        /// <summary>
        /// Movie File
        /// </summary>
        private string m_movieFile = "ecell.avi";
        /// <summary>
        /// 
        /// </summary>
        private AviManager m_aviManager = null;
        /// <summary>
        /// 
        /// </summary>
        private VideoStream m_stream = null;
        #endregion

        #region Object Fields
        /// <summary>
        /// PathwayControl.
        /// </summary>
        protected PathwayControl m_con = null;
        /// <summary>
        /// CanvasControl.
        /// </summary>
        protected CanvasControl m_canvas = null;
        /// <summary>
        /// DataManager
        /// </summary>
        DataManager m_dManager = null;
        /// <summary>
        /// EventTimer for animation.
        /// </summary>
        private Timer m_timer;

        /// <summary>
        /// EventFlag isPausing
        /// </summary>
        private bool m_isPausing = false;
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public bool DoesAnimationOnGoing
        {
            get { return (m_con.ProjectStatus == ProjectStatus.Running || m_con.ProjectStatus == ProjectStatus.Stepping || m_con.ProjectStatus == ProjectStatus.Suspended); }
        }

        /// <summary>
        /// Get/Set m_thresholdHigh
        /// </summary>
        public float ThresholdHigh
        {
            get { return m_thresholdHigh; }
            set { m_thresholdHigh = value; }
        }

        /// <summary>
        /// Get/Set m_thresholdLow
        /// </summary>
        public float ThresholdLow
        {
            get { return m_thresholdLow; }
            set { m_thresholdLow = value; }
        }

        /// <summary>
        /// Get/Set m_normalEdgeWidth
        /// </summary>
        public float EdgeWidth
        {
            get { return m_normalEdgeWidth; }
            set { m_normalEdgeWidth = value; }
        }

        /// <summary>
        /// Get/Set m_maxEdgeWidth
        /// </summary>
        public float MaxEdgeWidth
        {
            get { return m_maxEdgeWidth; }
            set { m_maxEdgeWidth = value; }
        }

        /// <summary>
        /// Get BGBrush
        /// </summary>
        public Brush BGBrush
        {
            get
            {
                if (DoesAnimationOnGoing)
                    return ViewBGBrush;
                else
                    return EditBGBrush;
            }
        }

        /// <summary>
        /// Get/Set m_editBGBrush
        /// </summary>
        public Brush EditBGBrush
        {
            get { return m_editBGBrush; }
            set { m_editBGBrush = value; }
        }

        /// <summary>
        /// Get/Set m_viewBGBrush
        /// </summary>
        public Brush ViewBGBrush
        {
            get { return m_viewBGBrush; }
            set { m_viewBGBrush = value; }
        }

        /// <summary>
        /// Get EdgeBrush
        /// </summary>
        public Brush EdgeBrush
        {
            get
            {
                if (DoesAnimationOnGoing)
                    return ViewEdgeBrush;
                else
                    return EditEdgeBrush;
            }
        }

        /// <summary>
        /// Get/Set m_editEdgeBrush
        /// </summary>
        public Brush EditEdgeBrush
        {
            get { return m_editEdgeBrush; }
            set { m_editEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_viewEdgeBrush
        /// </summary>
        public Brush ViewEdgeBrush
        {
            get { return m_viewEdgeBrush; }
            set { m_viewEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_lowEdgeBrush
        /// </summary>
        public Brush LowEdgeBrush
        {
            get { return m_lowEdgeBrush; }
            set { m_lowEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_highEdgeBrush
        /// </summary>
        public Brush HighEdgeBrush
        {
            get { return m_highEdgeBrush; }
            set { m_highEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_ngEdgeBrush
        /// </summary>
        public Brush NgEdgeBrush
        {
            get { return m_ngEdgeBrush; }
            set { m_ngEdgeBrush = value; }
        }

        /// <summary>
        /// Get/Set m_propBrush
        /// </summary>
        public Brush PropertyBrush
        {
            get { return m_propBrush; }
            set { m_propBrush = value; }
        }

        /// <summary>
        /// Get/Set m_isLogarithmic
        /// </summary>
        public bool IsLogarithmic
        {
            get { return m_isLogarithmic; }
            set { m_isLogarithmic = value; }
        }
        /// <summary>
        /// Get/Set m_autoThreshold
        /// </summary>
        public bool AutoThreshold
        {
            get { return m_autoThreshold; }
            set { m_autoThreshold = value; }
        }
        /// <summary>
        /// Get/Set m_isRecordMovie
        /// </summary>
        public bool DoesRecordMovie
        {
            get { return m_isRecordMovie; }
            set { m_isRecordMovie = value; }
        }
        /// <summary>
        /// Get/Set m_movieFile
        /// </summary>
        public string AviFile
        {
            get { return m_movieFile; }
            set { m_movieFile = value; }
        }
        /// <summary>
        /// get PropertyDialogTabPage for Animation settings.
        /// </summary>
        public PropertyDialogPage AnimationSettingsPage
        {
            get { return new AnimationSettingsPage(this); }
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
            m_con = control;
            m_con.CanvasChange += new EventHandler(m_con_CanvasChange);
            m_con.ProjectStatusChange += new EventHandler(m_con_ProjectStatusChange);
            m_con.AnimationChange += new EventHandler(m_con_AnimationChange);
            LoadSettings();
            m_dManager = m_con.Window.DataManager;
            // Set Timer.
            m_timer = new Timer();
            m_timer.Enabled = false;
            m_timer.Interval = 200;
            m_timer.Tick += new EventHandler(TimerFire);
        }

        void m_con_AnimationChange(object sender, EventArgs e)
        {
            if (m_con.IsAnimation)
                SetSimulation(m_con.ProjectStatus);
            else
                StopSimulation();
        }

        /// <summary>
        /// Event on Dispose
        /// </summary>
        public void Dispose()
        {
            StopSimulation();
            m_con.ProjectStatusChange -= m_con_ProjectStatusChange;
        }

        #endregion

        #region EventHanlers
        /// <summary>
        /// Event on CnavasChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_con_CanvasChange(object sender, EventArgs e)
        {
            m_canvas = m_con.Canvas;
        }

        /// <summary>
        /// Event on ProjectStatusChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_con_ProjectStatusChange(object sender, EventArgs e)
        {
            ProjectStatus status = m_con.ProjectStatus;
            // When simulation started.
            SetSimulation(status);
        }

        private void SetSimulation(ProjectStatus status)
        {
            if (status == ProjectStatus.Running)
            {
                StartSimulation();
            }
            else if (status == ProjectStatus.Stepping)
            {
                StartSimulation();
            }
            else if (status == ProjectStatus.Suspended)
            {
                PauseSimulation();
            }
            else
            {
                StopSimulation();
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
            m_timer.Enabled = false;
            UpdatePropForSimulation();
            m_timer.Enabled = true;
        }
        /// <summary>
        /// Start Simulation
        /// </summary>
        public void StartSimulation()
        {
            if(!m_con.IsAnimation)
                return;
            if (m_autoThreshold)
                m_thresholdHigh = 0f;
            SetPropForSimulation();
            // Avi
            if (m_isRecordMovie && m_aviManager == null)
            {
                try
                {
                    // Delete File.
                    if (File.Exists(m_movieFile))
                        File.Delete(m_movieFile);
                    // Create Movie.
                    m_aviManager = new AviManager(m_movieFile, false);
                    Bitmap bmp = new Bitmap(m_canvas.PCanvas.Camera.ToImage(640, 480, m_canvas.BackGroundBrush));
                    m_stream = m_aviManager.AddVideoStream(false, 10, bmp);
                }
                catch(Exception e)
                {
                    Util.ShowErrorDialog(MessageResources.ErrCreateAvi + "\n" + e.Message);
                    m_isRecordMovie = false;
                    m_stream = null;
                    m_aviManager = null;
                }
            }
            TimerStart();
            m_isPausing = true;

        }
        /// <summary>
        /// Pause Simulation
        /// </summary>
        public void PauseSimulation()
        {
            TimerStop();
            m_isPausing = true;
        }
        /// <summary>
        /// Stop Simulation
        /// </summary>
        public void StopSimulation()
        {
            TimerStop();
            ResetPropForSimulation();
            m_isPausing = false;
            if (m_aviManager != null)
            {
                //m_stream.Close();
                m_aviManager.Close();
                m_aviManager = null;
                m_stream = null;
            }
        }
        /// <summary>
        /// Start Timer.
        /// </summary>
        public void TimerStart()
        {
            m_isPausing = false;
            m_timer.Enabled = true;
            m_timer.Start();
        }
        /// <summary>
        /// Stop Timer.
        /// </summary>
        public void TimerStop()
        {
            m_timer.Enabled = false;
            m_timer.Stop();
        }
        #endregion

        #region Methods for Animation
        /// <summary>
        /// 
        /// </summary>
        public void SetPropForSimulation()
        {
            if (m_con.Canvas == null)
                return;
            m_canvas = m_con.Canvas;
            m_canvas.BackGroundBrush = m_viewBGBrush;

            foreach (PPathwayProcess process in m_canvas.Processes.Values)
            {
                process.ViewMode = true;
                if (!process.Visible)
                    continue;
                // Line setting.
                foreach (PPathwayLine line in process.Relations)
                {
                    line.EdgeBrush = m_viewEdgeBrush;
                }
                // Set threshold
                float activity = GetFloatValue(process.EcellObject.FullID + ":" + Constants.xpathMolarActivity);
                if (m_autoThreshold)
                    SetThreshold(activity);
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                variable.MoveToFront();
                variable.PPropertyText.Visible = true;
                variable.PPropertyText.TextBrush = m_propBrush;
                variable.PPropertyText.MoveToFront();
            }
            if (m_isPausing)
                UpdatePropForSimulation();
        }

        /// <summary>
        /// Set threshold
        /// </summary>
        /// <param name="activity"></param>
        private void SetThreshold(float activity)
        {
            if (activity > m_thresholdHigh)
                m_thresholdHigh = activity;
            if (activity < m_thresholdLow)
                m_thresholdLow = activity;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdatePropForSimulation()
        {
            if (m_canvas == null)
                return;
            foreach (PPathwayProcess process in m_canvas.Processes.Values)
            {
                if (!process.Visible)
                    continue;

                // Line setting.
                float activity = GetFloatValue(process.EcellObject.FullID + ":" + Constants.xpathMolarActivity);
                float width = GetEdgeWidth(activity);
                Brush brush = GetEdgeBrush(activity);

                foreach (PPathwayLine line in process.Relations)
                {
                    if (line.Info.LineType != LineType.Dashed)
                        line.SetEdge(brush, width);
                }
                // Set threshold
                if (m_autoThreshold)
                    SetThreshold(activity);
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                // Variable setting.
                float molerConc = GetFloatValue(variable.EcellObject.FullID + ":" + Constants.xpathMolarConc);
                float width = GetEdgeWidth(molerConc);
                Brush brush = GetEdgeBrush(molerConc);

                variable.PPropertyText.Text = GetPropertyString(molerConc);
                // Set Effector.
                foreach (PPathwayLine line in variable.Relations)
                {
                    if (line.Info.LineType == LineType.Dashed)
                        line.SetEdge(brush, width);
                }

            }
            m_canvas.PCanvas.Refresh();

            // write video stream.
            if (m_stream != null)
            {
                Bitmap bmp = new Bitmap(
                    m_canvas.PCanvas.Camera.ToImage(640, 480, m_canvas.BackGroundBrush),
                    m_stream.Width,
                    m_stream.Height);
                m_stream.AddFrame(bmp);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetPropForSimulation()
        {
            TimerStop();
            if (m_canvas == null)
                return;
            m_canvas.BackGroundBrush = m_editBGBrush;
            // Reset objects.
            foreach (PPathwayObject obj in m_canvas.GetAllObjects())
                obj.Refresh();
            foreach (PPathwayProcess process in m_canvas.Processes.Values)
            {
                if (!process.Visible)
                    continue;
                // Line setting.
                process.ViewMode = false;
                foreach (PPathwayLine line in process.Relations)
                {
                    line.SetEdge(m_editEdgeBrush, m_normalEdgeWidth);
                }
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                // Line setting.
                variable.PPropertyText.Text = "";
                variable.PPropertyText.Visible = false;
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
                xmlOut.WriteElementString(AnimationConstants.xPathEditBGBrush, BrushManager.ParseBrushToString(m_editBGBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathEditEdgeBrush, BrushManager.ParseBrushToString(m_editEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathNormalEdgeWidth, m_normalEdgeWidth.ToString());

                xmlOut.WriteElementString(AnimationConstants.xPathViewBGBrush, BrushManager.ParseBrushToString(m_viewBGBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathViewEdgeBrush, BrushManager.ParseBrushToString(m_viewEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathMaxEdgeWidth, m_maxEdgeWidth.ToString());

                xmlOut.WriteElementString(AnimationConstants.xPathHighEdgeBrush, BrushManager.ParseBrushToString(m_highEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathLowEdgeBrush, BrushManager.ParseBrushToString(m_lowEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathAutoThreshold, m_autoThreshold.ToString());
                xmlOut.WriteElementString(AnimationConstants.xPathThresholdHigh, m_thresholdHigh.ToString());
                xmlOut.WriteElementString(AnimationConstants.xPathThresholdLow, m_thresholdLow.ToString());

                xmlOut.WriteElementString(AnimationConstants.xPathNGEdgeBrush, BrushManager.ParseBrushToString(m_ngEdgeBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathPropertyBrush, BrushManager.ParseBrushToString(m_propBrush));
                xmlOut.WriteElementString(AnimationConstants.xPathIsLogarithmic, m_isLogarithmic.ToString());

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
        /// Load Settings.
        /// </summary>
        public void LoadSettings()
        {
            string filename = Path.Combine(Util.GetUserDir(), AnimationConstants.xPathFileName);
            if (!File.Exists(filename))
                return;

            // Get Animation settings.
            XmlDocument xmlD = new XmlDocument();
            xmlD.Load(filename);
            XmlNode settings = GetAnimationSettings(xmlD);
            if (settings == null)
                return;

            // Load settings.
            foreach (XmlNode setting in settings.ChildNodes)
            {
                switch (setting.Name)
                {
                    // EditBGBrush
                    case AnimationConstants.xPathEditBGBrush:
                        m_editBGBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // EditEdgeBrush
                    case AnimationConstants.xPathEditEdgeBrush:
                        m_editEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // NormalEdgeWidth
                    case AnimationConstants.xPathNormalEdgeWidth:
                        m_normalEdgeWidth = float.Parse(setting.InnerText);
                        break;
                    // ViewBGBrush
                    case AnimationConstants.xPathViewBGBrush:
                        m_viewBGBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // ViewEdgeBrush
                    case AnimationConstants.xPathViewEdgeBrush:
                        m_viewEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // MaxEdgeWidth
                    case AnimationConstants.xPathMaxEdgeWidth:
                        m_maxEdgeWidth = float.Parse(setting.InnerText);
                        break;
                    // HighEdgeBrush
                    case AnimationConstants.xPathHighEdgeBrush:
                        m_highEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // LowEdgeBrush
                    case AnimationConstants.xPathLowEdgeBrush:
                        m_lowEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // ThresholdHigh
                    case AnimationConstants.xPathThresholdHigh:
                        m_thresholdHigh = float.Parse(setting.InnerText);
                        break;
                    // ThresholdLow
                    case AnimationConstants.xPathThresholdLow:
                        m_thresholdLow = float.Parse(setting.InnerText);
                        break;
                    // NGEdgeBrush
                    case AnimationConstants.xPathNGEdgeBrush:
                        m_ngEdgeBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // PropertyBrush
                    case AnimationConstants.xPathPropertyBrush:
                        m_propBrush = BrushManager.ParseStringToBrush(setting.InnerText);
                        break;
                    // IsLogarithmic
                    case AnimationConstants.xPathIsLogarithmic:
                        m_isLogarithmic = bool.Parse(setting.InnerText);
                        break;
                    // AutoThreshold
                    case AnimationConstants.xPathAutoThreshold:
                        m_autoThreshold = bool.Parse(setting.InnerText);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlD"></param>
        /// <returns></returns>
        private static XmlNode GetAnimationSettings(XmlDocument xmlD)
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
        /// <param name="fullPN"></param>
        /// <returns></returns>
        private float GetFloatValue(string fullPN)
        {
            float num = 0.0f;
            try
            {
                num = (float)m_dManager.GetPropertyValue(fullPN);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                num = float.NaN;
            }
            return num;
        }
        /// <summary>
        /// Get line width.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private float GetEdgeWidth(float activity)
        {
            if (float.IsNaN(activity))
                return 0f;
            else if (activity <= m_thresholdLow || m_thresholdHigh == 0f)
                return 0f;
            else if (activity >= m_thresholdHigh)
                return m_maxEdgeWidth;
            return m_maxEdgeWidth * activity / m_thresholdHigh;
        }
        /// <summary>
        /// Get line color
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private Brush GetEdgeBrush(float activity)
        {
            if (float.IsNaN(activity) || float.IsInfinity(activity))
                return m_ngEdgeBrush;
            else if (activity <= m_thresholdLow)
                return m_lowEdgeBrush;
            else if (activity >= m_thresholdHigh)
                return m_highEdgeBrush;
            return m_viewEdgeBrush;
        }
        /// <summary>
        /// GetPropertyString
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetPropertyString(float value)
        {
            if (m_isLogarithmic)
                return value.ToString(FormatLog);
            return value.ToString(FormatNatural);
        }


    }
}
