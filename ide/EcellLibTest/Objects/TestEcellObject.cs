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
using System.Drawing;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell.Objects
{
    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestEcellObject
    {
        private ApplicationEnvironment _env;
        private EcellObject _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = EcellObject.CreateObject("Model","","Project","",null);
        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _env = null;
            _unitUnderTest = null;
        }

        /// <summary>
        /// TestCreateObject
        /// </summary>
        [Test()]
        public void TestCreateObject()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string classname = null;
            List<EcellData> data = null;
            EcellObject resultEcellObject = null;

            // invalid modelID.
            try
            {
                resultEcellObject = EcellObject.CreateObject(modelID, key, type, classname, data);
                Assert.Fail("Failed to throw Error.");
            }
            catch (EcellException)
            {
            }
            // invalid modelID.
            try
            {
                modelID = "";
                key = "";
                type = "";
                classname = "";
                resultEcellObject = EcellObject.CreateObject(modelID, key, type, classname, data);
                Assert.Fail("Failed to throw Error.");
            }
            catch (EcellException)
            {
            }
            // invalid type.
            try
            {
                modelID = "Model";
                key = "";
                type = "";
                classname = "";
                resultEcellObject = EcellObject.CreateObject(modelID, key, type, classname, data);
                Assert.Fail("Failed to throw Error.");
            }
            catch (EcellException)
            {
            }

            // invalid type.
            try
            {
                modelID = "Model";
                key = "";
                type = "Test";
                classname = "";
                resultEcellObject = EcellObject.CreateObject(modelID, key, type, classname, data);
                Assert.Fail("Failed to throw Error.");
            }
            catch (EcellException)
            {
            }

            List<EcellData> datas;
            EcellObject eo;

            // Create EcellProject
            modelID = "Model";
            key = "";
            type = "Project";
            classname = "";
            datas = new List<EcellData>();
            eo = EcellObject.CreateObject(modelID, key, type, classname, datas);
            Assert.IsNotNull(eo, "Failed to Create EcellProject.");
            Assert.IsTrue(eo is EcellProject, "Failed to Create EcellProject.");
            Assert.AreEqual("Project", eo.Type, "Type is not expected value.");
            Assert.AreEqual("Model", eo.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("Project", eo.Classname, "Classname is not expected value.");
            Assert.AreEqual("", eo.Key, "Key is not expected value.");
            Assert.AreEqual("Project:", eo.FullID, "FullID is not expected value.");
            Assert.AreEqual("", eo.ParentSystemID, "ParentSystemID is not expected value.");
            Assert.AreEqual("", eo.LocalID, "LocalID is not expected value.");
            Assert.AreEqual(true, eo.IsUsable, "IsUsable is not expected value.");
            Assert.AreEqual(false, eo.IsPosSet, "IsPosSet is not expected value.");
            Assert.AreEqual(false, eo.isFixed, "isFixed is not expected value.");
            Assert.AreEqual(false, eo.Logged, "Logged is not expected value.");
            eo.ModelID = "";
            Assert.AreEqual(false, eo.IsUsable, "IsUsable is not expected value.");
            Assert.IsNotNull(eo.GetHashCode(), "GetHashCode method returns unexpected value.");

            // Create EcellModel
            modelID = "Model";
            key = "";
            type = "Model";
            classname = "";
            datas = new List<EcellData>();
            eo = EcellObject.CreateObject(modelID, key, type, classname, datas);
            Assert.IsNotNull(eo, "Failed to Create EcellProject.");
            Assert.IsTrue(eo is EcellModel, "Failed to Create EcellModel.");
            Assert.AreEqual("Model", eo.Type, "Type is not expected value.");
            Assert.AreEqual("Model", eo.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("Model", eo.Classname, "Classname is not expected value.");
            Assert.AreEqual("", eo.Key, "Key is not expected value.");
            Assert.AreEqual("Model:", eo.FullID, "FullID is not expected value.");
            Assert.AreEqual("", eo.ParentSystemID, "ParentSystemID is not expected value.");
            Assert.AreEqual("", eo.LocalID, "LocalID is not expected value.");
            Assert.AreEqual(true, eo.IsUsable, "IsUsable is not expected value.");
            Assert.AreEqual(false, eo.IsPosSet, "IsPosSet is not expected value.");
            Assert.AreEqual(false, eo.isFixed, "isFixed is not expected value.");
            Assert.AreEqual(false, eo.Logged, "Logged is not expected value.");
            Assert.IsNotNull(eo.GetHashCode(), "GetHashCode method returns unexpected value.");

            // Create EcellStepper
            modelID = "Model";
            key = "";
            type = "Stepper";
            classname = "DAEStepper";
            datas = new List<EcellData>();
            eo = EcellObject.CreateObject(modelID, key, type, classname, datas);
            Assert.IsNotNull(eo, "Failed to Create EcellProject.");
            Assert.IsTrue(eo is EcellStepper, "Failed to Create EcellStepper.");
            Assert.AreEqual("Stepper", eo.Type, "Type is not expected value.");
            Assert.AreEqual("Model", eo.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("DAEStepper", eo.Classname, "Classname is not expected value.");
            Assert.AreEqual("", eo.Key, "Key is not expected value.");
            Assert.AreEqual("Stepper:", eo.FullID, "FullID is not expected value.");
            Assert.AreEqual("", eo.ParentSystemID, "ParentSystemID is not expected value.");
            Assert.AreEqual("", eo.LocalID, "LocalID is not expected value.");
            Assert.AreEqual(false, eo.IsUsable, "IsUsable is not expected value.");
            Assert.AreEqual(false, eo.IsPosSet, "IsPosSet is not expected value.");
            Assert.AreEqual(false, eo.isFixed, "isFixed is not expected value.");
            Assert.AreEqual(false, eo.Logged, "Logged is not expected value.");
            eo.Classname = "FixedODEStepper";
            Assert.AreEqual("FixedODEStepper", eo.Classname, "Classname is not expected value.");
            Assert.IsNotNull(eo.GetHashCode(), "GetHashCode method returns unexpected value.");

            // Create EcellSystem
            modelID = "Model";
            key = "/";
            type = "System";
            classname = "System";
            datas = new List<EcellData>();
            eo = EcellObject.CreateObject(modelID, key, type, classname, datas);
            Assert.IsNotNull(eo, "Failed to Create EcellProject.");
            Assert.IsTrue(eo is EcellSystem, "Failed to Create EcellSystem.");
            Assert.AreEqual("System", eo.Type, "Type is not expected value.");
            Assert.AreEqual("Model", eo.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("System", eo.Classname, "Classname is not expected value.");
            Assert.AreEqual("/", eo.Key, "Key is not expected value.");
            Assert.AreEqual("System:/", eo.FullID, "FullID is not expected value.");
            Assert.AreEqual("", eo.ParentSystemID, "ParentSystemID is not expected value.");
            Assert.AreEqual("/", eo.LocalID, "LocalID is not expected value.");
            Assert.IsNotNull(eo.GetHashCode(), "GetHashCode method returns unexpected value.");

            eo.Key = "/Hoge/S1";
            Assert.AreEqual("/Hoge/S1", eo.Key, "Key is not expected value.");
            Assert.AreEqual("System:/Hoge/:S1", eo.FullID, "FullID is not expected value.");
            Assert.AreEqual("/Hoge", eo.ParentSystemID, "ParentSystemID is not expected value.");
            Assert.AreEqual("S1", eo.LocalID, "LocalID is not expected value.");
            eo.Classname = "";
            eo.isFixed = true;
            Assert.AreEqual(false, eo.IsUsable, "IsUsable is not expected value.");
            Assert.AreEqual(false, eo.IsPosSet, "IsPosSet is not expected value.");
            Assert.AreEqual(true, eo.isFixed, "isFixed is not expected value.");
            Assert.AreEqual(false, eo.Logged, "Logged is not expected value.");
            Assert.IsNotNull(eo.GetHashCode(), "GetHashCode method returns unexpected value.");

            // Create EcellProcess
            modelID = "Model";
            key = "/:P0";
            type = "Process";
            classname = "MassActionProcess";
            datas = new List<EcellData>();
            eo = EcellObject.CreateObject(modelID, key, type, classname, datas);
            Assert.IsNotNull(eo, "Failed to Create EcellProject.");
            Assert.IsTrue(eo is EcellProcess, "Failed to Create EcellProcess.");
            Assert.AreEqual("Process", eo.Type, "Type is not expected value.");
            Assert.AreEqual("Model", eo.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("MassActionProcess", eo.Classname, "Classname is not expected value.");
            Assert.AreEqual("/:P0", eo.Key, "Key is not expected value.");
            Assert.AreEqual("Process:/:P0", eo.FullID, "FullID is not expected value.");
            Assert.AreEqual("/", eo.ParentSystemID, "ParentSystemID is not expected value.");
            Assert.AreEqual("P0", eo.LocalID, "LocalID is not expected value.");
            Assert.IsNotNull(eo.GetHashCode(), "GetHashCode method returns unexpected value.");

            eo.X = 10;
            Assert.AreEqual(true, eo.IsUsable, "IsUsable is not expected value.");
            Assert.AreEqual(true, eo.IsPosSet, "IsPosSet is not expected value.");
            Assert.AreEqual(false, eo.isFixed, "isFixed is not expected value.");
            Assert.AreEqual(false, eo.Logged, "Logged is not expected value.");

            Assert.AreEqual(10, eo.X, "X is not expected value.");
            Assert.AreEqual(0, eo.Y, "Y is not expected value.");
            Assert.AreEqual(new PointF(10, 0), eo.PointF, "PointF is not expected value.");
            eo.PointF = new PointF(100, 200);
            Assert.AreEqual(100, eo.X, "X is not expected value.");
            Assert.AreEqual(200, eo.Y, "Y is not expected value.");
            Assert.AreEqual(new PointF(100, 200), eo.PointF, "PointF is not expected value.");
            eo.CenterPointF = new PointF(50, 50);
            Assert.AreEqual(50, eo.X, "X is not expected value.");
            Assert.AreEqual(50, eo.Y, "Y is not expected value.");
            Assert.AreEqual(new PointF(50, 50), eo.PointF, "PointF is not expected value.");
            Assert.AreEqual(new PointF(50, 50), eo.CenterPointF, "PointF is not expected value.");

            // Create EcellVariable
            modelID = "Model";
            key = "/Hoge/:V0";
            type = "Variable";
            classname = "Variable";
            datas = new List<EcellData>();
            eo = EcellObject.CreateObject(modelID, key, type, classname, datas);
            Assert.IsNotNull(eo, "Failed to Create EcellProject.");
            Assert.IsTrue(eo is EcellVariable, "Failed to Create EcellVariable.");
            Assert.AreEqual("Variable", eo.Type, "Type is not expected value.");
            Assert.AreEqual("Model", eo.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("Variable", eo.Classname, "Classname is not expected value.");
            Assert.AreEqual("/Hoge/:V0", eo.Key, "Key is not expected value.");
            Assert.AreEqual("Variable:/Hoge/:V0", eo.FullID, "FullID is not expected value.");
            Assert.AreEqual("/Hoge/", eo.ParentSystemID, "ParentSystemID is not expected value.");
            Assert.AreEqual("V0", eo.LocalID, "LocalID is not expected value.");
            Assert.IsNotNull(eo.GetHashCode(), "GetHashCode method returns unexpected value.");
            eo.Layer = "Layer1";
            Assert.AreEqual("Layer1", eo.Layer, "Layer is not expected value.");

            // Create EcellText
            modelID = "Model";
            key = "/:Text0";
            type = "Text";
            classname = "Text";
            datas = new List<EcellData>();
            eo = EcellObject.CreateObject(modelID, key, type, classname, datas);
            Assert.IsNotNull(eo, "Failed to Create EcellProject.");
            Assert.IsTrue(eo is EcellText, "Failed to Create EcellText.");
            Assert.AreEqual("Text", eo.Type, "Type is not expected value.");
            Assert.AreEqual("Model", eo.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("Text", eo.Classname, "Classname is not expected value.");
            Assert.AreEqual("/:Text0", eo.Key, "Key is not expected value.");
            Assert.AreEqual("Text:/:Text0", eo.FullID, "FullID is not expected value.");
            Assert.AreEqual("/", eo.ParentSystemID, "ParentSystemID is not expected value.");
            Assert.AreEqual("Text0", eo.LocalID, "LocalID is not expected value.");
            Assert.IsNotNull(eo.GetHashCode(), "GetHashCode method returns unexpected value.");

        }

        /// <summary>
        /// TestEcellModel
        /// </summary>
        [Test()]
        public void TestEcellModel()
        {
            // Create EcellModel
            EcellModel eo = (EcellModel)EcellObject.CreateObject("Model", "", "Model", "", new List<EcellData>());
            Assert.IsNotNull(eo, "Failed to Create EcellProject.");
            Assert.IsTrue(eo is EcellModel, "Failed to Create EcellModel.");

            Assert.IsNotNull(eo.Layers, "Layer is not expected value.");
            Assert.IsEmpty(eo.Layers, "Layer is not expected value.");

            Assert.IsNotNull(eo.Children, "Children is not expected value.");
            Assert.IsEmpty(eo.Children, "Children is not expected value.");

            Assert.IsNotNull(eo.Value, "Value is not expected value.");
            Assert.IsNotEmpty(eo.Value, "Value is not expected value.");

            Assert.IsNotNull(eo.StepperDic, "StepperDic is not expected value.");
            Assert.IsEmpty(eo.StepperDic, "StepperDic is not expected value.");

            Assert.IsNotNull(eo.Systems, "Systems is not expected value.");
            Assert.IsEmpty(eo.Systems, "Systems is not expected value.");

            Assert.IsNull(eo.ModelFile, "ModelFile is not expected value.");
            eo.ModelFile = "c:/temp/rbc.eml";
            Assert.AreEqual("c:/temp/rbc.eml", eo.ModelFile, "ModelFile is not expected value.");

            eo.ModelID = "Model1";
            Assert.AreEqual("Model1", eo.ModelID, "ModelID is not expected value.");
            EcellObject temp = eo.GetSystem("/");
            Assert.IsNull(temp);

            eo.Children.Add(new EcellSystem("Model1", "/", "", "", new List<EcellData>()));
            eo.ModelID = "Model2";
            Assert.AreEqual("Model2", eo.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("Model2", eo.Children[0].ModelID, "ModelID is not expected value.");

            Assert.IsEmpty(eo.Layers, "ModelID is not expected value.");

            List<EcellLayer> layers = new List<EcellLayer>();
            layers.Add(new EcellLayer("Layer0", true));
            eo.Layers = layers;

            Assert.IsNotEmpty(eo.Layers, "ModelID is not expected value.");
            Assert.AreEqual("Layer0", eo.Layers[0].Name, "Name is not expected value.");
            Assert.AreEqual(true, eo.Layers[0].Visible, "Visible is not expected value.");

            // Add Entity.
            temp = eo.GetSystem("/");
            Assert.IsNotNull(temp);

            EcellVariable var = new EcellVariable("Model2", "/:S", EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
            eo.AddEntity(var);
        }

        /// <summary>
        /// TestEcellSystem
        /// </summary>
        [Test()]
        public void TestEcellSystem()
        {
            // Create EcellModel
            EcellSystem eo = (EcellSystem)EcellObject.CreateObject("Model", "/", "System", "System", new List<EcellData>());
            Assert.IsNotNull(eo, "Failed to Create EcellSystem.");
            Assert.IsTrue(eo is EcellSystem, "Failed to Create EcellSystem.");

            Assert.IsNull(eo.StepperID, "StepperID is not expected value.");

            eo.StepperID = "TestStepper";
            Assert.AreEqual("TestStepper", eo.StepperID, "StepperID is not expected value.");

            Assert.AreEqual(0.1, eo.SizeInVolume, "SizeInVolume is not expected value.");

            eo.SizeInVolume = 0.0001;
            Assert.AreEqual(0.0001, eo.SizeInVolume, "SizeInVolume is not expected value.");

            eo.SizeInVolume = 0.0000001;
            eo.Children.Clear();
            Assert.AreEqual(0.0000001, eo.SizeInVolume, "SizeInVolume is not expected value.");
        }

        /// <summary>
        /// TestEcellText
        /// </summary>
        [Test()]
        public void TestEcellText()
        {
            EcellText text = new EcellText("Model", "/:Text1", "Text", "", new List<EcellData>());

            Assert.AreEqual(StringAlignment.Near, text.Alignment, "Alignment is not expected value.");
            text.Alignment = StringAlignment.Center;
            Assert.AreEqual(StringAlignment.Center, text.Alignment, "Alignment is not expected value.");

            Assert.AreEqual("Text1", text.Comment, "Comment is not expected value.");
            text.Comment = "Test";
            Assert.AreEqual("Test", text.Comment, "Comment is not expected value.");
            text.RemoveEcellValue("Comment");
            Assert.AreEqual(null, text.Comment, "Comment is not expected value.");

            text.Comment = "Sample";
            text.Alignment = StringAlignment.Center;
            EcellText text2 = (EcellText)text.Clone();
            Assert.AreEqual(text.Comment, text2.Comment, "Comment is not expected value.");
            Assert.AreEqual(text.Alignment, text2.Alignment, "Comment is not expected value.");

            text.SetEcellValue(EcellText.ALIGN, new EcellValue(4));
            Assert.AreEqual(StringAlignment.Near, text.Alignment, "Alignment is not expected value.");
        }

        /// <summary>
        /// TestEcellProcess
        /// </summary>
        [Test()]
        public void TestEcellProcess()
        {
            EcellProcess process = new EcellProcess("Model", "/:P0", "Process", "ConstantFluxProcess", new List<EcellData>());
            Assert.AreEqual(0, process.Activity, "Activity is not expected value.");
            Assert.AreEqual(null, process.Expression, "Expression is not expected value.");
            Assert.AreEqual(0, process.IsContinuous, "IsContinuous is not expected value.");
            Assert.AreEqual(0, process.Priority, "Priority is not expected value.");
            Assert.AreEqual(null, process.StepperID, "StepperID is not expected value.");

            process.Activity = 1;
            process.Expression = "sin ( 1 )";
            process.IsContinuous = 1;
            process.Priority = 1;
            process.StepperID = "DE";

            Assert.AreEqual(1, process.Activity, "Activity is not expected value.");
            Assert.AreEqual("sin ( 1 )", process.Expression, "Expression is not expected value.");
            Assert.AreEqual(1, process.IsContinuous, "IsContinuous is not expected value.");
            Assert.AreEqual(1, process.Priority, "Priority is not expected value.");
            Assert.AreEqual("DE", process.StepperID, "StepperID is not expected value.");

            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            List<EcellReference> expected = EcellReference.ConvertFromString(str);
            process.ReferenceList = expected;
            Assert.AreEqual(expected, process.ReferenceList, "ReferenceList is not expected value.");
        }

        /// <summary>
        /// TestSetPosition
        /// </summary>
        [Test()]
        public void TestSetPosition()
        {
            _unitUnderTest = EcellObject.CreateObject("Model", "", "Model", "", new List<EcellData>());
            EcellObject obj = EcellObject.CreateObject("Model", "", "Model", "", new List<EcellData>());
            obj.X = 100;
            obj.Y = 150;
            obj.Width = 200;
            obj.Height = 250;
            obj.OffsetX = 300;
            obj.OffsetY = 350;

            _unitUnderTest.SetPosition(obj);
            Assert.AreEqual(obj.X, _unitUnderTest.X, "X is unexpected value.");
            Assert.AreEqual(obj.Y, _unitUnderTest.Y, "Y is unexpected value.");
            Assert.AreEqual(obj.Width, _unitUnderTest.Width, "Width is unexpected value.");
            Assert.AreEqual(obj.Height, _unitUnderTest.Height, "Height is unexpected value.");
            Assert.AreEqual(obj.OffsetX, _unitUnderTest.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(obj.OffsetY, _unitUnderTest.OffsetY, "OffsetY is unexpected value.");

            Assert.AreEqual(new RectangleF(100, 150, 200, 250), _unitUnderTest.Rect, "Rect is unexpected value.");
            Assert.AreEqual(new PointF(200, 275), _unitUnderTest.CenterPointF, "CenterPointF is unexpected value.");

        }

        /// <summary>
        /// TestParentSystemID
        /// </summary>
        [Test()]
        public void TestParentSystemID()
        {
            EcellSystem eo = new EcellSystem("Model", "/", "System", "System", new List<EcellData>());
            eo.SizeInVolume = 0.01;

            eo.ParentSystemID = "/";
            Assert.AreEqual("/", eo.Key, "Key is unexpected value.");
            Assert.AreEqual("", eo.ParentSystemID, "ParentSystemID is unexpected value.");
            eo.ParentSystemID = "/S0";
            Assert.AreEqual("/S0", eo.Key, "Key is unexpected value.");
            Assert.AreEqual("/", eo.ParentSystemID, "ParentSystemID is unexpected value.");
            eo.ParentSystemID = "/S1";
            Assert.AreEqual("/S1/S0", eo.Key, "Key is unexpected value.");
            Assert.AreEqual("/S1", eo.ParentSystemID, "ParentSystemID is unexpected value.");
            eo.ParentSystemID = "/";
            Assert.AreEqual("/S0", eo.Key, "Key is unexpected value.");
            Assert.AreEqual("/", eo.ParentSystemID, "ParentSystemID is unexpected value.");

            EcellVariable var = new EcellVariable("Model", "/:V1", "Variable", "Variable", new List<EcellData>());
            Assert.AreEqual("/:V1", var.Key, "Key is unexpected value.");
            Assert.AreEqual("/", var.ParentSystemID, "ParentSystemID is unexpected value.");
            var.ParentSystemID = "/S0";
            Assert.AreEqual("/S0:V1", var.Key, "Key is unexpected value.");
            Assert.AreEqual("/S0", var.ParentSystemID, "ParentSystemID is unexpected value.");
            var.ParentSystemID = "/S0/S1";
            Assert.AreEqual("/S0/S1:V1", var.Key, "Key is unexpected value.");
            Assert.AreEqual("/S0/S1", var.ParentSystemID, "ParentSystemID is unexpected value.");

        }

        /// <summary>
        /// TestEquals
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            EcellObject obj;

            obj = new EcellProcess("Model", "/:P0", "Process", "ConstantFluxProcess", new List<EcellData>());
            expectedBoolean = false;
            resultBoolean = false;

            resultBoolean = obj.Equals(new object());
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellProcess("Model", "/:P1", "Process", "ConstantFluxProcess", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellProcess("Model1", "/:P0", "Process", "ConstantFluxProcess", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellVariable("Model", "/:P0", "Variable", "Variable", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellSystem("Model", "/", "System", "System", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;

            resultBoolean = obj.Equals(new EcellProcess("Model", "/:P0", "Process", "ConstantFluxProcess", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");


            obj = new EcellVariable("Model", "/:P0", "Variable", "Variable", new List<EcellData>());
            expectedBoolean = false;
            resultBoolean = false;

            resultBoolean = obj.Equals(new object());
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellVariable("Model", "/:P1", "Variable", "Variable", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellVariable("Model1", "/:P0", "Variable", "Variable", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellProcess("Model", "/:P0", "Process", "ConstantFluxProcess", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellSystem("Model", "/:P0", "System", "System", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;

            resultBoolean = obj.Equals(new EcellVariable("Model", "/:P0", "Variable", "Variable", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            obj = new EcellSystem("Model", "/P0", "System", "System", new List<EcellData>());
            expectedBoolean = false;
            resultBoolean = false;

            resultBoolean = obj.Equals(new object());
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellSystem("Model", "/P1", "System", "System", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellSystem("Model1", "/P0", "System", "System", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellProcess("Model", "/P0", "Process", "ConstantFluxProcess", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = obj.Equals(new EcellVariable("Model", "/P0", "Variable", "Variable", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;

            resultBoolean = obj.Equals(new EcellSystem("Model", "/P0", "System", "System", new List<EcellData>()));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

        }

        /// <summary>
        /// TestRemoveValue
        /// </summary>
        [Test()]
        public void TestRemoveValue()
        {
            EcellProcess process = new EcellProcess("Model", "/:P0", "Process", "ConstantFluxProcess", new List<EcellData>());
            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            List<EcellReference> expected = EcellReference.ConvertFromString(str);
            process.ReferenceList = expected;
            Assert.AreEqual(expected, process.ReferenceList, "ReferenceList is not expected value.");

            process.RemoveEcellValue(EcellProcess.VARIABLEREFERENCELIST);
            Assert.IsNull(process.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST), "GetEcellValue method returned unexpected result.");

            process.RemoveEcellValue(EcellProcess.VARIABLEREFERENCELIST);
            Assert.IsNull(process.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST), "GetEcellValue method returned unexpected result.");

        }

        /// <summary>
        /// TestSetEcellDatas
        /// </summary>
        [Test()]
        public void TestSetEcellDatas()
        {
            EcellSystem sys = new EcellSystem("Model", "/", "System", "System", new List<EcellData>());
            sys.SizeInVolume = 0.01;
            Assert.IsNotEmpty(sys.Value, "Value is not expected value.");
            sys.SetEcellDatas(new List<EcellData>());
            Assert.IsEmpty(sys.Value, "Value is not expected value.");
            
        }

        /// <summary>
        /// TestGetEcellData
        /// </summary>
        [Test()]
        public void TestGetEcellData()
        {
            EcellSystem sys = new EcellSystem("Model", "/", "System", "System", null);
            EcellData expectedEcellData = null;
            EcellData resultEcellData = sys.GetEcellData("Size");
            Assert.AreEqual(expectedEcellData, resultEcellData, "GetEcellData method returned unexpected result.");

            sys.SetEcellDatas(new List<EcellData>());
            resultEcellData = sys.GetEcellData("Size");
            Assert.AreEqual(expectedEcellData, resultEcellData, "GetEcellData method returned unexpected result.");

            expectedEcellData = new EcellData("Size", new EcellValue(0.1), "System::/:Size");
            sys.Value.Add(expectedEcellData);
            resultEcellData = sys.GetEcellData("Size");
            Assert.AreEqual(expectedEcellData, resultEcellData, "GetEcellData method returned unexpected result.");

            sys.SizeInVolume = 0.2;
            expectedEcellData = new EcellData("Size", new EcellValue(0.2), "System::/:Size");
            resultEcellData = sys.GetEcellData("Size");
            Assert.AreEqual(expectedEcellData, resultEcellData, "GetEcellData method returned unexpected result.");

            resultEcellData.Logable = true;
            resultEcellData.Logged = true;
            Assert.AreEqual(true, sys.Logged, "Logged is not expected value.");
        }

        /// <summary>
        /// TestGetEcellValue
        /// </summary>
        [Test()]
        public void TestGetEcellValue()
        {
            EcellSystem sys = new EcellSystem("Model", "/", "System", "System", null);
            EcellValue expectedEcellValue = null;
            EcellValue resultEcellValue = null;
            resultEcellValue = sys.GetEcellValue("Size");
            Assert.AreSame(expectedEcellValue, resultEcellValue, "GetEcellValue method returned unexpected result.");

            sys.SetEcellDatas(new List<EcellData>());
            resultEcellValue = sys.GetEcellValue("Size");
            Assert.AreSame(expectedEcellValue, resultEcellValue, "GetEcellValue method returned unexpected result.");

            sys.SizeInVolume = 0.1;
            expectedEcellValue = new EcellValue(0.1);
            resultEcellValue = sys.GetEcellValue("Size");
            Assert.IsTrue(expectedEcellValue.Equals(resultEcellValue), "GetEcellValue method returned unexpected result.");

        }

        /// <summary>
        /// TestIsEcellValueExists
        /// </summary>
        [Test()]
        public void TestIsEcellValueExists()
        {
            EcellSystem sys = new EcellSystem("Model", "/", "System", "System", null);
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = sys.IsEcellValueExists("Size");
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsEcellValueExists method returned unexpected result.");

            sys.SetEcellDatas(new List<EcellData>());
            resultBoolean = sys.IsEcellValueExists("Size");
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsEcellValueExists method returned unexpected result.");

            sys.SizeInVolume = 0.02;
            expectedBoolean = true;
            resultBoolean = sys.IsEcellValueExists("Size");
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsEcellValueExists method returned unexpected result.");

        }

        /// <summary>
        /// TestSetEcellValue
        /// </summary>
        [Test()]
        public void TestSetEcellValue()
        {
            EcellSystem sys = new EcellSystem("Model", "/", "System", "System", new List<EcellData>());
            sys.SizeInVolume = 0.02;
            sys.SetEcellValue("Size", new EcellValue(0.02));
            Assert.AreEqual(0.02, sys.SizeInVolume, "SizeInVolume is unexpected result.");
        }

        /// <summary>
        /// TestToString
        /// </summary>
        [Test()]
        public void TestToString()
        {
            EcellSystem sys = new EcellSystem("Model", "/", "System", "System", new List<EcellData>());
            sys.SizeInVolume = 0.01;
            Assert.IsNotNull(sys.ToString(), "ToString method returned unexpected result.");

            sys.Children.Add(null);
            Assert.IsNotNull(sys.ToString(), "ToString method returned unexpected result.");
        }

        /// <summary>
        /// TestClone
        /// </summary>
        [Test()]
        public void TestClone()
        {
            EcellSystem sys = new EcellSystem("Model", "/", "System", "System", new List<EcellData>());
            sys.SizeInVolume = 0.01;

            EcellSystem newSys = (EcellSystem)sys.Clone();
            Assert.AreEqual(sys, newSys, "Clone method returned unexpected result.");

            newSys = ((ICloneable)sys).Clone() as EcellSystem;
            Assert.AreEqual(sys, newSys, "Clone method returned unexpected result.");
        }
    }
}
