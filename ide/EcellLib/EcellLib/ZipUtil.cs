//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using Ecell.Exceptions;
using System.Threading;

namespace Ecell.IDE
{
    /// <summary>
    /// ZipUtil
    /// </summary>
    public class ZipUtil
    {
        private string output = null;
        private string input = null;
        /// <summary>
        /// Save Zipped File.
        /// </summary>
        /// <param name="zipname"></param>
        /// <param name="filePath"></param>
        public void ZipFile(string zipname, string filePath)
        {
            output = zipname;
            input = filePath;

            Thread thread = new Thread(new ThreadStart(ZipFile));
            thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ZipFile()
        {
            try
            {
                FastZip fz = new FastZip();
                fz.CreateZip(output, input, false, "");

                Util.ShowNoticeDialog(string.Format(MessageResources.InfoExportFile, output));
            }
            catch (Exception e)
            {
                throw new EcellException(string.Format(MessageResources.ErrSaveZip, output), e);
            }

        }

        /// <summary>
        /// Zip Folder
        /// </summary>
        /// <param name="zipname"></param>
        /// <param name="folderPath"></param>
        public void ZipFolder(string zipname, string folderPath)
        {
            output = zipname;
            input = folderPath;

            Thread thread = new Thread(new ThreadStart(ZipFolder));
            thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ZipFolder()
        {
            try
            {
                FastZip fz = new FastZip();
                fz.CreateZip(output, input, true, "", "");

                Util.ShowNoticeDialog(string.Format(MessageResources.InfoExportFile, output));
            }
            catch (Exception e)
            {
                throw new EcellException(string.Format(MessageResources.ErrSaveZip, output), e);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>temporary project file name.</returns>
        public string UnzipProject(string filename)
        {
            output = null;
            // Check File
            if (!File.Exists(filename))
                throw new EcellException(string.Format(MessageResources.ErrLoadPrj, filename));

            // Extract zip
            string dir = Path.Combine(Util.GetTmpDir(), Path.GetRandomFileName());
            FastZip fz = new FastZip();
            fz.ExtractZip(filename, dir, null);

            // Check Project File.
            string project = Path.Combine(dir,Constants.fileProjectXML);
            string info = Path.Combine(dir,Constants.fileProjectInfo);
            if(File.Exists(project))
                output = project;
            else if(File.Exists(info))
                output = info;

            if (output == null)
            {
                Directory.Delete(dir);
                throw new EcellException(string.Format(MessageResources.ErrLoadPrj, filename));
            }

            // Return project path.
            return output;
        }
    }
}
