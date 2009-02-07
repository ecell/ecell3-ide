//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//       This file is part of the E-Cell System
//
//       Copyright (C) 1996-2009 Keio University
//       Copyright (C) 2005-2008 The Molecular Sciences Institute
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell System is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
// 
// E-Cell System is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public
// License along with E-Cell System -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
// 
// END_HEADER
//
// Created by suzuki <suzuki@sfc.keio.ac.jp>,
// Keio University.
// Created    :2002/03/16
// Last Update:2002/09/11
//
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
// Created    :2009/01/14
// Last Update:2009/02/02
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    /// <summary>
    /// 
    /// </summary>
    public class Eml
    {
        XmlDocument theDocument = null;
        XmlNode theEmlNode = null;
        string theComment = null;
        Dictionary<string, XmlNode> entityNodeCache = new Dictionary<string, XmlNode>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public Eml(string filename)
        {
            this.theDocument = new XmlDocument();
            if(string.IsNullOrEmpty(filename))
                theDocument.CreateElement("eml");
            else
                theDocument.Load(filename);

            foreach (XmlNode aNode in theDocument.ChildNodes)
            {
                if (aNode.Name == "#comment")
                    this.theComment = aNode.Value;
                else if (aNode.Name == "eml")
                    this.theEmlNode = aNode;
            }

            // this.clearCache();
            this.reconstructCache();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string asString()
        {
            return this.theDocument.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void save(string filename)
        {
            this.theDocument.Save(filename);
        }

        #region Methods for Stepper
        /// <summary>
        /// create a stepper
        /// </summary>
        /// <param name="aClass"></param>
        /// <param name="anID"></param>
        public void createStepper(string aClass, string anID)
        {
            XmlNode aStepperElement = this.createElement( "stepper" );
            aStepperElement.setAttribute( "class", aClass );
            aStepperElement.setAttribute("id", anID);
            this.theDocument.DocumentElement.AppendChild(aStepperElement);
        }
        /// <summary>
        /// delete a stepper
        /// </summary>
        /// <param name="anID"></param>
        public void deleteStepper(string anID)
        {
            foreach(XmlNode anElement in this.theEmlNode.ChildNodes)
            {
                if(anElement.Name == "stepper" && anElement.Attributes["id"].Value == anID)
                    theEmlNode.RemoveChild( anElement );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> getStepperList()
        {
            List<string> aStepperList = new List<string>();
            foreach(XmlNode aTargetStepperNode in this.getStepperNodeList())
            {
                string aStepperID = aTargetStepperNode.Attributes["id"].Value;
                aStepperList.Add(aStepperID);
            }
            return aStepperList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aStepperID"></param>
        /// <returns></returns>
        public List<string> getStepperPropertyList( string aStepperID )
        {
            XmlNode aStepperNodeList = this.getStepperNode( aStepperID );
            List<string> aPropertyList = new List<string>();

            foreach(XmlNode aChildNode in aStepperNodeList.ChildNodes)
            {
                if(aChildNode.Name == "property")
                {
                    string aPropertyName = aChildNode.Attributes[ "name"].Value;
                    aPropertyList.Add(aPropertyName);
                }
            }
            return aPropertyList;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aStepperID"></param>
        /// <param name="aPropertyName"></param>
        /// <returns></returns>
        public List<string> getStepperProperty(string aStepperID, string aPropertyName )
        {

            List<string> aValueList = new List<string>();

            XmlNode aStepperNode = this.getStepperNode( aStepperID );
            foreach(XmlNode aPropertyNode in aStepperNode.ChildNodes)
            {
                if (aPropertyNode.Name != "property")
                    continue;
                if (aPropertyNode.Attributes["name"].Value != aPropertyName)
                    continue;

                foreach(XmlNode aChildNode in aPropertyNode.ChildNodes)
                {
                    if (aChildNode.Name == "value")
                    {
                        string aValue = aChildNode.FirstChild.Value;
                        aValueList.Add( aValue );
                    }
                }
            }
            return aValueList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aStepperID"></param>
        /// <returns></returns>
        public string getStepperClass(string aStepperID )
        {
            XmlNode aStepperNode = this.getStepperNode( aStepperID );
            return aStepperNode.Attributes[ "class"].Value;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<XmlNode> getStepperNodeList()
        {
            List<XmlNode> aStepperNodeList = new List<XmlNode>();

            foreach(XmlNode aTargetNode in this.theDocument.DocumentElement.ChildNodes)
            {
                if (aTargetNode.Name == "stepper")
                {
                    aStepperNodeList.Add( aTargetNode );
                }
            }
            return aStepperNodeList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aStepperID"></param>
        /// <param name="aPropertyName"></param>
        /// <param name="aValue"></param>
        private void setStepperProperty(string aStepperID, string aPropertyName, string aValue )
        {
            // what if a property with the same name already exist?
            XmlNode aPropertyElement = this.createPropertyNode( aPropertyName, aValue );
            XmlNode aStepperNode = this.getStepperNode( aStepperID );
            aStepperNode.AppendChild( aPropertyElement );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aStepperID"></param>
        private XmlNode getStepperNode(string aStepperID)
        {
            List<XmlNode> aStepperNodeList = this.getStepperNodeList();

            foreach(XmlNode aTargetStepperNode in aStepperNodeList)
            {
                if (aTargetStepperNode.Attributes["id"].Value == aStepperID)
                    return aTargetStepperNode;
            }
            return null;
        }

        #endregion

        #region Methods for Entity
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aClass"></param>
        /// <param name="aFullID"></param>
        public void createEntity(string aClass, string aFullID )
        {
            string anEntityType, aTargetPath, anID;
            Util.ParseFullID(aFullID, out anEntityType, out aTargetPath, out anID);

            XmlNode anEntityElement = this.createElement( anEntityType.ToLower() );
            anEntityElement.setAttribure("class", aClass);


            if( anEntityType == "System" )
            {
                XmlNode dummy;
                if (aTargetPath != "")  // check if the supersystem exists
                    dummy = this.getSystemNode(aTargetPath);

                anID = convertSystemFullID2SystemID( aFullID );
                anEntityElement.setAttribute( "id", anID );
                this.theDocument.DocumentElement.AppendChild( anEntityElement );
            }
            else if( anEntityType == "Variable" || anEntityType == "Process" )
            {
                anEntityElement.setAttribute( "id", anID );
                XmlNode aTargetSystemNode = this.getSystemNode( aTargetPath );
                aTargetSystemNode.AppendChild( anEntityElement );
            }
            else            
                throw new EcellException(string.Format("unexpected error. {0} should be System, Variable, or Process.", anEntityType));

            this.addToCache(aFullID, anEntityElement);
        }
        /// <summary>
        /// delete an entity
        /// </summary>
        /// <param name="aFullID"></param>
        public void deleteEntity( string aFullID )
        {
            string aType = aFullID.Split(':')[0];

            if (aType == "System")
            {
                foreach(XmlNode anElement in this.theEmlNode.ChildNodes)
                {
                    if (convertSystemID2SystemFullID( anElement.Attributes["id"]) == aFullID)
                        this.theEmlNode.RemoveChild( anElement );
                }
            }
            else
            {
                string anEntityType, aTargetPath, anID;
                Util.ParseFullID(aFullID, out anEntityType, out aTargetPath, out anID);

                foreach(XmlNode anElement in this.theEmlNode.ChildNodes)
                {
                    if (anElement.Name != "system")
                        continue;
                    if (anElement.Attributes["id"].Value != aTargetPath)
                        continue;

                    foreach(XmlNode aChild in anElement.ChildNodes)
                    {
                        if (aChild.Name == anEntityType.ToLower() &&
                           aChild.Attributes["id"].Value == anID)
                            anElement.RemoveChild( aChild );
                    }
                }
            }
            this.removeFromCache(aFullID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <returns></returns>
        public bool isEntityExist( string aFullID )
        {
            XmlNode node = null;
            try
            {
                node = this.getEntityNode(aFullID);
            }
            catch (Exception)
            {
                return false;
            }
            return node != null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <returns></returns>
        public string getEntityClass(string aFullID )
        {
            XmlNode anEntityNode = this.getEntityNode( aFullID );
            return anEntityNode.Attributes["class"].Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <param name="aPropertyName"></param>
        /// <param name="aValueList"></param>
        public void setEntityProperty(string aFullID, string aPropertyName, List<string> aValueList )
        {
            XmlNode anEntityPropertyElement = this.createPropertyNode( aPropertyName, aValueList );
            XmlNode aTargetNode = this.getEntityNode( aFullID );
            aTargetNode.AppendChild(anEntityPropertyElement);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <param name="aPropertyName"></param>
        public void deleteEntityProperty(string aFullID, string aPropertyName )
        {
            XmlNode aTargetNode = this.getEntityNode( aFullID );

            foreach(XmlNode aChild in aTargetNode.ChildNodes)
            {
                if (aChild.Name == "property" && aChild.Attributes["name"].Value == aPropertyName)
                    aTargetNode.RemoveChild(aChild);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anEntityType"></param>
        /// <param name="aSystemPath"></param>
        /// <returns></returns>
        public List<string> getEntityList(string anEntityType, string aSystemPath)
        {
            // better if this method creates entity cache on the fly?

            string aType = anEntityType.ToLower();
            List<string> anEntityList;

            if (aType == "system")
                anEntityList = this.getSubSystemList( aSystemPath );
            else
            {
                anEntityList = new List<string>();

                foreach(XmlNode aSystemNode in this.theEmlNode.ChildNodes)
                {
                    if (aSystemNode.Name != "system" || aSystemNode.Attributes["id"].Value != aSystemPath)
                        continue;
                    
                    foreach(XmlNode aChildNode in aSystemNode.ChildNodes)
                    {
                        if (aChildNode.Name == aType)
                            anEntityList.Add(aChildNode.Attributes["id"].Value);
                    }
                }
            }
            return anEntityList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <returns></returns>
        public List<string> getEntityPropertyList( string aFullID )
        {
            XmlNode anEntityNode = this.getEntityNode( aFullID );
            List<string> anEntityPropertyList = new List<string>();
            
            foreach(XmlNode aChildNode in anEntityNode.ChildNodes)
            {
                if (aChildNode.Name == "property")
                    anEntityPropertyList.Add(aChildNode.Attributes["name"].Value);
            }
            return anEntityPropertyList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullPNString"></param>
        /// <returns></returns>
        public string getEntityProperty(string aFullPNString )
        {
            string aType, aKey, aPropertyName;
            Util.ParseFullPN(aFullPNString, out aType, out aKey, out aPropertyName);

            string aFullID = aType + ":" + aKey;
            XmlNode anEntityPropertyNode = this.getEntityPropertyNode( aFullID, aPropertyName );

            return this.createValueList(anEntityPropertyNode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <param name="InfoStrings"></param>
        public void setEntityInfo(string aFullID, string InfoStrings )
        {
            XmlNode anEntityInfoElement = this.createInfoNode( InfoStrings );
            XmlNode aTargetNode = this.getEntityNode( aFullID );
            aTargetNode.AppendChild(anEntityInfoElement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <returns></returns>
        public string getEntityInfo( string aFullID )
        {
            XmlNode anEntityInfoNode = this.getEntityInfoNode(aFullID);

            return this.createValueList( anEntityInfoNode );
        }
        #endregion

        #region Cache manipulations
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <returns></returns>
        private XmlNode findInCache( string aFullID )
        {
            return this.entityNodeCache[ aFullID ];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <param name="aNode"></param>
        private void addToCache(string aFullID, XmlNode aNode )
        {
            this.entityNodeCache.Add(aFullID, aNode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        private void removeFromCache(string aFullID)
        {
            this.entityNodeCache.Remove(aFullID);
        }
        private void clearCache()
        {
            this.entityNodeCache.Clear();
        }

        private void reconstructCache()
        {
            this.clearCache();

            foreach(XmlNode aSystemNode in this.theEmlNode.ChildNodes)
            {
                if (aSystemNode.Name != "system")
                    continue;

                string aSystemPath = aSystemNode.Attributes["id"].Value;
                string aSystemFullID = convertSystemID2SystemFullID( aSystemPath );
                this.addToCache( aSystemFullID, aSystemNode );

                foreach(XmlNode aChildNode in aSystemNode.ChildNodes)
                {
                    string aType = aChildNode.Name.ToLower();

                    if ( aType == "Variable" || aType == "Process" )
                    {
                        string anID = aChildNode.Attributes["id"].Value;
                        string aFullID = aType + ':' + aSystemPath + ':' + anID;
                        this.addToCache(aFullID, aChildNode);
                    }
                }
            }
        }
        #endregion


        #region Utils
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aValueNode"></param>
        /// <returns></returns>
        private string createValueList(XmlNode aValueNode )
        {
            XmlNode aNode = aValueNode.FirstChild;
            XmlNodeType aNodeType = aNode.NodeType;
            string aValue = "";
            if (aNodeType == XmlNodeType.Text)
            {
                aValue = aNode.Value.Replace('\x0A', '\n');
                return aValue;
            }
            else if (aNodeType == XmlNodeType.Element)
            {
                foreach (XmlNode child in aValueNode.ChildNodes)
                {
                    aValue = aValue + this.createValueList(child);
                }
            }
            else
                throw new EcellException("unexpected error.");
            return aValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSystemPath"></param>
        /// <returns></returns>
        private List<string> getSubSystemList(string aSystemPath )
        {
            List<string> aSystemList = new List<string>();
            string[] aTargetPath = aSystemPath.Split( '/' );
            int aTargetPathLength = aTargetPath.Length;

            // if '' is given, return the root system ('/')
            if (aTargetPathLength == 1)
            {
                foreach(XmlNode aSystemNode in this.theEmlNode.ChildNodes)
                {
                    if( aSystemNode.Name == "system" &&
                         aSystemNode.Attributes["id"].Value == "/")
                    {
                        aSystemList.Add("/");
                    }
                }
                return aSystemList;
            }

            foreach (XmlNode aSystemNode in this.theEmlNode.ChildNodes)
            {
                if (aSystemNode.Name != "system")
                    continue;
                string sysKey, localID;
                Util.ParseSystemKey(aSystemNode.Attributes["id"].Value, out sysKey, out localID);
                if (aSystemPath != sysKey)
                    continue;
                aSystemList.Add(localID);
            }
            return aSystemList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <returns></returns>
        private XmlNode getEntityNode(string aFullID)
        {
            // first look up the cache
            try
            {
                return this.findInCache(aFullID);
            }
            catch (Exception)
            {
            }

            string aType, aSystemPath, anID;
            Util.ParseFullID(aFullID, out aType, out aSystemPath, out anID);

            if (aType == "System")
            {
                aSystemPath = joinSystemPath(aSystemPath, anID);
                return this.getSystemNode(aSystemPath);
            }

            XmlNode aSystemNode = this.getSystemNode(aSystemPath);

            foreach (XmlNode aChildNode in aSystemNode.ChildNodes)
            {
                if (aChildNode.Name.ToLower() == aType.ToLower() &&
                       aChildNode.Attributes["id"].Value == anID)
                {
                    this.addToCache(aFullID, aChildNode);
                    return aChildNode;
                }
            }
            throw new EcellException(string.Format("Entity [{0}] not found.", aFullID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSystemPath"></param>
        /// <returns></returns>
        public XmlNode getSystemNode( string aSystemPath )
        {

            string aFullID = convertSystemID2SystemFullID( aSystemPath );

            // first look up the cache
            try
            {
                return this.findInCache( aFullID );
            }
            catch(Exception)
            {
            }

            foreach(XmlNode aSystemNode in this.theEmlNode.ChildNodes)
            {
                if( aSystemNode.Name == "system" &&
                       aSystemNode.Attributes["id"].Value == aSystemPath)
                {
                    this.addToCache( aFullID, aSystemNode );
                    return aSystemNode;
                }
            }
            throw new EcellException(string.Format( "System [{0}] not found.", aFullID));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFullID"></param>
        /// <param name="aPropertyName"></param>
        /// <returns></returns>
        private XmlNode getEntityPropertyNode(string aFullID, string aPropertyName )
        {
            XmlNode anEntityNode = this.getEntityNode( aFullID );

            // what if multiple propety elements with the same name exist?
            foreach(XmlNode aChildNode in anEntityNode.ChildNodes)
            {
                if (aChildNode.Name == "property" &&
                       aChildNode.Attributes["name"].Value == aPropertyName)
                    return aChildNode;
            }
            throw new EcellException(string.Format("Property [{0}] not found.", aPropertyName));
        }

        private XmlNode getEntityInfoNode(string aFullID )
        {
            XmlNode anEntityNode = this.getEntityNode( aFullID );

            foreach(XmlNode aChildNode in anEntityNode.ChildNodes)
            {
                if (aChildNode.Name == "info")
                    return aChildNode;
            }
            throw new EcellException(string.Format("EntityInfo [{0}] not found.", aFullID));
        }
        #endregion

        #region Methods for Methods
        /// <summary>
        /// make an element
        /// </summary>
        /// <param name="aTagName"></param>
        /// <returns></returns>
        private XmlNode createElement(string aTagName )
        {
            return this.theDocument.CreateElement( aTagName );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPropertyName"></param>
        /// <param name="aValueList"></param>
        /// <returns></returns>
        private XmlNode createPropertyNode(string aPropertyName, List<string> aValueList )
        {
            XmlNode aPropertyElement = this.createElement( "property" );
            aPropertyElement.setAttribute( "name", aPropertyName );

            //map( aPropertyElement.AppendChild(
            //     map( this.createValueNode, aValueList ) )

            return aPropertyElement;
        }

        private XmlNode createValueNode(string aValue )
        {
            XmlNode aValueNode = this.createElement( "value" );

            //if (　type( aValue ) in ( TupleType, ListType ))    // vector value
            //{
            //    map( aValueNode.AppendChild,\
            //         map( self.__createValueNode, aValue ) )
            //}
            //else                                             // scaler value
            //{
            //    string aNormalizedValue =  aValue.replace( '\n', '\x0A' );

            //    XmlNode aValueData = this.theDocument.CreateTextNode( aNormalizedValue );
            //    aValueNode.AppendChild( aValueData );
            //}

            return aValueNode;
        }
        #endregion
    }
}
