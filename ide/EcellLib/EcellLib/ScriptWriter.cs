﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
using Ecell.Objects;
using System.IO;

namespace Ecell
{
    /// <summary>
    /// ScriptWriter
    /// </summary>
    public class ScriptWriter
    {
        /// <summary>
        /// The number of Stepper.
        /// </summary>
        private int m_stepperCount = 0;
        /// <summary>
        /// The number of System.
        /// </summary>
        private int m_sysCount = 0;
        /// <summary>
        /// The number of Process.
        /// </summary>
        private int m_proCount = 0;
        /// <summary>
        /// The number of Variable.
        /// </summary>
        private int m_varCount = 0;
        /// <summary>
        /// The number of Log.
        /// </summary>
        private int m_logCount = 0;
        /// <summary>
        /// The list of exported Stepper.
        /// </summary>
        private Dictionary<string, int> m_exportStepper = new Dictionary<string, int>();
        /// <summary>
        /// The list of exported System.
        /// </summary>
        private Dictionary<string, int> m_exportSystem = new Dictionary<string, int>();
        /// <summary>
        /// The list of exported Process.
        /// </summary>
        private Dictionary<string, int> m_exportProcess = new Dictionary<string, int>();
        /// <summary>
        /// The list of exported Variable.
        /// </summary>
        private Dictionary<string, int> m_exportVariable = new Dictionary<string, int>();
        /// <summary>
        /// The current project.
        /// </summary>
        private Project m_currentProject = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="project">Project object.</param>
        public ScriptWriter(Project project)
        {
            m_currentProject = project;
        }

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="fileName">the saved file name,</param>
        public void SaveScript(string fileName)
        {
            ClearScriptInfo();
            Encoding enc = Encoding.GetEncoding(932);
            File.WriteAllText(fileName, "", enc);
//            WritePrefix(fileName, enc);
            foreach (EcellObject modelObj in m_currentProject.ModelList)
            {
                string modelName = modelObj.ModelID;
//                WriteModelEntry(fileName, enc, modelName);
//                WriteModelProperty(fileName, enc, modelName);
                File.AppendAllText(fileName, "\n# System\n", enc);
                foreach (EcellObject sysObj in m_currentProject.SystemDic[modelName])
                {
                    WriteSystemEntry(fileName, enc, modelName, sysObj);
                    WriteSystemProperty(fileName, enc, modelName, sysObj);
                }
                foreach (EcellObject sysObj in m_currentProject.SystemDic[modelName])
                {
                    WriteComponentEntry(fileName, enc, sysObj);
                    WriteComponentProperty(fileName, enc, sysObj);
                }
            }
            WriteSimulationForStepUnix(fileName, 1, enc);
        }

        /// <summary>
        /// Clear the counter to count object in model.
        /// </summary>
        public void ClearScriptInfo()
        {
            m_stepperCount = 0;
            m_sysCount = 0;
            m_varCount = 0;
            m_proCount = 0;
            m_logCount = 0;
            m_exportSystem.Clear();
            m_exportProcess.Clear();
            m_exportVariable.Clear();
            m_exportStepper.Clear();
        }

        /// <summary>
        /// Write the prefix information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        public void WritePrefix(string fileName, Encoding enc)
        {
            DateTime n = DateTime.Now;
            string timeStr = n.ToString("d");
            File.AppendAllText(fileName, "from EcellIDE import *\n", enc);
            File.AppendAllText(fileName, "import time\n", enc);
            File.AppendAllText(fileName, "import System.Threading\n\n", enc);

            File.AppendAllText(fileName, "count = 1000\n", enc);
            File.AppendAllText(fileName, "session=Session()\n", enc);
            File.AppendAllText(fileName, "session.createProject(\"" + m_currentProject.Info.Name + "\",\"" + timeStr + "\")\n\n", enc);
        }

