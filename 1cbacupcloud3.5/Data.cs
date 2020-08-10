using System.Collections.Generic;
using System.IO;

namespace _1cbacupcloud3._5
{
    struct Data
    {
        internal static string Path { get; set; } = Directory.GetCurrentDirectory();
        internal static string PathPort { get; } = @"\metadata\port.properties";
        internal static string PublicKey { get; set; }
        internal static string Login { get; set; }
        internal static string Password { get; set; }
        internal static string ImagePathAgent { get; set; }
        internal static string StrageDay { get; set; }
        internal static string Log { get; set; }
        internal const double LogSize = 2e+6;
        internal static  List<string> BackupNameList = new List<string>();
    }
    class Type
    {
        internal static string[] type = { "*.zip", "*.backup", "*.xml" };
    }
}
