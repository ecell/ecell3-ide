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
// written by Sachio Nohara<nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.IO;
using Ecell.IDE.Plugins.Analysis.AnalysisFile;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// SensitivityResultWindow
    /// </summary>
    public partial class SensitivityAnalysisResultWindow : UserControl
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private Color m_headerColor;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructors
        /// </summary>
        public SensitivityAnalysisResultWindow()
        {
            InitializeComponent();

            m_headerColor = Color.LightCyan;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void ClearResult()
        {
            SACCCGridView.Rows.Clear();
            SAFCCGridView.Rows.Clear();
        }

        /// <summary>
        /// Set the header string of sensitivity matrix.
        /// </summary>
        /// <param name="activityList">the list of activity.</param>
        public void SetSensitivityHeader(List<string> activityList)
        {
            SACCCGridView.Columns.Clear();
            SACCCGridView.Rows.Clear();
            SAFCCGridView.Columns.Clear();
            SAFCCGridView.Rows.Clear();

            CreateSensitivityHeader(SACCCGridView, activityList);
            CreateSensitivityHeader(SAFCCGridView, activityList);
        }

        /// <summary>
        /// Create the header of sensitivity matrix.
        /// </summary>
        /// <param name="gridView">DataGridView.</param>
        /// <param name="data">Header List.</param>
        private void CreateSensitivityHeader(DataGridView gridView, List<string> data)
        {
            DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
            gridView.Columns.Add(c);
            foreach (string key in data)
            {
                c = new DataGridViewTextBoxColumn();
                c.Name = key;
                if (key.Contains(":"))
                {
                    string[] ele = key.Split(new char[] { ':' });
                    c.HeaderText = ele[ele.Length - 2];
                }
                else
                    c.HeaderText = key;
                gridView.Columns.Add(c);
            }

            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = "Item";
            c1.Style.BackColor = m_headerColor;
            r.Cells.Add(c1);
            c1.ReadOnly = true;

            foreach (string key in data)
            {
                c1 = new DataGridViewTextBoxCell();
                c1.Style.BackColor = m_headerColor;
                if (key.Contains(":"))
                {
                    string[] ele = key.Split(new char[] { ':' });
                    c1.Value = ele[ele.Length - 2];
                }
                else
                    c1.Value = key;
                r.Cells.Add(c1);
                c1.ReadOnly = true;
            }
            gridView.Rows.Add(r);
        }

        /// <summary>
        /// Create the the row data of analysis result for variable.
        /// </summary>
        /// <param name="key">the property name of parameter.</param>
        /// <param name="sensList">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfCCC(string key, List<double> sensList)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
            if (key.Contains(":"))
            {
                string[] ele = key.Split(new char[] { ':' });
                c.Value = ele[ele.Length - 2];
            }
            else
            {
                c.Value = key;
            }
            c.Style.BackColor = m_headerColor;
            r.Cells.Add(c);
            c.ReadOnly = true;

            foreach (double d in sensList)
            {
                c = new DataGridViewTextBoxCell();
                c.Value = d.ToString("###0.000");
                c.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                r.Cells.Add(c);
                c.ReadOnly = true;
            }
            SACCCGridView.Rows.Add(r);
        }

        /// <summary>
        /// Create the row data of analysis result for process
        /// </summary>
        /// <param name="key">the property name of parameter.</param>
        /// <param name="sensList">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfFCC(string key, List<double> sensList)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
            if (key.Contains(":"))
            {
                string[] ele = key.Split(new char[] { ':' });
                c.Value = ele[ele.Length - 2];
            }
            else
                c.Value = key;
            c.Style.BackColor = m_headerColor;
            r.Cells.Add(c);
            c.ReadOnly = true;

            foreach (double d in sensList)
            {
                c = new DataGridViewTextBoxCell();
                c.Value = d.ToString("###0.000");
                c.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                r.Cells.Add(c);
                c.ReadOnly = true;
            }
            SAFCCGridView.Rows.Add(r);
        }

        /// <summary>
        /// Update the color of result by using the result value.
        /// </summary>
        public void UpdateResultColor()
        {
            foreach (DataGridViewRow r in SACCCGridView.Rows)
            {
                foreach (DataGridViewCell c in r.Cells)
                {
                    try
                    {
                        Double d = Convert.ToDouble(c.Value);
                        if (Math.Abs(d) > ARTrackBar.Value / 100.0)
                        {
                            c.Style.BackColor = Color.Red;
                        }
                        else
                        {
                            c.Style.BackColor = Color.White;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            foreach (DataGridViewRow r in SAFCCGridView.Rows)
            {
                foreach (DataGridViewCell c in r.Cells)
                {
                    try
                    {
                        Double d = Convert.ToDouble(c.Value);
                        if (Math.Abs(d) > ARTrackBar.Value / 100.0)
                        {
                            c.Style.BackColor = Color.Red;
                        }
                        else
                        {
                            c.Style.BackColor = Color.White;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void ARTrackBarChanged(object sender, EventArgs e)
        {
            RATrackLabel.Text = Convert.ToString(ARTrackBar.Value / 100.0);
            UpdateResultColor();
        }
    }
}
