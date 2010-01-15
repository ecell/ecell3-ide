using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EcellCoreLib;
using NUnit.Framework;

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
        public void TestGetEntityList()
        {
            IList<string> entities = s.GetEntityList("System", "");
            Assert.That(entities, Is.Not.Null);
            Assert.That(entities, Has.Count.EqualTo(1));
            Assert.That(entities[0], Is.EqualTo("/"));
        }

        [Test]
        public void TestCreateSystem()
        {
            s.CreateEntity("System", "System:/:TEST");

            {
                IList<string> entities = s.GetEntityList("System", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(1));
                Assert.That(entities[0], Is.EqualTo("TEST"));
            }

            {
                IList<string> entities = s.GetEntityList("Variable", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(0));
            }

            {
                IList<string> entities = s.GetEntityList("Process", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(0));
            }
        }

        [Test]
        public void TestCreateVariable()
        {
            s.CreateEntity("Variable", "Variable:/:TEST");

            {
                IList<string> entities = s.GetEntityList("Variable", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(1));
                Assert.That(entities[0], Is.EqualTo("TEST"));
            }

            {
                IList<string> entities = s.GetEntityList("System", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(0));
            }

            {
                IList<string> entities = s.GetEntityList("Process", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(0));
            }
        }

        [Test]
        public void TestCreateProcess()
        {
            s.CreateEntity("MassActionFluxProcess", "Process:/:TEST");

            {
                IList<string> entities = s.GetEntityList("Process", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(1));
                Assert.That(entities[0], Is.EqualTo("TEST"));
            }

            {
                IList<string> entities = s.GetEntityList("System", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(0));
            }

            {
                IList<string> entities = s.GetEntityList("Variable", "/");
                Assert.That(entities, Is.Not.Null);
                Assert.That(entities, Has.Count.EqualTo(0));
            }
        }

        [Test]
        public void TestCreateStepper()
        {
            s.CreateStepper("PassiveStepper", "testStepper");
            Assert.That(
                s.GetStepperClassName("testStepper"),
                Is.EqualTo("PassiveStepper"));
        }

        [Test]
        public void TestEntityProperty()
        {
            string stepperID = GenerateRandomID();
            string variableFullID = "Variable:/:_" + GenerateRandomID();

            s.CreateStepper("PassiveStepper", stepperID);

            {
                s.SetEntityProperty(
                    "System::/:StepperID",
                    stepperID);
                object o = s.GetEntityProperty("System::/:StepperID");
                Assert.That(o, Is.InstanceOf(typeof(string)));
                Assert.That(o, Is.EqualTo(stepperID));
                try
                {
                    s.GetEntityProperty("System::/:NonExistentProperty");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOf(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("NoSlot"));
                }
            }

            s.CreateEntity("Variable", variableFullID);
            {
                double value = GenerateRandomDouble();
                s.SetEntityProperty(variableFullID + ":Value", value);
                object _value = s.GetEntityProperty(variableFullID + ":Value");
                Assert.That(_value, Is.Not.Null);
                Assert.That(_value, Is.EqualTo(value));
                try
                {
                    s.GetEntityProperty("INVALID:::::");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOf(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("InvalidEntityType"));
                }
                try
                {
                    s.GetEntityProperty("Variable:INVALID:INVALID:BAD");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOf(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("NotFound"));
                }
                try
                {
                    s.GetEntityProperty("Variable:/:");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOf(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("BadID"));
                }
                try
                {
                    s.GetEntityProperty("Variable:/:NonExistent:Value");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOf(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("NotFound"));
                }
                try
                {
                    s.GetEntityProperty(variableFullID + ":NonExistentProperty");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOf(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("NoSlot"));
                }
            }
        }

        [Test]
        public void TestLibecsException()
        {
            s.CreateEntity("ExpressionFluxProcess", "Process:/:TEST");
            s.CreateStepper("ODEStepper", "defaultStepper");
            s.LoadEntityProperty("System::/:StepperID", "defaultStepper");

            try
            {
                s.Initialize();
            }
            catch (WrappedLibecsException e)
            {
                Assert.That(e.Source, Is.EqualTo("InitializationFailed"));
                Assert.That(e.FullID, Is.EqualTo("Process:/:TEST"));
            }
        }

        [Test]
        public void TestGetDescriptor()
        {
            try
            {
                s.GetDescription("");
                Assert.Fail();
            }
            catch (WrappedStdException)
            {
            }

            Assert.IsNotEmpty(s.GetDescription("ExpressionFluxProcess"));
            foreach (string dmFile in Directory.GetFiles(GetDMDirectory(), "*.dll"))
            {
                FileInfo fi = new FileInfo(dmFile);
                string className = fi.Name.Replace(fi.Extension, "");
                try
                {
                    Console.WriteLine(s.GetDescription(className));
                }
                catch (Exception) { }
            }
        }

        [Test]
        public void TestCreateFluxDistributionStepper()
        {
            s.CreateStepper("FluxDistributionStepper", "default");
        }
    }
}