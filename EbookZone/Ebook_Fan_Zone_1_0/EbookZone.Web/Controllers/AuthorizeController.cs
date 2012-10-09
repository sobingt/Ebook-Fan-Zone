using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using EbookZone.Domain.Enums;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace EbookZone.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        // Google Register Helper
        private static readonly OpenIdRelyingParty OpenIdProvider = new OpenIdRelyingParty();
        private const string userOpenId = @"https://www.google.com/accounts/o8/id";

        // Facebook Register Helper
        private const string getFacebookCode =
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

        public ActionResult Index(string registerType)
        {
            AccountType accountType = (AccountType)Enum.Parse(typeof(AccountType), registerType);

            switch (accountType)
            {
                case AccountType.Default:
                    break;
                case AccountType.Google:
                    return RegisterGoogleAccount();
                case AccountType.Facebook:
                    var request = string.Format(getFacebookCode, this.ClientID, this.FacebookCallbackUrl);
                    return Redirect(request);
                    break;
                case AccountType.Twitter:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult FacebookAuth()
        {
            if (Request.Params.AllKeys.Contains("code"))
            {
                var code = Request.Params["code"];
                RestClient client = new RestClient("https://graph.facebook.com/oauth/access_token");
                RestRequest request = new RestRequest(Method.GET);

                //request.AddParameter("action", "access_token");
                request.AddParameter("client_id", this.ClientID);
                request.AddParameter("redirect_uri", this.FacebookCallbackUrl);
                request.AddParameter("client_secret", this.ClientSecret);
                request.AddParameter("code", code);

                RestResponse response = client.Execute(request);

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

                if (Identifier.TryParse(userOpenId, out id))
                {
                    try
                    {
                        IAuthenticationRequest request = OpenIdProvider.CreateRequest(userOpenId);

                        var fetch = new FetchRequest();

                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);

                        request.AddExtension(fetch);

                        request.AddExtension(new ClaimsRequest()
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