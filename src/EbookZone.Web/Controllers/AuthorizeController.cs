using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using EbookZone.Domain.Enums;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace EbookZone.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        // Google Register Helper
        private static readonly OpenIdRelyingParty OpenIdProvider = new OpenIdRelyingParty();
        private const string UserOpenId = @"https://www.google.com/accounts/o8/id";

        // Facebook Register Helper
        private const string GetFacebookCode =
            @"https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}";

        private string ClientID
        {
            get { return ConfigurationManager.AppSettings["FacebookAppID"]; }
        }

        private string ClientSecret
        {
            get { return ConfigurationManager.AppSettings["FacebookSecret"]; }
        }

        private string FacebookCallbackUrl
        {
            get { return ConfigurationManager.AppSettings["FacebookCallbackUrl"]; }
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

            switch (accountType)
            {
                case AccountType.Default:
                    break;
                case AccountType.Google:
                    return RegisterGoogleAccount();
                case AccountType.Facebook:
                    var request = string.Format(GetFacebookCode, this.ClientID, this.FacebookCallbackUrl);
                    return Redirect(request);
                case AccountType.Twitter:
                    return Redirect(GetTwitterAccount());
                default:
                    throw new ArgumentOutOfRangeException();
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

        public ActionResult FacebookAuth()
        {
            if (Request.Params.AllKeys.Contains("code"))
            {
                var code = Request.Params["code"];
                var client = new RestClient("https://graph.facebook.com/oauth/access_token");
                var request = new RestRequest(Method.GET);

                //request.AddParameter("action", "access_token");
                request.AddParameter("client_id", this.ClientID);
                request.AddParameter("redirect_uri", this.FacebookCallbackUrl);
                request.AddParameter("client_secret", this.ClientSecret);
                request.AddParameter("code", code);

                var response = client.Execute(request);

                var pairResponse = response.Content.Split('&');
                var accessToken = pairResponse[0].Split('=')[1];

                client = new RestClient("https://graph.facebook.com/me");
                request = new RestRequest(Method.GET);

                request.AddParameter("access_token", accessToken);

                response = client.Execute(request);

                JObject jObject = JObject.Parse(response.Content);
            }

            return RedirectToAction("Index", "Home");
        }

        private ActionResult RegisterGoogleAccount()
        {
            // Response from provider
            IAuthenticationResponse response = OpenIdProvider.GetResponse();

            if (response == null)
            {
                Identifier id;

                if (Identifier.TryParse(UserOpenId, out id))
                {
                    try
                    {
                        IAuthenticationRequest request = OpenIdProvider.CreateRequest(UserOpenId);

                        var fetch = new FetchRequest();

                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);

                        request.AddExtension(fetch);

                        request.AddExtension(new ClaimsRequest
                        {
                            FullName = DemandLevel.Require,
                            Nickname = DemandLevel.Require,
                            BirthDate = DemandLevel.Require,
                            Gender = DemandLevel.Require,
                            Country = DemandLevel.Require
                        });

                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        TempData["error"] = ex.Message;
                    }
                }

                return RedirectToAction("Index", "Login");
            }

            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    {
                        var fetches = response.GetExtension<FetchResponse>(); // fetches - get google account informations
                        var profileInfo = response.GetExtension<ClaimsResponse>();

                        TempData["id"] = response.ClaimedIdentifier;
                        return RedirectToAction("Index", "Home");
                    }
                case AuthenticationStatus.Canceled:
                    {
                        TempData["message"] = "Authentification was cancelled";
                        return RedirectToAction("Index", "Login");
                    }
                case AuthenticationStatus.Failed:
                    {
                        TempData["error"] = response.Exception.Message;
                        TempData["message"] = "Authentification was failed";
                        return RedirectToAction("Index", "Login");
                    }
                default:
                    {
                        return RedirectToAction("Index", "Login");
                    }
            }
        }
    }
}