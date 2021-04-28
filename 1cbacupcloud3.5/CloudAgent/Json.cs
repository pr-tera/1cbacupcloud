using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace _1cbacupcloud3._5.CloudAgent
{
    class Json : Agent
    {
        [JsonProperty("@login")]
        public string Login { get; set; }
        [JsonProperty("@password")]
        public string Password { get; set; }
        [JsonProperty("@serviceNick")]
        public string ServiceNick { get; set; }
    }
    class JsonUpload : Agent
    {
        [JsonProperty("@dateLabel")]
        public DateTime DateLabel { get; set; }
        [JsonProperty("@ibPath")]
        public string IbPath { get; set; }
        [JsonProperty("@backupType")]
        public string BackupType { get; set; }
    }
    class JsonAgentVersion : Agent
    {
        [JsonProperty("@version")]
        public string Version { get; set; }
    }
    class SetTimetableJ : Agent
    {
        [JsonProperty("@dbid")]
        public string Dbid { get; set; }
        [JsonProperty("@ibName")]
        public string IbName { get; set; }
        [JsonProperty("@ibPath")]
        public string IbPath { get; set; }
        [JsonProperty("@id")]
        public string Id { get; set; }
        [JsonProperty("@lastItems")]
        public string LastItems { get; set; }
        [JsonProperty("@status")]
        public string Status { get; set; }
        [JsonProperty("@timeConfigDATA")]
        public List<timeConfigDATA> TimeConfigDATA { get; set; }
        [JsonProperty("@ttlUrl")]
        public string TtlUrl { get; set; }
    }
    class timeConfigDATA : Agent
    {
        [JsonProperty("@beginDate")]
        public string BeginDate { get; set; }
        [JsonProperty("@dayConfigDetailDATA")]
        public List<dayConfigDetailDATA> DayConfigDetailDATA { get; set; }
        [JsonProperty("@repeatPeriodDays")]
        public string RepeatPeriodDays { get; set; }
        [JsonProperty("@repeatPeriodWeeks")]
        public string RepeatPeriodWeeks { get; set; }     
    }
    class dayConfigDetailDATA : Agent
    {
        [JsonProperty("@repeatPeriodWeeks")]
        public string beginTime { get; set; }
    }
    class Ticketcs : Agent
    {
        [JsonProperty("@repeatPeriodWeeks")]
        public string ticket { get; set; }
    }
    //
    class To1C : Diagnostics
    {
        [JsonProperty("@ibid")]
        public string Ibid { get; set; }
        [JsonProperty("@itslogin")]
        public string Itslogin { get; set; }
        [JsonProperty("@message")]
        public string Message { get; set; }
        [JsonProperty("@timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("@ibsize")]
        public double Ibsize { get; set; }
        [JsonProperty("@srvr")]
        public bool Srvr { get; set; } //true - server false - file
        [JsonProperty("@status")]
        public bool Status { get; set; }
    }
    class LogAgent : Diagnostics
    {
        public class Root
        {
            [JsonProperty("@timestamp")]
            public DateTime Timestamp { get; set; }
            [JsonProperty("@version")]
            public string Version { get; set; }
            [JsonProperty("@message")]
            public string Message { get; set; }
            [JsonProperty("@logger_name")]
            public string Logger_name { get; set; }
            [JsonProperty("@thread_name")]
            public string Thread_name { get; set; }
            [JsonProperty("@level")]
            public string Level { get; set; }
            [JsonProperty("@level_value")]
            public int Level_value { get; set; }
            [JsonProperty("@BackupID")]
            public string BackupID { get; set; }
            [JsonProperty("@BackupRemoteID")]
            public string BackupRemoteID { get; set; }
            [JsonProperty("@Comments")]
            public string Comments { get; set; }
            [JsonProperty("@PID")]
            public string PID { get; set; }
            [JsonProperty("@log_owner")]
            public string Log_owner { get; set; }
            [JsonProperty("@agentId")]
            public string AgentId { get; set; }
            [JsonProperty("@SecurityPCHash")]
            public string SecurityPCHash { get; set; }
            [JsonProperty("@version")]
            public string version { get; set; }
            [JsonProperty("@localTimestamp")]
            public string LocalTimestamp { get; set; }
        }
    }
}
