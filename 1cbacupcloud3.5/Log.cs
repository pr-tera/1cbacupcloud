﻿using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace _1cbacupcloud3._5
{
    class Log
    {
        internal static void Write()
        {
            if (!string.IsNullOrEmpty(Data.Log))
            {
                string path = $"{Data.Path}\\LogBackupAgent.log";
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
        internal async Task SendEmailAsync()
        {
            Message();
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Logs.Head, Logs.SendEmail));
            emailMessage.To.Add(new MailboxAddress("", Logs.Email));
            emailMessage.Subject = Logs.Topic;
            try
            {
                emailMessage.Body = new TextPart("Plain")
                {
                    Text = Logs.Body
                };
            }
            catch
            {
                //
            }
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(Logs.SendEmail, Logs.SendEmailPassword);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
        private static void Message()
        {
            string _backupstatus = string.Empty;
            string _servicestatus = string.Empty;
            if (Data.BackupStatus == false)
            {
                _backupstatus = "не успешно";
            }
            else
            {
                _backupstatus = "успешно";
            }
            if (Data.ServiceStatus == false)
            {
                _servicestatus = "не запущена";
            }
            else
            {
                _servicestatus = "запущена";
            }
            Logs.Topic = $"Логи клиента";
            Logs.Head = $"Облачный архив 2.0";
            Logs.Body = $"Логин ИТС: {Data.Login + Environment.NewLine}" +
                $"Выгрузка в облако: {_backupstatus + Environment.NewLine}" +
                $"Статус службы агента резервного копирования: {_servicestatus + Environment.NewLine}" +
                $"Лог текущей выгрузки: {Environment.NewLine + Data.Log + Environment.NewLine}";
        }
    }
    struct Logs
    {
        internal static string Head { get; set; }
        internal static string Topic { get; set; }
        internal static string Body { get; set; }
        internal static string SendEmail { get; } = "backuplog@pr365.ru"; //"prtestalert@gmail.com";
        internal static string SendEmailPassword { get; } = "qvpgailsfjzkfyje"; //"EFIAmors123";
        internal static string Email { get; } = "tera@pr365.ru"; //"backuplog@pr365.ru";
    }
}
