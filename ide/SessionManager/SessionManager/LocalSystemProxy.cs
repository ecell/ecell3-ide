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

namespace SessionManager
{
    public class LocalSystemProxy : SystemProxy
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalSystemProxy()
            : base()
        {
            this.DefaultConcurrency = 1;
        }

        /// <summary>
        /// Create the proxy for session.
        /// </summary>
        /// <returns>LocalSessionProxy.</returns>
        public new SessionProxy CreateSessionProxy()
        {
            return new LocalSessionProxy();
        }

        /// <summary>
        /// Execute the queue session in Local.
        /// </summary>
        public new void Update()
        {
            int dispatchNum =  Manager.Concurrency - Manager.GetRunningJobList().Count;
            if (dispatchNum != 0)
            {
                foreach (SessionProxy p in Manager.SessionList.Values)
                {
                    if (p.Status == JobStatus.QUEUED)
                    {
                        p.run();
                        dispatchNum--;

                        if (dispatchNum <= 0) break;
                    }
                }
            }

        }

    }
}
