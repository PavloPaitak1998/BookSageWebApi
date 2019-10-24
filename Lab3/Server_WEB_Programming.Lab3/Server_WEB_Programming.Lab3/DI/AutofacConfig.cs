using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

using AutoMapper;

using Server_WEB_Programming.Lab2.Dal.UoW.Implementations;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;

namespace Server_WEB_Programming.Lab3.DI
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            builder
                .RegisterType<UnitOfWork>()
                .As<IUnitOfWork>();

            builder.RegisterAssemblyTypes()
                .AssignableTo(typeof(Profile))
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
                    {
                        foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                        {
                            cfg.AddProfile(profile);
                        }
                    }))
                .AsSelf()
                .SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}