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
    public partial class AnalysisResultWindow : EcellDockContent
    {
        #region Fields
        private Dictionary<int, bool> m_jobList = new Dictionary<int, bool>();
        /// <summary>
        /// Plugin Controller.
        /// </summary>
        private Analysis m_owner;
        private SensitivityAnalysisResultWindow m_sensResultWindow;
        private GraphResultWindow m_graphResultWindow;
        private ParameterEstimationResultWindow m_paramResultWindow;
        private EcellDockContent m_sensContent;
        private EcellDockContent m_graphContent;
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
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EcellDockContent> GetWindowsForms()
        {
            m_graphContent = new EcellDockContent();
            m_graphResultWindow.Dock = DockStyle.Fill;
            m_graphContent.Controls.Add(m_graphResultWindow);
            m_graphContent.Name = "GraphResullt";
            m_graphContent.Text = MessageResources.GraphResult;
            m_graphContent.Icon = Resources.GraphResult;
            m_graphContent.TabText = m_graphContent.Text;
            m_graphContent.IsSavable = true;

            m_sensContent = new EcellDockContent();
            m_sensResultWindow.Dock = DockStyle.Fill;
            m_sensContent.Controls.Add(m_sensResultWindow);
            m_sensContent.Name = "SensitivityResullt";
            m_sensContent.Text = MessageResources.SensitivityResult;
            m_sensContent.Icon = Resources.SensitivityResult;
            m_sensContent.TabText = m_sensContent.Text;
            m_sensContent.IsSavable = true;

            m_paramContent = new EcellDockContent();
            m_paramResultWindow.Dock = DockStyle.Fill;
            m_paramContent.Controls.Add(m_paramResultWindow);
            m_paramContent.Name = "ParameterEstimationResullt";
            m_paramContent.Text = MessageResources.ParameterEstimationResult;
            m_paramContent.Icon = Resources.ParameterEstimationResult;
            m_paramContent.TabText = m_paramContent.Text;
            m_paramContent.IsSavable = true;

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
        /// Load the file written the analysis result.
        /// </summary>
        /// <param name="filename">the load file.</param>
        public void LoadResultFile(string filename)
        {
            StreamReader reader = null;
            try
            {
                ClearResult();
                string metaFile = filename + ".meta";

                string analysisName;
                List<string> labels;

                if (!AnalysisResultMetaFile.LoadFile(metaFile, out analysisName, out labels))
                    return;

                // not implement.
                List<EcellObject> sysObjList = new List<EcellObject>();
                List<EcellObject> stepperObjList = new List<EcellObject>();

                reader = new StreamReader(filename, Encoding.ASCII);

                if (analysisName.Equals("BifurcationAnalysis"))
                {
                    LoadBifurcationResult(reader, labels);
                }
                else if (analysisName.Equals("ParameterEstimation"))
                {
                    LoadParameterEstimationResult(reader, labels);
                }
                else if (analysisName.Equals("RobustAnalysis"))
                {
                    LoadRobustAnalysisResult(reader, labels, sysObjList, stepperObjList);
                }
                else if (analysisName.Equals("SensitivityAnalysis"))
                {
                    LoadSensitivityAnalysisResult(reader);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                m_owner.PluginManager.ChangeStatus(ProjectStatus.Loaded);
                if (reader != null)
                    reader.Close();
            }
        }

        private void LoadBifurcationResult(StreamReader reader, List<string> labels)
        {
            List<PointF> list = new List<PointF>();
            string line;

            string xlabel = "";
            string ylabel = "";
            if (labels.Count == 2)
            {
                xlabel = labels[0];
                ylabel = labels[1];
                m_owner.SetResultEntryBox(xlabel, true, false);
                m_owner.SetResultEntryBox(ylabel, false, true);
            }

            m_graphResultWindow.PreGraphSet();
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue;
                if (String.IsNullOrEmpty(line) || line[0] == '\n' || line[0] == '\r')
                {
                    if (list.Count > 0)
                    {
                        m_graphResultWindow.AddJudgementDataForBifurcation(list);
                    }
                    list.Clear();
                    continue;
                }
                string[] ele = line.Split(new char[] { ',' });
                list.Add(new PointF((float)Convert.ToDouble(ele[0]), (float)Convert.ToDouble(ele[1])));
            }
            m_graphResultWindow.PostGraphSet();
        }

        private void LoadParameterEstimationResult(StreamReader reader, List<string> labels)
        {
            int readPos = 1;
            int maxGene = 0;
            double data = 0.0;
            string line;

            ExecuteParameter param = new ExecuteParameter();
            m_graphResultWindow.PreGraphSet();
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                else if (line.Length <= 1)
                {
                    readPos++;
                    continue;
                }

                if (readPos == 1)
                {
                    string[] ele = line.Split(new char[] { ',' });
                    int g = Convert.ToInt32(ele[0]);
                    double d = Convert.ToDouble(ele[1]);
                    if (g > maxGene)
                    {
                        maxGene = g;
                        data = d;
                    }
                    m_graphResultWindow.AddEstimationData(g, d);
                }
                else if (readPos == 2)
                {
                    string[] ele = line.Split(new char[] { ',' });
                    param.AddParameter(ele[0], Convert.ToDouble(ele[1]));
                }
            }
            m_paramResultWindow.AddEstimateParameter(param, data, maxGene);
            m_graphResultWindow.PostGraphSet();
        }

        private void LoadRobustAnalysisResult(StreamReader reader, List<string> labels, List<EcellObject> sysObjList, List<EcellObject> stepperList)
        {
            string line;
            string[] ele;

            JobGroup group = m_owner.JobManager.CreateJobGroup("RobustAnalysis", sysObjList, stepperList);
            Dictionary<int, string> paramDic = new Dictionary<int, string>();
            int i = 0;
            foreach (string label in labels)
            {
                if (i == 0)
                    m_graphResultWindow.SetResultEntryBox(label, true, false);
                else if (i == 1)
                    m_graphResultWindow.SetResultEntryBox(label, false, true);
                else
                    m_graphResultWindow.SetResultEntryBox(label, false, false);
                paramDic.Add(i, label);
                i++;
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
                    p.AddParameter(paramDic[j - 1], Convert.ToDouble(ele[j]));
                }
                int jobid = m_owner.JobManager.CreateJobEntry(group.GroupName, p);
                AddJudgementData(jobid, x, y, result);
            }
        }

        private void LoadSensitivityAnalysisResult(StreamReader reader)
        {
            bool isFirst = true;
            int readPos = 1;
            string line;
            string[] ele;
            int i;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line.Length <= 1)
                {
                    isFirst = true;
                    readPos++;
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
                        m_sensResultWindow.SetSensitivityHeader(headList);
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
                    m_sensResultWindow.AddSensitivityDataOfCCC(ele[0], valList);
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
                    m_sensResultWindow.AddSensitivityDataOfFCC(ele[0], valList);                    
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
                m_graphResultWindow.SaveBifurcationResult(fileName, writer);
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
                m_graphResultWindow.SaveParameterEstimationResult(fileName, writer);
                m_paramResultWindow.SaveParameterEstimationResult(writer);
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
                m_graphResultWindow.SaveRobustAnalysisResult(fileName, writer);
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
