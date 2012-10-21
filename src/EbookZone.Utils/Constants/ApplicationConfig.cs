using System.Configuration;

namespace EbookZone.Utils.Constants
{
    public class ApplicationConfig
    {
        public static string ConnStringSetting
        {
            get
            {
                var value = ConfigurationManager.GetSection("ConnectionStringName");
                return value.ToString();
            }
        }
    }
}