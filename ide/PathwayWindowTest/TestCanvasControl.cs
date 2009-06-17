namespace Ecell.IDE.Plugins.PathwayWindow
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestCanvasControl
    {

        private CanvasControl _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            string modelID = null;
            _unitUnderTest = new CanvasControl(control, modelID);
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
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorCanvasControl()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            string modelID = null;
            CanvasControl testCanvasControl = new CanvasControl(control, modelID);
            Assert.IsNotNull(testCanvasControl, "Constructor of type, CanvasControl failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRefreshEdges()
        {
            //_unitUnderTest.RefreshEdges();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifySelectChanged()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject obj = null;
            _unitUnderTest.NotifySelectChanged(obj);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyAddSelect()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject obj = null;
            _unitUnderTest.NotifyAddSelect(obj);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyRemoveSelect()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject obj = null;
            _unitUnderTest.NotifyRemoveSelect(obj);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDoesSystemOverlapsRect()
        {
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.DoesSystemOverlaps(rect);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesSystemOverlaps method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDoesSystemOverlapsSystem()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwaySystem system = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.DoesSystemOverlaps(system);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesSystemOverlaps method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDoesSystemOverlapsSystemNameRect()
        {
            string systemName = null;
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.DoesSystemOverlaps(systemName, rect);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesSystemOverlaps method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDoesSystemContainsKeyRect()
        {
            string key = null;
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.DoesSystemContains(key, rect);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesSystemContains method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDoesSystemContainsKeyPoint()
        {
            string key = null;
            System.Drawing.PointF point = new System.Drawing.PointF();
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.DoesSystemContains(key, point);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesSystemContains method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCanvasPosToSystemPos()
        {
            System.Drawing.PointF pos = new System.Drawing.PointF();
            System.Drawing.Point expectedPoint = new System.Drawing.Point(0, 0);
            System.Drawing.Point resultPoint = new System.Drawing.Point(0, 0);
            resultPoint = _unitUnderTest.CanvasPosToSystemPos(pos);
            Assert.AreEqual(expectedPoint, resultPoint, "CanvasPosToSystemPos method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSystemPosToCanvasPos()
        {
            System.Drawing.Point pos = new System.Drawing.Point(0, 0);
            System.Drawing.PointF expectedPointF = new System.Drawing.PointF(0, 0);
            System.Drawing.PointF resultPointF = new System.Drawing.PointF(0, 0);
            resultPointF = _unitUnderTest.SystemPosToCanvasPos(pos);
            Assert.AreEqual(expectedPointF, resultPointF, "SystemPosToCanvasPos method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataAdd()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject obj = null;
            bool hasCoords = false;
            _unitUnderTest.DataAdd(obj, hasCoords);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddLayerNameVisible()
        {
            string name = null;
            bool visible = false;
            _unitUnderTest.AddLayer(name, visible);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRemoveLayer()
        {
            string name = null;
            _unitUnderTest.RemoveLayer(name);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestChangeLayerVisibility()
        {
            string layerName = null;
            bool isShown = false;
            _unitUnderTest.ChangeLayerVisibility(layerName, isShown);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetLayer()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject obj = null;
            _unitUnderTest.SetLayer(obj, null);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLayerMoveToFrontName()
        {
            string name = null;
            _unitUnderTest.LayerMoveToFront(name);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLayerMoveToFrontLayer()
        {
            UMD.HCIL.Piccolo.PLayer layer = null;
            _unitUnderTest.LayerMoveToFront(layer);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLayerMoveToBackName()
        {
            string name = null;
            _unitUnderTest.LayerMoveToBack(name);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLayerMoveToBackLayer()
        {
            UMD.HCIL.Piccolo.PLayer layer = null;
            _unitUnderTest.LayerMoveToBack(layer);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLayerNameList()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            //resultList = _unitUnderTest.GetLayerNameList();
            Assert.AreEqual(expectedList, resultList, "GetLayerNameList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyAddLayer()
        {
            string name = null;
            bool isAnchored = false;
            //_unitUnderTest.NotifyAddLayer(name, isAnchored);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyRemoveLayer()
        {
            string name = null;
            //_unitUnderTest.NotifyRemoveLayer(name, isAnchored);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyRenameLayer()
        {
            string oldName = null;
            string newName = null;
            _unitUnderTest.NotifyRenameLayer(oldName, newName);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyMergeLayer()
        {
            //_unitUnderTest.NotifyMergeLayer(oldName, newName);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyLayerChange()
        {
            //_unitUnderTest.NotifyLayerChange(isAnchored);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSurroundingSystem()
        {
            System.Drawing.PointF point = new System.Drawing.PointF();
            string excludedSystem = null;
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject expectedPPathwayObject = null;
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject resultPPathwayObject = null;
            resultPPathwayObject = _unitUnderTest.GetSurroundingSystem(point, excludedSystem);
            Assert.AreEqual(expectedPPathwayObject, resultPPathwayObject, "GetSurroundingSystem method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSurroundingSystemKeyPoint()
        {
            System.Drawing.PointF point = new System.Drawing.PointF();
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetSurroundingSystemKey(point);
            Assert.AreEqual(expectedString, resultString, "GetSurroundingSystemKey method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSurroundingSystemKeyPointExcludedSystem()
        {
            System.Drawing.PointF point = new System.Drawing.PointF();
            string excludedSystem = null;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetSurroundingSystemKey(point, excludedSystem);
            Assert.AreEqual(expectedString, resultString, "GetSurroundingSystemKey method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSelectedNode()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject obj = null;
            _unitUnderTest.AddSelectedNode(obj);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetPickedNode()
        {
            System.Drawing.PointF pointF = new System.Drawing.PointF();
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayEntity expectedPPathwayNode = null;
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayEntity resultPPathwayNode = null;
            resultPPathwayNode = _unitUnderTest.GetPickedNode(pointF);
            Assert.AreEqual(expectedPPathwayNode, resultPPathwayNode, "GetPickedNode method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetObject()
        {
            string key = null;
            string type = null;
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject expectedPPathwayObject = null;
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject resultPPathwayObject = null;
            resultPPathwayObject = _unitUnderTest.GetObject(key, type);
            Assert.AreEqual(expectedPPathwayObject, resultPPathwayObject, "GetObject method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSurroundedObject()
        {
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> expectedList = null;
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> resultList = null;
            //resultList = _unitUnderTest.GetSurroundedObject(rect);
            Assert.AreEqual(expectedList, resultList, "GetSurroundedObject method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetAllObjects()
        {
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> expectedList = null;
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> resultList = null;
            resultList = _unitUnderTest.GetAllEntities();
            Assert.AreEqual(expectedList, resultList, "GetAllObjects method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetAllObjectUnder()
        {
            string systemKey = null;
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> expectedList = null;
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> resultList = null;
            resultList = _unitUnderTest.GetAllObjectUnder(systemKey);
            Assert.AreEqual(expectedList, resultList, "GetAllObjectUnder method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSystemList()
        {
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> expectedList = null;
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> resultList = null;
            resultList = _unitUnderTest.GetSystemList();
            Assert.AreEqual(expectedList, resultList, "GetSystemList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetNodeList()
        {
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> expectedList = null;
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> resultList = null;
            resultList = _unitUnderTest.GetNodeList();
            Assert.AreEqual(expectedList, resultList, "GetNodeList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetTextList()
        {
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> expectedList = null;
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> resultList = null;
            resultList = _unitUnderTest.GetTextList();
            Assert.AreEqual(expectedList, resultList, "GetTextList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStepperList()
        {
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> expectedList = null;
            System.Collections.Generic.List<Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject> resultList = null;
            resultList = _unitUnderTest.GetStepperList();
            Assert.AreEqual(expectedList, resultList, "GetStepperList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestPanCanvas()
        {
            Ecell.IDE.Plugins.PathwayWindow.Direction direction = Direction.Horizontal;
            int delta = 0;
            _unitUnderTest.PanCanvas(direction, delta);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestToImage()
        {
            System.Drawing.Bitmap expectedBitmap = null;
            System.Drawing.Bitmap resultBitmap = null;
            resultBitmap = _unitUnderTest.ToImage();
            Assert.AreEqual(expectedBitmap, resultBitmap, "ToImage method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChanged()
        {
            string oldKey = null;
            string newKey = null;
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject obj = null;
            _unitUnderTest.DataChanged(oldKey, newKey, obj);
            Assert.Fail("Create or modify test(s).");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDelete()
        {
            string key = null;
            string type = null;
            _unitUnderTest.DataDelete(key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSelectKeyType()
        {
            string key = null;
            string type = null;
            _unitUnderTest.AddSelect(key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSelectObj()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject obj = null;
            _unitUnderTest.AddSelect(obj);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRemoveSelect()
        {
            string key = null;
            string type = null;
            _unitUnderTest.RemoveSelect(key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSelectChanged()
        {
            string key = null;
            string type = null;
            _unitUnderTest.SelectChanged(key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdateOverviewAfterTime()
        {
            int miliSec = 0;
            _unitUnderTest.UpdateOverviewAfterTime(miliSec);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRefreshOverView()
        {
            //_unitUnderTest.RefreshOverView();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestZoom()
        {
            float rate = 0;
            _unitUnderTest.Zoom(rate);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDispose()
        {
            _unitUnderTest.Dispose();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestResetSelectedNodes()
        {
            _unitUnderTest.ResetSelectedNodes();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestResetSelectedLine()
        {
            _unitUnderTest.ResetSelectedLine();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyResetSelect()
        {
            _unitUnderTest.NotifyResetSelect();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestResetSelect()
        {
            _unitUnderTest.ResetSelect();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCheckNodePosition()
        {
            string sysKey = null;
            System.Drawing.PointF point = new System.Drawing.PointF();
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.CheckNodePosition(sysKey, point);
            Assert.AreEqual(expectedBoolean, resultBoolean, "CheckNodePosition method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetVacantPointSysKey()
        {
            string sysKey = null;
            System.Drawing.PointF expectedPointF = new System.Drawing.PointF();
            System.Drawing.PointF resultPointF = new System.Drawing.PointF();
            resultPointF = _unitUnderTest.GetVacantPoint(sysKey);
            Assert.AreEqual(expectedPointF, resultPointF, "GetVacantPoint method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetVacantPointSysKeyPoint()
        {
            string sysKey = null;
            System.Drawing.PointF point = new System.Drawing.PointF();
            System.Drawing.PointF expectedPointF = new System.Drawing.PointF();
            System.Drawing.PointF resultPointF = new System.Drawing.PointF();
            resultPointF = _unitUnderTest.GetVacantPoint(sysKey, point);
            Assert.AreEqual(expectedPointF, resultPointF, "GetVacantPoint method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetVacantPointSysKeyRectF()
        {
            string sysKey = null;
            System.Drawing.RectangleF rectF = new System.Drawing.RectangleF();
            System.Drawing.PointF expectedPointF = new System.Drawing.PointF();
            System.Drawing.PointF resultPointF = new System.Drawing.PointF();
            resultPointF = _unitUnderTest.GetVacantPoint(sysKey, rectF);
            Assert.AreEqual(expectedPointF, resultPointF, "GetVacantPoint method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestMoveSelectedObjects()
        {
            //_unitUnderTest.MoveSelectedObjects(offset);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyMoveObjects()
        {
            //_unitUnderTest.NotifyMoveObjects(isAnchored);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsInsideRoot()
        {
            System.Drawing.RectangleF rectF = new System.Drawing.RectangleF();
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsInsideRoot(rectF);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsInsideRoot method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
