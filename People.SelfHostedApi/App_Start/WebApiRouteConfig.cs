namespace People.SelfHostedApi
{
    using System.Web.Http;
    using Controllers;
    using Infrastructure.Formatters;

    public class WebApiRouteConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Formatters.Add(new BrowserFormatter());

            configuration.MapHttpAttributeRoutes();
            configuration.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new { controller = nameof(PeopleController), id = RouteParameter.Optional });
        }
    }
}