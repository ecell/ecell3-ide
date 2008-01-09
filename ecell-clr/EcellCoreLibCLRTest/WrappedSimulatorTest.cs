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
        public void TestGetEntityList()
        {
            WrappedPolymorph wp = s.GetEntityList("System", "");
            Assert.That(wp.IsList(), Is.True);
            List<WrappedPolymorph> l = wp.CastToList();
            Assert.That(l, Is.Not.Null);
            Assert.That(l, Has.Count(1));
            Assert.That(l[0].IsString(), Is.True);
            Assert.That(l[0].CastToString(), Is.EqualTo("/"));
        }

        [Test]
        public void TestCreateSystem()
        {
            s.CreateEntity("System", "System:/:TEST");

            {
                WrappedPolymorph wp = s.GetEntityList("System", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(1));
                Assert.That(l[0].IsString(), Is.True);
                Assert.That(l[0].CastToString(), Is.EqualTo("TEST"));
            }

            {
                WrappedPolymorph wp = s.GetEntityList("Variable", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(0));
            }

            {
                WrappedPolymorph wp = s.GetEntityList("Process", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(0));
            }
        }

        [Test]
        public void TestCreateVariable()
        {
            s.CreateEntity("Variable", "Variable:/:TEST");

            {
                WrappedPolymorph wp = s.GetEntityList("Variable", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(1));
                Assert.That(l[0].IsString(), Is.True);
                Assert.That(l[0].CastToString(), Is.EqualTo("TEST"));
            }

            {
                WrappedPolymorph wp = s.GetEntityList("System", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(0));
            }

            {
                WrappedPolymorph wp = s.GetEntityList("Process", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(0));
            }
        }

        [Test]
        public void TestCreateProcess()
        {
            s.CreateEntity("MassActionFluxProcess", "Process:/:TEST");

            {
                WrappedPolymorph wp = s.GetEntityList("Process", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(1));
                Assert.That(l[0].IsString(), Is.True);
                Assert.That(l[0].CastToString(), Is.EqualTo("TEST"));
            }

            {
                WrappedPolymorph wp = s.GetEntityList("System", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(0));
            }

            {
                WrappedPolymorph wp = s.GetEntityList("Variable", "/");
                Assert.That(wp.IsList(), Is.True);
                List<WrappedPolymorph> l = wp.CastToList();
                Assert.That(l, Is.Not.Null);
                Assert.That(l, Has.Count(0));
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
                    new WrappedPolymorph(stepperID));
                WrappedPolymorph wp;
                wp = s.GetEntityProperty("System::/:StepperID");
                Assert.That(wp.IsString(), Is.True);
                Assert.That(wp.CastToString(), Is.EqualTo(stepperID));
                try
                {
                    wp = s.GetEntityProperty("System::/:NonExistentProperty");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOfType(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("NoSlot"));
                }
            }

            s.CreateEntity("Variable", variableFullID);
            {
                double value = GenerateRandomDouble();
                s.SetEntityProperty(
                    variableFullID + ":Value",
                    new WrappedPolymorph(value));
                WrappedPolymorph wp;
                wp = s.GetEntityProperty(variableFullID + ":Value");
                Assert.That(wp.IsDouble(), Is.True);
                Assert.That(wp.CastToDouble(), Is.EqualTo(value));
                try
                {
                    wp = s.GetEntityProperty("INVALID:::::");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOfType(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("InvalidEntityType"));
                }
                try
                {
                    wp = s.GetEntityProperty("Variable:INVALID:INVALID:BAD");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOfType(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("BadSystemPath"));
                }
                try
                {
                    wp = s.GetEntityProperty("Variable:/:");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOfType(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("BadID"));
                }
                try
                {
                    wp = s.GetEntityProperty("Variable:/:NonExistent:Value");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOfType(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("NotFound"));
                }
                try
                {
                    wp = s.GetEntityProperty(variableFullID + ":NonExistentProperty");
                    Assert.Fail();
                }
                catch (Exception e)
                {
                    Assert.That(e, Is.InstanceOfType(typeof(WrappedLibecsException)));
                    Assert.That(e.Source, Is.EqualTo("NoSlot"));
                }
            }
        }
    }
}