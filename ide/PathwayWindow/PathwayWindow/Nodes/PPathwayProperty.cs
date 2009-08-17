﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
using UMD.HCIL.Piccolo;
using Ecell.Objects;
using UMD.HCIL.Piccolo.Nodes;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    /// <summary>
    /// Property Panel for PPathwayObject
    /// </summary>
    public class PPathwayProperties : PPathwayNode
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        List<PPathwayProperty> m_properties = new List<PPathwayProperty>();
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public List<PPathwayProperty> Properties
        {
            get { return m_properties; }
            set { m_properties = value; }
        }

        #endregion


        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayProperties(PPathwayObject obj)
        {
            this.Pickable = false;
            this.Visible = false;
            SetObject(obj);
        }

        /// <summary>
        /// Set Object
        /// </summary>
        /// <param name="obj"></param>
        private void SetObject(PPathwayObject obj)
        {
            // Reset
            this.RemoveAllChildren();
            this.m_properties.Clear();
            // Set Property
            if(obj is PPathwayProcess)
            {
                AddValue("Activity");
                AddValue("MolarActivity");
            }
            else if(obj is PPathwayVariable)
            {
                AddValue("Value");
                AddValue("MolarConc");
                AddValue("NumberConc");
            }

        }

        /// <summary>
        /// Set parameter
        /// </summary>
        /// <param name="param"></param>
        private void AddValue(string param)
        {
            string name = param;
            string value = "";
            PPathwayProperty property = new PPathwayProperty(name, value);
            m_properties.Add(property);
            this.AddChild(property);
            Refresh();
        }
        #endregion

        #region Methods


        /// <summary>
        /// 
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            int i = 0;
            foreach (PPathwayProperty property in m_properties)
            {
                if (!property.Visible)
                    continue;
                property.X = this.X;
                property.Y = this.Y + i * 20.5f;
                property.Refresh();
                i++;
            }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class PPathwayProperty : PPathwayNode
    {
        #region Fields
        const float VALUE_POS = 102.5f;
        const float VALUE_WIDTH = 140.5f;
        const float MARGIN_Y = 2f;
        internal PText label = null;
        internal PText value = null;
        #endregion

        #region Properties
        /// <summary>
        /// Label of this Property.
        /// </summary>
        public string Label
        {
            set { this.label.Text = value; }
            get { return this.label.Text; }
        }

        /// <summary>
        /// Value of this Property.
        /// </summary>
        public string Value
        {
            get { return this.value.Text; }
            set { this.value.Text = value; }
        }

        /// <summary>
        /// TextBrush of value text.
        /// </summary>
        public Brush TextBrush
        {
            get { return this.value.TextBrush; }
            set { this.value.TextBrush = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public PPathwayProperty()
        {
            this.Pickable = false;
            this.Pen = new Pen(Brushes.Black);
            this.Brush = Brushes.AntiqueWhite;
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0, 0, VALUE_POS + VALUE_WIDTH, 20.5f));
            path.AddLine(VALUE_POS, 0, VALUE_POS, 20.5f);
            this.AddPath(path, false);

            this.label = new PText();
            this.label.X = this.X;
            this.label.Y = this.Y;
            this.label.Brush = Brushes.Transparent;
            this.label.Pickable = false;
            this.label.TextAlignment = StringAlignment.Near;
            this.AddChild(this.label);

            this.value = new PText();
            this.value.X = this.X + VALUE_POS;
            this.value.Y = this.Y;
            this.value.Brush = Brushes.Transparent;
            this.value.Pickable = false;
            this.AddChild(this.value);
        }

        /// <summary>
        /// Constructor with params
        /// </summary>
        /// <param name="label"></param>
        /// <param name="value"></param>
        public PPathwayProperty(string label, string value)
            : this()
        {
            this.label.Text = label;
            this.value.Text = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.label.X = this.X;
            this.label.Y = this.Y;
            this.value.X = this.X + VALUE_POS;
            this.value.Y = this.Y;
        }
        #endregion
    }
}
