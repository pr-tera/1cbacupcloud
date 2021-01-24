using System;
using System.IO;
using _1cbacupcloud3._5.CloudAgent;
using _1cbacupcloud3._5.Local;

namespace _1cbacupcloud3._5
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.OutputEncoding = Encoding.UTF8;
            if (args[0] == "Upload")
            {
                try
                {
                    Reqistry.GetKey();
                    Agent.Start();
                    Log.Write();
                }
                catch (Exception ex)
                {
                    Data.Log += ex;
                    Log.Write();
                }
            }
            else if (args[0] == "Clean")
            {
                if (GetParametrs.Get() == true)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Reqistry.GetKey();
                        IO.CleanOldBackup(Data.Path, Type.type[i]);
                        Log.Write();
                    }
                }
                else
                {
                    Log.Write();
                }
            }
            else if (args[0] == "test")
            {
                //diagnostics.GetLog(null, @"C:\Users\tera\source\repos\1cbacupcloud\1cbacupcloud3.5\bin\Debug\logFile.log", "bp_dda639f4-764f-46c2-a570-2fb172a519a2", null, false, 32.0, "efia", dateTime);
                DateTime dateTime = DateTime.Now.Date;
                Reqistry.GetKey();
                IO.GetPath(Data.Path, Type.type[1], true, false, true);
                IO.GetPath($@"{Data.ImagePathAgent}\logs", Type.type[3], false, true, false, dateTime);
                IO.GetPath($@"{Data.ImagePathAgent}\logs", Type.type[3], false, false, false, dateTime);
                for (int i = 0; i != 2; i++)
                {
                    IO.GetPath(Data.Path, Type.type[i]);
                }
                IO.UnGzip(Data.LogGzPath, $@"{Data.ImagePathAgent}\logs{Data.LogAgentOld}");
                for (int i = 0; i != Data.BackupNameList.Count; i++)
                {
                    if (File.Exists(Data.BackupNameList[i]))
                    {
                        FileInfo fileInfo = new FileInfo(Data.BackupNameList[i]);
                        diagnostics.GetLog(null, Data.LogAgent, Data.IbDUID[i], null, false, fileInfo.Length, Data.Login, dateTime);
                    }
                }
                //foreach (var i in Data.BackupNameList)
                //{
                //    if (File.Exists(i))
                //    {
                //        FileInfo fileInfo = new FileInfo(i);
                //        diagnostics.GetLog(null, Data.LogAgent, , null, false, 32.0, "efia", dateTime);
                //    }
                //}
            }
        }
    }
}
