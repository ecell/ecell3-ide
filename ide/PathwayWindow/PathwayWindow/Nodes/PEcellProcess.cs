using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Nodes;
using System.Drawing;
using System.Windows.Forms;

namespace EcellLib.PathwayWindow
{
    class PEcellProcess : PPathwayNode
    {
        protected Dictionary<PEcellVariable,PPath> m_relatedVariables = new Dictionary<PEcellVariable,PPath>();

        public override PPathwayObject CreateNewObject()
        {
            return new PEcellProcess();
        }
        public override void AddRelatedNode(PPathwayNode node)
        {
            if(node is PEcellVariable && !m_relatedVariables.ContainsKey((PEcellVariable)node))
            {
                PPath path = new PPath();
                path.Tag = node;
                path.Pickable = false;
                path.Pen = new Pen(new SolidBrush(Color.Black), 3);
                float startx = node.FullBounds.X + node.FullBounds.Width / 2;
                float starty = node.FullBounds.Y + node.FullBounds.Height / 2;
                PointF start = path.GlobalToLocal(new PointF(startx, starty));
                float endx = this.FullBounds.X + this.FullBounds.Width / 2;
                float endy = this.FullBounds.Y + this.FullBounds.Height / 2;
                PointF end = path.GlobalToLocal(new PointF(endx, endy));
                path.AddLine(start.X, start.Y, end.X, end.Y);
                base.Parent.AddChild(path);
                m_relatedVariables.Add((PEcellVariable)node,path);
            }
        }
        public override List<PathwayElement> GetElements()
        {
            List<PathwayElement> returnList = new List<PathwayElement>();

            NodeElement nodeEle = new NodeElement();
            if (this.ECellObject != null)
            {
                nodeEle.ModelID = this.ECellObject.modelID;
                nodeEle.Key = this.ECellObject.key;
            }
            nodeEle.Type = "process";
            nodeEle.X = this.X + this.OffsetX;
            nodeEle.Y = this.Y + this.OffsetY;
            nodeEle.CsId = this.m_csId;

            returnList.Add(nodeEle);
            return returnList;
        }
        public override void OnMouseDrag(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDrag(e);
            this.UpdateEdges();
        }
        public void UpdateEdges()
        {
            foreach (KeyValuePair<PEcellVariable, PPath> pair in m_relatedVariables)
            {
                PEcellVariable pvar = (PEcellVariable)pair.Key;
                PPath path = (PPath)pair.Value;

                path.Reset();

                float startx = pvar.FullBounds.X + pvar.FullBounds.Width / 2;
                float starty = pvar.FullBounds.Y + pvar.FullBounds.Height / 2;

                PointF start = path.Parent.LocalToGlobal(new PointF(startx, starty));

                float endx = this.FullBounds.X + this.FullBounds.Width / 2;
                float endy = this.FullBounds.Y + this.FullBounds.Height / 2;
                PointF end =path.Parent.LocalToGlobal(new PointF(endx, endy));

                start = path.GlobalToLocal(start);
                end = path.GlobalToLocal(end);
                path.AddLine(start.X,start.Y,end.X,end.Y);
                path.Repaint();
            }
        }
    }
}