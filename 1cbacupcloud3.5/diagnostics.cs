﻿using System;
using System.IO;
using System.ServiceProcess;
using Newtonsoft.Json;
using _1cbacupcloud3._5.CloudAgent;
using System.Collections.Generic;

namespace _1cbacupcloud3._5
{
    class diagnostics
    {
        internal static string DigLog { get; set; }
        internal static bool Dig;
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
                Data.Log += $"{DateTime.Now} Не зарегистрированная ошибка:\n{ex}\n";
                _check = false;
            }
            return _check;
        }
        private static bool CheckService()
        {
            bool _check = false;
            try
            {
                if (!string.IsNullOrEmpty(Local.Reqistry.AgentServiceName()))
                {
                    ServiceController service = new ServiceController(Local.Reqistry.AgentServiceName());
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.Stopped:
                            _check = false;
                            break;
                        case ServiceControllerStatus.Running:
                            _check = true;
                            break;
                        case ServiceControllerStatus.StopPending:
                            _check = false;
                            break;
                        case ServiceControllerStatus.ContinuePending:
                            _check = false;
                            break;
                        case ServiceControllerStatus.Paused:
                            _check = false;
                            break;
                        case ServiceControllerStatus.PausePending:
                            _check = false;
                            break;
                        case ServiceControllerStatus.StartPending:
                            _check = true;
                            break;
                    }
                    service.Dispose();
                }
                else
                {
                    _check = false;
                }
            }
            catch
            {
                _check = false;
                DigLog += "SR1001";
            }
            return _check;
        }
        internal static void GetLog(string id, string logFile, string db_id, string messageto1c, bool status, double ibsize, string itslogin, DateTime timestamp, bool oldlog = false)
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
                    !string.IsNullOrEmpty(itslogin))
                {
                    bool tmp = false;
                    foreach (var str in m_logFile)
                    {
                        LogAgent.Root root = JsonConvert.DeserializeObject<LogAgent.Root>(str);
                        if (root.Timestamp.Day == dateTime.Day - DateCon && root.Timestamp.Month == dateTime.Month && !string.IsNullOrEmpty(root.BackupID) && root.message.Contains("RETRY COUNT operation") && root.message.Contains(db_id))
                        {
                            tmp = true;
                            GetLog(root.BackupID, logFile, db_id, null, false, ibsize, itslogin, timestamp);
                            break;
                        }
                    }
                    if (tmp == false && DateCon != 0)
                    {
                        tmp = true;
                        DateCon = 0;
                        GetLog(null, Data.LogAgent, db_id, null, false, ibsize, itslogin, timestamp);
                    }
                    else
                    {
                        CheckParam();
                        CheckService();
                        to1C = new To1C { ibid = db_id.Substring(db_id.IndexOf('_') + 1), ibsize = ibsize, itslogin = itslogin, message = DigLog, status = status, timestamp = timestamp };
                    }
                }
                else if (!string.IsNullOrEmpty(id) &&
                    !string.IsNullOrEmpty(logFile) &&
                    !string.IsNullOrEmpty(db_id) &&
                    string.IsNullOrEmpty(messageto1c) &&
                    !string.IsNullOrEmpty(itslogin))
                {
                    foreach (var str in m_logFile)
                    {
                        LogAgent.Root root = JsonConvert.DeserializeObject<LogAgent.Root>(str);
                        if (root.Timestamp.Day == dateTime.Day - DateCon && root.Timestamp.Month == dateTime.Month && root.BackupID == id && root.message.Contains("OK"))
                        {
                            GetLog(id, logFile, db_id, root.message, true, ibsize, itslogin, root.Timestamp);
                        }
                        //else error
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
                        to1C = new To1C { ibid = db_id.Substring(db_id.IndexOf('_') + 1), ibsize = ibsize, itslogin = itslogin, message = messageto1c, status = status, timestamp = timestamp };
                    }
                    else
                    {
                        to1C = new To1C { ibid = db_id.Substring(db_id.IndexOf('_') + 1), ibsize = ibsize, itslogin = itslogin, message = "IO0002", status = false, timestamp = timestamp };
                    }
                    Data.JsonTo1C = JsonConvert.SerializeObject(to1C, settings);
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не зарегистрированная ошибка:\n{ex}\n";
            }
        }
    }
}
