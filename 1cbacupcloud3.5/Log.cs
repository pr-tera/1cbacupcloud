using System.IO;
using System.Text;

namespace _1cbacupcloud3._5
{
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
