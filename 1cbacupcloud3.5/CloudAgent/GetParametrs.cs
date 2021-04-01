using System;
using System.IO;
using System.Xml;

namespace _1cbacupcloud3._5.CloudAgent
{
    class GetParametrs
    {
        internal static bool Get()
        {
            string path = Data.Path + @"\Parametrs.xml";
            if (File.Exists(path))
            {
                _get(path);
                if (!string.IsNullOrEmpty(Data.Login) && !string.IsNullOrEmpty(Data.Password) && !string.IsNullOrEmpty(Data.PublicKey))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Не удалось прочитать параметры");
                    Data.Log += $"\n{Program.GetDate()} XM0002{Environment.NewLine}";
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Файл параметров не обнаружен");
                Data.Log += $"\n{Program.GetDate()} XM0003{Environment.NewLine}";
                return false;
            }
        }
        internal static string Port()
        {
            string AgentPort;
            if (File.Exists(Data.ImagePathAgent + Data.PathPort))
            {
                AgentPort = _port();
                if (string.IsNullOrEmpty(AgentPort))
                {
                    Data.Log += $"\n{Program.GetDate()} IO3008{Environment.NewLine}";
                }
            }
            else
            {
                Data.Log += $"\n{Program.GetDate()} IO3008{Environment.NewLine}";
                AgentPort = string.Empty;
            }
            return AgentPort;
        }
        private static string _port()
        {
            string Port;
            try
            {
                using (FileStream fstream = File.OpenRead(Data.ImagePathAgent + Data.PathPort))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    Port = System.Text.Encoding.Default.GetString(array);
                }
            }
            catch (ArgumentNullException)
            {
                Data.Log += $"\n{Program.GetDate()} IO3001{Environment.NewLine}";
                return Port = null;
            }
            catch (PathTooLongException)
            {
                Data.Log += $"\n{Program.GetDate()} IO3002{Environment.NewLine}";
                return Port = null;
            }
            catch (DirectoryNotFoundException)
            {
                Data.Log += $"\n{Program.GetDate()} IO3003{Environment.NewLine}";
                return Port = null;
            }
            catch (UnauthorizedAccessException)
            {
                Data.Log += $"\n{Program.GetDate()} IO3004{Environment.NewLine}";
                return Port = null;
            }
            catch (FileNotFoundException)
            {
                Data.Log += $"\n{Program.GetDate()} IO3005{Environment.NewLine}";
                return Port = null;
            }
            catch (NotSupportedException)
            {
                Data.Log += $"\n{Program.GetDate()} IO3006{Environment.NewLine}";
                return Port = null;
            }
            catch (IOException)
            {
                Data.Log += $"\n{Program.GetDate()} IO3007{Environment.NewLine}";
                return Port = null;
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{Program.GetDate()} {ex}{Environment.NewLine}";
                return Port = null;
            }
            return Port;
        }
        private static void _get(string path)
        {
            try
            {
                XmlDocument XmlParam = new XmlDocument();
                XmlParam.Load(path);
                Data.PublicKey = Crypto.Crypto.Decrypt(XmlParam.SelectSingleNode("Options/PublicKey").InnerText, Crypto.Crypto.PrivatKey);
                Data.Login = Crypto.Crypto.Decrypt(XmlParam.SelectSingleNode("Options/Login").InnerText, Data.PublicKey);
                Data.Password = Crypto.Crypto.Decrypt(XmlParam.SelectSingleNode("Options/Pwd").InnerText, Data.PublicKey);
                Data.StrageDay = XmlParam.SelectSingleNode("Options/Days").InnerText;
            }
            catch (XmlException)
            {
                Data.Log += $"\n{Program.GetDate()} XM0001{Environment.NewLine}";
            }
        }
    }
}
