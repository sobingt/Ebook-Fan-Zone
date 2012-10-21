using EbookZone.Web.Models;

namespace EbookZone.Web.Services
{
    public interface ITwitterService
    {
        void ExecuteRequest();

        TwitterViewModel GetAccount(string verifier, string requestToken);
    }
}