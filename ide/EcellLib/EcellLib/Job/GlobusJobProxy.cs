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
    public class GlobusJobProxy : JobProxy
    {
        public GlobusJobProxy()
            : base()
        {
            GlobusJobProxy.Initialize();    
        }

        private static void Initialize()
        {
        }

        public override Job CreateJob()
        {
            return new GlobusJob();
        }

        public override Job CreateJob(string script, string arg, List<string> extFile, string tmpDir)
        {
            GlobusJob job = new GlobusJob();
            job.ScriptFile = script;
            job.Argument = arg;
            job.ExtraFileList = extFile;
            job.JobDirectory = tmpDir + "/" + job.JobID;
            return job;
        }

        public override string Name
        {
            get { return "Globus"; }
        }

        public override bool IsIDE()
        {
            return false;
        }

        public override Dictionary<string, object> GetDefaultProperty()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override Dictionary<string, object> GetProperty()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetDefaultScript()
        {
            return GlobusJob.GetDefaultScript();
        }

        public override void SetProperty(Dictionary<string, object> list)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Update()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

}
