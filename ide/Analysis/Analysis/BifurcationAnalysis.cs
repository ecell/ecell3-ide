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
using System.Windows.Forms;

using EcellLib.Objects;
using EcellLib.Job;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Class to manage the bifurcation analysis.
    /// </summary>
    class BifurcationAnalysis
    {
        #region Fields
        /// <summary>
        /// Manage of session.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// Timer to update the status of jobs.
        /// </summary>
        private System.Windows.Forms.Timer m_timer;
        /// <summary>
        /// The flag whether the analysis is running.
        /// </summary>
        private bool m_isRunning = false;
        /// <summary>
        /// Model name to execute bifurcation analysis.
        /// </summary>
        private string m_model;
        /// <summary>
        /// The parameter for bifurcation analysis.
        /// </summary>
        private BifurcationAnalysisParameter m_param;
        /// <summary>
        /// The path of data for X Axis.
        /// </summary>
        private string m_xPath;
        /// <summary>
        /// The path of data for Y Axis.
        /// </summary>
        private string m_yPath;
        /// <summary>
        /// The max data for X Axis.
        /// </summary>
        private double m_xMax;
        /// <summary>
        /// The min data for Y Axis.
        /// </summary>
        private double m_xMin;
        /// <summary>
        /// The max data for Y Axis.
        /// </summary>
        private double m_yMax;
        /// <summary>
        /// The min data for Y Axis.
        /// </summary>
        private double m_yMin;
        /// <summary>
        /// The dictionary of result data.
        /// </summary>
        private BifurcationResult[,] m_result;
        /// <summary>
        /// The dictionary of result and parameter region.
        /// </summary>
        private int[,] m_region;
        /// <summary>
        /// The candidate data for X Axis.
        /// </summary>
        private List<double> m_xList = new List<double>();
        /// <summary>
        /// The candidate data for Y Axis.
        /// </summary>
        private List<double> m_yList = new List<double>();
        /// <summary>
        /// Dictionary the jobid and the parameter of analysis.
        /// </summary>
        private Dictionary<int, ExecuteParameter> m_execParam;
        /// <summary>
        /// The number of data for Axis.
        /// </summary>
        private static int s_num = 50;
        /// <summary>
        /// The number of the interval of skip.
        /// </summary>
        private static int s_skip =10;
        private bool m_isDone = false;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public BifurcationAnalysis(Analysis owner)
        {
            m_owner = owner;

            m_result = new BifurcationResult[s_num + 1, s_num + 1];
            m_region = new int[(int)(s_num / s_skip) + 1, (int)(s_num / s_skip) + 1];

            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Interval = 5000;
            m_timer.Tick += new EventHandler(FireTimer);
        }

        #region Accessors
        /// <summary>
        /// get / set the flag whether the bifurcation analysis is running.
        /// </summary>
        public bool IsRunning
        {
            get { return this.m_isRunning; }
        }
        #endregion

        /// <summary>
        /// Execute the bifurcation analysis.
        /// </summary>
        public void ExecuteAnalysis()
        {
            m_param = m_owner.GetBifurcationAnalysisPrameter();
            String tmpDir = m_owner.JobManager.TmpRootDir;
            double simTime = m_param.SimulationTime;
            m_isDone = false;

            if (simTime <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResAnalysis.ErrLarger,
                    MessageResAnalysis.NameSimulationTime, 0.0));
                return;
            }

            m_model = "";
            List<string> modelList = m_owner.DataManager.GetModelList();
            if (modelList.Count > 0) m_model = modelList[0];

            List<EcellParameterData> paramList = m_owner.DataManager.GetParameterData();
            if (paramList == null) return;
            if (paramList.Count != 2)
            {
                Util.ShowErrorDialog(String.Format(MessageResAnalysis.ErrSetNumber,
                    MessageResAnalysis.NameParameterData, 2));
                return;
            }
            List<SaveLoggerProperty> saveList = m_owner.GetBAObservedDataList();
            if (saveList == null) return;

            int count = 0;
            for (int i = 0; i <= s_num; i++)
            {
                for (int j = 0; j <= s_num; j++)
                {
                    m_result[i, j] = BifurcationResult.None;
                }
            }
            for (int i = 0; i <= (int)(s_num / s_skip ) ; i++)
            {
                for (int j = 0; j <= (int)(s_num / s_skip) ; j++)
                {
                    m_region[i, j] = 1;
                }
            }
            

            Dictionary<int, ExecuteParameter> tmpDic = new Dictionary<int, ExecuteParameter>();
            int jobid = 0;
            foreach (EcellParameterData p in paramList)
            {
                double step = (p.Max - p.Min) / (double)s_num;
                bool isX = false;
                bool isY = false;
                if (count == 0)
                {
                    isX = true;
                    m_xPath = p.Key;
                    m_xMax = p.Max;
                    m_xMin = p.Min;
                    m_xList.Clear();
                    for (int i = 0  ; i <= s_num ; i++ )
                    {
                        double d = p.Min + step * i;
                        m_xList.Add(d);
                    }
                }
                else if (count == 1)
                {
                    isY = true;
                    m_yPath = p.Key;
                    m_yMax = p.Max;
                    m_yMin = p.Min;
                    m_yList.Clear();
                    for (int i = 0; i <= s_num; i++)
                    {
                        double d = p.Min + step * i;
                        m_yList.Add(d);
                    }
                }
                m_owner.SetResultEntryBox(p.Key, isX, isY);
                count++;
            }
            m_owner.SetResultGraphSize(m_xMax, m_xMin, m_yMax, m_yMin, false, false);

            for (int i = 0; i <= s_num; i = i + s_skip)
            {
                double xd = m_xList[i];
                for (int j = 0; j <= s_num; j = j + s_skip)
                {
                    double yd = m_yList[j];
                    Dictionary<string, double> paramDic = new Dictionary<string,double>();
                    paramDic.Add(m_xPath, xd);
                    paramDic.Add(m_yPath, yd);
                    tmpDic.Add(jobid, new ExecuteParameter(paramDic));
                    jobid++;
                }
            }

            m_owner.JobManager.SetLoggerData(saveList);
            m_execParam = m_owner.JobManager.RunSimParameterSet(tmpDir, m_model, simTime, false, tmpDic);
            m_owner.ClearResult();
            m_isRunning = true;
            m_timer.Enabled = true;
            m_timer.Start();
        }

        /// <summary>
        /// Stop the bifurcation analysis.
        /// </summary>
        public void StopAnalysis()
        {
            m_owner.JobManager.StopRunningJobs();
            m_isRunning = false;
            m_owner.StopSensitivityAnalysis();
        }

        /// <summary>
        /// Add the result of bifurcation analysis.
        /// </summary>
        /// <param name="x">The candidate data for X Axis.</param>
        /// <param name="y">The candidate data for Y Axis.</param>
        /// <param name="res">The result of bifurcation analysis.</param>
        private void AddPoint(double x, double y, BifurcationResult res)
        {
            int i, j;
            for (i = 0; i < m_xList.Count; i++)
            {
                if (m_xList[i] == x) break;
            }
            for (j = 0; j < m_yList.Count; j++)
            {
                if (m_yList[j] == y) break;
            }
            if (i > s_num || j > s_num) return;
            if (res == BifurcationResult.NG)
            {
                int resX = i / s_skip;
                int resY = j / s_skip;
                m_region[resX, resY] = 0;
            }
            m_result[i, j] = res;
        }

        /// <summary>
        /// Search the points to execute analysis at next step.
        /// </summary>
        /// <returns>the list of point.</returns>
        private int[,] SearchPoint()
        {
            int[,] res = new int[s_num + 1, s_num + 1];
            int count = 0;
            for (int i = 0; i <= s_num; i++)
            {
                for (int j = 0; j <= s_num; j++)
                {
                    if (m_result[i, j] != BifurcationResult.OK) continue;
                    for (int k = -1; k <= 1; k = k + 2)
                    {
                        if (i + k >= 0 && i + k <= s_num)
                            res[i + k, j] = 1;
                        if (j + k >= 0 && j + k <= s_num)
                            res[i, j + k] = 1;
                        count++;
                    }
                    m_result[i, j] = BifurcationResult.FindOk;
                }
            }
            if (count == 0 && m_isDone == false)
            {
                for (int i = s_skip / 2; i <= s_num; i = i + s_skip)
                {
                    for (int j = s_skip / 2; j <= s_num; j = j + s_skip)
                    {
                        res[i, j] = 1;
                    }
                }
                m_isDone = true;
            }


            return res;
        }

        /// <summary>
        /// Create the parameter data by using the points to execute analysis.
        /// </summary>
        /// <param name="pos">the list of points.</param>
        /// <returns>the parameter data.</returns>
        private Dictionary<int, ExecuteParameter> CreateExecuteParameter(int[,] pos)
        {
            int jobid = 0;
            Dictionary<int, ExecuteParameter> res = new Dictionary<int, ExecuteParameter>();
            for (int i = 0; i <= s_num; i++)
            {
                for (int j = 0; j <= s_num; j++)
                {
                    Dictionary<string, double> paramDic = new Dictionary<string, double>();
                    if (pos[i, j] == 0) continue;
                    if (m_result[i, j] != BifurcationResult.None) continue;
                    int acount = 0;
                    int ncount = 0;
                    int gcount = 0;
                    int ocount = 0;
                    for (int m = -1; m <= 1; m = m + 2)
                    {
                        if (i + m < 0 || i + m > s_num) continue;
                        for (int n = -1; n <= 1; n = n + 2)
                        {
                            if (j + n < 0 || j + n > s_num) continue;
                            acount++;
                            if (m_result[i + m, j + n] == BifurcationResult.None)
                                ncount++;
                            else if (m_result[i + m, j + n] == BifurcationResult.OK ||
                                m_result[i + m, j + n] == BifurcationResult.FindOk)
                                ocount++;
                            else
                                gcount++;
                        }
                    }
                    if (acount == ocount)
                    {
                        // nothing
                    }
                    else if (ncount == acount)
                    {
                        int resX = i / s_skip;
                        int resY = i / s_skip;

                        bool isEnableEdge = false;
                        if (resX != 0)
                            if (m_region[resX - 1, resY] == 0)
                                isEnableEdge = true;
                        if (resY != 0)
                            if (m_region[resX, resY - 1] == 0)
                                isEnableEdge = true;
                        if (resX != (int)(s_num / s_skip))
                            if (m_region[resX + 1, resY] == 0)
                                isEnableEdge = true;
                        if (resY != (int)(s_num / s_skip))
                            if (m_region[resX, resY + 1] == 0)
                                isEnableEdge = true;
                        if (isEnableEdge)
                        {
                            paramDic.Add(m_xPath, m_xList[i]);
                            paramDic.Add(m_yPath, m_yList[j]);
                            res.Add(jobid, new ExecuteParameter(paramDic));
                            jobid++;
                        }
                    }
                    else
                    {
                        // gcount > 0 and so on.
                        paramDic.Add(m_xPath, m_xList[i]);
                        paramDic.Add(m_yPath, m_yList[j]);
                        res.Add(jobid, new ExecuteParameter(paramDic));
                        jobid++;
                    }

                }
            }
            return res;
        }

        /// <summary>
        /// Print the edge data of bifurcation analysis.
        /// </summary>
        private void PrintResultData()
        {
            m_owner.ClearResult();
            for (int i = 0; i <= s_num; i++)
            {
                for (int j = 0; j <= s_num; j++)
                {
                    if (m_result[i, j] != BifurcationResult.OK &&
                        m_result[i, j] != BifurcationResult.FindOk)
                        continue;
                    bool isEdge = false;
                    for (int m = -1; m <= 1; m = m + 2)
                    {
                        if (i + m >= 0 && i + m <= s_num)
                            if (m_result[i + m, j] == BifurcationResult.NG)
                            {
                                isEdge = true;
                                break;
                            }
                        if (j + m >= 0 && j + m <= s_num)
                            if (m_result[i, j + m] == BifurcationResult.NG)
                            {
                                isEdge = true;
                                break;
                            }
                    }
                    if (isEdge)
                        m_owner.AddJudgementDataForBifurcation(m_xList[i], m_yList[j]);
                }
            }
        }

        /// <summary>
        /// Judge the bifurcation from the simulation result.
        /// </summary>
        private void JudgeBifurcationAnalysis()
        {
            List<EcellObservedData> judgeList = m_owner.DataManager.GetObservedData();
            foreach (int jobid in m_execParam.Keys)
            {
                if (m_owner.JobManager.JobList[jobid].Status != JobStatus.FINISHED)
                    continue;
                double x = m_owner.JobManager.ParameterDic[jobid].GetParameter(m_xPath);
                double y = m_owner.JobManager.ParameterDic[jobid].GetParameter(m_yPath);

                bool isOK = true;
                foreach (EcellObservedData p in judgeList)
                {
                    Dictionary<double, double> logList =
                        m_owner.JobManager.JobList[jobid].GetLogData(p.Key);

                    double simTime = Convert.ToDouble(m_param.SimulationTime);
                    double winSize = Convert.ToDouble(m_param.WindowSize);
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
                    Application.DoEvents();
                    bool rJudge = JudgeBifurcationAnalysisByRange(logList, p.Max, p.Min, p.Differ);
                    bool pJudge = JudgeBifurcationAnalysisByFFT(logList, p.Rate);
                    if (rJudge == false || pJudge == false)
                    {
                        isOK = false;
                        break;
                    }
                }
                Application.DoEvents();
                if (isOK)
                    AddPoint(x, y, BifurcationResult.OK);
                else
                    AddPoint(x, y, BifurcationResult.NG);
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
        private bool JudgeBifurcationAnalysisByRange(Dictionary<double, double> resDic, double max, double min, double diff)
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
        private bool JudgeBifurcationAnalysisByFFT(Dictionary<double, double> resDic, double rate)
        {
            double maxFreq = m_param.MaxFreq;
            double minFreq = m_param.MinFreq;
            int maxSize = m_param.MaxInput;

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
            if ((double)count / (double)resDic.Count > rate)
                return true;

            return false;
        }

        #region Events
        /// <summary>
        /// Update the status of session at intervals while program is running.
        /// </summary>
        /// <param name="sender">Timer.</param>
        /// <param name="e">EventArgs.</param>
        void FireTimer(object sender, EventArgs e)
        {
            if (!m_owner.JobManager.IsFinished())
            {
                if (m_isRunning == false)
                {
                    m_owner.JobManager.StopRunningJobs();
                    m_timer.Enabled = false;
                    m_timer.Stop();
                }
                return;
            }
            m_timer.Enabled = false;
            m_timer.Stop();

            if (m_owner.JobManager.IsError())
            {
                if (!Util.ShowYesNoDialog(MessageResAnalysis.ConfirmFindErrorJob))


                {
                    return;
                }
            }
            JudgeBifurcationAnalysis();
            int[,] respos = SearchPoint();
            Dictionary<int, ExecuteParameter> paramList = CreateExecuteParameter(respos);
            if (paramList.Count <= 0)
            {
                PrintResultData();
                m_isRunning = false;
                m_owner.StopBifurcationAnalysis();
                Util.ShowNoticeDialog(String.Format(MessageResAnalysis.InfoFinishExecute,
                    MessageResAnalysis.NameBifurcation));

                return;
            }
            PrintResultData();
            m_owner.JobManager.ClearFinishedJobs();
            String tmpDir = m_owner.JobManager.TmpRootDir;
            m_execParam = m_owner.JobManager.RunSimParameterSet(tmpDir, m_model, m_param.SimulationTime, false, paramList);

            m_timer.Enabled = true;
            m_timer.Start();
        }
        #endregion

    }

    /// <summary>
    /// The parameter object of bifurcation analysis.
    /// </summary>
    public class BifurcationAnalysisParameter
    {
        /// <summary>
        /// Simulation time to create the data for analysis.
        /// </summary>
        private double m_simTime;
        /// <summary>
        /// Window size to use the data for analysis.
        /// </summary>
        private double m_winSize;
        /// <summary>
        /// The max number of input to calculate FFT.
        /// </summary>
        private int m_maxInput;
        /// <summary>
        /// Max frequency of FFT.
        /// </summary>
        private double m_maxFreq;
        /// <summary>
        /// Min frequency of FFT.
        /// </summary>
        private double m_minFreq;

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public BifurcationAnalysisParameter()
        {
            m_simTime = 100.0;
            m_winSize = 10.0;
            m_maxInput = 65535;
            m_maxFreq = 50.0;
            m_minFreq = 30.0;
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="simTime"></param>
        /// <param name="winSize"></param>
        /// <param name="maxInput"></param>
        /// <param name="maxFreq"></param>
        /// <param name="minFreq"></param>
        public BifurcationAnalysisParameter(double simTime, double winSize,
            int maxInput, double maxFreq, double minFreq)
        {
            m_simTime = simTime;
            m_winSize = winSize;
            m_maxInput = maxInput;
            m_maxFreq = maxFreq;
            m_minFreq = minFreq;
        }
        #endregion

        #region Accsessor
        /// <summary>
        /// get / set the simulation time.
        /// </summary>
        public double SimulationTime
        {
            get { return this.m_simTime; }
            set { this.m_simTime = value; }
        }

        /// <summary>
        /// get / set the window size.
        /// </summary>
        public double WindowSize
        {
            get { return this.m_winSize; }
            set { this.m_winSize = value; }
        }

        /// <summary>
        /// get / set the max number of input for FFT.
        /// </summary>
        public int MaxInput
        {
            get { return this.m_maxInput; }
            set { this.m_maxInput = value; }
        }

        /// <summary>
        /// get / set the max frequency to use FFT.
        /// </summary>
        public double MaxFreq
        {
            get { return this.m_maxFreq; }
            set { this.m_maxFreq = value; }
        }

        /// <summary>
        /// get / set the min frequency to use FFT.
        /// </summary>
        public double MinFreq
        {
            get { return this.m_minFreq; }
            set { this.m_minFreq = value; }
        }
        #endregion
    }

    /// <summary>
    /// The result information for Bifurcation.
    /// </summary>
    public enum BifurcationResult
    {
        /// <summary>
        /// This data is not done the analysis yet.
        /// </summary>
        None = -5,
        /// <summary>
        /// This data is NG for judgement.
        /// </summary>
        NG = 0,
        /// <summary>
        /// This data is OK for judgement.
        /// </summary>
        OK = 1,
        /// <summary>
        /// Data around this data is already judgement.
        /// </summary>
        FindOk = 2
    }
}
