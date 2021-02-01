using System;
using System.IO;
using System.IO.Compression;

namespace _1cbacupcloud3._5.Local
{
    class IO
    {
        internal static void UnGzip(string Folber, string outFolber)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Folber);
                if (File.Exists($"{fileInfo.FullName}.new"))
                {
                    File.Delete($"{fileInfo.FullName}.new");
                }
                if (File.Exists(outFolber))
                {
                    File.Delete(outFolber);
                }
                File.Copy(fileInfo.FullName, $"{fileInfo.FullName}.new");
                using (var inputFileStream = new FileStream($"{Folber}.new", FileMode.Open))
                using (var gzipStream = new GZipStream(inputFileStream, CompressionMode.Decompress))
                using (var outputFileStream = new FileStream(outFolber, FileMode.Create))
                {
                    gzipStream.CopyTo(outputFileStream);
                }
                if (File.Exists($"{fileInfo.FullName}.new"))
                {
                    File.Delete($"{fileInfo.FullName}.new");
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не зарегистрированная ошибка:\n{ex}\n";
            }
        }
        internal static void GetPath(string Folder, string Type, bool back = true, bool loggz = false, bool dipath = false, DateTime? dt = null)
        {
            try
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
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не зарегистрированная ошибка(IO0001):\n{ex}\n";
            }
        }
        internal static void CleanOldBackup(string Folder, string Type)
        {
            try
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
                            Data.Log += $"{DateTime.Now} IO2001\n";
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Data.Log += $"{DateTime.Now} IO2002\n";
                        }
                        catch (NotSupportedException)
                        {
                            Data.Log += $"{DateTime.Now} IO2003\n";
                        }
                        catch (PathTooLongException)
                        {
                            Data.Log += $"{DateTime.Now} IO2004\n";
                        }
                        catch (IOException)
                        {
                            Data.Log += $"{DateTime.Now} IO2005\n";
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
                        Data.Log += $"{DateTime.Now} DI1001\n";
                    }
                    catch (PathTooLongException)
                    {
                        Data.Log += $"{DateTime.Now} DI1002\n";
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Data.Log += $"{DateTime.Now} DI1003\n";
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
                        Data.Log += $"{DateTime.Now} DI1001\n";
                    }
                    catch (PathTooLongException)
                    {
                        Data.Log += $"{DateTime.Now} DI1002\n";
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Data.Log += $"{DateTime.Now} DI1003\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не зарегистрированная ошибка(IO0001):\n{ex}\n";
            }
        }
        internal static void Rename(string Folder, string Type)
        {
            try
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
                                Data.Log += $"{DateTime.Now} IO1002\n";
                            }
                            catch (ArgumentNullException)
                            {
                                Data.Log += $"{DateTime.Now} IO1003\n";
                            }
                            catch (ArgumentException)
                            {
                                Data.Log += $"{DateTime.Now} IO1004\n";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"{DateTime.Now} IO1005\n";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"{DateTime.Now} IO1006\n";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"{DateTime.Now} IO1007\n";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"{DateTime.Now} IO1001\n";
                            }
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (ArgumentException)
                            {
                                Data.Log += $"{DateTime.Now} IO2001\n";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"{DateTime.Now} IO2002\n";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"{DateTime.Now} IO2003\n";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"{DateTime.Now} IO2004\n";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"{DateTime.Now} IO2005\n";
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
                                Data.Log += $"{DateTime.Now} IO2001\n";
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Data.Log += $"{DateTime.Now} IO2002\n";
                            }
                            catch (NotSupportedException)
                            {
                                Data.Log += $"{DateTime.Now} IO2003\n";
                            }
                            catch (PathTooLongException)
                            {
                                Data.Log += $"{DateTime.Now} IO2004\n";
                            }
                            catch (IOException)
                            {
                                Data.Log += $"{DateTime.Now} IO2005\n";
                            }
                        }
                    }
                }
                foreach (DirectoryInfo df in diA)
                {
                    Rename(df.FullName, Type);
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не зарегистрированная ошибка(IO0001):\n{ex}\n";
            }
        }
    }
}
