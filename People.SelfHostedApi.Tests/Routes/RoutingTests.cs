namespace People.SelfHostedApi.Tests.Routes
{
    using System.Net.Http;
    using System.Web.Http;
    using NUnit.Framework;
    using static NUnit.Framework.Assert;

    [TestFixture]
    public class RoutingTests
    {
        [TestCase("http://localhost:3001/invalid/route", "GET", false, null, null)]
        [TestCase("http://localhost:3001/api/people/", "GET", true, "people", null)]
        [TestCase("http://localhost:3001/api/people/", "POST", true, "people", null)]
        [TestCase("http://localhost:3001/api/people/1", "PUT", true, "people", "1")]
        [TestCase("http://localhost:3001/api/people/1", "GET", true, "people", "1")]
        [TestCase("http://localhost:3001/api/people/1", "DELETE", true, "people", "1")]
        [TestCase("http://localhost:3001/api/user/", "GET", true, "user", null)]
        public void DefaultRoute_Returns_Correct_RouteData_Test(string url, string method, bool exists, string controller, string id)
        {
            // Arrange
            var config = SetupHttpConfiguration();
            var request = SetupHttpRequestMessage(url, method);

            // Act
            var routeData = config.Routes.GetRouteData(request);

            // Assert
            AreEqual(exists, routeData != null);
            if (exists)
            {
                AreEqual(controller, routeData.Values["controller"]);
                AreEqual(id ?? (object)RouteParameter.Optional, routeData.Values["id"]);
            }
        }

        private HttpConfiguration SetupHttpConfiguration()
        {
            var config = new HttpConfiguration();
            WebApiRouteConfig.Register(config);
            config.EnsureInitialized();
            return config;
        }

        private static HttpRequestMessage SetupHttpRequestMessage(string url, string method)
        {
            var httpMethod = new HttpMethod(method);
            return new HttpRequestMessage(httpMethod, url);
        }
    }
}
