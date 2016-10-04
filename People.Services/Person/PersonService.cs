namespace People.Services.Person
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repository;

    public class PersonService: IPersonService
    {
        private readonly IRepository<Person, string> _repository;

        public PersonService(IRepository<Person, string> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Person> GetPeople()
        {
            return _repository.All.ToList();
        }
    }
}