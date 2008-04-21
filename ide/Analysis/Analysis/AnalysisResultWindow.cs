using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using EcellLib;
using EcellLib.Objects;
using EcellLib.SessionManager;
using ZedGraph;

namespace EcellLib.Analysis
{
    public partial class AnalysisResultWindow : EcellDockContent
    {
        #region Fields
        private Dictionary<int, bool> m_jobList = new Dictionary<int, bool>();
        /// <summary>
        /// Plugin Controller.
        /// </summary>
        private Analysis m_control;
        /// <summary>
        /// Graph control to display the matrix of analysis result.
        /// </summary>
        private ZedGraphControl m_zCnt = null;
        /// <summary>
        /// The line information of dot plot.
        /// </summary>
        private LineItem m_line;
        /// <summary>
        /// SessionManager to manage the analysis session.
        /// </summary>
        private SessionManager.SessionManager m_manager;

        private Color m_headerColor;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public AnalysisResultWindow()
        {
            InitializeComponent();

            m_zCnt = new ZedGraphControl();
            m_zCnt.Dock = DockStyle.Fill;
            m_zCnt.GraphPane.Title.Text = "";
            m_zCnt.GraphPane.XAxis.Title.Text = "X";
            m_zCnt.GraphPane.YAxis.Title.Text = "Y";
            m_zCnt.GraphPane.Legend.IsVisible = false;
            m_zCnt.GraphPane.XAxis.Scale.Max = 100;
            m_zCnt.GraphPane.XAxis.Scale.Min = 0;
            m_zCnt.GraphPane.YAxis.Scale.Max = 100;
            m_zCnt.GraphPane.YAxis.Scale.Min = 0;
            RAAnalysisTableLayout.Controls.Add(m_zCnt, 0, 0);
            m_zCnt.AxisChange();
            m_zCnt.Refresh();

            m_manager = SessionManager.SessionManager.GetManager();
            this.FormClosed += new FormClosedEventHandler(CloseCurrentForm);

            ContextMenuStrip peCntMenu = new ContextMenuStrip();
            ToolStripMenuItem peit = new ToolStripMenuItem();
            peit.Text = Analysis.s_resources.GetString("ReflectMenuText");
            peit.Click += new EventHandler(ClickReflectMenu);
            peCntMenu.Items.AddRange(new ToolStripItem[] {peit});
            PEEstimateView.ContextMenuStrip = peCntMenu;

            m_headerColor = Color.LightCyan;
        }

        #endregion

        #region Accessors
        /// <summary>
        /// get / set the plugin controller.
        /// </summary>
        public Analysis Control
        {
            get { return this.m_control; }
            set { this.m_control = value; }
        }
        #endregion

        /// <summary>
        /// Draw the result point on graph view,
        /// </summary>
        /// <param name="x">the position of X.</param>
        /// <param name="y">the position of Y.</param>
        /// <param name="isOK">The flag whether this data is ok.</param>
        private void DrawPoint(double x, double y, bool isOK)
        {
            LineItem line = null;
            Color drawColor = Color.Blue;
            Color styleColor = Color.White;
            if (!isOK)
            {
                drawColor = Color.Red;
                styleColor = Color.Silver;
            }

            line = m_zCnt.GraphPane.AddCurve(
                "Result",
                new PointPairList(),
                drawColor,
                SymbolType.Circle);

            Fill f = new Fill(drawColor);
            line.Symbol.Fill = f;

            line.Line.Width = 5;
            line.AddPoint(new PointPair(x, y));

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
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
            RAXComboBox.Enabled = true;
            RAYComboBox.Enabled = true;

            DrawPoint(x, y, isOK);
            m_jobList.Add(jobid, isOK);
        }

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="x">the value of parameter.</param>
        /// <param name="y">the value of parameter.</param>
        public void AddJudgementDataForBifurcation(double x, double y)
        {
            RAXComboBox.Enabled = false;
            RAYComboBox.Enabled = false;
            
            DrawPoint(x, y, true);
        }

