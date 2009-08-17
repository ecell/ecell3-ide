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
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Ecell;
using Ecell.Objects;
using Ecell.Job;
using Ecell.Exceptions;
using Ecell.Plugin;

using Ecell.IDE.Plugins.Analysis.AnalysisFile;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Manager class of analysis result window.
    /// </summary>
    public partial class AnalysisResultWindow : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// Dictionary of job id and judgement.
        /// </summary>
        private Dictionary<int, bool> m_jobList = new Dictionary<int, bool>();
        /// <summary>
        /// Plugin Controller.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// The pain to show the result of sensitivity analysis.
        /// </summary>
        private SensitivityAnalysisResultWindow m_sensResultWindow;
        /// <summary>
        /// The pain to show the result of graph data.
        /// </summary>
        private GraphResultWindow m_graphResultWindow;
        /// <summary>
        /// The pain to show the result of parameter estimation.
        /// </summary>
        private ParameterEstimationResultWindow m_paramResultWindow;
        /// <summary>
        /// The dock content to show the result of sensitivity analysis.
        /// </summary>
        private EcellDockContent m_sensContent;
        /// <summary>
        /// The dock content to show the result of graph data.
        /// </summary>
        private EcellDockContent m_graphContent;
        /// <summary>
        /// The dock content to show the result of parameter estimation.
        /// </summary>
        private EcellDockContent m_paramContent;
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the dictionary of job id and result.
        /// </summary>
        public Dictionary<int, bool> JobList
        {
            get { return this.m_jobList; }
            set { this.m_jobList = value; }
        }

        /// <summary>
        /// get dock content for sensitivity analysis.
        /// </summary>
        public EcellDockContent SensitivityAnalysisContent
        {
            get { return this.m_sensContent; }
        }

        /// <summary>
        /// get dock content for graph result view.
        /// </summary>
        public EcellDockContent GraphContent
        {
            get { return this.m_graphContent; }
        }

        /// <summary>
        /// get dock content for parameter estimation.
        /// </summary>
        public EcellDockContent ParameterEsitmationContent
        {
            get { return this.m_paramContent; }
        }

        /// <summary>
        /// get the graph window.
        /// </summary>
        public GraphResultWindow GraphWindow
        {
            get { return this.m_graphResultWindow; }
        }

        /// <summary>
        /// get the result window of parameter esimation.
        /// </summary>
        public ParameterEstimationResultWindow ParameterEstimationWindow
        {
            get { return this.m_paramResultWindow; }
        }

        /// <summary>
        /// get the result window of sensitivity analysis.
        /// </summary>
        public SensitivityAnalysisResultWindow SensitivityWindow
        {
            get { return this.m_sensResultWindow; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public AnalysisResultWindow(Analysis anal)
        {
            InitializeComponent();

            m_owner = anal;
            m_sensResultWindow = new SensitivityAnalysisResultWindow();
            m_graphResultWindow = new GraphResultWindow(m_owner, this);
            m_paramResultWindow = new ParameterEstimationResultWindow(m_owner);

        }
        #endregion

        /// <summary>
        /// Get the pain to show the analysis result.
        /// </summary>
        /// <returns>the list of analysis result window.</returns>
        public IEnumerable<EcellDockContent> GetWindowsForms()
        {
            m_graphContent = new EcellDockContent();
            m_graphResultWindow.Dock = DockStyle.Fill;
            m_graphContent.Controls.Add(m_graphResultWindow);
            m_graphContent.Name = "GraphResullt";
            m_graphContent.Text = MessageResources.GraphResult;
            m_graphContent.Icon = Resources.GraphResult;
            m_graphContent.TabText = m_graphContent.Text;
            m_graphContent.ContentType = DockContentType.ANALYSIS;

            m_sensContent = new EcellDockContent();
            m_sensResultWindow.Dock = DockStyle.Fill;
            m_sensContent.Controls.Add(m_sensResultWindow);
            m_sensContent.Name = "SensitivityResullt";
            m_sensContent.Text = MessageResources.SensitivityResult;
            m_sensContent.Icon = Resources.SensitivityResult;
            m_sensContent.TabText = m_sensContent.Text;
            m_sensContent.ContentType = DockContentType.ANALYSIS;

            m_paramContent = new EcellDockContent();
            m_paramResultWindow.Dock = DockStyle.Fill;
            m_paramContent.Controls.Add(m_paramResultWindow);
            m_paramContent.Name = "ParameterEstimationResullt";
            m_paramContent.Text = MessageResources.ParameterEstimationResult;
            m_paramContent.Icon = Resources.ParameterEstimationResult;
            m_paramContent.TabText = m_paramContent.Text;
            m_paramContent.ContentType = DockContentType.ANALYSIS;

            return new EcellDockContent[] { m_graphContent, m_sensContent, m_paramContent };
        }

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="jobid">the jobidof this parameters.</param>
        /// <param name="x">the value of parameter.</param>
        /// <param name="y">the value of parameter.</param>
        /// <param name="isOK">the flag whether this parameter is robustness.</param>
        public void AddJudgementData(int jobid, double x, double y, bool isOK)
        {
            m_graphResultWindow.AddJudgementData(jobid, x, y, isOK);
            m_jobList.Add(jobid, isOK);
        }

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="list">the values of parameter.[List[PointF]]</param>
        public void AddJudgementDataForBifurcation(List<PointF> list)
        {
            m_graphResultWindow.AddJudgementDataForBifurcation(list);
        }

        /// <summary>
        /// Add the judgement data of parameter estimation into graph.
        /// </summary>
        /// <param name="x">the number of generation.</param>
        /// <param name="y">the value of estimation.</param>
        public void AddEstimationData(int x, double y)
        {
            m_graphResultWindow.AddEstimationData(x, y);
        }

        /// <summary>
        /// Clear the entries in result data.
        /// </summary>
        public void ClearResult()
        {
            m_jobList.Clear();

            m_sensResultWindow.ClearResult();
            m_graphResultWindow.ClearResult();
            m_paramResultWindow.ClearResult();
        }

        /// <summary>
        /// Set the graph size of result.
        /// </summary>
        /// <param name="xmax">Max value of X axis.</param>
        /// <param name="xmin">Min value of X axis.</param>
        /// <param name="ymax">Max value of Y axis.</param>
        /// <param name="ymin">Min value of Y axis.</param>
        /// <param name="isAutoX">The flag whether X axis is auto scale.</param>
        /// <param name="isAutoY">The flag whether Y axis is auto scale.</param>
        public void SetResultGraphSize(double xmax, double xmin, double ymax, double ymin,
            bool isAutoX, bool isAutoY)
        {
            m_graphResultWindow.SetResultGraphSize(xmax, xmin, ymax, ymin, isAutoX, isAutoY);
        }

        /// <summary>
        /// Set the parameter entry to display the result.
        /// </summary>
        /// <param name="name">the parameter name.</param>
        /// <param name="isX">the flag whether this parameter is default parameter at X axis.</param>
        /// <param name="isY">the flag whether this parameter is default parameter at Y axis.</param>
        public void SetResultEntryBox(string name, bool isX, bool isY)
        {
            m_graphResultWindow.SetResultEntryBox(name, isX, isY);
        }

        /// <summary>
        /// Update the color of result by using the result value.
        /// </summary>
        public void UpdateResultColor()
        {
            m_sensResultWindow.UpdateResultColor();
        }

        /// <summary>
        /// Create the the row data of analysis result for variable.
        /// </summary>
        /// <param name="key">the property name of parameter.</param>
        /// <param name="sensList">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfCCC(string key, List<double> sensList)
        {
            m_sensResultWindow.AddSensitivityDataOfCCC(key, sensList);
        }

        /// <summary>
        /// Create the row data of analysis result for process
        /// </summary>
        /// <param name="key">the property name of parameter.</param>
        /// <param name="sensList">the list of sensitivity analysis result.</param>
        public void AddSensitivityDataOfFCC(string key, List<double> sensList)
        {
            m_sensResultWindow.AddSensitivityDataOfFCC(key, sensList);
        }

        /// <summary>
        /// Set the estimated parameter.
        /// </summary>
        /// <param name="param">the execution parameter.</param>
        /// <param name="result">the estimated value.</param>
        /// <param name="generation">the generation.</param>
        public void AddEstimateParameter(ExecuteParameter param, double result, int generation)
        {
            m_paramResultWindow.AddEstimateParameter(param, result, generation);
        }

        /// <summary>
        /// Set the header string of sensitivity matrix.
        /// </summary>
        /// <param name="activityList">the list of activity.</param>
        public void SetSensitivityHeader(List<string> activityList)
        {
            m_sensResultWindow.SetSensitivityHeader(activityList);
        }

        #region Events
        /// <summary>
        /// Event to close this window.
        /// </summary>
        /// <param name="sender">This form.</param>
        /// <param name="e">FormClosedEventArgs</param>
        void CloseCurrentForm(object sender, FormClosedEventArgs e)
        {
            if (m_owner != null)
            {
                m_owner.CloseAnalysisResultWindow();
            }
            m_owner = null;
        }
        #endregion
    }
}
