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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EcellLib.PathwayWindow.Element
{
    /// <summary>
    /// ComponentElement is an element class for a component.
    /// A component is a compornent part of an E-cell pathway
    /// </summary>
    [Serializable]
    public abstract class ComponentElement : PathwayElement
    {
        #region Fields
        /// <summary>
        /// Model ID
        /// </summary>
        protected string m_modelId;

        /// <summary>
        /// key of this component
        /// </summary>
        protected string m_key;

        /// <summary>
        /// Canvas ID, which this component belongs.
        /// </summary>
        protected string m_canvasId;

        /// <summary>
        /// Layer ID, which this component belongs.
        /// </summary>
        protected string m_layerId;

        /// <summary>
        /// System ID of this component's parent system.
        /// </summary>
        protected string m_parentSystemId;

        /// <summary>
        /// type of this component, such as System, Variable, Process.
        /// </summary>
        protected string m_type;

        /// <summary>
        /// ComponentSetting ID of this component.
        /// </summary>
        protected string m_csId;

        /// <summary>
        /// X coordinate of this node.
        /// </summary>
        protected float m_x;

        /// <summary>
        /// Y coordinate of this node.
        /// </summary>
        protected float m_y;

        /// <summary>
        /// For optional information
        /// </summary>
        protected object[] m_optional;

        /// <summary>
        /// Whether this object has Logger or not.
        /// </summary>
        protected bool m_isLogger = false;

        /// <summary>
        /// Model ID
        /// </summary>
        protected Node.PPathwayObject m_pObject;

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_modelId.
        /// </summary>
        public string ModelID
        {
            get { return m_modelId; }
            set { m_modelId = value; }
        }
        /// <summary>
        /// Accessor for m_key.
        /// </summary>
        public virtual string Key
        {
            get { return m_key; }
            set
            {
                m_key = value;
                m_parentSystemId = PathUtil.GetParentSystemId(m_key);
            }
        }
        /// <summary>
        /// Accessor for m_canvasId.
        /// </summary>
        public string CanvasID
        {
            get { return m_canvasId; }
            set { m_canvasId = value; }
        }
        /// <summary>
        /// Accessor for m_layerId.
        /// </summary>
        public string LayerID
        {
            get { return m_layerId; }
            set { m_layerId = value; }
        }
        
        /// <summary>
        /// Accessor for m_parentSystemId.
        /// </summary>
        [XmlIgnore]
        public string ParentSystemID
        {
            get { return m_parentSystemId; }            
        }

        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        [XmlIgnore]
        public string Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// Accessor for m_csId.
        /// </summary>
        public string CsId
        {
            get { return m_csId; }
            set { m_csId = value; }
        }

        /// <summary>
        /// Accessor for name of a component.
        /// </summary>
        public string Name
        {
            get { return PathUtil.RemovePath(m_key); }
        }
        /// <summary>
        /// Accessor for Text.
        /// </summary>
        public string Text
        {
            get
            {
                if (EcellObject == null || !EcellObject.IsLogger)
                    return this.Name;
                else
                    return this.Name + " *";
            }
        }
        /// <summary>
        /// Accessor for m_x.
        /// </summary>
        public float X
        {
            get { return m_x; }
            set { m_x = value; }
        }
        /// <summary>
        /// Accessor for m_y.
        /// </summary>
        public float Y
        {
            get { return m_y; }
            set { m_y = value; }
        }
        /// <summary>
        /// Accessor for m_optional.
        /// </summary>
        public object[] Optional
        {
            get { return m_optional; }
            set { m_optional = value; }
        }
        /// <summary>
        /// Accessor for EcellObject.
        /// </summary>
        public EcellObject EcellObject
        {
            get
            {
                try
                {
                    EcellObject obj = DataManager.GetDataManager().GetEcellObject(this.ModelID, this.Key, this.Type);
                    return obj;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion

    }
}
