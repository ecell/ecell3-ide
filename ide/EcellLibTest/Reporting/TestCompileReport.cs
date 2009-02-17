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

namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestCompileReport
    {

        private CompileReport _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.MessageType type = MessageType.Information;
            string message = null;
            string group = null;
            _unitUnderTest = new CompileReport(type, message, group);
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
        public void TestConstructorCompileReport()
        {
            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            CompileReport testCompileReport = new CompileReport(type, message, group);
            Assert.IsNotNull(testCompileReport, "Constructor of type, CompileReport failed to create instance.");
            Assert.AreEqual(MessageType.Error, testCompileReport.Type, "Type is unexpected value.");
            Assert.AreEqual(message, testCompileReport.Message, "Message is unexpected value.");
            Assert.AreEqual(group, testCompileReport.Group, "Group is unexpected value.");
            Assert.AreEqual("Compile Error", testCompileReport.Location, "Location is unexpected value.");
            Assert.AreNotEqual(0, testCompileReport.GetHashCode(), "GetHashCode method returns unexpected value.");
            Assert.IsNotNull(testCompileReport.ToString(), "ToString method returns unexpected value.");
        }
    }
}
