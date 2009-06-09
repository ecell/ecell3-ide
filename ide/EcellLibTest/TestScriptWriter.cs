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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
namespace Ecell
{
    using System;
    using System.Windows.Forms;
    using NUnit.Framework;
    using System.IO;
    using System.Diagnostics;
    using Ecell.Objects;
    using System.Collections.Generic;
    using Ecell.Exceptions;
    using EcellCoreLib;
    using System.Reflection;
    using System.Collections;
    using Ecell.Plugin;
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestScriptWriter
    {
        private ApplicationEnvironment _env;
        private DataManager _unitUnderTest;
        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [SetUp()]
        public void TestFixtureSetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = _env.DataManager;
        }
        /// <summary>
        /// TestFixtureTearDown
        /// </summary>
        [TearDown()]
        public void TestFixtureTearDown()
        {
            _unitUnderTest = null;
            _env = null;
        }

        /// <summary>
        /// TestScriptWriterForUnix
        /// </summary>
        [Test()]
        public void TestScriptWriterForUnix()
        {
            // Load Drosophila
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string processLog = "Process:/CELL/CYTOPLASM:R_toy10:Activity";
            string variableLob = "Variable:/CELL/CYTOPLASM:P0:Value";
            string systemLog = "System:/:CELL:Size";
            
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding(51932);
            int jobID = 0;
            double count = 1.0;
            ScriptWriter writer = new ScriptWriter(_unitUnderTest.CurrentProject);
            List<SaveLoggerProperty> m_logList = new List<SaveLoggerProperty>();
            Dictionary<string, double> paramDic = new Dictionary<string, double>();
            m_logList.Add(new SaveLoggerProperty(processLog, 0.0, 0.0, TestConstant.TestDirectory));
            m_logList.Add(new SaveLoggerProperty(variableLob, 0.0, 0.0, TestConstant.TestDirectory));            
            paramDic.Add(processLog, 10.0);
            paramDic.Add(variableLob, 10.0);
            paramDic.Add(systemLog, 0.1);

            if (File.Exists(TestConstant.Script_File))
                File.Delete(TestConstant.Script_File);


            string fileName = TestConstant.Script_File;
            File.WriteAllText(fileName, "", enc);
            writer.WriteModelEntryUnix(fileName, enc, jobID);            
            foreach (EcellObject sysObj in _unitUnderTest.CurrentProject.SystemList)
            {
                EcellObject tmpObj = sysObj.Clone();
                foreach (string path in paramDic.Keys)
                {
                    foreach (EcellObject obj in tmpObj.Children)
                    {
                        if (obj.Value == null) continue;
                        foreach (EcellData v in obj.Value)
                        {
                            if (!path.Equals(v.EntityPath))
                                continue;
                            v.Value = new EcellValue(paramDic[path]);
                            writer.WriteComponentPropertyUnix(fileName, enc, obj, v);
                            break;
                        }
                    }
                }
            }
                        List<string> sList = new List<string>();
            foreach (SaveLoggerProperty s in m_logList)
            {
                sList.Add(s.FullPath);
            }

            writer.WriteLoggerPropertyUnix(fileName, enc, sList);
            writer.WriteSimulationForStepUnix(fileName, (int)(count), enc);
            writer.WriteSimulationForTimeUnix(fileName, count, enc);
            writer.WriteLoggerSaveEntryUnix(fileName, enc, jobID,
                m_logList,  TestConstant.TestDirectory);
            writer.WriteLoggerSaveEntryUnix(fileName, enc, jobID,
                m_logList, null);
            writer.WriteLoggerSaveEntryUnix(fileName, enc, jobID,
                null, TestConstant.TestDirectory);
        }

        /// <summary>
        /// TestScriptWriterForUnix
        /// </summary>
        [Test()]
        public void TestScriptWriterForWindows()
        {
            // Load Drosophila
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string processLog = "Process:/CELL/CYTOPLASM:R_toy10:Activity";
            string variableLob = "Variable:/CELL/CYTOPLASM:P0:Value";
            string systemLog = "System:/:CELL:SIZE";

            System.Text.Encoding enc = System.Text.Encoding.GetEncoding(51932);
            double count = 1.0;
            ScriptWriter writer = new ScriptWriter(_unitUnderTest.CurrentProject);
            List<SaveLoggerProperty> m_logList = new List<SaveLoggerProperty>();
            Dictionary<string, double> paramDic = new Dictionary<string, double>();
            m_logList.Add(new SaveLoggerProperty(processLog, 0.0, 0.0, TestConstant.TestDirectory));
            m_logList.Add(new SaveLoggerProperty(variableLob, 0.0, 0.0, TestConstant.TestDirectory));
            paramDic.Add(processLog, 10.0);
            paramDic.Add(variableLob, 10.0);
            paramDic.Add(systemLog, 0.1);

            if (File.Exists(TestConstant.Script_File))
                File.Delete(TestConstant.Script_File);


            string fileName = TestConstant.Script_File;
            File.WriteAllText(fileName, "", enc);            
            writer.WriteModelEntry(fileName, enc, "Drosophila", _unitUnderTest.CurrentProject.StepperDic["Drosophila"]);
            foreach (EcellObject sysObj in _unitUnderTest.CurrentProject.SystemList)
            {
                EcellObject tmpObj = sysObj.Clone();
                foreach (string path in paramDic.Keys)
                {
                    foreach (EcellObject obj in tmpObj.Children)
                    {
                        if (obj.Value == null) continue;
                        foreach (EcellData v in obj.Value)
                        {
                            if (!path.Equals(v.EntityPath))
                                continue;
                            v.Value = new EcellValue(paramDic[path]);
                            break;
                        }
                    }
                }

                writer.WriteComponentEntry(fileName, enc, tmpObj);
                writer.WriteComponentProperty(fileName, enc, tmpObj);
                if (tmpObj.Key.Equals("/"))
                {
                    tmpObj.Children = null;
                    writer.WriteComponentEntry(fileName, enc, tmpObj);
                    writer.WriteComponentProperty(fileName, enc, tmpObj);
                }
            }
            List<string> sList = new List<string>();
            foreach (SaveLoggerProperty s in m_logList)
            {
                sList.Add(s.FullPath);
            }

            writer.WriteLoggerProperty(fileName, enc, sList);
            writer.WriteLoggerProperty(fileName, enc, null);
            writer.WriteSimulationForStep(fileName, (int)(count), enc);
            writer.WriteSimulationForTime(fileName, count, enc);             
            writer.WriteLoggerSaveEntry(fileName, enc, m_logList);
            writer.WriteLoggerSaveEntry(fileName, enc, null);
        }

    }
}
