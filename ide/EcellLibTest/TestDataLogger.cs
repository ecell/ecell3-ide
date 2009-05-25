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
    public class TestDataLogger
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
        /// TestAccessor
        /// </summary>
        [Test()]
        public void TestLoggerPolicy()
        {
            DiskFullAction expectedFullAction = DiskFullAction.Terminate;
            int expectedMaxDiskSpace = 120;
            double expectedReloadInterval = 0.1;
            int expectedReloadStepCount = 111;

            LoggerPolicy d = new LoggerPolicy();
            d.DiskFullAction = expectedFullAction;
            d.MaxDiskSpace = expectedMaxDiskSpace;
            d.ReloadInterval = expectedReloadInterval;
            d.ReloadStepCount = expectedReloadStepCount;

            Assert.AreEqual(d.DiskFullAction, expectedFullAction, "LoggerPolicy Accessor returned unexpected value.");
            Assert.AreEqual(d.MaxDiskSpace, expectedMaxDiskSpace, "LoggerPolicy Accessor returned unexpected value.");
            Assert.AreEqual(d.ReloadInterval, expectedReloadInterval, "LoggerPolicy Accessor returned unexpected value.");
            Assert.AreEqual(d.ReloadStepCount, expectedReloadStepCount, "LoggerPolicy Accessor returned unexpected value.");

            LoggerPolicy dclone = (LoggerPolicy)d.Clone();
            Assert.AreEqual(d.DiskFullAction, dclone.DiskFullAction, "LoggerPolicy clone execute unexpected action.");
            Assert.AreEqual(d.ReloadStepCount, dclone.ReloadStepCount, "LoggerPolicy clone execute unexpected action.");
            Assert.AreEqual(d.ReloadInterval, dclone.ReloadInterval, "LoggerPolicy clone execute unexpected action.");
            Assert.AreEqual(d.MaxDiskSpace, dclone.MaxDiskSpace, "LoggerPolicy clone execute unexpected action.");
        }
        
        /// <summary>
        /// TestAccessor
        /// </summary>
        [Test()]
        public void TestSaveLoggerProperty()
        {
            double expectedStart = 0.0;
            double expectedEnd = 0.0;
            string expectedDir = "";
            string expectedPath = "";

            SaveLoggerProperty p = new SaveLoggerProperty();
            Assert.AreEqual(p.DirName, expectedDir, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(p.Start, expectedStart, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(p.End, expectedEnd, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(p.FullPath, expectedPath, "SaveLoggerProperty Accessor is unexpected value.");

            expectedStart = 10.0;
            expectedEnd = 100.0;
            expectedDir = TestConstant.TestDirectory;
            expectedPath = "Variable:/:P0:Value";
            
            p.DirName = expectedDir;
            p.End = expectedEnd;
            p.Start = expectedStart;
            p.FullPath = expectedPath;

            Assert.AreEqual(p.DirName, expectedDir, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(p.Start, expectedStart, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(p.End, expectedEnd, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(p.FullPath, expectedPath, "SaveLoggerProperty Accessor is unexpected value.");

            SaveLoggerProperty d = new SaveLoggerProperty(expectedPath, expectedStart, expectedEnd, expectedDir);
            Assert.AreEqual(d.DirName, expectedDir, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(d.Start, expectedStart, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(d.End, expectedEnd, "SaveLoggerProperty Accessor is unexpected value.");
            Assert.AreEqual(d.FullPath, expectedPath, "SaveLoggerProperty Accessor is unexpected value.");
        }

        /// <summary>
        /// TestAccessor
        /// </summary>
        [Test()]
        public void TestLogData()
        {
            string expectedModeID = null;
            string expectedKey = null;
            string expectedPropName = null;
            string expectedType = null;
            bool expectedIsLoad = false;
            string expectedFileName = null;

            LogData data = new LogData();
            Assert.AreEqual(expectedModeID, data.model, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedKey, data.key, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedType, data.type, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedPropName, data.propName, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedIsLoad, data.IsLoaded, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedFileName, data.FileName, "LogData accessor is unexpected value.");
            Assert.IsNull(data.logValueList, "LogData accessor is unexpected value.");

            expectedModeID = "Model";
            expectedKey = "KeyData";
            expectedType = "Type";
            expectedPropName = "PropData";
            expectedIsLoad = true;
            expectedFileName = "FileName";

            data.IsLoaded = expectedIsLoad;
            data.FileName = expectedFileName;
            Assert.AreEqual(expectedIsLoad, data.IsLoaded, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedFileName, data.FileName, "LogData accessor is unexpected value.");

            LogData d = new LogData(expectedModeID, expectedKey, expectedType, expectedPropName, new List<LogValue>());
            Assert.AreEqual(expectedModeID, d.model, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedKey, d.key, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedType, d.type, "LogData accessor is unexpected value.");
            Assert.AreEqual(expectedPropName, d.propName, "LogData accessor is unexpected value.");
            Assert.AreEqual(0, d.logValueList.Count, "LogData accessor is unexpected value.");
        }

        /// <summary>
        /// TestAccessor
        /// </summary>
        [Test()]
        public void TestLogValue()
        {
            double expectedAvg = double.NaN;
            double expectedMax = double.NaN;
            double expectedMin = double.NaN;
            double expectedTime = double.NaN;
            double expectedValue = double.NaN;

            LogValue v = new LogValue();
            Assert.AreEqual(v.avg, expectedAvg, "LogValue Accessor is unexpected value.");
            Assert.AreEqual(v.max, expectedMax, "LogValue Accessor is unexpected value.");
            Assert.AreEqual(v.min, expectedMin, "LogValue Accessor is unexpected value.");
            Assert.AreEqual(v.time, expectedTime, "LogValue Accessor is unexpected value.");
            Assert.AreEqual(v.value, expectedValue, "LogValue Accessor is unexpected value.");

            expectedAvg = 0.1;
            expectedMax = 0.2;
            expectedMin = 0.3;
            expectedTime = 0.4;
            expectedValue = 0.5;

            v = new LogValue(expectedTime, expectedValue, expectedAvg, expectedMin, expectedMax);
            Assert.AreEqual(v.avg, expectedAvg, "LogValue Accessor is unexpected value.");
            Assert.AreEqual(v.max, expectedMax, "LogValue Accessor is unexpected value.");
            Assert.AreEqual(v.min, expectedMin, "LogValue Accessor is unexpected value.");
            Assert.AreEqual(v.time, expectedTime, "LogValue Accessor is unexpected value.");
            Assert.AreEqual(v.value, expectedValue, "LogValue Accessor is unexpected value.");
        }
    }
}
