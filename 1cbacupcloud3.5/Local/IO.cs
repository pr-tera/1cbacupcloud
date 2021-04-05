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
                        Program.WritheLog(tt);
                    }
                    else
                    {
                        Program.WritheLog($"Ошибка при удалении {fileInfo.FullName}.new");
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
                        Program.WritheLog(tt);
                    }
                    else
                    {
                        Program.WritheLog($"Ошибка при удалении {outFolber}");
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
                    Program.WritheLog(tt);
                }
                else
                {
                    Program.WritheLog($"Ошибка при копировании {t}");
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
                            Program.WritheLog(tt);
                        }
                        else
                        {
                            Program.WritheLog($"Ошибка распаковки архива с логами");
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
                        Program.WritheLog(tt);
                    }
                    else
                    {
                        Program.WritheLog($"Ошибка удаления {fileInfoNew.FullName}");
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
                    Program.WritheLog("Директории не соответствуют маске *-*-*-*-*");
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
                            Program.WritheLog("IO2001");
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Program.WritheLog("IO2002");
                        }
                        catch (NotSupportedException)
                        {
                            Program.WritheLog("IO2003");
                        }
                        catch (PathTooLongException)
                        {
                            Program.WritheLog("IO2004");
                        }
                        catch (IOException)
                        {
                            Program.WritheLog("IO2005");
                        }
                        catch (Exception ex)
                        {
                            Program.WritheLog(Convert.ToString(ex));
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
                    Program.WritheLog("DI1001");
                }
                catch (PathTooLongException)
                {
                    Program.WritheLog("DI1002");
                }
                catch (DirectoryNotFoundException)
                {
                    Program.WritheLog("DI1003");
                }
                catch (Exception ex)
                {
                    Program.WritheLog(Convert.ToString(ex));
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
                    Program.WritheLog("DI1001");
                }
                catch (PathTooLongException)
                {
                    Program.WritheLog("DI1002");
                }
                catch (DirectoryNotFoundException)
                {
                    Program.WritheLog("DI1003");
                }
                catch (Exception ex)
                {
                    Program.WritheLog(Convert.ToString(ex));
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
                                Program.WritheLog("IO1002");
                            }
                            catch (ArgumentNullException)
                            {
                                Program.WritheLog("IO1003");
                            }
                            catch (ArgumentException)
                            {
                                Program.WritheLog("IO1004");
                            }
                            catch (PathTooLongException)
                            {
                                Program.WritheLog("IO1005");
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Program.WritheLog("IO1006");
                            }
                            catch (NotSupportedException)
                            {
                                Program.WritheLog("IO1007");
                            }
                            catch (IOException)
                            {
                                Program.WritheLog("XIO1001M0001");
                            }
                            catch (Exception ex)
                            {
                                Program.WritheLog(Convert.ToString(ex));
                            }
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (ArgumentException)
                            {
                                Program.WritheLog("IO2001");
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Program.WritheLog("IO2002");
                            }
                            catch (NotSupportedException)
                            {
                                Program.WritheLog("IO2003");
                            }
                            catch (PathTooLongException)
                            {
                                Program.WritheLog("IO2004");
                            }
                            catch (IOException)
                            {
                                Program.WritheLog("IO2005");
                            }
                            catch (Exception ex)
                            {
                                Program.WritheLog(Convert.ToString(ex));
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
                                Program.WritheLog("IO2001");
                            }
                            catch (DirectoryNotFoundException)
                            {
                                Program.WritheLog("IO2002");
                            }
                            catch (NotSupportedException)
                            {
                                Program.WritheLog("IO2003");
                            }
                            catch (PathTooLongException)
                            {
                                Program.WritheLog("IO2004");
                            }
                            catch (IOException)
                            {
                                Program.WritheLog("IO2005");
                            }
                            catch (Exception ex)
                            {
                                Program.WritheLog(Convert.ToString(ex));
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
                        Program.WritheLog(t);
                        return true;
                    }
                    else
                    {
                        Program.WritheLog("Ошибка при создании директории temp");
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
