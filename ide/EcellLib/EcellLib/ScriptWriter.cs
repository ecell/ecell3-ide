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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using EcellLib.Objects;
using System.IO;

namespace EcellLib
{
    /// <summary>
    /// ScriptWriter
    /// </summary>
    public class ScriptWriter
    {
        /// <summary>
        /// 
        /// </summary>
        private int m_stepperCount = 0;
        /// <summary>
        /// 
        /// </summary>
        private int m_sysCount = 0;
        /// <summary>
        /// 
        /// </summary>
        private int s_proCount = 0;
        /// <summary>
        /// 
        /// </summary>
        private int m_varCount = 0;
        /// <summary>
        /// 
        /// </summary>
        private int m_logCount = 0;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> m_exportStepper = new Dictionary<string, int>();
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> m_exportSystem = new Dictionary<string, int>();
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> m_exportProcess = new Dictionary<string, int>();
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> s_exportVariable = new Dictionary<string, int>();
        /// <summary>
        /// 
        /// </summary>
        private Project m_currentProject = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="project"></param>
        public ScriptWriter(Project project)
        {
            m_currentProject = project;
        }
        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="l_fileName"></param>
        public void SaveScript(string l_fileName)
        {
            try
            {
                Encoding enc = Encoding.GetEncoding(932);
                File.WriteAllText(l_fileName, "", enc);
                WritePrefix(l_fileName, enc);
                foreach (EcellObject modelObj in m_currentProject.ModelList)
                {
                    String modelName = modelObj.ModelID;
                    WriteModelEntry(l_fileName, enc, modelName);
                    WriteModelProperty(l_fileName, enc, modelName);
                    File.AppendAllText(l_fileName, "\n# System\n", enc);
                    foreach (EcellObject sysObj in m_currentProject.SystemDic[modelName])
                    {
                        WriteSystemEntry(l_fileName, enc, modelName, sysObj);
                        WriteSystemProperty(l_fileName, enc, modelName, sysObj);
                    }
                    foreach (EcellObject sysObj in m_currentProject.SystemDic[modelName])
                    {
                        WriteComponentEntry(l_fileName, enc, sysObj);
                        WriteComponentProperty(l_fileName, enc, sysObj);
                    }
                }
                WriteSimulationForStep(l_fileName, 100, enc);

            }
            catch (Exception l_ex)
            {
                l_ex.ToString();
            }
        }

        /// <summary>
        /// Write the prefix information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        private void WritePrefix(string fileName, Encoding enc)
        {
            DateTime n = DateTime.Now;
            String timeStr = n.ToString("d");
            File.AppendAllText(fileName, "from EcellIDE import *\n", enc);
            File.AppendAllText(fileName, "import time\n", enc);
            File.AppendAllText(fileName, "import System.Threading\n\n", enc);

            File.AppendAllText(fileName, "count = 1000\n", enc);
            File.AppendAllText(fileName, "session=Session()\n", enc);
            File.AppendAllText(fileName, "session.createProject(\"" + m_currentProject.Name + "\",\"" + timeStr + "\")\n\n", enc);
        }

        /// <summary>
        /// Write the postfix information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="count">step count.</param>
        /// <param name="enc">encoding(SJIS)</param>
        private void WriteSimulationForStep(string fileName, int count, Encoding enc)
        {
            for (int i = 0; i < count; i++)
            {
                File.AppendAllText(fileName, "session.step(" + 1 + ")\n", enc);
                File.AppendAllText(fileName, "while session.isActive():\n", enc);
                File.AppendAllText(fileName, "    System.Threading.Thread.Sleep(1000)\n", enc);
            }
        }

        /// <summary>
        /// Write the postfix information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="time">simulation time.</param>
        /// <param name="enc">encoding(SJIS)</param>
        private void WriteSimulationForTime(string fileName, double time, Encoding enc)
        {
            File.AppendAllText(fileName, "session.run(" + time + ")\n", enc);
            File.AppendAllText(fileName, "while session.isActive():\n", enc);
            File.AppendAllText(fileName, "    System.Threading.Thread.Sleep(1000)\n", enc);
        }

