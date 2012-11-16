using System.Web.Mvc;
using AutoMapper;
using EbookZone.Core;
using EbookZone.ViewModels;

namespace EbookZone.Web.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new IdentityViewModel();
            viewModel = Mapper.Map(SecurityManager.CurrentUser, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Save()
        {
            return RedirectToAction("Index");
        }
    }
}
