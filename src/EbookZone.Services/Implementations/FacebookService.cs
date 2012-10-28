using System.Configuration;
using System.Web;
using EbookZone.Services.Interfaces;
using EbookZone.ViewModels;
using Newtonsoft.Json;
using RestSharp;

namespace EbookZone.Services.Implementations
{
    public class FacebookService : IFacebookService
    {
        private const string AccessTokenUrl = @"https://graph.facebook.com/oauth/access_token";
        private const string GetFacebookCode = @"https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}";

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

        public void ExecuteRequest()
        {
            var url = string.Format(GetFacebookCode, this.ClientID, this.FacebookCallbackUrl);
            HttpContext.Current.Response.Redirect(url);
        }

        public FacebookViewModel GetAccount(string secretCode)
        {
            var client = new RestClient(AccessTokenUrl);
            var request = new RestRequest(Method.GET);

            request.AddParameter("client_id", this.ClientID);
            request.AddParameter("redirect_uri", this.FacebookCallbackUrl);
            request.AddParameter("client_secret", this.ClientSecret);
            request.AddParameter("code", secretCode);

            var response = client.Execute(request);

            var pairResponse = response.Content.Split('&');
            var accessToken = pairResponse[0].Split('=')[1];

            client = new RestClient("https://graph.facebook.com/me");
            request = new RestRequest(Method.GET);

            request.AddParameter("access_token", accessToken);

            response = client.Execute(request);

            var facebookModel = JsonConvert.DeserializeObject<FacebookViewModel>(response.Content);
            return facebookModel;
        }
    }
}