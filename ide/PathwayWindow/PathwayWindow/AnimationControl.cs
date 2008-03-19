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
using EcellLib.PathwayWindow.Nodes;
using System.Drawing;
using System.Drawing.Drawing2D;
using EcellLib.PathwayWindow.UIComponent;
using System.Diagnostics;
using EcellLib.PathwayWindow.Graphic;
using System.ComponentModel;

namespace EcellLib.PathwayWindow
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
        /// High threshold of line animation.
        /// </summary>
        private float m_thresholdHigh = 100f;
        /// <summary>
        /// Low threshold of line animation.
        /// </summary>
        private float m_thresholdMin = 0f;
        /// <summary>
        /// Normal line width.
        /// </summary>
        private float m_normalLineWidth = 0f;
        /// <summary>
        /// Max line width on line animation.
        /// </summary>
        private float m_maxLineWidth = 20f;
        /// <summary>
        /// CanvasBrush on EditMode.
        /// </summary>
        private Brush m_normalBGBrush = Brushes.White;
        /// <summary>
        /// CanvasBrush on ViewMode.
        /// </summary>
        private Brush m_viewBGBrush = Brushes.White;
        /// <summary>
        /// CanvasBrush on ViewMode.
        /// </summary>
        private Brush m_propBrush = Brushes.Blue;
        /// <summary>
        /// Line brush on EditMode.
        /// </summary>
        private Brush m_normalLineBrush = Brushes.Black;
        /// <summary>
        /// Line brush on ViewMode.
        /// </summary>
        private Brush m_viewLineBrush = Brushes.LightGreen;
        /// <summary>
        /// Low threshold line brush on ViewMode.
        /// </summary>
        private Brush m_minLineBrush = Brushes.Gray;
        /// <summary>
        /// High threshold line brush on ViewMode.
        /// </summary>
        private Brush m_maxLineBrush = Brushes.Yellow;
        /// <summary>
        /// NG line brush on ViewMode.
        /// </summary>
        private Brush m_ngLineBrush = Brushes.Red;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isLogarithmic = true;
        #endregion

        #region Object Fields
        /// <summary>
        /// PathwayControl.
        /// </summary>
        protected PathwayControl m_con = null;
        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        private ComponentResourceManager m_resources;
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
        /// MessageResourses
        /// </summary>
        public ComponentResourceManager Resources
        {
            get { return m_resources; }
            set { m_resources = value; }
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
        /// Get/Set m_thresholdMin
        /// </summary>
        public float ThresholdMin
        {
            get { return m_thresholdMin; }
            set { m_thresholdMin = value; }
        }

        /// <summary>
        /// Get/Set m_normalLineWidth
        /// </summary>
        public float NormalLineWidth
        {
            get { return m_normalLineWidth; }
            set { m_normalLineWidth = value; }
        }

        /// <summary>
        /// Get/Set m_maxLineWidth
        /// </summary>
        public float MaxLineWidth
        {
            get { return m_maxLineWidth; }
            set { m_maxLineWidth = value; }
        }

        /// <summary>
        /// Get/Set m_normalBGBrush
        /// </summary>
        public Brush NormalBGBrush
        {
            get { return m_normalBGBrush; }
            set { m_normalBGBrush = value; }
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
        /// Get/Set m_normalLineBrush
        /// </summary>
        public Brush NormalLineBrush
        {
            get { return m_normalLineBrush; }
            set { m_normalLineBrush = value; }
        }

        /// <summary>
        /// Get/Set m_viewLineBrush
        /// </summary>
        public Brush ViewLineBrush
        {
            get { return m_viewLineBrush; }
            set { m_viewLineBrush = value; }
        }

        /// <summary>
        /// Get/Set m_minLineBrush
        /// </summary>
        public Brush MinLineBrush
        {
            get { return m_minLineBrush; }
            set { m_minLineBrush = value; }
        }

        /// <summary>
        /// Get/Set m_maxLineBrush
        /// </summary>
        public Brush MaxLineBrush
        {
            get { return m_maxLineBrush; }
            set { m_maxLineBrush = value; }
        }

        /// <summary>
        /// Get/Set m_ngLineBrush
        /// </summary>
        public Brush NgLineBrush
        {
            get { return m_ngLineBrush; }
            set { m_ngLineBrush = value; }
        }

        /// <summary>
        /// Get/Set m_isLogarithmic
        /// </summary>
        public bool IsLogarithmic
        {
            get { return m_isLogarithmic; }
            set { m_isLogarithmic = value; }
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
            m_resources = control.Resources;
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
                float activity = GetFloatValue(process.EcellObject, "MolarActivity");
                process.EdgeBrush = m_viewLineBrush;
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
                float activity = GetFloatValue(process.EcellObject, "MolarActivity");
                process.EdgeBrush = GetEdgeBrush(activity);
                process.SetLineWidth(GetEdgeWidth(activity));
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                // Variable setting.
                float molerConc = GetFloatValue(variable.EcellObject, "MolarConc");
                variable.PPropertyText.Text = GetPropertyString(molerConc);
            }
            m_canvas.PathwayCanvas.Refresh();
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
                process.EdgeBrush = m_normalLineBrush;
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

        /// <summary>
        /// Create TabPage for PathwaySettingDialog
        /// </summary>
        /// <returns></returns>
        public PropertyDialogTabPage CreateTabPage()
        {
            PropertyDialogTabPage page = new AnimationTabPage(this);
            return page;
        }
        #endregion

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
                num = (float)value.CastToDouble();
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
            else if (activity <= m_thresholdMin)
                return 0f;
            else if (activity >= m_thresholdHigh)
                return m_maxLineWidth;
            return m_maxLineWidth * activity / m_thresholdHigh;
        }
        /// <summary>
        /// Get line color
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private Brush GetEdgeBrush(float activity)
        {
            if (float.IsNaN(activity) || float.IsInfinity(activity))
                return m_ngLineBrush;
            else if (activity <= m_thresholdMin)
                return m_minLineBrush;
            else if (activity >= m_thresholdHigh)
                return m_maxLineBrush;
            return m_viewLineBrush;
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
        /// private class for AnimationSettingDialog
        /// </summary>
        private class AnimationTabPage : PropertyDialogTabPage
        {
            private AnimationEditModeItem m_editModeItem;
            private AnimationViewModeItem m_viewModeItem;
            public AnimationTabPage(AnimationControl control)
            {
                m_editModeItem = new AnimationEditModeItem(control);
                m_viewModeItem = new AnimationViewModeItem(control);

                this.Text = "Pathway Setting";
                this.SuspendLayout();
                this.Controls.Add(m_editModeItem);
                this.Controls.Add(m_viewModeItem);
                
                m_viewModeItem.Top = m_editModeItem.Top + m_editModeItem.Height;
                this.ResumeLayout();
                this.PerformLayout();
            }

            public override void ApplyChange()
            {
                base.ApplyChange();
                m_editModeItem.ApplyChanges();
                m_viewModeItem.ApplyChanges();
            }
        }

        /// <summary>
        /// private class for AnimationSettingDialog
        /// </summary>
        private class AnimationEditModeItem : GroupBox
        {
            private PropertyBrushItem m_bgBrushItem;
            private PropertyBrushItem m_edgeBrushItem;
            private PropertyTextItem m_edgeWidth;

            private AnimationControl m_control;

            public AnimationEditModeItem(AnimationControl control)
            {
                m_control = control;

                // set Brushes
                List<string> list = BrushManager.GetBrushNameList();
                this.m_bgBrushItem = new PropertyBrushItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextBackgroundBrush), control.NormalBGBrush, list);
                this.m_edgeWidth = new PropertyTextItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextEdgeWidth), control.NormalLineWidth.ToString());
                this.m_edgeBrushItem = new PropertyBrushItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextEdgeBrush), control.NormalLineBrush, list);
                this.SuspendLayout();
                // 
                // Initialize
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(this.m_bgBrushItem);
                this.Controls.Add(this.m_edgeBrushItem);
                this.Controls.Add(this.m_edgeWidth);
                this.Text = m_control.Resources.GetString(PathwayDialogConstant.DialogTextEditMode);
                this.TabStop = false;

                // Set Position
                this.m_bgBrushItem.Location = new Point(5, 20);
                this.m_edgeBrushItem.Location = new Point(5, 50);
                this.m_edgeWidth.Location = new Point(230, 50);

                this.ResumeLayout(false);
                this.PerformLayout();
            }

            public void ApplyChanges()
            {
                m_control.NormalBGBrush = this.m_bgBrushItem.Brush;
                m_control.NormalLineBrush = this.m_edgeBrushItem.Brush;
                m_control.NormalLineWidth = float.Parse(this.m_edgeWidth.Text);
            }
        }

        /// <summary>
        /// private class for AnimationSettingDialog
        /// </summary>
        private class AnimationViewModeItem : GroupBox
        {
            private PropertyBrushItem m_bgBrush;
            private PropertyDialogItem m_animation;
            private PropertyBrushItem m_edgeBrush;
            private PropertyTextItem m_edgeWidth;

            private PropertyBrushItem m_edgeHighBrush;
            private PropertyBrushItem m_edgeLowBrush;
            private PropertyBrushItem m_edgeNGBrush;
            private PropertyTextItem m_thresholdHigh;
            private PropertyTextItem m_thresholdLow;
            private PropertyCheckBoxItem m_lineCheckBox;

            private AnimationControl m_control;

            public AnimationViewModeItem(AnimationControl control)
            {
                m_control = control;
                // set Brushes
                List<string> list = BrushManager.GetBrushNameList();
                m_bgBrush = new PropertyBrushItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextBackgroundBrush), control.ViewBGBrush, list);
                m_animation = new PropertyDialogItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextAnimationSetting));
                m_edgeBrush = new PropertyBrushItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextEdgeBrush), control.ViewLineBrush, list);
                m_edgeWidth = new PropertyTextItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextMaxEdgeWidth), control.MaxLineWidth.ToString());

                m_edgeHighBrush = new PropertyBrushItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextThresholdHigh), control.MaxLineBrush, list);
                m_edgeLowBrush = new PropertyBrushItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextThresholdLow), control.MinLineBrush, list);
                m_edgeNGBrush = new PropertyBrushItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextNGBrush), control.NgLineBrush, list);

                m_thresholdHigh = new PropertyTextItem("", control.ThresholdHigh.ToString());
                m_thresholdLow = new PropertyTextItem("", control.ThresholdMin.ToString());
                m_lineCheckBox = new PropertyCheckBoxItem(m_control.Resources.GetString(PathwayDialogConstant.DialogTextLogarithmic), control.IsLogarithmic);

                this.SuspendLayout();
                // 
                // This
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(m_bgBrush);
                this.Controls.Add(m_animation);
                this.Controls.Add(m_edgeBrush);
                this.Controls.Add(m_edgeHighBrush);
                this.Controls.Add(m_edgeLowBrush);
                this.Controls.Add(m_edgeWidth);
                this.Controls.Add(m_thresholdHigh);
                this.Controls.Add(m_thresholdLow);
                this.Controls.Add(m_edgeNGBrush);
                this.Controls.Add(m_lineCheckBox);
                this.Text = m_control.Resources.GetString(PathwayDialogConstant.DialogTextViewMode);
                this.TabStop = false;

                // SetPosition 
                m_bgBrush.Location = new Point(5, 20);
                m_edgeBrush.Location = new Point(5, 50);
                m_edgeWidth.Location = new Point(230, 50);
                m_animation.Location = new Point(5, 80);

                m_edgeHighBrush.Location = new Point(5, 110);
                m_edgeLowBrush.Location = new Point(5, 140);
                m_thresholdHigh.Location = new Point(170, 110);
                m_thresholdLow.Location = new Point(170, 140);
                m_edgeNGBrush.Location = new Point(5, 170);
                m_lineCheckBox.Location = new Point(5, 200);

                this.ResumeLayout(false);
                this.PerformLayout();
            }

            public void ApplyChanges()
            {
                m_control.ViewBGBrush = m_bgBrush.Brush;
                m_control.MaxLineWidth = float.Parse(m_edgeWidth.Text);
                m_control.ViewLineBrush = m_edgeBrush.Brush;
                m_control.ThresholdHigh = float.Parse(m_thresholdHigh.Text);
                m_control.ThresholdMin = float.Parse(m_thresholdLow.Text);
                m_control.MaxLineBrush = m_edgeHighBrush.Brush;
                m_control.MinLineBrush = m_edgeLowBrush.Brush;
                m_control.NgLineBrush = m_edgeNGBrush.Brush;
                m_control.IsLogarithmic = m_lineCheckBox.Checked;
            }
        }
    }

    /// <summary>
    /// PathwayDialogConstant
    /// </summary>
    public class PathwayDialogConstant
    {
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
        /// DialogTextLogarithmic
        /// </summary>
        public const string DialogTextLogarithmic = "DialogTextLogarithmic";
    }
}
