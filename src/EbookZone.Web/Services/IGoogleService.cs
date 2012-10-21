using EbookZone.Web.Models;

namespace EbookZone.Web.Services
{
    public interface IGoogleService
    {
        void ExecuteRequest();

        GoogleViewModel GetResponse();
    }
}