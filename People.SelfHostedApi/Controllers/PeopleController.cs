namespace People.SelfHostedApi.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Domain.Entities;
    using Services.Person;

    public class PeopleController : ApiController
    {
        private readonly IPersonService _service;

        public PeopleController(IPersonService service)
        {
            _service = service;
        }

        public IHttpActionResult Get()
        {
            var people = _service.GetPeople().ToList();
            return people.Any() ? (IHttpActionResult)Ok(people) : 
                NotFound();
        }

        public IHttpActionResult Get(int id)
        {
            if (id < 0) return BadRequest($"{nameof(id)} is not valid.");

            var person = _service.GetPerson(id);
            return person == null ? (IHttpActionResult)NotFound() : 
                Ok(person);
        }

        public IHttpActionResult Put(int id, Person person)
        {
            if (person == null)
                return BadRequest();

            var persistedEntity = _service.GetPerson(id);
            return persistedEntity == null ? NotFound() :
                UpdateAndReturnOkNegotiatedContentResult(person, persistedEntity);
        }

        private IHttpActionResult UpdateAndReturnOkNegotiatedContentResult(Person person, Person persistedEntity)
        {
            UpdatePersistedEntity(person, persistedEntity);
            return Ok(persistedEntity);
        }

        private void UpdatePersistedEntity(Person person, Person persistedEntity)
        {
            persistedEntity.Name = person.Name;
            persistedEntity.Age = person.Age;

            _service.Update(persistedEntity);
        }

        public IHttpActionResult Post(Person person)
        {
            if (person == null)
                return BadRequest();

            _service.Create(person);
            return Created($"{Request.RequestUri.AbsoluteUri}/{person.Id}", person);
        }

        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest($"{nameof(id)} is not valid.");

            var person = _service.GetPerson(id);
            return person == null ? NotFound() : 
                DeletePersonAndReturnOkResult(person);
        }

        private IHttpActionResult DeletePersonAndReturnOkResult(Person person)
        {
            _service.Delete(person);
            return Ok();
        }
    }
}