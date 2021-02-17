using System.Collections.Generic;
using System.IO;

namespace _1cbacupcloud3._5
{
    struct Data
    {
        internal static string Path { get; set; } = Directory.GetCurrentDirectory();
        internal static string PathSTemp { get; } = $@"{Path}\temp";
        internal static string PathPort { get; } = @"\metadata\port.properties";
        internal static string PublicKey { get; set; }
        internal static string Login { get; set; }
        internal static string Password { get; set; }
        internal static string ImagePathAgent { get; set; }
        internal static string StrageDay { get; set; }
        internal static string JsonTo1C { get; set; }
        internal static string Log { get; set; }
        internal static string LogGzPath { get; set; }
        internal static string LogAgent { get; set; }
        internal static string LogAgentOld { get; set; } = @"\LogFileOld.log";
        internal static string ReqistryKey { get; } = @"SYSTEM\CurrentControlSet\Services\1CBackupAgent";
        internal static string AgentTempDB { get; } = @"\temp\DB";
        internal static string AgentTempIN { get; } = @"\temp\input";
        internal static string ParametrsName { get; } = @"\Parametrs.xml";
        internal static string Login1C { get; } = "ServiceAPI";
        internal static string Pwd1C { get; } = "ServiceAPI_777";
        internal static List<string> BackupNameList = new List<string>();
        internal static List<string> IbDUID = new List<string>();
        internal const double LogSize = 2e+6;
        internal const double MinSizeBackup = 0.01;
    }
    struct URI
    {
        internal static string[] Protocol = { "http://", "https://" };
        internal static string LocalServer { get; } = "localhost:";
        internal static string APIagent { get; } = "/api/v1/agent";
        internal static string APIib { get; } = "/api/v1/infobases?URI=";
        internal static string APIbackup { get; } = "/api/v1/backups";
        internal static string PRserver { get; } = "mb.1eska.ru";
        internal static string API1C { get; } = "/service-api/hs/service-api/check-backup";
    }
    class Type
    {
        internal static string[] type = { "*.zip", "*.backup", "*.xml", "*.gz", "*.log" };
        internal static string[] RequestType = { "GET", "POST", "PUT" }; // 0.1.2
        internal static string ContenCa { get; } = "application/json;charset=utf-8";
        internal static string ContenAp { get; } = "application/json";
    }
}
