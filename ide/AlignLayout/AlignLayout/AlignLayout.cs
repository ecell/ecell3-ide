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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ecell.Plugin;
using Ecell.Objects;
using System.Reflection;

namespace Ecell.IDE.Plugins.AlignLayout
{
    /// <summary>
    /// Layout algorithm to align nodes
    /// </summary>
    public class AlignLayout : LayoutBase, IMenuStripProvider
    {
        enum Alignment {
            /// <summary>
            /// Align vertically at left side.
            /// </summary>
            Left,
            /// <summary>
            /// Align vertically at right side.
            /// </summary>
            Right,
            /// <summary>
            /// Align horizontally at upper side.
            /// </summary>
            Upper,
            /// <summary>
            /// Align horizontally at lower side.
            /// </summary>
            Lower
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AlignLayout()
        {
            m_panel = new AlignLayoutPanel(this);
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"MessageWindow"</returns>
        public override string GetPluginName()
        {
            return "AlignLayout";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override string GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        /// <summary>
        /// Execute layout
        /// </summary>
        /// <param name="subNum">
        /// An index of sub command which was clicked on subMenu.
        /// Sub command which is in subCommandNum position in the list returned by GetSubCommands() [0 origin]
        /// If layout name itself was clicked, subCommandNum = -1.
        /// </param>
        /// <param name="layoutSystem">Whether systems should be layouted or not</param>
        /// <param name="systemList">Systems (can null)</param>
        /// <param name="nodeList">Nodes (Variables, Processes)</param>
        /// <returns>Whether layout is completed or aborted</returns>
        public override bool DoLayout(int subNum,
                             bool layoutSystem,
                             List<EcellObject> systemList,
                             List<EcellObject> nodeList)
        {
            // Error check.
            nodeList = GetSelectedObject(nodeList);
            if (nodeList.Count <= 1)
                return false;

            RectangleF rect = GetSurroundingRect(nodeList);

            Alignment align = GetAlignment(subNum);
            float alignValue = GetAlignValue(rect, align); // Set all nodes X or Y to this position

            // Set x or y coordinate to alignValue
            foreach (EcellObject node in nodeList)
            {
                switch (align)
                {
                    case Alignment.Left:
                    case Alignment.Right:
                        node.CenterPointF = new PointF(alignValue, node.CenterPointF.Y);
                        break;
                    case Alignment.Upper:
                    case Alignment.Lower:
                        node.CenterPointF = new PointF(node.CenterPointF.X, alignValue);
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Get LayoutType of this layout
        /// </summary>
        /// <returns>LayoutType of this layout</returns>
        public override LayoutType GetLayoutType()
        {
            return LayoutType.Selected;
        }

        /// <summary>
        /// Get a name of this layout
        /// </summary>
        /// <returns>a name of this layout</returns>
        public override string GetLayoutName()
        {
            return "Align";
        }

        #region Private Methods
        /// <summary>
        /// Get aligned value.
        /// </summary>
        /// <param name="rect">List</param>
        /// <param name="align">Alignment</param>
        /// <returns>float</returns>
        private float GetAlignValue(RectangleF rect, Alignment align)
        {
            switch (align)
            {
                case Alignment.Left:
                default:
                    return rect.X;
                case Alignment.Right:
                    return rect.X + rect.Width;
                case Alignment.Upper:
                    return rect.Y;
                case Alignment.Lower:
                    return rect.Y + rect.Height;
            }
        }

        /// <summary>
        /// Get Alignment direction.
        /// </summary>
        /// <param name="subNum">Number of Layout</param>
        /// <returns>Alignment</returns>
        private Alignment GetAlignment(int subNum)
        {
            switch (subNum)
            {
                case 0:default:
                    return Alignment.Left;
                case 1:
                    return Alignment.Right;
                case 2:
                    return Alignment.Upper;
                case 3:
                    return Alignment.Lower;
            }
        }
        #endregion

        /// <summary>
        /// Return MenuStrips for Ecell IDE's MainMenu.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            ToolStripMenuItem layoutMenu = new ToolStripMenuItem();
            layoutMenu.Name = MenuConstants.MenuItemLayout;

            ToolStripMenuItem algoMenu = new ToolStripMenuItem(
                MessageResAlignLayout.MenuItemAlign,
                null,
                new ToolStripMenuItem(MessageResAlignLayout.MenuItemSubLeft, null,
                    new EventHandler(delegate(object o, EventArgs e)
                    {
                        m_env.PluginManager.DiagramEditor.InitiateLayout(this, (int)Alignment.Left);
                    })
                ),
                new ToolStripMenuItem(MessageResAlignLayout.MenuItemSubRight, null,
                    new EventHandler(delegate(object o, EventArgs e)
                    {
                        m_env.PluginManager.DiagramEditor.InitiateLayout(this, (int)Alignment.Right);
                    })
                ),
                new ToolStripMenuItem(MessageResAlignLayout.MenuItemSubUpper, null,
                    new EventHandler(delegate(object o, EventArgs e)
                    {
                        m_env.PluginManager.DiagramEditor.InitiateLayout(this, (int)Alignment.Upper);
                    })
                ),
                new ToolStripMenuItem(MessageResAlignLayout.MenuItemSubLower, null,
                    new EventHandler(delegate(object o, EventArgs e)
                    {
                        m_env.PluginManager.DiagramEditor.InitiateLayout(this, (int)Alignment.Lower);
                    })
                )
            );

            algoMenu.ToolTipText = MessageResAlignLayout.ToolTip;

            layoutMenu.DropDownItems.Add(algoMenu);
            return new ToolStripMenuItem[] { layoutMenu };
        }
    }
}
