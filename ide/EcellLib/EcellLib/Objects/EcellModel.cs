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
        /// List of EcellSystems in this model.
        /// </summary>
        private List<EcellObject> m_systems;
        /// <summary>
        /// Dictionary of stepper list in this model.
        /// List with Simulation parameter.
        /// </summary>
        private Dictionary<string, List<EcellObject>> m_stepperDic;
        /// <summary>
        /// The error message.
        /// </summary>
        private string m_err = "";
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
            : base(modelID, "", MODEL, MODEL, data)
        {
            m_systems = new List<EcellObject>();
            m_stepperDic = new Dictionary<string, List<EcellObject>>();
            if (!IsEcellValueExists(EcellLayer.Layers))
                this.Layers = new List<EcellLayer>();
        }
        #endregion

        #region Accessors
        /// <summary>
        /// ModelID
        /// </summary>
        public override string ModelID
        {
            get
            {
                return base.ModelID;
            }
            set
            {
                base.ModelID = value;
                ChangeModelIDforChildren(this);
            }
        }

        /// <summary>
        /// get / set the error message.
        /// </summary>
        public string ErrMsg
        {
            get { return m_err; }
            set { m_err = value; }
        }

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
        public List<EcellLayer> Layers
        {
            get
            {
                EcellValue layers = this.GetEcellValue(EcellLayer.Layers);
                return EcellLayer.ConvertFromEcellValue(layers);
            }
            set
            {
                EcellValue layers = EcellLayer.ConvertToEcellValue(value);
                SetEcellValue(EcellLayer.Layers, layers);
                GetEcellData(EcellLayer.Layers).Settable = false;
            }
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
        }
        #endregion

        /// <summary>
        /// Change the model ID of object.
        /// </summary>
        /// <param name="obj">the changed object.</param>
        private void ChangeModelIDforChildren(EcellObject obj)
        {
            if (obj.Children == null || obj.Children.Count <= 0)
                return;
            foreach (EcellObject child in obj.Children)
            {
                child.ModelID = obj.ModelID;
                ChangeModelIDforChildren(child);
            }
        }

        /// <summary>
        /// Add the entity.
        /// </summary>
        /// <param name="entity">the added object.</param>
        public void AddEntity(EcellObject entity)
        {
            EcellObject system = GetSystem(entity.ParentSystemID);
            system.Children.Add(entity);
        }
        
        /// <summary>
        /// Get System.
        /// </summary>
        /// <param name="key">the system key.</param>
        /// <returns>System object.</returns>
        public EcellObject GetSystem(string key)
        {
            // Check systemList
            if (m_children == null || m_children.Count <= 0)
                return null;

            EcellObject system = null;
            foreach (EcellObject sys in m_children)
            {
                if (!(sys is EcellSystem))
                    continue;
                if (!sys.Key.Equals(key))
                    continue;
                system = sys;
                break;
            }
            return system;
        }
    }
}