        /// <summary>
        /// Add the judgement data of parameter estimation into graph.
        /// </summary>
        /// <param name="x">the number of generation.</param>
        /// <param name="y">the value of estimation.</param>
        public void AddEstimationData(int x, double y)
        {
            RAXComboBox.Enabled = false;
            RAYComboBox.Enabled = false;

            if (m_line == null)
            {
                m_line = m_zCnt.GraphPane.AddCurve(
                        "Result",
                        new PointPairList(),
                        Color.Blue,
                        SymbolType.Circle);

                Fill f = new Fill(Color.Blue);
                m_line.Symbol.Fill = f;
            }
            m_line.AddPoint(new PointPair(x, y));

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Clear the entries in result data.
        /// </summary>
        public void ClearResult()
        {
            RAXComboBox.Items.Clear();
            RAYComboBox.Items.Clear();
            m_jobList.Clear();

            m_line = null;
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();

            PEEstimateView.Rows.Clear();
            SACCCGridView.Rows.Clear();
            SAFCCGridView.Rows.Clear();
        }

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
                c.HeaderText = key;
                gridView.Columns.Add(c);
            }

            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Style.BackColor = m_headerColor;
            r.Cells.Add(c1);
            c1.ReadOnly = true;

            foreach (string key in data)
            {
                c1 = new DataGridViewTextBoxCell();
                c1.Style.BackColor = m_headerColor;
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
            c.Value = key;
            c.Style.BackColor = m_headerColor;
            r.Cells.Add(c);
            c.ReadOnly = true;

            foreach (double d in sensList)
            {
                c = new DataGridViewTextBoxCell();
                c.Value = d;
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
            c.Value = key;
            c.Style.BackColor = m_headerColor;
            r.Cells.Add(c);
            c.ReadOnly = true;

            foreach (double d in sensList)
            {
                c = new DataGridViewTextBoxCell();
                c.Value = d;
                r.Cells.Add(c);
                c.ReadOnly = true;
            }
            SAFCCGridView.Rows.Add(r);
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
            m_zCnt.GraphPane.XAxis.Scale.Max = xmax;
            m_zCnt.GraphPane.XAxis.Scale.Min = xmin;
            m_zCnt.GraphPane.YAxis.Scale.Max = ymax;
            m_zCnt.GraphPane.YAxis.Scale.Min = ymin;
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = isAutoX;
            m_zCnt.GraphPane.YAxis.Scale.MaxAuto = isAutoY;
        }

        /// <summary>
        /// Redraw the result table and graph when axis data is changed.
        /// </summary>
        public void ChangeAxisIndex()
        {
            if (RAXComboBox.Text.Equals(RAYComboBox.Text))
                return;

            string xPath = RAXComboBox.Text;
            string yPath = RAYComboBox.Text;

            m_line = null;
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();

            foreach (int jobid in m_jobList.Keys)
            {
                double xd = m_manager.ParameterDic[jobid].ParamDic[xPath];
                double yd = m_manager.ParameterDic[jobid].ParamDic[yPath];

                DrawPoint(xd, yd, m_jobList[jobid]);
            }
        }

        /// <summary>
        /// Set the parameter entry to display the result.
        /// </summary>
        /// <param name="name">the parameter name.</param>
        /// <param name="isX">the flag whether this parameter is default parameter at X axis.</param>
        /// <param name="isY">the flag whether this parameter is default parameter at Y axis.</param>
        public void SetResultEntryBox(string name, bool isX, bool isY)
        {
            RAXComboBox.Items.Add(name);
            RAYComboBox.Items.Add(name);
            if (isX) RAXComboBox.SelectedText = name;
            if (isY) RAYComboBox.SelectedText = name;
        }

        #region Events
        /// <summary>
        /// Event to close this window.
        /// </summary>
        /// <param name="sender">This form.</param>
        /// <param name="e">FormClosedEventArgs</param>
        void CloseCurrentForm(object sender, FormClosedEventArgs e)
        {
            if (m_control != null)
            {
                m_control.CloseAnalysisResultWindow();
            }
            m_control = null;
        }

        /// <summary>
        /// Event to change the index of selected data.
        /// </summary>
        /// <param name="sender">object(ComboBox).</param>
        /// <param name="e">EventArgs.</param>
        private void XSelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeAxisIndex();
        }

        /// <summary>
        /// Event to change the index of selected data.
        /// </summary>
        /// <param name="sender">object(ComboBox).</param>
        /// <param name="e">EventArgs.</param>
        private void YSelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeAxisIndex();
        }

        /// <summary>
        /// Event to click the menu of result for PE.
        /// </summary>
        /// <param name="sender">object(MenuToolStrip)</param>
        /// <param name="e">EventArgs.</param>
        private void ClickReflectMenu(object sender, EventArgs e)
        {
            DataManager manager = DataManager.GetDataManager();
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
        #endregion
    }


}