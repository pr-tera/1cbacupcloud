using _1cbacupcloud3._5.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

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
                            int elapsed = 0;
                            while (IO.LooksFile(i) && (elapsed < 60000))
                            {
                                Thread.Sleep(10000);
                                elapsed += 10000;
                            }
                            Program.WritheLog($"Ответ агента: {Upload(i)}");
                        }
                        else
                        {
                            Program.WritheLog("Список бекапов пуст!");
                            Program.WritheDigLog("Не удалось получить список бекапов.");
                        }
                    }
                }
                else
                {
                    Program.WritheLog("Список бекапов пуст!");
                    Program.WritheDigLog("Не удалось получить список бекапов.");
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string GetTicket()
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
                    Json _json = new Json { Login = Data.Login, Password = Data.Password, ServiceNick = "1C-Cloud-backup" };
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(_json, Formatting.Indented);
                        streamWriter.Write(json);
                        streamWriter.Close();
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                        responseStream.Close();
                    }
                    CheckResponseStatusPortal(response.StatusCode);
                    webClient.Dispose();
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
                        request.Headers.Add($"X-Token: {result.Ticket}");
                        request.ContentType = Type.ContenCa;
                        request.Method = Type.RequestType[1]; //POST
                        //JsonUpload _json = new JsonUpload { targetPath = path, targetName = "", dateLabel = "2022-07-20T10:06:23" }; // произвольный файл
                        JsonUpload _json = new JsonUpload { IbPath = path, BackupType = "manual", DateLabel = DateTime.Now };
                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            string json = JsonConvert.SerializeObject(_json, Formatting.Indented, settings);
                            streamWriter.Write(json);
                            streamWriter.Close();
                        }
                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            responseString = reader.ReadToEnd();
                            responseStream.Close();
                        }
                        CheckResponseStatus(response.StatusCode);
                        webClient.Dispose();
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
                    request.Headers.Add($"X-Token: {result.Ticket}");
                    request.ContentType = Type.ContenCa;
                    request.Method = Type.RequestType[2]; //PUT
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = "null";
                        streamWriter.Write(json);
                        streamWriter.Close();
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                        responseStream.Close();
                    }
                    CheckResponseStatus(response.StatusCode);
                    webClient.Dispose();
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
                    request.Headers.Add($"X-Token: {result.Ticket}");
                    request.ContentType = Type.ContenCa;
                    request.Method = Type.RequestType[2]; // PUT
                    List<dayConfigDetailDATA> dayConfigDetailDATA = new List<dayConfigDetailDATA> { new dayConfigDetailDATA { beginTime = DateTime.Now.ToString("HH:mm") } };
                    List<timeConfigDATA> timeConfigDATA = new List<timeConfigDATA> { new timeConfigDATA { BeginDate = DateTime.Now.ToString("yyyy'-'MM'-'dd"), DayConfigDetailDATA = dayConfigDetailDATA, RepeatPeriodDays = "1", RepeatPeriodWeeks = "1", } };
                    SetTimetableJ setTimetableJ = new SetTimetableJ { Dbid = null, IbName = null, IbPath = IBpath, Id = null, LastItems = Data.StrageDay, Status = "inactive", TimeConfigDATA = timeConfigDATA, TtlUrl = null };
                    //string json = $"{{\"ibPath\": \"{IBpath.Replace("\\", "\\\\")}\",\"lastItems\": {Data.StrageDay},\"status\": \"inactive\",\"timeConfigDATA\": [{{\"beginDate\": \"2020-07-24\",\"repeatPeriodWeeks\": 1,\"dayConfigDetailDATA\": [{{\"beginTime\": \"21:00\"}}]}}]}}";
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(JsonConvert.SerializeObject(setTimetableJ, Formatting.Indented));
                        streamWriter.Close();
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                        responseStream.Close();
                    }
                    CheckResponseStatus(response.StatusCode);
                    webClient.Dispose();
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
                version = jsonAgentVersion.Version;
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
                        responseStream.Close();
                    }
                    CheckResponseStatus(response.StatusCode);
                    webClient.Dispose();
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
            string responseString;
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
                        streamWriter.Close();
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                        reader.Close();
                    }
                    CheckResponseStatus1C(response.StatusCode);
                    webClient.Dispose();
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
        }
        private static void CheckResponseStatusPortal(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case (HttpStatusCode)200:
                    Program.WritheLog("Тикет получен успешно(login.1c.ru)");
                    break;
                case (HttpStatusCode)201:
                    Program.WritheLog("Тикет получен успешно(login.1c.ru)");
                    break;
                case (HttpStatusCode)202:
                    Program.WritheLog("Тикет получен успешно(login.1c.ru)");
                    break;
                case (HttpStatusCode)400:
                    Program.WritheLog("Ошибка авторизации(login.1c.ru)");
                    Program.WritheDigLog("Не удалось получить тикет с портала login.1c.ru. Ошибка авторизации.");
                    break;
                case (HttpStatusCode)401:
                    Program.WritheLog("Ошибка авторизации(login.1c.ru)");
                    Program.WritheDigLog("Не удалось получить тикет с портала login.1c.ru. Ошибка авторизации.");
                    break;
                case (HttpStatusCode)403:
                    Program.WritheLog("ошибка получения тикета 403(login.1c.ru)");
                    Program.WritheDigLog("Не удалось получить тикет с портала login.1c.ru. Ошибка доступа к порталу.");
                    break;
                case (HttpStatusCode)404:
                    Program.WritheLog("ошибка получения тикета 404(login.1c.ru)");
                    Program.WritheDigLog("Не удалось получить тикет с портала login.1c.ru. Ошибка доступа к порталу.");
                    break;
            }
        }
        private static void CheckResponseStatus1C(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case (HttpStatusCode)200:
                    Program.WritheLog("Обмен с 1С: ОК");
                    Data.StatusSendTo1C = "200";
                    break;
                case (HttpStatusCode)500:
                    Program.WritheLog("Обмен с 1С: Внутренняя ошибка сервера.");
                    Data.StatusSendTo1C = "500";
                    break;
                case (HttpStatusCode)403:
                    Program.WritheLog("Обмен с 1С: Ошибка обработки запроса.");
                    Data.StatusSendTo1C = "403";
                    break;
                case (HttpStatusCode)401:
                    Program.WritheLog("Обмен с 1С: Ошибка авторизации.");
                    Data.StatusSendTo1C = "401";
                    break;
                case (HttpStatusCode)404:
                    Program.WritheLog("Обмен с 1С: API не доступен.");
                    Data.StatusSendTo1C = "404";
                    break;
            }
        }
        private static void CheckResponseStatus(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case (HttpStatusCode)200:
                    Program.WritheLog("Ответ агента: ОК");
                    break;
                case (HttpStatusCode)201:
                    Program.WritheLog("Ответ агента: Created");
                    break;
                case (HttpStatusCode)202:
                    Program.WritheLog("Ответ агента: Accepted");
                    break;
                case (HttpStatusCode)400:
                    Program.WritheLog("Ответ агента: Bad Request - Ошибка авторизации");
                    Program.WritheDigLog("Агент: Ошибка авторизации.");
                    break;
                case (HttpStatusCode)401:
                    Program.WritheLog("Ответ агента: Agent not initialized. Token is rotten - Ошибка авторизации на агенте");
                    Program.WritheDigLog("Агент: Ошибка авторизации на агенте.");
                    break;
                case (HttpStatusCode)402:
                    Program.WritheLog("Ответ агента: Payment Required. Not enough options - Недостаточно дискового пространства. Проверьте доступный размер дисковой квоты в Личном Кабинете");
                    Program.WritheDigLog("Агент: Закончилась квота в облаке.");
                    break;
                case (HttpStatusCode)403:
                    Program.WritheLog("Ответ агента: Forbidden");
                    Program.WritheDigLog("Агент: Ошибка обработки запроса.");
                    break;
                case (HttpStatusCode)404:
                    Program.WritheLog("Ответ агента: Target not found exception - Объект копирования не доступен для агента");
                    Program.WritheDigLog("Агент: Бекап не доступен агенту.");
                    break;
                case (HttpStatusCode)406:
                    Program.WritheLog("Ответ агента: Backup not triggering by path");
                    break;
                case (HttpStatusCode)409:
                    Program.WritheLog("Ответ агента: Infobase load exception or not found - Расписание копирования не найдено");
                    break;
                case (HttpStatusCode)410:
                    Program.WritheLog("Ответ агента: Can`t upload manual backup to the server - Не удалось создать ручной бекап");
                    break;
                case (HttpStatusCode)422:
                    Program.WritheLog("Ответ агента: Infobase not save: internet problem or save exception - Расписание копирования не удалось сохранить из-за сетевых ошибок");
                    break;
                case (HttpStatusCode)424:
                    Program.WritheLog("Ответ агента: File version not found (may be: version crashed in install time) - Не удалось определить версию агента");
                    Program.WritheDigLog("Агент: Не удалось определить версию агента.");
                    break;
                case (HttpStatusCode)500:
                    Program.WritheLog("Ответ агента: Server error - Внутренняя ошибка сервера");
                    Program.WritheDigLog("Агент: Внутренняя ошибка сервера.");
                    break;
            }
        }
    }
}