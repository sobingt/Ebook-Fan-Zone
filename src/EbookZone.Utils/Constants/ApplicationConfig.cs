using System.Configuration;

namespace EbookZone.Utils.Constants
{
    public class ApplicationConfig
    {
        #region Fields

        #region Box Service

        public static string ApiKey = @"cuyr8txmnzm7itqvu84kxc8myjek9xuw";
        public static string _apiUrl2 = @"https://api.box.com/2.0/";
        public static string ApiUrlOld = @"https://www.box.com/api/1.0/";
        public static string UserRedirectAuthUrl = @"https://www.box.com/api/1.0/auth/{0}";
        public static string ResourceRest = "rest";

        #endregion Box Service

        #endregion Fields

        #region Properties

        public static string ConnStringSetting
        {
            get { return ConfigurationManager.ConnectionStrings["EFZDataContext"].ConnectionString; }
        }

        #endregion Properties
    }
}