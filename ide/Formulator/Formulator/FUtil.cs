// Formulator C# Library
// COPYRIGHT (C) 2006  MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Formulator
{
    class FUtil
    {
        // The data type of FNode.
        static public int NONE = -1;
        static public int INPUT = 0;
        static public int STRING = 1;
        static public int SPLIT = 2;
        static public int PLUS = 3;
        static public int MINUS = 4;
        static public int MULTI = 5;
        static public int RIGHT = 6;
        static public int LEFT = 7;
        static public int OINPUT = 8;
        static public int LOG = 9;
        static public int LOG10 = 10;
        static public int SQRT = 11;
        static public int EXP = 12;
        static public int CEIL = 13;
        static public int FLOOR = 14;
        static public int ABS = 15;
        static public int SIN = 16;
        static public int COS = 17;
        static public int TAN = 18;
        static public int ASIN = 19;
        static public int ACOS = 20;
        static public int ATAN = 21;

        static public int POW = 22;
        static public int IFNOT = 23;
        static public int IFAND = 24;
        static public int IFOR = 25;
        static public int IFXOR = 26;
        static public int IFEQ = 27;
        static public int IFNEQ = 28;
        static public int IFGT = 29;
        static public int IFLT = 30;
        static public int IFGEQ = 31;
        static public int IFLEQ = 32;

        static public int KAMMA = 33;


        // The width of drawing FNode.
        static public float INPUT_WIDTH = 10.0F;
        static public float LEFT_WIDTH = 10.0F;
        static public float RIGHT_WIDTH = 10.0F;
        static public float SPLIT_WIDTH = 10.0F;
        static public float PLUS_WIDTH = 10.0F;
        static public float MINUS_WIDTH = 10.0F;
        static public float MULTI_WIDTH = 10.0F;


        static public float LINE_HEIGHT = 20.0F;
        static public float HEIGHT_GAP = 5.0F;
        static public float WIDTH_GAP = 5.0F;
        static public float MARGIN = 10.0F;


        static public int XMAX = 100;
        static public int YMAX = 30;


        // The reserved function name.
        static public string STRLOG = "log";
        static public string STRLOG10 = "log10";
        static public string STRSQRT = "sqrt";
        static public string STREXP = "exp";
        static public string STRCEIL = "ceil";
        static public string STRFLOOR = "floor";
        static public string STRABS = "abs";
        static public string STRSIN = "sin";
        static public string STRCOS = "cos";
        static public string STRTAN = "tan";
        static public string STRASIN = "asin";
        static public string STRACOS = "acos";
        static public string STRATAN = "atan";

        static public string STRPOW = "pow";
        static public string STRNOT = "not";
        static public string STRAND = "and";
        static public string STROR = "or";
        static public string STRXOR = "xor";
        static public string STREQ = "eq";
        static public string STRNEQ = "neq";
        static public string STRGT = "gt";
        static public string STRLT = "lt";
        static public string STRGEQ = "geq";
        static public string STRLEQ = "leq";

        static public String STRKAMMA = ",";

        static public bool CheckReserveStrin(String s)
        {
            if (s.Equals(STRLOG)) return false;
            if (s.Equals(STRLOG10)) return false;
            if (s.Equals(STRSQRT)) return false;
            if (s.Equals(STREXP)) return false;
            if (s.Equals(STRCEIL)) return false;
            if (s.Equals(STRFLOOR)) return false;
            if (s.Equals(STRABS)) return false;
            if (s.Equals(STRSIN)) return false;
            if (s.Equals(STRCOS)) return false;
            if (s.Equals(STRTAN)) return false;
            if (s.Equals(STRASIN)) return false;
            if (s.Equals(STRACOS)) return false;
            if (s.Equals(STRATAN)) return false;
            if (s.Equals(STRPOW)) return false;
            if (s.Equals(STRNOT)) return false;
            if (s.Equals(STRAND)) return false;
            if (s.Equals(STROR)) return false;
            if (s.Equals(STRXOR)) return false;
            if (s.Equals(STREQ)) return false;
            if (s.Equals(STRNEQ)) return false;
            if (s.Equals(STRGT)) return false;
            if (s.Equals(STRLT)) return false;
            if (s.Equals(STRGEQ)) return false;
            if (s.Equals(STRLEQ)) return false;

            return true;
        }


    }
}
