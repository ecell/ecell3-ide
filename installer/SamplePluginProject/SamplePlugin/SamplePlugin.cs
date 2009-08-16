using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Ecell;
using Ecell.Plugin;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.SamplePlugin
{
    public class SamplePlugin : PluginBase
    {
        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"PropertyWindow"</returns>
        public override string GetPluginName()
        {
            return "SamplePlugin";
        }

        /* This function is to display the new menu.
        public override IEnumerable<System.Windows.Forms.ToolStripMenuItem> GetMenuStripItems()
        {
            return base.GetMenuStripItems();
        }
         */

        /* This function is to display the new dock contents.
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return base.GetWindowsForms();
        }
         */

        /* This function is called when data is added in the loaded project.
        public override void DataAdd(EcellObject data)
        {
            base.DataAdd(data);
        }
         */

        /* This function is called when data is deleted from the loaded project.
        public override void DataDelete(string modelID, string key, string type)
        {
            base.DataDelete(modelID, key, type);
        }
         */

        /* This function is called when data is changed in the loaded project.
        public override void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            base.DataChanged(modelID, key, type, data);
        }
         */

        /* This function is called when the status of project is changed.
        public override void ChangeStatus(ProjectStatus type)
        {
            base.ChangeStatus(type);
        }
         */

        /* This function is called when the selected object is changed.
        public override void SelectChanged(string modelID, string key, string type)
        {
            base.SelectChanged(modelID, key, type);
        }
         */
    }
}
