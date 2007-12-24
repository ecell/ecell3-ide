using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EcellLib.PathwayWindow.Nodes;

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
            float left = 30;
            float top = 60;
            PPathwayLayer layer1 = new PPathwayLayer();
            pCanvas1.Root.AddChild(layer1);
            pCanvas1.Camera.AddLayer(layer1);
            pCanvas1.Camera.ScaleViewBy(0.7f);
            pCanvas1.RemoveInputEventListener(pCanvas1.PanEventHandler);
            pCanvas1.RemoveInputEventListener(pCanvas1.ZoomEventHandler);
            PPathwayObject obj1 = m_con.ComponentManager.DefaultSystemSetting.CreateTemplate();
            obj1.Pickable = false;
            obj1.X = left;
            obj1.Y = Top;
            obj1.Width = PPathwaySystem.MIN_X_LENGTH;
            obj1.Height = PPathwaySystem.MIN_Y_LENGTH;
            layer1.AddChild(obj1);
            
            PPathwayLayer layer2 = new PPathwayLayer();
            pCanvas2.Root.AddChild(layer2);
            pCanvas2.Camera.AddLayer(layer2);
            pCanvas2.Camera.ScaleViewBy(0.7f);
            pCanvas2.RemoveInputEventListener(pCanvas2.PanEventHandler);
            pCanvas2.RemoveInputEventListener(pCanvas2.ZoomEventHandler);
            PPathwayObject obj2 = m_con.ComponentManager.DefaultVariableSetting.CreateTemplate();
            obj2.Pickable = false;
            obj2.X = left;
            obj2.Y = Top;
            layer2.AddChild(obj2);

            PPathwayLayer layer3 = new PPathwayLayer();
            pCanvas3.Root.AddChild(layer3);
            pCanvas3.Camera.AddLayer(layer3);
            pCanvas3.Camera.ScaleViewBy(0.7f);
            pCanvas3.RemoveInputEventListener(pCanvas3.PanEventHandler);
            pCanvas3.RemoveInputEventListener(pCanvas3.ZoomEventHandler);
            PPathwayObject obj3 = m_con.ComponentManager.DefaultProcessSetting.CreateTemplate();
            obj3.Pickable = false;
            obj3.X = left;
            obj3.Y = Top;
            layer3.AddChild(obj3);
        }
    }
}