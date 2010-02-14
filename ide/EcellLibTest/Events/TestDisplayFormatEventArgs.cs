﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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

namespace Ecell.Job
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.IO;
    using Ecell.Objects;
    using Ecell.Events;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestDisplayFormatEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAccessor()
        {
            string displayFormant = "AAAAA";
            DisplayFormatEventArgs d = new DisplayFormatEventArgs(displayFormant);

            Assert.AreEqual(d.DisplayFormat, displayFormant, "DisplayEventArgs Accessors is unexpected value.");
        }
    }
}
