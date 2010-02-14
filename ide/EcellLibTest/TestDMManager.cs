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

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;

namespace Ecell
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestDMManager
    {
        private DynamicModuleManager _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            ApplicationEnvironment env = new ApplicationEnvironment();
            _unitUnderTest = new DynamicModuleManager(env);
        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        /// <summary>
        /// Test Get DynamicModules
        /// </summary>
        [Test()]
        public void TestGetDynamicModule()
        {
            DynamicModule module;
            module = _unitUnderTest.ModuleDic["ConstantFluxProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["DecayFluxProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["ExpressionAlgebraicProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["ExpressionFluxProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["GillespieProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["GMAProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["MassActionFluxProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["MichaelisUniUniFluxProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["PythonFluxProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["PythonProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["QuasiDynamicFluxProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["SSystemProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["DAEStepper"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["ESSYNSStepper"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["FixedDAE1Stepper"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["FixedODE1Stepper"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["FluxDistributionStepper"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["ODE23Stepper"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["ODE45Stepper"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["ODEStepper"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["TauLeapProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["PingPongBiBiFluxProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["ExpressionAssignmentProcess"];
            Assert.IsNotNull(module);
            module = _unitUnderTest.ModuleDic["TauLeapStepper"];
            Assert.IsNotNull(module);
        }

        /// <summary>
        /// TestGetDynamicModule Error case
        /// </summary>
        [Test()]
        public void TestGetDynamicModuleErrorCase()
        {
            try
            {
                DynamicModule module;
                module = _unitUnderTest.ModuleDic["UnknownProcess"];
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// TestDynamicModules
        /// </summary>
        [Test()]
        public void TestDynamicModules()
        {
            TestDynamicModule("ConstantFluxProcess");
            TestDynamicModule("DecayFluxProcess");
            TestDynamicModule("ExpressionAlgebraicProcess");
            TestDynamicModule("ExpressionFluxProcess");
            TestDynamicModule("GillespieProcess");
            TestDynamicModule("GMAProcess");
            TestDynamicModule("MassActionFluxProcess");
            TestDynamicModule("MichaelisUniUniFluxProcess");
            TestDynamicModule("PythonFluxProcess");
            TestDynamicModule("PythonProcess");
            TestDynamicModule("QuasiDynamicFluxProcess");
            TestDynamicModule("SSystemProcess");
            TestDynamicModule("DAEStepper");
            TestDynamicModule("ESSYNSStepper");
            TestDynamicModule("FixedDAE1Stepper");
            TestDynamicModule("FixedODE1Stepper");
            TestDynamicModule("FluxDistributionStepper");
            TestDynamicModule("ODE23Stepper");
            TestDynamicModule("ODE45Stepper");
            TestDynamicModule("ODEStepper");
            TestDynamicModule("TauLeapProcess");
            TestDynamicModule("PingPongBiBiFluxProcess");
            TestDynamicModule("ExpressionAssignmentProcess");
            TestDynamicModule("TauLeapStepper");
        }

        private void TestDynamicModule(string dmName)
        {
            DynamicModule module;
            try
            {
                module = _unitUnderTest.ModuleDic[dmName];
                Assert.IsNotNull(module);

                Assert.AreEqual(module.Name, dmName);
                Assert.AreEqual(module.Description, dmName);
                Assert.AreEqual(module.Path, "");
                Assert.AreEqual(module.IsProjectDM, false);
                Assert.IsNotEmpty(module.Property);
                Trace.WriteLine("DynamicModule:" + module.Name);
                foreach (DynamicModuleProperty prop in module.Property.Values)
                {
                    Trace.WriteLine("PropertyName:" + prop.Name);
                    Trace.WriteLine("PropertyType:" + prop.Type);
                    Trace.WriteLine("IsGettable  :" + prop.IsGettable);
                    Trace.WriteLine("IsLoadable  :" + prop.IsLoadable);
                    Trace.WriteLine("IsSavable   :" + prop.IsSavable);
                    Trace.WriteLine("IsSettable  :" + prop.IsSettable);
                    Trace.WriteLine("DefaultData :" + prop.DefaultData);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }
    }
}
