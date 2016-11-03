namespace People.SelfHostedApi.Tests.Common
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;

    public class HttpRequestMessageCommons
    {
        public static HttpRequestMessage SetupHttpRequestMessage(string url, string method)
        {
            var httpMethod = new HttpMethod(method);
            return new HttpRequestMessage(httpMethod, url);
        }

        public static void SetupRequestProperties(HttpRequestMessage request, IHttpRouteData routeData, 
            HttpConfiguration config)
        {
            SetupRequestRouteDataKeyProperty(request, routeData);
            SetupRequestHttpConfigurationKeyProperty(request, config);
        }

        public static void SetupRequestRouteDataKeyProperty(HttpRequestMessage request,
            IHttpRouteData routeData)
        {
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
        }

        public static void SetupRequestHttpConfigurationKeyProperty(HttpRequestMessage request,
            HttpConfiguration config)
        {
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }
    }
}