        /// <summary>
        /// Write the model information in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        public void WriteModelEntry(string fileName, Encoding enc, string modelFile)
        {
            File.AppendAllText(fileName, "loadModel(\"" + modelFile + "\")\n", enc); 
        }

        /// <summary>
        /// Write the model property in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="modelName">model name.</param>
        public void WriteModelProperty(string fileName, Encoding enc, string modelName, List<EcellObject> stepperList)
        {
            File.AppendAllText(fileName, "\n# Stepper\n", enc);
            foreach (EcellObject stepObj in stepperList)
            {
                int count = m_exportStepper[stepObj.Key];
                foreach (EcellData d in stepObj.Value)
                {
                    if (!d.Settable) continue;
                    if (d.Value.ToString().Equals("+∞"))
                        File.AppendAllText(fileName,
                            "stepperStub" + count + ".setProperty(\"" + d.EntityPath + "\",\"" + "1.79769313486231E+308" + "\")\n", enc);
                    else
                        File.AppendAllText(fileName,
                            "stepperStub" + count + ".setProperty(\"" + d.EntityPath + "\",\"" + Convert.ToDouble(d.Value.ToString()) + "\")\n", enc);
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
        public void WriteSystemEntry(string fileName, Encoding enc, string modelName, EcellObject sysObj)
        {
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
        public void WriteSystemProperty(string fileName, Encoding enc, string modelName, EcellObject sysObj)
        {
            int count = m_exportSystem[sysObj.Key];
            foreach (EcellData d in sysObj.Value)
            {
                if (!d.Settable)
                    continue;
                File.AppendAllText(fileName,
                    "systemStub" + count + ".setProperty(\"" + d.EntityPath + "\",\"" + d.Value.ToString() + "\")\n", enc);
            }
        }


        /// <summary>
        /// Write the component entry of model in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="sysObj">system object.</param>
        public void WriteComponentEntry(string fileName, Encoding enc, EcellObject sysObj)
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
                m_exportVariable.Add(obj.Key, m_varCount);
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
                    "processStub" + m_proCount + name + "= session.createEntityStub(\"Process:" + obj.Key + "\")\n",
                    enc);
                File.AppendAllText(fileName,
                    "processStub" + m_proCount + name + ".create(\"" + obj.Classname + "\")\n", enc);
                m_exportProcess.Add(obj.Key, m_proCount);
                m_proCount++;
            }
        }

        /// <summary>
        /// Write the component property of model in script file.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="sysObj">system object.</param>
        public void WriteComponentProperty(string fileName, Encoding enc, EcellObject sysObj)
        {
            File.AppendAllText(fileName, "\n# Variable\n", enc);
            if (sysObj.Children == null) return;
            foreach (EcellObject obj in sysObj.Children)
            {
                if (!obj.Type.Equals("Variable"))
                    continue;
                string[] names = obj.Key.Split(new char[] { ':' });
                string name = names[names.Length - 1];
                int count = m_exportVariable[obj.Key];

                foreach (EcellData d in obj.Value)
                {
                    if (!d.Settable)
                        continue;
                    File.AppendAllText(fileName,
                        "variableStub" + count + name + ".setProperty(\"" + d.EntityPath + "\",\"" + d.Value.ToString() + "\")\n", enc);
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
                        "processStub" + count + name + ".setProperty(\"" + d.EntityPath + "\",\"" + d.Value.ToString().Replace("\"", "\\\"") + "\")\n", enc);
                    //                        "processStub" + count + name + ".setProperty(\"" + d.Name + "\",\"" + GetEntityProperty(d.EntityPath).ToString().Replace("\"", "\\\"") + "\")\n", enc);
                }
            }
        }

        #region UnixCommand
        /// <summary>
        /// Write simulation step for Unix script.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="count">step count.</param>
        public void WriteSimulationForStepUnix(string fileName, int count, Encoding enc)
        {
            File.AppendAllText(fileName, "step(" + count + ")\n", enc);
        }

