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
using Ecell.Objects;
using System.Drawing;

namespace Ecell.Objects
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestEcellLayout
    {
        private EcellLayout _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = EcellLayout.Empty;
        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = EcellLayout.Empty;
        }

        /// <summary>
        /// TestConstructors
        /// </summary>
        [Test()]
        public void TestConstructors()
        {
            EcellLayout layout = new EcellLayout();
            Assert.AreEqual(PointF.Empty, layout.Location, "Location is unexpected value.");
            Assert.AreEqual(0, layout.X, "X is unexpected value.");
            Assert.AreEqual(0, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(RectangleF.Empty, layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(0, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(0, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(0, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(0, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(0, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(0, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(SizeF.Empty, layout.Size, "Size is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Center, "Center is unexpected value.");
            Assert.AreEqual(0, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(0, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(true, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            PointF point = new PointF(10, 20);
            layout = new EcellLayout(point);
            Assert.AreEqual(new PointF(10, 20), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, layout.X, "X is unexpected value.");
            Assert.AreEqual(20, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 20, 0, 0), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(0, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(0, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(20, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(20, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(10, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(10, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(SizeF.Empty, layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(10, 20), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(10, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(20, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(10, 20, 100, 200);
            layout.Layer = "Layer";
            Assert.AreEqual(new PointF(10, 20), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, layout.X, "X is unexpected value.");
            Assert.AreEqual(20, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 20, 100, 200), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(100, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(200, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(20, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(220, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(10, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(110, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(100, 200), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(60, 120), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(60, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual("Layer", layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            RectangleF rect = new RectangleF(10, 20, 100, 200);
            layout = new EcellLayout(rect);
            layout.Layer = "Layer2";
            Assert.AreEqual(new PointF(10, 20), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, layout.X, "X is unexpected value.");
            Assert.AreEqual(20, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 20, 100, 200), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(100, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(200, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(20, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(220, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(10, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(110, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(100, 200), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(60, 120), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(60, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual("Layer2", layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");
        }

        /// <summary>
        /// TestSetCoodinates
        /// </summary>
        [Test()]
        public void TestSetCoodinates()
        {
            EcellLayout layout = new EcellLayout();
            layout.X = 10;
            Assert.AreEqual(new PointF(10, 0), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, layout.X, "X is unexpected value.");
            Assert.AreEqual(0, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 0, 0, 0), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(0, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(0, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(0, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(0, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(10, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(10, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(SizeF.Empty, layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(10, 0), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(10, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(0, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(10, 20, 100, 200);
            layout.Y = 50;
            Assert.AreEqual(new PointF(10, 50), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, layout.X, "X is unexpected value.");
            Assert.AreEqual(50, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 50, 100, 200), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(100, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(200, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(50, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(250, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(10, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(110, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(100, 200), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(60, 150), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(60, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(150, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(10, 50, 100, 200);
            layout.Width = 200;
            Assert.AreEqual(new PointF(10, 50), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, layout.X, "X is unexpected value.");
            Assert.AreEqual(50, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 50, 200, 200), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(200, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(200, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(50, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(250, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(10, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(210, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(200, 200), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(110, 150), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(110, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(150, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(10, 50, 200, 200);
            layout.Height = 100;
            Assert.AreEqual(new PointF(10, 50), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, layout.X, "X is unexpected value.");
            Assert.AreEqual(50, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 50, 200, 100), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(200, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(100, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(50, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(150, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(10, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(210, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(200, 100), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(110, 100), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(110, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(100, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(10, 50, 200, 100);
            layout.Location = new PointF(100, 100);
            Assert.AreEqual(new PointF(100, 100), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(100, layout.X, "X is unexpected value.");
            Assert.AreEqual(100, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(100, 100, 200, 100), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(200, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(100, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(100, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(200, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(100, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(300, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(200, 100), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(200, 150), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(200, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(150, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(100, 100, 200, 100);
            layout.Size = new SizeF(60, 40);
            Assert.AreEqual(new PointF(100, 100), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(100, layout.X, "X is unexpected value.");
            Assert.AreEqual(100, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(100, 100, 60, 40), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(60, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(40, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(100, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(140, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(100, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(160, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(60, 40), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(130, 120), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(130, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(100, 100, 60, 40);
            layout.Rect = new RectangleF(0.5f, 0.2f, 0.4f, 0.2f);
            Assert.AreEqual(new PointF(0.5f, 0.2f), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(0.5f, layout.X, "X is unexpected value.");
            Assert.AreEqual(0.2f, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(0.5f, 0.2f, 0.4f, 0.2f), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(0.4f, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(0.2f, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(0.2f, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(0.4f, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(0.5f, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(0.9f, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(0.4f, 0.2f), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(0.7f, 0.3f), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(0.7f, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(0.3f, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(100, 100, 60, 40);
            layout.Offset = new PointF(60, 40);
            Assert.AreEqual(new PointF(100, 100), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(100, layout.X, "X is unexpected value.");
            Assert.AreEqual(100, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(100, 100, 60, 40), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(60, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(40, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(100, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(140, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(100, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(160, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(60, 40), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(130, 120), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(130, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(new PointF(60, 40), layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(60, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(40, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(100, 100, 60, 40);
            layout.OffsetX = 60;
            Assert.AreEqual(new PointF(100, 100), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(100, layout.X, "X is unexpected value.");
            Assert.AreEqual(100, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(100, 100, 60, 40), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(60, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(40, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(100, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(140, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(100, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(160, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(60, 40), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(130, 120), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(130, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(new PointF(60, 0), layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(60, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(100, 100, 60, 40);
            layout.OffsetY = 40;
            Assert.AreEqual(new PointF(100, 100), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(100, layout.X, "X is unexpected value.");
            Assert.AreEqual(100, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(100, 100, 60, 40), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(60, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(40, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(100, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(140, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(100, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(160, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(60, 40), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(130, 120), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(130, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(new PointF(0, 40), layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(40, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(100, 100, 60, 40);
            layout.CenterX = 50;
            Assert.AreEqual(new PointF(20, 100), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(20, layout.X, "X is unexpected value.");
            Assert.AreEqual(100, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(20, 100, 60, 40), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(60, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(40, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(100, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(140, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(20, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(80, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(60, 40), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(50, 120), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(50, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(20, 100, 60, 40);
            layout.CenterY = 50;
            Assert.AreEqual(new PointF(20, 30), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(20, layout.X, "X is unexpected value.");
            Assert.AreEqual(30, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(20, 30, 60, 40), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(60, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(40, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(30, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(70, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(20, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(80, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(new SizeF(60, 40), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(50, 50), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(50, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(50, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout(20, 30, 60, 40);
            layout.Center = new PointF(100, 100);
            Assert.AreEqual(new PointF(70, 80), layout.Location, "Location is unexpected value.");
            Assert.AreEqual(70, layout.X, "X is unexpected value.");
            Assert.AreEqual(80, layout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(70, 80, 60, 40), layout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(60, layout.Width, "Width is unexpected value.");
            Assert.AreEqual(40, layout.Height, "Height is unexpected value.");
            Assert.AreEqual(70, layout.Left, "Left is unexpected value.");
            Assert.AreEqual(130, layout.Right, "Right is unexpected value.");
            Assert.AreEqual(80, layout.Top, "Top is unexpected value.");
            Assert.AreEqual(120, layout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(new SizeF(60, 40), layout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(100, 100), layout.Center, "Center is unexpected value.");
            Assert.AreEqual(100, layout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(100, layout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(PointF.Empty, layout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, layout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, layout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, layout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, layout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, layout.GetHashCode(), "GetHashCode method returned unexpected value.");

        }

        /// <summary>
        /// TestToString
        /// </summary>
        [Test()]
        public void TestToString()
        {
            EcellLayout layout = new EcellLayout();
            string expected = "(0, 0, 0, 0, 0, 0, )";
            Assert.AreEqual(expected, layout.ToString(), "ToString method returned unexpected value.");

            layout = new EcellLayout(10, 20, 100, 200);
            layout.Offset = new PointF(30, 40);
            layout.Layer = "Layer";
            expected = "(10, 20, 100, 200, 30, 40, Layer)";
            Assert.AreEqual(expected, layout.ToString(), "ToString method returned unexpected value.");
        }

        /// <summary>
        /// TestClone
        /// </summary>
        [Test()]
        public void TestClone()
        {
            EcellLayout layout = new EcellLayout(10, 20, 100, 200);

            layout.Offset = new PointF(5, 15);
            EcellLayout newLayout = layout.Clone();
            Assert.AreEqual(layout, newLayout, "Clone method returned unexpected value.");

            Assert.AreEqual(new PointF(10, 20), newLayout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, newLayout.X, "X is unexpected value.");
            Assert.AreEqual(20, newLayout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 20, 100, 200), newLayout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(100, newLayout.Width, "Width is unexpected value.");
            Assert.AreEqual(200, newLayout.Height, "Height is unexpected value.");
            Assert.AreEqual(10, newLayout.Left, "Left is unexpected value.");
            Assert.AreEqual(110, newLayout.Right, "Right is unexpected value.");
            Assert.AreEqual(20, newLayout.Top, "Top is unexpected value.");
            Assert.AreEqual(220, newLayout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(new SizeF(100, 200), newLayout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(60, 120), newLayout.Center, "Center is unexpected value.");
            Assert.AreEqual(60, newLayout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, newLayout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(new PointF(5, 15), newLayout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(5, newLayout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(15, newLayout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, newLayout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, newLayout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, newLayout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout.Layer = "Layer";
            newLayout = (EcellLayout)((ICloneable)layout).Clone();
            Assert.AreEqual(layout, newLayout, "Clone method returned unexpected value.");

            Assert.AreEqual(new PointF(10, 20), newLayout.Location, "Location is unexpected value.");
            Assert.AreEqual(10, newLayout.X, "X is unexpected value.");
            Assert.AreEqual(20, newLayout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(10, 20, 100, 200), newLayout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(100, newLayout.Width, "Width is unexpected value.");
            Assert.AreEqual(200, newLayout.Height, "Height is unexpected value.");
            Assert.AreEqual(10, newLayout.Left, "Left is unexpected value.");
            Assert.AreEqual(110, newLayout.Right, "Right is unexpected value.");
            Assert.AreEqual(20, newLayout.Top, "Top is unexpected value.");
            Assert.AreEqual(220, newLayout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(new SizeF(100, 200), newLayout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(60, 120), newLayout.Center, "Center is unexpected value.");
            Assert.AreEqual(60, newLayout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(120, newLayout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(new PointF(5, 15), newLayout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(5, newLayout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(15, newLayout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual("Layer", newLayout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(false, newLayout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, newLayout.GetHashCode(), "GetHashCode method returned unexpected value.");

            layout = new EcellLayout();
            newLayout = layout.Clone();
            Assert.AreEqual(layout, newLayout, "Clone method returned unexpected value.");

            Assert.AreEqual(new PointF(0, 0), newLayout.Location, "Location is unexpected value.");
            Assert.AreEqual(0, newLayout.X, "X is unexpected value.");
            Assert.AreEqual(0, newLayout.Y, "Y is unexpected value.");
            Assert.AreEqual(new RectangleF(0, 0, 0, 0), newLayout.Rect, "Rect is unexpected value.");
            Assert.AreEqual(0, newLayout.Width, "Width is unexpected value.");
            Assert.AreEqual(0, newLayout.Height, "Height is unexpected value.");
            Assert.AreEqual(0, newLayout.Left, "Left is unexpected value.");
            Assert.AreEqual(0, newLayout.Right, "Right is unexpected value.");
            Assert.AreEqual(0, newLayout.Top, "Top is unexpected value.");
            Assert.AreEqual(0, newLayout.Bottom, "Bottom is unexpected value.");
            Assert.AreEqual(new SizeF(0, 0), newLayout.Size, "Size is unexpected value.");
            Assert.AreEqual(new PointF(0, 0), newLayout.Center, "Center is unexpected value.");
            Assert.AreEqual(0, newLayout.CenterX, "CenterX is unexpected value.");
            Assert.AreEqual(0, newLayout.CenterY, "CenterY is unexpected value.");
            Assert.AreEqual(new PointF(0, 0), newLayout.Offset, "Offset is unexpected value.");
            Assert.AreEqual(0, newLayout.OffsetX, "OffsetX is unexpected value.");
            Assert.AreEqual(0, newLayout.OffsetY, "OffsetY is unexpected value.");
            Assert.AreEqual(null, newLayout.Layer, "Layer is unexpected value.");
            Assert.AreEqual(true, newLayout.IsEmpty, "IsEmpty is unexpected value.");
            Assert.AreNotEqual(0, newLayout.GetHashCode(), "GetHashCode method returned unexpected value.");
        }
        
        /// <summary>
        /// TestEqual
        /// </summary>
        [Test()]
        public void TestEqual()
        {
            EcellLayout layout1 = new EcellLayout(10, 20, 100, 200);
            EcellLayout layout2 = new EcellLayout(10, 20, 100, 200);
            EcellLayout layout3 = new EcellLayout(10, 20, 100, 200);
            layout3.Layer = "Layer";

            EcellLayout layout4 = new EcellLayout(10, 20, 100, 200);
            layout4.Offset = new PointF(10, 20);

            EcellLayout layout5 = new EcellLayout(11, 20, 100, 200);
            EcellLayout layout6 = new EcellLayout(10, 21, 100, 200);
            EcellLayout layout7 = new EcellLayout(10, 20, 101, 200);
            EcellLayout layout8 = new EcellLayout(10, 20, 100, 201);

            Assert.AreEqual(true, layout1.Equals(layout2), "Equal method returned unexpected value.");
            Assert.AreEqual(false, layout1.Equals(layout3), "Equal method returned unexpected value.");
            Assert.AreEqual(false, layout1.Equals(layout4), "Equal method returned unexpected value.");
            Assert.AreEqual(false, layout1.Equals(layout5), "Equal method returned unexpected value.");
            Assert.AreEqual(false, layout1.Equals(layout6), "Equal method returned unexpected value.");
            Assert.AreEqual(false, layout1.Equals(layout7), "Equal method returned unexpected value.");
            Assert.AreEqual(false, layout1.Equals(layout8), "Equal method returned unexpected value.");

            Assert.AreEqual(false, layout1.Equals(new object()), "Equal method returned unexpected value.");
        }
                
        /// <summary>
        /// TestContains
        /// </summary>
        [Test()]
        public void TestContains()
        {
            EcellLayout layout = new EcellLayout(10, 20, 100, 200);

            // edge left-top
            Assert.AreEqual(false, layout.Contains(new PointF(9, 20)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(10, 20)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(11, 20)), "Contains method returned unexpected value.");

            Assert.AreEqual(false, layout.Contains(new PointF(10, 19)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(10, 20)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(10, 21)), "Contains method returned unexpected value.");

            // edge left-bottom
            Assert.AreEqual(false, layout.Contains(new PointF(9, 219)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(10, 219)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(11, 219)), "Contains method returned unexpected value.");

            Assert.AreEqual(true, layout.Contains(new PointF(10, 218)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(10, 219)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new PointF(10, 220)), "Contains method returned unexpected value.");

            // edge right-top
            Assert.AreEqual(true, layout.Contains(new PointF(108, 20)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(109, 20)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new PointF(110, 20)), "Contains method returned unexpected value.");

            Assert.AreEqual(false, layout.Contains(new PointF(109, 19)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(109, 20)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(109, 21)), "Contains method returned unexpected value.");

            // edge right-top
            Assert.AreEqual(true, layout.Contains(new PointF(108, 219)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(109, 219)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new PointF(110, 219)), "Contains method returned unexpected value.");

            Assert.AreEqual(true, layout.Contains(new PointF(109, 218)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new PointF(109, 219)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new PointF(109, 220)), "Contains method returned unexpected value.");


            // rectangle
            Assert.AreEqual(true, layout.Contains(new RectangleF(10, 20, 100, 200)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new RectangleF(20, 30, 80, 180)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new RectangleF(10, 20, 50, 100)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new RectangleF(10, 120, 50, 100)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new RectangleF(60, 20, 50, 100)), "Contains method returned unexpected value.");
            Assert.AreEqual(true, layout.Contains(new RectangleF(60, 120, 50, 100)), "Contains method returned unexpected value.");

            Assert.AreEqual(false, layout.Contains(new RectangleF(0, 0, 10, 10)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new RectangleF(0, 0, 200, 300)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new RectangleF(10, 0, 50, 100)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new RectangleF(0, 20, 50, 100)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new RectangleF(10, 130, 50, 100)), "Contains method returned unexpected value.");
            Assert.AreEqual(false, layout.Contains(new RectangleF(70, 20, 50, 100)), "Contains method returned unexpected value.");
        }
    }
}
