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
}
