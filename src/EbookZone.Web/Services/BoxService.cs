using System.Web;
using System.Xml;
using EbookZone.Web.Models;
using Newtonsoft.Json;
using RestSharp;

namespace EbookZone.Web.Services
{
    public class BoxService : IBoxService
    {
        private const string _apiKey = @"cuyr8txmnzm7itqvu84kxc8myjek9xuw";
        private string _apiUrl2 = @"https://api.box.com/2.0/";
        private const string _apiUrlOld = @"https://www.box.com/api/1.0/";
        private const string _userRedirectAuthUrl = @"https://www.box.com/api/1.0/auth/{0}";
        private const string ResourceRest = "rest";

        public void ExecuteRequest()
        {
            RestClient client = new RestClient(_apiUrlOld);
            RestRequest request = new RestRequest(Method.GET) { Resource = ResourceRest };
            request.AddParameter("action", "get_ticket");
            request.AddParameter("api_key", _apiKey);

            var response = client.Execute<ActionGetTicketResponse>(request);

            HttpContext.Current.Response.Redirect(string.Format(_userRedirectAuthUrl, response.Data.Ticket));
        }

        public BoxViewModel GetAccount(string ticket, string auth_token)
        {
            RestClient client = new RestClient(_apiUrlOld);
            RestRequest request = new RestRequest(Method.GET) { Resource = ResourceRest };
            request.AddParameter("action", "get_auth_token");
            request.AddParameter("api_key", _apiKey);
            request.AddParameter("ticket", ticket);

            var response = client.Execute(request);

            XmlDocument document = new XmlDocument();
            document.LoadXml(response.Content);
            string jsonText = JsonConvert.SerializeXmlNode(document);

            var responseJson = JsonConvert.DeserializeObject<ActionGetAuthTokenResponse>(jsonText);

            return null;
        }
    }

    public class ActionGetTicketResponse
    {
        public string Status { get; set; }

        public string Ticket { get; set; }
    }

    public class BoxUser
    {
        [JsonProperty("login")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("access_id")]
        public string AccessId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public class ActionGetAuthTokenResponse
    {
        [JsonProperty("user")]
        public string User { get; set; }
    }
}