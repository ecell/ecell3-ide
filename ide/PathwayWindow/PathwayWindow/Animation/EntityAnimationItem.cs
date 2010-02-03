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
using Ecell.IDE.Plugins.PathwayWindow.Components;
using System.Drawing.Drawing2D;
using Ecell.IDE.Plugins.PathwayWindow.Graphics;
using System.Xml;

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
        /// <param name="doc"></param>
        /// <returns></returns>
        public override System.Xml.XmlElement GetAnimationStatus(System.Xml.XmlDocument doc)
        {
            XmlElement status = doc.CreateElement("EntityAnimationItem");
            status.SetAttribute("AutoThreshold", _autoThreshold.ToString());
            status.SetAttribute("ThresholdHigh", _thresholdHigh.ToString());
            status.SetAttribute("ThresholdLow", _thresholdLow.ToString());
            status.SetAttribute("HighBrush", BrushManager.ParseBrushToString(_highEdgeBrush));
            status.SetAttribute("LowBrush", BrushManager.ParseBrushToString(_lowEdgeBrush));
            status.SetAttribute("NGBrush", BrushManager.ParseBrushToString(_ngEdgeBrush));
            return status;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public override void SetAnimationStatus(System.Xml.XmlElement status)
        {
            _autoThreshold = bool.Parse(status.GetAttribute("AutoThreshold"));
            _thresholdHigh = float.Parse(status.GetAttribute("ThresholdHigh"));
            _thresholdLow = float.Parse(status.GetAttribute("ThresholdLow"));
            _highEdgeBrush = BrushManager.ParseStringToBrush(status.GetAttribute("HighBrush"));
            _lowEdgeBrush = BrushManager.ParseStringToBrush(status.GetAttribute("LowBrush"));
            _ngEdgeBrush = BrushManager.ParseStringToBrush(status.GetAttribute("NGBrush"));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetAnimation()
        {            
            base.SetAnimation();
            ProjectStatus status = _control.Control.ProjectStatus;
            bool onGoing = status == ProjectStatus.Running || status == ProjectStatus.Stepping || status == ProjectStatus.Suspended;

            if (_autoThreshold && !onGoing)
                _thresholdHigh = 0;

            foreach (PPathwayVariable variable in _variables)
            {
                if (!variable.Visible && variable.Aliases.Count == 0)
                    continue;
                if(!variable.ViewMode)
                    variable.ViewMode = true;
                // Line setting.

                // Reset size.
                if (onGoing)
                {
                    SetVariableAnimation(variable);
                }
                else
                {
                    ResetVariableAnimation(variable);
                    // Set threshold
                    if (!_autoThreshold)
                        continue;

                    double value = GetValue(variable.EcellObject.FullID + ":" + Constants.xpathMolarConc);
                    SetThreshold(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateAnimation()
        {
            foreach (PPathwayVariable variable in _variables)
            {
                // Variable setting.
                if (!variable.Visible)
                    continue;
                SetVariableAnimation(variable);
            }
        }

        private void SetVariableAnimation(PPathwayVariable variable)
        {
            double molarConc = GetValue(variable.EcellObject.FullID + ":" + Constants.xpathMolarConc);
            float size = GetEntitySize(variable.EcellObject, molarConc);
            float width =  size * variable.Figure.Width;
            float height =  size * variable.Figure.Height;
            PointF pos = variable.CenterPointF;
            
            // set variable.
            variable.Width = width;
            variable.Height = height;
            variable.CenterPointF = pos;
            variable.Brush = GetEntityBrush(molarConc, variable.Setting, variable.Path);

            // set alias
            foreach (PPathwayAlias alias in variable.Aliases)
            {
                pos = alias.CenterPointF;

                alias.Width = width;
                alias.Height = height;
                alias.CenterPointF = pos;
                alias.Brush = GetEntityBrush(molarConc, alias.Setting, alias.Path);
            }

            // set threshold
            if (!_autoThreshold)
                return;
            SetThreshold(molarConc);
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

                ResetVariableAnimation(variable);
            }

            base.ResetAnimation();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        private void ResetVariableAnimation(PPathwayVariable variable)
        {
            double molarConc = GetValue(variable.EcellObject.FullID + ":" + Constants.xpathMolarConc);
            float size = GetEntitySize(variable.EcellObject, molarConc);

            variable.ViewMode = false;
            float width = variable.Figure.Width;
            float height = variable.Figure.Height;
            PointF pos = variable.CenterPointF;
            Brush brush = GetEntityBrush(molarConc, variable.Setting, variable.Path);

            // set variable.
            variable.Width = width;
            variable.Height = height;
            variable.CenterPointF = pos;
            variable.RefreshView();

            // set alias
            foreach (PPathwayAlias alias in variable.Aliases)
            {
                pos = alias.CenterPointF;
                alias.Width = width;
                alias.Height = height;
                alias.CenterPointF = pos;
                alias.RefreshView();
            }
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
        /// <param name="setting"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Brush GetEntityBrush(double activity, ComponentSetting setting, GraphicsPath path)
        {
            if (double.IsNaN(activity) || double.IsInfinity(activity))
                return _ngEdgeBrush;
            else if (activity <= _thresholdLow)
                return _lowEdgeBrush;
            else if (activity >= _thresholdHigh)
                return _highEdgeBrush;
            return setting.CreateBrush(path);
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
            double dummy;
            if (!double.TryParse(text, out dummy))
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
            double dummy;
            if (!double.TryParse(text, out dummy))
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
