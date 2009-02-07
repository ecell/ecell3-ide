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
    using Ecell.Exceptions;
    using Ecell.IDE;
    using System.Diagnostics;

    /// <summary>
    /// Test of ZipUtil
    /// </summary>
    [TestFixture()]
    public class TestZipUtil
    {

        private ZipUtil _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new ZipUtil();
        }
        /// <summary>
        /// Destructor
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }
        /// <summary>
        /// Test of ZipFile() method.
        /// </summary>
        [Test()]
        public void TestZipFile()
        {
            string zipname = null;
            string filePath = null;
            zipname = null;
            filePath = null;
            try
            {
                ZipUtil.ZipFile(zipname, filePath);
                Assert.Fail("Failed to catch invalid file error.");
            }
            catch (EcellException e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            zipname = "";
            filePath = "";
            try
            {
                ZipUtil.ZipFile(zipname, filePath);
                Assert.Fail("Failed to catch invalid file error.");
            }
            catch (EcellException e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            zipname = "c:\\temp\\rbc.zip";
            filePath = "c:\\temp\\rbc.eml";
            ZipUtil.ZipFile(zipname, filePath);

            zipname = "c:\\temp\\test.zip";
            filePath = "";
            ZipUtil.ZipFile(zipname, filePath);
        }

        /// <summary>
        /// Test of TestZipFiles()
        /// </summary>
        [Test()]
        public void TestZipFiles()
        {
            string zipname = "c:\\temp\\rbc2.zip";
            string[] filePaths = new string[] { "c:\\temp\\rbc.eml", "c:\\temp\\rbc.leml" };
            ZipUtil.ZipFiles(zipname, filePaths);
        }

        /// <summary>
        /// Test of ZipFolder()
        /// </summary>
        [Test()]
        public void TestZipFolder()
        {
            string zipname = "c:\\temp\\Drosophila.zip";
            string folderPath = "c:\\temp\\Drosophila";
            ZipUtil.ZipFolder(zipname, folderPath);

            zipname = "c:\\temp\\Drosophila.zip";
            folderPath = "";
            try
            {
                ZipUtil.ZipFolder(zipname, folderPath);
                Assert.Fail("Failed to catch invalid file error.");
            }
            catch (EcellException e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }
    }
}
