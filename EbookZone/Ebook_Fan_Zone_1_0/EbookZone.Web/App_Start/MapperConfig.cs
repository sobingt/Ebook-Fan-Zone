using AutoMapper;
using EbookZone.Domain;
using EbookZone.Web.Models;

namespace EbookZone.Web.App_Start
{
    public class MapperConfig
    {
        public static void InitializeAutoMapper()
        {
            //Mapper.CreateMap<User, RegisterViewModel>();
            //Mapper.CreateMap<RegisterViewModel, User>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}