        /// <summary>
        /// Write the model information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        private void WriteModelEntry(string fileName, Encoding enc, string modelName)
        {
            File.AppendAllText(fileName, "session.createModel(\"" + modelName + "\")\n\n", enc);
            File.AppendAllText(fileName, "# Stepper\n", enc);
            foreach (EcellObject stepObj in
                m_currentProject.StepperDic[m_currentProject.SimulationParam][modelName])
            {
                File.AppendAllText(fileName, "stepperStub" + m_stepperCount + "=session.createStepperStub(\"" + stepObj.Key + "\")\n", enc);
                File.AppendAllText(fileName, "stepperStub" + m_stepperCount + ".create(\"" + stepObj.Classname + "\")\n", enc);
                m_exportStepper.Add(stepObj.Key, m_stepperCount);
                m_stepperCount++;
            }
        }

        /// <summary>
        /// Write the model property in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        private void WriteModelProperty(string fileName, Encoding enc, string modelName)
        {
            File.AppendAllText(fileName, "\n# Stepper\n", enc);
            foreach (EcellObject stepObj in
                m_currentProject.StepperDic[m_currentProject.SimulationParam][modelName])
            {
                int count = m_exportStepper[stepObj.Key];
                foreach (EcellData d in stepObj.Value)
                {
                    if (!d.Settable) continue;
                    if (d.Value.ToString().Equals("+∞"))
                        File.AppendAllText(fileName,
                            "stepperStub" + count + ".setProperty(\"" + d.Name + "\",\"" + "1.79769313486231E+308" + "\")\n", enc);
                    else
                        File.AppendAllText(fileName,
                            "stepperStub" + count + ".setProperty(\"" + d.Name + "\",\"" + Convert.ToDouble(d.Value.ToString()) + "\")\n", enc);
                }
            }
        }

        /// <summary>
        /// Write the system entry in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        /// <param name="sysObj">written system object.</param>
        private void WriteSystemEntry(string fileName, Encoding enc, string modelName, EcellObject sysObj)
        {
            if (sysObj == null) return;
            m_exportSystem.Add(sysObj.Key, m_sysCount);
            string prefix = "";
            string data = "";
            if (sysObj.Key.Equals("/"))
            {
                prefix = "";
                data = "/";
            }
            else
            {
                string[] ele = sysObj.Key.Split(new char[] { '/' });
                if (ele.Length == 2)
                {
                    prefix = "/";
                    data = ele[ele.Length - 1];
                }
                else
                {
                    for (int i = 1; i < ele.Length - 1; i++)
                    {
                        prefix = prefix + "/" + ele[i];
                    }
                    data = ele[ele.Length - 1];
                }
            }
            File.AppendAllText(fileName, "systemStub" + m_sysCount + "= session.createEntityStub(\"System:" + prefix + ":" + data + "\")\n", enc);
            File.AppendAllText(fileName, "systemStub" + m_sysCount + ".create(\"System\")\n", enc);
            m_sysCount++;
        }

        /// <summary>
        /// Write the system property in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        /// <param name="sysObj">written system object.</param>
        private void WriteSystemProperty(string fileName, Encoding enc, string modelName, EcellObject sysObj)
        {
            if (sysObj == null)
                return;
            int count = m_exportSystem[sysObj.Key];
            if (sysObj.Value == null)
                return;
            foreach (EcellData d in sysObj.Value)
            {
                if (!d.Settable)
                    continue;
                File.AppendAllText(fileName,
                    "systemStub" + count + ".setProperty(\"" + d.Name + "\",\"" + d.Value.ToString() + "\")\n", enc);
            }
        }

        /// <summary>
        /// Write the logger property in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="logList">Logger list.</param>
        private void WriteLoggerProperty(string fileName, Encoding enc, List<string> logList)
        {
            string curParam = m_currentProject.SimulationParam;
            if (curParam == null)
                return;
            LoggerPolicy l = m_currentProject.LoggerPolicyDic[curParam];
            File.AppendAllText(fileName, "\n# Logger Policy\n");
            if (logList == null)
                return;
            foreach (string path in logList)
            {
                File.AppendAllText(fileName,
                    "logger" + m_logCount + "=session.createLoggerStub(\"" + path + "\")\n",
                    enc);
                File.AppendAllText(fileName,
                    "logger" + m_logCount + ".create()\n",
                    enc);
                File.AppendAllText(fileName,
                    "logger" + m_logCount + ".setLoggerPolicy(" +
                    l.m_reloadStepCount + "," +
                    l.m_reloadInterval + "," +
                    l.m_diskFullAction + "," +
                    l.m_maxDiskSpace + ")\n", enc);
                m_logCount++;
            }

        }

