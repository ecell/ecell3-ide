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
    using System.Windows.Forms;
    using NUnit.Framework;
    using System.IO;
    using System.Diagnostics;
    using Ecell.Objects;
    using System.Collections.Generic;
    using Ecell.Exceptions;
    using EcellCoreLib;
    using System.Reflection;
    using System.Collections;
    using Ecell.Plugin;
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestDMCompiler
    {
        private ApplicationEnvironment _env;
        private DataManager _unitUnderTest;
        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [SetUp()]
        public void TestFixtureSetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = _env.DataManager;
        }
        /// <summary>
        /// TestFixtureTearDown
        /// </summary>
        [TearDown()]
        public void TestFixtureTearDown()
        {
            _unitUnderTest = null;
            _env = null;
        }

        
        /// <summary>
        /// TestScriptWriterForUnix
        /// </summary>
        [Test()]
        public void TestCompile()
        {
            if (Directory.Exists(TestConstant.DM_Source_Directory))
                Directory.Delete(TestConstant.DM_Source_Directory, true);

            string fileName = TestConstant.DM_Source_File;
            DMCompiler.Compile(fileName, _env);

            fileName = TestConstant.DM_Source_File2;
            DMCompiler.Compile(fileName, _env);
        }

    }
}
