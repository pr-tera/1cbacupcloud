using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace _1cbacupcloud3._5.Settings
{
    class Install
    {
        internal static bool GetAgent()
        {
            if (DownloadAgetn() == true)
            {
                if (InstallAgent() == true)
                {
                    Directory.Delete(Data.Path + @"\backupagentTemp", true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private static string Temp()
        {
            string backupagentTemp = Data.Path + @"\backupagentTemp";
            if (!Directory.Exists(backupagentTemp))
            {
                try
                {
                    Directory.CreateDirectory(backupagentTemp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Program.WritheLog(Convert.ToString(ex));
                }
            }
            return backupagentTemp;
        }
        private static bool DownloadAgetn()
        {
            string uri = "http://agentupdprod.hb.bizmrg.com/installAgent.exe";
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(uri, $"{Temp()}\\installagent.exe");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"При загрузке произошла ошибка:\n{ex}");
                Program.WritheLog(Convert.ToString(ex));

                return false;
            }
        }
        private static bool InstallAgent()
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = $"{Temp()}\\installagent.exe";
                process.Start();
                process.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Program.WritheLog(Convert.ToString(ex));
                return false;
            }
        }
    }
}
