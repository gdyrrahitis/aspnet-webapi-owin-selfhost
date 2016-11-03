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
    using StubEntities.Common;
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

            // Act
            var instance = new Repository<object, string>(_dbContextMock.Object);

            // Assert
            IsInstanceOf(type, instance);
        }

        [Test]
        public void All_ReturnsAnIQueryableOfEntityStub_Test()
        {
            // Arrange
            var entityStubs = SetupEntityStubsQueryable();
            var dbSetMock = GetDbSetMock();
            SetupIQueryable(dbSetMock, entityStubs);
            _dbContextMock.Setup(s => s.GetDbSet<CommonStubEntity>()).Returns(dbSetMock.Object);
            var repository = CreateRepository();

            // Act
            var result = repository.All.ToList();

            // Assert
            IsNotEmpty(result);
            AreEqual(3, result.Count);
        }

        private static IQueryable<CommonStubEntity> SetupEntityStubsQueryable()
        {
            var entityStubs = new List<CommonStubEntity>
            {
                new CommonStubEntity(),
                new CommonStubEntity(),
                new CommonStubEntity()
            }.AsQueryable();
            return entityStubs;
        }

        private static Mock<IDbSet<CommonStubEntity>> GetDbSetMock()
        {
            var dbSetMock = new Mock<IDbSet<CommonStubEntity>>();
            return dbSetMock;
        }

        private static void SetupIQueryable(Mock<IDbSet<CommonStubEntity>> dbSetMock, IQueryable entityStubs)
        {
            dbSetMock.Setup(s => s.Provider).Returns(entityStubs.Provider);
            dbSetMock.Setup(s => s.Expression).Returns(entityStubs.Expression);
            dbSetMock.Setup(s => s.ElementType).Returns(entityStubs.ElementType);
        }

        private Repository<CommonStubEntity, string> CreateRepository()
        {
            return new Repository<CommonStubEntity, string>(_dbContextMock.Object);
        }

        [Test]
        public void All_ReturnsAnEmptyCollection_Test()
        {
            // Arrange
            var expected = Enumerable.Empty<CommonStubEntity>().AsQueryable();
            var dbSetMock = GetDbSetMock();
            SetupIQueryable(dbSetMock, expected);
            _dbContextMock.Setup(s => s.GetDbSet<CommonStubEntity>()).Returns(dbSetMock.Object);
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
            var entityStub = new CommonStubEntity();
            var dbSetMock = GetDbSetMock();
            dbSetMock.Setup(m => m.Find(It.Is<string>(s => s == key))).Returns(entityStub);
            _dbContextMock.Setup(s => s.GetDbSet<CommonStubEntity>()).Returns(() => dbSetMock.Object);
            var repository = CreateRepository();

            // Act
            var result = repository.Find(key);

            // Assert
            IsNotNull(result);
            IsInstanceOf<CommonStubEntity>(result);
        }

        [Test]
        public void Find_ForNonExistentGivenKey_ReturnsAnNull_Test()
        {
            // Arrange
            const string nonExistentKey = "1234";
            var dbSetMock = GetDbSetMock();
            dbSetMock.Setup(m => m.Find(It.Is<string>(s => s == nonExistentKey))).Returns(() => null);
            _dbContextMock.Setup(s => s.GetDbSet<CommonStubEntity>()).Returns(() => dbSetMock.Object);
            var repository = CreateRepository();

            // Act
            var nullResult = repository.Find(nonExistentKey);

            // Assert
            IsNull(nullResult);
        }

        [Test]
        public void Update_EntryWasCalledOnceForModifying_SaveChangesWasCalledOnce_Test()
        {
            // Arrange
            _dbContextMock.Setup(m => m.Entry(It.IsAny<CommonStubEntity>(), 
                It.IsAny<Action<DbEntityEntry<CommonStubEntity>>>())).Verifiable();
            _dbContextMock.Setup(m => m.SaveChanges()).Verifiable();
            var repository = CreateRepository();

            // Act
            repository.Update(new CommonStubEntity());

            // Assert
            _dbContextMock.Verify(m => m.Entry(It.IsAny<CommonStubEntity>(), 
                It.IsAny<Action<DbEntityEntry<CommonStubEntity>>>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void Delete_EntryWasCalledOnce_SaveChangesWasCalledOnce_Test()
        {
            // Arrange
            _dbContextMock.Setup(m => m.Entry(It.IsAny<CommonStubEntity>(), 
                It.IsAny<Action<DbEntityEntry<CommonStubEntity>>>())).Verifiable();
            _dbContextMock.Setup(m => m.SaveChanges()).Verifiable();
            var repository = CreateRepository();

            // Act
            repository.Delete(new CommonStubEntity());

            // Assert
            _dbContextMock.Verify(m => m.Entry(It.IsAny<CommonStubEntity>(), 
                It.IsAny<Action<DbEntityEntry<CommonStubEntity>>>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void Create_EntryWasCalledOnce_SaveChangesWasCalledOnce_Test()
        {
            // Arrange
            _dbContextMock.Setup(m => m.Entry(It.IsAny<CommonStubEntity>(), 
                It.IsAny<Action<DbEntityEntry<CommonStubEntity>>>())).Verifiable();
            _dbContextMock.Setup(m => m.SaveChanges()).Verifiable();
            var repository = CreateRepository();

            // Act
            repository.Create(new CommonStubEntity());

            // Assert
            _dbContextMock.Verify(m => m.Entry(It.IsAny<CommonStubEntity>(), 
                It.IsAny<Action<DbEntityEntry<CommonStubEntity>>>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
