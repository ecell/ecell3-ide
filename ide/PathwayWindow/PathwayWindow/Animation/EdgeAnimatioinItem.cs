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

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public class EdgeAnimatioinItem: AnimationItemBase
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private bool _autoThreshold = false;

        /// <summary>
        /// High threshold of edge animation.
        /// </summary>
        private float _thresholdHigh = 100f;

        /// <summary>
        /// Low threshold of edge animation.
        /// </summary>
        private float _thresholdLow = 0f;

        /// <summary>
        /// Max edge width on edge animation.
        /// </summary>
        private float _maxEdgeWidth = 20f;
        
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
        public EdgeAnimatioinItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public EdgeAnimatioinItem(AnimationControl control)
            : base(control)
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EdgeAnimatioinItem));
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
            this.edgeBox.AccessibleDescription = null;
            this.edgeBox.AccessibleName = null;
            resources.ApplyResources(this.edgeBox, "edgeBox");
            this.edgeBox.BackgroundImage = null;
            this.edgeBox.Controls.Add(this.edgeLabel);
            this.edgeBox.Controls.Add(this.edgeHighBrush);
            this.edgeBox.Controls.Add(this.edgeLowBrush);
            this.edgeBox.Controls.Add(this.autoThresholdCheckBox);
            this.edgeBox.Controls.Add(this.thresholdHigh);
            this.edgeBox.Controls.Add(this.thresholdLow);
            this.edgeBox.Controls.Add(this.edgeNGBrush);
            this.edgeBox.Font = null;
            this.edgeBox.Name = "edgeBox";
            this.edgeBox.TabStop = false;
            // 
            // edgeLabel
            // 
            this.edgeLabel.AccessibleDescription = null;
            this.edgeLabel.AccessibleName = null;
            resources.ApplyResources(this.edgeLabel, "edgeLabel");
            this.edgeLabel.AutoEllipsis = true;
            this.edgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.edgeLabel.Font = null;
            this.edgeLabel.Name = "edgeLabel";
            // 
            // edgeHighBrush
            // 
            this.edgeHighBrush.AccessibleDescription = null;
            this.edgeHighBrush.AccessibleName = null;
            resources.ApplyResources(this.edgeHighBrush, "edgeHighBrush");
            this.edgeHighBrush.BackgroundImage = null;
            this.edgeHighBrush.Font = null;
            this.edgeHighBrush.Name = "edgeHighBrush";
            // 
            // edgeLowBrush
            // 
            this.edgeLowBrush.AccessibleDescription = null;
            this.edgeLowBrush.AccessibleName = null;
            resources.ApplyResources(this.edgeLowBrush, "edgeLowBrush");
            this.edgeLowBrush.BackgroundImage = null;
            this.edgeLowBrush.Font = null;
            this.edgeLowBrush.Name = "edgeLowBrush";
            // 
            // autoThresholdCheckBox
            // 
            this.autoThresholdCheckBox.AccessibleDescription = null;
            this.autoThresholdCheckBox.AccessibleName = null;
            resources.ApplyResources(this.autoThresholdCheckBox, "autoThresholdCheckBox");
            this.autoThresholdCheckBox.BackgroundImage = null;
            this.autoThresholdCheckBox.Checked = false;
            this.autoThresholdCheckBox.Font = null;
            this.autoThresholdCheckBox.Name = "autoThresholdCheckBox";
            // 
            // thresholdHigh
            // 
            this.thresholdHigh.AccessibleDescription = null;
            this.thresholdHigh.AccessibleName = null;
            resources.ApplyResources(this.thresholdHigh, "thresholdHigh");
            this.thresholdHigh.BackgroundImage = null;
            this.thresholdHigh.Font = null;
            this.thresholdHigh.Name = "thresholdHigh";
            this.thresholdHigh.Validating += new System.ComponentModel.CancelEventHandler(this.HighThresholdValidating);
            // 
            // thresholdLow
            // 
            this.thresholdLow.AccessibleDescription = null;
            this.thresholdLow.AccessibleName = null;
            resources.ApplyResources(this.thresholdLow, "thresholdLow");
            this.thresholdLow.BackgroundImage = null;
            this.thresholdLow.Font = null;
            this.thresholdLow.Name = "thresholdLow";
            this.thresholdLow.Validating += new System.ComponentModel.CancelEventHandler(this.LowThresholdValidating);
            // 
            // edgeNGBrush
            // 
            this.edgeNGBrush.AccessibleDescription = null;
            this.edgeNGBrush.AccessibleName = null;
            resources.ApplyResources(this.edgeNGBrush, "edgeNGBrush");
            this.edgeNGBrush.BackgroundImage = null;
            this.edgeNGBrush.Font = null;
            this.edgeNGBrush.Name = "edgeNGBrush";
            // 
            // EdgeAnimatioinItem
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.edgeBox);
            this.Font = null;
            this.Name = "EdgeAnimatioinItem";
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
        public override void SetProperty()
        {
            base.SetProperty();

            if (_autoThreshold)
                _thresholdHigh = 0;
            foreach (PPathwayProcess process in _processes)
            {
                process.ViewMode = true;
                process.Stepper.Visible = false;
                if (!process.Visible)
                    continue;
                // Line setting.
                foreach (PPathwayLine line in process.Relations)
                {
                    line.EdgeBrush = _viewEdgeBrush;
                }

                // Set threshold
                float activity = GetFloatValue(process.EcellObject.FullID + ":" + Constants.xpathMolarActivity);
                if (_autoThreshold)
                    SetThreshold(activity);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateProperty()
        {
            foreach (PPathwayProcess process in _processes)
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
                if (_autoThreshold)
                    SetThreshold(activity);
            }
            foreach (PPathwayVariable variable in _variables)
            {
                if (!variable.Visible)
                    continue;
                // Variable setting.
                float molerConc = GetFloatValue(variable.EcellObject.FullID + ":" + Constants.xpathMolarConc);
                float width = GetEdgeWidth(molerConc);
                Brush brush = GetEdgeBrush(molerConc);

                // Set Effector.
                foreach (PPathwayLine line in variable.Relations)
                {
                    if (line.Info.LineType == LineType.Dashed)
                        line.SetEdge(brush, width);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetProperty()
        {
            Brush editEdgeBrush = _control.EditEdgeBrush;
            float normalEdgeWidth = _control.EdgeWidth;
            foreach (PPathwayProcess process in _processes)
            {
                if (!process.Visible)
                    continue;
                // Line setting.
                process.ViewMode = false;
                process.Stepper.Visible = true;
                foreach (PPathwayLine line in process.Relations)
                {
                    line.SetEdge(editEdgeBrush, normalEdgeWidth);
                }
            }
            base.ResetProperty();
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
                UpdateProperty();

        }
        #endregion

        /// <summary>
        /// Get line width.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private float GetEdgeWidth(float activity)
        {
            if (float.IsNaN(activity))
                return 0f;
            else if (activity <= _thresholdLow || _thresholdHigh == 0f)
                return 0f;
            else if (activity >= _thresholdHigh)
                return _maxEdgeWidth;
            return _maxEdgeWidth * activity / _thresholdHigh;
        }

        /// <summary>
        /// Get line color
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private Brush GetEdgeBrush(float activity)
        {
            if (float.IsNaN(activity) || float.IsInfinity(activity))
                return _ngEdgeBrush;
            else if (activity <= _thresholdLow)
                return _lowEdgeBrush;
            else if (activity >= _thresholdHigh)
                return _highEdgeBrush;
            return _viewEdgeBrush;
        }

        /// <summary>
        /// Set threshold
        /// </summary>
        /// <param name="activity"></param>
        private void SetThreshold(float activity)
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
    }
}
