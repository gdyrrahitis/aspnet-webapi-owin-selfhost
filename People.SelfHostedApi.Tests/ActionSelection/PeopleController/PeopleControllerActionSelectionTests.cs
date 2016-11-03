namespace People.SelfHostedApi.Tests.ActionSelection.PeopleController
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;
    using NUnit.Framework;
    using SelfHostedApi.Controllers;
    using static NUnit.Framework.Assert;
    using static Common.HttpConfigurationCommons;
    using static Common.HttpRequestMessageCommons;
    using static Common.HttpRouteDataCommons;

    [TestFixture]
    public class PeopleControllerActionSelectionTests
    {
        [Test]
        [TestCase("http://localhost:3001/api/people/", "GET", typeof(PeopleController), "Get")]
        [TestCase("http://localhost:3001/api/people/1", "GET", typeof(PeopleController), "Get")]
        [TestCase("http://localhost:3001/api/people/1", "PUT", typeof(PeopleController), "Put")]
        [TestCase("http://localhost:3001/api/people/1", "DELETE", typeof(PeopleController), "Delete")]
        [TestCase("http://localhost:3001/api/people/", "POST", typeof(PeopleController), "Post")]
        public void CorrectControllerAndActionAreSelected_Test(string url, string method, Type controller, string action)
        {
            // Arrange
            var config = SetupHttpConfiguration();
            var actionSelector = config.Services.GetActionSelector();
            var controllerSelector = config.Services.GetHttpControllerSelector();
            var request = SetupHttpRequestMessageRequest(url, method);
            var routeData = SetupRouteData(request, config);
            SetupRequestProperties(request, routeData, config);

            // Act
            var controllerDescriptor = controllerSelector.SelectController(request);
            var context = SetupHttpControllerContext(config, routeData, request, controllerDescriptor);
            var actionDescriptor = actionSelector.SelectAction(context);

            // Assert
            AreEqual(controller, controllerDescriptor.ControllerType);
            AreEqual(action, actionDescriptor.ActionName);
        }

        private static HttpControllerContext SetupHttpControllerContext(HttpConfiguration config, IHttpRouteData routeData,
            HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor)
        {
            var context = new HttpControllerContext(config, routeData, request)
            {
                ControllerDescriptor = controllerDescriptor
            };
            return context;
        }
    }
}