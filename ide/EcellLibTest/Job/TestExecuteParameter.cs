//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestExecuteParameter
    {

        private ExecuteParameter _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new ExecuteParameter();
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
        public void TestConstructorExecuteParameter()
        {
            ExecuteParameter testExecuteParameter = new ExecuteParameter();
            Assert.IsNotNull(testExecuteParameter, "Constructor of type, ExecuteParameter failed to create instance.");
            Assert.IsNotNull(testExecuteParameter.ParamDic, "ParamDic is unexpected value.");
            Assert.IsEmpty(testExecuteParameter.ParamDic, "ParamDic is unexpected value.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorExecuteParameterData()
        {
            Dictionary<string, double> data = new Dictionary<string,double>();
            ExecuteParameter testExecuteParameter = new ExecuteParameter(data);
            Assert.IsNotNull(testExecuteParameter, "Constructor of type, ExecuteParameter failed to create instance.");
            Assert.IsNotNull(testExecuteParameter.ParamDic, "ParamDic is unexpected value.");
            Assert.IsEmpty(testExecuteParameter.ParamDic, "ParamDic is unexpected value.");

            data.Add("Variable:/:S:Value", 0.0);
            testExecuteParameter = new ExecuteParameter(data);
            Assert.IsNotNull(testExecuteParameter, "Constructor of type, ExecuteParameter failed to create instance.");
            Assert.IsNotNull(testExecuteParameter.ParamDic, "ParamDic is unexpected value.");
            Assert.IsNotEmpty(testExecuteParameter.ParamDic, "ParamDic is unexpected value.");

            testExecuteParameter = new ExecuteParameter();
            testExecuteParameter.ParamDic = data;
            Assert.IsNotNull(testExecuteParameter, "Constructor of type, ExecuteParameter failed to create instance.");
            Assert.IsNotNull(testExecuteParameter.ParamDic, "ParamDic is unexpected value.");
            Assert.IsNotEmpty(testExecuteParameter.ParamDic, "ParamDic is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddParameter()
        {
            string path = "Variable:/:S:Value";
            double value = 0.1;
            _unitUnderTest.AddParameter(path, value);
            _unitUnderTest.RemoveParameter(path);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetParameter()
        {
            string path = "Variable:/:S:Value";
            double expectedDouble = 0.0;
            double resultDouble = 0.0;

            resultDouble = _unitUnderTest.GetParameter(path);
            Assert.AreEqual(expectedDouble, resultDouble, "GetParameter method returned unexpected result.");

            expectedDouble = 0.1;
            _unitUnderTest.AddParameter(path, expectedDouble);
            resultDouble = _unitUnderTest.GetParameter(path);
            _unitUnderTest.RemoveParameter(path);
            Assert.AreEqual(expectedDouble, resultDouble, "GetParameter method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRemoveParameter()
        {
            string path = "Variable:/:S:Value";
            double value = 0.1;
            _unitUnderTest.AddParameter(path, value);
            _unitUnderTest.RemoveParameter(path);

        }
    }
}
