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
using ZedGraph;

namespace Ecell.IDE.Plugins.Analysis
{
    public partial class AnalysisResultWindow : EcellDockContent
    {
        #region Fields
        private Dictionary<int, bool> m_jobList = new Dictionary<int, bool>();
        /// <summary>
        /// Plugin Controller.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// Graph control to display the matrix of analysis result.
        /// </summary>
        private ZedGraphControl m_zCnt = null;
        /// <summary>
        /// The line information of dot plot.
        /// </summary>
        private LineItem m_line;

        private Color m_headerColor;
        private int x_index = 0;
        private int y_index = 0;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public AnalysisResultWindow(Analysis anal)
        {
            InitializeComponent();

            m_owner = anal;
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

            this.FormClosed += new FormClosedEventHandler(CloseCurrentForm);

            ContextMenuStrip peCntMenu = new ContextMenuStrip();
            ToolStripMenuItem peit = new ToolStripMenuItem();
            peit.Text = MessageResources.ReflectMenuText;
            peit.Click += new EventHandler(ClickReflectMenu);
            peCntMenu.Items.AddRange(new ToolStripItem[] {peit});
            PEEstimateView.ContextMenuStrip = peCntMenu;

            m_headerColor = Color.LightCyan;
            this.TabText = this.Text;

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
                string[] ele = key.Split(new char[] { ':' });
                c.HeaderText = ele[ele.Length - 2];
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
                string[] ele = key.Split(new char[] { ':' });
                c1.Value = ele[ele.Length - 2];
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
            string[] ele = key.Split(new char[] { ':' });
            c.Value = ele[ele.Length - 2];
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
            string[] ele = key.Split(new char[] { ':' });
            c.Value = ele[ele.Length - 2];
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
                throw new IgnoreException("");

            string xPath = RAXComboBox.Text;
            string yPath = RAYComboBox.Text;

            m_line = null;
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();

            foreach (int jobid in m_jobList.Keys)
            {
                double xd = m_owner.JobManager.ParameterDic[jobid].ParamDic[xPath];
                double yd = m_owner.JobManager.ParameterDic[jobid].ParamDic[yPath];

                DrawPoint(xd, yd, m_jobList[jobid]);
            }
            m_zCnt.GraphPane.XAxis.Scale.MinAuto = true;
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = true;
            m_zCnt.GraphPane.YAxis.Scale.MinAuto = true;
            m_zCnt.GraphPane.YAxis.Scale.MaxAuto = true;
            m_zCnt.AxisChange();
            m_zCnt.Refresh();           
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
            if (isX)
            {
                for (int i = 0; i < RAXComboBox.Items.Count; i++)
                {
                    if (RAXComboBox.Items[i].Equals(name))
                    {
                        RAXComboBox.SelectedIndex = i;
                    }
                }
            }
            if (isY)
            {
                for (int i = 0; i < RAYComboBox.Items.Count; i++)
                {
                    if (RAYComboBox.Items[i].Equals(name))
                    {
                        RAYComboBox.SelectedIndex = i;
                    }
                }
            }
        }

        /// <summary>
        /// Load the file written the analysis result.
        /// </summary>
        /// <param name="filename">the load file.</param>
        public void LoadResultFile(string filename)
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filename, Encoding.ASCII);

                string header = reader.ReadLine();
                if (header.StartsWith("#BIFURCATION"))
                {
                    LoadBifurcationResult(reader);
                }
                else if (header.StartsWith("#PARAMETER"))
                {
                    LoadParameterEstimationResult(reader);
                }
                else if (header.StartsWith("#ROBUST"))
                {
                    LoadRobustAnalysisResult(reader);
                }
                else if (header.StartsWith("#SENSITIVITY"))
                {
                    LoadSensitivityAnalysisResult(reader);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private void LoadBifurcationResult(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue;
                string[] ele = line.Split(new char[] { ',' });
                AddJudgementDataForBifurcation(Convert.ToDouble(ele[0]),
                    Convert.ToDouble(ele[1]));
            }
        }

