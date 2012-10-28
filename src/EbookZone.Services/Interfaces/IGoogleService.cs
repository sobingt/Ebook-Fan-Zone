using EbookZone.ViewModels;

namespace EbookZone.Services.Interfaces
{
    public interface IGoogleService
    {
        void ExecuteRequest();

        GoogleViewModel GetAccount();
    }
}