namespace People.Services.Tests.Person
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Context;
    using Domain.Entities;
    using Domain.Repository;
    using Moq;
    using NUnit.Framework;
    using Services.Person;

    [TestFixture]
    public class PersonServiceTests
    {
        private Mock<IRepository<Person, string>> _repositoryMock;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository<Person, string>>();
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
            var result = service.GetPeople();

            // Assert
            Assert.IsTrue(result.Any());
            Assert.AreEqual(people.Count, result.Count());
        }
    }
}
