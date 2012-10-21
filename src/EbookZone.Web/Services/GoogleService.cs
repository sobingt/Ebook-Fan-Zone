using System;
using System.Web;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using EbookZone.Web.Models;

namespace EbookZone.Web.Services
{
    public class GoogleService : IGoogleService
    {
        #region Fields

        private static readonly OpenIdRelyingParty OpenIdProvider = new OpenIdRelyingParty();
        private const string UserOpenId = @"https://www.google.com/accounts/o8/id";
        private const string CallbackUrl = "http://local.ebook-fan-zone.com/Authorize/GoogleAuth";

        #endregion Fields

        #region Proceed

        public void ExecuteRequest()
        {
            Identifier id;

            if (Identifier.TryParse(UserOpenId, out id))
            {
                IAuthenticationRequest request = OpenIdProvider.CreateRequest(UserOpenId, Realm.AutoDetect, new Uri(CallbackUrl));

                var fetch = new FetchRequest();

                fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);

                request.AddExtension(fetch);

                request.RedirectToProvider();
            }
        }

        public GoogleViewModel GetResponse()
        {
            IAuthenticationResponse response = OpenIdProvider.GetResponse();

            if (response.Status != AuthenticationStatus.Authenticated)
            {
                return null;
            }

            Uri uri = new Uri(response.ClaimedIdentifier.ToString());
            var query = HttpUtility.ParseQueryString(uri.Query);
            string id = query["id"];
            var fetches = response.GetExtension<FetchResponse>();

            GoogleViewModel viewModel = new GoogleViewModel
                {
                    GoogleId = id,
                    FirstName = fetches.GetAttributeValue(WellKnownAttributes.Name.First),
                    LastName = fetches.GetAttributeValue(WellKnownAttributes.Name.Last),
                    Email = fetches.GetAttributeValue(WellKnownAttributes.Contact.Email)
                };

            return viewModel;
        }

        #endregion Proceed
    }
}