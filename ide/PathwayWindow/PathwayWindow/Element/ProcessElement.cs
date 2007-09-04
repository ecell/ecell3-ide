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
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace EcellLib.PathwayWindow.Element
{
    #region Enum
    /// <summary>
    /// Enumeration for a direction of a edge
    /// </summary>
    public enum EdgeDirection
    {
        /// <summary>
        /// Outward direction
        /// </summary>
        Outward,
        /// <summary>
        /// Inward direction
        /// </summary>
        Inward,
        /// <summary>
        /// Outward and inward direction
        /// </summary>
        Bidirection,
        /// <summary>
        /// An edge has no direction
        /// </summary>
        None
    }
    /// <summary>
    /// Enumeration of a type of a line.
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        Unknown,
        /// <summary>
        /// Solid line
        /// </summary>
        Solid,
        /// <summary>
        /// Dashed line
        /// </summary>
        Dashed
    }
    #endregion
    
    /// <summary>
    /// ProcessElement is an element class for a process.
    /// </summary>
    [Serializable]
    public class ProcessElement : NodeElement
    {
        #region Static readonly fields 
        /// <summary>
        /// Regex for VariableReferenceList
        /// </summary>
        private static readonly Regex VRL_REGEX = new Regex("\\((?<refer>.+?)\\)");

        /// <summary>
        /// Regex for VariableReference.
        /// </summary>
        private static readonly Regex VR_REGEX = new Regex("\"(?<name>.+)\",(.+)\"(?<id>.+)\", (?<coe>.+), (?<fix>.+)");
        #endregion

        #region Fields
        /// <summary>
        /// Dictionary for 
        /// </summary>
        private Dictionary<string, EdgeInfo> m_edges = new Dictionary<string,EdgeInfo>();
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for a dictionary of edges.
        /// </summary>
        [XmlIgnore]        
        public Dictionary<string, EdgeInfo> Edges
        {
            get { return m_edges; }
            set { m_edges = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessElement()
        {
            base.Element = ElementType.Process;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set EdgeInfos of this process.
        /// </summary>
        /// <param name="varRefList">VariableReferenceList</param>
        /// <returns>True if the format of an argument is valid and m_edges are correctly substituted.
        /// Otherwise, false will be returned.</returns>
        public bool SetEdgesByStr(string varRefList)
        {
            if (varRefList == null || varRefList == "") return false;
            string text = varRefList.Substring(1, varRefList.Length - 1);

            // Flag for whether all VariableReferences are in valid format or not.
            bool isAllValid = true;

            Dictionary<string, EdgeInfo> edgeInfos = new Dictionary<string, EdgeInfo>();

            // Parse VariableReferenceList to VariableReference, and process each of them.
            foreach (Match mat in VRL_REGEX.Matches(text))
            {
                Match m = VR_REGEX.Match(mat.Groups["refer"].Value);
                if (m.Success)
                {
                    string key = PathUtil.FullID2Key(m.Groups["id"].Value);
                    string name = m.Groups["name"].Value;
                    EdgeDirection direction;
                    LineType type;
                    int coe = Convert.ToInt32(m.Groups["coe"].Value);
                    if (coe < 0)
                    {
                        direction = EdgeDirection.Inward;
                        type = LineType.Solid;
                    }
                    else if (coe == 0)
                    {
                        direction = EdgeDirection.None;
                        type = LineType.Dashed;
                    }
                    else
                    {
                        direction = EdgeDirection.Outward;
                        type = LineType.Solid;
                    }

                    int isAccessor = Convert.ToInt32(m.Groups["fix"].Value);

                    if (key == null)
                        continue;

                    if (edgeInfos.ContainsKey(key))
                    {
                        edgeInfos[key].AddRelation(name, direction, type, isAccessor);
                    }
                    else
                    {
                        EdgeInfo edgeInfo = new EdgeInfo(this.m_key, key);
                        edgeInfo.AddRelation(name, direction, type, isAccessor);
                        edgeInfos.Add(key, edgeInfo);
                    }
                }
                else
                    isAllValid = false;
            }

            if (isAllValid)
                m_edges = edgeInfos;

            return isAllValid;
        }
        /// <summary>
        /// Set EdgeInfos of this process.
        /// </summary>
        /// <param name="value">EcellValue</param>
        public void SetEdgesByEcellValue(EcellValue value)
        {
            // Get Variable Reference List
            if (value == null) return;
            List<EcellValue> varRefList = value.CastToList();
            if (varRefList == null || varRefList.Count == 0) return;

            // Flag for whether all VariableReferences are in valid format or not.
            Dictionary<string, EdgeInfo> edgeInfos = new Dictionary<string, EdgeInfo>();

            // Parse VariableReferenceList to VariableReference, and process each of them.
            foreach (EcellValue varRef in varRefList)
            {
                EdgeInfo edgeInfo = new EdgeInfo(this.Key, varRef);
                edgeInfos.Add(edgeInfo.VariableKey, edgeInfo);
            }
            m_edges = edgeInfos;
        }
        #endregion
    }

    /// <summary>
    /// EdgeInfo contains all information for one edge.
    /// </summary>
    public class EdgeInfo
    {
        #region Fields

        /// <summary>
        /// Key of a process, an owner of this edge.
        /// </summary>
        protected string m_proKey;

        /// <summary>
        /// Key of a variable with which a process has an edge.
        /// </summary>
        protected string m_varKey;

        /// <summary>
        /// Direction of this edge.
        /// </summary>
        protected EdgeDirection m_direction = EdgeDirection.None;

        /// <summary>
        /// Type of a line of this edge.
        /// </summary>
        protected LineType m_type = LineType.Unknown;

        /// <summary>
        /// Type of a line of this edge.
        /// </summary>
        protected int m_isFixed = 0;

        /// <summary>
        /// List of EachRelation.
        /// </summary>
        protected List<EachRelation> m_each = new List<EachRelation>();
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EdgeInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proKey">key of process</param>
        /// <param name="varKey">key of variable</param>
        public EdgeInfo(string proKey, string varKey)
        {
            m_proKey = proKey;
            m_varKey = varKey;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">EcellValue</param>
        public EdgeInfo(string processKey, EcellValue ecellValue)
        {
            List<EcellValue> values = ecellValue.CastToList();
            m_proKey = processKey;
            m_varKey = values[1].CastToString().Substring(1);
            // Set Relation
            string l_name = values[0].CastToString();
            int l_coef = values[2].CastToInt();
            int l_isFixed = values[3].CastToInt();

            EdgeDirection l_direction;
            LineType l_type;
            if (l_coef < 0)
            {
                l_direction = EdgeDirection.Inward;
                l_type = LineType.Solid;
            }
            else if (l_coef == 0)
            {
                l_direction = EdgeDirection.None;
                l_type = LineType.Dashed;
            }
            else
            {
                l_direction = EdgeDirection.Outward;
                l_type = LineType.Solid;
            }

            AddRelation(l_name, l_direction, l_type, l_isFixed);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string VariableKey
        {
            get { return m_varKey; }
            set { m_varKey = value; }
        }
        /// <summary>
        /// Accessor for m_varkey.
        /// </summary>
        public string ProcessKey
        {
            get { return m_proKey; }
            set { m_proKey = value; }
        }
        /// <summary>
        /// Accessor for m_direction.
        /// </summary>
        public EdgeDirection Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }
        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        public LineType TypeOfLine
        {
            get { return m_type; }
            set { m_type = value; }
        }
        /// <summary>
        /// Accessor for m_type.
        /// </summary>
        public int IsFixed
        {
            get { return m_isFixed; }
            set { m_isFixed = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add new relation to this .
        /// </summary>
        /// <param name="name">The name of VariableReference</param>
        /// <param name="direction">A type of direction</param>
        /// <param name="type">A line type.</param>
        /// <param name="isFixed">Whether this relation is fixed or not</param>
        public void AddRelation(string name, EdgeDirection direction, LineType type, int isFixed)
        {
            switch (m_direction)
            {
                case EdgeDirection.Bidirection:
                    break;
                case EdgeDirection.Inward:
                    if (direction == EdgeDirection.Outward)
                        m_direction = EdgeDirection.Bidirection;
                    break;
                case EdgeDirection.Outward:
                    if (direction == EdgeDirection.Inward)
                        m_direction = EdgeDirection.Bidirection;
                    break;
                case EdgeDirection.None:
                    m_direction = direction;
                    break;
            }
            
            switch (m_type)
            {
                case LineType.Unknown:
                    m_type = type;
                    break;
                case LineType.Solid:
                    m_type = type;
                    break;
                case LineType.Dashed:
                    m_type = type;
                    break;
            }

            m_isFixed = isFixed;

            m_each.Add( new EachRelation(name, direction, type, isFixed) );
        }

        #endregion

        /// <summary>
        /// A relation with a variable.        
        /// </summary>
        protected class EachRelation
        {
            #region Fields
            /// <summary>
            /// The name of variable reference.
            /// </summary>
            protected string m_name;

            /// <summary>
            /// The direction of this relation.
            /// </summary>
            protected EdgeDirection m_direction;

            /// <summary>
            /// The type of this relation.
            /// </summary>
            protected LineType m_type;

            /// <summary>
            /// Whether this relation is fixed or not.
            /// </summary>
            protected int m_isFixed;
            #endregion

            #region Accessors
            /// <summary>
            /// Accessor for m_name.
            /// </summary>
            public string Name
            {
                get { return m_name; }
                set { m_name = value; }
            }
            /// <summary>
            /// Accessor for m_direction.
            /// </summary>
            public EdgeDirection Direction
            {
                get { return m_direction; }
                set { m_direction = value; }
            }
            /// <summary>
            /// Accessor for m_type.
            /// </summary>
            public LineType Type
            {
                get { return m_type; }
                set { m_type = value; }
            }
            /// <summary>
            /// Accessor for m_isFixed.
            /// </summary>
            public int Fixed
            {
                get { return m_isFixed; }
                set { m_isFixed = value; }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// A constructor.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="direction"></param>
            /// <param name="type"></param>
            /// <param name="isFixed"></param>
            public EachRelation(string name, EdgeDirection direction, LineType type, int isFixed)
            {
                m_name = name;
                m_direction = direction;
                m_type = type;
                m_isFixed = isFixed;
            }
            #endregion
        }
    }
}