        private void LoadParameterEstimationResult(StreamReader reader)
        {
            int readPos = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#GENERATION"))
                {
                    readPos = 1;
                    continue;
                }
                else if (line.StartsWith("#PARAMETER"))
                {
                    readPos = 2;
                    continue;
                }
                else if (line.StartsWith("#VALUE"))
                {
                    readPos = 3;
                    continue;
                }
                else if (line.StartsWith("#"))
                {
                    continue;
                }

                ExecuteParameter param = new ExecuteParameter();
                if (readPos == 1)
                {
                    string[] ele = line.Split(new char[] { ',' });
                    AddEstimationData(Convert.ToInt32(ele[0]), Convert.ToDouble(ele[1]));
                }
                else if (readPos == 2)
                {
                    string[] ele = line.Split(new char[] { ',' });
                    param.AddParameter(ele[0], Convert.ToDouble(ele[1]));
                }
                else if (readPos == 3)
                {
                    string[] ele = line.Split(new char[] { ',' });
                    double v = Convert.ToDouble(ele[0]);
                    AddEstimateParameter(param, v, 10);
                }
            }
        }

        private void LoadRobustAnalysisResult(StreamReader reader)
        {
            string line;
            string[] ele;
            line = reader.ReadLine();
            ele = line.Split(new char[] { ',' });
            Dictionary<int, string> paramDic = new Dictionary<int, string>();
            for (int i = 0; i < ele.Length; i++)
            {
                if (String.IsNullOrEmpty(ele[i])) continue;

                if (i == 1)
                    SetResultEntryBox(ele[i], true, false);
                else if (i == 2)
                    SetResultEntryBox(ele[i], false, true);
                else
                    SetResultEntryBox(ele[i], false, false);
                paramDic.Add(i, ele[i]);
            }
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue;
                double x = 0.0;
                double y = 0.0;
                ExecuteParameter p = new ExecuteParameter();
                ele = line.Split(new char[] { ',' });
                bool result = Convert.ToBoolean(ele[0]);
                for (int j = 1; j < ele.Length; j++)
                {
                    if (String.IsNullOrEmpty(ele[j])) continue;
                    if (j == 1) x = Convert.ToDouble(ele[j]);
                    if (j == 2) y = Convert.ToDouble(ele[j]);
                    p.AddParameter(paramDic[j], Convert.ToDouble(ele[j]));
                }
                int jobid = m_owner.JobManager.CreateJobEntry(p);
                AddJudgementData(jobid, x, y, result);
            }
        }

        private void LoadSensitivityAnalysisResult(StreamReader reader)
        {
            bool isFirst = true;
            int readPos = 0;
            string line;
            string[] ele;
            int i;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#CCC"))
                {
                    isFirst = true;
                    readPos = 1;
                    continue;
                }
                else if (line.StartsWith("#FCC"))
                {
                    isFirst = true;
                    readPos = 2;
                    continue;
                }
                else if (line.StartsWith("#"))
                {
                    continue;
                }

                if (readPos == 1)
                {

                    if (isFirst)
                    {                        
                        List<string> headList = new List<string>();
                        ele = line.Split(new char[] { ',' });
                        for (i = 1; i < ele.Length; i++)
                        {
                            if (String.IsNullOrEmpty(ele[i])) continue;
                            headList.Add(ele[i]);
                        }
                        SetSensitivityHeader(headList);
                        isFirst = false;
                        continue;
                    }
                    List<double> valList = new List<double>();
                    ele = line.Split(new char[] { ',' });
                    for (i = 1; i < ele.Length; i++)
                    {
                        if (String.IsNullOrEmpty(ele[i])) continue;
                        valList.Add(Convert.ToDouble(ele[i]));
                    }
                    AddSensitivityDataOfCCC(ele[0], valList);
                }
                else if (readPos == 2)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }
                    List<double> valList = new List<double>();
                    ele = line.Split(new char[] { ',' });
                    for (i = 1; i < ele.Length; i++)
                    {
                        if (String.IsNullOrEmpty(ele[i])) continue;
                        valList.Add(Convert.ToDouble(ele[i]));
                    }
                    AddSensitivityDataOfFCC(ele[0], valList);                    
                }
            }
        }

        /// <summary>
        /// Save the result of bifurcation analysis to the file.
        /// </summary>
        /// <param name="fileName">the save file name.</param>
        public void SaveBifurcationResult(string fileName)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(fileName, false, Encoding.ASCII);

                writer.WriteLine("#BIFURCATION");
                foreach (LineItem c in m_zCnt.GraphPane.CurveList)
                {
                    for ( int i = 0 ; i < c.Points.Count ; i++ )
                    {
                        writer.WriteLine(c[i].X + "," + c[i].Y);
                    }
                }
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSaveFile,
                    new object[] { fileName }));
            }
            finally
            {
                writer.Close();
            }
        }

        /// <summary>
        /// Save the result of parameter estimation to the file.
        /// </summary>
        /// <param name="fileName">the save file name.</param>
        public void SaveParameterEstimationResult(string fileName)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(fileName, false, Encoding.ASCII);
                writer.WriteLine("#PARAMETER");
                writer.WriteLine("#GENERATION");
                for (int i = 0; i < m_line.Points.Count; i++)
                {
                    writer.WriteLine(m_line[i].X + "," + m_line[i].Y);
                }

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
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSaveFile,
                    new object[] { fileName }));
            }
            finally
            {
                writer.Close();
            }
        }

        /// <summary>
        /// Save the result of robust analysis to the file.
        /// </summary>
        /// <param name="fileName">the save file name.</param>
        public void SaveRobustAnalysisResult(string fileName)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(fileName, false, Encoding.ASCII);

                List<string> paramList = new List<string>();
                writer.Write(",");
                for (int ind = 0; ind < RAXComboBox.Items.Count; ind++)
                {
                    string name = RAXComboBox.Items[ind] as string;
                    writer.Write("," + name);
                }
                writer.WriteLine("");

                writer.WriteLine("#ROBUST");
                foreach (int jobid in m_jobList.Keys)
                {
                    writer.Write(m_jobList[jobid]);
                    foreach (string param in paramList)
                    {
                        double data = m_owner.JobManager.ParameterDic[jobid].ParamDic[param];
                        writer.Write("," + data);
                    }
                    writer.WriteLine("");
                }
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSaveFile,
                    new object[] { fileName }));
            }
            finally
            {
                writer.Close();
            }
        }

        /// <summary>
        /// Save the result of sensitivity analysis to the file.
        /// </summary>
        /// <param name="fileName">the save file name.</param>
        public void SaveSensitivityAnalysisResult(string fileName)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(fileName, false, Encoding.ASCII);

                writer.WriteLine("#SENSITIVITY");
                writer.WriteLine("#CCC");
                foreach (DataGridViewRow r in SACCCGridView.Rows)
                {
                    foreach (DataGridViewCell c in r.Cells)
                    {
                        if (c.Value == null)
                            writer.Write(",");
                        else
                            writer.Write(c.Value.ToString() + ",");
                    }
                    writer.WriteLine("");
                }
                writer.WriteLine("#FCC");
                foreach (DataGridViewRow r in SAFCCGridView.Rows)
                {
                    foreach (DataGridViewCell c in r.Cells)
                    {
                        if (c.Value == null)
                            writer.Write(",");
                        else
                            writer.Write(c.Value.ToString() + ",");
                    }
                    writer.WriteLine("");
                }

            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSaveFile,
                    new object[] { fileName }));
            }
            finally
            {
                writer.Close();
            }
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

        /// <summary>
        /// Event to change the index of selected data.
        /// </summary>
        /// <param name="sender">object(ComboBox).</param>
        /// <param name="e">EventArgs.</param>
        private void XSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAxisIndex();
                x_index = RAXComboBox.SelectedIndex;
            }
            catch (IgnoreException)
            {
                RAXComboBox.SelectedIndex = x_index;
            }
        }

        /// <summary>
        /// Event to change the index of selected data.
        /// </summary>
        /// <param name="sender">object(ComboBox).</param>
        /// <param name="e">EventArgs.</param>
        private void YSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAxisIndex();
                y_index = RAYComboBox.SelectedIndex;
            }
            catch (IgnoreException)
            {
                RAYComboBox.SelectedIndex = y_index;
            }
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

        private void ARTrackBarChanged(object sender, EventArgs e)
        {
            RATrackLabel.Text = Convert.ToString(ARTrackBar.Value / 100.0);
            UpdateResultColor();
        }
        #endregion
    }
}