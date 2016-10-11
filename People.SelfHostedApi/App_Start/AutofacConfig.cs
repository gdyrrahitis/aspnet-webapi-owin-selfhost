namespace People.SelfHostedApi
{
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Controllers;
    using Domain.Context;
    using Domain.Entities;
    using Domain.Repository;
    using Infrastructure.Security;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Services.Person;

    public class AutofacConfig
    {
        public static void Register(HttpConfiguration configuration, out IContainer container)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ApplicationDbContext>().As<BaseDbContext>();
            builder.RegisterType<ApplicationDbContext>().AsSelf();
            builder.RegisterType<ApplicationUserManager>().AsSelf();
            builder.Register(c => new UserStore<IdentityUser>(c.Resolve<ApplicationDbContext>()))
                .AsImplementedInterfaces();

            builder.RegisterType<Repository<Person, int>>().As<IRepository<Person, int>>();
            builder.RegisterType<PersonService>().As<IPersonService>().InstancePerRequest();
            builder.RegisterType<UserController>().PropertiesAutowired();
            builder.RegisterType<CustomAuthorizationServerProvider>().PropertiesAutowired();

            container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
