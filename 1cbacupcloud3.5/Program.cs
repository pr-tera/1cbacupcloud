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
            DateTime dateTime = DateTime.Now.Date;
            string log = null;
            try
            {
                GetParametrs.Get();
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{DateTime.Now} Не удалось получить параметры {ex}\n";
            }
            try
            {
                IO.GetPath(Data.Path, Type.type[1], true, false, true);
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{DateTime.Now} Не удалось получить путь к бекапу {ex}\n";
            }
            try
            {
                IO.GetPath($@"{Data.ImagePathAgent}\logs", Type.type[4], false, true, false, dateTime);
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{DateTime.Now} Не удалось получить путь к логу {ex}\n";
            }
            try
            {
                IO.GetPath($@"{Data.ImagePathAgent}\logs", Type.type[3], false, false, false, dateTime);
                for (int i = 0; i != 2; i++)
                {
                    IO.GetPath(Data.Path, Type.type[i]);
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{DateTime.Now} Ошибка при получении пути к архиву логов {ex}\n";
            }
            try
            {
                if (!string.IsNullOrEmpty(Data.LogGzPath))
                {
                    IO.UnGzip(Data.LogGzPath, $@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}");
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{DateTime.Now} Ошибка при распаковке архива {ex}\n";
            }
            try
            {
                for (int i = 0; i != Data.BackupNameList.Count; i++)
                {
                    if (File.Exists(Data.BackupNameList[i]))
                    {
                        if (!File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}") && File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgent}"))
                        {
                            log = $@"{Data.ImagePathAgent}\logs{Data.LogAgent}";
                        }
                        else if (File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}") && !File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgent}"))
                        {
                            log = $@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}";
                        }
                        else if (!File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}") && !File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgent}"))
                        {
                            break;
                        }
                        FileInfo fileInfo = new FileInfo(Data.BackupNameList[i]);
                        diagnostics.GetLog(null, log, Data.IbDUID[i], null, false, Math.Round((double)fileInfo.Length / 1024 / 1024 / 1024, 3), Data.Login, dateTime);
                        Agent.Send1C();
                        //Data.Log += $"\n{Data.JsonTo1C}\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{DateTime.Now} Ошибка при обработке логов {ex}\n";
            }
        }
    }
}