using Microsoft.Win32;
using System;
using System.Security;

namespace _1cbacupcloud3._5.Local
{
    class Reqistry
    {
        internal static void GetKey()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine;
                key = key.OpenSubKey(Data.ReqistryKey);
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
        internal static string AgentServiceName()
        {
            string name = string.Empty;
            try
            {
                RegistryKey key = Registry.LocalMachine;
                key = key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\1CBackupAgent");
                name = key.GetValue("DisplayName").ToString();
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
            return name;
        }
    }
}
