//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Class to calculate the value from the formulator.
    /// This class refers to the program of MSDN.
    /// </summary>
    public class Calculator
    {
        private const int FOUND = 0;
        private const int NOTFOUND = 1;
        private string m_formulator;
        private double m_result;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Calculator()
        {
            m_formulator = "";
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="form">the formulator.</param>
        public Calculator(string form)
        {
            m_formulator = form;
        }

        /// <summary>
        /// get / set the formulator.
        /// </summary>
        public string Formulator
        {
            get { return this.m_formulator; }
            set { this.m_formulator = value; }
        }

        /// <summary>
        /// Calculate the value of formulator.
        /// </summary>
        /// <returns>the value of formulator.</returns>
        public double Calculate()
        {
            m_result = 0.0;
            CalculateData(ref m_formulator, 0);
            return m_result;
        }

        private void CalculateData(ref string sInputData, int iStart)
        {
            int[] iCount = new int[2];	
            int i, iCaseflag = FOUND, iEnd = 0;	
            Regex rSet;		
            Match mSearch;	

            if (sInputData == null)
                return;

            rSet = new Regex("[\\(\\)\\+\\-/\\*]", RegexOptions.IgnoreCase | RegexOptions.Compiled);	

            if (iStart == 0)
            {
                mSearch = rSet.Match(sInputData);	

                if (mSearch.Success == true && mSearch.Groups[0].Index != 0)
                    iCount[0] = mSearch.Groups[0].Index;	
                else
                {
                    m_result = Convert.ToDouble(sInputData);
                    sInputData = null;
                    return;
                }

                mSearch = mSearch.NextMatch();

                if (mSearch.Groups[0].Index - iCount[0] == 1 && sInputData[mSearch.Groups[0].Index] != '(')	// マイナスの数値の場合わけ
                    mSearch = mSearch.NextMatch();	

                if (mSearch.Success)
                    iCount[1] = mSearch.Groups[0].Index;	
                else
                    iCaseflag = NOTFOUND;	

            }
            else	
            {
                mSearch = rSet.Match(sInputData);

                while (mSearch.Groups[0].Index != iStart)
                {
                    if (sInputData[0] == '(' && iStart == 1)
                    {
                        iStart = 0;
                        break;
                    }
                    mSearch = mSearch.NextMatch();
                }

                for (i = 0; i < 2; i++)
                {
                    mSearch = mSearch.NextMatch();	

                    if (i == 0)
                    {
                        if (sInputData[iStart + 1] == '-')	
                            mSearch = mSearch.NextMatch();
                    }
                    else
                    {
                        if (mSearch.Groups[0].Index - iCount[0] == 1 && sInputData[mSearch.Groups[0].Index] != '(')	// マイナスの数値の場合わけ
                            mSearch = mSearch.NextMatch();	
                    }

                    if (mSearch.Success)
                        iCount[i] = mSearch.Groups[0].Index;
                    else
                        iCaseflag = NOTFOUND;
                }
            }

            if (iCaseflag == FOUND)	
                iEnd = iCount[1];
            else			
                iEnd = sInputData.Length;

            if (sInputData[iCount[0]] == '(')
            {
                if (sInputData[iCount[1]] == ')')
                {
                    sInputData = CalculateDelete(sInputData, iCount[0], iCount[1]);	
                    CalculateData(ref sInputData, 0);	
                }

                if (iCount[0] == 0)			
                    iCount[0] = 1;

                CalculateData(ref sInputData, iCount[0]);
            }
            else if (sInputData[iCount[0]] == '+' || sInputData[iCount[0]] == '-')	
            {
                if ((sInputData[iCount[0] - 1] != 'e' && sInputData[iCount[0] - 1] != 'E') &&
                    (sInputData[iCount[1]] == '+' || sInputData[iCount[1]] == '-' 
                    || sInputData[iCount[1]] == ')' || iCaseflag == NOTFOUND))
                    CalculateSubData(ref sInputData, iStart, iCount[0], iEnd);
                else
                    CalculateData(ref sInputData, iCount[0]);	
            }
            else if (sInputData[iCount[0]] == '*' || sInputData[iCount[0]] == '/')
            {
                if (sInputData[iCount[1]] != '(' || iCaseflag == NOTFOUND)
                    CalculateSubData(ref sInputData, iStart, iCount[0], iEnd);	
                else
                    CalculateData(ref sInputData, iCount[0]);	
            }
        }

        private void CalculateSubData(ref string sInputData, int iStart, int iMiddle, int iEnd)
        {
            StringBuilder sbTempData = new StringBuilder(sInputData);
            string sTemp, sTemp1, sTemp2, sSearch;	
            double dLeft = 0, dRight = 0, dResult = 0;	

            if (iStart != 0)
                iStart = iStart + 1;

            sTemp1 = sInputData.Substring(iStart, iMiddle - iStart);
            sTemp2 = sInputData.Substring(iMiddle + 1, iEnd - iMiddle - 1);

            if (sInputData[0] == '(' && iStart == 0)
                sTemp1 = sTemp1.Remove(0, 1);	

            sSearch = sTemp1 + sInputData[iMiddle] + sTemp2;

            dLeft = Convert.ToDouble(sTemp1);
            dRight = Convert.ToDouble(sTemp2);	

            switch (sInputData[iMiddle])
            {
                case '+':
                    dResult = dLeft + dRight;
                    break;
                case '-':
                    dResult = dLeft - dRight;
                    break;
                case '*':
                    dResult = dLeft * dRight;
                    break;
                case '/':
                    dResult = dLeft / dRight;
                    break;
                default:
                    break;
            }

            sTemp = Convert.ToString(dResult);		

            if (sInputData[0] == '(' && iStart == 0)	
                iStart = 1;

            sbTempData = sbTempData.Replace(sSearch, sTemp, iStart, sSearch.Length);	
            sInputData = sbTempData.ToString(0, sbTempData.Length);	

            CalculateData(ref sInputData, 0);
        }

        private string CalculateDelete(string sInputData, int iCount1, int iCount2)
        {
            sInputData = sInputData.Remove(iCount1, 1);		
            sInputData = sInputData.Remove(iCount2 - 1, 1);	
            return sInputData;
        }

        private bool CalculateFilter(ref string sInputData)
        {
            int iLength = 0, iCount1 = 0, iCount2 = 0, iFlag = 0;
            Regex rSet1, rSet2, rSet3;			
            Match mSearch1, mSearch2, mSearch3;	

            iLength = sInputData.Length;	

            if (sInputData[0] != '(' && Char.IsNumber(sInputData[0]) == false)	
                return false;

            if (sInputData[iLength - 1] != ')' && Char.IsNumber(sInputData[iLength - 1]) == false)	
                return false;

            rSet1 = new Regex("/0", RegexOptions.IgnoreCase | RegexOptions.Compiled);	
            mSearch1 = rSet1.Match(sInputData);		

            if (mSearch1.Success == true)
                return false;	

            rSet2 = new Regex("[\\(\\)]", RegexOptions.IgnoreCase | RegexOptions.Compiled);	
            for (mSearch2 = rSet2.Match(sInputData); mSearch2.Success; mSearch2 = mSearch2.NextMatch())	
            {
                if (sInputData[mSearch2.Groups[0].Index] == '(')
                    iCount1 = iCount1 + 1;	
                else
                    iCount2 = iCount2 + 1;	

                if (iCount1 - iCount2 < 0)	
                    return false;	
            }

            if (iCount1 != iCount2)	
                return false;	

            iCount1 = 0;	
            iCount2 = 0;	

            rSet3 = new Regex("[\\+\\-\\*/]", RegexOptions.IgnoreCase | RegexOptions.Compiled);	
            for (mSearch3 = rSet3.Match(sInputData); mSearch3.Success; mSearch3 = mSearch3.NextMatch())
            {
                if (iFlag == 0)
                {
                    iCount1 = mSearch3.Groups[0].Index;	
                    iFlag = 1;
                }
                else
                {
                    iCount2 = mSearch3.Groups[0].Index;	
                    if (Math.Abs(iCount2 - iCount1) == 1)	
                        return false;	
                    iFlag = 0;
                }
            }

            if (Math.Abs(iCount2 - iCount1) == 1 && iCount2 != 0)
                return false;	

            return true;

        }
    }
}
