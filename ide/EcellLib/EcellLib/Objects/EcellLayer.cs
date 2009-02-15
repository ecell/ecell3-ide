using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Ecell.Exceptions;
using System.Collections;

namespace Ecell.Objects
{
    /// <summary>
    /// EcellLayer
    /// </summary>
    public class EcellLayer : ICloneable
    {
        #region Constant
        /// <summary>
        /// Name of Layers value.
        /// </summary>
        public const string Layers = "Layers";
        /// <summary>
        /// Parser for layer string.
        /// </summary>
        private static Regex layerParser = new Regex("\"(?<name>.+)\",(\"|.*)(?<visible>\\d+)(\"|.*)");
        /// <summary>
        /// Parser for layer list string.
        /// </summary>
        private static Regex stringParser = new Regex("\\((?<layer>.+?)\\)");
        #endregion

        #region Fields
        /// <summary>
        /// Name of this layer.
        /// </summary>
        private string m_name;
        /// <summary>
        /// Visibility of this layer.
        /// Visible = 1, Invisible = 0.
        /// </summary>
        private int m_visible;
        #endregion

        #region Constractors
        /// <summary>
        /// Constructor.
        /// </summary>
        public EcellLayer()
        {
            m_name = null;
            m_visible = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="visible"></param>
        public EcellLayer(string name, bool visible)
        {
            this.m_name = name;
            this.Visible = visible;
        }
        
        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="str">string.</param>
        public EcellLayer(string str)
        {
            // Check null.
            if (string.IsNullOrEmpty(str))
                throw new EcellException("EcellLayer Constructor does not arrow empty string");

            Match m = layerParser.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_visible = Convert.ToInt32(m.Groups["visible"].Value);
                return;
            }
            throw new EcellException("EcellLayer parsing error:[" + str + "]");
        }

        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="obj">EcellValue</param>
        public EcellLayer(object obj)
        {
            if (obj == null)
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "value"));
            if (!(obj is List<object>))
                throw new EcellException("EcellLayer parsing error:[" + obj.ToString() + "]");

            List<object> list = (List<object>)obj;
            if(list.Count != 2)
                throw new EcellException("EcellLayer parsing error:[" + obj.ToString() + "]");

            this.m_name = (string)list[0];
            this.m_visible = (int)list[1];
        }

        #endregion

        #region Accessors
        /// <summary>
        ///The name of this EcellReference.
        /// get / set.
        /// </summary>
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// The full ID of connecting variable.
        /// get / set.
        /// </summary>
        public bool Visible
        {
            get { return this.m_visible == 1; }
            set
            { 
                if (value)
                    this.m_visible = 1;
                else
                    this.m_visible = 0;
            }
        }
        #endregion

        #region Converters
        /// <summary>
        /// Get the list of reference from string.
        /// </summary>
        /// <param name="str">string.</param>
        /// <returns>the list of reference.</returns>
        public static List<EcellLayer> ConvertFromString(string str)
        {
            List<EcellLayer> list = new List<EcellLayer>();
            if (str == null || str == "")
                return list;
            string text = str.Substring(1);
            text = text.Substring(0, text.Length - 1);
            MatchCollection coll = stringParser.Matches(text);

            IEnumerator iter = coll.GetEnumerator();
            while (iter.MoveNext())
            {
                Match match = (Match)iter.Current;
                EcellLayer er = new EcellLayer(match.Groups["layer"].Value);
                list.Add(er);
            }
            return list;
        }

        /// <summary>
        /// Get the list of layer from EcellValue "Layers".
        /// </summary>
        /// <param name="layers">VariableReferenceList.</param>
        /// <returns>the list of EcellReference.</returns>
        public static List<EcellLayer> ConvertFromEcellValue(EcellValue layers)
        {
            List<EcellLayer> list = new List<EcellLayer>();
            if (layers == null || !layers.IsList)
                return list;

            List<object> layerList = (List<object>)layers.Value;
            if (layerList == null || layerList.Count == 0)
                return list;
            foreach (object value in layerList)
            {
                EcellLayer er = new EcellLayer(value);
                list.Add(er);
            }
            return list;
        }

        /// <summary>
        /// Get the EcellValue from LayerList.
        /// </summary>
        /// <param name="layerList">LayerList.</param>
        /// <returns>the list of EcellReference.</returns>
        public static EcellValue ConvertToEcellValue(IEnumerable<EcellLayer> layerList)
        {
            List<object> list = new List<object>();
            if (layerList == null)
                return new EcellValue(list);

            foreach (EcellLayer layer in layerList)
            {
                List<object> values = new List<object>();
                values.Add(layer.m_name);
                values.Add(layer.m_visible);
                list.Add(values);
            }
            return new EcellValue(list);
        }
        #endregion

        #region ICloneable メンバ
        /// <summary>
        /// Create a copy of this EcellLayer.
        /// </summary>
        /// <returns>object</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Create a copy of this EcellValue.
        /// </summary>
        /// <returns>EcellValue</returns>
        public EcellLayer Clone()
        {
            EcellLayer el = new EcellLayer(
                this.Name,
                this.Visible);

            return el;
        }

        #endregion

        #region Inherited Method
        /// <summary>
        /// Compare to another EcellReference.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is EcellLayer))
                return false;

            EcellLayer el = (EcellLayer)obj;
            if (!el.Name.Equals(m_name))
                return false;
            if (!el.Visible.Equals(this.Visible))
                return false;
            return true;
        }

        /// <summary>
        /// Get Hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hash = m_visible.GetHashCode();
            if(!string.IsNullOrEmpty(m_name))
                hash = hash ^ m_name.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Get string of object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "(\"" + m_name + "\", " + m_visible.ToString() + ")";
            return str;
        }
        #endregion

    }
}
