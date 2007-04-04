using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;

namespace EcellLib.PathwayWindow
{
    public class PPathwayNode : PPathwayObject
    {
        #region Inherited Methods
        public override void Delete()
        {
        }
        public override bool HighLighted(bool highlight)
        {
            return true;
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
        public override List<PathwayElement> GetElements()
        {
            return new List<PathwayElement>();
        }
        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayNode();
        }
        public virtual void AddRelatedNode(PPathwayNode node)
        {
        }
        #endregion

        #region Picking
        
        /*
        /// <summary>
        /// Overridden.  Returns true if this node or any pickable descendends are picked.
        /// </summary>
        /// <remarks>
        /// If a pick occurs the pickPath is modified so that this node is always returned as
        /// the picked node, even if it was a decendent node that initialy reported the pick.
        /// </remarks>
        /// <param name="pickPath"></param>
        /// <returns>True if this node or any descendents are picked; false, otherwise.</returns>
        public override bool FullPick(PPickPath pickPath)
        {
            if (base.Intersects(pickPath.PickBounds))
            {
                bool isThisSelected = false;
                PEcellComposite dummyNode = new PEcellComposite();
                this.Layer.AddChild(dummyNode);
                foreach(PPathwayObject obj in m_set.SelectedNodes)
                {
                    if (obj == this)
                        isThisSelected = true;
                    //obj.Parent.RemoveChild(obj);
                    dummyNode.AddChild(obj);
                }
                if (!isThisSelected)
                {
                    this.Parent.RemoveChild(this);
                    dummyNode.AddChild(this);
                }
                Console.WriteLine("aho: " + this);
                //if (isThisSelected)
                pickPath.PushNode(dummyNode);
                //else
                  //  pickPath.PushNode(this);
                
                return true;
            }
            else
                return false;
        }*/
        #endregion
    }
}