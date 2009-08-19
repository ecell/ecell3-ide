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
using Ecell.IDE;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.CBGridLayout
{
    /// <summary>
    /// 
    /// </summary>
    public class CBGridLayoutPanel : LayoutPanel
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox kaTextBox;
        private System.Windows.Forms.TextBox iterationTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox marginTextBox;
        private System.Windows.Forms.TextBox krTextBox;
    
        public CBGridLayoutPanel(ILayoutAlgorithm layout)
        {
            InitializeComponent();
            m_algorithm = layout;

            CBGridLayout grid = (CBGridLayout)layout;
            this.krTextBox.Text = grid.Kr.ToString();
            this.kaTextBox.Text = grid.Ka.ToString();
            this.iterationTextBox.Text = grid.Iteration.ToString();
            this.marginTextBox.Text = grid.Margin.ToString();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CBGridLayoutPanel));
            this.krTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.kaTextBox = new System.Windows.Forms.TextBox();
            this.iterationTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.marginTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // krTextBox
            // 
            this.krTextBox.AccessibleDescription = null;
            this.krTextBox.AccessibleName = null;
            resources.ApplyResources(this.krTextBox, "krTextBox");
            this.krTextBox.BackgroundImage = null;
            this.krTextBox.Font = null;
            this.krTextBox.Name = "krTextBox";
            this.krTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Kr_Validating);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            // 
            // kaTextBox
            // 
            this.kaTextBox.AccessibleDescription = null;
            this.kaTextBox.AccessibleName = null;
            resources.ApplyResources(this.kaTextBox, "kaTextBox");
            this.kaTextBox.BackgroundImage = null;
            this.kaTextBox.Font = null;
            this.kaTextBox.Name = "kaTextBox";
            this.kaTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Ka_Validating);
            // 
            // iterationTextBox
            // 
            this.iterationTextBox.AccessibleDescription = null;
            this.iterationTextBox.AccessibleName = null;
            resources.ApplyResources(this.iterationTextBox, "iterationTextBox");
            this.iterationTextBox.BackgroundImage = null;
            this.iterationTextBox.Font = null;
            this.iterationTextBox.Name = "iterationTextBox";
            this.iterationTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Iteration_Validating);
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            // 
            // marginTextBox
            // 
            this.marginTextBox.AccessibleDescription = null;
            this.marginTextBox.AccessibleName = null;
            resources.ApplyResources(this.marginTextBox, "marginTextBox");
            this.marginTextBox.BackgroundImage = null;
            this.marginTextBox.Font = null;
            this.marginTextBox.Name = "marginTextBox";
            this.marginTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.marginTextBox_Validating);
            // 
            // CBGridLayoutPanel
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.marginTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.iterationTextBox);
            this.Controls.Add(this.kaTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.krTextBox);
            this.Font = null;
            this.Name = "CBGridLayoutPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public override void ApplyChange()
        {
            CBGridLayout alogorithm = (CBGridLayout)m_algorithm;
            alogorithm.Kr = float.Parse(krTextBox.Text);
            alogorithm.Ka = float.Parse(kaTextBox.Text);
            alogorithm.Iteration = Int32.Parse(iterationTextBox.Text);
            alogorithm.Margin = float.Parse(marginTextBox.Text);
        }
        #endregion

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kr_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CBGridLayout alogorithm = (CBGridLayout)m_algorithm;
            string text = krTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrNoInput, label2.Text));
                krTextBox.Text = Convert.ToString(alogorithm.Kr);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy < 0.0)
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrInvalidValue, label2.Text));
                krTextBox.Text = Convert.ToString(alogorithm.Kr);
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ka_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CBGridLayout alogorithm = (CBGridLayout)m_algorithm;
            string text = kaTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrNoInput, label3.Text));
                kaTextBox.Text = Convert.ToString(alogorithm.Ka);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy < 0.0)
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrInvalidValue, label3.Text));
                kaTextBox.Text = Convert.ToString(alogorithm.Ka);
                e.Cancel = true;
                return;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Iteration_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CBGridLayout alogorithm = (CBGridLayout)m_algorithm;
            string text = iterationTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrNoInput, label4.Text));
                iterationTextBox.Text = Convert.ToString(alogorithm.Iteration);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy) || dummy < 0)
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrInvalidValue, label4.Text));
                iterationTextBox.Text = Convert.ToString(alogorithm.Iteration);
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void marginTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CBGridLayout alogorithm = (CBGridLayout)m_algorithm;
            string text = marginTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrNoInput, label5.Text));
                marginTextBox.Text = Convert.ToString(alogorithm.Margin);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy < 0.0)
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrInvalidValue, label5.Text));
                marginTextBox.Text = Convert.ToString(alogorithm.Margin);
                e.Cancel = true;
                return;
            }

        }

        #endregion
    }
}
