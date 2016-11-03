namespace People.SelfHostedApi.Tests.ActionSelection.Common
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Routing;

    public class HttpRouteDataCommons
    {
        public static IHttpRouteData SetupRouteData(HttpRequestMessage request,
    HttpConfiguration config)
        {
            return config.Routes.GetRouteData(request);
        }
    }
}
