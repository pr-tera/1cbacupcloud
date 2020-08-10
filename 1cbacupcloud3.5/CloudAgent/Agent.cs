using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security;
using System.Text;

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
            using (var webClient = new WebClient())
            {
                var request = WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";
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
        private static string Upload(string path)
        {
            Init();
            SetTimetable(path);
            string Port = GetParametrs.Port();
            string responseString = string.Empty;
            if (!string.IsNullOrEmpty(Port))
            {
                string url = $"http://localhost:{Port}/api/v1/backups";
                using (var webClient = new WebClient())
                {
                    var result = JsonConvert.DeserializeObject<Ticketcs>(GetTicket());
                    var request = WebRequest.Create(url);
                    request.Headers.Add($"X-Token: {result.ticket}");
                    request.ContentType = "application/json;charset=utf-8";
                    request.Method = "POST";
                    //JsonUpload _json = new JsonUpload { targetPath = path, targetName = "", dateLabel = "2022-07-20T10:06:23" };
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
            return responseString;
        }
        private static void Init()
        {
            string responseString;
            string Port = GetParametrs.Port();
            string url = $"http://localhost:{Port}/api/v1/agent";
            using (var webClient = new WebClient())
            {
                var result = JsonConvert.DeserializeObject<Ticketcs>(GetTicket());
                var request = WebRequest.Create(url);
                request.Headers.Add($"X-Token: {result.ticket}");
                request.ContentType = "application/json;charset=utf-8";
                request.Method = "PUT";
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
        private static void SetTimetable(string IBpath)
        {
            string responseString;
            string Port = GetParametrs.Port();
            //string url = $"http://localhost:{Port}/api/v1/infobases?URI=F%3A%5Ctera%5C%D0%91%D0%B0%D1%80%D0%B6%D0%B5%D0%B5%D0%B2";
            string url = $"http://localhost:{Port}/api/v1/infobases?URI={IBpath}";
            using (var webClient = new WebClient())
            {
                var result = JsonConvert.DeserializeObject<Ticketcs>(GetTicket());
                var request = WebRequest.Create(url);
                request.Headers.Add($"X-Token: {result.ticket}");
                request.ContentType = "application/json;charset=utf-8";
                request.Method = "PUT";
                //dayConfigDetailDATA _json1 = new dayConfigDetailDATA { beginTime = "11:00" };
                //string dayConfigDetailDATA = JsonConvert.SerializeObject(new[] { _json1 }, Formatting.Indented);
                //timeConfigDATA _json2 = new timeConfigDATA { beginDate = "2020-07-20", repeatPeriodWeeks = "0", dayConfigDetailDATA = $"[{{\"beginTime\": \"21:00\"}}]" };
                //string timeConfigDATA = JsonConvert.SerializeObject(new[] { _json2 }, Formatting.Indented);
                //SetTimetable _json = new SetTimetable { ibPath = IBpath, lastItems = "1", status = "Inactive", timeConfigDATA = timeConfigDATA };
                string json = $"{{\"ibPath\": \"{IBpath.Replace("\\", "\\\\")}\",\"lastItems\": {Data.StrageDay},\"status\": \"inactive\",\"timeConfigDATA\": [{{\"beginDate\": \"2020-07-24\",\"repeatPeriodWeeks\": 1,\"dayConfigDetailDATA\": [{{\"beginTime\": \"21:00\"}}]}}]}}";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //string json = JsonConvert.SerializeObject(_json, Formatting.Indented);
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
    }
    class IO
    {
        internal static void GetPath(string Folder, string Type)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Folder);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fi = di.GetFiles(Type);
                foreach (FileInfo f in fi)
                {
                    Data.BackupNameList.Add(f.FullName);
                }
                foreach (DirectoryInfo df in diA)
                {
                    GetPath(df.FullName, Type);
                }
            }
            //catch (DirectoryNotFoundException)
            //{
            //    return 0;
            //}
            //catch (UnauthorizedAccessException)
            //{
            //    return 0;
            //}
            //catch (Exception)
            //{
            //    return 0;
            //}
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка:\n{ex}\n";
            }
        }
        internal static void CleanOldBackup(string Folder, string Type)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Folder);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fi = di.GetFiles(Type);
                foreach (FileInfo f in fi)
                {
                    if (f.FullName != $"{Data.Path}\\Parametrs.xml")
                    {
                        try
                        {
                            File.Delete(f.FullName);
                        }
                        catch (ArgumentException)
                        {
                            Data.Log += $"{DateTime.Now} IO2001\n";
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Data.Log += $"{DateTime.Now} IO2002\n";
                        }
                        catch (NotSupportedException)
                        {
                            Data.Log += $"{DateTime.Now} IO2003\n";
                        }
                        catch (PathTooLongException)
                        {
                            Data.Log += $"{DateTime.Now} IO2004\n";
                        }
                        catch (IOException)
                        {
                            Data.Log += $"{DateTime.Now} IO2005\n";
                        }
                    }
                }
                foreach (DirectoryInfo df in diA)
                {
                    CleanOldBackup(df.FullName, Type);
                }

            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка:\n{ex}\n";
            }
        }
        internal static void Rename(string Folder, string Type)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Folder);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fi = di.GetFiles(Type);
                foreach (FileInfo f in fi)
                {
                    if (f.Name != di.Name)
                    {
                        if (!File.Exists($"{di.FullName}\\{di.Name + Type}"))
                        {
                            try
                            {
                                File.Move(f.FullName, $"{di.FullName}\\{di.Name}{Type.Replace("*", "")}");
                            }
                            catch (FileNotFoundException)
                            {
                                Data.Log += $"{DateTime.Now} IO1002\n";
                            }
                            catch (ArgumentNullException)
                            {
                                Data.Log += $"{DateTime.Now} IO1003\n";
                            }
                            catch (ArgumentException)
                            {
                                Data.Log += $"{DateTime.Now} IO1004\n";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"{DateTime.Now} IO1005\n";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"{DateTime.Now} IO1006\n";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"{DateTime.Now} IO1007\n";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"{DateTime.Now} IO1001\n";
                            }
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (ArgumentException)
                            {
                                Data.Log += $"{DateTime.Now} IO2001\n";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"{DateTime.Now} IO2002\n";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"{DateTime.Now} IO2003\n";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"{DateTime.Now} IO2004\n";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"{DateTime.Now} IO2005\n";
                            }
                        }
                        else
                        {
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (ArgumentException)
                            {
                                Data.Log += $"{DateTime.Now} IO2001\n";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"{DateTime.Now} IO2002\n";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"{DateTime.Now} IO2003\n";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"{DateTime.Now} IO2004\n";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"{DateTime.Now} IO2005\n";
                            }
                        }
                    }
                }
                foreach (DirectoryInfo df in diA)
                {
                    Rename(df.FullName, Type);
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка:\n{ex}\n";
            }
        }
    }
    class Reqistry
    {
        public static void GetKey()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine;
                key = key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\1CBackupAgent");
                Data.ImagePathAgent = key.GetValue("ImagePath").ToString().Replace("agentNET.exe", "").Replace('"', ' ');
            }
            catch (ArgumentNullException)
            {
                Data.Log += $"{DateTime.Now} RE0001\n";
            }
            catch (ObjectDisposedException)
            {
                Data.Log += $"{DateTime.Now} RE0002\n";
            }
            catch (SecurityException)
            {
                Data.Log += $"{DateTime.Now} RE0003\n";
            }
        }
    }
}