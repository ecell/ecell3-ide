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
using Ecell.IDE.Plugins.PathwayWindow.Dialog;
using System.Drawing;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{

    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class AnimationItems : UserControl
    {
        private GroupBox groupBox;
        private PropertyBrushItem edgeHighBrush;
        private PropertyBrushItem edgeLowBrush;
        private PropertyBrushItem edgeNGBrush;
        private PropertyTextItem thresholdHigh;
        private PropertyTextItem thresholdLow;
        private PropertyCheckBoxItem autoThresholdCheckBox;
        private PropertyBrushItem propertyBrush;
        private PropertyCheckBoxItem logarithmicCheckBox;
        private PropertyCheckBoxItem aviOutputCheckBox;
        private PropertyFileItem aviFileName;

        private AnimationControl animCon;

        public AnimationItems()
        {
            InitializeComponent();
        }

        public AnimationItems(AnimationControl control)
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
            logarithmicCheckBox.Checked = control.IsLogarithmic;
            aviOutputCheckBox.Checked = control.DoesRecordMovie;
            aviFileName.FileName = control.AviFile;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationItems));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.aviOutputCheckBox = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyCheckBoxItem();
            this.edgeHighBrush = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyBrushItem();
            this.edgeLowBrush = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyBrushItem();
            this.autoThresholdCheckBox = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyCheckBoxItem();
            this.thresholdHigh = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyTextItem();
            this.thresholdLow = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyTextItem();
            this.edgeNGBrush = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyBrushItem();
            this.propertyBrush = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyBrushItem();
            this.logarithmicCheckBox = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyCheckBoxItem();
            this.aviFileName = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyFileItem();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Controls.Add(this.aviOutputCheckBox);
            this.groupBox.Controls.Add(this.edgeHighBrush);
            this.groupBox.Controls.Add(this.edgeLowBrush);
            this.groupBox.Controls.Add(this.autoThresholdCheckBox);
            this.groupBox.Controls.Add(this.thresholdHigh);
            this.groupBox.Controls.Add(this.thresholdLow);
            this.groupBox.Controls.Add(this.edgeNGBrush);
            this.groupBox.Controls.Add(this.propertyBrush);
            this.groupBox.Controls.Add(this.logarithmicCheckBox);
            this.groupBox.Controls.Add(this.aviFileName);
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // aviOutputCheckBox
            // 
            resources.ApplyResources(this.aviOutputCheckBox, "aviOutputCheckBox");
            this.aviOutputCheckBox.Checked = false;
            this.aviOutputCheckBox.Name = "aviOutputCheckBox";
            this.aviOutputCheckBox.CheckedChanged += new System.EventHandler(this.aviOutputCheckBox_CheckedChanged);
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
            // propertyBrush
            // 
            resources.ApplyResources(this.propertyBrush, "propertyBrush");
            this.propertyBrush.Name = "propertyBrush";
            // 
            // logarithmicCheckBox
            // 
            resources.ApplyResources(this.logarithmicCheckBox, "logarithmicCheckBox");
            this.logarithmicCheckBox.Checked = false;
            this.logarithmicCheckBox.Name = "logarithmicCheckBox";
            // 
            // aviFileName
            // 
            resources.ApplyResources(this.aviFileName, "aviFileName");
            this.aviFileName.FileName = "";
            this.aviFileName.Filter = null;
            this.aviFileName.FilterIndex = 0;
            this.aviFileName.Name = "aviFileName";
            // 
            // AnimationItems
            // 
            resources.ApplyResources(this, "$this");
            this.AutoSize = true;
            this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            this.Controls.Add(this.groupBox);
            this.Name = "AnimationItems";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();


        }

        void aviOutputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            aviFileName.Enabled = aviOutputCheckBox.Checked;
        }

        void HighThresholdValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = thresholdHigh.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, this.thresholdHigh.LabelText));
                thresholdHigh.Text = Convert.ToString(animCon.ThresholdHigh);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy) || Convert.ToDouble(thresholdLow.Text) > dummy)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                thresholdHigh.Text = Convert.ToString(animCon.ThresholdHigh);
                e.Cancel = true;
                return;
            }
        }

        void LowThresholdValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = thresholdLow.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, this.thresholdLow.LabelText));
                thresholdLow.Text = Convert.ToString(animCon.ThresholdLow);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy) || Convert.ToDouble(thresholdHigh.Text) < dummy)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
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
            animCon.IsLogarithmic = logarithmicCheckBox.Checked;
            animCon.DoesRecordMovie = aviOutputCheckBox.Checked;
            animCon.AviFile = aviFileName.FileName;
        }
    }
}
