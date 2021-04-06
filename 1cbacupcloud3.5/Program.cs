using _1cbacupcloud3._5.CloudAgent;
using _1cbacupcloud3._5.Local;
using System;
using System.Globalization;
using System.IO;

namespace _1cbacupcloud3._5
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.OutputEncoding = Encoding.UTF8;
            if (args[0] == "Upload")
            {
                Reqistry.GetKey();
                SendLogTo1C();
                Agent.Start();
                Log log = new Log();
                if (!string.IsNullOrEmpty(Data.Log))
                {
                    log.SendEmailAsync();
                }
                Log.Write();
            }
            else if (args[0] == "Clean")
            {
                if (GetParametrs.Get() == true)
                {
                    for (int i = 0; i != 2; i++)
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
            else if (args[0] == "test")
            {
                Log log = new Log();
                log.SendEmailAsync();
            }
        }
        private static void SendLogTo1C()
        {
            DateTime dateTime = DateTime.Now.Date;
            string log = null;
            Data.ScriptsVersion = GetParametrs.Version();
            GetParametrs.Get();
            try
            {
                IO.GetPath(Data.Path, Type.type[1], true, false, true);
            }
            catch (Exception ex)
            {
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    WritheLog(t);
                }
                else
                {
                    WritheLog("Не удалось получить путь к бекапу");
                }
            }
            try
            {
                IO.GetPath($@"{Data.ImagePathAgent}\logs", Type.type[4], false, true, false, dateTime);
            }
            catch (Exception ex)
            {
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    WritheLog(t);
                }
                else
                {
                    WritheLog("Не удалось получить путь к логу");
                }
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
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    WritheLog(t);
                }
                else
                {
                    WritheLog("Ошибка при получении пути к архиву логов");
                }
            }
            if (!string.IsNullOrEmpty(Data.LogGzPath))
            {
                if (IO.CreateDir(Data.PathSTemp) == true)
                {
                    IO.UnGzip(Data.LogGzPath, $@"{Data.PathSTemp}{Data.LogAgentOld}");
                }
                else
                {
                    IO.UnGzip(Data.LogGzPath, $@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}");
                }
            }
            if (Data.BackupNameList.Count != 0)
            {
                for (int i = 0; i != Data.BackupNameList.Count; i++)
                {
                    try
                    {
                        if (File.Exists(Data.BackupNameList[i]))
                        {
                            if (!File.Exists(Data.LogAgentOld) && File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgent}"))
                            {
                                log = $@"{Data.ImagePathAgent}\logs{Data.LogAgent}";
                            }
                            else if (File.Exists(Data.LogAgentOld) && !File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgent}"))
                            {
                                log = Data.LogAgentOld;
                            }
                            else if (!File.Exists(Data.LogAgentOld) && !File.Exists($@"{Data.ImagePathAgent}\logs{Data.LogAgent}"))
                            {
                                break;
                            }
                            FileInfo fileInfo = new FileInfo(Data.BackupNameList[i]);
                            Data.JsonTo1C = null;
                            if (!string.IsNullOrEmpty(Data.IbDUID[i]) && !string.IsNullOrEmpty(Data.Login))
                            {
                                bool srvr = false;
                                if (fileInfo.Extension == Type.type[0])
                                {
                                    srvr = true;
                                }
                                else if (fileInfo.Extension == Type.type[1])
                                {
                                    srvr = false;
                                }
                                diagnostics.GetLog(null, log, diagnostics.GetGUID(Data.IbDUID[i]), null, false, Math.Round((double)fileInfo.Length / 1024 / 1024 / 1024, 3), Data.Login, dateTime, srvr);
                                Agent.Send1C();
                            }
                            else
                            {
                                Data.Log += $"\nНе удалось найти GUID или логин итс{Environment.NewLine}";
                            }
                            if (!string.IsNullOrEmpty(Data.JsonTo1C))
                            {
                                Data.Log += $"\n{Data.JsonTo1C}{Environment.NewLine}";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string t = Convert.ToString(ex);
                        if (!string.IsNullOrEmpty(t))
                        {
                            WritheLog(t);
                        }
                        else
                        {
                            WritheLog("Ошибка при обработке логов");
                        }
                    }
                }
            }
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Data.PathSTemp);
                if (directoryInfo.Exists)
                {
                    directoryInfo.Delete(true);
                }
            }
            catch (Exception ex)
            {
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    WritheLog(t);
                }
                else
                {
                    WritheLog("Ошибка при удалении директории temp");
                }
            }
        }
        internal static void WritheLog(string temp)
        {
            CultureInfo culture = new CultureInfo("ru-RU");
            DateTime dateTime = DateTime.Now;
            string dt = Convert.ToString(dateTime, culture);
            string r = null;
            if (!string.IsNullOrEmpty(temp))
            {
                for (int i = 0; i < temp.Length + dt.Length; i++)
                {
                    r += "‾";
                }
                Data.Log += $"{dt}||{temp + Environment.NewLine + r + Environment.NewLine}";
            }
            else
            {
                for (int i = 0; i < 25; i++)
                {
                    r += "‾";
                }
                Data.Log += $"{dt}||{Environment.StackTrace + Environment.NewLine + r + Environment.NewLine}";
            }
        }
    }
}