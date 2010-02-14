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
namespace Ecell
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using System.Diagnostics;
    using System.Xml;
    using Ecell.Objects;
    using System.IO;
    using EcellCoreLib;


    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestEmlWriter
    {

        private EmlWriter _unitUnderTest;
        private string _filename = TestConstant.TestDirectory + "test/TestEml.eml";

        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            if(!Directory.Exists(Path.GetDirectoryName(_filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(_filename));
            XmlTextWriter tx = new XmlTextWriter(_filename, Encoding.UTF8);
            _unitUnderTest = new EmlWriter(tx);
        }
        /// <summary>
        /// 
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest.Close();
            _unitUnderTest = null;
            if (Directory.Exists(Path.GetDirectoryName(_filename)))
                Directory.Delete(Path.GetDirectoryName(_filename), true);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestWriteStartDocument()
        {
            _unitUnderTest.WriteStartDocument();
            _unitUnderTest.WriteEndDocument();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestWriteEndDocument()
        {
            _unitUnderTest.WriteStartDocument();
            _unitUnderTest.WriteEndDocument();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestWrite()
        {
            string[] dmDirs = Util.GetDMDirs();
            WrappedSimulator sim = new EcellCoreLib.WrappedSimulator(dmDirs);
            EmlReader testEmlReader = new EmlReader(TestConstant.Model_RBC, sim);
            EcellObject model = testEmlReader.Parse();

            List<EcellObject> storedList = model.Children;
            _unitUnderTest.Write(storedList);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorEmlWriter()
        {
            _unitUnderTest.Close();
            XmlTextWriter tx = new XmlTextWriter(_filename, Encoding.UTF8);
            EmlWriter testEmlWriter = new EmlWriter(tx);
            Assert.IsNotNull(testEmlWriter, "Constructor of type, EmlWriter failed to create instance.");
            testEmlWriter.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreate()
        {
            _unitUnderTest.Close();
            string[] dmDirs = Util.GetDMDirs();
            WrappedSimulator sim = new EcellCoreLib.WrappedSimulator(dmDirs);
            EmlReader testEmlReader = new EmlReader(TestConstant.Model_Oscillation, sim);
            EcellObject model = testEmlReader.Parse();

            List<EcellObject> storedList = model.Children;
            bool isProjectSave = true;
            EmlWriter.Create(_filename, storedList, isProjectSave);
            EmlWriter.Create(_filename + Constants.FileExtBackUp, storedList, isProjectSave);

        }
    }
}
