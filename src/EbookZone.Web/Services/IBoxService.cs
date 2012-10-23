using EbookZone.Web.Models;

namespace EbookZone.Web.Services
{
    public interface IBoxService
    {
        void ExecuteRequest();

        BoxViewModel GetAccount(string ticket, string auth_token);
    }
}