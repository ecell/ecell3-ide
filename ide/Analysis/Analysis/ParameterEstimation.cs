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

using EcellLib.SessionManager;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Class to manage the parameter estimation.
    /// </summary>
    public class ParameterEstimation
    {
        /// <summary>
        /// Form to display the setting and result of analysis.
        /// </summary>
        private AnalysisWindow m_win;
        /// <summary>
        /// Manage of session.
        /// </summary>
        private SessionManager.SessionManager m_manager;
        /// <summary>
        /// The flag whether the analysis is running.
        /// </summary>
        private bool m_isRunning = false;
        /// <summary>
        /// Timer to update the status of jobs.
        /// </summary>
        private System.Windows.Forms.Timer m_timer;
        /// <summary>
        /// Plugin controller.
        /// </summary>
        private Analysis m_control;
        private string m_model;
        private ParameterEstimationParameter m_param;
        private List<ParameterRange> m_paramList;
        private List<SaveLoggerProperty> m_saveList;
        private int m_generation;
        private Dictionary<int, ExecuteParameter> m_execParamList;
        private ExecuteParameter m_elite;
        private int m_eliteNum;
        private Random hRandom = new Random();
        private double m_mutation;
        private Dictionary<int, double> m_estimation;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParameterEstimation()
        {
            m_win = AnalysisWindow.GetWindow();
            m_manager = SessionManager.SessionManager.GetManager();

            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Interval = 5000;
            m_timer.Tick += new EventHandler(FireTimer);
            m_estimation = new Dictionary<int, double>();
        }

        /// <summary>
        /// get / set the flag whether the robust analysis is running.
        /// </summary>
        public bool IsRunning
        {
            get { return this.m_isRunning; }
        }

        /// <summary>
        /// get / set the parent plugin.
        /// </summary>
        public Analysis Control
        {
            get { return this.m_control; }
            set { this.m_control = value; }
        }

                /// <summary>
        /// Execute the robust analysis.
        /// </summary>
        public void ExecuteAnalysis()
        {
            m_estimation.Clear();
            m_param = m_win.GetParameterEstimationParameter();
            m_mutation = m_param.Param.Initial;
            m_manager.ClearJob(0);

            if (m_param.Population <= 0)
            {
                string errmes = Analysis.s_resources.GetString("ErrPopulationNumPositive");
                MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_param.Generation <= 0)
            {
                string errmes = Analysis.s_resources.GetString("ErrGenerationNumPositive");
                MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_param.SimulationTime <= 0.0)
            {
                string errmes = Analysis.s_resources.GetString("ErrSimTimeUnder");
                MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m_model = "";
            List<string> modelList = DataManager.GetDataManager().GetModelList();
            if (modelList.Count > 0) m_model = modelList[0];

            m_paramList = m_win.ExtractParameter();
            if (m_paramList == null) return;
            if (m_paramList.Count < 1)
            {
                String mes = Analysis.s_resources.GetString("ErrParamProp1");
                MessageBox.Show(mes, "ERRPR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_manager.SetParameterRange(m_paramList);
            m_saveList = m_win.GetParameterObservedDataList();
            if (m_saveList == null) return;
            m_manager.SetLoggerData(m_saveList);

            m_generation = 0;
            m_isRunning = true;
            m_timer.Enabled = true;
            m_timer.Start();  
        }


        /// <summary>
        /// Stop the robust analysis.
        /// </summary>
        public void StopAnalysis()
        {
            m_manager.StopRunningJobs();
            m_isRunning = false;
            m_control.StopParameterEstimation();
        }

        /// <summary>
        /// Update the status of session at intervals while program is running.
        /// </summary>
        /// <param name="sender">Timer.</param>
        /// <param name="e">EventArgs.</param>
        void FireTimer(object sender, EventArgs e)
        {
            String tmpDir = m_manager.TmpRootDir;
            if (!m_manager.IsFinished())
            {
                if (m_isRunning == false)
                {
                    m_manager.StopRunningJobs();
                    m_timer.Enabled = false;
                    m_timer.Stop();
                }
                return;
            }

            if (m_generation >= m_param.Generation)
            {
                m_isRunning = false;
                m_timer.Enabled = false;
                m_timer.Stop();
                Control.StopParameterEstimation();

                FindElite();
                String finMes = Analysis.s_resources.GetString("FinishRAnalysis");
                MessageBox.Show(finMes, "Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            m_timer.Enabled = false;
            m_timer.Stop();
            if (m_generation == 0)
            {
                m_execParamList = m_manager.RunSimParameterRange(tmpDir, m_model, m_param.Population, m_param.SimulationTime, false);
            }
            else
            {
                FindElite();
                SimplexCrossOver();
                Mutate();
                m_execParamList = m_manager.RunSimParameterSet(tmpDir, m_model, m_param.SimulationTime, false, m_execParamList);
            }

            m_generation++;
            m_timer.Enabled = true;
            m_timer.Start();

        }

        /// <summary>
        /// Find the elite sample in this generation.
        /// </summary>
        private void FindElite()
        {
            int elite = -1;
            double value = 0.0;
            foreach (int jobid in m_execParamList.Keys)
            {
                double tmp = Judgement(jobid);
                if (elite == -1)
                {
                    elite = jobid;
                    value = tmp;
                    continue;
                }
                if (m_param.Type == EstimationFormulatorType.Max ||
                    m_param.Type == EstimationFormulatorType.SumMax)
                {
                    if (value < tmp)
                    {
                        value = tmp;
                        elite = jobid;
                    }
                }
                else if (m_param.Type == EstimationFormulatorType.Min ||
                    m_param.Type == EstimationFormulatorType.SumMin)
                {
                    if (value > tmp)
                    {
                        value = tmp;
                        elite = jobid;
                    }
                }
                else if (m_param.Type == EstimationFormulatorType.EqualZero ||
                        m_param.Type == EstimationFormulatorType.SumEqualZero)
                {
                    if (value > Math.Abs(tmp))
                    {
                        value = Math.Abs(tmp);
                        elite = jobid;
                    }
                }
            }
            m_estimation.Add(m_generation, value);
            m_elite = m_execParamList[elite];
            m_win.AddEstimateParameter(m_elite, value, m_generation);
            m_eliteNum = elite;
        }

        /// <summary>
        /// Execute the simplex crossover in GA.
        /// </summary>
        private void SimplexCrossOver()
        {
            int M = m_param.Param.M;
            double upsilon = m_param.Param.Upsilon;
            Dictionary<int, ExecuteParameter> crossList = new Dictionary<int, ExecuteParameter>();
            crossList.Add(m_eliteNum, m_execParamList[m_eliteNum]);
            // ---------------------------------------------------------------------
            // [1] Choose m parents Pk (i=1,2,...,m) according to the generational 
            //     model used and calculate their center of gravity G, see (5).
            // ---------------------------------------------------------------------
            int paramCount = m_execParamList[m_eliteNum].ParamDic.Count;
            double[] aG = new double[paramCount];
            List<ExecuteParameter> aParentList = new List<ExecuteParameter>();
            for (int i = 0; i < M; i++)
            {
                int r = hRandom.Next(m_param.Population - 1);
                int m = 0;
                foreach (int n in m_execParamList.Keys)
                {
                    if (m == r)
                    {
                        aParentList.Add(m_execParamList[n]);
                        m = 0;
                        foreach (string key in m_execParamList[n].ParamDic.Keys)
                        {
                            aG[m] += m_execParamList[n].ParamDic[key];
                            m++;
                        }
                        break;
                    }
                    m++;
                }
            }
            for (int i = 0; i < paramCount; i++)
            {
                aG[i] = aG[i] / (double)M;
            }


            // ---------------------------------------------------------------------
            // [2] Generate random number rk, see (6)
            // ---------------------------------------------------------------------
            double[] aR = new double[M - 1];
            for (int i = 0; i < M - 1; i++)
            {
                aR[i] = Math.Pow(hRandom.NextDouble(), 1.0 / (double)(i + 1));
            }


            // ---------------------------------------------------------------------
            // [3] Calculate xk, see (7)
            // ---------------------------------------------------------------------
            double[,] aX = new double[M, paramCount];
            for (int i = 0; i < M; i++)
            {
                int k = 0;
                foreach (string key in aParentList[i].ParamDic.Keys)
                {
                    aX[i, k] = aG[k] + upsilon *
                        (aParentList[i].ParamDic[key] - aG[k]);
                    k++;
                }
            }


            // ---------------------------------------------------------------------
            // [4] Calculate Ck, see (8)
            // ---------------------------------------------------------------------
            double[,] aC = new double[M, paramCount];
            for (int i = 0; i < paramCount; i++)
                aC[0, i] = 0.0;
            for (int i = 1; i < M; i++)
            {
                for (int k = 0; k < paramCount; k++)
                {
                    aC[i, k] = aR[i - 1] *
                        (aX[i - 1, k] - aX[i, k] + aC[i - 1, k]);
                }
            }


            // ---------------------------------------------------------------------
            // [5] Generate an offspring C, see (9)
            // ---------------------------------------------------------------------
            int iPos = 0;
            foreach (int pPos in m_execParamList.Keys)
            {
                if (pPos == m_eliteNum) continue;
                double[] aCk = new double[paramCount];
                for (int k = 0; k < paramCount; k++)
                {
                    aCk[k] = aX[iPos, k] + aC[iPos, k];
                }

                ExecuteParameter p = new ExecuteParameter();
                int pos = 0;
                foreach (string key in m_execParamList[m_eliteNum].ParamDic.Keys)
                {
                    p.ParamDic.Add(key, aCk[pos]);
                    pos++;
                }
                crossList.Add(pPos, p);
                iPos++;
                if (iPos >= M) break;
            }

            iPos = 0;
            m_execParamList.Clear();
            foreach (int key in crossList.Keys)
            {
                if (iPos < key) iPos = key;
                m_execParamList.Add(key, crossList[key]);
            }
            iPos++;
            
            while (m_execParamList.Count < m_param.Population)
            {
                ExecuteParameter p = m_manager.CreateExecuteParameter();
                m_execParamList.Add(iPos, p);
                iPos++;
            }
        }

        private void Mutate()
        {
            Dictionary<int, ExecuteParameter> tmpList = new Dictionary<int, ExecuteParameter>();
            foreach (int j in m_execParamList.Keys)
            {
                if (j == m_eliteNum)
                {
                    tmpList.Add(j, m_execParamList[j]);
                    continue;
                }

                Dictionary<string, double> tmpPramDic = new Dictionary<string, double>();
                ExecuteParameter p = m_execParamList[j];
                foreach (string path in p.ParamDic.Keys)
                {
                    int rand = hRandom.Next(100);
                    if (rand > m_mutation)
                    {
                        tmpPramDic.Add(path, p.ParamDic[path]);
                        continue;
                    }
                    foreach (ParameterRange range in m_paramList)
                    {
                        if (!range.FullPath.Equals(path)) continue;
                        double maxV = range.Max;
                        double minV = range.Min;
                        double V = (maxV - minV) * hRandom.NextDouble() + minV;
//                        m_execParamList[j].ParamDic[path] = V;
                        tmpPramDic.Add(path, V);
                    }
                }
                tmpList.Add(j, new ExecuteParameter(tmpPramDic));
            }
            m_execParamList.Clear();
            foreach (int j in tmpList.Keys)
            {
                m_execParamList.Add(j, tmpList[j]);
            }

            m_mutation = m_mutation * m_param.Param.K;
            if (m_mutation > m_param.Param.Max)
                m_mutation = m_param.Param.Max;
        }


        /// <summary>
        /// Get the estimation value of job.
        /// </summary>
        /// <param name="jobid">the ID of calcuated job.</param>
        /// <returns>the value of job.</returns>
        private double Judgement(int jobid)
        {
            double value;
            String f = m_param.EstimationFormulator;
            List<String> fList = new List<string>();

            if (m_param.Type == EstimationFormulatorType.Max ||
                m_param.Type == EstimationFormulatorType.Min ||
                m_param.Type == EstimationFormulatorType.EqualZero)
            {
                foreach (SaveLoggerProperty p in m_saveList)
                {
                    Dictionary<double, double> logList = 
                        m_manager.SessionList[jobid].GetLogData(p.FullPath);
                    value = 0.0;
                    if (logList.Count <= 0)
                    {
                        return 0.0;
                    }
                    foreach (double v in logList.Values)
                    {
                        value = v;
                    }
                    f = f.Replace(p.FullPath, Convert.ToString(value));   
                }
                Calculator c = new Calculator(f);
                return c.Calculate();
            }

            foreach (SaveLoggerProperty p in m_saveList)
            {
                int i = 0;
                Dictionary<double, double> logList = 
                       m_manager.SessionList[jobid].GetLogData(p.FullPath);
                foreach (double v in logList.Values)
                {
                    if (fList.Count <= i) fList.Add(f);
                    fList[i] = fList[i].Replace(p.FullPath, Convert.ToString(v));
                    i++;
                }
            }
            value = 0.0;
            foreach (String form in fList)
            {
                Calculator c = new Calculator(form);
                value += c.Calculate();
            }
            
            return value;
        }

    }


    /// <summary>
    /// Class to manage the parameter of parameter estimation.
    /// </summary>
    public class ParameterEstimationParameter
    {
        /// <summary>
        /// Formulator to estimate this parameters.
        /// </summary>
        private string m_estimationFormulator;
        /// <summary>
        /// The simulation time.
        /// </summary>
        private double m_simulationTime;
        /// <summary>
        /// The number of population.
        /// </summary>
        private int m_population;
        /// <summary>
        /// The number of generation.
        /// </summary>
        private int m_generation;
        /// <summary>
        /// The type of formulator.
        /// </summary>
        private EstimationFormulatorType m_type;
        /// <summary>
        /// The parameter data of simplex cross over.
        /// </summary>
        private SimplexCrossoverParameter m_param;


        /// <summary>
        /// Constructor.
        /// </summary>
        public ParameterEstimationParameter()
        {
            m_estimationFormulator = "";
            m_simulationTime = 100.0;
            m_population = 30;
            m_generation = 10;
            m_type = EstimationFormulatorType.Max;
            m_param = new SimplexCrossoverParameter();
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="formulator">The formulator to estimate the result.</param>
        /// <param name="simTime">The simulation time.</param>
        /// <param name="populationNum">The number of population.</param>
        /// <param name="generationNum">The number of generation.</param>
        /// <param name="type">The type of fomulator.</param>
        /// <param name="param">The parameter data of simplex cross over.</param>
        public ParameterEstimationParameter(string formulator, double simTime, int populationNum, 
                int generationNum, EstimationFormulatorType type, SimplexCrossoverParameter param)
        {
            m_estimationFormulator = formulator;
            m_simulationTime = simTime;
            m_population = populationNum;
            m_generation = generationNum;
            m_type = type;
            m_param = param;
        }

        /// <summary>
        /// get / set the formulator to estimate the parameters.
        /// </summary>
        public string EstimationFormulator
        {
            get { return this.m_estimationFormulator; }
            set { this.m_estimationFormulator = value; }
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
        /// get / set the number of populations.
        /// </summary>
        public int Population
        {
            get { return m_population; }
            set { m_population = value; }
        }

        /// <summary>
        /// get / set the number of generations.
        /// </summary>
        public int Generation
        {
            get { return m_generation; }
            set { m_generation = value; }
        }

        /// <summary>
        /// get / set the parameter of simlex cross over.
        /// </summary>
        public SimplexCrossoverParameter Param
        {
            get { return m_param; }
            set { m_param = value; }
        }

        /// <summary>
        /// get / set the estimation type of fomulator.
        /// </summary>
        public EstimationFormulatorType Type
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }
    }
}
