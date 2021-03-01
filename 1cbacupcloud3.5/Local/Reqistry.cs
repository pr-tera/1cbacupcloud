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
                Data.Log += $"\n{Program.GetDate()} RE0001\n";
            }
            catch (ObjectDisposedException)
            {
                Data.Log += $"\n{Program.GetDate()} RE0002\n";
            }
            catch (SecurityException)
            {
                Data.Log += $"\n{Program.GetDate()} RE0003\n";
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{Program.GetDate()} {ex}\n";
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
                Data.Log += $"\n{Program.GetDate()} RE0001\n";
            }
            catch (ObjectDisposedException)
            {
                Data.Log += $"\n{Program.GetDate()} RE0002\n";
            }
            catch (SecurityException)
            {
                Data.Log += $"\n{Program.GetDate()} RE0003\n";
            }
            catch (Exception ex)
            {
                Data.Log += $"\n{Program.GetDate()} {ex}\n";
            }
            return name;
        }
    }
}
