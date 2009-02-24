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
using Ecell.Job;
using Ecell.Logging;
using Ecell.Action;
using Ecell.Plugin;

namespace Ecell
{

    /// <summary>
    /// Test for ApplicationEnvironment
    /// </summary>
    [TestFixture()]
    public class TestApplicationEnvironment
    {
        private ApplicationEnvironment _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new ApplicationEnvironment();
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
        /// Test of Constructors
        /// </summary>
        [Test()]
        public void TestAccessors()
        {
            DataManager dm = _unitUnderTest.DataManager;
            Assert.IsNotNull(dm);

            CommandManager cm = _unitUnderTest.CommandManager;
            Assert.IsNotNull(cm);

            DynamicModuleManager dmm = _unitUnderTest.DynamicModuleManager;
            Assert.IsNotNull(dmm);

            ConsoleManager console = _unitUnderTest.Console;
            Assert.IsNotNull(console);

            ActionManager am = _unitUnderTest.ActionManager;
            Assert.IsNotNull(am);

            IJobManager jm = _unitUnderTest.JobManager;
            Assert.IsNotNull(jm);

            PluginManager pm = _unitUnderTest.PluginManager;
            Assert.IsNotNull(pm);

            LogManager lm = _unitUnderTest.LogManager;
            Assert.IsNotNull(lm);

        }

    }
}
