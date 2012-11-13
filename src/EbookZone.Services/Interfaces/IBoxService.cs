using EbookZone.ViewModels;

namespace EbookZone.Services.Interfaces
{
    public interface IBoxService
    {
        void ExecuteRequest();

        BoxViewModel GetAccount(string ticket, string authToken);
    }
}