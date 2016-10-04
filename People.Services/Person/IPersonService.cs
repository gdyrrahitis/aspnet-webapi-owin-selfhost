namespace People.Services.Person
{
    using System.Collections.Generic;
    using Domain.Entities;

    public interface IPersonService
    {
        IEnumerable<Person> GetPeople();
    }
}