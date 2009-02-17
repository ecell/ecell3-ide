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
    public class TestStatusUpdateEventArgs
    {

        private StatusUpdateEventArgs _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.StatusBarMessageKind type = StatusBarMessageKind.Generic;
            string text = null;
            _unitUnderTest = new StatusUpdateEventArgs(type, text);
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
        public void TestConstructorStatusUpdateEventArgs()
        {
            Ecell.StatusBarMessageKind type = StatusBarMessageKind.QuickInspector;
            string text = "Statud";
            StatusUpdateEventArgs testStatusUpdateEventArgs = new StatusUpdateEventArgs(type, text);
            Assert.IsNotNull(testStatusUpdateEventArgs, "Constructor of type, StatusUpdateEventArgs failed to create instance.");
            Assert.AreEqual(text, testStatusUpdateEventArgs.Text, "Text is unexpected value.");
            Assert.AreEqual(type, testStatusUpdateEventArgs.Type, "Type is unexpected value.");

        }
    }
}
