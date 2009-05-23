//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
// written by Moriyoshi Koizumi <mozo@sfc.keio.ac.jp>
//

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using EcellCoreLib;
using Ecell.Objects;

namespace Ecell
{
    /// <summary>
    /// 
    /// </summary>
    public class DMDescriptorKeeper
    {
        #region Fields
        /// <summary>
        /// DM Paths.
        /// </summary>
        private string[] m_dmPaths;
        /// <summary>
        /// Dictionary of DMDescriptors [Type, [Name, DMDescriptors]]
        /// </summary>
        private Dictionary<string, Dictionary<string, DMDescriptor>> m_descs = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmPaths"></param>
        public DMDescriptorKeeper(string[] dmPaths)
        {
            Load(dmPaths);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DMDescriptor GetDMDescriptor(string type, string name)
        {
            DMDescriptor desc = null;
            m_descs[type].TryGetValue(name, out desc);
            return desc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsDescriptor(string type, string name)
        {
            DMDescriptor desc = GetDMDescriptor(type, name);
            return desc != null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ICollection<DMDescriptor> GetDMDescriptors(string type)
        {
            return m_descs[type].Values;
        }
        /// <summary>
        /// Load DMDescriptions.
        /// </summary>
        /// <param name="dmPaths"></param>
        /// <returns></returns>
        public void Load(string[] dmPaths)
        {
            m_dmPaths = dmPaths;

            // Set dictionary.
            Dictionary<string, Dictionary<string, List<DMModuleInfo>>> maps =
                    new Dictionary<string, Dictionary<string, List<DMModuleInfo>>>();
            Dictionary<string, Dictionary<string, List<DMModuleInfo>>> modulesToLookup =
                    new Dictionary<string, Dictionary<string, List<DMModuleInfo>>>();
            maps[Constants.xpathSystem] = new Dictionary<string, List<DMModuleInfo>>();
            maps[Constants.xpathStepper] = new Dictionary<string, List<DMModuleInfo>>();
            maps[Constants.xpathProcess] = new Dictionary<string, List<DMModuleInfo>>();
            maps[Constants.xpathVariable] = new Dictionary<string, List<DMModuleInfo>>();

            // Look for built-in modules
            {
                const string dmPath = "<BUILTIN>";
                Dictionary<string, List<DMModuleInfo>> perDirectoryModuleList =
                    new Dictionary<string, List<DMModuleInfo>>();
                perDirectoryModuleList[Constants.xpathSystem] = new List<DMModuleInfo>();
                perDirectoryModuleList[Constants.xpathStepper] = new List<DMModuleInfo>();
                perDirectoryModuleList[Constants.xpathProcess] = new List<DMModuleInfo>();
                perDirectoryModuleList[Constants.xpathVariable] = new List<DMModuleInfo>();

                modulesToLookup[dmPath] = perDirectoryModuleList;

                WrappedSimulator sim = new WrappedSimulator(new string[] { "" });
                foreach (DMInfo entry in sim.GetDMInfo())
                {
                    if (string.IsNullOrEmpty(entry.FileName))
                    {
                        perDirectoryModuleList[entry.TypeName].Add(
                            new DMModuleInfo(dmPath, entry));
                    }
                }
            }

            // Searches the DM paths
            foreach (string dmPath in m_dmPaths)
            {
                if (!Directory.Exists(dmPath))
                    continue;

                string[] modulePaths = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.FileExtDM
                    );
                if (modulePaths.Length == 0)
                    continue;

                Dictionary<string, List<DMModuleInfo>> perDirectoryModuleList =
                    new Dictionary<string, List<DMModuleInfo>>();
                perDirectoryModuleList[Constants.xpathSystem] = new List<DMModuleInfo>();
                perDirectoryModuleList[Constants.xpathStepper] = new List<DMModuleInfo>();
                perDirectoryModuleList[Constants.xpathProcess] = new List<DMModuleInfo>();
                perDirectoryModuleList[Constants.xpathVariable] = new List<DMModuleInfo>();

                modulesToLookup[dmPath] = perDirectoryModuleList;

                WrappedSimulator sim = new WrappedSimulator(new string[] { dmPath });
                foreach (string modulePath in modulePaths)
                {
                    string moduleName = Path.GetFileNameWithoutExtension(modulePath);
                    string moduleType = null;

                    if (moduleName.EndsWith(Constants.xpathStepper))
                        moduleType = Constants.xpathStepper;
                    else if (moduleName.EndsWith(Constants.xpathProcess))
                        moduleType = Constants.xpathProcess;
                    else if (moduleName.EndsWith(Constants.xpathVariable))
                        moduleType = Constants.xpathVariable;
                    else if (moduleName.EndsWith(Constants.xpathSystem))
                        moduleType = Constants.xpathSystem;

                    if (moduleType == null)
                        continue; // XXX: what are we supposed to do here?

                    List<DMModuleInfo> infoList = null;
                    maps[moduleType].TryGetValue(moduleName, out infoList);
                    if (infoList == null)
                    {
                        infoList = new List<DMModuleInfo>();
                        maps[moduleType][moduleName] = infoList;
                    }
                    string description = sim.GetDescription(moduleName);
                    DMModuleInfo info = new DMModuleInfo(modulePath, moduleName, description);
                    infoList.Add(info);
                    perDirectoryModuleList[moduleType].Add(info);
                }
            }

            Dictionary<string, Dictionary<string, DMDescriptor>> descs =
                new Dictionary<string, Dictionary<string, DMDescriptor>>();
            descs[Constants.xpathSystem] = new Dictionary<string, DMDescriptor>();
            descs[Constants.xpathProcess] = new Dictionary<string, DMDescriptor>();
            descs[Constants.xpathVariable] = new Dictionary<string, DMDescriptor>();
            descs[Constants.xpathStepper] = new Dictionary<string, DMDescriptor>();

            foreach (KeyValuePair<string, Dictionary<string, List<DMModuleInfo>>> kv in modulesToLookup)
            {
                WrappedSimulator sim = new WrappedSimulator(new string[] { kv.Key });
                {
                    sim.CreateStepper("PassiveStepper", "tmp");
                    string id = Util.BuildFullPN(Constants.xpathSystem, "", "/", "StepperID");
                    sim.SetEntityProperty(id, "tmp");
                }
                Trace.WriteLine("Checking DMs in " + kv.Key);

                // Test System DMs.
                foreach (DMModuleInfo pair in kv.Value[Constants.xpathSystem])
                {
                    LoadEntityDM(Constants.xpathSystem, descs, sim, pair);
                }

                // Test Process DMs.
                foreach (DMModuleInfo pair in kv.Value[Constants.xpathProcess])
                {
                    LoadEntityDM(Constants.xpathProcess, descs, sim, pair);
                }

                // Test Variable DMs.
                foreach (DMModuleInfo pair in kv.Value[Constants.xpathVariable])
                {
                    LoadEntityDM(Constants.xpathVariable, descs, sim, pair);
                }

                // Test Stepper DMs.
                foreach (DMModuleInfo pair in kv.Value[Constants.xpathStepper])
                {
                    LoadStepperDM(descs, sim, pair);
                }
            }

            m_descs = descs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descs"></param>
        /// <param name="sim"></param>
        /// <param name="info"></param>
        private static void LoadStepperDM(Dictionary<string, Dictionary<string, DMDescriptor>> descs, WrappedSimulator sim, DMModuleInfo info)
        {
            try
            {
                Trace.WriteLine("Checking properties for " + info.ModuleName);
                string stepper = info.ModuleName;
                sim.CreateStepper(stepper, stepper);

                // Get PropertyDescriptors
                Dictionary<string, PropertyDescriptor> pdescs = GetStepperPropertyDescriptors(sim, stepper);

                // Check DynamicProperty
                bool dynamic = CheckDynamicProperty(sim, stepper, pdescs);
                DMDescriptor desc = new DMDescriptor(stepper, info.Path, Constants.xpathStepper, dynamic, pdescs);
                desc.Description = info.Description;

                descs[Constants.xpathStepper][info.ModuleName] = desc;
            }
            catch (Exception)
            {
                Trace.WriteLine("Failed to load " + info.ModuleName);
                //Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="descs"></param>
        /// <param name="sim"></param>
        /// <param name="info"></param>
        private static void LoadEntityDM(string type, Dictionary<string, Dictionary<string, DMDescriptor>> descs, WrappedSimulator sim, DMModuleInfo info)
        {
            try
            {
                Trace.WriteLine("Checking properties for " + info.ModuleName);
                string id = Util.BuildFullID(type, "/", info.ModuleName);
                sim.CreateEntity(info.ModuleName, id);

                // Get PropertyDescriptors
                Dictionary<string, PropertyDescriptor> pdescs = GetEntityPropertyDescriptors(sim, id);

                // Check DynamicProperty
                bool dynamic = CheckDynamicProperty(sim, id, pdescs);
                DMDescriptor desc = new DMDescriptor(info.ModuleName, info.Path, type, dynamic, pdescs);
                desc.Description = info.Description;
                descs[type][info.ModuleName] = desc;
                    
            }
            catch (Exception)
            {
                Trace.WriteLine("Failed to load " + info.ModuleName);
                //Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Get PropertyDescriptors
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="stepper"></param>
        /// <returns></returns>
        private static Dictionary<string, PropertyDescriptor> GetStepperPropertyDescriptors(WrappedSimulator sim, string stepper)
        {

            // Get default property values.
            Dictionary<string, PropertyDescriptor> pdescs = new Dictionary<string, PropertyDescriptor>();
            foreach (string propName in sim.GetStepperPropertyList(stepper))
            {
                PropertyAttributes attrs = sim.GetStepperPropertyAttributes(stepper, propName);
                EcellValue defaultValue = null;
                if (attrs.Gettable)
                {
                    try
                    {
                        defaultValue = new EcellValue(sim.GetStepperProperty(stepper, propName));
                    }
                    catch (Exception)
                    {
                        Trace.WriteLine(string.Format("Failed to load Property {0} on {1}", propName, stepper));
                    }
                }
                pdescs[propName] = new PropertyDescriptor(
                    propName,
                    attrs.Settable, // settable
                    attrs.Gettable, // gettable
                    attrs.Loadable, // loadable
                    attrs.Savable,  // saveable
                    attrs.Dynamic,  // dynamic
                    attrs.Gettable, // logable
                    defaultValue
                );
            }
            return pdescs;
        }

        /// <summary>
        /// Get PropertyDescriptors
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="fullID"></param>
        /// <returns></returns>
        private static Dictionary<string, PropertyDescriptor> GetEntityPropertyDescriptors(WrappedSimulator sim, string fullID)
        {

            // Get default property values.
            Dictionary<string, PropertyDescriptor> pdescs = new Dictionary<string, PropertyDescriptor>();
            foreach (string propName in sim.GetEntityPropertyList(fullID))
            {
                string fullPN = Util.BuildFullPN(fullID, propName);
                PropertyAttributes attrs = sim.GetEntityPropertyAttributes(fullPN);
                EcellValue defaultValue = null;
                if (attrs.Gettable)
                {
                    try
                    {
                        defaultValue = new EcellValue(sim.GetEntityProperty(fullPN));
                    }
                    catch (Exception)
                    {
                        Trace.WriteLine(string.Format("Failed to load Property {0} on {1}", propName, fullID));
                    }
                }
                pdescs[propName] = new PropertyDescriptor(
                    propName,
                    attrs.Settable, // settable
                    attrs.Gettable, // gettable
                    attrs.Loadable, // loadable
                    attrs.Savable,  // saveable
                    attrs.Dynamic,  // dynamic
                    attrs.Gettable, // logable
                    defaultValue
                );
            }
            return pdescs;
        }

        /// <summary>
        /// Check DynamicProperty
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="id"></param>
        /// <param name="pdescs"></param>
        /// <returns></returns>
        private static bool CheckDynamicProperty(WrappedSimulator sim, string id, Dictionary<string, PropertyDescriptor> pdescs)
        {
            // Check DynamicProperty
            bool dynamic = true;
            try
            {
                string randomID = null;
                do
                {
                    randomID = Util.GenerateRandomID(32);
                }
                while (pdescs.ContainsKey(randomID));
                sim.SetEntityProperty(Util.BuildFullPN(id, randomID), 0.0);
            }
            catch (Exception)
            {
                dynamic = false;
            }
            return dynamic;
        }

    }

    /// <summary>
    /// InnerClass to manage DM file.
    /// </summary>
    public class DMModuleInfo
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        private string m_path;
        /// <summary>
        /// 
        /// </summary>
        private string m_moduleName;
        /// <summary>
        /// 
        /// </summary>
        private string m_description;
        #endregion

        #region Properties
        /// <summary>
        /// File Path.
        /// </summary>
        public string Path
        {
            get
            { return m_path; }
        }
        /// <summary>
        /// Name of DM
        /// </summary>
        public string ModuleName
        {
            get { return m_moduleName; }
        }
        /// <summary>
        /// Name of DM
        /// </summary>
        public string Description
        {
            get { return m_description; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        public DMModuleInfo(string path, DMInfo info)
            : this(path, info.ModuleName, info.Description)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="moduleName"></param>
        /// <param name="description"></param>
        public DMModuleInfo(string path, string moduleName, string description)
        {
            m_path = path;
            m_moduleName = moduleName;
            m_description = description;
        }
        #endregion
    }

}