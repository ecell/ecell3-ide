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

using Ecell.Exceptions;
using Ecell.Job;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// ParameterEstimationResultWindow
    /// </summary>
    public partial class ParameterEstimationResultWindow : UserControl
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private Analysis m_owner;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        public ParameterEstimationResultWindow(Analysis owner)
        {
            InitializeComponent();

            ContextMenuStrip peCntMenu = new ContextMenuStrip();
            ToolStripMenuItem peit = new ToolStripMenuItem();
            peit.Text = MessageResources.ReflectMenuText;
            peit.Click += new EventHandler(ClickReflectMenu);
            peCntMenu.Items.AddRange(new ToolStripItem[] { peit });
            PEEstimateView.ContextMenuStrip = peCntMenu;

            m_owner = owner;
        }
        #endregion

        /// <summary>
        /// Set the estimated parameter.
        /// </summary>
        /// <param name="param">the execution parameter.</param>
        /// <param name="result">the estimated value.</param>
        /// <param name="generation">the generation.</param>
        public void AddEstimateParameter(ExecuteParameter param, double result, int generation)
        {
            PEEstimateView.Rows.Clear();
            foreach (string key in param.ParamDic.Keys)
            {
                DataGridViewRow r = new DataGridViewRow();

                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = Convert.ToString(key);
                r.Cells.Add(c1);

                DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                c2.Value = Convert.ToString(param.ParamDic[key]);
                r.Cells.Add(c2);

                PEEstimateView.Rows.Add(r);
            }
            PEEstimationValue.Text = Convert.ToString(result);
            PEGenerateValue.Text = Convert.ToString(generation);
        }

        /// <summary>
        /// Clear the entries in result data.
        /// </summary>
        public void ClearResult()
        {
            PEEstimateView.Rows.Clear();
        }

        /// <summary>
        /// Save the result of parameter estimation to the file.
        /// </summary>
        /// <param name="writer">the save file name.</param>
        public void SaveParameterEstimationResult(StreamWriter writer)
        {
            writer.WriteLine("#PARAMETER");
            foreach (DataGridViewRow r in PEEstimateView.Rows)
            {
                foreach (DataGridViewCell c in r.Cells)
                {
                    writer.Write(c.Value.ToString() + ",");
                }
                writer.WriteLine("");
            }

            writer.WriteLine("#VALUE");
            writer.WriteLine(PEEstimationValue.Text);

        }

        /// <summary>
        /// Event to click the menu of result for PE.
        /// </summary>
        /// <param name="sender">object(MenuToolStrip)</param>
        /// <param name="e">EventArgs.</param>
        private void ClickReflectMenu(object sender, EventArgs e)
        {
            DataManager manager = m_owner.DataManager;
            foreach (DataGridViewRow r in PEEstimateView.Rows)
            {
                string path = Convert.ToString(r.Cells[0].Value);
                double v = Convert.ToDouble(r.Cells[1].Value);
                string[] ele = path.Split(new char[] { ':' });
                String objid = ele[1] + ":" + ele[2];
                List<string> modelList = manager.GetModelList();
                EcellObject obj = manager.GetEcellObject(modelList[0], objid, ele[0]);
                if (obj == null) continue;
                foreach (EcellData d in obj.Value)
                {
                    if (d.EntityPath.Equals(path))
                    {
                        d.Value = new EcellValue(v);
                        manager.RemoveParameterData(new EcellParameterData(d.EntityPath, 0.0));
                        manager.DataChanged(obj.ModelID, obj.Key, obj.Type, obj);
                    }
                }
            }
        }
    }
}