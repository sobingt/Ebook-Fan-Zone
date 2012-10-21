using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EbookZone.Domain.Enums;
using EbookZone.Web.Models;
using EbookZone.Web.Services;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace EbookZone.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IGoogleService _googleService;
        private readonly IFacebookService _facebookService;

        public AuthorizeController(IGoogleService googleService, IFacebookService facebookService)
        {
            _googleService = googleService;
            _facebookService = facebookService;
        }

        private string TwitterConsumerKey
        {
            get { return ConfigurationManager.AppSettings["TwitterConsumerKey"]; }
        }

        private string TwitterConsumerSecret
        {
            get { return ConfigurationManager.AppSettings["TwitterConsumerSecret"]; }
        }

        private string TwitterCallbackUrl
        {
            get { return ConfigurationManager.AppSettings["TwitterCallbackUrl"]; }
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

            //switch (accountType)
            //{
            //    case AccountType.Default:
            //        break;
            //    case AccountType.Google:
            //        return RegisterGoogleAccount();
            //    case AccountType.Facebook:
            //        var request = string.Format(GetFacebookCode, this.ClientID, this.FacebookCallbackUrl);
            //        return Redirect(request);
            //    case AccountType.Twitter:
            //        return Redirect(GetTwitterAccount());
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

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

        private string GetTwitterAccount()
        {
            var baseUrl = "http://api.twitter.com";
            var client = new RestClient(baseUrl);
            client.Authenticator = OAuth1Authenticator.ForRequestToken(this.TwitterConsumerKey,
                                                                       this.TwitterConsumerSecret);

            var request = new RestRequest("oauth/request_token", Method.POST);
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            var oauth_token = this.Session["oauth_token"] = qs["oauth_token"];
            var oauth_token_secret = this.Session["oauth_token_secret"] = qs["oauth_token_secret"];

            request = new RestRequest("oauth/authorize");
            request.AddParameter("oauth_token", oauth_token);
            request.AddParameter("oauth_callback", this.TwitterCallbackUrl);

            var url = client.BuildUri(request).ToString();

            return url;
        }

        public ActionResult TwitterAuth()
        {
            var verifier = Request.Params["oauth_verifier"];
            var oauth_token = this.Session["oauth_token"].ToString();
            var oauth_token_secret = this.Session["oauth_token_secret"].ToString();

            var baseUrl = "http://api.twitter.com";
            var client = new RestClient(baseUrl);

            var request = new RestRequest("oauth/access_token", Method.POST);

            client.Authenticator = OAuth1Authenticator.ForAccessToken(
                this.TwitterConsumerKey, this.TwitterConsumerSecret, oauth_token, oauth_token_secret, verifier);

            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            oauth_token = qs["oauth_token"];
            oauth_token_secret = qs["oauth_token_secret"];

            request = new RestRequest("1.1/account/verify_credentials.json");
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(
                TwitterConsumerKey, TwitterConsumerSecret, oauth_token, oauth_token_secret
            );

            response = client.Execute(request);
            JObject jObject = JObject.Parse(response.Content);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GoogleAuth()
        {
            GoogleViewModel model = this._googleService.GetAccount();
            RegisterViewModel viewModel = Mapper.Map<GoogleViewModel, RegisterViewModel>(model);

            return RedirectToAction("Index", "Home");
        }
    }
}