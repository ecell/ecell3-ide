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

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// AnimationControl
    /// </summary>
    public class AnimationControl
    {
        #region Fields
        private float m_thresholdHigh = 100f;
        private float m_thresholdMin = 0f;
        private float m_normalLineWidth = 0f;
        private float m_maxLineWidth = 20f;
        private Brush m_normalBGBrush = Brushes.White;
        private Brush m_viewBGBrush = Brushes.White;
        private Brush m_normalLineBrush = Brushes.Black;
        private Brush m_viewLineBrush = Brushes.LightGreen;
        private Brush m_minLineBrush = Brushes.Gray;
        private Brush m_maxLineBrush = Brushes.Yellow;
        private Brush m_ngLineBrush = Brushes.Red;
        private bool m_isLogarithmic = true;
        private const string FormatLog = "E2";
        private const string FormatNatural = "0.000000";
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
            if (m_con.CanvasControl == null)
                return;
            m_canvas = m_con.CanvasControl;
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
            private AnimationBackGroundItem m_boxBackGroundBrush;
            private AnimationLineItem m_boxLineSettings;
            public AnimationTabPage(AnimationControl control)
            {
                m_boxBackGroundBrush = new AnimationBackGroundItem(control);
                m_boxLineSettings = new AnimationLineItem(control);

                this.Text = "AnimationSettings";
                this.SuspendLayout();
                this.Controls.Add(m_boxBackGroundBrush);
                this.Controls.Add(m_boxLineSettings);
                
                m_boxLineSettings.Top = m_boxBackGroundBrush.Top + m_boxBackGroundBrush.Height;
                this.ResumeLayout();
                this.PerformLayout();
            }

            public override void ApplyChange()
            {
                base.ApplyChange();
                m_boxBackGroundBrush.ApplyChanges();
                m_boxLineSettings.ApplyChanges();
            }
        }

        /// <summary>
        /// private class for AnimationSettingDialog
        /// </summary>
        private class AnimationBackGroundItem : GroupBox
        {
            private PropertyBrushItem m_normalBrushItem;
            private PropertyBrushItem m_viewBrushItem;
            private AnimationControl m_control;

            public Brush NomalBrush
            {
                get { return m_normalBrushItem.Brush; }
                set { m_normalBrushItem.Brush = value; }
            }

            public Brush ViewBrush
            {
                get { return m_viewBrushItem.Brush; }
                set { m_viewBrushItem.Brush = value; }
            }

            public AnimationBackGroundItem(AnimationControl control)
            {
                m_control = control;

                // set Brushes
                List<string> list = BrushManager.GetBrushNameList();
                m_normalBrushItem = new PropertyBrushItem("Nomal Mode", control.NormalBGBrush, list);
                m_viewBrushItem = new PropertyBrushItem("View Mode", control.ViewBGBrush, list);

                this.SuspendLayout();
                // 
                // Initialize
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(this.m_normalBrushItem);
                this.Controls.Add(this.m_viewBrushItem);
                this.Text = "BackGroundBrush";
                this.TabStop = false;

                // Set Position
                this.m_normalBrushItem.Location = new System.Drawing.Point(5, 20);
                this.m_viewBrushItem.Location = new System.Drawing.Point(5, 50);

                this.ResumeLayout(false);
                this.PerformLayout();
            }

            public void ApplyChanges()
            {
                m_control.NormalBGBrush = this.m_normalBrushItem.Brush;
                m_control.ViewBGBrush = this.m_viewBrushItem.Brush;
            }
        }

        /// <summary>
        /// private class for AnimationSettingDialog
        /// </summary>
        private class AnimationLineItem : GroupBox
        {
            private PropertyBrushItem m_lineNormalBrush;
            private PropertyBrushItem m_lineViewBrush;
            private PropertyTextItem m_lineNormalWidth;
            private PropertyTextItem m_lineViewWidth;
            private PropertyCheckBoxItem m_lineCheckBox;
            private PropertyTextItem m_lineThresholdHigh;
            private PropertyTextItem m_lineThresholdLow;
            private PropertyBrushItem m_lineHighBrush;
            private PropertyBrushItem m_lineLowBrush;
            private PropertyBrushItem m_lineNGBrush;
            private AnimationControl m_control;

            public AnimationLineItem(AnimationControl control)
            {
                m_control = control;
                // set Brushes
                List<string> list = BrushManager.GetBrushNameList();
                this.m_lineNormalBrush = new PropertyBrushItem("Normal Line Brush", control.NormalLineBrush, list);
                this.m_lineViewBrush = new PropertyBrushItem("View Mode Line Brush", control.ViewLineBrush, list);
                this.m_lineHighBrush = new PropertyBrushItem("High Threshold Brush", control.MaxLineBrush, list);
                this.m_lineLowBrush = new PropertyBrushItem("Low Threshold Brush", control.MinLineBrush, list);
                this.m_lineNGBrush = new PropertyBrushItem("NG Brush", control.NgLineBrush, list);

                this.m_lineNormalWidth = new PropertyTextItem("Normal Line Width", control.NormalLineWidth.ToString());
                this.m_lineViewWidth = new PropertyTextItem("View Mode Line Width", control.MaxLineWidth.ToString());
                this.m_lineThresholdHigh = new PropertyTextItem("High Threshold", control.ThresholdHigh.ToString());
                this.m_lineThresholdLow = new PropertyTextItem("Low Threshold", control.ThresholdMin.ToString());

                this.m_lineCheckBox = new PropertyCheckBoxItem("logarithmic display", control.IsLogarithmic);

                this.SuspendLayout();
                // 
                // This
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(this.m_lineNormalBrush);
                this.Controls.Add(this.m_lineViewBrush);
                this.Controls.Add(this.m_lineHighBrush);
                this.Controls.Add(this.m_lineLowBrush);
                this.Controls.Add(this.m_lineNGBrush);
                this.Controls.Add(this.m_lineNormalWidth);
                this.Controls.Add(this.m_lineViewWidth);
                this.Controls.Add(this.m_lineThresholdHigh);
                this.Controls.Add(this.m_lineThresholdLow);
                this.Controls.Add(this.m_lineCheckBox);
                this.Text = "LineSettings";
                this.TabStop = false;

                // SetPosition 
                this.m_lineNormalBrush.Location = new System.Drawing.Point(5, 20);
                this.m_lineViewBrush.Location = new System.Drawing.Point(5, 50);
                this.m_lineNormalWidth.Location = new System.Drawing.Point(5, 80);
                this.m_lineViewWidth.Location = new System.Drawing.Point(5, 110);
                this.m_lineCheckBox.Location = new System.Drawing.Point(5, 140);
                this.m_lineThresholdHigh.Location = new System.Drawing.Point(5, 170);
                this.m_lineThresholdLow.Location = new System.Drawing.Point(5, 200);
                this.m_lineHighBrush.Location = new System.Drawing.Point(5, 230);
                this.m_lineLowBrush.Location = new System.Drawing.Point(5, 260);
                this.m_lineNGBrush.Location = new System.Drawing.Point(5, 290);

                this.ResumeLayout(false);
                this.PerformLayout();
            }

            public void ApplyChanges()
            {
                m_control.NormalLineBrush = this.m_lineNormalBrush.Brush;
                m_control.ViewLineBrush = this.m_lineViewBrush.Brush;
                m_control.MaxLineBrush = this.m_lineHighBrush.Brush;
                m_control.MinLineBrush = this.m_lineLowBrush.Brush;
                m_control.NgLineBrush = this.m_lineNGBrush.Brush;
                m_control.NormalLineWidth = float.Parse(this.m_lineNormalWidth.Text);
                m_control.MaxLineWidth = float.Parse(this.m_lineViewWidth.Text);
                m_control.ThresholdHigh = float.Parse(this.m_lineThresholdHigh.Text);
                m_control.ThresholdMin = float.Parse(this.m_lineThresholdLow.Text);
                m_control.IsLogarithmic = this.m_lineCheckBox.Checked;
            }
        }
    }
}
