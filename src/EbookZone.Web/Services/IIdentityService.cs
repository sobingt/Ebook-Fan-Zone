using EbookZone.Web.Models;

namespace EbookZone.Web.Services
{
    public interface IIdentityService
    {
        bool Register(IdentityViewModel viewModel);

        bool Login(IdentityViewModel viewModel);
    }
}