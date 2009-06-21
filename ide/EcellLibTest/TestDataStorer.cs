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

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using EcellCoreLib;
using Ecell.Objects;

namespace Ecell
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestDataStorer
    {
        private ApplicationEnvironment _env;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _env = null;
        }
        
        /// <summary>
        /// TestDataStored
        /// </summary>
        [Test()]
        public void TestDataLoad()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
        }
        
        /// <summary>
        /// TestDataStored
        /// </summary>
        [Test()]
        public void TestDataStored()
        {
            DataStorer dataStorer = new DataStorer();
            Assert.IsNotNull(dataStorer, "Constructor of type, DataStorer failed to create instance.");

            Type type = dataStorer.GetType();
            MethodInfo methodInfo = type.GetMethod("DataStored", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(methodInfo, "GetMethod method returned unexpected value.");

            WrappedSimulator simulator = new WrappedSimulator(Util.GetDMDirs(null));
            DMDescriptorKeeper dmm = _env.DMDescriptorKeeper;
            EcellObject eo = EcellObject.CreateObject("Model", "/", EcellObject.SYSTEM, EcellObject.SYSTEM, new List<EcellData>());
            Dictionary<string, double> initialCondition = new Dictionary<string, double>();
            methodInfo.Invoke(dataStorer, new object[] { simulator, dmm, eo, initialCondition });
        }

        /// <summary>
        /// TestGetValueFromDMM
        /// </summary>
        [Test()]
        public void TestGetValueFromDMM()
        {
            DataStorer dataStorer = new DataStorer();
            Assert.IsNotNull(dataStorer, "Constructor of type, DataStorer failed to create instance.");

            Type type = dataStorer.GetType();
            MethodInfo methodInfo = type.GetMethod("GetValueFromDMM", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(methodInfo, "GetMethod method returned unexpected value.");

            DMDescriptorKeeper dmm = _env.DMDescriptorKeeper;
            string className = "ExpressionFluxProcess";
            string name = "Expression";
            EcellValue value = (EcellValue)methodInfo.Invoke(dataStorer, new object[] { dmm, Constants.xpathProcess, className, name });
            Assert.IsNotNull(value, "GetValueFromDM method returned unexpected value.");
            Assert.IsTrue(value.IsString, "IsString is unexpected value.");
            Assert.AreEqual("", (string)value, "GetValueFromDMM method returned unexpected result.");

            name = "newValue";
            value = (EcellValue)methodInfo.Invoke(dataStorer, new object[] { dmm, Constants.xpathProcess, className, name });
            Assert.IsNotNull(value, "GetValueFromDM method returned unexpected value.");
            Assert.IsTrue(value.IsDouble, "IsString is unexpected value.");
            Assert.AreEqual(0.0, (double)value, "GetValueFromDMM method returned unexpected result.");

            className = "Hoge";
            value = (EcellValue)methodInfo.Invoke(dataStorer, new object[] { dmm, Constants.xpathProcess, className, name });
            Assert.IsNotNull(value, "GetValueFromDM method returned unexpected value.");
            Assert.IsTrue(value.IsString, "IsString is unexpected value.");
            Assert.AreEqual("", (string)value, "GetValueFromDMM method returned unexpected result.");

        }
        
        /// <summary>
        /// TestGetVariableValue
        /// </summary>
        [Test()]
        public void TestGetVariableValue()
        {
            DataStorer dataStorer = new DataStorer();
            Assert.IsNotNull(dataStorer, "Constructor of type, DataStorer failed to create instance.");

            Type type = dataStorer.GetType();
            MethodInfo methodInfo = type.GetMethod("GetVariableValue", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(methodInfo, "GetMethod method returned unexpected value.");

            WrappedSimulator simulator = new WrappedSimulator(Util.GetDMDirs(null));
            string name = "Value";
            string entityPath = "Variable:/:V1:Value";

            EcellValue value = (EcellValue)methodInfo.Invoke(dataStorer, new object[] { simulator, name, entityPath });
            Assert.IsNotNull(methodInfo, "GetMethod method returned unexpected value.");
            Assert.AreEqual(0.0, (double)value, "GetValueFromDMM method returned unexpected result.");

            name = "Fixed";
            value = (EcellValue)methodInfo.Invoke(dataStorer, new object[] { simulator, name, entityPath });
            Assert.IsNotNull(methodInfo, "GetMethod method returned unexpected value.");
            Assert.AreEqual(0.0, (double)value, "GetValueFromDMM method returned unexpected result.");

        }
    }
}
