using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EcellLib.PathwayWindow
{
    class PEcellVariable : PPathwayNode
    {
        protected List<PEcellProcess> m_relatedProcesses = new List<PEcellProcess>();

        public override PPathwayObject CreateNewObject()
        {
            return new PEcellVariable();
        }
        public override void AddRelatedNode(PPathwayNode node)
        {
            if(node is PEcellProcess && !m_relatedProcesses.Contains((PEcellProcess)node))
            {
                m_relatedProcesses.Add((PEcellProcess)node);
            }
        }
        public override List<PathwayElement> GetElements()
        {
            List<PathwayElement> elementList = new List<PathwayElement>();
            NodeElement nodeEle = new NodeElement();
            if(this.ECellObject != null)
            {
                nodeEle.ModelID = this.ECellObject.modelID;
                nodeEle.Key = this.ECellObject.key;
            }
            nodeEle.Type = "variable";
            nodeEle.X = this.X + this.OffsetX;
            nodeEle.Y = this.Y + this.OffsetY;
            nodeEle.CsId = this.m_csId;

            elementList.Add(nodeEle);

            return elementList;
        }
        public override void OnMouseDrag(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDrag(e);

            foreach(PEcellProcess process in m_relatedProcesses)
            {
                process.UpdateEdges();
            }
        }
    }
}