using System;
using System.IO;
using _1cbacupcloud3._5.CloudAgent;
using _1cbacupcloud3._5.Local;

namespace _1cbacupcloud3._5
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.OutputEncoding = Encoding.UTF8;
            if (args[0] == "Upload")
            {
                try
                {
                    Reqistry.GetKey();
                    SendLogTo1C();
                    Agent.Start();
                    Log.Write();
                }
                catch (Exception ex)
                {
                    Data.Log += ex;
                    Log.Write();
                }
            }
            else if (args[0] == "Clean")
            {
                if (GetParametrs.Get() == true)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Reqistry.GetKey();
                        IO.CleanOldBackup(Data.Path, Type.type[i]);
                        Log.Write();
                    }
                }
                else
                {
                    Log.Write();
                }
            }
        }
        private static void SendLogTo1C()
        {
            try
            {
                DateTime dateTime = DateTime.Now.Date;
                GetParametrs.Get();
                IO.GetPath(Data.Path, Type.type[1], true, false, true);
                IO.GetPath($@"{Data.ImagePathAgent}\logs", Type.type[4], false, true, false, dateTime);
                IO.GetPath($@"{Data.ImagePathAgent}\logs", Type.type[3], false, false, false, dateTime);
                for (int i = 0; i != 2; i++)
                {
                    IO.GetPath(Data.Path, Type.type[i]);
                }
                IO.UnGzip(Data.LogGzPath, $@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}");
                for (int i = 0; i != Data.BackupNameList.Count; i++)
                {
                    if (File.Exists(Data.BackupNameList[i]))
                    {
                        FileInfo fileInfo = new FileInfo(Data.BackupNameList[i]);
                        diagnostics.GetLog(null, $@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}", Data.IbDUID[i], null, false, Math.Round((double)fileInfo.Length / 1024 / 1024 / 1024, 3), Data.Login, dateTime);
                        Agent.Send1C();
                    }
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не зарегистрированная ошибка:\n{ex}\n";
            }
        }
    }
}
