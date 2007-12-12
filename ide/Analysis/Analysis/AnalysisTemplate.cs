//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace EcellLib.Analysis
{
    /// <summary>
    /// UserControl to manage the result and parameter of robust analysis.
    /// </summary>
    public partial class AnalysisTemplate : UserControl
    {
        #region Internal Class
        /// <summary>
        /// The parameter object to execute robust analysis.
        /// </summary>
        public class RandomCard
        {
            #region Fields
            /// <summary>
            /// The check flag whether the data is fixed.
            /// </summary>
            private bool m_isFixed;
            /// <summary>
            /// The max value on the selected data.
            /// </summary>
            private double m_Max;
            /// <summary>
            /// The min value on the selected data.
            /// </summary>
            private double m_Min;
            /// <summary>
            /// The max frequency on the selected data for FFT.
            /// </summary>
            private double m_MinFreq;
            /// <summary>
            /// The min frequency on the selected data for FFT.
            /// </summary>
            private double m_MaxFreq;
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor for RandomCard.
            /// </summary>
            public RandomCard()
            {
                m_isFixed = true;
                m_Max = 1.0;
                m_Min = 0.0;
                m_MaxFreq = 0.0;
                m_MinFreq = 0.0;
            }

            /// <summary>
            /// Constructor for RandomCard with initial parameters.
            /// </summary>
            /// <param name="max">max value on the data.</param>
            /// <param name="min">min value on the data.</param>
            /// <param name="maxFreq">max frequency on the data.</param>
            /// <param name="minFreq">min frequency on the data.</param>
            public RandomCard(double max, double min, double maxFreq, double minFreq)
            {
                m_isFixed = true;
                m_Max = max;
                m_Min = min;
                m_MaxFreq = maxFreq;
                m_MinFreq = minFreq;
            }
            #endregion

            #region Accessor
            /// <summary>
            /// get/set the flag whether the data is fixed.
            /// </summary>
            public bool IsFix
            {
                get { return this.m_isFixed; }
                set { this.m_isFixed = value; }
            }

            /// <summary>
            /// get/set the max value on the data.
            /// </summary>
            public double Max
            {
                get { return this.m_Max; }
                set { this.m_Max = value; }
            }

            /// <summary>
            /// get/set the min value on the data.
            /// </summary>
            public double Min
            {
                get { return this.m_Min; }
                set { this.m_Min = value; }
            }

            /// <summary>
            /// get/set the min frequency on the data.
            /// </summary>
            public double MinFreq
            {
                get { return m_MinFreq; }
                set { m_MinFreq = value; }
            }

            /// <summary>
            /// get/set the max frequency on the data.
            /// </summary>
            public double MaxFreq
            {
                get { return m_MaxFreq; }
                set { m_MaxFreq = value; }
            }
            #endregion
        }

        /// <summary>
        /// The parameter oject to check the simulation results.
        /// </summary>
        public class JudgeCard
        {
            #region Fields
            /// <summary>
            /// The check flag whether the data is the target to check.
            /// </summary>
            private bool m_isTarget;
            /// <summary>
            /// The min value allowed on this data.
            /// </summary>
            private double m_MinValue;
            /// <summary>
            /// The max value allowed on this data.
            /// </summary>
            private double m_MaxValue;
            /// <summary>
            /// The allowd difference between the max value and the min value on this data.
            /// </summary>
            private double m_Difference;
            /// <summary>
            /// The rate for FFT on this data.
            /// </summary>
            private double m_Rate;
            #endregion

            #region Constructor
            /// <summary>
            /// The constructor for JudgeCard.
            /// </summary>
            public JudgeCard()
            {
                m_isTarget = false;
                m_MinValue = 0.0;
                m_MaxValue = 0.0;
                m_Difference = 0.0;
                m_Rate = 0.0;
            }

            /// <summary>
            /// The constructor for JudgeCard with initial parameters.
            /// </summary>
            /// <param name="maxValue">The max value on this data.</param>
            /// <param name="minValue">The min value on this data.</param>
            /// <param name="diff">The difference between the max value and the min value.</param>
            /// <param name="rate">The rate for FFT on this data.</param>
            public JudgeCard(double maxValue, double minValue, 
                double diff, double rate)
            {
                m_isTarget = false;
                m_MinValue = minValue;
                m_MaxValue = maxValue;
                m_Difference = diff;
                m_Rate = rate;
            }
            #endregion

            #region Accessor
            /// <summary>
            /// get/set the flag whether the data is target to check the simulation results.
            /// </summary>
            public bool IsTarget
            {
                get { return m_isTarget; }
                set { m_isTarget = value; }
            }

            /// <summary>
            /// get/set the max value on this data.
            /// </summary>
            public double MaxValue
            {
                get { return m_MaxValue; }
                set { m_MaxValue = value; }
            }

            /// <summary>
            /// get/set the min value on this data.
            /// </summary>
            public double MinValue
            {
                get { return this.m_MinValue; }
                set { this.m_MinValue = value; }
            }

            /// <summary>
            /// get/set the diggernce between the max value and the min value.
            /// </summary>
            public double Difference
            {
                get { return m_Difference; }
                set { m_Difference = value; }
            }

            /// <summary>
            /// The rate for FFT on this data.
            /// </summary>
            public double Rate
            {
                get { return m_Rate; }
                set { m_Rate = value; }
            }
            #endregion
        }
        #endregion

        #region Fields
        /// <summary>
        /// The number of sample to analysis the robust.
        /// </summary>
        private int m_SampleNum = 50;
        /// <summary>
        /// The simulation time to analysis the robust.
        /// </summary>
        private double m_SimulationTime = 100.0;
        /// <summary>
        /// The check time to analysis the robust.
        /// </summary>
        private double m_WindowSize = 10.0;
        /// <summary>
        /// DataManager.
        /// </summary>
        private DataManager m_manager;
        /// <summary>
        /// The path of selected object.
        /// </summary>
        private String m_Path;
        /// <summary>
        /// The file name of model.
        /// </summary>
        private String m_ModelName = "RobustModel.eml";
        /// <summary>
        /// The file name written the robust results.
        /// </summary>
        private String m_ResultName = "RobustResult.txt";
        /// <summary>
        /// The file name of tool parameters.
        /// </summary>
        private String m_ToolParamName = "RobustToolParam.txt";
        /// <summary>
        /// The file name written the random parameters. 
        /// </summary>
        private String m_RandParamName = "RobustRandParam.txt";
        /// <summary>
        /// The file name written the checked parameters.
        /// </summary>
        private String m_CheckParamName = "RobustCheckParam.txt";
        /// <summary>
        /// The dictionary the path of selected object and the parameter object of random.
        /// </summary>
        private Dictionary<String, RandomCard> m_randomDict;
        /// <summary>
        /// The dictionary the path of selected object and the parameter object to check.
        /// </summary>
        private Dictionary<String, JudgeCard> m_judgeDict;
        /// <summary>
        /// The timer to check the end of program.
        /// </summary>
        System.Windows.Forms.Timer m_time;
        /// <summary>
        /// The delegate fucntion to finish the robust analysis.
        /// </summary>
        delegate void FinishAnalysisDelegate();
        /// <summary>
        /// The flag whether the select object is changing.
        /// </summary>
        bool isChangeSelect = false;
        #endregion

        /// <summary>
        /// Constructor for AnalysisTemplate.
        /// </summary>
        public AnalysisTemplate()
        {
            InitializeComponent();

            m_randomDict = new Dictionary<string, RandomCard>();
            m_judgeDict = new Dictionary<string, JudgeCard>();

            simTimeText.Text = Convert.ToString(m_SimulationTime);
            winSizeText.Text = Convert.ToString(m_WindowSize);
            sampleNumText.Text = Convert.ToString(m_SampleNum);
            ATViewButton.Enabled = false;
            ATStopButton.Enabled = false;

            m_manager = DataManager.GetDataManager();
            GetTreeData();

            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 3000;
            m_time.Tick += new EventHandler(TimerFire);

            m_ModelName = Util.GetTmpDir() + "/RobustModel.eml";
            m_ResultName = Util.GetTmpDir() + "/RobustResult.txt";
            m_ToolParamName = Util.GetTmpDir() + "/RobustToolParam.txt";
            m_RandParamName = Util.GetTmpDir() + "/RobustRandParam.txt";
            m_CheckParamName = Util.GetTmpDir() + "/RobustCheckParam.txt";


            AllUnEnable();
        }

        /// <summary>
        /// Unenable to change the all input fields.
        /// </summary>
        private void AllUnEnable()
        {
            isFixCheckBox.Enabled = false;
            targetCheckBox.Enabled = false;
            maxText.Enabled = false;
            minText.Enabled = false;
            maxFreqText.Enabled = false;
            minFreqText.Enabled = false;
            maxValueText.Enabled = false;
            minValueText.Enabled = false;
            diffText.Enabled = false;
            rateText.Enabled = false;
        }

        /// <summary>
        /// Enable to change the all input fields.
        /// </summary>
        private void AllEnable()
        {
            isFixCheckBox.Enabled = true;
            targetCheckBox.Enabled = true;
            maxText.Enabled = true;
            minText.Enabled = true;
            maxFreqText.Enabled = true;
            minFreqText.Enabled = true;
            maxValueText.Enabled = true;
            minValueText.Enabled = true;
            diffText.Enabled = true;
            rateText.Enabled = true;
        }

        /// <summary>
        /// Reflect the input data to each cards.
        /// </summary>
        private void InputReflection()
        {
            if (m_Path != null)
            {
                m_randomDict[m_Path].IsFix = isFixCheckBox.Checked;
                m_randomDict[m_Path].Max = Convert.ToDouble(maxText.Text);
                m_randomDict[m_Path].Min = Convert.ToDouble(minText.Text);
                m_randomDict[m_Path].MaxFreq = Convert.ToDouble(maxFreqText.Text);
                m_randomDict[m_Path].MinFreq = Convert.ToDouble(minFreqText.Text);

                m_judgeDict[m_Path].IsTarget = targetCheckBox.Checked;
                m_judgeDict[m_Path].MaxValue = Convert.ToDouble(maxValueText.Text);
                m_judgeDict[m_Path].MinValue = Convert.ToDouble(minValueText.Text);
                m_judgeDict[m_Path].Difference = Convert.ToDouble(diffText.Text);
                m_judgeDict[m_Path].Rate = Convert.ToDouble(rateText.Text);

                if (m_randomDict[m_Path].IsFix == false)
                {
                    if (m_randomDict[m_Path].Max < m_randomDict[m_Path].Min)
                    {
                        throw new Exception("Min should be smaller than Max.");
                    }

                    if (m_randomDict[m_Path].MaxFreq < m_randomDict[m_Path].MinFreq)
                    {
                        throw new Exception("Min Frequency should be smaller than Max Frequency.");
                    }
                }

                if (m_judgeDict[m_Path].IsTarget == true)
                {
                    if (m_judgeDict[m_Path].MaxValue < m_judgeDict[m_Path].MinValue)
                    {
                        throw new Exception("Min Value should be smaller than Max Value.");
                    }

                    if (m_judgeDict[m_Path].Difference <= 0)
                    {
                        throw new Exception("Difference should be bigger than 0.");
                    }
                }
            }
        }

        #region RobustAnalysis
        /// <summary>
        /// Start to execute the robust analysis with the set parameters.
        /// </summary>
        private void StartRobustAnalysis()
        {
            Util.InitialLanguage();

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "ipy.exe";
                p.StartInfo.Arguments = "\"" + Util.GetAnalysisDir() + "/robust.py\" \"" +
                    m_ModelName + "\" \"" + m_ToolParamName + "\" \"" +
                    m_RandParamName + "\" \"" + m_CheckParamName + "\" \"" +
                    m_ResultName + "\"";
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;

                p.Start();
            }
            catch (Exception ex)
            {
                try
                {
                    Process p1 = new Process();
                    ex.ToString();
                    p1.StartInfo.FileName = Util.GetAnalysisDir() + "\\ipy.exe";
                    p1.StartInfo.Arguments = "\"" + Util.GetAnalysisDir() + "/robust.py\" \"" +
                        m_ModelName + "\" \"" + m_ToolParamName + "\" \"" +
                        m_RandParamName + "\" \"" + m_CheckParamName + "\" \"" +
                        m_ResultName + "\"";
                    p1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p1.StartInfo.CreateNoWindow = true;

                    p1.Start();
                }
                catch (Exception ex1)
                {
                    String mes = Analysis.s_resources.GetString("ErrAnalysis");
                    MessageBox.Show(mes + "\n" + ex1.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Finish to execute the robust analysis.
        /// </summary>
        private void FinishRobustAnalysis()
        {
            StreamReader reader = new StreamReader(
                (System.IO.Stream)File.OpenRead(m_ResultName));
            string line = reader.ReadLine();
            Double result = 0.0;
            int isLine = 1;
            if (line.Equals("")) isLine = 0;
            else  result = Convert.ToDouble(line);
            if (isLine == 0 || result != 0)
            {
                line = reader.ReadLine();
                MessageBox.Show(line, "ERROR", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ATExecuteButton.Enabled = true;
                ATViewButton.Enabled = false;
                ATStopButton.Enabled = false;

                reader.Close();
                return;
            }

            xComboBox.Items.Clear();
            yComboBox.Items.Clear();

            line = reader.ReadLine();
            string[] ele = line.Split(new char[] { '\t' });
            for (int i = 2; i < ele.Length; i++)
            {
                if (ele[i].StartsWith("#")) continue;
                xComboBox.Items.Add(ele[i]);
                yComboBox.Items.Add(ele[i]);

                if (i == 2)
                {
                    xComboBox.Text = ele[i];
                    yComboBox.Text = ele[i];
                }
            }
            ATExecuteButton.Enabled = true;
            ATViewButton.Enabled = true;
            ATStopButton.Enabled = false;
            
            reader.Close();

            String mes = Analysis.s_resources.GetString("FinishRAnalysis");
            MessageBox.Show(mes, "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Output the file of tool parameters.
        /// </summary>
        /// <returns>success or fail.</returns>
        private bool OutputToolParameter()
        {
            FileStream fs = null;
            StreamWriter writer = null;

            try
            {
                fs = File.Open(m_ToolParamName, FileMode.OpenOrCreate);
                writer = new StreamWriter(fs);

                int sampleNum;
                double simTime;
                double winSize;

                try
                {
                    sampleNum = Convert.ToInt32(sampleNumText.Text);
                    simTime = Convert.ToDouble(simTimeText.Text);
                    winSize = Convert.ToDouble(winSizeText.Text);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    writer.Close();
                    fs.Close();
                    String errmes = Analysis.s_resources.GetString("ErrOutParam");
                    MessageBox.Show(errmes,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (sampleNum < 1 || simTime <= 0 || winSize <= 0)
                {
                    writer.Close();
                    fs.Close();
                    String errmes = Analysis.s_resources.GetString("ErrPosValue");
                    MessageBox.Show(errmes,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                writer.WriteLine("NumOfSampling = " + sampleNum);
                writer.WriteLine("SimTime = " + simTime);
                writer.WriteLine("CheckWindowSize = " + winSize);

                writer.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                if (writer != null) writer.Close();
                if (fs != null) fs.Close();
                e.ToString();
                String errmes = Analysis.s_resources.GetString("ErrOutParamFile");
                MessageBox.Show(errmes,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Output the file of random parameter.
        /// </summary>
        /// <returns>success or fail.</returns>
        private bool OutputRandomParameter()
        {
            FileStream fs = null;
            StreamWriter writer = null;

            try
            {
                int count = 0;
                fs = File.Open(m_RandParamName, FileMode.OpenOrCreate);
                writer = new StreamWriter(fs);

                foreach (string path in m_randomDict.Keys)
                {
                    if (m_randomDict[path].IsFix) continue;

                    string[] keys = path.Split(new char[] { ':' });
                    writer.WriteLine(keys[0] + "\t" + keys[1] + ":" + keys[2] + "\t"
                    + keys[3] + "\t" + m_randomDict[path].Min + "\t" +
                    m_randomDict[path].Max);
                    count++;
                }

                writer.Close();
                fs.Close();

                if (count <= 1)
                {
                    String errmes = Analysis.s_resources.GetString("ErrRandParam");
                    MessageBox.Show(errmes,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                if (writer != null) writer.Close();
                if (fs != null) fs.Close();
                e.ToString();
                String errmes = Analysis.s_resources.GetString("ErrOutParamFile");
                MessageBox.Show(errmes,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Output the file of checked parameters.
        /// </summary>
        /// <returns>success or fail.</returns>
        private bool OutputCheckParameter()
        {
            FileStream fs = null;
            StreamWriter writer = null;

            try
            {
                int count = 0;
                string cur = Directory.GetCurrentDirectory();
                fs = File.Open(m_CheckParamName, FileMode.OpenOrCreate);
                writer = new StreamWriter(fs);

                foreach (string path in m_judgeDict.Keys)
                {
                    if (!m_judgeDict[path].IsTarget) continue;

                    string[] keys = path.Split(new char[] { ':' });
                    writer.WriteLine(keys[0] + "\t" + keys[1] + ":" + keys[2] + "\t"
                    + keys[3] + "\t"
                    + m_judgeDict[path].Difference + "\t"
                    + m_judgeDict[path].MinValue + "\t"
                    + m_judgeDict[path].MaxValue + "\t"
                    + m_randomDict[path].MinFreq + "\t"
                    + m_randomDict[path].MaxFreq + "\t"
                    + m_judgeDict[path].Rate);
                    count++;
                }

                writer.Close();
                fs.Close();

                if (count <= 0)
                {
                    String errmes = Analysis.s_resources.GetString("ErrNotCheckParam");
                    MessageBox.Show(errmes,
                        "ERRROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                if (writer != null) writer.Close();
                if (fs != null) fs.Close();
                e.ToString();
                String errmes = Analysis.s_resources.GetString("ErrOutParamFile");
                MessageBox.Show(errmes,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Delete the file used in the robust analysis.
        /// </summary>
        private void CleanFile()
        {
            File.Delete(m_ResultName);
            File.Delete(m_ToolParamName);
            File.Delete(m_RandParamName);
            File.Delete(m_CheckParamName);
        }

        /// <summary>
        /// check the size of file and whether the program finished.
        /// </summary>
        /// <returns></returns>
        private bool CheckStatus()
        {
            if (!File.Exists(m_ResultName)) return false;
            FileInfo info = new FileInfo(m_ResultName);
            if (info.Length > 0) return true;

            return false;
        }

        private bool JudgeRobust(String comp, string[] param)
        {
            string[] ele = comp.Split(new char[] { ' ' });
            for (int i = 0; i < param.Length; i++)
            {
                string id = param[i].Substring(1);
                if (!m_judgeDict[id].IsTarget) continue;
                string[] data = ele[i].Split(new char[] { ',' });
                if (data.Length != 3) continue;
                if (Convert.ToDouble(data[0]) < m_judgeDict[id].MinValue)
                    return false;
                if (Convert.ToDouble(data[1]) > m_judgeDict[id].MaxValue)
                    return false;
                if (Convert.ToDouble(data[1]) - Convert.ToDouble(data[0]) >
                    m_judgeDict[id].Difference)
                    return false;
                if (Convert.ToDouble(data[2]) > m_judgeDict[id].Rate)
                    return false;
            }
            return true;
        }

        #endregion

        #region TreeData
        /// <summary>
        /// Get and display the model object when this UserControl is displayed.
        /// </summary>
        private void GetTreeData()
        {
            List<string> modelList = m_manager.GetModelList();

            foreach (string model in modelList)
            {
                TreeNode modelNode = new TreeNode(model);
                treeView1.Nodes.Add(modelNode);
                List<EcellObject> objList = m_manager.GetData(model, "");
                foreach (EcellObject obj in objList)
                {
                    if (obj.type == Constants.xpathModel) continue;
                    TreeNode parentNode;
                    TreeNode sysNode;
                    if (obj.key.Equals("/"))
                    {
                        parentNode = modelNode;
                        sysNode = new TreeNode("/");
                        sysNode.Tag = null;
                    }
                    else
                    {
                        string[] keydata = obj.key.Split(new char[] { '/' });
                        string resultKey = "";
                        
                        for (int i = 1 ; i < keydata.Length - 1; i++)
                        {
                            resultKey = resultKey + "/" + keydata[i];
                        }
                        if (resultKey.Equals("")) resultKey = "/";
                        parentNode = GetNodeElement(modelNode, resultKey);
                        sysNode = new TreeNode(keydata[keydata.Length - 1]);
                        sysNode.Tag = null;
                    }
                    parentNode.Nodes.Add(sysNode);

                    foreach (EcellObject child in obj.Children)
                    {
                        string[] keydata = child.key.Split(new char[] { ':' });
                        TreeNode childNode = new TreeNode(keydata[keydata.Length - 1]);
                        childNode.Tag = null;
                        sysNode.Nodes.Add(childNode);

                        foreach (EcellData data in child.Value)
                        {
                            if (data.Settable && data.Value.Type == typeof(double))
                            {
                                TreeNode propNode = new TreeNode(data.Name);
                                propNode.Tag = data.EntityPath;
                                childNode.Nodes.Add(propNode);
                                double d = data.Value.CastToDouble();
                                RandomCard r = new RandomCard(d + d * 0.3, d - d * 0.3,
                                    50.0, 30.0);
                                m_randomDict.Add(data.EntityPath, r);
                                JudgeCard j = new JudgeCard(d + d * 0.3, 
                                    d - d * 0.3, d * 0.6, 0.5);
                                m_judgeDict.Add(data.EntityPath, j);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the tree node object with the name of object.
        /// </summary>
        /// <param name="name">the name of object.</param>
        /// <returns>the tree node object.</returns>
        private TreeNode GetNodeElementByModel(string name)
        {
            IEnumerator iter = treeView1.Nodes.GetEnumerator();
            while (iter.MoveNext())
            {
                TreeNode n = (TreeNode)iter.Current;
                if (n.Text.Equals(name)) return n;
            }
            return null;
        }

        /// <summary>
        /// Search the adjust object within child nodes of current node.
        /// </summary>
        /// <param name="current">the current node.</param>
        /// <param name="key">the object key.</param>
        /// <returns>the target node.</returns>
        private TreeNode GetNodeElement(TreeNode current, string key)
        {
                if (current.Text.Equals(key)) return current;
                string[] keydata = key.Split(new char[] { '/' });
                IEnumerator nodes = current.Nodes.GetEnumerator();
                while (nodes.MoveNext())
                {
                    TreeNode node = nodes.Current as TreeNode;
                    if (node == null) continue;
                    if (node.Text.Equals("/") && key.StartsWith("/"))
                    {
                        if (key.Equals("/")) return node;
                        string keyResult = keydata[1];
                        for (int i = 2; i < keydata.Length; i++)
                        {
                            keyResult = keyResult + "/" + keydata[i];
                        }
                        return GetNodeElement(node, keyResult);
                    }
                    if (node.Text.Equals(keydata[0]))
                    {
                        if (keydata.Length == 1) return node;
                        string keyResult = keydata[1];
                        for (int i = 2; i < keydata.Length; i++)
                        {
                            keyResult = keyResult + "/" + keydata[i];
                        }
                        return GetNodeElement(node, keyResult);
                    }
                }
            return null;
        }
        #endregion

        #region Events
        /// <summary>
        /// The event sequence when Analysis button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void AnalysisButtonClicked(object sender, EventArgs e)
        {
            CleanFile();

            try
            {
                InputReflection();
            }
            catch (Exception ex)
            {
                ex.ToString();
                String errmes = Analysis.s_resources.GetString("ErrInputReflect");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_manager.ExportModel(m_manager.GetModelList(), m_ModelName);
            if (!OutputToolParameter()) return;
            if (!OutputRandomParameter()) return;
            if (!OutputCheckParameter()) return;

            Thread t = new Thread(new ThreadStart(StartRobustAnalysis));
            t.Start();

            m_time.Enabled = true;
            m_time.Start();
            ATExecuteButton.Enabled = false;
            ATStopButton.Enabled = true;
            ATViewButton.Enabled = false;
        }

        /// <summary>
        /// Execute redraw process on simulation running at every 3sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void TimerFire(object sender, EventArgs e)
        {
            m_time.Enabled = false;
            if (CheckStatus())
            {
                m_time.Stop();
                FinishAnalysisDelegate dlg = new FinishAnalysisDelegate(FinishRobustAnalysis);
                this.Invoke(dlg);
                return;
            }
            m_time.Enabled = true;
        }

        /// <summary>
        /// Event sequence when ViewButton is clicked.
        /// </summary>
        /// <param name="sender">ViewButton.</param>
        /// <param name="e">EventArgs.</param>
        private void ViewButtonClicked(object sender, EventArgs e)
        {
            String xStr = xComboBox.Text;
            String yStr = yComboBox.Text;

            if (xStr.Equals(yStr))
            {
                String errmes = Analysis.s_resources.GetString("ErrSameAxis");
                MessageBox.Show(errmes,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AnalysisResult win = new AnalysisResult();
            win.ChangeXAxis(xStr, m_randomDict[xStr].Max, m_randomDict[xStr].Min);
            win.ChangeYAxis(yStr, m_randomDict[yStr].Max, m_randomDict[yStr].Min);

            StreamReader reader = new StreamReader(
                (System.IO.Stream)File.OpenRead(m_ResultName));
            string line = reader.ReadLine();
            line = reader.ReadLine();
            string[] ele = line.Split(new char[] { '\t' });
            int xPos = 0;
            int yPos = 0;
            for (int i = 2; i < ele.Length; i++)
            {
                if (ele[i].Equals(xStr)) xPos = i;
                if (ele[i].Equals(yStr)) yPos = i;
            }
            string param = ele[ele.Length - 1];
            string[] paramarray = param.Split(new char[] { ' ' });
            win.Show();

            while ((line = reader.ReadLine()) != null)
            {
                ele = line.Split(new char[] { '\t' });
                bool res = JudgeRobust(ele[ele.Length - 1], paramarray);
                if (res)
                    win.DataAdd(Convert.ToDouble(ele[xPos]), 
                        Convert.ToDouble(ele[yPos]));
                else
                    win.NGDataAdd(Convert.ToDouble(ele[xPos]),
                        Convert.ToDouble(ele[yPos]));


            }
            reader.Close();
        }

        /// <summary>
        /// Event sequence when the CheckBox of isFix is changed.
        /// </summary>
        /// <param name="sender">CheckBox.</param>
        /// <param name="e">EventArgs.</param>
        private void IsFixCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c == null) return;
            if (m_Path == null) return;
            if (isChangeSelect) return;

            if (!c.Checked)
            {

                treeView1.SelectedNode.BackColor = Color.Gold;
            }
            else
            {
                treeView1.SelectedNode.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Event sequence when the CheckBox of target is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c == null) return;
            if (m_Path == null) return;
            if (isChangeSelect) return;

            if (c.Checked)
            {
                Font f = treeView1.Font;
                Font nf = new Font(f.Name, f.Size, FontStyle.Bold);
                treeView1.SelectedNode.NodeFont = nf;
            }
            else
            {
                Font f = treeView1.Font;
                treeView1.SelectedNode.NodeFont = f;
            }

        }

        /// <summary>
        /// Event sequence when TreeNode in TreeView is clicked.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeNodeMouseClickEventArgs.</param>
        private void TreeViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                InputReflection();
            }
            catch (Exception ex)
            {
                ex.ToString();
                String errmes = Analysis.s_resources.GetString("ErrInputReflect");
                MessageBox.Show(errmes + "\n" + "\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TreeView t = sender as TreeView;
            if (t == null) return;
            TreeNode node = t.GetNodeAt(e.X, e.Y);
            if (node == null) return;
            if (node.Tag == null)
            {
                m_Path = null;

                AllUnEnable();
                return;
            }
            AllEnable();
            isChangeSelect = true;
            string text = (string)node.Tag;

            RandomCard r = m_randomDict[text];
            isFixCheckBox.Checked = r.IsFix;
            maxText.Text = r.Max.ToString();
            minText.Text = r.Min.ToString();
            maxFreqText.Text = r.MaxFreq.ToString();
            minFreqText.Text = r.MinFreq.ToString();

            JudgeCard j = m_judgeDict[text];
            targetCheckBox.Checked = j.IsTarget;
            maxValueText.Text = j.MaxValue.ToString();
            minValueText.Text = j.MinValue.ToString();
            diffText.Text = j.Difference.ToString();
            rateText.Text = j.Rate.ToString();

            isChangeSelect = false;
            m_Path = text;
        }

        /// <summary>
        /// Event sequence when TreeNode is expand.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">TreeViewEventArgs.</param>
        private void TreeViewAfterExpand(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null) return;

            foreach (TreeNode n in node.Nodes)
            {
                if (n.Tag == null) continue;

                string key = (string)n.Tag;
                if (!m_randomDict[key].IsFix)
                    n.BackColor = Color.Gold;
                else
                    n.BackColor = Color.White;


                if (m_judgeDict[key].IsTarget)
                {
                    Font f = treeView1.Font;
                    Font nf = new Font(f.Name, f.Size, FontStyle.Bold);
                    n.NodeFont = nf;
                }
                else
                {
                    Font f = treeView1.Font;
                    n.NodeFont = f;
                }
            }
        }

        /// <summary>
        /// Event sequence when StopButton is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void StopButtonClicked(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcessesByName("ipy");
            int count = 0;
            foreach (Process p in ps)
            {
                p.Kill();
                count++;
            }

            m_time.Enabled = false;
            m_time.Stop();

            ATExecuteButton.Enabled = true;
            ATStopButton.Enabled = false;
            ATViewButton.Enabled = false;
        }

        /// <summary>
        /// Event sequence when SaveButton is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void SaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                InputReflection();
            }
            catch (Exception ex)
            {
                ex.ToString();
                String errmes = Analysis.s_resources.GetString("ErrInputReflect");
                MessageBox.Show(errmes + "\n" + "\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SaveRobustFileDialog.RestoreDirectory = true;
            if (DialogResult.OK == SaveRobustFileDialog.ShowDialog())
            {
                String filename = SaveRobustFileDialog.FileName;

                FileStream fs = File.Open(filename, FileMode.OpenOrCreate);
                StreamWriter writer = new StreamWriter(fs);

                foreach (string key in m_randomDict.Keys)
                {
                    writer.WriteLine("#" + key + "," +
                        m_randomDict[key].IsFix + "," +
                        m_randomDict[key].Max + "," +
                        m_randomDict[key].Min + "," +
                        m_randomDict[key].MaxFreq + "," +
                        m_randomDict[key].MinFreq);
                }
                foreach (string key in m_judgeDict.Keys)
                {
                    writer.WriteLine(key + "," +
                        m_judgeDict[key].IsTarget + "," +
                        m_judgeDict[key].MaxValue + "," +
                        m_judgeDict[key].MinValue + "," +
                        m_judgeDict[key].Difference + "," +
                        m_judgeDict[key].Rate);
                }

                writer.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// Event sequence when LoadButton is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void LoadButtonClicked(object sender, EventArgs e)
        {
            StreamReader reader = null;
            try
            {
                OpenRobustFileDialog.RestoreDirectory = true;
                if (DialogResult.OK == OpenRobustFileDialog.ShowDialog())
                {
                    string filename = OpenRobustFileDialog.FileName;
                    reader = new StreamReader(
                            (System.IO.Stream)File.OpenRead(filename));

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("#"))
                        {
                            line = line.Substring(1);
                            string[] ele = line.Split(new char[] { ',' });
                            string key = ele[0];
                            if (m_randomDict.ContainsKey(key))
                            {

                                m_randomDict[key].IsFix = Convert.ToBoolean(ele[1]);
                                m_randomDict[key].Max = Convert.ToDouble(ele[2]);
                                m_randomDict[key].Min = Convert.ToDouble(ele[3]);
                                m_randomDict[key].MaxFreq = Convert.ToDouble(ele[4]);
                                m_randomDict[key].MinFreq = Convert.ToDouble(ele[5]);
                            }
                            else
                            {
                                RandomCard r = new RandomCard(
                                    Convert.ToDouble(ele[2]),
                                    Convert.ToDouble(ele[3]),
                                    Convert.ToDouble(ele[4]),
                                    Convert.ToDouble(ele[5]));
                                r.IsFix = Convert.ToBoolean(ele[1]);
                                m_randomDict.Add(key, r);
                            }


                        }
                        else
                        {
                            string[] ele = line.Split(new char[] { ',' });
                            string key = ele[0];
                            if (m_judgeDict.ContainsKey(key))
                            {

                                m_judgeDict[key].IsTarget = Convert.ToBoolean(ele[1]);
                                m_judgeDict[key].MaxValue = Convert.ToDouble(ele[2]);
                                m_judgeDict[key].MinValue = Convert.ToDouble(ele[3]);
                                m_judgeDict[key].Difference = Convert.ToDouble(ele[4]);
                                m_judgeDict[key].Rate = Convert.ToDouble(ele[5]);
                            }
                            else
                            {
                                JudgeCard r = new JudgeCard(
                                    Convert.ToDouble(ele[2]),
                                    Convert.ToDouble(ele[3]),
                                    Convert.ToDouble(ele[4]),
                                    Convert.ToDouble(ele[5]));
                                r.IsTarget = Convert.ToBoolean(ele[1]);
                                m_judgeDict.Add(key, r);
                            }
                        }
                    }

                    reader.Close();
                }

                foreach (TreeNode t in treeView1.Nodes)
                {
                    t.Collapse();
                }
                m_Path = null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                string errmes = Analysis.s_resources.GetString("ErrLoadParam");
                MessageBox.Show(errmes,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (reader != null) reader.Close();
            }
        }
        #endregion
    }
}

