using System.Globalization;
using System.Web;
using System.Web.Security;
using EbookZone.Domain;
using EbookZone.Repository;

namespace EbookZone.Core
{
    public static class SecurityManager
    {
        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    int id = int.Parse(HttpContext.Current.User.Identity.Name, CultureInfo.InvariantCulture);

                    User user = RepositoryManager.Users.Load(id);

                    if (user == null)
                    {
                        FormsAuthentication.SignOut();
                        HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl, true);
                    }

                    return user;
                }

                return null;
            }
        }
    }
}