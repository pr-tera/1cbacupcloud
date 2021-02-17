using MimeKit;
using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
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
                    Text = Data.Log
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
            Logs.Topic = $"Облачный архив глобальный тест";
            Logs.Head = $"Облачный архив тесты";
        }
    }
    struct Logs
    {
        internal static string Head { get; set; }
        internal static string Topic { get; set; }
        internal static string Body { get; set; }
        internal static string SendEmail { get; } = "prtestalert@gmail.com";
        internal static string SendEmailPassword { get; } = "EFIAmors123";
        internal static string Email { get; } = "tera@1eska.ru";
    }
}
