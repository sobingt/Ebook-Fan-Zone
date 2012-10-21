using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using EbookZone.Domain.Enums;
using EbookZone.Web.Models;
using EbookZone.Web.Services;

namespace EbookZone.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IGoogleService _googleService;
        private readonly IFacebookService _facebookService;
        private readonly ITwitterService _twitterService;
        private readonly IIdentityService _identityService;

        public AuthorizeController(IGoogleService googleService, IFacebookService facebookService, ITwitterService twitterService, IIdentityService identityService)
        {
            _googleService = googleService;
            _facebookService = facebookService;
            _twitterService = twitterService;
            _identityService = identityService;
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

            if (accountType == AccountType.Twitter)
            {
                this._twitterService.ExecuteRequest();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Register(string username, string email, string password)
        {
            var viewModel = new IdentityViewModel
                {
                    AccountType = AccountType.Default,
                    Email = email,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    NetworkId = string.Empty,
                    UserName = email,
                    Password = password
                };

            if (_identityService.Register(viewModel))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult FacebookAuth()
        {
            if (Request.Params.AllKeys.Contains("code"))
            {
                var code = Request.Params["code"];

                var facebookModel = _facebookService.GetAccount(code);
                var viewModel = Mapper.Map<FacebookViewModel, IdentityViewModel>(facebookModel);
                if (_identityService.Register(viewModel))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult TwitterAuth()
        {
            var verifier = Request.Params["oauth_verifier"];
            var requestToken = Request.Params["oauth_token"];

            var twitterViewModel = _twitterService.GetAccount(verifier, requestToken);
            var viewModel = Mapper.Map<TwitterViewModel, IdentityViewModel>(twitterViewModel);

            if (_identityService.Register(viewModel))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GoogleAuth()
        {
            var model = this._googleService.GetAccount();
            var viewModel = Mapper.Map<GoogleViewModel, IdentityViewModel>(model);

            if (_identityService.Register(viewModel))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}