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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

using Ecell.Objects;

namespace Ecell
{
    public class DynamicModuleManager
    {
        #region Fields
        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        private ApplicationEnvironment m_env;
        private Dictionary<string, DynamicModule> m_moduleDic = new Dictionary<string, DynamicModule>();
        #endregion

        /// <summary>
        /// Creates the new "DynamicModuleManager" instance with no argument.
        /// </summary>
        public DynamicModuleManager(ApplicationEnvironment env)
        {
            this.m_env = env;
           
            // ここでDMをロードしてDMの説明、プロパティ情報を取得する
            // が、今のところプロパティ情報を追加する作業を行う
            DynamicModule tmp = null;
            tmp = new DynamicModule("ConstantsFluxProcess", "", false, "ConstantsFluxProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("k", true, true, true, true, (double)0.0, typeof(double));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("DecayFluxProcess", "", false, "DecayFluxProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("T", true, true, true, true, (double)1.0, typeof(double));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("ExpressionAlgebraicProcess", "", false, "ExpressionAlgebraicProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 0, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("Expression", true, true, true, true, "self.getSuperSystem().SizeN_A", typeof(string));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("ExpressionFluxProcess", "", false, "ExpressionFluxProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 0, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("Expression", true, true, true, true, "self.getSuperSystem().SizeN_A", typeof(string));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("GillespieProcess", "", false, "GillespieProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 0, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("MuV", true, true, true, true, (double)0.0, typeof(double));
            tmp.AddProperty("DependentProcessList", false, true, false, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("Order", false, true, false, false, 0, typeof(int));
            tmp.AddProperty("StepInterval", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("k", true, true, true, true, (double)0.0, typeof(double));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("GMAProcess", "", false, "GMAProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 0, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("GMASystemMatrix", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("Order", true, true, true, true, 0, typeof(int)); 
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("MassActionFluxProcess", "", false, "MassActionFluxProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("k", true, true, true, true, (double)0.0, typeof(double));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("MichaelisUniUniFluxProcess", "", false, "MichaelisUniUniFluxProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("KcF", true, true, true, true, (double)0.0, typeof(double));
            tmp.AddProperty("KcR", true, true, true, true, (double)0.0, typeof(double));
            tmp.AddProperty("KmP", true, true, true, true, (double)0.0, typeof(double));
            tmp.AddProperty("KmS", true, true, true, true, (double)0.0, typeof(double));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("PythonFluxProcess", "", false, "PythonFluxProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("Expression", true, true, true, true, "", typeof(string));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("PythonProcess", "", false, "PythonProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("FireMethod", true, true, true, true, "", typeof(string));
            tmp.AddProperty("InitializeMethod", true, true, true, true, "", typeof(string));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("QuasiDynamicFluxProcess", "", false, "QuasiDynamicFluxProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 0, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("Irreversible", true, true, true, true, 0, typeof(int));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("SSystemProcess", "", false, "SSystemProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 0, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("SSystemMatrix", true, true, true, true, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            //
            tmp = new DynamicModule("DAEStepper", "", false, "DAEStepper");
            tmp.AddProperty("AbsoluteTolerance", true, true, true, true, (double)1e-10, typeof(double));
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("JacobianRecalculateTheta", true, true, true, true, (double)0.001, typeof(double));
            tmp.AddProperty("MaxIterationNumber", true, true, true, true, 7, typeof(int));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1.79769313486e+308, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)2.22507385851e-308, typeof(double));
            tmp.AddProperty("NextStepInterval", false, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RelativeTolerance", true, true, true, true, (double)1e-10, typeof(double));
            tmp.AddProperty("RngSeed", true, false, true, false, "", typeof(string));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("Uround", true, true, true, true, (double)1e-16, typeof(double));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("ESSYNSStepper", "", false, "ESSYNSStepper");
            tmp.AddProperty("AbsoluteEpsilon", true, true, true, true, (double)0.1, typeof(double));
            tmp.AddProperty("AbsoluteToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("DerivativeToleranceFactor", true, true, true, true, (double)1.001, typeof(double));
            tmp.AddProperty("MaxErrorRatio", false, true, false, false, (double)1.0, typeof(double));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1e+100, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)1e-100, typeof(double));
            tmp.AddProperty("NextStepInterval", false, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Order", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RelativeEpsilon", true, true, true, true, (double)0.1, typeof(double));
            tmp.AddProperty("RngSeed", true, false, true, false, "", typeof(string));
            tmp.AddProperty("StateToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("StepInterval", true, true, true, true, (double)0.001, typeof(double));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("TaylorOrder", true, true, true, true, 1, typeof(int));
            tmp.AddProperty("Tolerance", true, true, true, true, (double)1e-6, typeof(double));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("FixedDAE1Stepper", "", false, "FixedDAE1Stepper");
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1.79769313486e+308, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)2.22507385851e-308, typeof(double));
            tmp.AddProperty("NextStepInterval", false, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("PerturbationRate", true, true, true, true, (double)1.0000000000000001e-09, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RngSeed", true, true, true, true, "", typeof(string));
            tmp.AddProperty("StepInterval", true, true, true, true, (double)0.001, typeof(double));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("Tolerance", true, true, true, true, (double)1e-10, typeof(double));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("FixedODE1Stepper", "", false, "FixedODE1Stepper");
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1.79769313486e+308, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)2.22507385851e-308, typeof(double));
            tmp.AddProperty("NextStepInterval", false, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RngSeed", true, false, true, false, "", typeof(string));
            tmp.AddProperty("StepInterval", true, true, true, true, (double)0.001, typeof(double));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("FluxDistributionStepper", "", false, "FluxDistributionStepper");
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1.79769313486e+308, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)2.22507385851e-308, typeof(double));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RngSeed", true, false, true, false, "", typeof(string));
            tmp.AddProperty("StepInterval", true, true, true, true, (double)0.001, typeof(double));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("ODE23Stepper", "", false, "ODE23Stepper");
            tmp.AddProperty("AbsoluteEpsilon", true, true, true, true, (double)0.1, typeof(double));
            tmp.AddProperty("AbsoluteToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("DerivativeToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("MaxErrorRatio", false, true, false, false, (double)1.0, typeof(double));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1e+100, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)1e-100, typeof(double));
            tmp.AddProperty("NextStepInterval", false, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Order", false, true, false, false, 2, typeof(int));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RngSeed", true, false, true, false, "", typeof(string));
            tmp.AddProperty("StateToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("StepInterval", true, true, true, true, (double)0.001, typeof(double));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("Tolerance", true, true, true, true, (double)1e-6, typeof(double));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("ODE45Stepper", "", false, "ODE45Stepper");
            tmp.AddProperty("AbsoluteEpsilon", true, true, true, true, (double)0.1, typeof(double));
            tmp.AddProperty("AbsoluteToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("DerivativeToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("MaxErrorRatio", false, true, false, false, (double)1.0, typeof(double));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1e+100, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)1e-100, typeof(double));
            tmp.AddProperty("NextStepInterval", false, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Order", false, true, false, false, 5, typeof(int));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RngSeed", true, false, true, false, "", typeof(string));
            tmp.AddProperty("SpectralRadius", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("StateToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("StepInterval", true, true, true, true, (double)0.01, typeof(double));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("Tolerance", true, true, true, true, (double)1e-6, typeof(double));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("ODEStepper", "", false, "ODEStepper");
            tmp.AddProperty("AbsoluteEpsilon", true, true, true, true, (double)0.1, typeof(double));
            tmp.AddProperty("AbsoluteToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("DerivativeToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("IsEpsilonChecked", true, true, true, true, 1, typeof(int));
            tmp.AddProperty("JacobianRecalculateTheta", true, true, true, true, (double)0.001, typeof(double));
            tmp.AddProperty("MaxErrorRatio", false, true, false, false, (double)1.0, typeof(double));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1e+100, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)1e-100, typeof(double));
            tmp.AddProperty("NextStepInterval", false, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Order", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RelativeEpsilon", true, true, true, true, (double)0.1, typeof(double));
            tmp.AddProperty("SpectralRadius", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("StateToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("StepInterval", true, true, true, true, (double)0.01, typeof(double));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            // ,が合っていない
            tmp.AddProperty("TolerableStepInterval", true, true, true, true, (double)0.001, typeof(double));
            tmp.AddProperty("Tolerance", true, true, true, true, (double)1e-6, typeof(double));
            tmp.AddProperty("Uround", true, true, true, true, (double)1e-6, typeof(double));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);

            // ここからdescriptionがなかった
            tmp = new DynamicModule("TauLeapProcess", "", false, "TauLeapProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("Order", false, true, false, false, 0, typeof(int));
            tmp.AddProperty("Propensity", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("k", true, true, true, true, (double)0.0, typeof(double));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("PingPongBiBiFluxProcess", "", false, "PingPongBiBiFluxProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("KcF", true, true, true, true, (double)0.0, typeof(double));
            tmp.AddProperty("KcR", true, true, true, true, (double)0.0, typeof(double));
            tmp.AddProperty("Keq", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("KmS0", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("KmS1", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("KmP0", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("KmP1", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("KiS0", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("KiP1", true, true, true, true, (double)1.0, typeof(double));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("ExpressionAssignmentProcess", "", false, "ExpressionAssignmentProcess");
            tmp.AddProperty("Activity", true, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("IsContinuous", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("Name", true, true, true, true, "", typeof(string));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("StepperID", true, true, true, true, "", typeof(string));
            tmp.AddProperty("VariableReferenceList", true, true, true, true, "", typeof(List<EcellValue>));
            tmp.AddProperty("Expression", true, true, true, true, "self.getSuperSystem().SizeN_A", typeof(string));
            tmp.AddProperty("Variable", true, true, true, true, "", typeof(string));
            this.m_moduleDic.Add(tmp.Name, tmp);

            tmp = new DynamicModule("TauLeapStepper", "", false, "TauLeapStepper");
            tmp.AddProperty("AbsoluteEpsilon", true, true, true, true, (double)0.1, typeof(double));
            tmp.AddProperty("AbsoluteToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("CurrentTime", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("DependentStepperList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("DerivativeToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("Epsilon", true, true, true, true, (double)0.03, typeof(double));
            tmp.AddProperty("MaxErrorRatio", false, true, false, false, (double)1.0, typeof(double));
            tmp.AddProperty("MaxStepInterval", true, true, true, true, (double)1e+100, typeof(double));
            tmp.AddProperty("MinStepInterval", true, true, true, true, (double)1e-100, typeof(double));
            tmp.AddProperty("NextStepInterval", false, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Order", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("OriginalStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Priority", true, true, true, true, 0, typeof(int));
            tmp.AddProperty("ProcessList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("ReadVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("RelativeEpsilon", true, true, true, true, (double)0.1, typeof(double));
            tmp.AddProperty("SpectralRadius", false, true, false, false, (double)0.0, typeof(double));
            tmp.AddProperty("Stage", false, true, false, false, 1, typeof(int));
            tmp.AddProperty("StateToleranceFactor", true, true, true, true, (double)1.0, typeof(double));
            tmp.AddProperty("StepInterval", true, true, true, true, (double)0.01, typeof(double));
            tmp.AddProperty("SystemList", false, true, false, false, "", typeof(List<EcellValue>));
            tmp.AddProperty("Tau", false, true, false, false, Double.PositiveInfinity, typeof(double));
            tmp.AddProperty("TolerableStepInterval", true, true, false, false, (double)0.001, typeof(double));
            tmp.AddProperty("Tolerance", true, true, true, true, (double)1e-6, typeof(double));
            tmp.AddProperty("Uround", true, true, true, true, (double)1e-6, typeof(double));
            tmp.AddProperty("WriteVariableList", false, true, false, false, "", typeof(List<EcellValue>));
            this.m_moduleDic.Add(tmp.Name, tmp);
        }

        public Dictionary<string, DynamicModule> ModuleDic
        {
            get { return this.m_moduleDic; }
        }
    }

    public class DynamicModule
    {
        #region Fields
        private string m_name;
        private string m_path;
        private bool m_isProjectDM;
        private string m_description;
        private Dictionary<string, DynamicModuleProperty> m_propertyDic = new Dictionary<string, DynamicModuleProperty>();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        public DynamicModule(string name, string path, bool isProject, string description)
        {
            this.m_name = name;
            this.m_path = path;
            this.m_isProjectDM = isProject;
            this.m_description = description;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Dynamic module name
        /// </summary>
        public String Name
        {
            get { return this.m_name; }
        }

        public String Path
        {
            get { return this.m_path; }
        }

        public bool IsProjectDM
        {
            get { return this.m_isProjectDM; }
        }

        public String Description
        {
            get { return this.m_description; }
        }

        public Dictionary<string, DynamicModuleProperty> Property
        {
            get { return this.m_propertyDic; }
        }
        #endregion

        public void AddProperty(string name, bool isSettable, bool isGettable,
            bool isLoadable, bool isSavable, object defaultobj, Type typedata)
        {
            this.m_propertyDic.Add(name, 
                new DynamicModuleProperty(name, isSettable, isGettable,
                isLoadable, isSavable, defaultobj, typedata));
        }
    }

    public class DynamicModuleProperty
    {
        #region Fields
        private string m_name;
        private bool m_isSettable;
        private bool m_isGettable;
        private bool m_isLoadable;
        private bool m_isSavable;
        private object m_default;
        private Type m_type;
        #endregion

        public DynamicModuleProperty(string name, bool isSettable, bool isGettable,
            bool isLoadable, bool isSavable, object defaultobj, Type typedata)
        {
            m_name = name;
            m_isSettable = isSettable;
            m_isGettable = isGettable;
            m_isLoadable = isLoadable;
            m_isSavable = isSavable;
            m_default = defaultobj;
            m_type = typedata;
        }

        public string Name
        {
            get { return this.m_name; }
        }

        public bool IsSettable
        {
            get { return m_isSettable; }
        }

        public bool IsGettable
        {
            get { return m_isGettable; }
        }

        public bool IsLoadable
        {
            get { return m_isLoadable; }
        }

        public bool IsSavable
        {
            get { return m_isSavable; }
        }

        public object DefaultData
        {
            get { return m_default; }
        }

        public Type Type
        {
            get { return m_type; }
        }
    }
}
