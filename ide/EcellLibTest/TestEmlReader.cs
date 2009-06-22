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
namespace Ecell
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using System.Diagnostics;
    using Ecell.Objects;
    using EcellCoreLib;


    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestEmlReader
    {

        private EmlReader _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            string[] dmDirs = Util.GetDMDirs();
            string filename = TestConstant.Model_RBC;
            WrappedSimulator sim = new EcellCoreLib.WrappedSimulator(dmDirs);
            _unitUnderTest = new EmlReader(filename, sim);
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
        public void TestConstructorEmlReader()
        {
            string[] dmDirs = Util.GetDMDirs();
            string filename = TestConstant.Model_RBC;
            WrappedSimulator sim = new EcellCoreLib.WrappedSimulator(dmDirs);
            EmlReader testEmlReader = new EmlReader(filename, sim);
            Assert.IsNotNull(testEmlReader, "Constructor of type, EmlReader failed to create instance.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParse()
        {
            EcellObject expectedEcellObject = _unitUnderTest.Parse();
            EcellObject resultEcellObject = null;

            string[] dmDirs = Util.GetDMDirs();
            string filename = TestConstant.Model_RBC;
            WrappedSimulator sim = new EcellCoreLib.WrappedSimulator(dmDirs);
            EmlReader testEmlReader = new EmlReader(filename, sim);

            resultEcellObject = testEmlReader.Parse();
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "Parse method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParseFileNameSim()
        {
            string[] dmDirs = Util.GetDMDirs();
            WrappedSimulator sim = new EcellCoreLib.WrappedSimulator(dmDirs);
            string fileName = TestConstant.Model_RBC;

            EcellObject expectedEcellObject = _unitUnderTest.Parse();
            EcellObject resultEcellObject = null;
            resultEcellObject = EmlReader.Parse(fileName, sim);
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "Parse method returned unexpected result.");

            fileName = TestConstant.Model_Oscillation;
            sim = new EcellCoreLib.WrappedSimulator(dmDirs);
            resultEcellObject = EmlReader.Parse(fileName, sim);
            Assert.IsNotNull(resultEcellObject, "Parse method returned unexpected result.");

        }
    }
}
