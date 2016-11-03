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
    using static NUnit.Framework.Assert;

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
            var hash = CreateHashedPassword();
            var user = new IdentityUser(username) { PasswordHash = hash };
            SetupClaimAndClaimIdentityToMockUserPrincipal();

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
            IsNotNull(result);
            IsNotNull(result.Content);
            AreEqual(username, result.Content.UserName);
            AreEqual(hash, result.Content.PasswordHash);
        }

        private static string CreateHashedPassword()
        {
            const string password = "123";
            var hash = new PasswordHasher().HashPassword(password);
            return hash;
        }

        private void SetupClaimAndClaimIdentityToMockUserPrincipal()
        {
            var claim = new Claim(ClaimTypes.NameIdentifier, "myId123");
            var mockIdentity = new Mock<ClaimsIdentity>();
            mockIdentity.Setup(m => m.FindFirst(It.Is<string>(c => c == ClaimTypes.NameIdentifier)))
                .Returns(claim);
            _userPrincipalMock.Setup(m => m.Identity).Returns(mockIdentity.Object);
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
            IsNotNull(result);
        }
    }
}
