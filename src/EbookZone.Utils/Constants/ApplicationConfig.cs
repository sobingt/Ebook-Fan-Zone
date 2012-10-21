using System.Configuration;

namespace EbookZone.Utils.Constants
{
    public class ApplicationConfig
    {
        public static string ConnStringSetting
        {
            get { return ConfigurationManager.ConnectionStrings["EFZDataContext"].ConnectionString; }
        }
    }
}