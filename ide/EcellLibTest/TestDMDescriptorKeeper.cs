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

namespace Ecell
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestDMDescriptorKeeper
    {
        private DMDescriptorKeeper _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            string[] path = Util.GetDMDirs(null);
            _unitUnderTest = new DMDescriptorKeeper(path);
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
            _unitUnderTest.Load();

            DMDescriptor module;
            module = _unitUnderTest.GetDMDescriptor("Process", "ConstantFluxProcess");
            Assert.IsNotNull(module);
            Assert.IsNotNull(module.Name);
            Assert.IsNotNull(module["Name"]);
            Assert.IsNotNull(module.Type);
            Assert.IsNull(module.Description);
            Assert.IsNotNull(module.Path);
            Assert.IsFalse(module.CanHaveDynamicProperties);
            Assert.AreNotEqual(0, module.GetHashCode());

            module = _unitUnderTest.GetDMDescriptor("Process", "DecayFluxProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "ExpressionAlgebraicProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "ExpressionFluxProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "GillespieProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "GMAProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "MassActionFluxProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "MichaelisUniUniFluxProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "PythonFluxProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "PythonProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "QuasiDynamicFluxProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "SSystemProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "TauLeapProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "PingPongBiBiFluxProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Process", "ExpressionAssignmentProcess");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "DAEStepper");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "ESSYNSStepper");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "FixedDAE1Stepper");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "FixedODE1Stepper");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "FluxDistributionStepper");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "ODE23Stepper");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "ODE45Stepper");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "ODEStepper");
            Assert.IsNotNull(module);
            module = _unitUnderTest.GetDMDescriptor("Stepper", "TauLeapStepper");
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
                DMDescriptor module;
                module = _unitUnderTest.GetDMDescriptor("Process", "UnknownProcess");
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// TestGetDMDescriptor
        /// </summary>
        [Test()]
        public void TestGetDMDescriptors()
        {
            _unitUnderTest.Load();
            ICollection<DMDescriptor> list = _unitUnderTest.GetDMDescriptors("Process");

        }
        /// <summary>
        /// TestGetDMDescriptor
        /// </summary>
        [Test()]
        public void TestGetDMDescriptor()
        {
            _unitUnderTest.Load();

            // Check BuiltIn DMs.
            TestDMDescriptor("System", "System");
            TestDMDescriptor("Variable", "Variable");
            TestDMDescriptor("Stepper", "DiscreteEventStepper");
            TestDMDescriptor("Stepper", "DiscreteTimeStepper");
            TestDMDescriptor("Stepper", "PassiveStepper");

            // Check Default DMs.
            TestDMDescriptor("Process", "ConstantFluxProcess");
            TestDMDescriptor("Process", "DecayFluxProcess");
            TestDMDescriptor("Process", "ExpressionAlgebraicProcess");
            TestDMDescriptor("Process", "ExpressionAssignmentProcess");
            TestDMDescriptor("Process", "ExpressionFluxProcess");
            TestDMDescriptor("Process", "GillespieProcess");
            TestDMDescriptor("Process", "GMAProcess");
            TestDMDescriptor("Process", "MassActionFluxProcess");
            TestDMDescriptor("Process", "MichaelisUniUniFluxProcess");
            TestDMDescriptor("Process", "PingPongBiBiFluxProcess");
            TestDMDescriptor("Process", "PythonFluxProcess");
            TestDMDescriptor("Process", "PythonProcess");
            TestDMDescriptor("Process", "QuasiDynamicFluxProcess");
            TestDMDescriptor("Process", "SSystemProcess");
            TestDMDescriptor("Process", "TauLeapProcess");
            TestDMDescriptor("Stepper", "DAEStepper");
            TestDMDescriptor("Stepper", "ESSYNSStepper");
            TestDMDescriptor("Stepper", "FixedDAE1Stepper");
            TestDMDescriptor("Stepper", "FixedODE1Stepper");
            TestDMDescriptor("Stepper", "FluxDistributionStepper");
            TestDMDescriptor("Stepper", "ODE23Stepper");
            TestDMDescriptor("Stepper", "ODE45Stepper");
            TestDMDescriptor("Stepper", "ODEStepper");
            TestDMDescriptor("Stepper", "TauLeapStepper");
        }

        private void TestDMDescriptor(string type, string dmName)
        {
            DMDescriptor module;
            Trace.WriteLine("");

            PropertyDescriptor dmd = new PropertyDescriptor("Name", false, false, false, false, false, false, new Ecell.Objects.EcellValue(1));

            try
            {
                module = _unitUnderTest.GetDMDescriptor(type, dmName);
                Assert.IsNotNull(module);

                Assert.AreEqual(module.Name, dmName);
                Assert.AreNotEqual(module.Path, "");
                Trace.WriteLine("DynamicModule:" + module.Name);
                foreach (PropertyDescriptor prop in module)
                {
                    Trace.WriteLine("PropertyName:" + prop.Name);
                    Trace.WriteLine("IsGettable  :" + prop.Gettable);
                    Trace.WriteLine("IsLoadable  :" + prop.Loadable);
                    Trace.WriteLine("IsLoadable  :" + prop.Logable);
                    Trace.WriteLine("IsSavable   :" + prop.Saveable);
                    Trace.WriteLine("IsSettable  :" + prop.Settable);
                    Trace.WriteLine("IsDynamic  :" + prop.Dynamic);
                    if (prop.DefaultValue == null)
                        Trace.WriteLine("DefaultValue: null");
                    else
                        Trace.WriteLine("DefaultValue:" + prop.DefaultValue);
                    Trace.WriteLine("HashCode :" + prop.GetHashCode());
                    Trace.WriteLine("");
                    Assert.AreEqual(false, prop.Equals(null));
                    Assert.AreEqual(false, prop.Equals(dmd));
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Type:" + type + " Name:" + dmName);
                Trace.WriteLine(e.StackTrace);
                Assert.Fail();
            }
        }
    }
}
