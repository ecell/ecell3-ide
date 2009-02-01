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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

using Ecell.Logging;
using Ecell.Job;
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Class to manage the sensitivity analysis.
    /// </summary>
    class SensitivityAnalysis
    {
        #region Fields
        /// <summary>
        /// Timer to update the status of jobs.
        /// </summary>
        private System.Windows.Forms.Timer m_timer;
        /// <summary>
        /// Plugin controller.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// The flag whether the analysis is running.
        /// </summary>
        private bool m_isRunning = false;
        /// <summary>
        /// The parameter for sensitivity analysis.
        /// </summary>
        private SensitivityAnalysisParameter m_param;
        /// <summary>
        /// The current model.
        /// </summary>
        private string m_model;
        /// <summary>
        /// The list of property value when the step is executed without changing the property.
        /// </summary>
        private Dictionary<string, double> m_currentData;
        /// <summary>
        /// The list of changed the property.
        /// </summary>
        private Dictionary<string, double> m_pertubateData;
        /// <summary>
        /// The list of property to get the property value after the step is executed.
        /// </summary>
        private List<SaveLoggerProperty> m_saveList;
        /// <summary>
        /// The list of job and execute parameters.
        /// </summary>
        private Dictionary<int, ExecuteParameter> m_execParam;
        /// <summary>
        /// The number of variable.
        /// </summary>
        private int m_vNum;
        /// <summary>
        /// The number of process.
        /// </summary>
        private int m_pNum;
        /// <summary>
        /// Stoichiometry Matrix(temporary data).
        /// </summary>
        private Matrix m_sMatrix;
        /// <summary>
        /// Elasticity Matrix(temporary data).
        /// </summary>
        private Matrix m_eMatrix;
        /// <summary>
        /// Kernel Matrix(temporary data).
        /// </summary>
        private Matrix m_kernelMatrix;
        /// <summary>
        /// Linkage Matrix(temporary data).
        /// </summary>
        private Matrix m_linkMatrix;
        /// <summary>
        /// CCC Matrix of unscaled data.
        /// </summary>
        private Matrix m_unscaledCCCMatrix;
        /// <summary>
        /// FCC Matrix of unscaled data.
        /// </summary>
        private Matrix m_unscaledFCCMatrix;
        /// <summary>
        /// CCC Matrix of scaled data.
        /// </summary>
        private Matrix m_scaledCCCMatrix;
        /// <summary>
        /// FCC Matrix of scaled data.
        /// </summary>
        private Matrix m_scaledFCCMatrix;
        /// <summary>
        /// The list of independ data.
        /// </summary>
        private List<int> m_independList = new List<int>();
        /// <summary>
        /// The list of value data of Variable after one step.
        /// </summary>
        private List<double> m_valueBuffer = new List<double>();
        /// <summary>
        /// The list of activity data of Process after one step.
        /// </summary>
        private List<double> m_activityBuffer = new List<double>();
        /// <summary>
        /// The list of entity path of value data.
        /// </summary>
        private List<string> m_valueList = new List<string>();
        /// <summary>
        /// The list of entity path of activity data.
        /// </summary>
        private List<string> m_activityList = new List<string>();
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public SensitivityAnalysis(Analysis owner)
        {
            m_owner = owner;

            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Interval = 5000;
            m_timer.Tick += new EventHandler(FireTimer);

            m_currentData = new Dictionary<string, double>();
            m_pertubateData = new Dictionary<string, double>();
            m_execParam = new Dictionary<int, ExecuteParameter>();
            m_saveList = new List<SaveLoggerProperty>();
        }

        #region Accessors
        /// <summary>
        /// get / set the flag whether the sensitivity analysis is running.
        /// </summary>
        public bool IsRunning
        {
            get { return this.m_isRunning; }
        }
        #endregion

        /// <summary>
        /// Execute the sensitivity analysis.
        /// </summary>
        public void ExecuteAnalysis()
        {
            DataManager dManager = m_owner.DataManager;
            m_owner.ClearResult();
            m_param = m_owner.GetSensitivityAnalysisParameter();
            m_model = "";
            List<string> modelList = m_owner.DataManager.GetModelList();
            if (modelList.Count > 0) m_model = modelList[0];

            if (m_param.Step <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                    new object[] { MessageResources.NameStepNum, 0 }));
                m_owner.FinishedAnalysisByError();
                return;
            }
            if (m_param.AbsolutePerturbation <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                    new object[] { MessageResources.NameAbsolutePert, 0.0 }));
                m_owner.FinishedAnalysisByError();
                return;
            }
            if (m_param.RelativePerturbation <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                   new object[] { MessageResources.NameRelativePert, 0.0 }));
                m_owner.FinishedAnalysisByError();
                return;
            }

            m_isRunning = true;
            Dictionary<EcellObject, int> varList = GetVariableList(m_model);
            Dictionary<EcellObject, int> proList = GetProcessList(m_model);
            m_vNum = varList.Count;
            m_pNum = proList.Count;
            CreateStoichiomatryMatrix(varList, proList);

            m_currentData.Clear();
            m_pertubateData.Clear();
            m_saveList.Clear();

            CreateExecuteParameter(varList, proList);

            if (m_isRunning)
            {
                m_timer.Enabled = true;
                m_timer.Start();
            }
        }

        /// <summary>
        /// Create the execute parameters and get the simulation data at one step.
        /// </summary>
        /// <param name="varList">the variable list.</param>
        /// <param name="proList">the process list.</param>
        private void CreateExecuteParameter(Dictionary<EcellObject, int> varList,
            Dictionary<EcellObject, int> proList)
        {
            DataManager dManager = m_owner.DataManager;
            string tmpDir = m_owner.JobManager.TmpDir;
            double start = 0.0;
            double end = 0.0;
            int jobid = 0;
            List<string> valueList = new List<string>();

            dManager.Initialize(true);
            Dictionary<int, ExecuteParameter> execDict = new Dictionary<int, ExecuteParameter>();
            foreach (EcellObject obj in varList.Keys)
            {
                foreach (EcellData data in obj.Value)
                {
                    if (data.EntityPath.EndsWith(":Value"))
                    {
                        double dV = dManager.GetPropertyValue(data.EntityPath);

                        double pert = dV * m_param.RelativePerturbation + m_param.AbsolutePerturbation;
                        m_pertubateData.Add(data.EntityPath, pert);

                        dV = dV + pert;
                        Dictionary<string, double> tmpDic = new Dictionary<string, double>();
                        tmpDic.Add(data.EntityPath, dV);
                        ExecuteParameter param = new ExecuteParameter(tmpDic);
                        execDict.Add(jobid, param);
                        valueList.Add(data.EntityPath);
                        jobid++;
                    }
                }
            }

            foreach (EcellObject obj in proList.Keys)
            {
                foreach (EcellData data in obj.Value)
                {
                    if (data.EntityPath.EndsWith(":Activity"))
                    {
                        m_saveList.Add(new SaveLoggerProperty(data.EntityPath, start, end, tmpDir));
                    }
                }
            }

            m_activityList.Clear();
            m_valueList.Clear();
            m_valueBuffer.Clear();
            m_activityBuffer.Clear();
//            dManager.SimulationStartKeepSetting(m_param.Step);
            dManager.StartStepSimulation(m_param.Step);
            foreach (SaveLoggerProperty p in m_saveList)
            {
                double dV = dManager.GetPropertyValue(p.FullPath);
                m_currentData.Add(p.FullPath, dV);
                m_activityBuffer.Add(dV);
                m_activityList.Add(p.FullPath);
            }
            foreach (string p in valueList)
            {
                double dV = dManager.GetPropertyValue(p);
                m_valueBuffer.Add(dV);
                m_valueList.Add(p);
            }
            double cTime = dManager.GetCurrentSimulationTime();
            dManager.SimulationStop();

            m_owner.JobManager.SetLoggerData(m_saveList);
            m_execParam = m_owner.JobManager.RunSimParameterSet(tmpDir, m_model, cTime, false, execDict);
        }

        /// <summary>
        /// Stop the sensitivity analysis.
        /// </summary>
        public void StopAnalysis()
        {
            m_owner.JobManager.StopRunningJobs();
            m_isRunning = false;
            m_owner.StopSensitivityAnalysis();
        }

        /// <summary>
        /// Get the process list in this model.
        /// </summary>
        /// <param name="model">the model name.</param>
        /// <returns>dictionaty the process and position.</returns>
        private Dictionary<EcellObject, int> GetProcessList(string model)
        {
            int i = 0;
            Dictionary<EcellObject, int> resList = new Dictionary<EcellObject, int>();
            DataManager dManager = m_owner.DataManager;
            List<EcellObject> objList = dManager.GetData(model, null);
            foreach (EcellObject sObj in objList)
            {
                if (!(sObj is EcellSystem)) continue;
                if (sObj.Children == null) continue;

                foreach (EcellObject obj in sObj.Children)
                {
                    if (!(obj is EcellProcess)) continue;
                    resList.Add(obj, i);
                    i++;
                }
            }
            return resList;
        }

        /// <summary>
        /// Get the variable list in this model.
        /// </summary>
        /// <param name="model">the model name.</param>
        /// <returns>dictionaty the variable and position.</returns>
        private Dictionary<EcellObject, int> GetVariableList(string model)
        {
            int i = 0;
            Dictionary<EcellObject, int> resList = new Dictionary<EcellObject, int>();
            DataManager dManager = m_owner.DataManager;
            List<EcellObject> objList = dManager.GetData(model, null);
            foreach (EcellObject sObj in objList)
            {
                if (!(sObj is EcellSystem)) continue;
                if (sObj.Children == null) continue;

                foreach (EcellObject obj in sObj.Children)
                {
                    if (!(obj is EcellVariable)) continue;
                    if (obj.Key.EndsWith(":SIZE")) continue;
                    resList.Add(obj, i);
                    i++;
                }
            }
            return resList;
        }


        #region Analysis
        /// <summary>
        /// Create the epsilon elasticty matrix.
        /// </summary>
        private void CreateEpsilonElastictyMatrix()
        {
            double[,] eMatrix = new double[m_vNum, m_pNum];
            List<string> headerList = new List<string>();
            foreach (SaveLoggerProperty p in m_saveList)
            {
                headerList.Add(p.FullPath);
            }
            int i = 0;

            foreach (int jobid in m_execParam.Keys)
            {
                List<double> resList = new List<double>();
                string paramName = "";
                foreach (string key in m_execParam[jobid].ParamDic.Keys)
                {
                    paramName = key;
                    break;
                }
                int j = 0;
                foreach (string path in headerList)
                {
                    Dictionary<double, double> logList =
                        m_owner.JobManager.JobList[jobid].GetLogData(path);
                    double value = 0.0;
                    foreach (double t in logList.Keys)
                        value = logList[t];
                    double value1 = (value - m_currentData[path]) / m_pertubateData[paramName];
                    eMatrix[i, j] = value1;
//                    Console.WriteLine(i + " : " + j + " === " + value + " ==> " + m_currentData[path]);

                    j++;
                }
                i++;
            }
            m_eMatrix = Matrix.Create(eMatrix);
        }

        /// <summary>
        /// Create the generate full rank matrix.
        /// </summary>
        private void GenerateFullRankMatrix()
        {
            Matrix rMatrix = m_sMatrix.Clone();
            Matrix pMatrix = Identity(m_vNum);

            List<int> dependList = IntRange(m_vNum);
            List<int> tmpSkippedList = new List<int>();
            List<int> skippedList = new List<int>();
            m_independList.Clear();

            for (int j = 0; j < m_pNum; j++)
            {
                if (dependList.Count <= 0) break;
                int maxIndex = dependList[0];
                double maxElement = rMatrix[maxIndex, j];

                foreach (int i in dependList)
                {
                    if (Math.Abs(rMatrix[i, j]) > Math.Abs(maxElement))
                    {
                        maxIndex = i;
                        maxElement = rMatrix[i, j];
                    }
                }

                if (Math.Abs(maxElement) == 0.0)
                {
                    tmpSkippedList.Add(j);
                    continue;
                }

                for (int k = 0; k < m_pNum; k++)
                    rMatrix[maxIndex, k] *= 1.0 / maxElement;
                for (int k = 0; k < m_vNum; k++)
                    pMatrix[maxIndex, k] *= 1.0 / maxElement;

                for (int i = 0; i < m_vNum; i++)
                {
                    if (i == maxIndex) continue;
                    double t = rMatrix[i, j];
                    for (int k = 0; k < m_pNum; k++)
                        rMatrix[i, k] -= t * rMatrix[maxIndex, k];
                    for (int k = 0; k < m_vNum; k++)
                        pMatrix[i, k] -= t * pMatrix[maxIndex, k];
                }
                if (tmpSkippedList.Count > 0)
                {
                    foreach (int k in tmpSkippedList)
                        skippedList.Add(k);
                    tmpSkippedList.Clear();
                }

                dependList.Remove(maxIndex);
                m_independList.Add(maxIndex);
            }

            Matrix tmplinkMatrix = Identity(m_vNum);
            foreach (int i in dependList)
            {
                for (int j = 0; j < m_vNum; j++)
                {
                    tmplinkMatrix[i, j] -= pMatrix[i, j];
                }
            }

            int rank = m_independList.Count;
            m_kernelMatrix = new Matrix(m_pNum, m_pNum - rank);
            int parsedRank = rank + skippedList.Count;
            List<int> extractList = new List<int>();
            for (int i = parsedRank; i < m_pNum; i++)
            {
                extractList.Add(i);
            }
            foreach (int i in skippedList)
            {
                if (extractList.Contains(i)) continue;
                extractList.Add(i);
            }
            Matrix redMatrix = PyTake(rMatrix, extractList, false);

            int cnt1 = 0, cnt2 = 0;
            for (int i = 0; i < parsedRank; i++)
            {
                if (skippedList.Count > cnt1 && skippedList[cnt1] == i)
                {
                    m_kernelMatrix[i, m_pNum - parsedRank + cnt1] = 1.0;
                    cnt1++;
                }
                else
                {
                    for (int j = 0; j < m_pNum - rank; j++)
                    {
                        m_kernelMatrix[i, j] = -1.0 * redMatrix[m_independList[cnt2], j];
                    }
                    cnt2++;
                }
            }

            for (int i = 0; i < m_pNum - parsedRank; i++)
            {
                m_kernelMatrix[i + parsedRank, i] = 1.0;
            }
            m_independList.Sort();
            m_linkMatrix = PyTake(tmplinkMatrix, m_independList, false);            
        }

        /// <summary>
        /// Calculate the unscaled control coefficient.
        /// </summary>
        private void CalculateUnScaledControlCoefficient()
        {
            Matrix eMatrix = m_eMatrix.Clone();
            eMatrix.Transpose();
            GenerateFullRankMatrix();
            Matrix reduceMatrix = PyTake(m_sMatrix, m_independList, true);

            Matrix epsilonMatrix = reduceMatrix * eMatrix;
            Matrix jocobianMatrix = epsilonMatrix * m_linkMatrix;
            if (jocobianMatrix.Determinant() == 0.0)
            {
                String errmes = String.Format(MessageResources.ErrExecute,
                    new object[] { MessageResources.NameSensAnalysis });
                Util.ShowErrorDialog(errmes);
                throw new IgnoreException(errmes);
            }
            Matrix invJacobian = jocobianMatrix.Inverse();

            m_unscaledCCCMatrix = -1.0 * m_linkMatrix * invJacobian;
            m_unscaledCCCMatrix = m_unscaledCCCMatrix * reduceMatrix;

            m_unscaledFCCMatrix = Identity(m_pNum) + eMatrix * m_unscaledCCCMatrix;
        }

        /// <summary>
        /// Calculate the scaled control coefficient.
        /// </summary>
        private void CalculateScaledControlCoefficient()
        {
            m_scaledCCCMatrix = InvDiag(m_valueBuffer) * m_unscaledCCCMatrix;
            m_scaledCCCMatrix = m_scaledCCCMatrix * Diag(m_activityBuffer);

            m_scaledFCCMatrix = InvDiag(m_activityBuffer) * m_unscaledFCCMatrix;
            m_scaledFCCMatrix = m_scaledFCCMatrix * Diag(m_activityBuffer);
        }

        /// <summary>
        /// Create the stoichiomatry matrix.
        /// </summary>
        /// <param name="varList">the list of value data of variable.</param>
        /// <param name="proList">the list of activity data of process.</param>
        private void CreateStoichiomatryMatrix(Dictionary<EcellObject, int> varList,
                                               Dictionary<EcellObject, int> proList)
        {
            int proNum = proList.Count;
            int varNum = varList.Count;

            double[,] res = new double[varNum,proNum];

            foreach (EcellObject obj in proList.Keys)
            {
                int pIndex = proList[obj];
                foreach (EcellData d in obj.Value)
                {
                    if (!d.EntityPath.EndsWith(Constants.xpathVRL))
                        continue;
                    List<EcellReference> refList = EcellReference.ConvertFromEcellValue(d.Value);
                    foreach (EcellReference r in refList)
                    {
                        int vIndex = -1;
                        foreach (EcellObject vObj in varList.Keys)
                        {
                            if (r.FullID.EndsWith(vObj.Key))
                            {
                                vIndex = varList[vObj];
                                break;
                            }
                        }
                        if (vIndex == -1)
                        {
                            Console.WriteLine("Not Fount : " + r.FullID);
                            continue;
                        }
                        res[vIndex,pIndex] += r.Coefficient;
                    }
                }
            }

            m_sMatrix =  Matrix.Create(res);
        }
        #endregion

        #region Matrix
        /// <summary>
        /// Returns the identity 2-d array of shape n x n.
        /// </summary>
        /// <param name="n">the number of matrix.</param>
        /// <returns>Identity matrix.</returns>
        private Matrix Identity(int n)
        {
            return Matrix.Identity(n, n);
        }

        /// <summary>
        /// Returns the list of sequece values.
        /// </summary>
        /// <param name="n">the number of sequece values.</param>
        /// <returns>the list of sequence values.</returns>
        private List<int> IntRange(int n)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < n; i++)
            {
                res.Add(i);
            }
            return res;
        }

        /// <summary>
        /// Returns the matrix that extract the column data by using the list of column from original matrix.
        /// </summary>
        /// <param name="org">the original matrix.</param>
        /// <param name="extractList">the list of extracted column.</param>
        /// <param name="isRow">the flag whether row is extracted.</param>
        /// <returns>The extracted matrix.</returns>
        private Matrix PyTake(Matrix org, List<int> extractList, bool isRow)
        {
            Matrix result;
            if (isRow)
            {
                result = new Matrix(extractList.Count, org.ColumnCount);
                for (int i = 0, k = 0; i < org.RowCount; i++)
                {
                    if (!extractList.Contains(i)) continue;
                    for (int j = 0; j < org.ColumnCount; j++)
                    {
                        result[k, j] = org[i, j];
                    }
                    k++;
                }
            }
            else
            {
                result = new Matrix(org.RowCount, extractList.Count);
                for (int i = 0, k = 0; i < org.ColumnCount; i++)
                {
                    if (!extractList.Contains(i)) continue;
                    for (int j = 0; j < org.RowCount; j++)
                    {
                        result[j, k] = org[j, i];
                    }
                    k++;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns matrix that inverse and copy the diagonal from the list of data.
        /// </summary>
        /// <param name="data">the list of data.</param>
        /// <returns>The convert matrix.</returns>
        private Matrix InvDiag(List<double> data)
        {
            int size = data.Count;
            Matrix m = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                if (Math.Abs(data[i]) > 0.0)
                {
                    m[i, i] = 1.0 / data[i];
                }
            }
            return m;
        }

        /// <summary>
        /// Returns a copy of the the k-th diagonal if v is a 2-d array.
        /// </summary>
        /// <param name="data">the list of data.</param>
        /// <returns>The convert matrix.</returns>
        private Matrix Diag(List<double> data)
        {
            int size = data.Count;
            Matrix m = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                m[i, i] = data[i];
            }
            return m;
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
            try
            {
                if (!m_isRunning)
                {
                    m_timer.Enabled = false;
                    m_timer.Stop();
                    return;
                }

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
                m_isRunning = false;
                m_timer.Enabled = false;
                m_timer.Stop();
                m_owner.StopSensitivityAnalysis();

                CreateEpsilonElastictyMatrix();
                CalculateUnScaledControlCoefficient();
                CalculateScaledControlCoefficient();

                m_owner.SetSensitivityHeader(m_activityList);
                for (int i = 0; i < m_scaledCCCMatrix.RowCount; i++)
                {
                    List<double> res = new List<double>();
                    for (int j = 0; j < m_scaledCCCMatrix.ColumnCount; j++)
                    {
                        res.Add(m_scaledCCCMatrix[i, j]);
                    }
                    m_owner.AddSensitivityDataOfCCC(m_valueList[i], res);
                }
                for (int i = 0; i < m_scaledFCCMatrix.RowCount; i++)
                {
                    List<double> res = new List<double>();
                    for (int j = 0; j < m_scaledFCCMatrix.ColumnCount; j++)
                    {
                        res.Add(m_scaledFCCMatrix[i, j]);
                    }
                    m_owner.AddSensitivityDataOfFCC(m_activityList[i], res);
                }
                m_owner.UpdateResultColor();

                Util.ShowNoticeDialog(String.Format(MessageResources.InfoFinishExecute,
                    new object[] { MessageResources.NameSensAnalysis }));
                m_owner.ActivateResultWindow();
                m_owner.FinishedAnalysis();
            }
            catch (IgnoreException ex)
            {
                Trace.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrExecute,
                    new object[] { MessageResources.NameSensAnalysis }));

                m_owner.Environment.LogManager.Append(
                        new ApplicationLogEntry(MessageType.Error, ex.ToString(), this));
            }
        }
        #endregion
    }

    /// <summary>
    /// Class to manage the parameter for sensitivity analysis.
    /// </summary>
    public class SensitivityAnalysisParameter
    {
        /// <summary>
        /// The number of step.
        /// </summary>
        private int m_step;
        /// <summary>
        /// The relative perturbation.
        /// </summary>
        private double m_relativePert;
        /// <summary>
        /// The absolute perturbation.
        /// </summary>
        private double m_absolutePert;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SensitivityAnalysisParameter()
        {
            m_step = 1;
            m_relativePert = 1E-6;
            m_absolutePert = 1E-6;
        }

        /// <summary>
        /// Constructor with the initial parameter.
        /// </summary>
        /// <param name="step">The initial data of the number of step</param>
        /// <param name="relativePert">The initial data of the relative perturbation.</param>
        /// <param name="absolutePert">The initial data of the absolute perturbation.</param>
        public SensitivityAnalysisParameter(int step, double relativePert, double absolutePert)
        {
            m_step = step;
            m_relativePert = relativePert;
            m_absolutePert = absolutePert;
        }

        /// <summary>
        /// get / set the number of step.
        /// </summary>
        public int Step
        {
            get { return this.m_step; }
            set { this.m_step = value; }
        }

        /// <summary>
        /// get / set the relative perturbation.
        /// </summary>
        public double RelativePerturbation
        {
            get { return this.m_relativePert; }
            set { this.m_relativePert = value; }
        }

        /// <summary>
        /// get / set the absolute perturbation.
        /// </summary>
        public double AbsolutePerturbation
        {
            get { return this.m_absolutePert; }
            set { this.m_absolutePert = value; }
        }
    }
}
