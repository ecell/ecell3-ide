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
using System.Drawing;


namespace Ecell.Plugin
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestLayoutBase
    {
        private ApplicationEnvironment _env;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _env = null;
        }

        /// <summary>
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestConstructor()
        {
            TestLayout layout = new TestLayout();
            Assert.IsNotNull(layout, "Constructor of type, object failed to create instance.");
            // env
            layout.Environment = _env;
            layout.Initialize();
            layout.ChangeStatus(ProjectStatus.Uninitialized);
            layout.SetPluginStatus(null);
            Assert.AreEqual(_env, layout.Environment, "Environment is unexpected value.");

            Assert.AreEqual("TestLayout", layout.GetPluginName(), "GetToolBarMenuStrip method returned unexpected value.");
            Assert.AreEqual("TestLayout", layout.GetLayoutName(), "GetToolBarMenuStrip method returned unexpected value.");
            Assert.IsNull(layout.Panel, "Panel is not expected value.");
            layout.SubIndex = 0;
            Assert.AreEqual(0, layout.SubIndex, "SubIndex is not expected value.");
            Assert.IsNull(layout.GetPluginStatus(), "GetPluginStatus method returned unexpected value.");
            Assert.IsNull(layout.GetPropertySettings(), "GetPropertySettings method returned unexpected value.");
            Assert.IsNull(layout.GetPublicDelegate(), "GetPublicDelegate method returned unexpected value.");

        }
        
        /// <summary>
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestStaticMethod()
        {
            TestLayout layout = new TestLayout();
            Assert.IsNotNull(layout, "Constructor of type, object failed to create instance.");
            // env
            layout.Environment = _env;

            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            List<EcellObject> list = _env.DataManager.GetData("Drosophila", null);

            List<EcellObject> selected = TestLayout.GetSelectedObject(list);
            List<EcellObject> related = TestLayout.GetRelatedObject(list);
            RectangleF rect = TestLayout.GetSurroundingRect(list);

        }

        /// <summary>
        /// 
        /// </summary>
        internal class TestLayout : LayoutBase
        {

            public override bool DoLayout(int subCommandNum, bool layoutSystem, List<Ecell.Objects.EcellObject> systemList, List<Ecell.Objects.EcellObject> nodeList)
            {
                return false;
            }

            public override string GetLayoutName()
            {
                return "TestLayout";
            }

            public override LayoutType GetLayoutType()
            {
                return LayoutType.Whole;
            }

            public override string GetPluginName()
            {
                return "TestLayout";
            }

            public override string GetVersionString()
            {
                return "1.0";
            }
        }
    }
}
