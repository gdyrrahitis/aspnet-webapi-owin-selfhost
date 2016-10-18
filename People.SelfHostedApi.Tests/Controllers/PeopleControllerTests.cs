namespace People.SelfHostedApi.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Results;
    using Domain.Entities;
    using Moq;
    using NUnit.Framework;
    using SelfHostedApi.Controllers;
    using Services.Person;
    using System.Web.Http;
    using System.Net.Http;

    [TestFixture]
    public class PeopleControllerTests
    {
        private Mock<IPersonService> _personService;

        [SetUp]
        public void Setup()
        {
            _personService = new Mock<IPersonService>();
        }

        [Test]
        public void PeopleController_IsInstanceOf_PeopleController_ReceivingPersonServiceOnCreation_Test()
        {
            // Arrange
            var controller = new PeopleController(_personService.Object);

            // Act | Assert
            Assert.IsInstanceOf<PeopleController>(controller);
        }

        [Test]
        public void PeopleController_Get_PeopleFromService_ReturnsOkResultWithPeopleListAsContent_Test()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person(),
                new Person()
            };
            _personService.Setup(m => m.GetPeople()).Returns(people.AsQueryable());
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Get() as OkNegotiatedContentResult<List<Person>>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(people.Count, result.Content.Count);
        }

        [Test]
        public void PeopleController_Get_PeopleFromService_ListIsEmpty_NotFoundResultIsReturned_Test()
        {
            // Arrange
            _personService.Setup(m => m.GetPeople()).Returns(new List<Person>().AsQueryable());
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Get() as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PeopleController_Get_PersonByIdFromService_ReturnsOkWithPersonEntityAsContent_Test()
        {
            // Arrange
            const int id = 1;
            const string name = "George";
            const int age = 26;
            var person = new Person
            {
                Id = id,
                Name = name,
                Age = age
            };
            _personService.Setup(m => m.GetPerson(It.IsAny<int>())).Returns(person);
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Get(id) as OkNegotiatedContentResult<Person>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Content.Id);
            Assert.AreEqual(name, result.Content.Name);
            Assert.AreEqual(age, result.Content.Age);
        }

        [Test]
        public void PeopleController_Get_PersonByIdFromService_PersonCouldNotBeFound_ReturnsNotFoundResult_Test()
        {
            // Arrange
            const int id = 1;
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Get(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PeopleController_Get_PersonByIdFromService_IdIsNegative_ReturnsBadRequestResult_Test()
        {
            // Arrange
            const int id = -5;
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Get(id) as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PeopleController_Post_PersonToService_ReturnsCreatedResult_Test()
        {
            // Arrange
            var person = new Person()
            {
                Id = 1,
                Name = "George",
                Age = 26
            };
            var request = new HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost:3001/api/people")
            };
            var controller = new PeopleController(_personService.Object)
            {
                Request = request
            };

            // Act
            var result = controller.Post(person) as CreatedNegotiatedContentResult<Person>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("http://localhost:3001/api/people/1", result.Location.ToString());
            Assert.AreEqual(1, result.Content.Id);
            Assert.AreEqual("George", result.Content.Name);
            Assert.AreEqual(26, result.Content.Age);
        }

        [Test]
        public void PeopleController_Post_PersonEntityIsNull_ReturnsBadRequest_Test()
        {
            // Arrange
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Post(null) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PeopleController_Put_PersonUpdated_ReturnsOkResultWithContent_Test()
        {
            // Arrange
            const int id = 1;
            var person = new Person()
            {
                Id = 1,
                Name = "George",
                Age = 26
            };
            _personService.Setup(m => m.GetPerson(It.IsAny<int>())).Returns(person);
            _personService.Setup(m => m.Update(It.IsAny<Person>())).Verifiable();
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Put(id, person) as OkNegotiatedContentResult<Person>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(person.Id, result.Content.Id);
            Assert.AreEqual(person.Name, result.Content.Name);
            Assert.AreEqual(person.Age, result.Content.Age);
            _personService.Verify(m => m.GetPerson(It.IsAny<int>()), Times.Once());
            _personService.Verify(m => m.Update(It.IsAny<Person>()), Times.Once());
        }

        [Test]
        public void PeopleController_Put_PersonEntityIsNull_ReturnsBadRequestResult_Test()
        {
            // Arrange
            _personService.Setup(m => m.Update(It.IsAny<Person>())).Verifiable();
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Put(0, null) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            _personService.Verify(m => m.Update(It.IsAny<Person>()), Times.Never());
        }

        [Test]
        public void PeopleController_Put_PersonCouldNotBeFound_ReturnsNotFoundResult_Test()
        {
            // Arrange
            const int id = 1;
            const int invalidId = 2;
            var person = new Person()
            {
                Id = id,
                Name = "George",
                Age = 26
            };
            _personService.Setup(m => m.GetPerson(It.Is<int>(s => s == invalidId))).Returns(() => null);
            _personService.Setup(m => m.Update(It.Is<Person>(s => s == person))).Verifiable();
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Put(invalidId, person) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            _personService.Verify(m => m.GetPerson(It.Is<int>(s => s == invalidId)), Times.Once());
            _personService.Verify(m => m.Update(It.IsAny<Person>()), Times.Never());
        }

        [Test]
        public void PeopleController_Delete_PersonDeleted_ReturnsOkResult_Test()
        {
            // Arrange
            const int id = 1;
            var person = new Person();
            _personService.Setup(m => m.GetPerson(It.IsAny<int>())).Returns(person);
            _personService.Setup(m => m.Update(It.IsAny<Person>())).Verifiable();
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Delete(id) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            _personService.Verify(m => m.GetPerson(It.IsAny<int>()), Times.Once());
            _personService.Verify(m => m.Delete(It.IsAny<Person>()), Times.Once());
        }

        [Test]
        public void PeopleController_Delete_PersonIdIsNotValid_ReturnsBadRequest_Test()
        {
            // Arrange
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Delete(-5) as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("id is not valid.", result.Message);
        }

        [Test]
        public void PeopleController_Delete_PersonCouldNotBeFound_ReturnsNotFoundResult_Test()
        {
            // Arrange
            const int invalidId = 2;
            var person = new Person();
            _personService.Setup(m => m.GetPerson(It.Is<int>(s => s == invalidId))).Returns(() => null);
            var controller = new PeopleController(_personService.Object);

            // Act
            var result = controller.Delete(invalidId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            _personService.Verify(m => m.GetPerson(It.Is<int>(s => s == invalidId)), Times.Once());
            _personService.Verify(m => m.Delete(It.IsAny<Person>()), Times.Never());
        }
    }
}