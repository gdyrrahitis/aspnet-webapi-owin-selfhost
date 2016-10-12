namespace People.Domain.Tests.Repository
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Domain.Context;
    using Domain.Repository;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class RepositoryTests
    {
        private Mock<BaseDbContext> _dbContextMock;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<BaseDbContext>();
        }

        [Test]
        public void RepositoryOfObjectWithStringKey_IsInstanceOfRepositoryOfObjectWithStringKey_Test()
        {
            // Arrange
            var type = typeof(Repository<object, string>);
            var instance = new Repository<object, string>(_dbContextMock.Object);

            // Act | Assert
            Assert.IsInstanceOf(type, instance);
        }

        [Test]
        public void Find_ForGivenKey_ReturnsAnEntityStubInstance()
        {
            // Arrange
            const string key = "123";
            const string nonExistentKey = "1234";
            var entityStub = new EntityStub();
            var dbSetMock = new Mock<IDbSet<EntityStub>>();
            dbSetMock.Setup(m => m.Find(It.Is<string>(s => s == key))).Returns(entityStub);
            dbSetMock.Setup(m => m.Find(It.Is<string>(s => s == nonExistentKey))).Returns(() => null);

            _dbContextMock.Setup(s => s.GetDbSet<EntityStub>()).Returns(() => dbSetMock.Object);
            var repository = new Repository<EntityStub, string>(_dbContextMock.Object);

            // Act
            var nullResult = repository.Find(nonExistentKey);
            var result = repository.Find(key);

            // Assert
            Assert.IsNull(nullResult);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EntityStub>(result);
        }

        [Test]
        public void Update_EntryWasCalledTwiceForDetachingAndModifying_SaveChangesWasCalledOnce_Test()
        {
            // Arrange
            _dbContextMock.Setup(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>())).Verifiable();
            _dbContextMock.Setup(m => m.SaveChanges()).Verifiable();
            var repository = new Repository<EntityStub,string>(_dbContextMock.Object);

            // Act
            repository.Update(new EntityStub());

            // Assert
            _dbContextMock.Verify(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void Delete_EntryWasCalledOnce_SaveChangesWasCalledOnce_Test()
        {
            // Arrange
            _dbContextMock.Setup(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>())).Verifiable();
            _dbContextMock.Setup(m => m.SaveChanges()).Verifiable();
            var repository = new Repository<EntityStub, string>(_dbContextMock.Object);

            // Act
            repository.Delete(new EntityStub());

            // Assert
            _dbContextMock.Verify(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void Create_EntryWasCalledOnce_SaveChangesWasCalledOnce_Test() {
            // Arrange
            _dbContextMock.Setup(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>())).Verifiable();
            _dbContextMock.Setup(m => m.SaveChanges()).Verifiable();
            var repository = new Repository<EntityStub, string>(_dbContextMock.Object);

            // Act
            repository.Create(new EntityStub());

            // Assert
            _dbContextMock.Verify(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        public class EntityStub { }
    }
}
