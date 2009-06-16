//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public static void ConvertToExpression(EcellObject obj)
        {
            if (!(obj is EcellProcess))
                return;
            EcellProcess process = (EcellProcess)obj;
            EcellProcess newProcess = null;

            string classname = process.Classname;

            switch(classname)
            {
                case ProcessConstants.ExpressionFluxProcess:
                    newProcess = process;
                    break;
                case ProcessConstants.ConstantFluxProcess:
                    newProcess = ConstantFlux2Expression(process);
                    break;
                case ProcessConstants.DecayFluxProcess:
                    newProcess = DecayFlux2Expression(process);
                    break;
                case ProcessConstants.MassActionFluxProcess:
                    newProcess = MassAction2Expression(process);
                    break;
                case ProcessConstants.PingPongBiBiFluxProcess:
                    newProcess = PingPongBiBiFlux2Expression(process);
                    break;
                case ProcessConstants.MichaelisUniUniFluxProcess:
                    newProcess = MichaelisUniUniFlux2Expression(process);
                    break;
                default:
                    throw new EcellException(string.Format("{0} is not supported.", classname));
            }

            obj = newProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static EcellProcess ConstantFlux2Expression(EcellProcess process)
        {
            EcellProcess newProcess = new EcellProcess(process.ModelID, process.Key, process.Type, ProcessConstants.ExpressionFluxProcess, new List<EcellData>());
            newProcess.ReferenceList = process.ReferenceList;
            newProcess.SetEcellValue("k", process.GetEcellValue("k"));
            newProcess.Expression = "k";
            newProcess.Layout = process.Layout;
            return newProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static EcellProcess DecayFlux2Expression(EcellProcess process)
        {
            EcellProcess newProcess = new EcellProcess(process.ModelID, process.Key, process.Type, ProcessConstants.ExpressionFluxProcess, new List<EcellData>());
            newProcess.ReferenceList = process.ReferenceList;
            newProcess.SetEcellValue("T", process.GetEcellValue("T"));
            newProcess.Expression = "( log ( 2 ) ) / ( T ) * S0.Value";
            newProcess.Layout = process.Layout;
            return newProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static EcellProcess MassAction2Expression(EcellProcess process)
        {
            EcellProcess newProcess = new EcellProcess(process.ModelID, process.Key, process.Type, ProcessConstants.ExpressionFluxProcess, new List<EcellData>());
            newProcess.ReferenceList = process.ReferenceList;
            newProcess.SetEcellValue("T", process.GetEcellValue("T"));
            newProcess.Expression = "S0.Value";
            newProcess.Layout = process.Layout;
            return newProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static EcellProcess MichaelisUniUniFlux2Expression(EcellProcess process)
        {
            EcellProcess newProcess = new EcellProcess(process.ModelID, process.Key, process.Type, ProcessConstants.ExpressionFluxProcess, new List<EcellData>());
            newProcess.ReferenceList = process.ReferenceList;
            newProcess.SetEcellValue("KmS", process.GetEcellValue("KmS"));
            newProcess.SetEcellValue("KmP", process.GetEcellValue("KmP"));
            newProcess.SetEcellValue("KcF", process.GetEcellValue("KcF"));
            newProcess.SetEcellValue("KcR", process.GetEcellValue("KcR"));
            newProcess.Expression = "( ( KcF * KmP * S0.MolerConc - KcR * KmS * P0.MolerConc ) * C0.Value ) / ( KmS * P0.MolerConc + KmP * S0.MolerConc + KmS * KmP )";
            newProcess.Layout = process.Layout;
            return newProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static EcellProcess PingPongBiBiFlux2Expression(EcellProcess process)
        {
            EcellProcess newProcess = new EcellProcess(process.ModelID, process.Key, process.Type, ProcessConstants.ExpressionFluxProcess, new List<EcellData>());
            newProcess.ReferenceList = process.ReferenceList;
            newProcess.SetEcellValue("KcF", process.GetEcellValue("KcF"));
            newProcess.SetEcellValue("KcR", process.GetEcellValue("KcR"));
            newProcess.SetEcellValue("Keq", process.GetEcellValue("Keq"));
            newProcess.SetEcellValue("KmS0", process.GetEcellValue("KmS0"));
            newProcess.SetEcellValue("KmS1", process.GetEcellValue("KmS1"));
            newProcess.SetEcellValue("KmP0", process.GetEcellValue("KmP0"));
            newProcess.SetEcellValue("KmP1", process.GetEcellValue("KmP1"));
            newProcess.SetEcellValue("KiS0", process.GetEcellValue("KiS0"));
            newProcess.SetEcellValue("KiP1", process.GetEcellValue("KiP1"));
            newProcess.Expression = "( KcF * KcR * C0.Value * ( S0.MolarConc * S1.MolarConc - P0.MolarConc * P1.MolarConc / Keq ) ) / ( KcR * KmS1 * S0.MolarConc + KcR * KmS0 * S1.MolarConc + KmP1 * P0.MolarConc * KcF / Keq + KmP0 * P1.MolarConc * KcF / Keq + KcR * S0.MolarConc * S1.MolarConc + KmP1 * S0.MolarConc * P0.MolarConc * KcF / Keq / KiS0 + P0.MolarConc * P1.MolarConc * KcF / Keq + KcR * KmS0 * S1.MolarConc * P1.MolarConc / KiP1 )";
            newProcess.Layout = process.Layout;
            return newProcess;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ProcessConstants
    {
        public const string ConstantFluxProcess = "ConstantFluxProcess";
        public const string DecayFluxProcess = "DecayFluxProcess";
        public const string ExpressionAlgebraicProcess = "ExpressionAlgebraicProcess";
        public const string ExpressionAssignmentProcess = "ExpressionAssignmentProcess";
        public const string ExpressionFluxProcess = "ExpressionFluxProcess";
        public const string GillespieProcess = "GillespieProcess";
        public const string GMAProcess = "GMAProcess";
        public const string MassActionFluxProcess = "MassActionFluxProcess";
        public const string MichaelisUniUniFluxProcess = "MichaelisUniUniFluxProcess";
        public const string PingPongBiBiFluxProcess = "PingPongBiBiFluxProcess";
        public const string PythonFluxProcess = "PythonFluxProcess";
        public const string PythonProcess = "PythonProcess";
        public const string QuasiDynamicFluxProcess = "QuasiDynamicFluxProcess";
        public const string SSystemProcess = "SSystemProcess";
        public const string TauLeapProcess = "TauLeapProcess";
    }
}
