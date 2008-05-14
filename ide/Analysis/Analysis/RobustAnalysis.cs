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
using EcellLib.Session;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Class to manage the robust analysis.
    /// </summary>
    public class RobustAnalysis
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
        /// Timer to update the status of jobs.
        /// </summary>
        private System.Windows.Forms.Timer m_timer;
        /// <summary>
        /// The flag whether the analysis is running.
        /// </summary>
        private bool m_isRunning = false;
        private Dictionary<int, ExecuteParameter> m_paramDic;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public RobustAnalysis(Analysis owner)
        {
            m_owner = owner;

            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Interval = 5000;
            m_timer.Tick += new EventHandler(FireTimer);
        }

        #region accessors
        /// <summary>
        /// get / set the flag whether the robust analysis is running.
        /// </summary>
        public bool IsRunning
        {
            get { return this.m_isRunning; }
        }
        #endregion


        #region Events
        /// <summary>
        /// Update the status of session at intervals while program is running.
        /// </summary>
        /// <param name="sender">Timer.</param>
        /// <param name="e">EventArgs.</param>
        void FireTimer(object sender, EventArgs e)
        {
            if (!m_owner.SessionManager.IsFinished())
            {
                if (m_isRunning == false)
                {
                    m_owner.SessionManager.StopRunningJobs();
                    m_timer.Enabled = false;
                    m_timer.Stop();
                }
                return;
            }
            m_isRunning = false;
            m_timer.Enabled = false;
            m_timer.Stop();
            m_owner.StopRobustAnalysis();

            if (m_owner.SessionManager.IsError())
            {
                String mes = Analysis.s_resources.GetString(MessageConstants.ErrFindErrorJob);
                if (!Util.ShowYesNoDialog(mes))
                {
                    return;
                }
            }
            JudgeRobustAnalysis();
            String finMes = Analysis.s_resources.GetString(MessageConstants.FinishRAnalysis);
            Util.ShowNoticeDialog(finMes);
        }
        #endregion

        /// <summary>
        /// Execute the robust analysis.
        /// </summary>
        public void ExecuteAnalysis()
        {
            m_param = m_owner.GetRobustAnalysisParameter();
            String tmpDir = m_owner.SessionManager.TmpRootDir;
            int num = m_param.SampleNum;
            double simTime = m_param.SimulationTime; ;
            int maxSize = Convert.ToInt32(m_param.MaxData);
            if (num <= 0)
            {
                string errmes = Analysis.s_resources.GetString(MessageConstants.ErrSampleNumPositive);
                Util.ShowErrorDialog(errmes);
                return;
            }
            if (simTime <= 0.0)
            {
                string errmes = Analysis.s_resources.GetString(MessageConstants.ErrSimTimeUnder);
                Util.ShowErrorDialog(errmes);
                return;
            }
            if (maxSize > AnalysisWindow.MaxSize)
            {
                string errmes = Analysis.s_resources.GetString(MessageConstants.ErrOverMax) + "[" + AnalysisWindow.MaxSize + "]";
                Util.ShowErrorDialog(errmes);
                return;
            }

            string model = "";
            List<string> modelList = m_owner.DataManager.GetModelList();
            if (modelList.Count > 0) model = modelList[0];

            List<EcellParameterData> paramList = m_owner.DataManager.GetParameterData();
            if (paramList == null) return;
            if (paramList.Count < 2)
            {
                String mes = Analysis.s_resources.GetString(MessageConstants.ErrParamProp2);
                Util.ShowErrorDialog(mes);
                return;
            }
            List<SaveLoggerProperty> saveList = m_owner.GetRAObservedDataList();
            if (saveList == null) return;

            m_owner.SessionManager.SetParameterRange(paramList);
            m_owner.SessionManager.SetLoggerData(saveList);
            if (m_param.IsRandomCheck == true)
            {
                m_paramDic = m_owner.SessionManager.RunSimParameterRange(tmpDir, model, num, simTime, false);
            }
            else
            {
                m_paramDic = m_owner.SessionManager.RunSimParameterMatrix(tmpDir, model, simTime, false);
            }
            m_isRunning = true;
            m_timer.Enabled = true;
            m_timer.Start();
        }

        /// <summary>
        /// Stop the robust analysis.
        /// </summary>
        public void StopAnalysis()
        {
            m_owner.SessionManager.StopRunningJobs();
            m_isRunning = false;
            m_owner.StopRobustAnalysis();
        }

        /// <summary>
        /// Judge the robustness from the simulation result.
        /// </summary>
        private void JudgeRobustAnalysis()
        {
            m_owner.ClearResult();
            string xPath = "";
            string yPath = "";
            double xmax = 0.0;
            double xmin = 0.0;
            double ymax = 0.0;
            double ymin = 0.0;
            List<EcellParameterData> pList = m_owner.DataManager.GetParameterData();
            if (pList == null) return;
            if (pList.Count < 2)
            {
                String mes = Analysis.s_resources.GetString(MessageConstants.ErrParamProp2);
                Util.ShowErrorDialog(mes);
                return;
            }
            int count = 0;
            foreach (EcellParameterData r in pList)
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
            m_owner.SetResultGraphSize(xmax, xmin, ymax, ymin, false, false);

            List<EcellObservedData> judgeList = m_owner.DataManager.GetObservedData();
            foreach (int jobid in m_paramDic.Keys)
            {
                if (m_owner.SessionManager.SessionList[jobid].Status != JobStatus.FINISHED)
                    continue;
                double x = m_owner.SessionManager.ParameterDic[jobid].GetParameter(xPath);
                double y = m_owner.SessionManager.ParameterDic[jobid].GetParameter(yPath);
                bool isOK = true;
                foreach (EcellObservedData p in judgeList)
                {
                    Dictionary<double, double> logList =
                        m_owner.SessionManager.SessionList[jobid].GetLogData(p.Key);

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
                m_owner.AddJudgementData(jobid, x, y, isOK);
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
            if ((double)count / (double)resDic.Count > rate)
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
