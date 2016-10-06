namespace People.SelfHostedApi.Tests.Routes
{
    using System.Net.Http;
    using System.Web.Http;
    using NUnit.Framework;

    [TestFixture]
    public class RoutingTests
    {
        [TestCase("http://localhost:3001/invalid/route", "GET", false, null, null)]
        [TestCase("http://localhost:3001/api/people/", "GET", true, "people", null)]
        [TestCase("http://localhost:3001/api/people/1", "GET", true, "people", "1")]
        [TestCase("http://localhost:3001/api/user/", "GET", true, "user", null)]
        public void DefaultRoute_Returns_Correct_RouteData_Test(string url, string method, bool exists, string controller, string id)
        {
            // Arrange
            HttpRequestMessage request;
            var config = SetupRequest(url, method, out request);

            // Act
            var routeData = config.Routes.GetRouteData(request);

            // Assert
            Assert.AreEqual(exists, routeData != null);
            if (exists)
            {
                Assert.AreEqual(controller, routeData.Values["controller"]);
                Assert.AreEqual(id ?? (object)RouteParameter.Optional, routeData.Values["id"]);
            }
        }

        private static HttpConfiguration SetupRequest(string url, string method, out HttpRequestMessage request)
        {
            var config = new HttpConfiguration();
            WebApiRouteConfig.Register(config);
            config.EnsureInitialized();
            var httpMethod = new HttpMethod(method);
            request = new HttpRequestMessage(httpMethod, url);
            return config;
        }
    }
}
