using System;
using System.IO;
using System.ServiceProcess;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;
using _1cbacupcloud3._5.CloudAgent;

namespace _1cbacupcloud3._5
{
    class diagnostics
    {
        internal static string DigLog { get; set; }
        internal static bool Dig;
        internal static void Start()
        {
            if (CheckService() == true)
            {
                if (CheckParam() == true)
                {
                    Dig = true;
                }
            }
            else
            {
                Dig = false;
                DigLog += "SR1001";
            }
        }
        private static bool CheckParam()
        {
            bool _check = false;
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
        internal static void GetLog(string id, string logFile, string db_id, string messageto1c, bool status, double ibsize, string itslogin, DateTime timestamp)
        {
            string[] m_logFile = File.ReadAllLines(logFile);
            DateTime dateTime = DateTime.Now;
            if (string.IsNullOrEmpty(id) && 
                !string.IsNullOrEmpty(logFile) && 
                !string.IsNullOrEmpty(db_id) && 
                string.IsNullOrEmpty(messageto1c) && 
                !string.IsNullOrEmpty(itslogin))
            {
                foreach (var str in m_logFile)
                {
                    LogAgent.Root root = JsonConvert.DeserializeObject<LogAgent.Root>(str);
                    if (root.Timestamp.Day == dateTime.Day && root.Timestamp.Month == dateTime.Month && !string.IsNullOrEmpty(root.BackupID) && root.message.Contains("RETRY COUNT operation") && root.message.Contains(db_id))
                    {
                        GetLog(root.BackupID, logFile, db_id, null, false, ibsize, itslogin, timestamp);
                        break;
                    }
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
                    if (root.Timestamp.Day == dateTime.Day && root.Timestamp.Month == dateTime.Month && root.BackupID == id && root.message.Contains("OK"))
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
                To1C to1C = new To1C { ibid = db_id, ibsize = ibsize, itslogin = itslogin, message = messageto1c, status = status, timestamp = timestamp };
                //
                string temp = JsonConvert.SerializeObject(to1C);
                int i = 4 + 9;
            }
        }
    }
}
