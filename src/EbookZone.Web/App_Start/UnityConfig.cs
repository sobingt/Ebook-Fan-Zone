using System.Web.Mvc;
using AutoMapper;
using EbookZone.Web.Core;
using EbookZone.Web.Services;
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
            container.RegisterType<ITwitterService, TwitterService>();

            // register types
            container.RegisterInstance(Mapper.Engine);
        }
    }
}