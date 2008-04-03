//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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

namespace EcellLib.MainWindow
{
    /// <summary>
    /// ZipUtil
    /// </summary>
    public class ZipUtil
    {
        /// <summary>
        /// Save Zipped File.
        /// </summary>
        /// <param name="zipname"></param>
        /// <param name="filePath"></param>
        public static void ZipFile(string zipname, string filePath)
        {
            if (!File.Exists(filePath))
                return;
            string[] filePaths = {filePath};
            ZipFiles(zipname, filePaths);
        }

        /// <summary>
        /// Save Zipped File.
        /// </summary>
        /// <param name="zipname"></param>
        /// <param name="filePaths"></param>
        public static void ZipFiles(string zipname, string[] filePaths)
        {
            FileStream zipwriter = null;
            ZipOutputStream zos = null;
            Crc32 crc = new Crc32();

            try
            {
                zipwriter = new FileStream(
                     zipname,
                     FileMode.Create,
                     FileAccess.Write,
                     FileShare.Write);
                zos = new ZipOutputStream(zipwriter);
                zos.SetLevel(6);

                //Add files.
                foreach (string filePath in filePaths)
                {
                    if (!File.Exists(filePath))
                        return;

                    string filename = Path.GetFileName(filePath);
                    ZipEntry ze = new ZipEntry(filename);

                    FileStream fs = new FileStream(
                        filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);
                    ze.Crc = crc.Value;
                    ze.Size = buffer.Length;
                    ze.DateTime = DateTime.Now;

                    zos.PutNextEntry(ze);
                    zos.Write(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                if(zos != null)
                    zos.Close();
                if (zipwriter != null)
                    zipwriter.Close();
            }
        }

        /// <summary>
        /// Zip Folder
        /// </summary>
        /// <param name="zipname"></param>
        /// <param name="folderPath"></param>
        public static void ZipFolder(string zipname, string folderPath)
        {
            FileStream zipwriter = null;
            ZipOutputStream zos = null;
            Crc32 crc = new Crc32();

            string[] filePaths = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            folderPath = Path.GetDirectoryName(folderPath);
            try
            {
                zipwriter = new FileStream(
                     zipname,
                     FileMode.Create,
                     FileAccess.Write,
                     FileShare.Write);
                zos = new ZipOutputStream(zipwriter);
                zos.SetLevel(6);

                //Add files.
                foreach (string filePath in filePaths)
                {
                    if (!File.Exists(filePath))
                        return;

                    string filename = GetRelativePath(folderPath, filePath);
                    ZipEntry ze = new ZipEntry(filename);

                    FileStream fs = new FileStream(
                        filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);
                    ze.Crc = crc.Value;
                    ze.Size = buffer.Length;
                    ze.DateTime = DateTime.Now;

                    zos.PutNextEntry(ze);
                    zos.Write(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                if (zos != null)
                    zos.Close();
                if (zipwriter != null)
                    zipwriter.Close();
            }

        }

        /// <summary>
        /// Get Relative Path.
        /// </summary>
        /// <param name="targetPath"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string targetPath, string filePath)
        {
            string relativePath = filePath.Replace(targetPath, "");
            return relativePath;
        }
    }
}
