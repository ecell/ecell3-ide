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
using System.IO;

namespace Ecell
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestProjectInfo
    {

        const string Name = "Project";
        const string Comment = "Comment";
        const string Time = "2008/12/28";
        const string SimParam = "DefaultParameter";

        private ProjectInfo _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new ProjectInfo();
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
        public void TestProjectInfoConstructors()
        {
            // Correct Check
            try
            {
                ProjectInfo info;
                info = new ProjectInfo();
                //
                info = new ProjectInfo(Name, Comment, Time);
                info = new ProjectInfo(Name, "", Time);
                info = new ProjectInfo(Name, Comment, "");
                info = new ProjectInfo(Name, "", "");
                info = new ProjectInfo(Name, Comment, null);
                info = new ProjectInfo(Name, null, Time);
                info = new ProjectInfo(Name, null, null);
                //
                info = new ProjectInfo(Name, Comment, Time, SimParam);
                info = new ProjectInfo(Name, "", Time, SimParam);
                info = new ProjectInfo(Name, null, Time, SimParam);
                info = new ProjectInfo(Name, Comment, "", SimParam);
                info = new ProjectInfo(Name, Comment, null, SimParam);
                info = new ProjectInfo(Name, null, null, SimParam);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Error on Constructor.");
            }
        }

        /// <summary>
        /// Test of Constructors
        /// </summary>
        [Test()]
        public void TestProjectInfoConstructorsErrorCase()
        {
            // Error Check
            // ProjectInfo don't arrow null for name field.
            try
            {
                ProjectInfo info = new ProjectInfo("", Comment, Time);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            // Error Check
            // ProjectInfo don't arrow null for name field.
            try
            {
                ProjectInfo info = new ProjectInfo(null, Comment, Time);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            // Error Check 
            try
            {
                ProjectInfo info = new ProjectInfo("", Comment, Time, SimParam);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            // Error Check 
            try
            {
                ProjectInfo info = new ProjectInfo(null, Comment, Time, SimParam);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            // Error Check
            try
            {
                ProjectInfo info = new ProjectInfo(Name, "", "", "");
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            // Error Check
            try
            {
                ProjectInfo info = new ProjectInfo(Name, "", "", null);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Test of Accessors.
        /// </summary>
        [Test()]
        public void TestProjectInfoAccessors()
        {
            // Check Default Parameter.
            try
            {
                ProjectInfo info = new ProjectInfo();
                Assert.IsNotNull(info, "Constructor of type, ProjectInfo failed to create instance.");
                Assert.AreEqual(Constants.defaultPrjID, info.Name, "Name is unexpected value.");
                Assert.AreEqual(Constants.defaultComment, info.Comment, "Comment is unexpected value.");
                Assert.AreEqual(Constants.defaultSimParam, info.SimulationParam, "SimulationParam is unexpected value.");
                Assert.AreEqual(ProjectType.NewProject, info.ProjectType, "ProjectType is unexpected value.");
                Assert.IsNotNull(info.UpdateTime, "UpdateTime is unexpected value.");
                Assert.IsNull(info.ProjectPath, "ProjectPath is unexpected value.");
                Assert.IsNull(info.ProjectFile, "ProjectFile is unexpected value.");
                Assert.IsEmpty(info.DMDirList, "DMDirList is unexpected value.");
                Assert.IsEmpty(info.Models, "Models is unexpected value.");

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Error on Default Parameter.");
            }
            // Check Default Parameter.
            try
            {
                ProjectInfo info = new ProjectInfo(Name, Comment, Time, SimParam);
                Assert.IsNotNull(info, "Constructor of type, ProjectInfo failed to create instance.");
                Assert.AreEqual(Name, info.Name, "Name is unexpected value.");
                Assert.AreEqual(Comment, info.Comment, "Comment is unexpected value.");
                Assert.AreEqual(SimParam, info.SimulationParam, "SimulationParam is unexpected value.");
                Assert.AreEqual(Time, info.UpdateTime, "UpdateTime is unexpected value.");
                Assert.AreEqual(ProjectType.NewProject, info.ProjectType, "ProjectType is unexpected value.");
                Assert.IsNull(info.ProjectPath, "ProjectPath is unexpected value.");
                Assert.IsNull(info.ProjectFile, "ProjectFile is unexpected value.");
                Assert.IsEmpty(info.DMDirList, "DMDirList is unexpected value.");
                Assert.IsEmpty(info.Models, "Models is unexpected value.");

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Error on Default Parameter.");
            }

            // Check Accessor's Setter.
            try
            {
                ProjectInfo info = new ProjectInfo();
                Assert.IsNotNull(info, "Constructor of type, ProjectInfo failed to create instance.");

                info.Name = Name;
                info.Comment = Comment;
                info.UpdateTime = Time;
                info.SimulationParam = SimParam;
                info.ProjectPath = "";
                info.ProjectFile = "";
                info.DMDirList.Add("");
                info.Models.Add("");

                info.Comment = null;
                info.UpdateTime = null;
                info.ProjectPath = null;
                info.ProjectFile = null;
                info.DMDirList.Clear();
                info.Models.Clear();

                Assert.AreEqual(Name, info.Name, "Name is unexpected value.");
                Assert.AreEqual(null, info.Comment, "Comment is unexpected value.");
                Assert.AreEqual(SimParam, info.SimulationParam, "SimulationParam is unexpected value.");
                Assert.AreNotEqual(null, info.UpdateTime, "UpdateTime is unexpected value.");
                Assert.AreEqual(ProjectType.NewProject, info.ProjectType, "ProjectType is unexpected value.");
                Assert.IsNull(info.ProjectPath, "ProjectPath is unexpected value.");
                Assert.IsNull(info.ProjectFile, "ProjectFile is unexpected value.");
                Assert.IsEmpty(info.DMDirList, "DMDirList is unexpected value.");
                Assert.IsEmpty(info.Models, "Models is unexpected value.");

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Error on Default Parameter.");
            }

        }
        
        /// <summary>
        /// Test of Accessors.
        /// </summary>
        [Test()]
        public void TestProjectInfoAccessorsErrorCase()
        {
            // Check Accessor error.
            try
            {
                ProjectInfo info = new ProjectInfo();
                info.Name = null;
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            try
            {
                ProjectInfo info = new ProjectInfo();
                info.SimulationParam = null;
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Test of ProjectInfoLoader.
        /// </summary>
        [Test()]
        public void TestProjectInfoLoader()
        {
            try
            {
                // Load from ProjectXML
                string infoFile1 = TestConstant.Project_Drosophila;
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                Assert.AreEqual("Drosophila", info1.Name, "Name is unexpected value.");
                Assert.AreEqual(ProjectType.Project, info1.ProjectType, "ProjectType is unexpected value.");
                Assert.AreEqual(infoFile1, info1.ProjectFile, "ProjectFile is unexpected value.");
                Assert.AreEqual(Path.GetDirectoryName(infoFile1), info1.ProjectPath, "ProjectPath is unexpected value.");
                Assert.IsEmpty(info1.Models, "Models is unexpected value.");

                // Load from eml.
                string infoFile5 = TestConstant.Model_Drosophila;
                ProjectInfo info5 = ProjectInfoLoader.Load(infoFile5);
                Assert.AreEqual("Drosophila", info5.Name, "Name is unexpected value.");
                Assert.AreEqual(ProjectType.Model, info5.ProjectType, "ProjectType is unexpected value.");
                Assert.AreEqual(infoFile5, info5.ProjectFile, "ProjectFile is unexpected value.");
                Assert.IsNull(info5.ProjectPath, "ProjectPath is unexpected value.");
                Assert.IsNotEmpty(info5.Models, "Models is unexpected value.");
                info5.FindModels();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("ProjectInfoLoader Error.");
            }
        }

        /// <summary>
        /// Test of ProjectInfoLoader.
        /// </summary>
        [Test()]
        public void TestProjectInfoLoaderErrorCase()
        {
            try
            {
                // File Not Found.
                string infoFile1 = TestConstant.TestDirectory + "Drosophila/projectNotFound.xml";
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            try
            {
                // Not Project File.
                string infoFile1 = TestConstant.TestDirectory + "Drosophila/Model.png";
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            try
            {
                // File Is Empty.
                string infoFile1 = TestConstant.TestDirectory + "Drosophila/projectNull.xml";
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            try
            {
                // Inccorect ProjectInfo XML.
                string infoFile1 = TestConstant.TestDirectory + "Drosophila/projectIncorrect.xml";
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Test of ProjectInfoSaver.
        /// </summary>
        [Test()]
        public void TestProjectInfoSaver()
        {
            try
            {
                // Load and Save.
                string infoFile1 = TestConstant.Project_Drosophila;
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                info1.Save();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Failed to Save.");
            }

            try
            {
                // Load and Save.
                string infoFile1 = TestConstant.Project_Drosophila;
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                info1.Save(infoFile1);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Failed to Save.");
            }

            try
            {
                // Save to Another Directory.
                string infoFile1 = TestConstant.Model_Drosophila;
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                info1.Save(infoFile1);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Failed to Save.");
            }

            try
            {
                // Load and Save.
                string infoFile1 = TestConstant.Project_Drosophila;
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                info1.Save(infoFile1);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Failed to Save.");
            }

            try
            {
                // Save to Another Directory.
                string saveDir = TestConstant.TestDirectory + "Drosophila2/";
                string infoFile1 = TestConstant.Project_Drosophila;
                if (Directory.Exists(saveDir))
                    Directory.Delete(saveDir, true);
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                info1.Save(saveDir);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Failed to Save.");
            }
        }
        /// <summary>
        /// Test of ProjectInfoSaver.
        /// </summary>
        [Test()]
        public void TestProjectInfoSaverErrorCase()
        {
            try
            {
                ProjectInfo info = new ProjectInfo();
                info.Save();
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            try
            {
                ProjectInfo info = new ProjectInfo();
                info.Save();
                Assert.Fail("Incorrect null check.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Test of ProjectInfo.
        /// </summary>
        [Test()]
        public void TestProjectInfoMethods()
        {
            try
            {
                // Load and Save.
                string infoFile1 = TestConstant.Project_CoupledOscillator;
                ProjectInfo info1 = ProjectInfoLoader.Load(infoFile1);
                info1.FindModels();
                info1.Save();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                Assert.Fail("Failed to Save.");
            }

        }
    }
}
