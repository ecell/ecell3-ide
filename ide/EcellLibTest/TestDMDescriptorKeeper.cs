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
using EcellCoreLib;
using System.Collections;
using System.Reflection;

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
            string[] path = Util.GetDMDirs();
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
            DMDescriptor module;
            module = _unitUnderTest.GetDMDescriptor("Process", "ConstantFluxProcess");
            Assert.IsNotNull(module);
            Assert.IsNotNull(module.Name);
            Assert.IsNotNull(module["Name"]);
            Assert.IsNotNull(module.Type);
            Assert.IsNotNull(module.Description);
            Assert.IsNotNull(module.Path);
            Assert.IsFalse(module.CanHaveDynamicProperties);
            Assert.AreNotEqual(0, module.GetHashCode());

            module = _unitUnderTest.GetDMDescriptor("Process", "DecayFluxProcess");
            Assert.IsNotNull(module, "DecayFluxProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "ExpressionAlgebraicProcess");
            Assert.IsNotNull(module, "ExpressionAlgebraicProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "ExpressionFluxProcess");
            Assert.IsNotNull(module, "ExpressionFluxProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "GillespieProcess");
            Assert.IsNotNull(module, "GillespieProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "GMAProcess");
            Assert.IsNotNull(module, "GMAProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "MassActionFluxProcess");
            Assert.IsNotNull(module, "MassActionFluxProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "MichaelisUniUniFluxProcess");
            Assert.IsNotNull(module, "MichaelisUniUniFluxProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "QuasiDynamicFluxProcess");
            Assert.IsNotNull(module, "QuasiDynamicFluxProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "SSystemProcess");
            Assert.IsNotNull(module, "SSystemProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "TauLeapProcess");
            Assert.IsNotNull(module, "TauLeapProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "PingPongBiBiFluxProcess");
            Assert.IsNotNull(module, "PingPongBiBiFluxProcess");
            module = _unitUnderTest.GetDMDescriptor("Process", "ExpressionAssignmentProcess");
            Assert.IsNotNull(module, "ExpressionAssignmentProcess");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "DAEStepper");
            Assert.IsNotNull(module, "DAEStepper");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "ESSYNSStepper");
            Assert.IsNotNull(module, "ESSYNSStepper");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "FixedDAE1Stepper");
            Assert.IsNotNull(module, "FixedDAE1Stepper");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "FixedODE1Stepper");
            Assert.IsNotNull(module, "FixedODE1Stepper");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "FluxDistributionStepper");
            Assert.IsNotNull(module, "FluxDistributionStepper");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "ODE23Stepper");
            Assert.IsNotNull(module, "ODE23Stepper");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "ODE45Stepper");
            Assert.IsNotNull(module, "ODE45Stepper");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "ODEStepper");
            Assert.IsNotNull(module, "ODEStepper");
            module = _unitUnderTest.GetDMDescriptor("Stepper", "TauLeapStepper");
            Assert.IsNotNull(module, "TauLeapStepper");
            
            // following DMs are not included in installer.
            try
            {
                module = _unitUnderTest.GetDMDescriptor("Process", "PythonFluxProcess");
                Assert.IsNotNull(module, "PythonFluxProcess");
                module = _unitUnderTest.GetDMDescriptor("Process", "PythonProcess");
                Assert.IsNotNull(module, "PythonProcess");
            }
            catch (Exception)
            {
            }

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
            List<DMDescriptor> list = new List<DMDescriptor>();
            list = new List<DMDescriptor>(_unitUnderTest.GetDMDescriptors("Stepper"));
            Assert.IsNotEmpty(list, "Stepper DM is not exist.");

            list = new List<DMDescriptor>(_unitUnderTest.GetDMDescriptors("Process"));
            Assert.IsNotEmpty(list, "Process DM is not exist.");

            list = new List<DMDescriptor>(_unitUnderTest.GetDMDescriptors("System"));
            Assert.IsNotEmpty(list, "System DM is not exist.");

            list = new List<DMDescriptor>(_unitUnderTest.GetDMDescriptors("Variable"));
            Assert.IsNotEmpty(list, "Variable DM is not exist.");
        }
        
        /// <summary>
        /// TestGetDMDescriptor
        /// </summary>
        [Test()]
        public void TestContainsDescriptor()
        {
            bool exists = false;
            bool expected = true;

            // Built in
            exists = _unitUnderTest.ContainsDescriptor("System", "System");
            Assert.AreEqual(expected, exists, "System is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Variable", "Variable");
            Assert.AreEqual(expected, exists, "Variable is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "DiscreteEventStepper");
            Assert.AreEqual(expected, exists, "DiscreteEventStepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "DiscreteTimeStepper");
            Assert.AreEqual(expected, exists, "DiscreteTimeStepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "PassiveStepper");
            Assert.AreEqual(expected, exists, "PassiveStepper is not exists.");

            // Process
            exists = _unitUnderTest.ContainsDescriptor("Process", "ConstantFluxProcess");
            Assert.AreEqual(expected, exists, "ConstantFluxProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "DecayFluxProcess");
            Assert.AreEqual(expected, exists, "DecayFluxProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "ExpressionAlgebraicProcess");
            Assert.AreEqual(expected, exists, "ExpressionAlgebraicProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "ExpressionAssignmentProcess");
            Assert.AreEqual(expected, exists, "ExpressionAssignmentProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "ExpressionFluxProcess");
            Assert.AreEqual(expected, exists, " is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "GillespieProcess");
            Assert.AreEqual(expected, exists, "GillespieProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "GMAProcess");
            Assert.AreEqual(expected, exists, "GMAProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "MassActionFluxProcess");
            Assert.AreEqual(expected, exists, "MassActionFluxProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "MichaelisUniUniFluxProcess");
            Assert.AreEqual(expected, exists, "MichaelisUniUniFluxProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "PingPongBiBiFluxProcess");
            Assert.AreEqual(expected, exists, "PingPongBiBiFluxProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "QuasiDynamicFluxProcess");
            Assert.AreEqual(expected, exists, "QuasiDynamicFluxProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "SSystemProcess");
            Assert.AreEqual(expected, exists, "SSystemProcess is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Process", "TauLeapProcess");
            Assert.AreEqual(expected, exists, "TauLeapProcess is not exists.");

            // Stepper
            exists = _unitUnderTest.ContainsDescriptor("Stepper", "DAEStepper");
            Assert.AreEqual(expected, exists, "DAEStepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "ESSYNSStepper");
            Assert.AreEqual(expected, exists, "ESSYNSStepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "FixedDAE1Stepper");
            Assert.AreEqual(expected, exists, "FixedDAE1Stepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "FixedODE1Stepper");
            Assert.AreEqual(expected, exists, "FixedODE1Stepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "ODE23Stepper");
            Assert.AreEqual(expected, exists, "ODE23Stepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "ODE45Stepper");
            Assert.AreEqual(expected, exists, "ODE45Stepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "ODEStepper");
            Assert.AreEqual(expected, exists, "ODEStepper is not exists.");

            exists = _unitUnderTest.ContainsDescriptor("Stepper", "TauLeapStepper");
            Assert.AreEqual(expected, exists, "TauLeapStepper is not exists.");

            // following DMs are not included in installer.
            try
            {
                exists = _unitUnderTest.ContainsDescriptor("Process", "PythonFluxProcess");
                Assert.AreEqual(expected, exists, "PythonFluxProcess is not exists.");

                exists = _unitUnderTest.ContainsDescriptor("Process", "PythonProcess");
                Assert.AreEqual(expected, exists, "PythonProcess is not exists.");
            }
            catch (Exception)
            {
            }

            // Not exist.
            exists = _unitUnderTest.ContainsDescriptor("Process", "HogeProcess");
            Assert.AreEqual(false, exists, "ContainsDescriptor returns unexpected value.");

        }

        /// <summary>
        /// TestGetDMDescriptor
        /// </summary>
        [Test()]
        public void TestGetDMDescriptor()
        {
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


            // following DMs are not included in installer.
            try
            {
                TestDMDescriptor("Process", "PythonFluxProcess");
                TestDMDescriptor("Process", "PythonProcess");
            }
            catch (Exception)
            {
            }

        }

        private void TestDMDescriptor(string type, string dmName)
        {
            DMDescriptor module;
            Trace.WriteLine("");

            PropertyDescriptor dmd = new PropertyDescriptor("Name", false, false, false, false, false, false, new Ecell.Objects.EcellValue(1));

            try
            {
                module = _unitUnderTest.GetDMDescriptor(type, dmName);
                Assert.IsFalse(module.ContainsProperty("hoge"));
                Assert.IsNotNull(module);
                module.Equals(_unitUnderTest.GetDMDescriptor("Process", "ExpressionFluxProcess"));
                Assert.AreEqual(module, module);
                Assert.AreNotEqual(module, new object());
                Assert.AreEqual(module.Name, dmName);
                Assert.AreNotEqual(module.Path, "");
                Assert.IsNotNull(((IEnumerable)module).GetEnumerator());

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

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDescription()
        {
            string[] paths = Util.GetDMDirs();
            string desc = "";

            WrappedSimulator sim = new WrappedSimulator(paths);
            desc = sim.GetDescription("");
            Assert.AreEqual("", desc, "GetDescription method returns unexpected value.");

            desc = sim.GetDescription("ExpressionFluxProcess");
            Assert.IsNotEmpty(desc, "GetDescription method returns unexpected value.");

        }
        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestPrivateMethods()
        {
            Type type = _unitUnderTest.GetType();
            // GetModuleType
            MethodInfo info = type.GetMethod("GetModuleType", BindingFlags.NonPublic | BindingFlags.Static);
            string value = (string)info.Invoke(_unitUnderTest, new object[] { "System" });
            Assert.AreEqual("System", value, "GetModuleType method returns unexpected value.");
            value = (string)info.Invoke(_unitUnderTest, new object[] { "Variable" });
            Assert.AreEqual("Variable", value, "GetModuleType method returns unexpected value.");
            value = (string)info.Invoke(_unitUnderTest, new object[] { "ExpressionFluxProcess" });
            Assert.AreEqual("Process", value, "GetModuleType method returns unexpected value.");
            value = (string)info.Invoke(_unitUnderTest, new object[] { "ODEStepper" });
            Assert.AreEqual("Stepper", value, "GetModuleType method returns unexpected value.");

            info = type.GetMethod("LoadStepperDM", BindingFlags.NonPublic | BindingFlags.Static);
            DMDescriptor stepper = (DMDescriptor)info.Invoke(_unitUnderTest, new object[] { new WrappedSimulator(Util.GetDMDirs()), new DMModuleInfo("", new DMInfo()) });
            Assert.IsNull(stepper, "LoadStepperDM method returns unexpected value.");

        }
    }
}
