using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace E_Cell_IDE_Installer
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        public static extern int FindExecutable(
            string lpFile, string lpDirectory,
            System.Text.StringBuilder lpResult);


        static void Main(string[] args)
        {
            //関連付けられた実行ファイルを取得するファイル名
            string fileName =  args[0];
            //結果を受け取るためのStringBuilderオブジェクト
            System.Text.StringBuilder exePath =
                new System.Text.StringBuilder(255);
            //fileNameに関連付けられた実行ファイルのパスを取得する
            if (FindExecutable(fileName, null, exePath) > 32)
            {
                //成功した時は、exePathの内容を表示する
                Console.WriteLine(exePath.ToString());
                Process proc = new Process();
                proc.StartInfo.FileName = exePath.ToString();
                proc.StartInfo.Arguments = fileName;
                proc.Start();
                proc.WaitForExit();


            }
            else
            {
                Console.WriteLine("失敗しました。");
            }
        }
    }
}
