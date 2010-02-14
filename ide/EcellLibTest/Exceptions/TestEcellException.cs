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
using EcellCoreLib;

namespace Ecell.Exceptions
{
    /// <summary>
    /// TestEcellException
    /// </summary>
    [TestFixture()]
    public class TestEcellException
    {
        private EcellException _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EcellException();
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
        /// TestConstructorEcellException
        /// </summary>
        [Test()]
        public void TestConstructorEcellException()
        {
            EcellException exception = new EcellException();
            Assert.IsNotNull(exception, "Constructor of type, EcellException failed to create instance.");

            exception = new EcellException("Message");
            Assert.IsNotNull(exception, "Constructor of type, EcellException failed to create instance.");
            Assert.AreEqual("Message", exception.Message, "Message is unexpected value.");

            exception = new EcellException("Message", new Exception());
            Assert.IsNotNull(exception, "Constructor of type, EcellException failed to create instance.");
            Assert.AreEqual("Message", exception.Message, "Message is unexpected value.");
        }
        
        /// <summary>
        /// TestConstructorIgnoreException
        /// </summary>
        [Test()]
        public void TestConstructorIgnoreException()
        {
            IgnoreException exception = new IgnoreException("Message");
            Assert.IsNotNull(exception, "Constructor of type, IgnoreException failed to create instance.");
            Assert.AreEqual("Message", exception.Message, "Message is unexpected value.");

            exception = new IgnoreException("Message", new Exception());
            Assert.IsNotNull(exception, "Constructor of type, IgnoreException failed to create instance.");
            Assert.AreEqual("Message", exception.Message, "Message is unexpected value.");
        }
        
        /// <summary>
        /// TestConstructorSimulationException
        /// </summary>
        [Test()]
        public void TestConstructorSimulationException()
        {
            SimulationException exception = new SimulationException("Message", new WrappedException());
            Assert.IsNotNull(exception, "Constructor of type, SimulationException failed to create instance.");
            Assert.AreEqual("Message", exception.Message, "Message is unexpected value.");
            Assert.IsNotNull(exception.InnerException, "InnerException is unexpected value.");

        }
    }
}
