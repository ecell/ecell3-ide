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
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.GridLayout
{
    /// <summary>
    /// 
    /// </summary>
    public class GridLayoutPanel : LayoutPanel
    {
        #region Fields
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox defMargin;
        private System.Windows.Forms.TextBox annealingRepeats;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox initialT;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox naturalLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox scratchCheckBox;
        private System.Windows.Forms.Label label4;
        
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        public GridLayoutPanel(ILayoutAlgorithm algorithm)
        {
            InitializeComponent();
            m_algorithm = algorithm;

            GridLayout grid = (GridLayout)algorithm;
            this.annealingRepeats.Text = grid.Kmax.ToString();
            this.naturalLength.Text = grid.NaturalLength.ToString();
            this.initialT.Text = grid.InitialT.ToString();
            this.defMargin.Text = grid.DefMargin.ToString();
            this.scratchCheckBox.Checked = (grid.SubIndex == 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridLayoutPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.defMargin = new System.Windows.Forms.TextBox();
            this.annealingRepeats = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.initialT = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.naturalLength = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.scratchCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // defMargin
            // 
            resources.ApplyResources(this.defMargin, "defMargin");
            this.defMargin.Name = "defMargin";
            this.defMargin.Validating += new System.ComponentModel.CancelEventHandler(this.defMargin_Validating);
            // 
            // annealingRepeats
            // 
            resources.ApplyResources(this.annealingRepeats, "annealingRepeats");
            this.annealingRepeats.Name = "annealingRepeats";
            this.annealingRepeats.Validating += new System.ComponentModel.CancelEventHandler(this.annealingRepeats_Validating);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // initialT
            // 
            resources.ApplyResources(this.initialT, "initialT");
            this.initialT.Name = "initialT";
            this.initialT.Validating += new System.ComponentModel.CancelEventHandler(this.initialT_Validating);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // naturalLength
            // 
            resources.ApplyResources(this.naturalLength, "naturalLength");
            this.naturalLength.Name = "naturalLength";
            this.naturalLength.Validating += new System.ComponentModel.CancelEventHandler(this.naturalLength_Validating);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // scratchCheckBox
            // 
            resources.ApplyResources(this.scratchCheckBox, "scratchCheckBox");
            this.scratchCheckBox.Name = "scratchCheckBox";
            this.scratchCheckBox.UseVisualStyleBackColor = true;
            // 
            // GridLayoutPanel
            // 
            this.Controls.Add(this.scratchCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.naturalLength);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.initialT);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.annealingRepeats);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.defMargin);
            this.Controls.Add(this.label1);
            this.Name = "GridLayoutPanel";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public override void ApplyChange()
        {
            GridLayout alogorithm = (GridLayout)m_algorithm;
            alogorithm.Kmax = int.Parse(this.annealingRepeats.Text);
            alogorithm.NaturalLength = int.Parse(this.naturalLength.Text);
            alogorithm.InitialT = int.Parse(this.initialT.Text);
            alogorithm.DefMargin = int.Parse(this.defMargin.Text);
            alogorithm.SubIndex = (scratchCheckBox.Checked) ? 0 : 1;
        }
        #endregion

        #region Event
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void defMargin_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridLayout alogorithm = (GridLayout)m_algorithm;
            string text = defMargin.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrNoInput, this.defMargin.Text));
                defMargin.Text = Convert.ToString(alogorithm.DefMargin);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrInvalidValue, defMargin.Text));
                defMargin.Text = Convert.ToString(alogorithm.DefMargin);
                e.Cancel = true;
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void annealingRepeats_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridLayout alogorithm = (GridLayout)m_algorithm;
            string text = annealingRepeats.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrNoInput, this.annealingRepeats.Text));
                annealingRepeats.Text = Convert.ToString(alogorithm.Kmax);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrInvalidValue, annealingRepeats.Text));
                annealingRepeats.Text = Convert.ToString(alogorithm.Kmax);
                e.Cancel = true;
                return;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initialT_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridLayout alogorithm = (GridLayout)m_algorithm;
            string text = initialT.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrNoInput, this.initialT.Text));
                initialT.Text = Convert.ToString(alogorithm.InitialT);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrInvalidValue, initialT.Text));
                initialT.Text = Convert.ToString(alogorithm.InitialT);
                e.Cancel = true;
                return;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void naturalLength_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridLayout alogorithm = (GridLayout)m_algorithm;
            string text = naturalLength.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrNoInput, this.naturalLength.Text));
                naturalLength.Text = Convert.ToString(alogorithm.NaturalLength);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(string.Format(MessageResGridLayout.ErrInvalidValue, naturalLength.Text));
                naturalLength.Text = Convert.ToString(alogorithm.NaturalLength);
                e.Cancel = true;
                return;
            }

        }
        #endregion

    }
}
