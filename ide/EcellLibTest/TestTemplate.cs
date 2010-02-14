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

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.Drawing;

namespace Ecell
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestTemplate
    {
        private object _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new object();
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
            object obj = new object();
            Assert.IsNotNull(obj, "Constructor of type, object failed to create instance.");
            Assert.AreEqual(obj.GetType(), obj.GetType(), "Type is unexpected value.");
            Assert.AreEqual("System.Object", obj.GetType().ToString(), "GetType method returned unexpected value.");
        }

        /// <summary>
        /// TestSample for Error case
        /// </summary>
        [Test()]
        public void TestSampleForErrorCase()
        {
            try
            {
                object obj = new object();
                // parse error.
                List<string> list = (List<string>)obj;
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }

        
        /// <summary>
        /// Test SampleCode
        /// </summary>
        [Test()]
        public void TestSampleCode()
        {
            List<PointF> list = new List<PointF>();
            PointF point1 = new PointF(0, 0);
            list.Add(point1);

            Assert.IsTrue(list.Contains(point1));

            PointF point2 = new PointF(0, 0);
            Assert.IsTrue(list.Contains(point2));

            PointF point3 = new PointF(0, 10);
            Assert.IsFalse(list.Contains(point3));

        }

    }
}
