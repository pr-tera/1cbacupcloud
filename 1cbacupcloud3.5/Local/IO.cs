using System;
using System.IO;
using System.IO.Compression;

namespace _1cbacupcloud3._5.Local
{
    class IO
    {
        internal static void UnGzip(string Folber, string outFolber)
        {
            string t = string.Empty;
            FileInfo fileInfo = new FileInfo(Folber);
            if (File.Exists($"{fileInfo.FullName}.new"))
            {
                try
                {
                    File.Delete($"{fileInfo.FullName}.new");
                }
                catch (Exception ex)
                {
                    string tt = Convert.ToString(ex);
                    if (!string.IsNullOrEmpty(tt))
                    {
                        Data.Log += $"\n{Program.GetDate()} {tt}\n";
                    }
                    else
                    {
                        Data.Log += $"{Program.GetDate()} Не зарегистрированная ошибка\n";
                    }
                }
            }
            if (File.Exists(outFolber))
            {
                try
                {
                    File.Delete(outFolber);
                }
                catch (Exception ex)
                {
                    string tt = Convert.ToString(ex);
                    if (!string.IsNullOrEmpty(tt))
                    {
                        Data.Log += $"\n{Program.GetDate()} {tt}\n";
                    }
                    else
                    {
                        Data.Log += $"\n{Program.GetDate()} Не зарегистрированная ошибка:\n";
                    }
                }
            }
            try
            {
                File.Copy(fileInfo.FullName, $"{Data.PathSTemp}{fileInfo.Name}");
                t = $"{Data.PathSTemp}{fileInfo.Name}";
            }
            catch (Exception ex)
            {
                string tt = Convert.ToString(ex);
                if (!string.IsNullOrEmpty(tt))
                {
                    Data.Log += $"\n{Program.GetDate()} {tt}\n";
                }
                else
                {
                    Data.Log += $"\n{Program.GetDate()} Не зарегистрированная ошибка\n";
                }
            }
            if (!string.IsNullOrEmpty(t))
            {
                using (var inputFileStream = new FileStream(t, FileMode.Open))
                using (var gzipStream = new GZipStream(inputFileStream, CompressionMode.Decompress))
                using (var outputFileStream = new FileStream(outFolber, FileMode.Create))
                {
                    try
                    {
                        gzipStream.CopyTo(outputFileStream);
                        Data.LogAgentOld = outFolber;
                    }
                    catch (Exception ex)
                    {
                        string tt = Convert.ToString(ex);
                        if (!string.IsNullOrEmpty(tt))
                        {
                            Data.Log += $"\n{Program.GetDate()} {tt}\n";
                        }
                        else
                        {
                            Data.Log += $"{Program.GetDate()} Не зарегистрированная ошибка\n";
                        }
                    }
                }
            }
            FileInfo fileInfoNew = new FileInfo(t);
            if (fileInfoNew.Exists)
            {
                try
                {
                    File.Delete(fileInfoNew.FullName);
                }
                catch (Exception ex)
                {
                    string tt = Convert.ToString(ex);
                    if (!string.IsNullOrEmpty(tt))
                    {
                        Data.Log += $"\n{Program.GetDate()} {tt}\n";
                    }
                    else
                    {
                        Data.Log += $"\n{Program.GetDate()} Не зарегистрированная ошибка:\n";
                    }
                }
            }
        }
        internal static void GetPath(string Folder, string Type, bool back = true, bool loggz = false, bool dipath = false, DateTime? dt = null)
        {
            DirectoryInfo di = new DirectoryInfo(Folder);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles(Type);
            if (dipath == true)
            {
                DirectoryInfo[] d = di.GetDirectories("*-*-*-*-*");
                foreach (var dtemp in d)
                {
                    Data.IbDUID.Add(dtemp.Name);
                }
            }
            else
            {
                foreach (FileInfo f in fi)
                {
                    if (back == true)
                    {
                        Data.BackupNameList.Add(f.FullName);
                    }
                    if (back == false && f.CreationTime.Date == dt)
                    {
                        Data.LogGzPath = f.FullName;
                    }
                    if (loggz == true && f.LastWriteTime.Date == dt)
                    {
                        Data.LogAgent = f.FullName;
                    }
                }
                foreach (DirectoryInfo df in diA)
                {
                    GetPath(df.FullName, Type, back, loggz, dipath, dt);
                }
            }
        }
        internal static void CleanOldBackup(string Folder, string Type)
        {
            DirectoryInfo di = new DirectoryInfo(Folder);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles(Type);
            foreach (FileInfo f in fi)
            {
                if (f.FullName != Data.Path + Data.ParametrsName)
                {
                    try
                    {
                        File.Delete(f.FullName);
                    }
                    catch (ArgumentException)
                    {
                        Data.Log += $"\n{Program.GetDate()} IO2001\n";
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Data.Log += $"\n{Program.GetDate()} IO2002\n";
                    }
                    catch (NotSupportedException)
                    {
                        Data.Log += $"\n{Program.GetDate()} IO2003\n";
                    }
                    catch (PathTooLongException)
                    {
                        Data.Log += $"\n{Program.GetDate()} IO2004\n";
                    }
                    catch (IOException)
                    {
                        Data.Log += $"\n{Program.GetDate()} IO2005\n";
                    }
                    catch (Exception ex)
                    {
                        Data.Log += $"\n{Program.GetDate()} {ex}\n";
                    }
                }
            }
            foreach (DirectoryInfo df in diA)
            {
                CleanOldBackup(df.FullName, Type);
            }
            if (Directory.Exists(Data.ImagePathAgent + Data.AgentTempDB))
            {
                try
                {
                    Directory.Delete(Data.ImagePathAgent + Data.AgentTempDB, true);
                }
                catch (ArgumentException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1001\n";
                }
                catch (PathTooLongException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1002\n";
                }
                catch (DirectoryNotFoundException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1003\n";
                }
                catch (Exception ex)
                {
                    Data.Log += $"\n{Program.GetDate()} {ex}\n";
                }
            }
            if (Directory.Exists(Data.ImagePathAgent + Data.AgentTempIN))
            {
                try
                {
                    Directory.Delete(Data.ImagePathAgent + Data.AgentTempIN, true);
                }
                catch (ArgumentException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1001\n";
                }
                catch (PathTooLongException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1002\n";
                }
                catch (DirectoryNotFoundException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1003\n";
                }
                catch (Exception ex)
                {
                    Data.Log += $"\n{Program.GetDate()} {ex}\n";
                }
            }
        }
        internal static void Rename(string Folder, string Type)
        {
            DirectoryInfo di = new DirectoryInfo(Folder);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles(Type);
            foreach (FileInfo f in fi)
            {
                if (f.Name != di.Name)
                {
                    if (!File.Exists($"{di.FullName}\\{di.Name + Type}"))
                    {
                        try
                        {
                            File.Move(f.FullName, $"{di.FullName}\\{di.Name}{Type.Replace("*", "")}");
                        }
                        catch (FileNotFoundException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO1002\n";
                        }
                        catch (ArgumentNullException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO1003\n";
                        }
                        catch (ArgumentException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO1004\n";
                        }
                        catch (PathTooLongException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO1005\n";
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO1006\n";
                        }
                        catch (NotSupportedException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO1007\n";
                        }
                        catch (IOException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO1001 ({f.FullName})\n";
                        }
                        catch (Exception ex)
                        {
                            Data.Log += $"\n{Program.GetDate()} {ex} ({f.FullName})\n";
                        }
                        try
                        {
                            File.Delete(f.FullName);
                        }
                        catch (ArgumentException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2001\n";
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2002\n";
                        }
                        catch (NotSupportedException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2003\n";
                        }
                        catch (PathTooLongException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2004\n";
                        }
                        catch (IOException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2005\n";
                        }
                        catch (Exception ex)
                        {
                            Data.Log += $"\n{Program.GetDate()} {ex}\n";
                        }
                    }
                    else
                    {
                        try
                        {
                            File.Delete(f.FullName);
                        }
                        catch (ArgumentException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2001\n";
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2002\n";
                        }
                        catch (NotSupportedException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2003\n";
                        }
                        catch (PathTooLongException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2004\n";
                        }
                        catch (IOException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2005\n";
                        }
                        catch (Exception ex)
                        {
                            Data.Log += $"\n{Program.GetDate()} {ex}\n";
                        }
                    }
                }
            }
            foreach (DirectoryInfo df in diA)
            {
                Rename(df.FullName, Type);
            }
        }
        internal static bool CreateDir(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                try
                {
                    Directory.CreateDirectory(directoryInfo.FullName);
                    return true;
                }
                catch (Exception ex)
                {
                    string t = Convert.ToString(ex);
                    if (!string.IsNullOrEmpty(t))
                    {
                        Data.Log += $"\n{Program.GetDate()} {t}\n";
                        return false;
                    }
                    else
                    {
                        Data.Log += $"\n{Program.GetDate()} Ошибка при создании директории temp\n";
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }
    }
}
