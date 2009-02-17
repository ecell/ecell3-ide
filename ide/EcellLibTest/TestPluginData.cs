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

namespace Ecell
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestPluginData
    {
        private PluginData _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new PluginData();
        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }
        
        /// <summary>
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestConstructor()
        {
            PluginData data = new PluginData();
            Assert.IsNotNull(data, "Constructor of type, PluginData failed to create instance.");
            Assert.AreEqual("model1", data.ModelID, "ModelID is unexpected value.");
            Assert.AreEqual("key1", data.Key, "Key is unexpected value.");

            data.ModelID = "newModel";
            data.Key = "newKey";
            Assert.AreEqual("newModel", data.ModelID, "ModelID is unexpected value.");
            Assert.AreEqual("newKey", data.Key, "Key is unexpected value.");

            data = new PluginData("Model", "Key");
            Assert.IsNotNull(data, "Constructor of type, PluginData failed to create instance.");
            Assert.AreEqual("Model", data.ModelID, "ModelID is unexpected value.");
            Assert.AreEqual("Key", data.Key, "Key is unexpected value.");

        }
        
        /// <summary>
        /// TestEquals
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            PluginData data1 = new PluginData("Model", "Key");
            PluginData data2 = new PluginData("Model1", "Key");
            PluginData data3 = new PluginData("Model", "Key1");
            PluginData data4 = new PluginData("Model", "Key");

            Assert.IsFalse(data1.Equals(data2), "Equals method returned unexpected value.");
            Assert.IsFalse(data1.Equals(data3), "Equals method returned unexpected value.");

            Assert.IsTrue(data1.Equals(data4), "Equals method returned unexpected value.");
        }
    }
}
