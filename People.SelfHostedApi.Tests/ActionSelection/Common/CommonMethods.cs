namespace People.SelfHostedApi.Tests.ActionSelection.Common
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;

    public class CommonMethods
    {
        public static HttpRequestMessage SetupRequest(string url, string method, HttpConfiguration config, out IHttpRouteData routeData)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), url);
            routeData = config.Routes.GetRouteData(request);
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            return request;
        }
    }
}