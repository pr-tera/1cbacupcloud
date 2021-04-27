using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
        public DateTime dateLabel { get; set; }
        public string ibPath { get; set; }
        public string backupType { get; set; }
    }
    class JsonAgentVersion : Agent
    { 
        public string version { get; set; }
    }
    class SetTimetableJ : Agent
    {
        public string dbid { get; set; }
        public string ibName { get; set; }
        public string ibPath { get; set; }
        public string id { get; set; }
        public string lastItems { get; set; }
        public string status { get; set; }
        //public timeConfigDATA timeConfigDATA { get; set; }
        public List<timeConfigDATA> timeConfigDATA { get; set; }
        public string ttlUrl { get; set; }
    }
    class timeConfigDATA : Agent
    {
        public string beginDate { get; set; }
        //public dayConfigDetailDATA dayConfigDetailDATA { get; set; }
        public List<dayConfigDetailDATA> dayConfigDetailDATA { get; set; }
        public string repeatPeriodDays { get; set; }
        public string repeatPeriodWeeks { get; set; }     
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
        public bool srvr { get; set; } //true - server false - file
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
