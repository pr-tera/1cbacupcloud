using System;
using System.Text;
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
                DateTime dateTime = DateTime.Now;
                diagnostics.GetLog(null, @"C:\Users\tera\source\repos\1cbacupcloud\1cbacupcloud3.5\bin\Debug\logFile.log", "bp_dda639f4-764f-46c2-a570-2fb172a519a2", null, false, 32.0, "efia", dateTime);
            }
        }
    }
}
