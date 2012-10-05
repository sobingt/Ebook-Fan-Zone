using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using EbookZone.Web.Models;

namespace EbookZone.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private static readonly OpenIdRelyingParty OpenIdProvider = new OpenIdRelyingParty();

        public ActionResult Index(string userOpenId)
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