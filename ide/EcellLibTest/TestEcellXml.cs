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
using System.Reflection;
using System.Xml;
using Ecell.Objects;
using System.IO;

namespace Ecell
{

    /// <summary>
    /// TestEcellXml
    /// </summary>
    [TestFixture()]
    public class TestEcellXml
    {
        /// <summary>
        /// TestIsValidNode
        /// </summary>
        [Test()]
        public void TestIsValidNode()
        {
            EcellXmlReader reader = new EcellXmlReader();
            Assert.IsNotNull(reader, "Constructor of type, EcellXmlReader failed to create instance.");
            Type type = reader.GetType();
            MethodInfo methodInfo = type.GetMethod("IsValidNode", BindingFlags.NonPublic | BindingFlags.Instance);

            bool expected = true;
            XmlDocument eml = new XmlDocument();
            eml.Load("c:/temp/rbc.eml");
            XmlNode node = eml.ChildNodes[0];
            bool result = (bool)methodInfo.Invoke(reader, new object[] { node });

            Assert.AreEqual(expected, result, "IsValidNode method returned unexpected value.");

            node = eml.CreateComment("");
            expected = false;
            result = (bool)methodInfo.Invoke(reader, new object[] { node });
            Assert.AreEqual(expected, result, "IsValidNode method returned unexpected value.");

            result = (bool)methodInfo.Invoke(reader, new object[] { null });
            Assert.AreEqual(expected, result, "IsValidNode method returned unexpected value.");

        }
        
        /// <summary>
        /// TestParseEcellValue
        /// </summary>
        [Test()]
        public void TestParseEcellValue()
        {
            EcellXmlReader reader = new EcellXmlReader();
            Assert.IsNotNull(reader, "Constructor of type, EcellXmlReader failed to create instance.");
            Type type = reader.GetType();
            MethodInfo readerMethod = type.GetMethod("ParseEcellValue", BindingFlags.NonPublic | BindingFlags.Instance);

            XmlDocument eml = new XmlDocument();

            eml.LoadXml("<property name=\"Value\"><value>1E-15</value></property>");
            XmlNode node = eml.DocumentElement;
            EcellValue value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });
            double expected = 1e-15;
            Assert.IsTrue(value.IsDouble, "IsValidNode method returned unexpected value.");
            Assert.AreEqual(expected, (double)value.Value, "IsValidNode method returned unexpected value.");

