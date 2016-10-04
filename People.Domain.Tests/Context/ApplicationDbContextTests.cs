namespace People.Domain.Tests.Context
{
    using System;
    using Domain.Context;
    using Microsoft.AspNet.Identity.EntityFramework;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationDbContextTests
    {
        [Test]
        public void Create_ReturnsInstaceOf_ApplicationDbContext_Test()
        {
            // Arrange
            Func<ApplicationDbContext> createDelegate = ApplicationDbContext.Create;

            // Act
            var result = createDelegate();

            // Assert
            Assert.IsInstanceOf<ApplicationDbContext>(result);
        }

        [Test]
        public void ApplicationDbContext_IsDerivingFromBaseDbContext_Test()
        {
            // Arrange
            var instance = ApplicationDbContext.Create();

            // Act | Assert
            Assert.IsInstanceOf<BaseDbContext>(instance);
        }

        [Test]
        public void ApplicationDbContext_IBaseDbContextContractIsImplemented_Test()
        {
            // Arrange
            var instance = ApplicationDbContext.Create();

            // Act | Assert
            Assert.IsInstanceOf<IBaseDbContext>(instance);
        }

        [Test]
        public void ApplicationDbContext_IsDerivingFromIdentityDbContext_Test()
        {
            // Arrange
            var instance = ApplicationDbContext.Create();

            // Act | Assert
            Assert.IsInstanceOf<IdentityDbContext<IdentityUser>>(instance);
        }

        [Test]
        public void ApplicationDbContext_CallingEntryWillThrowApplicationExceptionInOrderToUseTheOneFromIBaseContextInterface_Test()
        {
            // Arrange
            var context = ApplicationDbContext.Create();
            var entityStub = new EntityStub();

            // Act | Assert
            Assert.Throws<ApplicationException>(() => context.Entry(entityStub), "Use overload for unit tests.");
        }

        [Test]
        public void ApplicationDbContext_CallingSetWillThrowApplicationExceptionInOrderToUseTheOneFromIBaseContextInterface_Test()
        {
            // Arrange
            var context = ApplicationDbContext.Create();

            // Act | Assert
            Assert.Throws<ApplicationException>(() => context.Set(typeof(EntityStub)), "Use overload for unit tests.");
            Assert.Throws<ApplicationException>(() => context.Set<EntityStub>(), "Use overload for unit tests.");
        }

        private class EntityStub { }
    }
}
