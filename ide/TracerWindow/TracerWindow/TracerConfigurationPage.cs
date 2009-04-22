//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ecell.IDE;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// TracerConfigDialog
    /// </summary>
    public class TracerConfigurationPage : PropertyDialogPage
    {
        #region Fields
        private int m_plotNum;
        private double m_redrawInterval;
        private TextBox intervalTextBox;
        private TextBox numberTextBox;
        private TracerWindow m_owner;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="plotNum"></param>
        /// <param name="redrawInt"></param>
        public TracerConfigurationPage(TracerWindow owner, int plotNum, double redrawInt)
        {
            InitializeComponent();
            m_owner = owner;
            m_plotNum = plotNum;
            m_redrawInterval = redrawInt;

            numberTextBox.Text = plotNum.ToString();
            intervalTextBox.Text = redrawInt.ToString();
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.Label label4;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TracerConfigurationPage));
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.numberTextBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // intervalTextBox
            // 
            resources.ApplyResources(this.intervalTextBox, "intervalTextBox");
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.RedrawInterval_Validating);
            // 
            // numberTextBox
            // 
            resources.ApplyResources(this.numberTextBox, "numberTextBox");
            this.numberTextBox.Name = "numberTextBox";
            this.numberTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.PlotNumber_Validating);
            // 
            // TracerConfigDialog
            // 
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(this.intervalTextBox);
            this.Controls.Add(label1);
            this.Controls.Add(this.numberTextBox);
            this.Name = "TracerConfigDialog";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Override
        /// <summary>
        /// Apply the changed property.
        /// </summary>
        public override void ApplyChange()
        {
            m_owner.PlotNumber = m_plotNum;
            m_owner.RedrawInterval = m_redrawInterval;
        }

        /// <summary>
        /// Close this control.
        /// </summary>
        public override void PropertyDialogClosing()
        {
            if (m_redrawInterval <= 0.0 || m_redrawInterval > 3600.0)
            {
                throw new EcellException(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRedrawInterval));
            }
            if (m_plotNum < 0)
            {
                throw new EcellException(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePlotNumber));
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Validate the input data in PlotNumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlotNumber_Validating(object sender, CancelEventArgs e)
        {
            string text = numberTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NamePlotNumber));
                numberTextBox.Text = Convert.ToString(m_plotNum);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePlotNumber));
                numberTextBox.Text = Convert.ToString(m_plotNum);
                e.Cancel = true;
                return;
            }
            m_plotNum = dummy;
        }

        /// <summary>
        /// Validate the input data in the RedrawInterval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedrawInterval_Validating(object sender, CancelEventArgs e)
        {
            string text = intervalTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameRedrawInterval));
                intervalTextBox.Text = Convert.ToString(m_redrawInterval);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameRedrawInterval));
                intervalTextBox.Text = Convert.ToString(m_redrawInterval);
                e.Cancel = true;
                return;
            }
            m_redrawInterval = dummy;
        }
        #endregion

    }
}
