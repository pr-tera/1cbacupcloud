using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _1cbacupcloud3._5.CloudAgent
{
    class Json
    {
        public string login { get; set; }
        public string password { get; set; }
        public string serviceNick { get; set; }
    }
    class JsonUpload
    {
        //public string targetPath { get; set; }
        //public string targetName { get; set; }
        public string dateLabel { get; set; }
        public string ibPath { get; set; }
        public string backupType { get; set; }
    }
    class SetTimetable
    {
        public string ibPath { get; set; }
        public string lastItems { get; set; }
        public string status { get; set; }
        public string timeConfigDATA { get; set; }
    }
    class timeConfigDATA
    { 
        public string beginDate { get; set; }
        public string repeatPeriodWeeks { get; set; }
        public string dayConfigDetailDATA { get; set; }
    }
    class dayConfigDetailDATA
    { 
        public string beginTime { get; set; }
    }
}
