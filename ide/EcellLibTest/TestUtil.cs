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
    using NUnit.Framework;
    using Ecell;
    using System.Globalization;
    using Ecell.Exceptions;
    using System.Drawing;
    using Ecell.Objects;
    using System.Collections.Generic;
    /// <summary>
    /// Testof Ecell Util class.
    /// </summary>
    [TestFixture()]
    public class TestUtil
    {
        private Ecell.Util _unitUnderTest;
        /// <summary>
        /// Constructor.
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new Ecell.Util();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        /// <summary>
        /// Test of IsNGforID method
        /// </summary>
        [Test()]
        public void TestIsNGforID()
        {
            string key = null;
            bool expectedBoolean = true;
            bool resultBoolean = false;

            key = null;
            resultBoolean = Util.IsNGforID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforID method returned unexpected result.");

            key = "";
            resultBoolean = Util.IsNGforID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforID method returned unexpected result.");

            key = "abc;1";
            resultBoolean = Util.IsNGforID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforID method returned unexpected result.");

            key = "abc-1";
            resultBoolean = Util.IsNGforID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforID method returned unexpected result.");

            key = "abc#";
            resultBoolean = Util.IsNGforID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforID method returned unexpected result.");

            key = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1";
            resultBoolean = Util.IsNGforID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforID method returned unexpected result.");

            expectedBoolean = false;
            key = "abc_1";
            resultBoolean = Util.IsNGforID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforID method returned unexpected result.");

            key = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef";
            resultBoolean = Util.IsNGforID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforID method returned unexpected result.");
        }

        /// <summary>
        /// Test of TestIsNGforIDonWindows method
        /// </summary>
        [Test()]
        public void TestIsNGforIDonWindows()
        {
            string key = null;
            bool expectedBoolean = true;
            bool resultBoolean = false;
            // null
            key = null;
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // empty
            key = "";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // ;
            key = "abc;1";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // :
            key = "abc:1";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // /
            key = "abc/01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // \
            key = "abc\\01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // |
            key = "abc|01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // "
            key = "abc\"01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // *
            key = "abc*01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // ?
            key = "abc?01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // <
            key = "abc<01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // >
            key = "abc>?01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
            // ~
            key = "abc~01";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");

            // key > 128 char
            key = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");

            expectedBoolean = false;
            key = "abc_1";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");

            key = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef";
            resultBoolean = Util.IsNGforIDonWindows(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "TestIsNGforIDonWindows method returned unexpected result.");
        }

        /// <summary>
        /// Test of IsReservedID() method.
        /// </summary>
        [Test()]
        public void TestIsReservedID()
        {
            string key = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;

            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");

            key = "";
            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");

            key = "/:test";
            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");

            key = "/SIZE/hoge/";
            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");

            key = "/hoge/:SIZEa";
            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");

            expectedBoolean = true;

            key = "/:size";
            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");

            key = "/:Size";
            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");

            key = "/:SIZE";
            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");

            key = "SIZE";
            resultBoolean = Util.IsReservedID(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsReservedID method returned unexpected result.");        
        }
        
        /// <summary>
        /// Test of IsNGforType() method.
        /// </summary>
        [Test()]
        public void TestIsNGforType()
        {
            string type = null;
            bool expectedBoolean = true;
            bool resultBoolean = true;

            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = "";
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = "hoge";
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = "system";
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            expectedBoolean = false;

            type = EcellObject.PROJECT;
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = EcellObject.MODEL;
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = EcellObject.STEPPER;
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = EcellObject.SYSTEM;
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = EcellObject.PROCESS;
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = EcellObject.VARIABLE;
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

            type = EcellObject.TEXT;
            resultBoolean = Util.IsNGforType(type);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforType method returned unexpected result.");

        }

        /// <summary>
        /// Test of IsNGforFullPN() method.
        /// </summary>
        [Test()]
        public void TestIsNGforFullPN()
        {
            string fullPN = null;
            bool expectedBoolean = true;
            bool resultBoolean = true;

            resultBoolean = Util.IsNGforFullPN(fullPN);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullPN = "hoge:/S0";
            resultBoolean = Util.IsNGforFullPN(fullPN);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullPN = "Variable:/S0";
            resultBoolean = Util.IsNGforFullPN(fullPN);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullPN = "Variable:/S0:Value:P+";
            resultBoolean = Util.IsNGforFullPN(fullPN);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            expectedBoolean = false;

            fullPN = "System:/S0/:SIZE";
            resultBoolean = Util.IsNGforFullPN(fullPN);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullPN = "Variable:/S0/:Value:Value";
            resultBoolean = Util.IsNGforFullPN(fullPN);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullPN = "Process:/S0/:P0:Activity";
            resultBoolean = Util.IsNGforFullPN(fullPN);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullPN = "Stepper:/:DE:Interval";
            resultBoolean = Util.IsNGforFullPN(fullPN);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");
        }

        /// <summary>
        /// Test of IsNGforFullID() method.
        /// </summary>
        [Test()]
        public void TestIsNGforFullID()
        {
            string fullID = null;
            bool expectedBoolean = true;
            bool resultBoolean = true;

            resultBoolean = Util.IsNGforFullID(fullID);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullID = "hoge/S0";
            resultBoolean = Util.IsNGforFullID(fullID);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullID = "hoge:/S0";
            resultBoolean = Util.IsNGforFullID(fullID);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullID = "Variable:/S0";
            resultBoolean = Util.IsNGforFullID(fullID);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            expectedBoolean = false;

            fullID = "System:/S0/";
            resultBoolean = Util.IsNGforFullID(fullID);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullID = "Variable:/S0/:Value";
            resultBoolean = Util.IsNGforFullID(fullID);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullID = "Process:/S0/:P0";
            resultBoolean = Util.IsNGforFullID(fullID);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");

            fullID = "Stepper:/DE";
            resultBoolean = Util.IsNGforFullID(fullID);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforFullID method returned unexpected result.");
        }

        /// <summary>
        /// Test of IsNGforSystemKey() method.
        /// </summary>
        [Test()]
        public void TestIsNGforSystemKey()
        {
            string key = null;
            bool expectedBoolean = true;
            bool resultBoolean = true;

            resultBoolean = Util.IsNGforSystemKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforSystemKey method returned unexpected result.");

            key = "";
            resultBoolean = Util.IsNGforSystemKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforSystemKey method returned unexpected result.");

            key = "hoge";
            resultBoolean = Util.IsNGforSystemKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforSystemKey method returned unexpected result.");

            key = "hoge/";
            resultBoolean = Util.IsNGforSystemKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforSystemKey method returned unexpected result.");

            key = "/hoge/:test";
            resultBoolean = Util.IsNGforSystemKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforSystemKey method returned unexpected result.");

            key = "/hoge//test";
            resultBoolean = Util.IsNGforSystemKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforSystemKey method returned unexpected result.");

            expectedBoolean = false;

            key = "/hoge/test_1";
            resultBoolean = Util.IsNGforSystemKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforSystemKey method returned unexpected result.");

            key = "/hoge/test0";
            resultBoolean = Util.IsNGforSystemKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforSystemKey method returned unexpected result.");

        }

        /// <summary>
        /// Test of IsNGforEntityKey() method.
        /// </summary>
        [Test()]
        public void TestIsNGforEntityKey()
        {
            string key = null;
            bool expectedBoolean = true;
            bool resultBoolean = false;

            resultBoolean = Util.IsNGforEntityKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforComponentKey method returned unexpected result.");

            key = "";
            resultBoolean = Util.IsNGforEntityKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforComponentKey method returned unexpected result.");

            key = "hoge/:test";
            resultBoolean = Util.IsNGforEntityKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforComponentKey method returned unexpected result.");

            key = "/hoge/hg^d/:test";
            resultBoolean = Util.IsNGforEntityKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforComponentKey method returned unexpected result.");

            key = "/hoge//:test";
            resultBoolean = Util.IsNGforEntityKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforComponentKey method returned unexpected result.");

            key = "/hoge/test";
            resultBoolean = Util.IsNGforEntityKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforComponentKey method returned unexpected result.");

            key = "/hoge/:test/:test";
            resultBoolean = Util.IsNGforEntityKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforComponentKey method returned unexpected result.");

            expectedBoolean = false;

            key = "/hoge/:test";
            resultBoolean = Util.IsNGforEntityKey(key);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsNGforComponentKey method returned unexpected result.");

        }

        /// <summary>
        /// Test of ConvertSystemEntityPath method.
        /// </summary>
        [Test()]
        public void TestConvertSystemEntityPath()
        {
            string key = null;
            string prop = null;
            string expectedString = null;
            string resultString = null;

            // Invalid key.
            try
            {
                resultString = Util.ConvertSystemEntityPath(key, prop);
                Assert.Fail("Failed to catch invalid key.");
            }
            catch (EcellException)
            {
            }

            // Invalid key.
            try
            {
                key = "hoge";
                prop = "";
                resultString = Util.ConvertSystemEntityPath(key, prop);
                Assert.Fail("Failed to catch invalid key.");
            }
            catch (EcellException)
            {
            }

            // Invalid prop.
            try
            {
                key = "/test";
                prop = "";
                resultString = Util.ConvertSystemEntityPath(key, prop);
                Assert.Fail("Failed to catch invalid prop.");
            }
            catch (EcellException)
            {
            }

            // Invalid prop.
            try
            {
                key = "/test";
                prop = "";
                resultString = Util.ConvertSystemEntityPath(key, prop);
                Assert.Fail("Failed to catch invalid prop.");
            }
            catch (EcellException)
            {
            }

            // /
            key = "/";
            prop = "Test";
            expectedString = "System::/:Test";
            resultString = Util.ConvertSystemEntityPath(key, prop);
            Assert.AreEqual(expectedString, resultString, "ConvertSystemEntityPath method returned unexpected result.");

            // /Test
            key = "/Test";
            prop = "Prop";
            expectedString = "System:/:Test:Prop";
            resultString = Util.ConvertSystemEntityPath(key, prop);
            Assert.AreEqual(expectedString, resultString, "ConvertSystemEntityPath method returned unexpected result.");

            // /Test
            key = "/hoge/Test";
            prop = "Prop";
            expectedString = "System:/hoge:Test:Prop";
            resultString = Util.ConvertSystemEntityPath(key, prop);
            Assert.AreEqual(expectedString, resultString, "ConvertSystemEntityPath method returned unexpected result.");

        }

        /// <summary>
        /// TestBuildFullID
        /// </summary>
        [Test()]
        public void TestBuildFullID()
        {
            string type = null;
            string systemPath = null;
            string localID = null;
            string expectedString = null;
            string resultString = null;

            // Invalid type
            try
            {
                resultString = Util.BuildFullID(type, systemPath, localID);
                Assert.Fail("Failed to catch invalid type.");
            }
            catch (EcellException)
            {
            }

            // Invalid systemPath
            try
            {
                type = EcellObject.VARIABLE;
                systemPath = "";
                localID = "";
                resultString = Util.BuildFullID(type, systemPath, localID);
                Assert.Fail("Failed to catch invalid systemPath.");
            }
            catch (EcellException)
            {
            }

            // Invalid systemPath
            try
            {
                type = EcellObject.VARIABLE;
                systemPath = "/!";
                localID = "V0";
                resultString = Util.BuildFullID(type, systemPath, localID);
                Assert.Fail("Failed to catch invalid systemPath.");
            }
            catch (EcellException)
            {
            }

            // Invalid localID
            try
            {
                type = EcellObject.VARIABLE;
                systemPath = "/S0";
                localID = "";
                resultString = Util.BuildFullID(type, systemPath, localID);
                Assert.Fail("Failed to catch invalid systemPath.");
            }
            catch (EcellException)
            {
            }

            // Invalid localID
            try
            {
                type = EcellObject.VARIABLE;
                systemPath = "/S0";
                localID = "A1/B2";
                resultString = Util.BuildFullID(type, systemPath, localID);
                Assert.Fail("Failed to catch invalid systemPath.");
            }
            catch (EcellException)
            {
            }

            type = EcellObject.VARIABLE;
            systemPath = "/S0";
            localID = "A1";
            expectedString = "Variable:/S0:A1";
            resultString = Util.BuildFullID(type, systemPath, localID);
            Assert.AreEqual(expectedString, resultString, "BuildFullID method returned unexpected result.");

            type = EcellObject.PROCESS;
            systemPath = "/S0";
            localID = "A1";
            expectedString = "Process:/S0:A1";
            resultString = Util.BuildFullID(type, systemPath, localID);
            Assert.AreEqual(expectedString, resultString, "BuildFullID method returned unexpected result.");

            type = EcellObject.SYSTEM;
            systemPath = "/S0";
            localID = "A1";
            expectedString = "System:/S0/A1";
            resultString = Util.BuildFullID(type, systemPath, localID);
            Assert.AreEqual(expectedString, resultString, "BuildFullID method returned unexpected result.");

        }

        /// <summary>
        /// TestBuildFullPNFullIDPropName
        /// </summary>
        [Test()]
        public void TestBuildFullPNFullIDPropName()
        {
            string fullID = null;
            string propName = null;
            string expectedString = null;
            string resultString = null;
            // Invalid fullID.
            try
            {
                resultString = Util.BuildFullPN(fullID, propName);
                Assert.Fail("Failed to catch invalid fullID.");
            }
            catch (EcellException)
            {
            }
            try
            {
                fullID = "/";
                propName = "";
                resultString = Util.BuildFullPN(fullID, propName);
                Assert.Fail("Failed to catch invalid fullID.");
            }
            catch (EcellException)
            {
            }
            // Invalid Property Name.
            try
            {
                fullID = "System:/S0";
                propName = "ss#2";
                resultString = Util.BuildFullPN(fullID, propName);
                Assert.Fail("Failed to catch invalid propName.");
            }
            catch (EcellException)
            {
            }

            fullID = "Variable:/S0:aa";
            propName = "ss";
            expectedString = "Variable:/S0:aa:ss";
            resultString = Util.BuildFullPN(fullID, propName);
            Assert.AreEqual(expectedString, resultString, "BuildFullPN method returned unexpected result.");

        }

        /// <summary>
        /// TestBuildFullPNTypeSystemPathLocalIDPropName
        /// </summary>
        [Test()]
        public void TestBuildFullPNTypeSystemPathLocalIDPropName()
        {
            string type = null;
            string systemPath = null;
            string localID = null;
            string propName = null;
            string expectedString = null;
            string resultString = null;
            try
            {
                resultString = Util.BuildFullPN(type, systemPath, localID, propName);
                Assert.Fail("Failed to catch invalid type.");
            }
            catch (EcellException)
            {
            }
            try
            {
                type = "hoge";
                resultString = Util.BuildFullPN(type, systemPath, localID, propName);
                Assert.Fail("Failed to catch invalid type.");
            }
            catch (EcellException)
            {
            }

            type = EcellObject.VARIABLE;
            systemPath = "/S0/S1";
            localID = "V0";
            propName = "Value";
            expectedString = "Variable:/S0/S1:V0:Value";
            resultString = Util.BuildFullPN(type, systemPath, localID, propName);
            Assert.AreEqual(expectedString, resultString, "BuildFullPN method returned unexpected result.");

        }

        /// <summary>
        /// TestCopyFile
        /// </summary>
        [Test()]
        public void TestCopyFile()
        {
            string filename = null;
            string targetDir = null;
            // Invalid file
            try
            {
                Util.CopyFile(filename, targetDir);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            // Invalid file
            try
            {
                filename = "c:/hoge/hoge.txt";
                Util.CopyFile(filename, targetDir);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            // Invalid dir
            try
            {
                filename = "c:/temp/rbc.eml";
                Util.CopyFile(filename, targetDir);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            filename = "c:/temp/rbc.eml";
            targetDir = "c:/temp/hoge";
            Util.CopyFile(filename, targetDir);

        }

        /// <summary>
        /// TestCopyDirectory
        /// </summary>
        [Test()]
        public void TestCopyDirectory()
        {
            string sourceDir = null;
            string targetDir = null;

            // Invalid sourceDir
            try
            {
                Util.CopyDirectory(sourceDir, targetDir);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid sourceDir
            try
            {
                sourceDir = "c:/Temp/hoge";
                Util.CopyDirectory(sourceDir, targetDir);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid targetDir
            try
            {
                sourceDir = "c:/Temp/Drosophila";
                targetDir = "";
                Util.CopyDirectory(sourceDir, targetDir);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            sourceDir = "c:/Temp/Drosophila";
            targetDir = "c:/Temp/Drosophila";
            Util.CopyDirectory(sourceDir, targetDir);

            sourceDir = "c:/Temp/Drosophila";
            targetDir = "c:/Temp/Hoge/Drosophila";
            Util.CopyDirectory(sourceDir, targetDir);

        }

        /// <summary>
        /// TestGetRevNo
        /// </summary>
        [Test()]
        public void TestGetRevNo()
        {
            string sourceDir = "c:/temp/Drosophila";
            string expectedString = "Revision1";
            string resultString = null;
            resultString = Util.GetRevNo(sourceDir);
            Assert.AreEqual(expectedString, resultString, "GetRevNo method returned unexpected result.");
        }

        /// <summary>
        /// TestGetMovedKey
        /// </summary>
        [Test()]
        public void TestGetMovedKey()
        {
            string originalKey = null;
            string originalSystemKey = null;
            string newSystemKey = null;
            string expectedString = null;
            string resultString = null;
            // Invalid
            try
            {
                resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            try
            {
                originalKey = "/Hoge:Value";
                originalSystemKey = "/Hoge";
                newSystemKey = "";
                resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            try
            {
                originalKey = "/Hoge:Value";
                originalSystemKey = "";
                newSystemKey = "/Hoge";
                resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            try
            {
                originalKey = "Hoge/Hoge:Value";
                originalSystemKey = "/";
                newSystemKey = "/Hoge";
                resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            originalKey = "/Hoge:Value";
            originalSystemKey = "/Hoge";
            newSystemKey = "/Hoge/S0";
            expectedString = "/Hoge/S0:Value";
            resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
            Assert.AreEqual(expectedString, resultString, "GetMovedKey method returned unexpected result.");

            originalKey = "/Hoge:Value";
            originalSystemKey = "/Hoge";
            newSystemKey = "/";
            expectedString = "/:Value";
            resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
            Assert.AreEqual(expectedString, resultString, "GetMovedKey method returned unexpected result.");

            originalKey = "/Hoge/s1:Value";
            originalSystemKey = "/Hoge";
            newSystemKey = "/";
            expectedString = "/s1:Value";
            resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
            Assert.AreEqual(expectedString, resultString, "GetMovedKey method returned unexpected result.");

            originalKey = "/:Value";
            originalSystemKey = "/";
            newSystemKey = "/Hoge/s1";
            expectedString = "/Hoge/s1:Value";
            resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
            Assert.AreEqual(expectedString, resultString, "GetMovedKey method returned unexpected result.");

            originalKey = "/Hoge/s1/s2:Value";
            originalSystemKey = "/Hoge/s1";
            newSystemKey = "/Hoge/s3";
            expectedString = "/Hoge/s3/s2:Value";
            resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
            Assert.AreEqual(expectedString, resultString, "GetMovedKey method returned unexpected result.");

            originalKey = "/cell";
            originalSystemKey = "/";
            newSystemKey = "/cell_copy2";
            expectedString = "/cell_copy2/cell";
            resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
            Assert.AreEqual(expectedString, resultString, "GetMovedKey method returned unexpected result.");

            originalKey = "/:V0";
            originalSystemKey = "/";
            newSystemKey = "/";
            expectedString = "/:V0";
            resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
            Assert.AreEqual(expectedString, resultString, "GetMovedKey method returned unexpected result.");

            originalKey = "/CELL/CELL_copy0";
            originalSystemKey = "/CELL";
            newSystemKey = "/CELL/CELL_copy0";
            expectedString = "/CELL/CELL_copy0/CELL_copy0";
            resultString = Util.GetMovedKey(originalKey, originalSystemKey, newSystemKey);
            Assert.AreEqual(expectedString, resultString, "GetMovedKey method returned unexpected result.");

        }

        /// <summary>
        /// TestGetNewProjectName
        /// </summary>
        [Test()]
        public void TestGetNewProjectName()
        {
            string resultString = Util.GetNewProjectName();
            Assert.IsNotNull(resultString, "GetNewProjectName method returned unexpected result.");

        }

        /// <summary>
        /// TestGetNewProjectName
        /// </summary>
        [Test()]
        public void TestGetNewFileName()
        {
            string expectedString = "c:/temp/rbc.eml";
            string resultString = Util.GetNewFileName(expectedString);
            Assert.AreNotEqual(resultString, "GetNewFileName method returned unexpected result.");

        }

        /// <summary>
        /// TestParseEntityKey
        /// </summary>
        [Test()]
        public void TestParseKey()
        {
            string key = null;
            string systemPath = null;
            string expectedsystemPath = null;
            string localID = null;
            string expectedlocalID = null;
            // Invalid key
            try
            {
                Util.ParseKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid key
            try
            {
                key = "Variable:/System/Param/";
                Util.ParseKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid key
            try
            {
                key = "System:/System:Param";
                Util.ParseKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            key = "/System:Param";
            expectedsystemPath = "/System";
            expectedlocalID = "Param";
            Util.ParseKey(key, out systemPath, out localID);
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

            key = "/System/Param";
            expectedsystemPath = "/System";
            expectedlocalID = "Param";
            Util.ParseKey(key, out systemPath, out localID);
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

            key = "/";
            expectedsystemPath = "";
            expectedlocalID = "/";
            Util.ParseKey(key, out systemPath, out localID);
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

        }

        /// <summary>
        /// TestParseEntityKey
        /// </summary>
        [Test()]
        public void TestParseEntityKey()
        {
            string key = null;
            string systemPath = null;
            string expectedsystemPath = null;
            string localID = null;
            string expectedlocalID = null;
            // Invalid key
            try
            {
                Util.ParseEntityKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid key
            try
            {
                key = "/System/Param/";
                Util.ParseEntityKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid key
            try
            {
                key = "/System/Param/:Param-";
                Util.ParseEntityKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid key
            try
            {
                key = "/System/Param/:Param/a";
                Util.ParseEntityKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid key
            try
            {
                key = "/System/=+/Param/:Param";
                Util.ParseEntityKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid key
            try
            {
                key = "Variable:/System/:Param";
                Util.ParseEntityKey(key, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            key = "/System/:Param";
            expectedsystemPath = "/System/";
            expectedlocalID = "Param";
            Util.ParseEntityKey(key, out systemPath, out localID);
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

            key = "/:Param";
            expectedsystemPath = "/";
            expectedlocalID = "Param";
            Util.ParseEntityKey(key, out systemPath, out localID);
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

        }

        /// <summary>
        /// TestParseSystemKey
        /// </summary>
        [Test()]
        public void TestParseSystemKey()
        {
            string systemKey = null;
            string systemPath = null;
            string expectedsystemPath = null;
            string localID = null;
            string expectedlocalID = null;
            try
            {
                Util.ParseSystemKey(systemKey, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            try
            {
                systemKey = "/hoge:Value";
                Util.ParseSystemKey(systemKey, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            systemKey = "/hoge/hoge";
            expectedsystemPath = "/hoge";
            expectedlocalID = "hoge";
            Util.ParseSystemKey(systemKey, out systemPath, out localID);
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

            systemKey = "/hoge";
            expectedsystemPath = "/";
            expectedlocalID = "hoge";
            Util.ParseSystemKey(systemKey, out systemPath, out localID);
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

            systemKey = "/";
            expectedsystemPath = "";
            expectedlocalID = "/";
            Util.ParseSystemKey(systemKey, out systemPath, out localID);
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

        }

        /// <summary>
        /// TestParseFullIDFullIDTypeSystemPathLocalID
        /// </summary>
        [Test()]
        public void TestParseFullIDFullIDTypeSystemPathLocalID()
        {
            string fullID = null;
            string type = null;
            string expectedtype = null;
            string systemPath = null;
            string expectedsystemPath = null;
            string localID = null;
            string expectedlocalID = null;

            try
            {
                Util.ParseFullID(fullID, out type, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            try
            {
                fullID = "/hoge/hoge/";
                Util.ParseFullID(fullID, out type, out systemPath, out localID);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            fullID = "System:/hoge/hoge";
            expectedtype = "System";
            expectedsystemPath = "/hoge";
            expectedlocalID = "hoge";
            Util.ParseFullID(fullID, out type, out systemPath, out localID);
            Assert.AreEqual(expectedtype, type, "type out parameter is not expected value.");
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

            fullID = "Variable:/hoge/hoge:Value";
            expectedtype = "Variable";
            expectedsystemPath = "/hoge/hoge";
            expectedlocalID = "Value";
            Util.ParseFullID(fullID, out type, out systemPath, out localID);
            Assert.AreEqual(expectedtype, type, "type out parameter is not expected value.");
            Assert.AreEqual(expectedsystemPath, systemPath, "systemPath out parameter is not expected value.");
            Assert.AreEqual(expectedlocalID, localID, "localID out parameter is not expected value.");

        }

        /// <summary>
        /// TestParseFullIDFullIDTypeKey
        /// </summary>
        [Test()]
        public void TestParseFullIDFullIDTypeKey()
        {
            string fullID = null;
            string type = null;
            string expectedtype = null;
            string key = null;
            string expectedkey = null;
            try
            {
                Util.ParseFullID(fullID, out type, out key);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            try
            {
                fullID = "/Sys/:Value";
                Util.ParseFullID(fullID, out type, out key);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            try
            {
                fullID = "System::Value";
                Util.ParseFullID(fullID, out type, out key);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            fullID = "Variable:/sys/:Value";
            expectedtype = "Variable";
            expectedkey = "/sys/:Value";
            Util.ParseFullID(fullID, out type, out key);
            Assert.AreEqual(expectedtype, type, "type out parameter is not expected value.");
            Assert.AreEqual(expectedkey, key, "key out parameter is not expected value.");

            fullID = "System:/Sys/Value";
            expectedtype = "System";
            expectedkey = "/Sys/Value";
            Util.ParseFullID(fullID, out type, out key);
            Assert.AreEqual(expectedtype, type, "type out parameter is not expected value.");
            Assert.AreEqual(expectedkey, key, "key out parameter is not expected value.");

            fullID = "System:/";
            expectedtype = "System";
            expectedkey = "/";
            Util.ParseFullID(fullID, out type, out key);
            Assert.AreEqual(expectedtype, type, "type out parameter is not expected value.");
            Assert.AreEqual(expectedkey, key, "key out parameter is not expected value.");

        }

        /// <summary>
        /// TestParseFullPN
        /// </summary>
        [Test()]
        public void TestParseFullPN()
        {
            string fullPN = null;
            string type = null;
            string expectedtype = null;
            string key = null;
            string expectedkey = null;
            string propName = null;
            string expectedpropName = null;

            // Invalid fullPN
            try
            {
                Util.ParseFullPN(fullPN, out type, out key, out propName);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid fullPN
            try
            {
                fullPN = "Test:/Temp/:SIZE";
                Util.ParseFullPN(fullPN, out type, out key, out propName);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid fullPN
            try
            {
            fullPN = "System:/Temp/:SIZE:Value";
                Util.ParseFullPN(fullPN, out type, out key, out propName);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            // Invalid fullPN
            try
            {
                fullPN = "System:/Temp/:SIZE:Value";
                Util.ParseFullPN(fullPN, out type, out key, out propName);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            fullPN = "Variable:/Temp/:Test:Value";
            Util.ParseFullPN(fullPN, out type, out key, out propName);
            expectedtype = "Variable";
            expectedkey = "/Temp/:Test";
            expectedpropName = "Value";
            Assert.AreEqual(expectedtype, type, "type out parameter is not expected value.");
            Assert.AreEqual(expectedkey, key, "type out parameter is not expected value.");
            Assert.AreEqual(expectedpropName, propName, "type out parameter is not expected value.");

            fullPN = "System:/Temp/:SIZE";
            Util.ParseFullPN(fullPN, out type, out key, out propName);
            expectedtype = "System";
            expectedkey = "/Temp/";
            expectedpropName = "SIZE";
            Assert.AreEqual(expectedtype, type, "type out parameter is not expected value.");
            Assert.AreEqual(expectedkey, key, "type out parameter is not expected value.");
            Assert.AreEqual(expectedpropName, propName, "type out parameter is not expected value.");

        }

        /// <summary>
        /// TestParseFullPN
        /// </summary>
        [Test()]
        public void TestParseTemporaryID()
        {
            string localID = null;
            int id = 0;
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(0, id, "ParseTemporaryID method returned unexpected result.");

            localID = "hoge";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(0, id, "ParseTemporaryID method returned unexpected result.");

            localID = "Test1";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(1, id, "ParseTemporaryID method returned unexpected result.");

            localID = "Test12345";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(12345, id, "ParseTemporaryID method returned unexpected result.");

            localID = "12345Test";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(0, id, "ParseTemporaryID method returned unexpected result.");

            localID = "Test12A";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(0, id, "ParseTemporaryID method returned unexpected result.");

            localID = "Test12_A3";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(3, id, "ParseTemporaryID method returned unexpected result.");

            localID = "12345Test12";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(12, id, "ParseTemporaryID method returned unexpected result.");

            localID = "A_copy3";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(3, id, "ParseTemporaryID method returned unexpected result.");

            localID = "P3";
            id = Util.ParseTemporaryID(localID);
            Assert.AreEqual(3, id, "ParseTemporaryID method returned unexpected result.");

        }

        /// <summary>
        /// TestGetSuperSystemPath
        /// </summary>
        [Test()]
        public void TestGetSuperSystemPath()
        {
            string systemPath = null;
            string expectedString = "";
            string resultString = "";
            resultString = Util.GetSuperSystemPath(systemPath);
            Assert.AreEqual(expectedString, resultString, "GetSuperSystemPath method returned unexpected result.");

            systemPath = "/";
            resultString = Util.GetSuperSystemPath(systemPath);
            Assert.AreEqual(expectedString, resultString, "GetSuperSystemPath method returned unexpected result.");

            systemPath = "/S0";
            expectedString = "/";
            resultString = Util.GetSuperSystemPath(systemPath);
            Assert.AreEqual(expectedString, resultString, "GetSuperSystemPath method returned unexpected result.");

            systemPath = "/S0:S1";
            expectedString = "/S0";
            resultString = Util.GetSuperSystemPath(systemPath);
            Assert.AreEqual(expectedString, resultString, "GetSuperSystemPath method returned unexpected result.");

            systemPath = "/AAA/BBB";
            expectedString = "/AAA";
            resultString = Util.GetSuperSystemPath(systemPath);
            Assert.AreEqual(expectedString, resultString, "GetSuperSystemPath method returned unexpected result.");

            systemPath = "/S0/S1/S2";
            expectedString = "/S0/S1";
            resultString = Util.GetSuperSystemPath(systemPath);
            Assert.AreEqual(expectedString, resultString, "GetSuperSystemPath method returned unexpected result.");

        }

        /// <summary>
        /// TestNormalizeSystemPath
        /// </summary>
        [Test()]
        public void TestNormalizeSystemPath()
        {
            string systemPath = null;
            string currentSystemPath = null;
            string expectedString = null;
            string resultString = null;

            resultString = Util.NormalizeSystemPath(systemPath, currentSystemPath);
            Assert.AreEqual(expectedString, resultString, "NormalizeSystemPath method returned unexpected result.");

            systemPath = "/";
            expectedString = "/";
            resultString = Util.NormalizeSystemPath(systemPath, currentSystemPath);
            Assert.AreEqual(expectedString, resultString, "NormalizeSystemPath method returned unexpected result.");

            systemPath = ".";
            currentSystemPath = "/S0/S1";
            expectedString = "/S0/S1";
            resultString = Util.NormalizeSystemPath(systemPath, currentSystemPath);
            Assert.AreEqual(expectedString, resultString, "NormalizeSystemPath method returned unexpected result.");

            systemPath = "..";
            currentSystemPath = "/S0/S1";
            expectedString = "/S0";
            resultString = Util.NormalizeSystemPath(systemPath, currentSystemPath);
            Assert.AreEqual(expectedString, resultString, "NormalizeSystemPath method returned unexpected result.");

            try
            {
                systemPath = "../:Value";
                currentSystemPath = "./S0/S1";
                resultString = Util.NormalizeSystemPath(systemPath, currentSystemPath);
                Assert.Fail("Failed to catch currentSystemPath Error.");
            }
            catch (EcellException)
            {
            }

        }

        /// <summary>
        /// TestNormalizeVariableReference
        /// </summary>
        [Test()]
        public void TestNormalizeVariableReference()
        {
            EcellReference er = new EcellReference("V1", ":.:V1", 1, 1);
            string systemPath = "/S0/S1";
            Util.NormalizeVariableReference(er, systemPath);
            Assert.AreEqual("/S0/S1:V1", er.Key, "NormalizeSystemPath method returned unexpected result.");

            er = new EcellReference("V1", ":..:V1", 1, 1);
            Util.NormalizeVariableReference(er, systemPath);
            Assert.AreEqual("/S0:V1", er.Key, "NormalizeSystemPath method returned unexpected result.");
        }

        /// <summary>
        /// TestDoesVariableReferenceChange
        /// </summary>
        [Test()]
        public void TestDoesVariableReferenceChange()
        {
            EcellObject oldObj = EcellObject.CreateObject("Model", "/S0", "System", "System", new List<EcellData>());
            EcellObject newObj = EcellObject.CreateObject("Model", "/S0", "System", "System", new List<EcellData>());
            bool expectedBoolean = false;
            bool resultBoolean = false;
            try
            {
                resultBoolean = Util.DoesVariableReferenceChange(oldObj, newObj);
            }
            catch (EcellException)
            {
            }
            try
            {
                oldObj = EcellObject.CreateObject("Model", "/S0:P0", "Process", "MassActionProcess", new List<EcellData>());
                resultBoolean = Util.DoesVariableReferenceChange(oldObj, newObj);
            }
            catch (EcellException)
            {
            }

            newObj = oldObj.Clone();
            resultBoolean = Util.DoesVariableReferenceChange(oldObj, newObj);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesVariableReferenceChange method returned unexpected result.");

            expectedBoolean = true;

            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            EcellValue value = EcellValue.ConvertFromListString(str);
            newObj.SetEcellValue(EcellProcess.VARIABLEREFERENCELIST, value);

            resultBoolean = Util.DoesVariableReferenceChange(oldObj, newObj);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesVariableReferenceChange method returned unexpected result.");

            resultBoolean = Util.DoesVariableReferenceChange(newObj, oldObj);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesVariableReferenceChange method returned unexpected result.");
        }

        /// <summary>
        /// TestGenerateRandomID
        /// </summary>
        [Test()]
        public void TestGenerateRandomID()
        {
            int len = 0;
            string expectedString = "";
            string resultString = null;
            resultString = Util.GenerateRandomID(len);
            Assert.AreEqual(expectedString, resultString, "GenerateRandomID method returned unexpected result.");

            len = 1;
            resultString = Util.GenerateRandomID(len);
            Assert.AreEqual(len, resultString.Length, "GenerateRandomID method returned unexpected result.");

            len = 10;
            resultString = Util.GenerateRandomID(len);
            Assert.AreEqual(len, resultString.Length, "GenerateRandomID method returned unexpected result.");
        }

        /// <summary>
        /// TestInitialLanguage
        /// </summary>
        [Test()]
        public void TestInitialLanguage()
        {
            CultureInfo lang = CultureInfo.GetCultureInfo("ja");
            Util.SetLanguage(lang);
            Util.InitialLanguage();
            CultureInfo resultCultureInfo = Util.GetLanguage();
            Assert.AreEqual(lang, resultCultureInfo, "TestInitialLanguage method set unexpected lang.");
        }

        /// <summary>
        /// TestGetLanguage
        /// </summary>
        [Test()]
        public void TestGetLanguage()
        {
            CultureInfo expectedCultureInfo = CultureInfo.InvariantCulture;
            CultureInfo resultCultureInfo = CultureInfo.InvariantCulture;
            Util.SetLanguage(expectedCultureInfo);
            resultCultureInfo = Util.GetLanguage();
            Assert.AreEqual(expectedCultureInfo, resultCultureInfo, "GetLanguage method returned unexpected result.");
        }

        /// <summary>
        /// TestSetLanguage
        /// </summary>
        [Test()]
        public void TestSetLanguage()
        {
            CultureInfo resultCultureInfo = CultureInfo.InvariantCulture;
            CultureInfo expectedCultureInfo = CultureInfo.GetCultureInfo("ja");
            Util.SetLanguage(expectedCultureInfo);
            resultCultureInfo = Util.GetLanguage();
            Assert.AreEqual(expectedCultureInfo, resultCultureInfo, "SetLanguage method returned unexpected result.");

            expectedCultureInfo = CultureInfo.InvariantCulture;
            Util.SetLanguage(expectedCultureInfo);
            resultCultureInfo = Util.GetLanguage();
            Assert.AreEqual(expectedCultureInfo, resultCultureInfo, "SetLanguage method returned unexpected result.");
        }

        /// <summary>
        /// TestShowErrorDialog
        /// </summary>
        [Test()]
        public void TestShowErrorDialog()
        {
            string msg = "ErrorDialog";
            Util.ShowErrorDialog(msg);
        }

        /// <summary>
        /// TestShowWarningDialog
        /// </summary>
        [Test()]
        public void TestShowWarningDialog()
        {
            string msg = "WarningDialog";
            Util.ShowWarningDialog(msg);

        }

        /// <summary>
        /// TestShowNoticeDialog
        /// </summary>
        [Test()]
        public void TestShowNoticeDialog()
        {
            string msg = "NoticeDialog";
            Util.ShowNoticeDialog(msg);
        }

        /// <summary>
        /// TestShowYesNoDialog
        /// </summary>
        [Test()]
        public void TestShowYesNoDialog()
        {
            string msg = "Click Yes Button";
            bool expectedBoolean = true;
            bool resultBoolean = false;
            resultBoolean = Util.ShowYesNoDialog(msg);
            Assert.AreEqual(expectedBoolean, resultBoolean, "ShowYesNoDialog method returned unexpected result.");

            msg = "Click No Button";
            expectedBoolean = false;
            resultBoolean = true;
            resultBoolean = Util.ShowYesNoDialog(msg);
            Assert.AreEqual(expectedBoolean, resultBoolean, "ShowYesNoDialog method returned unexpected result.");
        }

        /// <summary>
        /// TestShowYesNoCancelDialog
        /// </summary>
        [Test()]
        public void TestShowYesNoCancelDialog()
        {
            string msg = "Click Yes Button";
            bool expectedBoolean = true;
            bool resultBoolean = true;
            resultBoolean = Util.ShowYesNoCancelDialog(msg);
            Assert.AreEqual(expectedBoolean, resultBoolean, "ShowYesNoCancelDialog method returned unexpected result.");

            msg = "Click No Button";
            expectedBoolean = false;
            resultBoolean = true;
            resultBoolean = Util.ShowYesNoCancelDialog(msg);
            Assert.AreEqual(expectedBoolean, resultBoolean, "ShowYesNoCancelDialog method returned unexpected result.");

            try
            {
                msg = "Click Cancel Button";
                expectedBoolean = false;
                resultBoolean = true;
                resultBoolean = Util.ShowYesNoCancelDialog(msg);
                Assert.AreEqual(expectedBoolean, resultBoolean, "ShowYesNoCancelDialog method returned unexpected result.");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// TestShowOKCancelDialog
        /// </summary>
        [Test()]
        public void TestShowOKCancelDialog()
        {
            string msg = "Click OK Button";
            bool expectedBoolean = true;
            bool resultBoolean = true;
            resultBoolean = Util.ShowOKCancelDialog(msg);
            Assert.AreEqual(expectedBoolean, resultBoolean, "ShowOKCancelDialog method returned unexpected result.");

            msg = "Click Cancel Button";
            expectedBoolean = false;
            resultBoolean = true;
            resultBoolean = Util.ShowOKCancelDialog(msg);
            Assert.AreEqual(expectedBoolean, resultBoolean, "ShowOKCancelDialog method returned unexpected result.");
        }

        /// <summary>
        /// TestGetColorBrush
        /// </summary>
        [Test()]
        public void TestGetColorBrush()
        {
            Brush brush = Util.GetColorBlush(0);
            Assert.AreEqual(Brushes.OrangeRed, brush, "GetColorBlush method returned unexpected result.");
            brush = Util.GetColorBlush(1);
            Assert.AreEqual(Brushes.LightSkyBlue, brush, "GetColorBlush method returned unexpected result.");
            brush = Util.GetColorBlush(2);
            Assert.AreEqual(Brushes.LightGreen, brush, "GetColorBlush method returned unexpected result.");
            brush = Util.GetColorBlush(3);
            Assert.AreEqual(Brushes.OrangeRed, brush, "GetColorBlush method returned unexpected result.");
            brush = Util.GetColorBlush(4);
            Assert.AreEqual(Brushes.LightSkyBlue, brush, "GetColorBlush method returned unexpected result.");

            brush = Util.GetColorBlush(-1);
            Assert.AreEqual(Brushes.Black, brush, "GetColorBlush method returned unexpected result.");
        }

        /// <summary>
        /// TestGetColorBrush
        /// </summary>
        [Test()]
        public void TestGetColor()
        {
            Color color = Util.GetColor(0);
            Assert.AreEqual(Color.OrangeRed, color, "GetColorBlush method returned unexpected result.");
            color = Util.GetColor(1);
            Assert.AreEqual(Color.LightSkyBlue, color, "GetColorBlush method returned unexpected result.");
            color = Util.GetColor(2);
            Assert.AreEqual(Color.LightGreen, color, "GetColorBlush method returned unexpected result.");
            color = Util.GetColor(3);
            Assert.AreEqual(Color.OrangeRed, color, "GetColorBlush method returned unexpected result.");
            color = Util.GetColor(4);
            Assert.AreEqual(Color.LightSkyBlue, color, "GetColorBlush method returned unexpected result.");

            color = Util.GetColor(-1);
            Assert.AreEqual(Color.Black, color, "GetColorBlush method returned unexpected result.");
        }

        /// <summary>
        /// TestGetProcessTemplate
        /// </summary>
        [Test()]
        public void TestGetProcessTemplate()
        {
            List<string> list = Util.GetProcessTemplateList();
            string dm = Util.GetProcessTemplate("SampleProcess");
            Assert.AreEqual("", dm, "GetProcessTemplate returned unexpected result.");

            dm = Util.GetProcessTemplate("ConstantFluxProcess");
            Assert.AreNotEqual("", dm, "GetProcessTemplate returned unexpected result.");

        }

        /// <summary>
        /// TestIsDMFile
        /// </summary>
        [Test()]
        public void TestIsDMFile()
        {
            bool expectedBoolean = false;
            bool resultedBoolean = Util.IsDMFile("c:/temp/rbc.eml");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsDMFile returned unexpected result.");

            resultedBoolean = Util.IsDMFile("c:/temp/hoge.dll");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsDMFile returned unexpected result.");

            expectedBoolean = true;

            resultedBoolean = Util.IsDMFile("c:/temp/hogeProcess.dll");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsDMFile returned unexpected result.");

            resultedBoolean = Util.IsDMFile("C:/temp/CoupledOscillator/DMs/FOProcess.dll");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsDMFile returned unexpected result.");

            resultedBoolean = Util.IsDMFile("SampleStepper.dll");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsDMFile returned unexpected result.");

            resultedBoolean = Util.IsIgnoredDir("c:/Temp/Drosophila/Model");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsDMFile returned unexpected result.");

            expectedBoolean = false;

            resultedBoolean = Util.IsHidden("c:/Temp/Drosophila/");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsDMFile returned unexpected result.");

            Util.AddDMDir("");
            Util.AddPluginDir("");

            expectedBoolean = true;
            resultedBoolean = Util.IsInstalledSDK();
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsInstalledSDK returned unexpected result.");
        }
        
        /// <summary>
        /// TestGetColorBrush
        /// </summary>
        [Test()]
        public void TestIsExistProject()
        {
            bool expectedBoolean = false;
            bool resultedBoolean = false;

            resultedBoolean = Util.IsExistProject(null);
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsExistProject returned unexpected result.");

            resultedBoolean = Util.IsExistProject("Hoge");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsExistProject returned unexpected result.");

            expectedBoolean = true;

            resultedBoolean = Util.IsExistProject("Oscillation");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsExistProject returned unexpected result.");

            resultedBoolean = Util.IsExistProject("Drosophila");
            Assert.AreEqual(expectedBoolean, resultedBoolean, "IsExistProject returned unexpected result.");
        }

        
        /// <summary>
        /// TestGetColorBrush
        /// </summary>
        [Test()]
        public void TestDirectorySettings()
        {
            Dictionary<string, List<string>> dic = Util.GetDmDic(null);
            Assert.IsNotEmpty(dic, "GetDmDic method returned unexpected result.");
            Assert.IsNotEmpty(dic[EcellObject.SYSTEM], "dic[EcellObject.SYSTEM] returned unexpected result.");
            Assert.IsNotEmpty(dic[EcellObject.VARIABLE], "dic[EcellObject.VARIABLE] returned unexpected result.");
            Assert.IsNotEmpty(dic[EcellObject.PROCESS], "dic[EcellObject.PROCESS] returned unexpected result.");
            Assert.IsNotEmpty(dic[EcellObject.STEPPER], "dic[EcellObject.PROCESS] returned unexpected result.");

            string file = Util.GetStartupFile();
            Assert.IsNotNull(file, "GetStartupFile method returned unexpected result.");

            file = Util.GetOutputFileName("/S0/S1:V0:Value");
            Assert.IsNotNull(file, "GetStartupFile method returned unexpected result.");

            string dir = Util.GetBaseDir();
            Assert.IsNotNull(dir, "GetBaseDir method returned unexpected result.");
            Util.SetBaseDir(dir);

            dir = Util.GetBinDir();
            Assert.IsNotNull(dir, "GetBinDir method returned unexpected result.");

            dir = Util.GetAnalysisDir();
            Assert.IsNotNull(dir, "GetAnalysisDir method returned unexpected result.");

            dir = Util.GetWindowSettingDir();
            Assert.IsNotNull(dir, "GetWindowSettingDir method returned unexpected result.");
            Util.SetWindowSettingDir(dir);

            dir = Util.GetTmpDir();
            Assert.IsNotNull(dir, "GetTmpDir method returned unexpected result.");

            dir = Util.GetUserDir();
            Assert.IsNotNull(dir, "GetUserDir method returned unexpected result.");

            dir = Util.GetCommonDocumentDir();
            Assert.IsNotNull(dir, "GetCommonDocumentDir method returned unexpected result.");

            string[] dirs = Util.GetDMDirs(null);
            Assert.IsNotNull(dirs, "GetDMDirs method returned unexpected result.");

            dirs = Util.GetPluginDirs();
            Assert.IsNotNull(dirs, "GetPluginDirs method returned unexpected result.");

            Util.OmitDefaultPaths();

            Util.AddDMDir("");
            Util.AddPluginDir("");
        }

    }
}
