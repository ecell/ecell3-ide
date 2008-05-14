using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.Objects
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
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="l_modelID">model ID.</param>
        /// <param name="l_key">key.</param>
        /// <param name="l_type">type(="Variable").</param>
        /// <param name="l_class">class name.</param>
        /// <param name="l_data">properties.</param>
        public EcellText(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.ModelID = l_modelID;
            this.Key = l_key;
            this.Type = l_type;
            this.Classname = l_class;
            this.SetEcellDatas(l_data);
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
        #endregion

    }
}
