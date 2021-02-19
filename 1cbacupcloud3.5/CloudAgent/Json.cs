using Newtonsoft.Json;
using System;

namespace _1cbacupcloud3._5.CloudAgent
{
    class Json : Agent
    {
        public string login { get; set; }
        public string password { get; set; }
        public string serviceNick { get; set; }
    }
    class JsonUpload : Agent
    {
        public string dateLabel { get; set; }
        public string ibPath { get; set; }
        public string backupType { get; set; }
    }
    class SetTimetableJ : Agent
    {
        public string ibPath { get; set; }
        public string lastItems { get; set; }
        public string status { get; set; }
        public timeConfigDATA timeConfigDATA { get; set; }
    }
    class timeConfigDATA : Agent
    {
        public string beginDate { get; set; }
        public string repeatPeriodWeeks { get; set; }
        public dayConfigDetailDATA dayConfigDetailDATA { get; set; }
    }
    class dayConfigDetailDATA : Agent
    {
        public string beginTime { get; set; }
    }
    class Ticketcs : Agent
    {
        public string ticket { get; set; }
    }
    //
    class To1C : diagnostics
    {
        public string ibid { get; set; }
        public string itslogin { get; set; }
        public string message { get; set; }
        public DateTime timestamp { get; set; }
        public double ibsize { get; set; }
        public bool status { get; set; }
    }
    class LogAgent : diagnostics
    {
        public class Root
        {
            [JsonProperty("@timestamp")]
            public DateTime Timestamp { get; set; }
            [JsonProperty("@version")]
            public string Version { get; set; }
            public string message { get; set; }
            public string logger_name { get; set; }
            public string thread_name { get; set; }
            public string level { get; set; }
            public int level_value { get; set; }
            public string BackupID { get; set; }
            public string BackupRemoteID { get; set; }
            public string Comments { get; set; }
            public string PID { get; set; }
            public string log_owner { get; set; }
            public string agentId { get; set; }
            public string SecurityPCHash { get; set; }
            public string version { get; set; }
            public string localTimestamp { get; set; }
        }
    }
}
