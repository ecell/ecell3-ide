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
            //�֘A�t����ꂽ���s�t�@�C�����擾����t�@�C����
            string fileName =  args[0];
            //���ʂ��󂯎�邽�߂�StringBuilder�I�u�W�F�N�g
            System.Text.StringBuilder exePath =
                new System.Text.StringBuilder(255);
            //fileName�Ɋ֘A�t����ꂽ���s�t�@�C���̃p�X���擾����
            if (FindExecutable(fileName, null, exePath) > 32)
            {
                //�����������́AexePath�̓��e��\������
                Console.WriteLine(exePath.ToString());
                Process proc = new Process();
                proc.StartInfo.FileName = exePath.ToString();
                proc.StartInfo.Arguments = fileName;
                proc.Start();
                proc.WaitForExit();


            }
            else
            {
                Console.WriteLine("���s���܂����B");
            }
        }
    }
}
