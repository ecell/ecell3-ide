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

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestJob
    {

        private Job _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new Job();
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
        public void TestNextJobID()
        {
            Job.ClearJobID();

            int expectedInt32 = 1;
            int resultInt32 = 0;
            resultInt32 = Job.NextJobID();
            Assert.AreEqual(expectedInt32, resultInt32, "NextJobID method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClearJobID()
        {
            Job.ClearJobID();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorJob()
        {
            Job.ClearJobID();

            Job testJob = new Job();
            Assert.IsNotNull(testJob, "Constructor of type, Job failed to create instance.");
            Assert.AreEqual(1, testJob.JobID, "JobID is unexpected value.");
            Assert.AreEqual(-1, testJob.ProcessID, "ProcessID is unexpected value.");
            Assert.AreEqual(-1, testJob.ProcessID, "ProcessID is unexpected value.");
            Assert.AreEqual(JobStatus.NONE, testJob.Status, "Status is unexpected value.");
            Assert.IsNotNull(testJob.Machine, "Machine is unexpected value.");
            Assert.IsEmpty(testJob.Argument, "Argument is unexpected value.");
            Assert.IsEmpty(testJob.ScriptFile, "ScriptFile is unexpected value.");
            Assert.IsEmpty(testJob.JobDirectory, "JobDirectory is unexpected value.");
            Assert.IsNull(testJob.StdErr, "StdErr is unexpected value.");
            Assert.IsEmpty(testJob.ExtraFileList, "ExtraFileList is unexpected value.");

            testJob.Argument = "Error";
            Assert.AreEqual("Error", testJob.Argument, "Argument is unexpected value.");

            testJob.StdErr = "Error";
            Assert.AreEqual("Error", testJob.StdErr, "StdErr is unexpected value.");
            
            testJob.Machine = "Local";
            Assert.AreEqual("Local", testJob.Machine, "Machine is unexpected value.");

            Assert.AreEqual(0, Job.MaxCount, "MaxCount is unexpected value.");
            Job.MaxCount = 100;
            Assert.AreEqual(100, Job.MaxCount, "MaxCount is unexpected value.");

            Assert.AreEqual(null, Job.DMPATH, "DMPATH is unexpected value.");
            Job.DMPATH = Util.GetDMDirs(null)[0];
            Assert.AreEqual(Util.GetDMDirs(null)[0], Job.DMPATH, "DMPATH is unexpected value.");

            testJob.JobDirectory = Util.GetTmpDir();
            Assert.AreEqual(Util.GetTmpDir(), testJob.JobDirectory, "JobDirectory is unexpected value.");

            testJob.ScriptFile = TestConstant.TestDirectory + "0.ess";
            Assert.AreEqual(TestConstant.TestDirectory + "0.ess", testJob.ScriptFile, "ScriptFile is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorJobExeFileArgExtFileTmpDir()
        {
            string exeFile = null;
            string arg = null;
            System.Collections.Generic.List<System.String> extFile = null;
            string tmpDir = null;
            Job testJob = new Job(exeFile, arg, extFile, tmpDir);
            Assert.IsNotNull(testJob, "Constructor of type, Job failed to create instance.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void Testretry()
        {
            _unitUnderTest.retry();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void Testrun()
        {
            _unitUnderTest.run();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void Teststop()
        {
            _unitUnderTest.stop();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdate()
        {
            _unitUnderTest.Update();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetJudge()
        {
            string judgeFile = null;
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetJudge(judgeFile);
            Assert.AreEqual(expectedInt32, resultInt32, "GetJudge method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLogData()
        {
            string key = null;
            Dictionary<double, double> expectedDictionary = new Dictionary<double,double>();
            Dictionary<double, double> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetLogData(key);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetLogData method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStdOut()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdOut();
            Assert.AreEqual(expectedString, resultString, "GetStdOut method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStdErr()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdErr();
            Assert.AreEqual(expectedString, resultString, "GetStdErr method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestPrepareProcess()
        {
            _unitUnderTest.PrepareProcess();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClear()
        {
            string folder = TestConstant.TestDirectory + "TestFolder";
            string file = folder + "/test.txt";
            Directory.CreateDirectory(folder);
            StreamWriter writer = File.CreateText(file);
            writer.Write("Test");
            writer.Close();

            List<string> files = new List<string>();
            files.Add(file);

            _unitUnderTest.JobDirectory = folder;
            _unitUnderTest.ExtraFileList = files;
            _unitUnderTest.Clear();
        }
    }
}
