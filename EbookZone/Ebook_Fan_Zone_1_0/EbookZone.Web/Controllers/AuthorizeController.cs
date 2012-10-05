using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace EbookZone.Web.Controllers
{
    public class AuthorizeController : Controller
    {
        private static readonly OpenIdRelyingParty openIdProvider = new OpenIdRelyingParty();

        public ActionResult Index(string userOpenId)
        {
            // Response from provider
            IAuthenticationResponse response = openIdProvider.GetResponse();

            if (response == null)
            {
                Identifier id;

                if (Identifier.TryParse(userOpenId, out id))
                {
                    try
                    {
                        IAuthenticationRequest request = openIdProvider.CreateRequest(userOpenId);
                        FetchRequest fetch = new FetchRequest();
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.First, true));
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Last, true));
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.BirthDate.WholeBirthDate, true));
                        request.AddExtension(fetch);

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
                        //var fetches = response.GetExtension<FetchResponse>(); - fetches - get google account informations

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