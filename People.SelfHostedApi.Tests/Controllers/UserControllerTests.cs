namespace People.SelfHostedApi.Tests.Controllers
{
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Web.Http.Results;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Moq;
    using NUnit.Framework;
    using SelfHostedApi.Controllers;

    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IPrincipal> _userPrincipalMock;
        private Mock<IUserStore<IdentityUser>> _userStoreMock;

        [SetUp]
        public void Setup()
        {
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userPrincipalMock = new Mock<IPrincipal>();
        }

        [Test]
        public void UserControllerGet_CallingGetActionAndAuthenticatedUserIsFound_Returns200OKWithUserInstanceAsContent_Test()
        {
            // Arrange
            const string username = "user";
            const string password = "123";
            var hash = new PasswordHasher().HashPassword(password);
            var user = new IdentityUser(username) { PasswordHash = hash };

            var claim = new Claim(ClaimTypes.NameIdentifier, "myId123");
            var mockIdentity = new Mock<ClaimsIdentity>();
            mockIdentity.Setup(m => m.FindFirst(It.Is<string>(c => c == ClaimTypes.NameIdentifier)))
                .Returns(claim);
            _userPrincipalMock.Setup(m => m.Identity).Returns(mockIdentity.Object);

            _userStoreMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            var applicationUserManager = new ApplicationUserManager(_userStoreMock.Object);
            var controller = new UserController
            {
                UserManager = applicationUserManager,
                User = _userPrincipalMock.Object
            };

            // Act
            var result = controller.Get() as OkNegotiatedContentResult<IdentityUser>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(username, result.Content.UserName);
            Assert.AreEqual(hash, result.Content.PasswordHash);
        }

        [Test]
        public void UserControllerGet_CallingGetActionAndAuthenticatedUserIsNotFound_Returns500ServerException_Test()
        {
            // Arrange
            var mockIdentity = new Mock<ClaimsIdentity>();
            mockIdentity.Setup(m => m.FindFirst(It.Is<string>(c => c == ClaimTypes.NameIdentifier)))
                .Returns(() => null);
            _userPrincipalMock.Setup(m => m.Identity).Returns(mockIdentity.Object);
            var applicationUserManager = new ApplicationUserManager(_userStoreMock.Object);
            var controller = new UserController
            {
                UserManager = applicationUserManager,
                User = _userPrincipalMock.Object
            };

            // Act
            var result = controller.Get() as InternalServerErrorResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
