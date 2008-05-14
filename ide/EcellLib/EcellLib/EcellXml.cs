using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;
using EcellLib.Objects;

namespace EcellLib
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
        /// <param name="l_ecellValue">The "EcellValue"</param>
        /// <param name="l_isElement">The flag whether the "Value" element add</param>
        protected void WriteValueElements(EcellValue l_ecellValue, bool l_isElement)
        {
            Debug.Assert(l_ecellValue != null);

            if (l_ecellValue.IsDouble())
            {
                if (Double.IsInfinity(l_ecellValue.CastToDouble()))
                {
                    m_tx.WriteElementString(
                        Constants.xpathValue.ToLower(),
                        null,
                        XmlConvert.ToString(Double.PositiveInfinity));
                }
                else if (l_ecellValue.CastToDouble() == Double.MaxValue)
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
                        l_ecellValue.CastToDouble().ToString());
                }
            }
            else if (l_ecellValue.IsInt())
            {
                m_tx.WriteElementString(
                    Constants.xpathValue.ToLower(),
                    null,
                    l_ecellValue.CastToInt().ToString());
            }
            else if (l_ecellValue.IsList())
            {
                if (l_ecellValue.CastToList() == null || l_ecellValue.CastToList().Count <= 0)
                {
                    return;
                }
                if (l_isElement)
                {
                    m_tx.WriteStartElement(Constants.xpathValue.ToLower());
                    foreach (EcellValue l_childEcellValue in l_ecellValue.CastToList())
                    {
                        this.WriteValueElements(l_childEcellValue, true);
                    }
                    m_tx.WriteEndElement();
                }
                else
                {
                    foreach (EcellValue l_childEcellValue in l_ecellValue.CastToList())
                    {
                        this.WriteValueElements(l_childEcellValue, true);
                    }
                }
            }
            else
            {
                m_tx.WriteElementString(
                    Constants.xpathValue.ToLower(),
                    null,
                    l_ecellValue.CastToString());
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
        /// <param name="l_node">The checked element</param>
        /// <returns>false if the element is null or empty; true otherise</returns>
        protected bool IsValidNode(XmlNode l_node)
        {
            if (l_node == null || l_node.InnerText == null || l_node.InnerText.Length < 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads nested "value" elements.
        /// </summary>
        /// <param name="l_node">The "property" element that is parent element of "value" elements</param>
        /// <returns>The "EcellValue"</returns>
        protected EcellValue GetValueList(XmlNode l_node)
        {
            List<EcellValue> l_ecellValueList = new List<EcellValue>();
            foreach (XmlNode l_childNode in l_node.ChildNodes)
            {
                if (!this.IsValidNode(l_childNode))
                {
                    throw new EcellXmlReaderException("Invalid value node found");
                }

                switch (l_childNode.NodeType)
                {
                case XmlNodeType.Text:
                    string l_value = Util.StripWhitespaces(l_childNode.Value);
                    if (l_value.Equals(XmlConvert.ToString(Double.PositiveInfinity)))
                    {
                        l_ecellValueList.Add(new EcellValue(Double.PositiveInfinity));
                    }
                    else
                    {
                        l_ecellValueList.Add(new EcellValue(l_value));
                    }
                    break;
                case XmlNodeType.Element:
                    if (!l_childNode.Name.Equals(Constants.xpathValue.ToLower()))
                    {
                        throw new EcellXmlReaderException(
                            String.Format(
                                "Element {0} found where {1} is expceted",
                                new object[] { l_childNode.Name, Constants.xpathValue.ToLower() }
                            )
                        );
                    }
                    l_ecellValueList.Add(GetValueList(l_childNode));
                    break;
                }
            }
            return new EcellValue(l_ecellValueList);
        }
    }
}
