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

namespace Ecell
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
                case ProcessConstants.ConstantFluxProcess:
                    newProcess = ConstantFlux2ExpressionFlux(process);
                    break;
                case ProcessConstants.DecayFluxProcess:
                    newProcess = ConstantFlux2ExpressionFlux(process);
                    break;
                default:
                    throw new EcellException(string.Format("{0} is not supported.", classname));
            }
            List<EcellReference> refList = process.ReferenceList;

            obj = newProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static EcellProcess ConstantFlux2ExpressionFlux(EcellProcess process)
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
        public static EcellProcess DecayFlux2ExpressionFlux(EcellProcess process)
        {
            EcellProcess newProcess = new EcellProcess(process.ModelID, process.Key, process.Type, ProcessConstants.ExpressionFluxProcess, new List<EcellData>());
            newProcess.ReferenceList = process.ReferenceList;
            newProcess.SetEcellValue("T", process.GetEcellValue("T"));
            newProcess.Expression = "( log ( 2 ) ) / ( T ) * S0.Value";
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
