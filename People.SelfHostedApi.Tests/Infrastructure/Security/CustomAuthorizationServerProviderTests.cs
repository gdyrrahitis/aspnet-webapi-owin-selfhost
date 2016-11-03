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
            const string username = "user1";
            const string password = "pass123";
            var owinContextMock = new Mock<IOwinContext>();
            var contextMock = new Mock<OAuthGrantResourceOwnerCredentialsContext>(owinContextMock.Object,
                new OAuthAuthorizationServerOptions(), "clientId", username, password, Enumerable.Empty<string>());
            _applicationUserManagerMock.Setup(
                m => m.FindAsync(It.Is<string>(u => u == username), It.Is<string>(u => u == password)))
                .ReturnsAsync(null).Verifiable();
            var provider = new CustomAuthorizationServerProvider { UserManager = _applicationUserManagerMock.Object };

            // Act
            await provider.GrantResourceOwnerCredentials(contextMock.Object);

            // Assert
            _applicationUserManagerMock.Verify(m => m.FindAsync(It.Is<string>(u => u == username), It.Is<string>(u => u == password)), Times.Once());
            AreEqual(true, contextMock.Object.HasError);
        }

        [Test]
        public async Task GrantResourceOwnerCredentials_UserIsFound_GeneratesValidGrant_Test()
        {
            // Arrange
            const string id = "12345";
            const string username = "user1";
            const string password = "pass123";
            var owinContextMock = new Mock<IOwinContext>();
            var contextMock = new Mock<OAuthGrantResourceOwnerCredentialsContext>(owinContextMock.Object,
                new OAuthAuthorizationServerOptions(), "clientId", username, password, Enumerable.Empty<string>());
            _applicationUserManagerMock.Setup(
                m => m.FindAsync(It.Is<string>(u => u == username), It.Is<string>(u => u == password)))
                .ReturnsAsync(new IdentityUser
                {
                    Id = id,
                    UserName = username
                }).Verifiable();
            var provider = new CustomAuthorizationServerProvider { UserManager = _applicationUserManagerMock.Object };

            // Act
            await provider.GrantResourceOwnerCredentials(contextMock.Object);

            // Assert
            _applicationUserManagerMock.Verify(m => m.FindAsync(It.Is<string>(u => u == username), It.Is<string>(u => u == password)), Times.Once());
            AreEqual(false, contextMock.Object.HasError);
        }
    }
}
