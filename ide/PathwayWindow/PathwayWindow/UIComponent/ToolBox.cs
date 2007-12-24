using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.Handler;

namespace EcellLib.PathwayWindow.UIComponent
{
    public partial class ToolBox : EcellDockContent
    {
        PathwayControl m_con;
        public ToolBox(PathwayControl control)
        {
            m_con = control;
            InitializeComponent();
            SetToolBoxItems();
        }

        private void SetToolBoxItems()
        {
            PPathwayLayer layer1 = new PPathwayLayer();
            pCanvas1.Root.AddChild(layer1);
            pCanvas1.Camera.AddLayer(layer1);
            pCanvas1.Camera.ScaleViewBy(0.7f);
            pCanvas1.RemoveInputEventListener(pCanvas1.PanEventHandler);
            pCanvas1.RemoveInputEventListener(pCanvas1.ZoomEventHandler);
            pCanvas1.AddInputEventListener(new ToolBoxEventHandler(m_con));
            RectangleF bounds = pCanvas1.Camera.ViewBounds;
            PointF center = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
            PPathwayObject obj1 = m_con.ComponentManager.DefaultSystemSetting.CreateTemplate();
            obj1.Pickable = false;
            obj1.Width = PPathwaySystem.MIN_X_LENGTH;
            obj1.Height = PPathwaySystem.MIN_Y_LENGTH;
            obj1.CenterPointF = center;
            layer1.AddChild(obj1);
            
            PPathwayLayer layer2 = new PPathwayLayer();
            pCanvas2.Root.AddChild(layer2);
            pCanvas2.Camera.AddLayer(layer2);
            pCanvas2.Camera.ScaleViewBy(0.7f);
            pCanvas2.RemoveInputEventListener(pCanvas2.PanEventHandler);
            pCanvas2.RemoveInputEventListener(pCanvas2.ZoomEventHandler);
            pCanvas2.AddInputEventListener(new ToolBoxEventHandler(m_con));
            PPathwayObject obj2 = m_con.ComponentManager.DefaultVariableSetting.CreateTemplate();
            obj2.Pickable = false;
            obj2.CenterPointF = center;
            layer2.AddChild(obj2);

            PPathwayLayer layer3 = new PPathwayLayer();
            pCanvas3.Root.AddChild(layer3);
            pCanvas3.Camera.AddLayer(layer3);
            pCanvas3.Camera.ScaleViewBy(0.7f);
            pCanvas3.RemoveInputEventListener(pCanvas3.PanEventHandler);
            pCanvas3.RemoveInputEventListener(pCanvas3.ZoomEventHandler);
            pCanvas3.AddInputEventListener(new ToolBoxEventHandler(m_con));
            PPathwayObject obj3 = m_con.ComponentManager.DefaultProcessSetting.CreateTemplate();
            obj3.Pickable = false;
            obj3.CenterPointF = center;
            layer3.AddChild(obj3);
        }
    }
}