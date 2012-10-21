using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
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

            // register types
            container.RegisterInstance(Mapper.Engine);
            // container.RegisterType<ISayHelloService, SayHelloService>();
        }
    }
}