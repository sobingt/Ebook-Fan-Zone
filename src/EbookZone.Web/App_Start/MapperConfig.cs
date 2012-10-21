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

            Mapper.CreateMap<FacebookViewModel, RegisterViewModel>()
                .ForMember(dest => dest.NetworkId, opt => opt.MapFrom(src => src.FacebookId))
                .ForMember(dest => dest.AccountType, opt => opt.UseValue(AccountType.Facebook));

            Mapper.CreateMap<TwitterViewModel, RegisterViewModel>()
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.NetworkId, opt => opt.MapFrom(src => src.TwitterId))
                .ForMember(dest => dest.AccountType, opt => opt.UseValue(AccountType.Twitter));

            Mapper.AssertConfigurationIsValid();
        }
    }
}