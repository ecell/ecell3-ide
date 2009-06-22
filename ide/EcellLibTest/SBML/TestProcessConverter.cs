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
using Ecell.Objects;

namespace Ecell.SBML
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestProcessConverter
    {
        private ProcessConverter _unitUnderTest;
        private ApplicationEnvironment _env;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _env.DataManager.LoadProject(TestConstant.Model_Drosophila);
            _unitUnderTest = new ProcessConverter();

        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _env = null;
            _unitUnderTest = null;
        }

        /// <summary>
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestConvert()
        {
            // Expression
            string classname = ProcessConstants.ExpressionFluxProcess;
            EcellProcess process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            List<EcellData> list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);
            ProcessConverter.ConvertToExpression(process);

            Assert.AreEqual(ProcessConstants.ExpressionFluxProcess, process.Classname, "Classname is not expected value.");
            Assert.AreEqual("", process.Expression, "Expression is not expected value.");

            // Algebraic
            classname = ProcessConstants.ExpressionAlgebraicProcess;
            process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);
            ProcessConverter.ConvertToExpression(process);

            Assert.AreEqual(ProcessConstants.ExpressionAlgebraicProcess, process.Classname, "Classname is not expected value.");
            Assert.AreEqual("", process.Expression, "Expression is not expected value.");

            // Assignment
            classname = ProcessConstants.ExpressionAssignmentProcess;
            process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);
            ProcessConverter.ConvertToExpression(process);

            Assert.AreEqual(ProcessConstants.ExpressionAssignmentProcess, process.Classname, "Classname is not expected value.");
            Assert.AreEqual("", process.Expression, "Expression is not expected value.");

        }

        /// <summary>
        /// TestNotSupportedProcess
        /// </summary>
        [Test()]
        public void TestNotSupportedProcess()
        {
            // Expression
            string classname = ProcessConstants.GillespieProcess;
            EcellProcess process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            List<EcellData> list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);

            try
            {
                ProcessConverter.ConvertToExpression(process);
                Assert.Fail("Failed to catch exception.");
            }
            catch (Exception)
            {
            }


            // Not Process
            EcellObject variable = _env.DataManager.CreateDefaultObject("Drosophila", "/", "Variable");
            ProcessConverter.ConvertToExpression(variable);

        }

        /// <summary>
        /// TestConstantFlux2Expression
        /// </summary>
        [Test()]
        public void TestConstantFlux2Expression()
        {
            string classname = ProcessConstants.ConstantFluxProcess;
            EcellProcess process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            List<EcellData> list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);
            ProcessConverter.ConvertToExpression(process);

            Assert.AreEqual(ProcessConstants.ExpressionFluxProcess, process.Classname, "Classname is not expected value.");
            Assert.AreEqual("k", process.Expression, "Expression is not expected value.");
        }

        /// <summary>
        /// TestMassAction2Expression
        /// </summary>
        [Test()]
        public void TestMassAction2Expression()
        {
            string classname = ProcessConstants.MassActionFluxProcess;
            EcellProcess process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            List<EcellData> list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);
            ProcessConverter.ConvertToExpression(process);

            Assert.AreEqual(ProcessConstants.ExpressionFluxProcess, process.Classname, "Classname is not expected value.");
            Assert.AreEqual("k * S0.Value", process.Expression, "Expression is not expected value.");
        }

        /// <summary>
        /// TestDecayFlux2Expression
        /// </summary>
        [Test()]
        public void TestDecayFlux2Expression()
        {
            string classname = ProcessConstants.DecayFluxProcess;
            EcellProcess process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            List<EcellData> list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);
            ProcessConverter.ConvertToExpression(process);

            Assert.AreEqual(ProcessConstants.ExpressionFluxProcess, process.Classname, "Classname is not expected value.");
            Assert.AreEqual("( log ( 2 ) ) / ( T ) * S0.Value", process.Expression, "Expression is not expected value.");
        }

        /// <summary>
        /// TestDecayFlux2Expression
        /// </summary>
        [Test()]
        public void TestMichaelisUniUniFlux2Expression()
        {
            string classname = ProcessConstants.MichaelisUniUniFluxProcess;
            EcellProcess process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            List<EcellData> list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);
            ProcessConverter.ConvertToExpression(process);

            Assert.AreEqual(ProcessConstants.ExpressionFluxProcess, process.Classname, "Classname is not expected value.");
            Assert.AreEqual("( ( KcF * KmP * S0.MolerConc - KcR * KmS * P0.MolerConc ) * C0.Value ) / ( KmS * P0.MolerConc + KmP * S0.MolerConc + KmS * KmP )", process.Expression, "Expression is not expected value.");
        }

        /// <summary>
        /// TestDecayFlux2Expression
        /// </summary>
        [Test()]
        public void TestPingPongBiBiFlux2Expression()
        {
            string classname = ProcessConstants.PingPongBiBiFluxProcess;
            EcellProcess process = (EcellProcess)_env.DataManager.CreateDefaultObject("Drosophila", "/", "Process");
            process.Classname = classname;
            List<EcellData> list = new List<EcellData>();
            list.AddRange(_env.DataManager.GetProcessProperty(classname).Values);
            process.SetEcellDatas(list);
            ProcessConverter.ConvertToExpression(process);

            Assert.AreEqual(ProcessConstants.ExpressionFluxProcess, process.Classname, "Classname is not expected value.");
            Assert.AreEqual("( KcF * KcR * C0.Value * ( S0.MolarConc * S1.MolarConc - P0.MolarConc * P1.MolarConc / Keq ) ) / ( KcR * KmS1 * S0.MolarConc + KcR * KmS0 * S1.MolarConc + KmP1 * P0.MolarConc * KcF / Keq + KmP0 * P1.MolarConc * KcF / Keq + KcR * S0.MolarConc * S1.MolarConc + KmP1 * S0.MolarConc * P0.MolarConc * KcF / Keq / KiS0 + P0.MolarConc * P1.MolarConc * KcF / Keq + KcR * KmS0 * S1.MolarConc * P1.MolarConc / KiP1 )", process.Expression, "Expression is not expected value.");
        }

    }
}
