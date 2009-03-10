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
using Ecell.IDE.Plugins.PathwayWindow.Dialog;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class ViewModeItems : UserControl
    {
        private GroupBox groupBox;
        private PropertyBrushItem bgBrush;
        private PropertyBrushItem edgeBrush;
        private PropertyTextItem edgeWidth;

        private AnimationControl m_control;

        /// <summary>
        /// 
        /// </summary>
        public ViewModeItems()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public ViewModeItems(AnimationControl control)
        {
            m_control = control;
            InitializeComponent();

            //MessageResources.DialogTextBackgroundBrush
            //MessageResources.DialogTextEdgeBrush
            //MessageResources.DialogTextMaxEdgeWidth
            bgBrush.Brush = control.ViewBGBrush;
            edgeBrush.Brush = control.ViewEdgeBrush;
            edgeWidth.Text = control.MaxEdgeWidth.ToString();
        }

        void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewModeItems));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.bgBrush = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyBrushItem();
            this.edgeBrush = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyBrushItem();
            this.edgeWidth = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyTextItem();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Controls.Add(this.bgBrush);
            this.groupBox.Controls.Add(this.edgeBrush);
            this.groupBox.Controls.Add(this.edgeWidth);
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // bgBrush
            // 
            resources.ApplyResources(this.bgBrush, "bgBrush");
            this.bgBrush.Name = "bgBrush";
            // 
            // edgeBrush
            // 
            resources.ApplyResources(this.edgeBrush, "edgeBrush");
            this.edgeBrush.Name = "edgeBrush";
            // 
            // edgeWidth
            // 
            resources.ApplyResources(this.edgeWidth, "edgeWidth");
            this.edgeWidth.Name = "edgeWidth";
            this.edgeWidth.Validating += new System.ComponentModel.CancelEventHandler(this.MaxEdgeWidthValidating);
            // 
            // ViewModeItems
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBox);
            this.Name = "ViewModeItems";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        void MaxEdgeWidthValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = edgeWidth.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, edgeWidth.LabelText));
                edgeWidth.Text = Convert.ToString(m_control.MaxEdgeWidth);
                e.Cancel = true;
                return;
            }
            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, edgeWidth.LabelText));
                edgeWidth.Text = Convert.ToString(m_control.MaxEdgeWidth);
                e.Cancel = true;
                return;
            }
        }

        public void ApplyChanges()
        {
            m_control.ViewBGBrush = bgBrush.Brush;
            m_control.MaxEdgeWidth = float.Parse(edgeWidth.Text);
            m_control.ViewEdgeBrush = edgeBrush.Brush;
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
