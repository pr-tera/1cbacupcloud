using _1cbacupcloud3._5.CloudAgent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;

namespace _1cbacupcloud3._5
{
    class diagnostics
    {
        internal static string DigLog { get; set; }
        internal static int DateCon { get; set; } = 1;
        private static bool CheckParam()
        {
            bool _check = false;
            try
            {
                if (File.Exists(Data.Path + @"\Parametrs.xml"))
                {
                    if (GetParametrs.Get() == true)
                    {
                        _check = true;
                    }
                    else
                    {
                        _check = false;
                        DigLog += "XM0001";
                    }
                }
                else
                {
                    _check = false;
                    DigLog += "XM0003";
                }
            }
            catch (Exception ex)
            {
                string tt = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(tt))
                {
                    Program.WritheLog(tt);
                }
                else
                {
                    Program.WritheLog("Ошибка чтения файла Parametrs.xml");
                }
                _check = false;
            }
            return _check;
        }
        private static bool CheckService()
        {
            try
            {
                if (!string.IsNullOrEmpty(Local.Reqistry.AgentServiceName()))
                {
                    ServiceController service = new ServiceController(Local.Reqistry.AgentServiceName());
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.Stopped:
                            Data.ServiceStatus = false;
                            break;
                        case ServiceControllerStatus.Running:
                            Data.ServiceStatus = true;
                            break;
                        case ServiceControllerStatus.StopPending:
                            Data.ServiceStatus = false;
                            break;
                        case ServiceControllerStatus.ContinuePending:
                            Data.ServiceStatus = false;
                            break;
                        case ServiceControllerStatus.Paused:
                            Data.ServiceStatus = false;
                            break;
                        case ServiceControllerStatus.PausePending:
                            Data.ServiceStatus = false;
                            break;
                        case ServiceControllerStatus.StartPending:
                            Data.ServiceStatus = true;
                            break;
                    }
                    service.Dispose();
                }
                else
                {
                    Data.ServiceStatus = false;
                }
            }
            catch
            {
                Data.ServiceStatus = false;
                DigLog += "SR1001";
            }
            return Data.ServiceStatus;
        }
        internal static void GetLog(string id, string logFile, string db_id, string messageto1c, bool status, double ibsize, string itslogin, DateTime timestamp, bool srvr, bool oldlog = false)
        {
            List<string> m_logFile = new List<string>();
            To1C to1C;
            var settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK",
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            try
            {
                using (var logFileStream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var logFileSr = new StreamReader(logFileStream))
                {
                    while (!logFileSr.EndOfStream)
                    {
                        m_logFile.Add(logFileSr.ReadLine());
                    }
                }
                DateTime dateTime = DateTime.Now;
                if (string.IsNullOrEmpty(id) &&
                    !string.IsNullOrEmpty(logFile) &&
                    !string.IsNullOrEmpty(db_id) &&
                    string.IsNullOrEmpty(messageto1c) &&
                    !string.IsNullOrEmpty(itslogin) &&
                    m_logFile.Count != 0)
                {
                    bool tmp = false;
                    foreach (var str in m_logFile)
                    {
                        LogAgent.Root root = JsonConvert.DeserializeObject<LogAgent.Root>(str);
                        if (root.Timestamp.Day == dateTime.Day - DateCon && root.Timestamp.Month == dateTime.Month && !string.IsNullOrEmpty(root.BackupID) && root.message.Contains("START create archive by backup row with") && root.message.Contains(db_id))
                        {
                            tmp = true;
                            GetLog(root.BackupID, logFile, db_id, null, false, ibsize, itslogin, timestamp, srvr);
                            break;
                        }
                    }
                    if (tmp == false && DateCon != 0)
                    {
                        tmp = true;
                        DateCon = 0;
                        GetLog(null, Data.LogAgent, db_id, null, false, ibsize, itslogin, timestamp, srvr);
                    }
                    else
                    {
                        CheckParam();
                        CheckService();
                        to1C = new To1C { ibid = GetGUID(db_id), ibsize = ibsize, itslogin = itslogin, message = DigLog, status = status, timestamp = timestamp, srvr = srvr };
                    }
                }
                else if (!string.IsNullOrEmpty(id) &&
                    !string.IsNullOrEmpty(logFile) &&
                    !string.IsNullOrEmpty(db_id) &&
                    string.IsNullOrEmpty(messageto1c) &&
                    !string.IsNullOrEmpty(itslogin) &&
                    m_logFile.Count != 0)
                {
                    foreach (var str in m_logFile)
                    {
                        LogAgent.Root root = JsonConvert.DeserializeObject<LogAgent.Root>(str);
                        if (root.Timestamp.Day == dateTime.Day - DateCon && root.Timestamp.Month == dateTime.Month && root.BackupID == id && root.message.Contains("OK"))
                        {
                            Data.BackupStatus = true;
                            GetLog(id, logFile, db_id, root.message, true, ibsize, itslogin, root.Timestamp, srvr);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(id) &&
                    !string.IsNullOrEmpty(logFile) &&
                    !string.IsNullOrEmpty(db_id) &&
                    !string.IsNullOrEmpty(messageto1c) &&
                    !string.IsNullOrEmpty(itslogin))
                {
                    if (CheckParam() == true &&
                        CheckService() == true &&
                        ibsize > Data.MinSizeBackup)
                    {
                        to1C = new To1C { ibid = GetGUID(db_id), ibsize = ibsize, itslogin = itslogin, message = messageto1c, status = status, timestamp = timestamp, srvr = srvr };
                    }
                    else
                    {
                        to1C = new To1C { ibid = GetGUID(db_id), ibsize = ibsize, itslogin = itslogin, message = "IO0002", status = false, timestamp = timestamp, srvr = srvr };
                    }
                    Data.JsonTo1C = JsonConvert.SerializeObject(to1C, settings);
                }
                if (string.IsNullOrEmpty(Data.JsonTo1C))
                {
                    if (string.IsNullOrEmpty(db_id))
                    {
                        db_id = "null_error";
                    }
                    if (db_id.Contains("_"))
                    {

                    }
                    to1C = new To1C { ibid = GetGUID(db_id), ibsize = ibsize, itslogin = itslogin, message = null, status = false, timestamp = timestamp, srvr = srvr };
                    Data.JsonTo1C = JsonConvert.SerializeObject(to1C, settings);
                }
            }
            catch (Exception ex)
            {
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    Program.WritheLog(t);
                }
                else
                {
                    Program.WritheLog("Не зарегистрированная ошибка при обработке логов");
                }
                if (string.IsNullOrEmpty(Data.JsonTo1C))
                {
                    to1C = new To1C { ibid = GetGUID(db_id), ibsize = ibsize, itslogin = itslogin, message = ex.ToString(), status = false, timestamp = timestamp, srvr = srvr };
                    Data.JsonTo1C = JsonConvert.SerializeObject(to1C, settings);
                }
            }
        }
        internal static string GetGUID(string guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                while (guid.Contains("_"))
                {
                    guid = guid.Substring(guid.IndexOf("_") + 1);
                }
            }
            return guid;
        }
    }
}
