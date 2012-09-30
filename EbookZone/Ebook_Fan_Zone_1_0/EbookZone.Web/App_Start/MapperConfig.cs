using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EbookZone.Domain;
using EbookZone.Web.Models.Base;

namespace EbookZone.Web.App_Start
{
    public class MapperConfig
    {
        public static void InitializeAutoMapper()
        {
            //Mapper.CreateMap<BaseEntity, BaseViewModel>();
            //Mapper.CreateMap<BaseViewModel, BaseEntity>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}