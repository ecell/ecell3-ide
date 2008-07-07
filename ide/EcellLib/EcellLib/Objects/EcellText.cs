using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Ecell.Objects
{
    /// <summary>
    /// EcellText
    /// </summary>
    public class EcellText : EcellObject
    {
        /// <summary>
        /// Comment. The reserved name.
        /// </summary>
        public const string COMMENT = "Comment";
        /// <summary>
        /// Comment. The reserved name.
        /// </summary>
        public const string ALIGN = "Align";

        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="l_modelID">model ID.</param>
        /// <param name="l_key">key.</param>
        /// <param name="l_type">type(="Variable").</param>
        /// <param name="l_class">class name.</param>
        /// <param name="l_data">properties.</param>
        public EcellText(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
                : base(l_modelID, l_key, l_type, l_class, l_data)
        {
        }

        #region Accessors
        /// <summary>
        /// get /set the activity.
        /// </summary>
        public string Comment
        {
            get
            {
                if (IsEcellValueExists(COMMENT))
                    return GetEcellValue(COMMENT).CastToString();
                else
                    return null;
            }
            set
            {
                if (IsEcellValueExists(COMMENT))
                    GetEcellValue(COMMENT).Value = value;
                else
                    AddEcellValue(COMMENT, new EcellValue(value));
            }
        }
        /// <summary>
        /// get /set the activity.
        /// </summary>
        public StringAlignment Alignment
        {
            get
            {
                if (IsEcellValueExists(ALIGN))
                    return (StringAlignment)GetEcellValue(ALIGN).CastToInt();
                else
                    return StringAlignment.Near;
            }
            set
            {
                if (IsEcellValueExists(ALIGN))
                    GetEcellValue(ALIGN).Value = (int)value;
                else
                    AddEcellValue(ALIGN, new EcellValue((int)value));
            }
        }
        #endregion

    }

}
