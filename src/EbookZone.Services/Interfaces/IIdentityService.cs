using EbookZone.Domain.Enums;
using EbookZone.ViewModels;

namespace EbookZone.Services.Interfaces
{
    public interface IIdentityService
    {
        bool Register(IdentityViewModel viewModel);

        bool Login(IdentityViewModel viewModel, AccountType accountType, bool afterRegster);

        void LogOff();
    }
}