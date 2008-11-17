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

namespace Ecell.Objects
{

    /// <summary>
    /// Object class for Model.
    /// </summary>
    public class EcellModel : EcellObject
    {
        #region Fields
        /// <summary>
        /// name of model file.
        /// </summary>
        private string m_modelFile;
        /// <summary>
        /// List of Layers in this model.
        /// </summary>
        private List<string> m_layers;
        /// <summary>
        /// List of EcellSystems in this model.
        /// </summary>
        private List<EcellObject> m_systems;
        /// <summary>
        /// Dictionary of stepper list in this model.
        /// List with Simulation parameter.
        /// </summary>
        private Dictionary<string, List<EcellObject>> m_stepperDic;
        #endregion

        #region Constractors
        /// <summary>
        /// constructor with initial parameter.
        /// </summary>
        /// <param name="modelID">modelID.</param>
        /// <param name="key">key.</param>
        /// <param name="type">type(="Model")</param>
        /// <param name="classname">class name</param>
        /// <param name="data">properties of object.</param>
        public EcellModel(string modelID, string key,
             string type, string classname, List<EcellData> data)
            : base(modelID, key, type, classname, data)
        {
            m_layers = new List<string>();
            m_systems = new List<EcellObject>();
            m_stepperDic = new Dictionary<string, List<EcellObject>>();
        }
        #endregion

        #region Accessors
        /// <summary>
        /// name of model file.
        /// </summary>
        public string ModelFile
        {
            get { return m_modelFile; }
            set { m_modelFile = value; }
        }
        /// <summary>
        /// Layer list of this model.
        /// </summary>
        public List<string> Layers
        {
            get { return m_layers; }
        }
        /// <summary>
        /// List of EcellSystems int this model.
        /// </summary>
        public List<EcellObject> Systems
        {
            get { return m_systems; }
        }
        /// <summary>
        /// Dictionary of stepper list in this model.
        /// List with Simulation parameter.
        /// </summary>
        public Dictionary<string, List<EcellObject>> StepperDic
        {
            get { return m_stepperDic; }
            set { m_stepperDic = value; }
        }
        #endregion
    }
}