        /// <summary>
        /// Write the logger property to save the log in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="saveList">save property list.</param>
        private void WriteLoggerSaveEntry(string fileName, Encoding enc, List<SaveLoggerProperty> saveList)
        {
            File.AppendAllText(fileName, "\n# Save logging\n", enc);
            if (saveList == null)
                return;
            foreach (SaveLoggerProperty s in saveList)
            {
                File.AppendAllText(
                    fileName,
                    "session.saveLoggerData(\"" + s.FullPath + "\",\"" + s.DirName + "\"," +
                    s.Start + "," + s.End + ")\n",
                    enc);
            }
        }


        /// <summary>
        /// Write the component entry of model in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="sysObj">system object.</param>
        private void WriteComponentEntry(string fileName, Encoding enc, EcellObject sysObj)
        {
            File.AppendAllText(fileName, "\n# Variable\n", enc);
            if (sysObj.Children == null) return;
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.Type.Equals("Variable"))
                    continue;
                string[] names = obj.Key.Split(new char[] { ':' });
                string name = names[names.Length - 1];

                File.AppendAllText(fileName,
                    "variableStub" + m_varCount + name + "= session.createEntityStub(\"Variable:" + obj.Key + "\")\n",
                    enc);
                File.AppendAllText(fileName,
                    "variableStub" + m_varCount + name + ".create(\"Variable\")\n", enc);
                s_exportVariable.Add(obj.Key, m_varCount);
                m_varCount++;
            }

            File.AppendAllText(fileName, "\n# Process\n", enc);
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.Type.Equals("Process"))
                    continue;
                string[] names = obj.Key.Split(new char[] { ':' });
                string name = names[names.Length - 1];

                File.AppendAllText(fileName,
                    "processStub" + s_proCount + name + "= session.createEntityStub(\"Process:" + obj.Key + "\")\n",
                    enc);
                File.AppendAllText(fileName,
                    "processStub" + s_proCount + name + ".create(\"" + obj.Classname + "\")\n", enc);
                m_exportProcess.Add(obj.Key, s_proCount);
                s_proCount++;
            }
        }

        /// <summary>
        /// Write the component property of model in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="sysObj">system object.</param>
        private void WriteComponentProperty(string fileName, Encoding enc, EcellObject sysObj)
        {
            File.AppendAllText(fileName, "\n# Variable\n", enc);
            if (sysObj.Children == null) return;
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.Type.Equals("Variable"))
                    continue;
                string[] names = obj.Key.Split(new char[] { ':' });
                string name = names[names.Length - 1];
                int count = s_exportVariable[obj.Key];

                foreach (EcellData d in obj.Value)
                {
                    if (!d.Settable)
                        continue;
                    File.AppendAllText(fileName,
                        "variableStub" + count + name + ".setProperty(\"" + d.Name + "\",\"" + d.Value.ToString() + "\")\n", enc);
                    //                        "variableStub" + count + name + ".setProperty(\"" + d.Name + "\",\"" + GetEntityProperty(d.EntityPath).ToString() + "\")\n", enc);
                }
            }

            File.AppendAllText(fileName, "\n# Process\n", enc);
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.Type.Equals("Process")) continue;
                string[] names = obj.Key.Split(new char[] { ':' });
                string name = names[names.Length - 1];
                int count = m_exportProcess[obj.Key];

                foreach (EcellData d in obj.Value)
                {
                    if (!d.Settable)
                        continue;
                    File.AppendAllText(fileName,
                        "processStub" + count + name + ".setProperty(\"" + d.Name + "\",\"" + d.Value.ToString().Replace("\"", "\\\"") + "\")\n", enc);
                    //                        "processStub" + count + name + ".setProperty(\"" + d.Name + "\",\"" + GetEntityProperty(d.EntityPath).ToString().Replace("\"", "\\\"") + "\")\n", enc);
                }
            }
        }
    }
}
