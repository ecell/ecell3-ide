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
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.defMargin = new System.Windows.Forms.TextBox();
            this.annealingRepeats = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.initialT = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.naturalLength = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default grid margin";
            // 
            // defMargin
            // 
            this.defMargin.Location = new System.Drawing.Point(178, 10);
            this.defMargin.Name = "defMargin";
            this.defMargin.Size = new System.Drawing.Size(91, 19);
            this.defMargin.TabIndex = 1;
            this.defMargin.Text = "30";
            this.defMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.defMargin.Validating += new System.ComponentModel.CancelEventHandler(this.defMargin_Validating);
            // 
            // annealingRepeats
            // 
            this.annealingRepeats.Location = new System.Drawing.Point(178, 44);
            this.annealingRepeats.Name = "annealingRepeats";
            this.annealingRepeats.Size = new System.Drawing.Size(91, 19);
            this.annealingRepeats.TabIndex = 3;
            this.annealingRepeats.Text = "250";
            this.annealingRepeats.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.annealingRepeats.Validating += new System.ComponentModel.CancelEventHandler(this.annealingRepeats_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Simulated annealing repeats";
            // 
            // initialT
            // 
            this.initialT.Location = new System.Drawing.Point(178, 79);
            this.initialT.Name = "initialT";
            this.initialT.Size = new System.Drawing.Size(91, 19);
            this.initialT.TabIndex = 5;
            this.initialT.Text = "60";
            this.initialT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.initialT.Validating += new System.ComponentModel.CancelEventHandler(this.initialT_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Initial temperature";
            // 
            // naturalLength
            // 
            this.naturalLength.Location = new System.Drawing.Point(178, 114);
            this.naturalLength.Name = "naturalLength";
            this.naturalLength.Size = new System.Drawing.Size(91, 19);
            this.naturalLength.TabIndex = 7;
            this.naturalLength.Text = "4";
            this.naturalLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.naturalLength.Validating += new System.ComponentModel.CancelEventHandler(this.naturalLength_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Natural length";
            // 
            // GridLayoutPanel
            // 
            this.Controls.Add(this.naturalLength);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.initialT);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.annealingRepeats);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.defMargin);
            this.Controls.Add(this.label1);
            this.Name = "GridLayoutPanel";
            this.Text = "Grid Layout";
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
