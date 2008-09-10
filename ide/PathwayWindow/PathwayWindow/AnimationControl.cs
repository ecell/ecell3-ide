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
using Ecell.IDE.Plugins.PathwayWindow.Dialog;
using System.IO;
using System.Xml;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PathwayWindow
{
    /// <summary>
    /// AnimationControl
    /// </summary>
    public class AnimationControl
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
        private float m_normalEdgeWidth = 0f;
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
        private bool m_autoThreshold = true;
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
        private Timer m_time;

        /// <summary>
        /// EventFlag isPausing
        /// </summary>
        private bool m_isPausing = false;
        #endregion

        #region Accessors
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
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public AnimationControl(PathwayControl control)
        {
            m_con = control;
            LoadSettings();
            m_dManager = m_con.Window.DataManager;
            // Set Timer.
            m_time = new Timer();
            m_time.Enabled = false;
            m_time.Interval = 200;
            m_time.Tick += new EventHandler(TimerFire);
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
            m_time.Enabled = false;
            UpdatePropForSimulation();
            m_time.Enabled = true;
        }
        /// <summary>
        /// Start Simulation
        /// </summary>
        public void StartSimulation()
        {
            if (m_autoThreshold)
                m_thresholdHigh = 0f;
            SetPropForSimulation();
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
        /// Step Simulation
        /// </summary>
        public void StepSimulation()
        {
            SetPropForSimulation();
            UpdatePropForSimulation();
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
        }
        /// <summary>
        /// Start Timer.
        /// </summary>
        public void TimerStart()
        {
            m_isPausing = false;
            m_time.Enabled = true;
            m_time.Start();
        }
        /// <summary>
        /// Stop Timer.
        /// </summary>
        public void TimerStop()
        {
            m_time.Enabled = false;
            m_time.Stop();
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
            foreach (PPathwayProcess process in m_canvas.Processes.Values)
            {
                if (!process.Visible)
                    continue;
                // Line setting.
                float activity = GetFloatValue(process.EcellObject, Constants.xpathMolarActivity);
                process.EdgeBrush = m_viewEdgeBrush;
                // Set threshold
                if (m_autoThreshold && activity > m_thresholdHigh)
                    m_thresholdHigh = activity;
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                variable.MoveToFront();
                variable.PPropertyText.TextBrush = m_propBrush;
                variable.PPropertyText.MoveToFront();
            }
            if (m_isPausing)
                UpdatePropForSimulation();
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
                float activity = GetFloatValue(process.EcellObject, Constants.xpathMolarActivity);
                process.EdgeBrush = GetEdgeBrush(activity);
                process.SetLineWidth(GetEdgeWidth(activity));
                // Set threshold
                if (m_autoThreshold && activity > m_thresholdHigh)
                    m_thresholdHigh = activity;
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                // Variable setting.
                float molerConc = GetFloatValue(variable.EcellObject, Constants.xpathMolarConc);
                variable.PPropertyText.Text = GetPropertyString(molerConc);
            }
            m_canvas.PCanvas.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetPropForSimulation()
        {
            TimerStop();
            if (m_canvas == null)
                return;
            // Reset objects.
            foreach (PPathwayObject obj in m_canvas.GetAllObjects())
                obj.Refresh();
            foreach (PPathwayProcess process in m_canvas.Processes.Values)
            {
                if (!process.Visible)
                    continue;
                // Line setting.
                process.EdgeBrush = m_viewEdgeBrush;
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                // Line setting.
                variable.PPropertyText.Text = "";
            }
            m_canvas = null;
        }
        #endregion

        /// <summary>
        /// Create TabPage for PathwaySettingDialog
        /// </summary>
        /// <returns></returns>
        public PropertyDialogTabPage CreateTabPage()
        {
            PropertyDialogTabPage page = new AnimationTabPage(this);
            return page;
        }

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
                xmlOut.WriteComment(PathwayConstants.xPathFileHeader1);
                xmlOut.WriteComment(PathwayConstants.xPathFileHeader2);

                xmlOut.WriteStartElement(AnimationConstants.xPathAnimationSettings);
                // Application settings.
                xmlOut.WriteAttributeString(PathwayConstants.xPathName, Application.ProductName);
                xmlOut.WriteAttributeString(PathwayConstants.xPathFileVersion, AnimationConstants.xPathVersion);
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
        /// <param name="eo"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        private float GetFloatValue(EcellObject eo ,string propName)
        {
            string fullpath = eo.Type + ":" + eo.Key + ":" + propName;
            EcellValue value = null;
            float num = 0f;
            try
            {
                value = m_dManager.GetEntityProperty(fullpath);
                num = (float)((double)value);
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

        /// <summary>
        /// PathwayDialogConstant
        /// </summary>
        internal class AnimationConstants
        {
            #region Constants for dialog text.
            /// <summary>
            /// DialogTextAnimationSetting
            /// </summary>
            public const string DialogTextAnimationSetting = "DialogTextAnimationSetting";
            /// <summary>
            /// DialogTextBackgroundBrush
            /// </summary>
            public const string DialogTextBackgroundBrush = "DialogTextBackgroundBrush";
            /// <summary>
            /// DialogTextEdgeBrush
            /// </summary>
            public const string DialogTextEdgeBrush = "DialogTextEdgeBrush";
            /// <summary>
            /// DialogTextEditMode
            /// </summary>
            public const string DialogTextEditMode = "DialogTextEditMode";
            /// <summary>
            /// DialogTextEdgeWidth
            /// </summary>
            public const string DialogTextEdgeWidth = "DialogTextEdgeWidth";
            /// <summary>
            /// DialogTextMaxEdgeWidth
            /// </summary>
            public const string DialogTextMaxEdgeWidth = "DialogTextMaxEdgeWidth";
            /// <summary>
            /// DialogTextNGBrush
            /// </summary>
            public const string DialogTextNGBrush = "DialogTextNGBrush";
            /// <summary>
            /// DialogTextNormalEdge
            /// </summary>
            public const string DialogTextNormalEdge = "DialogTextNormalEdge";
            /// <summary>
            /// DialogTextThresholdHigh
            /// </summary>
            public const string DialogTextThresholdHigh = "DialogTextThresholdHigh";
            /// <summary>
            /// DialogTextThresholdLow
            /// </summary>
            public const string DialogTextThresholdLow = "DialogTextThresholdLow";
            /// <summary>
            /// DialogTextViewMode
            /// </summary>
            public const string DialogTextViewMode = "DialogTextViewMode";
            /// <summary>
            /// DialogTextViewMode
            /// </summary>
            public const string DialogTextPropertyBrush = "DialogTextPropertyBrush";
            /// <summary>
            /// DialogTextLogarithmic
            /// </summary>
            public const string DialogTextLogarithmic = "DialogTextLogarithmic";
            #endregion

            #region Constants for XML.
            /// <summary>
            /// 
            /// </summary>
            public const string xPathFileName = "AnimationSettings.xml";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathAnimationSettings = "AnimationSettings";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathVersion = "1.0";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathAutoThreshold = "AutoThreshold";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathThresholdHigh = "ThresholdHigh";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathThresholdLow = "ThresholdLow";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathNormalEdgeWidth = "NormalEdgeWidth";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathMaxEdgeWidth = "MaxEdgeWidth";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathEditBGBrush = "EditBGBrush";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathEditEdgeBrush = "EditEdgeBrush";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathViewBGBrush = "ViewBGBrush";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathViewEdgeBrush = "ViewEdgeBrush";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathLowEdgeBrush = "LowEdgeBrush";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathHighEdgeBrush = "HighEdgeBrush";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathNGEdgeBrush = "NGEdgeBrush";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathPropertyBrush = "PropertyBrush";
            /// <summary>
            /// 
            /// </summary>
            public const string xPathIsLogarithmic = "IsLogarithmic";
            #endregion
        }

        /// <summary>
        /// private class for AnimationSettingDialog
        /// </summary>
        internal class AnimationTabPage : PropertyDialogTabPage
        {
            private AnimationControl m_con;
            private EditModeItems m_editModeItems;
            private ViewModeItems m_viewModeItems;
            private AnimationItems m_animationItems;

            public AnimationTabPage(AnimationControl control)
            {
                m_con = control;
                m_editModeItems = new EditModeItems(control);
                m_viewModeItems = new ViewModeItems(control);
                m_animationItems = new AnimationItems(control);

                this.Text = MessageResources.DialogTextPathwaySetting;
                this.SuspendLayout();
                this.Controls.Add(m_editModeItems);
                this.Controls.Add(m_viewModeItems);
                this.Controls.Add(m_animationItems);

                m_viewModeItems.Top = m_editModeItems.Top + m_editModeItems.Height;
                m_animationItems.Top = m_viewModeItems.Top + m_viewModeItems.Height;
                this.ResumeLayout();
                this.PerformLayout();
            }

            public override void ApplyChange()
            {
                try
                {
                    base.ApplyChange();
                    m_editModeItems.ApplyChanges();
                    m_viewModeItems.ApplyChanges();
                    m_animationItems.ApplyChanges();
                    m_con.SaveSettings();
                }
                catch (Exception)
                {
                    Util.ShowErrorDialog(MessageResources.ErrUpdateConfig);
                }
            }
        }

        /// <summary>
        /// private class for AnimationSettingDialog
        /// </summary>
        internal class EditModeItems : GroupBox
        {
            private PropertyBrushItem m_bgBrushItem;
            private PropertyBrushItem m_edgeBrushItem;
            private PropertyTextItem m_edgeWidth;

            private AnimationControl m_control;

            public EditModeItems(AnimationControl control)
            {
                m_control = control;

                // set Brushes
                this.m_bgBrushItem = new PropertyBrushItem(MessageResources.DialogTextBackgroundBrush, control.EditBGBrush);
                this.m_edgeWidth = new PropertyTextItem(MessageResources.DialogTextEdgeWidth, control.EdgeWidth.ToString());
                this.m_edgeBrushItem = new PropertyBrushItem(MessageResources.DialogTextEdgeBrush, control.EditEdgeBrush);
                this.SuspendLayout();
                // 
                // Initialize
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(this.m_bgBrushItem);
                this.Controls.Add(this.m_edgeBrushItem);
                this.Controls.Add(this.m_edgeWidth);
                this.Text = MessageResources.DialogTextEditMode;
                this.TabStop = false;

                // Set Position
                this.m_bgBrushItem.Location = new Point(10, 20);
                this.m_edgeBrushItem.Location = new Point(10, 45);
                this.m_edgeWidth.Location = new Point(10, 70);
                this.m_edgeWidth.Validating += new System.ComponentModel.CancelEventHandler(EdgeWidthValidating);

                this.ResumeLayout(false);
                this.PerformLayout();
                this.Height = 120;

            }

            void EdgeWidthValidating(object sender, System.ComponentModel.CancelEventArgs e)
            {
                string text = m_edgeWidth.Text;
                if (String.IsNullOrEmpty(text))
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.DialogTextEdgeWidth));
                    m_edgeWidth.Text = Convert.ToString(m_control.EdgeWidth);
                    e.Cancel = true;
                    return;
                }
                float dummy;
                if (!float.TryParse(text, out dummy) || dummy < 0)
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    m_edgeWidth.Text = Convert.ToString(m_control.EdgeWidth);
                    e.Cancel = true;
                    return;
                }
            }

            public void ApplyChanges()
            {
                m_control.EditBGBrush = this.m_bgBrushItem.Brush;
                m_control.EditEdgeBrush = this.m_edgeBrushItem.Brush;
                m_control.EdgeWidth = float.Parse(this.m_edgeWidth.Text);
            }
        }

        /// <summary>
        /// private class for AnimationSettingDialog
        /// </summary>
        internal class ViewModeItems : GroupBox
        {
            private PropertyBrushItem m_bgBrush;
            private PropertyBrushItem m_edgeBrush;
            private PropertyTextItem m_edgeWidth;

            private AnimationControl m_control;

            public ViewModeItems(AnimationControl control)
            {
                m_control = control;
                // set Brushes
                m_bgBrush = new PropertyBrushItem(MessageResources.DialogTextBackgroundBrush, control.ViewBGBrush);
                m_edgeBrush = new PropertyBrushItem(MessageResources.DialogTextEdgeBrush, control.ViewEdgeBrush);
                m_edgeWidth = new PropertyTextItem(MessageResources.DialogTextMaxEdgeWidth, control.MaxEdgeWidth.ToString());

                this.SuspendLayout();
                // 
                // This
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(m_bgBrush);
                this.Controls.Add(m_edgeBrush);
                this.Controls.Add(m_edgeWidth);
                this.Text = MessageResources.DialogTextViewMode;
                this.TabStop = false;

                // SetPosition 
                this.m_bgBrush.Location = new Point(10, 20);
                this.m_edgeBrush.Location = new Point(10, 45);
                this.m_edgeWidth.Location = new Point(10, 70);
                this.m_edgeWidth.Validating += new System.ComponentModel.CancelEventHandler(MaxEdgeWidthValidating);

                this.ResumeLayout(false);
                this.PerformLayout();
                this.Height = 120;
            }

            void MaxEdgeWidthValidating(object sender, System.ComponentModel.CancelEventArgs e)
            {
                string text = m_edgeWidth.Text;
                if (String.IsNullOrEmpty(text))
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.DialogTextMaxEdgeWidth));
                    m_edgeWidth.Text = Convert.ToString(m_control.MaxEdgeWidth);
                    e.Cancel = true;
                    return;
                }
                float dummy;
                if (!float.TryParse(text, out dummy) || dummy < 0)
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    m_edgeWidth.Text = Convert.ToString(m_control.MaxEdgeWidth);
                    e.Cancel = true;
                    return;
                }
            }

            public void ApplyChanges()
            {
                m_control.ViewBGBrush = m_bgBrush.Brush;
                m_control.MaxEdgeWidth = float.Parse(m_edgeWidth.Text);
                m_control.ViewEdgeBrush = m_edgeBrush.Brush;
            }
        }

        /// <summary>
        /// private class for AnimationSettingDialog
        /// </summary>
        internal class AnimationItems : GroupBox
        {
            private PropertyBrushItem m_edgeHighBrush;
            private PropertyBrushItem m_edgeLowBrush;
            private PropertyBrushItem m_edgeNGBrush;
            private PropertyTextItem m_thresholdHigh;
            private PropertyTextItem m_thresholdLow;
            private PropertyCheckBoxItem m_autoShreshold;
            private PropertyBrushItem m_propBrush;
            private PropertyCheckBoxItem m_lineCheckBox;

            private AnimationControl m_control;

            public AnimationItems(AnimationControl control)
            {
                m_control = control;
                // set Brushes
                m_thresholdHigh = new PropertyTextItem(MessageResources.DialogTextThresholdHigh, control.ThresholdHigh.ToString());
                m_edgeHighBrush = new PropertyBrushItem("　　　" + MessageResources.DialogTextEdgeBrush, control.HighEdgeBrush);
                m_thresholdLow = new PropertyTextItem(MessageResources.DialogTextThresholdLow, control.ThresholdLow.ToString());
                m_edgeLowBrush = new PropertyBrushItem("　　　" + MessageResources.DialogTextEdgeBrush, control.LowEdgeBrush);
                m_edgeNGBrush = new PropertyBrushItem(MessageResources.DialogTextNGBrush, control.NgEdgeBrush);
                m_propBrush = new PropertyBrushItem(MessageResources.DialogTextPropertyBrush, control.PropertyBrush);
                m_lineCheckBox = new PropertyCheckBoxItem(MessageResources.DialogTextLogarithmic, control.IsLogarithmic);
                m_autoShreshold = new PropertyCheckBoxItem(MessageResources.DialogTextAutoThreshold, control.AutoThreshold);
                this.SuspendLayout();
                // 
                // This
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(m_edgeHighBrush);
                this.Controls.Add(m_edgeLowBrush);
                this.Controls.Add(m_autoShreshold);
                this.Controls.Add(m_thresholdHigh);
                this.Controls.Add(m_thresholdLow);
                this.Controls.Add(m_edgeNGBrush);
                this.Controls.Add(m_propBrush);
                this.Controls.Add(m_lineCheckBox);
                this.Text = MessageResources.DialogTextAnimationSetting;
                this.TabStop = false;

                // SetPosition 
                m_autoShreshold.Location = new Point(10, 20);
                m_thresholdHigh.Location = new Point(10, 45);
                m_edgeHighBrush.Location = new Point(10, 70);
                m_thresholdLow.Location = new Point(10, 95);
                m_edgeLowBrush.Location = new Point(10, 120);
                m_edgeNGBrush.Location = new Point(10, 145);
                m_propBrush.Location = new Point(10, 170);
                m_lineCheckBox.Location = new Point(10, 195);

                m_thresholdHigh.Validating += new System.ComponentModel.CancelEventHandler(HighThresholdValidating);
                m_thresholdLow.Validating += new System.ComponentModel.CancelEventHandler(LowThresholdValidating);

                this.ResumeLayout(false);
                this.PerformLayout();
            }

            void HighThresholdValidating(object sender, System.ComponentModel.CancelEventArgs e)
            {
                string text = m_thresholdHigh.Text;
                if (String.IsNullOrEmpty(text))
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.DialogTextThresholdHigh));
                    m_thresholdHigh.Text = Convert.ToString(m_control.ThresholdHigh);
                    e.Cancel = true;
                    return;
                }
                float dummy;
                if (!float.TryParse(text, out dummy) || Convert.ToDouble(m_thresholdLow.Text) > dummy)
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    m_thresholdHigh.Text = Convert.ToString(m_control.ThresholdHigh);
                    e.Cancel = true;
                    return;
                }
            }

            void LowThresholdValidating(object sender, System.ComponentModel.CancelEventArgs e)
            {
                string text = m_thresholdLow.Text;
                if (String.IsNullOrEmpty(text))
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.DialogTextThresholdLow));
                    m_thresholdLow.Text = Convert.ToString(m_control.ThresholdLow);
                    e.Cancel = true;
                    return;
                }
                float dummy;
                if (!float.TryParse(text, out dummy) || Convert.ToDouble(m_thresholdHigh.Text) < dummy)
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    m_thresholdLow.Text = Convert.ToString(m_control.ThresholdLow);
                    e.Cancel = true;
                    return;
                }
            }

            public void ApplyChanges()
            {
                m_control.AutoThreshold = m_autoShreshold.Checked;
                m_control.ThresholdHigh = float.Parse(m_thresholdHigh.Text);
                m_control.ThresholdLow = float.Parse(m_thresholdLow.Text);
                m_control.HighEdgeBrush = m_edgeHighBrush.Brush;
                m_control.LowEdgeBrush = m_edgeLowBrush.Brush;
                m_control.NgEdgeBrush = m_edgeNGBrush.Brush;
                m_control.PropertyBrush = m_propBrush.Brush;
                m_control.IsLogarithmic = m_lineCheckBox.Checked;
            }
        }
    }
}
