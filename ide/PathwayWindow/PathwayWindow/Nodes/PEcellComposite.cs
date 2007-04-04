using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo;

namespace EcellLib.PathwayWindow
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
