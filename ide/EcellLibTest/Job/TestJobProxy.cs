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

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestJobProxy
    {
        private ApplicationEnvironment _env;
        private JobProxy _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = new JobProxy();
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
        public void TestConstructor()
        {
            JobProxy jobProxy = new JobProxy();
            Assert.IsNotNull(jobProxy, "Constructor of type, JobProxy failed to create instance.");
            Assert.IsNull(jobProxy.Name, "Name is unexpected value.");

            Assert.AreEqual(0, jobProxy.DefaultConcurrency, "DefaultConcurrency is unexpected value.");
            jobProxy.DefaultConcurrency = 100;
            Assert.AreEqual(100, jobProxy.DefaultConcurrency, "DefaultConcurrency is unexpected value.");

            Assert.IsNull(jobProxy.Manager, "Manager is unexpected value.");
            jobProxy.Manager = (JobManager)_env.JobManager;
            Assert.AreEqual(_env.JobManager, jobProxy.Manager, "Manager is unexpected value.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateJob()
        {
            Job expectedJob = null;
            Job resultJob = null;
            resultJob = _unitUnderTest.CreateJob();
            Assert.AreEqual(expectedJob, resultJob, "CreateJob method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateJobScriptArgExtFileTmpDir()
        {
            string script = null;
            string arg = null;
            System.Collections.Generic.List<System.String> extFile = null;
            string tmpDir = null;
            Ecell.Job.Job expectedJob = null;
            Ecell.Job.Job resultJob = null;
            resultJob = _unitUnderTest.CreateJob(script, arg, extFile, tmpDir);
            Assert.AreEqual(expectedJob, resultJob, "CreateJob method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetProperty()
        {
            System.Collections.Generic.Dictionary<System.String, System.Object> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Object> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetProperty();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetProperty method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDefaultProperty()
        {
            System.Collections.Generic.Dictionary<System.String, System.Object> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Object> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetDefaultProperty();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetDefaultProperty method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetProperty()
        {
            System.Collections.Generic.Dictionary<System.String, System.Object> list = null;
            _unitUnderTest.SetProperty(list);

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
        public void TestIsIDE()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsIDE();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsIDE method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDefaultScript()
        {
            string expectedString = "";
            string resultString = null;
            resultString = _unitUnderTest.GetDefaultScript();
            Assert.AreEqual(expectedString, resultString, "GetDefaultScript method returned unexpected result.");

        }
    }
}
