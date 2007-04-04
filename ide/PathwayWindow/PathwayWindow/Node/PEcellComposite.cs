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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.

using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo;
using EcellLib.PathwayWindow.Element;

namespace EcellLib.PathwayWindow.Node
{
    public class PEcellComposite : PPathwayObject
    {
        #region inherited from PPathwayObject
        public override void Delete()
        {
        }

        public override bool HighLighted(bool highlight)
        {
            return false;
        }

        public override void Initialize()
        {
        }

        public override void DataChanged(EcellObject ecellObj)
        {
        }

        public override void DataDeleted()
        {
        }

        public override void SelectChanged()
        {
        }

        public override void Start()
        {
        }

        public override void Change()
        {
        }

        public override void Stop()
        {
        }

        public override void End()
        {
        }

        /// <summary>
        /// Get PathwayElements of PPathwayObjects under this PEcellComposite
        /// </summary>
        /// <returns>List of PathwayElement of PPathwayobjects under the control of this PEcellComposite</returns>
        public override List<PathwayElement> GetElements()
        {
            List<PathwayElement> elementList = new List<PathwayElement>();
            foreach(PNode child in this.ChildrenReference)
            {
                if (child is PPathwayObject)
                    elementList.AddRange(((PPathwayObject)child).GetElements());
            }

            return elementList;
        }

        public override PPathwayObject CreateNewObject()
        {
            return null;
        }
        #endregion

        public override void OnClick(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnClick(e);
            foreach (PNode node in this.ChildrenReference)
            {
                node.OnClick(e);
            }
        }

        public override void OnMouseDown(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDown(e);
            foreach(PNode node in this.ChildrenReference)
            {
                node.OnMouseDown(e);
            }
        }
    }
}
