namespace People.Domain.Tests.Entities
{
    using Domain.Entities;
    using NUnit.Framework;

    [TestFixture]
    public class PersonTests
    {
        [Test]
        public void Person_Entity_IsInstanceOfPerson_Test()
        {
            // Arrange
            var person = new Person();

            // Act | Assert
            Assert.IsInstanceOf<Person>(person);
        }
    }
}
