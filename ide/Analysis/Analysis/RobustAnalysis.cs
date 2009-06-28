//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
using System.Text;
using System.IO;
using System.Windows.Forms;

using Ecell.Objects;
using Ecell.Job;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;

using Ecell.IDE.Plugins.Analysis.AnalysisFile;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Class to manage the robust analysis.
    /// </summary>
    public class RobustAnalysis : IAnalysisModule
    {        
        #region Fields
        /// <summary>
        /// Parameter object of robust analysis.
        /// </summary>
        private RobustAnalysisParameter m_param;
        /// <summary>
        /// Plugin controller.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// The dictionary of execution parameters.
        /// </summary>
        private Dictionary<int, ExecuteParameter> m_paramDic;
        /// <summary>
        /// The max number of input data to be executed FFT.
        /// </summary>
        public const int MaxSize = 2097152;
        /// <summary>
        /// The list of parameter entry.
        /// </summary>
        private List<EcellParameterData> m_paramList = new List<EcellParameterData>();
        /// <summary>
        /// The list of observed entry.
        /// </summary>
        private List<EcellObservedData> m_observedList = new List<EcellObservedData>();
        /// <summary>
        /// Dictionary of judgement data.
        /// </summary>
        private Dictionary<int, bool> m_judgeResult = new Dictionary<int, bool>();
        /// <summary>
        /// Job group related with this analysis.
        /// </summary>
        private JobGroup m_group;
        /// <summary>
        /// The flag whether this analysis have any result.
        /// </summary>
        private bool m_isExistResult = false;
        private const string s_sampleNum = "Sample Num";
        private const string s_simTime = "Simulation Time";
        private const string s_isRandomCheck = "Random Check";
        private const string s_winSize = "Window Size";
        private const string s_maxInput = "Max Input for FFT";
        private const string s_maxFreq = "Max Frequency of FFT";
        private const string s_minFreq = "Min Frequency of FFT";
        /// <summary>
        /// Analysis name.
        /// </summary>
        public const string s_analysisName = "RobustAnalysis";
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public RobustAnalysis(Analysis owner)
        {
            m_owner = owner;
            m_param = new RobustAnalysisParameter();
        }

        #region accessors
        /// <summary>
        /// get / set the job group.
        /// </summary>
        public JobGroup Group
        {
            get { return this.m_group; }
            set { 
                this.m_group = value;
                value.AnalysisModule = this;
            }
        }

        /// <summary>
        /// set the analysis parameter.
        /// </summary>
        public object AnalysisParameter
        {
            set
            {
                RobustAnalysisParameter p = value as RobustAnalysisParameter;
                if (p != null)
                    m_param = p;
            }
        }

        /// <summary>
        /// get / set the parameter list.
        /// </summary>
        public List<EcellParameterData> ParameterDataList
        {
            get { return this.m_paramList; }
            set { this.m_paramList = value; }
        }

        /// <summary>
        /// get / set the observed list.
        /// </summary>
        public List<EcellObservedData> ObservedDataList
        {
            get { return this.m_observedList; }
            set { this.m_observedList = value; }
        }

        /// <summary>
        /// get the flag this analysis is enable to judge.
        /// </summary>
        public bool IsEnableReJudge
        {
            get { return true; }
        }


        /// <summary>
        /// get the flag this analysis is step execution.
        /// </summary>
        public bool IsStep
        {
            get { return false; }
        }

        /// <summary>
        /// get the simulation time or the step count.
        /// </summary>
        public double Count
        {
            get { return m_param.SimulationTime; }
        }

        /// <summary>
        /// get the flag whether this analysis have any result.
        /// </summary>
        public bool IsExistResult
        {
            get { return m_isExistResult; }
        }
        #endregion

        /// <summary>
        /// Get the property of analysis.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAnalysisProperty()
        {
            Dictionary<string, string> paramDic = new Dictionary<string, string>();

            paramDic.Add(s_sampleNum, m_param.SampleNum.ToString());
            paramDic.Add(s_simTime, m_param.SimulationTime.ToString());
            paramDic.Add(s_winSize, m_param.WinSize.ToString());
            paramDic.Add(s_maxInput, m_param.MaxData.ToString());
            paramDic.Add(s_maxFreq, m_param.MaxFreq.ToString());
            paramDic.Add(s_minFreq, m_param.MinFreq.ToString());
            paramDic.Add(s_isRandomCheck, m_param.IsRandomCheck.ToString());

            return paramDic;
        }

        /// <summary>
        /// Set the property of analysis.
        /// </summary>
        /// <param name="paramDic"></param>
        public void SetAnalysisProperty(Dictionary<string, string> paramDic)
        {
            foreach (string key in paramDic.Keys)
            {
                switch (key)
                {
                    case s_sampleNum:
                        m_param.SampleNum = Int32.Parse(paramDic[key]);
                        break;
                    case s_simTime:
                        m_param.SimulationTime = Double.Parse(paramDic[key]);
                        break;
                    case s_winSize:
                        m_param.WinSize = Double.Parse(paramDic[key]);
                        break;
                    case s_maxInput:
                        m_param.MaxData = Int32.Parse(paramDic[key]);
                        break;
                    case s_maxFreq:
                        m_param.MaxFreq = Double.Parse(paramDic[key]);
                        break;
                    case s_minFreq:
                        m_param.MinFreq = Double.Parse(paramDic[key]);
                        break;
                    case s_isRandomCheck:
                        m_param.IsRandomCheck = Boolean.Parse(paramDic[key]);
                        break;
                }
            }
        }

        /// <summary>
        /// Execute this function when this analysis is finished.
        /// </summary>
        public void NotifyAnalysisFinished()
        {
            JudgeRobustAnalysis();
        }

        /// <summary>
        /// Create the analysis instance.
        /// </summary>
        /// <param name="group">the job group.</param>
        /// <returns>Return new RobustAnalysis object.</returns>
        public IAnalysisModule CreateNewInstance(JobGroup group)
        {
            RobustAnalysis instance = new RobustAnalysis(m_owner);
            instance.Group = group;

            return instance;
        }

        /// <summary>
        /// Prepare to execute the analysis again.
        /// </summary>
        public void PrepareReAnalysis()
        {
        }

        /// <summary>
        /// Print the current result.
        /// </summary>
        public void PrintResult()
        {
            string xPath = "";
            string yPath = "";
            double xmax = 0.0;
            double xmin = 0.0;
            double ymax = 0.0;
            double ymin = 0.0;
            m_owner.ClearResult();

            int count = 0;
            foreach (EcellParameterData r in m_paramList)
            {
                bool isX = false;
                bool isY = false;
                if (count == 0)
                {
                    isX = true;
                    xPath = r.Key;
                    xmax = r.Max;
                    xmin = r.Min;
                }
                else if (count == 1)
                {
                    isY = true;
                    yPath = r.Key;
                    ymax = r.Max;
                    ymin = r.Min;
                }
                m_owner.SetResultEntryBox(r.Key, isX, isY);
                count++;
            }
            if (xmax == xmin)
            {
                xmax = xmin + 1.0;
                xmin = xmin - 1.0;
            }
            if (ymax == ymin)
            {
                ymax = ymax + 1.0;
                ymin = ymin - 1.0;
            }
            m_owner.SetResultGraphSize(xmax, xmin, ymax, ymin, false, false);

            foreach (Job.Job j in m_owner.JobManager.GroupDic[m_group.GroupName].Jobs)
            {
                if (!m_judgeResult.ContainsKey(j.JobID)) continue;
                double x = j.ExecParam.GetParameter(xPath);
                double y = j.ExecParam.GetParameter(yPath);

                m_owner.AddJudgementData(j.JobID, x, y, m_judgeResult[j.JobID]);
            }
            m_owner.ActivateResultWindow(m_group.GroupName, true, false, false);
        }

        /// <summary>
        /// Execute the robust analysis.
        /// </summary>
        public void ExecuteAnalysis()
        {
            try
            {
                m_owner.ClearResult();
                String tmpDir = m_owner.JobManager.TmpDir;
                int num = m_param.SampleNum;
                double simTime = m_param.SimulationTime; ;
                int maxSize = Convert.ToInt32(m_param.MaxData);
                if (num <= 0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                        new object[] { MessageResources.NameMaxSample, 0 }));
                    m_group.IsGroupError = true;
                    return;
                }
                if (simTime <= 0.0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                        new object[] { MessageResources.NameSimulationTime, 0.0 }));
                    m_group.IsGroupError = true;
                    return;
                }
                if (maxSize > MaxSize)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrSmaller,
                        new object[] { MessageResources.NameMaxSample, MaxSize }));
                    m_group.IsGroupError = true;
                    return;
                }

                string model = "";
                List<string> modelList = m_owner.DataManager.GetModelList();
                if (modelList.Count > 0) model = modelList[0];

                List<EcellParameterData> paramList = m_owner.DataManager.GetParameterData();
                List<EcellObservedData> observedList = m_owner.DataManager.GetObservedData();
                if (paramList == null) return;
                if (paramList.Count < 2)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrSetNumberMore,
                        new object[] { MessageResources.NameParameterData, 2 }));
                    m_group.IsGroupError = true;
                    return;
                }
                if (!m_param.IsRandomCheck && paramList.Count != 2)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrSetNumber,
                        new object[] { MessageResources.NameParameterData, 2 }));
                    m_group.IsGroupError = true;
                    return;
                }
                List<SaveLoggerProperty> saveList = m_owner.GetRAObservedDataList();
                if (saveList == null) return;

                m_paramList.Clear();
                foreach (EcellParameterData p in paramList)
                {
                    m_paramList.Add(p.Copy());
                }
                m_observedList.Clear();
                foreach (EcellObservedData o in observedList)
                {
                    m_observedList.Add(o.Copy());
                }

                m_owner.JobManager.SetParameterRange(paramList);
                m_owner.JobManager.SetLoggerData(saveList);
                m_group.AnalysisParameter = GetAnalysisProperty();
                if (m_param.IsRandomCheck == true)
                {
                    m_paramDic = m_owner.JobManager.RunSimParameterRange(m_group.GroupName, tmpDir, model, num, simTime, false);
                }
                else
                {
                    m_paramDic = m_owner.JobManager.RunSimParameterMatrix(m_group.GroupName, tmpDir, model, simTime, false);
                }
            }
            catch (Exception)
            {
                if (m_group.Status != AnalysisStatus.Stopped)
                    Util.ShowErrorDialog(string.Format(MessageResources.ErrExecute, MessageResources.NameRobustAnalysis));
            }
        }

        /// <summary>
        /// Get the flag whether this property is editable.
        /// </summary>
        /// <param name="key">the property name.</param>
        /// <returns>true or false.</returns>
        public bool IsEnableEditProperty(string key)
        {
            switch (key)
            {
                case s_sampleNum:
                    return false;
                case s_simTime:
                    return false;
                case s_winSize:
                    return false;
                case s_maxInput:
                    return true;
                case s_maxFreq:
                    return true;
                case s_minFreq:
                    return true;
                case s_isRandomCheck:
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Judgement.
        /// </summary>
        public void Judgement()
        {
            if (m_paramDic == null)
                m_paramDic = new Dictionary<int, ExecuteParameter>();
            m_paramDic.Clear();
            foreach (Job.Job j in m_group.Jobs)
            {
                m_paramDic.Add(j.JobID, j.ExecParam);
            }
            JudgeRobustAnalysis();
            PrintResult();
            m_owner.ActivateResultWindow(m_group.GroupName, true, false, false);
        }

        /// <summary>
        /// Load the analysis model, parameters, logs and result.
        /// </summary>
        /// <param name="dirName">the top directory of the loaded analysis.</param>
        public void LoadAnalysisInfo(string dirName)
        {
            List<string> labels;
            string analysisName;
            string paramFile = dirName + "/" + m_group.DateString + ".param";
            string resultFile = dirName + "/" + m_group.DateString + ".result";
            string metaFile = resultFile + ".meta";

            // Load the meta file of result.
            if (!AnalysisResultMetaFile.LoadFile(metaFile, out analysisName, out labels))
                return;

            // Load the result file.
            LoadAnalysisResultFile(resultFile);

            // Load the parameter file.
            RobustAnalysisParameterFile f = new RobustAnalysisParameterFile(this, paramFile);
            f.Parameter = m_param;
            f.Read();
        }

        /// <summary>
        /// Save the analysis model, parameters, logs and result.
        /// </summary>
        /// <param name="dirName">the top directory of the saved analysis.</param>
        public void SaveAnalysisInfo(string dirName)
        {
            string paramFile = dirName + "/" + m_group.DateString + ".param";
            string resultFile = dirName + "/" + m_group.DateString + ".result";
            string metaFile = resultFile + ".meta";

            // Save the meta file of result.
            List<string> list = new List<string>();
            AnalysisResultMetaFile.CreatePlotMetaFile(metaFile, s_analysisName, list);

            // Save the result file.
            SaveAnalysisResultFile(resultFile);

            // Save the parameter file.
            RobustAnalysisParameterFile f = new RobustAnalysisParameterFile(this, paramFile);
            f.Parameter = m_param;
            f.Write();
        }

        /// <summary>
        /// Load the analysis result file.
        /// </summary>
        /// <param name="resultFile">the result file.</param>
        private void LoadAnalysisResultFile(string resultFile)
        {
            string line;
            string[] ele;
            StreamReader reader;
            reader = new StreamReader(resultFile, Encoding.ASCII);
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue;
                ele = line.Split(new char[] { ',' });
                if (ele.Length != 2)
                    continue;
                int jobid = Int32.Parse(ele[0]);
                bool result = Convert.ToBoolean(ele[1]);
                m_judgeResult[jobid] = result;
                m_isExistResult = true;
            }
            reader.Close();
        }

        /// <summary>
        /// Save the analysis result file.
        /// </summary>
        /// <param name="resultFile">the result file.</param>
        private void SaveAnalysisResultFile(string resultFile)
        {
            StreamWriter writer = new StreamWriter(resultFile, false, Encoding.ASCII);

            foreach (Job.Job j in m_group.Jobs)
            {
                writer.WriteLine(j.JobID + "," + m_judgeResult[j.JobID]);
            }
            writer.Close();
        }

        /// <summary>
        /// Judge the robustness from the simulation result.
        /// </summary>
        private void JudgeRobustAnalysis()
        {
            try
            {
                if (m_paramList == null) return;
                if (m_paramList.Count < 2)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrSetNumberMore,
                        new object[] { MessageResources.NameParameterData, 2 }));
                    return;
                }

                foreach (int jobid in m_paramDic.Keys)
                {
                    Job.Job j = m_owner.JobManager.GroupDic[m_group.GroupName].GetJob(jobid);
                    if (j.Status != JobStatus.FINISHED)
                        continue;
                    bool isOK = true;
                    foreach (EcellObservedData p in m_observedList)
                    {
                        Dictionary<double, double> logList =
                            j.GetLogData(p.Key);

                        double simTime = Convert.ToDouble(m_param.SimulationTime);
                        double winSize = Convert.ToDouble(m_param.WinSize);
                        if (simTime > winSize)
                        {
                            Dictionary<double, double> tmpList = new Dictionary<double, double>();
                            foreach (double t in logList.Keys)
                            {
                                if (simTime - winSize > t) continue;
                                tmpList.Add(t, logList[t]);
                            }
                            logList.Clear();

                            foreach (double t in tmpList.Keys)
                            {
                                logList.Add(t, tmpList[t]);
                            }
                        }

                        bool rJudge = JudgeRobustAnalysisByRange(logList, p.Max, p.Min, p.Differ);
                        bool pJudge = JudgeRobustAnalysisByFFT(logList, p.Rate);
                        if (rJudge == false || pJudge == false)
                        {
                            isOK = false;
                            break;
                        }
                    }
                    m_judgeResult[jobid] = isOK;
                    m_isExistResult = true;
                }
            }
            catch (Exception)
            {
                if (m_group.Status != AnalysisStatus.Stopped)
                    Util.ShowErrorDialog(string.Format(MessageResources.ErrExecute, MessageResources.NameRobustAnalysis));
            }
        }

        /// <summary>
        /// Judge the range of log data for target property.
        /// </summary>
        /// <param name="resDic">log data.</param>
        /// <param name="max">the maximum data of log data.</param>
        /// <param name="min">the minimum data of log data.</param>
        /// <param name="diff">the difference of log data.</param>
        /// <returns>if all condition is OK, return true.</returns>
        private bool JudgeRobustAnalysisByRange(Dictionary<double, double> resDic, double max, double min, double diff)
        {
            bool isFirst = true;
            double minValue = 0.0;
            double maxValue = 0.0;
            foreach (double time in resDic.Keys)
            {
                if (resDic[time] > max || resDic[time] < min) return false;
                if (isFirst)
                {
                    isFirst = false;
                    minValue = resDic[time];
                    maxValue = resDic[time];
                    continue;
                }
                if (minValue > resDic[time]) minValue = resDic[time];
                if (maxValue < resDic[time]) maxValue = resDic[time];
            }

            if (maxValue - minValue > diff) return false;
            return true;
        }

        /// <summary>
        /// judge the FFT result for log data.
        /// </summary>
        /// <param name="resDic">log data.</param>
        /// <param name="rate">FFT rate.</param>
        /// <returns>if all judgement is ok, return true.</returns>
        private bool JudgeRobustAnalysisByFFT(Dictionary<double, double> resDic, double rate)
        {
            double maxFreq = m_param.MaxFreq;
            double minFreq = m_param.MinFreq;
            int maxSize = m_param.MaxData;

            ComplexFourierTransformation cft = new ComplexFourierTransformation();
            int size = 4;
            int divide = 1;
            while (true)
            {
                if (resDic.Count <= size) break;
                if (size >= maxSize)
                {
                    divide = resDic.Count / maxSize + 1;
                    break;
                }
                size = size * 2;
            }
            double[] data = new double[size * 2];
            int i = 0;
            foreach (double d in resDic.Keys)
            {
                if (i % divide != 0) continue;
                data[i * 2] = resDic[d];
                data[i * 2 + 1] = 0.0;
                i++;
            }

            cft.Convention = TransformationConvention.Matlab; // so we can check MATLAB consistency
            cft.TransformForward(data);

            int count = 0;
            for (int j = 0; j < size; j++)
            {
                double d = Math.Sqrt(data[j * 2] * data[j * 2] + data[j * 2 + 1] * data[j * 2 + 1]);
                if (maxFreq > d && minFreq < d) count++;
            }
            if ((double)count / (double)size > rate)
                return true;

            return false;
        }
    }


    /// <summary>
    /// Class to manage the parameter of robust analysis.
    /// </summary>
    public class RobustAnalysisParameter
    {
        private int m_sampleNum;
        private double m_simulationTime;
        private int m_maxData;
        private bool m_isRandomCheck;
        private double m_winSize;
        private double m_maxFreq;
        private double m_minFreq;


        /// <summary>
        /// Constructor.
        /// </summary>
        public RobustAnalysisParameter()
        {
            m_sampleNum = 10;
            m_simulationTime = 100.0;
            m_winSize = 10.0;
            m_isRandomCheck = true;
            m_maxData = 65536;
            m_maxFreq = 50;
            m_minFreq = 30;
        }

        /// <summary>
        /// get / set the number of samples.
        /// </summary>
        public int SampleNum
        {
            get { return m_sampleNum; }
            set { m_sampleNum = value; }
        }

        /// <summary>
        /// get / set the simulation time.
        /// </summary>
        public double SimulationTime
        {
            get { return m_simulationTime; }
            set { m_simulationTime = value; }
        }

        /// <summary>
        /// get / set the max number of samples to calculate FFT.
        /// </summary>
        public int MaxData
        {
            get { return m_maxData; }
            set { m_maxData = value; }
        }

        /// <summary>
        /// get / set the flag whether the parameter of this samples is random.
        /// </summary>
        public bool IsRandomCheck
        {
            get { return m_isRandomCheck; }
            set { m_isRandomCheck = value; }
        }

        /// <summary>
        /// get / set the window size to check.
        /// </summary>
        public double WinSize
        {
            get { return m_winSize; }
            set { m_winSize = value; }
        }

        /// <summary>
        /// get / set the max frequency of FFT.
        /// </summary>
        public double MaxFreq
        {
            get { return m_maxFreq; }
            set { m_maxFreq = value; }
        }

        /// <summary>
        /// get /set the min frequency of FFT.
        /// </summary>
        public double MinFreq
        {
            get { return m_minFreq; }
            set { m_minFreq = value; }
        }
    }

}
