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
using System.Drawing;

using Ecell.Objects;
using Ecell.Job;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;

using Ecell.IDE.Plugins.Analysis.AnalysisFile;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Class to manage the bifurcation analysis.
    /// </summary>
    public class BifurcationAnalysis : IAnalysisModule
    {
        #region Fields
        /// <summary>
        /// Manage of session.
        /// </summary>
        private Analysis m_owner;
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
        /// The number of data for Axis(50).
        /// </summary>
        private static int s_num = 7;
        /// <summary>
        /// The number of the interval of skip(5).
        /// </summary>
        private static int s_skip = 2;
        /// <summary>
        /// Job group related with this analysis.
        /// </summary>
        private JobGroup m_group;
        private bool m_isDone = false;
        private int m_resultPoint = 0;
        private const string s_simTime = "Simulation Time";
        private const string s_winSize = "Window Size";
        private const string s_maxInput = "Max Input for FFT";
        private const string s_maxFreq = "Max Frequency of FFT";
        private const string s_minFreq = "Min Frequency of FFT";
        /// <summary>
        /// Analysis name.
        /// </summary>
        public const string s_analysisName = "Bifurcation";
        /// <summary>
        /// The list of parameter entry.
        /// </summary>
        private List<EcellParameterData> m_paramList = new List<EcellParameterData>();
        /// <summary>
        /// The list of observed entry.
        /// </summary>
        private List<EcellObservedData> m_observedList = new List<EcellObservedData>();
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public BifurcationAnalysis(Analysis owner)
        {
            m_owner = owner;
            m_param = new BifurcationAnalysisParameter();

            m_result = new BifurcationResult[s_num + 1, s_num + 1];
            m_region = new int[(int)(s_num / s_skip) + 1, (int)(s_num / s_skip) + 1];
        }

        #region Accessors
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
                BifurcationAnalysisParameter p = value as BifurcationAnalysisParameter;
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
        #endregion

        /// <summary>
        /// Get the property of analysis.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAnalysisProperty()
        {
            Dictionary<string, string> paramDic = new Dictionary<string, string>();

            paramDic.Add(s_simTime, m_param.SimulationTime.ToString());
            paramDic.Add(s_winSize, m_param.WindowSize.ToString());
            paramDic.Add(s_maxInput, m_param.MaxInput.ToString());
            paramDic.Add(s_maxFreq, m_param.MaxFreq.ToString());
            paramDic.Add(s_minFreq, m_param.MinFreq.ToString());

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
                    case s_simTime:
                        m_param.SimulationTime = Double.Parse(paramDic[key]);
                        break;
                    case s_winSize:
                        m_param.WindowSize = Double.Parse(paramDic[key]);
                        break;
                    case s_maxInput:
                        m_param.MaxInput = Int32.Parse(paramDic[key]);
                        break;
                    case s_maxFreq:
                        m_param.MaxFreq = Double.Parse(paramDic[key]);
                        break;
                    case s_minFreq:
                        m_param.MinFreq = Double.Parse(paramDic[key]);
                        break;
                }
            }
        }

        /// <summary>
        /// Execute this function when this analysis is finished.
        /// </summary>
        public void NotifyAnalysisFinished()
        {
            JudgeBifurcationAnalysis();
            PrintResultData(false);
            int[,] respos = SearchPoint();
            Dictionary<int, ExecuteParameter> paramList = CreateExecuteParameter(respos);
            if (paramList.Count <= 0)
            {
                return;
            }
            String tmpDir = m_owner.JobManager.TmpDir;
            Group.Run();
            m_execParam = m_owner.JobManager.RunSimParameterSet(m_group.GroupName, tmpDir, m_model, m_param.SimulationTime, false, paramList);
        }

        /// <summary>
        /// Create the analysis instance.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public IAnalysisModule CreateNewInstance(JobGroup group)
        {
            BifurcationAnalysis instance = new BifurcationAnalysis(m_owner);
            instance.Group = group;

            return instance;
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
            }
            return false;
        }

        /// <summary>
        /// Judgement.
        /// </summary>
        public void Judgement()
        {
            m_owner.ClearResult();
            if (m_execParam == null)
                m_execParam = new Dictionary<int, ExecuteParameter>();
            m_execParam.Clear();
            foreach (Job.Job j in m_group.Jobs)
            {
                m_execParam.Add(j.JobID, j.ExecParam);
            }

            JudgeBifurcationAnalysis();
            PrintResultData(true);
            m_owner.ActivateResultWindow(true, false, false);
        }

        /// <summary>
        /// Execute the bifurcation analysis.
        /// </summary>
        public void ExecuteAnalysis()
        {
            m_resultPoint = 0;
            String tmpDir = m_owner.JobManager.TmpDir;
            double simTime = m_param.SimulationTime;
            m_isDone = false;

            if (simTime <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                    new object[] { MessageResources.NameSimulationTime, 0.0 }));
                m_group.IsGroupError = true;
                return;
            }

            m_model = "";
            List<string> modelList = m_owner.DataManager.GetModelList();
            if (modelList.Count > 0) m_model = modelList[0];

            List<EcellParameterData> paramList = m_owner.DataManager.GetParameterData();
            List<EcellObservedData> observedList = m_owner.DataManager.GetObservedData();
            if (paramList == null) return;
            if (paramList.Count != 2)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSetNumber,
                    new object[] { MessageResources.NameParameterData, 2 }));
                m_group.IsGroupError = true;
                return;
            }
            List<SaveLoggerProperty> saveList = m_owner.GetBAObservedDataList();
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

            m_owner.ClearResult();
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
            SetList(true);

            if (m_xMax == m_xMin)
            {
                m_xMax = m_xMin + 1.0;
                m_xMin = m_xMin - 1.0;
            }
            if (m_yMax == m_yMin)
            {
                m_yMax = m_yMin + 1.0;
                m_yMin = m_yMin - 1.0;
            }
            m_owner.SetResultGraphSize(m_xMax, m_xMin, m_yMax, m_yMin, false, false);

            int jobid = 0;
            Dictionary<int, ExecuteParameter> tmpDic = new Dictionary<int, ExecuteParameter>();
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
            m_group.AnalysisParameter = GetAnalysisProperty();
            m_execParam = m_owner.JobManager.RunSimParameterSet(m_group.GroupName, tmpDir, m_model, simTime, false, tmpDic);
        }

        private void SetList(bool isSetAxis)
        {
            int count = 0;
            foreach (EcellParameterData p in m_paramList)
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
                    for (int i = 0; i <= s_num; i++)
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
                if (isSetAxis)
                    m_owner.SetResultEntryBox(p.Key, isX, isY);
                count++;
            }
        }

        /// <summary>
        /// Prepare to execute the analysis again.
        /// </summary>
        public void PrepareReAnalysis()
        {
            for (int i = 0; i <= s_num; i++)
            {
                for (int j = 0; j <= s_num; j++)
                {
                    m_result[i, j] = BifurcationResult.None;
                }
            }
            for (int i = 0; i <= (int)(s_num / s_skip); i++)
            {
                for (int j = 0; j <= (int)(s_num / s_skip); j++)
                {
                    m_region[i, j] = 1;
                }
            }

            m_execParam.Clear();
            foreach (Job.Job j in m_group.Jobs)
            {
                m_execParam.Add(j.JobID, j.ExecParam);
            }
        }

        /// <summary>
        /// Print the current result.
        /// </summary>
        public void PrintResult()
        {
            m_owner.ClearResult();
            SetList(true);
            PrintResultData(true);
            m_owner.ActivateResultWindow(true, false, false);
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
                    m_resultPoint++;
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
                        bool isExec = false;
                        if (i == 0 || i == s_num)
                            isExec = true;
                        if (j == 0 || j == s_num)
                            isExec = true;
                        if (i + 1 <= s_num && m_result[i + 1, j] == BifurcationResult.NG)
                            isExec = true;
                        if (i - 1 >= 0 && m_result[i - 1, j] == BifurcationResult.NG)
                            isExec = true;
                        if (j + 1 <= s_num && m_result[i, j + 1] == BifurcationResult.NG)
                            isExec = true;
                        if (j - 1 >= 0 && m_result[i, j - 1] == BifurcationResult.NG)
                            isExec = true;

                        if (isExec)
                        {
                            paramDic.Add(m_xPath, m_xList[i]);
                            paramDic.Add(m_yPath, m_yList[j]);
                            res.Add(jobid, new ExecuteParameter(paramDic));
                            jobid++;
                        }
                    }
                    else if (ncount == acount)
                    {
                        int resX = i / s_skip;
                        int resY = j / s_skip;

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
        private void PrintResultData(bool isWrite)
        {
            int count = 0;
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
                    {
                        for (int m = -1; m <= 1; m++)
                        {
                            for (int n = -1; n <= 1; n++)
                            {
                                if (m == 0 && n == 0) continue;
                                if (i + m < 0 || i + m > s_num) continue;
                                if (j + n < 0 || j + n > s_num) continue;
                                if (m_result[i + m, j + n] != BifurcationResult.OK &&
                                    m_result[i + m, j + n] != BifurcationResult.FindOk)
                                    continue;
                                if (m == 0)
                                {
                                    bool isDraw = false;
                                    if (i - 1 >= 0)
                                    {
                                        if (m_result[i - 1, j + n] == BifurcationResult.NG &&
                                            m_result[i - 1, j] == BifurcationResult.NG)
                                            isDraw = true;
                                    }
                                    if (i + 1 <= s_num)
                                    {
                                        if (m_result[i + 1, j + n] == BifurcationResult.NG &&
                                            m_result[i+1, j] == BifurcationResult.NG)
                                            isDraw = true;
                                    }
                                    if (isDraw && isWrite)
                                    {
                                        List<PointF> list = new List<PointF>();
                                        list.Add(new PointF((float)m_xList[i], (float)m_yList[j]));
                                        list.Add(new PointF((float)m_xList[i + m], (float)m_yList[j + n]));
                                        m_owner.AddJudgementDataForBifurcation(list);
                                    } 
                                }
                                else if (n == 0)
                                {
                                    bool isDraw = false;
                                    if (j - 1 >= 0)
                                    {
                                        if (m_result[i + m, j - 1] == BifurcationResult.NG &&
                                            m_result[i, j - 1] == BifurcationResult.NG)
                                            isDraw = true;
                                    }
                                    if (j + 1 <= s_num)
                                    {
                                        if (m_result[i + m, j + 1] == BifurcationResult.NG &&
                                            m_result[i, j + 1] == BifurcationResult.NG)
                                            isDraw = true;
                                    }
                                    if (isDraw && isWrite)
                                    {
                                        List<PointF> list = new List<PointF>();
                                        list.Add(new PointF((float)m_xList[i], (float)m_yList[j]));
                                        list.Add(new PointF((float)m_xList[i + m], (float)m_yList[j + n]));
                                        m_owner.AddJudgementDataForBifurcation(list);
                                    }
                                }
                                else
                                {
                                    if (m_result[i, j + n] == BifurcationResult.NG &&
                                        m_result[i + m, j] == BifurcationResult.NG)
                                    {
                                        // nothing
                                    }
                                    else
                                    {
                                        if (m_result[i, j + n] == BifurcationResult.NG && isWrite)
                                        {
                                            List<PointF> list = new List<PointF>();
                                            list.Add(new PointF((float)m_xList[i], (float)m_yList[j]));
                                            list.Add(new PointF((float)m_xList[i + m], (float)m_yList[j + n]));
                                            m_owner.AddJudgementDataForBifurcation(list);
                                        }
                                        else if (m_result[i + m, j] == BifurcationResult.NG && isWrite)
                                        {
                                            List<PointF> list = new List<PointF>();
                                            list.Add(new PointF((float)m_xList[i], (float)m_yList[j]));
                                            list.Add(new PointF((float)m_xList[i + m], (float)m_yList[j + n]));
                                            m_owner.AddJudgementDataForBifurcation(list);
                                        }
                                    }
                                }
                            }
                        }

                        if (isWrite)
                        {
                            List<PointF> plist = new List<PointF>();
                            plist.Add(new PointF((float)m_xList[i], (float)m_yList[j]));
                            m_owner.AddJudgementDataForBifurcation(plist);
                        }
                        count++;
                    }
                }
            }
        }

        /// <summary>
        /// Judge the bifurcation from the simulation result.
        /// </summary>
        private void JudgeBifurcationAnalysis()
        {
            foreach (int jobid in m_execParam.Keys)
            {
                Job.Job j = m_owner.JobManager.GroupDic[m_group.GroupName].GetJob(jobid);
                if (j.Status != JobStatus.FINISHED)
                    continue;
                double x = j.ExecParam.GetParameter(m_xPath);
                double y = j.ExecParam.GetParameter(m_yPath);

                bool isOK = true;
                foreach (EcellObservedData p in m_observedList)
                {
                    Dictionary<double, double> logList =
                        j.GetLogData(p.Key);

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
            BifurcationAnlaysisParameterFile f = new BifurcationAnlaysisParameterFile(this, paramFile);
            f.Parameter = m_param;
            f.Read();

            SetList(false);
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
            BifurcationAnlaysisParameterFile f = new BifurcationAnlaysisParameterFile(this, paramFile);
            f.Parameter = m_param;
            f.Write();
        }

        private void SaveAnalysisResultFile(string resultFile)
        {
            StreamWriter writer = new StreamWriter(resultFile, false, Encoding.ASCII);

            for (int i = 0; i <= s_num; i++)
            {
                for (int j = 0; j <= s_num; j++)
                {
                    writer.Write(Type2Int(m_result[i, j]) + ",");
                }
                writer.WriteLine("");
            }

            writer.Close();
        }

        private void LoadAnalysisResultFile(string resultFile)
        {
            string line;
            string[] ele;
            StreamReader reader;
            reader = new StreamReader(resultFile, Encoding.ASCII);
            int i = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue;
                ele = line.Split(new char[] { ',' });

                for (int j = 0; j < ele.Length - 1; j++)
                {
                    int d = Int32.Parse(ele[j]);
                    m_result[i, j] = Int2Type(d);
                }
                i++;
            }
            reader.Close();
        }

        static private BifurcationResult Int2Type(int d)
        {
            switch (d)
            {
                case -1:
                    return BifurcationResult.None;
                case 0:
                    return BifurcationResult.OK;
                case 1:
                    return BifurcationResult.NG;
                case 2:
                    return BifurcationResult.FindOk;
            }
            return BifurcationResult.None;
        }

        static private int Type2Int(BifurcationResult type)
        {
            switch (type)
            {
                case BifurcationResult.None:
                    return -1;
                case BifurcationResult.OK:
                    return 0;
                case BifurcationResult.NG:
                    return 1;
                case BifurcationResult.FindOk:
                    return 2;
            }
            return -1;
        }
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
