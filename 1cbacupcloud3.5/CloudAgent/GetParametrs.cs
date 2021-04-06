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
                    Program.WritheLog("XM0002");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Файл параметров не обнаружен");
                Program.WritheLog("XM0003");
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
                    Program.WritheLog("IO3008");
                }
            }
            else
            {
                Program.WritheLog("IO3008");
                AgentPort = string.Empty;
            }
            return AgentPort;
        }
        internal static string Version()
        {
            string version;
            if (File.Exists($@"{Data.Path}/ver.ver"))
            {
                version = _version();
            }
            else
            {
                Program.WritheLog("Файл с локальной версией скриптов не существует!");
                version = "000";
            }
            return version;
        }
        private static string _version()
        {
            string version;
            try
            {
                using (FileStream fstream = File.OpenRead($@"{Data.Path}/ver.ver"))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    version = System.Text.Encoding.Default.GetString(array);
                }
            }
            catch
            {
                Program.WritheLog("Ошибка определения локальной версии скриптов.");
                version = "000";
            }
            return version;
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
                Program.WritheLog("IO3001");
                return Port = null;
            }
            catch (PathTooLongException)
            {
                Program.WritheLog("IO3002");
                return Port = null;
            }
            catch (DirectoryNotFoundException)
            {
                Program.WritheLog("IO3003");
                return Port = null;
            }
            catch (UnauthorizedAccessException)
            {
                Program.WritheLog("IO3004");
                return Port = null;
            }
            catch (FileNotFoundException)
            {
                Program.WritheLog("IO3005");
                return Port = null;
            }
            catch (NotSupportedException)
            {
                Program.WritheLog("IO3006");
                return Port = null;
            }
            catch (IOException)
            {
                Program.WritheLog("IO3007");
                return Port = null;
            }
            catch (Exception ex)
            {
                Program.WritheLog($"{ex}");
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
                Program.WritheLog("XM0001");
            }
        }
    }
}
