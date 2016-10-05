namespace People.Services.Tests.Person
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repository;
    using Moq;
    using NUnit.Framework;
    using Services.Person;

    [TestFixture]
    public class PersonServiceTests
    {
        private Mock<IRepository<Person, int>> _repositoryMock;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository<Person, int>>();
        }

        [Test]
        public void PersonService_IsInstanceOfPersonService_Test()
        {
            // Arrange
            var instance = new PersonService(_repositoryMock.Object);

            // Act | Assert
            Assert.IsInstanceOf<PersonService>(instance);
        }

        [Test]
        public void PersonService_IsImplementingIPersonService_Test()
        {
            // Arrange
            var instance = new PersonService(_repositoryMock.Object);

            // Act | Assert
            Assert.IsInstanceOf<IPersonService>(instance);
        }

        [Test]
        public void GetAllPeople_ReturnsListOfPeopleEntities_Test()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person(),
                new Person()
            };
            _repositoryMock.Setup(p => p.All).Returns(() => people.AsQueryable());
            var service = new PersonService(_repositoryMock.Object);

            // Act
            var result = service.GetPeople().ToList();

            // Assert
            Assert.IsTrue(result.Any());
            Assert.AreEqual(people.Count, result.Count());
        }

        [Test]
        public void GetPerson_ReturnsPersonEntity_Test()
        {
            // Arrange
            const int invalidId = 5;
            const int id = 4;
            const string name = "George";
            const int age = 26;
            var person = new Person
            {
                Id = id,
                Name = name,
                Age = age
            };
            _repositoryMock.Setup(m => m.Find(It.Is<int>(i => i == id))).Returns(person);
            _repositoryMock.Setup(m => m.Find(It.Is<int>(i => i == invalidId))).Returns(() => null);
            var service = new PersonService(_repositoryMock.Object);

            // Act
            var result = service.GetPerson(id);
            var invalidResult = service.GetPerson(invalidId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(invalidResult);
            Assert.IsInstanceOf<Person>(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(age, result.Age);
        }

        [Test]
        public void Update_ShouldCallRepositoryUpdateOnce_Test()
        {
            // Arrange
            const int id = 4;
            const string name = "George";
            const int age = 26;
            var person = new Person
            {
                Id = id,
                Name = name,
                Age = age
            };
            var invalidPerson = new Person();
            _repositoryMock.Setup(m => m.Update(It.Is<Person>(p => p == person)))
                .Verifiable();
            var service = new PersonService(_repositoryMock.Object);

            // Act
            service.Update(person);

            // Assert
            _repositoryMock.Verify(m => m.Update(It.Is<Person>(p => p == person)), Times.Once());
            _repositoryMock.Verify(m => m.Update(It.Is<Person>(p => p == invalidPerson)), Times.Never());
        }

        [Test]
        public void Delete_ShouldCallRepositoryDeleteOnce_Test()
        {
            // Arrange
            const int id = 4;
            const string name = "George";
            const int age = 26;
            var person = new Person
            {
                Id = id,
                Name = name,
                Age = age
            };
            var invalidPerson = new Person();
            _repositoryMock.Setup(m => m.Delete(It.Is<Person>(p => p == person)))
                .Verifiable();
            var service = new PersonService(_repositoryMock.Object);

            // Act
            service.Delete(person);

            // Assert
            _repositoryMock.Verify(m => m.Delete(It.Is<Person>(p => p == person)), Times.Once());
            _repositoryMock.Verify(m => m.Delete(It.Is<Person>(p => p == invalidPerson)), Times.Never());
        }
    }
}
