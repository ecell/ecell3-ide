//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Job
{
    /// <summary>
    /// SystemProxy to execute the simulation in Local Environment.
    /// </summary>
    public class LocalJobProxy : JobProxy
    {
        private Dictionary<string, object> m_optDic = new Dictionary<string, object>();
        static private Dictionary<string, object> s_optDic = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalJobProxy()
            : base()
        {
            LocalJobProxy.Initialize();
            this.DefaultConcurrency = 1;
            m_optDic = s_optDic;
        }

        private static void Initialize()
        {
            if (s_optDic == null)
            {
                s_optDic = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// Create the proxy for session.
        /// </summary>
        /// <returns>Return Job.</returns>
        public override Job CreateJob()
        {
            return new LocalJob();
        }

        /// <summary>
        /// Create the proxy for session with initial parameters.
        /// </summary>
        /// <param name="script">script file name.</param>
        /// <param name="arg">argument of script file.</param>
        /// <param name="extFile">extra file of script file.</param>
        /// <param name="tmpDir">tmp directory of script file</param>
        /// <returns>Class of proxy for session.</returns>
        public override Job CreateJob(string script, string arg, List<string> extFile, string tmpDir)
        {
            LocalJob job =  new LocalJob();
            job.ScriptFile = script;
            job.Argument = arg;
            job.ExtraFileList = extFile;
            job.JobDirectory = tmpDir + "/" + job.JobID;
            return job;
        }

        /// <summary>
        /// Get the environment name. This class return "Local".
        /// </summary>
        /// <returns>"Local".</returns>
        public override string Name
        {
            get { return "Local"; }
        }

        /// <summary>
        /// Execute the queue session in Local.
        /// </summary>
        public override void Update()
        {
            int dispatchNum = Manager.Concurrency - Manager.GetRunningJobList().Count;
            if (dispatchNum != 0)
            {
                foreach (Job p in Manager.GetQueuedJobList())
                {
                    p.run();
                    dispatchNum--;

                    if (dispatchNum <= 0)
                        break;
                }
            }
        }

        /// <summary>
        /// Get the flag whether this script use IDE functions.
        /// </summary>
        /// <returns>return true.</returns>
        public override bool IsIDE()
        {
            return true;
        }

        /// <summary>
        /// Set the list of option to all session.
        /// </summary>
        /// <param name="list">property dictionary.</param>
        public override void SetProperty(Dictionary<string, object> list)
        {
            m_optDic = list;
        }

        /// <summary>
        /// Get the list of option set all session.
        /// </summary>
        /// <returns>dictionary of option.</returns>
        public override Dictionary<string, object> GetDefaultProperty()
        {
            LocalJobProxy.Initialize();
            return LocalJobProxy.s_optDic;
        }

        /// <summary>
        /// Get the list of option set all session.
        /// </summary>
        /// <returns>dictionary of option.</returns>
        public override Dictionary<string, object> GetProperty()
        {
            return m_optDic;
        }

        /// <summary>
        /// Get the script file name.
        /// </summary>
        /// <returns>the script file name.</returns>
        public override string GetDefaultScript()
        {
            return LocalJob.GetDefaultScript();
        }
    }
}
