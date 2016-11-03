namespace People.SelfHostedApi.Tests.Common
{
    using System.Web.Http;

    public class HttpConfigurationCommons
    {
        public static HttpConfiguration SetupHttpConfiguration()
        {
            var config = new HttpConfiguration();
            WebApiRouteConfig.Register(config);
            config.EnsureInitialized();
            return config;
        }
    }
}