        /// <summary>
        /// Write simulation time for Unix script
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="time">simulation time.</param>
        public void WriteSimulationForTimeUnix(string fileName, double time, Encoding enc)
        {
            File.AppendAllText(fileName, "run(" + time + ")\n", enc);
        }

        /// <summary>
        /// Write the load model script for Unix script.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="jobid">job id.</param>
        public void WriteModelEntryUnix(string fileName, Encoding enc, int jobid)
        {
            File.AppendAllText(fileName, "loadModel(\"" + jobid + ".eml\")\n", enc);            
        }

        /// <summary>
        /// Write the export logger for Unix script.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="logList">the list of logger.</param>
        public void WriteLoggerPropertyUnix(string fileName, Encoding enc, List<string> logList)
        {
            foreach (string name in logList)
            {
                File.AppendAllText(fileName, "aLogger" + m_logCount + " = createLoggerStub(\"" + name + "\")\n", enc);
                File.AppendAllText(fileName, "aLogger" + m_logCount + ".create()\n");
                m_logCount++;
            }
        }

        /// <summary>
        /// Write the save logger entry for Unix script.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="saveList">the list of saved object.</param>
        /// <param name="topdir">the top directory.</param>
        public void WriteLoggerSaveEntryUnix(string fileName, Encoding enc, int jobID,
            List<SaveLoggerProperty> saveList, string topdir)
        {
            File.AppendAllText(fileName, "\n# Save logging\n", enc);
            if (saveList == null)
                return;
            foreach (SaveLoggerProperty s in saveList)
            {
                if (topdir == null)
                    topdir = s.DirName;
                string dir = topdir + "/" + Environment.MachineName + "/" + jobID;
                File.AppendAllText(
                    fileName,
                    "saveLoggerData(\"" + s.FullPath + "\",\"" + dir + "\"," +
                    s.Start + "," + s.End + ")\n",
                    enc);
            }
        }

        /// <summary>
        /// Write the save logger entry for Unix script.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="saveList">the list of saved object.</param>
        /// <param name="topdir">the top directory.</param>
        public void WriteLoggerSaveEntryLocal(string fileName, Encoding enc, int jobID,
            List<SaveLoggerProperty> saveList, string topdir)
        {
            File.AppendAllText(fileName, "\n# Save logging\n", enc);
            if (saveList == null)
                return;
            foreach (SaveLoggerProperty s in saveList)
            {
                if (topdir == null)
                    topdir = s.DirName;
                string dir = topdir + "/";
                File.AppendAllText(
                    fileName,
                    "saveLoggerData(\"" + s.FullPath + "\",\"" + dir + "\"," +
                    s.Start + "," + s.End + ")\n",
                    enc);
            }
        }

        /// <summary>
        /// Write componentn property for Unix script.
        /// </summary>
        /// <param name="fileName">script file name.</param>
        /// <param name="enc">encoding(SJIS)</param>
        /// <param name="obj">the object.</param>
        /// <param name="data">the data of object.</param>
        public void WriteComponentPropertyUnix(string fileName, Encoding enc, EcellObject obj, EcellData data)
        {
            int i = 0;
            if (obj.Type.Equals(EcellObject.SYSTEM))
            {
                i = m_sysCount;
                m_sysCount++;
            }
            else if (obj.Type.Equals(EcellObject.PROCESS))
            {
                i = m_proCount;
                m_proCount++;
            }
            else if (obj.Type.Equals(EcellObject.VARIABLE))
            {
                i = m_varCount;
                m_varCount++;
            }
            string entName = obj.Type + i;
            File.AppendAllText(fileName,
                entName + " = createEntityStub(\"" + obj.FullID + "\")\n", enc);
            File.AppendAllText(fileName,
                entName + ".setProperty(\"" + data.Name + "\"," + data.Value.ToString() + ")\n",
                enc);
        }
        #endregion
    }
}
