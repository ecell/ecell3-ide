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
using System.Windows.Forms;

using Ecell.Exceptions;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.IDE.Plugins.PathwayWindow.Graphics;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{

    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class AnimationSettings : UserControl
    {
        private GroupBox edgeBox;
        private BrushComboBox edgeHighBrush;
        private BrushComboBox edgeLowBrush;
        private BrushComboBox edgeNGBrush;
        private PropertyTextItem thresholdHigh;
        private PropertyTextItem thresholdLow;
        private PropertyCheckBoxItem autoThresholdCheckBox;
        private BrushComboBox propertyBrush;
        private GroupBox variableBox;
        private Label edgeLabel;
        private Label variableLabel;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;

        private AnimationControl animCon;

        public AnimationSettings()
        {
            InitializeComponent();
        }

        public AnimationSettings(AnimationControl control)
        {
            animCon = control;
            InitializeComponent();

            thresholdHigh.Text = control.ThresholdHigh.ToString();
            thresholdLow.Text = control.ThresholdLow.ToString();
            edgeHighBrush.Brush = control.HighEdgeBrush;
            edgeLowBrush.Brush = control.LowEdgeBrush;
            edgeNGBrush.Brush = control.NgEdgeBrush;
            autoThresholdCheckBox.Checked = control.AutoThreshold;
            propertyBrush.Brush = control.PropertyBrush;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationSettings));
            this.edgeBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.edgeLabel = new System.Windows.Forms.Label();
            this.edgeHighBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.edgeLowBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.autoThresholdCheckBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyCheckBoxItem();
            this.thresholdHigh = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyTextItem();
            this.thresholdLow = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyTextItem();
            this.edgeNGBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.variableBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.variableLabel = new System.Windows.Forms.Label();
            this.propertyBrush = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.BrushComboBox();
            this.edgeBox.SuspendLayout();
            this.variableBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // edgeBox
            // 
            resources.ApplyResources(this.edgeBox, "edgeBox");
            this.edgeBox.Controls.Add(this.label4);
            this.edgeBox.Controls.Add(this.label3);
            this.edgeBox.Controls.Add(this.label2);
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // variableBox
            // 
            resources.ApplyResources(this.variableBox, "variableBox");
            this.variableBox.Controls.Add(this.label5);
            this.variableBox.Controls.Add(this.variableLabel);
            this.variableBox.Controls.Add(this.propertyBrush);
            this.variableBox.Name = "variableBox";
            this.variableBox.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // variableLabel
            // 
            resources.ApplyResources(this.variableLabel, "variableLabel");
            this.variableLabel.Name = "variableLabel";
            // 
            // propertyBrush
            // 
            resources.ApplyResources(this.propertyBrush, "propertyBrush");
            this.propertyBrush.Name = "propertyBrush";
            // 
            // AnimationSettings
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.variableBox);
            this.Controls.Add(this.edgeBox);
            this.Name = "AnimationSettings";
            this.edgeBox.ResumeLayout(false);
            this.edgeBox.PerformLayout();
            this.variableBox.ResumeLayout(false);
            this.variableBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void ItemClosing()
        {
            string hightext = thresholdHigh.Text;
            string lowtext = thresholdLow.Text;
            double high, low;
            if (!double.TryParse(hightext, out high))
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidValue, thresholdHigh.LabelText));
            }
            if (!double.TryParse(lowtext, out low))
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidValue, thresholdLow.LabelText));
            }
            if (high < low)
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidValue, 
                    thresholdHigh.LabelText + "," + thresholdLow.LabelText));
            }
        }

        void HighThresholdValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = thresholdHigh.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, this.thresholdHigh.LabelText));
                thresholdHigh.Text = Convert.ToString(animCon.ThresholdHigh);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, thresholdHigh.LabelText));
                thresholdHigh.Text = Convert.ToString(animCon.ThresholdHigh);
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
                thresholdLow.Text = Convert.ToString(animCon.ThresholdLow);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, thresholdLow.LabelText));
                thresholdLow.Text = Convert.ToString(animCon.ThresholdLow);
                e.Cancel = true;
                return;
            }
        }

        public void ApplyChanges()
        {
            animCon.AutoThreshold = autoThresholdCheckBox.Checked;
            animCon.ThresholdHigh = float.Parse(thresholdHigh.Text);
            animCon.ThresholdLow = float.Parse(thresholdLow.Text);
            animCon.HighEdgeBrush = edgeHighBrush.Brush;
            animCon.LowEdgeBrush = edgeLowBrush.Brush;
            animCon.NgEdgeBrush = edgeNGBrush.Brush;
            animCon.PropertyBrush = propertyBrush.Brush;
        }
    }
}
