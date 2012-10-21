using EbookZone.Web.Models;

namespace EbookZone.Web.Services
{
    public interface IFacebookService
    {
        void ExecuteRequest();

        FacebookViewModel GetAccount(string secretCode);
    }
}