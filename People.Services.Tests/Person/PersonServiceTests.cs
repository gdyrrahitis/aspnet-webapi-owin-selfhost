namespace People.Services.Tests.Person
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repository;
    using Moq;
    using NUnit.Framework;
    using Services.Person;
    using static NUnit.Framework.Assert;

    [TestFixture]
    public class PersonServiceTests
    {
        private const int Id = 1;
        private const string Name = "George";
        private const int Age = 26;
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
            IsInstanceOf<PersonService>(instance);
        }

        [Test]
        public void PersonService_IsImplementingIPersonService_Test()
        {
            // Arrange
            var instance = new PersonService(_repositoryMock.Object);

            // Act | Assert
            IsInstanceOf<IPersonService>(instance);
        }

        [Test]
        public void GetAllPeople_ReturnsListOfPeopleEntities_Test()
        {
            // Arrange
            var people = GetListOfPeople();
            _repositoryMock.Setup(p => p.All).Returns(() => people.AsQueryable());
            var service = new PersonService(_repositoryMock.Object);

            // Act
            var result = service.GetPeople().ToList();

            // Assert
            IsTrue(result.Any());
            AreEqual(people.Count, result.Count);
        }

        private static List<Person> GetListOfPeople()
        {
            var people = new List<Person>
            {
                new Person(),
                new Person()
            };
            return people;
        }

        [Test]
        public void GetPerson_ReturnsPersonEntity_Test()
        {
            // Arrange
            var person = CreateDefaultPersonObject();
            _repositoryMock.Setup(m => m.Find(It.Is<int>(i => i == Id))).Returns(person);
            var service = new PersonService(_repositoryMock.Object);

            // Act
            var result = service.GetPerson(Id);

            // Assert
            IsNotNull(result);
            IsInstanceOf<Person>(result);
            AreEqual(Id, result.Id);
            AreEqual(Name, result.Name);
            AreEqual(Age, result.Age);
        }

        private static Person CreateDefaultPersonObject()
        {
            var person = new Person
            {
                Id = Id,
                Name = Name,
                Age = Age
            };
            return person;
        }

        [Test]
        public void GetPerson_CannotFindPerson_ReturnsNullAsResult_Test()
        {
            // Arrange
            const int invalidId = 5;
            _repositoryMock.Setup(m => m.Find(It.Is<int>(i => i == invalidId))).Returns(() => null);
            var service = new PersonService(_repositoryMock.Object);

            // Act
            var result = service.GetPerson(invalidId);

            // Assert
            IsNull(result);
        }

        [Test]
        public void Update_ShouldCallRepositoryUpdateOnceForPerson_Test()
        {
            // Arrange
            var person = CreateDefaultPersonObject();
            _repositoryMock.Setup(m => m.Update(It.Is<Person>(p => p == person)))
                .Verifiable();
            var service = new PersonService(_repositoryMock.Object);

            // Act
            service.Update(person);

            // Assert
            _repositoryMock.Verify(m => m.Update(It.Is<Person>(p => p == person)), Times.Once());
        }

        [Test]
        public void Delete_ShouldCallRepositoryDeleteOnce_Test()
        {
            // Arrange
            var person = CreateDefaultPersonObject();
            _repositoryMock.Setup(m => m.Find(It.Is<int>(s => s == 1))).Returns(person);
            _repositoryMock.Setup(m => m.Delete(It.Is<Person>(p => p == person)))
                .Verifiable();
            var service = new PersonService(_repositoryMock.Object);

            // Act
            service.Delete(person);

            // Assert
            _repositoryMock.Verify(m => m.Delete(It.Is<Person>(p => p == person)), Times.Once());
        }

        [Test]
        public void Create_ShouldCallRepositoryCreateOnce_Test() {
            // Arrange
            var person = CreateDefaultPersonObject();
            _repositoryMock.Setup(m => m.Create(It.Is<Person>(p => p == person)))
                .Verifiable();
            var service = new PersonService(_repositoryMock.Object);

            // Act
            service.Create(person);

            // Assert
            _repositoryMock.Verify(m => m.Create(It.Is<Person>(p => p == person)), Times.Once());
        }
    }
}
