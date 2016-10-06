namespace People.SelfHostedApi
{
    using System.Reflection;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Domain.Context;
    using Domain.Entities;
    using Domain.Repository;
    using Services.Person;
    using Owin;

    public class AutofacConfig
    {
        public static void Register(HttpConfiguration configuration, IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ApplicationDbContext>().As<BaseDbContext>();
            builder.RegisterType<Repository<Person, int>>().As<IRepository<Person, int>>();
            builder.RegisterType<PersonService>().As<IPersonService>().InstancePerRequest();

            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(configuration);
        }
    }
}
