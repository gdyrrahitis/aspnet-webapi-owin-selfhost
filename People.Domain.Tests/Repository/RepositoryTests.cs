namespace People.Domain.Tests.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Domain.Context;
    using Domain.Repository;
    using Moq;
    using NUnit.Framework;
    using static NUnit.Framework.Assert;

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
            IsInstanceOf(type, instance);
        }

        [Test]
        public void All_ReturnsAnIQueryableOfEntityStub_Test()
        {
            // Arrange
            var entityStubs = SetupEntityStubsQueryable();
            var dbSetMock = GetDbSetMock();
            SetupIQueryable(dbSetMock, entityStubs);
            _dbContextMock.Setup(s => s.GetDbSet<EntityStub>()).Returns(dbSetMock.Object);
            var repository = CreateRepository();

            // Act
            var result = repository.All.ToList();

            // Assert
            IsNotEmpty(result);
            AreEqual(3, result.Count);
        }

        private static IQueryable<EntityStub> SetupEntityStubsQueryable()
        {
            var entityStubs = new List<EntityStub>
            {
                new EntityStub(),
                new EntityStub(),
                new EntityStub()
            }.AsQueryable();
            return entityStubs;
        }

        private static Mock<IDbSet<EntityStub>> GetDbSetMock()
        {
            var dbSetMock = new Mock<IDbSet<EntityStub>>();
            return dbSetMock;
        }

        private static void SetupIQueryable(Mock<IDbSet<EntityStub>> dbSetMock, IQueryable<EntityStub> entityStubs)
        {
            dbSetMock.Setup(s => s.Provider).Returns(entityStubs.Provider);
            dbSetMock.Setup(s => s.Expression).Returns(entityStubs.Expression);
            dbSetMock.Setup(s => s.ElementType).Returns(entityStubs.ElementType);
        }

        private Repository<EntityStub, string> CreateRepository()
        {
            return new Repository<EntityStub, string>(_dbContextMock.Object);
        }

        [Test]
        public void All_ReturnsAnEmptyCollection_Test()
        {
            // Arrange
            var expected = Enumerable.Empty<EntityStub>().AsQueryable();
            var dbSetMock = GetDbSetMock();
            SetupIQueryable(dbSetMock, expected);
            _dbContextMock.Setup(s => s.GetDbSet<EntityStub>()).Returns(dbSetMock.Object);
            var repository = CreateRepository();

            // Act
            var result = repository.All;

            // Assert
            IsEmpty(result);
        }

        [Test]
        public void Find_ForGivenKey_ReturnsAnEntityStubInstance()
        {
            // Arrange
            const string key = "123";
            const string nonExistentKey = "1234";
            var entityStub = new EntityStub();
            var dbSetMock = GetDbSetMock();
            dbSetMock.Setup(m => m.Find(It.Is<string>(s => s == key))).Returns(entityStub);
            dbSetMock.Setup(m => m.Find(It.Is<string>(s => s == nonExistentKey))).Returns(() => null);
            _dbContextMock.Setup(s => s.GetDbSet<EntityStub>()).Returns(() => dbSetMock.Object);
            var repository = CreateRepository();

            // Act
            var nullResult = repository.Find(nonExistentKey);
            var result = repository.Find(key);

            // Assert
            IsNull(nullResult);
            IsNotNull(result);
            IsInstanceOf<EntityStub>(result);
        }

        [Test]
        public void Update_EntryWasCalledTwiceForDetachingAndModifying_SaveChangesWasCalledOnce_Test()
        {
            // Arrange
            _dbContextMock.Setup(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>())).Verifiable();
            _dbContextMock.Setup(m => m.SaveChanges()).Verifiable();
            var repository = CreateRepository();

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
            var repository = CreateRepository();

            // Act
            repository.Delete(new EntityStub());

            // Assert
            _dbContextMock.Verify(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void Create_EntryWasCalledOnce_SaveChangesWasCalledOnce_Test()
        {
            // Arrange
            _dbContextMock.Setup(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>())).Verifiable();
            _dbContextMock.Setup(m => m.SaveChanges()).Verifiable();
            var repository = CreateRepository();

            // Act
            repository.Create(new EntityStub());

            // Assert
            _dbContextMock.Verify(m => m.Entry(It.IsAny<EntityStub>(), It.IsAny<Action<DbEntityEntry<EntityStub>>>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        public class EntityStub { }
    }
}
