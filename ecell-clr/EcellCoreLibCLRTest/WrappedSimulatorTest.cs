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
    }
}