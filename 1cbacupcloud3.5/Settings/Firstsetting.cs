using System;
using System.Xml;

namespace _1cbacupcloud3._5.Settings
{
    class Firstsetting : XML
    {
        public static bool Start()
        {
            if (Install.GetAgent() == true)
            {
                if (GenParametrsXML() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
    class XML
    {
        protected static bool GenParametrsXML()
        {
            XmlDocument XmlParam = new XmlDocument();
            XmlDeclaration XmlDec = XmlParam.CreateXmlDeclaration("1.0", null, null);
            XmlParam.AppendChild(XmlDec);
            //Корень <Options>
            XmlElement xOptions = XmlParam.CreateElement("Options");
            XmlParam.AppendChild(xOptions);
            //PublicKey
            Data.PublicKey = DateTime.Now + Crypto.Crypto.PrivatKey + DateTime.Now + Data.Path;
            XmlElement xPublicKey = XmlParam.CreateElement("PublicKey");
            xPublicKey.InnerText = Crypto.Crypto.Encrypt(Data.PublicKey, Crypto.Crypto.PrivatKey);
            xOptions.AppendChild(xPublicKey);
            //login
            Console.Write("Логин:");
            Data.Login = Console.ReadLine();
            XmlElement xLogin = XmlParam.CreateElement("Login");
            xLogin.InnerText = Crypto.Crypto.Encrypt(Data.Login, Data.PublicKey);
            xOptions.AppendChild(xLogin);
            //Password
            Console.Write("Пароль:");
            Data.Password = Console.ReadLine();
            XmlElement xPassword = XmlParam.CreateElement("Pwd");
            xPassword.InnerText = Crypto.Crypto.Encrypt(Data.Password, Data.PublicKey);
            xOptions.AppendChild(xPassword);
            if (!string.IsNullOrEmpty(Data.Login) && !string.IsNullOrEmpty(Data.Password) && !string.IsNullOrEmpty(Data.PublicKey))
            {
                /*вызвать функцию для активации
                 * активироватьАгента()
                 * {бла-бла}
                 */
                try
                {
                    XmlParam.Save(Data.Path + @"/Parametrs.xml");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Program.WritheLog(Convert.ToString(ex));
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Введены не все данные\n");
                GenParametrsXML();
                return false;
            }
        }
    }
}