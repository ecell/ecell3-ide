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
    public class GlobusJob : Job
    {
        public GlobusJob()
            : base()
        {
            this.JobDirectory = "";            
        }

        public override void retry()
        {
            this.stop();
            this.Status = JobStatus.QUEUED;
            this.run();
        }

        public override void run()
        {
            // not implements
            // 初期化
            // grid-proxy-init
            // 実行
            // cog-job-submit -e $script -args $ROOT/$JobID/$jobfile -p $provider -s $server
        }

        public override void stop()
        {
            // not implements
        }

        public override void Update()
        {
            // not implements
        }

        public override string GetStdOut()
        {
            // not implements
            return base.GetStdOut();
        }

        public override string GetStdErr()
        {
            // not implements
            return base.GetStdErr();
        }

        public override void PrepareProcess()
        {
            // not implements
            // 初期化
            // grid-proxy-init
            // 実行ディレクトリを作成
            // cog-job-submit -e /bin/mkdir -args $ROOT/$JobID -p $Provider -s $Server           
            // grid-ftpでサーバにスクリプトを持っていく
            // cog-file-transfer -s $jobfile -d $ROOT/$JobID
            base.PrepareProcess();
        }

        public override Dictionary<double, double> GetLogData(string key)
        {
            // not implements
            return base.GetLogData(key);
        }

        static public string GetDefaultScript()
        {
            return "";
        }
    }
}
