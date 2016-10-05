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
    }
}