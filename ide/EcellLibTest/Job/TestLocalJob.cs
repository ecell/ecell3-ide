//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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

namespace Ecell.Job
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestLocalJob
    {

        private LocalJob _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new LocalJob();
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
        public void TestConstructorLocalJob()
        {
            Job.ClearJobID();

            LocalJob testLocalJob = new LocalJob();
            Assert.IsNotNull(testLocalJob, "Constructor of type, LocalJob failed to create instance.");
            Assert.AreEqual(1, testLocalJob.JobID, "JobID is unexpected value.");
            Assert.AreEqual(-1, testLocalJob.ProcessID, "ProcessID is unexpected value.");
            Assert.AreEqual(JobStatus.NONE, testLocalJob.Status, "Status is unexpected value.");
            Assert.AreEqual("Local", testLocalJob.Machine, "Machine is unexpected value.");
            Assert.IsEmpty(testLocalJob.Argument, "Argument is unexpected value.");
            Assert.IsEmpty(testLocalJob.ScriptFile, "ScriptFile is unexpected value.");
            Assert.IsEmpty(testLocalJob.JobDirectory, "JobDirectory is unexpected value.");
            Assert.IsNull(testLocalJob.StdErr, "StdErr is unexpected value.");
            Assert.IsEmpty(testLocalJob.ExtraFileList, "ExtraFileList is unexpected value.");
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
            Assert.AreEqual(JobStatus.ERROR, _unitUnderTest.Status, "");

            _unitUnderTest.ScriptFile = "c:/temp/0.ess";
            _unitUnderTest.run();
            Assert.AreEqual(JobStatus.ERROR, _unitUnderTest.Status, "");
            _unitUnderTest.stop();
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
        public void TestGetStdOut()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdOut();
            Assert.AreEqual(expectedString, resultString, "GetStdOut method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

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
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestPrepareProcess()
        {
            _unitUnderTest.PrepareProcess();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLogData()
        {
            string key = null;
            System.Collections.Generic.Dictionary<System.Double, System.Double> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.Double, System.Double> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetLogData(key);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetLogData method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDefaultScript()
        {
            string expectedString = Util.GetAnalysisDir() + "/ipy.exe";
            string resultString = null;
            resultString = LocalJob.GetDefaultScript();
            Assert.AreEqual(expectedString, resultString, "GetDefaultScript method returned unexpected result.");
        }
    }
}
