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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ecell.Exceptions;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class EditModeItems : UserControl
    {
        private GroupBox groupBox;
        private PropertyBrushItem bgBrushItem;
        private PropertyBrushItem edgeBrushItem;
        private PropertyTextItem edgeWidth;
        private PropertyCheckBoxItem highQualityCheckBox;
        private AnimationControl m_control;

        public EditModeItems()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public EditModeItems(AnimationControl control)
        {
            m_control = control;
            InitializeComponent();

            bgBrushItem.Brush = control.EditBGBrush;
            edgeWidth.Text = control.EdgeWidth.ToString();
            edgeBrushItem.Brush = control.EditEdgeBrush;
            highQualityCheckBox.Checked = control.Control.HighQuality;

            //MessageResources.DialogTextBackgroundBrush;
            //MessageResources.DialogTextEdgeWidth;
            //MessageResources.DialogTextEdgeBrush;
        }

        void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditModeItems));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.highQualityCheckBox = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyCheckBoxItem();
            this.bgBrushItem = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.edgeBrushItem = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.edgeWidth = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyTextItem();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.AccessibleDescription = null;
            this.groupBox.AccessibleName = null;
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.BackgroundImage = null;
            this.groupBox.Controls.Add(this.highQualityCheckBox);
            this.groupBox.Controls.Add(this.bgBrushItem);
            this.groupBox.Controls.Add(this.edgeBrushItem);
            this.groupBox.Controls.Add(this.edgeWidth);
            this.groupBox.Font = null;
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // highQualityCheckBox
            // 
            this.highQualityCheckBox.AccessibleDescription = null;
            this.highQualityCheckBox.AccessibleName = null;
            resources.ApplyResources(this.highQualityCheckBox, "highQualityCheckBox");
            this.highQualityCheckBox.BackgroundImage = null;
            this.highQualityCheckBox.Checked = false;
            this.highQualityCheckBox.Font = null;
            this.highQualityCheckBox.Name = "highQualityCheckBox";
            // 
            // bgBrushItem
            // 
            this.bgBrushItem.AccessibleDescription = null;
            this.bgBrushItem.AccessibleName = null;
            resources.ApplyResources(this.bgBrushItem, "bgBrushItem");
            this.bgBrushItem.BackgroundImage = null;
            this.bgBrushItem.Font = null;
            this.bgBrushItem.Name = "bgBrushItem";
            // 
            // edgeBrushItem
            // 
            this.edgeBrushItem.AccessibleDescription = null;
            this.edgeBrushItem.AccessibleName = null;
            resources.ApplyResources(this.edgeBrushItem, "edgeBrushItem");
            this.edgeBrushItem.BackgroundImage = null;
            this.edgeBrushItem.Font = null;
            this.edgeBrushItem.Name = "edgeBrushItem";
            // 
            // edgeWidth
            // 
            this.edgeWidth.AccessibleDescription = null;
            this.edgeWidth.AccessibleName = null;
            resources.ApplyResources(this.edgeWidth, "edgeWidth");
            this.edgeWidth.BackgroundImage = null;
            this.edgeWidth.Font = null;
            this.edgeWidth.Name = "edgeWidth";
            this.edgeWidth.Validating += new System.ComponentModel.CancelEventHandler(this.EdgeWidthValidating);
            // 
            // EditModeItems
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.groupBox);
            this.Font = null;
            this.Name = "EditModeItems";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void EdgeWidthValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = edgeWidth.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, edgeWidth.LabelText));
                edgeWidth.Text = Convert.ToString(m_control.EdgeWidth);
                e.Cancel = true;
                return;
            }
            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, edgeWidth.LabelText));
                edgeWidth.Text = Convert.ToString(m_control.EdgeWidth);
                e.Cancel = true;
                return;
            }
        }

        public void ApplyChanges()
        {
            m_control.EditBGBrush = this.bgBrushItem.Brush;
            m_control.EditEdgeBrush = this.edgeBrushItem.Brush;
            m_control.EdgeWidth = float.Parse(this.edgeWidth.Text);
            m_control.Control.HighQuality = this.highQualityCheckBox.Checked;
        }

        public void ItemClosing()
        {
            string text = edgeWidth.Text;

            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy <= 0 || dummy > 100)
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidValue, edgeWidth.LabelText));
            }
        }
    }
}
