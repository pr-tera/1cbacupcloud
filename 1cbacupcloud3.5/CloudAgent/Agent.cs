using _1cbacupcloud3._5.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace _1cbacupcloud3._5.CloudAgent
{
    class Agent
    {
        internal static bool Start()
        {
            if (GetParametrs.Get() == true)
            {
                for (int i = 0; i <= 2; i++)
                {
                    IO.Rename(Data.Path, Type.type[i]);
                }
                for (int i = 0; i <= 2; i++)
                {
                    IO.GetPath(Data.Path, Type.type[i]);
                }
                if (Data.BackupNameList.Count != 0)
                {
                    foreach (var i in Data.BackupNameList)
                    {
                        if (!string.IsNullOrEmpty(i))
                        {
                            Program.WritheLog($"Ответ агента: {Upload(i)}");
                        }
                        else
                        {
                            Program.WritheLog("Список бекапов пуст!");
                        }
                    }
                }
                else
                {
                    Program.WritheLog("Список бекапов пуст!");
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private static string GetTicket()
        {
            string responseString;
            const string url = "https://login.1c.ru//rest/public/ticket/get";
            try
            {
                using (var webClient = new WebClient())
                {
                    var request = WebRequest.Create(url);
                    request.ContentType = Type.ContenAp;
                    request.Method = Type.RequestType[1]; //POST
                    Json _json = new Json { login = Data.Login, password = Data.Password, serviceNick = "1C-Cloud-backup" };
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(_json, Formatting.Indented);
                        streamWriter.Write(json);
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                    }
                }
                return responseString;
            }
            catch (Exception ex)
            {
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    Program.WritheLog(t);
                    Program.WritheLog(url);
                }
                else
                {
                    Program.WritheLog("Не зарегистрированная ошибка(WB0001)");
                }
                return null;
            }
        }
        private static string Upload(string path)
        {
            Init();
            SetTimetable(path);
            string Port = GetParametrs.Port();
            string responseString = string.Empty;
            var settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            if (!string.IsNullOrEmpty(Port))
            {
                string url = URI.Protocol[0] + URI.LocalServer + Port + URI.APIbackup; //$"http://localhost:{Port}/api/v1/backups";
                try
                {
                    using (var webClient = new WebClient())
                    {
                        var result = JsonConvert.DeserializeObject<Ticketcs>(GetTicket());
                        var request = WebRequest.Create(url);
                        request.Headers.Add($"X-Token: {result.ticket}");
                        request.ContentType = Type.ContenCa;
                        request.Method = Type.RequestType[1]; //POST
                        //JsonUpload _json = new JsonUpload { targetPath = path, targetName = "", dateLabel = "2022-07-20T10:06:23" }; // произвольный файл
                        JsonUpload _json = new JsonUpload { ibPath = path, backupType = "manual", dateLabel = DateTime.Now };
                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            string json = JsonConvert.SerializeObject(_json, Formatting.Indented, settings);
                            streamWriter.Write(json);
                        }
                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            responseString = reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string t = Convert.ToString(ex);
                    if (!string.IsNullOrEmpty(t))
                    {
                        Program.WritheLog(t);
                        Program.WritheLog(url);
                    }
                    else
                    {
                        Program.WritheLog("Не зарегистрированная ошибка(WB0001)");
                    }
                }
            }
            return responseString;
        }
        private static void Init()
        {
            string responseString;
            string Port = GetParametrs.Port();
            string url = URI.Protocol[0] + URI.LocalServer + Port + URI.APIagent; //$"http://localhost:{Port}/api/v1/agent"
            try
            {
                using (var webClient = new WebClient())
                {
                    var result = JsonConvert.DeserializeObject<Ticketcs>(GetTicket());
                    var request = WebRequest.Create(url);
                    request.Headers.Add($"X-Token: {result.ticket}");
                    request.ContentType = Type.ContenCa;
                    request.Method = Type.RequestType[2]; //PUT
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = "null";
                        streamWriter.Write(json);
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    Program.WritheLog(t);
                    Program.WritheLog(url);
                }
                else
                {
                    Program.WritheLog("Не зарегистрированная ошибка(WB0001)");
                }
            }
        }
        private static void SetTimetable(string IBpath)
        {
            string responseString;
            string Port = GetParametrs.Port();
            //string url = $"http://localhost:{Port}/api/v1/infobases?URI=F%3A%5Ctera%5C%D0%91%D0%B0%D1%80%D0%B6%D0%B5%D0%B5%D0%B2";
            //string url = URI.Protocol[0] + URI.LocalServer + Port + URI.APIib + IBpath; //$"http://localhost:{Port}/api/v1/infobases?URI={IBpath}";
            string url = _getURL(URI.Protocol[0] + URI.LocalServer + Port + URI.APIib + IBpath);
            try
            {
                using (var webClient = new WebClient())
                {
                    var result = JsonConvert.DeserializeObject<Ticketcs>(GetTicket());
                    var request = WebRequest.Create(url);
                    request.Headers.Add($"X-Token: {result.ticket}");
                    request.ContentType = Type.ContenCa;
                    request.Method = Type.RequestType[2]; // PUT
                    List<dayConfigDetailDATA> dayConfigDetailDATA = new List<dayConfigDetailDATA> { new dayConfigDetailDATA { beginTime = DateTime.Now.ToString("HH:mm") } };
                    List<timeConfigDATA> timeConfigDATA = new List<timeConfigDATA> { new timeConfigDATA { beginDate = DateTime.Now.ToString("yyyy'-'MM'-'dd"), dayConfigDetailDATA = dayConfigDetailDATA, repeatPeriodDays = "1", repeatPeriodWeeks = "1", } };
                    SetTimetableJ setTimetableJ = new SetTimetableJ { dbid = null, ibName = null, ibPath = IBpath, id = null, lastItems = Data.StrageDay, status = "inactive", timeConfigDATA = timeConfigDATA, ttlUrl = null };
                    //string json = $"{{\"ibPath\": \"{IBpath.Replace("\\", "\\\\")}\",\"lastItems\": {Data.StrageDay},\"status\": \"inactive\",\"timeConfigDATA\": [{{\"beginDate\": \"2020-07-24\",\"repeatPeriodWeeks\": 1,\"dayConfigDetailDATA\": [{{\"beginTime\": \"21:00\"}}]}}]}}";
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(JsonConvert.SerializeObject(setTimetableJ, Formatting.Indented));
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    Program.WritheLog(t);
                    Program.WritheLog(url);
                }
                else
                {
                    Program.WritheLog("Не зарегистрированная ошибка(WB0001)");
                }
            }
        }
        public static string GetVersion()
        {
            string t = _getVersion();
            string version;
            if (t.Contains("N\\a"))
            {
                version = t;
            }
            else if (string.IsNullOrEmpty(t))
            {
                version = "N\\a";
            }
            else
            {
                JsonAgentVersion jsonAgentVersion = JsonConvert.DeserializeObject<JsonAgentVersion>(_getVersion());
                version = jsonAgentVersion.version;
            }
            return version;
        }
        private static string _getVersion()
        {
            string responseString;
            string Port = GetParametrs.Port();
            string url = URI.Protocol[0] + URI.LocalServer + Port + URI.APIVersion;
            try
            {
                using (var webClient = new WebClient())
                {
                    var request = WebRequest.Create(url);
                    request.Method = Type.RequestType[0]; // GET
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
                    {
                        responseString = responseStream.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                string t = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(t))
                {
                    Program.WritheLog(t);
                    Program.WritheLog(url);
                }
                else
                {
                    Program.WritheLog("Не зарегистрированная ошибка(WB0001)");
                }
                responseString = "N\\a";
            }
            return responseString;
        }
        private static string _getURL(string _url)
        {

            return Uri.EscapeUriString(_url);
        }
        internal static void Send1C()
        {
            string responseString = string.Empty;
            try
            {
                string url = URI.Protocol[1] + URI.PRserver + URI.API1C;
                using (var webClient = new WebClient())
                {
                    var request = WebRequest.Create(url);
                    //request.ContentType = Type.ContenCa;
                    request.Method = Type.RequestType[1]; //POST
                    var byteArray = Encoding.ASCII.GetBytes($"{Data.Login1C}:{Data.Pwd1C}");
                    request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(byteArray));
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(Data.JsonTo1C);
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                    }
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
                    Program.WritheLog("Не зарегистрированная ошибка(WB0001)");
                }
            }
            if (responseString.Contains("200"))
            {
                Program.WritheLog("Успешный обмен с 1С");
                Data.StatusSendTo1C = "200";
            }
            else if (responseString.Contains("500"))
            {
                Program.WritheLog("Ошибка обмена с 1С 500");
                Data.StatusSendTo1C = "500";
            }
            else if (responseString.Contains("403"))
            {
                Program.WritheLog("Ошибка обмена с 1С 403");
                Data.StatusSendTo1C = "403";
            }
            else if (responseString.Contains("401"))
            {
                Program.WritheLog("Ошибка обмена с 1с 401");
                Data.StatusSendTo1C = "401";
            }
            else if (string.IsNullOrEmpty(responseString))
            { 
                Program.WritheLog("Успешный обмен с 1С");
                Data.StatusSendTo1C = "200";
            }
            else
            {
                Data.StatusSendTo1C = responseString;
            }
        }
    }
}