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
        /// <param name="modelID">model ID.</param>
        /// <param name="key">key.</param>
        /// <param name="type">type(="Variable").</param>
        /// <param name="classname">class name.</param>
        /// <param name="data">properties.</param>
        public EcellText(string modelID, string key,
            string type, string classname, List<EcellData> data)
            : base(modelID, key, type, classname, data)
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
                    return GetEcellValue(COMMENT).ToString();
                else
                    return null;
            }
            set
            {
                SetEcellValue(COMMENT, new EcellValue(value));
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
                    return (StringAlignment)(int)GetEcellValue(ALIGN);
                else
                    return StringAlignment.Near;
            }
            set
            {
                SetEcellValue(ALIGN, new EcellValue(value));
            }
        }
        #endregion

    }

}
