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
            m_dmPaths = dmPaths;
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
            return m_descs[type][name];
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
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, List<PathAndModuleNamePair>>> Load()
        {
            Dictionary<string, Dictionary<string, List<PathAndModuleNamePair>>> maps =
                    new Dictionary<string, Dictionary<string, List<PathAndModuleNamePair>>>();
            Dictionary<string, Dictionary<string, List<PathAndModuleNamePair>>> modulesToLookup =
                    new Dictionary<string, Dictionary<string, List<PathAndModuleNamePair>>>();

            maps[Constants.xpathSystem] = new Dictionary<string, List<PathAndModuleNamePair>>();
            maps[Constants.xpathStepper] = new Dictionary<string, List<PathAndModuleNamePair>>();
            maps[Constants.xpathProcess] = new Dictionary<string, List<PathAndModuleNamePair>>();
            maps[Constants.xpathVariable] = new Dictionary<string, List<PathAndModuleNamePair>>();

            // 
            // Look for built-in modules
            // 
            {
                const string dmPath = "<BUILTIN>";
                Dictionary<string, List<PathAndModuleNamePair>> perDirectoryModuleList =
                    new Dictionary<string, List<PathAndModuleNamePair>>();
                perDirectoryModuleList[Constants.xpathSystem] = new List<PathAndModuleNamePair>();
                perDirectoryModuleList[Constants.xpathStepper] = new List<PathAndModuleNamePair>();
                perDirectoryModuleList[Constants.xpathProcess] = new List<PathAndModuleNamePair>();
                perDirectoryModuleList[Constants.xpathVariable] = new List<PathAndModuleNamePair>();

                modulesToLookup[dmPath] = perDirectoryModuleList;

                WrappedSimulator sim = new WrappedSimulator(new string[] { "" });
                foreach (DMInfo entry in sim.GetDMInfo())
                {
                    if (string.IsNullOrEmpty(entry.FileName))
                    {
                        perDirectoryModuleList[entry.TypeName].Add(
                            new PathAndModuleNamePair(dmPath, entry.ModuleName));
                    }
                }
                sim.Dispose();
            }

            //
            // Searches the DM paths
            //
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

                Dictionary<string, List<PathAndModuleNamePair>> perDirectoryModuleList =
                    new Dictionary<string, List<PathAndModuleNamePair>>();
                perDirectoryModuleList[Constants.xpathSystem] = new List<PathAndModuleNamePair>();
                perDirectoryModuleList[Constants.xpathStepper] = new List<PathAndModuleNamePair>();
                perDirectoryModuleList[Constants.xpathProcess] = new List<PathAndModuleNamePair>();
                perDirectoryModuleList[Constants.xpathVariable] = new List<PathAndModuleNamePair>();

                modulesToLookup[dmPath] = perDirectoryModuleList;

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

                    List<PathAndModuleNamePair> pairs = null;
                    maps[moduleType].TryGetValue(moduleName, out pairs);
                    if (pairs == null)
                    {
                        pairs = new List<PathAndModuleNamePair>();
                        maps[moduleType][moduleName] = pairs;
                    }

                    PathAndModuleNamePair pair = new PathAndModuleNamePair(modulePath, moduleName);
                    pairs.Add(pair);
                    perDirectoryModuleList[moduleType].Add(pair);
                }
            }

            Dictionary<string, Dictionary<string, DMDescriptor>> descs =
                new Dictionary<string, Dictionary<string, DMDescriptor>>();
            descs[Constants.xpathSystem] = new Dictionary<string, DMDescriptor>();
            descs[Constants.xpathProcess] = new Dictionary<string, DMDescriptor>();
            descs[Constants.xpathVariable] = new Dictionary<string, DMDescriptor>();
            descs[Constants.xpathStepper] = new Dictionary<string, DMDescriptor>();

            foreach (KeyValuePair<string, Dictionary<string, List<PathAndModuleNamePair>>> kv in modulesToLookup)
            {
                WrappedSimulator sim = new WrappedSimulator(new string[] { kv.Key });
                {
                    sim.CreateStepper("PassiveStepper", "tmp");
                    string id = Util.BuildFullPN(Constants.xpathSystem, "", "/", "StepperID");
                    sim.SetEntityProperty(id, "tmp");
                }
                Trace.WriteLine("Checking DMs in " + kv.Key);

                foreach (PathAndModuleNamePair pair in kv.Value[Constants.xpathSystem])
                {
                    Trace.WriteLine("Checking properties for " + pair.ModuleName);
                    Dictionary<string, PropertyDescriptor> pdescs =
                        new Dictionary<string, PropertyDescriptor>();
                    string id = Util.BuildFullID(Constants.xpathSystem, "/", pair.ModuleName);
                    sim.CreateEntity(pair.ModuleName, id);
                    sim.SetEntityProperty(Util.BuildFullPN(id, "StepperID"), "tmp");
                    sim.CreateEntity(Constants.xpathVariable, Util.BuildFullID(Constants.xpathVariable, "/" + pair.ModuleName, "SIZE"));
                    bool dynamic = true;
                    foreach (string propName in sim.GetEntityPropertyList(id))
                    {
                        string fullPN = Util.BuildFullPN(id, propName);
                        EcellCoreLib.PropertyAttributes attrs = sim.GetEntityPropertyAttributes(fullPN);
                        EcellValue defaultValue = null;
                        try
                        {
                            if (attrs.Gettable)
                                defaultValue = new EcellValue(sim.GetEntityProperty(fullPN));
                        }
                        catch (Exception)
                        {
                        }
                        pdescs[propName] = new PropertyDescriptor(
                            propName,
                            attrs.Settable, // settable
                            attrs.Gettable, // gettable
                            attrs.Loadable, // loadable
                            attrs.Savable,  // saveable
                            attrs.Dynamic,  // dynamic
                            attrs.Gettable,
                            defaultValue
                        );
                    }
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
                    descs[Constants.xpathSystem][pair.ModuleName] =
                        new DMDescriptor(pair.ModuleName, pair.Path, Constants.xpathSystem, dynamic, pdescs);
                }

                foreach (PathAndModuleNamePair pair in kv.Value[Constants.xpathProcess])
                {
                    Trace.WriteLine("Checking properties for " + pair.ModuleName);
                    Dictionary<string, PropertyDescriptor> pdescs =
                        new Dictionary<string, PropertyDescriptor>();
                    string id = Util.BuildFullID(Constants.xpathProcess, "/", pair.ModuleName);
                    sim.CreateEntity(pair.ModuleName, id);
                    bool dynamic = true;
                    foreach (string propName in sim.GetEntityPropertyList(id))
                    {
                        string fullPN = Util.BuildFullPN(id, propName);
                        PropertyAttributes attrs = sim.GetEntityPropertyAttributes(fullPN);
                        EcellValue defaultValue = null;
                        if (attrs.Gettable)
                        {
                            try
                            {
                                defaultValue = new EcellValue(sim.GetEntityProperty(fullPN));
                            }
                            catch (Exception) { }
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
                    descs[Constants.xpathProcess][pair.ModuleName] =
                        new DMDescriptor(pair.ModuleName, pair.Path, Constants.xpathProcess, dynamic, pdescs);
                }

                foreach (PathAndModuleNamePair pair in kv.Value[Constants.xpathVariable])
                {
                    Trace.WriteLine("Checking properties for " + pair.ModuleName);
                    Dictionary<string, PropertyDescriptor> pdescs =
                        new Dictionary<string, PropertyDescriptor>();
                    string id = Util.BuildFullID(Constants.xpathVariable, "/", pair.ModuleName);
                    sim.CreateEntity(pair.ModuleName, id);
                    bool dynamic = true;
                    foreach (string propName in sim.GetEntityPropertyList(id))
                    {
                        string fullPN = Util.BuildFullPN(id, propName);
                        PropertyAttributes attrs = sim.GetEntityPropertyAttributes(fullPN);
                        EcellValue defaultValue = null;
                        if (attrs.Gettable)
                        {
                            try
                            {
                                defaultValue = new EcellValue(sim.GetEntityProperty(fullPN));
                            }
                            catch (Exception) { }
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
                    descs[Constants.xpathVariable][pair.ModuleName] =
                        new DMDescriptor(pair.ModuleName, pair.Path, Constants.xpathVariable, dynamic, pdescs);
                }

                foreach (PathAndModuleNamePair pair in kv.Value[Constants.xpathStepper])
                {
                    Trace.WriteLine("Checking properties for " + pair.ModuleName);
                    Dictionary<string, PropertyDescriptor> pdescs =
                        new Dictionary<string, PropertyDescriptor>();
                    sim.CreateStepper(pair.ModuleName, pair.ModuleName);
                    bool dynamic = true;
                    foreach (string propName in sim.GetStepperPropertyList(pair.ModuleName))
                    {
                        PropertyAttributes attrs = sim.GetStepperPropertyAttributes(pair.ModuleName, propName);
                        EcellValue defaultValue = null;
                        if (attrs.Gettable)
                        {
                            try
                            {
                                defaultValue = new EcellValue(sim.GetStepperProperty(pair.ModuleName, propName));
                            }
                            catch (Exception) { }
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
                    try
                    {
                        string randomID = null;
                        do
                        {
                            randomID = Util.GenerateRandomID(32);
                        }
                        while (pdescs.ContainsKey(randomID));
                        sim.SetStepperProperty(pair.ModuleName, randomID, 0.0);
                    }
                    catch (Exception)
                    {
                        dynamic = false;
                    }
                    descs[Constants.xpathStepper][pair.ModuleName] =
                        new DMDescriptor(pair.ModuleName, pair.Path, Constants.xpathStepper, dynamic, pdescs);
                }
                try
                {
                    sim.Dispose();
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.StackTrace);
                }
            }

            m_descs = descs;
            return maps;
        }
    }

    /// <summary>
    /// InnerClass to manage DM file.
    /// </summary>
    public class PathAndModuleNamePair
    {
        #region MyRegion
        /// <summary>
        /// 
        /// </summary>
        private string m_path;
        /// <summary>
        /// 
        /// </summary>
        private string m_moduleName;
        #endregion

        #region MyRegion
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
        #endregion

        #region MyRegion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="moduleName"></param>
        public PathAndModuleNamePair(string path, string moduleName)
        {
            m_path = path;
            m_moduleName = moduleName;
        }
        #endregion
    }

}