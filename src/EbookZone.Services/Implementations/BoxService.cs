using System.Web;
using System.Xml;
using EbookZone.Services.Interfaces;
using EbookZone.Utils.Constants;
using EbookZone.ViewModels;
using RestSharp;

namespace EbookZone.Services.Implementations
{
    public class BoxService : IBoxService
    {
        public void ExecuteRequest()
        {
            var client = new RestClient(ApplicationConfig.ApiUrlOld);
            var request = new RestRequest(Method.GET) { Resource = ApplicationConfig.ResourceRest };
            request.AddParameter("action", "get_ticket");
            request.AddParameter("api_key", ApplicationConfig.ApiKey);

            var response = client.Execute<ActionGetTicketResponse>(request);

            HttpContext.Current.Response.Redirect(string.Format(ApplicationConfig.UserRedirectAuthUrl, response.Data.Ticket));
        }

        public BoxViewModel GetAccount(string ticket, string auth_token)
        {
            var client = new RestClient(ApplicationConfig.ApiUrlOld);
            var request = new RestRequest(Method.GET) { Resource = ApplicationConfig.ResourceRest };
            request.AddParameter("action", "get_auth_token");
            request.AddParameter("api_key", ApplicationConfig.ApiKey);
            request.AddParameter("ticket", ticket);

            var response = client.Execute(request);

            var xml = new XmlDocument();
            xml.LoadXml(response.Content);

            XmlNode xnNode = xml.SelectSingleNode("/response/user");

            if (xnNode != null &&
                xnNode["email"] != null &&
                xnNode["login"] != null &&
                xnNode["user_id"] != null)
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