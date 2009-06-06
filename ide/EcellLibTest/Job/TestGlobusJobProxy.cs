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
    public class TestGlobusJobProxy
    {
        private ApplicationEnvironment _env;
        private GlobusJobProxy _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = new GlobusJobProxy();
            _unitUnderTest.Manager = (JobManager)_env.JobManager;
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
        public void TestConstructorLocalJobProxy()
        {
            GlobusJobProxy jobProxy = new GlobusJobProxy();
            Assert.IsNotNull(jobProxy, "Constructor of type, LocalJobProxy failed to create instance.");
            Assert.AreEqual("Globus", jobProxy.Name, "Name is unexpected value.");

            Assert.AreEqual(4, jobProxy.Concurrency, "DefaultConcurrency is unexpected value.");
            jobProxy.Concurrency = 100;
            Assert.AreEqual(100, jobProxy.Concurrency, "DefaultConcurrency is unexpected value.");

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
            Ecell.Job.Job resultJob = null;
            resultJob = _unitUnderTest.CreateJob();
            Assert.IsNotNull(resultJob, "CreateJob method returned unexpected result.");

            resultJob = _unitUnderTest.CreateJob("", "", new List<string>(), "");
            Assert.IsNotNull(resultJob, "CreateJob method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdate()
        {
            JobGroup g = _unitUnderTest.Manager.CreateJobGroup("AAAA", new List<Ecell.Objects.EcellObject>(), new List<Ecell.Objects.EcellObject>());
            _unitUnderTest.Manager.CreateJobEntry(g.GroupName, new ExecuteParameter());
            foreach (Job job in _unitUnderTest.Manager.GetFinishedJobList())
                job.Status = JobStatus.QUEUED;
            _unitUnderTest.Update();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsIDE()
        {
            bool expectedBoolean = false;
            bool resultBoolean = true;
            resultBoolean = _unitUnderTest.IsIDE();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsIDE method returned unexpected result.");

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
        public void TestGetDefaultProperty()
        {
            Dictionary<string, object> resultDictionary = _unitUnderTest.GetDefaultProperty();
            Assert.AreEqual(5, resultDictionary.Count, "GetDefaultProperty method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetProperty()
        {
            Dictionary<string, object> resultDictionary = _unitUnderTest.GetProperty();
            Assert.AreEqual(5, resultDictionary.Count, "GetProperty method returned unexpected result.");

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
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDatat()
        {
            string expectedString = null;
            string resultString = _unitUnderTest.GetData("");
            Assert.AreEqual(expectedString, resultString, "GetDefaultScript method returned unexpected result.");

        }
    }
}
