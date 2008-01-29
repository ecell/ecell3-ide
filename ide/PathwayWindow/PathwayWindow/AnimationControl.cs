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

namespace EcellLib.PathwayWindow
{
    public class AnimationControl
    {
        #region constant fields
        bool isLogarithm = false;
        protected float ThresholdHigh = 100f;
        protected float ThresholdMin = 0f;
        protected float NormalLineWidth = 0f;
        protected float MaxLineWidth = 20f;
        protected Brush BackGroundBrush = Brushes.White;
        protected Brush ViewModeBGBrush = Brushes.White;
        protected Brush NormalLineBrush = Brushes.Black;
        protected Brush ViewLineBrush = Brushes.LightGreen;
        protected Brush MinLineBrush = Brushes.Gray;
        protected Brush MaxLineBrush = Brushes.Yellow;
        protected Brush NGLineBrush = Brushes.Red;
        protected bool IsLogarithmic = true;
        protected string FormatLog = "E2";
        protected string FormatNatural = "0.000000";
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
        /// Start Timer.
        /// </summary>
        public void TimerStart()
        {
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
            if (m_con.ActiveCanvas == null)
                return;
            m_canvas = m_con.ActiveCanvas;
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
                process.MoveToFront();
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                // Variable setting.
                float molerConc = GetFloatValue(variable.EcellObject, "MolarConc");
                variable.PPropertyText.Text = molerConc.ToString(FormatLog);
                variable.MoveToFront();
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
                process.EdgeBrush = Brushes.Black;
                process.MoveToFront();
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                // Line setting.
                variable.PPropertyText.Text = "";
                variable.MoveToFront();
            }
            m_canvas = null;
        }

        /// <summary>
        /// Create TabPage for PathwaySettingDialog
        /// </summary>
        /// <returns></returns>
        public TabPage CreateTabPage()
        {
            TabPage page = new TabPage("AnimationSettings");
            page.SuspendLayout();
            GroupBox boxBackGroundBrush = new AnimationBackGroundItem(this);
            page.Controls.Add(boxBackGroundBrush);
            GroupBox boxLineBrush = new AnimationLineItem(this);
            boxLineBrush.Top = boxBackGroundBrush.Top + boxBackGroundBrush.Height;
            page.Controls.Add(boxLineBrush);
            page.ResumeLayout();
            page.PerformLayout();
            return page;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        private float GetFloatValue(EcellObject eo ,string propName)
        {
            string fullpath = eo.type + ":" + eo.key + ":" + propName;
            EcellValue value = m_dManager.GetEntityProperty(fullpath);
            //EcellObject currentObj = m_con.Window.GetEcellObject(eo.modelID, eo.key, eo.type);
            //EcellValue value = currentObj.GetEcellValue(propName);
            if (value == null)
                return 0f;
            return (float)value.CastToDouble();
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
            else if (activity <= ThresholdMin)
                return 0f;
            else if (activity >= ThresholdHigh)
                return NormalLineWidth;
            return NormalLineWidth * activity / ThresholdHigh;
        }
        /// <summary>
        /// Get line color
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private Brush GetEdgeBrush(float activity)
        {
            if (float.IsNaN(activity) || float.IsInfinity(activity))
                return NGLineBrush;
            else if (activity <= ThresholdMin)
                return MinLineBrush;
            else if (activity >= ThresholdHigh)
                return MaxLineBrush;
            return ViewLineBrush;
        }
        private string GetPropertyString(float value)
        {
            if (isLogarithm)
                return value.ToString(FormatLog);
            return value.ToString(FormatNatural);
        }


        /// <summary>
        /// private class for ComponentSettingDialog
        /// </summary>
        private class AnimationBackGroundItem : GroupBox
        {
            private PropertyBrushItem m_normalBrushItem;
            private PropertyBrushItem m_viewBrushItem;

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
                // set Brushes
                List<string> list = BrushManager.GetBrushNameList();
                m_normalBrushItem = new PropertyBrushItem("Nomal Mode", control.BackGroundBrush, list);
                m_viewBrushItem = new PropertyBrushItem("View Mode", control.ViewModeBGBrush, list);

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
        }

        /// <summary>
        /// private class for ComponentSettingDialog
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

            public AnimationLineItem(AnimationControl control)
            {
                List<string> list = BrushManager.GetBrushNameList();
                this.m_lineNormalBrush = new PropertyBrushItem("Normal Line Brush", control.NormalLineBrush, list);
                this.m_lineViewBrush = new PropertyBrushItem("View Mode Line Brush", control.ViewLineBrush, list);
                this.m_lineHighBrush = new PropertyBrushItem("High Threshold Brush", control.MaxLineBrush, list);
                this.m_lineLowBrush = new PropertyBrushItem("Low Threshold Brush", control.MinLineBrush, list);
                this.m_lineNGBrush = new PropertyBrushItem("NG Brush", control.NGLineBrush, list);

                this.m_lineNormalWidth = new PropertyTextItem("Normal Line Width", control.NormalLineWidth.ToString());
                this.m_lineViewWidth = new PropertyTextItem("View Mode Line Width", control.MaxLineWidth.ToString());
                this.m_lineThresholdHigh = new PropertyTextItem("High Threshold", control.ThresholdHigh.ToString());
                this.m_lineThresholdLow = new PropertyTextItem("Low Threshold", control.ThresholdMin.ToString());

                this.m_lineCheckBox = new PropertyCheckBoxItem("logarithmic display", true);

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

            void cBoxFillColor_TextChanged(object sender, EventArgs e)
            {
                ComboBox colorBox = (ComboBox)sender;
                Brush brush = BrushManager.ParseStringToBrush(colorBox.Text);
            }

            void cBoxLineColor_TextChanged(object sender, EventArgs e)
            {
                ComboBox colorBox = (ComboBox)sender;
                Brush brush = BrushManager.ParseStringToBrush(colorBox.Text);
            }
        }
    }
}
