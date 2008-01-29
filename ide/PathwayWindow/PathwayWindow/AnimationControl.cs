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

namespace EcellLib.PathwayWindow
{
    public class AnimationControl
    {
        #region constant fields
        bool isLogarithm = false;
        protected float MaxActivity = 100f;
        protected float MinActivity = 0f;
        protected float MaxLineWidth = 20f;
        protected Brush NGColor = Brushes.Red;
        protected Brush DefLineColor = Brushes.LightGreen;
        protected Brush MinLineColor = Brushes.Gray;
        protected Brush MaxLineColor = Brushes.Yellow;
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
            page.Controls.Add(new AnimationBrushItem("BackGroundBrush", Brushes.White, Brushes.White));
            page.Controls.Add(new AnimationBrushItem("LineBrush", Brushes.Black, Brushes.LightGreen));
            page.ResumeLayout();
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
            else if (activity <= MinActivity)
                return 0f;
            else if (activity >= MaxActivity)
                return MaxLineWidth;
            return MaxLineWidth * activity / MaxActivity;
        }
        /// <summary>
        /// Get line color
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private Brush GetEdgeBrush(float activity)
        {
            if (float.IsNaN(activity) || float.IsInfinity(activity))
                return NGColor;
            else if (activity <= MinActivity)
                return MinLineColor;
            else if (activity >= MaxActivity)
                return MaxLineColor;
            return DefLineColor;
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
        private class AnimationBrushItem : GroupBox
        {
            private Label labelNomalMode;
            private Label labelViewMode;
            private ComboBox cBoxNomalMode;
            private ComboBox cBoxViewMode;
            private Brush m_nomalBrush = null;
            private Brush m_viewBrush = null;

            public Brush NomalBrush
            {
                get { return m_nomalBrush; }
                set { m_nomalBrush = value; }
            }

            public Brush ViewBrush
            {
                get { return m_viewBrush; }
                set { m_viewBrush = value; }
            }

            public AnimationBrushItem(string title, Brush nomalBrush, Brush viewBrush)
            {
                // set Brushes
                m_nomalBrush = nomalBrush;
                m_viewBrush = viewBrush;

                this.labelNomalMode = new Label();
                this.labelViewMode = new Label();
                this.cBoxNomalMode = new ComboBox();
                this.cBoxViewMode = new ComboBox();
                this.SuspendLayout();
                // 
                // groupBox
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(this.labelNomalMode);
                this.Controls.Add(this.labelViewMode);
                this.Controls.Add(this.cBoxNomalMode);
                this.Controls.Add(this.cBoxViewMode);
                this.Text = title;
                this.TabStop = false;
                // 
                // labelNomalMode
                // 
                this.labelNomalMode.AutoSize = true;
                this.labelNomalMode.Location = new System.Drawing.Point(14, 16);
                this.labelNomalMode.Size = new System.Drawing.Size(35, 12);
                this.labelNomalMode.Text = "Nomal Mode";
                // 
                // labelViewMode
                // 
                this.labelViewMode.AutoSize = true;
                this.labelViewMode.Location = new System.Drawing.Point(14, 42);
                this.labelViewMode.Size = new System.Drawing.Size(53, 12);
                this.labelViewMode.Text = "View Mode";
                // 
                // cBoxNomalMode
                // 
                this.cBoxNomalMode.FormattingEnabled = true;
                this.cBoxNomalMode.Location = new System.Drawing.Point(100, 13);
                this.cBoxNomalMode.Name = "cBoxNomalBrush";
                this.cBoxNomalMode.Size = new System.Drawing.Size(128, 20);
                this.cBoxNomalMode.TabIndex = 1;
                this.cBoxNomalMode.Text = BrushManager.ParseBrushToString(m_nomalBrush);
                this.cBoxNomalMode.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
                this.cBoxNomalMode.TextChanged += new EventHandler(cBoxNomalBrush_TextChanged);
                // 
                // cBoxViewMode
                // 
                this.cBoxViewMode.FormattingEnabled = true;
                this.cBoxViewMode.Location = new System.Drawing.Point(100, 39);
                this.cBoxViewMode.Name = "cBoxViewBrush";
                this.cBoxViewMode.Size = new System.Drawing.Size(128, 20);
                this.cBoxViewMode.TabIndex = 2;
                this.cBoxViewMode.Text = BrushManager.ParseBrushToString(m_viewBrush);
                this.cBoxViewMode.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
                this.cBoxViewMode.TextChanged += new EventHandler(cBoxViewBrush_TextChanged);

                this.ResumeLayout(false);
                this.PerformLayout();

                this.Height = 125;
            }

            void cBoxNomalBrush_TextChanged(object sender, EventArgs e)
            {
                ComboBox cBox = (ComboBox)sender;
                Brush brush = BrushManager.ParseStringToBrush(cBox.Text);
                m_nomalBrush = brush;
            }

            void cBoxViewBrush_TextChanged(object sender, EventArgs e)
            {
                ComboBox cBox = (ComboBox)sender;
                Brush brush = BrushManager.ParseStringToBrush(cBox.Text);
                m_viewBrush = brush;
            }
        }

        /// <summary>
        /// private class for ComponentSettingDialog
        /// </summary>
        private class AnimationLineItem : GroupBox
        {
            private Label labelLineWidth;
            private Label labelLineThresholdLow;
            private Label labelLineThresholdHigh;
            private Label labelFillColor;
            private ComboBox cBoxFigure;
            private ComboBox cBoxLineColor;
            private ComboBox cBoxFillColor;

            public AnimationLineItem(AnimationControl animCon)
            {
                this.labelLineWidth = new Label();
                this.labelLineThresholdLow = new Label();
                this.labelLineThresholdHigh = new Label();
                this.labelFillColor = new Label();
                this.cBoxFigure = new ComboBox();
                this.cBoxLineColor = new ComboBox();
                this.cBoxFillColor = new ComboBox();
                this.SuspendLayout();
                // 
                // groupBox
                // 
                this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
                this.AutoSize = true;
                this.Controls.Add(this.labelLineWidth);
                this.Controls.Add(this.labelLineThresholdLow);
                this.Controls.Add(this.labelLineThresholdHigh);
                this.Controls.Add(this.labelFillColor);
                this.Controls.Add(this.cBoxFigure);
                this.Controls.Add(this.cBoxFillColor);
                this.Controls.Add(this.cBoxLineColor);
                this.Text = "LineSettings";
                this.TabStop = false;
                // 
                // labelName
                // 
                this.labelLineWidth.AutoSize = true;
                this.labelLineWidth.Location = new System.Drawing.Point(14, 16);
                this.labelLineWidth.Text = "";
                this.labelLineWidth.Size = new System.Drawing.Size(35, 12);
                // 
                // labelFigure
                // 
                this.labelLineThresholdLow.AutoSize = true;
                this.labelLineThresholdLow.Location = new System.Drawing.Point(14, 42);
                this.labelLineThresholdLow.Name = "labelFigure";
                this.labelLineThresholdLow.Size = new System.Drawing.Size(53, 12);
                this.labelLineThresholdLow.Text = "Figure";
                // 
                // labelLineColor
                // 
                this.labelLineThresholdHigh.AutoSize = true;
                this.labelLineThresholdHigh.Location = new System.Drawing.Point(14, 68);
                this.labelLineThresholdHigh.Name = "labelLineColor";
                this.labelLineThresholdHigh.Size = new System.Drawing.Size(53, 12);
                this.labelLineThresholdHigh.Text = "LineColor";
                // 
                // labelFillColor
                // 
                this.labelFillColor.AutoSize = true;
                this.labelFillColor.Location = new System.Drawing.Point(14, 94);
                this.labelFillColor.Name = "labelFillColor";
                this.labelFillColor.Size = new System.Drawing.Size(48, 12);
                this.labelFillColor.Text = "FillColor";
                // 
                // cBoxFigure
                // 
                this.cBoxFigure.FormattingEnabled = true;
                this.cBoxFigure.Location = new System.Drawing.Point(100, 39);
                this.cBoxFigure.Name = "cBoxLineColor";
                this.cBoxFigure.Size = new System.Drawing.Size(128, 20);
                this.cBoxFigure.TabIndex = 0;
                this.cBoxFigure.Text = "";
                // 
                // cBoxLineColor
                // 
                this.cBoxLineColor.FormattingEnabled = true;
                this.cBoxLineColor.Location = new System.Drawing.Point(100, 65);
                this.cBoxLineColor.Name = "cBoxLineColor";
                this.cBoxLineColor.Size = new System.Drawing.Size(128, 20);
                this.cBoxLineColor.TabIndex = 1;
                this.cBoxLineColor.Text = BrushManager.ParseBrushToString(null);
                this.cBoxLineColor.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
                this.cBoxLineColor.TextChanged += new EventHandler(cBoxLineColor_TextChanged);
                // 
                // cBoxFillColor
                // 
                this.cBoxFillColor.FormattingEnabled = true;
                this.cBoxFillColor.Location = new System.Drawing.Point(100, 91);
                this.cBoxFillColor.Name = "cBoxFillColor";
                this.cBoxFillColor.Size = new System.Drawing.Size(128, 20);
                this.cBoxFillColor.TabIndex = 2;
                this.cBoxFillColor.Text = BrushManager.ParseBrushToString(null);
                this.cBoxFillColor.Items.AddRange(BrushManager.GetBrushNameList().ToArray());
                this.cBoxFillColor.TextChanged += new EventHandler(cBoxFillColor_TextChanged);

                this.ResumeLayout(false);
                this.PerformLayout();

                this.Height = 125;
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
