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


namespace Ecell.Job
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.IO;
    using Ecell.Objects;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestJobGroup
    {
        private ApplicationEnvironment _env;
        private JobManager _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = new JobManager(_env);
        }
        /// <summary>
        /// 
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAccessors()
        {
            // Load plugins
            foreach (string pluginDir in Util.GetPluginDirs())
            {
                string[] files = Directory.GetFiles(
                    pluginDir,
                    Constants.delimiterWildcard + Constants.FileExtPlugin);
                foreach (string fileName in files)
                {
                    _env.PluginManager.LoadPlugin(fileName);
                }
            }

            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            foreach (string name in _env.JobManager.GroupDic.Keys)
            {
                JobGroup g = _env.JobManager.GroupDic[name];
                Assert.IsNotNull(g.AnalysisModule, "Job Group return unexpected value.");
                Assert.AreEqual(name, g.GroupName, "Job Group return unexpected value.");

                string orgName = g.AnalysisName;
                string expectedValie = "AAAAA";
                g.AnalysisName = expectedValie;
                Assert.AreEqual(g.AnalysisName, expectedValie, "Job Group return unexpected value.");
                g.AnalysisName = orgName;

                orgName = g.DateString;
                g.DateString = expectedValie;
                Assert.AreEqual(g.DateString, expectedValie, "Job Group return unexpected value.");
                g.DateString = orgName;

                orgName = g.TopDir;
                g.TopDir = expectedValie;
                Assert.AreEqual(g.TopDir, expectedValie, "Job Group return unexpected value.");
                g.TopDir = orgName;

                List<EcellObject> orgList = g.SystemObjectList;
                g.SystemObjectList = new List<EcellObject>();
                Assert.AreEqual(0, g.SystemObjectList.Count, "Job Group return unexpected value.");
                g.SystemObjectList = orgList;

                orgList = g.StepperObjectList;
                g.StepperObjectList = new List<EcellObject>();
                Assert.AreEqual(0, g.StepperObjectList.Count, "Job Group return unexpected value.");
                g.StepperObjectList = orgList;

                Dictionary<string, string> param = g.AnalysisParameter;
                g.AnalysisParameter = param;

                List<Job> jobs = g.Jobs;
                g.Jobs = jobs;

                g.IsGroupError = true;
                g.UpdateStatus();

                Assert.IsTrue(g.IsFinished(), "Job Group return unexpected value.");

                g.Status = AnalysisStatus.Stopped;
                Assert.AreEqual(g.Status, AnalysisStatus.Stopped, "Job Group return unexpected value.");

                g.DeleteJob(1);

                Assert.IsNull(g.GetJob(1), "Job Group return unexpected value.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestJobGroupSaveAndLoad()
        {
            // Load plugins
            foreach (string pluginDir in Util.GetPluginDirs())
            {
                string[] files = Directory.GetFiles(
                    pluginDir,
                    Constants.delimiterWildcard + Constants.FileExtPlugin);
                foreach (string fileName in files)
                {
                    _env.PluginManager.LoadPlugin(fileName);
                }
            }

            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            foreach (string groupName in _env.JobManager.GroupDic.Keys)
            {
                string topdir = TestConstant.TestDirectory + "Drosophila/Analysis/" + groupName;
                _env.JobManager.GroupDic[groupName].SaveJobGroup(topdir);
            }
        }

        [Test()]
        public void TestJobGroupDeleteJob()
        {
            // Load plugins
            foreach (string pluginDir in Util.GetPluginDirs())
            {
                string[] files = Directory.GetFiles(
                    pluginDir,
                    Constants.delimiterWildcard + Constants.FileExtPlugin);
                foreach (string fileName in files)
                {
                    _env.PluginManager.LoadPlugin(fileName);
                }
            }

            JobGroup g = _env.JobManager.CreateJobGroup("RobustAnalysis", new List<EcellObject>(), new List<EcellObject>());

            g.Delete();

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestJobStatusConvert()
        {
            JobStatus returnValue = JobParameterConsts.IntConvert(1);
            JobStatus expectedValue = JobStatus.FINISHED;
            Assert.AreEqual(returnValue, expectedValue, "JobParameterConstants.IntConvert is unexpected value.");

            returnValue = JobParameterConsts.IntConvert(2);
            expectedValue = JobStatus.ERROR;
            Assert.AreEqual(returnValue, expectedValue, "JobParameterConstants.IntConvert is unexpected value.");

            returnValue = JobParameterConsts.IntConvert(3);
            expectedValue = JobStatus.QUEUED;
            Assert.AreEqual(returnValue, expectedValue, "JobParameterConstants.IntConvert is unexpected value.");

            returnValue = JobParameterConsts.IntConvert(4);
            expectedValue = JobStatus.RUNNING;
            Assert.AreEqual(returnValue, expectedValue, "JobParameterConstants.IntConvert is unexpected value.");

            returnValue = JobParameterConsts.IntConvert(5);
            expectedValue = JobStatus.STOPPED;
            Assert.AreEqual(returnValue, expectedValue, "JobParameterConstants.IntConvert is unexpected value.");

            returnValue = JobParameterConsts.IntConvert(6);
            expectedValue = JobStatus.NONE;
            Assert.AreEqual(returnValue, expectedValue, "JobParameterConstants.IntConvert is unexpected value.");

            int expectedData = 1;
            int returnData = JobParameterConsts.TypeConvert(JobStatus.FINISHED);
            Assert.AreEqual(returnData, expectedData, "JobParameterConstants.TypeConvert is unexpected value.");

            expectedData = 2;
            returnData = JobParameterConsts.TypeConvert(JobStatus.ERROR);
            Assert.AreEqual(returnData, expectedData, "JobParameterConstants.TypeConvert is unexpected value.");

            expectedData = 3;
            returnData = JobParameterConsts.TypeConvert(JobStatus.QUEUED);
            Assert.AreEqual(returnData, expectedData, "JobParameterConstants.TypeConvert is unexpected value.");

            expectedData = 4;
            returnData = JobParameterConsts.TypeConvert(JobStatus.RUNNING);
            Assert.AreEqual(returnData, expectedData, "JobParameterConstants.TypeConvert is unexpected value.");

            expectedData = 5;
            returnData = JobParameterConsts.TypeConvert(JobStatus.STOPPED);
            Assert.AreEqual(returnData, expectedData, "JobParameterConstants.TypeConvert is unexpected value.");

            expectedData = -1;
            returnData = JobParameterConsts.TypeConvert(JobStatus.NONE);
            Assert.AreEqual(returnData, expectedData, "JobParameterConstants.TypeConvert is unexpected value.");
        }
    }
}
