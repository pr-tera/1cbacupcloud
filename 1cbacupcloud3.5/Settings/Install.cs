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
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Data.Log += $"{DateTime.Now} {e} \n";
                }
            }
            //else
            //{
            //    Directory.Delete(backupagentTemp, true);
            //    Temp();
            //}
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
            catch (Exception e)
            {
                Console.WriteLine($"При загрузке произошла ошибка:\n{e}");
                Data.Log += $"{DateTime.Now} {e} \n";
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                Data.Log += $"{DateTime.Now} {e} \n";
                return false;
            }
        }
    }
}
