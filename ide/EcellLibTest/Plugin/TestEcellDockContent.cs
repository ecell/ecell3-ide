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
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using System.Drawing;

namespace Ecell.Plugin
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestEcellDockContent
    {
        private EcellDockContent _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EcellDockContent();
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
            EcellDockContent content = new EcellDockContent();
            Assert.IsNotNull(content, "Constructor of type, EcellDockContent failed to create instance.");
            Assert.AreEqual("Ecell.Plugin.EcellDockContent", content.GetType().ToString(), "Type is unexpected value.");
            Assert.AreEqual(false, content.IsSavable, "IsSavable is unexpected value.");

            content.IsSavable = true;
            Assert.AreEqual(true, content.IsSavable, "IsSavable is unexpected value.");

            Form form = null;
            try
            {
                form = new Form();
                DockPanel panel = new DockPanel();
                panel.DocumentStyle = DocumentStyle.DockingWindow;
                form.SuspendLayout();
                form.Controls.Add(panel);
                form.SuspendLayout();
                form.Show();

                //Create New DockContent
                content.Pane = null;
                content.PanelPane = null;
                content.FloatPane = null;
                content.DockHandler.DockPanel = panel;
                content.IsHidden = false;
                content.Show();
                content.GetDesktopLocation();
                content.FloatAt(new Rectangle(100, 100, 500, 500));
                content.GetDesktopLocation();
                content.Hide();
            }
            catch (Exception e)
            {
                Assert.Fail("Fail to create new DockContent");
            }
            finally
            {
                if (form != null)
                    form.Close();
            }
        }
    }
}
