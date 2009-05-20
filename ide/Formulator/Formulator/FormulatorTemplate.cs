// Formulator C# Library
// COPYRIGHT (C) 2006-2009  MITSUBISHI SPACE SOFTWARE CO.,LTD.
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

namespace Ecell.UI.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class FormulatorTemplate
    {        
        #region Fields
        /// <summary>
        /// Dictionary template name and template.
        /// </summary>
        private Dictionary<string, string> m_templetes;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public FormulatorTemplate()
        {
            m_templetes = new Dictionary<string, string>();

            m_templetes.Add(MessageResources.PRIMARYREACTION, "K * S0.MolarConc * self.getSuperSystem().SizeN_A");
            m_templetes.Add(MessageResources.SECONDARYREACTION, "2 * k * pow( S0.MolarConc , 2 ) * self.getSuperSystem().SizeN_A");
            m_templetes.Add(MessageResources.DEGRADATION, "Kd * S0.MolarConc * self.getSuperSystem().SizeN_A");
            m_templetes.Add(MessageResources.MICHAELIS, "(Vmax * S0.MolarConc) / (Km + S0.MolarConc) * self.getSuperSystem().SizeN_A");
            m_templetes.Add(MessageResources.COMPETITIVE, "(Vmax * S0.MolarConc) / ( (1 + (C0.MolarConc) / (Ki)) * Km + S0.MolarConc) * self.getSuperSystem().SizeN_A");
            m_templetes.Add(MessageResources.NOCOMPETITIVE, "(Vmax / (1 + (C0.MolarConc) / (Ki)) * S0.MolarConc) / ( Km + S0.MolarConc) * self.getSuperSystem().SizeN_A");
            m_templetes.Add(MessageResources.UNCOMPETITIVE, "(Vmax / (1 + (C0.MolarConc) / (Ki)) * S0.MolarConc) / ( (1 + (C0.MolarConc) / (Ki)) * Km + S0.MolarConc) * self.getSuperSystem().SizeN_A");
        }

        /// <summary>
        /// Get template name list.
        /// </summary>
        /// <returns>the list of string.</returns>
        public List<string> GetTemplateList()
        {
            List<string> result = new List<string>();

            foreach (string i in m_templetes.Keys)
            {
                result.Add(i);
            }

            return result;
        }

        /// <summary>
        /// Get template from the name of template.
        /// </summary>
        /// <param name="name">the name of template.</param>
        /// <returns>template string.</returns>
        public string GetTemplate(string name)
        {
            return m_templetes[name];
        }
    }
}
