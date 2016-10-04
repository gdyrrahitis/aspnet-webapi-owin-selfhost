namespace People.SelfHostedApi
{
    using System.Data.Entity;
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Domain.Context;

    public class AutofacConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var container = new ContainerBuilder();
            container.RegisterApiControllers(Assembly.GetExecutingAssembly());

            container.RegisterType<ApplicationDbContext>().As<DbContext>();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container.Build());
        }
    }
}
