using System;
using _1cbacupcloud3._5.CloudAgent;
using _1cbacupcloud3._5.Local;

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
        }
    }
}
