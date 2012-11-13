using System.Linq;
using System.Web.Mvc;
using EbookZone.Core;
using EbookZone.Services.Interfaces;
using EbookZone.ViewModels.Base;

namespace EbookZone.Web.Controllers
{
    public class BaseListController<T> : Controller where T : BaseViewModel
    {
        private readonly IBaseService<T> _baseService;
        private BaseListViewModel<T> _model;

        public BaseListViewModel<T> Model
        {
            get { return _model ?? (_model = new BaseListViewModel<T> { Items = this._baseService.Load() }); }
            set { _model = value; }
        }

        public BaseListController(IBaseService<T> baseService)
        {
            this._baseService = baseService;
        }

        public virtual ActionResult Index()
        {
            return null;
        }

        public ActionResult Delete(int id)
        {
            var viewModel = this.Model.Items.Single(x => x.Id == id);
            if (viewModel != null)
            {
                this._baseService.Execute(viewModel, EntityAction.Delete);
            }

            return RedirectToAction("Index", this.RouteData.Values["controller"]);
        }
    }
}