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
using System.IO;
using System.Windows.Forms;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

using Ecell.Logging;
using Ecell.Job;
using Ecell.Objects;
using Ecell.Exceptions;

using Ecell.IDE.Plugins.Analysis.AnalysisFile;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Class to manage the sensitivity analysis.
    /// </summary>
    public class SensitivityAnalysis : IAnalysisModule
    {
        #region Fields
        /// <summary>
        /// Plugin controller.
        /// </summary>
        private Analysis m_owner;
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
        /// <summary>
        /// Job group related with this analysis.
        /// </summary>
        private JobGroup m_group;
        /// <summary>
        /// The flag whether this analysis have any result.
        /// </summary>
        private bool m_isExistResult = false;
        private const string s_step = "Step";
        private const string s_relativePert = "Relative Perturbation";
        private const string s_absolutePert = "Absolute Perturbation";
        /// <summary>
        /// Analysis name.
        /// </summary>
        public const string s_analysisName = "SensitivityAnalysis";
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public SensitivityAnalysis(Analysis owner)
        {
            m_owner = owner;
            m_param = new SensitivityAnalysisParameter();

            m_currentData = new Dictionary<string, double>();
            m_pertubateData = new Dictionary<string, double>();
            m_execParam = new Dictionary<int, ExecuteParameter>();
            m_saveList = new List<SaveLoggerProperty>();
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
                SensitivityAnalysisParameter p = value as SensitivityAnalysisParameter;
                if (p != null)
                    m_param = p;
            }
        }

        /// <summary>
        /// get / set the parameter list.
        /// </summary>
        public List<EcellParameterData> ParameterDataList
        {
            get { return new List<EcellParameterData>(); }
            set { }
        }

        /// <summary>
        /// get / set the observed list.
        /// </summary>
        public List<EcellObservedData> ObservedDataList
        {
            get { return new List<EcellObservedData>(); }
            set { }
        }

        /// <summary>
        /// get the flag this analysis is enable to judge.
        /// </summary>
        public bool IsEnableReJudge
        {
            get { return false; }
        }


        /// <summary>
        /// get the flag this analysis is step execution.
        /// </summary>
        public bool IsStep
        {
            get { return true; }
        }

        /// <summary>
        /// get the simulation time or the step count.
        /// </summary>
        public double Count
        {
            get { return m_param.Step; }
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
        /// <returns>Return the dictionary of analysis parameters.</returns>
        public Dictionary<string, string> GetAnalysisProperty()
        {
            Dictionary<string, string> paramDic = new Dictionary<string, string>();

            paramDic.Add(s_step, m_param.Step.ToString());
            paramDic.Add(s_relativePert, m_param.RelativePerturbation.ToString());
            paramDic.Add(s_absolutePert, m_param.AbsolutePerturbation.ToString());

            return paramDic;
        }

        /// <summary>
        /// Set the property of analysis.
        /// </summary>
        /// <param name="paramDic">the dictionary of parameters.</param>
        public void SetAnalysisProperty(Dictionary<string, string> paramDic)
        {
            foreach (string key in paramDic.Keys)
            {
                switch (key)
                {
                    case s_step:
                        m_param.Step = Int32.Parse(paramDic[key]);
                        break;
                    case s_relativePert:
                        m_param.RelativePerturbation = Double.Parse(paramDic[key]);
                        break;
                    case s_absolutePert:
                        m_param.AbsolutePerturbation = Double.Parse(paramDic[key]);
                        break;
                }
            }
        }

        /// <summary>
        /// Execute this function when this analysis is finished.
        /// </summary>
        public void NotifyAnalysisFinished()
        {
            try
            {
                CreateEpsilonElastictyMatrix();
                CalculateUnScaledControlCoefficient();
                CalculateScaledControlCoefficient();
            }
            catch (Exception)
            {
                if (m_group.Status != AnalysisStatus.Stopped)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrExecute, MessageResources.NameSensAnalysis));
                    m_group.IsGroupError = true;
                }
                return;
            }
        }

        /// <summary>
        /// Create the analysis instance.
        /// </summary>
        /// <param name="group">the job group.</param>
        /// <returns>Return new SensitivityAnalysis object.</returns>
        public IAnalysisModule CreateNewInstance(JobGroup group)
        {
            SensitivityAnalysis instance = new SensitivityAnalysis(m_owner);
            instance.Group = group;

            return instance;
        }

        /// <summary>
        /// Execute the sensitivity analysis.
        /// </summary>
        public void ExecuteAnalysis()
        {
            DataManager dManager = m_owner.DataManager;
            m_owner.ClearResult();
            m_model = "";
            List<string> modelList = m_owner.DataManager.GetModelList();
            if (modelList.Count > 0) m_model = modelList[0];

            if (m_param.Step <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                    new object[] { MessageResources.NameStepNum, 0 }));
                m_group.IsGroupError = true;
                return;
            }
            if (m_param.AbsolutePerturbation <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                    new object[] { MessageResources.NameAbsolutePert, 0.0 }));
                m_group.IsGroupError = true;
                return;
            }
            if (m_param.RelativePerturbation <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger,
                   new object[] { MessageResources.NameRelativePert, 0.0 }));
                m_group.IsGroupError = true;
                return;
            }

            Dictionary<EcellObject, int> varList = GetVariableList(m_model);
            Dictionary<EcellObject, int> proList = GetProcessList(m_model);
            m_vNum = varList.Count;
            m_pNum = proList.Count;
            CreateStoichiomatryMatrix(varList, proList);

            m_currentData.Clear();
            m_pertubateData.Clear();
            m_saveList.Clear();

            try
            {
                CreateExecuteParameter(varList, proList);
            }
            catch (Exception)
            {
                if (m_group.Status != AnalysisStatus.Stopped)
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrExecute, MessageResources.NameSensAnalysis));
                return;
            }
        }

        /// <summary>
        /// Judgement.
        /// </summary>
        public void Judgement()
        {
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
            int ccolcount = Int32.Parse(labels[1]);
            int crowcount = Int32.Parse(labels[2]);
            int fcolcount = Int32.Parse(labels[4]);
            int frowcount = Int32.Parse(labels[5]);
            double[,] cmatrix = new double[crowcount, ccolcount];
            double[,] fmatrix = new double[frowcount, fcolcount];
            LoadAnalysisResultFile(resultFile, cmatrix, fmatrix);

            // Load the parameter file.
            SensitivityAnalysisParameterFile f = new SensitivityAnalysisParameterFile(this, paramFile);
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
            list.Add("CCC");
            list.Add(m_activityList.Count.ToString());
            list.Add(m_valueList.Count.ToString());
            list.Add("FCC");
            list.Add(m_activityList.Count.ToString());
            list.Add(m_activityList.Count.ToString());
            AnalysisResultMetaFile.CreateTableMetaFile(metaFile, s_analysisName, list);

            // Save the result file.
            SaveAnalysisResultFile(resultFile);

            // Save the parameter file.
            SensitivityAnalysisParameterFile f = new SensitivityAnalysisParameterFile(this, paramFile);
            f.Parameter = m_param;
            f.Write();
        }

        /// <summary>
        /// Load the analysis result file.
        /// </summary>
        /// <param name="resultFile">the result file.</param>
        /// <param name="cmatrix">CCC matrix.</param>
        /// <param name="fmatrix">FCC matrix.</param>
        private void LoadAnalysisResultFile(string resultFile, double[,] cmatrix, double[,] fmatrix)
        {
            StreamReader reader;
            reader = new StreamReader(resultFile, Encoding.ASCII);
            bool isFirst = true;
            int readPos = 1;
            string line;
            string[] ele;
            int i, j;
            j = 0;
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
                        m_activityList = headList;
                        isFirst = false;
                        j = 0;
                        continue;
                    }
                    ele = line.Split(new char[] { ',' });
                    for (i = 1; i < ele.Length; i++)
                    {
                        if (String.IsNullOrEmpty(ele[i])) continue;
                        cmatrix[j, i - 1] = Convert.ToDouble(ele[i]);
                    }
                    m_valueList.Add(ele[0]);
                    j++;
                }
                else if (readPos == 2)
                {
                    if (isFirst)
                    {
                        j = 0;
                        isFirst = false;
                        continue;
                    }
                    List<double> valList = new List<double>();
                    ele = line.Split(new char[] { ',' });
                    for (i = 1; i < ele.Length; i++)
                    {
                        if (String.IsNullOrEmpty(ele[i])) continue;
                        fmatrix[j, i - 1] = Convert.ToDouble(ele[i]);
                    }
                    j++;
                }
            }
            m_scaledCCCMatrix = Matrix.Create(cmatrix);
            m_scaledFCCMatrix = Matrix.Create(fmatrix);
            m_isExistResult = true;

            reader.Close();
        }

        /// <summary>
        /// Save the analysis result file.
        /// </summary>
        /// <param name="resultFile">the result file.</param>
        private void SaveAnalysisResultFile(string resultFile)
        {
            StreamWriter writer = new StreamWriter(resultFile, false, Encoding.ASCII);
            writer.Write("Item");
            foreach (string name in m_activityList)
            {
                writer.Write("," + name);
            }
            writer.WriteLine("");

            for (int i = 0; i < m_scaledCCCMatrix.RowCount; i++)
            {
                writer.Write(m_valueList[i]);
                for (int j = 0; j < m_scaledCCCMatrix.ColumnCount; j++)
                {
                    writer.Write(m_scaledCCCMatrix[i, j] + ",");
                }
                writer.WriteLine("");
            }
            writer.WriteLine("");

            writer.Write("Item");
            foreach (string name in m_activityList)
            {
                writer.Write("," + name);
            }
            writer.WriteLine("");

            for (int i = 0; i < m_scaledFCCMatrix.RowCount; i++)
            {
                writer.Write(m_activityList[i]);
                for (int j = 0; j < m_scaledFCCMatrix.ColumnCount; j++)
                {
                    writer.Write(m_scaledFCCMatrix[i, j] + ",");
                }
                writer.WriteLine("");
            }
            writer.WriteLine("");
            writer.Close();
        }

        /// <summary>
        /// Prepare to execute the analysis again.
        /// </summary>
        public void PrepareReAnalysis()
        {
        }

        /// <summary>
        /// Get the flag whether this property is editable.
        /// </summary>
        /// <param name="key">the property name.</param>
        /// <returns>true or false.</returns>
        public bool IsEnableEditProperty(string key)
        {
            return false;
        }

        /// <summary>
        /// Print the current result.
        /// </summary>
        public void PrintResult()
        {
            m_owner.ClearResult();
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
            m_owner.ActivateResultWindow(false, true, false);
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
//          dManager.SimulationStartKeepSetting(m_param.Step);
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
            m_group.AnalysisParameter = GetAnalysisProperty();
            m_execParam = m_owner.JobManager.RunSimParameterSet(m_group.GroupName, tmpDir, m_model, cTime, false, execDict);            
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
                Job.Job job = m_owner.JobManager.GroupDic[m_group.GroupName].GetJob(jobid);
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
                        job.GetLogData(path);
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
                string errmes = String.Format(MessageResources.ErrExecute, MessageResources.NameSensAnalysis);
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
            m_isExistResult = true;
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
