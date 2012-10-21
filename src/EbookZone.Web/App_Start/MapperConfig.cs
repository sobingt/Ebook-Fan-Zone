using AutoMapper;
using EbookZone.Domain.Enums;
using EbookZone.Web.Models;

namespace EbookZone.Web.App_Start
{
    public class MapperConfig
    {
        public static void InitializeAutoMapper()
        {
            Mapper.CreateMap<GoogleViewModel, RegisterViewModel>()
                .ForMember(dest => dest.NetworkId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dest => dest.AccountType, opt => opt.UseValue(AccountType.Google))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            Mapper.AssertConfigurationIsValid();
        }
    }
}