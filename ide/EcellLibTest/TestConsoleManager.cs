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
// written by Moriyoshi Koizumi <mozo@sfc.keio.ac.jp>
//

namespace Ecell
{
    using System;
    using NUnit.Framework;
    using Ecell;
    using System.Text;
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestConsoleManager
    {
        private ApplicationEnvironment _env;
        private ConsoleManager _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = new ConsoleManager(_env);
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
        public void TestConstructorConsoleManager()
        {
            ConsoleManager testConsoleManager = new ConsoleManager(_env);
            Assert.IsNotNull(testConsoleManager, "Constructor of type, ConsoleManager failed to create instance.");
            Assert.AreEqual(_env, testConsoleManager.Environment, "Environment is unexpected value.");
            Assert.AreEqual(Encoding.UTF8, testConsoleManager.Encoding, "Encoding is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestFlush()
        {
            _unitUnderTest.ConsoleDataAvailable += new ConsoleDataAvailableEventHandler(_unitUnderTest_ConsoleDataAvailable);
            _unitUnderTest.Flush();

        }

        void _unitUnderTest_ConsoleDataAvailable(object o, ConsoleDataAvailableEventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestWriteC()
        {
            _unitUnderTest.Write('A');
            _unitUnderTest.Write('\n');
            _unitUnderTest.Write('B');
            _unitUnderTest.Write('C');
            _unitUnderTest.Write('\r');
            _unitUnderTest.Write('\n');

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestWriteBufIndexCount()
        {
            char[] buf = new char[] { };
            int index = 0;
            int count = 0;
            _unitUnderTest.Write(buf, index, count);

            count = 3;
            buf = new char[] {'A', '\n','B', 'C', '\r', '\n'};
            _unitUnderTest.Write(buf, index, count);

        }
    }
}
