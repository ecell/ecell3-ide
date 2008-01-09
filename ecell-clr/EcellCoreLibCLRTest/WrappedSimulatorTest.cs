using System;
using System.Collections.Generic;
using System.Text;
using EcellCoreLib;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace EcellCoreLibCLRTest
{
    [TestFixture]
    public class WrappedSimulatorTest: TestBase
    {
        WrappedSimulator s;

        [SetUp]
        public void SetUp()
        {
            s = new WrappedSimulator(
                new string[] { GetDMDirectory() });
        }

        [TearDown]
        public void TearDown()
        {
            s.Dispose();
        }

        [Test]
        public void TestNone()
        {
        }

        [Test]
        public void TestCreateEntity()
        {
            s.CreateEntity("System", "System::/");
            Console.WriteLine(s.GetEntityList("System", ""));
        }

        [Test]
        public void TestCreateStepper()
        {  
            s.CreateStepper("PassiveStepper", "testStepper");
            Assert.That(
                s.GetStepperClassName("testStepper"),
                Is.EqualTo("testStepper"));
        }
    }
}