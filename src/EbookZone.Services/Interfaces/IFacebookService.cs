using EbookZone.ViewModels;

namespace EbookZone.Services.Interfaces
{
    public interface IFacebookService
    {
        void ExecuteRequest();

        FacebookViewModel GetAccount(string secretCode);
    }
}