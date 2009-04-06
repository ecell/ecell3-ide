//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
    /// SystemProxy to execute the simulation in Globus Environment,
    /// </summary>
    public class GlobusJobProxy : JobProxy
    {
        #region Fields
        private Dictionary<string, object> m_optDic = new Dictionary<string, object>();
        static private Dictionary<string, object> s_optDic = null;
        #endregion

        /// <summary>
        /// Constructors
        /// </summary>
        public GlobusJobProxy()
            : base()
        {
            GlobusJobProxy.Initialize();
            this.DefaultConcurrency = 4;
            m_optDic = s_optDic;
        }

        private static void Initialize()
        {
            if (s_optDic == null)
            {
                s_optDic.Add("servername", "globusserver.e-cell.org");
                s_optDic.Add("provider", "gt4");
                s_optDic.Add("script", "/usr/local/bin/ecell3-sesiion");
                s_optDic.Add("password", "XXX");
            }
        }

        /// <summary>
        /// Create the proxy of session.
        /// </summary>
        /// <returns>Return Job.</returns>
        public override Job CreateJob()
        {
            return new GlobusJob();
        }

        /// <summary>
        /// Create the proxy of session with initial parameters.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="arg"></param>
        /// <param name="extFile"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public override Job CreateJob(string script, string arg, List<string> extFile, string tmpDir)
        {
            GlobusJob job = new GlobusJob();
            job.ScriptFile = script;
            job.Argument = arg;
            job.ExtraFileList = extFile;
            job.JobDirectory = tmpDir + "/" + job.JobID;
            return job;
        }

        /// <summary>
        /// Get the environment name. This class return "Globus".
        /// </summary>
        public override string Name
        {
            get { return "Globus"; }
        }

        /// <summary>
        /// Get the flag whether this script use IDE function.
        /// </summary>
        /// <returns>return false.</returns>
        public override bool IsIDE()
        {
            return false;
        }

        /// <summary>
        /// Get the list of option set all session.
        /// </summary>
        /// <returns>dictionary of option.</returns>
        public override Dictionary<string, object> GetDefaultProperty()
        {
            GlobusJobProxy.Initialize();
            return s_optDic;
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
            return GlobusJob.GetDefaultScript();
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
    }

}
