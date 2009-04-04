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

            //MessageResources.DialogTextBackgroundBrush;
            //MessageResources.DialogTextEdgeWidth;
            //MessageResources.DialogTextEdgeBrush;
        }

        void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditModeItems));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.bgBrushItem = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.edgeBrushItem = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyBrushItem();
            this.edgeWidth = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PropertyTextItem();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Controls.Add(this.bgBrushItem);
            this.groupBox.Controls.Add(this.edgeBrushItem);
            this.groupBox.Controls.Add(this.edgeWidth);
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // bgBrushItem
            // 
            resources.ApplyResources(this.bgBrushItem, "bgBrushItem");
            this.bgBrushItem.Name = "bgBrushItem";
            // 
            // edgeBrushItem
            // 
            resources.ApplyResources(this.edgeBrushItem, "edgeBrushItem");
            this.edgeBrushItem.Name = "edgeBrushItem";
            // 
            // edgeWidth
            // 
            resources.ApplyResources(this.edgeWidth, "edgeWidth");
            this.edgeWidth.Name = "edgeWidth";
            this.edgeWidth.Validating += new System.ComponentModel.CancelEventHandler(this.EdgeWidthValidating);
            // 
            // EditModeItems
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox);
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
        }

        public void ItemClosing()
        {
            string text = edgeWidth.Text;

            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy < 0 || dummy > 100)
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidValue, edgeWidth.LabelText));
            }
        }
    }
}
