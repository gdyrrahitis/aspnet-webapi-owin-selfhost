namespace People.SelfHostedApi.Tests.ActionSelection.UserController
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using NUnit.Framework;
    using SelfHostedApi.Controllers;
    using static NUnit.Framework.Assert;
    using static Common.HttpRequestMessageCommons;
    using static Common.HttpRouteDataCommons;

    [TestFixture]
    public class UserControllerActionSelectionTests
    {
        [Test]
        [TestCase("http://localhost:3001/api/user/", "GET", typeof(UserController), "Get")]
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
            var context = new HttpControllerContext(config, routeData, request)
            {
                ControllerDescriptor = controllerDescriptor
            };
            var actionDescriptor = actionSelector.SelectAction(context);

            // Assert
            AreEqual(controller, controllerDescriptor.ControllerType);
            AreEqual(action, actionDescriptor.ActionName);
        }

        private static HttpConfiguration SetupHttpConfiguration()
        {
            var config = new HttpConfiguration();
            WebApiRouteConfig.Register(config);
            config.EnsureInitialized();
            return config;
        }
    }
}
