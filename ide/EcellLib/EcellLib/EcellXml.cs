using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;
using Ecell.Objects;
using Ecell.Exceptions;
using System.Collections;

namespace Ecell
{
    /// <summary>
    /// Exception class to read the xml.
    /// </summary>
    public class EcellXmlReaderException : EcellException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="msg">the error message.</param>
        public EcellXmlReaderException(string msg)
            : base(msg)
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="msg">the error message.</param>
        /// <param name="innerExc">the inner exception.</param>
        public EcellXmlReaderException(string msg, Exception innerExc)
            : base(msg, innerExc)
        {
        }
    }

    /// <summary>
    /// Writer for Ecell XML.
    /// </summary>
    public class EcellXmlWriter
    {
        /// <summary>
        /// XmlTextWriter
        /// </summary>
        protected XmlTextWriter m_tx;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tx"></param>
        public EcellXmlWriter(XmlTextWriter tx)
        {
            m_tx = tx;
        }
        /// <summary>
        /// Close writer.
        /// </summary>
        public virtual void Close()
        {
            m_tx.Close();
        }

        /// <summary>
        /// Creates the "value" elements.
        /// </summary>
        /// <param name="ecellValue">The "EcellValue"</param>
        protected void WriteValueElements(EcellValue ecellValue)
        {
            Debug.Assert(ecellValue != null);

            if (ecellValue.IsDouble)
            {
                if (Double.IsInfinity((double)ecellValue))
                {
                    m_tx.WriteElementString(
                        Constants.xpathValue.ToLower(),
                        null,
                        XmlConvert.ToString(Double.PositiveInfinity));
                }
                else if ((double)ecellValue == Double.MaxValue)
                {
                    m_tx.WriteElementString(
                        Constants.xpathValue.ToLower(),
                        null,
                        XmlConvert.ToString(Double.MaxValue));
                }
                else
                {
                    m_tx.WriteElementString(
                        Constants.xpathValue.ToLower(),
                        null,
                        ((double)ecellValue).ToString());
                }
            }
            else if (ecellValue.IsInt)
            {
                m_tx.WriteElementString(
                    Constants.xpathValue.ToLower(),
                    null,
                    ((int)ecellValue).ToString());
            }
            else if (ecellValue.IsList)
            {
                WriteValueList((List<object>)ecellValue.Value);
            }
            else
            {
                m_tx.WriteElementString(
                    Constants.xpathValue.ToLower(),
                    null,
                    (string)ecellValue);
            }
        }

        private void WriteValueList(List<object> list)
        {
            foreach (object obj in list)
            {
                if (obj is List<object>)
                {
                    m_tx.WriteStartElement(Constants.xpathValue.ToLower());
                    WriteValueList((List<object>)obj);
                    m_tx.WriteEndElement();
                }
                else
                {
                    m_tx.WriteElementString(
                        Constants.xpathValue.ToLower(),
                        null,
                        obj.ToString());
                }
            }
        }
    }

    public class EcellXmlReader
    {
        /// <summary>
        /// Tests whether the element is null or empty.
        /// </summary>
        /// <param name="node">The checked element</param>
        /// <returns>false if the element is null or empty; true otherise</returns>
        protected bool IsValidNode(XmlNode node)
        {
            if (node == null || node.InnerText == null || node.InnerText.Length < 1 )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads nested "value" elements.
        /// </summary>
        /// <param name="node">The "property" element that is parent element of "value" elements</param>
        /// <returns>The "EcellValue"</returns>
        protected EcellValue ParseEcellValue(XmlNode node)
        {
            List<object> valueList = new List<object>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (!this.IsValidNode(childNode))
                {
                    throw new EcellXmlReaderException("Invalid value node found :" + node.Name);
                }

                switch (childNode.NodeType)
                {
                case XmlNodeType.Text:
                    string value = childNode.Value.Trim();
                    int i;
                    double d;
                    if (int.TryParse(value, out i))
                    {
                        valueList.Add(i);
                    }
                    else if (double.TryParse(value, out d))
                    {
                        valueList.Add(d);
                    }
                    else
                    {
                        valueList.Add(value);
                    }
                    break;
                case XmlNodeType.Element:
                    if (!childNode.Name.Equals(Constants.xpathValue.ToLower()))
                    {
                        throw new EcellXmlReaderException(
                            string.Format(
                                "Element {0} found where {1} is expceted",
                                childNode.Name,
                                Constants.xpathValue.ToLower())
                        );
                    }
                    valueList.Add(ParseEcellValue(childNode).Value);
                    break;
                }
            }

            // Create new EcellValue object.
            object obj = null;
            if (valueList.Count <= 0)
                obj = valueList;
            else if ((valueList.Count == 1) && !(valueList[0] is List<object>))
                obj = valueList[0];
            else
                obj = valueList;

            return new EcellValue(obj);
        }
    }
}
