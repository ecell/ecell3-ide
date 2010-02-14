//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
    /// Process converter class.
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
            // Convert
            switch(classname)
            {
                case ProcessConstants.ExpressionFluxProcess:
                case ProcessConstants.ExpressionAlgebraicProcess:
                case ProcessConstants.ExpressionAssignmentProcess:
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
            // Reset
            process.Classname = newProcess.Classname;
            process.Expression = newProcess.Expression;
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
            newProcess.SetEcellValue("k", process.GetEcellValue("k"));
            newProcess.Expression = "k * S0.Value";
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
    /// Process convert constants.
    /// </summary>
    public class ProcessConstants
    {
        /// <summary>
        /// Reserved string ConstantFluxProcess.
        /// </summary>
        public const string ConstantFluxProcess = "ConstantFluxProcess";
        /// <summary>
        /// Reserved string DecayFluxProcess.
        /// </summary>
        public const string DecayFluxProcess = "DecayFluxProcess";
        /// <summary>
        /// Reserved string ExpressionAlgebraicProcess.
        /// </summary>
        public const string ExpressionAlgebraicProcess = "ExpressionAlgebraicProcess";
        /// <summary>
        /// Reserved string ExpressionAssignmentProcess.
        /// </summary>
        public const string ExpressionAssignmentProcess = "ExpressionAssignmentProcess";
        /// <summary>
        /// Reserved string ExpressionFluxProcess.
        /// </summary>
        public const string ExpressionFluxProcess = "ExpressionFluxProcess";
        /// <summary>
        /// Reserved string GillespieProcess.
        /// </summary>
        public const string GillespieProcess = "GillespieProcess";
        /// <summary>
        /// Reserved string GMAProcess.
        /// </summary>
        public const string GMAProcess = "GMAProcess";
        /// <summary>
        /// Reserved string MassActionFluxProcess.
        /// </summary>
        public const string MassActionFluxProcess = "MassActionFluxProcess";
        /// <summary>
        /// Reserved string MichaelisUniUniFluxProcess.
        /// </summary>
        public const string MichaelisUniUniFluxProcess = "MichaelisUniUniFluxProcess";
        /// <summary>
        /// Reserved string PingPongBiBiFluxProcess.
        /// </summary>
        public const string PingPongBiBiFluxProcess = "PingPongBiBiFluxProcess";
        /// <summary>
        /// Reserved string PythonFluxProcess.
        /// </summary>
        public const string PythonFluxProcess = "PythonFluxProcess";
        /// <summary>
        /// Reserved string PythonProcess.
        /// </summary>
        public const string PythonProcess = "PythonProcess";
        /// <summary>
        /// Reserved string QuasiDynamicFluxProcess.
        /// </summary>
        public const string QuasiDynamicFluxProcess = "QuasiDynamicFluxProcess";
        /// <summary>
        /// Reserved string SSystemProcess.
        /// </summary>
        public const string SSystemProcess = "SSystemProcess";
        /// <summary>
        /// Reserved string TauLeapProcess.
        /// </summary>
        public const string TauLeapProcess = "TauLeapProcess";
    }
}
