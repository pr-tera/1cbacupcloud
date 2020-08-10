using _1cbacupcloud3._5.CloudAgent;
using System.Text;
using System.IO;
using System;

namespace _1cbacupcloud3._5
{
    class Program
    {
        static void Main(string[] args)
        {
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
                        IO.CleanOldBackup(Data.Path, Type.type[i]);
                        Log.Write();
                    }
                }
                else
                {
                    Log.Write();
                }
            }
            //else if (args[0] == "test")
            //{
            //    if (GetParametrs.Get() == true)
            //    {
            //        for (int i = 0; i < 2; i++)
            //        {
            //            IO.Rename(Data.Path, Type.type[i]);
            //        }
            //    }
            //    else
            //    {
            //        Log.Write();
            //    }
            //}
        }
    }
    class Log
    {
        internal static void Write()
        {
            if (!string.IsNullOrEmpty(Data.Log))
            {
                string path = $"{Data.Path}\\LogBackupAgent.txt";
                FileInfo Filelog = new FileInfo(path);
                if (!File.Exists(path))
                {
                    using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter sw = new StreamWriter(fstream, Encoding.Default))
                        {
                            sw.Write(Data.Log);
                        }
                    }
                }
                else
                {
                    if (Filelog.Length < Data.LogSize)
                    {
                        using (FileStream fstream = new FileStream(path, FileMode.Append))
                        {
                            using (StreamWriter sw = new StreamWriter(fstream, Encoding.Default))
                            {
                                sw.Write(Data.Log);
                            }
                        }
                    }
                    else if (Filelog.Length > Data.LogSize)
                    {
                        File.Delete(path);
                        Write();
                    }
                }
            }
        }
    }
}
