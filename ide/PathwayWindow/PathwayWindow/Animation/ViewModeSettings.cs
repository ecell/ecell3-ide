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
using Ecell.IDE.Plugins.PathwayWindow.Graphic;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class ViewModeSettings : UserControl
    {
        private Ecell.IDE.Plugins.PathwayWindow.Graphic.BrushComboBox backgroundImageComboBox;
        private System.ComponentModel.IContainer components;
        private Ecell.IDE.Plugins.PathwayWindow.Graphic.BrushComboBox arrowImageComboBox;
        private TextBox maxWidthTextBox;

        private AnimationControl m_control;

        public AnimationControl Control
        {
            set
            {
                this.m_control = value;
                backgroundImageComboBox.Brush = m_control.ViewBGBrush;
                arrowImageComboBox.Brush = m_control.ViewEdgeBrush;
                maxWidthTextBox.Text = m_control.MaxEdgeWidth.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ViewModeSettings()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public ViewModeSettings(AnimationControl control) :
            this()
        {
            m_control = control;

            backgroundImageComboBox.Brush = control.ViewBGBrush;
            arrowImageComboBox.Brush = control.ViewEdgeBrush;
            maxWidthTextBox.Text = control.MaxEdgeWidth.ToString();
        }

        void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewModeSettings));
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.maxWidthTextBox = new System.Windows.Forms.TextBox();
            this.arrowImageComboBox = new Ecell.IDE.Plugins.PathwayWindow.Graphic.BrushComboBox();
            this.backgroundImageComboBox = new Ecell.IDE.Plugins.PathwayWindow.Graphic.BrushComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // maxWidthTextBox
            // 
            resources.ApplyResources(this.maxWidthTextBox, "maxWidthTextBox");
            this.maxWidthTextBox.Name = "maxWidthTextBox";
            // 
            // arrowImageComboBox
            // 
            resources.ApplyResources(this.arrowImageComboBox, "arrowImageComboBox");
            this.arrowImageComboBox.Name = "arrowImageComboBox";
            // 
            // backgroundImageComboBox
            // 
            resources.ApplyResources(this.backgroundImageComboBox, "backgroundImageComboBox");
            this.backgroundImageComboBox.Name = "backgroundImageComboBox";
            // 
            // ViewModeSettings
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.maxWidthTextBox);
            this.Controls.Add(label4);
            this.Controls.Add(this.arrowImageComboBox);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.backgroundImageComboBox);
            this.Controls.Add(label1);
            this.Name = "ViewModeSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        void MaxEdgeWidthValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = maxWidthTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, MessageResources.NameArrowMaxWidth));
                maxWidthTextBox.Text = Convert.ToString(m_control.MaxEdgeWidth);
                e.Cancel = true;
                return;
            }
            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrInvalidValue, MessageResources.NameArrowMaxWidth));
                maxWidthTextBox.Text = Convert.ToString(m_control.MaxEdgeWidth);
                e.Cancel = true;
                return;
            }
        }

        public void ApplyChanges()
        {
            m_control.ViewBGBrush = BrushManager.ParseStringToBrush(backgroundImageComboBox.Text);
            m_control.ViewEdgeBrush = BrushManager.ParseStringToBrush(arrowImageComboBox.Text);
            m_control.MaxEdgeWidth = float.Parse(maxWidthTextBox.Text);
            //m_control.ViewBGBrush = bgBrush.Brush;
            //m_control.MaxEdgeWidth = float.Parse(edgeWidth.Text);
            //m_control.ViewEdgeBrush = edgeBrush.Brush;
        }

        public void ItemClosing()
        {
            string text = maxWidthTextBox.Text;

            // 0 < EdgeWidth <= 100
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy <= 0 || dummy > 100)
            {
                throw new EcellException(string.Format(MessageResources.ErrInvalidValue, MessageResources.NameArrowMaxWidth));
            }
        }
    }
}
