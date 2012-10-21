using AutoMapper;
using EbookZone.Domain.Enums;
using EbookZone.Web.Models;

namespace EbookZone.Web.App_Start
{
    public class MapperConfig
    {
        public static void InitializeAutoMapper()
        {
            Mapper.CreateMap<GoogleViewModel, IdentityViewModel>()
                .ForMember(dest => dest.NetworkId, opt => opt.MapFrom(src => src.GoogleId))
                .ForMember(dest => dest.AccountType, opt => opt.UseValue(AccountType.Google))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            Mapper.CreateMap<FacebookViewModel, IdentityViewModel>()
                .ForMember(dest => dest.NetworkId, opt => opt.MapFrom(src => src.FacebookId))
                .ForMember(dest => dest.AccountType, opt => opt.UseValue(AccountType.Facebook))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            Mapper.CreateMap<TwitterViewModel, IdentityViewModel>()
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.NetworkId, opt => opt.MapFrom(src => src.TwitterId))
                .ForMember(dest => dest.AccountType, opt => opt.UseValue(AccountType.Twitter))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}