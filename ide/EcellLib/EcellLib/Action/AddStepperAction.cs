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
//

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Objects;

namespace Ecell.Action
{

    /// <summary>
    /// Action class to create stepper.
    /// </summary>
    public class AddStepperAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The parameter ID added the stepper.
        /// </summary>
        private string m_paramID;
        /// <summary>
        /// The stepper object.
        /// </summary>
        private EcellObject m_stepper;
        #endregion

        /// <summary>
        /// The constructor for AddStepperAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The parameter ID added the stepper.</param>
        /// <param name="stepper">The stepper object.</param>
        public AddStepperAction(EcellObject stepper)
        {
            m_stepper = stepper;
            m_isAnchor = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "AddStepperAction:" + m_paramID;
        }
        /// <summary>
        /// Execute to add the stepper using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.DataAdd(m_stepper, false, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DataDelete(m_stepper, false, false);
        }
    }
}
