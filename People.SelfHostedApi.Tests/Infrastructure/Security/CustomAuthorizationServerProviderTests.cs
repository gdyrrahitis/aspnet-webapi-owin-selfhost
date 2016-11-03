namespace People.SelfHostedApi.Tests.Infrastructure.Security
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.OAuth;
    using Moq;
    using NUnit.Framework;
    using SelfHostedApi.Infrastructure.Security;
    using static NUnit.Framework.Assert;

    [TestFixture]
    public class CustomAuthorizationServerProviderTests
    {
        private const string Username = "user1";
        private const string Password = "pass123";
        private Mock<ApplicationUserManager> _applicationUserManagerMock;

        [SetUp]
        public void Setup()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _applicationUserManagerMock = new Mock<ApplicationUserManager>(userStoreMock.Object);
        }

        [Test]
        public async Task GrantResourceOwnerCredentials_UserIsNotFound_GeneratesInvalidGrant_Test()
        {
            // Arrange

            var owinContextMock = new Mock<IOwinContext>();
            var contextMock = GetContextMock(owinContextMock);
            SetupApplicationUserManagerMock(null);
            var provider = new CustomAuthorizationServerProvider
            {
                UserManager = _applicationUserManagerMock.Object
            };

            // Act
            await provider.GrantResourceOwnerCredentials(contextMock.Object);

            // Assert
            _applicationUserManagerMock.Verify(m => m.FindAsync(It.Is<string>(u => u == Username), It.Is<string>(u => u == Password)), Times.Once());
            AreEqual(true, contextMock.Object.HasError);
        }

        private void SetupApplicationUserManagerMock(IdentityUser dataToReturn)
        {
            _applicationUserManagerMock.Setup(
                m => m.FindAsync(It.Is<string>(u => u == Username), It.Is<string>(u => u == Password)))
                .ReturnsAsync(dataToReturn).Verifiable();
        }

        private Mock<OAuthGrantResourceOwnerCredentialsContext> GetContextMock(Mock<IOwinContext> owinContextMock)
        {
            var contextMock = new Mock<OAuthGrantResourceOwnerCredentialsContext>(owinContextMock.Object,
                new OAuthAuthorizationServerOptions(), "clientId", Username, Password, Enumerable.Empty<string>());
            return contextMock;
        }

        [Test]
        public async Task GrantResourceOwnerCredentials_UserIsFound_GeneratesValidGrant_Test()
        {
            // Arrange
            var identityUser = CreateIdentityUser();
            var owinContextMock = new Mock<IOwinContext>();
            var contextMock = GetContextMock(owinContextMock);
            SetupApplicationUserManagerMock(identityUser);
            var provider = new CustomAuthorizationServerProvider
            {
                UserManager = _applicationUserManagerMock.Object
            };

            // Act
            await provider.GrantResourceOwnerCredentials(contextMock.Object);

            // Assert
            _applicationUserManagerMock.Verify(m => m.FindAsync(It.Is<string>(u => u == Username),
                It.Is<string>(u => u == Password)), Times.Once());
            AreEqual(false, contextMock.Object.HasError);
        }

        private static IdentityUser CreateIdentityUser()
        {
            const string id = "12345";
            var identityUser = new IdentityUser
            {
                Id = id,
                UserName = Username
            };
            return identityUser;
        }
    }
}
