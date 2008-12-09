using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Ecell.Objects;

namespace Ecell
{
    internal class EcellXmlReaderException: ApplicationException
    {
        public EcellXmlReaderException(string msg)
            : base(msg)
        {
        }

        public EcellXmlReaderException(string msg, Exception innerExc)
            : base(msg, innerExc)
        {
        }
    }

    internal class EcellXmlWriter
    {
        protected XmlTextWriter m_tx;

        /// <summary>
        /// Creates the "value" elements.
        /// </summary>
        /// <param name="ecellValue">The "EcellValue"</param>
        /// <param name="isElement">The flag whether the "Value" element add</param>
        protected void WriteValueElements(EcellValue ecellValue, bool isElement)
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
                foreach (object edge in (IEnumerable)ecellValue.Value)
                {
                    m_tx.WriteStartElement(Constants.xpathValue.ToLower());
                    foreach (object value in (IEnumerable)edge)
                    {
                        //this.WriteValueElements(childEcellValue, true);
                        m_tx.WriteElementString(
                            Constants.xpathValue.ToLower(),
                            null,
                            value.ToString());
                    }
                    m_tx.WriteEndElement();
                }
            }
            else
            {
                m_tx.WriteElementString(
                    Constants.xpathValue.ToLower(),
                    null,
                    (string)ecellValue);
            }
        }

        public EcellXmlWriter(XmlTextWriter tx)
        {
            m_tx = tx;
        }
    }

    internal class EcellXmlReader
    {
        /// <summary>
        /// Tests whether the element is null or empty.
        /// </summary>
        /// <param name="node">The checked element</param>
        /// <returns>false if the element is null or empty; true otherise</returns>
        protected bool IsValidNode(XmlNode node)
        {
            if (node == null || node.InnerText == null || node.InnerText.Length < 1)
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
        protected EcellValue GetValueList(XmlNode node)
        {
            List<EcellValue> ecellValueList = new List<EcellValue>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (!this.IsValidNode(childNode))
                {
                    throw new EcellXmlReaderException("Invalid value node found");
                }

                switch (childNode.NodeType)
                {
                case XmlNodeType.Text:
                    string value = Util.StripWhitespaces(childNode.Value);
                    if (value.Equals(XmlConvert.ToString(Double.PositiveInfinity)))
                    {
                        ecellValueList.Add(new EcellValue(Double.PositiveInfinity));
                    }
                    else
                    {
                        ecellValueList.Add(new EcellValue(value));
                    }
                    break;
                case XmlNodeType.Element:
                    if (!childNode.Name.Equals(Constants.xpathValue.ToLower()))
                    {
                        throw new EcellXmlReaderException(
                            String.Format(
                                "Element {0} found where {1} is expceted",
                                new object[] { childNode.Name, Constants.xpathValue.ToLower() }
                            )
                        );
                    }
                    ecellValueList.Add(GetValueList(childNode));
                    break;
                }
            }
            return ecellValueList.Count == 1 && !ecellValueList[0].IsList ? ecellValueList[0]: new EcellValue(ecellValueList);
        }
    }
}
