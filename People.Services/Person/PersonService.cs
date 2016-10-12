namespace People.Services.Person
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repository;

    public class PersonService : IPersonService
    {
        private readonly IRepository<Person, int> _repository;

        public PersonService(IRepository<Person, int> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Person> GetPeople()
        {
            return _repository.All.ToList();
        }

        public Person GetPerson(int id)
        {
            return _repository.Find(id);
        }

        public void Update(Person person)
        {
            _repository.Update(person);
        }

        public void Delete(Person entity)
        {
            _repository.Delete(entity);
        }

        public void Create(Person entity)
        {
            _repository.Create(entity);
        }
    }
}