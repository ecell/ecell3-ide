using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace E_Cell_IDE_Installer
{
    class Program
    {
        [DllImport("shell32.dll")]
        private static extern uint ShellExecute(
            IntPtr hwnd,
            string lpVerb, string lpFile, string lpParameters,
            string lpDirectory, int nShowCmd);

        [DllImport("kernel32.dll")]
        private static extern int FormatMessageW(
            uint dwFlags, IntPtr lpSource,
            uint dwMessageId, uint dwLanguageId,
            out IntPtr lpMsgBuf, int nSize, IntPtr args);

        [FlagsAttribute]
        private enum FormatMessageFlags: uint
        {
            ALLOCATE_BUFFER = 0x00000100,
            IGNORE_INSERTS = 0x00000200,
            FROM_STRING = 0x00000400,
            FROM_HMODULE = 0x00000800,
            FROM_SYSTEM = 0x00001000,
            ARGUMENT_ARRAY = 0x00002000,
            MAX_WIDTH_MASK = 0x000000FF
        };

        [DllImport("kernel32.dll")]
        private static extern void LocalFree(IntPtr lpPtr);

        static void Main(string[] args)
        {
            //関連付けられた実行ファイルを取得するファイル名
            string fileName =  args[0];
            //fileNameに関連付けられた実行ファイルのパスを取得する
            uint retval = ShellExecute(
                IntPtr.Zero,
                "Compile",
                fileName, null,
                Directory.GetCurrentDirectory(),
                0);
            if (retval <= 32)
            {
                IntPtr lpMsg;
                FormatMessageW(
                    (uint)(FormatMessageFlags.ALLOCATE_BUFFER | FormatMessageFlags.FROM_SYSTEM),
                    IntPtr.Zero, retval, (uint)0, out lpMsg, 0,
                    IntPtr.Zero);
                try
                {
                    Console.WriteLine("失敗しました (" + retval + ": " + Marshal.PtrToStringUni(lpMsg).TrimEnd() + ")");
                }
                finally
                {
                    LocalFree(lpMsg);
                }
            }
        }
    }
}
