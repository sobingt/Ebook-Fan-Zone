using System.Configuration;
using System.Web;
using EbookZone.Web.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace EbookZone.Web.Services
{
    public class TwitterService : ITwitterService
    {
        private const string TwitterCallbackUrl = @"http://local.ebook-fan-zone.com/Authorize/TwitterAuth";

        private string TwitterConsumerKey
        {
            get { return ConfigurationManager.AppSettings["TwitterConsumerKey"]; }
        }

        private string TwitterConsumerSecret
        {
            get { return ConfigurationManager.AppSettings["TwitterConsumerSecret"]; }
        }

        public void ExecuteRequest()
        {
            var baseUrl = "http://api.twitter.com";
            var client = new RestClient(baseUrl);
            client.Authenticator = OAuth1Authenticator.ForRequestToken(this.TwitterConsumerKey,
                                                                       this.TwitterConsumerSecret);

            var request = new RestRequest("oauth/request_token", Method.POST);
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            var oauth_token = qs["oauth_token"];
            var oauth_token_secret = qs["oauth_token_secret"];

            request = new RestRequest("oauth/authorize");
            request.AddParameter("oauth_token", oauth_token);
            request.AddParameter("oauth_callback", TwitterCallbackUrl);

            var url = client.BuildUri(request).ToString();
            HttpContext.Current.Response.Redirect(url);
        }

        public TwitterViewModel GetAccount(string verifier, string requestToken)
        {
            const string baseUrl = "http://api.twitter.com";
            var client = new RestClient(baseUrl);

            client.Authenticator = OAuth1Authenticator.ForRequestToken(this.TwitterConsumerKey,
                                                                       this.TwitterConsumerSecret);

            var request = new RestRequest("oauth/access_token", Method.POST);
            request.AddParameter("oauth_verifier", verifier);
            request.AddParameter("oauth_token", requestToken);

            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);
            var oauth_token = qs["oauth_token"];
            var oauth_token_secret = qs["oauth_token_secret"];

            request = new RestRequest("1.1/account/verify_credentials.json");

            client.Authenticator = OAuth1Authenticator.ForProtectedResource(
                TwitterConsumerKey, TwitterConsumerSecret, oauth_token, oauth_token_secret);

            response = client.Execute(request);

            var viewModel = JsonConvert.DeserializeObject<TwitterViewModel>(response.Content);
            return viewModel;
        }
    }
}