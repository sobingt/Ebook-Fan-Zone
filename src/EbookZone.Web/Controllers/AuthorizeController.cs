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

        public AuthorizeController(IGoogleService googleService, IFacebookService facebookService, ITwitterService twitterService)
        {
            _googleService = googleService;
            _facebookService = facebookService;
            _twitterService = twitterService;
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

        public ActionResult FacebookAuth()
        {
            if (Request.Params.AllKeys.Contains("code"))
            {
                var code = Request.Params["code"];

                var facebookModel = _facebookService.GetAccount(code);
                var viewModel = Mapper.Map<FacebookViewModel, RegisterViewModel>(facebookModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult TwitterAuth()
        {
            var verifier = Request.Params["oauth_verifier"];
            var requestToken = Request.Params["oauth_token"];

            var twitterViewModel = _twitterService.GetAccount(verifier, requestToken);
            var viewModel = Mapper.Map<TwitterViewModel, RegisterViewModel>(twitterViewModel);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GoogleAuth()
        {
            var model = this._googleService.GetAccount();
            var viewModel = Mapper.Map<GoogleViewModel, RegisterViewModel>(model);

            return RedirectToAction("Index", "Home");
        }
    }
}