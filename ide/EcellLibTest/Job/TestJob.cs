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


    [TestFixture()]
    public class TestJob
    {

        private Job _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new Job();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestNextJobID()
        {
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = Job.NextJobID();
            Assert.AreEqual(expectedInt32, resultInt32, "NextJobID method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClearJobID()
        {
            Job.ClearJobID();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestConstructorJob()
        {
            Job testJob = new Job();
            Assert.IsNotNull(testJob, "Constructor of type, Job failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestConstructorJobExeFileArgExtFileTmpDir()
        {
            string exeFile = null;
            string arg = null;
            System.Collections.Generic.List<System.String> extFile = null;
            string tmpDir = null;
            Job testJob = new Job(exeFile, arg, extFile, tmpDir);
            Assert.IsNotNull(testJob, "Constructor of type, Job failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void Testretry()
        {
            _unitUnderTest.retry();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void Testrun()
        {
            _unitUnderTest.run();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void Teststop()
        {
            _unitUnderTest.stop();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestUpdate()
        {
            _unitUnderTest.Update();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetJudge()
        {
            string judgeFile = null;
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetJudge(judgeFile);
            Assert.AreEqual(expectedInt32, resultInt32, "GetJudge method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

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

        [Test()]
        public void TestGetStdOut()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdOut();
            Assert.AreEqual(expectedString, resultString, "GetStdOut method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetStdErr()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdErr();
            Assert.AreEqual(expectedString, resultString, "GetStdErr method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestPrepareProcess()
        {
            _unitUnderTest.PrepareProcess();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Clear();
            Assert.Fail("Create or modify test(s).");

        }
    }
}
