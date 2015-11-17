using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace WinLog
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("すでに起動しています");
                return;
            }

            Application.Run(new Form1());
        }
    }

     class WinLog
    {
        public int SystemStartUp()
        {
            int s = System.Environment.TickCount / 1000;
            return s;
        }

        public int FolderToGraphCountGet(string path)
        {
            return System.IO.Directory.GetFiles(@path).Count();
        }

        public string[] FileRead(string path)
        {
            string[] str = new string[1];
            int i=0;
            StreamReader file = new System.IO.StreamReader(@path);

            while (!file.EndOfStream)
            {
                Array.Resize(ref str, i + 1);
                str[i] = file.ReadLine();
                i++;
            }
            file.Close();
            return str;
        }
    }
}
