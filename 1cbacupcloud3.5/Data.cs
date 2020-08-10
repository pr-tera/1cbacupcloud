﻿using _1cbacupcloud3._5.CloudAgent;
using System.Collections.Generic;
using System.IO;

namespace _1cbacupcloud3._5
{
    struct Data
    {
        internal static string Path { get; set; } = Directory.GetCurrentDirectory();
        internal static string PathPort { get; } = @"\metadata\port.properties";
        internal static string PublicKey { get; set; }
        internal static string Login { get; set; }
        internal static string Password { get; set; }
        internal static string ImagePathAgent { get; set; }
        internal static string StrageDay { get; set; }
        internal static string Log { get; set; }
        internal static string ReqistryKey { get; } = @"SYSTEM\CurrentControlSet\Services\1CBackupAgent";
        internal static string AgentTempDB { get; } = @"\temp\DB";
        internal static string AgentTempIN { get; } = @"\temp\input";
        internal const double LogSize = 2e+6;
        internal static string ParametrsName { get; } = @"\Parametrs.xml";
        internal static  List<string> BackupNameList = new List<string>();
    }
    struct URI
    {
        internal static string Protocol { get; } = "http://";
        internal static string LocalServer { get; } = "localhost:";
        internal static string APIagent { get; } = "/api/v1/agent";
        internal static string APIib { get; } = "/api/v1/infobases?URI=";
        internal static string APIbackup { get; } = "/api/v1/backups";
    }
    class Type
    {
        internal static string[] type = { "*.zip", "*.backup", "*.xml" };
        internal static string[] RequestType = { "GET", "POST", "PUT" }; // 0.1.2
        internal static string ContenCa { get; } = "application/json;charset=utf-8";
        internal static string ContenAp { get; } = "application/json";
    }
}
