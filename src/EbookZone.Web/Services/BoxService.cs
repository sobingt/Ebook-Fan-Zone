using System.Web;
using System.Xml;
using EbookZone.Web.Models;
using RestSharp;

namespace EbookZone.Web.Services
{
    public class BoxService : IBoxService
    {
        private const string ApiKey = @"cuyr8txmnzm7itqvu84kxc8myjek9xuw";
        private string _apiUrl2 = @"https://api.box.com/2.0/";
        private const string ApiUrlOld = @"https://www.box.com/api/1.0/";
        private const string UserRedirectAuthUrl = @"https://www.box.com/api/1.0/auth/{0}";
        private const string ResourceRest = "rest";

        public void ExecuteRequest()
        {
            var client = new RestClient(ApiUrlOld);
            var request = new RestRequest(Method.GET) { Resource = ResourceRest };
            request.AddParameter("action", "get_ticket");
            request.AddParameter("api_key", ApiKey);

            var response = client.Execute<ActionGetTicketResponse>(request);

            HttpContext.Current.Response.Redirect(string.Format(UserRedirectAuthUrl, response.Data.Ticket));
        }

        public BoxViewModel GetAccount(string ticket, string auth_token)
        {
            var client = new RestClient(ApiUrlOld);
            var request = new RestRequest(Method.GET) { Resource = ResourceRest };
            request.AddParameter("action", "get_auth_token");
            request.AddParameter("api_key", ApiKey);
            request.AddParameter("ticket", ticket);

            var response = client.Execute(request);

            var xml = new XmlDocument();
            xml.LoadXml(response.Content);

            XmlNode xnNode = xml.SelectSingleNode("/response/user");

            if(xnNode != null)
            {
                var viewModel = new BoxViewModel
                                    {
                                        Email = xnNode["email"].InnerXml,
                                        UserName = xnNode["login"].InnerXml,
                                        UserId = xnNode["user_id"].InnerXml
                                    };

                return viewModel;
            }

            return null;
        }
    }

    public class ActionGetTicketResponse
    {
        public string Status { get; set; }

        public string Ticket { get; set; }
    }
}