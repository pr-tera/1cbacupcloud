using System;
using System.IO;

namespace _1cbacupcloud3._5.Local
{
    class IO
    {
        internal static void GetPath(string Folder, string Type)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Folder);
                DirectoryInfo[] diA = di.GetDirectories();
                FileInfo[] fi = di.GetFiles(Type);
                foreach (FileInfo f in fi)
                {
                    Data.BackupNameList.Add(f.FullName);
                }
                foreach (DirectoryInfo df in diA)
                {
                    GetPath(df.FullName, Type);
                }
            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка(IO0001):\n{ex}\n";
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

            }
            catch (Exception ex)
            {
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка(IO0001):\n{ex}\n";
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
                Data.Log += $"{DateTime.Now} Не заругистрированная ошибка(IO0001):\n{ex}\n";
            }
        }
    }
}
