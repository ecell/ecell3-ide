using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Objects;

namespace Ecell.SBML
{
    public class ExpressionParser
    {
        private static List<EcellReference> aVariableReferenceList;
        private static string aReactionPath;
        private static bool aDelayFlag;
        private static List<string> ID_Namespace;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anExpression"></param>
        /// <param name="aVariableReferenceList"></param>
        /// <param name="aReactionPath"></param>
        /// <param name="ID_Namespace"></param>
        /// <returns></returns>
        public static object[] convertExpression(string anExpression,
            List<EcellReference> aVariableReferenceList,
            string aReactionPath,
            List<string> ID_Namespace)
        {
            aVariableReferenceList = aVariableReferenceList;
            aReactionPath = aReactionPath;
            aDelayFlag = false;
            ID_Namespace = ID_Namespace;

            Lexer aLexer = lex.lex(lextab = "expressionlextab");
            aParser = yacc.yacc( optimize=1, tabmodule="expressionparsetab" );

            return new object[] { aParser.parse( anExpression, lexer=aLexer, debug=debug ),
                     aDelayFlag };
        }
    }

    public class Lexer
    {

    }
}
