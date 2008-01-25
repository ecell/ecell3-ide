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
        protected Brush MaxLineColor = Brushes.Green;
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
            TimerStart();
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
                // Set gradient brush
                PathGradientBrush pthGrBrush = new PathGradientBrush(process.Path);
                pthGrBrush.CenterColor = Color.White;
                pthGrBrush.SurroundColors = new Color[] {Color.LightGreen};
                process.FillBrush = pthGrBrush;
                // Line setting.
                string propName = "Process:" + process.EcellObject.key + ":MolarActivity";
                float activity = GetFloatValue(propName);
                process.EdgeBrush = GetEdgeBrush(activity);
                process.SetLineWidth(GetEdgeWidth(activity));
                process.MoveToFront();
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                // Set gradient brush
                PathGradientBrush pthGrBrush = new PathGradientBrush(variable.Path);
                pthGrBrush.CenterColor = Color.White;
                pthGrBrush.SurroundColors = new Color[] { Color.LightBlue };
                variable.FillBrush = pthGrBrush;
                // Variable setting.
                string propName = "Variable:" + variable.EcellObject.key + ":MolarConc";
                float molerConc = GetFloatValue(propName);
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
                process.FillBrush = process.Setting.FillBrush;
                // Line setting.
                process.EdgeBrush = Brushes.Black;
                process.MoveToFront();
            }
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if (!variable.Visible)
                    continue;
                variable.FillBrush = variable.Setting.FillBrush;
            }
            m_canvas = null;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        private float GetFloatValue(string propName)
        {
            EcellValue value = m_dManager.GetEntityProperty(propName);
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
    }
}
