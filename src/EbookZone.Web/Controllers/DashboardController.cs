using System.Web.Mvc;
using EbookZone.Core;
using EbookZone.Domain.Enums;

namespace EbookZone.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            if(SecurityManager.CurrentUser != null && SecurityManager.CurrentUser.UserType == UserType.Administrator)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }
    }
}