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
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using System.Drawing;

using Ecell.Objects;
using Ecell.Reporting;
using Ecell.IDE.Plugins.PathwayWindow.Exceptions;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityAnimationItem: AnimationItemBase
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private bool _autoThreshold = false;

        /// <summary>
        /// High threshold of edge animation.
        /// </summary>
        private double _thresholdHigh = 100f;

        /// <summary>
        /// Low threshold of edge animation.
        /// </summary>
        private double _thresholdLow = 0f;

        /// <summary>
        /// Max edge width on edge animation.
        /// </summary>
        private float _maxEdgeWidth = 20f;
        
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

        private System.Windows.Forms.GroupBox edgeBox;
        private System.Windows.Forms.Label edgeLabel;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem edgeHighBrush;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem edgeLowBrush;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyCheckBoxItem autoThresholdCheckBox;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyTextItem thresholdHigh;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyTextItem thresholdLow;
        private Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem edgeNGBrush;

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public EntityAnimationItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public EntityAnimationItem(AnimationControl control)
            : base(control)
        {
            InitializeComponent();

            this._highEdgeBrush = _control.HighEdgeBrush;
            this._lowEdgeBrush = _control.LowEdgeBrush;
            this._ngEdgeBrush = _control.NgEdgeBrush;
            this._autoThreshold = _control.AutoThreshold;
            this._maxEdgeWidth = _control.MaxEdgeWidth;
            this._thresholdHigh = _control.ThresholdHigh;
            this._thresholdLow = _control.ThresholdLow;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityAnimationItem));
            this.edgeBox = new System.Windows.Forms.GroupBox();
            this.edgeLabel = new System.Windows.Forms.Label();
            this.edgeHighBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.edgeLowBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.autoThresholdCheckBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyCheckBoxItem();
            this.thresholdHigh = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyTextItem();
            this.thresholdLow = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyTextItem();
            this.edgeNGBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.edgeBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // edgeBox
            // 
            resources.ApplyResources(this.edgeBox, "edgeBox");
            this.edgeBox.Controls.Add(this.edgeLabel);
            this.edgeBox.Controls.Add(this.edgeHighBrush);
            this.edgeBox.Controls.Add(this.edgeLowBrush);
            this.edgeBox.Controls.Add(this.autoThresholdCheckBox);
            this.edgeBox.Controls.Add(this.thresholdHigh);
            this.edgeBox.Controls.Add(this.thresholdLow);
            this.edgeBox.Controls.Add(this.edgeNGBrush);
            this.edgeBox.Name = "edgeBox";
            this.edgeBox.TabStop = false;
            // 
            // edgeLabel
            // 
            this.edgeLabel.AutoEllipsis = true;
            this.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.edgeLabel, "edgeLabel");
            this.edgeLabel.Name = "edgeLabel";
            // 
            // edgeHighBrush
            // 
            resources.ApplyResources(this.edgeHighBrush, "edgeHighBrush");
            this.edgeHighBrush.Name = "edgeHighBrush";
            // 
            // edgeLowBrush
            // 
            resources.ApplyResources(this.edgeLowBrush, "edgeLowBrush");
            this.edgeLowBrush.Name = "edgeLowBrush";
            // 
            // autoThresholdCheckBox
            // 
            resources.ApplyResources(this.autoThresholdCheckBox, "autoThresholdCheckBox");
            this.autoThresholdCheckBox.Checked = false;
            this.autoThresholdCheckBox.Name = "autoThresholdCheckBox";
            this.autoThresholdCheckBox.CheckedChanged += new System.EventHandler(this.autoThresholdCheckBox_CheckedChanged);
            // 
            // thresholdHigh
            // 
            resources.ApplyResources(this.thresholdHigh, "thresholdHigh");
            this.thresholdHigh.Name = "thresholdHigh";
            this.thresholdHigh.Validating += new System.ComponentModel.CancelEventHandler(this.HighThresholdValidating);
            // 
            // thresholdLow
            // 
            resources.ApplyResources(this.thresholdLow, "thresholdLow");
            this.thresholdLow.Name = "thresholdLow";
            this.thresholdLow.Validating += new System.ComponentModel.CancelEventHandler(this.LowThresholdValidating);
            // 
            // edgeNGBrush
            // 
            resources.ApplyResources(this.edgeNGBrush, "edgeNGBrush");
            this.edgeNGBrush.Name = "edgeNGBrush";
            // 
            // EntityAnimationItem
            // 
            this.Controls.Add(this.edgeBox);
            this.Name = "EntityAnimationItem";
            resources.ApplyResources(this, "$this");
            this.edgeBox.ResumeLayout(false);
            this.edgeBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region IAnimationItem メンバ
        /// <summary>
        /// 
        /// </summary>
        public override void SetAnimation()
        {            
            base.SetAnimation();

            if (_autoThreshold)
                _thresholdHigh = 0;
            foreach (PPathwayVariable variable in _variables)
            {
                if (!variable.Visible)
                    continue;
                if(!variable.ViewMode)
                    variable.ViewMode = true;
                // Line setting.

                // Reset size.
                PointF pos = variable.CenterPointF;
                if (_control.Control.ProjectStatus == ProjectStatus.Suspended)
                {
                    double activity = GetValue(variable.EcellObject.FullID + ":" + Constants.xpathMolarConc);
                    float size = GetEntitySize(variable.EcellObject, activity);
                    variable.Width = size * variable.Figure.Width;
                    variable.Height = size * variable.Figure.Height;
                    variable.CenterPointF = pos;
                    variable.Brush = GetEntityBrush(activity, variable);
                }
                else
                {
                    variable.Width = variable.Figure.Width;
                    variable.Height = variable.Figure.Height;
                    variable.CenterPointF = pos;
                    variable.Brush = variable.Setting.CreateBrush(variable.Path);
                }

                // Set threshold
                if (!_autoThreshold)
                    continue;
                double molarConc = GetValue(variable.EcellObject.FullID + ":" + Constants.xpathMolarConc);
                SetThreshold(molarConc);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAnimation()
        {
            foreach (PPathwayVariable variable in _variables)
            {
                if (!variable.Visible)
                    continue;
                // Variable setting.
                double molarConc = GetValue(variable.EcellObject.FullID + ":" + Constants.xpathMolarConc);
                float size = GetEntitySize(variable.EcellObject, molarConc);
                PointF pos = variable.CenterPointF;
                variable.Width = size * variable.Figure.Width;
                variable.Height = size * variable.Figure.Height;
                variable.CenterPointF = pos;
                variable.Brush = GetEntityBrush(molarConc, variable);

                if (!_autoThreshold)
                    continue;
                SetThreshold(molarConc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StopAnimation()
        {
            SetAnimation();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetAnimation()
        {
            foreach (PPathwayVariable variable in _variables)
            {
                if (!variable.Visible)
                    continue;
                variable.ViewMode = false;
                PointF pos = variable.CenterPointF;
                variable.Width = variable.Figure.Width;
                variable.Height = variable.Figure.Height;
                variable.CenterPointF = pos;
                variable.RefreshView();
            }

            base.ResetAnimation();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetViewItem()
        {
            autoThresholdCheckBox.Checked = _autoThreshold;
            thresholdHigh.Text = _thresholdHigh.ToString();
            thresholdLow.Text = _thresholdLow.ToString();
            edgeHighBrush.Brush = _highEdgeBrush;
            edgeLowBrush.Brush = _lowEdgeBrush;
            edgeNGBrush.Brush = _ngEdgeBrush;

        }

        /// <summary>
        /// 
        /// </summary>
        public override void ApplyChange()
        {
            _autoThreshold = autoThresholdCheckBox.Checked;
            _thresholdHigh = float.Parse(thresholdHigh.Text);
            _thresholdLow = float.Parse(thresholdLow.Text);
            _highEdgeBrush = edgeHighBrush.Brush;
            _lowEdgeBrush = edgeLowBrush.Brush;
            _ngEdgeBrush = edgeNGBrush.Brush;

            if (_control.DoesAnimationOnGoing)
                UpdateAnimation();

        }

        /// <summary>
        /// 
        /// </summary>
        public override void CheckParameters()
        {
            if (this.autoThresholdCheckBox.Checked)
                return;

            float high = float.Parse(thresholdHigh.Text);
            float low = float.Parse(thresholdLow.Text);

            if (high <= low)
            {
                thresholdHigh.Text = Convert.ToString(_thresholdHigh);
                thresholdLow.Text = Convert.ToString(_thresholdLow);
                throw new PathwayException(this.Text + ":" + MessageResources.ErrMaxThreshold);
            }
        }
        #endregion

        /// <summary>
        /// Get line width.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private float GetEntitySize(EcellObject obj, double activity)
        {
            if (double.IsNaN(activity))
            {
                if (_canvas.Control.Session != null)
                {
                    _canvas.Control.Session.Add(new ObjectReport(MessageType.Warning,
                        MessageResources.WarnExtValue, Constants.groupDynamic, obj));
                }
                return 0.1f;
            }
            else if (activity <= _thresholdLow || _thresholdLow == _thresholdHigh)
            {
                if (_canvas.Control.Session != null)
                {
                    _canvas.Control.Session.Add(new ObjectReport(MessageType.Warning,
                        MessageResources.WarnExtValue, Constants.groupDynamic, obj));
                }
                return 0.1f;
            }
            else if (activity >= _thresholdHigh)
            {
                if (_canvas.Control.Session != null)
                {
                    _canvas.Control.Session.Add(new ObjectReport(MessageType.Warning,
                        MessageResources.WarnExtValue, Constants.groupDynamic, obj));
                }
                return 2.1f;
            }
            return (float)(2d * (activity - _thresholdLow)/ (_thresholdHigh - _thresholdLow) + 0.1d);
        }

        /// <summary>
        /// Get line color
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        private Brush GetEntityBrush(double activity, PPathwayVariable variable)
        {
            if (double.IsNaN(activity) || double.IsInfinity(activity))
                return _ngEdgeBrush;
            else if (activity <= _thresholdLow)
                return _lowEdgeBrush;
            else if (activity >= _thresholdHigh)
                return _highEdgeBrush;
            return variable.Setting.CreateBrush(variable.Path);
        }

        /// <summary>
        /// Set threshold
        /// </summary>
        /// <param name="activity"></param>
        private void SetThreshold(double activity)
        {
            if (activity > _thresholdHigh)
                _thresholdHigh = activity;
            if (activity < _thresholdLow)
                _thresholdLow = activity;
        }


        void HighThresholdValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = thresholdHigh.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, this.thresholdHigh.LabelText));
                thresholdHigh.Text = Convert.ToString(_control.ThresholdHigh);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, thresholdHigh.LabelText));
                thresholdHigh.Text = Convert.ToString(_control.ThresholdHigh);
                e.Cancel = true;
                return;
            }
        }

        void LowThresholdValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = thresholdLow.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, this.thresholdLow.LabelText));
                thresholdLow.Text = Convert.ToString(_control.ThresholdLow);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, thresholdLow.LabelText));
                thresholdLow.Text = Convert.ToString(_control.ThresholdLow);
                e.Cancel = true;
                return;
            }
        }

        private void autoThresholdCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoThresholdCheckBox.Checked)
            {
                thresholdHigh.Enabled = false;
                thresholdLow.Enabled = false;
            }
            else
            {
                thresholdHigh.Enabled = true;
                thresholdLow.Enabled = true;
            }
        }
    }
}