            eml.LoadXml("<property name=\"Value\"><value>+∞</value></property>");
            node = eml.DocumentElement;
            value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });
            expected = double.PositiveInfinity;
            Assert.IsTrue(value.IsDouble, "IsValidNode method returned unexpected value.");
            Assert.AreEqual(expected, (double)value.Value, "IsValidNode method returned unexpected value.");

            eml.LoadXml("<property name=\"Value\"><value>-∞</value></property>");
            node = eml.DocumentElement;
            value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });
            expected = double.NegativeInfinity;
            Assert.IsTrue(value.IsDouble, "IsValidNode method returned unexpected value.");
            Assert.AreEqual(expected, (double)value.Value, "IsValidNode method returned unexpected value.");

            eml.LoadXml("<property name=\"Value\"><value>-11</value></property>");
            node = eml.DocumentElement;
            value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });
            Assert.IsTrue(value.IsInt, "IsValidNode method returned unexpected value.");
            Assert.AreEqual(-11, (int)value.Value, "IsValidNode method returned unexpected value.");

            eml.LoadXml("<property name=\"Value\"><value>Test</value></property>");
            node = eml.DocumentElement;
            value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });
            Assert.IsTrue(value.IsString, "IsValidNode method returned unexpected value.");
            Assert.AreEqual("Test", (string)value.Value, "IsValidNode method returned unexpected value.");

            eml.LoadXml("<property name=\"VariableReferenceList\"><value><value>S0</value><value>Variable:/cell:LacI</value><value>-1</value><value>0</value></value></property>");
            node = eml.DocumentElement;
            value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });
            Assert.IsTrue(value.IsList, "IsValidNode method returned unexpected value.");
            List<object> list = (List<object>)value.Value;
            Assert.IsNotEmpty(list, "IsValidNode method returned unexpected value.");

            List<EcellReference> erList = EcellReference.ConvertFromEcellValue(value);
            EcellReference er = erList[0];
            Assert.AreEqual("S0", er.Name, "Name is unexpected value.");
            Assert.AreEqual("Variable:/cell:LacI", er.FullID, "FullID is unexpected value.");
            Assert.AreEqual(-1, er.Coefficient, "Coefficient is unexpected value.");
            Assert.AreEqual(0, er.IsAccessor, "IsAccessor is unexpected value.");

            try
            {
                eml.LoadXml("<property name=\"Value\" />");
                node = eml.DocumentElement;
                value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });

                Assert.Fail("IsValidNode method failed to throw exception.");
            }
            catch (Exception)
            {
            }

            try
            {
                eml.LoadXml("<property name=\"Value\"><Value/></property>");
                node = eml.DocumentElement;
                value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });

                Assert.Fail("IsValidNode method failed to throw exception.");
            }
            catch (Exception)
            {
            }

            try
            {
                eml.LoadXml("<property name=\"VariableReferenceList\"><hoge><value>S0</value><value>Variable:/cell:LacI</value><value>-1</value><value>0</value></hoge></property>");
                node = eml.DocumentElement;
                value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });

                Assert.Fail("IsValidNode method failed to throw exception.");
            }
            catch (Exception)
            {
            }
        }

                
        /// <summary>
        /// TestWriteValueElements
        /// </summary>
        [Test()]
        public void TestWriteValueElements()
        {
            EcellXmlReader reader = new EcellXmlReader();
            Assert.IsNotNull(reader, "Constructor of type, EcellXmlReader failed to create instance.");
            Type type = reader.GetType();
            MethodInfo readerMethod = type.GetMethod("ParseEcellValue", BindingFlags.NonPublic | BindingFlags.Instance);

            XmlDocument eml = new XmlDocument();
            eml.LoadXml("<property name=\"VariableReferenceList\"><value><value>S0</value><value>Variable:/cell:LacI</value><value>-1</value><value>0</value></value></property>");
            XmlNode node = eml.DocumentElement;
            EcellValue value = (EcellValue)readerMethod.Invoke(reader, new object[] { node });

            string filename = "c:/temp/test.xml";
            XmlTextWriter tx = new XmlTextWriter(filename, Encoding.UTF8);
            EcellXmlWriter writer = new EcellXmlWriter(tx);
            Assert.IsNotNull(writer, "Constructor of type, EcellXmlWriter failed to create instance.");
            type = writer.GetType();
            MethodInfo writerMethod = type.GetMethod("WriteValueElements", BindingFlags.NonPublic | BindingFlags.Instance);

            // int
            tx.WriteStartElement("Value");
            writerMethod.Invoke(writer, new object[] { new EcellValue(10) });

            // double
            writerMethod.Invoke(writer, new object[] { new EcellValue(0.01) });

            // double max
            writerMethod.Invoke(writer, new object[] { new EcellValue(double.MaxValue) });

            // double infinity
            writerMethod.Invoke(writer, new object[] { new EcellValue(double.PositiveInfinity) });

            // string
            writerMethod.Invoke(writer, new object[] { new EcellValue("Text") });
            tx.WriteEndElement();
            tx.Close();

            eml.LoadXml("<value><value>10</value><value>0.01</value><value>1.7976931348623157E+308</value><value>INF</value><value>Text</value></value>");
            node = eml.DocumentElement;

            eml.Load(filename);
            XmlNode nodeCopy = eml.DocumentElement;
            Assert.AreEqual(node.InnerText, nodeCopy.InnerText, "EcellXmlWriter method returned unexpected value.");

            // List
            tx = new XmlTextWriter(filename, Encoding.UTF8);
            writer = new EcellXmlWriter(tx);
            writerMethod.Invoke(writer, new object[] { value });
            tx.Close();
            writer = null;

            eml.LoadXml("<value><value>S0</value><value>Variable:/cell:LacI</value><value>-1</value><value>0</value></value>");
            node = eml.DocumentElement;

            eml.Load(filename);
            nodeCopy = eml.DocumentElement;
            Assert.AreEqual(node.InnerText, nodeCopy.InnerText, "EcellXmlWriter method returned unexpected value.");

            File.Delete(filename);
        }
        
        /// <summary>
        /// TestExceptions
        /// </summary>
        [Test()]
        public void TestExceptions()
        {
            EcellXmlReaderException ex = new EcellXmlReaderException("");
            Assert.IsNotNull(ex, "Constructor of type, EcellXmlReaderException failed to create instance.");

            ex = new EcellXmlReaderException("", new Exception());
            Assert.IsNotNull(ex, "Constructor of type, EcellXmlReaderException failed to create instance.");
        }
    }
}
