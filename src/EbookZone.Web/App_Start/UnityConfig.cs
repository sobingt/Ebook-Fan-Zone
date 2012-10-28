using System.Web.Mvc;
using AutoMapper;
using EbookZone.Domain;
using EbookZone.Repository;
using EbookZone.Repository.Base;
using EbookZone.Services.Implementations;
using EbookZone.Services.Interfaces;
using EbookZone.Web.Core;
using Microsoft.Practices.Unity;

namespace EbookZone.Web.App_Start
{
    public class UnityConfig
    {
        public static void RegisterTypes()
        {
            var container = new UnityContainer();

            var factory = new UnityControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(factory);

            container.RegisterType<IGoogleService, GoogleService>();
            container.RegisterType<IFacebookService, FacebookService>();
            container.RegisterType<IIdentityService, IdentityService>();
            container.RegisterType<IBoxService, BoxService>();

            // Repositories
            container.RegisterType<IEntityRepository<User>, UserRepository>();

            // register types
            container.RegisterInstance(Mapper.Engine);
        }
    }
}