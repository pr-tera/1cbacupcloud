using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using _1cbacupcloud3._5.Local;

namespace _1cbacupcloud3._5.CloudAgent
{
    class Agent
    {
        internal static bool Start()
        {
            if (GetParametrs.Get() == true)
            {
                for (int i = 0; i < 2; i++)
                {
                    IO.Rename(Data.Path, Type.type[i]);
                }
                for (int i = 0; i < 2; i++)
                {
                    IO.GetPath(Data.Path, Type.type[i]);
                }
                foreach (var i in Data.BackupNameList)
                {
                    Data.Log += $"{DateTime.Now} {Upload(i)} \n";
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
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка(WB0001):\n{ex}\n";
                return null;
            }
        }
        private static string Upload(string path)
        {
            Init();
            SetTimetable(path);
            string Port = GetParametrs.Port();
            string responseString = string.Empty;
            if (!string.IsNullOrEmpty(Port))
            {
                try
                {
                    string url = URI.Protocol + URI.LocalServer + Port + URI.APIbackup; //$"http://localhost:{Port}/api/v1/backups";
                    using (var webClient = new WebClient())
                    {
                        var result = JsonConvert.DeserializeObject<Ticketcs>(GetTicket());
                        var request = WebRequest.Create(url);
                        request.Headers.Add($"X-Token: {result.ticket}");
                        request.ContentType = Type.ContenCa;
                        request.Method = Type.RequestType[1]; //POST
                        //JsonUpload _json = new JsonUpload { targetPath = path, targetName = "", dateLabel = "2022-07-20T10:06:23" }; // произвольный файл
                        JsonUpload _json = new JsonUpload { ibPath = path, backupType = "manual", dateLabel = "2022-07-20T10:06:23" };
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
                }
                catch (Exception ex)
                {
                    Data.Log += $"{DateTime.Now} Не заругистрированная ошибка(WB0001):\n{ex}\n";
                }
            }
            return responseString;
        }
        private static void Init()
        {
            string responseString;
            string Port = GetParametrs.Port();
            string url = URI.Protocol + URI.LocalServer + Port + URI.APIagent; //$"http://localhost:{Port}/api/v1/agent"
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
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка(WB0001):\n{ex}\n";
            }
        }
        private static void SetTimetable(string IBpath)
        {
            string responseString;
            string Port = GetParametrs.Port();
            //string url = $"http://localhost:{Port}/api/v1/infobases?URI=F%3A%5Ctera%5C%D0%91%D0%B0%D1%80%D0%B6%D0%B5%D0%B5%D0%B2";
            string url = URI.Protocol + URI.LocalServer + Port + URI.APIib + IBpath; //$"http://localhost:{Port}/api/v1/infobases?URI={IBpath}";
            try
            {
                using (var webClient = new WebClient())
                {
                    var result = JsonConvert.DeserializeObject<Ticketcs>(GetTicket());
                    var request = WebRequest.Create(url);
                    request.Headers.Add($"X-Token: {result.ticket}");
                    request.ContentType = Type.ContenCa;
                    request.Method = Type.RequestType[2]; // PUT
                    dayConfigDetailDATA dayConfigDetailDATA = new dayConfigDetailDATA { beginTime = DateTime.Now.ToString("HH:mm") };
                    timeConfigDATA timeConfigDATA = new timeConfigDATA { beginDate = DateTime.Now.ToString("yyyy'-'MM'-'dd"), repeatPeriodWeeks = "1", dayConfigDetailDATA = dayConfigDetailDATA };
                    SetTimetableJ setTimetableJ = new SetTimetableJ { ibPath = IBpath, lastItems = Data.StrageDay, status = "inactive", timeConfigDATA = timeConfigDATA };
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
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка(WB0001):\n{ex}\n";
            }
        }
    }
}