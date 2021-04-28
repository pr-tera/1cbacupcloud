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
                Data.ImagePathAgent = Convert.ToString(key.GetValue("ImagePath")).Replace("agentNET.exe", "").Replace('"', ' ');
            }
            catch (ArgumentNullException)
            {
                Program.WritheLog("RE0001");
            }
            catch (ObjectDisposedException)
            {
                Program.WritheLog("RE0002");
            }
            catch (SecurityException)
            {
                Program.WritheLog("RE0003");
            }
            catch (Exception ex)
            {
                Program.WritheLog(Convert.ToString(ex));
            }
        }
        internal static string AgentServiceName()
        {
            string name = string.Empty;
            try
            {
                RegistryKey key = Registry.LocalMachine;
                key = key.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\1CBackupAgent");
                name = Convert.ToString(key.GetValue("DisplayName"));
            }
            catch (ArgumentNullException)
            {
                Program.WritheLog("RE0001");
            }
            catch (ObjectDisposedException)
            {
                Program.WritheLog("RE0002");
            }
            catch (SecurityException)
            {
                Program.WritheLog("RE0003");
            }
            catch (Exception ex)
            {
                Program.WritheLog(Convert.ToString(ex));
            }
            return name;
        }
    }
}
