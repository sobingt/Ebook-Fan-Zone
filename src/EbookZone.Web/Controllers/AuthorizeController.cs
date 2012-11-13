using System;
using System.Web.Mvc;
using AutoMapper;
using EbookZone.Core;
using EbookZone.Domain.Enums;
using EbookZone.Services.Interfaces;
using EbookZone.ViewModels;

namespace EbookZone.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IGoogleService _googleService;
        private readonly IFacebookService _facebookService;
        private readonly IIdentityService _identityService;
        private readonly IBoxService _boxService;

        public AuthorizeController(
            IGoogleService googleService,
            IFacebookService facebookService,
            IIdentityService identityService,
            IBoxService boxService)
        {
            _googleService = googleService;
            _facebookService = facebookService;
            _identityService = identityService;
            _boxService = boxService;
        }

        public ActionResult Index(string registerType)
        {
            var accountType = (AccountType)Enum.Parse(typeof(AccountType), registerType);

            if (accountType == AccountType.Google)
            {
                this._googleService.ExecuteRequest();
            }

            if (accountType == AccountType.Facebook)
            {
                this._facebookService.ExecuteRequest();
            }

            if (accountType == AccountType.BoxCloud)
            {
                this._boxService.ExecuteRequest();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Register(IdentityViewModel viewModel)
        {
            if (_identityService.Register(viewModel))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogOn(IdentityViewModel viewModel)
        {
            if(_identityService.Login(viewModel, AccountType.Default, false))
            {
                if(SecurityManager.CurrentUser.UserType == UserType.Administrator)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            _identityService.LogOff();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult FacebookAuth()
        {
            var code = Request.Params["code"];

            var facebookModel = _facebookService.GetAccount(code);
            var viewModel = Mapper.Map<FacebookViewModel, IdentityViewModel>(facebookModel);
            return this.Register(viewModel);
        }

        public ActionResult GoogleAuth()
        {
            var model = this._googleService.GetAccount();
            var viewModel = Mapper.Map<GoogleViewModel, IdentityViewModel>(model);
            return this.Register(viewModel);
        }

        public ActionResult BoxStorageAuth()
        {
            var ticket = this.Request.Params["ticket"];
            var authToken = this.Request.Params["auth_token"];

            var model = _boxService.GetAccount(ticket, authToken);
            var viewModel = Mapper.Map<BoxViewModel, IdentityViewModel>(model);

            return this.Register(viewModel);
        }
    }
}