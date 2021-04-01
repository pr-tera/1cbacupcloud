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
                        Data.Log += $"\n{Program.GetDate()} {tt}{Environment.NewLine}";
                    }
                    else
                    {
                        Data.Log += $"{Program.GetDate()} Не зарегистрированная ошибка{Environment.NewLine}";
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
                        Data.Log += $"\n{Program.GetDate()} {tt}{Environment.NewLine}";
                    }
                    else
                    {
                        Data.Log += $"\n{Program.GetDate()} Не зарегистрированная ошибка:{Environment.NewLine}";
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
                    Data.Log += $"\n{Program.GetDate()} {tt}{Environment.NewLine}";
                }
                else
                {
                    Data.Log += $"\n{Program.GetDate()} Не зарегистрированная ошибка{Environment.NewLine}";
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
                            Data.Log += $"\n{Program.GetDate()} {tt}{Environment.NewLine}";
                        }
                        else
                        {
                            Data.Log += $"{Program.GetDate()} Не зарегистрированная ошибка{Environment.NewLine}";
                        }
                    }
                }
                //

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
                        Data.Log += $"\n{Program.GetDate()} {tt}{Environment.NewLine}";
                    }
                    else
                    {
                        Data.Log += $"\n{Program.GetDate()} Не зарегистрированная ошибка:{Environment.NewLine}";
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
                if (d.Length != 0)
                {
                    foreach (var dtemp in d)
                    {
                        Data.IbDUID.Add(dtemp.Name);
                    }
                }
                else
                {
                    Data.Log += $"{Program.GetDate()}Директории не соответствуют маске *-*-*-*-*{Environment.NewLine}";
                }
            }
            else
            {
                if (fi.Length != 0)
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
                }
                if (diA.Length != 0)
                {
                    foreach (DirectoryInfo df in diA)
                    {
                        GetPath(df.FullName, Type, back, loggz, dipath, dt);
                    }
                }
            }
        }
        internal static void CleanOldBackup(string Folder, string Type)
        {
            DirectoryInfo di = new DirectoryInfo(Folder);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles(Type);
            if (fi.Length != 0)
            {
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
                            Data.Log += $"\n{Program.GetDate()} IO2001{Environment.NewLine}";
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2002{Environment.NewLine}";
                        }
                        catch (NotSupportedException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2003{Environment.NewLine}";
                        }
                        catch (PathTooLongException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2004{Environment.NewLine}";
                        }
                        catch (IOException)
                        {
                            Data.Log += $"\n{Program.GetDate()} IO2005{Environment.NewLine}";
                        }
                        catch (Exception ex)
                        {
                            Data.Log += $"\n{Program.GetDate()} {ex}{Environment.NewLine}";
                        }
                    }
                }
            }
            if (diA.Length != 0)
            {
                foreach (DirectoryInfo df in diA)
                {
                    CleanOldBackup(df.FullName, Type);
                }
            }
            if (Directory.Exists(Data.ImagePathAgent + Data.AgentTempDB))
            {
                try
                {
                    Directory.Delete(Data.ImagePathAgent + Data.AgentTempDB, true);
                }
                catch (ArgumentException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1001{Environment.NewLine}";
                }
                catch (PathTooLongException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1002{Environment.NewLine}";
                }
                catch (DirectoryNotFoundException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1003{Environment.NewLine}";
                }
                catch (Exception ex)
                {
                    Data.Log += $"\n{Program.GetDate()} {ex}{Environment.NewLine}";
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
                    Data.Log += $"\n{Program.GetDate()} DI1001{Environment.NewLine}";
                }
                catch (PathTooLongException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1002{Environment.NewLine}";
                }
                catch (DirectoryNotFoundException)
                {
                    Data.Log += $"\n{Program.GetDate()} DI1003{Environment.NewLine}";
                }
                catch (Exception ex)
                {
                    Data.Log += $"\n{Program.GetDate()} {ex}{Environment.NewLine}";
                }
            }
        }
        internal static void Rename(string Folder, string Type)
        {
            DirectoryInfo di = new DirectoryInfo(Folder);
            DirectoryInfo[] diA = di.GetDirectories();
            FileInfo[] fi = di.GetFiles(Type);
            if (fi.Length != 0)
            {
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
                                Data.Log += $"\n{Program.GetDate()} IO1002{Environment.NewLine}";
                            }
                            catch (ArgumentNullException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO1003{Environment.NewLine}";
                            }
                            catch (ArgumentException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO1004{Environment.NewLine}";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO1005{Environment.NewLine}";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO1006{Environment.NewLine}";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO1007{Environment.NewLine}";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO1001 ({f.FullName}){Environment.NewLine}";
                            }
                            catch (Exception ex)
                            {
                                Data.Log += $"\n{Program.GetDate()} {ex} ({f.FullName}){Environment.NewLine}";
                            }
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (ArgumentException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2001{Environment.NewLine}";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2002{Environment.NewLine}";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2003{Environment.NewLine}";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2004{Environment.NewLine}";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2005{Environment.NewLine}";
                            }
                            catch (Exception ex)
                            {
                                Data.Log += $"\n{Program.GetDate()} {ex}{Environment.NewLine}";
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
                                Data.Log += $"\n{Program.GetDate()} IO2001{Environment.NewLine}";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2002{Environment.NewLine}";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2003{Environment.NewLine}";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2004{Environment.NewLine}";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"\n{Program.GetDate()} IO2005{Environment.NewLine}";
                            }
                            catch (Exception ex)
                            {
                                Data.Log += $"\n{Program.GetDate()} {ex}{Environment.NewLine}";
                            }
                        }
                    }
                }
            }
            if (diA.Length != 0)
            {
                foreach (DirectoryInfo df in diA)
                {
                    Rename(df.FullName, Type);
                }
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
                        Data.Log += $"\n{Program.GetDate()} {t}{Environment.NewLine})";
                        return false;
                    }
                    else
                    {
                        Data.Log += $"\n{Program.GetDate()} Ошибка при создании директории temp{Environment.NewLine}";
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
    //framework 3.5
    //static class Gzip
    //{
    //    public static long CopyTo(this Stream source, Stream destination)
    //    {
    //        byte[] buffer = new byte[2048];
    //        int bytesRead;
    //        long totalBytes = 0;
    //        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
    //        {
    //            destination.Write(buffer, 0, bytesRead);
    //            totalBytes += bytesRead;
    //        }
    //        return totalBytes;
    //    }
    //}
}
